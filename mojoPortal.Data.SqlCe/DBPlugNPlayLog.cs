// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2010-04-06
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
    public static class DBPlugNPayLog
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_PlugNPayLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="responseCode"> responseCode </param>
        /// <param name="responseReasonCode"> responseReasonCode </param>
        /// <param name="reason"> reason </param>
        /// <param name="avsCode"> avsCode </param>
        /// <param name="ccvCode"> ccvCode </param>
        /// <param name="cavCode"> cavCode </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="transactionType"> transactionType </param>
        /// <param name="method"> method </param>
        /// <param name="authCode"> authCode </param>
        /// <param name="amount"> amount </param>
        /// <param name="tax"> tax </param>
        /// <param name="duty"> duty </param>
        /// <param name="freight"> freight </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            DateTime createdUtc,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string rawResponse,
            string responseCode,
            string responseReasonCode,
            string reason,
            string avsCode,
            string ccvCode,
            string cavCode,
            string transactionId,
            string transactionType,
            string method,
            string authCode,
            decimal amount,
            decimal tax,
            decimal duty,
            decimal freight)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PlugNPayLog ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("RawResponse, ");
            sqlCommand.Append("ResponseCode, ");
            sqlCommand.Append("ResponseReasonCode, ");
            sqlCommand.Append("Reason, ");
            sqlCommand.Append("AvsCode, ");
            sqlCommand.Append("CcvCode, ");
            sqlCommand.Append("CavCode, ");
            sqlCommand.Append("TransactionId, ");
            sqlCommand.Append("TransactionType, ");
            sqlCommand.Append("Method, ");
            sqlCommand.Append("AuthCode, ");
            sqlCommand.Append("Amount, ");
            sqlCommand.Append("Tax, ");
            sqlCommand.Append("Duty, ");
            sqlCommand.Append("Freight ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@StoreGuid, ");
            sqlCommand.Append("@CartGuid, ");
            sqlCommand.Append("@RawResponse, ");
            sqlCommand.Append("@ResponseCode, ");
            sqlCommand.Append("@ResponseReasonCode, ");
            sqlCommand.Append("@Reason, ");
            sqlCommand.Append("@AvsCode, ");
            sqlCommand.Append("@CcvCode, ");
            sqlCommand.Append("@CavCode, ");
            sqlCommand.Append("@TransactionId, ");
            sqlCommand.Append("@TransactionType, ");
            sqlCommand.Append("@Method, ");
            sqlCommand.Append("@AuthCode, ");
            sqlCommand.Append("@Amount, ");
            sqlCommand.Append("@Tax, ");
            sqlCommand.Append("@Duty, ");
            sqlCommand.Append("@Freight ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[21];

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

            arParams[6] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rawResponse;

            arParams[7] = new SqlCeParameter("@ResponseCode", SqlDbType.NVarChar, 10);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseCode;

            arParams[8] = new SqlCeParameter("@ResponseReasonCode", SqlDbType.NVarChar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = responseReasonCode;

            arParams[9] = new SqlCeParameter("@Reason", SqlDbType.NText);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = reason;

            arParams[10] = new SqlCeParameter("@AvsCode", SqlDbType.NVarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = avsCode;

            arParams[11] = new SqlCeParameter("@CcvCode", SqlDbType.NVarChar, 10);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ccvCode;

            arParams[12] = new SqlCeParameter("@CavCode", SqlDbType.NVarChar, 10);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cavCode;

            arParams[13] = new SqlCeParameter("@TransactionId", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionId;

            arParams[14] = new SqlCeParameter("@TransactionType", SqlDbType.NVarChar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = transactionType;

            arParams[15] = new SqlCeParameter("@Method", SqlDbType.NVarChar, 20);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = method;

            arParams[16] = new SqlCeParameter("@AuthCode", SqlDbType.NVarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = authCode;

            arParams[17] = new SqlCeParameter("@Amount", SqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = amount;

            arParams[18] = new SqlCeParameter("@Tax", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = tax;

            arParams[19] = new SqlCeParameter("@Duty", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = duty;

            arParams[20] = new SqlCeParameter("@Freight", SqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = freight;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_PlugNPayLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="rawResponse"> rawResponse </param>
        /// <param name="responseCode"> responseCode </param>
        /// <param name="responseReasonCode"> responseReasonCode </param>
        /// <param name="reason"> reason </param>
        /// <param name="avsCode"> avsCode </param>
        /// <param name="ccvCode"> ccvCode </param>
        /// <param name="cavCode"> cavCode </param>
        /// <param name="transactionId"> transactionId </param>
        /// <param name="transactionType"> transactionType </param>
        /// <param name="method"> method </param>
        /// <param name="authCode"> authCode </param>
        /// <param name="amount"> amount </param>
        /// <param name="tax"> tax </param>
        /// <param name="duty"> duty </param>
        /// <param name="freight"> freight </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            Guid siteGuid,
            Guid userGuid,
            Guid storeGuid,
            Guid cartGuid,
            string rawResponse,
            string responseCode,
            string responseReasonCode,
            string reason,
            string avsCode,
            string ccvCode,
            string cavCode,
            string transactionId,
            string transactionType,
            string method,
            string authCode,
            decimal amount,
            decimal tax,
            decimal duty,
            decimal freight)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PlugNPayLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("StoreGuid = @StoreGuid, ");
            sqlCommand.Append("CartGuid = @CartGuid, ");
            sqlCommand.Append("RawResponse = @RawResponse, ");
            sqlCommand.Append("ResponseCode = @ResponseCode, ");
            sqlCommand.Append("ResponseReasonCode = @ResponseReasonCode, ");
            sqlCommand.Append("Reason = @Reason, ");
            sqlCommand.Append("AvsCode = @AvsCode, ");
            sqlCommand.Append("CcvCode = @CcvCode, ");
            sqlCommand.Append("CavCode = @CavCode, ");
            sqlCommand.Append("TransactionId = @TransactionId, ");
            sqlCommand.Append("TransactionType = @TransactionType, ");
            sqlCommand.Append("Method = @Method, ");
            sqlCommand.Append("AuthCode = @AuthCode, ");
            sqlCommand.Append("Amount = @Amount, ");
            sqlCommand.Append("Tax = @Tax, ");
            sqlCommand.Append("Duty = @Duty, ");
            sqlCommand.Append("Freight = @Freight ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[20];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@StoreGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = storeGuid;

            arParams[4] = new SqlCeParameter("@CartGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cartGuid;

            arParams[5] = new SqlCeParameter("@RawResponse", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rawResponse;

            arParams[6] = new SqlCeParameter("@ResponseCode", SqlDbType.NVarChar, 10);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = responseCode;

            arParams[7] = new SqlCeParameter("@ResponseReasonCode", SqlDbType.NVarChar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseReasonCode;

            arParams[8] = new SqlCeParameter("@Reason", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = reason;

            arParams[9] = new SqlCeParameter("@AvsCode", SqlDbType.NVarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = avsCode;

            arParams[10] = new SqlCeParameter("@CcvCode", SqlDbType.NVarChar, 10);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ccvCode;

            arParams[11] = new SqlCeParameter("@CavCode", SqlDbType.NVarChar, 10);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = cavCode;

            arParams[12] = new SqlCeParameter("@TransactionId", SqlDbType.NVarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = transactionId;

            arParams[13] = new SqlCeParameter("@TransactionType", SqlDbType.NVarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionType;

            arParams[14] = new SqlCeParameter("@Method", SqlDbType.NVarChar, 20);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = method;

            arParams[15] = new SqlCeParameter("@AuthCode", SqlDbType.NVarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = authCode;

            arParams[16] = new SqlCeParameter("@Amount", SqlDbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = amount;

            arParams[17] = new SqlCeParameter("@Tax", SqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = tax;

            arParams[18] = new SqlCeParameter("@Duty", SqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = duty;

            arParams[19] = new SqlCeParameter("@Freight", SqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = freight;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the mp_PlugNPayLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PlugNPayLog ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PlugNPayLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PlugNPayLog ");
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
        /// Gets an IDataReader with rows from the mp_PlugNPayLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PlugNPayLog ");
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


    }
}
