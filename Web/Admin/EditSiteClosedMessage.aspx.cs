// Author:					
// Created:					2011-10-07
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
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.AdminUI
{

    public partial class EditSiteClosedMessagePage : NonCmsBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
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
            
            LoadSettings();
            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            editor.Text = siteSettings.SiteIsClosedMessage;

        }

        void btnSave_Click(object sender, EventArgs e)
        {

            siteSettings.SiteIsClosedMessage = editor.Text;

            siteSettings.Save();

            CacheHelper.ClearSiteSettingsCache(siteSettings.SiteId);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteClosedMessageLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.SiteClosedMessageLabel;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/EditSiteClosedMessage.aspx";

            heading.Text = Resource.SiteClosedMessageLabel;

            btnSave.Text = Resource.SaveButton;
            UIHelper.AddClearPageExitCode(btnSave);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;
        }

        private void LoadSettings()
        {
            editor.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            AddClassToBody("administration editclosedmessage");
        }

       


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            SiteUtils.SetupEditor(editor, false, this);


        }

        

        #endregion
    }
}
