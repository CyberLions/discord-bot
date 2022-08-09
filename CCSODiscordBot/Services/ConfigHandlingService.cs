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
        /// SMTP Email server
        /// </summary>
        private readonly string? _SMTPAddr;
        /// <summary>
        /// Port for SMTP server
        /// </summary>
        private readonly uint _SMTPPort;
        /// <summary>
        /// Email address for SMTP server
        /// </summary>
        private string? _SMTPEmail;
        /// <summary>
        /// The SMTP password
        /// </summary>
        private string? _SMTPPassword;

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

            // Get and set the SMTP email server:
            _SMTPAddr = (config["SMTPAddr"] == null) ? (Environment.GetEnvironmentVariable("SMTPAddr")) : (config["SMTPAddr"]);

            // Get and set the SMTP email port (with null check):
            _SMTPPort = uint.Parse(((config["SMTPPort"] == null) ? (Environment.GetEnvironmentVariable("SMTPPort")) : (config["SMTPPort"])) ?? "587");

            // Get and set the SMTP email address:
            _SMTPEmail = (config["SMTPEmail"] == null) ? (Environment.GetEnvironmentVariable("SMTPEmail")) : (config["SMTPEmail"]);

            // Get and set the SMTP password:
            _SMTPPassword = (config["SMTPPassword"] == null) ? (Environment.GetEnvironmentVariable("SMTPPassword")) : (config["SMTPPassword"]);
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
        /// <summary>
        /// The SMTP email server
        /// </summary>
        public string SMTPAddr
        {
            get
            {
                if (string.IsNullOrEmpty(_SMTPAddr))
                {
                    throw new NullReferenceException("The SMTP address string is not set.");
                }
                return _SMTPAddr;
            }
        }
        /// <summary>
        /// The SMTP email server
        /// </summary>
        public uint SMTPPort
        {
            get
            {
                return _SMTPPort;
            }
        }
        /// <summary>
        /// The SMTP email server
        /// </summary>
        public string SMTPEmail
        {
            get
            {
                if (string.IsNullOrEmpty(_SMTPEmail))
                {
                    throw new NullReferenceException("The SMTP email address string is not set.");
                }
                return _SMTPEmail;
            }
        }
        /// <summary>
        /// The SMTP email password
        /// </summary>
        public string SMTPPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_SMTPPassword))
                {
                    throw new NullReferenceException("The SMTP password string is not set.");
                }
                return _SMTPPassword;
            }
        }
    }
}

