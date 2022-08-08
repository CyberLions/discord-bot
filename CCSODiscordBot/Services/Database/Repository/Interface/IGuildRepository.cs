using System;
using CCSODiscordBot.Services.DataTables;
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
        Task CreateNewGuildAsync(guild newGuild);
        /// <summary>
        /// Get all guilds
        /// </summary>
        /// <returns></returns>
        Task<List<guild>> GetAllAsync();
        /// <summary>
        /// Get guild by BSON ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<guild> GetByBsonIdAsync(string id);
        /// <summary>
        /// Get guild by discord ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<guild> GetByDiscordIdAsync(ulong id);
        /// <summary>
        /// Get guild with linq filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<guild>> GetByLinqAsync(FilterDefinition<guild> filter, FindOptions? options = null);
        /// <summary>
        /// Update a guild
        /// </summary>
        /// <param name="guildToUpdate"></param>
        /// <returns></returns>
        Task UpdateGuildAsync(guild guildToUpdate);
        /// <summary>
        /// Delete a guild
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteGuildAsync(string id);
    }
}

