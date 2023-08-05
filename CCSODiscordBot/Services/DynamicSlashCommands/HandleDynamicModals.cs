using System;
using System.Reflection;
using System.Text.RegularExpressions;
using CCSODiscordBot.Modules.SSOCommands;
using CCSODiscordBot.Services.Attributes.DynamicSlashCommands;
using CCSODiscordBot.Services.DynamicSlashCommands.Attributes;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Zitadel.Api;

namespace CCSODiscordBot.Services.DynamicSlashCommands
{
	public class HandleDynamicModals
	{
        private readonly IServiceProvider Provider;

        public HandleDynamicModals(IServiceProvider provider)
		{
            Provider = provider;
		}
        public async Task ModalExecuted(SocketModal modal)
        {
            Console.WriteLine("Modal received");
            var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
                .SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
                .Where(x => x.IsClass) // only yields classes
                .SelectMany(x => x.GetMethods()) // returns all methods defined in those classes
                .Where(x => x.GetCustomAttributes(typeof(DynamicModalInteractionAttribute), false).FirstOrDefault() != null)
                .Where(x => x.GetCustomAttributes<DynamicModalInteractionAttribute>(false).Where(x => Regex.Match(modal.Data.CustomId, x.CustomId).Success).Count() > 0);

            foreach (var method in methods) // iterate through all found dynamic slash command registrations
            {
                try
                {
                    if (method.DeclaringType == null)
                    {
                        throw new NullReferenceException("Object does not have a declaring type (null)");
                    }

                    var obj = ActivatorUtilities.CreateInstance(Provider, method.DeclaringType); // Instantiate the class
                    method.Invoke(obj, new object[] { modal }); // invoke the method
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error creating dynamic slash command: " + e.Message);
                }
            }
        }
    }
}

