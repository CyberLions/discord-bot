using System;
namespace CCSODiscordBot.Services.Database.DataTables.SubClasses
{
    public class ClassStanding
    {
        public string Name { get; set; }
        public ulong Role { get; set; }
        public bool RequireVerification { get; set; }
    }
}

