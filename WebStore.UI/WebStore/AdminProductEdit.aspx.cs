/// Author:					Joe Audette
/// Created:				2007-02-24
/// Last Modified:			2015-04-13 (Joe Davis)
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
using mojoPortal.FileSystem;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{

    public partial class AdminProductEditPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminProductEditPage));

        private int pageId = -1;
        private int moduleId = -1;
        private Store store;
        private Guid productGuid = Guid.Empty;
        private string virtualRoot;
        private SiteUser siteUser = null;
        private string upLoadPath;
        private string teaserFileBasePath = string.Empty;
        private int intSortRank1;
        private int intSortRank2;
        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        ContentMetaRespository metaRepository = new ContentMetaRespository();
        private IFileSystem fileSystem = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if ((!UserCanEditModule(moduleId, Store.FeatureGuid))||(store == null))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
           
            PopulateLabels();
            PopulateControls();
            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsWebStoreSection", "store");
        }

        

        private void PopulateControls()
        {
            if (!Page.IsPostBack)
            {
                PopulateTaxClassList();
                PopulateStatusList();
                PopulateFullfillmentTypeList();
                PopulateProductControls();
                BindMeta();
                BindMetaLinks();
            }

            if (productGuid == Guid.Empty)
            {
                //new product
                
                btnDelete.Visible = false;
            }



        }

        private void PopulateProductControls()
        {
            if (store == null) { return; }

            Product product;
            ListItem listItem;

            if (productGuid != Guid.Empty)
            {
                product = new Product(productGuid);
                listItem = ddTaxClassGuid.Items.FindByValue(product.TaxClassGuid.ToString());
                if (listItem != null)
                {
                    ddTaxClassGuid.ClearSelection();
                    listItem.Selected = true;
                }
                
                Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductEditHeading + " - " + product.Name);
                heading.Text += Server.HtmlEncode(" : " + product.Name);
                txtName.Text = product.Name;
                edAbstract.Text = product.Teaser;
                edDescription.Text = product.Description;
                txtMetaDescription.Text = product.MetaDescription;
                txtMetaKeywords.Text = product.MetaKeywords;
                //txtSku.Text = product.Sku.ToString();
                txtModelNumber.Text = product.ModelNumber.ToString();
                chkShowInProductList.Checked = product.ShowInProductList;
                chkEnableRating.Checked = product.EnableRating;

                listItem = ddStatus.Items.FindByValue(((int)product.Status).ToString());
                if (listItem != null)
                {
                    ddStatus.ClearSelection();
                    listItem.Selected = true;
                }

                listItem = ddFullfillmentType.Items.FindByValue(((int)product.FulfillmentType).ToString());
                if (listItem != null)
                {
                    ddFullfillmentType.ClearSelection();
                    listItem.Selected = true;
                }

                if (product.FulfillmentType == FulfillmentType.Download)
                {
                    pnlUpload.Visible = true;

                    ProductFile productFile = new ProductFile(productGuid);
                    if (productFile.ProductGuid != Guid.Empty)
                    {
                        lnkDownload.Text = productFile.FileName;
                        lnkDownload.NavigateUrl = SiteRoot + "/WebStore/ProductDownload.aspx?pageid=" 
                            + pageId.ToInvariantString()
                            + "&mid=" + moduleId.ToInvariantString()
                            + "&prod=" + productFile.ProductGuid.ToString();
                    }

                    
                }

                if (product.TeaserFile.Length > 0)
                {
                    lnkTeaserDownload.NavigateUrl = teaserFileBasePath + product.TeaserFile;
                    lnkTeaserDownload.Visible = true;
                    if (product.TeaserFileLink.Length > 0)
                    {
                        lnkTeaserDownload.Text = product.TeaserFileLink;
                    }
                    else
                    {
                        lnkTeaserDownload.Text = product.TeaserFile;
                    }
                    txtTeaserFileLinkText.Text = product.TeaserFileLink;
                    btnDeleteTeaser.Visible = true;
                }

                txtWeight.Text = product.Weight.ToString();
                txtShippingAmount.Text = product.ShippingAmount.ToString("c", currencyCulture);
                txtQuantityOnHand.Text = product.QuantityOnHand.ToInvariantString();
                txtSortRank1.Text = product.SortRank1.ToInvariantString();
                txtSortRank2.Text = product.SortRank2.ToInvariantString();

                
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

        private void PopulateStatusList()
        {
            ListItem listItem = new ListItem();
            listItem.Value = "1";
            listItem.Text = WebStoreResources.ProductStatusEnumAvailable;
            ddStatus.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "2";
            listItem.Text = WebStoreResources.ProductStatusEnumDiscontinued;
            ddStatus.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "3";
            listItem.Text = WebStoreResources.ProductStatusEnumPlanned;
            ddStatus.Items.Add(listItem);
        }

        private void PopulateFullfillmentTypeList()
        {

            ListItem listItem = new ListItem();

            listItem.Value = "3";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumNone;
            ddFullfillmentType.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "1";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumDownload;
            ddFullfillmentType.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Value = "2";
            listItem.Text = WebStoreResources.FullfillmentTypeEnumShipped;
            ddFullfillmentType.Items.Add(listItem);

           
        }

        private void Save()
        {
            if (store == null) { return; }

            Product product;
            if (productGuid != Guid.Empty)
            {
                product = new Product(productGuid);
            }
            else
            {
                product = new Product();
                product.StoreGuid = store.Guid;
                if (siteUser != null)
                {
                    product.CreatedBy = siteUser.UserGuid;
                    product.Created = DateTime.UtcNow;
                }
            }

            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

            if (ddTaxClassGuid.SelectedIndex > -1)
            {
                Guid taxClassGuid = new Guid(ddTaxClassGuid.SelectedValue);
                product.TaxClassGuid = taxClassGuid;
            }

            product.Name = txtName.Text;
            product.Teaser = edAbstract.Text;
            product.Description = edDescription.Text;
            product.MetaDescription = txtMetaDescription.Text;
            product.MetaKeywords = txtMetaKeywords.Text;

            //product.Sku = txtSku.Text; //TODO: Changed in db so sku don't need to be unique, maybe change back later (IX_ws_Product)
            product.ModelNumber = txtModelNumber.Text;
            product.Status = Product.ProductStatusFromString(ddStatus.SelectedValue);

            product.FulfillmentType
                = Product.FulfillmentTypeFromString(ddFullfillmentType.SelectedValue);

            decimal weight;
            if (!decimal.TryParse(
                txtWeight.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out weight))
            {
                weight = 0;
            }
            product.Weight = weight;

            decimal shippingAmount;
            if (!decimal.TryParse(
                txtShippingAmount.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out shippingAmount))
            {
                shippingAmount = 0;
            }
            product.ShippingAmount = shippingAmount;

            int qty;
            if (!int.TryParse(txtQuantityOnHand.Text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out qty))
            {
                qty = 1;
            }
            product.QuantityOnHand = qty;
            product.ShowInProductList = chkShowInProductList.Checked;
            product.EnableRating = chkEnableRating.Checked;

            product.TeaserFileLink = txtTeaserFileLinkText.Text;

            if (int.TryParse(txtSortRank1.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank1))
            {
                product.SortRank1 = intSortRank1;
            }
            if (int.TryParse(txtSortRank2.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out intSortRank2))
            {
                product.SortRank2 = intSortRank2;
            }
            

            product.LastModified = DateTime.UtcNow;
            if (siteUser != null)
            {
                product.LastModifedBy = siteUser.UserGuid;
            }

            bool needToCreateFriendlyUrl = false;

            if ((product.Url.Length == 0) && (txtName.Text.Length > 0))
            {
                product.Url = "/"
                    + SiteUtils.SuggestFriendlyUrl(
                    txtName.Text + WebStoreResources.ProductUrlSuffix,
                    siteSettings);

                needToCreateFriendlyUrl = true;

            }
            else
            {

                //TODO: change url if title changed?

            }

            if (product.Save())
            {
                productGuid = product.Guid;

                if (needToCreateFriendlyUrl)
                {

                    FriendlyUrl newUrl = new FriendlyUrl();
                    newUrl.SiteId = siteSettings.SiteId;
                    newUrl.SiteGuid = siteSettings.SiteGuid;
                    newUrl.PageGuid = product.Guid;
                    newUrl.Url = product.Url.Replace("/", string.Empty);
                    newUrl.RealUrl = "~/WebStore/ProductDetail.aspx?pageid="
                        + pageId.ToInvariantString()
                        + "&mid=" + moduleId.ToInvariantString()
                        + "&product=" + product.Guid.ToString();

                    newUrl.Save();

                }

            }
            SiteUtils.QueueIndexing();
            WebUtils.SetupRedirect(this, GetRefreshUrl());

        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("Product");
            if (Page.IsValid)
            {
                Save();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product p = new Product(productGuid);

            if (WebConfigSettings.LogIpAddressForContentDeletions)
            {
                log.Info("user deleted product " + p.Name + " from ip address " + SiteUtils.GetIP4Address());

            }

            FriendlyUrl.DeleteByPageGuid(p.Guid);

            ContentChangedEventArgs args = new ContentChangedEventArgs();
            args.IsDeleted = true;
            product_ContentChanged(p, args);

            Product.Delete(
                productGuid,
                siteUser.UserGuid,
                SiteUtils.GetIP4Address());

            SiteUtils.QueueIndexing();
            WebUtils.SetupRedirect(this, GetReturnUrl());

        }

        void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["WebStoreProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (productUploader.HasFile)
            {
                ProductFile productFile = new ProductFile(productGuid);
                productFile.FileName = Path.GetFileName(productUploader.FileName);
                productFile.ByteLength = productUploader.FileBytes.Length;
                productFile.Created = DateTime.UtcNow;
                productFile.CreatedBy = siteUser.UserGuid;
                productFile.ServerFileName = productGuid.ToString() + ".config";

                string ext = System.IO.Path.GetExtension(productUploader.FileName);
                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                if (productFile.Save())
                {
                    string destPath = upLoadPath + productFile.ServerFileName;
                    fileSystem.DeleteFile(destPath);

                    using (productUploader.FileContent)
                    {
                        fileSystem.SaveFile(destPath, productUploader.FileContent, mimeType, true);
                    }
                    

                    WebUtils.SetupRedirect(this, GetRefreshUrl());
                }

            }


        }

        void btnUploadTeaser_Click(object sender, EventArgs e)
        {

            if (teaserUploader.HasFile)
            {
                Product product = new Product(productGuid);
                product.TeaserFile = Path.GetFileName(teaserUploader.FileName).ToCleanFileName();
                product.TeaserFileLink = txtTeaserFileLinkText.Text;

                string ext = System.IO.Path.GetExtension(teaserUploader.FileName);
                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                if (product.Save())
                {
                    string destPath = teaserFileBasePath + Path.GetFileName(teaserUploader.FileName).ToCleanFileName();
                    fileSystem.DeleteFile(destPath);

                    using (teaserUploader.FileContent)
                    {
                        fileSystem.SaveFile(destPath, teaserUploader.FileContent, mimeType, true);
                    }

                    

                    WebUtils.SetupRedirect(this, GetRefreshUrl());
                }

            }

        }

        void btnDeleteTeaser_Click(object sender, EventArgs e)
        {
           Product product = new Product(productGuid);
           if (fileSystem.FileExists(teaserFileBasePath + product.TeaserFile))
           {
               fileSystem.DeleteFile(teaserFileBasePath + product.TeaserFile);
               product.TeaserFile = string.Empty;
               product.Save();
           }

           WebUtils.SetupRedirect(this, GetRefreshUrl());
        }


        #region Meta Data

        private void BindMeta()
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            List<ContentMeta> meta = metaRepository.FetchByContent(product.Guid);
            grdContentMeta.DataSource = meta;
            grdContentMeta.DataBind();

            btnAddMeta.Visible = true;
        }

        void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

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
            List<ContentMeta> metaList = metaRepository.FetchByContent(product.Guid);
            metaRepository.ResortMeta(metaList);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

            BindMeta();
            upMeta.Update();


        }



        void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.Delete(guid);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();
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
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

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
                meta.SortRank = metaRepository.GetNextSortRank(product.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = product.Guid;
                meta.Dir = ddDirection.SelectedValue;
                meta.LangCode = txtLangCode.Text;
                meta.MetaContent = txtMetaContent.Text;
                meta.Name = txtName.Text;
                meta.Scheme = txtScheme.Text;
                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
                product.Save();

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
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(product.Guid);

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
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

            GridView grid = (GridView)sender;
            Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
            metaRepository.DeleteLink(guid);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

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
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

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
                meta.SortRank = metaRepository.GetNextLinkSortRank(product.Guid);
                meta.ModuleGuid = store.ModuleGuid;
            }

            if (meta != null)
            {
                meta.SiteGuid = siteSettings.SiteGuid;
                meta.ContentGuid = product.Guid;
                meta.Rel = txtRel.Text;
                meta.Href = txtHref.Text;
                meta.HrefLang = txtHrefLang.Text;

                if (siteUser != null) { meta.LastModBy = siteUser.UserGuid; }
                metaRepository.Save(meta);

                product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
                product.Save();

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
            if (productGuid == Guid.Empty) { return; }
            if (store == null) { return; }
            Product product = new Product(productGuid);
            if (product.StoreGuid != store.Guid) { return; }

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
            List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(product.Guid);
            metaRepository.ResortMeta(metaList);

            product.CompiledMeta = metaRepository.GetMetaString(product.Guid);
            product.Save();

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
                    + SiteRoot + "/WebStore/AdminProduct.aspx?pageid=" + pageId.ToInvariantString() + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + WebStoreResources.ProductAdministrationLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, WebStoreResources.ProductEditHeading);

            heading.Text = WebStoreResources.ProductEditHeading;
            btnSave.Text = WebStoreResources.ProductUpdateButton;
            btnSave.ToolTip = WebStoreResources.ProductUpdateButton;
            UIHelper.AddClearPageExitCode(btnSave);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            btnDelete.Text = WebStoreResources.ProductDeleteButton;
            btnDelete.ToolTip = WebStoreResources.ProductDeleteButton;
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, WebStoreResources.ProductDeleteWarning);

            btnUpload.Text = WebStoreResources.FileUploadButton;
            btnUploadTeaser.Text = WebStoreResources.FileUploadButton;
            btnDeleteTeaser.Text = WebStoreResources.DeleteButton;
            btnDeleteTeaser.Attributes.Add("OnClick", "return confirm('" + WebStoreResources.ProductDeleteTeaserWarning + "');");

            edAbstract.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edAbstract.WebEditor.Height = Unit.Parse("350px");
           
            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edDescription.WebEditor.Height = Unit.Parse("350px");

            litSettingsTab.Text = WebStoreResources.ProductSettingsTab;
            litAbstactTab.Text = WebStoreResources.ProductAbstractTab;
            litDescriptionTab.Text = WebStoreResources.ProductDescriptionTab;
            litFullfillmentTab.Text = WebStoreResources.ProductFullfillmentTypeLabel;
            litMetaTab.Text = WebStoreResources.MetaDataTab;
            

            lnkFullfillment.HRef = "#" + tabFullfillment.ClientID;

            btnAddMeta.Text = WebStoreResources.AddMetaButton;
            grdContentMeta.Columns[0].HeaderText = string.Empty;
            grdContentMeta.Columns[1].HeaderText = WebStoreResources.ContentMetaNameLabel;
            grdContentMeta.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaContentLabel;

            btnAddMetaLink.Text = WebStoreResources.AddMetaLinkButton;

            grdMetaLinks.Columns[0].HeaderText = string.Empty;
            grdMetaLinks.Columns[1].HeaderText = WebStoreResources.ContentMetaRelLabel;
            grdMetaLinks.Columns[2].HeaderText = WebStoreResources.ContentMetaMetaHrefLabel;


            productUploader.AddFileText = WebStoreResources.SelectFileButton;
            productUploader.DropFileText = WebStoreResources.DropProductFile;
            productUploader.UploadButtonText = WebStoreResources.FileUploadButton;
            productUploader.UploadCompleteText = WebStoreResources.UploadComplete;
            productUploader.UploadingText = WebStoreResources.Uploading;
            productUploader.ErrorOcurredMessage = WebStoreResources.UploadErrorMessage;

            teaserUploader.AddFileText = WebStoreResources.SelectFileButton;
            teaserUploader.DropFileText = WebStoreResources.DropTeaserFile;
            teaserUploader.UploadButtonText = WebStoreResources.FileUploadButton;
            teaserUploader.UploadCompleteText = WebStoreResources.UploadComplete;
            teaserUploader.UploadingText = WebStoreResources.Uploading;

            

        }

        protected string GetRefreshUrl()
        {

            string result = SiteRoot + "/WebStore/AdminProductEdit.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&prod=" + productGuid.ToString();

            return result;

        }

        private string GetReturnUrl()
        {
            return SiteRoot + "/WebStore/AdminProduct.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();
        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            store = StoreHelper.GetStore();
            if (store == null) { return; }
            

            siteUser = SiteUtils.GetCurrentSiteUser();

            productGuid = WebUtils.ParseGuidFromQueryString("prod", productGuid);

            virtualRoot = WebUtils.GetApplicationRoot();

            upLoadPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString()
                + "/webstoreproductfiles/";

            teaserFileBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString()
                + "/webstoreproductpreviewfiles/";

            

            AddClassToBody("webstore webstoreproductedit");

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null)
            {
                log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

            fileSystem = p.GetFileSystem();
            if (fileSystem == null)
            {
                log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

            if (!fileSystem.FolderExists(upLoadPath))
            {
                fileSystem.CreateFolder(upLoadPath);
            }

            if (!fileSystem.FolderExists(teaserFileBasePath))
            {
                fileSystem.CreateFolder(teaserFileBasePath);
            }

            if (productGuid == Guid.Empty) { return; }

            productUploader.ServiceUrl = SiteRoot + "/WebStore/upload.ashx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&prod=" + productGuid.ToString() ;
           
            productUploader.UploadButtonClientId = btnUpload.ClientID;

            productUploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 

            string refreshFunction = "function refresh" + moduleId.ToInvariantString()
                    + " (data, errorsOccurred) { if(errorsOccurred === false) { $('#" + btnSave.ClientID + "').click(); } } ";

            productUploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(), "refresh" + moduleId.ToInvariantString(),
                refreshFunction,
                true);

            teaserUploader.ServiceUrl = SiteRoot + "/WebStore/upload.ashx?type=teaser&pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&prod=" + productGuid.ToString();

            teaserUploader.UploadButtonClientId = btnUploadTeaser.ClientID;
            teaserUploader.FormFieldClientId = hdnState.ClientID; // not really used but prevents submitting all the form 
            teaserUploader.UploadCompleteCallback = "refresh" + moduleId.ToInvariantString();

            

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
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            btnUploadTeaser.Click += new EventHandler(btnUploadTeaser_Click);
            btnDeleteTeaser.Click += new EventHandler(btnDeleteTeaser_Click);
            
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
