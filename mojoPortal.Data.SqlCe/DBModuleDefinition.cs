// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2014-07-29
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
    public static class DBModuleDefinition
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int AddModuleDefinition(
            Guid featureGuid,
            int siteId,
            string featureName,
            string controlSrc,
            int sortOrder,
            int defaultCacheTime,
            String icon,
            bool isAdmin,
            string resourceFile,
            bool isCacheable,
            bool isSearchable,
            string searchListName,
            bool supportsPageReuse,
            string deleteProvider,
            string partialView)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ModuleDefinitions ");
            sqlCommand.Append("(");
            sqlCommand.Append("FeatureName, ");
            sqlCommand.Append("ControlSrc, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("IsAdmin, ");
            sqlCommand.Append("Icon, ");
            sqlCommand.Append("DefaultCacheTime, ");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ResourceFile, ");
            sqlCommand.Append("IsCacheable, ");
            sqlCommand.Append("IsSearchable, ");
            sqlCommand.Append("SearchListName, ");
            sqlCommand.Append("SupportsPageReuse, ");
            sqlCommand.Append("PartialView, ");
            sqlCommand.Append("DeleteProvider ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@FeatureName, ");
            sqlCommand.Append("@ControlSrc, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@IsAdmin, ");
            sqlCommand.Append("@Icon, ");
            sqlCommand.Append("@DefaultCacheTime, ");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@ResourceFile, ");
            sqlCommand.Append("@IsCacheable, ");
            sqlCommand.Append("@IsSearchable, ");
            sqlCommand.Append("@SearchListName, ");
            sqlCommand.Append("@SupportsPageReuse, ");
            sqlCommand.Append("@PartialView, ");
            sqlCommand.Append("@DeleteProvider ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[14];

            arParams[0] = new SqlCeParameter("@FeatureName", SqlDbType.NVarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureName;

            arParams[1] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = controlSrc;

            arParams[2] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new SqlCeParameter("@IsAdmin", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isAdmin;

            arParams[4] = new SqlCeParameter("@Icon", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = icon;

            arParams[5] = new SqlCeParameter("@DefaultCacheTime", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = defaultCacheTime;

            arParams[6] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = featureGuid;

            arParams[7] = new SqlCeParameter("@ResourceFile", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new SqlCeParameter("@IsCacheable", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = isCacheable;

            arParams[9] = new SqlCeParameter("@IsSearchable", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isSearchable;

            arParams[10] = new SqlCeParameter("@SearchListName", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new SqlCeParameter("@SupportsPageReuse", SqlDbType.Bit);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = supportsPageReuse;

            arParams[12] = new SqlCeParameter("@DeleteProvider", SqlDbType.NVarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new SqlCeParameter("@PartialView", SqlDbType.NVarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            EnsureInstallationInAdminSites();

            return newId;

        }

        public static bool UpdateModuleDefinition(
            int moduleDefId,
            string featureName,
            string controlSrc,
            int sortOrder,
            int defaultCacheTime,
            string icon,
            bool isAdmin,
            string resourceFile,
            bool isCacheable,
            bool isSearchable,
            string searchListName,
            bool supportsPageReuse,
            string deleteProvider,
            string partialView)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ModuleDefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FeatureName = @FeatureName, ");
            sqlCommand.Append("ControlSrc = @ControlSrc, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("IsAdmin = @IsAdmin, ");
            sqlCommand.Append("Icon = @Icon, ");
            sqlCommand.Append("DefaultCacheTime = @DefaultCacheTime, ");
            
            sqlCommand.Append("ResourceFile = @ResourceFile, ");
            sqlCommand.Append("IsCacheable = @IsCacheable, ");
            sqlCommand.Append("IsSearchable = @IsSearchable, ");
            sqlCommand.Append("SearchListName = @SearchListName, ");
            sqlCommand.Append("SupportsPageReuse = @SupportsPageReuse, ");
            sqlCommand.Append("PartialView = @PartialView, ");
            
            sqlCommand.Append("DeleteProvider = @DeleteProvider ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[14];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new SqlCeParameter("@FeatureName", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureName;

            arParams[2] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = controlSrc;

            arParams[3] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new SqlCeParameter("@IsAdmin", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isAdmin;

            arParams[5] = new SqlCeParameter("@Icon", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = icon;

            arParams[6] = new SqlCeParameter("@DefaultCacheTime", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = defaultCacheTime;

            arParams[7] = new SqlCeParameter("@ResourceFile", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new SqlCeParameter("@IsCacheable", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = isCacheable;

            arParams[9] = new SqlCeParameter("@IsSearchable", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isSearchable;

            arParams[10] = new SqlCeParameter("@SearchListName", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new SqlCeParameter("@SupportsPageReuse", SqlDbType.Bit);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = supportsPageReuse;

            arParams[12] = new SqlCeParameter("@DeleteProvider", SqlDbType.NVarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new SqlCeParameter("@PartialView", SqlDbType.NVarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSettingById(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSettingsByFeature(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetModuleDefinition(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
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

        public static IDataReader GetModuleDefinition(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[Guid] = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureInstallationInAdminSites()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("ModuleDefID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("AuthorizedRoles ");
            sqlCommand.Append(")");

            sqlCommand.Append(" SELECT ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("md.ModuleDefID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("md.[Guid], ");
            sqlCommand.Append("'All Users' ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Sites s ");
            sqlCommand.Append("CROSS JOIN ");
            sqlCommand.Append("mp_ModuleDefinitions md ");
            sqlCommand.Append("WHERE s.IsServerAdminSite = 1 ");
            sqlCommand.Append("AND md.[Guid] NOT IN ");
            sqlCommand.Append("(SELECT sd.FeatureGuid FROM mp_SiteModuleDefinitions sd ");
            sqlCommand.Append("WHERE sd.SiteGuid = s.SiteGuid) ");
            
           
            sqlCommand.Append(";");

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  md.*, ");
            sqlCommand.Append("smd.AuthorizedRoles ");

            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");
            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd ");
            sqlCommand.Append("ON smd.FeatureGuid = md.[Guid] ");
            sqlCommand.Append(" ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smd.SiteGuid = @SiteGuid ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("md.SortOrder, md.FeatureName ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            

            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleDefID", typeof(int));
            dt.Columns.Add("FeatureGuid", typeof(String));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));
            dt.Columns.Add("AuthorizedRoles", typeof(String));

            using (IDataReader reader = GetModuleDefinitions(siteGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleDefID"] = reader["ModuleDefID"];
                    row["FeatureGuid"] = reader["Guid"].ToString();
                    row["FeatureName"] = reader["FeatureName"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    row["AuthorizedRoles"] = reader["AuthorizedRoles"];
                    dt.Rows.Add(row);
                }

            }
            return dt;
        }

        public static IDataReader GetUserModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  md.*, ");
            sqlCommand.Append("smd.AuthorizedRoles ");

            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");
            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd ");
            sqlCommand.Append("ON smd.FeatureGuid = md.[Guid] ");
            sqlCommand.Append(" ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smd.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("md.IsAdmin = 0 ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("md.SortOrder, md.FeatureName ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  md.* ");

            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");
            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd ");
            sqlCommand.Append("ON smd.FeatureGuid = md.[Guid] ");
            sqlCommand.Append(" ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("smd.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("md.IsAdmin = 0 ");
            sqlCommand.Append("AND md.IsSearchable = 1 ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("md.SortOrder, md.FeatureName ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static void SyncDefinitions()
        {
            // TODO: this syntax does not work on Sql Ce, need alternative
            //StringBuilder sqlCommand = new StringBuilder();
            //sqlCommand.Append("UPDATE ms ");
            //sqlCommand.Append("SET ms.RegexValidationExpression = mds.RegexValidationExpression, ");
            //sqlCommand.Append("ms.ControlSrc = mds.ControlSrc, ");
            //sqlCommand.Append("ms.ControlType = mds.ControlType, ");
            //sqlCommand.Append("ms.SortOrder = mds.SortOrder, ");
            //sqlCommand.Append("ms.HelpKey = mds.HelpKey ");
            //sqlCommand.Append("FROM mp_ModuleSettings ms ");
            //sqlCommand.Append("JOIN mp_Modules m ");
            //sqlCommand.Append("ON ms.ModuleID = m.ModuleID ");
            //sqlCommand.Append("JOIN mp_ModuleDefinitionSettings mds ");
            //sqlCommand.Append("ON ms.SettingName = mds.SettingName ");
            //sqlCommand.Append("AND m.ModuleDefId = mds.ModuleDefId ");
            
            //SqlHelper.ExecuteNonQuery(
            //    GetConnectionString(),
            //    CommandType.Text,
            //    sqlCommand.ToString(),
            //    null);

        }

        private static bool ModuleDefinitionSettingExists(int moduleDefId, Guid featureGuid, string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("(ModuleDefID = @ModuleDefID OR FeatureGuid = @FeatureGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SettingName = @SettingName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        private static bool CreateModuleDefinitionSetting(
            int moduleDefID,
            string groupName,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            Guid featureGuid,
            string resourceFile,
            string controlSrc,
            int sortOrder,
            string helpKey)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ModuleDefinitionSettings ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleDefID, ");
            sqlCommand.Append("SettingName, ");
            sqlCommand.Append("SettingValue, ");
            sqlCommand.Append("ControlType, ");
            sqlCommand.Append("RegexValidationExpression, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ResourceFile, ");
            sqlCommand.Append("ControlSrc, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("GroupName, ");
            sqlCommand.Append("HelpKey ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleDefID, ");
            sqlCommand.Append("@SettingName, ");
            sqlCommand.Append("@SettingValue, ");
            sqlCommand.Append("@ControlType, ");
            sqlCommand.Append("@RegexValidationExpression, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ResourceFile, ");
            sqlCommand.Append("@ControlSrc, ");
            sqlCommand.Append("@SortOrder, ");
            sqlCommand.Append("@GroupName, ");
            sqlCommand.Append("@HelpKey ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefID;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqlCeParameter("@SettingValue", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new SqlCeParameter("@ControlType", SqlDbType.NVarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new SqlCeParameter("@RegexValidationExpression", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = featureGuid;

            arParams[6] = new SqlCeParameter("@ResourceFile", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new SqlCeParameter("@HelpKey", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;

            arParams[10] = new SqlCeParameter("@GroupName", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (newId > -1);

        }

        private static bool UpdateModuleDefinitionSetting(
            int moduleDefID,
            string groupName,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            Guid featureGuid,
            string resourceFile,
            string controlSrc,
            int sortOrder,
            string helpKey)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SettingValue = @SettingValue, ");
            sqlCommand.Append("ControlType = @ControlType, ");
            sqlCommand.Append("RegexValidationExpression = @RegexValidationExpression, ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid, ");
            sqlCommand.Append("ResourceFile = @ResourceFile, ");
            sqlCommand.Append("ControlSrc = @ControlSrc, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("GroupName = @GroupName, ");
            sqlCommand.Append("HelpKey = @HelpKey ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("(ModuleDefID = @ModuleDefID OR FeatureGuid = @FeatureGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SettingName = @SettingName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefID;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new SqlCeParameter("@SettingValue", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new SqlCeParameter("@ControlType", SqlDbType.NVarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new SqlCeParameter("@RegexValidationExpression", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = featureGuid;

            arParams[6] = new SqlCeParameter("@ResourceFile", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new SqlCeParameter("@HelpKey", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;

            arParams[10] = new SqlCeParameter("@GroupName", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool UpdateModuleDefinitionSetting(
            Guid featureGuid,
            int moduleDefId,
            string resourceFile,
            string groupName,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            if (ModuleDefinitionSettingExists(moduleDefId, featureGuid, settingName))
            {
                return UpdateModuleDefinitionSetting(
                    moduleDefId,
                    groupName,
                    settingName,
                    settingValue,
                    controlType,
                    regexValidationExpression,
                    featureGuid,
                    resourceFile,
                    controlSrc,
                    sortOrder,
                    helpKey);


            }
            else
            {
                return CreateModuleDefinitionSetting(
                    moduleDefId,
                    groupName,
                    settingName,
                    settingValue,
                    controlType,
                    regexValidationExpression,
                    featureGuid,
                    resourceFile,
                    controlSrc,
                    sortOrder,
                    helpKey);


            }

        }

        public static bool UpdateModuleDefinitionSettingById(
            int id,
            int moduleDefId,
            string resourceFile,
            string groupName,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID, ");
            sqlCommand.Append("SettingName = @SettingName, ");
            sqlCommand.Append("SettingValue = @SettingValue, ");
            sqlCommand.Append("ControlType = @ControlType, ");
            sqlCommand.Append("RegexValidationExpression = @RegexValidationExpression, ");
            sqlCommand.Append("ResourceFile = @ResourceFile, ");
            sqlCommand.Append("ControlSrc = @ControlSrc, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("GroupName = @GroupName, ");
            sqlCommand.Append("HelpKey = @HelpKey ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            arParams[3] = new SqlCeParameter("@SettingValue", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = settingValue;

            arParams[4] = new SqlCeParameter("@ControlType", SqlDbType.NVarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = controlType;

            arParams[5] = new SqlCeParameter("@RegexValidationExpression", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = regexValidationExpression;

            arParams[6] = new SqlCeParameter("@ResourceFile", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new SqlCeParameter("@ControlSrc", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new SqlCeParameter("@SortOrder", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new SqlCeParameter("@HelpKey", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;

            arParams[10] = new SqlCeParameter("@GroupName", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SiteModuleDefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("AuthorizedRoles = @AuthorizedRoles ");
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteID = @SiteID AND ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@AuthorizedRoles", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = authorizedRoles;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SettingName = @SettingName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            arParams[1] = new SqlCeParameter("@SettingName", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
