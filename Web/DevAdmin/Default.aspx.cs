﻿using System;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class DevToolsPage : NonCmsBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebUser.IsAdmin
			|| !siteSettings.IsServerAdminSite
			|| !WebConfigSettings.EnableDeveloperMenuInAdminMenu)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		PopulateLabels();
	}

	private void PopulateLabels()
	{
		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = $"{SiteRoot}/Admin/AdvancedTools.aspx";

		lnkThisPage.Text = DevTools.DevToolsHeading;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/DevAdmin/Default.aspx";

		Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.DevToolsHeading);
		heading.Text = DevTools.DevToolsHeading;

		lnkServerVariables.Text = Resource.ServerVariablesLink;
		lnkServerVariables.NavigateUrl = $"{SiteRoot}/DevAdmin/ServerVariables.aspx";

		lnkQueryTool.Text = DevTools.QueryToolLink;
		lnkQueryTool.NavigateUrl = $"{SiteRoot}/DevAdmin/QueryTool.aspx";
	}

	private void LoadSettings()
	{
		liQueryTool.Visible = WebConfigSettings.EnableQueryTool;
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);

		SuppressPageMenu();
	}
	#endregion
}