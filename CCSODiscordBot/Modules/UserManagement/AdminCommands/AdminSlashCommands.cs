using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.UserManagement.AccountVerification
{
    public class AdminSlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IGuildRepository _IGuildRepository;
        public AdminSlashCommands(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _IUserRepository = iUserRepository;
            _IGuildRepository = iGuildRepository;
        }

        [SlashCommand("verify-remove-user", "Remove the verification status from a user.")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [EnabledInDm(false)]
        public async Task VerifyRemoveUser([Summary("User", "The user to remove verification status from.")] SocketGuildUser user, [Summary("Confirm", "True to confirm you would like to proceed with this action.")] bool confirm = false)
        {
            await Context.Interaction.DeferAsync(true);

            if (!confirm)
            {
                await Context.Interaction.FollowupAsync("Command aborted.");
                return;
            }

            // Log this event:
            Console.WriteLine(Context.User.Id + " removed " + user.Id + "'s verification status");

            // Get user:
            var dbUser = await _IUserRepository.GetByDiscordIdAsync(user.Id, user.Guild.Id);
            dbUser.Verified = false;
            await _IUserRepository.UpdateUserAsync(dbUser);

            await Context.Interaction.FollowupAsync("Removed user verification status");
        }

        [SlashCommand("verify-remove-group", "Remove the verification status from all users in a group.")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [EnabledInDm(false)]
        public async Task VerifyRemoveGroup([Summary("Group", "The group to remove verification status' from.")] SocketRole group, [Summary("Confirm", "True to confirm you would like to proceed with this action.")] bool confirm = false)
        {
            await Context.Interaction.DeferAsync(true);

            if (!confirm)
            {
                await Context.Interaction.FollowupAsync("Command aborted.");
                return;
            }

            // Log this event:
            Console.WriteLine(Context.User.Id + " removed all user's in group " + group.Name + " verification status");

            int removeCount = 0;

            foreach(var user in group.Members)
            {
                var dbUser = await _IUserRepository.GetByDiscordIdAsync(user.Id, user.Guild.Id);
                if (dbUser.Verified)
                {
                    dbUser.Verified = false;
                    await _IUserRepository.UpdateUserAsync(dbUser);
                    removeCount++;
                }
            }

            await Context.Interaction.FollowupAsync("Removed verification status from "+ removeCount+" users.");
        }
    }
}

