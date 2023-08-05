using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CCSODiscordBot.Services.SSO.Interfaces
{
    public class SSOConfig
    {
        public SSOConfig()
        {
            Settings = new List<string>();
            Configuration = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// The name of the SSO application
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of settings to be configured
        /// </summary>
        public List<string> Settings { get; set; }
        /// <summary>
        /// All configuration and corresponding values
        /// </summary>
        public List<KeyValuePair<string, string>> Configuration { get; set; }
        /// <summary>
        /// Get a setting value
        /// </summary>
        public string GetSetting(string setting)
        {
            var matches = Configuration.Where(kvp => kvp.Key.Equals(setting));
            if (matches.Count() < 1)
            {
                throw new NullReferenceException("Setting not found");
            }
            return matches.First().Value;
        }
        /// <summary>
        /// Set a setting
        /// </summary>
        public void SetSetting(KeyValuePair<string,string> setting)
        {
            if (!Settings.Contains(setting.Key))
            {
                throw new NullReferenceException("Setting does not exist.");
            }
            Configuration.Add(setting);
        }
    }
}

