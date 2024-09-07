using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;
using System;
using System.Data;
using System.Web.UI;

namespace mojoPortal.Web.ForumUI;

public partial class ThreadListAlt : UserControl
{
	#region Properties

	protected double TimeOffset = 0;
	private TimeZoneInfo timeZone = null;
	private SiteSettings siteSettings = null;
	private string notificationUrl = string.Empty;
	protected bool isSubscribedToForum = false;
	private SiteUser currentUser = null;

	public Forum Forum { get; set; } = null;
	public int PageId { get; set; } = -1;
	public int ModuleId { get; set; } = -1;
	public int ItemId { get; set; } = -1;
	public int PageNumber { get; set; } = 1;
	public bool IsEditable { get; set; } = false;
	public ForumConfiguration Config { get; set; } = null;
	public string SiteRoot { get; set; } = string.Empty;
	public string NonSslSiteRoot { get; set; } = string.Empty;
	public string ImageSiteRoot { get; set; } = string.Empty;

	#endregion


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Visible)
		{
			return;
		}

		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}


	private void PopulateControls()
	{
		if (Forum == null)
		{
			return;
		}

		string pageUrl;

		if (ForumConfiguration.CombineUrlParams)
		{
			pageUrl = SiteRoot
				+ "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
				+ "&amp;f=" + Forum.ItemId.ToInvariantString()
				+ "~{0}";
		}
		else
		{
			pageUrl = SiteRoot
				+ "/Forums/ForumView.aspx?"
				+ "ItemID=" + Forum.ItemId.ToInvariantString()
				+ "&amp;mid=" + ModuleId.ToInvariantString()
				+ "&amp;pageid=" + PageId.ToInvariantString()
				+ "&amp;pagenumber={0}";
		}

		pgrTop.PageURLFormat = pageUrl;
		pgrTop.ShowFirstLast = true;
		pgrTop.CurrentIndex = PageNumber;
		pgrTop.PageSize = Forum.ThreadsPerPage;
		pgrTop.PageCount = Forum.TotalPages;
		pgrTop.Visible = (pgrTop.PageCount > 1);
		divPagerTop.Visible = pgrTop.Visible;

		pgrBottom.PageURLFormat = pageUrl;
		pgrBottom.ShowFirstLast = true;
		pgrBottom.CurrentIndex = PageNumber;
		pgrBottom.PageSize = Forum.ThreadsPerPage;
		pgrBottom.PageCount = Forum.TotalPages;
		pgrBottom.Visible = (pgrBottom.PageCount > 1);
		divPagerBottom.Visible = pgrBottom.Visible;

		lnkNewThread.HRef = SiteRoot
				+ "/Forums/EditPost.aspx?forumid=" + ItemId.ToInvariantString()
				+ "&amp;pageid=" + PageId.ToInvariantString()
				+ "&amp;mid=" + ModuleId.ToInvariantString();

		lnkNewThreadBottom.HRef = lnkNewThread.HRef;

		lnkNewThread.Visible = WebUser.IsInRoles(Forum.RolesThatCanPost) && !Forum.Closed;
		lnkNewThreadBottom.Visible = lnkNewThread.Visible;

		lnkLogin.Visible = !lnkNewThread.Visible && !Request.IsAuthenticated;

		using IDataReader reader = Forum.GetThreads(PageNumber);
		rptForums.DataSource = reader;

		DataBind();
	}


	protected string FormatUrl(int threadId)
	{
		if (ForumConfiguration.CombineUrlParams)
		{
			return SiteRoot + "/Forums/Thread.aspx?pageid="
			+ PageId.ToInvariantString()
			+ "&amp;t=" + ThreadParameterParser.FormatCombinedParam(threadId, 1);
		}

		return SiteRoot + "/Forums/Thread.aspx?pageid="
			+ PageId.ToInvariantString()
			+ "&amp;mid=" + ModuleId.ToInvariantString()
			+ "&amp;ItemID=" + ItemId.ToInvariantString()
			+ "&amp;thread=" + threadId.ToInvariantString()
			;
	}


	private void PopulateLabels()
	{
		lnkNewThread.InnerHtml = ForumResources.ForumViewNewThreadLabel;
		lnkNewThread.Title = ForumResources.ForumViewNewThreadLabel;
		lnkNewThreadBottom.InnerHtml = ForumResources.ForumViewNewThreadLabel;
		lnkNewThreadBottom.Title = ForumResources.ForumViewNewThreadLabel;
		lnkLogin.Text = ForumResources.ForumsLoginRequiredLink;
		pgrTop.NavigateToPageText = ForumResources.CutePagerNavigateToPageText;
		pgrTop.BackToFirstClause = ForumResources.CutePagerBackToFirstClause;
		pgrTop.GoToLastClause = ForumResources.CutePagerGoToLastClause;
		pgrTop.BackToPageClause = ForumResources.CutePagerBackToPageClause;
		pgrTop.NextToPageClause = ForumResources.CutePagerNextToPageClause;
		pgrTop.PageClause = ForumResources.CutePagerPageClause;
		pgrTop.OfClause = ForumResources.CutePagerOfClause;

		pgrBottom.NavigateToPageText = ForumResources.CutePagerNavigateToPageText;
		pgrBottom.BackToFirstClause = ForumResources.CutePagerBackToFirstClause;
		pgrBottom.GoToLastClause = ForumResources.CutePagerGoToLastClause;
		pgrBottom.BackToPageClause = ForumResources.CutePagerBackToPageClause;
		pgrBottom.NextToPageClause = ForumResources.CutePagerNextToPageClause;
		pgrBottom.PageClause = ForumResources.CutePagerPageClause;
		pgrBottom.OfClause = ForumResources.CutePagerOfClause;

		lnkNotify.ToolTip = ForumResources.SubscribeLink;
		lnkNotify2.Text = ForumResources.SubscribeLongLink;
	}


	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();
		TimeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		notificationUrl = SiteRoot + "/Forums/EditSubscriptions.aspx?mid="
			+ ModuleId.ToInvariantString()
			+ "&pageid=" + PageId.ToInvariantString() + "#forum" + ItemId.ToInvariantString();

		lnkNotify.ImageUrl = ImageSiteRoot + "/Data/SiteImages/email.png";
		lnkNotify.NavigateUrl = notificationUrl;
		lnkNotify2.NavigateUrl = notificationUrl;

		lnkLogin.NavigateUrl = PageUrlService.GetLoginLink(Request.RawUrl);

		if (Request.IsAuthenticated)
		{
			currentUser = SiteUtils.GetCurrentSiteUser();

			if (currentUser != null && ItemId > -1)
			{
				isSubscribedToForum = Forum.IsSubscribed(ItemId, currentUser.UserId);
			}

			if (!isSubscribedToForum)
			{
				pnlNotify.Visible = true;
			}
		}
	}


	protected string GetRowCssClass(int stickySort, bool isLocked)
	{
		if (isLocked)
		{
			return "lockedthreadrow";
		}

		if (stickySort < Forum.NormalThreadSort)
		{
			return "stickythreadrow";
		}

		return "normalthreadrow";
	}


	protected string GetFolderCssClass(int stickySort, bool isLocked)
	{
		if (isLocked)
		{
			return "lockedthread";
		}

		if (stickySort < Forum.NormalThreadSort)
		{
			return "stickythread";
		}

		return "normalthread";
	}


	protected string FormatDate(DateTime startDate)
	{
		if (timeZone != null)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString();
		}

		return startDate.AddHours(TimeOffset).ToString();
	}


	public bool GetPermission(object startedByUser)
	{
		// TODO: allow the user who started the thread to edit it?
		return IsEditable;
	}


	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}
}
