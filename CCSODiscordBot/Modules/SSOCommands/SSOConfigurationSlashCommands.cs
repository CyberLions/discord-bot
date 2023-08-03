using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord.Interactions;
using Google.Api;

namespace CCSODiscordBot.Modules.SSOCommands
{
    [DontAutoRegister] // This class handles dynamic slash commands. Dont register them automatically because we will do that "manually"
    public class SSOConfigurationSlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
        public SSOConfigurationSlashCommands(IGuildRepository iGuildRepository)
        {
            _iGuildRepository = iGuildRepository;
        }

        [SlashCommand("set-sso", "Configure an SSO server.")]
        public async Task ConfigureSSO([Summary("sso-provider")]string Integration)
        {
            Console.WriteLine("SSO command executed!");
            // Notify user:
            await Context.Interaction.DeferAsync(true);
            await Context.Interaction.FollowupAsync("Input: " + Integration);
        }
    }
}

