using Grpc.Net.Client;
using Zitadel.User.V2Beta;
using Grpc.Core;

namespace CCSODiscordBot.Services.SSO.Implementations.Zitadel
{
	public class GRPCClient
	{
		private readonly string Token;
		private readonly UserService.UserServiceClient _Client;
		private readonly string ZitadelDiscordIDPId;

        public GRPCClient(string apiUrl, string token, string zitadelDiscordIDPId)
		{
            var channel = GrpcChannel.ForAddress(apiUrl);
			_Client = new UserService.UserServiceClient(channel);
			ZitadelDiscordIDPId = zitadelDiscordIDPId;
			Token = token;
        }

        public void LinkUserIDP(string zitadelUserId, Database.DataTables.User user)
		{
			AddIDPLinkResponse response = _Client.AddIDPLink(new AddIDPLinkRequest()
			{
				IdpLink = new IDPLink()
				{
					IdpId = ZitadelDiscordIDPId,
					UserId = user.DiscordID.ToString(),
					UserName = user.FirstName + " " + user.LastName
                },
				UserId = zitadelUserId
            },
            new Metadata {{ "Authorization", $"Bearer {Token}" }}
			);

			Console.WriteLine("Zitadel IDP link created:\n" + response);
        }
		
	}
}

