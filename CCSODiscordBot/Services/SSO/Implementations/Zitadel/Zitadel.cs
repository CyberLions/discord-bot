using System.Net.Mail;
using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.SSO.Interfaces;
using Zitadel.Api;
using Zitadel.Management.V1;
using static Zitadel.Management.V1.ManagementService;

namespace CCSODiscordBot.Services.SSO.Implementations.Zitadel
{
	public class Zitadel : ISSOManagement
	{
        /// <summary>
        /// Auth service client
        /// </summary>
        private ManagementServiceClient _Client;
        private GRPCClient GRPCClient;
        /// <summary>
        /// Initialize Zitadel API with PAT
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="pat"></param>
		public Zitadel(ZitadelConfig conf)
		{
            _Client = Clients.ManagementService(new(conf.GetSetting("ApiUrl"), ITokenProvider.Static(conf.GetSetting("Pat"))));

            GRPCClient = new GRPCClient(conf.GetSetting("ApiUrl"), conf.GetSetting("Pat"), conf.GetSetting("ZitadelDiscordIDPId"));
        }

        /// <summary>
        /// The configuration type
        /// </summary>
        public ISSOConfig Configuration
        {
            get;
        } = new ZitadelConfig();

        /// <summary>
        /// Add a user to Zitadel
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ExistingUserException"></exception>
        public void AddUser(User user)
        {
            if(user.Email == null)
            {
                throw new NullReferenceException("User does not have an email");
            }

            MailAddress addr = new MailAddress(user.Email);

            // check for existing username:
            if (UserExists(user))
            {
                throw new ExistingUserException("User already exists in Zitadel. Aborting.");
            }

            // Add user:
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
                },
                UserName = addr.User
            });

            // Link the Discord user for SSO:
            GRPCClient.LinkUserIDP(result.UserId, user);
        }

        public void AddUserGroup(User user, string group, string project)
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(User user)
        {
            _Client.RemoveUser(new RemoveUserRequest
            {
                Id = GetUserID(user)
            });
        }

        public void UpdateUserRecord(User user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(User user)
        {
            if (user.Email == null)
            {
                throw new NullReferenceException("User does not have an email");
            }

            MailAddress addr = new MailAddress(user.Email);

            // check for existing username:
            var checkUsername = _Client.IsUserUnique(new IsUserUniqueRequest
            {
                UserName = addr.User,
                Email = user.Email
            });

            return !checkUsername.IsUnique;
        }

        private string GetUserID(User user)
        {
            if (user.Email == null)
            {
                throw new NullReferenceException("User does not have an email");
            }

            MailAddress addr = new MailAddress(user.Email);

            var zitadelUser = _Client.GetUserByLoginNameGlobal(new GetUserByLoginNameGlobalRequest
            {
                LoginName = addr.User
            });

            if(zitadelUser == null)
            {
                throw new NullReferenceException("User does not exist.");
            }

            return zitadelUser.User.Id;
        }
    }
}

