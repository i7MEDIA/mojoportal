// Author:					
// Created:					2010-08-06
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
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;



namespace mojoPortal.Web.AdminUI
{

	public partial class FeaturePermissionsPage : NonCmsBasePage
    {
         
        private int moduleDefId = -1;
        private bool isContentAdmin = false;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private SiteModuleDefinition feature = null;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;
            }

            if (feature == null)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
                return;

            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            //if(moduleDefId < 0) { return;}

            ModuleDefinition moduleDef = new ModuleDefinition(moduleDefId);
            

            lnkModuleDefinition.Text = ResourceHelper.GetResourceString(moduleDef.ResourceFile, moduleDef.FeatureName);

            heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.PermissionsFormat, lnkModuleDefinition.Text);

            lnkModuleDefinition.ToolTip = lnkModuleDefinition.Text;
            lnkModuleDefinition.NavigateUrl = SiteRoot + "/Admin/ModuleDefinitions.aspx?defid=" + moduleDefId.ToString();

            if (!IsPostBack) { BindRoles(); }
            

        }

        private void BindRoles()
        {
            chklAllowedRoles.Items.Clear();

            ListItem allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            if (feature.AuthorizedRoles.LastIndexOf("All Users") > -1)
            {
                allItem.Selected = true;
            }

            chklAllowedRoles.Items.Add(allItem);

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();

                    if (feature.AuthorizedRoles.LastIndexOf(listItem.Value + ";") > -1) 
                    {
                        listItem.Selected = true;
                    }

                    chklAllowedRoles.Items.Add(listItem);

                }
            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string newAllowedRoles = chklAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();
            ModuleDefinition.UpdateSiteModulePermissions(siteSettings.SiteId, feature.ModueDefId, newAllowedRoles);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.FeaturePermissions);

            lnkAdminMenu.Text = Resource.AdvancedToolsLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";
            lnkModuleAdmin.Text = Resource.AdminMenuFeatureModulesLink;
            lnkModuleAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";
            litInfo.Text = Resource.FeaturePermissionInfo;

            btnSave.Text = Resource.SaveButton;
        }

        private void LoadSettings()
        {
            moduleDefId = WebUtils.ParseInt32FromQueryString("defid", -1);
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if (moduleDefId > -1) { feature = ModuleDefinition.GetSiteFeature(siteSettings.SiteGuid, moduleDefId); }

            AddClassToBody("administration");
            AddClassToBody("featurepermissions");

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
