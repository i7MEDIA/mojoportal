// Author:					
// Created:					2012-01-01
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Globalization;



namespace mojoPortal.Web.AdminUI
{

	public partial class PermissionsMenuPage : NonCmsBasePage
    {
        private SiteSettings selectedSite = null;
        private int siteId = -1;
        private string siteParam = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if ((siteId > -1) && (!siteSettings.IsServerAdminSite))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

           
            LoadSettings();
            PopulateLabels();
            //PopulateControls();

        }

        //private void PopulateControls()
        //{


        //}


        private void PopulateLabels()
        {
            
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteSettingsPermissionsTab);

            if (selectedSite.SiteId != siteSettings.SiteId)
            {
                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.SitePermissionFormat, selectedSite.SiteName, Resource.SiteSettingsPermissionsTab);
            }
            else
            {
                heading.Text = Resource.SiteSettingsPermissionsTab;
            }

            

            liSiteEditorRoles.Visible = WebConfigSettings.UseRelatedSiteMode && (selectedSite.SiteId != siteSettings.SiteId);
            lnkSiteEditorRoles.Text = Resource.SiteEditRolesLabel;
            lnkSiteEditorRoles.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.SiteEditor + siteParam;

            lnkRolesThatCanViewCommerceReports.Text = Resource.RolesThatCanViewCommerceReportsLabel;
            lnkRolesThatCanViewCommerceReports.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.ViewCommerceReports + siteParam;

			lnkRolesThatCanBrowseFileSystem.Text = Resource.GeneralBrowseRoles;
			lnkRolesThatCanBrowseFileSystem.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.GeneralBrowse + siteParam;

			lnkRolesThatCanUploadAndBrowse.Text = Resource.GeneralBrowseAndUploadRoles;
            lnkRolesThatCanUploadAndBrowse.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.GeneralBrowseAndUpload + siteParam;

            lnkRolesThatCanUploadAndBrowseUserOnly.Text = Resource.UserFilesBrowseAndUploadRoles;
            lnkRolesThatCanUploadAndBrowseUserOnly.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.UserBrowseAndUpload + siteParam;

            lnkRolesThatCanDeleteFiles.Text = Resource.RolesThatCanDeleteFilesInEditor;
            lnkRolesThatCanDeleteFiles.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.DeleteFiles + siteParam;

            lnkRolesThatCanManageSkins.Text = Resource.RolesThatCanManageSkins;
            lnkRolesThatCanManageSkins.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.ManageSkins + siteParam;

            lnkRolesThatCanAssignSkinsToPages.Text = Resource.RolesThatCanAssignSkinsToPages;
            lnkRolesThatCanAssignSkinsToPages.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.AssignPageSkins + siteParam;

            lnkRolesThatCanEditContentTemplates.Text = Resource.RolesThatCanEditContentTemplates;
            lnkRolesThatCanEditContentTemplates.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.EditContentTemplates + siteParam;

            lnkRolesNOTAllowedInstanceSettings.Text = Resource.RolesNotAllowedToEditModuleSettings;
            lnkRolesNOTAllowedInstanceSettings.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.NoInstanceSettings + siteParam;

            lnkRolesThatCanLookupUsers.Text = Resource.RolesThatCanLookupUsers;
            lnkRolesThatCanLookupUsers.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.LookupUsers + siteParam;

            lnkRolesThatCanCreateUsers.Text = Resource.RolesThatCanCreateUsers;
            lnkRolesThatCanCreateUsers.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.CreateUsers + siteParam;

            lnkRolesThatCanManageUsers.Text = Resource.RolesThatCanManageUsers;
            lnkRolesThatCanManageUsers.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.ManageUsers + siteParam;

            lnkRolesThatCanViewMemberList.Text = Resource.RolesThatCanViewMemberList;
            lnkRolesThatCanViewMemberList.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.ViewMemberList + siteParam;

            liRolesThatCanUseMyPage.Visible = WebConfigSettings.MyPageIsInstalled;
            lnkRolesThatCanUseMyPage.Text = Resource.RolesThatCanViewMyPage;
            lnkRolesThatCanUseMyPage.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.UseMyPage + siteParam;

            lnkRolesThatCanCreateRootLevelPages.Text = Resource.RolesThatCanCreateRootPages;
            lnkRolesThatCanCreateRootLevelPages.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.CreateRootPages + siteParam;

            lnkDefaultRootLevelViewRoles.Text = Resource.DefaultRootPageViewRoles;
            lnkDefaultRootLevelViewRoles.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.ViewRootPages + siteParam;

            lnkDefaultRootLevelEditRoles.Text = Resource.DefaultRootPageEditRoles;
            lnkDefaultRootLevelEditRoles.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.EditRootPages + siteParam;

            lnkDefaultRootLevelCreateChildPageRoles.Text = Resource.DefaultRootPageCreateChildPageRoles;
            lnkDefaultRootLevelCreateChildPageRoles.NavigateUrl = SiteRoot + "/Admin/PermissionEdit.aspx?p=" + CorePermission.CreateChildBelowRootPages + siteParam;


            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkSiteList.Visible = ((WebConfigSettings.AllowMultipleSites) && (siteSettings.IsServerAdminSite));
            lnkSiteList.Text = Resource.SiteList;
            lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";
            litSiteListLinkSeparator.Visible = lnkSiteList.Visible;

            lnkPermissionsMenu.Text = Resource.SiteSettingsPermissionsTab;
            lnkPermissionsMenu.ToolTip = Resource.SiteSettingsPermissionsTab;
            lnkPermissionsMenu.NavigateUrl = SiteRoot + "/Admin/PermissionsMenu.aspx";

            if (siteId > -1)
            {
                lnkPermissionsMenu.NavigateUrl = SiteRoot + "/Admin/PermissionsMenu.aspx?SiteID=" + siteId.ToInvariantString();
            }

        }

        private void LoadSettings()
        {
            if ((siteId > -1)&&(siteSettings.IsServerAdminSite))
            {
                siteParam = "&SiteID=" + siteId.ToInvariantString();

                selectedSite = new SiteSettings(siteId);
                if (selectedSite.SiteId == -1)
                {
                    selectedSite = siteSettings; // not found so use current site
                }
            }
            else
            {
                selectedSite = siteSettings; //currentsite
            }

            AddClassToBody("administration");

        }

        private void LoadParams()
        {
            siteId = WebUtils.ParseInt32FromQueryString("SiteID", siteId);
            //moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

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
