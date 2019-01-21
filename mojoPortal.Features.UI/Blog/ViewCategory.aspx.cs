// Author:				    
// Created:			        2005-06-05
// Last Modified:		    2017-03-15
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
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.BlogUI
{
	
    public partial class BlogCategoryView : mojoBasePage
    {
        #region Properties

        
        protected int pageId = -1;
        protected int moduleId = -1;
        protected int categoryId = -1;
        protected string category = string.Empty;
        private Hashtable moduleSettings;
        protected BlogConfiguration config = new BlogConfiguration();
        private Module blogModule = null;
       

        
        #endregion

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
            

            if (!IsPostBack)
            {
                if ((moduleId > -1) && (categoryId > -1))
                {
                    using (IDataReader reader = Blog.GetCategory(categoryId))
                    {
                        if (reader.Read())
                        {
                            this.category = reader["Category"].ToString();
                        }
                    }

                    string prefixLabel = BlogResources.BlogCategoriesPrefixLabel;
                    if (displaySettings.OverrideCategoryPrefixLabel.Length > 0)
                    {
                        prefixLabel = displaySettings.OverrideCategoryPrefixLabel;
                    }

                    heading.Text = Page.Server.HtmlEncode(prefixLabel + category);

                    if (blogModule != null)
                    {
                        Title = SiteUtils.FormatPageTitle(siteSettings,
                            blogModule.ModuleTitle + " - " + prefixLabel + category);

                        MetaDescription = string.Format(CultureInfo.InvariantCulture,
                            BlogResources.CategoryMetaDescriptionFormat,
                            blogModule.ModuleTitle, category);
                    }

                }
            }

            LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
            LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsBlogSection", "blog");

		}

        

		private void LoadSettings()
		{
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new BlogConfiguration(moduleSettings);

            postList.ModuleId = moduleId;
            postList.PageId = pageId;
            postList.DisplayMode = "ByCategory";
			postList.ShowFeaturedPost = false;
			postList.IsEditable = UserCanEditModule(moduleId, Blog.FeatureGuid);
            postList.Config = config;
            postList.SiteRoot = SiteRoot;
            postList.ImageSiteRoot = ImageSiteRoot;

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

            AddClassToBody("blogviewcategory");

            if (BlogConfiguration.UseNoIndexFollowMetaOnLists)
            {
                SiteUtils.AddNoIndexFollowMeta(Page);
            }

		}

        

        private void LoadParams()
        {
           
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
            blogModule = GetModule(moduleId, Blog.FeatureGuid);


            // an experiment with ASP.NET routing

            //if (Page.RouteData.Values["category"] != null)
            //{
            //    categoryId = Convert.ToInt32(Page.RouteData.Values["category"]);
            //}

            //if (Page.RouteData.Values["pageid"] != null)
            //{
            //    pageId = Convert.ToInt32(Page.RouteData.Values["pageid"]);
            //}

            //if (Page.RouteData.Values["moduleid"] != null)
            //{
            //    moduleId = Convert.ToInt32(Page.RouteData.Values["moduleid"]);
            //}

        }

	}
}
