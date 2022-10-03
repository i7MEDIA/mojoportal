// Author:                     
// Created:                    2012-10-15
//	Last Modified:             2013-09-12
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public partial class PageContentWizard : UserControl
    {
        private SiteSettings siteSettings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
            {
                BindFeatureList();
            }
        }


        private void BindFeatureList()
        {
            if (siteSettings == null) { return; }

            using (IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId))
            {
                ListItem listItem;
                while (reader.Read())
                {
                    string allowedRoles = reader["AuthorizedRoles"].ToString();
                    if (WebUser.IsInRoles(allowedRoles))
                    {
                        listItem = new ListItem(
                            ResourceHelper.GetResourceString(
                            reader["ResourceFile"].ToString(),
                            reader["FeatureName"].ToString()),
                            reader["ModuleDefID"].ToString());

                        moduleType.Items.Add(listItem);
                    }

                }

            }

        }

        void btnCreateNewContent_Click(object sender, EventArgs e)
        {
            Page.Validate("contentwizard");
            if (!Page.IsValid) { return; }

            int moduleDefID = int.Parse(moduleType.SelectedItem.Value);
            ModuleDefinition moduleDefinition = new ModuleDefinition(moduleDefID);
            PageSettings CurrentPage = CacheHelper.GetCurrentPage();

            Module m = new Module();
            m.SiteId = siteSettings.SiteId;
            m.SiteGuid = siteSettings.SiteGuid;
            m.ModuleDefId = moduleDefID;
            m.FeatureGuid = moduleDefinition.FeatureGuid;
            m.Icon = moduleDefinition.Icon;
            m.CacheTime = moduleDefinition.DefaultCacheTime;
            m.PageId = CurrentPage.PageId;
            m.ModuleTitle = moduleTitle.Text;
			m.ShowTitle = chkShowTitle.Checked;
            m.PaneName = "contentpane";
            //m.AuthorizedEditRoles = "Admins";
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser != null)
            {
                m.CreatedByUserId = currentUser.UserId;
            }
            //m.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
            m.HeadElement = WebConfigSettings.ModuleTitleTag;
            m.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        private void PopulateLabels()
        {
            litInstructions.Text = Resource.ContentWizardInstructions;
            btnCreateNewContent.Text = Resource.MyPageAddContentLabel;

            reqModuleTitle.ErrorMessage = Resource.TitleRequiredWarning;
            reqModuleTitle.Enabled = WebConfigSettings.RequireContentTitle;

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
			chkShowTitle.Checked = WebConfigSettings.ShowModuleTitlesByDefault;

		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnCreateNewContent.Click += btnCreateNewContent_Click;
        }

        
    }
}