// Program Entry Point.
using CCSODiscordBot;
using CCSODiscordBot.Modules.Greeter;
using CCSODiscordBot.Modules.UserManagement;
using CCSODiscordBot.Services;
using CCSODiscordBot.Services.Attributes.DynamicSlashCommands;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.Email;
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
    client.Log += Logging.Log;

    // Add join and leave notifications
    var greeting = services.GetRequiredService<Greeting>();
    var leaving = services.GetRequiredService<Leaving>();
    client.UserJoined += greeting.UserJoin;
    client.UserLeft += leaving.UserLeft;

    await services.GetRequiredService<InteractionHandlingService>()
        .InitializeAsync();

    await services.GetRequiredService<CommandHandlingService>()
        .InitializeAsync();

    // Register dynamic slash commands:
    var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
        .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
        .Where(x => x.IsClass) // only yields classes
        .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
        .Where(x => x.ReturnType.Equals(typeof(SlashCommandProperties)))
        .Where(x => x.GetCustomAttributes(typeof(RegisterDynamicSlashCommandAttribute), false).FirstOrDefault() != null);

    foreach (var method in methods) // iterate through all found dynamic slash command registrations
    {
        var obj = Activator.CreateInstance(method.DeclaringType); // Instantiate the class
        SlashCommandProperties cmd = (SlashCommandProperties) method.Invoke(obj, null); // invoke the method
        await client.Rest.CreateGlobalCommand(cmd);
    }

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
        .BuildServiceProvider();