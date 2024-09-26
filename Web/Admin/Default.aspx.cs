using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.AdminUI;

public partial class Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// this is just a redirect for /Admin/
		 
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
