using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.UserManagement.AccountVerification
{
    public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private IUserRepository _IUserRepository;
        public SlashCommands(IUserRepository iUserRepository)
        {
            _IUserRepository = iUserRepository;
        }

        [SlashCommand("verify", "Verify your account with a code sent to your email.")]
        [EnabledInDm(false)]
        public async Task Verify([Summary("Code", "The code sent to your email.")][MinValue(100000)][MaxValue(999999)]int code)
        {
            // Show loading animation:
            await Context.Interaction.DeferAsync(true);

            // Pull user from DB:
            var user = await _IUserRepository.GetByDiscordIdAsync(Context.User.Id, Context.Guild.Id);
            // Ensure user is in the DB:
            if(user == null)
            {
                await Context.Interaction.FollowupAsync("You have not registered with the bot yet.");
            }
            // Ensure the user was sent a code:
            else if(user.VerificationNumber == null)
            {
                await Context.Interaction.FollowupAsync("You have not been sent a verification code.");
            }
            else if(user.VerificationNumber == code)
            {
                user.verified = true;
                user.VerificationNumber = null;
                await _IUserRepository.UpdateUserAsync(user);
                await Context.Interaction.FollowupAsync("Thanks! Your account has been verified.");
            }
            else
            {
                await Context.Interaction.FollowupAsync("Invalid code.");
            }
        }
    }
}

