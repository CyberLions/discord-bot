using System.Net.Mail;
using CCSODiscordBot.Services.Database.DataTables;
using CCSODiscordBot.Services.SSO.Interfaces;
using Google.Api;
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
        private readonly ManagementServiceClient _Client;
        private readonly GRPCClient GRPCClient;
        /// <summary>
        /// Initialize Zitadel API with PAT
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="pat"></param>
		public Zitadel(SSOConfig conf)
		{
            _Client = Clients.ManagementService(new(conf.GetSetting("ApiUrl"), ITokenProvider.Static(conf.GetSetting("Pat"))));

            GRPCClient = new GRPCClient(conf.GetSetting("ApiUrl"), conf.GetSetting("Pat"), conf.GetSetting("ZitadelDiscordIDPId"));
        }

        /// <summary>
        /// Used for discovery of configuration settings
        /// </summary>
        public Zitadel()
        {

        }

        /// <summary>
        /// The configuration type
        /// </summary>
        public SSOConfig Configuration
        {
            get;
        } = new ZitadelConfig();

        /// <summary>
        /// Add a user to Zitadel
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The users UUID</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ExistingUserException"></exception>
        public string AddUser(User user)
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

            // Set user metadata:
            try
            {
                AddUserMetadata(user);
            }
            catch(Grpc.Core.RpcException e)
            {
                Console.WriteLine("Failed metadata link: "+e);
            }

            return result.UserId;
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

        public string UpdateUserRecord(User user)
        {
            // Resync IDP:
            string uid = GetUserID(user);
            try
            {
                GRPCClient.LinkUserIDP(uid, user);
            }
            catch (Grpc.Core.RpcException e)
            when (e.StatusCode.Equals(Grpc.Core.StatusCode.AlreadyExists))
            {
                // Ignore. User already up to date.
            }

            // Set user metadata:
            try
            {
                AddUserMetadata(user);
            }
            catch (Grpc.Core.RpcException e)
            {
                Console.WriteLine("Failed metadata link: " + e);
            }

            return uid;
        }

        public bool UserExists(User user)
        {
            // Check ID from SSO in DB:
            if (UUIDExists(user))
            {
                return true;
            }

            // Check by username and email:
            if (user.Email == null)
            {
                throw new NullReferenceException("User does not have an email");
            }

            MailAddress addr = new MailAddress(user.Email);

            // check for existing email:
            var checkEmail = _Client.IsUserUnique(new IsUserUniqueRequest
            {
                Email = user.Email
            });

            // check for existing username:
            var checkUsername = _Client.IsUserUnique(new IsUserUniqueRequest
            {
                UserName = addr.User
            });

            return !checkUsername.IsUnique || !checkEmail.IsUnique;
        }

        public void RemoveUserGroup(User user, string group, string project)
        {
            throw new NotImplementedException();
        }

        private string GetUserID(User user)
        {
            if (UUIDExists(user) && user.SSOID!=null)
            {
                return user.SSOID;
            }

            if (user.Email == null)
            {
                throw new NullReferenceException("User does not have an email");
            }

            MailAddress addr = new MailAddress(user.Email);

            var zitadelUser = _Client.GetUserByLoginNameGlobal(new GetUserByLoginNameGlobalRequest
            {
                LoginName = addr.User
            });

            if (zitadelUser == null)
            {
                throw new NullReferenceException("User does not exist.");
            }

            return zitadelUser.User.Id;
        }

        private bool UUIDExists(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.SSOID))
            {
                try
                {
                    _Client.GetUserByID(new GetUserByIDRequest
                    {
                        Id = user.SSOID
                    });
                    return true;
                }
                catch (Grpc.Core.RpcException e) when (e.StatusCode.Equals(Grpc.Core.StatusCode.NotFound))
                {
                    Console.WriteLine("Failed to get user with stored UID");
                }
            }
            return false;
        }

        /// <summary>
        /// Add metadata to a users account
        /// </summary>
        /// <param name="user"></param>
        private void AddUserMetadata(User user)
        {
            _Client.SetUserMetadata(new SetUserMetadataRequest
            {
                Id = user.SSOID,
                Key = "DiscordUID",
                Value = Google.Protobuf.ByteString.CopyFromUtf8(user.DiscordID.ToString())
            });
            _Client.SetUserMetadata(new SetUserMetadataRequest
            {
                Id = user.SSOID,
                Key = "BotRegisteredEmail",
                Value = Google.Protobuf.ByteString.CopyFromUtf8(user.Email)
            });
        }
    }
}

