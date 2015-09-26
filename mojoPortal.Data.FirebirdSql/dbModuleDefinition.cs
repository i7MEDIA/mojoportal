/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2014-07-29
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
    
    public static class DBModuleDefinition
    {
       
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            #region Bit Conversion

            int intIsAdmin = 0;
            if (isAdmin) { intIsAdmin = 1; }

            int intIsCacheable = 0;
            if (isCacheable) { intIsCacheable = 1; }

            int intIsSearchable = 0;
            if (isSearchable) { intIsSearchable = 1; }

            int intSupportsPageReuse = 0;
            if (supportsPageReuse) { intSupportsPageReuse = 1; }


            #endregion

            FbParameter[] arParams = new FbParameter[14];

            arParams[0] = new FbParameter(":FeatureName", FbDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureName;

            arParams[1] = new FbParameter(":ControlSrc", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = controlSrc;

            arParams[2] = new FbParameter(":SortOrder", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new FbParameter(":IsAdmin", FbDbType.SmallInt);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intIsAdmin;

            arParams[4] = new FbParameter(":Icon", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = icon;

            arParams[5] = new FbParameter(":DefaultCacheTime", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = defaultCacheTime;

            arParams[6] = new FbParameter(":Guid", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = featureGuid.ToString();

            arParams[7] = new FbParameter(":ResourceFile", FbDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new FbParameter(":IsCacheable", FbDbType.SmallInt);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intIsCacheable;

            arParams[9] = new FbParameter(":IsSearchable", FbDbType.SmallInt);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsSearchable;

            arParams[10] = new FbParameter(":SearchListName", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new FbParameter(":SupportsPageReuse", FbDbType.SmallInt);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intSupportsPageReuse;

            arParams[12] = new FbParameter(":DeleteProvider", FbDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new FbParameter(":PartialView", FbDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;


            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_MODULEDEFINITIONS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

            if (siteId > -1)
            {
                StringBuilder sqlCommand = new StringBuilder();
                // now add to  mp_SiteModuleDefinitions

                sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions (");
                sqlCommand.Append("SiteID, ");
                sqlCommand.Append("SiteGuid, ");
                sqlCommand.Append("FeatureGuid, ");
                sqlCommand.Append("AuthorizedRoles, ");
                sqlCommand.Append("ModuleDefID ) ");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append("@SiteID, ");
                sqlCommand.Append("(SELECT FIRST 1 SiteGuid FROM mp_Sites WHERE SiteID = @SiteID), ");
                sqlCommand.Append("(SELECT FIRST 1 Guid FROM mp_ModuleDefinitions WHERE ModuleDefID = @ModuleDefID), ");
                sqlCommand.Append("'All Users', ");
                sqlCommand.Append("@ModuleDefID ) ; ");

                arParams = new FbParameter[2];

                arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = siteId;

                arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = newID;

                FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

            }

            return newID;

        }


        public static bool UpdateModuleDefinition(
            int moduleDefId,
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
            int intIsAdmin = 0;
            if (isAdmin) { intIsAdmin = 1; }

            int intIsCacheable = 0;
            if (isCacheable) { intIsCacheable = 1; }

            int intIsSearchable = 0;
            if (isSearchable) { intIsSearchable = 1; }

            int intSupportsPageReuse = 0;
            if (supportsPageReuse) { intSupportsPageReuse = 1; }

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ModuleDefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FeatureName = @FeatureName, ");
            sqlCommand.Append("ControlSrc = @ControlSrc, ");
            sqlCommand.Append("SortOrder = @SortOrder, ");
            sqlCommand.Append("DefaultCacheTime = @DefaultCacheTime, ");
            sqlCommand.Append("Icon = @Icon, ");
            sqlCommand.Append("IsAdmin = @IsAdmin, ");
            sqlCommand.Append("IsCacheable = @IsCacheable, ");
            sqlCommand.Append("IsSearchable = @IsSearchable, ");
            sqlCommand.Append("SearchListName = @SearchListName, ");
            sqlCommand.Append("SupportsPageReuse = @SupportsPageReuse, ");
            sqlCommand.Append("DeleteProvider = @DeleteProvider, ");
            sqlCommand.Append("PartialView = @PartialView, ");


            sqlCommand.Append("ResourceFile = @ResourceFile ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ;");

            FbParameter[] arParams = new FbParameter[14];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new FbParameter("@FeatureName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureName;

            arParams[2] = new FbParameter("@ControlSrc", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = controlSrc;

            arParams[3] = new FbParameter("@SortOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new FbParameter("@IsAdmin", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsAdmin;

            arParams[5] = new FbParameter("@Icon", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = icon;

            arParams[6] = new FbParameter("@DefaultCacheTime", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = defaultCacheTime;

            arParams[7] = new FbParameter("@ResourceFile", FbDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new FbParameter("@IsCacheable", FbDbType.SmallInt);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intIsCacheable;

            arParams[9] = new FbParameter("@IsSearchable", FbDbType.SmallInt);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsSearchable;

            arParams[10] = new FbParameter("@SearchListName", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new FbParameter("@SupportsPageReuse", FbDbType.SmallInt);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intSupportsPageReuse;

            arParams[12] = new FbParameter("@DeleteProvider", FbDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new FbParameter("@PartialView", FbDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(), sqlCommand.ToString(), arParams);

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

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@AuthorizedRoles", FbDbType.VarChar, -1);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = authorizedRoles;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetModuleDefinition(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModuleDefinition(
            Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @FeatureGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FeatureGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void EnsureInstallationInAdminSites()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("AuthorizedRoles, ");
            sqlCommand.Append("ModuleDefID ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.SiteID, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("md.Guid, ");
            sqlCommand.Append("'All Users', ");
            sqlCommand.Append("md.ModuleDefID ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_Sites s, ");
            sqlCommand.Append("mp_ModuleDefinitions md ");
            sqlCommand.Append("WHERE s.IsServerAdminSite = 1 ");
            sqlCommand.Append("AND md.ModuleDefID NOT IN ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT sd.ModuleDefID ");
            sqlCommand.Append("FROM mp_SiteModuleDefinitions sd ");
            sqlCommand.Append("WHERE sd.SiteID = s.SiteID ");
            sqlCommand.Append(") ");
            sqlCommand.Append(" ;");

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.*, ");
            sqlCommand.Append("smd.AuthorizedRoles ");

            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        //public static IDataReader GetModuleDefinitions(int siteId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT md.* ");
        //    sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

        //    sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
        //    sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

        //    sqlCommand.Append("WHERE smd.SiteID = @SiteID ");
        //    sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = siteId;

        //    return FBSqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            //StringBuilder sqlCommand = new StringBuilder();
            //sqlCommand.Append("SELECT md.* ");
            //sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

            //sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
            //sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            //sqlCommand.Append("WHERE smd.SiteGuid = @SiteGuid ");
            //sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            //FbParameter[] arParams = new FbParameter[1];

            //arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            //arParams[1].Direction = ParameterDirection.Input;
            //arParams[1].Value = siteGuid.ToString();

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


        //public static DataTable GetModuleDefinitionsBySite(int siteId)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT md.* ");
        //    sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

        //    sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
        //    sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

        //    sqlCommand.Append("WHERE smd.SiteID = @SiteID ");
        //    sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

        //    FbParameter[] arParams = new FbParameter[1];

        //    arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = siteId;

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ModuleDefID", typeof(int));
        //    dt.Columns.Add("FeatureName", typeof(String));
        //    dt.Columns.Add("ControlSrc", typeof(String));

        //    using (IDataReader reader = FBSqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams))
        //    {
        //        while (reader.Read())
        //        {
        //            DataRow row = dt.NewRow();
        //            row["ModuleDefID"] = reader["ModuleDefID"];
        //            row["FeatureName"] = reader["FeatureName"];
        //            row["ControlSrc"] = reader["ControlSrc"];
        //            dt.Rows.Add(row);

        //        }

        //    }

        //    return dt;

        //}

        public static IDataReader GetUserModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.*, ");
            sqlCommand.Append("smd.AuthorizedRoles ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = @SiteID AND md.IsAdmin = 0 ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

            sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
            sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

            sqlCommand.Append("WHERE smd.SiteID = @SiteID AND md.IsAdmin = 0 AND md.IsSearchable = 1 ");
            sqlCommand.Append("ORDER BY md.SortOrder, md.SearchListName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        //public static void SyncDefinitions()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_ModuleSettings ");
        //    sqlCommand.Append("SET ControlSrc = (SELECT FIRST 1 mds.ControlSrc ");
        //    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
        //    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
        //    sqlCommand.Append("FROM mp_Modules m ");
        //    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
        //    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName); ");
        //    sqlCommand.Append(" ");

        //    FBSqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_ModuleSettings ");
        //    sqlCommand.Append("SET ControlType = (SELECT FIRST 1 mds.ControlType ");
        //    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
        //    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
        //    sqlCommand.Append("FROM mp_Modules m ");
        //    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
        //    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName); ");
        //    sqlCommand.Append(" ");

        //    FBSqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_ModuleSettings ");
        //    sqlCommand.Append("SET SortOrder = (SELECT FIRST 1 mds.SortOrder ");
        //    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
        //    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
        //    sqlCommand.Append("FROM mp_Modules m ");
        //    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
        //    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName); ");
        //    sqlCommand.Append(" ");

        //    FBSqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_ModuleSettings ");
        //    sqlCommand.Append("SET HelpKey = (SELECT FIRST 1 mds.HelpKey ");
        //    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
        //    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
        //    sqlCommand.Append("FROM mp_Modules m ");
        //    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
        //    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName); ");
        //    sqlCommand.Append(" ");

        //    FBSqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_ModuleSettings ");
        //    sqlCommand.Append("SET RegexValidationExpression = (SELECT FIRST 1 mds.RegexValidationExpression ");
        //    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
        //    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
        //    sqlCommand.Append("FROM mp_Modules m ");
        //    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
        //    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName); ");
        //    sqlCommand.Append(" ");

        //    FBSqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        null);
            
            

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
            int sortOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT count(*) ");
            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE (ModuleDefID = @ModuleDefID OR FeatureGuid = @FeatureGuid)  ");
            sqlCommand.Append("AND SettingName = @SettingName  ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new FbParameter("@SettingName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new FbParameter("@FeatureGuid", FbDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            sqlCommand = new StringBuilder();

            int rowsAffected = 0;

            if (count > 0)
            {
                sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
                sqlCommand.Append("SET SettingValue = @SettingValue,  ");
                sqlCommand.Append("FeatureGuid = @FeatureGuid,  ");
                sqlCommand.Append("ResourceFile = @ResourceFile,  ");
                sqlCommand.Append("ControlType = @ControlType,  ");
                sqlCommand.Append("ControlSrc = @ControlSrc,  ");
                sqlCommand.Append("HelpKey = @HelpKey,  ");
                sqlCommand.Append("SortOrder = @SortOrder,  ");
                sqlCommand.Append("GroupName = @GroupName,  ");
                sqlCommand.Append("RegexValidationExpression = @RegexValidationExpression  ");

                sqlCommand.Append("WHERE (ModuleDefID = @ModuleDefID OR FeatureGuid = @FeatureGuid)  ");
                sqlCommand.Append("AND SettingName = @SettingName  ; ");

                arParams = new FbParameter[11];

                arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleDefId;

                arParams[1] = new FbParameter("@SettingName", FbDbType.VarChar, 50);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = settingName;

                arParams[2] = new FbParameter("@SettingValue", FbDbType.VarChar, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingValue;

                arParams[3] = new FbParameter("@ControlType", FbDbType.VarChar, 50);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = controlType;

                arParams[4] = new FbParameter("@RegexValidationExpression", FbDbType.VarChar);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = regexValidationExpression;

                arParams[5] = new FbParameter("@FeatureGuid", FbDbType.VarChar, 36);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = featureGuid;

                arParams[6] = new FbParameter("@ResourceFile", FbDbType.VarChar, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = resourceFile;

                arParams[7] = new FbParameter("@ControlSrc", FbDbType.VarChar, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = controlSrc;

                arParams[8] = new FbParameter("@HelpKey", FbDbType.VarChar, 255);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = helpKey;

                arParams[9] = new FbParameter("@SortOrder", FbDbType.Integer);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = sortOrder;

                arParams[10] = new FbParameter("@GroupName", FbDbType.VarChar, 255);
                arParams[10].Direction = ParameterDirection.Input;
                arParams[10].Value = groupName;

                rowsAffected = FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams);

                return (rowsAffected > 0);

            }
            else
            {
                arParams = new FbParameter[11];

                arParams[0] = new FbParameter(":ModuleDefID", FbDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = moduleDefId;

                arParams[1] = new FbParameter(":SettingName", FbDbType.VarChar, 50);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = settingName;

                arParams[2] = new FbParameter(":SettingValue", FbDbType.VarChar, 255);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingValue;

                arParams[3] = new FbParameter(":ControlType", FbDbType.VarChar, 50);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = controlType;

                arParams[4] = new FbParameter(":RegexValidationExpression", FbDbType.VarChar);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = regexValidationExpression;

                arParams[5] = new FbParameter(":FeatureGuid", FbDbType.Char, 36);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = featureGuid.ToString();

                arParams[6] = new FbParameter(":ResourceFile", FbDbType.VarChar, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = resourceFile;

                arParams[7] = new FbParameter(":ControlSrc", FbDbType.VarChar, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = controlSrc;

                arParams[8] = new FbParameter(":HelpKey", FbDbType.VarChar, 255);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = helpKey;

                arParams[9] = new FbParameter(":SortOrder", FbDbType.Integer);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = sortOrder;

                arParams[10] = new FbParameter(":GroupName", FbDbType.VarChar, 255);
                arParams[10].Direction = ParameterDirection.Input;
                arParams[10].Value = groupName;


                rowsAffected = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                    GetConnectionString(),
                    CommandType.StoredProcedure,
                    "EXECUTE PROCEDURE MP_MODULEDEFINITIONSETTINGS_INS ("
                    + FBSqlHelper.GetParamString(arParams.Length) + ")",
                    arParams));

                return (rowsAffected > 0);

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
            sqlCommand.Append("SET SettingName = @SettingName,  ");
            sqlCommand.Append("ResourceFile = @ResourceFile,  ");
            sqlCommand.Append("SettingValue = @SettingValue,  ");
            sqlCommand.Append("ControlType = @ControlType,  ");
            sqlCommand.Append("ControlSrc = @ControlSrc,  ");
            sqlCommand.Append("HelpKey = @HelpKey,  ");
            sqlCommand.Append("SortOrder = @SortOrder,  ");
            sqlCommand.Append("GroupName = @GroupName,  ");
            sqlCommand.Append("RegexValidationExpression = @RegexValidationExpression  ");

            sqlCommand.Append("WHERE ID = @ID  ");
            sqlCommand.Append("AND ModuleDefID = @ModuleDefID  ; ");

            FbParameter[] arParams = new FbParameter[11];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@SettingName", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            arParams[3] = new FbParameter("@SettingValue", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = settingValue;

            arParams[4] = new FbParameter("@ControlType", FbDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = controlType;

            arParams[5] = new FbParameter("@RegexValidationExpression", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = regexValidationExpression;

            arParams[6] = new FbParameter("@ResourceFile", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new FbParameter("@ControlSrc", FbDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new FbParameter("@HelpKey", FbDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = helpKey;

            arParams[9] = new FbParameter("@SortOrder", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = sortOrder;

            arParams[10] = new FbParameter("@GroupName", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteSettingById(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteSettingsByFeature(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");

            sqlCommand.Append("WHERE FeatureGuid = @FeatureGuid  ");
            sqlCommand.Append("AND SettingName = @SettingName ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@FeatureGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            arParams[1] = new FbParameter("@SettingName", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        

    }
}
