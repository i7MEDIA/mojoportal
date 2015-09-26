// Author:					Joe Audette
// Created:					2010-04-05
// Last Modified:			2012-03-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBPayPalLog
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PayPalLog ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("RequestType, ");
            sqlCommand.Append("ApiVersion, ");
            sqlCommand.Append("RawResponse, ");
            sqlCommand.Append("Token, ");
            sqlCommand.Append("PayerId, ");
            sqlCommand.Append("TransactionId, ");
            sqlCommand.Append("PaymentType, ");
            sqlCommand.Append("PaymentStatus, ");
            sqlCommand.Append("PendingReason, ");
            sqlCommand.Append("ReasonCode, ");
            sqlCommand.Append("CurrencyCode, ");
            sqlCommand.Append("ExchangeRate, ");
            sqlCommand.Append("CartTotal, ");
            sqlCommand.Append("PayPalAmt, ");
            sqlCommand.Append("TaxAmt, ");
            sqlCommand.Append("FeeAmt, ");
            sqlCommand.Append("SettleAmt, ");
            sqlCommand.Append("ProviderName, ");
            sqlCommand.Append("ReturnUrl, ");
            sqlCommand.Append("SerializedObject, ");
            sqlCommand.Append("PDTProviderName, ");
            sqlCommand.Append("IPNProviderName, ");
            sqlCommand.Append("Response ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@StoreGuid, ");
            sqlCommand.Append("@CartGuid, ");
            sqlCommand.Append("@RequestType, ");
            sqlCommand.Append("@ApiVersion, ");
            sqlCommand.Append("@RawResponse, ");
            sqlCommand.Append("@Token, ");
            sqlCommand.Append("@PayerId, ");
            sqlCommand.Append("@TransactionId, ");
            sqlCommand.Append("@PaymentType, ");
            sqlCommand.Append("@PaymentStatus, ");
            sqlCommand.Append("@PendingReason, ");
            sqlCommand.Append("@ReasonCode, ");
            sqlCommand.Append("@CurrencyCode, ");
            sqlCommand.Append("@ExchangeRate, ");
            sqlCommand.Append("@CartTotal, ");
            sqlCommand.Append("@PayPalAmt, ");
            sqlCommand.Append("@TaxAmt, ");
            sqlCommand.Append("@FeeAmt, ");
            sqlCommand.Append("@SettleAmt, ");
            sqlCommand.Append("@ProviderName, ");
            sqlCommand.Append("@ReturnUrl, ");
            sqlCommand.Append("@SerializedObject, ");
            sqlCommand.Append("@PDTProviderName, ");
            sqlCommand.Append("@IPNProviderName, ");
            sqlCommand.Append("@Response ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[29];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid;

            arParams[5] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid;

            arParams[6] = new SqlCeParameter("@RequestType", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new SqlCeParameter("@ApiVersion", SqlDbType.NVarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new SqlCeParameter("@Token", SqlDbType.NVarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new SqlCeParameter("@PayerId", SqlDbType.NVarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new SqlCeParameter("@TransactionId", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new SqlCeParameter("@PaymentType", SqlDbType.NVarChar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new SqlCeParameter("@PaymentStatus", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new SqlCeParameter("@PendingReason", SqlDbType.NVarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new SqlCeParameter("@ReasonCode", SqlDbType.NVarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new SqlCeParameter("@CurrencyCode", SqlDbType.NVarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new SqlCeParameter("@ExchangeRate", SqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new SqlCeParameter("@CartTotal", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new SqlCeParameter("@PayPalAmt", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new SqlCeParameter("@TaxAmt", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new SqlCeParameter("@FeeAmt", SqlDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new SqlCeParameter("@SettleAmt", SqlDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;

            arParams[23] = new SqlCeParameter("@ProviderName", SqlDbType.NVarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = providerName;

            arParams[24] = new SqlCeParameter("@ReturnUrl", SqlDbType.NVarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = returnUrl;

            arParams[25] = new SqlCeParameter("@SerializedObject", SqlDbType.NText);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = serializedObject;

            arParams[26] = new SqlCeParameter("@PDTProviderName", SqlDbType.NVarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = pdtProviderName;

            arParams[27] = new SqlCeParameter("@IPNProviderName", SqlDbType.NVarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = ipnProviderName;

            arParams[28] = new SqlCeParameter("@Response", SqlDbType.NVarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = response;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PayPalLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CreatedUtc = @CreatedUtc, ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("StoreGuid = @StoreGuid, ");
            sqlCommand.Append("CartGuid = @CartGuid, ");
            sqlCommand.Append("RequestType = @RequestType, ");
            sqlCommand.Append("ApiVersion = @ApiVersion, ");
            sqlCommand.Append("RawResponse = @RawResponse, ");
            sqlCommand.Append("Token = @Token, ");
            sqlCommand.Append("PayerId = @PayerId, ");
            sqlCommand.Append("TransactionId = @TransactionId, ");
            sqlCommand.Append("PaymentType = @PaymentType, ");
            sqlCommand.Append("PaymentStatus = @PaymentStatus, ");
            sqlCommand.Append("PendingReason = @PendingReason, ");
            sqlCommand.Append("ReasonCode = @ReasonCode, ");
            sqlCommand.Append("CurrencyCode = @CurrencyCode, ");
            sqlCommand.Append("ExchangeRate = @ExchangeRate, ");
            sqlCommand.Append("CartTotal = @CartTotal, ");
            sqlCommand.Append("PayPalAmt = @PayPalAmt, ");
            sqlCommand.Append("TaxAmt = @TaxAmt, ");
            sqlCommand.Append("FeeAmt = @FeeAmt, ");
            sqlCommand.Append("SettleAmt = @SettleAmt ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[23];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid;

            arParams[5] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid;

            arParams[6] = new SqlCeParameter("@RequestType", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new SqlCeParameter("@ApiVersion", SqlDbType.NVarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new SqlCeParameter("@Token", SqlDbType.NVarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new SqlCeParameter("@PayerId", SqlDbType.NVarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new SqlCeParameter("@TransactionId", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new SqlCeParameter("@PaymentType", SqlDbType.NVarChar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new SqlCeParameter("@PaymentStatus", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new SqlCeParameter("@PendingReason", SqlDbType.NVarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new SqlCeParameter("@ReasonCode", SqlDbType.NVarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new SqlCeParameter("@CurrencyCode", SqlDbType.NVarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new SqlCeParameter("@ExchangeRate", SqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new SqlCeParameter("@CartTotal", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new SqlCeParameter("@PayPalAmt", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new SqlCeParameter("@TaxAmt", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new SqlCeParameter("@FeeAmt", SqlDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new SqlCeParameter("@SettleAmt", SqlDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;

            

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the mp_PayPalLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByStore(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = @StoreGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetSetExpressCheckout(string token)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Token = @Token ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("RequestType = 'SetExpressCheckout' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Token", SqlDbType.NVarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = token;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMostRecentLog(Guid cartGuid, string requestType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");
            sqlCommand.Append("AND (RequestType = @RequestType OR @RequestType = '') ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("CreatedUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid;

            arParams[1] = new SqlCeParameter("@RequestType", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = requestType;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_PayPalLog table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }


    }
}
