/// Author:					
/// Created:				2008-09-12
/// Last Modified:			2009-09-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

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
    
    public static class DBSiteSettingsEx
    {
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

        public static IDataReader GetSiteSettingsExList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  e.* ");

            sqlCommand.Append("FROM	mp_SiteSettingsEx e ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_SiteSettingsExDef d ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.KeyName = d.KeyName ");
            sqlCommand.Append("AND e.GroupName = d.GroupName ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.SiteID = :SiteID ");

            sqlCommand.Append("ORDER BY d.GroupName, d.SortOrder ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteSettingsEx");
            sqlCommand.Append("( ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("KeyName, ");
            sqlCommand.Append("KeyValue, ");
            sqlCommand.Append("GroupName ");
            sqlCommand.Append(")");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("t.SiteID, ");
            sqlCommand.Append("t.SiteGuid, ");
            sqlCommand.Append("t.KeyName, ");
            sqlCommand.Append("t.DefaultValue, ");
            sqlCommand.Append("t.GroupName  ");

            sqlCommand.Append("FROM ");

            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SiteID As SiteID, ");
            sqlCommand.Append("s.SiteGuid As SiteGuid, ");
            sqlCommand.Append("d.KeyName As KeyName, ");
            sqlCommand.Append("d.DefaultValue As DefaultValue, ");
            sqlCommand.Append("d.GroupName As GroupName ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Sites s, ");
            sqlCommand.Append("mp_SiteSettingsExDef d ");
            sqlCommand.Append(") t ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_SiteSettingsEx e ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.SiteID = t.SiteID ");
            sqlCommand.Append("AND e.KeyName = t.KeyName ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.SiteID IS NULL ");
            sqlCommand.Append("; ");

            SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static bool SaveExpandoProperty(
           int siteId,
           Guid siteGuid,
           string groupName,
           string keyName,
           string keyValue)
        {
            int count = GetCount(siteId, keyName);
            if (count > 0)
            {
                return Update(siteId, keyName, keyValue);

            }
            else
            {
                return Create(siteId, siteGuid, keyName, keyValue, groupName);

            }

        }

        public static bool UpdateRelatedSitesProperty(
            int siteId,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = :KeyValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID <> :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = keyName;

            arParams[2] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = keyValue;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



        private static bool Create(
            int siteId,
            Guid siteGuid,
            string keyName,
            string keyValue,
            string groupName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteSettingsEx (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("KeyName, ");
            sqlCommand.Append("KeyValue, ");
            sqlCommand.Append("GroupName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":SiteId, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":KeyName, ");
            sqlCommand.Append(":KeyValue, ");
            sqlCommand.Append(":GroupName )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":SiteId", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":KeyName", DbType.String, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = keyName;

            arParams[3] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = keyValue;

            arParams[4] = new SqliteParameter(":GroupName", DbType.String, 128);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = groupName;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        private static bool Update(
            int siteID,
            string keyName,
            string keyValue)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteSettingsEx ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("KeyValue = :KeyValue ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteID;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = keyName;

            arParams[2] = new SqliteParameter(":KeyValue", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = keyValue;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }


        private static int GetCount(
            int siteID,
            string keyName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SiteSettingsEx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = :SiteID AND ");
            sqlCommand.Append("KeyName = :KeyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteID;

            arParams[1] = new SqliteParameter(":KeyName", DbType.String, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = keyName;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetDefaultExpandoSettings()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteSettingsExDef ");
            sqlCommand.Append(";");

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);
        }

    }
}
