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
            }
            // Check for unfinished setup:
            if ((await _iUserRepository.GetByLinqAsync(_ => _.DiscordGuildID == Context.Guild.Id && _.DiscordID == Context.User.Id)).Count > 0)
            {
                var user = (await _iUserRepository.GetByLinqAsync(_ => _.DiscordGuildID == Context.Guild.Id && _.Email == email.Address && _.DiscordID == Context.User.Id)).First();

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
                Services.DataTables.User newUser = new Services.DataTables.User();

                // Set vars:
                newUser.DiscordID = Context.User.Id;
                newUser.DiscordGuildID = Context.Guild.Id;
                newUser.Email = email.Address;
                newUser.FirstName = modal.FirstName.Trim();
                newUser.LastName = modal.LastName.Trim();
                newUser.verified = false;
                newUser.VerificationNumber = null;

                // Add to DB:
                await _iUserRepository.CreateNewUserAsync(newUser);
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

            // Validate email:


        }
    }
}

