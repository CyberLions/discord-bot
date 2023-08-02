using System;
using CCSODiscordBot.Services.SSO.Interfaces;

namespace CCSODiscordBot.Services.SSO.Implementations.Zitadel
{
    public class ZitadelConfig : SSOConfig
    {
        private List<KeyValuePair<string, string>> _configs = new List<KeyValuePair<string, string>>();

        public string Name
        {
            get;
        } = "Zitadel";

        public List<string> Settings
        {
            get
            {
                List<string> settings = new List<string>
                {
                    "ApiUrl",
                    "Pat",
                    "ZitadelDiscordIDPId"
                };

                return settings;
            }
        }

        public string GetSetting(string setting)
        {
            var matches = _configs.Where(kvp => kvp.Key.Equals(setting));
            if (matches.Count() < 1)
            {
                throw new NullReferenceException("Setting not found");
            }
            return matches.First().Value;
        }

        public void SetSetting(KeyValuePair<string, string> setting)
        {
            if (!Settings.Contains(setting.Key))
            {
                throw new NullReferenceException("Setting does not exist.");
            }
            _configs.Add(setting);
        }
    }
}

