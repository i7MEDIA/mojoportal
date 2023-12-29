using System;
using System.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class ServerVariablesPage : Page
{
	protected string SiteRoot = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		SiteRoot = SiteUtils.GetNavigationSiteRoot();
		Title = Resource.ServerVariablesHeading;
		litHeading.Text = Resource.ServerVariablesHeading;

		lnkHome.Text = Resource.HomePageLink;
		lnkHome.NavigateUrl = SiteRoot;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = $"{SiteRoot}/Admin/AdvancedTools.aspx";

		lnkDevTools.Text = DevTools.DevToolsHeading;
		lnkDevTools.NavigateUrl = $"{SiteRoot}/DevAdmin/Default.aspx";

		lnkThisPage.Text = DevTools.ServerVariablesLink;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/DevAdmin/ServerVariables.aspx";
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(this.Page_Load);
	}
}