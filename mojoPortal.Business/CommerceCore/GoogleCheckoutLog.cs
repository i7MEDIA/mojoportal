// Author:					
// Created:				    2008-03-09
// Last Modified:			2009-02-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a log of google checkout activity
    /// </summary>
    public class GoogleCheckoutLog
    {

        #region Constructors

        public GoogleCheckoutLog()
        { }


        public GoogleCheckoutLog(Guid rowGuid)
        {
            GetGoogleCheckoutLog(rowGuid);
        }

        public GoogleCheckoutLog(string googleOrderId)
        {
            GetGoogleCheckoutLog(googleOrderId);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid siteGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
        private string notificationType = string.Empty;
        private string rawResponse = string.Empty;
        private string serialNumber = string.Empty;
        private DateTime gTimestamp = DateTime.UtcNow;
        private string orderNumber = string.Empty;
        private string buyerId = string.Empty;
        private string fullfillState = string.Empty;
        private string financeState = string.Empty;
        private bool emailListOptIn = false;
        private string avsResponse = string.Empty;
        private string cvnResponse = string.Empty;
        private DateTime authExpDate = DateTime.UtcNow;
        private decimal authAmt = 0;
        private decimal discountTotal = 0;
        private decimal shippingTotal = 0;
        private decimal taxTotal = 0;
        private decimal orderTotal = 0;
        private decimal latestChgAmt = 0;
        private decimal totalChgAmt = 0;
        private decimal latestRefundAmt = 0;
        private decimal totalRefundAmt = 0;
        private decimal latestChargeback = 0;
        private decimal totalChargeback = 0;
        private string cartXml = string.Empty;
        private string providerName = string.Empty;

        

        #endregion

        #region Public Properties

        public Guid RowGuid
        {
            get { return rowGuid; }
            set { rowGuid = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }
        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }
        public string NotificationType
        {
            get { return notificationType; }
            set { notificationType = value; }
        }
        public string RawResponse
        {
            get { return rawResponse; }
            set { rawResponse = value; }
        }
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        public DateTime GTimestamp
        {
            get { return gTimestamp; }
            set { gTimestamp = value; }
        }
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }
        public string BuyerId
        {
            get { return buyerId; }
            set { buyerId = value; }
        }
        public string FullfillState
        {
            get { return fullfillState; }
            set { fullfillState = value; }
        }
        public string FinanceState
        {
            get { return financeState; }
            set { financeState = value; }
        }
        public bool EmailListOptIn
        {
            get { return emailListOptIn; }
            set { emailListOptIn = value; }
        }
        public string AvsResponse
        {
            get { return avsResponse; }
            set { avsResponse = value; }
        }
        public string CvnResponse
        {
            get { return cvnResponse; }
            set { cvnResponse = value; }
        }
        public DateTime AuthExpDate
        {
            get { return authExpDate; }
            set { authExpDate = value; }
        }
        public decimal AuthAmt
        {
            get { return authAmt; }
            set { authAmt = value; }
        }
        public decimal DiscountTotal
        {
            get { return discountTotal; }
            set { discountTotal = value; }
        }
        public decimal ShippingTotal
        {
            get { return shippingTotal; }
            set { shippingTotal = value; }
        }
        public decimal TaxTotal
        {
            get { return taxTotal; }
            set { taxTotal = value; }
        }
        public decimal OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }
        public decimal LatestChgAmt
        {
            get { return latestChgAmt; }
            set { latestChgAmt = value; }
        }
        public decimal TotalChgAmt
        {
            get { return totalChgAmt; }
            set { totalChgAmt = value; }
        }
        public decimal LatestRefundAmt
        {
            get { return latestRefundAmt; }
            set { latestRefundAmt = value; }
        }
        public decimal TotalRefundAmt
        {
            get { return totalRefundAmt; }
            set { totalRefundAmt = value; }
        }
        public decimal LatestChargeback
        {
            get { return latestChargeback; }
            set { latestChargeback = value; }
        }
        public decimal TotalChargeback
        {
            get { return totalChargeback; }
            set { totalChargeback = value; }
        }

        public string CartXml
        {
            get { return cartXml; }
            set { cartXml = value; }
        }

        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of GoogleCheckoutLog.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetGoogleCheckoutLog(Guid rowGuid)
        {
            using (IDataReader reader = DBGoogleCheckoutLog.GetOne(rowGuid))
            {
                PopulateFromReader(reader);
            }

        }

        /// <summary>
        /// Gets an instance of GoogleCheckoutLog.
        /// </summary>
        private void GetGoogleCheckoutLog(string googleOrderNumber)
        {
            using (IDataReader reader = DBGoogleCheckoutLog.GetMostRecentByOrder(googleOrderNumber))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.rowGuid = new Guid(reader["RowGuid"].ToString());
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.userGuid = new Guid(reader["UserGuid"].ToString());
                this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                this.cartGuid = new Guid(reader["CartGuid"].ToString());
                this.notificationType = reader["NotificationType"].ToString();
                this.rawResponse = reader["RawResponse"].ToString();
                this.serialNumber = reader["SerialNumber"].ToString();
                this.gTimestamp = Convert.ToDateTime(reader["GTimestamp"]);
                this.orderNumber = reader["OrderNumber"].ToString();
                this.buyerId = reader["BuyerId"].ToString();
                this.fullfillState = reader["FullfillState"].ToString();
                this.financeState = reader["FinanceState"].ToString();
                this.emailListOptIn = Convert.ToBoolean(reader["EmailListOptIn"]);
                this.avsResponse = reader["AvsResponse"].ToString();
                this.cvnResponse = reader["CvnResponse"].ToString();
                this.authExpDate = Convert.ToDateTime(reader["AuthExpDate"]);
                this.authAmt = Convert.ToDecimal(reader["AuthAmt"]);
                this.discountTotal = Convert.ToDecimal(reader["DiscountTotal"]);
                this.shippingTotal = Convert.ToDecimal(reader["ShippingTotal"]);
                this.taxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                this.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
                this.latestChgAmt = Convert.ToDecimal(reader["LatestChgAmt"]);
                this.totalChgAmt = Convert.ToDecimal(reader["TotalChgAmt"]);
                this.latestRefundAmt = Convert.ToDecimal(reader["LatestRefundAmt"]);
                this.totalRefundAmt = Convert.ToDecimal(reader["TotalRefundAmt"]);
                this.latestChargeback = Convert.ToDecimal(reader["LatestChargeback"]);
                this.totalChargeback = Convert.ToDecimal(reader["TotalChargeback"]);
                this.cartXml = reader["CartXml"].ToString();
                this.providerName = reader["ProviderName"].ToString();

            }
            
        }

        /// <summary>
        /// Persists a new instance of GoogleCheckoutLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBGoogleCheckoutLog.Create(
                this.rowGuid,
                this.createdUtc,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.notificationType,
                this.rawResponse,
                this.serialNumber,
                this.gTimestamp,
                this.orderNumber,
                this.buyerId,
                this.fullfillState,
                this.financeState,
                this.emailListOptIn,
                this.avsResponse,
                this.cvnResponse,
                this.authExpDate,
                this.authAmt,
                this.discountTotal,
                this.shippingTotal,
                this.taxTotal,
                this.orderTotal,
                this.latestChgAmt,
                this.totalChgAmt,
                this.latestRefundAmt,
                this.totalRefundAmt,
                this.latestChargeback,
                this.totalChargeback,
                this.cartXml,
                this.providerName);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of GoogleCheckoutLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBGoogleCheckoutLog.Update(
                this.rowGuid,
                this.createdUtc,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.notificationType,
                this.rawResponse,
                this.serialNumber,
                this.gTimestamp,
                this.orderNumber,
                this.buyerId,
                this.fullfillState,
                this.financeState,
                this.emailListOptIn,
                this.avsResponse,
                this.cvnResponse,
                this.authExpDate,
                this.authAmt,
                this.discountTotal,
                this.shippingTotal,
                this.taxTotal,
                this.orderTotal,
                this.latestChgAmt,
                this.totalChgAmt,
                this.latestRefundAmt,
                this.totalRefundAmt,
                this.latestChargeback,
                this.totalChargeback,
                this.cartXml);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of GoogleCheckoutLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowGuid != Guid.Empty)
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

        /// <summary>
        /// Deletes an instance of GoogleCheckoutLog. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBGoogleCheckoutLog.Delete(rowGuid);
        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            return DBGoogleCheckoutLog.DeleteByCart(cartGuid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBGoogleCheckoutLog.DeleteBySite(siteGuid);
        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            return DBGoogleCheckoutLog.DeleteByStore(storeGuid);
        }


        /// <summary>
        /// Gets a count of GoogleCheckoutLog. 
        /// </summary>
        public static int GetCountByCart(Guid cartGuid)
        {
            return DBGoogleCheckoutLog.GetCountByCart(cartGuid);
        }

        /// <summary>
        /// Gets a count of GoogleCheckoutLog. 
        /// </summary>
        public static int GetCountByStore(Guid storeGuid)
        {
            return DBGoogleCheckoutLog.GetCountByStore(storeGuid);
        }

        private static List<GoogleCheckoutLog> LoadListFromReader(IDataReader reader)
        {
            List<GoogleCheckoutLog> googleCheckoutLogList = new List<GoogleCheckoutLog>();
            try
            {
                while (reader.Read())
                {
                    GoogleCheckoutLog googleCheckoutLog = new GoogleCheckoutLog();
                    googleCheckoutLog.rowGuid = new Guid(reader["RowGuid"].ToString());
                    googleCheckoutLog.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    googleCheckoutLog.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    googleCheckoutLog.userGuid = new Guid(reader["UserGuid"].ToString());
                    googleCheckoutLog.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    googleCheckoutLog.cartGuid = new Guid(reader["CartGuid"].ToString());
                    googleCheckoutLog.notificationType = reader["NotificationType"].ToString();
                    googleCheckoutLog.rawResponse = reader["RawResponse"].ToString();
                    googleCheckoutLog.serialNumber = reader["SerialNumber"].ToString();
                    googleCheckoutLog.gTimestamp = Convert.ToDateTime(reader["GTimestamp"]);
                    googleCheckoutLog.orderNumber = reader["OrderNumber"].ToString();
                    googleCheckoutLog.buyerId = reader["BuyerId"].ToString();
                    googleCheckoutLog.fullfillState = reader["FullfillState"].ToString();
                    googleCheckoutLog.financeState = reader["FinanceState"].ToString();
                    googleCheckoutLog.emailListOptIn = Convert.ToBoolean(reader["EmailListOptIn"]);
                    googleCheckoutLog.avsResponse = reader["AvsResponse"].ToString();
                    googleCheckoutLog.cvnResponse = reader["CvnResponse"].ToString();
                    googleCheckoutLog.authExpDate = Convert.ToDateTime(reader["AuthExpDate"]);
                    googleCheckoutLog.authAmt = Convert.ToDecimal(reader["AuthAmt"]);
                    googleCheckoutLog.discountTotal = Convert.ToDecimal(reader["DiscountTotal"]);
                    googleCheckoutLog.shippingTotal = Convert.ToDecimal(reader["ShippingTotal"]);
                    googleCheckoutLog.taxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                    googleCheckoutLog.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
                    googleCheckoutLog.latestChgAmt = Convert.ToDecimal(reader["LatestChgAmt"]);
                    googleCheckoutLog.totalChgAmt = Convert.ToDecimal(reader["TotalChgAmt"]);
                    googleCheckoutLog.latestRefundAmt = Convert.ToDecimal(reader["LatestRefundAmt"]);
                    googleCheckoutLog.totalRefundAmt = Convert.ToDecimal(reader["TotalRefundAmt"]);
                    googleCheckoutLog.latestChargeback = Convert.ToDecimal(reader["LatestChargeback"]);
                    googleCheckoutLog.totalChargeback = Convert.ToDecimal(reader["TotalChargeback"]);
                    googleCheckoutLog.cartXml = reader["CartXml"].ToString();
                    googleCheckoutLog.providerName = reader["ProviderName"].ToString();
                    googleCheckoutLogList.Add(googleCheckoutLog);

                }
            }
            finally
            {
                reader.Close();
            }

            return googleCheckoutLogList;

        }

        ///// <summary>
        ///// Gets an IList with all instances of GoogleCheckoutLog.
        ///// </summary>
        //public static List<GoogleCheckoutLog> GetAll()
        //{
        //    IDataReader reader = DBGoogleCheckoutLog.GetAll();
        //    return LoadListFromReader(reader);

        //}

        /// <summary>
        /// Gets an IDataReader with page of instances of GoogleCheckoutLog.
        /// </summary>
        /// <param name="cartGuid">The cartGuid</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByCart(
            Guid cartGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            return DBGoogleCheckoutLog.GetPageByCart(cartGuid, pageNumber, pageSize, out totalPages);

        }

        /// <summary>
        /// Gets an IDataReader with page of instances of GoogleCheckoutLog.
        /// </summary>
        /// <param name="storeGuid">The storeGuid</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageByStore(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            return DBGoogleCheckoutLog.GetPageByStore(storeGuid, pageNumber, pageSize, out totalPages);

        }


        public static Guid GetCartGuidFromOrderNumber(string googleOrderNumber)
        {
            Guid cartGuid = Guid.Empty;

            using (IDataReader reader = DBGoogleCheckoutLog.GetMostRecentByOrder(googleOrderNumber))
            {
                if (reader.Read())
                {
                    cartGuid = new Guid(reader["CartGuid"].ToString());
                }
            }

            return cartGuid;


        }

        public static string GetProviderNameFromOrderNumber(string googleOrderNumber)
        {
            string providerName = string.Empty;

            using (IDataReader reader = DBGoogleCheckoutLog.GetMostRecentByOrder(googleOrderNumber))
            {
                if (reader.Read())
                {
                    providerName = reader["ProviderName"].ToString();
                }
            }

            return providerName;


        }


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByCreatedUtc(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.CreatedUtc.CompareTo(googleCheckoutLog2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByNotificationType(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.NotificationType.CompareTo(googleCheckoutLog2.NotificationType);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByRawResponse(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.RawResponse.CompareTo(googleCheckoutLog2.RawResponse);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareBySerialNumber(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.SerialNumber.CompareTo(googleCheckoutLog2.SerialNumber);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByGTimestamp(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.GTimestamp.CompareTo(googleCheckoutLog2.GTimestamp);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByOrderNumber(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.OrderNumber.CompareTo(googleCheckoutLog2.OrderNumber);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByBuyerId(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.BuyerId.CompareTo(googleCheckoutLog2.BuyerId);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByFullfillState(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.FullfillState.CompareTo(googleCheckoutLog2.FullfillState);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByFinanceState(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.FinanceState.CompareTo(googleCheckoutLog2.FinanceState);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByAvsResponse(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.AvsResponse.CompareTo(googleCheckoutLog2.AvsResponse);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByCvnResponse(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.CvnResponse.CompareTo(googleCheckoutLog2.CvnResponse);
        }
        /// <summary>
        /// Compares 2 instances of GoogleCheckoutLog.
        /// </summary>
        public static int CompareByAuthExpDate(GoogleCheckoutLog googleCheckoutLog1, GoogleCheckoutLog googleCheckoutLog2)
        {
            return googleCheckoutLog1.AuthExpDate.CompareTo(googleCheckoutLog2.AuthExpDate);
        }

        #endregion


    }

}
