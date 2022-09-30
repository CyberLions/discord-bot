using System;
using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.UserManagement.AccountVerification
{
    public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IGuildRepository _IGuildRepository;
        public SlashCommands(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _IUserRepository = iUserRepository;
            _IGuildRepository = iGuildRepository;
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
            // Check to see if the user has already been verified:
            else if (user.Verified)
            {
                await Context.Interaction.FollowupAsync("Your account has already been verified.");
                var guild = await _IGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
                // Set member role:
                if (guild.VerifiedMemberRole != null)
                {
                    await Context.Guild.GetUser(Context.User.Id).AddRoleAsync((ulong)guild.VerifiedMemberRole);
                }
            }
            // Ensure the user was sent a code:
            else if(user.VerificationNumber == null)
            {
                await Context.Interaction.FollowupAsync("You have not been sent a verification code.");
            }
            // Check to see if the code matched the one sent to their email:
            else if(user.VerificationNumber == code)
            {
                user.Verified = true;
                user.VerificationNumber = null;
                await _IUserRepository.UpdateUserAsync(user);
                var guild = await _IGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
                // Set member role:
                if(guild.VerifiedMemberRole != null)
                {
                    await Context.Guild.GetUser(Context.User.Id).AddRoleAsync((ulong) guild.VerifiedMemberRole);
                }

                // Ensure guild has set up standings and/or interest roles
                if (guild.ClassStandings?.Count > 0 || guild.InterestRoles?.Count > 0)
                {
                    await Context.Interaction.FollowupAsync(embed: RolePrompt.RolePromptEmbeds.Embeds(true, guild.ClassStandings, guild.InterestRoles, "Account Verified. Select Roles", "Your email address has been verified.").Build(), components: RolePrompt.RolePromptComponents.BtnComponent(true, guild.ClassStandings, guild.InterestRoles).Build());
                }
                else
                {
                    await Context.Interaction.FollowupAsync("Your account has been set up!", ephemeral: true);
                }
            }
            else
            {
                await Context.Interaction.FollowupAsync("Invalid code.");
            }
        }
    }
}
