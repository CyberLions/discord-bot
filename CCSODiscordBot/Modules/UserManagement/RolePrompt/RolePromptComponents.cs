using System;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.RolePrompt
{
    public class RolePromptComponents
    {
        public static ComponentBuilder BtnComponent(bool psuEmail, List<BtnRole> standings)
        {
            ComponentBuilder component = new ComponentBuilder();
            foreach(BtnRole standing in standings)
            {
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
                if(standing.Emote != null)
                {
                    btn.WithEmote(standing.Emote);
                }
                // Disable button if not verified
                if(standing.RequireVerification && !psuEmail)
                {
                    btn.WithDisabled(true);
                }

                component.WithButton(btn);
            }
            return component;
        }
    }
}

