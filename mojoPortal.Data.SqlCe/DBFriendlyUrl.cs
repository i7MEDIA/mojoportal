// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2010-04-04
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
    public static class DBFriendlyUrl
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_FriendlyUrls ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("FriendlyUrl, ");
            sqlCommand.Append("RealUrl, ");
            sqlCommand.Append("IsPattern, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ItemGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@FriendlyUrl, ");
            sqlCommand.Append("@RealUrl, ");
            sqlCommand.Append("@IsPattern, ");
            sqlCommand.Append("@PageGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ItemGuid ");
            sqlCommand.Append(")");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@FriendlyUrl", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            arParams[2] = new SqlCeParameter("@RealUrl", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = realUrl;

            arParams[3] = new SqlCeParameter("@IsPattern", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isPattern;

            arParams[4] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageGuid;

            arParams[5] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = siteGuid;

            arParams[6] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = itemGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


        }

        public static bool UpdateFriendlyUrl(
            int urlId,
            int siteId,
            Guid pageGuid,
            string friendlyUrl,
            string realUrl,
            bool isPattern)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_FriendlyUrls ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteID = @SiteID, ");
            sqlCommand.Append("FriendlyUrl = @FriendlyUrl, ");
            sqlCommand.Append("RealUrl = @RealUrl, ");
            sqlCommand.Append("IsPattern = @IsPattern, ");
            sqlCommand.Append("PageGuid = @PageGuid ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UrlID = @UrlID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@UrlID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new SqlCeParameter("@FriendlyUrl", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyUrl;

            arParams[3] = new SqlCeParameter("@RealUrl", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = realUrl;

            arParams[4] = new SqlCeParameter("@IsPattern", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isPattern;

            arParams[5] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }


        public static bool DeleteFriendlyUrl(int urlId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UrlID = @UrlID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UrlID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetFriendlyUrl(int urlId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UrlID = @UrlID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UrlID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetFriendlyUrl(int siteId, String friendlyUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl = @FriendlyUrl ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@FriendlyUrl", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByUrl(string hostName, string friendlyUrl)
        {
            int siteId = GetSiteIdByHost(hostName);
            
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1)  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FriendlyUrl = @FriendlyUrl ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SiteID = @SiteID ");
           
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@FriendlyUrl", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        private static int GetSiteIdByHost(string hostName)
        {
            if (string.IsNullOrEmpty(hostName)) { return GetDefaultSiteId(); }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT TOP(1) SiteID From mp_SiteHosts WHERE HostName = @HostName");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@HostName", SqlDbType.NVarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = hostName;

            object o = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (o != null)
            {
                return Convert.ToInt32(o);
            }
            return GetDefaultSiteId();

        }

        private static int GetDefaultSiteId()
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT TOP(1) SiteID From mp_Sites ORDER BY SiteID");
            sqlCommand.Append(";");

            object o = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            if (o != null)
            {
                return Convert.ToInt32(o);
            }
            return -1;

        }

        public static bool Exists(
            int siteId,
            string friendlyUrl)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl = @FriendlyUrl ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@FriendlyUrl", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static int GetCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static int GetCount(int siteId, string searchTerm)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl LIKE '%' + @SearchTerm + '%' ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchTerm", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = searchTerm;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteId);

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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_FriendlyUrls  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("FriendlyUrl  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.FriendlyUrl DESC  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.FriendlyUrl  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetPage(
            int siteId,
            string searchTerm,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_FriendlyUrls  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl LIKE '%' + @SearchTerm + '%' ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("FriendlyUrl  ");

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t1.FriendlyUrl DESC  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("t2.FriendlyUrl  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@SearchTerm", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = searchTerm;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
