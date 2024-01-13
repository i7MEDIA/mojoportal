using System;
using System.Data;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.AdminUI;

public partial class RoleUserSelectDialog : mojoDialogBasePage
{
	private int roleId = -1;
	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 15;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		if (!IsPostBack)
		{
			PopulateControls();
		}
	}

	private void PopulateControls()
	{
		if (siteSettings is null) { return; }

		if (WebUser.IsAdminOrRoleAdmin)
		{
			pnlLookup.Visible = true;
			pnlNotAllowed.Visible = false;
			BindListForAdmin();
		}
		else
		{
			pnlLookup.Visible = false;
			pnlNotAllowed.Visible = true;
		}
	}

	private void BindListForAdmin()
	{
		using IDataReader reader = Role.GetUsersNotInRole(siteSettings.SiteId, roleId, pageNumber, pageSize, out totalPages);

		if (pageNumber > totalPages)
		{
			pageNumber = 1;

		}

		string pageUrl = $"{SiteRoot}/Dialog/RoleUserSelectDialog.aspx?r={roleId.ToInvariantString()}&amp;pagenumber={{0}}";

		pgrMembers.PageURLFormat = pageUrl;
		pgrMembers.ShowFirstLast = true;
		pgrMembers.CurrentIndex = pageNumber;
		pgrMembers.PageSize = pageSize;
		pgrMembers.PageCount = totalPages;
		pgrMembers.Visible = (totalPages > 1);

		rptUsers.DataSource = reader;
		rptUsers.DataBind();
	}

	void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		Button btnSelect = (Button)e.Item.FindControl("btnSelect");
		if (btnSelect is null) { return; }

		btnSelect.Attributes.Add("onclick", GetClientScriptForButton(btnSelect.CommandArgument));
	}

	private string GetClientScriptForButton(string userId)
	{
		return $"top.window.SelectUser({userId});  return false;";
	}


	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
		roleId = WebUtils.ParseInt32FromQueryString("r", roleId);
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		rptUsers.ItemDataBound += new RepeaterItemEventHandler(rptUsers_ItemDataBound);
	}
}