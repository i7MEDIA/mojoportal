/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2018-01-02
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
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
   
    public static class DBModuleSettings
    {
       
        
        public static IDataReader GetModuleSettings(int moduleId)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ms.id, ");
            sqlCommand.Append("ms.moduleid, ");
            sqlCommand.Append("ms.settingname, ");
            sqlCommand.Append("ms.settingvalue, ");
            sqlCommand.Append("mds.moduledefid, ");
            sqlCommand.Append("mds.featureguid, ");
            sqlCommand.Append("mds.controltype, ");
            sqlCommand.Append("mds.regexvalidationexpression, ");
            sqlCommand.Append("mds.sortorder, ");
            sqlCommand.Append("mds.controlsrc, ");
            sqlCommand.Append("mds.helpkey, ");
            sqlCommand.Append("mds.groupname, ");
            sqlCommand.Append("mds.options, ");
            sqlCommand.Append("mds.attributes, ");
            sqlCommand.Append("mds.resourcefile ");

            sqlCommand.Append("FROM	mp_modulesettings ms ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON ms.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN mp_moduledefinitionsettings mds ");
            sqlCommand.Append("ON m.moduledefid = mds.moduledefid ");
            sqlCommand.Append("AND mds.settingname = ms.settingname ");

            sqlCommand.Append("WHERE ms.moduleid = :moduleid  ");
            sqlCommand.Append("ORDER BY mds.sortorder, mds.groupname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            
        }




        public static IDataReader GetDefaultModuleSettings(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_moduledefinitionsettings ");

            sqlCommand.Append("WHERE moduledefid = :moduledefid  ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sortorder, groupname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("INSERT INTO mp_modulesettings (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("settingname, ");
            sqlCommand.Append("settingvalue, ");
            sqlCommand.Append("controltype, ");
            sqlCommand.Append("regexvalidationexpression, ");
            sqlCommand.Append("settingguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("controlsrc, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("helpkey )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":settingname, ");
            sqlCommand.Append(":settingvalue, ");
            sqlCommand.Append(":controltype, ");
            sqlCommand.Append(":regexvalidationexpression, ");
            sqlCommand.Append(":settingguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":controlsrc, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":helpkey )");
            sqlCommand.Append(";");
            //sqlCommand.Append(" SELECT CURRVAL('mp_modulesettingsid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[10];
            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter(":settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new NpgsqlParameter(":settingvalue", NpgsqlTypes.NpgsqlDbType.Varchar, -1);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new NpgsqlParameter(":controltype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new NpgsqlParameter(":regexvalidationexpression", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new NpgsqlParameter(":settingguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = settingGuid.ToString();

            arParams[6] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = moduleGuid.ToString();

            arParams[7] = new NpgsqlParameter(":controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new NpgsqlParameter(":sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new NpgsqlParameter(":helpkey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;


            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            return (rowsAffected > 0);

           

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
            sqlCommand.Append("UPDATE mp_modulesettings ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("settingvalue = :settingvalue ");
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("settingname = :settingname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter(":settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new NpgsqlParameter(":settingvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


           
        }

        private static int GetCount(int moduleId, string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_modulesettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("settingname = :settingname ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter(":settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        

        public static bool DeleteModuleSettings(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_modulesettings_delete(:moduleid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static DataTable GetDefaultModuleSettingsForModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("m.moduleid,  ");
            sqlCommand.Append("m.guid AS moduleguid,  ");
            sqlCommand.Append("ds.settingname, ");
            sqlCommand.Append("ds.settingvalue, ");
            sqlCommand.Append("ds.controltype, ");
            sqlCommand.Append("ds.controlsrc, ");
            sqlCommand.Append("ds.helpkey, ");
            sqlCommand.Append("ds.sortorder, ");
            sqlCommand.Append("ds.groupname, ");
            sqlCommand.Append("ds.regexvalidationexpression ");

            sqlCommand.Append("FROM	mp_modules m ");
            sqlCommand.Append("JOIN	mp_moduledefinitionSettings ds ");
            sqlCommand.Append("ON ds.moduledefid = m.moduledefid ");
            sqlCommand.Append("WHERE m.moduleid = :moduleid ");

            sqlCommand.Append("ORDER BY	ds.sortorder, ds.groupname ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

    }
}
