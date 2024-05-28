using System;
using System.Globalization;
namespace mojoPortal.Web.BlogUI;


public class BlogDisplaySettings : BasePluginDisplaySettings
{
	public BlogDisplaySettings() : base() { }

	public bool UseBottomNavigation { get; set; } = false;
	public bool HideCalendar { get; set; } = false;
	public bool HideFeedLinks { get; set; } = false;
	public bool HideTopSideBar { get; set; } = false;
	public bool HideBottomSideBar { get; set; } = false;
	public bool BlogViewUseBottomDate { get; set; } = false;
	public bool BlogViewHideTopPager { get; set; } = false;
	public bool BlogViewHideBottomPager { get; set; } = false;
	public string BlogViewInnerWrapElement { get; set; } = string.Empty;
	public string BlogViewInnerBodyExtraCss { get; set; } = string.Empty;
	public string BlogViewDivBlogExtraCss { get; set; } = string.Empty;
	public string BlogViewHeaderLiteralTopContent { get; set; } = string.Empty;
	public string BlogViewHeaderLiteralBottomContent { get; set; } = string.Empty;
	public bool ArchiveViewHideFeedbackLink { get; set; } = false;
	public bool PostListForceExcerptMode { get; set; } = false;
	public bool PostListForceTitleOnly { get; set; } = false;
	public bool PostListUseBottomDate { get; set; } = false;
	public bool PostListHideDate { get; set; } = false;
	public bool PostListDisableContentRating { get; set; } = false;
	public bool CategoryListForceTitleOnly { get; set; } = false;
	public bool ArchiveListForceTitleOnly { get; set; } = false;
	public bool FeaturedImageAbovePost { get; set; } = true;
	public string FeaturedPostClass { get; set; } = "blog-list-view__post--featured";
	public string ListViewPostClass { get; set; } = "blogitem";
	public string ListViewPostTitleClass { get; set; } = "blogtitle";
	public string ListViewPostLinkClass { get; set; } = "blogitemtitle";
	public string ListViewPostBodyClass { get; set; } = "blogtext";
	public string PostViewPostBodyClass { get; set; } = "blogtext";
	public bool ListViewRenderPostPanel { get; set; } = true;
	public bool PostViewRenderPostPanel { get; set; } = true;
	public string AuthorInfoPanelClass { get; set; } = "avatarwrap authorinfo";
	public string AuthorBioClass { get; set; } = "authorbio";
	public string DatePanelClass { get; set; } = "blogdate";
	public string DateTopPanelClass { get; set; } = "blog-list-view__post-date--top";
	public string DateBottomPanelClass { get; set; } = "blog-list-view__post-date--bottom";
	public string SocialPanelClass { get; set; } = "bsocial";
	public string CommentLinkClass { get; set; } = "blogcommentlink";
	public string PagerPanelClass { get; set; } = "blogpager";
	public string CopyrightPanelClass { get; set; } = "blogcopyright";
	public string LayoutRowClass { get; set; } = string.Empty;
	public bool LayoutRowRender { get; set; } = false;
	public string ListViewCenterClass { get; set; } = "blog-center";
	public string ListViewCenterNoNavClass { get; set; } = "blogcenter-nonav";
	public string ListViewCenterRightNavClass { get; set; } = "blogcenter-rightnav";
	public string ListViewCenterLeftNavClass { get; set; } = "blogcenter-leftnav";
	public string PostViewCenterClass { get; set; } = "blog-center";
	public string PostViewCenterNoNavClass { get; set; } = "blogcenter-nonav";
	public string PostViewCenterRightNavClass { get; set; } = "blogcenter-rightnav";
	public string PostViewCenterLeftNavClass { get; set; } = "blogcenter-leftnav";
	public string NavClass { get; set; } = "blog-nav";
	public string NavRightClass { get; set; } = "blognavright";
	public string NavLeftClass { get; set; } = "blognavleft";
	public bool DetailViewDisableContentRating { get; set; } = false;
	public int PostListOverridePageSize { get; set; } = 0;
	public int CategoryListOverridePageSize { get; set; } = 0;
	public int ArchiveListOverridePageSize { get; set; } = 0;
	public bool DisableShowCategories { get; set; } = false;
	public bool DisableShowArchives { get; set; } = false;
	public bool DisableShowStatistics { get; set; } = false;
	public bool UsejQueryCalendarNavigation { get; set; } = false;
	public bool UseBottomContentRating { get; set; } = false;
	public string OverridePostDetailHeadingElement { get; set; } = string.Empty;
	public string OverrideListItemHeadingElement { get; set; } = string.Empty;
	public string CategoryListHeadingElement { get; set; } = "h3";
	public string CategoryListHeadingClass { get; set; } = string.Empty;
	public string CategoryListClass { get; set; } = "blognav";
	public string OverrideCategoryListItemHeadingElement { get; set; } = string.Empty;
	public string OverrideArchiveListItemHeadingElement { get; set; } = string.Empty;
	public string OverrideCategoryLabel { get; set; } = string.Empty;
	/// <summary>
	/// used to override the prefix in the heading and page title on ViewCategory.aspx
	/// </summary>
	public string OverrideCategoryPrefixLabel { get; set; } = string.Empty;
	public string OverridePostCategoriesLabel { get; set; } = string.Empty;
	private string overrideDateFormat = string.Empty;
	public string OverrideDateFormat
	{
		get { return overrideDateFormat; }
		set
		{
			try
			{
				string d = DateTime.Now.ToString(value, CultureInfo.CurrentCulture);
				overrideDateFormat = value;
			}
			catch (FormatException) { }
		}
	}

	public bool ShowTagsOnPostList { get; set; } = true;
	public bool ShowTagsOnPost { get; set; } = true;
	public bool HideSearchBoxInPostList { get; set; } = false;
	public bool HideSearchBoxInPostDetail { get; set; } = false;
	public string PostListExtraCss { get; set; } = string.Empty;
	public string OverrideRssFeedImageUrl { get; set; } = string.Empty;
	public string RssFeedLinkFormat { get; set; } = "<a href='{0}' class='rsslink' rel='nofollow' title='{1}'><img src='{2}' alt='{3}'></a>";
	public string RelatedPostsHeadingElement { get; set; } = "h3";
	public string CommentItemHeaderFormat { get; set; } = "<h4>{0}</h4>";
	public bool AlwaysShowSignInPromptForCommentsIfNotAuthenticated { get; set; } = false;
	public bool ShowJanrainWidetOnCommentSignInPrompt { get; set; } = false;
	public string RelatedPostsOverrideHeadingText { get; set; } = string.Empty;
	public string StatsHeadingElement { get; set; } = "h3";
	public string StatsOverrideHeadingText { get; set; } = string.Empty;
	public string ArchiveListHeadingElement { get; set; } = "h3";
	public string ArchiveListOverrideHeadingText { get; set; } = string.Empty;

	/// <summary>
	/// Side or Bottom
	/// </summary>
	public string RelatedPostsPosition { get; set; } = "Side";
	public bool LinkAuthorAvatarToProfile { get; set; } = true;
	public string AvatarUserNameTooltipFormat { get; set; } = "View User Profile for {0}";
	public bool ShowAvatarWithExcerpt { get; set; } = false;
	public bool HideAvatarInPostList { get; set; } = false;
	public bool HideAvatarInPostDetail { get; set; } = false;
	public bool ShowAuthorBioWithExcerpt { get; set; } = false;
	public bool ShowAuthorBioInPostList { get; set; } = true;
	public bool ShowAuthorBioInPostDetail { get; set; } = true;
	public string AvatarCssClass { get; set; } = string.Empty;
	public string AvatarExtraCssClass { get; set; } = string.Empty;
	public bool ShowArchivesInPostDetail { get; set; } = false;
	public bool ShowCategoriesInPostDetail { get; set; } = false;
	public bool ShowFeedLinksInPostDetail { get; set; } = false;
	public bool ShowStatisticsInPostDetail { get; set; } = false;
	public int ClosedPostsPageSize { get; set; } = 10;
	public int DraftPostsPageSize { get; set; } = 10;
	public bool ShowSubTitleOnDetailPage { get; set; } = true;
	public bool ShowSubTitleOnList { get; set; } = true;
	public string PostDetailSubtitleElement { get; set; } = "h3";
	public string PostDetailSubtitleClass { get; set; } = "subtitle";
	public string ListViewItemSubtitleElement { get; set; } = "span";
	public string ListViewItemSubtitleClass { get; set; } = "subtitle";
	public bool ShowSearchInNav { get; set; } = false;
	public string FeaturedImageFormat { get; set; } = "<figure class='blog-post__featured-image-figure'><img class='blog-post__featured-image' alt='{1}' src='{0}' /></figure>";
}