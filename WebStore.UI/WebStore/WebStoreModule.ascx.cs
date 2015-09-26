/// Author:					Joe Audette
/// Created:				2007-02-11
/// Last Modified:		    2009-07-13
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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using mojoPortal.Business.WebHelpers;
using WebStore.Business;

namespace WebStore.UI
{
    
    public partial class WebStoreModule : SiteModuleControl
    {
        protected ModuleTitleControl TitleControl;
        protected Store store;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected WebStoreConfiguration config = new WebStoreConfiguration();
        
        //private bool enableRatingsInProductList = false;
        //private bool enableRatingComments = false;

        const string GroupByProduct = "GroupByProduct";
        const string GroupByOffer = "GroupByOffer";

        //private string groupingMode = GroupByProduct;
        

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Site != null && this.Site.DesignMode) { return; }

            this.Load += new EventHandler(Page_Load);

            LoadSettings();
            productList1.Store = store;
            productList1.PageId = PageId;
            productList1.ModuleId = ModuleId;
            productList1.SiteRoot = SiteRoot;
            productList1.CurrencyCulture = currencyCulture;
            productList1.EnableRatings = config.EnableRatingsInProductList;
            productList1.EnableRatingComments = config.EnableRatingComments;
            productList1.Settings = Settings;


            offerList1.Store = store;
            offerList1.PageId = PageId;
            offerList1.ModuleId = ModuleId;
            offerList1.SiteRoot = SiteRoot;
            offerList1.CurrencyCulture = currencyCulture;
            offerList1.EnableRatings = config.EnableRatingsInProductList;
            offerList1.EnableRatingComments = config.EnableRatingComments;
            offerList1.Settings = Settings;
            

        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Site != null && this.Site.DesignMode) { return; }

            
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            TitleControl.EditUrl = SiteRoot + "/WebStore/AdminDashboard.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            //if (
            //    (store != null)
            //    && (WebUser.IsInRoles(store.StoreConfigRoles))
            //    )
            //{
            //    TitleControl.ShowEditLinkOverride = true;
            //}

            if (store.Guid == Guid.Empty)
            {
                // store not created yet
                pnlStore.Visible = false;
                pnlStoreClosed.Visible = true;

                return;

            }

            if (store.IsClosed)
            {
                litStoredClosed.Text = store.ClosedMessage;
                pnlStoreClosed.Visible = true;
                pnlStore.Visible = false;
                return;
            }

            litStoreDescription.Text = store.Description;

            //PopulateOfferList();
            PopulateSpecials(); 

        }

        //private void PopulateOfferList()
        //{
        //    IDataReader reader = Offer.GetPageForProductList(
        //        store.Guid,
        //        pageNumber,
        //        pageSize,
        //        out totalPages);

        //    //TODO: implement paging 

        //    //if (this.totalPages > 1)
        //    //{
        //    //    // TODO: edit this, fix the path
        //    //    string pageUrl = SiteRoot + "/YourFolder/AdminOffer.aspx"
        //    //        + "?pageid=" + PageId.ToString(CultureInfo.InvariantCulture)
        //    //        + "&amp;mid=" + ModuleId.ToString(CultureInfo.InvariantCulture)
        //    //        + "&amp;pagenumber" + this.ModuleId.ToString(CultureInfo.InvariantCulture)
        //    //        + "={0}";

        //    //    pgrOffer.PageURLFormat = pageUrl;
        //    //    pgrOffer.ShowFirstLast = true;
        //    //    pgrOffer.CurrentIndex = pageNumber;
        //    //    pgrOffer.PageSize = pageSize;
        //    //    pgrOffer.PageCount = totalPages;

        //    //}
        //    //else
        //    //{
        //        pgrOffer.Visible = false;
        //    //}


        //    rptOffers.DataSource = reader;
        //    rptOffers.DataBind();
        //    reader.Close();

        //}

        private void PopulateSpecials()
        {
            using (IDataReader reader = Offer.GetTop10Specials(store.Guid))
            {
                rptSpecials.DataSource = reader;
                rptSpecials.DataBind();
            }

            pnlSpecials.Visible = (rptSpecials.Items.Count > 0);

            

        }


        

        private void PopulateLabels()
        {
            litOfferListHeading.Text = WebStoreResources.OfferListHeading;

            litSpecialsHeading.Text = WebStoreResources.CurrentSpecialsHeading;
            lnkCart.PageID = PageId;
            lnkCart.ModuleID = ModuleId;
            //lnkCart.Text = "Cart";
            //lnkCart.NavigateUrl = Page.ResolveUrl(
            //    "~/WebStore/Cart.aspx?pageid=" + PageID.ToString()
            //    + "&mid=" + ModuleID.ToString());

            TitleControl.EditText = WebStoreResources.StoreManagerLink;
            

            litStoredClosed.Text = WebStoreResources.StoreClosedDefaultMessage;

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            store = new Store(siteSettings.SiteGuid, ModuleId);

            config = new WebStoreConfiguration(Settings);

            //enableRatingsInProductList = WebUtils.ParseBoolFromHashtable(
            //    Settings, "EnableContentRatingInProductListSetting", enableRatingsInProductList);

            //enableRatingComments = WebUtils.ParseBoolFromHashtable(
            //    Settings, "EnableRatingCommentsSetting", enableRatingComments);


            //if (Settings.Contains("ProductListGroupingSetting"))
            //{
            //    groupingMode = Settings["ProductListGroupingSetting"].ToString();
            //}

            switch (config.GroupingMode)
            {
                case GroupByOffer:
                    this.offerList1.Visible = true;
                    this.productList1.Visible = false;

                    break;

                default:

                    this.offerList1.Visible = false;
                    this.productList1.Visible = true;

                    break;

            }

            //pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            //if (store == null) { return; }

            //currencyCulture = ResourceHelper.GetCurrencyCulture(store.DefaultCurrency);
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage.AnalyticsSection.Length == 0)
                {
                    basePage.AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");
                }
            }

        }

        
       
    }
}