/// Author:					Joe Audette
/// Created:				2008-03-26
/// Last Modified:			2012-10-02
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
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{

    public partial class AdminCartBrowserPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private string pageNumberParam;
        //private string sortParam;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        
        private Store store;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected string detailUrlFormat = string.Empty;
        private bool canEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();
            if (!canEdit)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();
            if (store == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            SetupCss();
            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindGrid();

        }

        private void BindGrid()
        {
            if (store == null) return;

            heading.Text = string.Format(CultureInfo.InvariantCulture,
                WebStoreResources.CartBrowserHeadingFormatString,
                store.Name);

            using (IDataReader reader = Cart.GetPage(
                store.Guid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                string pageUrl = SiteUtils.GetNavigationSiteRoot() + "/WebStore/AdminCartBrowser.aspx"
                    + "?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "&amp;pagenumber" + moduleId.ToString(CultureInfo.InvariantCulture)
                    + "={0}";

                pgrCart.PageURLFormat = pageUrl;
                pgrCart.ShowFirstLast = true;
                pgrCart.CurrentIndex = pageNumber;
                pgrCart.PageSize = pageSize;
                pgrCart.PageCount = totalPages;
                pgrCart.Visible = (totalPages > 1);


                grdCart.DataSource = reader;
                grdCart.PageIndex = pageNumber;
                grdCart.PageSize = pageSize;
                grdCart.DataBind();
            }

        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            int daysOld = 90;
            int.TryParse(txtDaysOld.Text, out daysOld);
            DateTime olderThan = DateTime.UtcNow.AddDays(-daysOld);
            if (chkOnlyAnonymous.Checked)
            {
                Cart.DeleteAnonymousByStore(store.Guid, olderThan);
            }
            else
            {
                Cart.DeleteByStore(store.Guid, olderThan);
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if ((c != null) && (store != null))
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/WebStore/AdminDashboard.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.BrowseCartsLink);

            grdCart.Columns[0].HeaderText = WebStoreResources.CartBrowserUserHeading;
            grdCart.Columns[1].HeaderText = WebStoreResources.CartBrowserOrderTotalHeading;
            grdCart.Columns[2].HeaderText = WebStoreResources.CartBrowserCreatedHeading;
            grdCart.Columns[3].HeaderText = WebStoreResources.CartBrowserLastModifiedHeading;

            btnDelete.Text = WebStoreResources.DeleteCartsButton;
            litDays.Text = WebStoreResources.DaysLabel;
            chkOnlyAnonymous.Text = WebStoreResources.CheckBoxAnonymousCarts;
            UIHelper.AddConfirmationDialog(btnDelete, WebStoreResources.DeleteCartsWarning);

            if (!Page.IsPostBack)
            {
                txtDaysOld.Text = "90";
                chkOnlyAnonymous.Checked = true;
            }
            


        }

        private void LoadSettings()
        {
            
            store = StoreHelper.GetStore();
            if (store == null) { return; }

            pageNumberParam = "pagenumber" + this.moduleId.ToInvariantString();
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            detailUrlFormat = SiteUtils.GetNavigationSiteRoot() + "/WebStore/AdminCartDetail.aspx"
                + "?pageid=" + pageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&cart={0}";

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            AddClassToBody("webstore webstorecartbrowser");

        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            canEdit = UserCanEditModule(moduleId, Store.FeatureGuid);

        }

        protected virtual void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("stylewebstore") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "stylewebstore";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl()
                    + "stylewebstore.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
            
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
            btnDelete.Click += new EventHandler(btnDelete_Click);

            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;

        }

        

        #endregion
    }
}
