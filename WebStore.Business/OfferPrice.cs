using System;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-03-03
    /// Last Modified:			2008-02-25
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public class OfferPrice
    {

        #region Constructors

        public OfferPrice()
        { }


        public OfferPrice(Guid guid)
        {
            GetOfferPrice(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        private Guid currencyGuid = Guid.Empty;
        private decimal price;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private string createdFromIP;
        private DateTime lastModifed = DateTime.UtcNow;
        private Guid lastModifiedBy = Guid.Empty;
        private string modifiedFromIP;

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
        public Guid CurrencyGuid
        {
            get { return currencyGuid; }
            set { currencyGuid = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
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
        public DateTime LastModifed
        {
            get { return lastModifed; }
            set { lastModifed = value; }
        }
        public Guid LastModifiedBy
        {
            get { return lastModifiedBy; }
            set { lastModifiedBy = value; }
        }
        public string ModifiedFromIP
        {
            get { return modifiedFromIP; }
            set { modifiedFromIP = value; }
        }

        #endregion

        #region Private Methods

        private void GetOfferPrice(Guid guid)
        {
            IDataReader reader = DBOfferPrice.Get(guid);

            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.offerGuid = new Guid(reader["OfferGuid"].ToString());
                this.currencyGuid = new Guid(reader["CurrencyGuid"].ToString());
                this.price = Convert.ToDecimal(reader["Price"]);
                this.created = Convert.ToDateTime(reader["Created"]);
                this.createdBy = new Guid(reader["CreatedBy"].ToString());
                this.createdFromIP = reader["CreatedFromIP"].ToString();
                this.lastModifed = Convert.ToDateTime(reader["LastModifed"]);
                this.lastModifiedBy = new Guid(reader["LastModifiedBy"].ToString());
                this.modifiedFromIP = reader["ModifiedFromIP"].ToString();

            }

            reader.Close();

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBOfferPrice.Add(
                this.guid,
                this.offerGuid,
                this.currencyGuid,
                this.price,
                this.created,
                this.createdBy,
                this.createdFromIP,
                this.lastModifed,
                this.lastModifiedBy,
                this.modifiedFromIP);

            return (rowsAffected > 0);

        }



        private bool Update()
        {
            OfferPrice offerPrice = new OfferPrice(this.guid);
            DBOfferPrice.AddHistory(
                Guid.NewGuid(),
                offerPrice.Guid,
                offerPrice.OfferGuid,
                offerPrice.CurrencyGuid,
                offerPrice.Price,
                offerPrice.Created,
                offerPrice.CreatedBy,
                offerPrice.CreatedFromIP,
                offerPrice.LastModifed,
                offerPrice.LastModifiedBy,
                offerPrice.ModifiedFromIP,
                DateTime.UtcNow);

            return DBOfferPrice.Update(
                this.guid,
                this.currencyGuid,
                this.price,
                this.lastModifed,
                this.lastModifiedBy,
                this.modifiedFromIP);

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

        public static bool Delete(Guid guid)
        {
            OfferPrice offerPrice = new OfferPrice(guid);
            DBOfferPrice.AddHistory(
                Guid.NewGuid(),
                offerPrice.Guid,
                offerPrice.OfferGuid,
                offerPrice.CurrencyGuid,
                offerPrice.Price,
                offerPrice.Created,
                offerPrice.CreatedBy,
                offerPrice.CreatedFromIP,
                offerPrice.LastModifed,
                offerPrice.LastModifiedBy,
                offerPrice.ModifiedFromIP,
                DateTime.UtcNow);

            return DBOfferPrice.Delete(guid);
        }


        
        public static DataTable GetByOffer(Guid offerGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid",typeof(Guid));
            dataTable.Columns.Add("OfferGuid",typeof(Guid));
            dataTable.Columns.Add("CurrencyGuid",typeof(Guid));
            dataTable.Columns.Add("Price",typeof(decimal));
           
            dataTable.Columns.Add("Currency",typeof(string));
            dataTable.Columns.Add("CurrencyCode", typeof(string));
            dataTable.Columns.Add("SymbolLeft", typeof(string));
            dataTable.Columns.Add("SymbolRight", typeof(string));


            IDataReader reader = DBOfferPrice.GetByOffer(offerGuid);
            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["Guid"] = reader["Guid"];
                row["OfferGuid"] = reader["OfferGuid"];
                row["CurrencyGuid"] = reader["CurrencyGuid"];
                row["Price"] = reader["Price"];

                row["Currency"] = reader["Currency"];
                row["CurrencyCode"] = reader["CurrencyCode"];
                row["SymbolLeft"] = reader["SymbolLeft"];
                row["SymbolRight"] = reader["SymbolRight"];
                
                dataTable.Rows.Add(row);
            }
		
            reader.Close();
		
            return dataTable;
		
        }
	
       

        #endregion


    }

}
