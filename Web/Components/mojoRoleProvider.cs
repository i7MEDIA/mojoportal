using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;


namespace mojoPortal.Web;

public class mojoRoleProvider : RoleProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(mojoRoleProvider));
	private string applicationName = "unknown";


	public mojoRoleProvider()
	{ }


	public override string ApplicationName
	{
		get
		{
			if (HttpContext.Current != null)
			{
				var siteSettings = CacheHelper.GetCurrentSiteSettings();

				if (siteSettings != null)
				{
					applicationName = siteSettings.SiteName;
				}
			}

			return applicationName;
		}
		set
		{
			applicationName = value;
		}
	}


	/// <summary>
	/// Get any needed parameters from config section
	/// </summary>
	/// <param name="name">name of the provider</param>
	/// <param name="config">configuration collection</param>
	public override void Initialize(string name, NameValueCollection config)
	{
		base.Initialize(name, config);
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="userNames">a list of usernames</param>
	/// <param name="roleNames">a list of roles</param>
	public override void AddUsersToRoles(string[] userNames, string[] roleNames)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null && userNames != null && roleNames != null)
		{
			foreach (var userName in userNames)
			{
				var siteUser = new SiteUser(siteSettings, userName);

				if (siteUser.UserId > -1)
				{
					foreach (var roleName in roleNames)
					{
						var role = new Role(siteSettings.SiteId, roleName);

						if (role.RoleId > -1)
						{
							Role.AddUser(
								role.RoleId,
								siteUser.UserId,
								role.RoleGuid,
								siteUser.UserGuid
							);
						}
					}
				}
			}
		}
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="roleName">a role name</param>
	public override void CreateRole(string roleName)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null && roleName != null && roleName.Length > 0)
		{
			if (!Role.Exists(siteSettings.SiteId, roleName))
			{
				var role = new Role
				{
					SiteId = siteSettings.SiteId,
					SiteGuid = siteSettings.SiteGuid,
					RoleName = roleName
				};

				role.Save();
			}
		}
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="roleName">a role</param>
	/// <param name="throwOnPopulatedRole">get upset of users are in a role</param>
	/// <returns></returns>
	public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var result = false;

		if (siteSettings != null && roleName != null && roleName.Length > 0)
		{
			var role = new Role(siteSettings.SiteId, roleName);

			if (role.RoleId > 0)
			{
				if (throwOnPopulatedRole && role.HasUsers())
				{
					throw new Exception("This role cannot be deleted because it has users.");
				}

				Role.DeleteRole(role.RoleId);

				result = true;
			}
		}

		return result;
	}


	/// <summary>
	/// required implemention
	/// </summary>
	/// <param name="roleName">a role</param>
	/// <param name="usernameToMatch">a username to look for in the role</param>
	/// <returns></returns>
	public override string[] FindUsersInRole(string roleName, string usernameToMatch)
	{
		//return service.FindUsersInRole(_RemoteProviderName, _ApplicationName, roleName, usernameToMatch);


		throw new Exception("The method or operation is not implemented.");
	}


	/// <summary>
	/// required implementation
	/// this should not be used to get data for a dropdown list
	/// because it doesn't have role id or display name
	/// </summary>
	/// <returns></returns>
	public override string[] GetAllRoles()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null)
		{
			var roleList = new string[Role.CountOfRoles(siteSettings.SiteId)];
			var i = 0;

			using IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId);

			while (reader.Read())
			{
				roleList[i] = reader["RoleName"].ToString();
				i += 1;

			}

			return roleList;
		}

		return [];
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="username">a username</param>
	/// <returns>a list of roles</returns>
	public override string[] GetRolesForUser(string userName)
	{
		if (HttpContext.Current != null)
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			var roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

			if (
				HttpContext.Current.Request.IsAuthenticated &&
				HttpContext.Current.User.Identity.Name == userName &&
				siteSettings != null
			)
			{
				if (
					CookieHelper.CookieExists(roleCookieName) &&
					!string.IsNullOrWhiteSpace(CookieHelper.GetCookieValue(roleCookieName))
				)
				{
					try
					{
						return GetRolesFromCookie();

						// the below errors are expected if the machine key has been changed and the user already has a role cookie
						// apparently the update for http://weblogs.asp.net/scottgu/archive/2010/09/28/asp-net-security-update-now-available.aspx
						// changed it from throwing a CryptographyException to an HttpException
					}
					catch (CryptographicException)
					{
						return GetRolesAndSetCookieInternal();
					}
					catch (HttpException)
					{
						return GetRolesAndSetCookieInternal();
					}
					catch (NullReferenceException ex)
					{
						// https://www.mojoportal.com/Forums/Thread.aspx?thread=9515&mid=34&pageid=5&ItemID=2&pagenumber=1#post39505
						// not sure what is null here but someone reported it happening using the Amazon silk browser
						// which does some very weird things like caching everything on their own servers 
						// so their servers make the web request and the brwoser gets it from their server
						// its like a strange proxy server
						// then it happened on my own site after applying a windows update
						log.Error("handled exception", ex);

						return GetRolesAndSetCookieInternal();
					}
				}
				else
				{
					return GetRolesAndSetCookieInternal();
				}
			}
			else
			{
				// not current user or not authenticated
				if (siteSettings != null && userName != null && userName.Length > 0)
				{
					return SiteUser.GetRoles(siteSettings, userName);
				}
			}
		}

		return [];
	}


	public static void ResetCurrentUserRolesCookie()
	{
		if (
			HttpContext.Current == null ||
			HttpContext.Current.Request == null ||
			!HttpContext.Current.Request.IsAuthenticated
		)
		{
			return;
		}

		GetRolesAndSetCookieInternal();
	}


	private static string[] GetRolesAndSetCookieInternal()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var currentUserRoles = new string[0];

		if (siteSettings != null)
		{
			var roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);
			var roleStr = "";

			currentUserRoles = SiteUser.GetRoles(siteSettings, HttpContext.Current.User.Identity.Name);

			foreach (var role in currentUserRoles)
			{
				roleStr += role;
				roleStr += ";";
			}

			if (WebConfigSettings.PreEncryptRolesForCookie)
			{
				roleStr = SiteUtils.Encrypt(roleStr);
			}

			var ticket = new FormsAuthenticationTicket(
				1,                                      // version
				HttpContext.Current.User.Identity.Name, // user name
				DateTime.Now,                           // issue time
				DateTime.Now.AddHours(1),               // expires every hour
				false,                                  // don't persist cookie
				roleStr                                 // roles
			);

			var cookieStr = FormsAuthentication.Encrypt(ticket);
			var roleCookie = new HttpCookie(roleCookieName, cookieStr)
			{
				//Expires = DateTime.Now.AddMinutes(20),
				HttpOnly = true,
				Path = "/"
			};

			if (SiteUtils.SslIsAvailable() && WebConfigSettings.RequireSslForRoleCookie)
			{
				roleCookie.Secure = true;
			}

			HttpContext.Current.Response.Cookies.Add(roleCookie);
		}

		return currentUserRoles;
	}


	private string[] GetRolesFromCookie()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var currentUserRoles = new string[0];

		if (siteSettings != null)
		{
			var roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);
			var userRoles = new ArrayList();
			var roleCookie = HttpContext.Current.Request.Cookies[roleCookieName];

			if (roleCookie != null)
			{
				var ticket = FormsAuthentication.Decrypt(roleCookie.Value);

				if (null == ticket || ticket.Expired)
				{
					return GetRolesAndSetCookieInternal();
				}

				var roles = ticket.UserData;

				if (WebConfigSettings.PreEncryptRolesForCookie)
				{
					try
					{
						roles = SiteUtils.Decrypt(roles);
					}
					catch (CryptographicException)
					{ }
					catch (FormatException)
					{ }
				}

				foreach (var role in roles.Split(';'))
				{
					userRoles.Add(role);
				}
			}

			currentUserRoles = (string[])userRoles.ToArray(typeof(string));
		}

		return currentUserRoles;
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="roleName">a role</param>
	/// <returns>a list of users</returns>
	public override string[] GetUsersInRole(string roleName)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null)
		{
			var role = new Role(siteSettings.SiteId, roleName);

			if (role.RoleId > -1)
			{
				var userList = new string[role.CountOfUsers()];
				var i = 0;

				using IDataReader reader = Role.GetRoleMembers(role.RoleId);

				while (reader.Read())
				{
					if (siteSettings.UseEmailForLogin)
					{
						userList[i] = reader["Email"].ToString();
					}
					else
					{
						userList[i] = reader["LoginName"].ToString();
					}

					i += 1;
				}

				return userList;
			}
		}

		return [];
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="userName">a username</param>
	/// <param name="roleName">a role</param>
	/// <returns>true or false</returns>
	public override bool IsUserInRole(string userName, string roleName)
	{
		var userRoles = GetRolesForUser(userName);
		var result = false;

		foreach (var role in userRoles)
		{
			if (role == roleName)
			{
				result = true;
			}
		}

		return result;
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="userNames">a list of usernames</param>
	/// <param name="roleNames">a list of roles</param>
	public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null && userNames != null && roleNames != null)
		{
			foreach (string userName in userNames)
			{
				var siteUser = new SiteUser(siteSettings, userName);

				if (siteUser.UserId > 0)
				{
					foreach (var roleName in roleNames)
					{
						var role = new Role(siteSettings.SiteId, roleName);

						if (role.RoleId > 0)
						{
							Role.RemoveUser(role.RoleId, siteUser.UserId);
						}
					}
				}
			}
		}
	}


	/// <summary>
	/// required implementation
	/// </summary>
	/// <param name="roleName">a role</param>
	/// <returns>true or false</returns>
	public override bool RoleExists(string roleName)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var result = false;

		if (siteSettings != null && roleName != null && roleName.Length > 0)
		{
			result = Role.Exists(siteSettings.SiteId, roleName);
		}

		return result;
	}


	//private string[] GetRolesAndSetCookie()
	//{
	//	return GetRolesAndSetCookieInternal();

	//	var siteSettings = CacheHelper.GetCurrentSiteSettings();
	//	var currentUserRoles = new string[0];
	//	var hostName = WebUtils.GetHostName();

	//	if (siteSettings != null)
	//	{
	//		var roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);
	//		var roleStr = "";

	//		currentUserRoles = SiteUser.GetRoles(siteSettings, HttpContext.Current.User.Identity.Name);

	//		foreach (string role in currentUserRoles)
	//		{
	//			roleStr += role;
	//			roleStr += ";";
	//		}

	//		var ticket = new FormsAuthenticationTicket(
	//			1,                                      // version
	//			HttpContext.Current.User.Identity.Name, // user name
	//			DateTime.Now,                           // issue time
	//			DateTime.Now.AddHours(1),               // expires every hour
	//			false,                                  // don't persist cookie
	//			roleStr                                 // roles
	//		);

	//		var cookieStr = FormsAuthentication.Encrypt(ticket);
	//		var roleCookie = new HttpCookie(roleCookieName, cookieStr)
	//		{
	//			//roleCookie.Expires = DateTime.Now.AddMinutes(20);
	//			HttpOnly = true,
	//			Path = "/"
	//		};

	//		HttpContext.Current.Response.Cookies.Add(roleCookie);
	//	}

	//	return currentUserRoles;
	//}


	//public override string[] GetRolesForUser(string userName)
	//{
	//	if (HttpContext.Current != null)
	//	{
	//		var siteSettings = CacheHelper.GetCurrentSiteSettings();

	//		if (
	//			HttpContext.Current.Request.IsAuthenticated &&
	//			HttpContext.Current.User.Identity.Name == userName
	//		)
	//		{
	//			string[] currentUserRoles;

	//			try
	//			{
	//				if (log.IsDebugEnabled)
	//				{
	//					log.Debug("Decrypting ticket");
	//				}

	//				var ticket = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies["portalroles"].Value);
	//				var userRoles = new ArrayList();

	//				foreach (var role in ticket.UserData.Split(';'))
	//				{
	//					userRoles.Add(role);
	//				}

	//				currentUserRoles = (string[])userRoles.ToArray(typeof(string));

	//				return currentUserRoles;
	//			}
	//			catch (Exception ex)
	//			{
	//				if (log.IsDebugEnabled)
	//				{
	//					log.Debug($"Caught {ex}, so creating a new cookie");
	//				}

	//				// Exception occurs if cookie does not exist, or was issued to a different user (eg logging out
	//				// and logging back in as another user).
	//				siteSettings = CacheHelper.GetCurrentSiteSettings();
	//				currentUserRoles = SiteUser.GetRoles(siteSettings, HttpContext.Current.User.Identity.Name);
	//				var roleStr = "";

	//				foreach (var role in currentUserRoles)
	//				{
	//					roleStr += role;
	//					roleStr += ";";
	//				}

	//				var ticket = new FormsAuthenticationTicket(
	//					1,                                      // version
	//					HttpContext.Current.User.Identity.Name, // user name
	//					DateTime.Now,                           // issue time
	//					DateTime.Now.AddHours(1),               // expires every hour
	//					false,                                  // don't persist cookie
	//					roleStr                                 // roles
	//				);

	//				var cookieStr = FormsAuthentication.Encrypt(ticket);
	//				var roleCookie = new HttpCookie("portalroles", cookieStr)
	//				{
	//					Expires = DateTime.Now.AddMinutes(20),
	//					Path = "/"
	//				};

	//				HttpContext.Current.Response.Cookies.Add(roleCookie);

	//				return currentUserRoles;
	//			}
	//		}
	//		else
	//		{
	//			// not current user or not authenticated
	//			if (siteSettings != null && userName != null && userName.Length > 0)
	//			{
	//				return SiteUser.GetRoles(siteSettings, userName);
	//			}
	//		}
	//	}

	//	return [];
	//}
}
