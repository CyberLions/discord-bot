using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CCSODiscordBot.Services.DataTables
{
    public class guild
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("discordId")]
        public string DiscordID { get; set; }

        [BsonElement("welcomeEnabled")]
        public bool WelcomeEnabled { get; set; }

        [BsonElement("welcomeChannel")]
        public ulong WelcomeChannel { get; set; }

        [BsonElement("welcomeRoles")]
        public List<ulong> WelcomeRoles { get; set; }

        [BsonElement("leaveEnabled")]
        public bool LeaveEnabled { get; set; }
    }
}

