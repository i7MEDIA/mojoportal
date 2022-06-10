using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
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
			int publishMode)
		{
			// this is an unsigned int in the table
			// and this is a hack to fix a setup bug
			// where -1 is assigned when creating initial content
			// better solution is to lookup UserID of Admin user
			// and pass it in instead of -1
			if (createdByUserId < 0) createdByUserId = 0;

			#region Bit Conversion

			int inthideFromAuthenticated;
			if (hideFromAuthenticated)
			{
				inthideFromAuthenticated = 1;
			}
			else
			{
				inthideFromAuthenticated = 0;
			}

			int inthideFromUnauthenticated;
			if (hideFromUnauthenticated)
			{
				inthideFromUnauthenticated = 1;
			}
			else
			{
				inthideFromUnauthenticated = 0;
			}

			int intShowTitle;
			if (showTitle)
			{
				intShowTitle = 1;
			}
			else
			{
				intShowTitle = 0;
			}

			int myAvailable;
			if (availableForMyPage)
			{
				myAvailable = 1;
			}
			else
			{
				myAvailable = 0;
			}

			int allowMultiple;
			if (allowMultipleInstancesOnMyPage)
			{
				allowMultiple = 1;
			}
			else
			{
				allowMultiple = 0;
			}

			#endregion

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_Modules (");
			sqlCommand.Append("SiteID, ");
			sqlCommand.Append("ModuleDefID, ");
			sqlCommand.Append("ModuleTitle, ");
			sqlCommand.Append("ViewRoles, ");
			sqlCommand.Append("AuthorizedEditRoles, ");
			sqlCommand.Append("DraftEditRoles, ");
			sqlCommand.Append("DraftApprovalRoles, ");
			sqlCommand.Append("CacheTime, ");
			sqlCommand.Append("ShowTitle, ");
			sqlCommand.Append("AvailableForMyPage, ");
			sqlCommand.Append("AllowMultipleInstancesOnMyPage, ");
			sqlCommand.Append("Icon, ");
			sqlCommand.Append("CreatedByUserID, ");
			sqlCommand.Append("CreatedDate, ");
			sqlCommand.Append("CountOfUseOnMyPage, ");
			sqlCommand.Append("Guid, ");
			sqlCommand.Append("FeatureGuid, ");
			sqlCommand.Append("HideFromAuth, ");
			sqlCommand.Append("HideFromUnAuth, ");
			sqlCommand.Append("IncludeInSearch, ");
			sqlCommand.Append("IsGlobal, ");
			sqlCommand.Append("PublishMode, ");
			sqlCommand.Append("HeadElement, ");
			sqlCommand.Append("SiteGuid ");
			sqlCommand.Append(" )");

			sqlCommand.Append(" VALUES (");
			sqlCommand.Append("?SiteID, ");
			sqlCommand.Append("?ModuleDefID, ");
			sqlCommand.Append("?ModuleTitle, ");
			sqlCommand.Append("?ViewRoles, ");
			sqlCommand.Append("?AuthorizedEditRoles, ");
			sqlCommand.Append("?DraftEditRoles, ");
			sqlCommand.Append("?DraftApprovalRoles, ");
			sqlCommand.Append("?CacheTime, ");
			sqlCommand.Append("?ShowTitle, ");
			sqlCommand.Append("?AvailableForMyPage, ");
			sqlCommand.Append("?AllowMultipleInstancesOnMyPage, ");
			sqlCommand.Append("?Icon, ");
			sqlCommand.Append("?CreatedByUserID, ");
			sqlCommand.Append("?CreatedDate, ");
			sqlCommand.Append("0, ");
			sqlCommand.Append("?Guid, ");
			sqlCommand.Append("?FeatureGuid, ");
			sqlCommand.Append("?HideFromAuth, ");
			sqlCommand.Append("?HideFromUnAuth, ");
			sqlCommand.Append("1, ");
			sqlCommand.Append("0, ");
			sqlCommand.Append("?PublishMode, ");
			sqlCommand.Append("?HeadElement, ");
			sqlCommand.Append("?SiteGuid ");
			sqlCommand.Append(" )");
			sqlCommand.Append(";");

			sqlCommand.Append("SELECT LAST_INSERT_ID();");

			MySqlParameter[] arParams = new MySqlParameter[21];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?ModuleTitle", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleTitle;

			arParams[3] = new MySqlParameter("?AuthorizedEditRoles", MySqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = authorizedEditRoles;

			arParams[4] = new MySqlParameter("?CacheTime", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = cacheTime;

			arParams[5] = new MySqlParameter("?ShowTitle", MySqlDbType.Int32);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = intShowTitle;

			arParams[6] = new MySqlParameter("?AvailableForMyPage", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = myAvailable;

			arParams[7] = new MySqlParameter("?CreatedByUserID", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = createdByUserId;

			arParams[8] = new MySqlParameter("?CreatedDate", MySqlDbType.DateTime);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = createdDate;

			arParams[9] = new MySqlParameter("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = allowMultiple;

			arParams[10] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = icon;

			arParams[11] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = guid.ToString();

			arParams[12] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = featureGuid.ToString();

			arParams[13] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = siteGuid.ToString();

			arParams[14] = new MySqlParameter("?HideFromAuth", MySqlDbType.Int32);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = inthideFromAuthenticated;

			arParams[15] = new MySqlParameter("?HideFromUnAuth", MySqlDbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = inthideFromUnauthenticated;

			arParams[16] = new MySqlParameter("?ViewRoles", MySqlDbType.Text);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = viewRoles;

			arParams[17] = new MySqlParameter("?DraftEditRoles", MySqlDbType.Text);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = draftEditRoles;

			arParams[18] = new MySqlParameter("?HeadElement", MySqlDbType.VarChar, 25);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = headElement;

			arParams[19] = new MySqlParameter("?PublishMode", MySqlDbType.Int32);
			arParams[19].Direction = ParameterDirection.Input;
			arParams[19].Value = publishMode;

			arParams[20] = new MySqlParameter("?DraftApprovalRoles", MySqlDbType.Text);
			arParams[20].Direction = ParameterDirection.Input;
			arParams[20].Value = draftApprovalRoles;


			int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());

			if ((newID > -1) && (pageId > -1))
			{
				sqlCommand = new StringBuilder();
				sqlCommand.Append("INSERT INTO mp_PageModules (");
				sqlCommand.Append("PageID, ");
				sqlCommand.Append("ModuleID, ");

				sqlCommand.Append("PageGuid, ");
				sqlCommand.Append("ModuleGuid, ");

				sqlCommand.Append("ModuleOrder, ");
				sqlCommand.Append("PaneName, ");
				sqlCommand.Append("PublishBeginDate ");

				sqlCommand.Append(") ");

				sqlCommand.Append(" VALUES (");
				sqlCommand.Append("?PageID, ");
				sqlCommand.Append("?ModuleID, ");

				sqlCommand.Append("(SELECT PageGuid FROM mp_Pages WHERE PageID = ?PageID LIMIT 1), ");
				sqlCommand.Append("(SELECT Guid FROM mp_Modules WHERE ModuleID = ?ModuleID LIMIT 1), ");

				sqlCommand.Append("?ModuleOrder, ");
				sqlCommand.Append("?PaneName, ");
				sqlCommand.Append("?PublishBeginDate ");

				sqlCommand.Append("); ");

				arParams = new MySqlParameter[5];

				arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
				arParams[0].Direction = ParameterDirection.Input;
				arParams[0].Value = pageId;

				arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
				arParams[1].Direction = ParameterDirection.Input;
				arParams[1].Value = newID;

				arParams[2] = new MySqlParameter("?ModuleOrder", MySqlDbType.Int32);
				arParams[2].Direction = ParameterDirection.Input;
				arParams[2].Value = moduleOrder;

				arParams[3] = new MySqlParameter("?PaneName", MySqlDbType.VarChar, 50);
				arParams[3].Direction = ParameterDirection.Input;
				arParams[3].Value = paneName;

				arParams[4] = new MySqlParameter("?PublishBeginDate", MySqlDbType.DateTime);
				arParams[4].Direction = ParameterDirection.Input;
				arParams[4].Value = DateTime.UtcNow.AddMinutes(-30);

				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand.ToString(),
					arParams);

			}

			return newID;

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
			int publishMode)
		{

			#region Bit Conversion

			int inthideFromAuthenticated;
			if (hideFromAuthenticated)
			{
				inthideFromAuthenticated = 1;
			}
			else
			{
				inthideFromAuthenticated = 0;
			}

			int inthideFromUnauthenticated;
			if (hideFromUnauthenticated)
			{
				inthideFromUnauthenticated = 1;
			}
			else
			{
				inthideFromUnauthenticated = 0;
			}

			int intShowTitle;
			if (showTitle)
			{
				intShowTitle = 1;
			}
			else
			{
				intShowTitle = 0;
			}

			int myAvailable;
			if (availableForMyPage)
			{
				myAvailable = 1;
			}
			else
			{
				myAvailable = 0;
			}

			int allowMultiple;
			if (allowMultipleInstancesOnMyPage)
			{
				allowMultiple = 1;
			}
			else
			{
				allowMultiple = 0;
			}

			int intIncludeInSearch = 1;
			if (!includeInSearch)
			{
				intIncludeInSearch = 0;
			}

			int intIsGlobal = 0;
			if (isGlobal)
			{
				intIsGlobal = 1;
			}

			#endregion

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_Modules ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID, ");
			sqlCommand.Append("ModuleTitle = ?ModuleTitle, ");
			sqlCommand.Append("ViewRoles = ?ViewRoles, ");
			sqlCommand.Append("AuthorizedEditRoles = ?AuthorizedEditRoles, ");
			sqlCommand.Append("DraftEditRoles = ?DraftEditRoles, ");
			sqlCommand.Append("DraftApprovalRoles = ?DraftApprovalRoles, ");
			sqlCommand.Append("CacheTime = ?CacheTime, ");
			sqlCommand.Append("ShowTitle = ?ShowTitle, ");
			sqlCommand.Append("EditUserID = ?EditUserID, ");
			sqlCommand.Append("HideFromAuth = ?HideFromAuth, ");
			sqlCommand.Append("HideFromUnAuth = ?HideFromUnAuth, ");
			sqlCommand.Append("AvailableForMyPage = ?AvailableForMyPage, ");
			sqlCommand.Append("AllowMultipleInstancesOnMyPage = ?AllowMultipleInstancesOnMyPage, ");
			sqlCommand.Append("IncludeInSearch = ?IncludeInSearch, ");
			sqlCommand.Append("IsGlobal = ?IsGlobal, ");
			sqlCommand.Append("PublishMode = ?PublishMode, ");
			sqlCommand.Append("HeadElement = ?HeadElement, ");
			sqlCommand.Append("Icon = ?Icon ");

			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("ModuleID = ?ModuleID ;");

			MySqlParameter[] arParams = new MySqlParameter[19];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?ModuleTitle", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleTitle;

			arParams[3] = new MySqlParameter("?AuthorizedEditRoles", MySqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = authorizedEditRoles;

			arParams[4] = new MySqlParameter("?CacheTime", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = cacheTime;

			arParams[5] = new MySqlParameter("?ShowTitle", MySqlDbType.Int32);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = intShowTitle;

			arParams[6] = new MySqlParameter("?EditUserID", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = editUserId;

			arParams[7] = new MySqlParameter("?AvailableForMyPage", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = myAvailable;

			arParams[8] = new MySqlParameter("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = allowMultiple;

			arParams[9] = new MySqlParameter("?Icon", MySqlDbType.VarChar, 255);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = icon;

			arParams[10] = new MySqlParameter("?HideFromAuth", MySqlDbType.Int32);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = inthideFromAuthenticated;

			arParams[11] = new MySqlParameter("?HideFromUnAuth", MySqlDbType.Int32);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = inthideFromUnauthenticated;

			arParams[12] = new MySqlParameter("?ViewRoles", MySqlDbType.Text);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = viewRoles;

			arParams[13] = new MySqlParameter("?DraftEditRoles", MySqlDbType.Text);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = draftEditRoles;

			arParams[14] = new MySqlParameter("?IncludeInSearch", MySqlDbType.Int32);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = intIncludeInSearch;

			arParams[15] = new MySqlParameter("?IsGlobal", MySqlDbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = intIsGlobal;

			arParams[16] = new MySqlParameter("?HeadElement", MySqlDbType.VarChar, 25);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = headElement;

			arParams[17] = new MySqlParameter("?PublishMode", MySqlDbType.Int32);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = publishMode;

			arParams[18] = new MySqlParameter("?DraftApprovalRoles", MySqlDbType.Text);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = draftApprovalRoles;


			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool UpdateModuleOrder(
			int pageId,
			int moduleId,
			int moduleOrder,
			string paneName)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_PageModules ");
			sqlCommand.Append("SET ModuleOrder = ?ModuleOrder , ");
			sqlCommand.Append("PaneName = ?PaneName  ");
			sqlCommand.Append("WHERE ModuleID = ?ModuleID AND PageID = ?PageID ; ");

			MySqlParameter[] arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleId;

			arParams[2] = new MySqlParameter("?ModuleOrder", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleOrder;

			arParams[3] = new MySqlParameter("?PaneName", MySqlDbType.VarChar, 50);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = paneName;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}


		public static bool DeleteModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("DELETE FROM mp_PageModules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleID = ?ModuleID ;");

			sqlCommand.Append("DELETE FROM mp_Modules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleID = ?ModuleID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteModuleInstance(int moduleId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_PageModules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleID = ?ModuleID AND PageID = ?PageID ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool PageModuleDeleteByPage(int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_PageModules ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("PageID = ?PageID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool PageModuleExists(int moduleId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM	mp_PageModules ");
			sqlCommand.Append("WHERE ModuleID = ?ModuleID AND PageID = ?PageID ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

			return (count > 0);

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
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  pm.*, ");
			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("p.PageName, ");
			sqlCommand.Append("p.UseUrl, ");
			sqlCommand.Append("p.Url ");

			sqlCommand.Append("FROM	mp_PageModules pm ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_Modules m ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_Pages p ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.PageID = p.PageID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("pm.ModuleID = ?ModuleID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);
		}

		public static IDataReader PageModuleGetReaderByPage(int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  pm.*, ");
			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("p.PageName, ");
			sqlCommand.Append("p.UseUrl, ");
			sqlCommand.Append("p.Url ");

			sqlCommand.Append("FROM	mp_PageModules pm ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_Modules m ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN ");
			sqlCommand.Append("mp_Pages p ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("pm.PageID = p.PageID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("pm.PageID = ?PageID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);
		}

		public static bool Publish(
			Guid pageGuid,
			Guid moduleGuid,
			int moduleId,
			int pageId,
			String paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate)
		{
			if (PageModuleExists(moduleId, pageId))
			{
				return PageModuleUpdate(
					moduleId,
					pageId,
					paneName,
					moduleOrder,
					publishBeginDate,
					publishEndDate);
			}
			else
			{
				return PageModuleInsert(
					pageGuid,
					moduleGuid,
					moduleId,
					pageId,
					paneName,
					moduleOrder,
					publishBeginDate,
					publishEndDate);

			}

		}

		public static bool PageModuleInsert(
			Guid pageGuid,
			Guid moduleGuid,
			int moduleId,
			int pageId,
			String paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_PageModules (");

			sqlCommand.Append("PageGuid, ");
			sqlCommand.Append("ModuleGuid, ");
			sqlCommand.Append("ModuleID, ");
			sqlCommand.Append("PageID, ");

			sqlCommand.Append("PaneName, ");
			sqlCommand.Append("ModuleOrder, ");
			sqlCommand.Append("PublishBeginDate, ");
			sqlCommand.Append("PublishEndDate ");
			sqlCommand.Append(") ");

			sqlCommand.Append(" VALUES (");
			sqlCommand.Append("?PageGuid, ");
			sqlCommand.Append("?ModuleGuid, ");

			sqlCommand.Append("?ModuleID, ");
			sqlCommand.Append("?PageID, ");
			sqlCommand.Append("?PaneName, ");
			sqlCommand.Append("?ModuleOrder, ");
			sqlCommand.Append("?PublishBeginDate, ");
			sqlCommand.Append("?PublishEndDate ");

			sqlCommand.Append(") ;");

			MySqlParameter[] arParams = new MySqlParameter[8];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			arParams[2] = new MySqlParameter("?PaneName", MySqlDbType.VarChar, 50);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = paneName;

			arParams[3] = new MySqlParameter("?ModuleOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = moduleOrder;

			arParams[4] = new MySqlParameter("?PublishBeginDate", MySqlDbType.DateTime);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = publishBeginDate;

			arParams[5] = new MySqlParameter("?PublishEndDate", MySqlDbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;

			if (publishEndDate != DateTime.MinValue)
			{
				arParams[5].Value = publishEndDate;
			}
			else
			{
				arParams[5].Value = DBNull.Value;
			}

			arParams[6] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = pageGuid.ToString();

			arParams[7] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = moduleGuid.ToString();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool PageModuleUpdate(
			int moduleId,
			int pageId,
			String paneName,
			int moduleOrder,
			DateTime publishBeginDate,
			DateTime publishEndDate)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_PageModules ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("PaneName = ?PaneName, ");
			sqlCommand.Append("ModuleOrder = ?ModuleOrder, ");
			sqlCommand.Append("PublishBeginDate = ?PublishBeginDate, ");
			sqlCommand.Append("PublishEndDate = ?PublishEndDate ");
			sqlCommand.Append("WHERE ModuleID = ?ModuleID AND PageID = ?PageID ; ");

			MySqlParameter[] arParams = new MySqlParameter[6];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			arParams[2] = new MySqlParameter("?PaneName", MySqlDbType.VarChar, 50);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = paneName;

			arParams[3] = new MySqlParameter("?ModuleOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = moduleOrder;

			arParams[4] = new MySqlParameter("?PublishBeginDate", MySqlDbType.DateTime);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = publishBeginDate;

			arParams[5] = new MySqlParameter("?PublishEndDate", MySqlDbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;
			if (publishEndDate != DateTime.MinValue)
			{
				arParams[5].Value = publishEndDate;
			}
			else
			{
				arParams[5].Value = DBNull.Value;
			}

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Modules ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("CountOfUseOnMyPage = CountOfUseOnMyPage + ?Increment ");

			sqlCommand.Append("WHERE ModuleID = ?ModuleID  ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?Increment", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = increment;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_PageModules ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("PageID = ?NewPageID, ");
			sqlCommand.Append("PageGuid = (SELECT PageGuid FROM mp_Pages WHERE PageID = ?NewPageID LIMIT 1) ");

			sqlCommand.Append("WHERE ModuleID = ?ModuleID AND PageID = ?PageID ; ");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = oldPageId;

			arParams[2] = new MySqlParameter("?NewPageID", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = newPageId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static int CountNonAdminModules(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
			sqlCommand.Append("WHERE m.SiteID = ?SiteID ");
			sqlCommand.Append("AND md.IsAdmin = 0 ");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

			return count;

		}

		public static int CountForMyPage(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("WHERE m.SiteID = ?SiteID AND m.AvailableForMyPage = 1 ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

			return count;

		}

		public static int GetCount(int siteId, int moduleDefId, string title)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
			sqlCommand.Append("WHERE m.SiteID = ?SiteID ");
			sqlCommand.Append("AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) ");
			if (title.Length > 0)
			{
				sqlCommand.Append("AND (m.ModuleTitle LIKE CONCAT('%', ?Title,'%')) ");
			}
			sqlCommand.Append("AND md.IsAdmin = 0 ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = title;

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

		}

		public static int GetCountByFeature(int moduleDefId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_Modules   ");
			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("ModuleDefID = ?ModuleDefID ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleDefId;

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));
		}

		public static DataTable SelectPage(
			int siteId,
			int moduleDefId,
			string title,
			int pageNumber,
			int pageSize,
			bool sortByModuleType,
			bool sortByAuthor,
			out int totalPages)
		{

			int pageLowerBound = (pageSize * pageNumber) - pageSize;
			totalPages = 1;
			int totalRows = GetCount(siteId, moduleDefId, title);

			if (pageSize > 0) totalPages = totalRows / pageSize;

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

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT m.*,  ");
			sqlCommand.Append("md.FeatureName, ");
			sqlCommand.Append("md.ControlSrc, ");
			sqlCommand.Append("md.ResourceFile, ");
			sqlCommand.Append("u.Name As CreatedBy, ");
			sqlCommand.Append("(SELECT COUNT(pm.PageID) FROM mp_PageModules pm WHERE pm.ModuleID = m.ModuleID) AS UseCount  ");

			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

			sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
			sqlCommand.Append("ON m.CreatedByUserID = u.UserID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.SiteID = ?SiteID ");
			sqlCommand.Append("AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) ");
			if (title.Length > 0)
			{
				sqlCommand.Append("AND (m.ModuleTitle LIKE CONCAT('%',?Title,'%')) ");
			}
			sqlCommand.Append("AND md.IsAdmin = 0 ");

			if (sortByModuleType)
			{
				sqlCommand.Append("ORDER BY md.FeatureName, m.ModuleTitle ");

			}
			else if (sortByAuthor)
			{
				sqlCommand.Append("ORDER BY u.Name, m.ModuleTitle ");

			}
			else
			{
				sqlCommand.Append("ORDER BY m.ModuleTitle, md.FeatureName ");

			}

			if (pageNumber > 1)
			{
				sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
					+ ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}
			else
			{
				sqlCommand.Append("LIMIT "
					+ pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}

			sqlCommand.Append(" ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = title;


			DataTable dt = new DataTable();
			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(String));
			dt.Columns.Add("FeatureName", typeof(String));
			dt.Columns.Add("ResourceFile", typeof(String));
			dt.Columns.Add("ControlSrc", typeof(String));
			dt.Columns.Add("AuthorizedEditRoles", typeof(String));
			dt.Columns.Add("CreatedBy", typeof(String));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
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

		public static int GetGlobalCount(int siteId, int moduleDefId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
			sqlCommand.Append("WHERE m.SiteID = ?SiteID ");
			sqlCommand.Append("AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) ");

			sqlCommand.Append("AND m.IsGlobal = 1 ");
			sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = ?PageID) ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = pageId;

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

		}

		public static DataTable SelectGlobalPage(
			int siteId,
			int moduleDefId,
			int pageId,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;
			totalPages = 1;
			int totalRows = GetGlobalCount(siteId, moduleDefId, pageId);

			if (pageSize > 0) totalPages = totalRows / pageSize;

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

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT m.*,  ");
			sqlCommand.Append("md.FeatureName, ");
			sqlCommand.Append("md.ControlSrc, ");
			sqlCommand.Append("md.ResourceFile, ");
			sqlCommand.Append("u.Name As CreatedBy, ");
			sqlCommand.Append("(SELECT COUNT(pm.PageID) FROM mp_PageModules pm WHERE pm.ModuleID = m.ModuleID) AS UseCount  ");

			sqlCommand.Append("FROM mp_Modules m  ");
			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

			sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
			sqlCommand.Append("ON m.CreatedByUserID = u.UserID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.SiteID = ?SiteID ");
			sqlCommand.Append("AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) ");

			sqlCommand.Append("AND m.IsGlobal = 1 ");
			sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = ?PageID) ");


			sqlCommand.Append("ORDER BY m.ModuleTitle, md.FeatureName ");

			if (pageNumber > 1)
			{
				sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
					+ ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}
			else
			{
				sqlCommand.Append("LIMIT "
					+ pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}

			sqlCommand.Append(" ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?ModuleDefID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = moduleDefId;

			arParams[2] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = pageId;


			DataTable dt = new DataTable();
			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(String));
			dt.Columns.Add("FeatureName", typeof(String));
			dt.Columns.Add("ResourceFile", typeof(String));
			dt.Columns.Add("ControlSrc", typeof(String));
			dt.Columns.Add("AuthorizedEditRoles", typeof(String));
			dt.Columns.Add("CreatedBy", typeof(String));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
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


		public static IDataReader GetModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  ");

			sqlCommand.Append("m.*,  ");

			sqlCommand.Append("md.ControlSrc,  ");
			sqlCommand.Append("md.FeatureName  ");

			sqlCommand.Append("FROM	mp_Modules m ");

			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.ModuleID = ?ModuleID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader GetModule(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  ");

			sqlCommand.Append("m.*,  ");

			sqlCommand.Append("md.ControlSrc,  ");
			sqlCommand.Append("md.FeatureName  ");

			sqlCommand.Append("FROM	mp_Modules m ");

			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("m.Guid = ?ModuleGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


		public static IDataReader GetModule(int moduleId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  ");

			sqlCommand.Append("m.*,  ");
			sqlCommand.Append("pm.PageID,  ");
			sqlCommand.Append("pm.ModuleOrder,  ");
			sqlCommand.Append("pm.PaneName,  ");
			sqlCommand.Append("pm.PublishBeginDate,  ");
			sqlCommand.Append("pm.PublishEndDate,  ");
			sqlCommand.Append("md.ControlSrc,  ");
			sqlCommand.Append("md.FeatureName  ");

			sqlCommand.Append("FROM	mp_Modules m ");

			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("JOIN mp_PageModules pm ");
			sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

			sqlCommand.Append("WHERE pm.PageID = ?PageID ");
			sqlCommand.Append(" AND m.ModuleID = ?ModuleID ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader GetPageModules(int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT   ");
			sqlCommand.Append("m.*, ");

			sqlCommand.Append("pm.PageID,   ");
			sqlCommand.Append("pm.ModuleOrder,   ");
			sqlCommand.Append("pm.PaneName,   ");
			sqlCommand.Append("pm.PublishBeginDate,   ");
			sqlCommand.Append("pm.PublishEndDate,   ");
			sqlCommand.Append("md.ControlSrc, ");
			sqlCommand.Append("md.FeatureName, ");
			sqlCommand.Append("md.Guid AS FeatureGuid ");

			sqlCommand.Append("FROM	mp_Modules m ");

			sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("JOIN	mp_PageModules pm ");
			sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

			sqlCommand.Append("WHERE pm.PageID = ?PageID ");

			sqlCommand.Append("AND pm.PublishBeginDate <= ?NowDate ");
			sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > ?NowDate) ");


			sqlCommand.Append("ORDER BY pm.ModuleOrder ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = pageId;

			arParams[1] = new MySqlParameter("?NowDate", MySqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = DateTime.UtcNow;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader GetMyPageModules(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT   ");
			sqlCommand.Append("m.ModuleID, ");
			sqlCommand.Append("m.SiteID, ");
			sqlCommand.Append("m.ModuleDefID, ");
			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("m.HideFromAuth, ");
			sqlCommand.Append("m.HideFromUnAuth, ");
			sqlCommand.Append("m.AllowMultipleInstancesOnMyPage, ");
			sqlCommand.Append("m.Icon As ModuleIcon, ");
			sqlCommand.Append("md.Icon As FeatureIcon, ");
			sqlCommand.Append("md.FeatureName ");
			sqlCommand.Append("FROM	mp_Modules m ");
			sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("WHERE m.SiteID = ?SiteID AND m.AvailableForMyPage = 1 ");
			sqlCommand.Append("ORDER BY m.ModuleTitle ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


		public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT   ");
			sqlCommand.Append("m.ModuleID, ");
			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("m.AuthorizedEditRoles, ");
			sqlCommand.Append("m.EditUserID, ");
			sqlCommand.Append("p.Url, ");
			sqlCommand.Append("p.PageName, ");
			sqlCommand.Append("p.UseUrl, ");
			sqlCommand.Append("p.PageID, ");
			sqlCommand.Append("p.EditRoles ");

			sqlCommand.Append("FROM mp_Modules m ");

			sqlCommand.Append("JOIN mp_PageModules pm ");
			sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

			sqlCommand.Append("JOIN mp_Pages p ");
			sqlCommand.Append("ON pm.PageID = p.PageID ");

			sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("WHERE md.Guid = ?FeatureGuid ");
			sqlCommand.Append("AND m.SiteId = ?SiteID ");

			sqlCommand.Append("ORDER BY p.PageName, m.ModuleTitle ");
			sqlCommand.Append("; ");

			//            string sql = @"SELECT m.ModuleID, m.ModuleTitle, m.AuthorizedEditRoles, p.url, p.PageName, p.UseUrl
			//        FROM mp_Modules m
			//          JOIN mp_PageModules mp ON m.ModuleID=mp.ModuleID
			//            JOIN mp_pages p ON mp.PageID=p.PageID
			//          JOIN mp_ModuleDefinitions md ON m.ModuleDefID=md.ModuleDefID
			//        WHERE md.FeatureName='BlogFeatureName' AND m.SiteId=?SiteID
			//        ORDER BY p.PageName, m.ModuleTitle";

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?FeatureGuid", MySqlDbType.VarChar, 36);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = featureGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);
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

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				commandText
			);
		}
	}
}
