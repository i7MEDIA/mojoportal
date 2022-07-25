using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.EF;
using mojoPortal.Data.EF; // TODO: Ideally we wouldn't need this using because we would use Dependancy Injection. Soon™
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
	public partial class BannedIPAddressesPage : NonCmsBasePage
	{
		private int totalPages = 1;
		private int pageNumber = 1;
		private int pageSize = 20;
		protected double timeOffset = 0;
		protected TimeZoneInfo timeZone = null;
		private readonly IUnitOfWork unitOfWork;


		#region Constructors

		public BannedIPAddressesPage()
		{
			unitOfWork = new UnitOfWork(new mojoPortalDbContext());
		}

		#endregion


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

			if (!siteSettings.IsServerAdminSite)
			{
				WebUtils.SetupRedirect(this, SiteRoot + "/Admin/AdminMenu.aspx");

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

		private void LoadSettings()
		{
			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
			timeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();

			AddClassToBody("administration");
			AddClassToBody("ipadmin");
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
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
			lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

			lnkBannedIPs.Text = Resource.AdminMenuBannedIPAddressesLabel;
			lnkBannedIPs.NavigateUrl = SiteRoot + "/Admin/BannedIPAddresses.aspx";

			btnIPLookup.Text = Resource.BannedIPSearchButton;
		}


		private void PopulateControls()
		{
			if (!IsPostBack)
			{
				RegisterAsyncTask(new PageAsyncTask(BindGrid));
			}
		}


		private async Task BindGrid()
		{
			if (txtIPAddress.Text.Length > 0)
			{
				BindSearch();

				return;
			}

			//var bannedIPs = BannedIPAddress.GetPage(pageNumber, pageSize, out totalPages);
			IEnumerable<Core.EF.Domain.BannedIPAddress> bannedIPs;

			(bannedIPs, totalPages) = await unitOfWork.BannedIPAddresses.GetPageAsync(pageNumber, pageSize);

			if (totalPages > 1)
			{
				string pageUrl = SiteUtils.GetNavigationSiteRoot() + "/Admin/BannedIPAddresses.aspx?pagenumber={0}";

				pgrBannedIPAddresses.PageURLFormat = pageUrl;
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


		private async void BindSearch()
		{
			pgrBannedIPAddresses.Visible = false;

			//using (IDataReader reader = BannedIPAddress.GeByIpAddress(txtIPAddress.Text))
			//{
			//	grdBannedIPAddresses.DataSource = reader;
			//	grdBannedIPAddresses.DataBind();
			//}

			var bannedIPAddress = await unitOfWork.BannedIPAddresses.FindAsync(b => b.IPAddress.Equals(txtIPAddress.Text.Trim(), StringComparison.InvariantCultureIgnoreCase));

			grdBannedIPAddresses.DataSource = bannedIPAddress;
			grdBannedIPAddresses.DataBind();
		}


		#region Event Handlers

		private void btnIPLookup_Click(object sender, EventArgs e)
		{
			if (txtIPAddress.Text.Length == 0)
			{
				WebUtils.SetupRedirect(this, Request.RawUrl);

				return;
			}

			BindSearch();
		}


		private void btnAddNew_Click(object sender, EventArgs e)
		{
			var dataTable = new DataTable();

			dataTable.Columns.Add("ID", typeof(int));
			dataTable.Columns.Add("IPAddress", typeof(string));
			dataTable.Columns.Add("BannedUTC", typeof(DateTime));
			dataTable.Columns.Add("Reason", typeof(string));
			dataTable.Columns.Add("TotalPages", typeof(int));

			DataRow row = dataTable.NewRow();

			row["ID"] = -1;
			row["IPAddress"] = string.Empty;
			row["BannedUTC"] = DateTime.UtcNow;
			row["Reason"] = string.Empty;
			row["TotalPages"] = 1;

			dataTable.Rows.Add(row);

			grdBannedIPAddresses.EditIndex = 0;
			grdBannedIPAddresses.DataSource = dataTable.DefaultView;
			grdBannedIPAddresses.DataBind();

			btnAddNew.Visible = false;
			pgrBannedIPAddresses.Visible = false;
		}


		private async void grdBannedIPAddresses_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			var grid = (GridView)sender;
			var rowID = (int)grid.DataKeys[e.RowIndex].Value;
			var txtBannedIP = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedIP");
			var txtBannedUTC = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedUTC");
			var txtBannedReason = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtBannedReason");

			//BannedIPAddress bannedIPAddress = new BannedIPAddress();

			var newBannedIPAddress = new Core.EF.Domain.BannedIPAddress();

			if (rowID > 0)
			{
				//bannedIPAddress = new BannedIPAddress(rowID);
				newBannedIPAddress = await unitOfWork.BannedIPAddresses.GetAsync(rowID);
			}

			//bannedIPAddress.BannedIP = txtBannedIP.Text;
			newBannedIPAddress.IPAddress = txtBannedIP.Text.Trim();

			DateTime.TryParse(txtBannedUTC.Text, out var bannedTime);

			if (timeZone != null)
			{
				bannedTime = bannedTime.ToUtc(timeZone);
			}

			//bannedIPAddress.BannedUtc = bannedTime;
			//bannedIPAddress.BannedReason = txtBannedReason.Text;
			//bannedIPAddress.Save();

			newBannedIPAddress.BannedUTC = bannedTime;
			newBannedIPAddress.Reason = txtBannedReason.Text.Trim();

			if (rowID <= 0)
			{
				unitOfWork.BannedIPAddresses.Add(newBannedIPAddress);
			}

			unitOfWork.Complete();

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		private async void grdBannedIPAddresses_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			var grid = (GridView)sender;
			var rowID = (int)grid.DataKeys[e.RowIndex].Value;

			//BannedIPAddress.Delete(rowID);

			await unitOfWork.BannedIPAddresses.Remove(rowID);
			unitOfWork.Complete();

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		private void grdBannedIPAddresses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{
			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		private async void grdBannedIPAddresses_RowEditing(object sender, GridViewEditEventArgs e)
		{
			var grid = (GridView)sender;

			grid.EditIndex = e.NewEditIndex;

			await BindGrid();

			var btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");

			if (btnDelete != null)
			{
				btnDelete.Attributes.Add("OnClick", "return confirm('" + Resource.BannedIPAddressDeleteConfirmMessage + "');");
			}
		}


		//private void grdBannedIPAddresses_Sorting(object sender, GridViewSortEventArgs e)
		//{
		//	// TODO
		//	String redirectUrl = WebUtils.GetSiteRoot()
		//		+ "/YourPath/AdminBannedIPAddresses.aspx?pageid=" + PageID.ToString(CultureInfo.InvariantCulture)
		//		+ "&mid=" + ModuleID.ToString(CultureInfo.InvariantCulture)
		//		+ "&pagenumber"
		//		+ ModuleID.ToString(CultureInfo.InvariantCulture)
		//		+ "=" + pageNumber.ToString(CultureInfo.InvariantCulture)
		//		+ "&sort"
		//		+ ModuleID.ToString(CultureInfo.InvariantCulture)
		//		+ "=" + e.SortExpression;

		//	WebUtils.SetupRedirect(this, redirectUrl);
		//}


		#endregion


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			//grdBannedIPAddresses.Sorting += new GridViewSortEventHandler(grdBannedIPAddresses_Sorting);
			grdBannedIPAddresses.RowEditing += new GridViewEditEventHandler(grdBannedIPAddresses_RowEditing);
			grdBannedIPAddresses.RowCancelingEdit += new GridViewCancelEditEventHandler(grdBannedIPAddresses_RowCancelingEdit);
			grdBannedIPAddresses.RowUpdating += new GridViewUpdateEventHandler(grdBannedIPAddresses_RowUpdating);
			grdBannedIPAddresses.RowDeleting += new GridViewDeleteEventHandler(grdBannedIPAddresses_RowDeleting);
			btnAddNew.Click += new EventHandler(btnAddNew_Click);
			btnIPLookup.Click += new EventHandler(btnIPLookup_Click);

			SuppressMenuSelection();
			SuppressPageMenu();

			ScriptConfig.IncludeJQTable = true;
		}

		#endregion

	}
}
