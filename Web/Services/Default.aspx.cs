using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Services;

public partial class Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		//prevent 404

		string redirectUrl = SiteUtils.GetNavigationSiteRoot();

		WebUtils.SetupRedirect(this, redirectUrl);
		return;
	}
}
