// Author:					Joe Audette
// Created:					2010-03-11
// Last Modified:			2012-10-02
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
using System.Web.UI;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using WebStore.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace WebStore.UI
{

    public partial class OfferListPage : mojoBasePage
    {
        protected Store store;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private bool enableRatingsInProductList = false;
        private bool enableRatingComments = false;
        private Hashtable Settings = null;
        private Module module = null;
        private int pageId = -1;
        private int moduleId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!UserCanViewPage(moduleId))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            LoadSettings();
            if ((store == null) || (store.IsClosed))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferListHeading);

            TitleControl.EditUrl = SiteRoot + "/WebStore/AdminDashboard.aspx";
            TitleControl.EditText = WebStoreResources.StoreManagerLink;
            if (module != null){ TitleControl.ModuleInstance = module; }
            TitleControl.CanEdit = UserCanEditModule(moduleId);

            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null) && (store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                
            }

            litOfferListHeading.Text = WebStoreResources.OfferListHeading;

            

           

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            store = new Store(siteSettings.SiteGuid, moduleId);
            Settings = ModuleSettings.GetModuleSettings(moduleId);
            module = GetModule(moduleId);

            lnkCart.PageID = pageId;
            lnkCart.ModuleID = moduleId;

            enableRatingsInProductList = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableContentRatingInProductListSetting", enableRatingsInProductList);

            enableRatingComments = WebUtils.ParseBoolFromHashtable(
                Settings, "EnableRatingCommentsSetting", enableRatingComments);

            

            offerList1.Store = store;
            offerList1.PageId = pageId;
            offerList1.ModuleId = moduleId;
            offerList1.SiteRoot = SiteRoot;
            offerList1.CurrencyCulture = currencyCulture;
            offerList1.EnableRatings = enableRatingsInProductList;
            offerList1.EnableRatingComments = enableRatingComments;
            offerList1.Settings = Settings;

            AddClassToBody("webstore webstoreofferlist");

            if (WebStoreConfiguration.UseNoIndexFollowMetaOnLists)
            {
                SiteUtils.AddNoIndexFollowMeta(Page);
            }
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

        }


        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


        }

        #endregion
    }
}
