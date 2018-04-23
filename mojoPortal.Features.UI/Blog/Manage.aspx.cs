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

    public partial class ManagePage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int countOfDrafts = 0;
        private int countOfExpiredPosts = 0;
        private BlogConfiguration config;
        private SiteUser currentUser = null;
        

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
			LoadParams();

        
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


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.BlogAdministration);

            heading.Text = BlogResources.BlogAdministration;

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

            lnkCategories.Text = BlogResources.ManageCategories;
            lnkNewPost.Text = BlogResources.NewPostOrDraft;
            lnkDrafts.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.DraftsFormat, countOfDrafts);
            lnkClosedPosts.Text = string.Format(CultureInfo.InvariantCulture, BlogResources.ExpiredPostsFormat, countOfExpiredPosts);
        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            config = new BlogConfiguration(ModuleSettings.GetModuleSettings(moduleId));

            lnkCategories.NavigateUrl = SiteRoot + "/Blog/EditCategory.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString();

            lnkNewPost.NavigateUrl = SiteRoot + "/Blog/EditPost.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString();

            lnkDrafts.NavigateUrl = SiteRoot + "/Blog/Drafts.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString();

            lnkClosedPosts.NavigateUrl = SiteRoot + "/Blog/ClosedPosts.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString();

            if (currentUser == null) { return; }

            if (BlogConfiguration.SecurePostsByUser)
            {
                if (WebUser.IsInRoles(config.ApproverRoles))
                {
                    countOfDrafts = Blog.GetCountOfDrafts(moduleId, Guid.Empty);
                }
                else
                {
                    countOfDrafts = Blog.GetCountOfDrafts(moduleId, currentUser.UserGuid);
                }
            }
            else
            {
                countOfDrafts = Blog.GetCountOfDrafts(moduleId, Guid.Empty);
            }

            countOfExpiredPosts = Blog.GetCountClosed(moduleId);

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

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
