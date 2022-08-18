using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Greeter
{
    public class Greeting
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IGuildRepository _iGuildRepository;

        public Greeting(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _iUserRepository = iUserRepository;
            _iGuildRepository = iGuildRepository;
        }

        public async Task UserJoin(SocketGuildUser user)
        {
            // ignore bots:
            if (user.IsBot)
            {
                return;
            }
            // Get the guild:
            var dbGuild = await _iGuildRepository.GetByDiscordIdAsync(user.Guild.Id);
            var dbUser = await _iUserRepository.GetByDiscordIdAsync(user.Id, user.Guild.Id);
            // Check if guild has enabled the greeting module
            // Also check if the user was already added to the DB and verified:
            if (!dbGuild.WelcomeEnabled || (dbUser != null && dbUser.verified))
            {
                // If not, return
                return;
            }
            // Get channel to send welcome to:
            var welcomeChan = user.Guild.GetTextChannel(dbGuild.WelcomeChannel);

            // Build welcome embed:
            EmbedBuilder welcomeEmbed = new EmbedBuilder();
            welcomeEmbed.Title = "Welcome " + user.DisplayName + "!";
            welcomeEmbed.Description = "Welcome to the " + user.Guild.Name + " server! Please take a moment to read the rules and then click the button below to verify your membership!";
            welcomeEmbed.Color = Color.Teal;
            welcomeEmbed.Timestamp = DateTimeOffset.Now;
            welcomeEmbed.ThumbnailUrl = user.GetAvatarUrl(ImageFormat.Auto);

            // Welcome Embed Button:
            ButtonBuilder getStartedButton = new ButtonBuilder();
            getStartedButton.WithLabel("Get Started");
            getStartedButton.Style = ButtonStyle.Success;
            getStartedButton.WithCustomId("get-started-" + user.Id);
            ComponentBuilder component = new ComponentBuilder();
            component.WithButton(getStartedButton);

            // Send the message and mention the user:
            await welcomeChan.SendMessageAsync(text: MentionUtils.MentionUser(user.Id),embed: welcomeEmbed.Build(), components: component.Build(), allowedMentions: AllowedMentions.All);
       
        }
    }
}

