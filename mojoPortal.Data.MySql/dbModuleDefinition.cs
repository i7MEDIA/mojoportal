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
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Text;

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
			int intIsAdmin = 0;
			if (isAdmin) { intIsAdmin = 1; }

			int intIsCacheable = 0;
			if (isCacheable) { intIsCacheable = 1; }

			int intIsSearchable = 0;
			if (isSearchable) { intIsSearchable = 1; }

			int intSupportsPageReuse = 0;
			if (supportsPageReuse) { intSupportsPageReuse = 1; }


			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_ModuleDefinitions (");
			sqlCommand.Append("Guid, ");
			sqlCommand.Append("FeatureName, ");
			sqlCommand.Append("ControlSrc, ");
			sqlCommand.Append("SortOrder, ");
			sqlCommand.Append("DefaultCacheTime, ");
			sqlCommand.Append("Icon, ");
			sqlCommand.Append("IsAdmin, ");
			sqlCommand.Append("IsCacheable, ");
			sqlCommand.Append("IsSearchable, ");
			sqlCommand.Append("SearchListName, ");
			sqlCommand.Append("SupportsPageReuse, ");
			sqlCommand.Append("DeleteProvider, ");
			sqlCommand.Append("PartialView, ");
			sqlCommand.Append("ResourceFile, ");
			sqlCommand.Append("SkinFileName ");
			sqlCommand.Append(" )");

			sqlCommand.Append(" VALUES (");
			sqlCommand.Append("?FeatureGuid, ");
			sqlCommand.Append("?FeatureName, ");
			sqlCommand.Append("?ControlSrc, ");
			sqlCommand.Append("?SortOrder, ");
			sqlCommand.Append("?DefaultCacheTime, ");
			sqlCommand.Append("?Icon, ");
			sqlCommand.Append("?IsAdmin, ");
			sqlCommand.Append("?IsCacheable, ");
			sqlCommand.Append("?IsSearchable, ");
			sqlCommand.Append("?SearchListName, ");
			sqlCommand.Append("?SupportsPageReuse, ");
			sqlCommand.Append("?DeleteProvider, ");
			sqlCommand.Append("?PartialView, ");
			sqlCommand.Append("?ResourceFile, ");
			sqlCommand.Append("?SkinFileName ");
			sqlCommand.Append(" );");

			sqlCommand.Append("SELECT LAST_INSERT_ID();");

			MySqlParameter[] arParams = new MySqlParameter[16];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?FeatureName", MySqlDbType.VarChar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = featureName;

			arParams[2] = new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = controlSrc;

			arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = sortOrder;

			arParams[4] = new MySqlParameter("?IsAdmin", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = intIsAdmin;

			arParams[5] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 255);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = icon;

			arParams[6] = new MySqlParameter("?DefaultCacheTime", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = defaultCacheTime;

			arParams[7] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = featureGuid;

			arParams[8] = new MySqlParameter("?ResourceFile", MySqlDbType.VarChar, 255);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = resourceFile;

			arParams[9] = new MySqlParameter("?IsCacheable", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = intIsCacheable;

			arParams[10] = new MySqlParameter("?IsSearchable", MySqlDbType.Int32);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = intIsSearchable;

			arParams[11] = new MySqlParameter("?SearchListName", MySqlDbType.VarChar, 255);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = searchListName;

			arParams[12] = new MySqlParameter("?SupportsPageReuse", MySqlDbType.Int32);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = intSupportsPageReuse;

			arParams[13] = new MySqlParameter("?DeleteProvider", MySqlDbType.VarChar, 255);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = deleteProvider;

			arParams[14] = new MySqlParameter("?PartialView", MySqlDbType.VarChar, 255);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = partialView;

			arParams[15] = new MySqlParameter("?SkinFileName", MySqlDbType.VarChar, 255);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = skinFileName;

			int newID = -1;

			newID = Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());

			if (siteId > -1)
			{
				// now add to  mp_SiteModuleDefinitions
				sqlCommand = new StringBuilder();
				sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions (");
				sqlCommand.Append("SiteID, ");
				sqlCommand.Append("SiteGuid, ");
				sqlCommand.Append("FeatureGuid, ");
				sqlCommand.Append("AuthorizedRoles, ");
				sqlCommand.Append("ModuleDefID ) ");

				sqlCommand.Append(" VALUES (");
				sqlCommand.Append("?SiteID, ");
				sqlCommand.Append("(SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID LIMIT 1), ");
				sqlCommand.Append("(SELECT Guid FROM mp_ModuleDefinitions WHERE ModuleDefID = ?ModuleDefID LIMIT 1), ");
				sqlCommand.Append("'All Users', ");
				sqlCommand.Append("?ModuleDefID ) ; ");

				arParams = new MySqlParameter[2];

				arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
				arParams[0].Direction = ParameterDirection.Input;
				arParams[0].Value = siteId;

				arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
				arParams[1].Direction = ParameterDirection.Input;
				arParams[1].Value = newID;

				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
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
			string partialView,
			string skinFileName)
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
			sqlCommand.Append("FeatureName = ?FeatureName, ");
			sqlCommand.Append("ControlSrc = ?ControlSrc, ");
			sqlCommand.Append("SortOrder = ?SortOrder, ");
			sqlCommand.Append("DefaultCacheTime = ?DefaultCacheTime, ");
			sqlCommand.Append("Icon = ?Icon, ");
			sqlCommand.Append("IsAdmin = ?IsAdmin, ");
			sqlCommand.Append("IsCacheable = ?IsCacheable, ");
			sqlCommand.Append("IsSearchable = ?IsSearchable, ");
			sqlCommand.Append("SearchListName = ?SearchListName, ");
			sqlCommand.Append("SupportsPageReuse = ?SupportsPageReuse, ");
			sqlCommand.Append("DeleteProvider = ?DeleteProvider, ");
			sqlCommand.Append("PartialView = ?PartialView, ");
			sqlCommand.Append("ResourceFile = ?ResourceFile, ");
			sqlCommand.Append("SkinFileName = ?SkinFileName ");

			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ;");

			MySqlParameter[] arParams = new MySqlParameter[15];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			arParams[1] = new MySqlParameter("?FeatureName", MySqlDbType.VarChar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = featureName;

			arParams[2] = new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = controlSrc;

			arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = sortOrder;

			arParams[4] = new MySqlParameter("?IsAdmin", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = intIsAdmin;

			arParams[5] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 255);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = icon;

			arParams[6] = new MySqlParameter("?DefaultCacheTime", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = defaultCacheTime;

			arParams[7] = new MySqlParameter("?ResourceFile", MySqlDbType.VarChar, 255);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = resourceFile;

			arParams[8] = new MySqlParameter("?IsCacheable", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = intIsCacheable;

			arParams[9] = new MySqlParameter("?IsSearchable", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = intIsSearchable;

			arParams[10] = new MySqlParameter("?SearchListName", MySqlDbType.VarChar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = searchListName;

			arParams[11] = new MySqlParameter("?SupportsPageReuse", MySqlDbType.Int32);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = intSupportsPageReuse;

			arParams[12] = new MySqlParameter("?DeleteProvider", MySqlDbType.VarChar, 255);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = deleteProvider;

			arParams[13] = new MySqlParameter("?PartialView", MySqlDbType.VarChar, 255);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = partialView;

			arParams[14] = new MySqlParameter("?SkinFileName", MySqlDbType.VarChar, 255);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = skinFileName;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_SiteModuleDefinitions ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("AuthorizedRoles = ?AuthorizedRoles ");

			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("SiteID = ?SiteID AND ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?AuthorizedRoles", MySqlDbType.Text);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = authorizedRoles;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}


		public static bool DeleteModuleDefinition(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ModuleDefinitions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteModuleDefinitionFromSites(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_SiteModuleDefinitions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
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
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
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
			sqlCommand.Append("Guid = ?FeatureGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = featureGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static int GetModuleDefinitionIDFromGuid(Guid featureGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  ModuleDefID ");
			sqlCommand.Append("FROM	mp_ModuleDefinitions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("Guid = ?FeatureGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = featureGuid.ToString();

			int moduleDefId = -1;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					moduleDefId = Convert.ToInt32(reader["ModuleDefID"].ToString(), CultureInfo.InvariantCulture);
				}

			}

			return moduleDefId;

		}

		public static void EnsureInstallationInAdminSites()
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_SiteModuleDefinitions ");
			sqlCommand.Append("(");
			sqlCommand.Append("SiteID, ");
			sqlCommand.Append("SiteGuid, ");
			sqlCommand.Append("FeatureGuid, ");
			sqlCommand.Append("ModuleDefID, ");
			sqlCommand.Append("AuthorizedRoles ");
			sqlCommand.Append(") ");

			sqlCommand.Append("SELECT ");
			sqlCommand.Append("s.SiteID, ");
			sqlCommand.Append("s.SiteGuid, ");
			sqlCommand.Append("md.Guid, ");
			sqlCommand.Append("md.ModuleDefID, ");
			sqlCommand.Append("'All Users' ");

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

			MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
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

			sqlCommand.Append("WHERE smd.SiteGuid = ?SiteGuid ");
			sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
		{
			//StringBuilder sqlCommand = new StringBuilder();
			//sqlCommand.Append("SELECT md.* ");
			//sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

			//sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
			//sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

			//sqlCommand.Append("WHERE smd.SiteGuid = ?SiteGuid ");
			//sqlCommand.Append("ORDER BY md.SortOrder, md.FeatureName ;");

			//MySqlParameter[] arParams = new MySqlParameter[1];

			//arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
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

		public static IDataReader GetModuleDefinitionBySkinFileName(string skinFileName)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * FROM mp_ModuleDefinitions WHERE SkinFileName = ?SkinFileName LIMIT 1;");
			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SkinFileName", MySqlDbType.VarChar, 255);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = skinFileName;

			return MySqlHelper.ExecuteReader(
				 ConnectionString.GetReadConnectionString(),
				 sqlCommand.ToString(),
				 arParams);
		}

		public static IDataReader GetAllModuleSkinFileNames()
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT SkinFileName FROM mp_ModuleDefinitions;");

			return MySqlHelper.ExecuteReader(
				 ConnectionString.GetReadConnectionString(),
				 sqlCommand.ToString());
		}


		public static IDataReader GetUserModules(int siteId)
		{
			var commandText = @"
SELECT md.*, smd.FeatureGuid, smd.AuthorizedRoles
FROM mp_ModuleDefinitions md
JOIN mp_SiteModuleDefinitions smd
ON smd.ModuleDefID = md.ModuleDefID
WHERE smd.SiteID = ?SiteID
AND md.IsAdmin = 0
ORDER BY 
md.SortOrder,
md.FeatureName";

			var commandParameters = new MySqlParameter[]
			{
				new MySqlParameter("?SiteID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetSearchableModules(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT md.* ");
			sqlCommand.Append("FROM	mp_ModuleDefinitions md ");

			sqlCommand.Append("JOIN	mp_SiteModuleDefinitions smd  ");
			sqlCommand.Append("ON md.ModuleDefID = smd.ModuleDefID  ");

			sqlCommand.Append("WHERE smd.SiteID = ?SiteID AND md.IsAdmin = 0 AND md.IsSearchable = 1 ");
			sqlCommand.Append("ORDER BY md.SortOrder, md.SearchListName ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		//public static void SyncDefinitions()
		//{
		//    StringBuilder sqlCommand = new StringBuilder();
		//    sqlCommand.Append("UPDATE mp_ModuleSettings ");
		//    sqlCommand.Append("SET ControlSrc = (SELECT mds.ControlSrc ");
		//    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
		//    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
		//    sqlCommand.Append("FROM mp_Modules m ");
		//    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
		//    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName LIMIT 1 ) ");
		//    //sqlCommand.Append(" ");
		//    sqlCommand.Append("; ");

		//    MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
		//        sqlCommand.ToString(),
		//        null);

		//    sqlCommand = new StringBuilder();
		//    sqlCommand.Append("UPDATE mp_ModuleSettings ");
		//    sqlCommand.Append("SET ControlType = (SELECT  mds.ControlType ");
		//    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
		//    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
		//    sqlCommand.Append("FROM mp_Modules m ");
		//    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
		//    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName LIMIT 1 ) ");

		//    sqlCommand.Append("; ");

		//    MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
		//        sqlCommand.ToString(),
		//        null);

		//    sqlCommand = new StringBuilder();
		//    sqlCommand.Append("UPDATE mp_ModuleSettings ");
		//    sqlCommand.Append("SET SortOrder = COALESCE((SELECT mds.SortOrder ");
		//    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
		//    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
		//    sqlCommand.Append("FROM mp_Modules m ");
		//    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
		//    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName LIMIT 1 ), 100); ");
		//    sqlCommand.Append(" ");

		//    MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
		//        sqlCommand.ToString(),
		//        null);

		//    sqlCommand = new StringBuilder();
		//    sqlCommand.Append("UPDATE mp_ModuleSettings ");
		//    sqlCommand.Append("SET HelpKey = (SELECT mds.HelpKey ");
		//    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
		//    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
		//    sqlCommand.Append("FROM mp_Modules m ");
		//    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
		//    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName LIMIT 1 ); ");
		//    sqlCommand.Append(" ");

		//    MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
		//        sqlCommand.ToString(),
		//        null);

		//    sqlCommand = new StringBuilder();
		//    sqlCommand.Append("UPDATE mp_ModuleSettings ");
		//    sqlCommand.Append("SET RegexValidationExpression = (SELECT mds.RegexValidationExpression ");
		//    sqlCommand.Append("FROM mp_ModuleDefinitionSettings mds  ");
		//    sqlCommand.Append("WHERE mds.ModuleDefId IN (SELECT ModuleDefId  ");
		//    sqlCommand.Append("FROM mp_Modules m ");
		//    sqlCommand.Append("WHERE m.ModuleID = mp_ModuleSettings.ModuleID) ");
		//    sqlCommand.Append("AND mds.SettingName = mp_ModuleSettings.SettingName LIMIT 1 ); ");
		//    sqlCommand.Append(" ");

		//    MySqlHelper.ExecuteNonQuery(
		//        ConnectionString.GetWriteConnectionString(),
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
			int sortOrder,
			string attributes,
			string options)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT count(*) ");
			sqlCommand.Append("FROM	mp_ModuleDefinitionSettings ");

			sqlCommand.Append("WHERE (ModuleDefID = ?ModuleDefID OR FeatureGuid = ?FeatureGuid)  ");
			sqlCommand.Append("AND SettingName = ?SettingName  ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			arParams[1] = new MySqlParameter("?SettingName", MySqlDbType.VarChar, 50);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = settingName;

			arParams[2] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = featureGuid;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());

			sqlCommand = new StringBuilder();

			int rowsAffected = 0;

			if (count > 0)
			{
				sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
				sqlCommand.Append("SET SettingValue = ?SettingValue,  ");
				sqlCommand.Append("FeatureGuid = ?FeatureGuid,  ");
				sqlCommand.Append("ResourceFile = ?ResourceFile,  ");
				sqlCommand.Append("ControlType = ?ControlType,  ");
				sqlCommand.Append("ControlSrc = ?ControlSrc,  ");
				sqlCommand.Append("HelpKey = ?HelpKey,  ");
				sqlCommand.Append("SortOrder = ?SortOrder,  ");
				sqlCommand.Append("GroupName = ?GroupName,  ");
				sqlCommand.Append("RegexValidationExpression = ?RegexValidationExpression,  ");
				sqlCommand.Append("Attributes = ?Attributes,  ");
				sqlCommand.Append("Options = ?Options  ");

				sqlCommand.Append("WHERE (ModuleDefID = ?ModuleDefID OR FeatureGuid = ?FeatureGuid)  ");
				sqlCommand.Append("AND SettingName = ?SettingName  ; ");

				arParams = new MySqlParameter[13];

				arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
				arParams[0].Direction = ParameterDirection.Input;
				arParams[0].Value = moduleDefId;

				arParams[1] = new MySqlParameter("?SettingName", MySqlDbType.VarChar, 50);
				arParams[1].Direction = ParameterDirection.Input;
				arParams[1].Value = settingName;

				arParams[2] = new MySqlParameter("?SettingValue", MySqlDbType.Text);
				arParams[2].Direction = ParameterDirection.Input;
				arParams[2].Value = settingValue;

				arParams[3] = new MySqlParameter("?ControlType", MySqlDbType.VarChar, 50);
				arParams[3].Direction = ParameterDirection.Input;
				arParams[3].Value = controlType;

				arParams[4] = new MySqlParameter("?RegexValidationExpression", MySqlDbType.Text);
				arParams[4].Direction = ParameterDirection.Input;
				arParams[4].Value = regexValidationExpression;

				arParams[5] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
				arParams[5].Direction = ParameterDirection.Input;
				arParams[5].Value = featureGuid;

				arParams[6] = new MySqlParameter("?ResourceFile", MySqlDbType.VarChar, 255);
				arParams[6].Direction = ParameterDirection.Input;
				arParams[6].Value = resourceFile;

				arParams[7] = new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255);
				arParams[7].Direction = ParameterDirection.Input;
				arParams[7].Value = controlSrc;

				arParams[8] = new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255);
				arParams[8].Direction = ParameterDirection.Input;
				arParams[8].Value = helpKey;

				arParams[9] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
				arParams[9].Direction = ParameterDirection.Input;
				arParams[9].Value = sortOrder;

				arParams[10] = new MySqlParameter("?GroupName", MySqlDbType.VarChar, 255);
				arParams[10].Direction = ParameterDirection.Input;
				arParams[10].Value = groupName;

				arParams[11] = new MySqlParameter("?Attributes", MySqlDbType.Text);
				arParams[11].Direction = ParameterDirection.Input;
				arParams[11].Value = attributes;

				arParams[12] = new MySqlParameter("?Options", MySqlDbType.Text);
				arParams[12].Direction = ParameterDirection.Input;
				arParams[12].Value = options;

				rowsAffected = MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand.ToString(),
					arParams);

				return (rowsAffected > 0);

			}
			else
			{

				sqlCommand.Append("INSERT INTO mp_ModuleDefinitionSettings ");
				sqlCommand.Append("( ");
				sqlCommand.Append("FeatureGuid, ");
				sqlCommand.Append("ModuleDefID, ");
				sqlCommand.Append("ResourceFile, ");
				sqlCommand.Append("SettingName, ");
				sqlCommand.Append("SettingValue, ");
				sqlCommand.Append("ControlType, ");
				sqlCommand.Append("ControlSrc, ");
				sqlCommand.Append("HelpKey, ");
				sqlCommand.Append("SortOrder, ");
				sqlCommand.Append("GroupName, ");
				sqlCommand.Append("RegexValidationExpression, ");
				sqlCommand.Append("Attributes, ");
				sqlCommand.Append("Options ");
				sqlCommand.Append(")");

				sqlCommand.Append("VALUES (  ");
				sqlCommand.Append(" ?FeatureGuid , ");
				sqlCommand.Append(" ?ModuleDefID , ");
				sqlCommand.Append(" ?ResourceFile  , ");
				sqlCommand.Append(" ?SettingName  , ");
				sqlCommand.Append(" ?SettingValue  ,");
				sqlCommand.Append(" ?ControlType  ,");
				sqlCommand.Append(" ?ControlSrc, ");
				sqlCommand.Append(" ?HelpKey, ");
				sqlCommand.Append(" ?SortOrder, ");
				sqlCommand.Append(" ?GroupName, ");
				sqlCommand.Append(" ?RegexValidationExpression,  ");
				sqlCommand.Append(" ?Attributes,  ");
				sqlCommand.Append(" ?Options  ");
				sqlCommand.Append(");");

				arParams = new MySqlParameter[13];

				arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
				arParams[0].Direction = ParameterDirection.Input;
				arParams[0].Value = moduleDefId;

				arParams[1] = new MySqlParameter("?SettingName", MySqlDbType.VarChar, 50);
				arParams[1].Direction = ParameterDirection.Input;
				arParams[1].Value = settingName;

				arParams[2] = new MySqlParameter("?SettingValue", MySqlDbType.Text);
				arParams[2].Direction = ParameterDirection.Input;
				arParams[2].Value = settingValue;

				arParams[3] = new MySqlParameter("?ControlType", MySqlDbType.VarChar, 50);
				arParams[3].Direction = ParameterDirection.Input;
				arParams[3].Value = controlType;

				arParams[4] = new MySqlParameter("?RegexValidationExpression", MySqlDbType.Text);
				arParams[4].Direction = ParameterDirection.Input;
				arParams[4].Value = regexValidationExpression;

				arParams[5] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
				arParams[5].Direction = ParameterDirection.Input;
				arParams[5].Value = featureGuid;

				arParams[6] = new MySqlParameter("?ResourceFile", MySqlDbType.VarChar, 255);
				arParams[6].Direction = ParameterDirection.Input;
				arParams[6].Value = resourceFile;

				arParams[7] = new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255);
				arParams[7].Direction = ParameterDirection.Input;
				arParams[7].Value = controlSrc;

				arParams[8] = new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255);
				arParams[8].Direction = ParameterDirection.Input;
				arParams[8].Value = helpKey;

				arParams[9] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
				arParams[9].Direction = ParameterDirection.Input;
				arParams[9].Value = sortOrder;

				arParams[10] = new MySqlParameter("?GroupName", MySqlDbType.VarChar, 255);
				arParams[10].Direction = ParameterDirection.Input;
				arParams[10].Value = groupName;

				arParams[11] = new MySqlParameter("?Attributes", MySqlDbType.Text);
				arParams[11].Direction = ParameterDirection.Input;
				arParams[11].Value = attributes;

				arParams[12] = new MySqlParameter("?Options", MySqlDbType.Text);
				arParams[12].Direction = ParameterDirection.Input;
				arParams[12].Value = options;

				rowsAffected = MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand.ToString(),
					arParams);

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
			int sortOrder,
			string attributes,
			string options)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ModuleDefinitionSettings ");
			sqlCommand.Append("SET SettingName = ?SettingName,  ");
			sqlCommand.Append("ResourceFile = ?ResourceFile,  ");
			sqlCommand.Append("SettingValue = ?SettingValue,  ");
			sqlCommand.Append("ControlType = ?ControlType,  ");
			sqlCommand.Append("ControlSrc = ?ControlSrc,  ");
			sqlCommand.Append("HelpKey = ?HelpKey,  ");
			sqlCommand.Append("SortOrder = ?SortOrder,  ");
			sqlCommand.Append("GroupName = ?GroupName,  ");
			sqlCommand.Append("RegexValidationExpression = ?RegexValidationExpression,  ");
			sqlCommand.Append("Attributes = ?Attributes,  ");
			sqlCommand.Append("Options = ?Options  ");

			sqlCommand.Append("WHERE ID = ?ID  ");
			sqlCommand.Append("AND ModuleDefID = ?ModuleDefID  ; ");

			MySqlParameter[] arParams = new MySqlParameter[13];

			arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = id;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?SettingName", MySqlDbType.VarChar, 50);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = settingName;

			arParams[3] = new MySqlParameter("?SettingValue", MySqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = settingValue;

			arParams[4] = new MySqlParameter("?ControlType", MySqlDbType.VarChar, 50);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = controlType;

			arParams[5] = new MySqlParameter("?RegexValidationExpression", MySqlDbType.Text);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = regexValidationExpression;

			arParams[6] = new MySqlParameter("?ResourceFile", MySqlDbType.VarChar, 255);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = resourceFile;

			arParams[7] = new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = controlSrc;

			arParams[8] = new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = helpKey;

			arParams[9] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = sortOrder;

			arParams[10] = new MySqlParameter("?GroupName", MySqlDbType.VarChar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = groupName;

			arParams[11] = new MySqlParameter("?Attributes", MySqlDbType.Text);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = attributes;

			arParams[12] = new MySqlParameter("?Options", MySqlDbType.Text);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = options;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteSettingById(int id)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ID = ?ID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = id;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteSettingsByFeature(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ModuleDefinitionSettings ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
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

			sqlCommand.Append("WHERE FeatureGuid = ?FeatureGuid  ");
			sqlCommand.Append("AND SettingName = ?SettingName ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = featureGuid.ToString();

			arParams[1] = new MySqlParameter("?SettingName", MySqlDbType.VarChar, 50);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = settingName;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);
		}




	}
}
