// Author:					i7MEDIA
// Created:					2010-03-06
// Last Modified:			2015-08-24
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;

namespace SuperFlexiUI
{

    public partial class EditHeaderPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Module module = null;
        private Hashtable moduleSettings;
        private bool isFooter = false;
        private ModuleConfiguration config = new ModuleConfiguration();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();


            if (!UserCanEditModule(moduleId, config.FeatureGuid))
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

            if ((isFooter && !config.UseFooter) || (!isFooter && !config.UseHeader))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {
            edContent.Text = isFooter ? config.FooterContent : config.HeaderContent;

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (module != null)
            {
                string contentType = isFooter ? "FooterContent" : "HeaderContent";
                ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, contentType, edContent.Text);
            }

            WebUtils.SetupRedirect(this, lnkCancel.NavigateUrl);
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, isFooter ? SuperFlexiResources.EditFooter : SuperFlexiResources.EditHeader);

            heading.Text = isFooter ? SuperFlexiResources.EditFooter : SuperFlexiResources.EditHeader;
            btnSave.Text = SuperFlexiResources.UpdateButton;
            lnkCancel.Text = SuperFlexiResources.CancelButton;
            
        }

        private void LoadSettings()
        {

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            //we want to get the module using this method because it will let the module be editable when placed on the page with a ModuleWrapper
            module = SuperFlexiHelpers.GetSuperFlexiModule(moduleId);
            if (module == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;

            }
            config = new ModuleConfiguration(module);


            //edContent.Height = Unit.Pixel(400);
            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            AddClassToBody(isFooter ? "flexi-editfooter" : "flexi-editheader");
            AddClassToBody(config.EditPageCssClass);
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            isFooter = WebUtils.ParseBoolFromQueryString("f", isFooter);

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
