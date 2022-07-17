using System;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.Embeds.Modals
{
    public class EmbedCreator : IModal
    {
        // Modal title:
        public string Title => "Embed Builder";

        // Embed title prompt:
        [InputLabel("Embed Title")]
        [ModalTextInput("embed_title", placeholder: "A good title", maxLength: 256)]
        public string EmbedTitle { get; set; }

        // Embed URL:
        [InputLabel("Embed URL")]
        [ModalTextInput("embed_url", TextInputStyle.Short, placeholder: "https://ccso.psu.edu/ or none to skip", initValue: "none", maxLength: 256)]
        public string EmbedURL { get; set; }

        // Embed body:
        [InputLabel("Embed Body")]
        [ModalTextInput("embed_body", TextInputStyle.Paragraph, placeholder: "Markdown supported body")]
        public string EmbedBody { get; set; }

        // Embed color:
        [InputLabel("Embed Color")]
        [ModalTextInput("embed_color", TextInputStyle.Short, initValue: "blue", maxLength: 20)]
        public string EmbedColor { get; set; }
    }
}

