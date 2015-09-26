/// Author:					Joe Audette
/// Created:				2007-02-24
/// Last Modified:			2013-01-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class AdminOfferEditPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminOfferEditPage));

        private int pageId = -1;
        private int moduleId = -1;
        private Store store;
        private Guid offerGuid = Guid.Empty;
        private string virtualRoot;
        private SiteUser siteUser = null;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private int intSortRank1;
        private int intSortRank2;
        ContentMetaRespository metaRepository = new ContentMetaRespository();
        protected WebStoreConfiguration config = new WebStoreConfiguration();
        
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
            

            if (ScriptController != null)
            {
                ScriptController.RegisterAsyncPostBackControl(grdOfferProduct);
                
                ScriptController.RegisterAsyncPostBackControl(grdOfferAvailability);
            }

            
            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");

        }

        private void PopulateControls()
        {
            if (store == null) { return; }

            if (!Page.IsPostBack) 
            {
                PopulateTaxClassList();
                PopulateOfferControls();
                BindProductGrid();
                BindAvailabilityGrid();
                BindMeta();
                BindMetaLinks();
            }

            if (offerGuid == Guid.Empty)
            {
                btnDelete.Visible = false;
                liProducts.Visible = false;
                
                tabAvailability.Visible = false;
                
                tabProducts.Visible = false;
            }


        }

        private void PopulateOfferControls()
        {
            if (store == null) { return; }

            if (offerGuid != Guid.Empty)
            {
                Offer offer = new Offer(offerGuid);
                Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferEditHeading + " - " + offer.Name);
                txtName.Text = offer.Name;
                txtProductListName.Text = offer.ProductListName;
                txtPrice.Text = offer.Price.ToString("c", currencyCulture);
                edAbstract.Text = offer.Teaser;
                edDescription.Text = offer.Description;
                txtSortRank1.Text = offer.SortRank1.ToInvariantString();
                txtSortRank2.Text = offer.SortRank2.ToInvariantString();
                txtMetaDescription.Text = offer.MetaDescription;
                txtMetaKeywords.Text = offer.MetaKeywords;

                ListItem listItem = ddTaxClassGuid.Items.FindByValue(offer.TaxClassGuid.ToString());
                if (listItem != null)
                {
                    ddTaxClassGuid.ClearSelection();
                    listItem.Selected = true;
                }

                chkIsVisible.Checked = offer.IsVisible;
                chkIsSpecial.Checked = offer.IsSpecial;
                chkIsDonation.Checked = offer.IsDonation;
                chkShowDetailLink.Checked = offer.ShowDetailLink;

            }
            
        }


        private void PopulateTaxClassList()
        {
            using (IDataReader reader = TaxClass.GetBySite(siteSettings.SiteGuid))
            {
                ddTaxClassGuid.DataSource = reader;
                ddTaxClassGuid.DataBind();
            }

        }

        private void Save()
        {
            if (store == null) { return; }

            Offer offer;
            if (offerGuid != Guid.Empty)
            {
                offer = new Offer(offerGuid);
            }
            else
            {
                offer = new Offer();
                offer.Created = DateTime.UtcNow;
                offer.CreatedBy = siteUser.UserGuid;
                offer.CreatedFromIP = SiteUtils.GetIP4Address();
            }

            if (config.IndexOffersInSearch)
            {
                offer.ContentChanged += new ContentChangedEventHandler(offer_ContentChanged);
            }

            offer.Name = txtName.Text;
            offer.ProductListName = txtProductListName.Text;
            offer.Teaser = edAbstract.Text;
            offer.Description = edDescription.Text;
            offer.StoreGuid = store.Guid;
            offer.IsVisible = chkIsVisible.Checked;
            offer.IsSpecial = chkIsSpecial.Checked;
            offer.IsDonation = chkIsDonation.Checked;
            offer.ShowDetailLink = chkShowDetailLink.Checked;
            offer.MetaDescription = txtMetaDescription.Text;
            offer.MetaKeywords = txtMetaKeywords.Text;
			
			// If the sort rank inputs are left null on the product
			//		and offer creation screens, the in.Parse gets all
			//		bent out of shape so I converted to TryParse	
            if (int.TryParse(txtSortRank1.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank1))
            {
                offer.SortRank1 = intSortRank1;
            }
            if (int.TryParse(txtSortRank2.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank2))
            {
                offer.SortRank2 = intSortRank2;
            }

            offer.Price = decimal.Parse(txtPrice.Text, NumberStyles.Currency, currencyCulture);

            if (!String.IsNullOrEmpty(ddTaxClassGuid.SelectedValue))
            {
                Guid taxClassGuid = new Guid(ddTaxClassGuid.SelectedValue);
                offer.TaxClassGuid = taxClassGuid;
            }

            offer.LastModified = DateTime.UtcNow;
            offer.LastModifiedBy = siteUser.UserGuid;
            offer.LastModifiedFromIP = SiteUtils.GetIP4Address();

            bool needToCreateFriendlyUrl = false;

            if ((offer.Url.Length == 0) && (txtName.Text.Length > 0))
            {
                offer.Url = "/"
                    + SiteUtils.SuggestFriendlyUrl(
                    txtName.Text + WebStoreResources.OfferUrlSuffix,
                    siteSettings);

                needToCreateFriendlyUrl = true;

            }
            else
            {

                //TODO: change url if title changed?

            }


            if (offer.Save())
            {
                offerGuid = offer.Guid;

                if (needToCreateFriendlyUrl)
                {

                    FriendlyUrl newUrl = new FriendlyUrl();
                    newUrl.SiteId = siteSettings.SiteId;
                    newUrl.SiteGuid = siteSettings.SiteGuid;
                    newUrl.PageGuid = offer.Guid;
                    newUrl.Url = offer.Url.Replace("/", string.Empty);
                    newUrl.RealUrl = "~/WebStore/OfferDetail.aspx?pageid="
                        + pageId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&offer=" + offer.Guid.ToString();

                    newUrl.Save();

                }


            }
            SiteUtils.QueueIndexing();
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        void offer_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["WebStoreOfferIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (offerGuid != Guid.Empty)
            {
                if (config.IndexOffersInSearch)
                {
                    Offer offer = new Offer(offerGuid);

                    if (WebConfigSettings.LogIpAddressForContentDeletions)
                    {
                        log.Info("user deleted offer " + offer.Name + " from ip address " + SiteUtils.GetIP4Address());

                    }

                    ContentChangedEventArgs args = new ContentChangedEventArgs();
                    args.IsDeleted = true;
                    offer_ContentChanged(offer, args);
                }

                FriendlyUrl.DeleteByPageGuid(offerGuid);

                Offer.Delete(
                    offerGuid,
                    siteUser.UserGuid,
                    SiteUtils.GetIP4Address());

                SiteUtils.QueueIndexing();
                WebUtils.SetupRedirect(this, GetReturnUrl());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("Product");
            if ((Page.IsValid)&&(IsValidForSave()))
            {
                Save();
            }

        }

        private bool IsValidForSave()
        {
            decimal priceTest;
            if (!decimal.TryParse(txtPrice.Text, NumberStyles.Currency, currencyCulture, out priceTest))
            {
                lblError.Text = WebStoreResources.OfferPriceInvalidMessage;
                return false;
            }

            return true;
        }

        #region Offer Products

        private void BindProductGrid()
        {
            if (store == null) { return; }

            DataTable dt = OfferProduct.GetByOffer(offerGuid);
            grdOfferProduct.DataSource = dt;
            grdOfferProduct.DataBind();
        }



        private void grdOfferProduct_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (store == null) { return; }

            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;

            if (guid == Guid.Empty)
            {
                grdOfferProduct.EditIndex = -1;
                grdOfferProduct.Columns[2].Visible = true;
                grdOfferProduct.Columns[3].Visible = true;
                BindProductGrid();
                upProducts.Update();
                return;
            }

            HiddenField hdnFType = (HiddenField)grid.Rows[e.RowIndex].Cells[0].FindControl("hdnFType");
            
            DropDownList ddFullFillTerms = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddFullFillTerms");
            TextBox txtQuantity = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtQuantity");
            TextBox txtSortOrder = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtSortOrder");
            FulfillmentType fulfillmentType = Product.FulfillmentTypeFromString(hdnFType.Value);
            OfferProduct offerProduct = new OfferProduct(guid);
            Guid fulfillTermsGuid = Guid.Empty;

            if((fulfillmentType == FulfillmentType.Download)&&(ddFullFillTerms != null))
            {
                if (ddFullFillTerms.SelectedValue.Length == 36)
                {
                    fulfillTermsGuid = new Guid(ddFullFillTerms.SelectedValue);
                }
                offerProduct.FullFillTermsGuid = fulfillTermsGuid;
            }

            int qty;
            if(!int.TryParse(txtQuantity.Text, out qty)) qty = 1;
            offerProduct.Quantity = qty;

            int sort;
            if (!int.TryParse(txtSortOrder.Text, out sort)) sort = 100;
            offerProduct.SortOrder = sort;
            if (siteUser == null) siteUser = SiteUtils.GetCurrentSiteUser();
            offerProduct.LastModifiedBy = siteUser.UserGuid;
            offerProduct.LastModifiedFromIP = SiteUtils.GetIP4Address();
            offerProduct.Save();
           
            grdOfferProduct.EditIndex = -1;
            grdOfferProduct.Columns[2].Visible = true;
            grdOfferProduct.Columns[3].Visible = true;
            BindProductGrid();
            upProducts.Update();
           

        }

        private void grdOfferProduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            if (siteUser == null) siteUser = SiteUtils.GetCurrentSiteUser();
            OfferProduct.Delete(guid, siteUser.UserGuid, SiteUtils.GetIP4Address());
            
            grdOfferProduct.EditIndex = -1;
            grdOfferProduct.Columns[2].Visible = true;
            grdOfferProduct.Columns[3].Visible = true;
            BindProductGrid();
            upProducts.Update();

        }

        private void grdOfferProduct_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdOfferProduct.EditIndex = -1;
            grdOfferProduct.Columns[2].Visible = true;
            grdOfferProduct.Columns[3].Visible = true;
            BindProductGrid();
            
            upProducts.Update();
        }

        private void grdOfferProduct_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (store == null) { return; }

            grdOfferProduct.Columns[3].Visible = false;
            grdOfferProduct.Columns[2].Visible = false;

            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindProductGrid();

            Guid guid = (Guid)grid.DataKeys[e.NewEditIndex].Value;
            HiddenField hdnFType = (HiddenField)grid.Rows[e.NewEditIndex].Cells[0].FindControl("hdnFType");
            HiddenField hdnFTerms = (HiddenField)grid.Rows[e.NewEditIndex].Cells[0].FindControl("hdnFTerms");
            DropDownList ddFullfillTerms = (DropDownList)grid.Rows[e.NewEditIndex].Cells[1].FindControl("ddFullFillTerms");
            Panel divFulfillment = (Panel)grid.Rows[e.NewEditIndex].Cells[1].FindControl("divFulfillment");
            FulfillmentType fulfillmentType = Product.FulfillmentTypeFromString(hdnFType.Value);
          
            if (fulfillmentType == FulfillmentType.Download)
            {

                using (IDataReader reader = FullfillDownloadTerms.GetAll(store.Guid))
                {
                    ddFullfillTerms.DataSource = reader;
                    ddFullfillTerms.DataBind();
                }
            }
            else
            {
                divFulfillment.Visible = false;
            }

            if ((guid != Guid.Empty)&&(fulfillmentType == FulfillmentType.Download))
            {
                if ((hdnFTerms != null) && (hdnFTerms.Value.Length == 36))
                {
                    ListItem listItem = ddFullfillTerms.Items.FindByValue(hdnFTerms.Value);
                    if (listItem != null)
                    {
                        ddFullfillTerms.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnGridDelete");
            if (btnDelete != null)
            {
                if (guid == Guid.Empty)
                {
                    btnDelete.Visible = false;
                }
                else
                {
                    btnDelete.Attributes.Add("OnClick", "return confirm('"
                        + WebStoreResources.OfferProductGridDeleteWarning + "');");
                }
            }

            
            upProducts.Update();
    
        }

        void btnAddFromGreyBox_Click(object sender, ImageClickEventArgs e)
        {
            Guid productGuid = Guid.Empty;
            FulfillmentType fType = FulfillmentType.None;
            Guid fulfillmentTermsGuid = Guid.Empty;

            if (hdnProductGuid.Value.Length == 36)
            {
                productGuid = new Guid(hdnProductGuid.Value);
            }

            if (hdnFulfillmentType.Value.Length == 1)
            {
                fType = Product.FulfillmentTypeFromString(hdnFulfillmentType.Value);
            }

            if (hdnFulfillmentTermsGuid.Value.Length == 36)
            {
                fulfillmentTermsGuid = new Guid(hdnFulfillmentTermsGuid.Value);
            }

            if ((offerGuid != Guid.Empty) && (productGuid != Guid.Empty))
            {
                OfferProduct offerProduct = new OfferProduct();
                offerProduct.CreatedBy = siteUser.UserGuid;
                offerProduct.CreatedFromIP = SiteUtils.GetIP4Address();
                offerProduct.ProductGuid = productGuid;
                offerProduct.FullfillType = (byte)fType;
                offerProduct.FullFillTermsGuid = fulfillmentTermsGuid;
                offerProduct.Quantity = 1;
                offerProduct.SortOrder = 100;
                offerProduct.OfferGuid = offerGuid;
                offerProduct.Save();

            }

            hdnFulfillmentTermsGuid.Value = string.Empty;
            hdnFulfillmentType.Value = string.Empty;
            hdnProductGuid.Value = string.Empty;

            BindProductGrid();
            upProducts.Update();
        }

        //private void btnAddProductToOffer_Click(object sender, EventArgs e)
        //{
        //    if (store == null) { return; }

        //    btnAddProductToOffer.Visible = false;
            
        //    DataTable dataTable = new DataTable();

        //    dataTable.Columns.Add("Guid", typeof(Guid));
        //    dataTable.Columns.Add("OfferGuid", typeof(Guid));
        //    dataTable.Columns.Add("ProductGuid", typeof(Guid));
        //    dataTable.Columns.Add("FullfillType", typeof(byte));
        //    dataTable.Columns.Add("FullFillTermsGuid", typeof(Guid));
        //    dataTable.Columns.Add("Quantity", typeof(int));
        //    dataTable.Columns.Add("SortOrder", typeof(int));
        //    dataTable.Columns.Add("Name", typeof(string));
        //    DataRow row = dataTable.NewRow();

        //    row["Guid"] = Guid.Empty;
        //    row["OfferGuid"] = offerGuid;
        //    row["ProductGuid"] = Guid.Empty;
        //    row["FullfillType"] = 1;
        //    row["FullFillTermsGuid"] = Guid.Empty;
        //    row["Quantity"] = 1;
        //    row["SortOrder"] = 100;
        //    row["Name"] = string.Empty;
            
        //    dataTable.Rows.Add(row);
        //    grdOfferProduct.Columns[3].Visible = false;
        //    grdOfferProduct.Columns[2].Visible = false;
        //    grdOfferProduct.EditIndex = 0;
        //    grdOfferProduct.DataSource = dataTable.DefaultView;
        //    grdOfferProduct.DataBind();

        //    DropDownList ddProduct = (DropDownList)grdOfferProduct.Rows[grdOfferProduct.EditIndex].Cells[0].FindControl("ddProduct");

        //    using (IDataReader reader = Product.GetAll(store.Guid))
        //    {
        //        ddProduct.DataSource = reader;
        //        ddProduct.DataBind();
        //    }

        //    DropDownList ddFullfillTerms = (DropDownList)grdOfferProduct.Rows[grdOfferProduct.EditIndex].Cells[0].FindControl("ddFullFillTerms");
        //    using (IDataReader reader = FullfillDownloadTerms.GetAll(store.Guid))
        //    {
        //        ddFullfillTerms.DataSource = reader;
        //        ddFullfillTerms.DataBind();
        //    }

        //    Button btnDelete = (Button)grdOfferProduct.Rows[grdOfferProduct.EditIndex].Cells[0].FindControl("btnGridDelete");
        //    if (btnDelete != null) btnDelete.Visible = false;

        //    upProducts.Update();

        //}


        #endregion

        

        #region Availability

        private void BindAvailabilityGrid()
        {
            if (offerGuid != Guid.Empty)
            {
                DataTable dt = OfferAvailability.GetByOffer(offerGuid);
                grdOfferAvailability.DataSource = dt;
                grdOfferAvailability.DataBind();

            }

        }



        private void grdOfferAvailability_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            DatePickerControl dpBeginUTC = (DatePickerControl)grid.Rows[e.RowIndex].Cells[1].FindControl("dpBeginDate");
            DatePickerControl dpEndUTC = (DatePickerControl)grid.Rows[e.RowIndex].Cells[2].FindControl("dpEndDate");
            CheckBox chkRequiresOfferCode = (CheckBox)grid.Rows[e.RowIndex].Cells[3].FindControl("chkRequiresOfferCode");
            TextBox txtOfferCode = (TextBox)grid.Rows[e.RowIndex].Cells[4].FindControl("txtOfferCode");
            TextBox txtMaxAllowedPerCustomer = (TextBox)grid.Rows[e.RowIndex].Cells[5].FindControl("txtMaxAllowedPerCustomer");

            OfferAvailability offerAvailability;
            if (guid != Guid.Empty)
            {
                offerAvailability = new OfferAvailability(guid);
                
            }
            else
            {
                offerAvailability = new OfferAvailability();
                offerAvailability.OfferGuid = offerGuid;
                offerAvailability.CreatedBy = siteUser.UserGuid;
                offerAvailability.CreatedFromIP = SiteUtils.GetIP4Address();
            }

            if (timeZone != null)
            {
                offerAvailability.BeginUtc = DateTime.Parse(dpBeginUTC.Text).ToUtc(timeZone);
                if (dpEndUTC.Text.Length > 0)
                {
                    offerAvailability.EndUtc = DateTime.Parse(dpEndUTC.Text).ToUtc(timeZone);
                }
            }
            else
            {
                offerAvailability.BeginUtc = DateTime.Parse(dpBeginUTC.Text).AddHours(-timeOffset);
                if (dpEndUTC.Text.Length > 0)
                {
                    offerAvailability.EndUtc = DateTime.Parse(dpEndUTC.Text).AddHours(-timeOffset);
                }
            }
            offerAvailability.RequiresOfferCode = chkRequiresOfferCode.Checked;
            offerAvailability.OfferCode = txtOfferCode.Text;
            offerAvailability.MaxAllowedPerCustomer = int.Parse(txtMaxAllowedPerCustomer.Text);
            
            offerAvailability.LastModifedBy = siteUser.UserGuid;
            offerAvailability.LastModifedFromIP = SiteUtils.GetIP4Address();
            offerAvailability.Save();

            //WebUtils.SetupRedirect(this, Request.RawUrl);

            grdOfferAvailability.EditIndex = -1;
            btnAddNewAvailability.Visible = true;
            BindAvailabilityGrid();
            upAvailability.Update();

        }

        private void grdOfferAvailability_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            OfferAvailability.Delete(
                guid, 
                siteUser.UserGuid, 
                DateTime.UtcNow, 
                SiteUtils.GetIP4Address());

            //WebUtils.SetupRedirect(this, Request.RawUrl);

            grdOfferAvailability.EditIndex = -1;
            btnAddNewAvailability.Visible = true;
            BindAvailabilityGrid();
            upAvailability.Update();

        }

        private void grdOfferAvailability_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            
            grdOfferAvailability.EditIndex = -1;
            btnAddNewAvailability.Visible = true;
            BindAvailabilityGrid();
            upAvailability.Update();

        }

        private void grdOfferAvailability_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.NewEditIndex].Value;
            grid.EditIndex = e.NewEditIndex;
            BindAvailabilityGrid();

            Button btnGridDelete = (Button)grid.Rows[e.NewEditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnGridDelete != null)
            {
                if (guid == Guid.Empty)
                {
                    btnDelete.Visible = false;
                }
                else
                {
                    btnGridDelete.Attributes.Add("OnClick", "return confirm('"
                        + WebStoreResources.OfferAvailabilityGridDeleteWarning + "');");
                }
            }

            btnAddNewAvailability.Visible = false;
           
            upAvailability.Update();
        }

        private void btnAddNewAvailability_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("OfferGuid", typeof(Guid));
            dataTable.Columns.Add("BeginUTC", typeof(DateTime));
            dataTable.Columns.Add("EndUTC", typeof(DateTime));
            dataTable.Columns.Add("RequiresOfferCode", typeof(bool));
            dataTable.Columns.Add("OfferCode", typeof(string));
            dataTable.Columns.Add("MaxAllowedPerCustomer", typeof(int));
            
            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["OfferGuid"] = offerGuid;
            //row["BeginUTC"] = 
            //row["EndUTC"] = 
            row["RequiresOfferCode"] = false;
            row["OfferCode"] = string.Empty;
            row["MaxAllowedPerCustomer"] = 0;
            dataTable.Rows.Add(row);

            
            grdOfferAvailability.EditIndex = 0;
            grdOfferAvailability.DataSource = dataTable.DefaultView;
            grdOfferAvailability.DataBind();

            btnAddNewAvailability.Visible = false;
            
            Button btnDelete = (Button)grdOfferAvailability.Rows[grdOfferAvailability.EditIndex].Cells[0].FindControl("btnGridDelete");
            if (btnDelete != null) btnDelete.Visible = false;

            upAvailability.Update();

        }

        #endregion

        #region Meta Data

        private void BindMeta()
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            List<ContentMeta> meta = metaRepository.FetchByContent(offer.Guid);
            grdContentMeta.DataSource = meta;
            grdContentMeta.DataBind();

            btnAddMeta.Visible = true;
        }

        void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMeta meta = metaRepository.Fetch(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMeta> metaList = metaRepository.FetchByContent(offer.Guid);
            metaRepository.ResortMeta(metaList);

            offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
            offer.Save();

            BindMeta();
            upMeta.Update();


        }



        void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.Delete(guid);

            offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
            offer.Save();
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void grdContentMeta_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMeta();

            //Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDeleteMeta = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMeta");
            if (btnDeleteMeta != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.ContentMetaDeleteWarning + "');");

                //if (guid == Guid.Empty) { btnDeleteMeta.Visible = false; }
            }

            upMeta.Update();
        }

        void grdContentMeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddDirection = (DropDownList)e.Row.Cells[1].FindControl("ddDirection");
                    if (ddDirection != null)
                    {
                        if (e.Row.DataItem is ContentMeta)
                        {
                            ListItem item = ddDirection.Items.FindByValue(((ContentMeta)e.Row.DataItem).Dir);
                            if (item != null)
                            {
                                ddDirection.ClearSelection();
                                item.Selected = true;
                            }
                        }

                    }

                    if (!(e.Row.DataItem is ContentMeta))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMeta = (Button)e.Row.Cells[1].FindControl("btnDeleteMeta");
                        if (btnDeleteMeta != null) { btnDeleteMeta.Visible = false; }

                    }

                }

            }

        }

        void grdContentMeta_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
            TextBox txtScheme = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtScheme");
            TextBox txtLangCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtLangCode");
            DropDownList ddDirection = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddDirection");
            TextBox txtMetaContent = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContent");

            ContentMeta meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.Fetch(guid);
            }
            else
            {
                meta = new ContentMeta();
                if (siteUser != null) { meta.CreatedBy = siteUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextSortRank(offer.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = offer.Guid;
                meta.Dir = ddDirection.SelectedValue;
                meta.LangCode = txtLangCode.Text;
                meta.MetaContent = txtMetaContent.Text;
                meta.Name = txtName.Text;
                meta.Scheme = txtScheme.Text;
                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
                offer.Save();

            }

            grid.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();

        }

        void grdContentMeta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdContentMeta.EditIndex = -1;
            grdContentMeta.Columns[2].Visible = true;
            BindMeta();
            upMeta.Update();
        }

        void btnAddMeta_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Scheme", typeof(string));
            dataTable.Columns.Add("LangCode", typeof(string));
            dataTable.Columns.Add("Dir", typeof(string));
            dataTable.Columns.Add("MetaContent", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Name"] = string.Empty;
            row["Scheme"] = string.Empty;
            row["LangCode"] = string.Empty;
            row["Dir"] = string.Empty;
            row["MetaContent"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdContentMeta.EditIndex = 0;
            grdContentMeta.DataSource = dataTable.DefaultView;
            grdContentMeta.DataBind();
            grdContentMeta.Columns[2].Visible = false;
            btnAddMeta.Visible = false;

            upMeta.Update();

        }

        private void BindMetaLinks()
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(offer.Guid);

            grdMetaLinks.DataSource = meta;
            grdMetaLinks.DataBind();

            btnAddMetaLink.Visible = true;
        }

        void btnAddMetaLink_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ContentGuid", typeof(Guid));
            dataTable.Columns.Add("Rel", typeof(string));
            dataTable.Columns.Add("Href", typeof(string));
            dataTable.Columns.Add("HrefLang", typeof(string));
            dataTable.Columns.Add("SortRank", typeof(int));

            DataRow row = dataTable.NewRow();
            row["Guid"] = Guid.Empty;
            row["SiteGuid"] = siteSettings.SiteGuid;
            row["ModuleGuid"] = Guid.Empty;
            row["ContentGuid"] = Guid.Empty;
            row["Rel"] = string.Empty;
            row["Href"] = string.Empty;
            row["HrefLang"] = string.Empty;
            row["SortRank"] = 3;

            dataTable.Rows.Add(row);

            grdMetaLinks.Columns[2].Visible = false;
            grdMetaLinks.EditIndex = 0;
            grdMetaLinks.DataSource = dataTable.DefaultView;
            grdMetaLinks.DataBind();
            btnAddMetaLink.Visible = false;

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (grid.EditIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (!(e.Row.DataItem is ContentMetaLink))
                    {
                        //the add button was clicked so hide the delete button
                        Button btnDeleteMetaLink = (Button)e.Row.Cells[1].FindControl("btnDeleteMetaLink");
                        if (btnDeleteMetaLink != null) { btnDeleteMetaLink.Visible = false; }

                    }

                }

            }
        }

        void grdMetaLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.DeleteLink(guid);

            offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
            offer.Save();

            grid.Columns[2].Visible = true;
            BindMetaLinks();

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMetaLinks.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;

            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            TextBox txtRel = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRel");
            TextBox txtHref = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHref");
            TextBox txtHrefLang = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHrefLang");

            ContentMetaLink meta = null;
            if (guid != Guid.Empty)
            {
                meta = metaRepository.FetchLink(guid);
            }
            else
            {
                meta = new ContentMetaLink();
                if (siteUser != null) { meta.CreatedBy = siteUser.UserGuid; }
                meta.SortRank = metaRepository.GetNextLinkSortRank(offer.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = offer.Guid;
                meta.Rel = txtRel.Text;
                meta.Href = txtHref.Text;
                meta.HrefLang = txtHrefLang.Text;

                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
                offer.Save();

            }

            grid.EditIndex = -1;
            grdMetaLinks.Columns[2].Visible = true;
            BindMetaLinks();
            updMetaLinks.Update();
        }

        void grdMetaLinks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;

            BindMetaLinks();

            Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

            Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMetaLink");
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + WebStoreResources.ContentMetaLinkDeleteWarning + "');");

                if (guid == Guid.Empty) { btnDelete.Visible = false; }
            }

            updMetaLinks.Update();
        }

        void grdMetaLinks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (offerGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Offer offer = new Offer(offerGuid);
            if (offer.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            string sGuid = e.CommandArgument.ToString();
            if (sGuid.Length != 36) { return; }

            Guid guid = new Guid(sGuid);
            ContentMetaLink meta = metaRepository.FetchLink(guid);
            if (meta == null) { return; }

            switch (e.CommandName)
            {
                case "MoveUp":
                    meta.SortRank -= 3;
                    break;

                case "MoveDown":
                    meta.SortRank += 3;
                    break;

            }

            metaRepository.Save(meta);
            List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(offer.Guid);
            metaRepository.ResortMeta(metaList);

            offer.CompiledMeta = metaRepository.GetMetaString(offer.Guid);
            offer.Save();

            BindMetaLinks();
            updMetaLinks.Update();
        }


        #endregion
        

        
        private void PopulateLabels()
        {
            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs = crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot + "/WebStore/AdminDashboard.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.StoreManagerLink
                    + "</a>" + crumbs.ItemWrapperBottom 
                    + crumbs.Separator 
                    + crumbs.ItemWrapperTop + "<a href='"
                    + SiteRoot + "/WebStore/AdminOffer.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.OfferAdministrationLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.OfferEditHeading);
            heading.Text = WebStoreResources.OfferEditHeading;
            btnSave.Text = WebStoreResources.OfferUpdateButton;
            UIHelper.AddClearPageExitCode(btnSave);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            btnDelete.Text = WebStoreResources.OfferDeleteButton;
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, WebStoreResources.OfferDeleteWarning);

            edAbstract.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edAbstract.WebEditor.Height = Unit.Parse("350px");

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edDescription.WebEditor.Height = Unit.Parse("350px");

            btnAddNewAvailability.Text = WebStoreResources.OfferAvailabilityGridAddNewButton;

            grdOfferAvailability.Columns[1].HeaderText = WebStoreResources.OfferAvailabilityBeginDateHeader;
            grdOfferAvailability.Columns[2].HeaderText = WebStoreResources.OfferAvailabilityEndDateHeader;
            grdOfferAvailability.Columns[3].HeaderText = WebStoreResources.OfferAvailabilityRequiresOfferCodeHeader;
            grdOfferAvailability.Columns[4].HeaderText = WebStoreResources.OfferAvailabilityOfferCodeHeader;
            grdOfferAvailability.Columns[5].HeaderText = WebStoreResources.OfferAvailabilityMaxAllowedPerCustomerHeader;


            //btnAddProductToOffer.Text = WebStoreResources.OfferProductGridAddNewButton;

            grdOfferProduct.Columns[1].HeaderText = WebStoreResources.OfferProductGridNameHeader;
            grdOfferProduct.Columns[2].HeaderText = WebStoreResources.OfferProductGridFullfillTypeHeader;
            grdOfferProduct.Columns[3].HeaderText = WebStoreResources.OfferProductGridQuantityHeader;

            

            litSettingsTab.Text = WebStoreResources.OfferEditGeneralSettingsLabel;
            litAbstactTab.Text = WebStoreResources.ProductAbstractTab;
            litDescriptionTab.Text = WebStoreResources.OfferDescriptionTab;
            litProductsTab.Text = WebStoreResources.OfferProductsLabel;
            litMetaTab.Text = WebStoreResources.MetaDataTab;
            
            lnkProducts.HRef = "#" + tabProducts.ClientID;

            if (!Page.IsPostBack)
            {
                decimal defaultPrice = 0;
                txtPrice.Text = defaultPrice.ToString("c", currencyCulture);
            }

            reqName.ErrorMessage = WebStoreResources.OfferNameRequired;
            reqPrice.ErrorMessage = WebStoreResources.OfferPriceRequired;

            btnAddFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnAddFromGreyBox.Attributes.Add("tabIndex", "-1");
            btnAddFromGreyBox.AlternateText = " ";
            UIHelper.AddClearPageExitCode(btnAddFromGreyBox);

            //lnkAddProducts.Text = WebStoreResources.OfferProductGridAddNewButton;
            //lnkAddProducts.ToolTip = WebStoreResources.OfferProductGridAddNewButton;
            //lnkAddProducts.DialogCloseText = WebStoreResources.CloseDialogButton;
            //lnkAddProducts.NavigateUrl = SiteRoot + "/WebStore/OfferProductSelectorDialog.aspx?pageid="
            //    + pageId.ToInvariantString()
            //    + "&mid=" + moduleId.ToInvariantString()
            //    + "&offer=" + offerGuid.ToString();

            lnkProductsAdd.Text = WebStoreResources.OfferProductGridAddNewButton;
            lnkProductsAdd.ToolTip = WebStoreResources.OfferProductGridAddNewButton;
            lnkProductsAdd.NavigateUrl = SiteRoot + "/WebStore/OfferProductSelectorDialog.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&offer=" + offerGuid.ToString();

            

            btnAddMeta.Text = WebStoreResources.AddMetaButton;
            grdContentMeta.Columns[0].HeaderText = string.Empty;
            grdContentMeta.Columns[1].HeaderText = WebStoreResources.ContentMetaNameLabel;
            grdContentMeta.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaContentLabel;

            btnAddMetaLink.Text = WebStoreResources.AddMetaLinkButton;

            grdMetaLinks.Columns[0].HeaderText = string.Empty;
            grdMetaLinks.Columns[1].HeaderText = WebStoreResources.ContentMetaRelLabel;
            grdMetaLinks.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaHrefLabel;
            
          
        }

        

        protected string GetRefreshUrl()
        {

            string result = SiteRoot + "/WebStore/AdminOfferEdit.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&offer=" + offerGuid.ToString();

            return result;

        }

        private string GetReturnUrl()
        {
            return SiteRoot + "/WebStore/AdminOffer.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();
        }


        private void SetupProductAddScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("function AddProduct(productGuid, fulfillType, fulfillGuid) {");

            //script.Append("GB_hide();");
            //script.Append("alert(fulfillType);");

            script.Append("var hdnP = document.getElementById('" + this.hdnProductGuid.ClientID + "'); ");
            script.Append("hdnP.value = productGuid; ");

            script.Append("var hdnFt = document.getElementById('" + this.hdnFulfillmentType.ClientID + "'); ");
            script.Append("hdnFt.value = fulfillType; ");

            script.Append("var hdnFTG = document.getElementById('" + this.hdnFulfillmentTermsGuid.ClientID + "'); ");
            script.Append("hdnFTG.value = fulfillGuid; ");

            script.Append("var btn = document.getElementById('" + this.btnAddFromGreyBox.ClientID + "');  ");
            script.Append("btn.click(); ");
            script.Append("$.colorbox.close(); ");

            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "prodHandler", script.ToString());

        }
        

        private void LoadSettings()
        {
            ScriptConfig.IncludeColorBox = true;
            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            
            store = StoreHelper.GetStore();
            if (store == null) { return; }
            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new WebStoreConfiguration(moduleSettings);
                
           

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            siteUser = SiteUtils.GetCurrentSiteUser();
            offerGuid = WebUtils.ParseGuidFromQueryString("offer", offerGuid);

            if (offerGuid == Guid.Empty)
            {
                lnkProductsAdd.Visible = false;
            }
            else
            {
                SetupProductAddScript();
            }

            virtualRoot = WebUtils.GetApplicationRoot();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            if (store == null) { return; }

            AddClassToBody("webstore adminofferedit");

            //currencyCulture = ResourceHelper.GetCurrencyCulture(store.DefaultCurrency);
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            

        }

        protected String GetBeginDate(Object o)
        {
            if ((o == null) || (o.ToString() == String.Empty))
            {
                if (timeZone != null)
                {
                    return DateTime.UtcNow.ToLocalTime(timeZone).ToString("g");
                }
                return DateTime.UtcNow.AddHours(timeOffset).ToString("g");

            }
            if (o is DateTime)
            {
                if (timeZone != null)
                {
                    return ((DateTime)o).ToLocalTime(timeZone).ToString("g");
                }
                return ((DateTime)o).AddHours(timeOffset).ToString("g");
            }

            return o.ToString();

        }

        

        #region OnInit

        

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
            SiteUtils.SetupEditor(edAbstract, AllowSkinOverride, this);
            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);
        }

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnAddNewAvailability.Click += new EventHandler(btnAddNewAvailability_Click);
            
          
            this.grdOfferAvailability.RowEditing += new GridViewEditEventHandler(grdOfferAvailability_RowEditing);
            this.grdOfferAvailability.RowCancelingEdit += new GridViewCancelEditEventHandler(grdOfferAvailability_RowCancelingEdit);
            this.grdOfferAvailability.RowUpdating += new GridViewUpdateEventHandler(grdOfferAvailability_RowUpdating);
            this.grdOfferAvailability.RowDeleting += new GridViewDeleteEventHandler(grdOfferAvailability_RowDeleting);

            this.grdOfferProduct.RowEditing += new GridViewEditEventHandler(grdOfferProduct_RowEditing);
            this.grdOfferProduct.RowCancelingEdit += new GridViewCancelEditEventHandler(grdOfferProduct_RowCancelingEdit);
            this.grdOfferProduct.RowUpdating += new GridViewUpdateEventHandler(grdOfferProduct_RowUpdating);
            this.grdOfferProduct.RowDeleting += new GridViewDeleteEventHandler(grdOfferProduct_RowDeleting);
            //this.btnAddProductToOffer.Click += new EventHandler(btnAddProductToOffer_Click);
            btnAddFromGreyBox.Click += new ImageClickEventHandler(btnAddFromGreyBox_Click);

            SuppressPageMenu();
            SuppressGoogleAds();

            grdContentMeta.RowCommand += new GridViewCommandEventHandler(grdContentMeta_RowCommand);
            grdContentMeta.RowEditing += new GridViewEditEventHandler(grdContentMeta_RowEditing);
            grdContentMeta.RowUpdating += new GridViewUpdateEventHandler(grdContentMeta_RowUpdating);
            grdContentMeta.RowCancelingEdit += new GridViewCancelEditEventHandler(grdContentMeta_RowCancelingEdit);
            grdContentMeta.RowDeleting += new GridViewDeleteEventHandler(grdContentMeta_RowDeleting);
            grdContentMeta.RowDataBound += new GridViewRowEventHandler(grdContentMeta_RowDataBound);
            btnAddMeta.Click += new EventHandler(btnAddMeta_Click);

            grdMetaLinks.RowCommand += new GridViewCommandEventHandler(grdMetaLinks_RowCommand);
            grdMetaLinks.RowEditing += new GridViewEditEventHandler(grdMetaLinks_RowEditing);
            grdMetaLinks.RowUpdating += new GridViewUpdateEventHandler(grdMetaLinks_RowUpdating);
            grdMetaLinks.RowCancelingEdit += new GridViewCancelEditEventHandler(grdMetaLinks_RowCancelingEdit);
            grdMetaLinks.RowDeleting += new GridViewDeleteEventHandler(grdMetaLinks_RowDeleting);
            grdMetaLinks.RowDataBound += new GridViewRowEventHandler(grdMetaLinks_RowDataBound);
            btnAddMetaLink.Click += new EventHandler(btnAddMetaLink_Click);

            ScriptConfig.IncludeJQTable = true;
        }

        

        

        #endregion
    }
}
