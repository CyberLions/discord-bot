using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.SSO.Interfaces;
using Zitadel.Api;
using Zitadel.Management.V1;
using static Zitadel.Management.V1.ManagementService;

namespace CCSODiscordBot.Services.SSO.Implementations.Zitadel
{
	public class Zitadel : SSOManagement
	{
        /// <summary>
        /// Auth service client
        /// </summary>
        private ManagementServiceClient _Client;
        private GRPCClient GRPCClient;
        private string _idpID;
        /// <summary>
        /// Initialize Zitadel API with PAT
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="pat"></param>
		public Zitadel(string apiUrl, string pat, string idpId, string zitadelDiscordIDPId)
		{
            _Client = Clients.ManagementService(new(apiUrl, ITokenProvider.Static(pat)));
            _idpID = idpId;

            GRPCClient = new GRPCClient(apiUrl, pat, zitadelDiscordIDPId);
        }

        public void AddUser(User user)
        {
            var result = _Client.AddHumanUser(new AddHumanUserRequest
            {
                Profile = new AddHumanUserRequest.Types.Profile
                {
                    DisplayName = user.FirstName + " " + user.LastName,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Email = new AddHumanUserRequest.Types.Email
                {
                    Email_ = user.Email,
                    IsEmailVerified = user.Verified
                }
            });

            GRPCClient.LinkUserIDP(result.UserId, user);
        }

        public void AddUserGroup(User user, string group, string project)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserRecord(User user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(User user)
        {
            throw new NotImplementedException();
        }
    }
}

