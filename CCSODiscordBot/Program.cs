﻿// Program Entry Point.
using CCSODiscordBot;
using CCSODiscordBot.Modules.Greeter;
using CCSODiscordBot.Modules.UserManagement;
using CCSODiscordBot.Services;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.DynamicSlashCommands;
using CCSODiscordBot.Services.Email;
using CCSODiscordBot.Services.ExceptionHandling;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

// Services:
using (var services = ConfigureServices())
{
    var config = services.GetRequiredService<ConfigHandlingService>();
    var client = services.GetRequiredService<DiscordShardedClient>();

    // The Sharded Client does not have a Ready event.
    // The ShardReady event is used instead, allowing for individual
    // control per shard.
    client.ShardReady += Logging.ReadyAsync;
    client.ShardReady += RegisterDynamicSlashCommands.RegisterCommandsToGuild;
    client.Log += Logging.Log;

    // Register Modals
    var dynamicModalHandler = services.GetRequiredService<HandleDynamicModals>();
    client.ModalSubmitted += dynamicModalHandler.ModalExecuted;

    // Add join and leave notifications
    var greeting = services.GetRequiredService<Greeting>();
    var leaving = services.GetRequiredService<Leaving>();
    client.UserJoined += greeting.UserJoin;
    client.UserLeft += leaving.UserLeft;

    // Command exception handling
    var interactionService = services.GetRequiredService<InteractionService>();
    interactionService.SlashCommandExecuted += SlashCommandExceptionHandler.SlashCommandExecuted;
    // Disabled for custom modals:
    //interactionService.ComponentCommandExecuted += ComponentCommandExceptionHandler.ComponentCommandExecuted;
    interactionService.ModalCommandExecuted += ModalExceptionHandler.ModalExecuted;

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
        .AddSingleton(x => new DiscordShardedClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.All }))
        .AddSingleton<CommandService>()
        // Add InteractionService service with config to run all commands async:
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordShardedClient>(), new InteractionServiceConfig { DefaultRunMode = Discord.Interactions.RunMode.Async }))
        .AddSingleton<CommandHandlingService>()
        .AddSingleton<InteractionHandlingService>()
        .AddSingleton<IMongoDatabase>(options => {
            var config = new ConfigHandlingService();
            var client = new MongoClient(config.MongoDBConnectionString);
            return client.GetDatabase("ccsobot");
        })
        .AddSingleton<IGuildRepository, GuildRepository>()
        .AddSingleton<IUserRepository, UserRepository>()
        .AddSingleton<EmailSender>()
        .AddSingleton<Greeting>()
        .AddSingleton<Leaving>()
        .AddSingleton<HandleDynamicModals>()
        .AddSingleton<SlashCommandExceptionHandler>()
        .BuildServiceProvider();