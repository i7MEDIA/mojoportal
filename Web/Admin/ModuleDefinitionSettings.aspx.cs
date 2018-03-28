// Author:					    
// Created:				        2006-05-19
// Last Modified:			    2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ModuleDefinitionSettingsPage : NonCmsBasePage
    {
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.grdSettings.RowEditing += new GridViewEditEventHandler(grdSettings_RowEditing);
            this.grdSettings.RowCancelingEdit += new GridViewCancelEditEventHandler(grdSettings_RowCancelingEdit);
            this.grdSettings.RowUpdating += new GridViewUpdateEventHandler(grdSettings_RowUpdating);
            this.grdSettings.RowDeleting += new GridViewDeleteEventHandler(grdSettings_RowDeleting);
            this.grdSettings.RowDataBound += new GridViewRowEventHandler(grdSettings_RowDataBound);
            this.btnCreateNewSetting.Click += new EventHandler(btnCreateNewSetting_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        

        #endregion

        private static readonly ILog log = LogManager.GetLogger(typeof(ModuleDefinitionSettingsPage));

        private int moduleDefId = -1;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        

        protected void Page_Load(object sender, EventArgs e)
		{
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

            SecurityHelper.DisableBrowserCache();

            if (!siteSettings.IsServerAdminSite)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/AdminMenu.aspx");
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadParams();
            PopulateLabels();

            if (!IsPostBack)
            {
                BindControls();
            }

        }

        protected void BindControls()
        {
            if (this.moduleDefId > -1)
            {
                ModuleDefinition moduleDef = new ModuleDefinition(moduleDefId);
                //if (moduleDef.SiteID != siteSettings.SiteID) return;
                lnkModuleDefinition.Text = ResourceHelper.GetResourceString(moduleDef.ResourceFile, moduleDef.FeatureName);

                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.FeatureSettingsFormat, lnkModuleDefinition.Text);

                
                lnkModuleDefinition.ToolTip = lnkModuleDefinition.Text;
                lnkModuleDefinition.NavigateUrl = SiteRoot + "/Admin/ModuleDefinitions.aspx?defid=" + moduleDefId.ToString();

                ArrayList defSettings = ModuleSettings.GetDefaultSettings(this.moduleDefId);
                this.grdSettings.DataSource = defSettings;
                this.grdSettings.DataBind();

            }
           
        }

        protected void btnCreateNewSetting_Click(object sender, EventArgs e)
        {
            if (this.moduleDefId > -1)
            {
                ModuleDefinition featureDef = new ModuleDefinition(moduleDefId);
                ModuleDefinition.UpdateModuleDefinitionSetting(
                    featureDef.FeatureGuid,
                    this.moduleDefId,
                    this.txtNewResourceFile.Text,
                    txtGroupNameKey.Text,
                    this.txtNewSettingName.Text,
                    this.txtNewSettingValue.Text,
                    this.ddNewControlType.SelectedValue,
                    this.txtNewRegexValidationExpression.Text,
                    this.txtNewControlSrc.Text,
                    this.txtNewHelpKey.Text,
                    Convert.ToInt32(this.txtNewSortOrder.Text),
					this.txtAttributes.Text,
					this.txtOptions.Text);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        protected void grdSettings_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            int settingId = (int)grid.DataKeys[e.RowIndex].Value;

            TextBox txtResourceFile = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtResourceFile");
            TextBox txtGroupNameKey = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtGroupNameKey");
            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSettingName");
            TextBox txtValue = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSettingValue");
            TextBox txtRegex = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRegexValidationExpression");
            DropDownList ddType = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddControlType");

            TextBox txtControlSrc = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtControlSrc");
            TextBox txtHelpKey = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHelpKey");
            TextBox txtSortOrder = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSortOrder");
			TextBox txtAttributes = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtAttributes");
			TextBox txtOptions = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtOptions");

			if (this.moduleDefId > -1)
            {
                ModuleDefinition.UpdateModuleDefinitionSettingById(
                    settingId,
                    this.moduleDefId,
                    txtResourceFile.Text,
                    txtGroupNameKey.Text,
                    txtName.Text,
                    txtValue.Text,
                    ddType.SelectedValue,
                    txtRegex.Text,
                    txtControlSrc.Text,
                    txtHelpKey.Text,
                    Convert.ToInt32(txtSortOrder.Text),
					txtAttributes.Text,
					txtOptions.Text);
          
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            
        }

        protected void grdSettings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int settingID = (int)grid.DataKeys[e.RowIndex].Value;
            ModuleDefinition.DeleteSettingById(settingID);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        protected void grdSettings_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        protected void grdSettings_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindControls();
            

        }


        void grdSettings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button button = e.Row.FindControl("btnGridDelete") as Button;
            UIHelper.AddConfirmationDialog(button, Resource.ModuleDefinitionDeleteSettingWarning);
        }


        protected String GetEditImageAltText()
        {
            return Resource.ModuleDefinitionSettingsEditButton;
        }


        protected String GetEditImageUrl()
        {
            return ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
        }

        protected String GetEditButtonText()
        {
            return Resource.ModuleDefinitionSettingsEditButton;
        }

        protected String GetUpdateButtonText()
        {
            return Resource.ModuleDefinitionSettingsUpdateButton;
        }

        protected String GetDeleteButtonText()
        {
            return Resource.ModuleDefinitionSettingsDeleteButton;
        }

        protected String GetCancelButtonText()
        {
            return Resource.ModuleDefinitionSettingsCancelButton;
        }

        protected void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);

            

            subHeading.Text = Resource.ModuleDefinitionsAddSettingHeader;

            lnkAdminMenu.Text = Resource.AdvancedToolsLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";
            lnkModuleAdmin.Text = Resource.AdminMenuFeatureModulesLink;
            lnkModuleAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";
            btnCreateNewSetting.Text = Resource.ModuleDefinitionsAddSettingButton;
            

        }

        private void LoadParams()
        {
            moduleDefId = WebUtils.ParseInt32FromQueryString("defid", -1);

            AddClassToBody("administration");
            AddClassToBody("featuredefadmin");
            
        }


    }
}
