using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;
using Google.Api;

namespace CCSODiscordBot.Modules.UserManagement.AdminCommands
{
	public class UnverifyPSUAliasAccounts : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IUserRepository _iUserRepository;
        public UnverifyPSUAliasAccounts(IUserRepository iUserRepository)
		{
            _iUserRepository = iUserRepository;
        }
        [SlashCommand("verify-remove-alias", "Remove the verification status from all users with a PSU alias account.")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [EnabledInDm(false)]
        public async Task VerifyRemoveAlias([Summary("Confirm", "Confirm that you would like to proceed.")] bool confirm = false)
        {
            await Context.Interaction.DeferAsync(true);

            if (!confirm)
            {
                await Context.Interaction.FollowupAsync("Command aborted. Confirm param must be true.");
                return;
            }

            // Log this event:
            Console.WriteLine(Context.User.Id + " removed all PSU alias accounts verification status.");

            var users = await _iUserRepository.GetByLinqAsync(x => x.DiscordGuildID == Context.Guild.Id && x.Verified);
            int count = 0;


            // Parse all users and check email
            foreach(var user in users)
            {
                MailAddress? email = null;
                // Handle null emails:
                if (user.Email != null)
                {
                    email = new MailAddress(user.Email);
                }
                // Check email compliance status:
                if (email == null || !email.Host.EndsWith("psu.edu") || !Regex.Match(email.User, @"^[a-zA-Z]{3}\d+$").Success)
                {
                    // Email does not match requirements. Remove verification
                    user.Verified = false;
                    await _iUserRepository.UpdateUserAsync(user);
                    count++;
                }
            }
            // Notify user of status
            await Context.Interaction.FollowupAsync("Removed "+count+" user's verification status.");
        }
    }
}

