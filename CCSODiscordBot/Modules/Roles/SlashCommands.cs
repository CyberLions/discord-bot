using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Roles
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
        [SlashCommand("reactroles", "Creates a react role embed with buttons for each role.")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [EnabledInDm(false)]
        public async Task RoleSelector(SocketRole role1, SocketRole? role2 = null, SocketRole? role3 = null, SocketRole? role4 = null)
        {
            // Tell Discord and user that we are processing their request:
            await DeferAsync(true);
            // Get roles and add them to a list
            List<SocketRole> roles = new List<SocketRole>();
            roles.Add(role1);
            if (role2 != null)
            {
                roles.Add(role2);
            }
            if (role3 != null)
            {
                roles.Add(role3);
            }
            if (role4 != null)
            {
                roles.Add(role4);
            }
            // Process the list
            // Create a list of button compontents:
            ComponentBuilder components = new ComponentBuilder();
            foreach (SocketRole role in roles)
            {
                if (role.Permissions.Administrator)
                {
                    await FollowupAsync("Cannot use role with Admin permissions.", ephemeral: true);
                    return;
                }
                // Create the button:
                ButtonBuilder roleBtn = new ButtonBuilder();
                roleBtn.WithLabel(role.Name);
                roleBtn.WithCustomId($"toggle-role-{role.Id}");
                roleBtn.WithStyle(ButtonStyle.Primary);

                // Add the button:
                components.WithButton(roleBtn);
            }

            // Log this event and report to user:
            Console.WriteLine("User '" + Context.User.Username + "' created a react roles embed.");
            await FollowupAsync("Done.", ephemeral: true);

            // Send the buttons
            await Context.Channel.SendMessageAsync(" ", components: components.Build());
        }
    }
}