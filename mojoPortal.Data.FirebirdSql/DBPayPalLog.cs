/// Author:					Joe Audette
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
using System.Globalization;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBPayPalLog
    {
        
       
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[29];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new FbParameter("@RequestType", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new FbParameter("@ApiVersion", FbDbType.VarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new FbParameter("@RawResponse", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new FbParameter("@Token", FbDbType.VarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new FbParameter("@PayerId", FbDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new FbParameter("@TransactionId", FbDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new FbParameter("@PaymentType", FbDbType.VarChar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new FbParameter("@PaymentStatus", FbDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new FbParameter("@PendingReason", FbDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new FbParameter("@ReasonCode", FbDbType.VarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new FbParameter("@CurrencyCode", FbDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new FbParameter("@ExchangeRate", FbDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new FbParameter("@CartTotal", FbDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new FbParameter("@PayPalAmt", FbDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new FbParameter("@TaxAmt", FbDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new FbParameter("@FeeAmt", FbDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new FbParameter("@SettleAmt", FbDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;

            arParams[23] = new FbParameter("@ProviderName", FbDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = providerName;

            arParams[24] = new FbParameter("@ReturnUrl", FbDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = returnUrl;

            arParams[25] = new FbParameter("@SerializedObject", FbDbType.VarChar);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = serializedObject;

            arParams[26] = new FbParameter("@PDTProviderName", FbDbType.VarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = pdtProviderName;

            arParams[27] = new FbParameter("@IPNProviderName", FbDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = ipnProviderName;

            arParams[28] = new FbParameter("@Response", FbDbType.VarChar, 255);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = response;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PayPalLog (");
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
            sqlCommand.Append("ProviderName, ");
            sqlCommand.Append("ReturnUrl, ");
            sqlCommand.Append("SerializedObject, ");

            sqlCommand.Append("PDTProviderName, ");
            sqlCommand.Append("IPNProviderName, ");
            sqlCommand.Append("Response, ");

            sqlCommand.Append("SettleAmt )");


            sqlCommand.Append(" VALUES (");
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
            sqlCommand.Append("@ProviderName, ");
            sqlCommand.Append("@ReturnUrl, ");
            sqlCommand.Append("@SerializedObject, ");

            sqlCommand.Append("@PDTProviderName, ");
            sqlCommand.Append("@IPNProviderName, ");
            sqlCommand.Append("@Response, ");

            sqlCommand.Append("@SettleAmt )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[23];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new FbParameter("@RequestType", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = requestType;

            arParams[7] = new FbParameter("@ApiVersion", FbDbType.VarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = apiVersion;

            arParams[8] = new FbParameter("@RawResponse", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = rawResponse;

            arParams[9] = new FbParameter("@Token", FbDbType.VarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = token;

            arParams[10] = new FbParameter("@PayerId", FbDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = payerId;

            arParams[11] = new FbParameter("@TransactionId", FbDbType.VarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = transactionId;

            arParams[12] = new FbParameter("@PaymentType", FbDbType.VarChar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = paymentType;

            arParams[13] = new FbParameter("@PaymentStatus", FbDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = paymentStatus;

            arParams[14] = new FbParameter("@PendingReason", FbDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pendingReason;

            arParams[15] = new FbParameter("@ReasonCode", FbDbType.VarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = reasonCode;

            arParams[16] = new FbParameter("@CurrencyCode", FbDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = currencyCode;

            arParams[17] = new FbParameter("@ExchangeRate", FbDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = exchangeRate;

            arParams[18] = new FbParameter("@CartTotal", FbDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = cartTotal;

            arParams[19] = new FbParameter("@PayPalAmt", FbDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = payPalAmt;

            arParams[20] = new FbParameter("@TaxAmt", FbDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = taxAmt;

            arParams[21] = new FbParameter("@FeeAmt", FbDbType.Decimal);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = feeAmt;

            arParams[22] = new FbParameter("@SettleAmt", FbDbType.Decimal);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = settleAmt;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@StoreGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Token = @Token ");
            sqlCommand.Append("AND RequestType = 'SetExpressCheckout' ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Token", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = token;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets an IDataReader with one row from the mp_PayPalLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetMostRecentLog(Guid cartGuid, string requestType)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = @CartGuid ");

            if (requestType.Length > 0)
            {
                sqlCommand.Append("AND RequestType = @RequestType  ");
            }
            sqlCommand.Append("ORDER BY CreatedUtc DESC ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@CartGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            arParams[1] = new FbParameter("@RequestType", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = requestType;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_PayPalLog table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PayPalLog ");
            sqlCommand.Append(";");

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }


        /// <summary>
        /// Gets a page of data from the mp_PayPalLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_PayPalLog  ");
            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("	; ");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = countryGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }
    }
}
