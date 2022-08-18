using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CCSODiscordBot.Services.Database.DataTables
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("discordId")]
        public ulong DiscordID { get; set; }

        [BsonElement("discordGuildId")]
        public ulong DiscordGuildID { get; set; }

        [BsonElement("fname")]
        public string FirstName { get; set; }

        [BsonElement("lname")]
        public string LastName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("verified")]
        public bool verified { get; set; }

        [BsonElement("verificationNumber")]
        public int? VerificationNumber { get; set; }
    }
}

