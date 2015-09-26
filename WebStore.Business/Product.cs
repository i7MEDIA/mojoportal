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

using System;
using System.Data;
using WebStore.Data;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;

namespace WebStore.Business
{
    /// <summary>
    /// Represents a product
    /// </summary>
    public class Product : IIndexableContent
    {

        #region Constructors

        public Product()
        {
        }


        public Product(Guid guid)
        {
            GetProduct(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;

        private Guid taxClassGuid = Guid.Empty;
        private TaxClass taxClass = null;

        private Guid languageGuid = Guid.Empty;
        private string name = string.Empty;
        private string description = string.Empty;
        private string url = string.Empty;
        private string teaser = string.Empty;

        private string sku = string.Empty;
        private string modelNumber = string.Empty;
        private byte status;
        private byte fulfillmentType = 3; //none
        private decimal weight = 0;
        private int quantityOnHand = 1;
        private string imageFileName;
        private byte[] imageFileBytes = null;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private Guid lastModifedBy = Guid.Empty;
        private bool showInProductList = true;
        private bool enableRating = true;

        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private string compiledMeta = string.Empty;

        // not persisted to the db set externally just before indexing to the search index
        private int siteId = -1;
        private string searchIndexPath = string.Empty;
        private int sortRank1 = 5000;
        private int sortRank2 = 5000;
        private string teaserFile = string.Empty;
        private string teaserFileLink = string.Empty;
        private decimal shippingAmount = 0;
        
        

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }

        public Guid LanguageGuid
        {
            get { return languageGuid; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string MetaKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }

        public string MetaDescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        public string CompiledMeta
        {
            get { return compiledMeta; }
            set { compiledMeta = value; }
        }

        public int SortRank1
        {
            get { return sortRank1; }
            set { sortRank1 = value; }
        }

        public int SortRank2
        {
            get { return sortRank2; }
            set { sortRank2 = value; }
        }

        public bool ShowInProductList
        {
            get { return showInProductList; }
            set { showInProductList = value; }
        }

        public bool EnableRating
        {
            get { return enableRating; }
            set { enableRating = value; }
        }

        public string Teaser
        {
            get { return teaser; }
            set { teaser = value; }
        }

        public Guid TaxClassGuid
        {
            get { return taxClassGuid; }
            set
            {
                if (taxClassGuid != value)
                {
                    taxClassGuid = value;
                    taxClass = null;
                }

            }
        }

        public TaxClass TaxClass
        {
            get
            {
                if (taxClass == null) taxClass = new TaxClass(taxClassGuid);
                return taxClass;
            }
        }
        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }
        public string ModelNumber
        {
            get { return modelNumber; }
            set { modelNumber = value; }
        }
        public ProductStatus Status
        {
            get { return ProductStatusFromInt32(status); }
            set { status = (byte)value; }
        }
        public FulfillmentType FulfillmentType
        {
            get { return FulfillmentTypeFromInt32(fulfillmentType); }
            set { fulfillmentType = (byte)value; }
        }
        public decimal Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public int QuantityOnHand
        {
            get { return quantityOnHand; }
            set { quantityOnHand = value; }
        }
        public string ImageFileName
        {
            get { return imageFileName; }
            set { imageFileName = value; }
        }
        public byte[] ImageFileBytes
        {
            get { return imageFileBytes; }
            set { imageFileBytes = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public Guid LastModifedBy
        {
            get { return lastModifedBy; }
            set { lastModifedBy = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }

        public string TeaserFile
        {
            get { return teaserFile; }
            set { teaserFile = value; }
        }

        public string TeaserFileLink
        {
            get { return teaserFileLink; }
            set { teaserFileLink = value; }
        }

        public decimal ShippingAmount
        {
            get { return shippingAmount; }
            set { shippingAmount = value; }
        }

        #endregion

        #region Private Methods

        private void GetProduct(Guid guid) 
		{
            using (IDataReader reader = DBProduct.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    this.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    //this.sku = reader["Sku"].ToString();
                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.modelNumber = reader["ModelNumber"].ToString();
                    this.status = Convert.ToByte(reader["Status"]);
                    this.fulfillmentType = Convert.ToByte(reader["FullfillmentType"]);
                    this.weight = Convert.ToDecimal(reader["Weight"]);
                    this.quantityOnHand = Convert.ToInt32(reader["QuantityOnHand"]);
                    this.imageFileName = reader["ImageFileName"].ToString();
                    // TODO:
                    //this.imageFileBytes = Byte[]
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastModifedBy = new Guid(reader["LastModifedBy"].ToString());

                    this.url = reader["Url"].ToString();
                    this.teaser = reader["Abstract"].ToString();
                    this.showInProductList = Convert.ToBoolean(reader["ShowInProductList"]);
                    this.enableRating = Convert.ToBoolean(reader["EnableRating"]);

                    this.sortRank1 = Convert.ToInt32(reader["SortRank1"]);
                    this.sortRank2 = Convert.ToInt32(reader["SortRank2"]);
                    this.teaserFile = reader["TeaserFile"].ToString();
                    this.teaserFileLink = reader["TeaserFileLink"].ToString();
                    this.metaDescription = reader["MetaDescription"].ToString();
                    this.metaKeywords = reader["MetaKeywords"].ToString();
                    this.compiledMeta = reader["CompiledMeta"].ToString();
                    if (reader["ShippingAmount"] != DBNull.Value)
                    {
                        this.shippingAmount = Convert.ToDecimal(reader["ShippingAmount"]);
                    }
                    

                }

            }
		
		}

        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBProduct.Add(
                this.guid,
                this.storeGuid,
                this.taxClassGuid,
                this.modelNumber,
                this.status,
                this.fulfillmentType,
                this.weight,
                this.quantityOnHand,
                this.imageFileName,
                this.imageFileBytes,
                this.created,
                this.createdBy,
                this.lastModified,
                this.lastModifedBy,
                this.url,
                this.name,
                this.description,
                this.teaser,
                this.showInProductList,
                this.enableRating,
                this.metaDescription,
                this.metaKeywords,
                this.sortRank1,
                this.sortRank2,
                this.teaserFile,
                this.teaserFileLink,
                this.compiledMeta,
                this.shippingAmount);

            bool result = (rowsAffected > 0);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }



        private bool Update()
        {
            Product product = new Product(this.guid);
            DBProduct.AddHistory(
                Guid.NewGuid(),
                product.Guid,
                product.StoreGuid,
                product.TaxClassGuid,
                string.Empty,
                ConvertProductStatusToByte(product.Status),
                ConvertFulfillmentTypeToByte(product.FulfillmentType),
                product.Weight,
                product.QuantityOnHand,
                product.ImageFileName,
                product.ImageFileBytes,
                product.Created,
                product.CreatedBy,
                product.LastModified,
                product.LastModifedBy,
                DateTime.UtcNow,
                product.shippingAmount);

            bool result = DBProduct.Update(
                this.guid,
                this.taxClassGuid,
                this.modelNumber,
                this.status,
                this.fulfillmentType,
                this.weight,
                this.quantityOnHand,
                this.imageFileName,
                this.imageFileBytes,
                this.lastModified,
                this.lastModifedBy,
                this.url,
                this.name,
                this.description,
                this.teaser,
                this.showInProductList,
                this.enableRating,
                this.metaDescription,
                this.metaKeywords,
                this.sortRank1,
                this.sortRank2,
                this.teaserFile,
                this.teaserFileLink,
                this.compiledMeta,
                this.shippingAmount);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;

        }

        

        #endregion

        #region Public Methods


        public bool Save()
        {
            
            bool result = false;

            if (this.guid != Guid.Empty)
            {
                result = Update();
            }
            else
            {
                result = Create();
            }

            return result;
        }




        #endregion

        #region Static Methods

        public static bool Delete(
            Guid productGuid, 
            Guid userGuid, 
            string userIPAddress)
        {

            Product product = new Product(productGuid);
            if (
                (product.Guid == productGuid)
                &&(productGuid != Guid.Empty)
                )
            {
                DBProduct.AddHistory(
                    Guid.NewGuid(),
                    product.Guid,
                    product.StoreGuid,
                    product.TaxClassGuid,
                    product.Sku,
                    ConvertProductStatusToByte(product.Status),
                    ConvertFulfillmentTypeToByte(product.FulfillmentType),
                    product.Weight,
                    product.QuantityOnHand,
                    product.ImageFileName,
                    product.ImageFileBytes,
                    product.Created,
                    product.CreatedBy,
                    product.LastModified,
                    product.LastModifedBy,
                    DateTime.UtcNow,
                    product.shippingAmount);

                OfferProduct.DeleteByProduct(
                    productGuid,
                    userGuid,
                    userIPAddress);
            }
            
            return DBProduct.Delete(
                productGuid,
                DateTime.UtcNow, 
                userGuid, 
                userIPAddress);
        }


        public static IDataReader GetAll(Guid storeGuid)
        {
            return DBProduct.GetAll(storeGuid);
        }

        public static IDataReader GetForSiteMap(Guid siteGuid, Guid storeGuid)
        {
            return DBProduct.GetForSiteMap(siteGuid, storeGuid);
        }

        public static DataTable GetPageForAdminList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBProduct.GetPageForAdminList(storeGuid, pageNumber, pageSize, out totalPages);
        }

        
        
        public static DataTable GetPage(
            Guid storeGuid,
            int pageNumber, 
            int pageSize,
            out int totalPages)
        {

            return DBProduct.GetPage(
                storeGuid,
                pageNumber,
                pageSize,
                out totalPages);
		
        }

        public static DataTable GetBySitePage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("ModelNumber", typeof(string));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("MetaDescription", typeof(string));
            dataTable.Columns.Add("MetaKeywords", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("ShippingAmount", typeof(Decimal));

            using (IDataReader reader = DBProduct.GetProductByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();


                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["ModelNumber"] = reader["ModelNumber"];
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["MetaDescription"] = reader["MetaDescription"];
                    row["MetaKeywords"] = reader["MetaKeywords"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    row["Created"] = Convert.ToDateTime(reader["Created"]);
                    row["LastModified"] = Convert.ToDateTime(reader["LastModified"]);
                    if (reader["ShippingAmount"] != DBNull.Value)
                    {
                        row["ShippingAmount"] = Convert.ToDecimal(reader["ShippingAmount"]);
                    }
                    else
                    {
                        row["ShippingAmount"] = 0;
                    }
                    
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public static DataTable GetListForPageOfOffers(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            return DBProduct.GetListForPageOfOffers(storeGuid, pageNumber, pageSize);
        }

        private static byte ConvertProductStatusToByte(ProductStatus status)
        {
            switch (status)
            {
                case ProductStatus.Available:
                    return 1;

                case ProductStatus.Discontinued:
                    return 2;

                case ProductStatus.Planned:
                    return 3;
            }

            return 3;

        }

        private static byte ConvertFulfillmentTypeToByte(FulfillmentType fulfillmentType)
        {
            switch (fulfillmentType)
            {
                case FulfillmentType.None:
                    return 3;

                case FulfillmentType.Download:
                    return 1;

                case FulfillmentType.PhysicalShipment:
                    return 2;

            }

            return 3;
        }

        public static ProductStatus ProductStatusFromInt32(int input)
        {
            return (ProductStatus)Enum.ToObject(typeof(ProductStatus), input);

        }

        public static ProductStatus ProductStatusFromString(string input)
        {
            int i;
            if(!int.TryParse(input, out i))
            {
                i = -1;
            }
            return ProductStatusFromInt32(i);

        }

        public static FulfillmentType FulfillmentTypeFromInt32(int input)
        {
            return (FulfillmentType)Enum.ToObject(typeof(FulfillmentType), input);

        }

        public static FulfillmentType FulfillmentTypeFromString(string input)
        {
            int i;
            if (!int.TryParse(input, out i))
            {
                i = -1;
            }
            return FulfillmentTypeFromInt32(i);

        }

        #endregion


        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, e);
            }
        }




        #endregion

    }

}
