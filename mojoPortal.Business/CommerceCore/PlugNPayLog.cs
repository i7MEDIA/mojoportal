// Author:					Voir Hillaire
// Created:				    2009-03-10
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
    /// Represents a log of Plug N Pay activity in the AuthNet table
    /// </summary>
    public class PlugNPayLog
    {

        #region Constructors

        public PlugNPayLog()
        { }


        public PlugNPayLog(Guid rowGuid)
        {
            GetPlugNPayLog(rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid siteGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
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

        #region Private Methods

        /// <summary>
        /// Gets an instance of PlugNPayLog.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetPlugNPayLog(Guid rowGuid)
        {
            using (IDataReader reader = DBPlugNPayLog.GetOne(rowGuid))
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
        /// Persists a new instance of PlugNPayLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBPlugNPayLog.Create(
                this.rowGuid,
                this.createdUtc,
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

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of PlugNPayLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBPlugNPayLog.Update(
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





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of PlugNPayLog. Returns true on success.
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
        /// Deletes an instance of PlugNPayLog. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBPlugNPayLog.Delete(rowGuid);
        }


        

        private static List<PlugNPayLog> LoadListFromReader(IDataReader reader)
        {
            List<PlugNPayLog> PlugNPayLogList = new List<PlugNPayLog>();
            try
            {
                while (reader.Read())
                {
                    PlugNPayLog PlugNPayLog = new PlugNPayLog();
                    PlugNPayLog.rowGuid = new Guid(reader["RowGuid"].ToString());
                    PlugNPayLog.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    PlugNPayLog.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    PlugNPayLog.userGuid = new Guid(reader["UserGuid"].ToString());
                    PlugNPayLog.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    PlugNPayLog.cartGuid = new Guid(reader["CartGuid"].ToString());
                    PlugNPayLog.rawResponse = reader["RawResponse"].ToString();
                    PlugNPayLog.responseCode = reader["ResponseCode"].ToString();
                    PlugNPayLog.responseReasonCode = reader["ResponseReasonCode"].ToString();
                    PlugNPayLog.reason = reader["Reason"].ToString();
                    PlugNPayLog.avsCode = reader["AvsCode"].ToString();
                    PlugNPayLog.ccvCode = reader["CcvCode"].ToString();
                    PlugNPayLog.cavCode = reader["CavCode"].ToString();
                    PlugNPayLog.transactionId = reader["TransactionId"].ToString();
                    PlugNPayLog.transactionType = reader["TransactionType"].ToString();
                    PlugNPayLog.method = reader["Method"].ToString();
                    PlugNPayLog.authCode = reader["AuthCode"].ToString();
                    PlugNPayLog.amount = Convert.ToDecimal(reader["Amount"]);
                    PlugNPayLog.tax = Convert.ToDecimal(reader["Tax"]);
                    PlugNPayLog.duty = Convert.ToDecimal(reader["Duty"]);
                    PlugNPayLog.freight = Convert.ToDecimal(reader["Freight"]);
                    PlugNPayLogList.Add(PlugNPayLog);

                }
            }
            finally
            {
                reader.Close();
            }

            return PlugNPayLogList;

        }

        

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PlugNPayLog table.
        /// </summary>
        /// <param name="cartGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            return DBPlugNPayLog.GetByCart(cartGuid);
        }

        ///// <summary>
        ///// Gets an IList with page of instances of PlugNPayLog.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static List<PlugNPayLog> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBPlugNPayLog.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByCreatedUtc(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.CreatedUtc.CompareTo(PlugNPayLog2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByRawResponse(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.RawResponse.CompareTo(PlugNPayLog2.RawResponse);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByResponseCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.ResponseCode.CompareTo(PlugNPayLog2.ResponseCode);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByResponseReasonCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.ResponseReasonCode.CompareTo(PlugNPayLog2.ResponseReasonCode);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByReason(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.Reason.CompareTo(PlugNPayLog2.Reason);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByAvsCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.AvsCode.CompareTo(PlugNPayLog2.AvsCode);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByCcvCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.CcvCode.CompareTo(PlugNPayLog2.CcvCode);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByCavCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.CavCode.CompareTo(PlugNPayLog2.CavCode);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByTransactionId(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.TransactionId.CompareTo(PlugNPayLog2.TransactionId);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByTransactionType(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.TransactionType.CompareTo(PlugNPayLog2.TransactionType);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByMethod(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.Method.CompareTo(PlugNPayLog2.Method);
        }
        /// <summary>
        /// Compares 2 instances of PlugNPayLog.
        /// </summary>
        public static int CompareByAuthCode(PlugNPayLog PlugNPayLog1, PlugNPayLog PlugNPayLog2)
        {
            return PlugNPayLog1.AuthCode.CompareTo(PlugNPayLog2.AuthCode);
        }

        #endregion


    }

}
