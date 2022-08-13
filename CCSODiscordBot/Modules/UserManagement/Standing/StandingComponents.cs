using System;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using Discord;

namespace CCSODiscordBot.Modules.UserManagement.Standing
{
    public class StandingComponents
    {
        public static ComponentBuilder StandingComponent(bool psuEmail, List<ClassStanding> standings)
        {
            ComponentBuilder component = new ComponentBuilder();
            foreach(ClassStanding standing in standings)
            {
                ButtonBuilder btn = new ButtonBuilder();
                btn.WithLabel(standing.Name);
                btn.WithStyle(ButtonStyle.Primary);
                btn.WithCustomId("toggle-role-" + standing.Role);
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

