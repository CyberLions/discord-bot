using System;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.RolePrompt
{
    public class RolePromptComponents
    {
        public static ComponentBuilder BtnComponent(bool psuEmail, List<BtnRole>? standings, List<BtnRole>? interestRoles)
        {
            ComponentBuilder component = new ComponentBuilder();
            // Standings:
            if (standings != null)
            {
                for (int i = 0; i < standings.Count(); i++)
                {
                    BtnRole standing = standings[i];

                    ButtonBuilder btn = new ButtonBuilder();
                    btn.WithLabel(standing.Name);
                    btn.WithStyle(ButtonStyle.Primary);
                    // Check if role requires verified email
                    if (standing.RequireVerification)
                    {
                        // Use protected react role
                        btn.WithCustomId("protected-toggle-role-" + standing.Role);
                    }
                    else
                    {
                        // Use unprotected react role
                        btn.WithCustomId("toggle-role-" + standing.Role);
                    }
                    // Get emote:
                    if (standing.Emote != null)
                    {
                        btn.WithEmote(standing.Emote);
                    }
                    // Disable button if not verified
                    if (standing.RequireVerification && !psuEmail)
                    {
                        btn.WithDisabled(true);
                    }

                    // Five buttons to a row.
                    component.WithButton(btn, i/5);
                }
            }
            // Interests:
            if(interestRoles != null)
            {
                for(int i = 0; i<interestRoles.Count(); i++)
                {
                    BtnRole interest = interestRoles[i];

                    ButtonBuilder btn = new ButtonBuilder();
                    btn.WithLabel(interest.Name);
                    btn.WithStyle(ButtonStyle.Success);
                    btn.WithCustomId("toggle-role-" + interest.Role);
                    // Get emote:
                    if (interest.Emote != null)
                    {
                        btn.WithEmote(interest.Emote);
                    }
                    // Five buttons to a row.
                    // Add previous rows:
                    int offset = 0;
                    if(standings != null)
                    {
                        offset = standings.Count() / 5 + 1;
                    }
                    component.WithButton(btn, i/5 + offset);
                }
            }
            return component;
        }
    }
}

