// Author:					
// Created:				    2011-06-13
// Last Modified:			2014-07-14
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
using System.Data;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class PostListAlt : UserControl
    {
        #region Properties

        private ForumConfiguration config = null;
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;
        //Gravatar public enum RatingType { G, PG, R, X }
        protected mojoPortal.Web.UI.Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        protected bool allowGravatars = false;
        protected string gravatarLinkUrl = "http://www.gravatar.com";
        protected bool disableAvatars = true;
        protected bool filterContentFromTrustedUsers = false;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected bool showUserRevenue = false;
        protected string notificationUrl = string.Empty;
        protected string UserNameTooltipFormat = "View User Profile for {0}";

        private Forum forum = null;

        public Forum Forum
        {
            get { return forum; }
            set { forum = value; }
        }

        private ForumThread thread = null;

        public ForumThread Thread
        {
            get { return thread; }
            set { thread = value; }
        }

        private SiteSettings siteSettings = null;

        public SiteSettings SiteSettings
        {
            get { return siteSettings; }
            set { siteSettings = value; }
        }

        private bool isAdmin = false;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

        private bool isCommerceReportViewer = false;

        public bool IsCommerceReportViewer
        {
            get { return isCommerceReportViewer; }
            set { isCommerceReportViewer = value; }
        }

        private bool isSubscribedToForum = false;

        public bool IsSubscribedToForum
        {
            get { return isSubscribedToForum; }
            set { isSubscribedToForum = value; }
        }

        private int userId = -1;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private int itemId = -1;

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private int threadId = -1;

        public int ThreadId
        {
            get { return threadId; }
            set { threadId = value; }
        }

        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        private int nextPageNumber = 0;

        public int NextPageNumber
        {
            get { return nextPageNumber; }
            set { nextPageNumber = value; }
        }

        private bool isEditable = false;

        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        protected bool IsModerator = false;

        public ForumConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        private string siteRoot = string.Empty;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private string imageSiteRoot = string.Empty;

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        private bool useReverseSort = false;

        public bool UseReverseSort
        {
            get { return useReverseSort; }
            set { useReverseSort = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (forum == null) { return; }
            if (thread == null) { return; }



            if ((thread.TotalReplies + 1) == forum.PostsPerPage)
            {
                nextPageNumber = PageNumber + 1;
            }
            else
            {
                nextPageNumber = PageNumber;
            }

            string pageUrl;
            string viewAllUrl = string.Empty;

            if (ForumConfiguration.CombineUrlParams)
            {
                pageUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;t=" + threadId.ToInvariantString()
                    + "~{0}";

                viewAllUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;t=" + threadId.ToInvariantString()
                    + "~-1";
            }
            else
            {
                pageUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid="
                    + PageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;ItemID=" + ItemId.ToInvariantString()
                    + "&amp;thread=" + threadId.ToInvariantString()
                    + "&amp;pagenumber={0}";
            }

            if (ForumConfiguration.ShowPagerViewAllLink)
            {
                pgrTop.ViewAllUrl = viewAllUrl;
                pgrBottom.ViewAllUrl = viewAllUrl;  
            }
            pgrTop.PageURLFormat = pageUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = PageNumber;
            pgrTop.PageSize = forum.ThreadsPerPage;
            pgrTop.PageCount = thread.TotalPages;
            pgrTop.Visible = (pgrTop.PageCount > 1);
            divPagerTop.Visible = pgrTop.Visible;

            pgrBottom.PageURLFormat = pageUrl;            
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = PageNumber;
            pgrBottom.PageSize = forum.ThreadsPerPage;
            pgrBottom.PageCount = thread.TotalPages;
            pgrBottom.Visible = (pgrBottom.PageCount > 1);
            divPagerBottom.Visible = pgrBottom.Visible;

            
            lnkNewPost.InnerHtml = ForumResources.ForumThreadViewReplyLabel;

            lnkNewPost.HRef = SiteRoot
                + "/Forums/EditPost.aspx?"
                + "thread=" + threadId.ToString()
                + "&amp;forumid=" + forum.ItemId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToString()
                + "&amp;pagenumber=" + nextPageNumber.ToInvariantString();

            lnkNewPost.Visible = WebUser.IsInRoles(forum.RolesThatCanPost) && !forum.Closed;
            lnkNewPostBottom.Visible = lnkNewPost.Visible;
            lnkNewPostBottom.InnerHtml = ForumResources.ForumThreadViewReplyLabel;
            lnkNewPostBottom.HRef = lnkNewPost.HRef;

            lnkLogin.Visible = !lnkNewPost.Visible && !Request.IsAuthenticated;
            lnkLoginBottom.Visible = lnkLogin.Visible;

            if ((thread.IsLocked || forum.Closed || thread.IsClosed(config.CloseThreadsOlderThanDays)) && (!isEditable))
            {
                lnkNewPost.Visible = false;
                lnkNewPostBottom.Visible = false;
                lblClosedTop.Visible = true;
                lblClosedBottom.Visible = true;
            }

            //lnkLogin.NavigateUrl = SiteRoot + "/Secure/Login.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);
            lnkLogin.Text = ForumResources.ForumsLoginRequiredLink;

            //lnkLoginBottom.NavigateUrl = SiteRoot + "/Secure/Login.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);
            lnkLoginBottom.Text = ForumResources.ForumsLoginRequiredLink;

            if (useReverseSort)
            {
                using (IDataReader reader = thread.GetPostsReverseSorted())
                {
                    this.rptMessages.DataSource = reader;
                    this.rptMessages.DataBind();

                }

                pgrTop.Visible = false;
                pgrBottom.Visible = false;
                lnkNewPost.Visible = false;
                lnkNewPostBottom.Visible = false;
            }
            else
            {
                if (PageNumber == -1)
                {
                    using (IDataReader reader = thread.GetPosts())
                    {
                        rptMessages.DataSource = reader;
                        rptMessages.DataBind();
                    }

                    if (thread.TotalPages <= 1)
                    {
                        divPagerTop.Visible = false;
                        divPagerBottom.Visible = false;
                    }
                    
                }
                else
                {
                    using (IDataReader reader = thread.GetPosts(PageNumber))
                    {
                        rptMessages.DataSource = reader;
                        rptMessages.DataBind();
                    }
                }

                if (
                (rptMessages.Items.Count == 0)
                && (ItemId > -1)
                )
                {
                    // when the last post in a thread is deleted
                    // the ForumPostEditPage redirects to this page
                    // but it will hit this code and go back to the forum instead of showing 
                    // the empty thread
                    string redirectUrl;

                    if (ForumConfiguration.CombineUrlParams)
                    {
                        if (pageNumber > 1)
                        {   // try going back to page 1
                            redirectUrl = SiteRoot
                                + "/Forums/Thread.aspx?pageid=" + PageId.ToInvariantString()
                                + "&t=" + threadId.ToInvariantString() + "~1";
                        }
                        else
                        {
                            redirectUrl = SiteRoot
                                + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                                + "&f=" + ItemId.ToInvariantString() + "~1";
                        }
                    }
                    else
                    {
                        redirectUrl = SiteRoot
                            + "/Forums/ForumView.aspx?"
                            + "ItemID=" + ItemId.ToInvariantString()
                            + "&pageid=" + PageId.ToInvariantString()
                            + "&mid=" + moduleId.ToInvariantString();
                    }

                    WebUtils.SetupRedirect(this, redirectUrl);
                }
                else
                {
                    thread.UpdateThreadViewStats();
                }

            }

        }

        protected string FormatDate(DateTime startDate)
        {
            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString();

            }

            return startDate.AddHours(TimeOffset).ToString();

        }

        protected string GetLink(object url)
        {
            string sUrl = url.ToString();
            string result = string.Empty;
            if (sUrl.Length > 0)
            {
                result = "<a  href='" + sUrl + "'>" + sUrl + "</a>";
            }


            return result;

        }

        protected string GetProfileLinkOrLabel(int userId, string userName)
        {

            if (userName.Length == 0) { return string.Empty; }
            // user profile follows the same view rules as member list
            
            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList)) 
                { 
                    return string.Format(CultureInfo.InvariantCulture, ForumResources.PostedByFormat, userName); 
                }
            }

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return string.Format(CultureInfo.InvariantCulture, ForumResources.PostedByFormat,"<a  href='" + SiteRoot + "/ProfileView.aspx?userid=" + userId.ToInvariantString() + "'>" + userName + "</a>");

        }

        protected bool UseProfileLink()
        {
            if ((allowGravatars) && (!config.LinkGravatarToUserProfile))
            {
                return false;
            }

            if (Request.IsAuthenticated)
            {
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList))
                {
                    return false;
                }
            }

            // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
            return true;
        }

        protected string GetInternalAvatarUrl(int userId, string avatar, string userName)
        {
            if (allowGravatars) { return string.Empty; }
            if (disableAvatars) { return string.Empty; }

            // if we get here we are using our own avatars
            if (string.IsNullOrEmpty(avatar))
            {
                return Page.ResolveUrl("~/Data/SitesImages/1x1.gif");
            }

            return Page.ResolveUrl("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/" + avatar);

        }

        

        protected string GetGravatarLinkTitle(string userName)
        {
            if (config.LinkGravatarToUserProfile)
            {
                return string.Format(CultureInfo.InvariantCulture, ForumResources.UserProfileLinkTooltipFormat, Server.HtmlEncode(userName));
            }

            return ForumResources.GravatarLinkTitle;
        }

        protected string GetGravatarLinkUrl(int userId)
        {
            if (config.LinkGravatarToUserProfile)
            {
                // user profile follows the same view rules as member list
                if (Request.IsAuthenticated)
                {
                    // if we know the user is signed in and not in a role allowed then return the gravatar link
                    if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList))
                    {
                        return gravatarLinkUrl;
                    }
                }

                // if user is not authenticated we don't know if he will be allowed but he will be prompted to login first so its ok to show the link
                return SiteRoot + "/ProfileView.aspx?userid=" + userId.ToInvariantString();
            }

            return gravatarLinkUrl;
        }


        protected bool GetPermission(int postUserId, bool isLocked, DateTime postDate)
        {

            if (isEditable) { return true; }
            if (isLocked) { return false; }
            if (config.AllowEditingPostsLessThanMinutesOld != -1)
            {
                if (postDate < DateTime.UtcNow.AddMinutes(-config.AllowEditingPostsLessThanMinutesOld)) { return false; }
            }
            if ((postUserId == userId) && (postUserId > -1)) { return true; }

            return false;
        }


        private void LoadSettings()
        {
            if (siteSettings == null) { return; }

            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            showUserRevenue = (WebConfigSettings.ShowRevenueInForums && isCommerceReportViewer);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            filterContentFromTrustedUsers = ForumConfiguration.FilterContentFromTrustedUsers;
            if (ForumConfiguration.AllowExternalImages)
            {
                allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;
            }

            IsModerator = isEditable;

            if(forum != null)
            {
                if(WebUser.IsInRoles(forum.RolesThatCanModerate)) { IsModerator = true; }
            }

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;
                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;

            }

            if (displaySettings.HideAvatars)
            {
                allowGravatars = false;
                disableAvatars = true;
            }

            notificationUrl = SiteRoot + "/Forums/EditSubscriptions.aspx?mid="
                + moduleId.ToInvariantString()
                + "&pageid=" + PageId.ToInvariantString() + "#forum" + ItemId.ToInvariantString();

            pnlNotify.Visible = (!isSubscribedToForum) && !displaySettings.HideNotificationLinkOnPostList;
            if (!Request.IsAuthenticated) { pnlNotify.Visible = false; }

            if (WebConfigSettings.LoginPageRelativeUrl.Length > 0)
            {
                lnkLogin.NavigateUrl = SiteRoot + WebConfigSettings.LoginPageRelativeUrl + "?returnurl=" + Server.UrlEncode(Request.RawUrl);
                lnkLoginBottom.NavigateUrl = SiteRoot + WebConfigSettings.LoginPageRelativeUrl + "?returnurl=" + Server.UrlEncode(Request.RawUrl);
            }
            else
            {
                lnkLogin.NavigateUrl = SiteRoot + "/Secure/Login.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);
                lnkLoginBottom.NavigateUrl = SiteRoot + "/Secure/Login.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);
            }
        }

        protected string FormatEditUrl(int forumId, int threadId, int postId)
        {
            // doesn't need url encoding because NavigateUrl property on hyperlink does that for you
            return SiteRoot + "/Forums/EditPost.aspx?pageid="
                + PageId.ToInvariantString() + "&mid=" + ModuleId.ToInvariantString()
                + "&postid=" + postId.ToInvariantString()
                + "&thread=" + threadId.ToInvariantString()
                + "&forumid=" + forumId.ToInvariantString()
                + "&pagenumber=" + PageNumber.ToInvariantString();
        }

        private void PopulateLabels()
        {
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

            lnkNotify.Text = ForumResources.SubscribeLink;
            lnkNotify.ImageUrl = ImageSiteRoot + "/Data/SiteImages/email.png";
            lnkNotify.NavigateUrl = notificationUrl;

            lnkNotify2.Text = ForumResources.SubscribeLongLink;
            lnkNotify2.ToolTip = ForumResources.SubscribeLongLink;
            lnkNotify2.NavigateUrl = notificationUrl;

            UserNameTooltipFormat = ForumResources.UserProfileLinkTooltipFormat;

            lblClosedTop.Text = ForumResources.ThreadClosed;
            lblClosedBottom.Text = ForumResources.ThreadClosed;

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

    }
}