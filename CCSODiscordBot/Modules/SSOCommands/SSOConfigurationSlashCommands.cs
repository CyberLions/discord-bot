using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.SSO.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

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
        public async Task ConfigureSSO([Summary("sso-provider")]string integration)
        {
            // Handle "none" disable SSO option
            if (integration == "None")
            {
                // Update the DB
                var guildDb = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
                guildDb.SSOConfigSettings = null;
                await _iGuildRepository.UpdateGuildAsync(guildDb);

                // Inform the user
                await Context.Interaction.RespondAsync("SSO disabled.");
                return;
            }

            // Get class:
            var type = typeof(ISSOManagement);
            var ssoImplementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                .Where(p => p.Name == integration)
                .First();

            ISSOManagement classInstance = (ISSOManagement) Activator.CreateInstance(ssoImplementations);

            var mb = new ModalBuilder();
            mb.WithTitle(integration + " Configuration");
            mb.WithCustomId("sso_config_"+ integration);

            foreach(string setting in classInstance.Configuration.Settings)
            {
                mb.AddTextInput(setting, setting);
            }

            await Context.Interaction.RespondWithModalAsync(mb.Build());
        }
        [DynamicModalInteraction("sso_config_*")]
        public async Task ConfigureSSOModal(SocketModalData modal)
        {
            Console.WriteLine("Implementation: ");
            modal.Components.
        }
    }
}

