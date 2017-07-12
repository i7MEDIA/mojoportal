// Author:					
// Created:				    2011-06-27
// Last Modified:			2013-08-18
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class UserThreadList : UserControl
    {
        private int userId = -1;
        private int pageNumber = 1;
        private int pageSize = 20;
        private int totalPages = 1;
        protected Double timeOffset = 0;
        private SiteUser forumUser = null;
        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        public SiteSettings SiteSettings
        {
            set { siteSettings = value; }
        }

        public SiteUser ForumUser
        {
            set { forumUser = value; }
        }

        public int PageNumber
        {
            set { pageNumber = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (forumUser == null) return;

           
            using (IDataReader reader = ForumThread.GetPageByUser(
                userId,
                forumUser.SiteId,
                pageNumber,
                pageSize,
                out totalPages))
            {

                string pageUrl = SiteRoot
                    + "/Forums/UserThreads.aspx?"
                    + "userid=" + userId.ToInvariantString()
                    + "&amp;pagenumber={0}";

                pgrTop.PageURLFormat = pageUrl;
                pgrTop.ShowFirstLast = true;
                pgrTop.CurrentIndex = pageNumber;
                pgrTop.PageSize = pageSize;
                pgrTop.PageCount = totalPages;
                pgrTop.Visible = (pgrTop.PageCount > 1);

                pgrBottom.PageURLFormat = pageUrl;
                pgrBottom.ShowFirstLast = true;
                pgrBottom.CurrentIndex = pageNumber;
                pgrBottom.PageSize = pageSize;
                pgrBottom.PageCount = totalPages;
                pgrBottom.Visible = (pgrBottom.PageCount > 1);


                rptForums.DataSource = reader;
                rptForums.DataBind();
            }

        }

        protected string FormatThreadUrl(int threadId, int moduleId, int itemId, int pageId)
        {
            if (ForumConfiguration.CombineUrlParams)
            {
                return SiteRoot + "/Forums/Thread.aspx?pageid="
                + pageId.ToInvariantString()
                + "&amp;t=" + ThreadParameterParser.FormatCombinedParam(threadId, pageNumber);
            }

            return SiteRoot + "/Forums/Thread.aspx?pageid="
                + pageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;ItemID=" + itemId.ToInvariantString()
                + "&amp;thread=" + threadId.ToInvariantString()
                ;
        }

        protected string FormatForumUrl(int itemId, int moduleId, int pageId)
        {
            if (ForumConfiguration.CombineUrlParams)
            {
                return SiteRoot + "/Forums/ForumView.aspx?pageid="
                + pageId.ToInvariantString()
                + "&amp;f=" + ForumParameterParser.FormatCombinedParam(itemId, 1);
            }

            return SiteRoot + "/Forums/ForumView.aspx?pageid="
                + pageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString() + "&amp;ItemID=" + itemId.ToInvariantString();
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

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            if (forumUser != null) { userId = forumUser.UserId; }

        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
        }
    }
}