// Author:					Joe Audette
// Created:					2011-10-29
// Last Modified:			2011-10-29
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
using mojoPortal.Data; // add a project reference to mojoPortal.Data.SQLite to get this

namespace mojoPortal.Data
{
    public static class DBTagItem
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
        /// Inserts a row in the mp_TagItem table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="tagGuid"> tagGuid </param>
        /// <param name="extraGuid"> extraGuid </param>
        /// <param name="taggedBy"> taggedBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid tagGuid,
            Guid extraGuid,
            Guid taggedBy)
        {
        
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TagItem (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("TagGuid, ");
            sqlCommand.Append("ExtraGuid, ");
            sqlCommand.Append("TaggedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":FeatureGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":TagGuid, ");
            sqlCommand.Append(":ExtraGuid, ");
            sqlCommand.Append(":TaggedBy )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[8];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid.ToString();

            arParams[3] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = itemGuid.ToString();

            arParams[5] = new SqliteParameter(":TagGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tagGuid.ToString();

            arParams[6] = new SqliteParameter(":ExtraGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = extraGuid.ToString();

            arParams[7] = new SqliteParameter(":TaggedBy", DbType.String, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = taggedBy.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }
 

        /// <summary>
        /// Deletes a row from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
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
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = :ItemGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByExtraGuid(Guid extraGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ExtraGuid = :ExtraGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ExtraGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = extraGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByTag(Guid tagGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("TagGuid = :TagGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":TagGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = tagGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }


        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
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
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = :FeatureGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FeatureGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TagItem ");
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

    }
}
