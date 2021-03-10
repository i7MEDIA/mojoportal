// Created:       2012-01-01
// Last Modified: 2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.AdminUI
{

	public partial class PermissionEditPage : NonCmsBasePage
	{
		int siteId = -1;
		string permissionGuid = string.Empty;
		private SiteSettings selectedSite = null;
		private string currentPermissionRoles = string.Empty;
		private string helpKey = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadParams();

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
			ListItem allItem = new ListItem();
			// localize display
			allItem.Text = Resource.RolesAllUsersRole;
			allItem.Value = "All Users";

			switch (permissionGuid)
			{
				case CorePermission.ViewMemberList:
				case CorePermission.UseMyPage:
				case CorePermission.ViewRootPages:
					if ((currentPermissionRoles.LastIndexOf(allItem.Value + ";")) > -1) { allItem.Selected = true; }
					chkAllowedRoles.Items.Add(allItem);

					break;


			}

			using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
			{
				while (reader.Read())
				{
					string roleName = reader["RoleName"].ToString();

					if (!IncludeRole(roleName)) { continue; }

					ListItem listItem = new ListItem();
					listItem.Text = reader["DisplayName"].ToString();
					listItem.Value = reader["RoleName"].ToString();
					if ((currentPermissionRoles.LastIndexOf(listItem.Value + ";")) > -1) { listItem.Selected = true; }
					chkAllowedRoles.Items.Add(listItem);


				}
			}


		}

		private bool IncludeRole(string roleName)
		{
			if (roleName == Role.AdministratorsRole) { return false; }

			switch (permissionGuid)
			{
				case CorePermission.LookupUsers:
				case CorePermission.CreateUsers:
				case CorePermission.ManageUsers:
				case CorePermission.ViewCommerceReports:
				case CorePermission.ViewMemberList:
					// don't filter content admins out
					break;

				default:
					if (roleName == Role.ContentAdministratorsRole) { return false; }
					break;
			}



			return true;
		}

		void btnSave_Click(object sender, EventArgs e)
		{
			SavePermissions();
		}

		private void SavePermissions()
		{

			switch (permissionGuid)
			{
				case CorePermission.AssignPageSkins:
					selectedSite.RolesThatCanAssignSkinsToPages = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.CreateChildBelowRootPages:
					selectedSite.DefaultRootPageCreateChildPageRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.CreateRootPages:
					selectedSite.RolesThatCanCreateRootPages = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.CreateUsers:
					selectedSite.RolesThatCanCreateUsers = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.DeleteFiles:
					selectedSite.RolesThatCanDeleteFilesInEditor = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.EditContentTemplates:
					selectedSite.RolesThatCanEditContentTemplates = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.EditRootPages:
					selectedSite.DefaultRootPageEditRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.GeneralBrowse:
					selectedSite.GeneralBrowseRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.GeneralBrowseAndUpload:
					selectedSite.GeneralBrowseAndUploadRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.LookupUsers:
					selectedSite.RolesThatCanLookupUsers = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.ManageSkins:
					selectedSite.RolesThatCanManageSkins = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.ManageUsers:
					selectedSite.RolesThatCanManageUsers = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.NoInstanceSettings:
					selectedSite.RolesNotAllowedToEditModuleSettings = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.SiteEditor:
					selectedSite.SiteRootEditRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.UseMyPage:
					selectedSite.RolesThatCanViewMyPage = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.UserBrowseAndUpload:
					selectedSite.UserFilesBrowseAndUploadRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.ViewCommerceReports:
					selectedSite.CommerceReportViewRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.ViewMemberList:
					selectedSite.RolesThatCanViewMemberList = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.ViewRootPages:
					selectedSite.DefaultRootPageViewRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

				case CorePermission.CanManageTags:
					selectedSite.TagManagementRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
					break;

			}

			selectedSite.Save();


			if (WebConfigSettings.UseRelatedSiteMode)
			{

				SiteSettings.SyncRelatedSites(selectedSite, WebConfigSettings.UseFolderBasedMultiTenants);

				// reset the sitesettings cache for each site
				CacheHelper.ClearRelatedSiteCache(-1);
			}
			else
			{
				CacheHelper.ClearSiteSettingsCache(selectedSite.SiteId);
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);

		}


		private void PopulateLabels()
		{
			btnSave.Text = Resource.SaveButton;

			switch (permissionGuid)
			{
				case CorePermission.AssignPageSkins:
					heading.Text = FormatHeading(Resource.RolesThatCanAssignSkinsToPages);
					currentPermissionRoles = selectedSite.RolesThatCanAssignSkinsToPages;
					helpKey = "RolesThatCanAssignSkinsToPages-help";
					break;

				case CorePermission.CreateChildBelowRootPages:
					heading.Text = FormatHeading(Resource.DefaultRootPageCreateChildPageRoles);
					currentPermissionRoles = selectedSite.DefaultRootPageCreateChildPageRoles;
					helpKey = "sitesettings-DefaultRootPageCreateChildPageRoles-help";
					break;

				case CorePermission.CreateRootPages:
					heading.Text = FormatHeading(Resource.RolesThatCanCreateRootPages);
					currentPermissionRoles = selectedSite.RolesThatCanCreateRootPages;
					helpKey = "RolesThatCanCreateRootPages-help";
					break;

				case CorePermission.CreateUsers:
					heading.Text = FormatHeading(Resource.RolesThatCanCreateUsers);
					currentPermissionRoles = selectedSite.RolesThatCanCreateUsers;
					helpKey = "RolesThatCanCreateUsers-help";
					break;

				case CorePermission.DeleteFiles:
					heading.Text = FormatHeading(Resource.RolesThatCanDeleteFilesInEditor);
					currentPermissionRoles = selectedSite.RolesThatCanDeleteFilesInEditor;
					helpKey = "RolesThatCanDeleteFilesInEditor-help";
					break;

				case CorePermission.EditContentTemplates:
					heading.Text = FormatHeading(Resource.RolesThatCanEditContentTemplates);
					currentPermissionRoles = selectedSite.RolesThatCanEditContentTemplates;
					helpKey = "RolesThatCanEditContentTemplates-help";
					break;

				case CorePermission.EditRootPages:
					heading.Text = FormatHeading(Resource.DefaultRootPageEditRoles);
					currentPermissionRoles = selectedSite.DefaultRootPageEditRoles;
					helpKey = "sitesettings-DefaultRootPageEditRoles-help";
					break;

				case CorePermission.GeneralBrowse:
					heading.Text = FormatHeading(Resource.GeneralBrowseRoles);
					currentPermissionRoles = selectedSite.GeneralBrowseRoles;
					helpKey = "GeneralBrowseRoles-help";
					break;

				case CorePermission.GeneralBrowseAndUpload:
					heading.Text = FormatHeading(Resource.GeneralBrowseAndUploadRoles);
					currentPermissionRoles = selectedSite.GeneralBrowseAndUploadRoles;
					helpKey = "GeneralBrowseAndUploadRoles-help";
					break;

				case CorePermission.LookupUsers:
					heading.Text = FormatHeading(Resource.RolesThatCanLookupUsers);
					currentPermissionRoles = selectedSite.RolesThatCanLookupUsers;
					helpKey = "RolesThatCanLookupUsers-help";
					break;

				case CorePermission.ManageSkins:
					heading.Text = FormatHeading(Resource.RolesThatCanManageSkins);
					currentPermissionRoles = selectedSite.RolesThatCanManageSkins;
					helpKey = "RolesThatCanmanageskins-help";
					break;

				case CorePermission.ManageUsers:
					heading.Text = FormatHeading(Resource.RolesThatCanManageUsers);
					currentPermissionRoles = selectedSite.RolesThatCanManageUsers;
					helpKey = "RolesThatCanManageUsers-help";
					break;

				case CorePermission.NoInstanceSettings:
					heading.Text = FormatHeading(Resource.RolesNotAllowedToEditModuleSettings);
					currentPermissionRoles = selectedSite.RolesNotAllowedToEditModuleSettings;
					helpKey = "RolesNotAllowedToEditModuleSettings-help";
					break;

				case CorePermission.SiteEditor:
					heading.Text = FormatHeading(Resource.SiteEditRolesLabel);
					currentPermissionRoles = selectedSite.SiteRootEditRoles;
					helpKey = "sitesettingseditroleshelp";
					break;

				case CorePermission.UseMyPage:
					heading.Text = FormatHeading(Resource.RolesThatCanViewMyPage);
					currentPermissionRoles = selectedSite.RolesThatCanViewMyPage;
					helpKey = "RolesThatCanViewMyPage-help";
					break;

				case CorePermission.UserBrowseAndUpload:
					heading.Text = FormatHeading(Resource.UserFilesBrowseAndUploadRoles);
					currentPermissionRoles = selectedSite.UserFilesBrowseAndUploadRoles;
					helpKey = "UserFilesBrowseAndUploadRoles-help";
					break;

				case CorePermission.ViewCommerceReports:
					heading.Text = FormatHeading(Resource.RolesThatCanViewCommerceReportsLabel);
					currentPermissionRoles = selectedSite.CommerceReportViewRoles;
					//helpKey = "UserFilesBrowseAndUploadRoles-help";
					break;

				case CorePermission.ViewMemberList:
					heading.Text = FormatHeading(Resource.RolesThatCanViewMemberList);
					currentPermissionRoles = selectedSite.RolesThatCanViewMemberList;
					helpKey = "RolesThatCanViewMemberList-help";
					break;

				case CorePermission.ViewRootPages:
					heading.Text = FormatHeading(Resource.DefaultRootPageViewRoles);
					currentPermissionRoles = selectedSite.DefaultRootPageViewRoles;
					helpKey = "sitesettings-DefaultRootPageViewRoles-help";
					break;

			}

			helpLink.HelpKey = helpKey;

			Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

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

		private string FormatHeading(string heading)
		{
			if (selectedSite.SiteId != siteSettings.SiteId)
			{
				return string.Format(CultureInfo.InvariantCulture, Resource.SitePermissionFormat, selectedSite.SiteName, heading);
			}
			else
			{
				return heading;
			}
		}

		private void LoadSettings()
		{
			if ((siteId > -1) && (siteSettings.IsServerAdminSite))
			{
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
			AddClassToBody("permission");

		}

		private void LoadParams()
		{
			siteId = WebUtils.ParseInt32FromQueryString("SiteID", siteId);
			permissionGuid = WebUtils.ParseStringFromQueryString("p", permissionGuid);

		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			btnSave.Click += new EventHandler(btnSave_Click);

			SuppressMenuSelection();
			SuppressPageMenu();


		}



		#endregion
	}
}
