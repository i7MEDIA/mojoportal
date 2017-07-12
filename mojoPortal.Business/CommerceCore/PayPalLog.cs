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
    /// Represents a log used to track paypal activity
    /// </summary>
    public class PayPalLog
    {

        #region Constructors

        public PayPalLog()
        { }


        public PayPalLog(Guid rowGuid)
        {
            GetPayPalLog(rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid siteGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
        private string requestType = string.Empty;
        private string apiVersion = string.Empty;
        private string rawResponse = string.Empty;
        private string token = string.Empty;
        private string payerId = string.Empty;
        private string transactionId = string.Empty;
        private string paymentType = string.Empty;
        private string paymentStatus = string.Empty;
        private string pendingReason = string.Empty;
        private string reasonCode = string.Empty;
        private string currencyCode = string.Empty;
        private decimal exchangeRate = 1;
        private decimal cartTotal = 0;
        private decimal payPalAmt = 0;
        private decimal taxAmt = 0;
        private decimal feeAmt = 0;
        private decimal settleAmt = 0;
        private string providerName = string.Empty;
        private string pdtProviderName = string.Empty;
        private string ipnProviderName = string.Empty;
        private string response = string.Empty;
        private string returnUrl = string.Empty;
        private string serializedObject = string.Empty;


        
        

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
        public string RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }
        public string ApiVersion
        {
            get { return apiVersion; }
            set { apiVersion = value; }
        }
        public string RawResponse
        {
            get { return rawResponse; }
            set { rawResponse = value; }
        }
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public string PayerId
        {
            get { return payerId; }
            set { payerId = value; }
        }
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        public string PaymentStatus
        {
            get { return paymentStatus; }
            set { paymentStatus = value; }
        }
        public string PendingReason
        {
            get { return pendingReason; }
            set { pendingReason = value; }
        }
        public string ReasonCode
        {
            get { return reasonCode; }
            set { reasonCode = value; }
        }
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        public decimal ExchangeRate
        {
            get { return exchangeRate; }
            set { exchangeRate = value; }
        }
        public decimal CartTotal
        {
            get { return cartTotal; }
            set { cartTotal = value; }
        }
        public decimal PayPalAmt
        {
            get { return payPalAmt; }
            set { payPalAmt = value; }
        }
        public decimal TaxAmt
        {
            get { return taxAmt; }
            set { taxAmt = value; }
        }
        public decimal FeeAmt
        {
            get { return feeAmt; }
            set { feeAmt = value; }
        }
        public decimal SettleAmt
        {
            get { return settleAmt; }
            set { settleAmt = value; }
        }
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        public string SerializedObject
        {
            get { return serializedObject; }
            set { serializedObject = value; }
        }


        public string PDTProviderName
        {
            get { return pdtProviderName; }
            set { pdtProviderName = value; }
        }

        public string IPNProviderName
        {
            get { return ipnProviderName; }
            set { ipnProviderName = value; }
        }

        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of PayPalLog.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetPayPalLog(Guid rowGuid)
        {
            using (IDataReader reader = DBPayPalLog.GetOne(rowGuid))
            {
                PopulateFromReader(this, reader);
            }

        }


        

        /// <summary>
        /// Persists a new instance of PayPalLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBPayPalLog.Create(
                this.rowGuid,
                this.createdUtc,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.requestType,
                this.apiVersion,
                this.rawResponse,
                this.token,
                this.payerId,
                this.transactionId,
                this.paymentType,
                this.paymentStatus,
                this.pendingReason,
                this.reasonCode,
                this.currencyCode,
                this.exchangeRate,
                this.cartTotal,
                this.payPalAmt,
                this.taxAmt,
                this.feeAmt,
                this.settleAmt,
                this.providerName,
                this.returnUrl,
                this.serializedObject,
                this.pdtProviderName,
                this.ipnProviderName,
                this.response);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of PayPalLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBPayPalLog.Update(
                this.rowGuid,
                this.createdUtc,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.requestType,
                this.apiVersion,
                this.rawResponse,
                this.token,
                this.payerId,
                this.transactionId,
                this.paymentType,
                this.paymentStatus,
                this.pendingReason,
                this.reasonCode,
                this.currencyCode,
                this.exchangeRate,
                this.cartTotal,
                this.payPalAmt,
                this.taxAmt,
                this.feeAmt,
                this.settleAmt);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of PayPalLog. Returns true on success.
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
        /// Deletes an instance of PayPalLog. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBPayPalLog.Delete(rowGuid);
        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            return DBPayPalLog.DeleteByCart(cartGuid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBPayPalLog.DeleteBySite(siteGuid);
        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            return DBPayPalLog.DeleteByStore(storeGuid);
        }

        private static void PopulateFromReader(PayPalLog payPalLog, IDataReader reader)
        {
            if (reader.Read())
            {
                payPalLog.rowGuid = new Guid(reader["RowGuid"].ToString());
                payPalLog.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                payPalLog.siteGuid = new Guid(reader["SiteGuid"].ToString());
                payPalLog.userGuid = new Guid(reader["UserGuid"].ToString());
                payPalLog.storeGuid = new Guid(reader["StoreGuid"].ToString());
                payPalLog.cartGuid = new Guid(reader["CartGuid"].ToString());
                payPalLog.requestType = reader["RequestType"].ToString();
                payPalLog.apiVersion = reader["ApiVersion"].ToString();
                payPalLog.rawResponse = reader["RawResponse"].ToString();
                payPalLog.token = reader["Token"].ToString();
                payPalLog.payerId = reader["PayerId"].ToString();
                payPalLog.transactionId = reader["TransactionId"].ToString();
                payPalLog.paymentType = reader["PaymentType"].ToString();
                payPalLog.paymentStatus = reader["PaymentStatus"].ToString();
                payPalLog.pendingReason = reader["PendingReason"].ToString();
                payPalLog.reasonCode = reader["ReasonCode"].ToString();
                payPalLog.currencyCode = reader["CurrencyCode"].ToString();
                payPalLog.exchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
                payPalLog.cartTotal = Convert.ToDecimal(reader["CartTotal"]);
                payPalLog.payPalAmt = Convert.ToDecimal(reader["PayPalAmt"]);
                payPalLog.taxAmt = Convert.ToDecimal(reader["TaxAmt"]);
                payPalLog.feeAmt = Convert.ToDecimal(reader["FeeAmt"]);
                payPalLog.settleAmt = Convert.ToDecimal(reader["SettleAmt"]);
                payPalLog.providerName = reader["ProviderName"].ToString();
                payPalLog.returnUrl = reader["ReturnUrl"].ToString();
                payPalLog.serializedObject = reader["SerializedObject"].ToString();
                payPalLog.pdtProviderName = reader["PDTProviderName"].ToString();
                payPalLog.ipnProviderName = reader["IPNProviderName"].ToString();
                payPalLog.response = reader["Response"].ToString();

            }
            
        }

        public static PayPalLog GetSetExpressCheckout(string token)
        {
            PayPalLog log = new PayPalLog();
            using (IDataReader reader = DBPayPalLog.GetSetExpressCheckout(token))
            {
                PopulateFromReader(log, reader);
            }

            if (log.RowGuid == Guid.Empty) return null;

            return log;

        }

        public static PayPalLog GetMostRecent(Guid cartGuid, string requestType)
        {
            PayPalLog log = new PayPalLog();
            using (IDataReader reader = DBPayPalLog.GetMostRecentLog(cartGuid, requestType))
            {
                PopulateFromReader(log, reader);
            }

            if (log.RowGuid == Guid.Empty) return null;

            return log;

        }

        ///// <summary>
        ///// Gets a count of PayPalLog. 
        ///// </summary>
        //public static int GetCount()
        //{
        //    return DBPayPalLog.GetCount();
        //}

        private static List<PayPalLog> LoadListFromReader(IDataReader reader)
        {
            List<PayPalLog> payPalLogList = new List<PayPalLog>();
            try
            {
                while (reader.Read())
                {
                    PayPalLog payPalLog = new PayPalLog();
                    payPalLog.rowGuid = new Guid(reader["RowGuid"].ToString());
                    payPalLog.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    payPalLog.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    payPalLog.userGuid = new Guid(reader["UserGuid"].ToString());
                    payPalLog.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    payPalLog.cartGuid = new Guid(reader["CartGuid"].ToString());
                    payPalLog.requestType = reader["RequestType"].ToString();
                    payPalLog.apiVersion = reader["ApiVersion"].ToString();
                    payPalLog.rawResponse = reader["RawResponse"].ToString();
                    payPalLog.token = reader["Token"].ToString();
                    payPalLog.payerId = reader["PayerId"].ToString();
                    payPalLog.transactionId = reader["TransactionId"].ToString();
                    payPalLog.paymentType = reader["PaymentType"].ToString();
                    payPalLog.paymentStatus = reader["PaymentStatus"].ToString();
                    payPalLog.pendingReason = reader["PendingReason"].ToString();
                    payPalLog.reasonCode = reader["ReasonCode"].ToString();
                    payPalLog.currencyCode = reader["CurrencyCode"].ToString();
                    payPalLog.exchangeRate = Convert.ToDecimal(reader["ExchangeRate"]);
                    payPalLog.cartTotal = Convert.ToDecimal(reader["CartTotal"]);
                    payPalLog.payPalAmt = Convert.ToDecimal(reader["PayPalAmt"]);
                    payPalLog.taxAmt = Convert.ToDecimal(reader["TaxAmt"]);
                    payPalLog.feeAmt = Convert.ToDecimal(reader["FeeAmt"]);
                    payPalLog.settleAmt = Convert.ToDecimal(reader["SettleAmt"]);
                    payPalLog.providerName = reader["ProviderName"].ToString();
                    payPalLog.returnUrl = reader["ReturnUrl"].ToString();
                    payPalLog.serializedObject = reader["SerializedObject"].ToString();

                    payPalLogList.Add(payPalLog);

                }
            }
            finally
            {
                reader.Close();
            }

            return payPalLogList;

        }

        ///// <summary>
        ///// Gets an IList with all instances of PayPalLog.
        ///// </summary>
        //public static List<PayPalLog> GetAll()
        //{
        //    IDataReader reader = DBPayPalLog.GetAll();
        //    return LoadListFromReader(reader);

        //}

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PayPalLog table.
        /// </summary>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            return DBPayPalLog.GetByCart(cartGuid);

        }

        ///// <summary>
        ///// Gets an IList with page of instances of PayPalLog.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static List<PayPalLog> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBPayPalLog.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByCreatedUtc(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.CreatedUtc.CompareTo(payPalLog2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByRequestType(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.RequestType.CompareTo(payPalLog2.RequestType);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByApiVersion(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.ApiVersion.CompareTo(payPalLog2.ApiVersion);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByRawResponse(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.RawResponse.CompareTo(payPalLog2.RawResponse);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByToken(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.Token.CompareTo(payPalLog2.Token);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByPayerId(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.PayerId.CompareTo(payPalLog2.PayerId);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByTransactionId(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.TransactionId.CompareTo(payPalLog2.TransactionId);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByPaymentType(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.PaymentType.CompareTo(payPalLog2.PaymentType);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByPaymentStatus(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.PaymentStatus.CompareTo(payPalLog2.PaymentStatus);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByPendingReason(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.PendingReason.CompareTo(payPalLog2.PendingReason);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByReasonCode(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.ReasonCode.CompareTo(payPalLog2.ReasonCode);
        }
        /// <summary>
        /// Compares 2 instances of PayPalLog.
        /// </summary>
        public static int CompareByCurrencyCode(PayPalLog payPalLog1, PayPalLog payPalLog2)
        {
            return payPalLog1.CurrencyCode.CompareTo(payPalLog2.CurrencyCode);
        }

        #endregion


    }

}
