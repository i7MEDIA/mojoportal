/// Author:					Joe Audette
/// Created:				2007-02-28
/// Last Modified:			2010-05-31
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
using mojoPortal.Business;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents an Offer. Products are not sold directly but via offers.
    /// It allows bundling products together for sale.
    /// </summary>
    public class Offer : IIndexableContent
    {
        #region Constructors

        public Offer()
        {

        }

        public Offer(Guid guid)
        {
            GetOffer(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private bool isVisible;
        private bool isSpecial;
        private Guid taxClassGuid = Guid.Empty;
        private string url = string.Empty;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private string createdFromIP = string.Empty;
        private bool isDeleted;
        private DateTime deletedTime = DateTime.MaxValue;
        private Guid deletedBy = Guid.Empty;
        private string deletedFromIP = string.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private Guid lastModifiedBy = Guid.Empty;
        private string lastModifiedFromIP;
        private string name = string.Empty;
        private string productListName = string.Empty;
        private string description = string.Empty;
        private string teaser = string.Empty;

        private bool availabilityChecked = false;
        private bool isAvailable = false;
        private bool isDonation = false;
        private bool showDetailLink = true;
        private decimal price = 0;

        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private string compiledMeta = string.Empty;

        private bool userCanSetPrice = false;
        private int maxPerOrder = 0; // 0 = unlimited
        private int sortRank1 = 5000;
        private int sortRank2 = 5000;

        // not persisted to the db set externally just before indexing to the search index
        private int siteId = -1;
        private string searchIndexPath = string.Empty;

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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ProductListName
        {
            get { return productListName; }
            set { productListName = value; }
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

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public int MaxPerOrder
        {
            get { return maxPerOrder; }
            set { maxPerOrder = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Teaser
        {
            get { return teaser; }
            set { teaser = value; }
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

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        public bool IsSpecial
        {
            get { return isSpecial; }
            set { isSpecial = value; }
        }

        public bool IsDonation
        {
            get { return isDonation; }
            set { isDonation = value; }
        }

        public bool UserCanSetPrice
        {
            get { return userCanSetPrice; }
            set { userCanSetPrice = value; }
        }

        public bool ShowDetailLink
        {
            get { return showDetailLink; }
            set { showDetailLink = value; }
        }

        public Guid TaxClassGuid
        {
            get { return taxClassGuid; }
            set { taxClassGuid = value; }
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
        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set { createdFromIP = value; }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        public DateTime DeletedTime
        {
            get { return deletedTime; }
            set { deletedTime = value; }
        }
        public Guid DeletedBy
        {
            get { return deletedBy; }
            set { deletedBy = value; }
        }
        public string DeletedFromIP
        {
            get { return deletedFromIP; }
            set { deletedFromIP = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public Guid LastModifiedBy
        {
            get { return lastModifiedBy; }
            set { lastModifiedBy = value; }
        }
        public string LastModifiedFromIP
        {
            get { return lastModifiedFromIP; }
            set { lastModifiedFromIP = value; }
        }

        public bool IsAvailable
        {
            get
            {
                if (!availabilityChecked)
                {
                    CheckAvailability();
                }
                return isAvailable;
            }
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

        #endregion

        #region Private Methods

        private void CheckAvailability()
        {
            // TODO: lookup

            isAvailable = true;

            availabilityChecked = true;
        }

        private void GetOffer(Guid guid)
        {
            using (IDataReader reader = DBOffer.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    this.isVisible = Convert.ToBoolean(reader["IsVisible"]);
                    this.isSpecial = Convert.ToBoolean(reader["IsSpecial"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.url = reader["Url"].ToString();
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["DeletedTime"] != DBNull.Value)
                    {
                        this.deletedTime = Convert.ToDateTime(reader["DeletedTime"]);
                    }
                    if (reader["DeletedBy"] != DBNull.Value)
                    {
                        this.deletedBy = new Guid(reader["DeletedBy"].ToString());
                    }
                    this.deletedFromIP = reader["DeletedFromIP"].ToString();
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastModifiedBy = new Guid(reader["LastModifiedBy"].ToString());
                    this.lastModifiedFromIP = reader["LastModifiedFromIP"].ToString();

                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.teaser = reader["Abstract"].ToString();

                    if (reader["TaxClassGuid"] != DBNull.Value)
                    {
                        this.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    }

                    this.isDonation = Convert.ToBoolean(reader["IsDonation"]);

                    if (reader["Price"] != DBNull.Value)
                    {
                        this.price = Convert.ToDecimal(reader["Price"].ToString());
                    }

                    this.showDetailLink = Convert.ToBoolean(reader["ShowDetailLink"]);
                    this.productListName = reader["ProductListName"].ToString();

                    this.userCanSetPrice = Convert.ToBoolean(reader["UserCanSetPrice"]);

                    this.maxPerOrder = Convert.ToInt32(reader["MaxPerOrder"]);
                    this.sortRank1 = Convert.ToInt32(reader["SortRank1"]);
                    this.sortRank2 = Convert.ToInt32(reader["SortRank2"]);
                    this.metaDescription = reader["MetaDescription"].ToString();
                    this.metaKeywords = reader["MetaKeywords"].ToString();
                    this.compiledMeta = reader["CompiledMeta"].ToString();

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBOffer.Create(
                this.guid,
                this.storeGuid,
                this.isVisible,
                this.isSpecial,
                this.taxClassGuid,
                this.url,
                this.created,
                this.createdBy,
                this.createdFromIP,
                this.lastModified,
                this.lastModifiedBy,
                this.lastModifiedFromIP,
                this.isDonation,
                this.name,
                this.description,
                this.teaser,
                this.price,
                this.productListName,
                this.showDetailLink,
                this.userCanSetPrice,
                this.maxPerOrder,
                this.sortRank1,
                this.sortRank2,
                this.metaDescription,
                this.metaKeywords,
                this.compiledMeta);

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
            Offer offer = new Offer(this.guid);

            DBOffer.AddOfferHistory(
                Guid.NewGuid(),
                offer.Guid,
                offer.StoreGuid,
                offer.IsVisible,
                offer.IsSpecial,
                offer.Created,
                offer.CreatedBy,
                offer.CreatedFromIP,
                offer.IsDeleted,
                offer.DeletedTime,
                offer.DeletedBy,
                offer.DeletedFromIP,
                offer.LastModified,
                offer.LastModifiedBy,
                offer.LastModifiedFromIP,
                DateTime.UtcNow,
                offer.TaxClassGuid,
                offer.Url);

            bool result = DBOffer.Update(
                this.guid,
                this.isVisible,
                this.isSpecial,
                this.taxClassGuid,
                this.url,
                this.lastModified,
                this.lastModifiedBy,
                this.lastModifiedFromIP,
                this.isDonation,
                this.name,
                this.description,
                this.teaser,
                this.price,
                this.productListName,
                this.showDetailLink,
                this.userCanSetPrice,
                this.maxPerOrder,
                this.sortRank1,
                this.sortRank2,
                this.metaDescription,
                this.metaKeywords,
                this.compiledMeta);

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
           
            //if (String.IsNullOrEmpty(name)) return false;

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
            Guid guid,
            Guid deletedBy,
            string deletedFromIP)
        {
            Offer offer = new Offer(guid);

            DBOffer.AddOfferHistory(
                Guid.NewGuid(),
                offer.Guid,
                offer.StoreGuid,
                offer.IsVisible,
                offer.IsSpecial,
                offer.Created,
                offer.CreatedBy,
                offer.CreatedFromIP,
                offer.IsDeleted,
                offer.DeletedTime,
                offer.DeletedBy,
                offer.DeletedFromIP,
                offer.LastModified,
                offer.LastModifiedBy,
                offer.LastModifiedFromIP,
                DateTime.UtcNow,
                offer.TaxClassGuid,
                offer.Url);

            return DBOffer.Delete(
                guid,
                DateTime.UtcNow,
                deletedBy,
                deletedFromIP);
        }


        


        /// <summary>
        /// this is for admin view and includes hidden products where IsVisible is false
        /// </summary>
        /// <param name="storeGuid"></param>
        /// <param name="languageGuid"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBOffer.GetPage(
                storeGuid,
                pageNumber,
                pageSize,
                out totalPages);

        }

        public static DataTable GetPublicPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBOffer.GetPublicPage(
                storeGuid,
                pageNumber,
                pageSize,
                out totalPages);
        }

        /// <summary>
        /// Gets a page of data from the ws_Offer table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageForProductList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBOffer.GetPageForProductList(
                storeGuid,
                pageNumber,
                pageSize,
                out totalPages);

        }

        public static DataTable GetListForPageOfProducts(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            return DBOffer.GetListForPageOfProducts(storeGuid, pageNumber, pageSize);
        }

        public static DataTable GetByProduct(Guid productGuid)
        {
            return DBOffer.GetByProduct(productGuid);
        }

        public static IDataReader GetTop10Specials(Guid storeGuid)
        {
            return DBOffer.GetTop10Specials(storeGuid);

        }

        
        //public static DataTable GetPage(
        //    Guid storeGuid,
        //    Guid languageGuid,
        //    int pageNumber, 
        //    int pageSize)
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Guid",typeof(Guid));
        //    dataTable.Columns.Add("StoreGuid",typeof(Guid));
        //    dataTable.Columns.Add("Name", typeof(string));
        //    dataTable.Columns.Add("IsVisible",typeof(bool));
        //    dataTable.Columns.Add("IsSpecial",typeof(bool));
        //    dataTable.Columns.Add("Created",typeof(DateTime));
        //    dataTable.Columns.Add("CreatedBy",typeof(Guid));
        //    dataTable.Columns.Add("CreatedFromIP",typeof(string));
            
        //    dataTable.Columns.Add("LastModified",typeof(DateTime));
        //    dataTable.Columns.Add("LastModifiedBy",typeof(Guid));
        //    dataTable.Columns.Add("LastModifiedFromIP",typeof(string));
        //    dataTable.Columns.Add("TotalPages", typeof(int));
		
        //    IDataReader reader = DBOffer.GetPage(
        //        storeGuid, 
        //        languageGuid, 
        //        pageNumber, 
        //        pageSize);	

        //    while (reader.Read())
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row["Guid"] = reader["Guid"];
        //        row["StoreGuid"] = reader["StoreGuid"];
        //        row["Name"] = reader["Name"];
        //        row["IsVisible"] = reader["IsVisible"];
        //        row["IsSpecial"] = reader["IsSpecial"];
        //        row["Created"] = reader["Created"];
        //        row["CreatedBy"] = reader["CreatedBy"];
        //        row["CreatedFromIP"] = reader["CreatedFromIP"];
                
        //        row["LastModified"] = reader["LastModified"];
        //        row["LastModifiedBy"] = reader["LastModifiedBy"];
        //        row["LastModifiedFromIP"] = reader["LastModifiedFromIP"];
        //        row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
        //        dataTable.Rows.Add(row);
        //    }
		
        //    reader.Close();
		
        //    return dataTable;
		
        //}


        public static DataTable GetBySitePage(int siteId, int pageId)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("MetaDescription", typeof(string));
            dataTable.Columns.Add("MetaKeywords", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));

            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("LastModified", typeof(DateTime));


            using (IDataReader reader = DBOffer.GetByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();


                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["MetaDescription"] = reader["MetaDescription"];
                    row["MetaKeywords"] = reader["MetaKeywords"];
                    row["ViewRoles"] = reader["ViewRoles"];

                    row["Created"] = Convert.ToDateTime(reader["Created"]);
                    row["LastModified"] = Convert.ToDateTime(reader["LastModified"]);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
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
