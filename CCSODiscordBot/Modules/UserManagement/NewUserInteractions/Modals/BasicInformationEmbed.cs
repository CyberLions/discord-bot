using System;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.UserManagement.Modals
{
    public class BasicInformationEmbed : IModal
    {
        public string Title => "New Member Registration";

        // First Name prompt:
        [InputLabel("First Name")]
        [ModalTextInput("fname", placeholder: "John", maxLength: 25)]
        public string FirstName { get; set; }

        // Last Name prompt:
        [InputLabel("Last Name")]
        [ModalTextInput("lname", placeholder: "Doe", maxLength: 25)]
        public string LastName { get; set; }

        // Email prompt:
        [InputLabel("Email (PSU required for student/faculty)")]
        [ModalTextInput("email", placeholder: "ccso@psu.edu", maxLength: 100)]
        public string Email { get; set; }
    }
}

