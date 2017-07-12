/// Author:					
/// Created:				2008-06-22
/// Last Modified:			2010-07-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    public static class DBAuthorizeNetLog
    {
       
        ///// <summary>
        ///// Gets the connection string.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}


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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_AuthorizeNetLog_Insert", 21);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@ResponseCode", SqlDbType.NChar, 1, ParameterDirection.Input, responseCode);
            sph.DefineSqlParameter("@ResponseReasonCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, responseReasonCode);
            sph.DefineSqlParameter("@Reason", SqlDbType.NVarChar, -1, ParameterDirection.Input, reason);
            sph.DefineSqlParameter("@AvsCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, avsCode);
            sph.DefineSqlParameter("@CcvCode", SqlDbType.NChar, 1, ParameterDirection.Input, ccvCode);
            sph.DefineSqlParameter("@CavCode", SqlDbType.NChar, 1, ParameterDirection.Input, cavCode);
            sph.DefineSqlParameter("@TransactionId", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionId);
            sph.DefineSqlParameter("@TransactionType", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionType);
            sph.DefineSqlParameter("@Method", SqlDbType.NVarChar, 20, ParameterDirection.Input, method);
            sph.DefineSqlParameter("@AuthCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, authCode);
            sph.DefineSqlParameter("@Amount", SqlDbType.Decimal, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@Tax", SqlDbType.Decimal, ParameterDirection.Input, tax);
            sph.DefineSqlParameter("@Duty", SqlDbType.Decimal, ParameterDirection.Input, duty);
            sph.DefineSqlParameter("@Freight", SqlDbType.Decimal, ParameterDirection.Input, freight);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_AuthorizeNetLog_Update", 20);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@RawResponse", SqlDbType.NVarChar, -1, ParameterDirection.Input, rawResponse);
            sph.DefineSqlParameter("@ResponseCode", SqlDbType.NChar, 1, ParameterDirection.Input, responseCode);
            sph.DefineSqlParameter("@ResponseReasonCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, responseReasonCode);
            sph.DefineSqlParameter("@Reason", SqlDbType.NVarChar, -1, ParameterDirection.Input, reason);
            sph.DefineSqlParameter("@AvsCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, avsCode);
            sph.DefineSqlParameter("@CcvCode", SqlDbType.NChar, 1, ParameterDirection.Input, ccvCode);
            sph.DefineSqlParameter("@CavCode", SqlDbType.NChar, 1, ParameterDirection.Input, cavCode);
            sph.DefineSqlParameter("@TransactionId", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionId);
            sph.DefineSqlParameter("@TransactionType", SqlDbType.NVarChar, 50, ParameterDirection.Input, transactionType);
            sph.DefineSqlParameter("@Method", SqlDbType.NVarChar, 20, ParameterDirection.Input, method);
            sph.DefineSqlParameter("@AuthCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, authCode);
            sph.DefineSqlParameter("@Amount", SqlDbType.Decimal, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@Tax", SqlDbType.Decimal, ParameterDirection.Input, tax);
            sph.DefineSqlParameter("@Duty", SqlDbType.Decimal, ParameterDirection.Input, duty);
            sph.DefineSqlParameter("@Freight", SqlDbType.Decimal, ParameterDirection.Input, freight);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_AuthorizeNetLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_AuthorizeNetLog_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_AuthorizeNetLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_AuthorizeNetLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_AuthorizeNetLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_AuthorizeNetLog_SelectByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Gets a count of rows in the mp_AuthorizeNetLog table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_AuthorizeNetLog_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_AuthorizeNetLog table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_AuthorizeNetLog_SelectAll",
                null);

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
            totalPages = 1;
            int totalRows
                = GetCount();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_AuthorizeNetLog_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
