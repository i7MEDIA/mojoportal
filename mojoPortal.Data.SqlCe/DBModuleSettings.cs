// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2010-08-06
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
    public static class DBModuleSettings
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        public static IDataReader GetModuleSettings(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
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
            sqlCommand.Append("mds.ResourceFile ");

            sqlCommand.Append("FROM	mp_ModuleSettings ms ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ms.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN mp_ModuleDefinitionSettings mds ");
            sqlCommand.Append("ON m.ModuleDefID = mds.ModuleDefID ");
            sqlCommand.Append("AND mds.SettingName = ms.SettingName ");

            sqlCommand.Append("WHERE ms.ModuleID = @ModuleID  ");
            sqlCommand.Append("ORDER BY mds.SortOrder, mds.GroupName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetDefaultModuleSettings(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE ModuleDefID = @ModuleDefID ");
            sqlCommand.Append("ORDER BY SortOrder, GroupName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("INSERT INTO mp_ModuleSettings ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("SettingName, ");
            sqlCommand.Append("SettingValue, ");
            sqlCommand.Append("ControlType, ");
            sqlCommand.Append("RegexValidationExpression, ");
            sqlCommand.Append("SettingGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ControlSrc, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("HelpKey ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@SettingName, ");
            sqlCommand.Append("@SettingValue, ");
            sqlCommand.Append("@ControlType, ");
            sqlCommand.Append("@RegexValidationExpression, ");
            sqlCommand.Append("@SettingGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@ControlSrc, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@HelpKey ");
            sqlCommand.Append(")");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqlCeParameter("@SettingValue", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new SqlCeParameter("@ControlType", SqlDbType.NVarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new SqlCeParameter("@RegexValidationExpression", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new SqlCeParameter("@SettingGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = settingGuid;

            arParams[6] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = moduleGuid;

            arParams[7] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new SqlCeParameter("@HelpKey", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;


            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        private static int GetCount(int moduleId, string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ModuleSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SettingName = @SettingName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static bool UpdateModuleSetting(
            Guid moduleGuid,
            int moduleId,
            string settingName,
            string settingValue)
        {
            int existingCount = GetCount(moduleId, settingName);
            if (existingCount == 0)
            {
                return CreateModuleSetting(
                    Guid.NewGuid(),
                    moduleGuid,
                    moduleId,
                    settingName,
                    settingValue,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    100);
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ModuleSettings ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("SettingValue = @SettingValue ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND SettingName = @SettingName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqlCeParameter("@SettingValue", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static DataTable GetDefaultModuleSettingsForModule(int moduleId)
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
            sqlCommand.Append("WHERE m.ModuleID = @ModuleID ");

            sqlCommand.Append("ORDER BY	ds.SortOrder, ds.GroupName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            

            return DBPortal.GetTableFromDataReader(reader);



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

        public static bool DeleteModuleSettings(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

    }
}
