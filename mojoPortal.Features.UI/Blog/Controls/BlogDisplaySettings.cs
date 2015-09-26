// Author:					Joe Audette
// Created:				    2011-06-09
// Last Modified:			2012-12-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.BlogUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class BlogDisplaySettings : WebControl
    {

        private bool useBottomNavigation = false;

        public bool UseBottomNavigation
        {
            get { return useBottomNavigation; }
            set { useBottomNavigation = value; }
        }

        private bool hideCalendar = false;

        public bool HideCalendar
        {
            get { return hideCalendar; }
            set { hideCalendar = value; }
        }

        private bool hideFeedLinks = false;

        public bool HideFeedLinks
        {
            get { return hideFeedLinks; }
            set { hideFeedLinks = value; }
        }

        
        private bool hideTopSideBar = false;

        public bool HideTopSideBar
        {
            get { return hideTopSideBar; }
            set { hideTopSideBar = value; }
        }

        private bool hideBottomSideBar = false;

        public bool HideBottomSideBar
        {
            get { return hideBottomSideBar; }
            set { hideBottomSideBar = value; }
        }

        private bool blogViewUseBottomDate = false;

        public bool BlogViewUseBottomDate
        {
            get { return blogViewUseBottomDate; }
            set { blogViewUseBottomDate = value; }
        }

        private bool blogViewHideTopPager = false;

        public bool BlogViewHideTopPager
        {
            get { return blogViewHideTopPager; }
            set { blogViewHideTopPager = value; }
        }

        private bool blogViewHideBottomPager = false;

        public bool BlogViewHideBottomPager
        {
            get { return blogViewHideBottomPager; }
            set { blogViewHideBottomPager = value; }
        }

        private string blogViewInnerWrapElement = string.Empty;

        public string BlogViewInnerWrapElement
        {
            get { return blogViewInnerWrapElement; }
            set { blogViewInnerWrapElement = value; }
        }

        //private string blogViewOuterBodyExtraCss = string.Empty;

        //public string BlogViewOuterBodyExtraCss
        //{
        //    get { return blogViewOuterBodyExtraCss; }
        //    set { blogViewOuterBodyExtraCss = value; }
        //}

        private string blogViewInnerBodyExtraCss = string.Empty;

        public string BlogViewInnerBodyExtraCss
        {
            get { return blogViewInnerBodyExtraCss; }
            set { blogViewInnerBodyExtraCss = value; }
        }

        private string blogViewDivBlogExtraCss = string.Empty;

        public string BlogViewDivBlogExtraCss
        {
            get { return blogViewDivBlogExtraCss; }
            set { blogViewDivBlogExtraCss = value; }
        }

        private string blogViewHeaderLiteralTopContent = string.Empty;

        public string BlogViewHeaderLiteralTopContent
        {
            get { return blogViewHeaderLiteralTopContent; }
            set { blogViewHeaderLiteralTopContent = value; }
        }

        private string blogViewHeaderLiteralBottomContent = string.Empty;

        public string BlogViewHeaderLiteralBottomContent
        {
            get { return blogViewHeaderLiteralBottomContent; }
            set { blogViewHeaderLiteralBottomContent = value; }
        }


        private bool archiveViewHideFeedbackLink = false;

        public bool ArchiveViewHideFeedbackLink
        {
            get { return archiveViewHideFeedbackLink; }
            set { archiveViewHideFeedbackLink = value; }
        }

        private bool postListForceExcerptMode = false;

        public bool PostListForceExcerptMode
        {
            get { return postListForceExcerptMode; }
            set { postListForceExcerptMode = value; }
        }

        private bool postListForceTitleOnly = false;

        public bool PostListForceTitleOnly
        {
            get { return postListForceTitleOnly; }
            set { postListForceTitleOnly = value; }
        }

        private bool postListUseBottomDate = false;

        public bool PostListUseBottomDate
        {
            get { return postListUseBottomDate; }
            set { postListUseBottomDate = value; }
        }

        private bool postListHideDate = false;

        public bool PostListHideDate
        {
            get { return postListHideDate; }
            set { postListHideDate = value; }
        }
       
        private bool postListDisableContentRating = false;

        public bool PostListDisableContentRating
        {
            get { return postListDisableContentRating; }
            set { postListDisableContentRating = value; }
        }

        private bool categoryListForceTitleOnly = false;

        public bool CategoryListForceTitleOnly
        {
            get { return categoryListForceTitleOnly; }
            set { categoryListForceTitleOnly = value; }
        }

        private bool archiveListForceTitleOnly = false;

        public bool ArchiveListForceTitleOnly
        {
            get { return archiveListForceTitleOnly; }
            set { archiveListForceTitleOnly = value; }
        }

        private bool headlineImageAboveExcerpt = true;

        public bool HeadlineImageAboveExcerpt
        {
            get { return headlineImageAboveExcerpt; }
            set { headlineImageAboveExcerpt = value; }
        }

        private bool detailViewDisableContentRating = false;

        public bool DetailViewDisableContentRating
        {
            get { return detailViewDisableContentRating; }
            set { detailViewDisableContentRating = value; }
        }

        private int postListOverridePageSize = 0; // only used if greater than 0

        public int PostListOverridePageSize
        {
            get { return postListOverridePageSize; }
            set { postListOverridePageSize = value; }
        }

        private int categoryListOverridePageSize = 0; // only used if greater than 0

        public int CategoryListOverridePageSize
        {
            get { return categoryListOverridePageSize; }
            set { categoryListOverridePageSize = value; }
        }

        private int archiveListOverridePageSize = 0; // only used if greater than 0

        public int ArchiveListOverridePageSize
        {
            get { return archiveListOverridePageSize; }
            set { archiveListOverridePageSize = value; }
        }

        private bool disableShowCategories = false;

        public bool DisableShowCategories
        {
            get { return disableShowCategories; }
            set { disableShowCategories = value; }
        }

        private bool disableShowArchives = false;

        public bool DisableShowArchives
        {
            get { return disableShowArchives; }
            set { disableShowArchives = value; }
        }

        private bool disableShowStatistics = false;

        public bool DisableShowStatistics
        {
            get { return disableShowStatistics; }
            set { disableShowStatistics = value; }
        }

        private bool usejQueryCalendarNavigation = false;

        public bool UsejQueryCalendarNavigation
        {
            get { return usejQueryCalendarNavigation; }
            set { usejQueryCalendarNavigation = value; }
        }


        private bool useBottomContentRating = false;

        public bool UseBottomContentRating
        {
            get { return useBottomContentRating; }
            set { useBottomContentRating = value; }
        }

        private string overridePostDetailHeadingElement = string.Empty;

        public string OverridePostDetailHeadingElement
        {
            get { return overridePostDetailHeadingElement; }
            set { overridePostDetailHeadingElement = value; }
        }

        private string overrideListItemHeadingElement = string.Empty;

        public string OverrideListItemHeadingElement
        {
            get { return overrideListItemHeadingElement; }
            set { overrideListItemHeadingElement = value; }
        }

        private string categoryListHeadingElement = "h3";

        public string CategoryListHeadingElement
        {
            get { return categoryListHeadingElement; }
            set { categoryListHeadingElement = value; }
        }

        
        private string overrideCategoryListItemHeadingElement = string.Empty;

        public string OverrideCategoryListItemHeadingElement
        {
            get { return overrideCategoryListItemHeadingElement; }
            set { overrideCategoryListItemHeadingElement = value; }
        }

        private string overrideArchiveListItemHeadingElement = string.Empty;

        public string OverrideArchiveListItemHeadingElement
        {
            get { return overrideArchiveListItemHeadingElement; }
            set { overrideArchiveListItemHeadingElement = value; }
        }

        private string overrideCategoryLabel = string.Empty;

        public string OverrideCategoryLabel
        {
            get { return overrideCategoryLabel; }
            set { overrideCategoryLabel = value; }
        }

        private string overrideCategoryPrefixLabel = string.Empty;
        /// <summary>
        /// used to override the prefix in the heading and page title on ViewCategory.aspx
        /// </summary>
        public string OverrideCategoryPrefixLabel
        {
            get { return overrideCategoryPrefixLabel; }
            set { overrideCategoryPrefixLabel = value; }
        }

        private string overridePostCategoriesLabel = string.Empty;

        public string OverridePostCategoriesLabel
        {
            get { return overridePostCategoriesLabel; }
            set { overridePostCategoriesLabel = value; }
        }

        private string overrideDateFormat = string.Empty;

        public string OverrideDateFormat
        {
            get { return overrideDateFormat; }
            set {
                try
                {
                    string d = DateTime.Now.ToString(value, CultureInfo.CurrentCulture);
                    overrideDateFormat = value;
                }
                catch (FormatException) { }
                 
            }
        }

        private bool showTagsOnPostList = true;

        public bool ShowTagsOnPostList
        {
            get { return showTagsOnPostList; }
            set { showTagsOnPostList = value; }
        }

        private bool showTagsOnPost = true;

        public bool ShowTagsOnPost
        {
            get { return showTagsOnPost; }
            set { showTagsOnPost = value; }
        }

        private bool hideSearchBoxInPostList = false;

        public bool HideSearchBoxInPostList
        {
            get { return hideSearchBoxInPostList; }
            set { hideSearchBoxInPostList = value; }
        }

        private bool hideSearchBoxInPostDetail = false;

        public bool HideSearchBoxInPostDetail
        {
            get { return hideSearchBoxInPostDetail; }
            set { hideSearchBoxInPostDetail = value; }
        }

        private string postListExtraCss = string.Empty;

        public string PostListExtraCss
        {
            get { return postListExtraCss; }
            set { postListExtraCss = value; }
        }

        private string overrideRssFeedImageUrl = string.Empty;

        public string OverrideRssFeedImageUrl
        {
            get { return overrideRssFeedImageUrl; }
            set { overrideRssFeedImageUrl = value; }
        }

        private string msnSubscribeIconUrl = "~/Data/SiteImages/rss_mymsn.gif";

        public string MsnSubscribeIconUrl
        {
            get { return msnSubscribeIconUrl; }
            set { msnSubscribeIconUrl = value; }
        }

        private string liveSubscribeIcon = "~/Data/SiteImages/addtolive.gif";

        public string LiveSubscribeIcon
        {
            get { return liveSubscribeIcon; }
            set { liveSubscribeIcon = value; }
        }

        private string myYahooSubscribeIcon = "~/Data/SiteImages/addtomyyahoo2.gif";
        public string MyYahooSubscribeIcon
        {
            get { return myYahooSubscribeIcon; }
            set { myYahooSubscribeIcon = value; }
        }

        private string googleSubscribeIcon = "~/Data/SiteImages/googleaddrss.gif";

        public string GoogleSubscribeIcon
        {
            get { return googleSubscribeIcon; }
            set { googleSubscribeIcon = value; }
        }

        private string relatedPostsHeadingElement = "h3";

        public string RelatedPostsHeadingElement
        {
            get { return relatedPostsHeadingElement; }
            set { relatedPostsHeadingElement = value; }
        }

        private string commentItemHeaderElement = "h4";

        public string CommentItemHeaderElement
        {
            get { return commentItemHeaderElement; }
            set { commentItemHeaderElement = value; }
        }

        private bool alwaysShowSignInPromptForCommentsIfNotAuthenticated = false;

        public bool AlwaysShowSignInPromptForCommentsIfNotAuthenticated
        {
            get { return alwaysShowSignInPromptForCommentsIfNotAuthenticated; }
            set { alwaysShowSignInPromptForCommentsIfNotAuthenticated = value; }
        }

        private bool showJanrainWidetOnCommentSignInPrompt = false;

        public bool ShowJanrainWidetOnCommentSignInPrompt
        {
            get { return showJanrainWidetOnCommentSignInPrompt; }
            set { showJanrainWidetOnCommentSignInPrompt = value; }
        }

        private string relatedPostsOverrideHeadingText = string.Empty;

        public string RelatedPostsOverrideHeadingText
        {
            get { return relatedPostsOverrideHeadingText; }
            set { relatedPostsOverrideHeadingText = value; }
        }

        private string statsHeadingElement = "h3";

        public string StatsHeadingElement
        {
            get { return statsHeadingElement; }
            set { statsHeadingElement = value; }
        }

        private string statsOverrideHeadingText = string.Empty;

        public string StatsOverrideHeadingText
        {
            get { return statsOverrideHeadingText; }
            set { statsOverrideHeadingText = value; }
        }

        private string archiveListHeadingElement = "h3";

        public string ArchiveListHeadingElement
        {
            get { return archiveListHeadingElement; }
            set { archiveListHeadingElement = value; }
        }

        private string archiveListOverrideHeadingText = string.Empty;

        public string ArchiveListOverrideHeadingText
        {
            get { return archiveListOverrideHeadingText; }
            set { archiveListOverrideHeadingText = value; }
        }

        private string relatedPostsPosition = "Side";
        /// <summary>
        /// Side or Bottom
        /// </summary>
        public string RelatedPostsPosition
        {
            get { return relatedPostsPosition; }
            set { relatedPostsPosition = value; }
        }

        private bool linkAuthorAvatarToProfile = true;

        public bool LinkAuthorAvatarToProfile
        {
            get { return linkAuthorAvatarToProfile; }
            set { linkAuthorAvatarToProfile = value; }
        }

        private string avatarUserNameTooltipFormat = "View User Profile for {0}";

        public string AvatarUserNameTooltipFormat
        {
            get { return avatarUserNameTooltipFormat; }
            set { avatarUserNameTooltipFormat = value; }
        }

        private bool showAvatarWithExcerpt = false;

        public bool ShowAvatarWithExcerpt
        {
            get { return showAvatarWithExcerpt; }
            set { showAvatarWithExcerpt = value; }
        }

        private bool hideAvatarInPostList = false;

        public bool HideAvatarInPostList
        {
            get { return hideAvatarInPostList; }
            set { hideAvatarInPostList = value; }
        }

        private bool hideAvatarInPostDetail = false;

        public bool HideAvatarInPostDetail
        {
            get { return hideAvatarInPostDetail; }
            set { hideAvatarInPostDetail = value; }
        }

        private bool showAuthorBioWithExcerpt = false;

        public bool ShowAuthorBioWithExcerpt
        {
            get { return showAuthorBioWithExcerpt; }
            set { showAuthorBioWithExcerpt = value; }
        }

        private bool showAuthorBioInPostList = true;

        public bool ShowAuthorBioInPostList
        {
            get { return showAuthorBioInPostList; }
            set { showAuthorBioInPostList = value; }
        }

        private bool showAuthorBioInPostDetail = true;

        public bool ShowAuthorBioInPostDetail
        {
            get { return showAuthorBioInPostDetail; }
            set { showAuthorBioInPostDetail = value; }
        }

        private bool showArchivesInPostDetail = false;

        public bool ShowArchivesInPostDetail
        {
            get { return showArchivesInPostDetail; }
            set { showArchivesInPostDetail = value; }
        }

        private bool showCategoriesInPostDetail = false;

        public bool ShowCategoriesInPostDetail
        {
            get { return showCategoriesInPostDetail; }
            set { showCategoriesInPostDetail = value; }
        }

        private bool showFeedLinksInPostDetail = false;

        public bool ShowFeedLinksInPostDetail
        {
            get { return showFeedLinksInPostDetail; }
            set { showFeedLinksInPostDetail = value; }
        }

        private bool showStatisticsInPostDetail = false;

        public bool ShowStatisticsInPostDetail
        {
            get { return showStatisticsInPostDetail; }
            set { showStatisticsInPostDetail = value; }
        }

        private int closedPostsPageSize = 10;
        public int ClosedPostsPageSize
        {
            get { return closedPostsPageSize; }
            set { closedPostsPageSize = value; }
        }

        private int draftPostsPageSize = 10;
        public int DraftPostsPageSize
        {
            get { return draftPostsPageSize; }
            set { draftPostsPageSize = value; }
        }

        private bool showSubTitleOnDetailPage = true;

        public bool ShowSubTitleOnDetailPage
        {
            get { return showSubTitleOnDetailPage; }
            set { showSubTitleOnDetailPage = value; }
        }

        private bool showSubTitleOnList = true;

        public bool ShowSubTitleOnList
        {
            get { return showSubTitleOnList; }
            set { showSubTitleOnList = value; }
        }

        private string postDetailSubTitleElement = "h3";

        public string PostDetailSubTitleElement
        {
            get { return postDetailSubTitleElement; }
            set { postDetailSubTitleElement = value; }
        }

        private string listItemSubtitleElement = "span";

        public string ListItemSubtitleElement
        {
            get { return listItemSubtitleElement; }
            set { listItemSubtitleElement = value; }
        }


        private bool showSearchInNav = false;

        public bool ShowSearchInNav
        {
            get { return showSearchInNav; }
            set { showSearchInNav = value; }
        }
        

        private string excerptImageFormat = "<img class='floatpanel bheadlineimg' alt=' ' src='{0}' />";

        public string ExcerptImageFormat
        {
            get { return excerptImageFormat; }
            set { excerptImageFormat = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}