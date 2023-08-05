using System;
using CCSODiscordBot.Services.Attributes.DynamicSlashCommands;
using Discord;
using Discord.WebSocket;
using Zitadel.Api;

namespace CCSODiscordBot.Services.DynamicSlashCommands
{
	public class RegisterDynamicSlashCommands
	{
        /// <summary>
        /// Registers a dynamic slash command with Discord
        /// </summary>
        /// <param name="client">Discord client</param>
        /// <returns></returns>
        public static async Task RegisterCommandsToGuild(DiscordSocketClient client)
        {
            Console.WriteLine("Registering dynamic commands");

            // Find all dynamic slash commands by attribute:
            var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
                .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
                .Where(x => x.IsClass) // only yields classes
                .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
                .Where(x => x.ReturnType.Equals(typeof(SlashCommandBuilder)))
                .Where(x => x.GetCustomAttributes(typeof(RegisterDynamicSlashCommandAttribute), false).FirstOrDefault() != null);

            try
            {
                foreach (var method in methods) // iterate through all found dynamic slash command registrations
                {
                    var obj = Activator.CreateInstance(method.DeclaringType); // Instantiate the class
                    SlashCommandBuilder cmd = (SlashCommandBuilder)method.Invoke(obj, null); // invoke the method
                    await client.CreateGlobalApplicationCommandAsync(cmd.Build());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error creating dynamic slash command: " + e.Message);
            }
		}
	}
}

