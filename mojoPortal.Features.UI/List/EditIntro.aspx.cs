// Author:					
// Created:					2010-12-08
// Last Modified:			2018-03-28
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

    public partial class EditIntroPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Module module = null;
        private Hashtable moduleSettings;
        private ListConfiguration config = new ListConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadParams();


            if (!UserCanEditModule(moduleId, Link.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            if (!IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {
            edContent.Text = config.IntroContent;

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (module != null)
            {
                ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, "IntroContent", edContent.Text);
            }

            WebUtils.SetupRedirect(this, lnkCancel.NavigateUrl);
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, LinkResources.EditIntro);

            heading.Text = LinkResources.EditIntro;
            btnSave.Text = LinkResources.EditLinksUpdateButton;
            lnkCancel.Text = LinkResources.EditLinksCancelButton;
            
        }

        private void LoadSettings()
        {
            module = GetModule(moduleId, Link.FeatureGuid);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ListConfiguration(moduleSettings);

            //edContent.Height = Unit.Pixel(400);
            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            AddClassToBody("listeditintro");
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

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
            btnSave.Click += new EventHandler(btnSave_Click);

            SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);
            SuppressPageMenu();
        }

        

        

        #endregion
    }
}
