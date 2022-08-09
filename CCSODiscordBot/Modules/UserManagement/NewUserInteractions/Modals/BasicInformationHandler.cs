using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CCSODiscordBot.Modules.Embeds.Modals;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.UserManagement.Modals
{
    public class BasicInformationHandler : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IGuildRepository _iGuildRepository;

        public BasicInformationHandler(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _iUserRepository = iUserRepository;
            _iGuildRepository = iGuildRepository;
        }

        [ModalInteraction("user-basic-info")]
        public async Task ModalResponse(BasicInformationEmbed modal)
        {
            await Context.Interaction.DeferAsync(true);

            //Check email:
            if(!new EmailAddressAttribute().IsValid(modal.Email))
            {
                // Handle invalid email
                await Context.Interaction.RespondAsync("Invalid email format. Try again.");
                return;
            }
            // Parse email:
            MailAddress email = new MailAddress(modal.Email);

            bool psuEmail = false;
            if (email.Host.EndsWith("psu.edu"))
            {
                psuEmail = true;
            }
            // Check DB for alt account:
            if((await _iUserRepository.GetByLinqAsync(_ => _.DiscordGuildID == Context.Guild.Id && _.Email == email.Address && _.DiscordID != Context.User.Id)).Count > 0)
            {
                // Duplicate email:
                await Context.Interaction.RespondAsync("Your email is already registered in the DB under a seperate account. Please contact the mods for further support.");
                return;
            }
            Services.DataTables.User user;
            // Check for unfinished setup:
            if ((await _iUserRepository.GetByLinqAsync(_ => _.DiscordGuildID == Context.Guild.Id && _.DiscordID == Context.User.Id)).Count > 0)
            {
                user = (await _iUserRepository.GetByLinqAsync(_ => _.DiscordGuildID == Context.Guild.Id && _.Email == email.Address && _.DiscordID == Context.User.Id)).First();

                user.FirstName = modal.FirstName.Trim();
                user.LastName = modal.LastName.Trim();
                if(user.Email != email.Address)
                {
                    user.Email = email.Address;
                    user.verified = false;
                    user.VerificationNumber = null;
                }

                await _iUserRepository.UpdateUserAsync(user);
            }
            else
            {
                // Create the user account:
                user = new Services.DataTables.User();

                // Set vars:
                user.DiscordID = Context.User.Id;
                user.DiscordGuildID = Context.Guild.Id;
                user.Email = email.Address;
                user.FirstName = modal.FirstName.Trim();
                user.LastName = modal.LastName.Trim();
                user.verified = false;
                user.VerificationNumber = null;

                // Add to DB:
                await _iUserRepository.CreateNewUserAsync(user);
            }

            // Set Discord nickname:
            try
            {
                await Context.Guild.GetUser(Context.User.Id).ModifyAsync(_ => _.Nickname = modal.FirstName.Trim() + " " + modal.LastName.Trim());
            }
            catch(Discord.Net.HttpException e) when (e.HttpCode == System.Net.HttpStatusCode.Forbidden)
            {
                Console.WriteLine("403: Not allowed to set nickname in " + Context.Guild.Name);
            }

            // See if email validation is needed:
            // Only needed for PSU emails
            if(!user.verified || psuEmail)
            {
                // Verification needed:
                Random random = new Random();
                user.VerificationNumber = random.Next(100000, 999999);
                // Upload to DB:
                await _iUserRepository.UpdateUserAsync(user);
            }
            else
            {
                // Role assignment prompts:

            }

        }
    }
}

