using System;
using CCSODiscordBot.Services.Database.DataTables;
using MongoDB.Driver;

namespace CCSODiscordBot.Services.Database.Repository
{
    public interface IGuildRepository
    {
        /// <summary>
        /// Create a new guild
        /// </summary>
        /// <param name="newGuild">Guild class</param>
        /// <returns></returns>
        Task CreateNewGuildAsync(Guild newGuild);
        /// <summary>
        /// Get all guilds
        /// </summary>
        /// <returns></returns>
        Task<List<Guild>> GetAllAsync();
        /// <summary>
        /// Get guild by BSON ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Guild> GetByBsonIdAsync(string id);
        /// <summary>
        /// Get guild by discord ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Guild> GetByDiscordIdAsync(ulong id);
        /// <summary>
        /// Get guild with linq filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<Guild>> GetByLinqAsync(System.Linq.Expressions.Expression<Func<DataTables.Guild, bool>> filter, FindOptions? options = null);
        /// <summary>
        /// Update a guild
        /// </summary>
        /// <param name="guildToUpdate"></param>
        /// <returns></returns>
        Task UpdateGuildAsync(Guild guildToUpdate);
        /// <summary>
        /// Delete a guild
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteGuildAsync(string id);
    }
}

