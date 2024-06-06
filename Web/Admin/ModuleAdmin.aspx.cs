using System;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class ModuleAdminPage : NonCmsBasePage
{
	private bool IsAdmin = false;
	protected bool isServerAdminSite = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}
		LoadSettings();

		if (!IsAdmin)
		{
			WebUtils.SetupRedirect(this, "AccessDenied.aspx".ToLinkBuilder().ToString());
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}


	private void PopulateControls()
	{
		if (Page.IsPostBack) return;

		using var reader = ModuleDefinition.GetModuleDefinitions(siteSettings.SiteGuid);
		featuresList.DataSource = reader;
		featuresList.DataBind();
	}



	//private void DefsList_ItemCommand(object sender, DataListCommandEventArgs e)
	//{
	//	// TODO: make this a link instead of postback then redirect? JA

	//	int moduleDefID = (int)featuresList.DataKeys[e.Item.ItemIndex];
	//	string redirectUrl = "Admin/ModuleDefinitions.aspx".ToLinkBuilder().AddParam("defid", moduleDefID).ToString();
	//	WebUtils.SetupRedirect(this, redirectUrl);
	//}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);
		heading.Text = Resource.AdminMenuFeatureModulesLink;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = "/Admin/AdminMenu.aspx".ToLinkBuilder().ToString();

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = "/Admin/AdvancedTools.aspx".ToLinkBuilder().ToString();

		lnkFeatureAdmin.Text = Resource.AdminMenuFeatureModulesLink;
		lnkFeatureAdmin.ToolTip = Resource.AdminMenuFeatureModulesLink;
		lnkFeatureAdmin.NavigateUrl = "/Admin/ModuleAdmin.aspx".ToLinkBuilder().ToString();

		lnkNewModule.Text = Resource.ModuleDefsAddButton;
		lnkNewModule.ToolTip = Resource.ModuleDefsAddButton;

	}

	private void LoadSettings()
	{
		IsAdmin = WebUser.IsAdmin;
		//EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
		isServerAdminSite = siteSettings.IsServerAdminSite;

		lnkNewModule.Visible = isServerAdminSite;

		AddClassToBody("administration");
		AddClassToBody("moduleadmin");

	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		//this.defsList.ItemCommand += new DataListCommandEventHandler(this.DefsList_ItemCommand);

		SuppressMenuSelection();
		SuppressPageMenu();

	}

	#endregion
}
