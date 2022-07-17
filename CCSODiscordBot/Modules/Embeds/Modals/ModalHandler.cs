using System;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.Embeds.Modals
{
    public class EmbedHandler : InteractionModuleBase<ShardedInteractionContext>
    {
        // Responds to the modal.
        [ModalInteraction("embed_creator")]
        public async Task ModalResponse(EmbedCreator modal)
        {
            // Log embed creation:
            Console.WriteLine("Embed created by " + Context.User.Username + " with the title: " + Context.User.Username);

            // Create embed:
            var embed = new EmbedBuilder
            {
                // Embed property can be set within object initializer
                Title = modal.EmbedTitle,
                Description = modal.EmbedBody
            };

            // Check and add URL
            try
            {
                Uri embedUrl;
                if (modal.EmbedURL != null && modal.EmbedURL.Length >= 1 && modal.EmbedURL.ToLower() != "none")
                {
                    embedUrl = new Uri(modal.EmbedURL);
                    embed.WithUrl(embedUrl.ToString());
                }
            }
            catch (UriFormatException)
            {
                await RespondAsync("Could not parse URL. URL: " + modal.EmbedURL, ephemeral: true);
                return;
            }

            // Check color and add
            try
            {
                Color color = new Color(System.Drawing.ColorTranslator.FromHtml(modal.EmbedColor).R, System.Drawing.ColorTranslator.FromHtml(modal.EmbedColor).G, System.Drawing.ColorTranslator.FromHtml(modal.EmbedColor).B);
                embed.WithColor(color);
            }
            catch(ArgumentException)
            {
                await RespondAsync("Could not parse color. Color: " + modal.EmbedColor, ephemeral: true);
                return;
            }
            // Add timestamp
            embed.WithCurrentTimestamp();

            // Send embed
            await Context.Channel.SendMessageAsync(embed: embed.Build());

            // Respond to the modal.
            await RespondAsync("Embed created!", ephemeral: true);
        }
    }
}

