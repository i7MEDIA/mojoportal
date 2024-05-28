using System;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class ClosedPage : NonCmsBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{

		if (!siteSettings.SiteIsClosed)
		{
			SiteUtils.RedirectToSiteRoot();
			return;
		}

		AddClassToBody("closedpage");
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteClosedPageTitle);
		litSiteClosedMessage.Text = siteSettings.SiteIsClosedMessage;
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);

		if (Global.SkinConfig.MenuOptions.HideOnSiteClosed)
		{
			SuppressAllMenus();
		}
	}
	#endregion
}