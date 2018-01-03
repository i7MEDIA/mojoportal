/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2008-08-06
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
   
    public static class DBModuleSettings
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

        

        

        public static bool DeleteModuleSettings(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetModuleSettings(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT DISTINCT  ");
            sqlCommand.Append("ms.ID, ");
            sqlCommand.Append("ms.ModuleID, ");
            sqlCommand.Append("ms.SettingName, ");
            sqlCommand.Append("ms.SettingValue, ");
            sqlCommand.Append("mds.ModuleDefID, ");
            sqlCommand.Append("mds.FeatureGuid, ");
            sqlCommand.Append("mds.ControlType, ");
            sqlCommand.Append("mds.RegexValidationExpression, ");
            sqlCommand.Append("mds.SortOrder, ");
            sqlCommand.Append("mds.ControlSrc, ");
            sqlCommand.Append("mds.HelpKey, ");
            sqlCommand.Append("mds.GroupName, ");
            sqlCommand.Append("mds.Attributes, ");
            sqlCommand.Append("mds.Options, ");
			sqlCommand.Append("mds.ResourceFile ");

            sqlCommand.Append("FROM	mp_ModuleSettings ms ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ms.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_ModuleDefinitionSettings mds ");
            sqlCommand.Append("ON m.ModuleDefID = mds.ModuleDefID ");
            sqlCommand.Append("AND mds.SettingName = ms.SettingName ");

            sqlCommand.Append("WHERE ms.ModuleID = :ModuleID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("mds.SortOrder, mds.GroupName ");
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        public static bool CreateModuleSetting(
            Guid settingGuid,
            Guid moduleGuid,
            int moduleId,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ModuleSettings (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("SettingName, ");
            sqlCommand.Append("SettingValue, ");
            sqlCommand.Append("ControlType, ");
            sqlCommand.Append("ControlSrc, ");
            sqlCommand.Append("HelpKey, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("RegexValidationExpression, ");
            sqlCommand.Append("SettingGuid, ");
            sqlCommand.Append("ModuleGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":SettingName, ");
            sqlCommand.Append(":SettingValue, ");
            sqlCommand.Append(":ControlType, ");
            sqlCommand.Append(":ControlSrc, ");
            sqlCommand.Append(":HelpKey, ");
            sqlCommand.Append(":SortOrder, ");
            sqlCommand.Append(":RegexValidationExpression, ");
            sqlCommand.Append(":SettingGuid, ");
            sqlCommand.Append(":ModuleGuid )");
            sqlCommand.Append(";");


            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqliteParameter(":SettingValue", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new SqliteParameter(":ControlType", DbType.String, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new SqliteParameter(":RegexValidationExpression", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new SqliteParameter(":SettingGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = settingGuid.ToString();

            arParams[6] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = moduleGuid.ToString();

            arParams[7] = new SqliteParameter(":ControlSrc", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqliteParameter(":HelpKey", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = helpKey;

            arParams[9] = new SqliteParameter(":SortOrder", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = sortOrder;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateModuleSetting(
            Guid moduleGuid,
            int moduleId,
            string settingName,
            string settingValue)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) ");
            sqlCommand.Append("FROM	mp_ModuleSettings ");

            sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
            sqlCommand.Append("AND SettingName = :SettingName  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;



            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();

            int rowsAffected = 0;

            if (count > 0)
            {
                sqlCommand.Append("UPDATE mp_ModuleSettings ");
                sqlCommand.Append("SET SettingValue = :SettingValue  ");

                sqlCommand.Append("WHERE ModuleID = :ModuleID  ");
                sqlCommand.Append("AND SettingName = :SettingName  ");

                arParams = new SqliteParameter[3];

                arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleId;

                arParams[1] = new SqliteParameter(":SettingName", DbType.String, 50);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = settingName;

                arParams[2] = new SqliteParameter(":SettingValue", DbType.Object);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingValue;

                rowsAffected = SqliteHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

                return (rowsAffected > 0);

            }
            else
            {
                
                return false;

                //return CreateModuleSetting(
                //    Guid.NewGuid(),
                //    moduleGuid,
                //    moduleId,
                //    settingName,
                //    settingValue,
                //    "TextBox",
                //    string.Empty);

                

            }

        }



        public static IDataReader GetDefaultModuleSettings(
            int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE ModuleDefID = :ModuleDefID  ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SortOrder, GroupName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleDefID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static DataTable GetDefaultModuleSettingsForModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("m.ModuleID,  ");
            sqlCommand.Append("m.Guid AS ModuleGuid,  ");
            sqlCommand.Append("ds.SettingName, ");
            sqlCommand.Append("ds.SettingValue, ");
            sqlCommand.Append("ds.ControlType, ");
            sqlCommand.Append("ds.ControlSrc, ");
            sqlCommand.Append("ds.HelpKey, ");
            sqlCommand.Append("ds.SortOrder, ");
            sqlCommand.Append("ds.GroupName, ");
            sqlCommand.Append("ds.RegexValidationExpression ");

            sqlCommand.Append("FROM	mp_Modules m ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitionSettings ds ");
            sqlCommand.Append("ON ds.ModuleDefID = m.ModuleDefID ");
            sqlCommand.Append("WHERE m.ModuleID = :ModuleID ");

            sqlCommand.Append("ORDER BY	ds.SortOrder, ds.GroupName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("ModuleGuid", typeof(string));
            dt.Columns.Add("SettingName", typeof(string));
            dt.Columns.Add("SettingValue", typeof(string));
            dt.Columns.Add("ControlType", typeof(string));
            dt.Columns.Add("ControlSrc", typeof(string));
            dt.Columns.Add("HelpKey", typeof(string));
            dt.Columns.Add("GroupName", typeof(string));
            dt.Columns.Add("SortOrder", typeof(int));
            dt.Columns.Add("RegexValidationExpression", typeof(string));

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleGuid"] = reader["ModuleGuid"];
                    row["SettingName"] = reader["SettingName"];
                    row["SettingValue"] = reader["SettingValue"];
                    row["GroupName"] = reader["GroupName"];
                    row["ControlType"] = reader["ControlType"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    row["HelpKey"] = reader["HelpKey"];
                    row["SortOrder"] = reader["SortOrder"];
                    row["RegexValidationExpression"] = reader["RegexValidationExpression"];

                    dt.Rows.Add(row);

                }

            }


            return dt;
            //return DBPortal.GetTableFromDataReader(reader);




        }




        public static bool CreateDefaultModuleSettings(int moduleId)
        {
            DataTable dataTable = GetDefaultModuleSettingsForModule(moduleId);

            foreach (DataRow row in dataTable.Rows)
            {
                int sortOrder = 100;
                if (row["SortOrder"] != DBNull.Value)
                    sortOrder = Convert.ToInt32(row["SortOrder"]);

                CreateModuleSetting(
                    Guid.NewGuid(),
                    new Guid(row["ModuleGuid"].ToString()),
                    moduleId,
                    row["SettingName"].ToString(),
                    row["SettingValue"].ToString(),
                    row["ControlType"].ToString(),
                    row["RegexValidationExpression"].ToString(),
                    row["ControlSrc"].ToString(),
                    row["HelpKey"].ToString(),
                    sortOrder);

            }

            return (dataTable.Rows.Count > 0);

         
        }


        //public static bool CreateDefaultModuleSettings(int moduleId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO mp_ModuleSettings( ");
        //    sqlCommand.Append("ModuleID, ");
        //    sqlCommand.Append("SettingName, ");
        //    sqlCommand.Append("SettingValue, ");
        //    sqlCommand.Append("ControlType, ");
        //    sqlCommand.Append("RegexValidationExpression ");

        //    sqlCommand.Append(") ");

        //    sqlCommand.Append("SELECT m.ModuleID, ");
        //    sqlCommand.Append("ds.SettingName, ");
        //    sqlCommand.Append("ds.SettingValue, ");
        //    sqlCommand.Append("ds.ControlType, ");
        //    sqlCommand.Append("ds.RegexValidationExpression ");

        //    sqlCommand.Append("FROM	mp_Modules m ");
        //    sqlCommand.Append("JOIN	mp_ModuleDefinitionSettings ds ");
        //    sqlCommand.Append("ON ds.ModuleDefID = m.ModuleDefID ");
        //    sqlCommand.Append("WHERE m.ModuleID = :ModuleID ");

        //    sqlCommand.Append("ORDER BY	ds.ID ;");


        //    SqliteParameter[] arParams = new SqliteParameter[1];

        //    arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = moduleId;

        //    int count = SqliteHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (count > 0);


        //}


        

    }
}
