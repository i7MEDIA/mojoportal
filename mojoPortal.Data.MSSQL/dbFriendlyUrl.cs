/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2010-06-17
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
    
    public static class DBFriendlyUrl
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
        /// Inserts a row in the mp_FriendlyUrls table. Returns new integer id.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="friendlyUrl"> friendlyUrl </param>
        /// <param name="realUrl"> realUrl </param>
        /// <param name="isPattern"> isPattern </param>
        /// <returns>int</returns>
        public static int AddFriendlyUrl(
            Guid itemGuid,
            Guid siteGuid,
            Guid pageGuid,
            int siteId,
            string friendlyUrl,
            string realUrl,
            bool isPattern)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_FriendlyUrls_Insert", 7);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FriendlyUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyUrl);
            sph.DefineSqlParameter("@RealUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, realUrl);
            sph.DefineSqlParameter("@IsPattern", SqlDbType.Bit, ParameterDirection.Input, isPattern);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateFriendlyUrl(
            int urlId,
            int siteId,
            Guid pageGuid,
            string friendlyUrl,
            string realUrl,
            bool isPattern)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_FriendlyUrls_Update", 6);
            sph.DefineSqlParameter("@UrlID", SqlDbType.Int, ParameterDirection.Input, urlId);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FriendlyUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyUrl);
            sph.DefineSqlParameter("@RealUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, realUrl);
            sph.DefineSqlParameter("@IsPattern", SqlDbType.Bit, ParameterDirection.Input, isPattern);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteFriendlyUrl(int urlId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_FriendlyUrls_Delete", 1);
            sph.DefineSqlParameter("@UrlID", SqlDbType.Int, ParameterDirection.Input, urlId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByPageId(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_FriendlyUrls_DeleteByPageID", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByPageGuid(Guid pageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_FriendlyUrls_DeleteByPageGuid", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetFriendlyUrl(int urlId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_SelectOne", 1);
            sph.DefineSqlParameter("@UrlID", SqlDbType.Int, ParameterDirection.Input, urlId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetFriendlyUrl(int siteId, String friendlyUrl)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_SelectBySiteUrl", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FriendlyUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyUrl);
            return sph.ExecuteReader();
        }


        //public static DataTable GetByHostName(string hostName)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_FriendlyUrls_SelectByHost", 1);
        //    sph.DefineSqlParameter("@HostName", SqlDbType.VarChar, 100, ParameterDirection.Input, hostName);

        //    DataTable dt = new DataTable();

        //    dt.Columns.Add("UrlID", typeof(int));
        //    dt.Columns.Add("FriendlyUrl", typeof(string));
        //    dt.Columns.Add("RealUrl", typeof(string));
        //    dt.Columns.Add("IsPattern", typeof(bool));


        //    using (IDataReader reader = sph.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dt.NewRow();
        //            row["UrlID"] = reader["UrlID"];
        //            row["FriendlyUrl"] = reader["FriendlyUrl"];
        //            row["RealUrl"] = reader["RealUrl"];
        //            row["IsPattern"] = reader["IsPattern"];
        //            dt.Rows.Add(row);

        //        }

        //    }

        //    return dt;

        //}

        //public static DataTable GetBySite(int siteId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_FriendlyUrls_SelectBySiteID", 1);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("UrlID", typeof(int));
        //    dt.Columns.Add("FriendlyUrl", typeof(string));
        //    dt.Columns.Add("RealUrl", typeof(string));
        //    dt.Columns.Add("IsPattern", typeof(bool));

        //    using (IDataReader reader = sph.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dt.NewRow();
        //            row["UrlID"] = reader["UrlID"];
        //            row["FriendlyUrl"] = reader["FriendlyUrl"];
        //            row["RealUrl"] = reader["RealUrl"];
        //            row["IsPattern"] = reader["IsPattern"];
        //            dt.Rows.Add(row);

        //        }

        //    }

        //    return dt;

        //}

        public static IDataReader GetByUrl(string hostName, string friendlyUrl)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_SelectOneByUrl", 2);
            sph.DefineSqlParameter("@HostName", SqlDbType.VarChar, 255, ParameterDirection.Input, hostName);
            sph.DefineSqlParameter("@FriendlyUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyUrl);
            return sph.ExecuteReader();
        }

        public static bool Exists(
            int siteId,
            string friendlyUrl)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_Exists", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FriendlyUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, friendlyUrl);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        /// <summary>
        /// Gets a count of rows in the mp_FriendlyUrls table.
        /// </summary>
        public static int GetCount(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_GetCount", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return Convert.ToInt32(sph.ExecuteScalar());

          
        }

        /// <summary>
        /// Gets a count of rows in the mp_FriendlyUrls table.
        /// </summary>
        public static int GetCount(int siteId, string searchTerm)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_GetSearchCount", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchTerm);
            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets a page of data from the mp_FriendlyUrls table.
        /// </summary>
        /// <param name="siteId">The siteId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(siteId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_SelectPage", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the mp_FriendlyUrls table.
        /// </summary>
        /// <param name="siteId">The siteId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int siteId,
            string searchTerm,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteId, searchTerm);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_FriendlyUrls_SelectSearchPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchTerm", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchTerm);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }
        

    }
}
