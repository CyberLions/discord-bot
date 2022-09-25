using System;
using System.Text.Json.Serialization;

namespace CCSODiscordBot.Services.VPNAPI
{
	public class RequestData
	{
		public RequestData(string discord, string first, string last, string psuemail)
		{
			this.DiscordName = discord;
			this.FirstName = first;
			this.LastName = last;
			this.PSUEmail = psuemail;
		}
		[JsonPropertyName("discord")]
		public string DiscordName { get; set; }
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("psuemail")]
        public string PSUEmail { get; set; }
    }
}

