/// Author:					Joe Audette
/// Created:				2008-06-14
/// Last Modified:			2011-02-26
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentWorkflowPage : NonCmsBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!WebUser.IsAdminOrContentAdminOrContentPublisher && !WebUser.IsInRoles(WebConfigSettings.RolesAllowedToUseWorkflowAdminPages))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentWorkflowLabel);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";

            heading.Text = Resource.AdminMenuContentWorkflowLabel;

            lnkAwaitingApproval.Text = Resource.AwaitingApprovalHeading;
            lnkAwaitingApproval.NavigateUrl = SiteRoot + "/Admin/ContentAwaitingApproval.aspx";

            liAwaitingPublishing.Visible = WebConfigSettings.Use3LevelContentWorkflow;
            lnkAwaitingPublishing.Text = Resource.AwaitingPublishingHeading; 
            lnkAwaitingPublishing.NavigateUrl = SiteRoot + "/Admin/ContentAwaitingPublishing.aspx"; 

            lnkRejectedContent.Text = Resource.RejectedContentHeading;
            lnkRejectedContent.NavigateUrl = SiteRoot + "/Admin/RejectedContent.aspx";

            lnkPendingPages.Text = Resource.PendingPagesTitle;
            lnkPendingPages.NavigateUrl = SiteRoot + "/Admin/PendingPages.aspx";

            AddClassToBody("administration");
            AddClassToBody("wfadmin");

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
