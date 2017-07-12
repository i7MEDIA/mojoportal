// Author:					
// Created:					2009-10-11
// Last Modified:			2009-10-29
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

namespace mojoPortal.Data
{
    public static class DBLetterSubscription
    {
        

        /// <summary>
        /// Inserts a row in the mp_LetterSubscribe table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="email"> email </param>
        /// <param name="isVerified"> isVerified </param>
        /// <param name="verifyGuid"> verifyGuid </param>
        /// <param name="beginUtc"> beginUtc </param>
        /// <param name="useHtml"> useHtml </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid letterInfoGuid,
            Guid userGuid,
            string email,
            bool isVerified,
            Guid verifyGuid,
            DateTime beginUtc,
            bool useHtml,
            string ipAddress)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_Insert", 10);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@IsVerified", SqlDbType.Bit, ParameterDirection.Input, isVerified);
            sph.DefineSqlParameter("@VerifyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, verifyGuid);
            sph.DefineSqlParameter("@BeginUtc", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@UseHtml", SqlDbType.Bit, ParameterDirection.Input, useHtml);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, ipAddress);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_LetterSubscribe table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="useHtml"> useHtml </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            bool useHtml)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_Update", 3);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@UseHtml", SqlDbType.Bit, ParameterDirection.Input, useHtml);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static bool Exists(Guid letterInfoGuid, string email)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_Exists", 2);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            int count =  Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);

        }

        public static bool Verify(
            Guid guid,
            bool isVerified,
            Guid verifyGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_Verify", 3);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsVerified", SqlDbType.Bit, ParameterDirection.Input, isVerified);
            sph.DefineSqlParameter("@VerifyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, verifyGuid);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_DeleteByLetter", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_DeleteUnverified", 2);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@OlderThanUtc", SqlDbType.DateTime, ParameterDirection.Input, olderThanUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribe_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByLetter(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectByLetter", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectByUser", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByEmail(Guid siteGuid, string email)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectByEmail", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            return sph.ExecuteReader();

        }

        public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_Search", 2);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@EmailOrIpAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailOrIpAddress);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectByLetterAndEmail", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            return sph.ExecuteReader();

        }

        public static IDataReader GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfoSelectUsersNotSubscribed", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@ExcludeIfAnyUnsubscribeHx", SqlDbType.Bit, ParameterDirection.Input, excludeIfAnyUnsubscribeHx);
            return sph.ExecuteReader();

        }

        public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfoCountUsersNotSubscribed", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@ExcludeIfAnyUnsubscribeHx", SqlDbType.Bit, ParameterDirection.Input, excludeIfAnyUnsubscribeHx);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets an IDataReader from the mp_LetterSubscriber table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectUnsentByLetter", 2);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);

            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static int GetCountByLetter(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_CountByLetter", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a page of data from the mp_LetterSubscriber table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountByLetter(letterInfoGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSubscribe_SelectPage", 3);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        /// <summary>
        /// Inserts a row in the mp_LetterSubscribeHx table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="subscribeGuid"> subscribeGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="email"> email </param>
        /// <param name="isVerified"> isVerified </param>
        /// <param name="useHtml"> useHtml </param>
        /// <param name="beginUtc"> beginUtc </param>
        /// <param name="endUtc"> endUtc </param>
        /// <returns>int</returns>
        public static int CreateHistory(
            Guid rowGuid,
            Guid siteGuid,
            Guid subscribeGuid,
            Guid letterInfoGuid,
            Guid userGuid,
            string email,
            bool isVerified,
            bool useHtml,
            DateTime beginUtc,
            DateTime endUtc,
            string ipAddress)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribeHx_Insert", 11);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SubscribeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subscribeGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@IsVerified", SqlDbType.Bit, ParameterDirection.Input, isVerified);
            sph.DefineSqlParameter("@UseHtml", SqlDbType.Bit, ParameterDirection.Input, useHtml);
            sph.DefineSqlParameter("@BeginUtc", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            sph.DefineSqlParameter("@EndUtc", SqlDbType.DateTime, ParameterDirection.Input, endUtc);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, ipAddress);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        public static bool DeleteHistoryBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribeHx_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSubscribeHx_DeleteByLetterInfo", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

    }
}
