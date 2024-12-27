using System;
using System.Web.UI;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web;

/// <summary>
/// primary purpose of this page is a few things where we need to set a cookie and redirect
/// for example to allow a mobile phone user to choose if they want to view the site using the main skin instead of the mobile skin
/// 
/// </summary>
public partial class RedirectPage : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		ProcessSkinCookie();
		Response.Redirect("~/Default.aspx".ToLinkBuilder().ToString()); // the default.aspx is needed to work with folder based child sites
	}

	private void ProcessSkinCookie()
	{
		//toggle
		if (SiteUtils.IsMobileDevice())
		{
			if (CookieHelper.CookieExists(SiteUtils.MobileUseFullViewCookieName))
			{
				CookieHelper.ExpireCookie(SiteUtils.MobileUseFullViewCookieName);
			}
			else
			{
				CookieHelper.SetCookie(SiteUtils.MobileUseFullViewCookieName, "y");
			}
		}
		else
		{
			if (CookieHelper.CookieExists(SiteUtils.NonMobileUseMobileViewCookieName))
			{
				CookieHelper.ExpireCookie(SiteUtils.NonMobileUseMobileViewCookieName);
			}
			else
			{
				CookieHelper.SetCookie(SiteUtils.NonMobileUseMobileViewCookieName, "y");
			}
		}
	}
}