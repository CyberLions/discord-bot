using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.UserManagement.NewUserInteractions
{
    public class InitialWelcomeButtonRespond : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IGuildRepository _iGuildRepository;

        public InitialWelcomeButtonRespond(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _iUserRepository = iUserRepository;
            _iGuildRepository = iGuildRepository;
        }

        [ComponentInteraction("get-started-*")]
        [RequireContext(ContextType.Guild)]
        public async Task WelcomeButton(ulong userID)
        {
            // Defer interaction:
            await Context.Interaction.DeferAsync(true);
            // (try to) Get user from DB:
            var dbUser = await _iUserRepository.GetByDiscordIdAsync(Context.User.Id, Context.Guild.Id);
            // Ensure user has not been welcomed before:
            if (dbUser != null && dbUser.verified)
            {
                await Context.Interaction.FollowupAsync("You have already verified your membership. Please contact a mod if you are having issues or would like to update your information.", ephemeral: true);
                return;
            }
            // Begin new user registration process:

        }
    }
}

