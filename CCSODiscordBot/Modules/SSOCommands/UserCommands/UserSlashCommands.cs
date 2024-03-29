﻿using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.SSO.Implementations.Zitadel;
using CCSODiscordBot.Services.SSO.Interfaces;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.SSOCommands.UserCommands
{
	public class UserSlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
        private readonly IUserRepository _iUserRepository;
        public UserSlashCommands(IGuildRepository iGuildRepository, IUserRepository iUserRepository)
        {
            _iGuildRepository = iGuildRepository;
            _iUserRepository = iUserRepository;
        }
        [SlashCommand("sso-sync", "Sync your account to the SSO server.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        public async Task SyncSSO()
        {
            await Context.Interaction.DeferAsync(true);

            // Get and check user verification:
            var user = await _iUserRepository.GetByDiscordIdAsync(Context.User.Id, Context.Guild.Id);
            if (user == null || !user.Verified)
            {
                ButtonBuilder getStartedButton = new ButtonBuilder();
                getStartedButton.WithLabel("Get Started");
                getStartedButton.Style = ButtonStyle.Success;
                getStartedButton.WithCustomId("get-started-" + Context.User.Id);
                ComponentBuilder component = new ComponentBuilder();
                component.WithButton(getStartedButton);

                await Context.Interaction.FollowupAsync("You must be a verified user to use this command. Click the button to get started.", components: component.Build());
                return;
            }
            // Check guild SSO config status:
            var guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            if(guild.SSOConfigSettings == null)
            {
                await Context.Interaction.FollowupAsync("This guild does not have SSO enabled.");
                return;
            }

            // Get class:
            var type = typeof(ISSOManagement);
            var ssoImplementation = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .Where(p => p.Name.Equals(guild.SSOConfigSettings.Name))
                .First();

            ISSOManagement? ssoHandler = null;
            try
            {
                ssoHandler = (ISSOManagement?)Activator.CreateInstance(ssoImplementation, new SSOConfig[] { guild.SSOConfigSettings });
            }
            catch(Exception e)
            {
                await Context.Interaction.FollowupAsync("Failed to load SSO");
                Console.WriteLine("Failed to create SSO object. " + e.Message);
                return;
            }

            // Check for null value
            if(ssoHandler == null)
            {
                await Context.Interaction.FollowupAsync("Failed to load SSO");
                Console.WriteLine("SSO object null.");
                return;
            }

            // Clear existing SSO ID if user was on an old SSO implementation
            if (!guild.SSOConfigSettings.Name.Equals(user.SSOImplementation))
            {
                // Change SSO over to new platform:
                user.SSOImplementation = guild.SSOConfigSettings.Name;
                user.SSOID = null;
            }

            // See if SSO user exists:
            if (ssoHandler.UserExists(user))
            {
                string updatedUID = ssoHandler.UpdateUserRecord(user);
                // Update the DB if the UID doesnt exist:
                if (!updatedUID.Equals(user.SSOID))
                {
                    user.SSOID = updatedUID;
                    await _iUserRepository.UpdateUserAsync(user);
                }

                await Context.Interaction.FollowupAsync("Found existing user. User synced.");
                return;
            }

            // Create SSO user:
            try
            {
                user.SSOID = ssoHandler.AddUser(user);
                // Update DB with SSO UID:
                await _iUserRepository.UpdateUserAsync(user);
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to create SSO user. Error: " + e.Message);
                await Context.Interaction.FollowupAsync("Failed to create new SSO user.");
                return;
            }
            await Context.Interaction.FollowupAsync("User created!");
        }
    }
}

