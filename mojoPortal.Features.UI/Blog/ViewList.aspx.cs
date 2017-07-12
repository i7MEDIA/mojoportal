// Author:				    
// Created:			        2010-05-22
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
    public partial class ViewList : mojoBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        private bool userCanEdit = false;
       // private int countOfDrafts = 0;
        private int pageNumber = 1;

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
            LoadParams();

            if (!UserCanViewPage(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateControls();

        }

        private void PopulateControls()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.PostList);

            moduleTitle.EditUrl = SiteRoot + "/Blog/EditPost.aspx";
            moduleTitle.EditText = BlogResources.BlogAddPostLabel;
            moduleTitle.ModuleInstance = GetModule(moduleId);
            moduleTitle.CanEdit = userCanEdit;
            //if ((userCanEdit) && (countOfDrafts > 0))
            //{
            //    moduleTitle.LiteralExtraMarkup =
            //        "&nbsp;<a href='"
            //        + SiteRoot
            //        + "/Blog/EditCategory.aspx?pageid=" + pageId.ToInvariantString()
            //        + "&amp;mid=" + moduleId.ToInvariantString()
            //        + "' class='ModuleEditLink' title='" + BlogResources.BlogEditCategoriesLabel + "'>" + BlogResources.BlogEditCategoriesLabel + "</a>"
            //        + "&nbsp;<a href='"
            //        + SiteRoot
            //        + "/Blog/Drafts.aspx?pageid=" + pageId.ToInvariantString()
            //        + "&amp;mid=" + moduleId.ToInvariantString()
            //        + "' class='ModuleEditLink' title='" + BlogResources.BlogDraftsLink + "'>" + BlogResources.BlogDraftsLink + "</a>";
            //}
            //else 
            if (userCanEdit)
            {
                moduleTitle.LiteralExtraMarkup =
                    "&nbsp;<a href='"
                    + SiteRoot
                    + "/Blog/Manage.aspx?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='"
                    + BlogResources.Administration + "'>"
                    + BlogResources.Administration + "</a>"
                    ;
            }

            postList.ModuleId = moduleId;
            postList.PageId = pageId;
            postList.IsEditable = userCanEdit;
            BlogConfiguration config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));
            postList.Config = config;
            postList.SiteRoot = SiteRoot;
            postList.ImageSiteRoot = ImageSiteRoot;

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            searchBoxTop.Visible = config.ShowBlogSearchBox && !displaySettings.HideSearchBoxInPostList;

            //make this page look as close as possible to the way a cms page with the blog module on it looks
            LoadSideContent(true, true);
            LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);

        }

        private void AddNoIndexFollowMeta()
        {


        }


        //private void AddCanonicalUrl()
        //{
        //    if (Page.Header == null) { return; }

        //    string canonicalUrl = SiteRoot
        //        + "/Blog/ViewList.aspx?pageid="
        //        + pageId.ToInvariantString()
        //        + "&amp;mid=" + moduleId.ToInvariantString()
        //        + "&amp;pagenumber=" + pageNumber.ToInvariantString();

        //    if (SiteUtils.IsSecureRequest() && (!CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
        //    {
        //        if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
        //        {
        //            canonicalUrl = canonicalUrl.Replace("https:", "http:");
        //        }

        //    }

        //    Literal link = new Literal();
        //    link.ID = "blogcaturl";
        //    link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

        //    Page.Header.Controls.Add(link);

        //}

        private void LoadSettings()
        {
            userCanEdit = UserCanEditModule(moduleId);
            //if (userCanEdit) { countOfDrafts = Blog.CountOfDrafts(moduleId); }

            if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

            AddClassToBody("blogviewlist");
            if (BlogConfiguration.UseNoIndexFollowMetaOnLists)
            {
                SiteUtils.AddNoIndexFollowMeta(Page);
            }

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            
        }

        
    }
}