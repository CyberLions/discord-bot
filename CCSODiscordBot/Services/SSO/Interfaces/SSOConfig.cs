using System;
namespace CCSODiscordBot.Services.SSO.Interfaces
{
    public interface SSOConfig
    {
        /// <summary>
        /// The name of the SSO application
        /// </summary>
        public string Name
        {
            get;
        }
        /// <summary>
        /// List of settings to be configured
        /// </summary>
        public List<string> Settings
        {
            get;
        }
        /// <summary>
        /// Get a setting value
        /// </summary>
        public string GetSetting(string setting);
        /// <summary>
        /// Set a setting
        /// </summary>
        public void SetSetting(KeyValuePair<string,string> setting);
    }
}

