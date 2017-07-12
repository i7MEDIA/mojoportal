// Author:					
// Created:					2012-11-10
// Last Modified:			2017-03-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace mojoPortal.Web.BlogUI
{

    public partial class ClosedPostsPage : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        private int pageNumber = 1;
        private int totalPages = 1;
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private BlogConfiguration config = null;
        private SiteUser currentUser = null;
        private bool useFriendlyUrls = true;
        
        protected string EditLinkText = BlogResources.BlogEditEntryLink;
        
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }

            if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            BindList();

        }

        private void BindList()
        {
            string pageUrl = SiteRoot + "/Blog/ClosedPosts.aspx?pageid=" + pageId.ToInvariantString()
                           + "&amp;mid=" + moduleId.ToInvariantString()
                           + "&amp;pagenumber={0}";

            DataSet dsBlogPosts = Blog.GetClosedDataSet(moduleId, pageNumber, displaySettings.ClosedPostsPageSize, out totalPages);

            rpt.DataSource = dsBlogPosts.Tables["Posts"];
            rpt.DataBind();

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = displaySettings.ClosedPostsPageSize;
            pgr.PageCount = totalPages;
            pgr.CurrentIndex = pageNumber;
            pgr.Visible = (totalPages > 1);

        }

        protected bool CanEditPost(int postAuthorId)
        {
            if (BlogConfiguration.SecurePostsByUser)
            {
                if (WebUser.IsInRoles(config.ApproverRoles)) { return true; }

                if (currentUser == null) { return false; }

                return (postAuthorId == currentUser.UserId);
            }

            return UserCanEditModule(moduleId, Blog.FeatureGuid);

        }

        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (useFriendlyUrls && (itemUrl.Length > 0))
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + pageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                ;

        }

        protected string FormatBlogDate(DateTime startDate, DateTime endDate)
        {
            string dateRangeFormat = ResourceHelper.GetResourceString("Resource", "DateRangeFormat");

            if (timeZone != null)
            {
                return string.Format(CultureInfo.InvariantCulture, 
                    dateRangeFormat, 
                    TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString("G"),
                    TimeZoneInfo.ConvertTimeFromUtc(endDate, timeZone).ToString("G")
                    );

            }

            return startDate.AddHours(TimeOffset).ToString(config.DateTimeFormat);

        }

        protected string FormatPostAuthor(string authorName, string firstName, string lastName)
        {
            
           
            if ((!string.IsNullOrEmpty(firstName)) && (!string.IsNullOrEmpty(lastName)))
            {
                return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, firstName + " " + lastName);
            }

            return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, authorName);
            

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Blog/Manage.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='selectedcrumb'>" + BlogResources.Administration
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.ExpiredBlogPosts);

            heading.Text = BlogResources.ExpiredBlogPosts;
        }

        private void LoadSettings()
        {
            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));
            currentUser = SiteUtils.GetCurrentSiteUser();

            useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);
            if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}
