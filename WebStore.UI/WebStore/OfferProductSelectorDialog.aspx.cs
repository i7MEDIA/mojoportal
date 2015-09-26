// Author:					Joe Audette
// Created:				    2009-04-25
// Last Modified:			2010-11-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business.Commerce;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using WebStore.Helpers;
using Resources;

namespace WebStore.UI
{
    public partial class OfferProductSelectorDialog : mojoDialogBasePage
    {
        private Store store = null;
        private Guid offerGuid = Guid.Empty;
        //private Offer offer = null;
        private int pageId = -1;
        private int moduleId = -1;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;

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
            SetupProductAddScript();
            PopulateControls();


        }

        private void PopulateControls()
        {
            if (!Page.IsPostBack)
            {
                BindProductList();
            }

        }



        private void BindProductList()
        {
            if (store == null) { return; }

            DataTable dt = Product.GetPageForAdminList(
                store.Guid,
                pageNumber,
                pageSize,
                out totalPages);

            pgrProduct.ShowFirstLast = true;
            pgrProduct.CurrentIndex = pageNumber;
            pgrProduct.PageSize = pageSize;
            pgrProduct.PageCount = totalPages;
            pgrProduct.Visible = (this.totalPages > 1);
           
            grdProduct.DataSource = dt;
            grdProduct.PageIndex = pageNumber;
            grdProduct.PageSize = pageSize;
            grdProduct.DataBind();

        }

        void pgrProduct_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgrProduct.CurrentIndex = pageNumber;
            BindProductList();
            pnlPicker.Update();
        }


        void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnAddProduct = (Button)e.Row.FindControl("btnAddProduct");
            if (btnAddProduct == null) { return; }

            string[] pair = btnAddProduct.CommandArgument.Split('|');
            if (pair.Length != 2) { return; }
            if (pair[0].Length != 36) { return; }

            Guid productGuid = new Guid(pair[0]);
            FulfillmentType fulfillmentType = Product.FulfillmentTypeFromString(pair[1]);
            if (fulfillmentType != FulfillmentType.Download)
            {
                btnAddProduct.Attributes.Add("onclick", GetClientScriptForButton(productGuid, fulfillmentType, Guid.Empty));
            }
           
           
        }

        void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //this should only fire for download products at leastif javascript is enabled
           
            if (e.CommandName != "addProduct") { return; }

            string[] pair = e.CommandArgument.ToString().Split('|');
            if (pair.Length != 2) { return; }
            if (pair[0].Length != 36) { return; }
            
            Guid productGuid = new Guid(pair[0]);
            FulfillmentType fulfillmentType = Product.FulfillmentTypeFromString(pair[1]);

           
            switch (fulfillmentType)
            {
                case FulfillmentType.None:
                case FulfillmentType.PhysicalShipment:

                    //TODO add product to offer as backup if javascript is disabled
                    


                    break;

                case FulfillmentType.Download:
                    ShowFulfillmentTermsPanel(productGuid, fulfillmentType);
                    break;
            }

            pnlPicker.Update();
            

        }

        void btnAddWithDownloadTerms_Click(object sender, EventArgs e)
        {
            // this shouldonly fire if javascript is disabled
            //TODO: add backup implementation to add product if js is disabled
        }

        private void ShowFulfillmentTermsPanel(Guid productGuid, FulfillmentType fulfillmentType)
        {
            pnlDownloadTerms.Visible = true;
            pnlGrid.Visible = false;
            hdnFulfillmentType.Value = ((byte)fulfillmentType).ToString();
            hdnProductGuid.Value = productGuid.ToString();

            Product product = new Product(productGuid);
            lblProduct.Text = product.Name;

            using (IDataReader reader = FullfillDownloadTerms.GetAll(store.Guid))
            {
                ddFulfillTerms.DataSource = reader;
                ddFulfillTerms.DataBind();
            }

        }

        private string GetClientScriptForButton(Guid productGuid, FulfillmentType fulfillmentType, Guid fulfillmentTermsGuid)
        {
            return "top.window.AddProduct('" + productGuid.ToString() 
                + "','" + ((byte)fulfillmentType).ToString() 
                + "','" + fulfillmentTermsGuid.ToString() + "');  return false;";

            
        }

        private void SetupProductAddScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");
            script.Append("function AddProduct() {");

            
            //script.Append("alert(productGuid);");

            script.Append("var hdnP = document.getElementById('" + this.hdnProductGuid.ClientID + "'); ");
            script.Append("var productGuid = hdnP.value; ");

            script.Append("var hdnFt = document.getElementById('" + this.hdnFulfillmentType.ClientID + "'); ");
            script.Append("var fulfillType = hdnFt.value; ");

            script.Append("var hdnFTG = document.getElementById('" + this.ddFulfillTerms.ClientID + "'); ");
            script.Append("var fulfillGuid = hdnFTG.options[hdnFTG.selectedIndex].value; ");


            script.Append("top.window.AddProduct(productGuid,fulfillType,fulfillGuid); ");

            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "prodHandler", script.ToString());

        }

        private void PopulateLabels()
        {
            btnAddWithDownloadTerms.Text = WebStoreResources.AddProductToOfferGridButton;
            btnAddWithDownloadTerms.Attributes.Add("onclick", "AddProduct(); return false;");
            grdProduct.Columns[0].HeaderText = WebStoreResources.ProductModelNumberLabel;
            grdProduct.Columns[1].HeaderText = WebStoreResources.ProductNameLabel;
        }

        private void LoadSettings()
        {
            
            store = StoreHelper.GetStore();
            

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            offerGuid = WebUtils.ParseGuidFromQueryString("offer", offerGuid);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            grdProduct.RowDataBound += new GridViewRowEventHandler(grdProduct_RowDataBound);
            grdProduct.RowCommand += new GridViewCommandEventHandler(grdProduct_RowCommand);
            btnAddWithDownloadTerms.Click += new EventHandler(btnAddWithDownloadTerms_Click);
            pgrProduct.Command += new CommandEventHandler(pgrProduct_Command);

            ScriptConfig.IncludeJQTable = true;
        }

        
        

        

        
    }
}
