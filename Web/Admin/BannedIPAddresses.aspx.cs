using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class BannedIPAddressesPage : NonCmsBasePage
{
	#region Fields

	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 20;
	protected double timeOffset = 0;
	protected TimeZoneInfo timeZone = null;

	#endregion


	#region Event Handlers

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);

			return;
		}

		if (!WebUser.IsAdmin || SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);

			return;
		}

		if (!siteSettings.IsServerAdminSite)
		{
			WebUtils.SetupRedirect(this, "Admin/AdminMenu.aspx".ToLinkBuilder().ToString());

			return;
		}

		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}


	private void btnIPLookup_Click(object sender, EventArgs e)
	{
		if (txtIPAddress.Text.Length == 0)
		{
			WebUtils.SetupRedirect(this, Request.RawUrl);

			return;
		}

		BindSearch();
	}


	private void grdBannedIPAddresses_Sorting(object sender, GridViewSortEventArgs e)
	{
		// TODO: Implement Sorting
	}


	private void grdBannedIPAddresses_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		if (!Page.IsValid)
		{
			return;
		}

		var grid = (GridView)sender;

		var rowID = (int)grid.DataKeys[e.RowIndex].Value;
		var txtBannedIP = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedIP");
		//var txtBannedUTC = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedUTC");
		var txtBannedReason = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedReason");

		var bannedIPAddress = new BannedIPAddress(rowID)
		{
			BannedIP = txtBannedIP.Text,
			BannedUtc = DateTime.UtcNow,
			BannedReason = txtBannedReason.Text
		};

		bannedIPAddress.Save();

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdBannedIPAddresses_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		var grid = (GridView)sender;
		var rowID = (int)grid.DataKeys[e.RowIndex].Value;

		_ = BannedIPAddress.Delete(rowID);

		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdBannedIPAddresses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		WebUtils.SetupRedirect(this, Request.RawUrl);
	}


	private void grdBannedIPAddresses_RowEditing(object sender, GridViewEditEventArgs e)
	{
		var grid = (GridView)sender;

		grid.EditIndex = e.NewEditIndex;

		BindGrid();

		if (grid.Rows[e.NewEditIndex].Cells[1].FindControl("valIP") is RegularExpressionValidator valIP)
		{
			valIP.ErrorMessage = Resource.IpAddress;
		}

		if (grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete") is Button btnDelete)
		{
			btnDelete?.Attributes.Add("OnClick", $"return confirm('{Resource.BannedIPAddressDeleteConfirmMessage}');");
		}
	}
	private void grdBannedIPAddresses_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.FindControl("valIP") is RegularExpressionValidator valIP)
		{
			valIP.ErrorMessage = Resource.IpAddress;
		}
	}

	private void btnAddNew_Click(object sender, EventArgs e)
	{
		var dataTable = new DataTable();

		dataTable.Columns.Add("RowID", typeof(int));
		dataTable.Columns.Add("BannedIP", typeof(string));
		dataTable.Columns.Add("BannedUTC", typeof(DateTime));
		dataTable.Columns.Add("BannedReason", typeof(string));
		dataTable.Columns.Add("TotalPages", typeof(int));

		DataRow row = dataTable.NewRow();

		row["RowID"] = -1;
		row["BannedIP"] = string.Empty;
		row["BannedUTC"] = DateTime.UtcNow;
		row["BannedReason"] = string.Empty;
		row["TotalPages"] = 1;
		dataTable.Rows.Add(row);

		grdBannedIPAddresses.EditIndex = 0;
		grdBannedIPAddresses.DataSource = dataTable.DefaultView;

		grdBannedIPAddresses.DataBind();

		btnAddNew.Visible = false;
		pgrBannedIPAddresses.Visible = false;
	}

	#endregion


	#region Private Methods

	private void PopulateControls()
	{
		if (!IsPostBack)
		{
			BindGrid();
		}
	}


	private void BindGrid()
	{
		if (txtIPAddress.Text.Length > 0)
		{
			BindSearch();

			return;
		}

		List<BannedIPAddress> bannedIPs = BannedIPAddress.GetPage(pageNumber, pageSize, out totalPages);

		if (totalPages > 1)
		{
			pgrBannedIPAddresses.PageURLFormat = "Admin/BannedIPAddresses.aspx".ToLinkBuilder().PageNumber("{0}").ToString();
			pgrBannedIPAddresses.ShowFirstLast = true;
			pgrBannedIPAddresses.CurrentIndex = pageNumber;
			pgrBannedIPAddresses.PageSize = pageSize;
			pgrBannedIPAddresses.PageCount = totalPages;
		}
		else
		{
			pgrBannedIPAddresses.Visible = false;
		}

		grdBannedIPAddresses.DataSource = bannedIPs;
		grdBannedIPAddresses.PageIndex = pageNumber;
		grdBannedIPAddresses.PageSize = pageSize;

		grdBannedIPAddresses.DataBind();
	}


	private void BindSearch()
	{
		pgrBannedIPAddresses.Visible = false;

		using (IDataReader reader = BannedIPAddress.GeByIpAddress(txtIPAddress.Text))
		{
			grdBannedIPAddresses.DataSource = reader;

			grdBannedIPAddresses.DataBind();
		}
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.BannedIPAddressesHeading);
		heading.Text = Resource.BannedIPAddressesHeading;

		btnAddNew.Text = Resource.BannedIPAddressesAddNewButton;
		grdBannedIPAddresses.Columns[1].HeaderText = Resource.BannedIPAddressLabel;
		grdBannedIPAddresses.Columns[2].HeaderText = Resource.BannedIPAddressReasonLabel;
		grdBannedIPAddresses.Columns[3].HeaderText = Resource.BannedIPAddressUTCLabel;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = "Admin/AdminMenu.aspx".ToLinkBuilder().ToString();

		lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
		lnkAdvancedTools.NavigateUrl = "Admin/AdvancedTools.aspx".ToLinkBuilder().ToString();

		lnkBannedIPs.Text = Resource.AdminMenuBannedIPAddressesLabel;
		lnkBannedIPs.NavigateUrl = "Admin/BannedIPAddresses.aspx".ToLinkBuilder().ToString();

		btnIPLookup.Text = Resource.BannedIPSearchButton;
	}


	private void LoadSettings()
	{
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		AddClassToBody("administration");
		AddClassToBody("ipadmin");
	}

	#endregion


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		grdBannedIPAddresses.Sorting += new GridViewSortEventHandler(grdBannedIPAddresses_Sorting);
		grdBannedIPAddresses.RowEditing += new GridViewEditEventHandler(grdBannedIPAddresses_RowEditing);
		grdBannedIPAddresses.RowCancelingEdit += new GridViewCancelEditEventHandler(grdBannedIPAddresses_RowCancelingEdit);
		grdBannedIPAddresses.RowUpdating += new GridViewUpdateEventHandler(grdBannedIPAddresses_RowUpdating);
		grdBannedIPAddresses.RowDeleting += new GridViewDeleteEventHandler(grdBannedIPAddresses_RowDeleting);
		grdBannedIPAddresses.RowCreated += new GridViewRowEventHandler(grdBannedIPAddresses_RowCreated);
		btnAddNew.Click += new EventHandler(btnAddNew_Click);
		btnIPLookup.Click += new EventHandler(btnIPLookup_Click);

		SuppressMenuSelection();
		SuppressPageMenu();
		ScriptConfig.IncludeJQTable = true;
	}

	#endregion
}