using System;
using System.Text.RegularExpressions;
using CCSODiscordBot.Services.Database.Repository;
using CCSODiscordBot.Services.DynamicSlashCommands.Attributes;
using CCSODiscordBot.Services.SSO.Interfaces;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.SSOCommands
{
	public class SSOModalHandler
	{
        private readonly IGuildRepository _iGuildRepository;
        public SSOModalHandler(IGuildRepository iGuildRepository)
        {
            _iGuildRepository = iGuildRepository;
        }

        [DynamicModalInteraction("sso_config_*")]
        public async Task ConfigureSSOModal(SocketModal modal)
        {
            await modal.DeferAsync(true);

            List<SocketMessageComponentData> components = modal.Data.Components.ToList();
            string implementationName = Regex.Match(modal.Data.CustomId, "sso_config_*").Value;

            if (modal.GuildId == null)
            {
                throw new NullReferenceException("Guild is null");
            }
            var guild = await _iGuildRepository.GetByDiscordIdAsync((ulong)modal.GuildId);

            var type = typeof(ISSOManagement);

            Type? ssoImplementation;
            try
            {
                ssoImplementation = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.Name.Equals(implementationName) && type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                    .First();
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Debug");

            try
            {
                ISSOManagement? classInstance = (ISSOManagement?)Activator.CreateInstance(ssoImplementation);

                if (classInstance == null)
                {
                    throw new NullReferenceException("Instance of class is null");
                }

                foreach (var component in components)
                {
                    classInstance.Configuration.SetSetting(new KeyValuePair<string, string>(component.CustomId, component.Value));
                }

                guild.SSOConfigSettings = classInstance.Configuration;
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to create instance of class. " + e.Message);
                await modal.FollowupAsync("Failed to save settings.");
                return;
            }

            // Update DB
            await _iGuildRepository.UpdateGuildAsync(guild);
            await modal.FollowupAsync("Settings saved!");
        }
    }
}

