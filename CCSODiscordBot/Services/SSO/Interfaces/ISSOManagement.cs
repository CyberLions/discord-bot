using System;
using CCSODiscordBot.Services.Database.DataTables;

namespace CCSODiscordBot.Services.SSO.Interfaces
{
	public interface ISSOManagement
	{
		/// <summary>
		/// The configuration class
		/// </summary>
		public SSOConfig Configuration
		{
			get;
		}
        /// <summary>
        /// Adds a user to the SSO platform
        /// </summary>
		/// <returns>The users ID</returns>
        /// <exception cref="ExistingUserException">Thrown when a user already exists</exception>
        public string AddUser(User user);
		/// <summary>
		/// Removes a user from the SSO platform
		/// </summary>
		public void RemoveUser(User user);
		/// <summary>
		/// Check if a user exists on the SSO platform
		/// Check via the discord user and guild ID
		/// </summary>
		/// <returns>True if exists</returns>
		public bool UserExists(User user);
		/// <summary>
		/// Update a users record on the SSO platform
		/// </summary>
		/// <returns>The users UID</returns>
		public string UpdateUserRecord(User user);
		/// <summary>
		/// Add user to a group
		/// </summary>
		/// <param name="user">User info</param>
		/// <param name="group">Targeted Group</param>
		public void AddUserGroup(User user, string group, string project);
		/// <summary>
		/// Removes a group from a user
		/// </summary>
		/// <param name="user"></param>
		/// <param name="group"></param>
		/// <param name="project"></param>
        public void RemoveUserGroup(User user, string group, string project);
    }
}

