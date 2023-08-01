using System;
using CCSODiscordBot.Services.Database.DataTables;

namespace CCSODiscordBot.Services.SSO.Interfaces
{
	public interface SSOManagement
	{
		/// <summary>
		/// Adds a user to the SSO platform
		/// </summary>
		/// <exception cref="ExistingUserException">Thrown when a user already exists</exception>
		public void AddUser(User user);
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
		public void UpdateUserRecord(User user);
		/// <summary>
		/// Add user to a group
		/// </summary>
		/// <param name="user">User info</param>
		/// <param name="group">Targeted Group</param>
		public void AddUserGroup(User user, string group, string project);
	}
}

