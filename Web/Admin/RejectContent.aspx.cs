// Author:						Kevin Needham
// Created:					    2009-06-23
// Modified:               
// 2011-02-26 
// 2013-04-24 Joe Davis
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 


using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.UI;

namespace mojoPortal.Web.AdminUI 
{
    public partial class RejectContent : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Module module = null;
        private String cacheDependencyKey;
        private ContentWorkflow workInProgress = null;
        //private bool isPublisher = false; //joe davis

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();
            LoadSettings();
            
            if (!UserCanEditModule(moduleId) && !UserCanApproveDraftModule(moduleId, module.FeatureGuid)) //joe davis
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }
            
            PopulateLabels();

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RejectContentHeading);
            btnUpdate.Text = Resource.RejectionPageUpdateButton;
            btnCancel.Text = Resource.EditHtmlCancelButton;

            UIHelper.DisableButtonAfterClick(
                btnUpdate,
                Resource.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            if (module == null) { return; }

            string modTitle = String.Empty;
            if (module.ModuleTitle.Length > 0)
            {
                litModuleTitle.Text = Server.HtmlEncode(module.ModuleTitle);
                modTitle = String.Format("'{0}'", module.ModuleTitle);
            }
            else
            {
                modTitle = Resource.RejectionPageUntitledModuleName;
            }

            string contentAuthorName = "?";
            if (workInProgress != null)
            {
                contentAuthorName = workInProgress.CreatedByUserName;
            }   

            litRejectionIntroduction.Text = String.Format(Resource.RejectionIntroduction, Resource.RejectionPageUpdateButton, modTitle, contentAuthorName);
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            cacheDependencyKey = "Module-" + moduleId.ToString();
        }

        private void LoadSettings()
        {
            module = new Module(moduleId);
            workInProgress = ContentWorkflow.GetWorkInProgress(module.ModuleGuid);

            AddClassToBody("administration");
            AddClassToBody("wfadmin");
            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressPageMenu();
            
           
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (workInProgress == null)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) 
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return; 
            }
			//joe davis
            SiteUser draftSubmitter = GetDraftSubmitter(workInProgress);

            switch (workInProgress.Status)
            {
                case ContentWorkflowStatus.AwaitingApproval:
                    {
                        workInProgress.RejectContentChanges(
                            currentUser.UserGuid,
                            txtRejectionComments.Text);

                        WorkflowHelper.SendRejectionNotification(
                            SiteUtils.GetSmtpSettings(),
                            siteSettings,
                            currentUser,
                            workInProgress,
                            txtRejectionComments.Text);
                        break;
                    }
                case ContentWorkflowStatus.AwaitingPublishing:
                    {
                        workInProgress.RejectContentChanges(
                            currentUser.UserGuid,
                            txtRejectionComments.Text);

                        WorkflowHelper.SendPublishRejectionNotification(
                            SiteUtils.GetSmtpSettings(),
                            siteSettings,
                            currentUser,
                            draftSubmitter,
                            workInProgress,
                            txtRejectionComments.Text);
                        break;
                    }
            }
			//end joe davis
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }
		
		//joe davis
        protected SiteUser GetDraftSubmitter(ContentWorkflow workflow)
        {
            // hey joe d, note this will still return a user object even if the draft submitter guid comes back empty?
            return new SiteUser(siteSettings, ContentWorkflow.GetDraftSubmitter(workflow.Guid));
        }
    }
}
