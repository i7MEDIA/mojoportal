// Author:					Joe Audette
// Created:					2009-03-03
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
using System.Data.Common;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Business;
using Resources;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{

    public partial class AdminDiscountEditPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private Store store = null;
        private Guid discountGuid = Guid.Empty;
        protected Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        protected SiteUser currentUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (!UserCanEditModule(moduleId, Store.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            if (store == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
            }

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

       

        private void PopulateControls()
        {
            if (discountGuid == Guid.Empty) { return; }

            Discount discount = new Discount(discountGuid);
            
            txtDiscountCode.Text = discount.DiscountCode;
            txtDescription.Text = discount.Description;

            if (timeZone != null)
            {
                dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(discount.ValidityStartDate.ToLocalTime(timeZone).ToString("g"));
                if (discount.ValidityEndDate < DateTime.MaxValue)
                {
                    dpEndDate.Text = DateTimeHelper.LocalizeToCalendar(discount.ValidityEndDate.ToLocalTime(timeZone).ToString("g"));
                }

            }
            else
            {
                dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(discount.ValidityStartDate.AddHours(timeOffset).ToString("g"));
                if (discount.ValidityEndDate < DateTime.MaxValue)
                {
                    dpEndDate.Text = DateTimeHelper.LocalizeToCalendar(discount.ValidityEndDate.AddHours(timeOffset).ToString("g"));
                }
            }
            
            lblCountOfUse.Text = discount.UseCount.ToInvariantString();
            txtMaxCount.Text = discount.MaxCount.ToInvariantString();
            txtMinOrderAmount.Text = discount.MinOrderAmount.ToString("c", currencyCulture);
            txtAbsoluteDiscount.Text = discount.AbsoluteDiscount.ToString("c", currencyCulture);
            txtPercentageDiscount.Text = string.Format(currencyCulture, "{0:0%}", discount.PercentageDiscount);
            ckAllowOtherDiscounts.Checked = discount.AllowOtherDiscounts;
            
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("Discount");
            if ((!Page.IsValid) || (!ValidateBusinessRules())) { return; }

            Discount discount = new Discount(discountGuid);

            discount.DiscountCode = txtDiscountCode.Text;
            discount.Description = txtDescription.Text;
            discount.SiteGuid = siteSettings.SiteGuid;
            discount.ModuleGuid = store.ModuleGuid;
            discount.StoreGuid = store.Guid;

            //TODO: add support for offer specific discounts
            discount.OfferGuid = Guid.Empty;
            if (timeZone != null)
            {
                discount.ValidityStartDate = DateTime.Parse(dpBeginDate.Text).ToUtc(timeZone);
                if (dpEndDate.Text.Length > 0)
                {
                    discount.ValidityEndDate = DateTime.Parse(dpEndDate.Text).ToUtc(timeZone);
                }
                else
                {
                    discount.ValidityEndDate = DateTime.MaxValue;
                }
            }
            else
            {
                discount.ValidityStartDate = DateTime.Parse(dpBeginDate.Text).AddHours(-timeOffset);
                if (dpEndDate.Text.Length > 0)
                {
                    discount.ValidityEndDate = DateTime.Parse(dpEndDate.Text).AddHours(-timeOffset);
                }
                else
                {
                    discount.ValidityEndDate = DateTime.MaxValue;
                }
            }

            try
            {
                discount.AbsoluteDiscount = decimal.Parse(txtAbsoluteDiscount.Text, NumberStyles.Currency, currencyCulture);
            }
            catch (FormatException)
            {
                lblError.Text = WebStoreResources.DiscountInvalidAmountWarning;
                return;
            }

            decimal percentDiscount = 0;
            try
            {
                percentDiscount = decimal.Parse(txtPercentageDiscount.Text.Replace("%", string.Empty), NumberStyles.Number, currencyCulture);
                percentDiscount = percentDiscount / 100;
            }
            catch (FormatException)
            {
                lblError.Text = WebStoreResources.DiscountPercentInvalidWarning;
                return;
            }

            discount.PercentageDiscount = percentDiscount;

            int maxCount = 0;
            int.TryParse(txtMaxCount.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxCount);
            discount.MaxCount = maxCount;

            decimal minOrderAmount = 0;
            decimal.TryParse(txtMinOrderAmount.Text, NumberStyles.Currency, currencyCulture, out minOrderAmount);
            discount.MinOrderAmount = minOrderAmount;
            discount.AllowOtherDiscounts = ckAllowOtherDiscounts.Checked;

            discount.CreatedBy = currentUser.UserGuid;
            discount.LastModBy = currentUser.UserGuid;

            try
            {
                discount.Save();

                string redirectUrl = SiteRoot + "/WebStore/AdminDiscountEdit.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    + "&d=" + discount.DiscountGuid.ToString();

                WebUtils.SetupRedirect(this, redirectUrl);
            }
            catch (DbException)
            {
                // most likely a duplicate contstraint
                lblError.Text = WebStoreResources.DuplicateCodeException;

            }


        }

        private bool ValidateBusinessRules()
        {
            

            // check if the discount code is already used
            Discount d = new Discount(store.ModuleGuid, txtDiscountCode.Text);
            if ((d.DiscountGuid != Guid.Empty) && (d.DiscountGuid != discountGuid))
            {
                lblError.Text = WebStoreResources.DuplicateCodeException;
                return false;

            }

            return true;
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (discountGuid == Guid.Empty) { return; }

            // TODO: should we allow deleting discounts that have been used?
            Discount.Delete(discountGuid);

            string redirectUrl = SiteRoot + "/WebStore/AdminDiscounts.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString();

            WebUtils.SetupRedirect(this, redirectUrl);
            

        }

        


        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom 
                    + crumbs.Separator 
                    + crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot + "/WebStore/AdminDiscounts.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "'>" + WebStoreResources.DiscountAdministration
                    + "</a>" + crumbs.ItemWrapperTop;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.DiscountAdministration);

            btnSave.Text = WebStoreResources.DiscountSaveButton;
            btnDelete.Text = WebStoreResources.DiscountDeleteButton;

            reqDiscountCode.ErrorMessage = WebStoreResources.DiscountCodeRequiredWarning;
            reqDescription.ErrorMessage = WebStoreResources.DiscountDescriptionRequiredWarning;
            reqBeginDate.ErrorMessage = WebStoreResources.DiscountBeginDateRequiredWarning;
            reqMaxUseCount.ErrorMessage = WebStoreResources.DiscountMaxUsesRequiredWarning;
            reqMinOrderAmount.ErrorMessage = WebStoreResources.DiscountMinOrderRequiredFieldWarning;
            reqDiscountAmount.ErrorMessage = WebStoreResources.DiscountAmountRequiredFieldWarning;
            reqPercentDiscount.ErrorMessage = WebStoreResources.DiscountPercentageRequiredFieldWarning;
            lblLeaveBlankForNoEndDate.Text = WebStoreResources.DiscountBlankEndDateHelp;
            lblZeroIsUnlimitedUse.Text = WebStoreResources.DiscountMaxUseHelp;
            lblZeroMeansNoMinimum.Text = WebStoreResources.DiscountMinOrderHelp;

            if (!Page.IsPostBack)
            {
                if (timeZone != null)
                {
                    dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.ToLocalTime(timeZone).ToString("g"));
                }
                else
                {
                    dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddHours(timeOffset).ToString("g"));
                }
                decimal zero = 0;
                txtMinOrderAmount.Text = zero.ToString("c", currencyCulture);
                txtAbsoluteDiscount.Text = zero.ToString("c", currencyCulture);
                txtPercentageDiscount.Text = string.Format(currencyCulture, "{0:0%}", zero);
            }

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            currentUser = SiteUtils.GetCurrentSiteUser();

            store = StoreHelper.GetStore();

            btnDelete.Visible = (discountGuid != Guid.Empty);

            AddClassToBody("webstore webstorediscountedit");
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            discountGuid = WebUtils.ParseGuidFromQueryString("d", discountGuid);
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
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        

        #endregion
    }
}
