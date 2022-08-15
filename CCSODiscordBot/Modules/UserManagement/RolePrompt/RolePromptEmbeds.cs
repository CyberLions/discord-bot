using System;
using System.Text;
using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.RolePrompt
{
    public class RolePromptEmbeds
    {
        public static EmbedBuilder Embeds(bool psuEmail, List<BtnRole>? standings, List<BtnRole>? interestRoles, string title = "Roles", string? prependBody = null)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithColor(Color.Teal);

            string description = "Select your Discord roles!";
            if (!string.IsNullOrEmpty(prependBody))
            {
                description = prependBody + "\n" + description;
            }
            embed.WithDescription(description);

            // Club relations:
            if (standings?.Count > 0)
            {
                StringBuilder clubStandingBuilder = new StringBuilder();
                clubStandingBuilder.Append("__Blurple buttons__: select your class rank or association with the club.\n");
                if (!psuEmail)
                {
                    clubStandingBuilder.Append("__NOTE__: You did not enter and verify a PSU email, some roles may be disabled.\n");
                }
                clubStandingBuilder.Append("__**Roles**__:\n");
                foreach (BtnRole standing in standings)
                {
                    clubStandingBuilder.Append("**" + standing.Name + "**: " + standing.Description + "\n");
                }
                embed.AddField("Club Relation", clubStandingBuilder);
            }
            // Interests
            if (interestRoles?.Count > 0)
            {
                StringBuilder clubStandingBuilder = new StringBuilder();
                clubStandingBuilder.Append("__Green buttons__: Select roles that interest you.\n");
                clubStandingBuilder.Append("__**Roles**__:\n");
                foreach (BtnRole interest in interestRoles)
                {
                    clubStandingBuilder.Append("**" + interest.Name + "**: " + interest.Description + "\n");
                }
                embed.AddField("Club Interests", clubStandingBuilder);
            }

            // Send embed:
            return embed;
        }
    }
}

