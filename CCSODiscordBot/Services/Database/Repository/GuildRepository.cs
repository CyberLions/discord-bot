using System;
using MongoDB.Driver;

namespace CCSODiscordBot.Services.Database.Repository
{
    public class GuildRepository:IGuildRepository
    {
        private readonly IMongoCollection<DataTables.guild> _guildCollection;

        public GuildRepository(IMongoDatabase mongoDatabase)
        {
            _guildCollection = mongoDatabase.GetCollection<DataTables.guild>("guild");
        }
    }
}

