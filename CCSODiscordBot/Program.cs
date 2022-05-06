// Program Entry Point.
using CCSODiscordBot;
using CCSODiscordBot.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

// Services:
using (var services = ConfigureServices())
{
    var config = services.GetRequiredService<ConfigHandlingService>();
    var client = services.GetRequiredService<DiscordShardedClient>();

    // The Sharded Client does not have a Ready event.
    // The ShardReady event is used instead, allowing for individual
    // control per shard.
    client.ShardReady += Logging.ReadyAsync;
    client.Log += Logging.Log;

    await services.GetRequiredService<InteractionHandlingService>()
        .InitializeAsync();

    await services.GetRequiredService<CommandHandlingService>()
        .InitializeAsync();

    // Tokens should be considered secret data, and never hard-coded.
    await client.LoginAsync(TokenType.Bot, config.DiscordToken);
    await client.StartAsync();
    
    // Dont close the program. Background threads are running.
    await Task.Delay(Timeout.Infinite);
}

// Dependancy Injection function:
ServiceProvider ConfigureServices()
    => new ServiceCollection()
        .AddSingleton<ConfigHandlingService>()
        // Add the Discord Client with intents
        .AddSingleton(x=> new DiscordShardedClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.AllUnprivileged}))
        .AddSingleton<CommandService>()
        // Add InteractionService service with config to run all commands async:
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordShardedClient>(), new InteractionServiceConfig { DefaultRunMode = Discord.Interactions.RunMode.Async }))
        .AddSingleton<CommandHandlingService>()
        .AddSingleton<InteractionHandlingService>()
        .BuildServiceProvider();