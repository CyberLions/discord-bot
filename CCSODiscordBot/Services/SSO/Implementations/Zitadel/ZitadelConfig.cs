using System;
using CCSODiscordBot.Services.SSO.Interfaces;

namespace CCSODiscordBot.Services.SSO.Implementations.Zitadel
{
    public class ZitadelConfig : SSOConfig
    {
        public ZitadelConfig()
        {
            Settings = new List<string>
                {
                    "ApiUrl",
                    "Pat",
                    "ZitadelDiscordIDPId",
                    "ZitadelDiscordExternalSSOSync"
                };
            Name = "Zitadel";
        }
    }
}

