﻿using System;
using CCSODiscordBot.Services.Attributes.DynamicSlashCommands;
using CCSODiscordBot.Services.SSO.Interfaces;
using Discord;

namespace CCSODiscordBot.Modules.SSOCommands
{
	public class SSOSlashCommands
	{
		[RegisterDynamicSlashCommand]
        public SlashCommandProperties RegisterSSOSlashCommands()
		{
            // Get all types of the SSO plugins:
            var type = typeof(ISSOManagement);
            var ssoImplementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            // Register each SSO implementation as a slash command
            var cmd = new SlashCommandBuilder();
            cmd.WithName("set-sso");
            cmd.WithDescription("Configure an SSO server.");
            cmd.WithDMPermission(false);
            cmd.WithDefaultPermission(true);
            cmd.WithDefaultMemberPermissions(GuildPermission.Administrator);

            var ssoOption = new SlashCommandOptionBuilder()
                .WithName("sso-provider")
                .WithDescription("The implementation of SSO to be used in your server.")
                .WithRequired(true)
                .WithType(ApplicationCommandOptionType.String);

            // Add each implementation
            foreach(var ssoImplementation in ssoImplementations)
            {
                // Gets the name of the class and adds an option (different from the ISSOManagement's Name field):
                ssoOption.AddChoice(ssoImplementation.Name, ssoImplementation.Name);
            }
            ssoOption.AddChoice("None", "None");
            cmd.AddOption(ssoOption);

            // Return the slash command to be registered.
            return cmd.Build();
        }

    }
}

