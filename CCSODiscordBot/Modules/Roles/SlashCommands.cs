using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Roles
{
	public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
	{
        [SlashCommand("reactroles", "Creates a react role embed with buttons for each role.", runMode: RunMode.Async)]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task RoleSelector([Summary(description: "Role that the users can assign themselves to.")] SocketRole role, [Summary(description: "Title of the embed")] string? title = null, [Summary(description: "Embed description")] string? description = null)
        {
            // Tell Discord and user that we are processing their request:
            await DeferAsync(true);

            EmbedBuilder? embed = null;
            // See if either one is null:
            if (title is not null || description is not null)
            {
                embed = new EmbedBuilder();
                if (title is not null)
                {
                    embed.WithTitle(title);
                }
                if (description is not null)
                {
                    embed.WithDescription(description);
                }
            }
            // Create a list of button compontents:
            ComponentBuilder components = new ComponentBuilder();

            ButtonBuilder roleBtn = new ButtonBuilder();
            roleBtn.WithLabel(role.Name);
            roleBtn.WithCustomId($"toggle-role-{role.Id}");
            roleBtn.WithStyle(ButtonStyle.Primary);
            components.WithButton(roleBtn);

            // Log this event:
            Console.WriteLine("User '" + Context.User.Username + "' created a react roles embed.");

            // Send the buttons
            await FollowupAsync("Role selector created.");
            await Context.Channel.SendMessageAsync(embed: embed?.Build(), components: components.Build());
        }
    }
}