using System;
using MongoDB.Driver;

namespace CCSODiscordBot.Services.Database.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly IMongoCollection<DataTables.User> _userCollection;

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _userCollection = mongoDatabase.GetCollection<DataTables.User>("user");
        }
    }
}

