// Author:					
// Created:					2011-03-21
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
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using Resources;



namespace mojoPortal.Web.AdminUI
{

    public partial class EditRegistrationAgreementPage : NonCmsBasePage
	{
        


        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!WebUser.IsAdminOrContentAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            edPreamble.Text = siteSettings.RegistrationPreamble;
            edAgreement.Text = siteSettings.RegistrationAgreement;

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.EditRegistrationAgreement);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.RegistrationAgreementLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/EditRegistrationAgreement.aspx";

            heading.Text = Resource.EditRegistrationAgreement;

            btnSave.Text = Resource.SaveButton;
            UIHelper.AddClearPageExitCode(btnSave);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            siteSettings.RegistrationPreamble = edPreamble.Text;
            siteSettings.RegistrationAgreement = edAgreement.Text;

            siteSettings.Save();

            CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId);
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void LoadSettings()
        {
            edPreamble.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edAgreement.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            AddClassToBody("administration editregagreement");
           
        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);

            SiteUtils.SetupEditor(edPreamble, false, this);
            SiteUtils.SetupEditor(edAgreement, false, this);

            SuppressMenuSelection();
            SuppressPageMenu();

        }

        

        #endregion
    }
}
