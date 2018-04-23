/// Author:				        
/// Created:			        2004-08-24
///	Last Modified:              2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI 
{
    public partial class ModuleDefinitions : NonCmsBasePage
	{
        private int moduleDefinitionId = -1;
        private int pageId = -1;
        //private string iconPath;

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
            this.cancelButton.Click += new EventHandler(this.CancelBtn_Click);
            this.deleteButton.Click += new EventHandler(this.DeleteBtn_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            
        }
        #endregion
       
        private void Page_Load(object sender, EventArgs e)
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
            //SetupIconScript();
            
            if (!IsPostBack)
			{
                PopulateControls();
            }
        }

	
		private void PopulateControls()
		{

			if (moduleDefinitionId > -1) 
			{
				ModuleDefinition moduleDefinition = new ModuleDefinition(this.moduleDefinitionId);
				txtFeatureName.Text = moduleDefinition.FeatureName;
                txtResourceFile.Text = moduleDefinition.ResourceFile;
                txtFeatureGuid.Text = moduleDefinition.FeatureGuid.ToString();
				this.txtControlSource.Text = moduleDefinition.ControlSrc;
                this.txtSortOrder.Text = moduleDefinition.SortOrder.ToString(CultureInfo.InvariantCulture);
                this.txtDefaultCacheDuration.Text = moduleDefinition.DefaultCacheTime.ToString(CultureInfo.InvariantCulture);
				this.chkIsAdmin.Checked = moduleDefinition.IsAdmin;
                //if (moduleDefinition.Icon.Length > 0)
                //{
                //    ddIcons.SelectedValue = moduleDefinition.Icon;
                //    imgIcon.Src = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/" + moduleDefinition.Icon);

                //}
                //else
                //{
                //    imgIcon.Src = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/blank.gif");
                //}

                chkIsCacheable.Checked = moduleDefinition.IsCacheable;
                chkIsSearchable.Checked = moduleDefinition.IsSearchable;
                txtSearchListName.Text = moduleDefinition.SearchListName;

				
			}
			else 
			{
                txtFeatureName.Text = Resource.ModuleDefinitionsDefaultModuleName;
                txtControlSource.Text = Resource.ModuleDefinitionsDefaultControlSource;
                txtFeatureGuid.Text = Guid.NewGuid().ToString();
                txtSortOrder.Text = "500";
                this.txtDefaultCacheDuration.Text = "0";
                this.lnkConfigureSettings.Visible = false;
                //imgIcon.Src = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/blank.gif");
                this.deleteButton.Visible = false;
				
			}

		}


        private void UpdateBtn_Click(Object sender, EventArgs e)
		{
            if (Page.IsValid) 
			{
				ModuleDefinition moduleDefinition = new ModuleDefinition(this.moduleDefinitionId);
                if (txtFeatureGuid.Text.Length == 36)
                {
                    moduleDefinition.FeatureGuid = new Guid(txtFeatureGuid.Text);
                }
				moduleDefinition.SiteId = siteSettings.SiteId;
				moduleDefinition.FeatureName = this.txtFeatureName.Text;
                moduleDefinition.ResourceFile = txtResourceFile.Text;
				moduleDefinition.ControlSrc = this.txtControlSource.Text;
                moduleDefinition.SortOrder = int.Parse(this.txtSortOrder.Text, CultureInfo.InvariantCulture);
                moduleDefinition.DefaultCacheTime = int.Parse(this.txtDefaultCacheDuration.Text, CultureInfo.InvariantCulture);
				moduleDefinition.IsAdmin = this.chkIsAdmin.Checked;
                //moduleDefinition.Icon = this.ddIcons.SelectedValue;
                moduleDefinition.IsCacheable = chkIsCacheable.Checked;
                moduleDefinition.IsSearchable = chkIsSearchable.Checked;
                moduleDefinition.SearchListName = txtSearchListName.Text;

				moduleDefinition.Save();

                string redirectUrl = SiteRoot 
                    + "/Admin/ModuleDefinitions.aspx?defid=" 
                    + moduleDefinition.ModuleDefId.ToString(CultureInfo.InvariantCulture);

                WebUtils.SetupRedirect(this, redirectUrl);
            }
        }

        
        private void DeleteBtn_Click(Object sender, EventArgs e) 
		{
            lblErrorMessage.Text = String.Empty;

            int countOfUse = Module.GetCountByFeature(moduleDefinitionId);
            if (countOfUse > 0)
            {
                lblErrorMessage.Text = Resource.ModuleDefinitionsDeleteInstancesBeforeModuleDefinitionMessage;
                return;
            }
            try
            {
                
                ModuleDefinition.DeleteModuleDefinition(moduleDefinitionId);
                ModuleDefinition.DeleteSettingsByFeature(moduleDefinitionId);

                string redirectUrl;
                if (pageId > -1)
                {
                    redirectUrl = SiteRoot + "/Default.aspx?pageid=" + pageId.ToInvariantString();
                }
                else
                {
                    redirectUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";
                }
                WebUtils.SetupRedirect(this, redirectUrl);
        
            }
            catch (DbException)
            {
                lblErrorMessage.Text = Resource.ModuleDefinitionsDeleteInstancesBeforeModuleDefinitionMessage;
            }
            
        }

       

        private void CancelBtn_Click(Object sender, EventArgs e)
		{
            string redirectUrl;
            if (pageId > -1)
            {
                redirectUrl = SiteRoot + "/Default.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                redirectUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";
            }
            

            WebUtils.SetupRedirect(this, redirectUrl);
        }

        //private void SetupIconScript()
        //{
        //    string logoScript = "<script type=\"text/javascript\">"
        //        + "function showIcon(listBox) { if(!document.images) return; "
        //        + "var iconPath = '" + iconPath + "'; "
        //        + "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
        //        + "}</script>";

        //    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showIcon", logoScript);

        //}

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuFeatureModulesLink);

            heading.Text = Resource.ModuleDefinitionsModuleDefinitionLabel;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";
            lnkModuleAdmin.Text = Resource.AdminMenuFeatureModulesLink;
            lnkModuleAdmin.ToolTip = Resource.AdminMenuFeatureModulesLink;
            lnkModuleAdmin.NavigateUrl = SiteRoot + "/Admin/ModuleAdmin.aspx";

            reqFeatureName.ErrorMessage = Resource.ModuleDefinitionsFeatureNameRequiredHelp;
            reqControlSource.ErrorMessage = Resource.ModuleDefinitionsControlSourceRequiredHelp;

            reqSortOrder.ErrorMessage = Resource.ModuleDefinitionSortRequiredMessage;
            reqDefaultCache.ErrorMessage = Resource.ModuleDefinitionDefaultCacheRequiredMessage;
            regexSortOrder.ErrorMessage = Resource.ModuleDefinitionSortRegexWarning;
            regexCacheDuration.ErrorMessage = Resource.ModuleDefinitionDefaultCacheRegexWarning;

            updateButton.Text = Resource.ModuleDefinitionsUpdateButton;
            SiteUtils.SetButtonAccessKey(updateButton, AccessKeys.ModuleDefinitionsUpdateButtonAccessKey);

            cancelButton.Text = Resource.ModuleDefinitionsCancelButton;
            SiteUtils.SetButtonAccessKey(cancelButton, AccessKeys.ModuleDefinitionsCancelButtonAccessKey);

            deleteButton.Text = Resource.ModuleDefinitionsDeleteButton;
            SiteUtils.SetButtonAccessKey(deleteButton, AccessKeys.ModuleDefinitionsDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(deleteButton, Resource.ModuleDefinitionsDeleteWarning);

            lnkConfigureSettings.Text = Resource.ModuleDefinitionsConfigureLink;
            lnkConfigureSettings.NavigateUrl = SiteRoot
                + "/Admin/ModuleDefinitionSettings.aspx?defid="
                + this.moduleDefinitionId.ToString(CultureInfo.InvariantCulture);

            //if (!Page.IsPostBack)
            //{
            //    FileInfo[] fileInfo = SiteUtils.GetFeatureIconList();
            //    this.ddIcons.DataSource = fileInfo;
            //    this.ddIcons.DataBind();

            //    ddIcons.Items.Insert(0, new ListItem(Resource.ModuleSettingsNoIconLabel, "blank.gif"));
            //    ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
            //    ddIcons.Attributes.Add("size", "6");
            //}

            
           
        }


        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleDefinitionId = WebUtils.ParseInt32FromQueryString("defid", -1);
            //iconPath = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/");

            AddClassToBody("administration");
            AddClassToBody("featureadmin");
        }
        
    }
}
