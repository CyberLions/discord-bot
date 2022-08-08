using System;
using CCSODiscordBot.Services.Database.Repository;
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
            // Start welcome.
            
        }
    }
}

