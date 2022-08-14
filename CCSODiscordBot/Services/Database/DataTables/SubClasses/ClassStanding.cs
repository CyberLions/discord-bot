using System;
using Discord;

namespace CCSODiscordBot.Services.Database.DataTables.SubClasses
{
    public class ClassStanding
    {
        public string Name { get; set; }
        public ulong Role { get; set; }
        public bool RequireVerification { get; set; }
        public IEmote? Emote { get; set; }
    }
}

