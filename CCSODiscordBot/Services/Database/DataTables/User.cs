using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CCSODiscordBot.Services.Database.DataTables
{
    public class User
    {
        public User()
        {
            Id = null;
            DiscordID = default;
            DiscordGuildID = default;
            FirstName = null;
            LastName = null;
            Email = null;
            Verified = false;
            VerificationNumber = null;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("discordId")]
        public ulong DiscordID { get; set; }

        [BsonElement("discordGuildId")]
        public ulong DiscordGuildID { get; set; }

        [BsonElement("fname")]
        public string? FirstName { get; set; }

        [BsonElement("lname")]
        public string? LastName { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("verified")]
        public bool Verified { get; set; }

        [BsonElement("verificationNumber")]
        public int? VerificationNumber { get; set; }
    }
}

