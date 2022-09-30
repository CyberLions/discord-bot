using System;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.VPNAPI;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.VPNRequest
{
	public class UserSlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
        private readonly IUserRepository _iUserRepository;
        public UserSlashCommands(IGuildRepository guildRepository, IUserRepository userRepository)
        {
            _iGuildRepository = guildRepository;
            _iUserRepository = userRepository;
        }
        [SlashCommand("requestvpn", "Requests a VPN account.")]
        [DefaultMemberPermissions(GuildPermission.SendMessages)]
        [EnabledInDm(false)]
        public async Task RequestVPNAccount([MaxLength(100)][Summary(description: "Please enter a breif reason for why you are requesting the VPN.")]string reason)
        {
            await Context.Interaction.DeferAsync(true);
            // get and check guild for configuration:
            var guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            if (guild == null)
            {
                await Context.Interaction.FollowupAsync("The guild has not been configured to support this command. Please ask an admin for further help.", ephemeral: true);
                return;
            }
            else if(string.IsNullOrEmpty(guild.VPNAPIKey) || string.IsNullOrEmpty(guild.VPNAPIURL))
            {
                await Context.Interaction.FollowupAsync("The guild has not been configured to support this command. Please ask an admin for further help. Error: empty url/key", ephemeral: true);
                return;
            }

            // Get user:
            var user = await _iUserRepository.GetByDiscordIdAsync(Context.User.Id, Context.Guild.Id);
            if(user is null || !user.Verified)
            {
                ButtonBuilder getStartedButton = new ButtonBuilder();
                getStartedButton.WithLabel("Get Started");
                getStartedButton.Style = ButtonStyle.Success;
                getStartedButton.WithCustomId("get-started-" + Context.User.Id);
                ComponentBuilder component = new ComponentBuilder();
                component.WithButton(getStartedButton);

                await Context.Interaction.FollowupAsync("You must have a verified PSU email registered with the bot to request VPN access.", ephemeral: true, components: component.Build());
                return;
            }
            // Check to see if the request was already sent:
            if (user.VpnRequestSent)
            {
                await Context.Interaction.FollowupAsync("You have already requested VPN access. Please contact an admin if you are having issues.", ephemeral: true);
                return;
            }
            // Log event:
            Console.WriteLine("Making VPN request for " + Context.User.Id + ", guild: " + Context.Guild.Name);
            // Make the request and ensure it was successful:
            bool requestStatus = await RequestHandler.MakeVPNRequest(guild, user, Context.User.ToString(), reason);
            if (requestStatus)
            {
                user.VpnRequestSent = true;
                // Update the DB:
                await _iUserRepository.UpdateUserAsync(user);
                await Context.Interaction.FollowupAsync("Your request was sent. Once your request has been accepted, you will receive an email with your login credentials. The email will be sent to "+user.Email+".", ephemeral: true);
            }
            else
            {
                await Context.Interaction.FollowupAsync("Request failed. Please try again later.", ephemeral: true);
            }
        }
    }
}

