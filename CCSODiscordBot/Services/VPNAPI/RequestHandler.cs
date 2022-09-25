using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text.Json;
using CCSODiscordBot.Services.Database.DataTables;

namespace CCSODiscordBot.Services.VPNAPI
{
	public class RequestHandler
	{
		public static async Task<bool> MakeVPNRequest(Guild guild, User user, string username, string reason)
		{
			// Check for null values:
			if (guild.VPNAPIKey == null || guild.VPNAPIURL == null || user.FirstName == null || user.LastName == null || user.Email == null || !user.Verified)
			{
				return false;
			}

			RequestData data = new RequestData(user.DiscordID.ToString(), user.FirstName, user.LastName, user.Email, reason, username);
			using (HttpClient client = new HttpClient())
			{

				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("St2-Api-Key", guild.VPNAPIKey);
				HttpContent content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
				HttpResponseMessage result = await client.PostAsync(new Uri(guild.VPNAPIURL), content);

				if (result.IsSuccessStatusCode)
				{
					Console.WriteLine("VPN Request success: " + user.DiscordID);
                    return true;
				}
				Console.WriteLine("VPN Request failed: " + user.DiscordID + " " + result.StatusCode + " " + result.ReasonPhrase);
				return false;
			}
        }
	}
}

