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
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
namespace mojoPortal.Data
{
    public static class DBPaymentLog
    {
        

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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PaymentLog_Insert", 22);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@Provider", SqlDbType.NVarChar, 100, ParameterDirection.Input, provider);
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PaymentLog_Update", 20);
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
        /// Deletes a row from the mp_PaymentLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PaymentLog_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_PaymentLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PaymentLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_PaymentLog table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetByCart(Guid cartGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PaymentLog_SelectByCart", 1);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            return sph.ExecuteReader();

        }

    }
}
