namespace mojoPortal.Web.ForumUI;


public class ForumDisplaySettings : BasePluginDisplaySettings
{
	public ForumDisplaySettings() : base() { }

	public bool UseOldTableAttributes { get; set; } = true;
	public string ForumListCssClass { get; set; } = "forumlist";
	public string PostListCssClass { get; set; } = "postlist";
	public string ThreadListCssClass { get; set; } = "threadlist";
	public string UserThreadListCssClass { get; set; } = "threadlist userthreadlist";
	public bool UseAltForumList { get; set; } = false;
	public bool UseAltThreadList { get; set; } = false;
	public bool UseAltPostList { get; set; } = false;
	public bool UseAltUserThreadList { get; set; } = false;
	public bool HideHeadingOnThreadView { get; set; } = false;
	public bool HideCurrentCrumbOnThreadcrumbs { get; set; } = false;
	public bool HideNotificationLinkOnPostList { get; set; } = false;
	public bool HideForumDescriptionOnPostList { get; set; } = false;
	public bool UseBottomSearchOnForumList { get; set; } = false;
	public bool HideSearchOnForumList { get; set; } = false;
	public bool UseBottomSearchOnForumView { get; set; } = false;
	public bool HideSearchOnForumView { get; set; } = false;
	public bool ForumViewHideForumDescription { get; set; } = false;
	public bool ForumViewHideStartedBy { get; set; } = false;
	public bool ForumViewHideTotalViews { get; set; } = false;
	public bool ForumViewHideTotalReplies { get; set; } = false;
	public bool ForumViewHideLastPostDate { get; set; } = false;
	public bool ForumViewHideLastPostUser { get; set; } = false;
	public bool HideAvatars { get; set; } = false;
	public bool HideFeedLinks { get; set; } = false;
	public bool HideUserTotalPosts { get; set; } = false;
	public string OverrideThreadHeadingElement { get; set; } = string.Empty;
	public bool HideForumDescriptionOnPostEdit { get; set; } = false;
}