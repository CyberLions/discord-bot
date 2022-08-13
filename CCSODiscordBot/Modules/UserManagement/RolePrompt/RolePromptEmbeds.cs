using System;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.RolePrompt
{
    public class RolePromptEmbeds
    {
        public static EmbedBuilder Embeds(bool psuEmail, string title = "Class Standing", string? prependBody = null)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithColor(Color.Teal);

            string description = "Select your class/year.";
            if (!string.IsNullOrEmpty(prependBody))
            {
                description = prependBody + "\n" + description;
            }
            embed.WithDescription(description);

            if (!psuEmail)
            {
                embed.AddField("Student/Faculty", "You did not enter and verify a PSU email, some roles may be disabled.");
            }
            return embed;
        }
    }
}

