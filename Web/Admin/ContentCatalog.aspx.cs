using System;
using System.Data;
using System.Web.UI.WebControls;
//using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class ContentCatalogPage : NonCmsBasePage
{
	//private static readonly ILog log = LogManager.GetLogger(typeof(ContentCatalogPage));

	private string sortParam;
	private string sort = "ModuleTitle";
	private int pageNumber = 1;
	private int pageSize = WebConfigSettings.ContentCatalogPageSize;
	private int totalPages = 0;
	private bool sortByFeature = false;
	private bool sortByAuthor = false;
	private bool isContentAdmin = false;
	private bool isAdmin = false;
	private bool isSiteEditor = false;
	private int moduleDefId = -1;
	private string title = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		LoadSettings();

		if (!isContentAdmin && !isSiteEditor)
		{
			SiteUtils.RedirectToAccessDeniedPage();
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (Page.IsPostBack) return;

		BindFeatureList();
		BindGrid();

		if (moduleDefId > -1)
		{
			ListItem item = ddModuleType.Items.FindByValue(moduleDefId.ToInvariantString());
			if (item != null)
			{
				ddModuleType.ClearSelection();
				item.Selected = true;
			}
		}

		txtTitleFilter.Text = title;
	}

	private void BindGrid()
	{
		DataTable dt = Module.SelectPage(
			siteSettings.SiteId,
			moduleDefId,
			title,
			pageNumber,
			pageSize,
			sortByFeature,
			sortByAuthor,
			out totalPages);

		string pageUrl = Invariant($"{SiteRoot}/Admin/ContentCatalog.aspx?md={moduleDefId}&amp;title={Server.UrlEncode(title)}&amp;sort={sort}&amp;pagenumber={{0}}");

		pgrContent.PageURLFormat = pageUrl;
		pgrContent.ShowFirstLast = true;
		pgrContent.CurrentIndex = pageNumber;
		pgrContent.PageSize = pageSize;
		pgrContent.PageCount = totalPages;
		pgrContent.Visible = (totalPages > 1);

		grdContent.DataSource = dt;
		grdContent.DataBind();
	}

	private void BindFeatureList()
	{
		ddModuleType.Items.Add(new ListItem("All", "-1"));
		using IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId);
		while (reader.Read())
		{
			string allowedRoles = reader["AuthorizedRoles"].ToString();
			if (WebUser.IsInRoles(allowedRoles))
			{
				ddModuleType.Items.Add(new ListItem(
					ResourceHelper.GetResourceString(
					reader["ResourceFile"].ToString(),
					reader["FeatureName"].ToString()),
					reader["ModuleDefID"].ToString())
					);
			}
		}
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentManagerLink);
		heading.Text = Resource.AdminMenuContentManagerLink;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

		lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
		lnkContentManager.ToolTip = Resource.AdminMenuContentManagerLink;
		lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";

		grdContent.Columns[0].HeaderText = Resource.ContentManagerContentTitleColumnHeader;
		grdContent.Columns[1].HeaderText = Resource.ContentManagerFeatureTypeColumnHeader;
		grdContent.Columns[2].HeaderText = Resource.ContentManagerAuthorColumnHeader;

		btnFind.Text = Resource.ContentManagerFindButton;
	}

	void btnFind_Click(object sender, EventArgs e)
	{
		int.TryParse(ddModuleType.SelectedValue, out moduleDefId);

		title = txtTitleFilter.Text;

		string redirectUrl = Invariant($"{SiteRoot}/Admin/ContentCatalog.aspx?md={moduleDefId}&title={Server.UrlEncode(title)}&sort={sort}&pagenumber={pageNumber}");

		WebUtils.SetupRedirect(this, redirectUrl);
	}

	void grdContent_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (isAdmin) { return; }

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (e.Row.DataItem is DataRowView row)
			{
				if (row["AuthorizedEditRoles"].ToString() == "Admins;")
				{
					e.Row.Visible = false;
				}
			}
		}
	}

	protected void grdContent_Sorting(object sender, GridViewSortEventArgs e)
	{
		string redirectUrl = Invariant($"{SiteRoot}/Admin/ContentCatalog.aspx?md={moduleDefId}&title={Server.UrlEncode(title)}&sort={e.SortExpression}&pagenumber={pageNumber}");

		WebUtils.SetupRedirect(this, redirectUrl);
	}

	private void LoadSettings()
	{
		isSiteEditor = SiteUtils.UserIsSiteEditor();
		isContentAdmin = WebUser.IsAdminOrContentAdmin;
		isAdmin = WebUser.IsAdmin;
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
		moduleDefId = WebUtils.ParseInt32FromQueryString("md", moduleDefId);

		AddClassToBody("administration");
		AddClassToBody("cmadmin");

		if (Request.QueryString["title"] != null)
		{
			title = Request.QueryString["title"];
		}

		sortParam = "sort";

		if (Page.Request.Params[sortParam] != null)
		{
			sort = Page.Request.Params[sortParam];
			switch (sort)
			{
				case "FeatureName":
					sortByFeature = true;
					sortByAuthor = false;
					break;

				case "CreatedBy":
					sortByFeature = false;
					sortByAuthor = true;
					break;

				case "ModuleTitle":
				default:
					sortByFeature = false;
					sortByAuthor = false;
					sort = "ModuleTitle";
					break;
			}
		}
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		grdContent.Sorting += new GridViewSortEventHandler(grdContent_Sorting);
		grdContent.RowDataBound += new GridViewRowEventHandler(grdContent_RowDataBound);
		btnFind.Click += new EventHandler(btnFind_Click);

		SuppressMenuSelection();
		SuppressPageMenu();

		ScriptConfig.IncludeJQTable = true;
	}
	#endregion
}
