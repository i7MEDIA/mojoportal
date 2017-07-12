///							DBAuthorizeNetLog.cs
/// Author:					
/// Created:				2008-06-22
/// Last Modified:			2012-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
    public static class DBAuthorizeNetLog
    {
        
       

        /// <summary>
        /// Inserts a row in the mp_AuthorizeNetLog table. Returns rows affected count.
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

            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_AuthorizeNetLog (");
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
            sqlCommand.Append("Freight )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?CartGuid, ");
            sqlCommand.Append("?RawResponse, ");
            sqlCommand.Append("?ResponseCode, ");
            sqlCommand.Append("?ResponseReasonCode, ");
            sqlCommand.Append("?Reason, ");
            sqlCommand.Append("?AvsCode, ");
            sqlCommand.Append("?CcvCode, ");
            sqlCommand.Append("?CavCode, ");
            sqlCommand.Append("?TransactionId, ");
            sqlCommand.Append("?TransactionType, ");
            sqlCommand.Append("?Method, ");
            sqlCommand.Append("?AuthCode, ");
            sqlCommand.Append("?Amount, ");
            sqlCommand.Append("?Tax, ");
            sqlCommand.Append("?Duty, ");
            sqlCommand.Append("?Freight )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[21];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = storeGuid.ToString();

            arParams[5] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = cartGuid.ToString();

            arParams[6] = new MySqlParameter("?RawResponse", MySqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rawResponse;

            arParams[7] = new MySqlParameter("?ResponseCode", MySqlDbType.VarChar, 1);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseCode;

            arParams[8] = new MySqlParameter("?ResponseReasonCode", MySqlDbType.VarChar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = responseReasonCode;

            arParams[9] = new MySqlParameter("?Reason", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = reason;

            arParams[10] = new MySqlParameter("?AvsCode", MySqlDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = avsCode;

            arParams[11] = new MySqlParameter("?CcvCode", MySqlDbType.VarChar, 1);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = ccvCode;

            arParams[12] = new MySqlParameter("?CavCode", MySqlDbType.VarChar, 1);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cavCode;

            arParams[13] = new MySqlParameter("?TransactionId", MySqlDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionId;

            arParams[14] = new MySqlParameter("?TransactionType", MySqlDbType.VarChar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = transactionType;

            arParams[15] = new MySqlParameter("?Method", MySqlDbType.VarChar, 20);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = method;

            arParams[16] = new MySqlParameter("?AuthCode", MySqlDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = authCode;

            arParams[17] = new MySqlParameter("?Amount", MySqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = amount;

            arParams[18] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = tax;

            arParams[19] = new MySqlParameter("?Duty", MySqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = duty;

            arParams[20] = new MySqlParameter("?Freight", MySqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = freight;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_AuthorizeNetLog table. Returns true if row updated.
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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_AuthorizeNetLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid, ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("StoreGuid = ?StoreGuid, ");
            sqlCommand.Append("CartGuid = ?CartGuid, ");
            sqlCommand.Append("RawResponse = ?RawResponse, ");
            sqlCommand.Append("ResponseCode = ?ResponseCode, ");
            sqlCommand.Append("ResponseReasonCode = ?ResponseReasonCode, ");
            sqlCommand.Append("Reason = ?Reason, ");
            sqlCommand.Append("AvsCode = ?AvsCode, ");
            sqlCommand.Append("CcvCode = ?CcvCode, ");
            sqlCommand.Append("CavCode = ?CavCode, ");
            sqlCommand.Append("TransactionId = ?TransactionId, ");
            sqlCommand.Append("TransactionType = ?TransactionType, ");
            sqlCommand.Append("Method = ?Method, ");
            sqlCommand.Append("AuthCode = ?AuthCode, ");
            sqlCommand.Append("Amount = ?Amount, ");
            sqlCommand.Append("Tax = ?Tax, ");
            sqlCommand.Append("Duty = ?Duty, ");
            sqlCommand.Append("Freight = ?Freight ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[20];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = storeGuid.ToString();

            arParams[4] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cartGuid.ToString();

            arParams[5] = new MySqlParameter("?RawResponse", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rawResponse;

            arParams[6] = new MySqlParameter("?ResponseCode", MySqlDbType.VarChar, 1);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = responseCode;

            arParams[7] = new MySqlParameter("?ResponseReasonCode", MySqlDbType.VarChar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = responseReasonCode;

            arParams[8] = new MySqlParameter("?Reason", MySqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = reason;

            arParams[9] = new MySqlParameter("?AvsCode", MySqlDbType.VarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = avsCode;

            arParams[10] = new MySqlParameter("?CcvCode", MySqlDbType.VarChar, 1);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ccvCode;

            arParams[11] = new MySqlParameter("?CavCode", MySqlDbType.VarChar, 1);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = cavCode;

            arParams[12] = new MySqlParameter("?TransactionId", MySqlDbType.VarChar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = transactionId;

            arParams[13] = new MySqlParameter("?TransactionType", MySqlDbType.VarChar, 50);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = transactionType;

            arParams[14] = new MySqlParameter("?Method", MySqlDbType.VarChar, 20);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = method;

            arParams[15] = new MySqlParameter("?AuthCode", MySqlDbType.VarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = authCode;

            arParams[16] = new MySqlParameter("?Amount", MySqlDbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = amount;

            arParams[17] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = tax;

            arParams[18] = new MySqlParameter("?Duty", MySqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = duty;

            arParams[19] = new MySqlParameter("?Freight", MySqlDbType.Decimal);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = freight;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_AuthorizeNetLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_AuthorizeNetLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_AuthorizeNetLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_AuthorizeNetLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = ?RowGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_AuthorizeNetLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_AuthorizeNetLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("CartGuid = ?CartGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = cartGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_AuthorizeNetLog table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_AuthorizeNetLog ");
            sqlCommand.Append(";");

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets a count of rows in the mp_AuthorizeNetLog table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_AuthorizeNetLog ");
            sqlCommand.Append(";");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null));
        }

        /// <summary>
        /// Gets a page of data from the mp_AuthorizeNetLog table.
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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_AuthorizeNetLog  ");
            //sqlCommand.Append("ORDER BY  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?Offset, ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Offset", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageLowerBound;

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
