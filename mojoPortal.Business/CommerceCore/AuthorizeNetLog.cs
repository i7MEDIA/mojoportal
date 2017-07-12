// Author:					
// Created:				    2008-03-10
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
    /// Represents a log of Authorize.NET activity
    /// </summary>
    public class AuthorizeNetLog
    {

        #region Constructors

        public AuthorizeNetLog()
        { }


        public AuthorizeNetLog(
            Guid rowGuid)
        {
            GetAuthorizeNetLog(
                rowGuid);
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
        /// Gets an instance of AuthorizeNetLog.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetAuthorizeNetLog(Guid rowGuid)
        {
            using (IDataReader reader = DBAuthorizeNetLog.GetOne(rowGuid))
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
        /// Persists a new instance of AuthorizeNetLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.rowGuid = Guid.NewGuid();

            int rowsAffected = DBAuthorizeNetLog.Create(
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
        /// Updates this instance of AuthorizeNetLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBAuthorizeNetLog.Update(
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

        #region Static Methods

        /// <summary>
        /// Deletes an instance of AuthorizeNetLog. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBAuthorizeNetLog.Delete(rowGuid);
        }


        ///// <summary>
        ///// Gets a count of AuthorizeNetLog. 
        ///// </summary>
        //public static int GetCount()
        //{
        //    return DBAuthorizeNetLog.GetCount();
        //}

        private static List<AuthorizeNetLog> LoadListFromReader(IDataReader reader)
        {
            List<AuthorizeNetLog> authorizeNetLogList = new List<AuthorizeNetLog>();
            try
            {
                while (reader.Read())
                {
                    AuthorizeNetLog authorizeNetLog = new AuthorizeNetLog();
                    authorizeNetLog.rowGuid = new Guid(reader["RowGuid"].ToString());
                    authorizeNetLog.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    authorizeNetLog.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    authorizeNetLog.userGuid = new Guid(reader["UserGuid"].ToString());
                    authorizeNetLog.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    authorizeNetLog.cartGuid = new Guid(reader["CartGuid"].ToString());
                    authorizeNetLog.rawResponse = reader["RawResponse"].ToString();
                    authorizeNetLog.responseCode = reader["ResponseCode"].ToString();
                    authorizeNetLog.responseReasonCode = reader["ResponseReasonCode"].ToString();
                    authorizeNetLog.reason = reader["Reason"].ToString();
                    authorizeNetLog.avsCode = reader["AvsCode"].ToString();
                    authorizeNetLog.ccvCode = reader["CcvCode"].ToString();
                    authorizeNetLog.cavCode = reader["CavCode"].ToString();
                    authorizeNetLog.transactionId = reader["TransactionId"].ToString();
                    authorizeNetLog.transactionType = reader["TransactionType"].ToString();
                    authorizeNetLog.method = reader["Method"].ToString();
                    authorizeNetLog.authCode = reader["AuthCode"].ToString();
                    authorizeNetLog.amount = Convert.ToDecimal(reader["Amount"]);
                    authorizeNetLog.tax = Convert.ToDecimal(reader["Tax"]);
                    authorizeNetLog.duty = Convert.ToDecimal(reader["Duty"]);
                    authorizeNetLog.freight = Convert.ToDecimal(reader["Freight"]);
                    authorizeNetLogList.Add(authorizeNetLog);

                }
            }
            finally
            {
                reader.Close();
            }

            return authorizeNetLogList;

        }

        ///// <summary>
        ///// Gets an IList with all instances of AuthorizeNetLog.
        ///// </summary>
        //public static List<AuthorizeNetLog> GetAll()
        //{
        //    IDataReader reader = DBAuthorizeNetLog.GetAll();
        //    return LoadListFromReader(reader);

        //}

        /// <summary>
        /// Gets an IDataReader with rows from the mp_AuthorizeNetLog table.
        /// </summary>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            return DBAuthorizeNetLog.GetByCart(cartGuid);
        }

        ///// <summary>
        ///// Gets an IList with page of instances of AuthorizeNetLog.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static List<AuthorizeNetLog> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBAuthorizeNetLog.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByCreatedUtc(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.CreatedUtc.CompareTo(authorizeNetLog2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByRawResponse(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.RawResponse.CompareTo(authorizeNetLog2.RawResponse);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByResponseCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.ResponseCode.CompareTo(authorizeNetLog2.ResponseCode);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByResponseReasonCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.ResponseReasonCode.CompareTo(authorizeNetLog2.ResponseReasonCode);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByReason(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.Reason.CompareTo(authorizeNetLog2.Reason);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByAvsCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.AvsCode.CompareTo(authorizeNetLog2.AvsCode);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByCcvCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.CcvCode.CompareTo(authorizeNetLog2.CcvCode);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByCavCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.CavCode.CompareTo(authorizeNetLog2.CavCode);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByTransactionId(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.TransactionId.CompareTo(authorizeNetLog2.TransactionId);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByTransactionType(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.TransactionType.CompareTo(authorizeNetLog2.TransactionType);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByMethod(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.Method.CompareTo(authorizeNetLog2.Method);
        }
        /// <summary>
        /// Compares 2 instances of AuthorizeNetLog.
        /// </summary>
        public static int CompareByAuthCode(AuthorizeNetLog authorizeNetLog1, AuthorizeNetLog authorizeNetLog2)
        {
            return authorizeNetLog1.AuthCode.CompareTo(authorizeNetLog2.AuthCode);
        }

        #endregion


    }

}
