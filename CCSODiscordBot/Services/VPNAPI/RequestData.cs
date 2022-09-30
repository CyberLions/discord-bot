using System;
using System.Text.Json.Serialization;

namespace CCSODiscordBot.Services.VPNAPI
{
	public class RequestData
	{
		public RequestData(string discord, string first, string last, string psuemail, string reason, string username)
		{
			this.DiscordName = discord;
			this.FirstName = first;
			this.LastName = last;
			this.PSUEmail = psuemail;
            this.Reason = reason;
            this.DiscordUsername = username;
		}
		[JsonPropertyName("discord")]
		public string DiscordName { get; set; }
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("psuemail")]
        public string PSUEmail { get; set; }
        [JsonPropertyName("reason")]
        public string Reason { get; set; }
        [JsonPropertyName("discordname")]
        public string DiscordUsername { get; set; }
    }
}

