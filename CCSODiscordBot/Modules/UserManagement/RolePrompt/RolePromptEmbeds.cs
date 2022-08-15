using System;
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
                string clubRelation = "__Blurple buttons__: select your class rank or association with the club.\n";
                if (!psuEmail)
                {
                    clubRelation += "__NOTE__: You did not enter and verify a PSU email, some roles may be disabled.\n";
                }
                clubRelation += "__**Roles**__:\n";
                foreach (BtnRole standing in standings)
                {
                    clubRelation += "**" + standing.Name + "**: " + standing.Description + "\n";
                }
                embed.AddField("Club Relation", clubRelation);
            }
            if (interestRoles?.Count > 0)
            {
                // Interests
                string clubInterests = "__Green buttons__: Select roles that interest you.\n";
                clubInterests += "__**Roles**__:\n";
                foreach (BtnRole interest in interestRoles)
                {
                    clubInterests += "**" + interest.Name + "**: " + interest.Description + "\n";
                }
                embed.AddField("Club Interests", clubInterests);
            }

            // Send embed:
            return embed;
        }
    }
}

