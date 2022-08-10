using System;
using CCSODiscordBot.Services.Database.DataTables.SubClasses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CCSODiscordBot.Services.Database.DataTables
{
    public class Guild
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("discordId")]
        public ulong DiscordID { get; set; }

        [BsonElement("welcomeEnabled")]
        public bool WelcomeEnabled { get; set; }

        [BsonElement("welcomeChannel")]
        public ulong WelcomeChannel { get; set; }

        [BsonElement("standings")]
        public List<ClassStanding>? ClassStandings { get; set; }

        [BsonElement("leaveEnabled")]
        public bool LeaveEnabled { get; set; }
    }
}

