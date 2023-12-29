using System;
using System.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class QueryToolPage : Page
{
	protected string SiteRoot = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		SiteRoot = SiteUtils.GetNavigationSiteRoot();

		Title = DevTools.QueryToolLink;

		lnkHome.Text = Resource.HomePageLink;
		lnkHome.NavigateUrl = SiteRoot;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = $"{SiteRoot}/Admin/AdvancedTools.aspx";

		lnkDevTools.Text = DevTools.DevToolsHeading;
		lnkDevTools.NavigateUrl = $"{SiteRoot}/DevAdmin/Default.aspx";

		lnkThisPage.Text = DevTools.QueryToolLink;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/DevAdmin/QueryTool.aspx";
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}
}