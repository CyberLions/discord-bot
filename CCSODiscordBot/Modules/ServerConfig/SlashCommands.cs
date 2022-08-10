using System;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.Database.DataTables;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.ServerConfig
{
    public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private IGuildRepository _iGuildRepository;
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
            guild.ClassStandings = new List<Services.Database.DataTables.SubClasses.ClassStanding>();
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

