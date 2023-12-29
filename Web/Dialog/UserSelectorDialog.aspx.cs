using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public partial class UserSelectorDialog : mojoDialogBasePage
{
	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 15;
	private string userNameBeginsWith = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		if (!IsPostBack)
		{
			PopulateControls();
		}
	}

	private void PopulateControls()
	{
		if (siteSettings == null) { return; }

		if (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
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
		List<SiteUser> siteUserPage = SiteUser.GetUserSearchPage(
			SiteInfo.SiteId,
			pageNumber,
			pageSize,
			userNameBeginsWith,
			0,
			out totalPages);

		if (pageNumber > totalPages)
		{
			pageNumber = 1;

		}

		if (userNameBeginsWith.Length > 1)
		{
			txtSearchUser.Text = Server.HtmlEncode(userNameBeginsWith);
		}

		var topPageLinks = new Literal();
		string pageUrl = $"{SiteRoot}/Dialog/UserSelectorDialog.aspx?pagenumber=";

		string alphaChars;

		if (WebConfigSettings.GetAlphaPagerCharsFromResourceFile)
		{
			alphaChars = Resource.AlphaPagerChars;
		}
		else
		{
			alphaChars = WebConfigSettings.AlphaPagerChars;
		}

		topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
			pageUrl,
			pageNumber,
			alphaChars,
			userNameBeginsWith);

		spnTopPager.Controls.Add(topPageLinks);

		pageUrl = $"{SiteRoot}/Dialog/UserSelectorDialog.aspx?letter={userNameBeginsWith}&amp;pagenumber={{0}}";

		pgrMembers.PageURLFormat = pageUrl;
		pgrMembers.ShowFirstLast = true;
		pgrMembers.CurrentIndex = pageNumber;
		pgrMembers.PageSize = pageSize;
		pgrMembers.PageCount = totalPages;
		pgrMembers.Visible = totalPages > 1;

		rptUsers.DataSource = siteUserPage;

		DataBind();
	}

	void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		Button btnSelect = (Button)e.Item.FindControl("btnSelect");
		if (btnSelect == null) { return; }

		string[] args = btnSelect.CommandArgument.Split('|');
		if (args.Length < 3) { return; }
		if (args[0].Length != 36) { return; }

		btnSelect.Attributes.Add("onclick", GetClientScriptForButton(args[0], args[1], args[2]));
	}

	private string GetClientScriptForButton(string userGuid, string userId, string email)
	{
		return $"top.window.SelectUser('{userGuid}',{userId},'{email}'); return false;";
	}

	void btnSearchUser_Click(object sender, EventArgs e)
	{
		string pageUrl = $"{SiteRoot}/Dialog/UserSelectorDialog.aspx?letter={Server.HtmlEncode(txtSearchUser.Text)}&pagenumber=";

		WebUtils.SetupRedirect(this, pageUrl);
		return;
	}

	private void PopulateLabels()
	{
		lnkAllUsers.Text = Resource.MemberListAllUsersLink;
		lnkAllUsers.NavigateUrl = $"{SiteRoot}/Dialog/UserSelectorDialog.aspx";
		btnSearchUser.Text = Resource.MemberListSearchButton;
	}

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
		if (Request.Params["letter"] != null)
		{
			userNameBeginsWith = Request.Params["letter"];
		}
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnSearchUser.Click += new EventHandler(btnSearchUser_Click);
		rptUsers.ItemDataBound += new RepeaterItemEventHandler(rptUsers_ItemDataBound);
	}
}