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

		chkShowTitle.Checked = Global.SkinConfig.Display.ShowModuleTitlesByDefault;

		divTitleElement.Visible = Global.SkinConfig.Display.EnableEditingModuleTitleElement
			&& Global.SkinConfig.Display.ModuleTitleElementOptions.Count() > 1;

		if (divTitleElement.Visible)
		{
			ddlTitleElements.DataSource = Global.SkinConfig.Display.ModuleTitleElementOptions;
			if (!string.IsNullOrWhiteSpace(Global.SkinConfig.Display.ModuleTitleElement)
				&& Global.SkinConfig.Display.ModuleTitleElementOptions.Contains(Global.SkinConfig.Display.ModuleTitleElement))
			{
				ddlTitleElements.SelectedValue = Global.SkinConfig.Display.ModuleTitleElement;
			}
			ddlTitleElements.DataBind();
		}

		ddlViewRoles.Items.Clear();
		ddlViewRoles.Items.Add(new ListItem(Resource.InheritFromPage, "inherit") { Selected = true });
		//ddlViewRoles.Items.Add(new ListItem(Resource.DefaultRootPageViewRoles, "default"));
		ddlViewRoles.Items.Add(new ListItem(Resource.Role_Administrators, "admins"));
		ddlViewRoles.Items.Add(new ListItem(Resource.Role_ContentAdministrators, "contentadmins"));
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
			"inherit" => CurrentPage.AuthorizedRoles,
			"default" => siteSettings.DefaultRootPageViewRoles,
			"admins" => "Admins;",
			_ => string.Empty, //empty means all roles
		};

		string moduleTitleElement = Global.SkinConfig.Display.ModuleTitleElement;
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