// Author:					
// Created:					2009-12-05
// Last Modified:			2009-12-05
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
    public static class DBContentMetaLink
    {
        public static String DBPlatform()
        {
            return "SQLite";
        }

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
        /// Inserts a row in the mp_ContentMetaLink table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="rel"> rel </param>
        /// <param name="href"> href </param>
        /// <param name="hrefLang"> hrefLang </param>
        /// <param name="rev"> rev </param>
        /// <param name="type"> type </param>
        /// <param name="media"> media </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid contentGuid,
            string rel,
            string href,
            string hrefLang,
            string rev,
            string type,
            string media,
            int sortRank,
            DateTime createdUtc,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentMetaLink (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ContentGuid, ");
            sqlCommand.Append("Rel, ");
            sqlCommand.Append("Href, ");
            sqlCommand.Append("HrefLang, ");
            sqlCommand.Append("Rev, ");
            sqlCommand.Append("Type, ");
            sqlCommand.Append("Media, ");
            sqlCommand.Append("SortRank, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":ContentGuid, ");
            sqlCommand.Append(":Rel, ");
            sqlCommand.Append(":Href, ");
            sqlCommand.Append(":HrefLang, ");
            sqlCommand.Append(":Rev, ");
            sqlCommand.Append(":Type, ");
            sqlCommand.Append(":Media, ");
            sqlCommand.Append(":SortRank, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":CreatedBy, ");
            sqlCommand.Append(":LastModUtc, ");
            sqlCommand.Append(":LastModBy )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[15];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = contentGuid.ToString();

            arParams[4] = new SqliteParameter(":Rel", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rel;

            arParams[5] = new SqliteParameter(":Href", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = href;

            arParams[6] = new SqliteParameter(":HrefLang", DbType.String, 10);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = hrefLang;

            arParams[7] = new SqliteParameter(":Rev", DbType.String, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rev;

            arParams[8] = new SqliteParameter(":Type", DbType.String, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = type;

            arParams[9] = new SqliteParameter(":Media", DbType.String, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = media;

            arParams[10] = new SqliteParameter(":SortRank", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = sortRank;

            arParams[11] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdUtc;

            arParams[12] = new SqliteParameter(":CreatedBy", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdBy.ToString();

            arParams[13] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdBy.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_ContentMetaLink table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="rel"> rel </param>
        /// <param name="href"> href </param>
        /// <param name="hrefLang"> hrefLang </param>
        /// <param name="rev"> rev </param>
        /// <param name="type"> type </param>
        /// <param name="media"> media </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string rel,
            string href,
            string hrefLang,
            string rev,
            string type,
            string media,
            int sortRank,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ContentMetaLink ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Rel = :Rel, ");
            sqlCommand.Append("Href = :Href, ");
            sqlCommand.Append("HrefLang = :HrefLang, ");
            sqlCommand.Append("Rev = :Rev, ");
            sqlCommand.Append("Type = :Type, ");
            sqlCommand.Append("Media = :Media, ");
            sqlCommand.Append("SortRank = :SortRank, ");
            sqlCommand.Append("LastModUtc = :LastModUtc, ");
            sqlCommand.Append("LastModBy = :LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":Rel", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = rel;

            arParams[2] = new SqliteParameter(":Href", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = href;

            arParams[3] = new SqliteParameter(":HrefLang", DbType.String, 10);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = hrefLang;

            arParams[4] = new SqliteParameter(":Rev", DbType.String, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rev;

            arParams[5] = new SqliteParameter(":Type", DbType.String, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = type;

            arParams[6] = new SqliteParameter(":Media", DbType.String, 50);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = media;

            arParams[7] = new SqliteParameter(":SortRank", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortRank;

            arParams[8] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new SqliteParameter(":LastModBy", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModBy.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentMetaLink table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="moduleGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="contentGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentMetaLink table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_ContentMetaLink table.
        /// </summary>
        /// <param name="contentGuid"> guid </param>
        public static IDataReader GetByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SortRank ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetMaxSortRank(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(SortRank),1)  ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }


    }
}
