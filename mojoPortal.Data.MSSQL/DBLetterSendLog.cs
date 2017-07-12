/// Author:					
/// Created:				2007-12-27
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
   
    public static class DBLetterSendLog
    {
        
        

        /// <summary>
        /// Inserts a row in the mp_LetterSendLog table. Returns new integer id.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterGuid,
            Guid userGuid,
            Guid subscribeGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_Insert", 7);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@SubscribeGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, subscribeGuid);
            sph.DefineSqlParameter("@EmailAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailAddress);
            sph.DefineSqlParameter("@UTC", SqlDbType.DateTime, ParameterDirection.Input, uTC);
            sph.DefineSqlParameter("@ErrorOccurred", SqlDbType.Bit, ParameterDirection.Input, errorOccurred);
            sph.DefineSqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, errorMessage);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the mp_LetterSendLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            Guid letterGuid,
            Guid userGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_Update", 7);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@EmailAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailAddress);
            sph.DefineSqlParameter("@UTC", SqlDbType.DateTime, ParameterDirection.Input, uTC);
            sph.DefineSqlParameter("@ErrorOccurred", SqlDbType.Bit, ParameterDirection.Input, errorOccurred);
            sph.DefineSqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1, ParameterDirection.Input, errorMessage);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_LetterSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_Delete", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_LetterSendLog table for the letterGuid. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_DeleteByLetter", 1);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_DeleteByLetterInfo", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterSendLog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSendLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSendLog table.
        /// </summary>
        public static int GetCount(Guid letterGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSendLog_GetCount", 1);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_LetterSendLog table.
        /// </summary>
        public static IDataReader GetByLetter(Guid letterGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSendLog_SelectByLetter", 1);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            return sph.ExecuteReader();



        }

        /// <summary>
        /// Gets a page of data from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid letterGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(letterGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterSendLog_SelectPage", 3);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }



    }
}
