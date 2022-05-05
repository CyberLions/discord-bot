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
        private string? _DiscordToken;

        /// <summary>
        /// Initialize the secrets:
        /// </summary>
        /// <returns></returns>
        public ConfigHandlingService()
        {
            // Load the secrets:
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            // Get and set the secrets:
            _DiscordToken = config["DiscordToken"];
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
    }
}

