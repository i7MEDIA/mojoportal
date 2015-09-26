/// Author:					Joe Audette
/// Created:				2007-04-29
/// Last Modified:			2013-10-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class AdminMenuPage : NonCmsBasePage
    {
        private bool IsAdminOrContentAdmin = false;
        private bool IsAdmin = false;
        private bool isSiteEditor = false;
        private bool isCommerceReportViewer = false;
        private CommerceConfiguration commerceConfig = null;
        SecurityAdvisor securityAdvisor = new SecurityAdvisor();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (
                (!WebUser.IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin)
                &&(!isSiteEditor)
                && (!isCommerceReportViewer)
                )
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            SecurityHelper.DisableBrowserCache();
            
            PopulateLabels();
            PopulateControls();
            
            
        }

        private void PopulateControls()
        {
            BuildAdditionalMenuListItems();

        }

        private void BuildAdditionalMenuListItems()
        {
            if (siteSettings == null) return;

            ContentAdminLinksConfiguration linksConfig 
                = ContentAdminLinksConfiguration.GetConfig(siteSettings.SiteId);

            StringBuilder addedLinks = new StringBuilder();
            foreach (ContentAdminLink link in linksConfig.AdminLinks)
            {
                if (
                    (link.VisibleToRoles.Length == 0)
                    ||(WebUser.IsInRoles(link.VisibleToRoles))
                    )
                {
                    addedLinks.Append("\n<li>");
                    addedLinks.Append("<a ");
                    string title = ResourceHelper.GetResourceString(link.ResourceFile, link.ResourceKey);
                    addedLinks.Append("title='" + title + "' ");
                    addedLinks.Append("class='" + link.CssClass + "' ");
                    string url = link.Url;
                    if (url.StartsWith("~/"))
                    {
                        url = SiteRoot + "/" + url.Replace("~/", string.Empty);
                    }
                    addedLinks.Append("href='" + url + "'>" + title + "</a>");
                    addedLinks.Append("</li>");
                }

            }

            litSupplementalLinks.Text = addedLinks.ToString();
        }
        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuHeading);

            heading.Text = Resource.AdminMenuHeading;

            liSiteSettings.Visible = IsAdminOrContentAdmin || isSiteEditor;
            lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
            lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

            liSiteList.Visible = ((WebConfigSettings.AllowMultipleSites) && (siteSettings.IsServerAdminSite) && IsAdmin);
            lnkSiteList.Text = Resource.SiteList;
            lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";

            lnkSecurityAdvisor.Text = Resource.SecurityAdvisor;
            lnkSecurityAdvisor.NavigateUrl = SiteRoot + "/Admin/SecurityAdvisor.aspx";
            if (IsAdmin && siteSettings.IsServerAdminSite)
            {
                liSecurityAdvisor.Visible = true;
                if (!securityAdvisor.UsingCustomMachineKey())
                {
                    imgMachineKeyDanger.Visible = true;
                    lblNeedsAttantion.Visible = true;
                    lnkSecurityAdvisor.CssClass = "lnkSecurityAdvisorWarning";
                }

            }
            
            

            imgMachineKeyDanger.ImageUrl = "~/Data/SiteImages/warning.png";
            imgMachineKeyDanger.AlternateText = Resource.SecurityDangerLabel;
            
           
            liCommerceReports.Visible = (isCommerceReportViewer &&(commerceConfig != null)&&(commerceConfig.IsConfigured));
            lnkCommerceReports.Text = Resource.CommerceReportsLink;
            lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

            liContentManager.Visible = (IsAdminOrContentAdmin || isSiteEditor);
            lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
            lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";

            liContentWorkFlow.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
            lnkContentWorkFlow.Visible = siteSettings.EnableContentWorkflow && WebUser.IsAdminOrContentAdminOrContentPublisher;
            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkContentWorkFlow.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";

            liContentTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
            lnkContentTemplates.Text = Resource.ContentTemplates;
            lnkContentTemplates.NavigateUrl = SiteRoot + "/Admin/ContentTemplates.aspx";

            liStyleTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
            lnkStyleTemplates.Text = Resource.ContentStyleTemplates;
            lnkStyleTemplates.NavigateUrl = SiteRoot + "/Admin/ContentStyles.aspx";

            

            liPageTree.Visible = (IsAdminOrContentAdmin||isSiteEditor||(SiteMapHelper.UserHasAnyCreatePagePermissions(siteSettings)));
            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + WebConfigSettings.PageTreeRelativeUrl;

            liRoleAdmin.Visible = (WebUser.IsRoleAdmin || IsAdmin);
            lnkRoleAdmin.Text = Resource.AdminMenuRoleAdminLink;
            lnkRoleAdmin.NavigateUrl = SiteRoot + "/Admin/RoleManager.aspx";

            liPermissions.Visible = IsAdmin;
            lnkPermissionAdmin.Text = Resource.SiteSettingsPermissionsTab;
            lnkPermissionAdmin.NavigateUrl = SiteRoot + "/Admin/PermissionsMenu.aspx";

            if ((WebConfigSettings.UseRelatedSiteMode)&&(WebConfigSettings.RelatedSiteModeHideRoleManagerInChildSites))
            {
                if (siteSettings.SiteId != WebConfigSettings.RelatedSiteID) { liRoleAdmin.Visible = false; }
            }

            liFileManager.Visible = IsAdminOrContentAdmin;
            lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
            if (WebConfigSettings.UseAlternateFileManagerAsDefault)
            {
                lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManagerAlt.aspx";
            }
            else
            {
                lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManager.aspx";
            }
//#if MONO
//            lnkFileManager.NavigateUrl = SiteRoot + "/Admin/FileManagerAlt.aspx";
//#endif

            if (
                    (!siteSettings.IsServerAdminSite)
                    && (!WebConfigSettings.AllowFileManagerInChildSites)
                    )
            {
                liFileManager.Visible = false;
            }

            if (WebConfigSettings.DisableFileManager)
            {
                liFileManager.Visible = false;
            }

            lnkMemberList.Text = Resource.MemberListLink;
            lnkMemberList.NavigateUrl = SiteRoot + WebConfigSettings.MemberListUrl;
            lnkMemberList.Visible = ((WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList)) || (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)));

            liAddUser.Visible = ((WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers))||(WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)) || (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)));
            lnkAddUser.Text = Resource.MemberListAddUserLabel;
            lnkAddUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1";

            if (WebConfigSettings.EnableNewsletter)
            {
                liNewsletter.Visible = (IsAdmin || isSiteEditor || WebUser.IsNewsletterAdmin);
                lnkNewsletter.Text = Resource.AdminMenuNewsletterAdminLabel;
                lnkNewsletter.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

                //liTaskQueue.Visible = IsAdmin || WebUser.IsNewsletterAdmin;
                //lnkTaskQueue.Text = Resource.TaskQueueMonitorHeading;
                //lnkTaskQueue.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            }
            else
            {
                liNewsletter.Visible = false;
                //liTaskQueue.Visible = false;
            }

            liRegistrationAgreement.Visible = (IsAdminOrContentAdmin);
            lnkRegistrationAgreement.Text = Resource.RegistrationAgreementLink;
            lnkRegistrationAgreement.NavigateUrl = SiteRoot + "/Admin/EditRegistrationAgreement.aspx";

            liLoginInfo.Visible = (IsAdminOrContentAdmin) && !WebConfigSettings.DisableLoginInfo;
            lnkLoginInfo.Text = Resource.LoginPageContent;
            lnkLoginInfo.NavigateUrl = SiteRoot + "/Admin/EditLoginInfo.aspx";

            

            liCoreData.Visible = (IsAdminOrContentAdmin && siteSettings.IsServerAdminSite);
            lnkCoreData.Text = Resource.CoreDataAdministrationLink;
            lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            liAdvancedTools.Visible = (IsAdminOrContentAdmin || isSiteEditor);
            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";
            

            liServerInfo.Visible = (IsAdminOrContentAdmin || isSiteEditor) && (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu);
            lnkServerInfo.Text = Resource.AdminMenuServerInfoLabel;
            lnkServerInfo.NavigateUrl = SiteRoot + "/Admin/ServerInformation.aspx";

            liLogViewer.Visible = IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer;
            lnkLogViewer.Text = Resource.AdminMenuServerLogLabel;
            lnkLogViewer.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";

        }




        private void LoadSettings()
        {
            IsAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
            IsAdmin = WebUser.IsAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            commerceConfig = SiteUtils.GetCommerceConfig();
            AddClassToBody("administration");
            AddClassToBody("adminmenu");
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
