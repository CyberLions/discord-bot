using System;
using System.Threading.Tasks;
using CCSODiscordBot.Services.Database.DataTables;
using MongoDB.Driver;

namespace CCSODiscordBot.Services.Database.Repository
{
    public class GuildRepository:IGuildRepository
    {
        private readonly IMongoCollection<Guild> _guildCollection;

        public GuildRepository(IMongoDatabase mongoDatabase)
        {
            _guildCollection = mongoDatabase.GetCollection<Guild>("guild");
        }

        // CRUD Operations:
        #region Create
        public async Task CreateNewGuildAsync(Guild newGuild)
        {
            await _guildCollection.InsertOneAsync(newGuild);
        }
        #endregion Create
        #region Read
        public async Task<List<Guild>> GetAllAsync()
        {
            return await _guildCollection.Find(_ => true).ToListAsync();
        }
        public async Task<Guild> GetByBsonIdAsync(string id)
        {
            return await _guildCollection.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Guild> GetByDiscordIdAsync(ulong id)
        {
            return await _guildCollection.Find(_ => _.DiscordID == id).FirstOrDefaultAsync();
        }
        public async Task<List<Guild>> GetByLinqAsync(System.Linq.Expressions.Expression<Func<DataTables.Guild, bool>> filter, FindOptions? options = null)
        {
            return await _guildCollection.Find(filter, options).ToListAsync();
        }
        #endregion Read
        #region Update
        public async Task UpdateGuildAsync(Guild guildToUpdate)
        {
            await _guildCollection.ReplaceOneAsync(x => x.Id == guildToUpdate.Id, guildToUpdate);
        }
        #endregion Update
        #region Delete
        public async Task DeleteGuildAsync(string id)
        {
            await _guildCollection.DeleteOneAsync(x => x.Id == id);
        }
        #endregion Delete
    }
}

