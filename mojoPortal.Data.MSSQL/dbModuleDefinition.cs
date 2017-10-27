/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2017-10-26
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
    public static class DBModuleDefinition
    {

        
        

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
            string partialView,
			string skinFileName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitions_Insert", 16);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureName", SqlDbType.NVarChar, 255, ParameterDirection.Input, featureName);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@DefaultCacheTime", SqlDbType.Int, ParameterDirection.Input, defaultCacheTime);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@IsCacheable", SqlDbType.Bit, ParameterDirection.Input, isCacheable);
            sph.DefineSqlParameter("@IsSearchable", SqlDbType.Bit, ParameterDirection.Input, isSearchable);
            sph.DefineSqlParameter("@SearchListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchListName);
            sph.DefineSqlParameter("@SupportsPageReuse", SqlDbType.Bit, ParameterDirection.Input, supportsPageReuse);
            sph.DefineSqlParameter("@DeleteProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, deleteProvider);
            sph.DefineSqlParameter("@PartialView", SqlDbType.NVarChar, 255, ParameterDirection.Input, partialView);
            sph.DefineSqlParameter("@SkinFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, skinFileName);


			int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
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
            string partialView,
			string skinFileName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitions_Update", 15);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@FeatureName", SqlDbType.NVarChar, 255, ParameterDirection.Input, featureName);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@DefaultCacheTime", SqlDbType.Int, ParameterDirection.Input, defaultCacheTime);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@IsCacheable", SqlDbType.Bit, ParameterDirection.Input, isCacheable);
            sph.DefineSqlParameter("@IsSearchable", SqlDbType.Bit, ParameterDirection.Input, isSearchable);
            sph.DefineSqlParameter("@SearchListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, searchListName);
            sph.DefineSqlParameter("@SupportsPageReuse", SqlDbType.Bit, ParameterDirection.Input, supportsPageReuse);
            sph.DefineSqlParameter("@DeleteProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, deleteProvider);
            sph.DefineSqlParameter("@PartialView", SqlDbType.NVarChar, 255, ParameterDirection.Input, partialView);
            sph.DefineSqlParameter("@SkinFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, skinFileName);
			int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteModuleDefinitions_Update", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorizedRoles);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitions_Delete", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteModuleDefinitions_DeleteByFeature", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteSettingById(int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitionSettings_DeleteByID", 1);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteSettingsByFeature(int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitionSettings_DeleteByFeature", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetModuleDefinition(
                int moduleDefId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_SelectOne", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetModuleDefinition(
            Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            return sph.ExecuteReader();
        }

        public static void EnsureInstallationInAdminSites()
        {
            new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteModuleDefinitions_EnsureForAdminSites", 0).ExecuteNonQuery();
        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_Select", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            
            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleDefID", typeof(int));
            dt.Columns.Add("FeatureGuid", typeof(String));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));
            dt.Columns.Add("AuthorizedRoles", typeof(String));

            using (IDataReader reader = sph.ExecuteReader())
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

		public static IDataReader GetModuleDefinitionBySkinFileName(string skinFileName)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_SelectOneBySkinFileName", 1);
			sph.DefineSqlParameter("@SkinFileName", SqlDbType.NVarChar, ParameterDirection.Input, skinFileName);
			return sph.ExecuteReader();
		}

		public static IDataReader GetAllModuleSkinFileNames()
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_GetSkinFileNames", 0);
			return sph.ExecuteReader();
		}

		public static IDataReader GetUserModules(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_SelectUserModules", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitions_SelectSearchableModules", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        //public static void SyncDefinitions()
        //{
        //    SqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_ModuleSettings_SyncDefinitions");
            
        //}

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
            int sortOrder,
			string attributes,
			string options)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitionSettings_Update", 13);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, settingValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 50, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@RegexValidationExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, regexValidationExpression);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, groupName);
            sph.DefineSqlParameter("@Attributes", SqlDbType.NText, ParameterDirection.Input, attributes);
            sph.DefineSqlParameter("@Options", SqlDbType.NText, ParameterDirection.Input, options);

			int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
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
            int sortOrder,
			string attributes,
			string options)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_ModuleDefinitionSettings_UpdateByID", 13);
            sph.DefineSqlParameter("@ID", SqlDbType.Int, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            sph.DefineSqlParameter("@SettingValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, settingValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 50, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@RegexValidationExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, regexValidationExpression);
            sph.DefineSqlParameter("@ResourceFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, resourceFile);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, 255, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@GroupName", SqlDbType.NVarChar, 255, ParameterDirection.Input, groupName);
            sph.DefineSqlParameter("@Attributes", SqlDbType.NText, ParameterDirection.Input, attributes);
            sph.DefineSqlParameter("@Options", SqlDbType.NText, ParameterDirection.Input, options);
			int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ModuleDefinitionSettings_SelectOne", 2);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@SettingName", SqlDbType.NVarChar, 50, ParameterDirection.Input, settingName);
            return sph.ExecuteReader();
        }

        

    }
}
