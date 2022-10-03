/// Author:				
/// Created:			2004-08-14
/// Last Modified:	    2017-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class BlogArchiveView : mojoBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected int Month = DateTime.UtcNow.Month;
        protected int Year = DateTime.UtcNow.Year;
        private Hashtable moduleSettings;
        protected BlogConfiguration config = new BlogConfiguration();
        private Module blogModule = null;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion

       

        private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsBlogSection", "blog");

            LoadParams();

            

            if (!UserCanViewPage(moduleId, Blog.FeatureGuid))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this, Request.RawUrl);
                    return;
                }

                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            DateTime selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
            try
            {
                selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
            }
            catch (Exception)
            { }

            heading.Text = Page.Server.HtmlEncode(BlogResources.BlogArchivesPrefixLabel
                + selectedMonth.ToString("MMMM, yyyy"));

            if (blogModule != null)
            {
                Title = SiteUtils.FormatPageTitle(SiteInfo, blogModule.ModuleTitle + " - " + BlogResources.BlogArchivesPrefixLabel
                + selectedMonth.ToString("MMMM, yyyy"));

                MetaDescription = string.Format(CultureInfo.InvariantCulture,
                    BlogResources.ArchiveMetaDescriptionFormat,
                    blogModule.ModuleTitle,
                    selectedMonth.ToString("MMMM, yyyy"));


            }
            
            
		}

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            Month = WebUtils.ParseInt32FromQueryString("month", Month);
            Year = WebUtils.ParseInt32FromQueryString("year", Year);

        }

        private void LoadSettings()
        {
            
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new BlogConfiguration(moduleSettings);

            blogModule = GetModule(moduleId, Blog.FeatureGuid);

            postList.ModuleId = moduleId;
            postList.PageId = pageId;
            postList.DisplayMode = "ByMonth";
			postList.ShowFeaturedPost = false;
            postList.IsEditable = UserCanEditModule(moduleId, Blog.FeatureGuid);
            postList.Config = config;
            postList.SiteRoot = SiteRoot;
            postList.ImageSiteRoot = ImageSiteRoot;

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
            LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);

            if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

            AddClassToBody("blogviewarchive");

            if (BlogConfiguration.UseNoIndexFollowMetaOnLists)
            {
                SiteUtils.AddNoIndexFollowMeta(Page);
            }
        }
        

	}
}
