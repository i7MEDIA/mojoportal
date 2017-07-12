/// Author:					
/// Created:				2008-03-18
/// Last Modified:			2011-06-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class ForumUserThreadsPage : mojoBasePage
    {
        private int userId = -1;
        private int pageNumber = 1;
        private SiteUser forumUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebConfigSettings.AllowUserThreadBrowsing)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            AddConnoicalUrl();
            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);
            PopulateControls();

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");

        }

        private void PopulateControls()
        {
            if (forumUser == null) return;

            heading.Text = string.Format(CultureInfo.InvariantCulture,
                ForumResources.ForumUserThreadHeading,
                Server.HtmlEncode(forumUser.Name));

            Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
                ForumResources.UserThreadTitleFormat, Server.HtmlEncode(forumUser.Name)));

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                ForumResources.UserThreadMetaFormat, Server.HtmlEncode(forumUser.Name));

        }

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            Literal link = new Literal();
            link.ID = "threadurl";
            link.Text = "\n<link rel='canonical' href='"
                + SiteUtils.GetNavigationSiteRoot()
                + "/Forums/UserThreads.aspx?userid="
                + userId.ToInvariantString()
                + "&amp;pagenumber=" + pageNumber.ToInvariantString()
                + "' />";

            Page.Header.Controls.Add(link);

        }


       

        private void LoadSettings()
        {
            userId = WebUtils.ParseInt32FromQueryString("userId", -1);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
           
            forumUser = new SiteUser(siteSettings, userId);
            if (forumUser.UserId == -1) { forumUser = null; }

            threadList.SiteSettings = siteSettings;
            threadList.ForumUser = forumUser;
            threadList.PageNumber = pageNumber;
            threadList.SiteRoot = SiteRoot;
            threadList.ImageSiteRoot = ImageSiteRoot;

            threadListAlt.SiteSettings = siteSettings;
            threadListAlt.ForumUser = forumUser;
            threadListAlt.PageNumber = pageNumber;
            threadListAlt.SiteRoot = SiteRoot;
            threadListAlt.ImageSiteRoot = ImageSiteRoot;

            if (displaySettings.UseAltUserThreadList)
            {
                threadList.Visible = false;
                threadListAlt.Visible = true;
            }

            AddClassToBody("forumuserthreads");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
