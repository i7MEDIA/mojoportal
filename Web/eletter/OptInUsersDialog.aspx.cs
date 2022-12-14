// Author:					
// Created:				    2012-07-13
// Last Modified:			2012-07-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class OptInUsersDialog : mojoDialogBasePage
    {
        private Guid taskGuid = Guid.Empty;
        private Guid letterInfoGuid = Guid.Empty;
        private SiteUser currentUser = null;
        private LetterInfo letterInfo = null;
        private int countOfAvailableUsers = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebUser.IsAdminOrNewsletterAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadParams();
            LoadSettings();
            PopulateLabels();
            PopulateControls();
            SetupScript();
        }

        private void PopulateControls()
        {
            if (letterInfoGuid == Guid.Empty)
            {
                btnOptInUsers.Visible = false;
                return;
            }

            letterInfo = new LetterInfo(letterInfoGuid);
            heading.Text = letterInfo.Title;

            countOfAvailableUsers = SubscriberRepository.CountUsersNotSubscribedByLetter(
                SiteInfo.SiteGuid,
                letterInfoGuid,
                WebConfigSettings.NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers);

            btnOptInUsers.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsLetterOptInMembersFormat, countOfAvailableUsers);

        }

        void btnOptInUsers_Click(object sender, EventArgs e)
        {
            if (letterInfoGuid == Guid.Empty) { return; }
            if (currentUser == null) { return; }

            LetterOptInTask optInTask = new LetterOptInTask();
            optInTask.SiteGuid = SiteInfo.SiteGuid;
            optInTask.QueuedBy = currentUser.UserGuid;
            optInTask.LetterInfoGuid = letterInfoGuid;
            optInTask.ExcludeIfAnyUnsubscribeHx = WebConfigSettings.NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers;
            optInTask.QueueTask();

            string redirectUrl = SiteRoot + "/eletter/OptInUsersDialog.aspx?l=" + letterInfoGuid.ToString()
                + "&t=" + optInTask.TaskGuid.ToString();

            WebTaskManager.StartOrResumeTasks();

            WebUtils.SetupRedirect(this, redirectUrl);


        }


        private void PopulateLabels()
        {
            //btnOptInUsers.Text = "Opt In All Verified Site Users";
            UIHelper.AddConfirmationDialog(btnOptInUsers, Resource.NewsletterOptInUsersWarning);
        
        
        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();

            

        }

        private void LoadParams()
        {
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", letterInfoGuid);
            
            taskGuid = WebUtils.ParseGuidFromQueryString("t", taskGuid);

        }

        private void SetupScript()
        {
            if (taskGuid == Guid.Empty) { return; }

            btnOptInUsers.Visible = false;

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");


            script.Append("$('#" + pnlProgress.ClientID + "').progressbar({ value: 0 });");
            //script.Append("$('#" + lnkSendLog.ClientID + "').hide(); ");
            script.Append("setTimeout(function(){updateProgress" + pnlProgress.ClientID + "();}, 500);");

            //function to check task status
            script.Append("function updateProgress" + pnlProgress.ClientID + "() {");
            script.Append("$.getJSON('" + SiteRoot + "/Services/TaskProgress.ashx?t=" + taskGuid.ToString() + "'");
            script.Append(",function(data){");
            script.Append("$('#" + pnlProgress.ClientID + "').progressbar('option','value',data.percentComplete);");
            script.Append("$('#" + pnlStatus.ClientID + "').text(data.status); ");

            script.Append("if(data.percentComplete < 100){");
            script.Append("setTimeout(function(){updateProgress" + pnlProgress.ClientID + "();}, 4000); ");
            script.Append("}");
            script.Append("else{");
            script.Append("parent.location.reload(); ");
            //script.Append("$('#" + lnkSendLog.ClientID + "').show(); ");
            script.Append("}");
            //script.Append("alert(data.percentComplete);");
            script.Append("});");

            // client side long task simulation code
            //script.Append("var progress = $('#" + pnlProgress.ClientID + "').progressbar('option','value'); ");
            //script.Append("if (progress < 100) {");
            //script.Append("$('#" + pnlProgress.ClientID + "').progressbar('option','value', progress + 1);");
            //script.Append("setTimeout(updateProgress" + pnlProgress.ClientID + ", 500);");
            //script.Append("}");

            script.Append("}");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.pnlProgress.UniqueID,
                script.ToString());

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnOptInUsers.Click += new EventHandler(btnOptInUsers_Click);
            

        }

        
    }
}