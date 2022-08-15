using System;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.Database.DataTables;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Channels;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using Discord.Commands;

namespace CCSODiscordBot.Modules.ServerConfig
{
    public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
        public SlashCommands(IGuildRepository iGuildRepository)
        {
            _iGuildRepository = iGuildRepository;
        }

        [SlashCommand("welcome", "Enable and disable the welcome module.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task EnableWelcome(bool enabled)
        {
            await Context.Interaction.DeferAsync(true);

            Guild guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            // Check for new server:
            if(guild == null)
            {
                // Create new
                guild = await CreateNewGuild(Context.Guild);
            }
            //Enable welcome:
            guild.WelcomeEnabled = enabled;

            // Update DB:
            await _iGuildRepository.UpdateGuildAsync(guild);

            // Notify user:
            await Context.Interaction.FollowupAsync("Settings updated!");
        }

        [SlashCommand("welcomechan", "Set the welcome channel for the welcome module.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task WelcomeChan(SocketChannel channel)
        {
            await Context.Interaction.DeferAsync(true);

            Guild guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            // Check for new server:
            if (guild == null)
            {
                // Create new
                guild = await CreateNewGuild(Context.Guild);
            }
            //Set welcome chan:
            guild.WelcomeChannel = channel.Id;

            // Update DB:
            await _iGuildRepository.UpdateGuildAsync(guild);

            // Notify user:
            await Context.Interaction.FollowupAsync("Settings updated!");
        }

        [SlashCommand("addstanding", "Add a class standing.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task AddStanding(SocketRole role, string name, string description, bool requireVerified = false)
        {
            await Context.Interaction.DeferAsync(true);

            Guild guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            // Check for new server:
            if (guild == null)
            {
                // Create new
                guild = await CreateNewGuild(Context.Guild);
            }
            //Set welcome chan:
            BtnRole newClass = new BtnRole();
            newClass.Name = name;
            newClass.Description = description;
            newClass.RequireVerification = requireVerified;
            newClass.Role = role.Id;
            newClass.Emote = null;
            // TODO: Add emote to btn
            //if(!string.IsNullOrEmpty(emote))
            //{
            //    try
            //    {
            //        newClass.Emote = new Emoji(emote.Substring(0,1));
            //    }
            //    catch (ArgumentException)
            //    {
            //        await Context.Interaction.FollowupAsync("Failed to parse emote.");
            //        return;
            //    }
            //}

            // Check null:
            if(guild.ClassStandings == null)
            {
                guild.ClassStandings = new List<BtnRole>();
            }

            // Add role to DB:
            guild.ClassStandings.Add(newClass);

            // Update DB:
            await _iGuildRepository.UpdateGuildAsync(guild);

            // Notify user:
            await Context.Interaction.FollowupAsync("Settings updated!");
        }

        [SlashCommand("addinterest", "Add a interest role")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task AddInterest(SocketRole role, string name, string description)
        {
            await Context.Interaction.DeferAsync(true);

            Guild guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            // Check for new server:
            if (guild == null)
            {
                // Create new
                guild = await CreateNewGuild(Context.Guild);
            }
            //Set welcome chan:
            BtnRole newClass = new BtnRole();
            newClass.Name = name;
            newClass.RequireVerification = false; // Not implemented.
            newClass.Role = role.Id;
            newClass.Emote = null;
            newClass.Description = description;

            // TODO: Add emote to btn
            //if(!string.IsNullOrEmpty(emote))
            //{
            //    try
            //    {
            //        newClass.Emote = new Emoji(emote.Substring(0,1));
            //    }
            //    catch (ArgumentException)
            //    {
            //        await Context.Interaction.FollowupAsync("Failed to parse emote.");
            //        return;
            //    }
            //}

            // Check null:
            if (guild.InterestRoles == null)
            {
                guild.InterestRoles = new List<BtnRole>();
            }

            // Add role to DB:
            guild.InterestRoles.Add(newClass);

            // Update DB:
            await _iGuildRepository.UpdateGuildAsync(guild);

            // Notify user:
            await Context.Interaction.FollowupAsync("Settings updated!");
        }

        /// <summary>
        /// Function to create a new guild in the DB
        /// </summary>
        /// <param name="newGuild">Discord guild to create</param>
        /// <returns>The guild in the DB</returns>
        private async Task<Guild> CreateNewGuild(SocketGuild newGuild)
        {
            // Create a new guild:
            Guild guild = new Guild();
            // Insert information:
            guild.ClassStandings = new List<BtnRole>();
            guild.DiscordID = newGuild.Id;
            guild.LeaveEnabled = false;
            guild.WelcomeChannel = newGuild.DefaultChannel.Id;
            guild.WelcomeEnabled = false;

            // Push new guild to server:
            await _iGuildRepository.CreateNewGuildAsync(guild);

            return guild;
        }
    }
}

