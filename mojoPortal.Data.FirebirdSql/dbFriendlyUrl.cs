/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2010-02-16
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBFriendlyUrl
    {
       
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
            #region Bit Conversion
            int intIsPattern;
            if (isPattern)
            {
                intIsPattern = 1;
            }
            else
            {
                intIsPattern = 0;
            }


            #endregion

            FbParameter[] arParams = new FbParameter[7];

            arParams[0] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter(":FriendlyUrl", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            arParams[2] = new FbParameter(":RealUrl", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = realUrl;

            arParams[3] = new FbParameter(":IsPattern", FbDbType.SmallInt);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intIsPattern;

            arParams[4] = new FbParameter(":PageGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageGuid.ToString();

            arParams[5] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = siteGuid.ToString();

            arParams[6] = new FbParameter(":ItemGuid", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = itemGuid.ToString();

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_FRIENDLYURLS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

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
            int intIsPattern;
            if (isPattern)
            {
                intIsPattern = 1;
            }
            else
            {
                intIsPattern = 0;
            }


            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_FriendlyUrls ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteID = @SiteID, ");
            sqlCommand.Append("FriendlyUrl = @FriendlyUrl, ");
            sqlCommand.Append("RealUrl = @RealUrl, ");
            sqlCommand.Append("IsPattern = @IsPattern, ");
            sqlCommand.Append("PageGuid = @PageGuid ");
           

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UrlID = @UrlID ;");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@UrlID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            arParams[1] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new FbParameter("@FriendlyUrl", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyUrl;

            arParams[3] = new FbParameter("@RealUrl", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = realUrl;

            arParams[4] = new FbParameter("@IsPattern", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsPattern;

            arParams[5] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteFriendlyUrl(int urlId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UrlID = @UrlID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UrlID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByPageId(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RealUrl LIKE '%pageid=" + pageId.ToString() + "' ;");


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), null);

            return (rowsAffected > 0);

        }

        public static bool DeleteByPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetFriendlyUrl(int urlId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UrlID = @UrlID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UrlID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static DataTable GetByHostName(string hostName)
        {

            DataTable dt = new DataTable();
            int siteId = 1;

            dt.Columns.Add("UrlID", typeof(int));
            dt.Columns.Add("FriendlyUrl", typeof(string));
            dt.Columns.Add("RealUrl", typeof(string));
            dt.Columns.Add("IsPattern", typeof(bool));


            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT mp_SiteHosts.SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE mp_SiteHosts.HostName = @HostName ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ;");

            arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["UrlID"] = reader["UrlID"];
                    row["FriendlyUrl"] = reader["FriendlyUrl"];
                    row["RealUrl"] = reader["RealUrl"];
                    row["IsPattern"] = reader["IsPattern"];
                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static DataTable GetBySite(int siteId)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("UrlID", typeof(int));
            dt.Columns.Add("FriendlyUrl", typeof(string));
            dt.Columns.Add("RealUrl", typeof(string));
            dt.Columns.Add("IsPattern", typeof(bool));

            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[1];

            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ORDER BY FriendlyUrl ;");

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["UrlID"] = reader["UrlID"];
                    row["FriendlyUrl"] = reader["FriendlyUrl"];
                    row["RealUrl"] = reader["RealUrl"];
                    row["IsPattern"] = reader["IsPattern"];
                    dt.Rows.Add(row);
                }

            }

            return dt;

        }

        public static IDataReader GetByUrl(string hostName, string friendlyUrl)
        {
            int siteId = 1;

            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@HostName", FbDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = hostName;

            sqlCommand.Append("SELECT SiteID ");
            sqlCommand.Append("FROM mp_SiteHosts ");
            sqlCommand.Append("WHERE HostName = @HostName ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteId = Convert.ToInt32(reader["SiteID"]);
                }
            }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND FriendlyUrl = @FriendlyUrl ;");

            arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@FriendlyUrl", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetFriendlyUrl(int siteId, String friendlyUrl)
        {

            StringBuilder sqlCommand = new StringBuilder();
            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@FriendlyUrl", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append(" SiteID = @SiteID ");
            sqlCommand.Append("AND FriendlyUrl = @FriendlyUrl ;");

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets a count of rows in the mp_FriendlyUrls table.
        /// </summary>
        public static int GetCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the mp_FriendlyUrls table.
        /// </summary>
        public static int GetCount(int siteId, string searchTerm)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_FriendlyUrls ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl LIKE @SearchTerm ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchTerm", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchTerm + "%";

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_FriendlyUrls table.
        /// </summary>
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_FriendlyUrls  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");

            sqlCommand.Append("ORDER BY FriendlyUrl  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;


            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_FriendlyUrls  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FriendlyUrl LIKE @SearchTerm ");
            sqlCommand.Append("ORDER BY FriendlyUrl  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@SearchTerm", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchTerm + "%";


            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
        

    }
}
