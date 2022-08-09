using Microsoft.Extensions.Configuration;

namespace CCSODiscordBot
{
	/// <summary>
    /// Stores all of the configurable variables.
    /// </summary>
	public class ConfigHandlingService
	{
        /// <summary>
        /// The discord token
        /// </summary>
        private readonly string? _DiscordToken;
        /// <summary>
        /// The mongo DB connection string
        /// </summary>
        private readonly string? _MongoDBConnectionString;

        /// <summary>
        /// Initialize the secrets:
        /// </summary>
        /// <returns></returns>
        public ConfigHandlingService()
        {
            // Load the secrets:
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            // Get and set the DiscordToken:
            _DiscordToken = (config["DiscordToken"] == null) ? (Environment.GetEnvironmentVariable("DiscordToken")) : (config["DiscordToken"]);

            // Get and set the MongoDBConnectionString:
            _MongoDBConnectionString = (config["MongoDBConnectionString"] == null) ? (Environment.GetEnvironmentVariable("MongoDBConnectionString")) : (config["MongoDBConnectionString"]);
        }

        /// <summary>
        /// The discord token
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the discord token is not set.</exception>
		public string DiscordToken {
            get
            {
                if (string.IsNullOrEmpty(_DiscordToken))
                {
                    throw new NullReferenceException("The discord token is not set.");
                }
                return _DiscordToken;
            }
        }
        /// <summary>
        /// The MongoDB connection string
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the connection string is not set</exception>
        public string MongoDBConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_MongoDBConnectionString))
                {
                    throw new NullReferenceException("The MongoDB connection string is not set.");
                }
                return _MongoDBConnectionString;
            }
        }
    }
}

