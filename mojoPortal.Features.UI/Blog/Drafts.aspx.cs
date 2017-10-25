/// Author:					
/// Created:				2007-12-14
/// Last Modified:			2017-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class BlogDraftsPage : NonCmsBasePage
    {
        #region Properties

        private int moduleId = 0;
        private int pageId = -1;
        private int pageNumber = 1;
        private int totalPages = 1;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private string blogDateTimeFormat;
        private BlogConfiguration config = null;
        private SiteUser currentUser = null;

        protected bool ShowAuthor = false;


        protected string BlogDateTimeFormat
        {
            get { return blogDateTimeFormat; }

        }

        protected int PageId
        {
            get { return pageId; }
        }

        protected int ModuleId
        {
            get { return moduleId; }
        }

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();
            
            LoadSettings();

            if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (BlogConfiguration.SecurePostsByUser)
            {
                if (WebUser.IsInRoles(config.ApproverRoles))
                {
                    ShowAuthor = true;
                    using (IDataReader reader = Blog.GetPageOfDrafts(
                        moduleId,
                        Guid.Empty,
                        pageNumber,
                        displaySettings.DraftPostsPageSize, out totalPages))
                    {
                        rptDrafts.DataSource = reader;
                        rptDrafts.DataBind();
                    }
                }
                else
                {

                    if (currentUser == null) { return; }

                    using (IDataReader reader = Blog.GetPageOfDrafts(
                            moduleId,
                            currentUser.UserGuid,
                            pageNumber,
                            displaySettings.DraftPostsPageSize, out totalPages))
                    {
                        rptDrafts.DataSource = reader;
                        rptDrafts.DataBind();
                    }
                }
            }
            else
            {
                ShowAuthor = true;

                using (IDataReader reader = Blog.GetPageOfDrafts(
                        moduleId,
                        Guid.Empty,
                        pageNumber,
                        displaySettings.DraftPostsPageSize, out totalPages))
                {
                    rptDrafts.DataSource = reader;
                    rptDrafts.DataBind();
                }
            }

            string pageUrl = SiteRoot + "/Blog/Drafts.aspx?pageid=" + pageId.ToInvariantString()
                           + "&amp;mid=" + moduleId.ToInvariantString()
                           + "&amp;pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = displaySettings.ClosedPostsPageSize;
            pgr.PageCount = totalPages;
            pgr.CurrentIndex = pageNumber;
            pgr.Visible = (totalPages > 1);


        }

        protected string FormatPostAuthor(string authorName, string firstName, string lastName)
        {


            if ((!string.IsNullOrEmpty(firstName)) && (!string.IsNullOrEmpty(lastName)))
            {
                return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, firstName + " " + lastName);
            }

            return string.Format(CultureInfo.InvariantCulture, BlogResources.PostAuthorFormat, authorName);


        }


        protected string FormatBlogDate(DateTime startDate)
        {
            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(blogDateTimeFormat);

            }

            return startDate.AddHours(timeOffset).ToString(blogDateTimeFormat);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.BlogDraftsLink);
            heading.Text = BlogResources.BlogDraftsHeading;

            blogDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

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

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            currentUser = SiteUtils.GetCurrentSiteUser();
            config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));

            AddClassToBody("blogdrafts");

        }


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}
