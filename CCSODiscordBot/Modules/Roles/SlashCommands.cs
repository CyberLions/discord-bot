using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Roles
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
        [SlashCommand("reactroles", "Creates a react role embed with buttons for each role.", runMode: RunMode.Async)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task RoleSelector()
        {
            // Tell Discord and user that we are processing their request:
            await DeferAsync(true);

            // Get roles in the guild:
            var roles = Context.Guild.Roles;

            // Create the basics of the dropdown:
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Select roles")
                .WithMinValues(1)
                .WithCustomId("roleSelect");

            // Loop through roles and fill the dropdown:
            foreach (SocketRole role in roles)
            {
                if (!role.IsEveryone)
                {
                    SelectMenuOptionBuilder option = new SelectMenuOptionBuilder();
                    option.WithLabel(role.Name);
                    option.WithValue(role.Id.ToString());
                    menuBuilder.AddOption(option);
                }
            }
            // Set max to 5 or less:
            menuBuilder.WithMaxValues((menuBuilder.Options.Count > 5) ? 5 : menuBuilder.Options.Count);

            // Create a component builder:
            var compBuilder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            // Send the message:
            await FollowupAsync("Select the roles that you would like to use.", components: compBuilder.Build());
        }
    }
}