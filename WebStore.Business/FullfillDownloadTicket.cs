/// Author:					Joe Audette
/// Created:				2007-03-25
/// Last Modified:			2012-01-21
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

namespace WebStore.Business
{
    /// <summary>
    /// Represents a ticket for downloading a product. A ticket can be generated as needed within the download terms,
    /// but each ticket has a short expiration.
    /// </summary>
    public class FullfillDownloadTicket
    {

        #region Constructors

        public FullfillDownloadTicket()
        { }


        public FullfillDownloadTicket(Guid guid)
        {
            GetFullfillDownloadTicket(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid orderGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid productGuid = Guid.Empty;
        private Guid fullfillTermsGuid = Guid.Empty;
        private int downloadsAllowed = 0;
        private int expireAfterDays = 0;
        private bool countAfterDownload = false;
        private DateTime purchaseTime;
        private int downloadedCount = 0;
        private string productName = string.Empty;

        
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
        public Guid OrderGuid
        {
            get { return orderGuid; }
            set { orderGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }
        public Guid FullfillTermsGuid
        {
            get { return fullfillTermsGuid; }
            set { fullfillTermsGuid = value; }
        }
        public int DownloadsAllowed
        {
            get { return downloadsAllowed; }
            set { downloadsAllowed = value; }
        }
        public int ExpireAfterDays
        {
            get { return expireAfterDays; }
            set { expireAfterDays = value; }
        }
        public bool CountAfterDownload
        {
            get { return countAfterDownload; }
            set { countAfterDownload = value; }
        }
        public DateTime PurchaseTime
        {
            get { return purchaseTime; }
            set { purchaseTime = value; }
        }
        public int DownloadedCount
        {
            get { return downloadedCount; }
            set { downloadedCount = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public bool CanDownload
        {
            get
            {
                if (
                    (this.downloadsAllowed > 0)
                    &&(this.downloadedCount >= this.downloadsAllowed)
                    )
                {
                    return false;
                }

                if (this.expireAfterDays > 0)
                {
                    if (this.countAfterDownload)
                    {
                        if (downloadedCount == 0) return true;

                        // TODO: get time of first download not time of purchase
                        return
                            (this.PurchaseTime.AddDays(this.expireAfterDays)
                            < DateTime.UtcNow);
                    }
                    else
                    {
                        return
                            (this.PurchaseTime.AddDays(this.expireAfterDays)
                            < DateTime.UtcNow);
                    }
                }

                if (this.downloadsAllowed == 0) return true;
                return (this.downloadedCount < this.downloadsAllowed);
            }
        }

        #endregion

        #region Private Methods

        private void GetFullfillDownloadTicket(Guid guid)
        {
            using (IDataReader reader = DBDownloadTicket.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    this.orderGuid = new Guid(reader["OrderGuid"].ToString());
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.productGuid = new Guid(reader["ProductGuid"].ToString());
                    this.fullfillTermsGuid = new Guid(reader["FullfillTermsGuid"].ToString());
                    this.downloadsAllowed = Convert.ToInt32(reader["DownloadsAllowed"]);
                    this.expireAfterDays = Convert.ToInt32(reader["ExpireAfterDays"]);
                    this.countAfterDownload = Convert.ToBoolean(reader["CountAfterDownload"]);
                    this.purchaseTime = Convert.ToDateTime(reader["PurchaseTime"]);
                    this.downloadedCount = Convert.ToInt32(reader["DownloadedCount"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBDownloadTicket.Add(
                this.guid,
                this.storeGuid,
                this.orderGuid,
                this.userGuid,
                this.productGuid,
                this.fullfillTermsGuid,
                this.downloadsAllowed,
                this.expireAfterDays,
                this.countAfterDownload,
                this.purchaseTime,
                this.downloadedCount);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBDownloadTicket.Update(
                this.guid,
                this.productGuid,
                this.fullfillTermsGuid,
                this.downloadsAllowed,
                this.expireAfterDays,
                this.countAfterDownload,
                this.purchaseTime,
                this.downloadedCount);

        }

        private void IncrementDownloadCount()
        {
            this.downloadedCount += 1;
            DBDownloadTicket.IncrementDownloadCount(this.guid);
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

        public void RecordDownloadHistory(string iPAddress)
        {
            DBDownloadTicket.AddDownloadHistory(
                Guid.NewGuid(),
                this.guid,
                DateTime.UtcNow,
                iPAddress);

            IncrementDownloadCount();

        }

        public bool IsExpired()
        {
            if (expireAfterDays > 0) //0 = no expiration
            {
                // TODO: implement more complete expiration determination
                // if countAfterDownload istrue we need to check against the first download date rather than the purchase date

                if (purchaseTime.AddDays(expireAfterDays) < DateTime.UtcNow) { return true; }
            }

            if (downloadsAllowed > 0) //0 = unlimited
            {
                if (downloadedCount >= downloadsAllowed) { return true; }
            }

            return false;
        }

        #endregion

        #region Static Methods

        public static bool Delete(Guid guid)
        {
            return DBDownloadTicket.Delete(guid);
        }

        public static bool DeleteByOrder(Guid orderGuid)
        {
            return DBDownloadTicket.DeleteByOrder(orderGuid);
        }

        public static DataTable GetPage(
            Guid storeGuid,
            int pageNumber, 
            int pageSize)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid",typeof(Guid));
            dataTable.Columns.Add("StoreGuid",typeof(Guid));
            dataTable.Columns.Add("OrderGuid",typeof(Guid));
            dataTable.Columns.Add("UserGuid",typeof(Guid));
            dataTable.Columns.Add("ProductGuid",typeof(Guid));
            dataTable.Columns.Add("FullfillTermsGuid",typeof(Guid));
            dataTable.Columns.Add("DownloadsAllowed",typeof(int));
            dataTable.Columns.Add("ExpireAfterDays",typeof(int));
            dataTable.Columns.Add("CountAfterDownload",typeof(bool));
            dataTable.Columns.Add("PurchaseTime",typeof(DateTime));
            dataTable.Columns.Add("DownloadedCount",typeof(int));
            dataTable.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = DBDownloadTicket.GetPageByStore(
                storeGuid,
                pageNumber,
                pageSize))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["StoreGuid"] = reader["StoreGuid"];
                    row["OrderGuid"] = reader["OrderGuid"];
                    row["UserGuid"] = reader["UserGuid"];
                    row["ProductGuid"] = reader["ProductGuid"];
                    row["FullfillTermsGuid"] = reader["FullfillTermsGuid"];
                    row["DownloadsAllowed"] = reader["DownloadsAllowed"];
                    row["ExpireAfterDays"] = reader["ExpireAfterDays"];
                    row["CountAfterDownload"] = reader["CountAfterDownload"];
                    row["PurchaseTime"] = reader["PurchaseTime"];
                    row["DownloadedCount"] = reader["DownloadedCount"];
                    row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                    dataTable.Rows.Add(row);
                }

            }
		
            return dataTable;
		
        }

        public static IDataReader GetByOrder(Guid orderGuid)
        {
            return DBDownloadTicket.GetByOrder(orderGuid);
        }

        public static IDataReader GetDownloadHistory(Guid ticketGuid)
        {
            return DBDownloadTicket.GetDownloadHistory(ticketGuid);
        }

        public static bool MoveOrder(
             Guid orderGuid,
             Guid newUserGuid)
        {
            return DBDownloadTicket.MoveOrder(orderGuid, newUserGuid);
        }

        #endregion


    }

}
