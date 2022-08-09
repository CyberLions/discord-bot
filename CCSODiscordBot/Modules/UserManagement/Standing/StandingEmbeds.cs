using System;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.Standing
{
    public class StandingEmbeds
    {
        public static EmbedBuilder StandingEmbed(bool psuEmail)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Class Standing");
            embed.WithColor(Color.Teal);
            embed.WithDescription("Select your class/year.");
            if (!psuEmail)
            {
                embed.AddField("Student/Faculty", "You did not enter and verify a PSU email, some roles may be disabled.");
            }
            return embed;
        }
    }
}

