using System;
namespace CCSODiscordBot.Services.DynamicSlashCommands.Attributes
{
	public class DynamicModalInteractionAttribute : Attribute
    {
        public string CustomId { get; }

        public DynamicModalInteractionAttribute(string customId)
        {
            CustomId = customId;
        }
    }
}

