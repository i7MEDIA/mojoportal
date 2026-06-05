using Lucene.Net.Search;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI;

public partial class AdminCountryPage : NonCmsBasePage
{
	private const string _sortName = "Name";
	private const string _sortIsoCode2 = "ISOCode2";
	private const string _sortIsoCode3 = "ISOCode3";

	private int _pageNumber = 1;
	private int _pageSize = 15;
	private int _totalPages = 0;
	private string _sort = _sortName;


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);

			return;
		}

		LoadSettings();

		if (!WebUser.IsAdminOrContentAdmin || !siteSettings.IsServerAdminSite)
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
		if (Page.IsPostBack)
		{
			return;
		}

		BindGrid();
	}


	private void BindGrid()
	{
		btnAddNew.Visible = true;

		var countries = GeoCountry.GetPage(_pageNumber, _pageSize, out _totalPages);
		var sortComparer = _sort switch
		{
			_sortIsoCode2 => new Comparison<GeoCountry>(GeoCountry.CompareByIsoCode2),
			_sortIsoCode3 => new Comparison<GeoCountry>(GeoCountry.CompareByIsoCode3),
			_ => new Comparison<GeoCountry>(GeoCountry.CompareByName),
		};

		if (sortComparer != null)
		{
			countries.Sort(sortComparer);
		}

		if (_totalPages > 1)
		{
			var pageUrl = $"~/Admin/AdminCountry.aspx"
				.ToLinkBuilder()
				.AddParam("pagenumber", "{0}")
				.AddParam("sort", _sort)
				.ToString();

			pgrCountry.Visible = true;
			pgrCountry.PageURLFormat = pageUrl;
			pgrCountry.ShowFirstLast = true;
			pgrCountry.CurrentIndex = _pageNumber;
			pgrCountry.PageSize = _pageSize;
			pgrCountry.PageCount = _totalPages;
		}
		else
		{
			pgrCountry.Visible = false;
		}

		grdCountry.DataSource = countries;
		grdCountry.PageIndex = _pageNumber;
		grdCountry.PageSize = _pageSize;

		grdCountry.DataBind();
	}


	private void grdCountry_Sorting(object sender, GridViewSortEventArgs e)
	{
		var redirectUrl = $"~/Admin/AdminCountry.aspx"
			.ToLinkBuilder()
			.AddParam("pagenumber", _pageNumber)
			.AddParam("sort", e.SortExpression)
			.ToString();

		WebUtils.SetupRedirect(this, redirectUrl);
	}


	private void grdCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		var grid = sender as GridView;
		var guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		var txtName = grid.Rows[e.RowIndex].Cells[1].FindControl("txtName") as TextBox;
		var txtISOCode2 = grid.Rows[e.RowIndex].Cells[1].FindControl("txtISOCode2") as TextBox;
		var txtISOCode3 = grid.Rows[e.RowIndex].Cells[1].FindControl("txtISOCode3") as TextBox;

		var country = guid == Guid.Empty ?
			new GeoCountry() :
			new GeoCountry(guid);

		country.Name = txtName.Text;
		country.IsoCode2 = txtISOCode2.Text;
		country.IsoCode3 = txtISOCode3.Text;

		country.Save();

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		var grid = sender as GridView;
		var guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());

		GeoCountry.Delete(guid);

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdCountry_RowEditing(object sender, GridViewEditEventArgs e)
	{
		var grid = sender as GridView;

		grid.EditIndex = e.NewEditIndex;

		BindGrid();

		var btnDelete = grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete") as Button;

		btnDelete?.Attributes.Add("OnClick", $"return confirm('{Resource.CountryGridDeleteWarning}');");
	}


	private void btnAddNew_Click(object sender, EventArgs e)
	{
		var dataTable = new DataTable();

		dataTable.Columns.Add("Guid", typeof(Guid));
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("ISOCode2", typeof(string));
		dataTable.Columns.Add("ISOCode3", typeof(string));
		dataTable.Columns.Add("TotalPages", typeof(int));

		var row = dataTable.NewRow();

		row["Guid"] = Guid.Empty;
		row["Name"] = string.Empty;
		row["ISOCode2"] = string.Empty;
		row["ISOCode3"] = string.Empty;
		row["TotalPages"] = 1;

		dataTable.Rows.Add(row);

		btnAddNew.Visible = false;
		pgrCountry.Visible = false;
		grdCountry.EditIndex = 0;
		grdCountry.DataSource = dataTable;

		grdCountry.DataBind();
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CountryAdministrationHeading);

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

		lnkCoreData.Text = Resource.CoreDataAdministrationLink;
		lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

		lnkCurrentPage.Text = Resource.CountryAdministrationLink;
		lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdminCountry.aspx";

		heading.Text = Resource.CountryAdministrationHeading;

		grdCountry.ToolTip = Resource.CountryAdministrationHeading;

		grdCountry.Columns[1].HeaderText = Resource.CountryGridNameHeader;
		grdCountry.Columns[2].HeaderText = Resource.CountryGridISOCode2Header;
		grdCountry.Columns[3].HeaderText = Resource.CountryGridISOCode3Header;

		btnAddNew.Text = Resource.CountryGridAddNewButton;
	}


	private void LoadSettings()
	{
		_pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
		_pageSize = WebUtils.ParseInt32FromQueryString("pagesize", 15);

		// Added insurance to get the default sort column, and to be case insensitive.
		_sort = WebUtils.ParseStringFromQueryString("sort", _sortName) switch
		{
			var sortParam when sortParam.Equals(_sortIsoCode2, StringComparison.OrdinalIgnoreCase) => _sortIsoCode2,
			var sortParam when sortParam.Equals(_sortIsoCode3, StringComparison.OrdinalIgnoreCase) => _sortIsoCode3,
			var sortParam when sortParam.Equals(_sortName, StringComparison.OrdinalIgnoreCase) => _sortName,
			_ => _sortName
		};

		AddClassToBody("administration");
		AddClassToBody("geoadmin");
	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		grdCountry.Sorting += new GridViewSortEventHandler(grdCountry_Sorting);
		grdCountry.RowEditing += new GridViewEditEventHandler(grdCountry_RowEditing);
		grdCountry.RowCancelingEdit += new GridViewCancelEditEventHandler(grdCountry_RowCancelingEdit);
		grdCountry.RowUpdating += new GridViewUpdateEventHandler(grdCountry_RowUpdating);
		grdCountry.RowDeleting += new GridViewDeleteEventHandler(grdCountry_RowDeleting);
		btnAddNew.Click += new EventHandler(btnAddNew_Click);

		SuppressMenuSelection();
		SuppressPageMenu();

		ScriptConfig.IncludeJQTable = true;
	}

	#endregion
}
