using Npgsql;
using System;
using System.Data;
using System.Text;

namespace mojoPortal.Data
{
	public static class DBModule
	{
		public static int AddModule(
			int pageId,
			int siteId,
			Guid siteGuid,
			int moduleDefId,
			int moduleOrder,
			string paneName,
			string moduleTitle,
			string viewRoles,
			string authorizedEditRoles,
			string draftEditRoles,
			string draftApprovalRoles,
			int cacheTime,
			bool showTitle,
			bool availableForMyPage,
			bool allowMultipleInstancesOnMyPage,
			String icon,
			int createdByUserId,
			DateTime createdDate,
			Guid guid,
			Guid featureGuid,
			bool hideFromAuthenticated,
			bool hideFromUnauthenticated,
			string headElement,
			int publishMode
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("INSERT INTO mp_modules (");
			sqlCommand.Append("siteid, ");
			sqlCommand.Append("moduledefid, ");
			sqlCommand.Append("moduletitle, ");
			sqlCommand.Append("authorizededitroles, ");
			sqlCommand.Append("cachetime, ");
			sqlCommand.Append("showtitle, ");
			sqlCommand.Append("edituserid, ");
			sqlCommand.Append("availableformypage, ");
			sqlCommand.Append("allowmultipleinstancesonmypage, ");
			sqlCommand.Append("icon, ");
			sqlCommand.Append("createdbyuserid, ");
			sqlCommand.Append("createddate, ");
			sqlCommand.Append("countofuseonmypage, ");
			sqlCommand.Append("guid, ");
			sqlCommand.Append("featureguid, ");
			sqlCommand.Append("siteguid, ");
			sqlCommand.Append("edituserguid, ");
			sqlCommand.Append("hidefromunauth, ");
			sqlCommand.Append("hidefromauth, ");
			sqlCommand.Append("drafteditroles, ");
			sqlCommand.Append("draftapprovalroles, ");
			sqlCommand.Append("includeinsearch, ");
			sqlCommand.Append("isglobal, ");
			sqlCommand.Append("publishmode, ");
			sqlCommand.Append("headelement, ");
			sqlCommand.Append("viewroles )");

			sqlCommand.Append(" VALUES (");
			sqlCommand.Append(":siteid, ");
			sqlCommand.Append(":moduledefid, ");
			sqlCommand.Append(":moduletitle, ");
			sqlCommand.Append(":authorizededitroles, ");
			sqlCommand.Append(":cachetime, ");
			sqlCommand.Append(":showtitle, ");
			sqlCommand.Append(":edituserid, ");
			sqlCommand.Append(":availableformypage, ");
			sqlCommand.Append(":allowmultipleinstancesonmypage, ");
			sqlCommand.Append(":icon, ");
			sqlCommand.Append(":createdbyuserid, ");
			sqlCommand.Append(":createddate, ");
			sqlCommand.Append(":countofuseonmypage, ");
			sqlCommand.Append(":guid, ");
			sqlCommand.Append(":featureguid, ");
			sqlCommand.Append(":siteguid, ");
			sqlCommand.Append(":edituserguid, ");
			sqlCommand.Append(":hidefromunauth, ");
			sqlCommand.Append(":hidefromauth, ");
			sqlCommand.Append(":drafteditroles, ");
			sqlCommand.Append(":draftapprovalroles, ");
			sqlCommand.Append("true, ");
			sqlCommand.Append("false, ");
			sqlCommand.Append(":publishmode, ");
			sqlCommand.Append(":headelement, ");
			sqlCommand.Append(":viewroles )");
			sqlCommand.Append(";");
			sqlCommand.Append(" SELECT CURRVAL('mp_modules_moduleid_seq');");

			NpgsqlParameter[] arParams = new NpgsqlParameter[24];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new NpgsqlParameter(":moduletitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleTitle;

			arParams[3] = new NpgsqlParameter(":authorizededitroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = authorizedEditRoles;

			arParams[4] = new NpgsqlParameter(":cachetime", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = cacheTime;

			arParams[5] = new NpgsqlParameter(":showtitle", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = showTitle;

			arParams[6] = new NpgsqlParameter(":edituserid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = -1;

			arParams[7] = new NpgsqlParameter(":availableformypage", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = availableForMyPage;

			arParams[8] = new NpgsqlParameter(":allowmultipleinstancesonmypage", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = allowMultipleInstancesOnMyPage;

			arParams[9] = new NpgsqlParameter(":icon", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = icon;

			arParams[10] = new NpgsqlParameter(":createdbyuserid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = createdByUserId;

			arParams[11] = new NpgsqlParameter(":createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = createdDate;

			arParams[12] = new NpgsqlParameter(":countofuseonmypage", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = 0;

			arParams[13] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = guid.ToString();

			arParams[14] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = featureGuid.ToString();

			arParams[15] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = siteGuid.ToString();

			arParams[16] = new NpgsqlParameter(":edituserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = Guid.Empty.ToString();

			arParams[17] = new NpgsqlParameter(":hidefromunauth", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = hideFromUnauthenticated;

			arParams[18] = new NpgsqlParameter(":hidefromauth", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = hideFromAuthenticated;

			arParams[19] = new NpgsqlParameter(":viewroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[19].Direction = ParameterDirection.Input;
			arParams[19].Value = viewRoles;

			arParams[20] = new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[20].Direction = ParameterDirection.Input;
			arParams[20].Value = draftEditRoles;

			arParams[21] = new NpgsqlParameter(":headelement", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
			arParams[21].Direction = ParameterDirection.Input;
			arParams[21].Value = headElement;

			arParams[22] = new NpgsqlParameter(":publishmode", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[22].Direction = ParameterDirection.Input;
			arParams[22].Value = publishMode;

			arParams[23] = new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[23].Direction = ParameterDirection.Input;
			arParams[23].Value = draftApprovalRoles;

			int moduleID = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			);

			if (pageId > -1 && moduleID > -1)
			{
				PageModuleInsert(
					moduleID,
					pageId,
					paneName,
					moduleOrder,
					DateTime.UtcNow,
					DateTime.MinValue
				);
			}

			return moduleID;
		}


		public static bool UpdateModule(
			int moduleId,
			int moduleDefId,
			string moduleTitle,
			string viewRoles,
			string authorizedEditRoles,
			string draftEditRoles,
			string draftApprovalRoles,
			int cacheTime,
			bool showTitle,
			int editUserId,
			bool availableForMyPage,
			bool allowMultipleInstancesOnMyPage,
			String icon,
			bool hideFromAuthenticated,
			bool hideFromUnauthenticated,
			bool includeInSearch,
			bool isGlobal,
			string headElement,
			int publishMode
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_modules ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("moduletitle = :moduletitle, ");
			sqlCommand.Append("authorizededitroles = :authorizededitroles, ");
			sqlCommand.Append("cachetime = :cachetime, ");
			sqlCommand.Append("showtitle = :showtitle, ");
			sqlCommand.Append("edituserid = :edituserid, ");
			sqlCommand.Append("availableformypage = :availableformypage, ");
			sqlCommand.Append("allowmultipleinstancesonmypage = :allowmultipleinstancesonmypage, ");
			sqlCommand.Append("icon = :icon, ");
			sqlCommand.Append("hidefromunauth = :hidefromunauth, ");
			sqlCommand.Append("hidefromauth = :hidefromauth, ");
			sqlCommand.Append("drafteditroles = :drafteditroles, ");
			sqlCommand.Append("draftapprovalroles = :draftapprovalroles, ");
			sqlCommand.Append("viewroles = :viewroles, ");
			sqlCommand.Append("includeinsearch = :includeinsearch, ");
			sqlCommand.Append("headelement = :headelement, ");
			sqlCommand.Append("publishmode = :publishmode, ");
			sqlCommand.Append("isglobal = :isglobal ");

			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("moduleid = :moduleid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[18];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new NpgsqlParameter(":moduletitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleTitle;

			arParams[2] = new NpgsqlParameter(":authorizededitroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = authorizedEditRoles;

			arParams[3] = new NpgsqlParameter(":cachetime", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = cacheTime;

			arParams[4] = new NpgsqlParameter(":showtitle", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = showTitle;

			arParams[5] = new NpgsqlParameter(":edituserid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = editUserId;

			arParams[6] = new NpgsqlParameter(":availableformypage", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = availableForMyPage;

			arParams[7] = new NpgsqlParameter(":allowmultipleinstancesonmypage", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = allowMultipleInstancesOnMyPage;

			arParams[8] = new NpgsqlParameter(":icon", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = icon;

			arParams[9] = new NpgsqlParameter(":hidefromunauth", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = hideFromUnauthenticated;

			arParams[10] = new NpgsqlParameter(":hidefromauth", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = hideFromAuthenticated;

			arParams[11] = new NpgsqlParameter(":viewroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = viewRoles;

			arParams[12] = new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = draftEditRoles;

			arParams[13] = new NpgsqlParameter(":includeinsearch", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = includeInSearch;

			arParams[14] = new NpgsqlParameter(":isglobal", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = isGlobal;

			arParams[15] = new NpgsqlParameter(":headelement", NpgsqlTypes.NpgsqlDbType.Varchar, 25);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = headElement;

			arParams[16] = new NpgsqlParameter(":publishmode", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = publishMode;

			arParams[17] = new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = draftApprovalRoles;


			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool UpdateModuleOrder(
			int pageId,
			int moduleId,
			int moduleOrder,
			string paneName
		)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[4];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			arParams[2] = new NpgsqlParameter(":moduleorder", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleOrder;

			arParams[3] = new NpgsqlParameter(":panename", NpgsqlTypes.NpgsqlDbType.Text, 50);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = paneName;

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_updatemoduleorder(:pageid,:moduleid,:moduleorder,:panename)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool UpdatePage(
			int oldPageId,
			int newPageId,
			int moduleId
		)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[3];

			arParams[0] = new NpgsqlParameter(":oldpageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = oldPageId;

			arParams[1] = new NpgsqlParameter(":newpageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = newPageId;

			arParams[2] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleId;

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_updatepage(:oldpageid,:newpageid,:moduleid)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool Publish(
			Guid pageGuid,
			Guid moduleGuid,
			int moduleId,
			int pageId,
			string paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate
		)
		{
			if (PageModuleExists(moduleId, pageId))
			{
				return PageModuleUpdate(
					moduleId,
					pageId,
					paneName,
					moduleOrder,
					publishBeginDate,
					publishEndDate
				);
			}
			else
			{
				return PageModuleInsert(
					moduleId,
					pageId,
					paneName,
					moduleOrder,
					publishBeginDate,
					publishEndDate
				);
			}
		}


		public static bool PageModuleExists(int moduleId, int pageId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			int count = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.StoredProcedure,
					"mp_pagemodule_exists(:moduleid,:pageid)",
					arParams
				)
			);

			return count > 0;
		}


		public static DataTable PageModuleGetByModule(int moduleId)
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("PageID", typeof(int));
			dataTable.Columns.Add("ModuleID", typeof(int));
			dataTable.Columns.Add("PaneName", typeof(string));
			dataTable.Columns.Add("ModuleOrder", typeof(int));
			dataTable.Columns.Add("PublishBeginDate", typeof(DateTime));
			dataTable.Columns.Add("PublishEndDate", typeof(DateTime));

			using (IDataReader reader = PageModuleGetReaderByModule(moduleId))
			{
				while (reader.Read())
				{
					DataRow row = dataTable.NewRow();

					row["PageID"] = reader["PageID"];
					row["ModuleID"] = reader["ModuleID"];
					row["PaneName"] = reader["PaneName"];
					row["ModuleOrder"] = reader["ModuleOrder"];
					row["PublishBeginDate"] = reader["PublishBeginDate"];
					row["PublishEndDate"] = reader["PublishEndDate"];

					dataTable.Rows.Add(row);
				}
			}

			return dataTable;
		}


		public static IDataReader PageModuleGetReaderByModule(int moduleId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT  pm.*, ");
			sqlCommand.Append("m.moduletitle, ");
			sqlCommand.Append("p.pagename, ");
			sqlCommand.Append("p.useurl, ");
			sqlCommand.Append("p.url ");

			sqlCommand.Append("FROM	mp_pagemodules pm ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_modules m ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.moduleid = m.moduleid ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_pages p ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.pageid = p.pageid ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("pm.moduleid = :moduleid ;");

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader PageModuleGetReaderByPage(int pageId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT  pm.*, ");
			sqlCommand.Append("m.moduletitle, ");
			sqlCommand.Append("p.pagename, ");
			sqlCommand.Append("p.useurl, ");
			sqlCommand.Append("p.url ");

			sqlCommand.Append("FROM	mp_pagemodules pm ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_modules m ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.moduleid = m.moduleid ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_pages p ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.pageid = p.pageid ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("pm.pageid = :pageid ;");

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static bool PageModuleInsert(
			int moduleId,
			int pageId,
			string paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate
		)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[6];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			arParams[2] = new NpgsqlParameter(":moduleorder", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleOrder;

			arParams[3] = new NpgsqlParameter(":panename", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = paneName;

			arParams[4] = new NpgsqlParameter(":publishbegindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = publishBeginDate;

			arParams[5] = new NpgsqlParameter(":publishenddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[5].Direction = ParameterDirection.Input;

			if (publishEndDate > DateTime.MinValue)
			{
				arParams[5].Value = publishEndDate;
			}
			else
			{
				arParams[5].Value = DBNull.Value;
			}

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_pagemodules_insert(:pageid,:moduleid,:moduleorder,:panename,:publishbegindate,:publishenddate)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool PageModuleUpdate(
			int moduleId,
			int pageId,
			string paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate
		)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[6];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			arParams[2] = new NpgsqlParameter(":moduleorder", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleOrder;

			arParams[3] = new NpgsqlParameter(":panename", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = paneName;

			arParams[4] = new NpgsqlParameter(":publishbegindate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = publishBeginDate;

			arParams[5] = new NpgsqlParameter(":publishenddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[5].Direction = ParameterDirection.Input;

			if (publishEndDate > DateTime.MinValue)
			{
				arParams[5].Value = publishEndDate;
			}
			else
			{
				arParams[5].Value = DateTime.UtcNow;
			}

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_pagemodules ");
			sqlCommand.Append("SET ");
			sqlCommand.Append("panename = :panename, ");
			sqlCommand.Append("moduleorder = :moduleorder, ");

			if (publishEndDate > DateTime.MinValue)
			{
				sqlCommand.Append("publishbegindate = :publishbegindate, ");
				sqlCommand.Append("publishenddate = :publishenddate ");
			}
			else
			{
				sqlCommand.Append("publishbegindate = :publishbegindate ");
			}

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduleid = :moduleid ");
			sqlCommand.Append("AND ");
			sqlCommand.Append("pageid = :pageid;");

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool PageModuleDeleteByPage(int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("DELETE FROM mp_pagemodules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("pageid = :pageid");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new NpgsqlParameter(":increment", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = increment;

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_updatecountofuseonmypage(:moduleid,:increment)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool DeleteModule(int moduleId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_delete(:moduleid)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static bool DeleteModuleInstance(int moduleId, int pageId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			int rowsAffected = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_deleteinstance(:moduleid,:pageid)",
					arParams
				)
			);

			return rowsAffected > -1;
		}


		public static IDataReader GetModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT  ");
			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("md.controlsrc,  ");
			sqlCommand.Append("md.featurename  ");

			sqlCommand.Append("FROM	mp_modules m ");

			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.moduleid = :moduleid;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetModule(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT  ");
			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("md.controlsrc,  ");
			sqlCommand.Append("md.featurename  ");

			sqlCommand.Append("FROM	mp_modules m ");

			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.guid = :moduleguid;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleGuid.ToString();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetModule(int moduleId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT  ");
			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("pm.pageid,  ");
			sqlCommand.Append("pm.moduleorder,  ");
			sqlCommand.Append("pm.panename,  ");
			sqlCommand.Append("pm.publishbegindate,  ");
			sqlCommand.Append("pm.publishenddate,  ");
			sqlCommand.Append("md.controlsrc,  ");
			sqlCommand.Append("md.featurename  ");

			sqlCommand.Append("FROM	mp_modules m ");

			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

			sqlCommand.Append("JOIN mp_pagemodules pm ");
			sqlCommand.Append("ON m.moduleid = pm.moduleid ");

			sqlCommand.Append("WHERE pm.pageid = :pageid ");
			sqlCommand.Append(" AND m.moduleid = :moduleid;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetPageModules(int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT   ");
			sqlCommand.Append("m.*, ");
			sqlCommand.Append("pm.pageid,   ");
			sqlCommand.Append("pm.moduleorder,   ");
			sqlCommand.Append("pm.panename,   ");
			sqlCommand.Append("pm.publishbeginDate,   ");
			sqlCommand.Append("pm.publishendDate,   ");
			sqlCommand.Append("md.controlsrc, ");
			sqlCommand.Append("md.featurename, ");
			sqlCommand.Append("md.guid AS featureguid ");

			sqlCommand.Append("FROM	mp_modules m ");

			sqlCommand.Append("JOIN	mp_moduledefinitions md ");
			sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

			sqlCommand.Append("JOIN	mp_pagemodules pm ");
			sqlCommand.Append("ON m.moduleid = pm.moduleid ");

			sqlCommand.Append("WHERE pm.pageid = :pageid ");

			sqlCommand.Append("AND pm.publishbegindate <= :nowdate ");
			sqlCommand.Append("AND (pm.publishenddate IS NULL OR pm.publishenddate > :nowdate) ");


			sqlCommand.Append("ORDER BY pm.moduleorder;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new NpgsqlParameter(":nowdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = DateTime.UtcNow;

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static IDataReader GetMyPageModules(int siteId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_modules_selectformypage(:siteid)",
				arParams
			);
		}


		public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT   ");
			sqlCommand.Append("m.moduleid, ");
			sqlCommand.Append("m.moduletitle, ");
			sqlCommand.Append("m.authorizededitroles, ");
			sqlCommand.Append("m.edituserid, ");
			sqlCommand.Append("p.url, ");
			sqlCommand.Append("p.pagename, ");
			sqlCommand.Append("p.useurl, ");
			sqlCommand.Append("p.pageid, ");
			sqlCommand.Append("p.editroles ");

			sqlCommand.Append("FROM mp_modules m ");

			sqlCommand.Append("JOIN mp_pagemodules pm ");
			sqlCommand.Append("ON m.moduleid = pm.moduleid ");

			sqlCommand.Append("JOIN mp_pages p ");
			sqlCommand.Append("ON pm.pageid = p.pageid ");

			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

			sqlCommand.Append("WHERE md.guid = :featureguid ");
			sqlCommand.Append("AND m.siteid = :siteid ");

			sqlCommand.Append("ORDER BY p.pagename, m.moduletitle;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = featureGuid.ToString();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		public static int CountNonAdminModules(int siteId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int count = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.StoredProcedure,
					"mp_modules_countnonadmin(:siteid)",
					arParams
				)
			);

			return count;
		}


		public static int GetCount(
			int siteId,
			int moduleDefId,
			string title
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_modules m  ");
			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON md.moduledefid = m.moduledefid ");
			sqlCommand.Append("WHERE m.siteid = :siteid ");
			sqlCommand.Append("AND ((m.moduledefid = :moduledefid) OR (:moduledefid = -1)) ");

			if (title.Length > 0)
			{
				sqlCommand.Append("AND (m.moduletitle LIKE '%' || :title || '%') ");
			}

			sqlCommand.Append("AND md.isadmin = false;");

			NpgsqlParameter[] arParams = new NpgsqlParameter[3];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = title;

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			);
		}


		public static DataTable SelectPage(
			int siteId,
			int moduleDefId,
			string title,
			int pageNumber,
			int pageSize,
			bool sortByModuleType,
			bool sortByAuthor,
			out int totalPages
		)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;

			totalPages = 1;

			int totalRows = GetCount(siteId, moduleDefId, title);

			if (pageSize > 0)
			{
				totalPages = totalRows / pageSize;
			}

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;

				Math.DivRem(totalRows, pageSize, out remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			int offset = pageSize * (pageNumber - 1);

			NpgsqlParameter[] arParams = new NpgsqlParameter[6];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = "%" + title + "%";

			arParams[3] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = pageNumber;

			arParams[4] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = pageSize;

			arParams[5] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = offset;

			DataTable dt = new DataTable();

			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(string));
			dt.Columns.Add("FeatureName", typeof(string));
			dt.Columns.Add("ResourceFile", typeof(string));
			dt.Columns.Add("ControlSrc", typeof(string));
			dt.Columns.Add("AuthorizedEditRoles", typeof(string));
			dt.Columns.Add("CreatedBy", typeof(string));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT ");
			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("md.featurename,  ");
			sqlCommand.Append("md.controlsrc,  ");
			sqlCommand.Append("md.resourcefile, ");
			sqlCommand.Append("COALESCE(u.name, '') As createdby,  ");
			sqlCommand.Append("(SELECT COUNT(pm.pageid) FROM mp_pagemodules pm WHERE pm.moduleid = m.moduleid) AS useCount  ");

			sqlCommand.Append("FROM	mp_modules m  ");
			sqlCommand.Append("JOIN	mp_moduledefinitions md  ");
			sqlCommand.Append("ON md.moduledefid = m.moduledefid  ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_users u  ");
			sqlCommand.Append("ON m.createdbyuserid = u.userid  ");

			sqlCommand.Append("WHERE m.siteid = :siteid ");
			sqlCommand.Append("AND ((m.moduledefid = :moduledefid) OR (:moduledefid = -1)) ");

			if (title.Length > 0)
			{
				sqlCommand.Append("AND (m.moduletitle LIKE  :title ) ");
			}

			sqlCommand.Append("AND md.IsAdmin = false ");

			if (sortByModuleType)
			{
				sqlCommand.Append("ORDER BY md.featurename, m.moduletitle ");
			}
			else if (sortByAuthor)
			{
				sqlCommand.Append("ORDER BY u.name, m.moduleTitle ");
			}
			else
			{
				sqlCommand.Append("   ORDER BY 	m.moduletitle, md.featurename ");
			}

			sqlCommand.Append("LIMIT  :pagesize");

			if (pageNumber > 1)
			{
				sqlCommand.Append(" OFFSET :pageoffset ");
			}

			sqlCommand.Append(";");

			using (IDataReader reader = NpgsqlHelper.ExecuteReader(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			)
			{
				while (reader.Read())
				{
					DataRow row = dt.NewRow();

					row["ModuleID"] = reader["ModuleID"];
					row["ModuleTitle"] = reader["ModuleTitle"];
					row["FeatureName"] = reader["FeatureName"];
					row["ResourceFile"] = reader["ResourceFile"];
					row["ControlSrc"] = reader["ControlSrc"];
					row["AuthorizedEditRoles"] = reader["AuthorizedEditRoles"];
					row["CreatedBy"] = reader["CreatedBy"];
					row["CreatedDate"] = reader["CreatedDate"];
					row["UseCount"] = reader["UseCount"];

					dt.Rows.Add(row);
				}
			}

			return dt;
		}


		public static int GetCountByFeature(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_modules   ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduledefid = :moduledefid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			);
		}


		public static int GetGlobalCount(
			int siteId,
			int moduleDefId,
			int pageId
		)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_modules m  ");
			sqlCommand.Append("JOIN mp_moduledefinitions md ");
			sqlCommand.Append("ON md.moduledefid = m.moduledefid ");
			sqlCommand.Append("WHERE m.siteid = :siteid ");
			sqlCommand.Append("AND ((m.moduledefid = :moduledefid) OR (:moduledefid = -1)) ");

			sqlCommand.Append("AND m.isglobal = true ");
			sqlCommand.Append("AND m.moduleid NOT IN (SELECT moduleid FROM mp_pagemodules WHERE pageid = :pageid) ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[3];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = pageId;

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			);
		}


		public static DataTable SelectGlobalPage(
			int siteId,
			int moduleDefId,
			int pageId,
			int pageNumber,
			int pageSize,
			out int totalPages
		)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;

			totalPages = 1;

			int totalRows = GetGlobalCount(siteId, moduleDefId, pageId);

			if (pageSize > 0)
			{
				totalPages = totalRows / pageSize;
			}

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;

				Math.DivRem(totalRows, pageSize, out remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			int offset = pageSize * (pageNumber - 1);

			NpgsqlParameter[] arParams = new NpgsqlParameter[6];

			arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new NpgsqlParameter(":moduledefid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = pageId;

			arParams[3] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = pageNumber;

			arParams[4] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = pageSize;

			arParams[5] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = offset;

			DataTable dt = new DataTable();

			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(string));
			dt.Columns.Add("FeatureName", typeof(string));
			dt.Columns.Add("ResourceFile", typeof(string));
			dt.Columns.Add("ControlSrc", typeof(string));
			dt.Columns.Add("AuthorizedEditRoles", typeof(string));
			dt.Columns.Add("CreatedBy", typeof(string));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT ");
			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("md.featurename,  ");
			sqlCommand.Append("md.controlsrc,  ");
			sqlCommand.Append("md.resourcefile, ");
			sqlCommand.Append("COALESCE(u.name, '') As createdby,  ");
			sqlCommand.Append("(SELECT COUNT(pm.pageid) FROM mp_pagemodules pm WHERE pm.moduleid = m.moduleid) AS useCount  ");

			sqlCommand.Append("FROM	mp_modules m  ");
			sqlCommand.Append("JOIN	mp_moduledefinitions md  ");
			sqlCommand.Append("ON md.moduledefid = m.moduledefid  ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_users u  ");
			sqlCommand.Append("ON m.createdbyuserid = u.userid  ");

			sqlCommand.Append("WHERE m.siteid = :siteid ");
			sqlCommand.Append("AND ((m.moduledefid = :moduledefid) OR (:moduledefid = -1)) ");

			sqlCommand.Append("AND m.isglobal = true ");
			sqlCommand.Append("AND m.moduleid NOT IN (SELECT moduleid FROM mp_pagemodules WHERE pageid = :pageid) ");


			sqlCommand.Append("   ORDER BY 	m.moduletitle, md.featurename ");

			sqlCommand.Append("LIMIT  :pagesize");

			if (pageNumber > 1)
			{
				sqlCommand.Append(" OFFSET :pageoffset ");
			}

			sqlCommand.Append(";");

			using (IDataReader reader = NpgsqlHelper.ExecuteReader(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			)
			{
				while (reader.Read())
				{
					DataRow row = dt.NewRow();

					row["ModuleID"] = reader["ModuleID"];
					row["ModuleTitle"] = reader["ModuleTitle"];
					row["FeatureName"] = reader["FeatureName"];
					row["ResourceFile"] = reader["ResourceFile"];
					row["ControlSrc"] = reader["ControlSrc"];
					row["AuthorizedEditRoles"] = reader["AuthorizedEditRoles"];
					row["CreatedBy"] = reader["CreatedBy"];
					row["CreatedDate"] = reader["CreatedDate"];
					row["UseCount"] = reader["UseCount"];

					dt.Rows.Add(row);
				}
			}

			return dt;
		}


		public static IDataReader GetGlobalContent(int siteId)
		{
			var commandText = @"
SELECT m.*,
	md.FeatureName,
	md.ControlSrc,
	md.ResourceFile,
	u.Name As CreatedBy,
	u.UserID AS CreatedById,
	(
		SELECT COUNT(pm.PageID)
		FROM mp_PageModules pm
		WHERE pm.ModuleID = m.ModuleID
	) AS UseCount
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
LEFT OUTER JOIN mp_Users u ON m.CreatedByUserID = u.UserID";

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				commandText
			);
		}
	}
}
