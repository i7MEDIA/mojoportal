/// Author:					
/// Created:				2008-10-06
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
using mojoPortal.Data;

namespace mojoPortal.Data
{
    public static class DBContentRating
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
        /// Inserts a row in the mp_ContentRating table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="rating"> rating </param>
        /// <param name="comments"> comments </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid contentGuid,
            Guid userGuid,
            string emailAddress,
            int rating,
            string comments,
            string ipAddress,
            DateTime createdUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_Insert", 10);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@EmailAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailAddress);
            sph.DefineSqlParameter("@Rating", SqlDbType.Int, ParameterDirection.Input, rating);
            sph.DefineSqlParameter("@Comments", SqlDbType.NVarChar, -1, ParameterDirection.Input, comments);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_ContentRating table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="rating"> rating </param>
        /// <param name="comments"> comments </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            string emailAddress,
            int rating,
            string comments,
            string ipAddress,
            DateTime lastModUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_Update", 6);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@EmailAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailAddress);
            sph.DefineSqlParameter("@Rating", SqlDbType.Int, ParameterDirection.Input, rating);
            sph.DefineSqlParameter("@Comments", SqlDbType.NVarChar, -1, ParameterDirection.Input, comments);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_Delete", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_DeleteByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByUser(Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ContentRating_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_SelectOne", 1);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        public static IDataReader GetOneByContentAndUser(Guid contentGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_SelectOneByContentAndUser", 2);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        public static IDataReader GetStatsByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_GetStatsByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            return sph.ExecuteReader();

        }

        

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_GetCountByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountByContentAndUser(Guid contentGuid, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_GetCountByContentAndUser", 2);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountOfRatingsSince(Guid contentGuid, string ipAddress, DateTime beginUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_GetCountByContentAndIPAddress", 3);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 50, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@BeginUtc", SqlDbType.DateTime, ParameterDirection.Input, beginUtc);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        

        /// <summary>
        /// Gets a page of data from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid">contentGuid</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid contentGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountByContent(contentGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ContentRating_SelectPageByContent", 3);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}


