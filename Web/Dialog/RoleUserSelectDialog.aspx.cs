using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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
		SetupScripts();
		if (!IsPostBack)
		{
			PopulateControls();
		}
	}

	private void SetupScripts()
	{
		var mojoPromptScriptName = "mojoDialog";

		if (!Page.ClientScript.IsStartupScriptRegistered(mojoPromptScriptName))
		{
			ScriptManager.RegisterStartupScript(
				Page,
				typeof(Page),
				mojoPromptScriptName,
				$"<script src=\"{ResolveUrl($"~/ClientScript/mojo-prompt.js?v={siteSettings.SkinVersion}")}\"></script>",
				false);
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

		var pageUrl = $"~/Dialog/RoleUserSelectDialog.aspx".ToLinkBuilder()
			.AddParam("r", roleId)
			.AddParam("pagenumber", "{0}")
			.ToString();

		pgrMembers.PageURLFormat = pageUrl;
		pgrMembers.ShowFirstLast = true;
		pgrMembers.CurrentIndex = pageNumber;
		pgrMembers.PageSize = pageSize;
		pgrMembers.PageCount = totalPages;
		pgrMembers.Visible = (totalPages > 1);

		rptUsers.DataSource = reader;
		rptUsers.DataBind();
	}

	private void RptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.FindControl("btnSelect") is not Button btnSelect)
		{
			return;
		}

		btnSelect.Attributes.Add("onclick", GetClientScriptForButton(btnSelect.CommandArgument));
	}

	private string GetClientScriptForButton(string userId)
	{
		return $$"""mojoPrompt('{{Resource.AddRolesToUserApprovalPromptMessage}}', (userPassword) => {if (userPassword) top.window.SelectUser({{userId}}, userPassword)}, '{{Resource.AddRoleToUseApprovalPromptTitle}}', 'password'); return false;""";
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
		rptUsers.ItemDataBound += new RepeaterItemEventHandler(RptUsers_ItemDataBound);
	}
}
