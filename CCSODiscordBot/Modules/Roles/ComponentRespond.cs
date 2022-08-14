using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Roles
{
	public class ComponentRespond : InteractionModuleBase<ShardedInteractionContext>
	{
		private IGuildRepository _IGuildRepository;
        private IUserRepository _IUserRepository;
        public ComponentRespond (IGuildRepository iGuildRepository, IUserRepository iUserRepository)
        {
			_IGuildRepository = iGuildRepository;
            _IUserRepository = iUserRepository;
        }

        [ComponentInteraction("toggle-role-*")]
		[RequireContext(ContextType.Guild)]
		public async Task RoleButton(ulong roleId)
		{
			await DeferAsync(true);

			// Get desired role:
			var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == roleId);
			// Get the user:
			SocketGuildUser? user = Context.User as SocketGuildUser;

			// Ensure role exists and isnt null:
			if (role is null)
			{
				await FollowupAsync("Role cannot be found. Contact an admin.");
				throw new NullReferenceException("Role button role cannot be found and is null.");
			}
			// Ensure user is a SocketGuildUser.
			if (user is null)
			{
				await FollowupAsync("User cannot be found. Contact an admin.");
				throw new NullReferenceException("User is not guild user.");
			}

			// Get user's roles:
			var usersRoles = user.Roles;
			// See if user has role:
			try
			{
				if (usersRoles.Contains(role))
				{
					// Remove:
					await user.RemoveRoleAsync(role);
					await FollowupAsync("Role " + role.Name + " has been removed from your account.", ephemeral: true);
				}
				else
				{
					// Add:
					await user.AddRoleAsync(role);
					await FollowupAsync("Role " + role.Name + " has been added to your account.", ephemeral: true);
				}
			}
			catch (HttpException e) when (e.DiscordCode == Discord.DiscordErrorCode.InsufficientPermissions)
			{
				await FollowupAsync("Bot does not have permission to perform this action. Contact an admin.", ephemeral: true);
			}
		}
        [ComponentInteraction("protected-toggle-role-*")]
        [RequireContext(ContextType.Guild)]
        public async Task ProtectedRoleButton(ulong roleId)
        {
            await DeferAsync(true);

            // Get desired role:
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Id == roleId);
            // Get the user:
            SocketGuildUser? user = Context.User as SocketGuildUser;

            // Ensure role exists and isnt null:
            if (role is null)
            {
                await FollowupAsync("Role cannot be found. Contact an admin.");
                throw new NullReferenceException("Role button role cannot be found and is null.");
            }
            // Ensure user is a SocketGuildUser.
            if (user is null)
            {
                await FollowupAsync("User cannot be found. Contact an admin.");
                throw new NullReferenceException("User is not guild user.");
            }
            // Check for verification:
            var dbUser = await _IUserRepository.GetByDiscordIdAsync(Context.User.Id, Context.Guild.Id);
            if (!dbUser.verified)
            {
                await FollowupAsync("Error: You need to have a verified PSU email to add this role.");
                return;
            }
            // Get user's roles:
            var usersRoles = user.Roles;
            // See if user has role:
            try
            {
                if (usersRoles.Contains(role))
                {
                    // Remove:
                    await user.RemoveRoleAsync(role);
                    await FollowupAsync("Role " + role.Name + " has been removed from your account.", ephemeral: true);
                }
                else
                {
                    // Add:
                    await user.AddRoleAsync(role);
                    await FollowupAsync("Role " + role.Name + " has been added to your account.", ephemeral: true);
                }
            }
            catch (HttpException e) when (e.DiscordCode == Discord.DiscordErrorCode.InsufficientPermissions)
            {
                await FollowupAsync("Bot does not have permission to perform this action. Contact an admin.", ephemeral: true);
            }
        }
        /// <summary>
        /// Roles to be created into buttons
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [ComponentInteraction("roleSelect")]
        public async Task SelectRolesToButtons(string[] roles)
        {
            // Create a list of button compontents:
            ComponentBuilder components = new ComponentBuilder();
            foreach (string roleIdStr in roles)
            {
                ulong roleId = ulong.Parse(roleIdStr);
                // Get the role:
                var role = Context.Guild.GetRole(roleId);

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
            await RespondAsync("Done.", ephemeral: true);

            // Send the buttons
            await Context.Channel.SendMessageAsync(" ", components: components.Build());
        }
    }
}