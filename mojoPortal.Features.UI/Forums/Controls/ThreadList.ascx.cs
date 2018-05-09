// Author:					
// Created:				    2011-06-13
// Last Modified:			2012-10-24
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class ThreadList : UserControl
    {
        #region Properties

        private ForumConfiguration config = null;
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private SiteSettings siteSettings = null;
        private string notificationUrl = string.Empty;
        private Forum forum = null;

        protected bool isSubscribedToForum = false;
        private SiteUser currentUser = null;

        private int pageNumber = 1;

        public Forum Forum
        {
            get { return forum; }
            set { forum = value; }
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

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        private bool isEditable = false;

        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

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

        private string nonSslSiteRoot = string.Empty;

        public string NonSslSiteRoot
        {
            get { return nonSslSiteRoot; }
            set { nonSslSiteRoot = value; }
        }

        private string imageSiteRoot = string.Empty;

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
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

            //TODO: need to look into this:
            // http://googlewebmastercentral.blogspot.com/2011/09/pagination-with-relnext-and-relprev.html
            //http://www.whatwg.org/specs/web-apps/current-work/multipage/links.html#sequential-link-types

            string pageUrl;

            if (ForumConfiguration.CombineUrlParams)
            {
                pageUrl = SiteRoot
                    + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;f=" + forum.ItemId.ToInvariantString()
                    + "~{0}";
            }
            else
            {
                pageUrl = SiteRoot
                    + "/Forums/ForumView.aspx?"
                    + "ItemID=" + forum.ItemId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "&amp;pageid=" + PageId.ToInvariantString()
                    + "&amp;pagenumber={0}";
            }

            pgrTop.PageURLFormat = pageUrl;
            pgrTop.ShowFirstLast = true;
            pgrTop.CurrentIndex = pageNumber;
            pgrTop.PageSize = forum.ThreadsPerPage;
            pgrTop.PageCount = forum.TotalPages;
            pgrTop.Visible = (pgrTop.PageCount > 1);

            pgrBottom.PageURLFormat = pageUrl;
            pgrBottom.ShowFirstLast = true;
            pgrBottom.CurrentIndex = pageNumber;
            pgrBottom.PageSize = forum.ThreadsPerPage;
            pgrBottom.PageCount = forum.TotalPages;
            pgrBottom.Visible = (pgrBottom.PageCount > 1);

            lnkNewThread.NavigateUrl = SiteRoot
                    + "/Forums/EditPost.aspx?forumid=" + ItemId.ToInvariantString()
                    + "&pageid=" + PageId.ToInvariantString()
                    + "&mid=" + ModuleId.ToInvariantString();

            lnkNewThreadBottom.NavigateUrl = lnkNewThread.NavigateUrl;

            lnkNewThread.Visible = WebUser.IsInRoles(forum.RolesThatCanPost) && !forum.Closed;
            lnkNewThreadBottom.Visible = lnkNewThread.Visible;

            lnkLogin.Visible = !lnkNewThread.Visible && !Request.IsAuthenticated;

            
            
            using (IDataReader reader = forum.GetThreads(pageNumber))
            {
                rptForums.DataSource = reader;

#if MONO
                this.rptForums.DataBind();
#else
                this.DataBind();
#endif
               
            }

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
                + "&amp;ItemID=" + itemId.ToInvariantString()
                + "&amp;thread=" + threadId.ToInvariantString()
                ;
        }

        //private void SetupCanonicalLinks()
        //{
        //    if (Page.Header != null)
        //    {

        //        Literal link = new Literal();
        //        link.ID = "forumurl";

        //        string canonicalUrl = SiteUtils.GetNavigationSiteRoot()
        //            + "/Forums/ForumView.aspx?"
        //            + "ItemID=" + forum.ItemId.ToInvariantString()
        //            + "&amp;mid=" + ModuleId.ToInvariantString()
        //            + "&amp;pageid=" + PageId.ToInvariantString()
        //            + "&amp;pagenumber=" + pageNumber.ToInvariantString();

        //        if (SiteUtils.IsSecureRequest() && (!CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
        //        {
        //            if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
        //            {
        //                canonicalUrl = canonicalUrl.Replace("https:", "http:");
        //            }

        //        }

        //        link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

        //        Page.Header.Controls.Add(link);
        //    }

        //}

        private void PopulateLabels()
        {
            lnkNewThread.Text = ForumResources.ForumViewNewThreadLabel;
            lnkNewThread.ToolTip = ForumResources.ForumViewNewThreadLabel;
            lnkNewThreadBottom.Text = ForumResources.ForumViewNewThreadLabel;
            lnkNewThreadBottom.ToolTip = ForumResources.ForumViewNewThreadLabel;
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
            //pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            notificationUrl = SiteRoot + "/Forums/EditSubscriptions.aspx?mid="
                + ModuleId.ToInvariantString()
                + "&pageid=" + PageId.ToInvariantString() +"#forum" + ItemId.ToInvariantString();

            lnkNotify.ImageUrl = ImageSiteRoot + "/Data/SiteImages/email.png";
            lnkNotify.NavigateUrl = notificationUrl;
            lnkNotify2.NavigateUrl = notificationUrl;

            if (WebConfigSettings.LoginPageRelativeUrl.Length > 0)
            {
                lnkLogin.NavigateUrl = SiteRoot + WebConfigSettings.LoginPageRelativeUrl + "?returnurl=" + Server.UrlEncode(Request.RawUrl);
            }
            else
            {
                lnkLogin.NavigateUrl = SiteRoot + "/Secure/Login.aspx?returnurl=" + Server.UrlEncode(Request.RawUrl);
            }

            if (Request.IsAuthenticated)
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if ((currentUser != null) && (ItemId > -1))
                {
                    isSubscribedToForum = Forum.IsSubscribed(ItemId, currentUser.UserId);
                }

                if (!isSubscribedToForum) { pnlNotify.Visible = true; }

            }
        }

        protected string GetRowCssClass(int stickySort, bool isLocked)
        {
            if (isLocked) { return "lockedthreadrow"; }
            if (stickySort < Forum.NormalThreadSort) { return "stickythreadrow"; }

            return "normalthreadrow";

        }

        protected string GetFolderCssClass(int stickySort, bool isLocked)
        {
            if (isLocked) { return "lockedthread"; }
            if (stickySort < Forum.NormalThreadSort) { return "stickythread"; }

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
            this.Load += new EventHandler(Page_Load);

        }
    }
}