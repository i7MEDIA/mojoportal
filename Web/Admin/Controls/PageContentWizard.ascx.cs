using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public partial class PageContentWizard : UserControl
{
	private SiteSettings siteSettings = null;
	private const string ROLES_ADMINISTRATORS = "admins";
	private const string ROLES_CONTENTADMINISTRATORS = "contentadmins";
	private const string ROLES_INHERITFROMPAGE = "inherit";
	private const string ROLES_COPYFROMPAGE = "copy";
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();

		if (!Page.IsPostBack)
		{
			PopulateControls();
		}
	}

	private void PopulateControls()
	{
		BindFeatureList();

		chkShowTitle.Checked = Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_ShowByDefault;

		divTitleElement.Visible = Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element_AllowEditing
			&& Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element_Options.Count() > 1;

		if (divTitleElement.Visible)
		{
			ddlTitleElements.DataSource = Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element_Options;
			if (!string.IsNullOrWhiteSpace(Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element)
				&& Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element_Options.Contains(Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element))
			{
				ddlTitleElements.SelectedValue = Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element;
			}
			ddlTitleElements.DataBind();
		}

		ddlViewRoles.Items.Clear();
		ddlViewRoles.Items.Add(new ListItem(Resource.InheritFromPage, ROLES_INHERITFROMPAGE) { Selected = true });
		ddlViewRoles.Items.Add(new ListItem(Resource.CopyFromPage, ROLES_COPYFROMPAGE));
		//ddlViewRoles.Items.Add(new ListItem(Resource.DefaultRootPageViewRoles, "default"));
		ddlViewRoles.Items.Add(new ListItem(Resource.Role_Administrators, ROLES_ADMINISTRATORS));
		ddlViewRoles.Items.Add(new ListItem(Resource.Role_ContentAdministrators, ROLES_CONTENTADMINISTRATORS));
	}
	private void BindFeatureList()
	{
		if (siteSettings == null)
		{
			return;
		}

		using IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId);

		while (reader.Read())
		{
			string allowedRoles = reader["AuthorizedRoles"].ToString();
			if (WebUser.IsInRoles(allowedRoles))
			{
				moduleType.Items.Add(new ListItem(
					ResourceHelper.GetResourceString(
					reader["ResourceFile"].ToString(),
					reader["FeatureName"].ToString()),
					reader["ModuleDefID"].ToString())
					);
			}
		}
	}

	void btnCreateNewContent_Click(object sender, EventArgs e)
	{
		Page.Validate("contentwizard");
		if (!Page.IsValid)
		{
			return;
		}

		int moduleDefID = int.Parse(moduleType.SelectedItem.Value);
		var moduleDefinition = new ModuleDefinition(moduleDefID);
		var CurrentPage = CacheHelper.GetCurrentPage();
		var rolesString = ddlViewRoles.SelectedValue switch
		{
			ROLES_COPYFROMPAGE => CurrentPage.AuthorizedRoles,
			ROLES_ADMINISTRATORS => $"{Role.AdministratorsRole};",
			ROLES_CONTENTADMINISTRATORS => $"{Role.ContentAdministratorsRole};",
			_ => new Module().ViewRoles, //get default from module definition
		};

		string moduleTitleElement = Global.SkinConfig.ModuleDisplayOptions.ModuleTitle_Element;
		if (divTitleElement.Visible)
		{
			moduleTitleElement = ddlTitleElements.SelectedValue;
		}
		else if (string.IsNullOrWhiteSpace(moduleTitleElement))
		{
			moduleTitleElement = WebConfigSettings.ModuleTitleTag;
		}

		var m = new Module
		{
			SiteId = siteSettings.SiteId,
			SiteGuid = siteSettings.SiteGuid,
			ModuleDefId = moduleDefID,
			FeatureGuid = moduleDefinition.FeatureGuid,
			Icon = moduleDefinition.Icon,
			CacheTime = moduleDefinition.DefaultCacheTime,
			PageId = CurrentPage.PageId,
			ModuleTitle = moduleTitle.Text,
			ShowTitle = chkShowTitle.Checked,
			HeadElement = moduleTitleElement,
			PaneName = "contentpane",
			ViewRoles = rolesString,
		};
		//m.AuthorizedEditRoles = "Admins";
		//var currentUser = SiteUtils.GetCurrentSiteUser();
		if (SiteUtils.GetCurrentSiteUser() is SiteUser currentUser)
		{
			m.CreatedByUserId = currentUser.UserId;
		}

		//m.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
		//m.HeadElement = WebConfigSettings.ModuleTitleTag;
		m.Save();

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void PopulateLabels()
	{
		litInstructions.Text = Resource.ContentWizardInstructions;
		btnCreateNewContent.Text = Resource.MyPageAddContentLabel;

		reqModuleTitle.ErrorMessage = Resource.TitleRequiredWarning;
		reqModuleTitle.Enabled = WebConfigSettings.RequireContentTitle;
	}

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnCreateNewContent.Click += btnCreateNewContent_Click;
	}
}