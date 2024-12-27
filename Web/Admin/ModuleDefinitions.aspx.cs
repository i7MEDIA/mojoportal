using System;
using System.Data.Common;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class ModuleDefinitions : NonCmsBasePage
{
	private int moduleDefinitionId = -1;
	private int pageId = -1;
	//private string iconPath;

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		updateButton.Click += new EventHandler(UpdateBtn_Click);
		cancelButton.Click += new EventHandler(CancelBtn_Click);
		deleteButton.Click += new EventHandler(DeleteBtn_Click);

		SuppressMenuSelection();
		SuppressPageMenu();

	}
	#endregion

	private void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		if (!WebUser.IsAdmin)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		SecurityHelper.DisableBrowserCache();

		if (!siteSettings.IsServerAdminSite)
		{
			WebUtils.SetupRedirect(this, "/Admin/AdminMenu.aspx".ToLinkBuilder().ToString());
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadParams();
		PopulateLabels();
		//SetupIconScript();

		if (!IsPostBack)
		{
			PopulateControls();
		}
	}


	private void PopulateControls()
	{

		if (moduleDefinitionId > -1)
		{
			var moduleDefinition = new ModuleDefinition(moduleDefinitionId);
			txtFeatureName.Text = moduleDefinition.FeatureName;
			txtResourceFile.Text = moduleDefinition.ResourceFile;
			txtFeatureGuid.Text = moduleDefinition.FeatureGuid.ToString();
			txtControlSource.Text = moduleDefinition.ControlSrc;
			txtSortOrder.Text = moduleDefinition.SortOrder.ToString(CultureInfo.InvariantCulture);
			txtDefaultCacheDuration.Text = moduleDefinition.DefaultCacheTime.ToString(CultureInfo.InvariantCulture);
			chkIsAdmin.Checked = moduleDefinition.IsAdmin;
			chkIsCacheable.Checked = moduleDefinition.IsCacheable;
			chkIsSearchable.Checked = moduleDefinition.IsSearchable;
			txtSearchListName.Text = moduleDefinition.SearchListName;
		}
		else
		{
			txtFeatureName.Text = Resource.ModuleDefinitionsDefaultModuleName;
			txtControlSource.Text = Resource.ModuleDefinitionsDefaultControlSource;
			txtFeatureGuid.Text = Guid.NewGuid().ToString();
			txtSortOrder.Text = "500";
			txtDefaultCacheDuration.Text = "0";
			lnkConfigureSettings.Visible = false;
			deleteButton.Visible = false;
		}
	}


	private void UpdateBtn_Click(object sender, EventArgs e)
	{
		if (Page.IsValid)
		{
			var moduleDefinition = new ModuleDefinition(moduleDefinitionId)
			{
				SiteId = siteSettings.SiteId,
				FeatureName = txtFeatureName.Text,
				ResourceFile = txtResourceFile.Text,
				ControlSrc = txtControlSource.Text,
				SortOrder = int.Parse(txtSortOrder.Text, CultureInfo.InvariantCulture),
				DefaultCacheTime = int.Parse(txtDefaultCacheDuration.Text, CultureInfo.InvariantCulture),
				IsAdmin = chkIsAdmin.Checked,
				IsCacheable = chkIsCacheable.Checked,
				IsSearchable = chkIsSearchable.Checked,
				SearchListName = txtSearchListName.Text
			};

			if (txtFeatureGuid.Text.Length == 36)
			{
				moduleDefinition.FeatureGuid = new Guid(txtFeatureGuid.Text);
			}
			moduleDefinition.Save();

			string redirectUrl = "/Admin/ModuleDefinitions.aspx".ToLinkBuilder().AddParam("defid", moduleDefinition.ModuleDefId).ToString();

			WebUtils.SetupRedirect(this, redirectUrl);
		}
	}


	private void DeleteBtn_Click(object sender, EventArgs e)
	{
		lblErrorMessage.Text = string.Empty;

		int countOfUse = Module.GetCountByFeature(moduleDefinitionId);
		if (countOfUse > 0)
		{
			lblErrorMessage.Text = Resource.ModuleDefinitionsDeleteInstancesBeforeModuleDefinitionMessage;
			return;
		}
		try
		{
			ModuleDefinition.DeleteModuleDefinition(moduleDefinitionId);
			ModuleDefinition.DeleteSettingsByFeature(moduleDefinitionId);

			DoRedirect();
		}
		catch (DbException)
		{
			lblErrorMessage.Text = Resource.ModuleDefinitionsDeleteInstancesBeforeModuleDefinitionMessage;
		}
	}


	private void CancelBtn_Click(object sender, EventArgs e) => DoRedirect();


	private void DoRedirect()
	{
		string redirectUrl;
		if (pageId > -1)
		{
			redirectUrl = "~/Default.aspx".ToLinkBuilder().PageId(pageId).ToString();
		}
		else
		{
			redirectUrl = "Admin/ModuleAdmin.aspx".ToLinkBuilder().ToString();
		}

		WebUtils.SetupRedirect(this, redirectUrl);
		return;
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);

		heading.Text = Resource.ModuleDefinitionsModuleDefinitionLabel;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = "/Admin/AdminMenu.aspx".ToLinkBuilder().ToString();

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = "/Admin/AdvancedTools.aspx".ToLinkBuilder().ToString();

		lnkModuleAdmin.Text = Resource.AdminMenuFeatureModulesLink;
		lnkModuleAdmin.ToolTip = Resource.AdminMenuFeatureModulesLink;
		lnkModuleAdmin.NavigateUrl = "/Admin/ModuleAdmin.aspx".ToLinkBuilder().ToString();

		reqFeatureName.ErrorMessage = Resource.ModuleDefinitionsFeatureNameRequiredHelp;
		reqControlSource.ErrorMessage = Resource.ModuleDefinitionsControlSourceRequiredHelp;

		reqSortOrder.ErrorMessage = Resource.ModuleDefinitionSortRequiredMessage;
		reqDefaultCache.ErrorMessage = Resource.ModuleDefinitionDefaultCacheRequiredMessage;
		regexSortOrder.ErrorMessage = Resource.ModuleDefinitionSortRegexWarning;
		regexCacheDuration.ErrorMessage = Resource.ModuleDefinitionDefaultCacheRegexWarning;

		updateButton.Text = Resource.ModuleDefinitionsUpdateButton;
		SiteUtils.SetButtonAccessKey(updateButton, AccessKeys.ModuleDefinitionsUpdateButtonAccessKey);

		cancelButton.Text = Resource.ModuleDefinitionsCancelButton;
		SiteUtils.SetButtonAccessKey(cancelButton, AccessKeys.ModuleDefinitionsCancelButtonAccessKey);

		deleteButton.Text = Resource.ModuleDefinitionsDeleteButton;
		SiteUtils.SetButtonAccessKey(deleteButton, AccessKeys.ModuleDefinitionsDeleteButtonAccessKey);
		UIHelper.AddConfirmationDialog(deleteButton, Resource.ModuleDefinitionsDeleteWarning);

		lnkConfigureSettings.Text = Resource.ModuleDefinitionsConfigureLink;
		lnkConfigureSettings.NavigateUrl = "/Admin/ModuleDefinitionSettings.aspx".ToLinkBuilder().AddParam("defid", moduleDefinitionId).ToString();
	}


	private void LoadParams()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		moduleDefinitionId = WebUtils.ParseInt32FromQueryString("defid", -1);

		AddClassToBody("administration");
		AddClassToBody("featureadmin");
	}
}