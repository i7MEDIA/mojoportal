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
		var winliveCookieName = $"winliveid{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";
		var roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

		Response.Cookies.Remove(roleCookieName);
		Response.Cookies.Remove("DisplayName");

		Response.Cookies.Add(new HttpCookie(roleCookieName, string.Empty)
		{
			Expires = DateTime.Now.AddDays(-30),
			Path = "/"
		}); // adding cookie with same name and expired date removes the cookie from the client

		Response.Cookies.Add(new HttpCookie("DisplayName", string.Empty)
		{
			Expires = DateTime.Now.AddDays(-30),
			Path = "/"
		}); // adding cookie with same name and expired date removes the cookie from the client

		// apparently we need this here for folder sites using windows auth
		// https://www.mojoportal.com/Forums/EditPost.aspx?thread=13195&forumid=2&mid=34&pageid=5&pagenumber=1
		CookieHelper.ExpireCookie("siteguid" + siteSettings.SiteGuid);

		if (WebConfigSettings.UseFolderBasedMultiTenants && !WebConfigSettings.UseRelatedSiteMode)
		{
			var siteCookieName = $"siteguid{siteSettings.SiteGuid}";

			Response.Cookies.Add(new HttpCookie(siteCookieName, string.Empty)
			{
				Expires = DateTime.Now.AddDays(-30),
				Path = "/"
			}); //adding cookie with same name and expired date removes the cookie from the client

			CookieHelper.ExpireCookie($"siteguid{siteSettings.SiteGuid}");
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