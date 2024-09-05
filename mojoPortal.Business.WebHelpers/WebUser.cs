using System;
using System.Collections;
using System.Web;
using mojoPortal.Core.Configuration;

namespace mojoPortal.Business.WebHelpers;

public static class WebUser
{
	public static bool IsInRole(string role)
	{
		if (HttpContext.Current is null || HttpContext.Current.User is null || string.IsNullOrWhiteSpace(role))
		{
			return false;
		}

		if (role.Contains("All Users"))
		{
			return true;
		}

		if (!isAuthenticated())
		{
			return false;
		}

		if (HttpContext.Current.User.IsInRole("Admins"))
		{
			return true;
		}

		return HttpContext.Current.User.IsInRole(role);
	}


	public static bool IsInRoles(string roles)
	{
		if (IsInRole("Admins"))
		{
			return true;
		}

		if (string.IsNullOrWhiteSpace(roles))
		{
			return false;
		}

		if (roles.Contains("All Users;"))
		{
			return true;
		}

		if (!isAuthenticated())
		{
			return false;
		}

		foreach (string role in roles.Split([';']))
		{
			if (role.IndexOf("All Users") > -1)
			{
				return true;
			}

			if (IsInRole(role))
			{
				return true;
			}
		}
		return false;
	}


	public static bool IsInRoles(IList roles)
	{
		if (IsInRole("Admins"))
		{
			return true;
		}

		if (roles is null)
		{
			return false;
		}

		if (roles.Contains("All Users"))
		{
			return true;
		}

		if (!isAuthenticated())
		{
			return false;
		}

		foreach (string role in roles)
		{
			if (role.Contains("All Users"))
			{
				return true;
			}

			if (IsInRole(role))
			{
				return true;
			}
		}
		return false;
	}


	public static bool IsAdmin
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Admins");
		}
	}


	public static bool IsContentAdmin
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Content Administrators");
		}
	}


	public static bool IsContentPublisher
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Content Publishers");
		}
	}


	public static bool IsContentAuthor
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Content Authors");
		}
	}


	public static bool IsRoleAdmin
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Role Admins");
		}
	}


	public static bool IsNewsletterAdmin
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			return IsInRole("Newsletter Administrators");
		}
	}


	public static bool IsSkinManager
	{
		get
		{
			if (!isAuthenticated())
			{
				return false;
			}

			var siteSettings = (SiteSettings)HttpContext.Current?.Items["SiteSettings"];

			if (siteSettings is null)
			{
				return false;
			}

			return IsInRoles(siteSettings.RolesThatCanManageSkins);
		}
	}


	public static bool IsAdminOrContentAdmin => IsAdmin || IsContentAdmin;

	public static bool IsAdminOrContentAdminOrContentAuthor => IsAdmin || IsContentAdmin || IsContentAuthor;

	public static bool IsAdminOrContentAdminOrContentPublisher => IsAdmin || IsContentAdmin || IsContentPublisher;

	public static bool IsAdminOrContentAdminOrContentPublisherOrContentAuthor => IsAdmin || IsContentAdmin || IsContentPublisher || IsContentAuthor;

	public static bool IsAdminOrContentAdminOrRoleAdmin => IsAdmin || IsContentAdmin || IsRoleAdmin;

	public static bool IsAdminOrRoleAdmin => IsAdmin || IsRoleAdmin;

	public static bool IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin => IsAdmin || IsContentAdmin || IsRoleAdmin || IsNewsletterAdmin;

	public static bool IsAdminOrNewsletterAdmin => IsAdmin || IsNewsletterAdmin;

	public static bool HasEditPermissions(int siteId, int moduleId, int pageId)
	{
		if (HttpContext.Current is null || HttpContext.Current.User is null)
		{
			return false;
		}

		if (!HttpContext.Current.Request.IsAuthenticated)
		{
			return false;
		}

		if (IsAdmin || IsContentAdmin)
		{
			return true;
		}

		var module = new Module(moduleId, pageId);
		var pageSettings = new PageSettings(siteId, module.PageId);

		if (pageSettings is null)
		{
			return false;
		}

		if (pageSettings.PageId < 0)
		{
			return false;
		}

		if (IsInRoles(pageSettings.EditRoles) || IsInRoles(module.AuthorizedEditRoles))
		{
			return true;
		}

		if (module.EditUserId > 0)
		{
			var siteSettings = new SiteSettings(siteId);
			var siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
			if (module.EditUserId == siteUser.UserId)
			{
				return true;
			}
		}

		return false;
	}


	public static bool IsSiteEditor()
	{
		if (IsAdmin)
		{
			return true;
		}

		var siteSettings = (SiteSettings)HttpContext.Current?.Items["SiteSettings"];
		if (siteSettings is not null)
		{
			return AppConfig.RelatedSiteModeEnabled && IsInRoles(siteSettings.SiteRootEditRoles);
		}

		return false;
	}


	public static bool CanUpload()
	{
		var siteSettings = (SiteSettings)HttpContext.Current?.Items["SiteSettings"];

		return IsAdminOrContentAdmin
			|| IsSiteEditor()
			|| IsInRoles(siteSettings.GeneralBrowseAndUploadRoles)
			|| IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)
			|| IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor);
	}


	private static bool isAuthenticated()
	{
		return HttpContext.Current?.Request.IsAuthenticated ?? false;
	}

}
