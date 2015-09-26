/// Author:					Joe Audette
/// Created:				2007-02-15
/// Last Modified:		    2012-10-02
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class AdminStoreSettingsPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store;
        private Module module;
        private SiteUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();

            if ((store == null) || (!UserCanEditModule(moduleId, Store.FeatureGuid)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (!Page.IsPostBack)
            {
                BindDropdownLists();
                PopulateControls();
            }
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            //Store store = new Store(siteSettings.SiteGuid, moduleId);
            //Module module = new Module(moduleId);
            if (store == null) return;
            if (module == null) return;
            if (module.ModuleId == -1) return;

            txtName.Text = module.ModuleTitle;
            edDescription.Text = store.Description;
            txtOwnerName.Text = store.OwnerName;
            txtOwnerEmail.Text = store.OwnerEmail;

            ListItem listItem = ddCountryGuid.Items.FindByValue(store.CountryGuid.ToString());
            if (listItem != null)
            {
                ddCountryGuid.ClearSelection();
                listItem.Selected = true;
                BindZoneList();
            }

            txtAddress.Text = store.Address;
            txtCity.Text = store.City;

            listItem = ddZoneGuid.Items.FindByValue(store.ZoneGuid.ToString());
            if (listItem != null)
            {
                ddZoneGuid.ClearSelection();
                listItem.Selected = true;
            }

            txtPostalCode.Text = store.PostalCode;
            txtPhone.Text = store.Phone;
            txtFax.Text = store.Fax;
            txtSalesEmail.Text = store.SalesEmail;
            txtSupportEmail.Text = store.SupportEmail;
            txtEmailFrom.Text = store.EmailFrom;
            txtOrderBCCEmail.Text = store.OrderBccEmail;

            
            chkIsClosed.Checked = store.IsClosed;
            edClosedMessage.Text = store.ClosedMessage;
            
            

        }

        private void BindDropdownLists()
        {
            BindCountryList();
            BindZoneList();
            
        }

        private void BindCountryList()
        {
            DataTable dataTable = GeoCountry.GetList();
            ddCountryGuid.DataSource = dataTable;
            ddCountryGuid.DataBind();
        }

        private void ddCountryGuid_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindZoneList();
        }

        private void BindZoneList()
        {
            if (ddCountryGuid.SelectedIndex > -1)
            {
                Guid countryGuid = new Guid(ddCountryGuid.SelectedValue);
                using (IDataReader reader = GeoZone.GetByCountry(countryGuid))
                {
                    ddZoneGuid.DataSource = reader;
                    ddZoneGuid.DataBind();
                }
            }
        }

        


        private void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("webstore");
            if ((Page.IsValid)&&(store != null))
            {
              
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                
                if (store.SiteGuid == Guid.Empty)
                {
                    store.SiteGuid = siteSettings.SiteGuid;
                }
                if (store.ModuleId == -1)
                {
                    store.ModuleId = moduleId;
                }

                Module module = new Module(moduleId);
                module.ModuleTitle = txtName.Text;
                module.Save();
                
                store.Name = txtName.Text;
                store.Description = edDescription.Text;
                store.OwnerName = txtOwnerName.Text;
                store.OwnerEmail = txtOwnerEmail.Text;
                store.SalesEmail = txtSalesEmail.Text;
                store.SupportEmail = txtSupportEmail.Text;
                store.EmailFrom = txtEmailFrom.Text;
                store.OrderBccEmail = txtOrderBCCEmail.Text;
                store.Phone = txtPhone.Text;
                store.Fax = txtFax.Text;
                store.Address = txtAddress.Text;
                store.City = txtCity.Text;

                if (!String.IsNullOrEmpty(ddZoneGuid.SelectedValue))
                {
                    store.ZoneGuid = new Guid(ddZoneGuid.SelectedValue);
                }

                store.PostalCode = txtPostalCode.Text;

                if (!String.IsNullOrEmpty(ddCountryGuid.SelectedValue))
                {
                    store.CountryGuid = new Guid(ddCountryGuid.SelectedValue); 
                }

                store.IsClosed = chkIsClosed.Checked;
                store.ClosedMessage = edClosedMessage.Text;
                

                if (store.Guid == Guid.Empty)
                {
                    store.Created = DateTime.UtcNow;
                    store.CreatedBy = siteUser.UserGuid;
                    
                }

                store.Save();

                List<TaxClass> taxClasses = TaxClass.GetList(siteSettings.SiteGuid);

                if (taxClasses.Count == 0)
                {
                    TaxClass taxClass = new TaxClass();
                    taxClass.SiteGuid = siteSettings.SiteGuid;
                    taxClass.Title = WebStoreResources.TaxClassTaxable;
                    taxClass.Description = WebStoreResources.TaxClassTaxable;
                    taxClass.Save();

                    taxClass = new TaxClass();
                    taxClass.SiteGuid = siteSettings.SiteGuid;
                    taxClass.Title = WebStoreResources.TaxClassNotTaxable;
                    taxClass.Description = WebStoreResources.TaxClassNotTaxable;
                    taxClass.Save();


                }

                List<FullfillDownloadTerms> downloadTerms = FullfillDownloadTerms.GetList(store.Guid);
                if (downloadTerms.Count == 0)
                {
                    if(currentUser == null)currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        FullfillDownloadTerms term = new FullfillDownloadTerms();
                        term.Name = WebStoreResources.DownloadUnlimited;
                        term.Description = WebStoreResources.DownloadUnlimited;
                        term.CreatedBy = currentUser.UserGuid;
                        term.CreatedFromIP = SiteUtils.GetIP4Address();
                        term.StoreGuid = store.Guid;
                        term.Save();
                        
                    }

                }

                WebUtils.SetupRedirect(this, Request.RawUrl);


            }
        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null)&&(store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot
                    + "/WebStore/AdminDashboard.aspx?pageid=" 
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.StoreSettingsLink);

            heading.Text = WebStoreResources.StoreSettingsLink;
            btnSave.Text = WebStoreResources.StoreSettingsSaveButton;
            litSettingsTab.Text = WebStoreResources.StoreContactInfoTab;
            litDescriptionTab.Text = WebStoreResources.StoreDescriptionTab;
            litClosedTab.Text = WebStoreResources.StoreOpenClosedTab;
           
            edDescription.WebEditor.ToolBar = ToolBar.Full;
            edDescription.WebEditor.Height = Unit.Parse("220px");

            edClosedMessage.WebEditor.ToolBar = ToolBar.Full;
            edClosedMessage.WebEditor.Height = Unit.Parse("220px");

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            
            store = StoreHelper.GetStore();
            if (store == null) { return; }

            module = GetModule(moduleId, Store.FeatureGuid);
            AddClassToBody("webstore adminstoresettings");
            
        }


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.ddCountryGuid.SelectedIndexChanged += new EventHandler(ddCountryGuid_SelectedIndexChanged);
            SuppressPageMenu();
            SuppressGoogleAds();
            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);
            SiteUtils.SetupEditor(edClosedMessage, AllowSkinOverride, this);
            
        }

        

        

        #endregion
    }
}
