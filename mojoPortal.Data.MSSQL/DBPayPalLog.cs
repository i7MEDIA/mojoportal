/// Author:					
/// Created:				2008-06-22
/// Last Modified:			2012-03-04
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
using System.Configuration;

namespace mojoPortal.Data
{
    public static class DBPayPalLog
    {
       
        /// <summary>
        /// Inserts a row in the mp_PayPalLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="requestType"> requestType </param>
        /// <param name="apiVersion"> apiVersion </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="token"> token </param>
        /// <param name="payerId"> payerId </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="paymentType"> paymentType </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="pendingReason"> pendingReason </param>
        /// <param name="reasonCode"> reasonCode </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="exchangeRate"> exchangeRate </param>
        /// <param name="cartTotal"> cartTotal </param>
        /// <param name="payPalAmt"> payPalAmt </param>
        /// <param name="taxAmt"> taxAmt </param>
        /// <param name="feeAmt"> feeAmt </param>
        /// <param name="settleAmt"> settleAmt </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string requestType,
            string apiVersion,
            string rawResponse,
            string token,
            string payerId,
            string transactionId,
            string paymentType,
            string paymentStatus,
            string pendingReason,
            string reasonCode,
            string currencyCode,
            decimal exchangeRate,
            decimal cartTotal,
            decimal payPalAmt,
            decimal taxAmt,
            decimal feeAmt,
            decimal settleAmt,
            string providerName,
            string returnUrl,
            string serializedObject,
            string pdtProviderName,
            string ipnProviderName,
            string response)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_Insert", 29);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@RequestType", SqlDbType.NVarChar, 255, ParameterDirection.Input, requestType);
            sph.DefineSqlParameter("@ApiVersion", SqlDbType.NVarChar, 50, ParameterDirection.Input, apiVersion);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, token);
            sph.DefineSqlParameter("@PayerId", SqlDbType.NVarChar, 50, ParameterDirection.Input, payerId);
            sph.DefineSqlParameter("@TransactionId", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionId);
            sph.DefineSqlParameter("@PaymentType", SqlDbType.NVarChar, 10, ParameterDirection.Input, paymentType);
            sph.DefineSqlParameter("@PaymentStatus", SqlDbType.NVarChar, 50, ParameterDirection.Input, paymentStatus);
            sph.DefineSqlParameter("@PendingReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, pendingReason);
            sph.DefineSqlParameter("@ReasonCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, reasonCode);
            sph.DefineSqlParameter("@CurrencyCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, currencyCode);
            sph.DefineSqlParameter("@ExchangeRate", SqlDbType.Decimal, ParameterDirection.Input, exchangeRate);
            sph.DefineSqlParameter("@CartTotal", SqlDbType.Decimal, ParameterDirection.Input, cartTotal);
            sph.DefineSqlParameter("@PayPalAmt", SqlDbType.Decimal, ParameterDirection.Input, payPalAmt);
            sph.DefineSqlParameter("@TaxAmt", SqlDbType.Decimal, ParameterDirection.Input, taxAmt);
            sph.DefineSqlParameter("@FeeAmt", SqlDbType.Decimal, ParameterDirection.Input, feeAmt);
            sph.DefineSqlParameter("@SettleAmt", SqlDbType.Decimal, ParameterDirection.Input, settleAmt);
            sph.DefineSqlParameter("@ProviderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, providerName);
            sph.DefineSqlParameter("@ReturnUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, returnUrl);
            sph.DefineSqlParameter("@SerializedObject", SqlDbType.NVarChar, -1, ParameterDirection.Input, serializedObject);
            sph.DefineSqlParameter("@PDTProviderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pdtProviderName);
            sph.DefineSqlParameter("@IPNProviderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, ipnProviderName);
            sph.DefineSqlParameter("@Response", SqlDbType.NVarChar, 255, ParameterDirection.Input, response);

            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_PayPalLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="requestType"> requestType </param>
        /// <param name="apiVersion"> apiVersion </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="token"> token </param>
        /// <param name="payerId"> payerId </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="paymentType"> paymentType </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="pendingReason"> pendingReason </param>
        /// <param name="reasonCode"> reasonCode </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="exchangeRate"> exchangeRate </param>
        /// <param name="cartTotal"> cartTotal </param>
        /// <param name="payPalAmt"> payPalAmt </param>
        /// <param name="taxAmt"> taxAmt </param>
        /// <param name="feeAmt"> feeAmt </param>
        /// <param name="settleAmt"> settleAmt </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string requestType,
            string apiVersion,
            string rawResponse,
            string token,
            string payerId,
            string transactionId,
            string paymentType,
            string paymentStatus,
            string pendingReason,
            string reasonCode,
            string currencyCode,
            decimal exchangeRate,
            decimal cartTotal,
            decimal payPalAmt,
            decimal taxAmt,
            decimal feeAmt,
            decimal settleAmt)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_Update", 23);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@RequestType", SqlDbType.NVarChar, 255, ParameterDirection.Input, requestType);
            sph.DefineSqlParameter("@ApiVersion", SqlDbType.NVarChar, 50, ParameterDirection.Input, apiVersion);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, token);
            sph.DefineSqlParameter("@PayerId", SqlDbType.NVarChar, 50, ParameterDirection.Input, payerId);
            sph.DefineSqlParameter("@TransactionId", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionId);
            sph.DefineSqlParameter("@PaymentType", SqlDbType.NVarChar, 10, ParameterDirection.Input, paymentType);
            sph.DefineSqlParameter("@PaymentStatus", SqlDbType.NVarChar, 50, ParameterDirection.Input, paymentStatus);
            sph.DefineSqlParameter("@PendingReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, pendingReason);
            sph.DefineSqlParameter("@ReasonCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, reasonCode);
            sph.DefineSqlParameter("@CurrencyCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, currencyCode);
            sph.DefineSqlParameter("@ExchangeRate", SqlDbType.Decimal, ParameterDirection.Input, exchangeRate);
            sph.DefineSqlParameter("@CartTotal", SqlDbType.Decimal, ParameterDirection.Input, cartTotal);
            sph.DefineSqlParameter("@PayPalAmt", SqlDbType.Decimal, ParameterDirection.Input, payPalAmt);
            sph.DefineSqlParameter("@TaxAmt", SqlDbType.Decimal, ParameterDirection.Input, taxAmt);
            sph.DefineSqlParameter("@FeeAmt", SqlDbType.Decimal, ParameterDirection.Input, feeAmt);
            sph.DefineSqlParameter("@SettleAmt", SqlDbType.Decimal, ParameterDirection.Input, settleAmt);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_PayPalLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_DeleteByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PayPalLog_DeleteByStore", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PayPalLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PayPalLog_SelectByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetSetExpressCheckout(string token)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PayPalLog_SelectSetExpressCheckoutByToken", 1);
            sph.DefineSqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, token);
            return sph.ExecuteReader();

        }

        
        public static IDataReader GetMostRecentLog(Guid cartGuid, string requestType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PayPalLog_SelectNewestLog", 2);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier,  ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@RequestType", SqlDbType.NVarChar, 255, ParameterDirection.Input, requestType);
            return sph.ExecuteReader();

        }


        ///// <summary>
        ///// Gets a count of rows in the mp_PayPalLog table.
        ///// </summary>
        //public static int GetCount()
        //{

        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(
        //        GetConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_PayPalLog_GetCount",
        //        null));

        //}

        ///// <summary>
        ///// Gets an IDataReader with all rows in the mp_PayPalLog table.
        ///// </summary>
        //public static IDataReader GetAll()
        //{

        //    return SqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_PayPalLog_SelectAll",
        //        null);

        //}

        ///// <summary>
        ///// Gets a page of data from the mp_PayPalLog table.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static IDataReader GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    totalPages = 1;
        //    int totalRows
        //        = GetCount();

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_PayPalLog_SelectPage", 2);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();

        //}

    }

}
