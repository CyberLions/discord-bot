using System;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.Greeter
{
    public class Greeting
    {
        public static async Task UserJoin(SocketGuildUser user)
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

