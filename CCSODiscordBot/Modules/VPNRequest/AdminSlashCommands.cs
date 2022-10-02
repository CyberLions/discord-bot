using System;
using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.VPNRequest
{
    public class AdminSlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
	public AdminSlashCommands(IGuildRepository iGuildRepository)
        {
            _iGuildRepository = iGuildRepository;
        }
        [SlashCommand("setvpnrequest", "Sets the VPN request URL and API key.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task SetVPNApiKey(string url, string key)
        {
            await Context.Interaction.DeferAsync(true);
            Uri urlvalid;
            // validate URL:
            bool result = Uri.TryCreate(url, UriKind.Absolute, out urlvalid) && urlvalid != null && (urlvalid.Scheme == Uri.UriSchemeHttp || urlvalid.Scheme == Uri.UriSchemeHttps);
            if (!result)
            {
                await Context.Interaction.FollowupAsync("The URL is invalid.", ephemeral: true);
                return;
            }
            // Ensure guild is configured:
            var guild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            if (guild == null)
            {
                await Context.Interaction.FollowupAsync("The guild must be set up and verification must be enabled.", ephemeral: true);
                return;
            }
            // Add values to guild:
            guild.VPNAPIURL = urlvalid?.ToString();
            guild.VPNAPIKey = key;

            // Update the DB:
            await _iGuildRepository.UpdateGuildAsync(guild);
            // Inform user:
            await Context.Interaction.FollowupAsync("Enabled VPN requests.", ephemeral: true);
            Console.WriteLine("VPN Request enabled. User, Guild: " + Context.User.Id + ", " + Context.Guild.Name);
        }
    }
}

