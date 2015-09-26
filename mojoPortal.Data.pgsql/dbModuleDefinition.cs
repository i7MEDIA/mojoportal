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
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBModuleDefinition
    {
       
        /// <summary>
        /// Inserts a row in the mp_ModuleDefinitions table. Returns new integer id.
        /// </summary>
        /// <param name="featureName"> featureName </param>
        /// <param name="controlSrc"> controlSrc </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="isAdmin"> isAdmin </param>
        /// <param name="icon"> icon </param>
        /// <param name="defaultCacheTime"> defaultCacheTime </param>
        /// <param name="guid"> guid </param>
        /// <param name="resourceFile"> resourceFile </param>
        /// <param name="isCacheable"> isCacheable </param>
        /// <param name="isSearchable"> isSearchable </param>
        /// <param name="searchListName"> searchListName </param>
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
            sqlCommand.Append("INSERT INTO mp_moduledefinitions (");
            sqlCommand.Append("featurename, ");
            sqlCommand.Append("controlsrc, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("isadmin, ");
            sqlCommand.Append("icon, ");
            sqlCommand.Append("defaultcachetime, ");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("resourcefile, ");
            sqlCommand.Append("supportspagereuse, ");
            sqlCommand.Append("deleteprovider, ");
            sqlCommand.Append("iscacheable, ");
            sqlCommand.Append("issearchable, ");
            sqlCommand.Append("partialview, ");

            sqlCommand.Append("searchlistname )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":featurename, ");
            sqlCommand.Append(":controlsrc, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":isadmin, ");
            sqlCommand.Append(":icon, ");
            sqlCommand.Append(":defaultcachetime, ");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":resourcefile, ");
            sqlCommand.Append(":supportspagereuse, ");
            sqlCommand.Append(":deleteprovider, ");
            sqlCommand.Append(":iscacheable, ");
            sqlCommand.Append(":issearchable, ");
            sqlCommand.Append(":partialview, ");

            sqlCommand.Append(":searchlistname )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_moduledefinitions_moduledefid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[14];

            arParams[0] = new NpgsqlParameter("featurename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureName;

            arParams[1] = new NpgsqlParameter("controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = controlSrc;

            arParams[2] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sortOrder;

            arParams[3] = new NpgsqlParameter("isadmin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isAdmin;

            arParams[4] = new NpgsqlParameter("icon", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = icon;

            arParams[5] = new NpgsqlParameter("defaultcachetime", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = defaultCacheTime;

            arParams[6] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = featureGuid.ToString();

            arParams[7] = new NpgsqlParameter("resourcefile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new NpgsqlParameter("iscacheable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = isCacheable;

            arParams[9] = new NpgsqlParameter("issearchable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isSearchable;

            arParams[10] = new NpgsqlParameter("searchlistname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new NpgsqlParameter("supportspagereuse", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = supportsPageReuse;

            arParams[12] = new NpgsqlParameter("deleteprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new NpgsqlParameter("partialview", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            if (siteId > -1)
            {

                // now add to mp_SiteModuleDefinitions
                sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO mp_sitemoduledefinitions (");
                sqlCommand.Append("siteid, ");
                sqlCommand.Append("siteguid, ");
                sqlCommand.Append("featureguid, ");
                sqlCommand.Append("moduledefid,  ");
                sqlCommand.Append("authorizedroles ");
                sqlCommand.Append(")");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append(":siteid, ");
                sqlCommand.Append("(SELECT siteguid FROM mp_sites WHERE siteid = :siteid LIMIT 1), ");
                sqlCommand.Append("(SELECT guid FROM mp_moduledefinitions WHERE moduledefid = :moduledefid LIMIT 1), ");
                sqlCommand.Append(":moduledefid, ");
                sqlCommand.Append("'All Users' ");
                sqlCommand.Append(" ) ;");


                arParams = new NpgsqlParameter[2];

                arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = siteId;

                arParams[1] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = newID;

                NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(), 
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            }


            return newID;

            

        }

        /// <summary>
        /// Updates a row in the mp_ModuleDefinitions table. Returns true if row updated.
        /// </summary>
        /// <param name="moduleDefID"> moduleDefID </param>
        /// <param name="featureName"> featureName </param>
        /// <param name="controlSrc"> controlSrc </param>
        /// <param name="sortOrder"> sortOrder </param>
        /// <param name="isAdmin"> isAdmin </param>
        /// <param name="icon"> icon </param>
        /// <param name="defaultCacheTime"> defaultCacheTime </param>
        /// <param name="guid"> guid </param>
        /// <param name="resourceFile"> resourceFile </param>
        /// <param name="isCacheable"> isCacheable </param>
        /// <param name="isSearchable"> isSearchable </param>
        /// <param name="searchListName"> searchListName </param>
        /// <returns>bool</returns>
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_moduledefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("featurename = :featurename, ");
            sqlCommand.Append("controlsrc = :controlsrc, ");
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("isadmin = :isadmin, ");
            sqlCommand.Append("icon = :icon, ");
            sqlCommand.Append("defaultcachetime = :defaultcachetime, ");
            sqlCommand.Append("resourcefile = :resourcefile, ");

            sqlCommand.Append("supportspagereuse = :supportspagereuse, ");
            sqlCommand.Append("deleteprovider = :deleteprovider, ");
            sqlCommand.Append("partialview = :partialview, ");

            sqlCommand.Append("iscacheable = :iscacheable, ");
            sqlCommand.Append("issearchable = :issearchable, ");
            sqlCommand.Append("searchlistname = :searchlistname ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("moduledefid = :moduledefid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[14];

            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            arParams[1] = new NpgsqlParameter("featurename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureName;

            arParams[2] = new NpgsqlParameter("controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = controlSrc;

            arParams[3] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = sortOrder;

            arParams[4] = new NpgsqlParameter("isadmin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isAdmin;

            arParams[5] = new NpgsqlParameter("icon", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = icon;

            arParams[6] = new NpgsqlParameter("defaultcachetime", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = defaultCacheTime;

            arParams[7] = new NpgsqlParameter("resourcefile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = resourceFile;

            arParams[8] = new NpgsqlParameter("iscacheable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = isCacheable;

            arParams[9] = new NpgsqlParameter("issearchable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isSearchable;

            arParams[10] = new NpgsqlParameter("searchlistname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = searchListName;

            arParams[11] = new NpgsqlParameter("supportspagereuse", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = supportsPageReuse;

            arParams[12] = new NpgsqlParameter("deleteprovider", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deleteProvider;

            arParams[13] = new NpgsqlParameter("partialview", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = partialView;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

           
        }

        public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_sitemoduledefinitions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("authorizedroles = :authorizedroles ");
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("siteid = :siteid AND ");
            sqlCommand.Append("moduledefid = :moduledefid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new NpgsqlParameter("authorizedroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = authorizedRoles;

            
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteModuleDefinition(int moduleDefId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_moduledefinitions_delete(:moduledefid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sitemoduledefinitions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduledefid = :moduledefid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSettingById(int id)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_moduledefinitions_deletesettingbyid(:id)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteSettingsByFeature(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_moduledefinitionsettings ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduledefid = :moduledefid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetModuleDefinition(int moduleDefId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_moduledefinitions_selectone(:moduledefid)",
                arParams);

        }

        public static IDataReader GetModuleDefinition(
            Guid featureGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_moduledefinitions_selectonebyguid(:featureguid)",
                arParams);

        }

        public static void EnsureInstallationInAdminSites()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("insert into mp_sitemoduledefinitions (siteid, moduledefid, siteguid, featureguid, authorizedroles) ");
            sqlCommand.Append("select distinct  ");
            sqlCommand.Append("s.siteid,  ");
            sqlCommand.Append("md.moduledefid,  ");
            sqlCommand.Append("s.siteguid,  ");
            sqlCommand.Append("(select guid from mp_moduledefinitions where moduledefid = md.moduledefid limit 1), ");
            sqlCommand.Append("'All Users' ");
            sqlCommand.Append("from mp_sites s,  ");
            sqlCommand.Append("mp_moduledefinitions md ");
            sqlCommand.Append("where s.isserveradminsite = true ");
            sqlCommand.Append("and md.moduledefid not in ");
            sqlCommand.Append("(select smd.moduledefid ");
            sqlCommand.Append("from mp_sitemoduledefinitions smd ");
            sqlCommand.Append("where smd.siteid = s.siteid ");
            sqlCommand.Append("); ");

            NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);
            

            //NpgsqlHelper.ExecuteNonQuery(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_sitemoduledefinitions_ensureforadminsites",
            //    null);
        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.*, ");
            sqlCommand.Append("smd.authorizedroles ");
            sqlCommand.Append("FROM	mp_moduledefinitions md ");

            sqlCommand.Append("JOIN	mp_sitemoduledefinitions smd  ");
            sqlCommand.Append("ON md.moduledefid = smd.moduledefid  ");

            sqlCommand.Append("WHERE smd.siteguid = :siteguid ");
            sqlCommand.Append("ORDER BY md.sortorder, md.featurename ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
           
            
            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_moduledefinitions_selectbysite(:siteguid)",
            //    arParams);
        }

        //public static IDataReader GetModuleDefinitions(int siteId)
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = siteId;
            
        //    return NpgsqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_moduledefinitions_select(:siteid)",
        //        arParams);
        //}

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            //arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = siteGuid.ToString();
            
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
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
        //    arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = siteId;
           
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("ModuleDefID", typeof(int));
        //    dt.Columns.Add("FeatureName", typeof(String));
        //    dt.Columns.Add("ControlSrc", typeof(String));

        //    using (IDataReader reader = NpgsqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        CommandType.StoredProcedure,
        //        "mp_moduledefinitions_select(:siteid)",
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
            sqlCommand.Append("smd.authorizedroles ");
            sqlCommand.Append("FROM	mp_moduledefinitions md ");

            sqlCommand.Append("JOIN	mp_sitemoduledefinitions smd  ");
            sqlCommand.Append("ON md.moduledefid = smd.moduledefid  ");

            sqlCommand.Append("WHERE smd.siteid = :siteid AND md.isadmin = false ");
            sqlCommand.Append("ORDER BY md.sortorder, md.featurename ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetSearchableModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT md.* ");
            sqlCommand.Append("FROM	mp_moduledefinitions md ");

            sqlCommand.Append("JOIN	mp_sitemoduledefinitions smd  ");
            sqlCommand.Append("ON md.moduledefid = smd.moduledefid  ");

            sqlCommand.Append("WHERE smd.siteid = :siteid AND md.isadmin = false AND md.issearchable = true ");
            sqlCommand.Append("ORDER BY md.sortorder, md.searchlistname ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        //public static void SyncDefinitions()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_modulesettings ");
        //    sqlCommand.Append("SET controlsrc = (SELECT mds.controlsrc ");
        //    sqlCommand.Append("FROM mp_moduledefinitionsettings mds  ");
        //    sqlCommand.Append("WHERE mds.moduledefid IN (SELECT moduledefid  ");
        //    sqlCommand.Append("FROM mp_modules m ");
        //    sqlCommand.Append("WHERE m.moduleid = mp_modulesettings.moduleid) ");
        //    sqlCommand.Append("AND mds.settingname = mp_modulesettings.settingname LIMIT 1 ) ");
        //    //sqlCommand.Append(" ");
        //    sqlCommand.Append("; ");

        //    NpgsqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString()
        //        );

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_modulesettings ");
        //    sqlCommand.Append("SET controltype = (SELECT  mds.controltype ");
        //    sqlCommand.Append("FROM mp_moduledefinitionsettings mds  ");
        //    sqlCommand.Append("WHERE mds.moduledefid IN (SELECT moduledefid  ");
        //    sqlCommand.Append("FROM mp_modules m ");
        //    sqlCommand.Append("WHERE m.moduleid = mp_modulesettings.moduleid) ");
        //    sqlCommand.Append("AND mds.settingname = mp_modulesettings.settingname LIMIT 1 ) ");

        //    sqlCommand.Append("; ");

        //    NpgsqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString()
        //        );

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_modulesettings ");
        //    sqlCommand.Append("SET sortorder = COALESCE((SELECT mds.sortorder ");
        //    sqlCommand.Append("FROM mp_moduledefinitionsettings mds  ");
        //    sqlCommand.Append("WHERE mds.moduledefid IN (SELECT moduledefid  ");
        //    sqlCommand.Append("FROM mp_modules m ");
        //    sqlCommand.Append("WHERE m.moduleid = mp_modulesettings.moduleid) ");
        //    sqlCommand.Append("AND mds.settingname = mp_modulesettings.settingname LIMIT 1 ), 100); ");
        //    sqlCommand.Append(" ");

        //    NpgsqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString()
        //        );

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_moduleSettings ");
        //    sqlCommand.Append("SET helpkey = (SELECT mds.helpkey ");
        //    sqlCommand.Append("FROM mp_moduledefinitionsettings mds  ");
        //    sqlCommand.Append("WHERE mds.moduledefid IN (SELECT moduledefid  ");
        //    sqlCommand.Append("FROM mp_modules m ");
        //    sqlCommand.Append("WHERE m.moduleid = mp_modulesettings.moduleid) ");
        //    sqlCommand.Append("AND mds.settingname = mp_modulesettings.settingname LIMIT 1 ); ");
        //    sqlCommand.Append(" ");

        //    NpgsqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString()
        //        );

        //    sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_modulesettings ");
        //    sqlCommand.Append("SET regexvalidationexpression = (SELECT mds.regexvalidationexpression ");
        //    sqlCommand.Append("FROM mp_moduledefinitionsettings mds  ");
        //    sqlCommand.Append("WHERE mds.moduledefid IN (SELECT moduledefid  ");
        //    sqlCommand.Append("FROM mp_modules m ");
        //    sqlCommand.Append("WHERE m.moduleid = mp_modulesettings.moduleid) ");
        //    sqlCommand.Append("AND mds.settingname = mp_modulesettings.settingname LIMIT 1 ); ");
        //    sqlCommand.Append(" ");

        //    NpgsqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        CommandType.Text,
        //        sqlCommand.ToString()
        //        );



        //}

        private static bool SettingExists(
            Guid featureGuid,
            int moduleDefId,
            string settingName)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_moduledefinitionsettings ");
            sqlCommand.Append("WHERE (moduledefid = :moduledefid OR featureguid = :featureguid)  ");
            sqlCommand.Append("AND settingname = :settingname  ;");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            arParams[1] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new NpgsqlParameter("settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            int count =  Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

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
            if (!SettingExists(featureGuid, moduleDefId, settingName))
            {
                return CreateSetting(
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
                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append("UPDATE mp_moduledefinitionsettings ");
                sqlCommand.Append("SET  ");
                //sqlCommand.Append("moduledefid = :moduledefid, ");
                //sqlCommand.Append("settingname = :settingname, ");
                sqlCommand.Append("settingvalue = :settingvalue, ");
                sqlCommand.Append("controltype = :controltype, ");
                sqlCommand.Append("regexvalidationexpression = :regexvalidationexpression, ");
                sqlCommand.Append("featureguid = :featureguid, ");
                sqlCommand.Append("resourcefile = :resourcefile, ");
                sqlCommand.Append("controlsrc = :controlsrc, ");
                sqlCommand.Append("sortorder = :sortorder, ");
                sqlCommand.Append("groupname = :groupname, ");
                sqlCommand.Append("helpkey = :helpkey ");
                sqlCommand.Append("WHERE (moduledefid = :moduledefid OR featureguid = :featureguid)  ");
                sqlCommand.Append("AND settingname = :settingname  ; ");

                sqlCommand.Append(";");

                NpgsqlParameter[] arParams = new NpgsqlParameter[11];

                arParams[0] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
                arParams[0].Direction = ParameterDirection.Input;
                arParams[0].Value = featureGuid.ToString();

                arParams[1] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[1].Direction = ParameterDirection.Input;
                arParams[1].Value = moduleDefId;

                arParams[2] = new NpgsqlParameter("settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
                arParams[2].Direction = ParameterDirection.Input;
                arParams[2].Value = settingName;

                arParams[3] = new NpgsqlParameter("settingvalue", NpgsqlTypes.NpgsqlDbType.Text);
                arParams[3].Direction = ParameterDirection.Input;
                arParams[3].Value = settingValue;

                arParams[4] = new NpgsqlParameter("controltype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
                arParams[4].Direction = ParameterDirection.Input;
                arParams[4].Value = controlType;

                arParams[5] = new NpgsqlParameter("regexvalidationexpression", NpgsqlTypes.NpgsqlDbType.Text);
                arParams[5].Direction = ParameterDirection.Input;
                arParams[5].Value = regexValidationExpression;

                arParams[6] = new NpgsqlParameter("resourcefile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
                arParams[6].Direction = ParameterDirection.Input;
                arParams[6].Value = resourceFile;

                arParams[7] = new NpgsqlParameter("controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
                arParams[7].Direction = ParameterDirection.Input;
                arParams[7].Value = controlSrc;

                arParams[8] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
                arParams[8].Direction = ParameterDirection.Input;
                arParams[8].Value = sortOrder;

                arParams[9] = new NpgsqlParameter("helpkey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
                arParams[9].Direction = ParameterDirection.Input;
                arParams[9].Value = helpKey;

                arParams[10] = new NpgsqlParameter("groupname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
                arParams[10].Direction = ParameterDirection.Input;
                arParams[10].Value = groupName;


                int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                    ConnectionString.GetWriteConnectionString(),
                    CommandType.Text,
                    sqlCommand.ToString(),
                    arParams);

                return (rowsAffected > -1);
            }
            
           

        }

        private static bool CreateSetting(
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
            sqlCommand.Append("INSERT INTO mp_moduledefinitionsettings (");
            sqlCommand.Append("moduledefid, ");
            sqlCommand.Append("settingname, ");
            sqlCommand.Append("settingvalue, ");
            sqlCommand.Append("controltype, ");
            sqlCommand.Append("regexvalidationexpression, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("resourcefile, ");
            sqlCommand.Append("controlsrc, ");
            sqlCommand.Append("sortorder, ");
            sqlCommand.Append("groupname, ");
            sqlCommand.Append("helpkey )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduledefid, ");
            sqlCommand.Append(":settingname, ");
            sqlCommand.Append(":settingvalue, ");
            sqlCommand.Append(":controltype, ");
            sqlCommand.Append(":regexvalidationexpression, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":resourcefile, ");
            sqlCommand.Append(":controlsrc, ");
            sqlCommand.Append(":sortorder, ");
            sqlCommand.Append(":groupname, ");
            sqlCommand.Append(":helpkey )");
            sqlCommand.Append(";");
            

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            arParams[0] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefID;

            arParams[1] = new NpgsqlParameter("settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            arParams[2] = new NpgsqlParameter("settingvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingValue;

            arParams[3] = new NpgsqlParameter("controltype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = controlType;

            arParams[4] = new NpgsqlParameter("regexvalidationexpression", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = regexValidationExpression;

            arParams[5] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = featureGuid.ToString();

            arParams[6] = new NpgsqlParameter("resourcefile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new NpgsqlParameter("controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new NpgsqlParameter("helpkey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;

            arParams[10] = new NpgsqlParameter("groupname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return (rowsAffected > 0);

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
            sqlCommand.Append("UPDATE mp_moduledefinitionsettings ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("moduledefid = :moduledefid, ");
            sqlCommand.Append("settingname = :settingname, ");
            sqlCommand.Append("settingvalue = :settingvalue, ");
            sqlCommand.Append("controltype = :controltype, ");
            sqlCommand.Append("regexvalidationexpression = :regexvalidationexpression, ");
           
            sqlCommand.Append("resourcefile = :resourcefile, ");
            sqlCommand.Append("controlsrc = :controlsrc, ");
            sqlCommand.Append("sortorder = :sortorder, ");
            sqlCommand.Append("groupname = :groupname, ");
            sqlCommand.Append("helpkey = :helpkey ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("id = :id ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];

            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new NpgsqlParameter("moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new NpgsqlParameter("settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = settingName;

            arParams[3] = new NpgsqlParameter("settingvalue", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = settingValue;

            arParams[4] = new NpgsqlParameter("controltype", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = controlType;

            arParams[5] = new NpgsqlParameter("regexvalidationexpression", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = regexValidationExpression;

            arParams[6] = new NpgsqlParameter("resourcefile", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = resourceFile;

            arParams[7] = new NpgsqlParameter("controlsrc", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = controlSrc;

            arParams[8] = new NpgsqlParameter("sortorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = sortOrder;

            arParams[9] = new NpgsqlParameter("helpkey", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = helpKey;

            arParams[10] = new NpgsqlParameter("groupname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = groupName;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

           

        }

        public static IDataReader ModuleDefinitionSettingsGetSetting(
            Guid featureGuid,
            string settingName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            arParams[1] = new NpgsqlParameter("settingname", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = settingName;

            return NpgsqlHelper.ExecuteReader(ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_moduledefinitionsettings_selectone(:featureguid,:settingname)",
                arParams);

        }

    }
}
