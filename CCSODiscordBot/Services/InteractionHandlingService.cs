using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace CCSODiscordBot.Services
{
	public class InteractionHandlingService
	{
        private readonly InteractionService _service;
        private readonly DiscordShardedClient _client;
        private readonly IServiceProvider _provider;

        public InteractionHandlingService(IServiceProvider services)
        {
            _service = services.GetRequiredService<InteractionService>();
            _client = services.GetRequiredService<DiscordShardedClient>();
            _provider = services;

            _service.Log += Logging.Log;
            _client.InteractionCreated += OnInteractionAsync;
            _client.ShardReady += ReadyAsync;
        }

        /// <summary>
        /// Registers all modules, and adds the commands from these modules to either guild or globally depending on the build state.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await _service.AddModulesAsync(typeof(InteractionHandlingService).Assembly, _provider);
        }
        /// <summary>
        /// Register slash commands when the shard is ready
        /// </summary>
        /// <returns></returns>
        private async Task ReadyAsync(DiscordSocketClient shard)
        {
            // Context & Slash commands can be automatically registered, but this process needs to happen after the client enters the READY state.
#if DEBUG
            // Since Global Commands take around 1 hour to register, we should use a test guild to instantly update and test our commands.
            Console.WriteLine("Set commands per guild.");
            foreach (SocketGuild guild in _client.Guilds)
            {
                Console.WriteLine("Set commands to guild "+guild.Name);
                await _service.RegisterCommandsToGuildAsync(guild.Id);
            }
#else
            // Register globally. This is cached by Discord and changes may take a bit.
            Console.WriteLine("Commands set globally.");
            await _service.RegisterCommandsGloballyAsync();
#endif
        }

        /// <summary>
        /// Handles an executed interaction
        /// </summary>
        /// <param name="interaction"></param>
        /// <returns></returns>
        private async Task OnInteractionAsync(SocketInteraction interaction)
        {
            _ = Task.Run(async () =>
            {
                var context = new ShardedInteractionContext(_client, interaction);
                await _service.ExecuteCommandAsync(context, _provider);
            });
            await Task.CompletedTask;
        }
    }
}

