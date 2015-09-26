/// Author:					Joe Audette
/// Created:				2007-03-02
/// Last Modified:			2009-02-01
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
using System.Collections.ObjectModel;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents a product included in an offer
    /// </summary>
    public class OfferProduct
    {

        #region Constructors

        public OfferProduct()
        { }


        public OfferProduct(Guid guid)
        {
            GetOfferProduct(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        private Guid productGuid = Guid.Empty;
        private byte fullfillType;
        private Guid fullFillTermsGuid = Guid.Empty;
        private int quantity;
        private int sortOrder;
        private bool isDeleted;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private string createdFromIP;
        private Guid deletedBy = Guid.Empty;
        private string deletedFromIP;
        private DateTime deletedTime;
        private DateTime lastModified = DateTime.UtcNow;
        private Guid lastModifiedBy = Guid.Empty;
        private string lastModifiedFromIP;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid OfferGuid
        {
            get { return offerGuid; }
            set { offerGuid = value; }
        }
        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }
        public byte FullfillType
        {
            get { return fullfillType; }
            set { fullfillType = value; }
        }
        public Guid FullFillTermsGuid
        {
            get { return fullFillTermsGuid; }
            set { fullFillTermsGuid = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
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
        public DateTime DeletedTime
        {
            get { return deletedTime; }
            set { deletedTime = value; }
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

        #endregion

        #region Private Methods

        private void GetOfferProduct(Guid guid)
        {
            using (IDataReader reader = DBOfferProduct.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.offerGuid = new Guid(reader["OfferGuid"].ToString());
                    this.productGuid = new Guid(reader["ProductGuid"].ToString());
                    this.fullfillType = Convert.ToByte(reader["FullfillType"]);
                    this.fullFillTermsGuid = new Guid(reader["FullFillTermsGuid"].ToString());
                    this.quantity = Convert.ToInt32(reader["Quantity"]);
                    this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    if (reader["DeletedBy"] != DBNull.Value)
                    {
                        this.deletedBy = new Guid(reader["DeletedBy"].ToString());
                    }
                    this.deletedFromIP = reader["DeletedFromIP"].ToString();
                    if (reader["DeletedTime"] != DBNull.Value)
                    {
                        this.deletedTime = Convert.ToDateTime(reader["DeletedTime"]);
                    }
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastModifiedBy = new Guid(reader["LastModifiedBy"].ToString());
                    this.lastModifiedFromIP = reader["LastModifiedFromIP"].ToString();

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBOfferProduct.Add(
                this.guid,
                this.offerGuid,
                this.productGuid,
                this.fullfillType,
                this.fullFillTermsGuid,
                this.quantity,
                this.sortOrder,
                this.created,
                this.createdBy,
                this.createdFromIP,
                this.lastModified,
                this.lastModifiedBy,
                this.lastModifiedFromIP);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBOfferProduct.Update(
                this.guid,
                this.fullfillType,
                this.fullFillTermsGuid,
                this.quantity,
                this.sortOrder,
                this.lastModified,
                this.lastModifiedBy,
                this.lastModifiedFromIP);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.guid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            string deletedFromIP)
        {
            return DBOfferProduct.Delete(
                guid,
                deletedBy,
                deletedFromIP,
                DateTime.UtcNow);
        }

        public static bool DeleteByProduct(
            Guid productGuid,
            Guid deletedBy,
            string deletedFromIP)
        {
            return DBOfferProduct.DeleteByProduct(
                productGuid,
                deletedBy,
                deletedFromIP,
                DateTime.UtcNow);

        }

        public static IDataReader GetReaderByOffer(Guid offerGuid)
        {
            return DBOfferProduct.GetByOffer(offerGuid);
        }

        public static int GetCountByOffer(Guid offerGuid)
        {
            return DBOfferProduct.GetCountByOffer(offerGuid);
        }

        public static DataTable GetByOffer(Guid offerGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("OfferGuid", typeof(Guid));
            dataTable.Columns.Add("ProductGuid", typeof(Guid));
            dataTable.Columns.Add("FullfillType", typeof(byte));
            dataTable.Columns.Add("FullFillTermsGuid", typeof(Guid));
            dataTable.Columns.Add("Quantity", typeof(int));
            dataTable.Columns.Add("SortOrder", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));


            using (IDataReader reader = DBOfferProduct.GetByOffer(offerGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["OfferGuid"] = reader["OfferGuid"];
                    row["ProductGuid"] = reader["ProductGuid"];
                    row["FullfillType"] = reader["FullfillType"];
                    row["FullFillTermsGuid"] = reader["FullFillTermsGuid"];
                    row["Quantity"] = reader["Quantity"];
                    row["SortOrder"] = reader["SortOrder"];
                    row["Name"] = reader["Name"];
                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;

        }

        public static Collection<OfferProduct> GetbyOffer(Guid offerGuid)
        {
            Collection<OfferProduct> offerProducts = new Collection<OfferProduct>();
            using (IDataReader reader = DBOfferProduct.GetByOffer(offerGuid))
            {
                while (reader.Read())
                {
                    OfferProduct offerProduct = new OfferProduct();
                    offerProduct.fullFillTermsGuid = new Guid(reader["FullfillTermsGuid"].ToString());
                    offerProduct.fullfillType = Convert.ToByte(reader["FullfillType"]);
                    offerProduct.guid = new Guid(reader["Guid"].ToString());
                    offerProduct.offerGuid = offerGuid;
                    offerProduct.productGuid = new Guid(reader["ProductGuid"].ToString());
                    offerProduct.quantity = Convert.ToInt32(reader["Quantity"]);
                    offerProduct.sortOrder = Convert.ToInt32(reader["SortOrder"]);

                    offerProducts.Add(offerProduct);
                }
            }


            return offerProducts;

        }
        

        #endregion


    }

}
