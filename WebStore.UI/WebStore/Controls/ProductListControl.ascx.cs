/// Author:					Joe Audette
/// Created:				2008-10-19
/// Last Modified:			2011-11-26
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;

namespace WebStore.UI
{
    //http://schema.org/Product
    //http://schema.org/Offer

    
    public partial class ProductListControl : UserControl
    {
        #region Private Properties

        private int pageId = -1;
        private int moduleId = -1;
        private int pageNumber = 1;
        private int totalPages = 1;
        private int pageSize = 10;
        private Store store = null;
        private string siteRoot = string.Empty;
        protected string teaserFileBaseUrl = string.Empty;
        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private DataSet dsProducts = null;
        private bool enableRatings = false;
        private bool enableRatingComments = false;
        private Hashtable settings = null;

        
        

        #endregion

        #region Public Properties

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public Store Store
        {
            get { return store; }
            set { store = value; }
        }

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        public bool EnableRatings
        {
            get { return enableRatings; }
            set { enableRatings = value; }
        }

        public bool EnableRatingComments
        {
            get { return enableRatingComments; }
            set { enableRatingComments = value; }
        }

        public Hashtable Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible) { return; }

            LoadSettings();

            if ((!Page.IsPostBack) && (ParamsAreValid()))
            {
                BindProducts();
            }
        }

        private void BindProducts()
        {
            
            dsProducts = store.GetProductPageWithOffers(
                pageNumber,
                pageSize,
                out totalPages);


            string pageUrl = SiteUtils.GetNavigationSiteRoot() + "/WebStore/ProductList.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.Visible = (totalPages > 1);
            
            rptProducts.DataSource = dsProducts.Tables["Products"]; 
            rptProducts.DataBind();
            

        }

        void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (dsProducts == null) { return; }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string productGuid = ((DataRowView)e.Item.DataItem).Row.ItemArray[0].ToString();
                Repeater rptOffers = (Repeater)e.Item.FindControl("rptOffers");

                if (rptOffers == null) { return; }

                string whereClause = string.Format("ProductGuid = '{0}'", productGuid);
                DataView dv = new DataView(dsProducts.Tables["ProductOffers"], whereClause, "", DataViewRowState.CurrentRows);

                rptOffers.DataSource = dv;
                rptOffers.DataBind();

               
            }
        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (displaySettings.UsejPlayerForMediaTeasers)
                {
                    basePage.ScriptConfig.IncludejPlayer = true;
                    basePage.ScriptConfig.IncludejPlayerPlaylist = true;
                }
                else
                {
                    basePage.ScriptConfig.IncludeYahooMediaPlayer = true;
                }
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            teaserFileBaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString()
                + "/webstoreproductpreviewfiles/";

            if (Settings != null)
            {
                pageSize = WebUtils.ParseInt32FromHashtable(Settings, "ProductListPageSize", pageSize);
            }

        }

        protected string FormatProductUrl(string productGuid, string url)
        {
            if (WebConfigSettings.UseUrlReWriting)
            {
                return siteRoot + url;
            }

            return siteRoot + "/WebStore/ProductDetail.aspx?pageid=" 
                + pageId.ToInvariantString() 
                + "&amp;mid=" + moduleId.ToInvariantString() 
                + "&amp;product=" + productGuid;
        }

        protected string FormatOfferUrl(string offerGuid, string url)
        {
            if (WebConfigSettings.UseUrlReWriting)
            {
                return siteRoot + url;
            }

            return siteRoot + "/WebStore/OfferDetail.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&offer=" + offerGuid;
        }

        private bool ParamsAreValid()
        {
            if (store == null) { return false; }
            if (pageId == -1) { return false; }
            if (moduleId == -1) { return false; }


            return true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            rptProducts.ItemDataBound += new RepeaterItemEventHandler(rptProducts_ItemDataBound);
        }

        
    }
}