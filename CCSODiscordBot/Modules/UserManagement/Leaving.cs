using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.UserManagement
{
    public class Leaving
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IGuildRepository _iGuildRepository;

        public Leaving(IUserRepository iUserRepository, IGuildRepository iGuildRepository)
        {
            _iUserRepository = iUserRepository;
            _iGuildRepository = iGuildRepository;
        }

        public async Task UserLeft(SocketGuild guild, SocketUser user)
        {
            var dbGuild = await _iGuildRepository.GetByDiscordIdAsync(guild.Id);
            if (dbGuild.LeaveEnabled && dbGuild.LeaveChannel != null)
            {
                await guild.GetTextChannel((ulong) dbGuild.LeaveChannel).SendMessageAsync("User " + user.Username + " has left the server.");
            }
        }
    }
}

