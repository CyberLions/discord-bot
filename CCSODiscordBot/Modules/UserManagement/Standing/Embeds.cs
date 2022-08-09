using System;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.Standing
{
    public class Embeds
    {
        public static EmbedBuilder StandingEmbed(bool psuEmail)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Class Standing");
            embed.WithColor(Color.Teal);
            embed.WithDescription("Select your class/year.");
            if (!psuEmail)
            {
                embed.AddField("Student/Faculty", "You did not enter a PSU email and cannot select the student or faculty role.");
            }
            return embed;
        }
    }
}

