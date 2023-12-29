using System;
using System.Data;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.UI;

public partial class AuthorizeNetLogList : UserControl
{
	private int pageId = -1;
	private int moduleId = -1;
	private int pageNumber = 1;
	private Guid storeGuid = Guid.Empty;
	private Guid cartGuid = Guid.Empty;

	public Guid StoreGuid
	{
		get { return storeGuid; }
		set { storeGuid = value; }
	}

	public Guid CartGuid
	{
		get { return cartGuid; }
		set { cartGuid = value; }
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		BindGrid();
	}

	private void BindGrid()
	{
		pgrCheckoutLog.Visible = false;

		if (cartGuid == Guid.Empty) { return; }

		using (IDataReader reader = AuthorizeNetLog.GetByCart(cartGuid))
		{
			grdCheckoutLog.DataSource = reader;
			grdCheckoutLog.DataBind();
		}

		if (grdCheckoutLog.Rows.Count == 0)
		{
			Visible = false;
		}
	}

	private void PopulateLabels()
	{
		litHeading.Text = Resource.AuthorizeNetLogHeading;
		grdCheckoutLog.Columns[0].HeaderText = Resource.AuthorizeNetLogTransactionType;
		grdCheckoutLog.Columns[1].HeaderText = Resource.AuthorizeNetLogTransactionId;
		grdCheckoutLog.Columns[2].HeaderText = Resource.AuthorizeNetLogMethod;
		grdCheckoutLog.Columns[3].HeaderText = Resource.AuthorizeNetLogResponseCode;
		grdCheckoutLog.Columns[4].HeaderText = Resource.AuthorizeNetLogReason;
		grdCheckoutLog.Columns[5].HeaderText = Resource.AuthorizeNetLogAuthCode;
		grdCheckoutLog.Columns[6].HeaderText = Resource.AuthorizeNetLogCreatedUtc;
	}

	private void LoadSettings()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
	}
}