using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class AdminLanguagePage : NonCmsBasePage
{
	private int pageNumber = 1;
	private int pageSize = 20;
	private int totalPages = 0;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		LoadParams();

		if ((!WebUser.IsAdminOrContentAdmin) || (!siteSettings.IsServerAdminSite))
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
		AddClassToBody("administration langadmin");

		if (!Page.IsPostBack)
		{
			BindGrid();
		}

	}

	private void BindGrid()
	{
		using IDataReader reader = Language.GetPage(pageNumber, pageSize, out totalPages);
		if (totalPages > 1)
		{
			string pageUrl = SiteRoot + "/Admin/AdminLanguage.aspx?pagenumber={0}";

			pgrLanguage.PageURLFormat = pageUrl;
			pgrLanguage.ShowFirstLast = true;
			pgrLanguage.CurrentIndex = pageNumber;
			pgrLanguage.PageSize = pageSize;
			pgrLanguage.PageCount = totalPages;
		}
		else
		{
			pgrLanguage.Visible = false;
		}

		grdLanguage.PageIndex = pageNumber;
		grdLanguage.PageSize = pageSize;
		grdLanguage.DataSource = reader;
		grdLanguage.DataBind();
	}

	private void grdLanguage_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		var grid = (GridView)sender;
		var guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		var txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
		var txtCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtCode");
		var txtSort = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSort");

		Language language;
		if (guid != Guid.Empty)
		{
			language = new Language(guid);
		}
		else
		{
			language = new Language();
		}

		language.Name = txtName.Text;
		language.Code = txtCode.Text;
		if (!int.TryParse(txtSort.Text, out int sort))
		{
			sort = 100;
		}
		language.Sort = sort;
		language.Save();

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void grdLanguage_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		var grid = (GridView)sender;
		var guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
		Language.Delete(guid);
		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void grdLanguage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		WebUtils.SetupRedirect(this, Request.RawUrl);
	}

	private void grdLanguage_RowEditing(object sender, GridViewEditEventArgs e)
	{
		var grid = (GridView)sender;
		grid.EditIndex = e.NewEditIndex;
		BindGrid();

		var btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
		btnDelete?.Attributes.Add("OnClick", $"return confirm('{Resource.LanguageGridDeleteWarning}');");
	}

	private void btnAddNew_Click(object sender, EventArgs e)
	{
		var dataTable = new DataTable();
		dataTable.Columns.Add("Guid", typeof(Guid));
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("Code", typeof(string));
		dataTable.Columns.Add("Sort", typeof(int));
		dataTable.Columns.Add("TotalPages", typeof(int));

		var row = dataTable.NewRow();
		row["Guid"] = Guid.Empty;
		row["Name"] = string.Empty;
		row["Code"] = string.Empty;
		row["Sort"] = 100;
		row["TotalPages"] = 1;
		dataTable.Rows.Add(row);

		grdLanguage.EditIndex = 0;
		grdLanguage.DataSource = dataTable.DefaultView;
		grdLanguage.DataBind();

		btnAddNew.Visible = false;
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.LanguageAdministrationHeading);
		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkCoreData.Text = Resource.CoreDataAdministrationLink;
		lnkCoreData.NavigateUrl = $"{SiteRoot}/Admin/CoreData.aspx";

		lnkCurrentPage.Text = Resource.LanguageAdministrationLink;
		lnkCurrentPage.NavigateUrl = $"{SiteRoot}/Admin/AdminLanguage.aspx";

		heading.Text = Resource.LanguageAdministrationHeading;

		grdLanguage.Columns[1].HeaderText = Resource.LanguageGridNameHeading;
		grdLanguage.Columns[2].HeaderText = Resource.LanguageGridCodeHeading;
		grdLanguage.Columns[3].HeaderText = Resource.LanguageGridSortHeading;

		btnAddNew.Text = Resource.LanguageGridAddNewButton;
	}

	private void LoadParams()
	{
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		grdLanguage.RowEditing += new GridViewEditEventHandler(grdLanguage_RowEditing);
		grdLanguage.RowCancelingEdit += new GridViewCancelEditEventHandler(grdLanguage_RowCancelingEdit);
		grdLanguage.RowUpdating += new GridViewUpdateEventHandler(grdLanguage_RowUpdating);
		grdLanguage.RowDeleting += new GridViewDeleteEventHandler(grdLanguage_RowDeleting);
		btnAddNew.Click += new EventHandler(btnAddNew_Click);

		SuppressMenuSelection();
		SuppressPageMenu();
		ScriptConfig.IncludeJQTable = true;
	}

	#endregion
}