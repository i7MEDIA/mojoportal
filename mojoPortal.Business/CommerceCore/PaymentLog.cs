// Author:					
// Created:				    2012-01-09
// Last Modified:			2012-01-09
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
    /// Represents a log of payment activity, this class is a consolidation of legacy classes
    /// AuthorizeNetLog and PlugNPayLog to reduce duplication and simplify adding more payment gateways
    /// </summary>
    public class PaymentLog
    {
        #region Constructors

        public PaymentLog()
        { }


        public PaymentLog(Guid rowGuid)
        {
            GetPaymentLog( rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid siteGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
        private string provider = string.Empty;
        private string rawResponse = string.Empty;
        private string responseCode = string.Empty;
        private string responseReasonCode = string.Empty;
        private string reason = string.Empty;
        private string avsCode = string.Empty;
        private string ccvCode = string.Empty;
        private string cavCode = string.Empty;
        private string transactionId = string.Empty;
        private string transactionType = string.Empty;
        private string method = string.Empty;
        private string authCode = string.Empty;
        private decimal amount = 0;
        private decimal tax = 0;
        private decimal duty = 0;
        private decimal freight = 0;

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
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }
        public string RawResponse
        {
            get { return rawResponse; }
            set { rawResponse = value; }
        }
        public string ResponseCode
        {
            get { return responseCode; }
            set { responseCode = value; }
        }
        public string ResponseReasonCode
        {
            get { return responseReasonCode; }
            set { responseReasonCode = value; }
        }
        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        public string AvsCode
        {
            get { return avsCode; }
            set { avsCode = value; }
        }
        public string CcvCode
        {
            get { return ccvCode; }
            set { ccvCode = value; }
        }
        public string CavCode
        {
            get { return cavCode; }
            set { cavCode = value; }
        }
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        public string AuthCode
        {
            get { return authCode; }
            set { authCode = value; }
        }
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        public decimal Duty
        {
            get { return duty; }
            set { duty = value; }
        }
        public decimal Freight
        {
            get { return freight; }
            set { freight = value; }
        }

        #endregion

        /// <summary>
        /// Gets an instance of PaymentLog.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetPaymentLog(Guid rowGuid)
        {
            using (IDataReader reader = DBPaymentLog.GetOne(rowGuid))
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
                this.provider = reader["Provider"].ToString();
                this.rawResponse = reader["RawResponse"].ToString();
                this.responseCode = reader["ResponseCode"].ToString();
                this.responseReasonCode = reader["ResponseReasonCode"].ToString();
                this.reason = reader["Reason"].ToString();
                this.avsCode = reader["AvsCode"].ToString();
                this.ccvCode = reader["CcvCode"].ToString();
                this.cavCode = reader["CavCode"].ToString();
                this.transactionId = reader["TransactionId"].ToString();
                this.transactionType = reader["TransactionType"].ToString();
                this.method = reader["Method"].ToString();
                this.authCode = reader["AuthCode"].ToString();
                this.amount = Convert.ToDecimal(reader["Amount"]);
                this.tax = Convert.ToDecimal(reader["Tax"]);
                this.duty = Convert.ToDecimal(reader["Duty"]);
                this.freight = Convert.ToDecimal(reader["Freight"]);

            }

        }

        /// <summary>
        /// Persists a new instance of PaymentLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBPaymentLog.Create(
                this.rowGuid,
                this.createdUtc,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.provider,
                this.rawResponse,
                this.responseCode,
                this.responseReasonCode,
                this.reason,
                this.avsCode,
                this.ccvCode,
                this.cavCode,
                this.transactionId,
                this.transactionType,
                this.method,
                this.authCode,
                this.amount,
                this.tax,
                this.duty,
                this.freight);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of PaymentLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBPaymentLog.Update(
                this.rowGuid,
                this.siteGuid,
                this.userGuid,
                this.storeGuid,
                this.cartGuid,
                this.rawResponse,
                this.responseCode,
                this.responseReasonCode,
                this.reason,
                this.avsCode,
                this.ccvCode,
                this.cavCode,
                this.transactionId,
                this.transactionType,
                this.method,
                this.authCode,
                this.amount,
                this.tax,
                this.duty,
                this.freight);

        }


        #region Public Methods

        /// <summary>
        /// Saves this instance of AuthorizeNetLog. Returns true on success.
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

        /// <summary>
        /// Deletes an instance of PaymentLog. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBPaymentLog.Delete(rowGuid);
        }

        public static IDataReader GetByCart(Guid cartGuid)
        {
            return DBPaymentLog.GetByCart(cartGuid);
        }

    }
}
