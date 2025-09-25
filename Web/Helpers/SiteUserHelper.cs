using System;
using System.Web;
using System.Web.Security;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Net;
using mojoPortal.Web.Extensions;
using Resources;

namespace mojoPortal.Web.Helpers;

public static class SiteUserHelper
{
	public static void TrackActivity()
	{
		var currentUser = GetCurrent();
		currentUser?.TrackUserActivity();
	}

	public static SiteUser GetCurrent()
	{
		bool bypassAuthCheck = false;

		var currentUser = GetCurrent(bypassAuthCheck);

		if ((currentUser is not null) && currentUser.IsLockedOut)
		{
			SiteUtils.RedirectToSignOut();
			return null;
		}

		return currentUser;
	}

	public static SiteUser GetCurrent(bool bypassAuthCheck)
	{
		if (HttpContext.Current is null || HttpContext.Current.Response is null)
		{
			return null;
		}

		if (bypassAuthCheck || HttpContext.Current.Request.IsAuthenticated)
		{
			if (HttpContext.Current.Items["CurrentUser"] is not null)
			{
				return (SiteUser)HttpContext.Current.Items["CurrentUser"];
			}

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				var siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
				if (siteUser.UserId > -1)
				{
					HttpContext.Current.Items["CurrentUser"] = siteUser;
					return siteUser;
				}
			}
		}

		return null;
	}

	public static bool UserIsSiteEditor()
	{
		if (WebUser.IsAdmin) { return true; }

		if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			return WebConfigSettings.UseRelatedSiteMode && WebUser.IsInRoles(siteSettings.SiteRootEditRoles);
		}

		return false;
	}

	public static bool UserCanEditModule(int moduleId)
	{
		if (HttpContext.Current is null || !HttpContext.Current.Request.IsAuthenticated)
		{
			return false;
		}

		if (WebUser.IsAdminOrContentAdmin)
		{
			return true;
		}


		if (CacheHelper.GetCurrentPage() is not PageSettings currentPage)
		{
			return false;
		}

		bool moduleFoundOnPage = false;

		foreach (Module m in currentPage.Modules)
		{
			if (m.ModuleId == moduleId)
			{
				moduleFoundOnPage = true;
			}
		}

		if (!moduleFoundOnPage)
		{
			return false;
		}

		if (WebUser.IsInRoles(currentPage.EditRoles))
		{
			return true;
		}

		if (GetCurrent() is not SiteUser currentUser)
		{
			return false;
		}

		foreach (Module m in currentPage.Modules)
		{
			if (m.ModuleId == moduleId)
			{
				if (m.EditUserId == currentUser.UserId)
				{
					return true;
				}

				if (WebUser.IsInRoles(m.AuthorizedEditRoles))
				{
					return true;
				}
			}
		}

		return false;
	}

	public static SiteUser CreateMinimalUser(SiteSettings siteSettings, string email, bool includeInMemberList, string adminComments)
	{
		if (siteSettings is null)
		{
			throw new ArgumentException("a valid siteSettings object is required for this method");
		}
		if (string.IsNullOrEmpty(email))
		{
			throw new ArgumentException("a valid email address is required for this method");
		}

		if (!Email.IsValidEmailAddressSyntax(email))
		{
			throw new ArgumentException("a valid email address is required for this method");
		}

		//first make sure he doesn't exist
		SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);
		if (siteUser is not null && siteUser.UserGuid != Guid.Empty)
		{
			return siteUser;
		}

		string login = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, email);

		siteUser = new SiteUser(siteSettings)
		{
			Email = email,
			LoginName = login,
			Name = login,
			Password = SiteUser.CreateRandomPassword(siteSettings.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars),
			ProfileApproved = true,
			DisplayInMemberList = includeInMemberList,
			PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion,
			PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer,
			Comment = adminComments
		};

		if (Membership.Provider is mojoMembershipProvider m)
		{
			siteUser.Password = m.EncodePassword(siteSettings, siteUser, siteUser.Password);
		}

		siteUser.Save();

		Role.AddUserToDefaultRoles(siteUser);

		return siteUser;
	}
}