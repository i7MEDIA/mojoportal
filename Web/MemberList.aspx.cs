using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class MemberList : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(MemberList));

	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 20;
	protected bool IsAdmin = false;
	protected bool canManageUsers = false;
	private string filterLetter = string.Empty;
	private string searchText = string.Empty;
	private string ipSearchText = string.Empty;
	private bool showLocked = false;
	private bool showUnApproved = false;

	protected bool ShowWebSiteColumn = false;
	protected bool ShowForumPostColumn = true;
	protected bool ShowEmailInMemberList = false;
	protected bool ShowUserIDInMemberList = false;
	protected bool ShowLoginNameInMemberList = false;
	protected bool ShowJoinDate = true;

	protected double timeOffset = 0;
	protected TimeZoneInfo timeZone = null;

	private bool allowView = false;

	protected string tableClassMarkup = string.Empty;
	protected string tableAttributes = string.Empty;

	private int sortMode = 0; // 0 = displayName Asc, 1 = JoinDate Desc, 2 = Last, First

	private Models.MemberListModel model;
	private List<SiteUser> siteUserPage;


	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		SuppressMenuSelection();

		if (Global.SkinConfig.MenuOptions.HideOnMemberList)
		{
			SuppressAllMenus();
		}

		else if (Global.SkinConfig.PageMenuOptions.HideOnMemberList)
		{
			SuppressPageMenu();
		}
	}

	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		SiteUtils.ForceSsl();

		LoadSettings();
		PopulateLabels();

		if (!allowView)
		{
			WebUtils.SetupRedirect(this, "~/Default.aspx".ToLinkBuilder().ToString());
			return;
		}

		if (!IsPostBack)
		{
			PopulateControls();
		}
	}


	private void PopulateControls()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

		if (Page.Header != null && CurrentPage.Url.Length > 0)
		{
			var link = new Literal
			{
				ID = "pageurl",
				Text = $"\n<link rel=\"canonical\" href=\"{SiteRoot}/MemberList.aspx\" />"
			};

			Page.Header.Controls.Add(link);
		}

		if (canManageUsers && showUnApproved)
		{
			BindNotApprovedUsers();
		}
		else if (canManageUsers && showLocked)
		{
			BindLockedUsers();
		}
		else if (searchText.Length > 0 || ipSearchText.Length > 0)
		{
			BindForSearch();
		}
		else
		{
			BindAlphaList();
		}

		if (pageNumber > totalPages)
		{
			pageNumber = 1;

		}

		PopulateModel();

		try
		{
			theLit.Text = RazorBridge.RenderPartialToString("MemberList", model, "Shared");
		}
		catch (HttpException ex)
		{
			log.Error($"layout (MemberList) was not found in skin {SiteUtils.DetermineSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
		}
	}


	private void BindAlphaList()
	{
		siteUserPage = SiteUser.GetPage(
			siteSettings.SiteId,
			pageNumber,
			pageSize,
			filterLetter,
			sortMode,
			sortMode == 2 ? "lastname" : "display",
			out totalPages
		);

		if (pageNumber > totalPages)
		{
			pageNumber = 1;

			siteUserPage = SiteUser.GetPage(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				filterLetter,
				sortMode,
				sortMode == 2 ? "lastname" : "display",
				out totalPages
			);
		}
	}


	private void BindForSearch()
	{
		if (canManageUsers)
		{
			if (ipSearchText.Length > 0)
			{
				siteUserPage = SiteUser.GetByIPAddress(siteSettings.SiteGuid, ipSearchText);
			}
			else
			{
				// admins can also search against email address
				siteUserPage = SiteUser.GetUserAdminSearchPage(
					siteSettings.SiteId,
					pageNumber,
					pageSize,
					searchText,
					sortMode,
					out totalPages
				);
			}
		}
		else
		{
			siteUserPage = SiteUser.GetUserSearchPage(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				searchText,
				sortMode,
				out totalPages
			);
		}
	}


	private void PopulateModel()
	{
		model = new Models.MemberListModel
		{
			Users = siteUserPage,
			DisplaySettings = displaySettings,

			PagerInfo = new Models.PagerInfo
			{
				CurrentIndex = pageNumber,
				PageSize = pageSize,
				PageCount = totalPages,
				LinkFormat = SiteRoot
					+ "/MemberList.aspx?pagenumber={0}"
					+ (sortMode == 0 ? "" : "&sd=" + sortMode.ToString())
					+ (string.IsNullOrWhiteSpace(filterLetter) ? "" : "&letter=" + filterLetter)
					+ (showLocked ? "&locked=true" : "")
					+ (string.IsNullOrWhiteSpace(searchText) ? "" : "&search=" + searchText)
					+ (showUnApproved ? "&needapproval=true" : "")
					+ (string.IsNullOrWhiteSpace(ipSearchText) ? "" : "&ips=" + ipSearchText)
			}
		};
	}

	void BindLockedUsers()
	{
		if (!canManageUsers)
		{
			return;
		}

		siteUserPage = SiteUser.GetPageLockedUsers(
			siteSettings.SiteId,
			pageNumber,
			pageSize,
			out totalPages
		);

		if (pageNumber > totalPages)
		{
			pageNumber = 1;
			siteUserPage = SiteUser.GetPageLockedUsers(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				out totalPages
			);
		}
	}

	void BindNotApprovedUsers()
	{
		if (!canManageUsers)
		{
			return;
		}

		siteUserPage = SiteUser.GetNotApprovedUsers(
			siteSettings.SiteId,
			pageNumber,
			pageSize,
			out totalPages
		);

		if ((pageNumber > 1) && (pageNumber > totalPages))
		{
			pageNumber = 1;
			siteUserPage = SiteUser.GetNotApprovedUsers(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				out totalPages
			);
		}

		log.Debug($"found {siteUserPage.Count} users not approved");
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);
		MetaDescription = string.Format(CultureInfo.InvariantCulture,
		Resource.MetaDescriptionMemberListFormat, siteSettings.SiteName);
	}

	private void LoadSettings()
	{
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();
		//lnkAllUsers.NavigateUrl = SiteRoot + "/MemberList.aspx";
		IsAdmin = WebUser.IsAdmin;
		canManageUsers = IsAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers);
		ShowEmailInMemberList = WebConfigSettings.ShowEmailInMemberList || displaySettings.ShowEmail;
		ShowUserIDInMemberList = WebConfigSettings.ShowUserIDInMemberList || displaySettings.ShowUserId;
		ShowLoginNameInMemberList = WebConfigSettings.ShowLoginNameInMemberList || displaySettings.ShowLoginName;
		ShowJoinDate = displaySettings.ShowJoinDate;

		// this can't be used in related site mode because we can't assume forum posts were in this site.
		//ShowForumPostColumn = WebConfigSettings.ShowForumPostsInMemberList && displaySettings.ShowForumPosts && !WebConfigSettings.UseRelatedSiteMode;

		allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);

		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

		sortMode = WebUtils.ParseInt32FromQueryString("sd", sortMode);

		if ((sortMode == 0) && (displaySettings.ShowFirstAndLastName))
		{
			sortMode = 2; // lastname, firstname
		}

		if (Request.Params["letter"] != null)
		{
			filterLetter = Request.Params["letter"].Trim();
		}

		if (Request.Params["search"] != null)
		{
			searchText = Request.Params["search"].Trim();
		}

		ipSearchText = WebUtils.ParseStringFromQueryString("ips", ipSearchText);
		showLocked = WebUtils.ParseBoolFromQueryString("locked", showLocked);
		showUnApproved = WebUtils.ParseBoolFromQueryString("needapproval", showUnApproved);

		pageSize = WebConfigSettings.MemberListPageSize;

		//this page has no content other than nav
		SiteUtils.AddNoIndexFollowMeta(Page);

		AddClassToBody("memberlist");
	}
}