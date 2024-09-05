using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.AdminUI;

public partial class Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// this is just a redirect for /Admin/
		//using GetRelativeNavigationSiteRoot so this will work in folder child sites
		//GetNavigationSiteRoot will use CacheHelper.GetCurrentSiteSettings but if user has already been to the main site, they can sometimes get the wrong site settings AHHHH!!!!
		//string siteRoot = SiteUtils.GetRelativeNavigationSiteRoot();
		string redirectUrl = "Admin/AdminMenu.aspx".ToLinkBuilder().ToString();

		if (Request.IsAuthenticated)
		{
			WebUtils.SetupRedirect(this, redirectUrl);
			return;
		}
		else
		{
			SiteUtils.RedirectToLoginPage(this, redirectUrl);
			return;
		}
	}
}
