using System;
using MongoDB.Driver;

namespace CCSODiscordBot.Services.Database.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Create new user entry in DB
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        Task CreateNewUserAsync(DataTables.User newUser);
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        Task<List<DataTables.User>> GetAllAsync();
        /// <summary>
        /// Get user by Bson ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataTables.User> GetByBsonIdAsync(string id);
        /// <summary>
        /// Get user by Discord ID
        /// Only shows users from specific guild.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataTables.User> GetByDiscordIdAsync(ulong userID, ulong guildID);
        /// <summary>
        /// Get users with a linq filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<List<DataTables.User>> GetByLinqAsync(System.Linq.Expressions.Expression<Func<DataTables.User, bool>> filter, FindOptions? options = null);
        /// <summary>
        /// Update user in DB
        /// </summary>
        /// <param name="userToUpdate"></param>
        /// <returns></returns>
        Task UpdateUserAsync(DataTables.User userToUpdate);
        /// <summary>
        /// Delete user from DB using BSON id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteUserAsync(string id);
    }
}
