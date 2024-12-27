using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI.Pages;

public partial class Logoff : Page
{
	const string WindowsLiveSecurityAlgorithm = "wsignin1.0";

	protected void Page_Load(object sender, EventArgs e)
	{
		DoLogout();
	}

	private void DoLogout()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		string winliveCookieName = $"winliveid{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";

		string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

		var roleCookie = new HttpCookie(roleCookieName, string.Empty)
		{
			Expires = DateTime.Now.AddMinutes(1),
			Path = "/"
		};
		Response.Cookies.Add(roleCookie);

		var displayNameCookie = new HttpCookie("DisplayName", string.Empty)
		{
			Expires = DateTime.Now.AddMinutes(1),
			Path = "/"
		};
		Response.Cookies.Add(displayNameCookie);

		// apparently we need this here for folder sites using windows auth
		//https://www.mojoportal.com/Forums/EditPost.aspx?thread=13195&forumid=2&mid=34&pageid=5&pagenumber=1
		CookieHelper.ExpireCookie("siteguid" + siteSettings.SiteGuid);

		if (WebConfigSettings.UseFolderBasedMultiTenants && !WebConfigSettings.UseRelatedSiteMode)
		{
			string cookieName = "siteguid" + siteSettings.SiteGuid.ToString();

			var siteCookie = new HttpCookie(cookieName, string.Empty)
			{
				Expires = DateTime.Now.AddMinutes(1),
				Path = "/"
			};

			Response.Cookies.Add(siteCookie);
			CookieHelper.ExpireCookie("siteguid" + siteSettings.SiteGuid);
		}
		else
		{
			FormsAuthentication.SignOut();
		}

		string winLiveToken = CookieHelper.GetCookieValue(winliveCookieName);
		WindowsLiveLogin.User liveUser = null;
		if (winLiveToken.Length > 0)
		{
			var windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();

			try
			{
				liveUser = windowsLive.ProcessToken(winLiveToken);
				if (liveUser != null)
				{
					Response.Redirect(windowsLive.GetLogoutUrl());
					Response.End();
				}
			}
			catch (InvalidOperationException)
			{
			}
		}

		try
		{
			if (Session != null)
			{
				Session.Clear();
				Session.Abandon();
			}
		}
		catch (HttpException) { }

		string redirectUrl = "~/Default.aspx".ToLinkBuilder().ToString();

		if (!siteSettings.UseSslOnAllPages)
		{
			if (redirectUrl.StartsWith("https:"))
			{
				redirectUrl = redirectUrl.Replace("https:", "http:");
			}
		}

		WebUtils.SetupRedirect(this, redirectUrl);
	}
}