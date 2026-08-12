using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI;

public partial class ModuleDefinitionSettingsPage : NonCmsBasePage
{
	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		grdSettings.RowEditing += new GridViewEditEventHandler(grdSettings_RowEditing);
		grdSettings.RowCancelingEdit += new GridViewCancelEditEventHandler(grdSettings_RowCancelingEdit);
		grdSettings.RowUpdating += new GridViewUpdateEventHandler(grdSettings_RowUpdating);
		grdSettings.RowDeleting += new GridViewDeleteEventHandler(grdSettings_RowDeleting);
		grdSettings.RowDataBound += new GridViewRowEventHandler(grdSettings_RowDataBound);
		btnCreateNewSetting.Click += new EventHandler(btnCreateNewSetting_Click);

		SuppressMenuSelection();
		SuppressPageMenu();

		ScriptConfig.IncludeJQTable = true;
	}

	#endregion

	private static readonly ILog log = LogManager.GetLogger(typeof(ModuleDefinitionSettingsPage));

	private int moduleDefId = -1;
	protected string EditContentImage = WebConfigSettings.EditContentImage;

	protected void Page_Load(object sender, EventArgs e)
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
			WebUtils.SetupRedirect(this, $"{SiteRoot}/Admin/AdminMenu.aspx");
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadParams();
		PopulateLabels();

		if (!IsPostBack)
		{
			BindControls();
		}

	}

	protected void BindControls()
	{
		if (moduleDefId > -1)
		{
			ModuleDefinition moduleDef = new ModuleDefinition(moduleDefId);
			//if (moduleDef.SiteID != siteSettings.SiteID) return;
			lnkModuleDefinition.Text = ResourceHelper.GetResourceString(moduleDef.ResourceFile, moduleDef.FeatureName);

			heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.FeatureSettingsFormat, lnkModuleDefinition.Text);


			lnkModuleDefinition.ToolTip = lnkModuleDefinition.Text;
			lnkModuleDefinition.NavigateUrl = Invariant($"{SiteRoot}/Admin/ModuleDefinitions.aspx?defid={moduleDefId}");

			ArrayList defSettings = ModuleSettings.GetDefaultSettings(moduleDefId);
			grdSettings.DataSource = defSettings;
			grdSettings.DataBind();
		}
	}

	protected void btnCreateNewSetting_Click(object sender, EventArgs e)
	{
		if (moduleDefId > -1)
		{
			var featureDef = new ModuleDefinition(moduleDefId);

			ModuleDefinition.UpdateModuleDefinitionSetting(
				featureDef.FeatureGuid,
				moduleDefId,
				txtNewResourceFile.Text,
				txtGroupNameKey.Text,
				txtNewSettingName.Text,
				txtNewSettingValue.Text,
				ddNewControlType.SelectedValue,
				txtNewRegexValidationExpression.Text,
				txtNewControlSrc.Text,
				txtNewHelpKey.Text,
				Convert.ToInt32(txtNewSortOrder.Text),
				txtAttributes.Text,
				txtOptions.Text,
				txtNewRoles.Text,
				chkNewShowToUnauthorized.Checked);
		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	protected void grdSettings_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		var grid = (GridView)sender;
		var settingId = (int)grid.DataKeys[e.RowIndex].Value;

		var txtResourceFile = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtResourceFile");
		var txtGroupNameKey = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtGroupNameKey");
		var txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSettingName");
		var txtValue = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSettingValue");
		var txtRegex = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRegexValidationExpression");
		var ddType = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddControlType");

		var txtControlSrc = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtControlSrc");
		var txtHelpKey = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHelpKey");
		var txtSortOrder = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSortOrder");
		var txtAttributes = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtAttributes");
		var txtOptions = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtOptions");
		var txtRoles = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRoles");
		var chkShowToUnauthorized = (CheckBox)grid.Rows[e.RowIndex].Cells[1].FindControl("chkShowToUnauthorized");

		if (moduleDefId > -1)
		{
			ModuleDefinition.UpdateModuleDefinitionSettingById(
				settingId,
				moduleDefId,
				txtResourceFile.Text,
				txtGroupNameKey.Text,
				txtName.Text,
				txtValue.Text,
				ddType.SelectedValue,
				txtRegex.Text,
				txtControlSrc.Text,
				txtHelpKey.Text,
				Convert.ToInt32(txtSortOrder.Text),
				txtAttributes.Text,
				txtOptions.Text,
				txtRoles.Text,
				chkShowToUnauthorized.Checked);
		}

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	protected void grdSettings_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		GridView grid = (GridView)sender;
		int settingID = (int)grid.DataKeys[e.RowIndex].Value;
		ModuleDefinition.DeleteSettingById(settingID);

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	protected void grdSettings_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	protected void grdSettings_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView grid = (GridView)sender;
		grid.EditIndex = e.NewEditIndex;
		BindControls();
	}


	void grdSettings_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		Button button = e.Row.FindControl("btnGridDelete") as Button;
		UIHelper.AddConfirmationDialog(button, Resource.ModuleDefinitionDeleteSettingWarning);
	}

	protected string GetEditImageAltText()
	{
		return Resource.ModuleDefinitionSettingsEditButton;
	}


	protected string GetEditImageUrl()
	{
		return ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
	}

	protected string GetEditButtonText()
	{
		return Resource.ModuleDefinitionSettingsEditButton;
	}

	protected string GetUpdateButtonText()
	{
		return Resource.ModuleDefinitionSettingsUpdateButton;
	}

	protected string GetDeleteButtonText()
	{
		return Resource.ModuleDefinitionSettingsDeleteButton;
	}

	protected string GetCancelButtonText()
	{
		return Resource.ModuleDefinitionSettingsCancelButton;
	}

	protected void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);

		subHeading.Text = Resource.ModuleDefinitionsAddSettingHeader;

		lnkAdminMenu.Text = Resource.AdvancedToolsLink;
		lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";
		lnkModuleAdmin.Text = Resource.AdminMenuFeatureModulesLink;
		lnkModuleAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";
		btnCreateNewSetting.Text = Resource.ModuleDefinitionsAddSettingButton;
	}

	private void LoadParams()
	{
		moduleDefId = WebUtils.ParseInt32FromQueryString("defid", -1);

		AddClassToBody("administration");
		AddClassToBody("featuredefadmin");
	}
}
