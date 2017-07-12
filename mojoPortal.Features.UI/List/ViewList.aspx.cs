// Author:					
// Created:					2010-12-06
// Last Modified:			2010-12-06
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
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.LinksUI
{

    public partial class ViewListPage : mojoBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        //private int pageNumber = 1;
        private Hashtable moduleSettings;
        private ListConfiguration config = new ListConfiguration();
        Module module = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();


            if (!UserCanViewPage(moduleId, Link.FeatureGuid))
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
            Title1.ModuleInstance = module;
            Title1.CanEdit = UserCanEditModule(moduleId, Link.FeatureGuid);
            Title1.EditUrl = SiteRoot + "/List/Edit.aspx";
            Title1.EditText = LinkResources.EditLinksAddLinkLabel;
            if (Title1.CanEdit)
            {
                Title1.LiteralExtraMarkup =
                    "&nbsp;<a href='"
                    + SiteRoot
                    + "/List/EditIntro.aspx?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + LinkResources.EditIntro + "'>" + LinkResources.EditIntro + "</a>";
            }


            LoadSideContent(config.ShowPageLeftContent, config.ShowPageRightContent);

        }


        private void PopulateLabels()
        {
            if (CurrentPage != null)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);
            }

        }

        private void LoadSettings()
        {
            module = GetModule(moduleId, Link.FeatureGuid);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ListConfiguration(moduleSettings);

            theList.Config = config;
            theList.PageId = pageId;
            theList.ModuleId = moduleId;
            theList.IsEditable = UserCanEditModule(moduleId, Link.FeatureGuid);
            theList.SiteRoot = SiteRoot;
            theList.ImageSiteRoot = ImageSiteRoot;

            AddClassToBody("listviewlist");
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