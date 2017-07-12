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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
    public static class DBPaymentLog
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }

        /// <summary>
        /// Inserts a row in the mp_PaymentLog table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="cartGuid"> cartGuid </param>
        /// <param name="provider"> provider </param>
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
            string provider,
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
            sqlCommand.Append("INSERT INTO mp_PaymentLog (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("CartGuid, ");
            sqlCommand.Append("Provider, ");
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
            sqlCommand.Append("Freight )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":StoreGuid, ");
            sqlCommand.Append(":CartGuid, ");
            sqlCommand.Append(":Provider, ");
            sqlCommand.Append(":RawResponse, ");
            sqlCommand.Append(":ResponseCode, ");
            sqlCommand.Append(":ResponseReasonCode, ");
            sqlCommand.Append(":Reason, ");
            sqlCommand.Append(":AvsCode, ");
            sqlCommand.Append(":CcvCode, ");
            sqlCommand.Append(":CavCode, ");
            sqlCommand.Append(":TransactionId, ");
            sqlCommand.Append(":TransactionType, ");
            sqlCommand.Append(":Method, ");
            sqlCommand.Append(":AuthCode, ");
            sqlCommand.Append(":Amount, ");
            sqlCommand.Append(":Tax, ");
            sqlCommand.Append(":Duty, ");
            sqlCommand.Append(":Freight )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[22];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":StoreGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new SqliteParameter(":CartGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new SqliteParameter(":RawResponse", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rawResponse;

            arParams[7] = new SqliteParameter(":ResponseCode", DbType.String, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseCode;

            arParams[8] = new SqliteParameter(":ResponseReasonCode", DbType.String, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = responseReasonCode;

            arParams[9] = new SqliteParameter(":Reason", DbType.Object);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = reason;

            arParams[10] = new SqliteParameter(":AvsCode", DbType.String, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = avsCode;

            arParams[11] = new SqliteParameter(":CcvCode", DbType.String, 1);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ccvCode;

            arParams[12] = new SqliteParameter(":CavCode", DbType.String, 1);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cavCode;

            arParams[13] = new SqliteParameter(":TransactionId", DbType.String, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionId;

            arParams[14] = new SqliteParameter(":TransactionType", DbType.String, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = transactionType;

            arParams[15] = new SqliteParameter(":Method", DbType.String, 20);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = method;

            arParams[16] = new SqliteParameter(":AuthCode", DbType.String, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = authCode;

            arParams[17] = new SqliteParameter(":Amount", DbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = amount;

            arParams[18] = new SqliteParameter(":Tax", DbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = tax;

            arParams[19] = new SqliteParameter(":Duty", DbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = duty;

            arParams[20] = new SqliteParameter(":Freight", DbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = freight;

            arParams[21] = new SqliteParameter(":Provider", DbType.String, 100);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = provider;


            int rowsAffected = 0;
            rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_PaymentLog table. Returns true if row updated.
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

            sqlCommand.Append("UPDATE mp_PaymentLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = :SiteGuid, ");
            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("StoreGuid = :StoreGuid, ");
            sqlCommand.Append("CartGuid = :CartGuid, ");
            sqlCommand.Append("RawResponse = :RawResponse, ");
            sqlCommand.Append("ResponseCode = :ResponseCode, ");
            sqlCommand.Append("ResponseReasonCode = :ResponseReasonCode, ");
            sqlCommand.Append("Reason = :Reason, ");
            sqlCommand.Append("AvsCode = :AvsCode, ");
            sqlCommand.Append("CcvCode = :CcvCode, ");
            sqlCommand.Append("CavCode = :CavCode, ");
            sqlCommand.Append("TransactionId = :TransactionId, ");
            sqlCommand.Append("TransactionType = :TransactionType, ");
            sqlCommand.Append("Method = :Method, ");
            sqlCommand.Append("AuthCode = :AuthCode, ");
            sqlCommand.Append("Amount = :Amount, ");
            sqlCommand.Append("Tax = :Tax, ");
            sqlCommand.Append("Duty = :Duty, ");
            sqlCommand.Append("Freight = :Freight ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[20];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new SqliteParameter(":StoreGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = storeGuid.ToString();

            arParams[4] = new SqliteParameter(":CartGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cartGuid.ToString();

            arParams[5] = new SqliteParameter(":RawResponse", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rawResponse;

            arParams[6] = new SqliteParameter(":ResponseCode", DbType.String, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = responseCode;

            arParams[7] = new SqliteParameter(":ResponseReasonCode", DbType.String, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseReasonCode;

            arParams[8] = new SqliteParameter(":Reason", DbType.Object);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = reason;

            arParams[9] = new SqliteParameter(":AvsCode", DbType.String, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = avsCode;

            arParams[10] = new SqliteParameter(":CcvCode", DbType.String, 1);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ccvCode;

            arParams[11] = new SqliteParameter(":CavCode", DbType.String, 1);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = cavCode;

            arParams[12] = new SqliteParameter(":TransactionId", DbType.String, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = transactionId;

            arParams[13] = new SqliteParameter(":TransactionType", DbType.String, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionType;

            arParams[14] = new SqliteParameter(":Method", DbType.String, 20);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = method;

            arParams[15] = new SqliteParameter(":AuthCode", DbType.String, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = authCode;

            arParams[16] = new SqliteParameter(":Amount", DbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = amount;

            arParams[17] = new SqliteParameter(":Tax", DbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = tax;

            arParams[18] = new SqliteParameter(":Duty", DbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = duty;

            arParams[19] = new SqliteParameter(":Freight", DbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = freight;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_PaymentLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PaymentLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PaymentLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PaymentLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PaymentLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_PaymentLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = :CartGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CartGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
