using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Hosting;

namespace mojoPortal.Data
{
	public static class DBModule
	{
		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

			if (connectionString == "defaultdblocation")
			{
				connectionString = "version=3,URI=file:" + HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");
			}

			return connectionString;
		}


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
			string icon,
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
			#region Bit Conversion

			int inthideFromAuthenticated = hideFromAuthenticated ? 1 : 0;
			int inthideFromUnauthenticated = hideFromUnauthenticated ? 1 : 0;
			int intShowTitle = showTitle ? 1 : 0;
			int myAvailable = availableForMyPage ? 1 : 0;
			int allowMultiple = allowMultipleInstancesOnMyPage ? 1 : 0;

			#endregion

			var commandText = @"
INSERT INTO mp_Modules (
	SiteID,
	ModuleDefID,
	ModuleTitle,
	ViewRoles,
	AuthorizedEditRoles,
	DraftEditRoles,
	DraftApprovalRoles,
	CacheTime,
	ShowTitle,
	AvailableForMyPage,
	AllowMultipleInstancesOnMyPage,
	Icon,
	CreatedByUserID,
	CreatedDate,
	CountOfUseOnMyPage,
	Guid,
	FeatureGuid,
	HideFromAuth,
	HideFromUnAuth,
	SiteGuid,
	HeadElement,
	IncludeInSearch,
	PublishMode,
	IsGlobal
)
VALUES (
	:SiteID,
	:ModuleDefID,
	:ModuleTitle,
	:ViewRoles,
	:AuthorizedEditRoles,
	:DraftEditRoles,
	:DraftApprovalRoles,
	:CacheTime,
	:ShowTitle,
	:AvailableForMyPage,
	:AllowMultipleInstancesOnMyPage,
	:Icon,
	:CreatedByUserID,
	:CreatedDate,
	0,
	:Guid,
	:FeatureGuid,
	:HideFromAuth,
	:HideFromUnAuth,
	:SiteGuid,
	:HeadElement,
	1,
	:PublishMode,
	0
);

SELECT LAST_INSERT_ROWID();";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":ModuleTitle", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = moduleTitle
				},
				new SqliteParameter(":AuthorizedEditRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = authorizedEditRoles
				},
				new SqliteParameter(":CacheTime", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = cacheTime
				},
				new SqliteParameter(":ShowTitle", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intShowTitle
				},
				new SqliteParameter(":AvailableForMyPage", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = myAvailable
				},
				new SqliteParameter(":AllowMultipleInstancesOnMyPage", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = allowMultiple
				},
				new SqliteParameter(":Icon", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = icon
				},
				new SqliteParameter(":CreatedByUserID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = createdByUserId
				},
				new SqliteParameter(":CreatedDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdDate
				},
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new SqliteParameter(":FeatureGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":HideFromAuth", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = inthideFromAuthenticated
				},
				new SqliteParameter(":HideFromUnAuth", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = inthideFromUnauthenticated
				},
				new SqliteParameter(":ViewRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = viewRoles
				},
				new SqliteParameter(":DraftEditRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = draftEditRoles
				},
				new SqliteParameter(":HeadElement", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = headElement
				},
				new SqliteParameter(":PublishMode", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = publishMode
				},
				new SqliteParameter(":DraftApprovalRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = draftApprovalRoles
				}
			};

			int newID = Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
				).ToString()
			);

			if (newID > -1 && pageId > -1)
			{
				commandText = @"
INSERT INTO mp_PageModules (
	PageID,
	ModuleID,
	PageGuid,
	ModuleGuid,
	ModuleOrder,
	PaneName,
	PublishBeginDate
)
VALUES (
	:PageID,
	:ModuleID,
	(
		SELECT PageGuid
		FROM mp_Pages
		WHERE PageID = :PageID
		LIMIT 1
	),
	(
		SELECT Guid
		FROM mp_Modules
		WHERE ModuleID = :ModuleID
		LIMIT 1
	),
	:ModuleOrder,
	:PaneName,
	:PublishBeginDate
);";

				commandParameters = new SqliteParameter[]
				{
					new SqliteParameter(":PageID", DbType.Int32)
					{
						Direction = ParameterDirection.Input,
						Value = pageId
					},
					new SqliteParameter(":ModuleID", DbType.Int32)
					{
						Direction = ParameterDirection.Input,
						Value = newID
					},
					new SqliteParameter(":ModuleOrder", DbType.Int32)
					{
						Direction = ParameterDirection.Input,
						Value = moduleOrder
					},
					new SqliteParameter(":PaneName", DbType.String, 50)
					{
						Direction = ParameterDirection.Input,
						Value = paneName
					},
					new SqliteParameter(":PublishBeginDate", DbType.DateTime)
					{
						Direction = ParameterDirection.Input,
						Value = createdDate
					}
				};

				SqliteHelper.ExecuteNonQuery(
					GetConnectionString(),
					commandText,
					commandParameters
				);
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
			string icon,
			bool hideFromAuthenticated,
			bool hideFromUnauthenticated,
			bool includeInSearch,
			bool isGlobal,
			string headElement,
			int publishMode
		)
		{
			#region Bit Conversion

			int inthideFromAuthenticated = hideFromAuthenticated ? 1 : 0;
			int inthideFromUnauthenticated = hideFromUnauthenticated ? 1 : 0;
			int intShowTitle = showTitle ? 1 : 0;
			int myAvailable = availableForMyPage ? 1 : 0;
			int allowMultiple = allowMultipleInstancesOnMyPage ? 1 : 0;
			int intIncludeInSearch = includeInSearch ? 1 : 0;
			int intIsGlobal = isGlobal ? 1 : 0;

			#endregion

			var commandText = @"
UPDATE mp_Modules
SET ModuleDefID = :ModuleDefID,
	ModuleTitle = :ModuleTitle,
	ViewRoles = :ViewRoles,
	AuthorizedEditRoles = :AuthorizedEditRoles,
	DraftEditRoles = :DraftEditRoles,
	DraftApprovalRoles = :DraftApprovalRoles,
	CacheTime = :CacheTime,
	ShowTitle = :ShowTitle,
	HideFromAuth = :HideFromAuth,
	HideFromUnAuth = :HideFromUnAuth,
	EditUserID = :EditUserID,
	AvailableForMyPage = :AvailableForMyPage,
	AllowMultipleInstancesOnMyPage = :AllowMultipleInstancesOnMyPage,
	Icon = :Icon,
	IncludeInSearch = :IncludeInSearch,
	HeadElement = :HeadElement,
	PublishMode = :PublishMode,
	IsGlobal = :IsGlobal
WHERE ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":ModuleTitle", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = moduleTitle
				},
				new SqliteParameter(":AuthorizedEditRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = authorizedEditRoles
				},
				new SqliteParameter(":CacheTime", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = cacheTime
				},
				new SqliteParameter(":ShowTitle", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intShowTitle
				},
				new SqliteParameter(":EditUserID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = editUserId
				},
				new SqliteParameter(":AvailableForMyPage", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = myAvailable
				},
				new SqliteParameter(":AllowMultipleInstancesOnMyPage", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = allowMultiple
				},
				new SqliteParameter(":Icon", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = icon
				},
					new SqliteParameter(":HideFromAuth", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = inthideFromAuthenticated
				},
					new SqliteParameter(":HideFromUnAuth", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = inthideFromUnauthenticated
				},
					new SqliteParameter(":ViewRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = viewRoles
				},
					new SqliteParameter(":DraftEditRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = draftEditRoles
				},
					new SqliteParameter(":IncludeInSearch", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intIncludeInSearch
				},
					new SqliteParameter(":IsGlobal", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intIsGlobal
				},
					new SqliteParameter(":HeadElement", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = headElement
				},
					new SqliteParameter(":PublishMode", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = publishMode
				},
					new SqliteParameter(":DraftApprovalRoles", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = draftApprovalRoles
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
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
			var commandText = @"
UPDATE mp_PageModules
SET
	ModuleOrder = :ModuleOrder,
	PaneName = :PaneName
WHERE ModuleID = :ModuleID
AND PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				},
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":ModuleOrder", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleOrder
				},
				new SqliteParameter(":PaneName", DbType.String, 50)
				{
					Direction = ParameterDirection.Input,
					Value = paneName
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > -1;

		}


		public static bool DeleteModule(int moduleId)
		{
			var commandText = @"
DELETE FROM mp_PageModules
WHERE ModuleID = :ModuleID;
DELETE FROM mp_Modules
WHERE ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
		}


		public static bool DeleteModuleInstance(int moduleId, int pageId)
		{
			var commandText = @"
DELETE FROM mp_PageModules
WHERE ModuleID = :ModuleID AND PageID = :PageID;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				arParams
			);

			return rowsAffected > 0;
		}


		public static bool PageModuleDeleteByPage(int pageId)
		{
			var commandText = @"
DELETE FROM mp_PageModules
WHERE PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
		}


		public static bool PageModuleExists(int moduleId, int pageId)
		{
			var commandText = @"
SELECT Count(*)
FROM mp_PageModules
WHERE ModuleID = :ModuleID AND PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			int count = Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
				)
			);

			return count > 0;
		}


		public static DataTable PageModuleGetByModule(int moduleId)
		{
			var dataTable = new DataTable();

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
					var row = dataTable.NewRow();

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
			var commandText = @"
SELECT
	pm.*,
	m.ModuleTitle,
	p.PageName,
	p.UseUrl,
	p.Url
FROM mp_PageModules pm
JOIN mp_Modules m ON pm.ModuleID = m.ModuleID
JOIN mp_Pages p ON pm.PageID = p.PageID
WHERE pm.ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader PageModuleGetReaderByPage(int pageId)
		{
			var commandText = @"
SELECT
	pm.*,
	m.ModuleTitle,
	p.PageName,
	p.UseUrl,
	p.Url
FROM mp_PageModules pm
JOIN mp_Modules m ON pm.ModuleID = m.ModuleID
JOIN mp_Pages p ON pm.PageID = p.PageID
WHERE pm.PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
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
					pageGuid,
					moduleGuid,
					moduleId,
					pageId,
					paneName,
					moduleOrder,
					publishBeginDate,
					publishEndDate
				);
			}
		}


		public static bool PageModuleInsert(
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
			var commandText = @"
INSERT INTO mp_PageModules (
	ModuleID, 
	PageID, 
	PaneName, 
	ModuleOrder, 
	PublishBeginDate,";

			if (publishEndDate > DateTime.MinValue)
			{
				commandText += "PublishEndDate,";
			}

			commandText += @"
	PageGuid,
	ModuleGuid
)
VALUES (
	:ModuleID,
	:PageID,
	:PaneName,
	:ModuleOrder,
	:PublishBeginDate,";

			if (publishEndDate > DateTime.MinValue)
			{
				commandText += ":PublishEndDate,";
			}

			commandText += @"
	:PageGuid,
	:ModuleGuid;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				},
				new SqliteParameter(":PaneName", DbType.String, 50)
				{
					Direction = ParameterDirection.Input,
					Value = paneName
				},
				new SqliteParameter(":ModuleOrder", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleOrder
				},
				new SqliteParameter(":PublishBeginDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = publishBeginDate
				},
				new SqliteParameter(":PublishEndDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = publishEndDate != DateTime.MinValue ? publishEndDate : (object)DateTime.Now.AddYears(40)
				},
				new SqliteParameter(":PageGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = pageGuid.ToString()
				},
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
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
			var commandText = @"
UPDATE mp_PageModules
SET
	PaneName = :PaneName,
	ModuleOrder = :ModuleOrder,
	PublishBeginDate = :PublishBeginDate,
	PublishEndDate = :PublishEndDate
WHERE ModuleID = :ModuleID
AND PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				},
				new SqliteParameter(":PaneName", DbType.String, 50)
				{
					Direction = ParameterDirection.Input,
					Value = paneName
				},
				new SqliteParameter(":ModuleOrder", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleOrder
				},
				new SqliteParameter(":PublishBeginDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = publishBeginDate
				},
				new SqliteParameter(":PublishEndDate", DbType.DateTime)
				{
					Value = publishEndDate != DateTime.MinValue ? publishEndDate : (object)DBNull.Value,
					Direction = ParameterDirection.Input
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
		}


		public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
		{

			var commandText = @"
UPDATE mp_Modules
SET CountOfUseOnMyPage = CountOfUseOnMyPage + :Increment
WHERE ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":Increment", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = increment
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
		}


		public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
		{
			var commandText = @"
UPDATE mp_PageModules
SET
	PageID = :NewPageID,
	PageGuid = (
		SELECT PageGuid
		FROM mp_Pages
		WHERE PageID = :NewPageID
		LIMIT 1
	)
WHERE ModuleID = :ModuleID
AND PageID = :PageID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = oldPageId
				},
				new SqliteParameter(":NewPageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = newPageId
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				commandText,
				commandParameters
			);

			return rowsAffected > 0;
		}


		public static int CountNonAdminModules(int siteId)
		{
			var sqlCommand = @"
SELECT Count(*)
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
WHERE m.SiteID = :SiteID
AND md.IsAdmin = 0";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					sqlCommand.ToString(),
					commandParameters
				)
			);
		}


		public static int CountForMyPage(int siteId)
		{
			var commandText = @"
SELECT Count(*)
FROM mp_Modules m
WHERE m.SiteID = :SiteID AND m.AvailableForMyPage = 1;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
				)
			);
		}


		public static int GetCount(int siteId, int moduleDefId, string title)
		{
			var commandText = @"
SELECT Count(*)
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
WHERE m.SiteID = :SiteID
AND ((m.ModuleDefID = :ModuleDefID) OR (:ModuleDefID = -1))";

			if (title.Length > 0)
			{
				commandText += "AND (m.ModuleTitle LIKE '%' || :Title || '%') ";
			}

			commandText += "AND md.IsAdmin = 0;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":Title", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = title
				}
			};

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
				)
			);
		}


		public static int GetCountByFeature(int moduleDefId)
		{
			var commandText = @"
SELECT Count(*)
FROM mp_Modules
WHERE ModuleDefID = :ModuleDefID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				}
			};

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
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

			var commandText = @"
SELECT
	m.*,
	md.FeatureName As FeatureName,
	md.ControlSrc As ControlSrc,
	md.ResourceFile As ResourceFile,
	u.Name As CreatedBy,
	(
		SELECT COUNT(pm.PageID)
		FROM mp_PageModules pm
		WHERE pm.ModuleID = m.ModuleID
	) AS UseCount
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
LEFT OUTER JOIN mp_Users u ON m.CreatedByUserID = u.UserID
WHERE m.SiteID = :SiteID
AND ((m.ModuleDefID = :ModuleDefID) OR (:ModuleDefID = -1))";

			if (title.Length > 0)
			{
				commandText += "AND (m.ModuleTitle LIKE '%' || :Title || '%') ";
			}

			commandText += "AND md.IsAdmin = 0 ";

			if (sortByModuleType)
			{
				commandText += "ORDER BY md.FeatureName, m.ModuleTitle ";
			}
			else if (sortByAuthor)
			{
				commandText += "ORDER BY u.[Name], m.ModuleTitle ";
			}
			else
			{
				commandText += "ORDER BY m.ModuleTitle ";
			}

			commandText += $"LIMIT { pageSize.ToString(CultureInfo.InvariantCulture) } ";
			commandText += $"OFFSET { pageLowerBound.ToString(CultureInfo.InvariantCulture) };";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":Title", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = title
				}
			};

			var dt = new DataTable();

			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(string));
			dt.Columns.Add("FeatureName", typeof(string));
			dt.Columns.Add("ResourceFile", typeof(string));
			dt.Columns.Add("ControlSrc", typeof(string));
			dt.Columns.Add("AuthorizedEditRoles", typeof(string));
			dt.Columns.Add("CreatedBy", typeof(string));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			using (IDataReader reader = SqliteHelper.ExecuteReader(
					GetConnectionString(),
					commandText,
					commandParameters
				)
			)
			{
				while (reader.Read())
				{
					var row = dt.NewRow();

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
			var commandText = @"
SELECT Count(*)
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
WHERE m.SiteID = :SiteID
AND ((m.ModuleDefID = :ModuleDefID) OR (:ModuleDefID = -1))
AND m.IsGlobal = 1
AND m.ModuleID NOT IN (
	SELECT ModuleID
	FROM mp_PageModules
	WHERE PageID = :PageID
);";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					commandText,
					commandParameters
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
				Math.DivRem(totalRows, pageSize, out int remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			var commandText = $@"
SELECT
	m.*,
	md.FeatureName As FeatureName,
	md.ControlSrc As ControlSrc,
	md.ResourceFile As ResourceFile,
	u.Name As CreatedBy,
	(
		SELECT COUNT(pm.PageID)
		FROM mp_PageModules pm
		WHERE pm.ModuleID = m.ModuleID
	) AS UseCount
FROM mp_Modules m
	JOIN mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
	LEFT OUTER JOIN mp_Users u ON m.CreatedByUserID = u.UserID
WHERE m.SiteID = :SiteID
	AND ((m.ModuleDefID = :ModuleDefID) OR (:ModuleDefID = -1))
	AND m.IsGlobal = 1
	AND m.ModuleID NOT IN (
		SELECT ModuleID
		FROM mp_PageModules
		WHERE PageID = :PageID
	)
ORDER BY m.ModuleTitle
LIMIT { pageSize.ToString(CultureInfo.InvariantCulture) }
OFFSET { pageLowerBound.ToString(CultureInfo.InvariantCulture) };";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":ModuleDefID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleDefId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			var dt = new DataTable();

			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleTitle", typeof(string));
			dt.Columns.Add("FeatureName", typeof(string));
			dt.Columns.Add("ResourceFile", typeof(string));
			dt.Columns.Add("ControlSrc", typeof(string));
			dt.Columns.Add("AuthorizedEditRoles", typeof(string));
			dt.Columns.Add("CreatedBy", typeof(string));
			dt.Columns.Add("CreatedDate", typeof(DateTime));
			dt.Columns.Add("UseCount", typeof(int));

			using (IDataReader reader = SqliteHelper.ExecuteReader(
					GetConnectionString(),
					commandText,
					commandParameters
				)
			)
			{
				while (reader.Read())
				{
					var row = dt.NewRow();

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
			var commandText = @"
SELECT m.ModuleID As ModuleID,
	m.Guid As Guid,
	m.SiteGuid As SiteGuid,
	m.SiteID As SiteID,
	m.EditUserGuid As EditUserGuid,
	m.FeatureGuid As FeatureGuid,
	m.IncludeInSearch As IncludeInSearch,
	m.IsGlobal As IsGlobal,
	m.ModuleDefID As ModuleDefID,
	m.ModuleTitle As ModuleTitle,
	m.ViewRoles As ViewRoles,
	m.AuthorizedEditRoles As AuthorizedEditRoles,
	m.DraftEditRoles As DraftEditRoles,
	m.DraftApprovalRoles As DraftApprovalRoles,
	m.CacheTime As CacheTime,
	m.ShowTitle As ShowTitle,
	m.HideFromAuth As HideFromAuth,
	m.HideFromUnAuth As HideFromUnAuth,
	m.EditUserID As EditUserID,
	m.AvailableForMyPage As AvailableForMyPage,
	m.AllowMultipleInstancesOnMyPage As AllowMultipleInstancesOnMyPage,
	m.CountOfUseOnMyPage As CountOfUseOnMyPage,
	m.Icon As Icon,
	m.HeadElement As HeadElement,
	m.CreatedByUserID As CreatedByUserID,
	m.CreatedDate As CreatedDate,
	m.PublishMode AS PublishMode,
	md.ControlSrc As ControlSrc,
	md.FeatureName As FeatureName
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
WHERE m.ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetModule(Guid moduleGuid)
		{
			var commandText = @"
SELECT m.ModuleID As ModuleID,
	m.Guid As Guid,
	m.SiteGuid As SiteGuid,
	m.SiteID As SiteID,
	m.EditUserGuid As EditUserGuid,
	m.FeatureGuid As FeatureGuid,
	m.IncludeInSearch As IncludeInSearch,
	m.IsGlobal As IsGlobal,
	m.ModuleDefID As ModuleDefID,
	m.ModuleTitle As ModuleTitle,
	m.ViewRoles As ViewRoles,
	m.AuthorizedEditRoles As AuthorizedEditRoles,
	m.DraftEditRoles As DraftEditRoles,
	m.DraftApprovalRoles As DraftApprovalRoles,
	m.CacheTime As CacheTime,
	m.ShowTitle As ShowTitle,
	m.HideFromAuth As HideFromAuth,
	m.HideFromUnAuth As HideFromUnAuth,
	m.EditUserID As EditUserID,
	m.AvailableForMyPage As AvailableForMyPage,
	m.AllowMultipleInstancesOnMyPage As AllowMultipleInstancesOnMyPage,
	m.CountOfUseOnMyPage As CountOfUseOnMyPage,
	m.Icon As Icon,
	m.HeadElement As HeadElement,
	m.CreatedByUserID As CreatedByUserID,
	m.CreatedDate As CreatedDate,
	m.PublishMode AS PublishMode,
	md.ControlSrc As ControlSrc,
	md.FeatureName As FeatureName
FROM mp_Modules m
	JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
WHERE m.Guid = :ModuleGuid;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetModule(int moduleId, int pageId)
		{
			var commandText = @"
SELECT
	m.ModuleID As ModuleID,
	m.Guid As Guid,
	m.SiteGuid As SiteGuid,
	m.SiteID As SiteID,
	m.EditUserGuid As EditUserGuid,
	m.FeatureGuid As FeatureGuid,
	m.IncludeInSearch As IncludeInSearch,
	m.IsGlobal As IsGlobal,
	m.ModuleDefID As ModuleDefID,
	m.ModuleTitle As ModuleTitle,
	m.ViewRoles As ViewRoles,
	m.AuthorizedEditRoles As AuthorizedEditRoles,
	m.DraftEditRoles As DraftEditRoles,
	m.DraftApprovalRoles As DraftApprovalRoles,
	m.CacheTime As CacheTime,
	m.ShowTitle As ShowTitle,
	m.HideFromAuth As HideFromAuth,
	m.HideFromUnAuth As HideFromUnAuth,
	m.EditUserID As EditUserID,
	m.AvailableForMyPage As AvailableForMyPage,
	m.AllowMultipleInstancesOnMyPage As AllowMultipleInstancesOnMyPage,
	m.CountOfUseOnMyPage As CountOfUseOnMyPage,
	m.Icon As Icon,
	m.HeadElement As HeadElement,
	m.CreatedByUserID As CreatedByUserID,
	m.CreatedDate As CreatedDate,
	m.PublishMode AS PublishMode,
	pm.ModuleOrder As ModuleOrder,
	pm.PaneName As PaneName,
	pm.PageID As PageID,
	pm.PublishBeginDate As PublishBeginDate,
	pm.PublishEndDate As PublishEndDate,
	md.ControlSrc As ControlSrc,
	md.FeatureName As FeatureName
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
JOIN mp_PageModules pm ON m.ModuleID = pm.ModuleID
WHERE pm.PageID = :PageID
AND m.ModuleID = :ModuleID;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetPageModules(int pageId)
		{
			var commandText = @"
SELECT
	m.*,
	pm.PageID As PageID,
	pm.ModuleOrder As ModuleOrder,
	pm.PaneName As PaneName,
	pm.PublishBeginDate As PublishBeginDate,
	pm.PublishEndDate As PublishEndDate,
	md.ControlSrc As ControlSrc,
	md.FeatureName AS FeatureName,
	md.Guid AS FeatureGuid
FROM mp_Modules m
JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
JOIN mp_PageModules pm ON m.ModuleID = pm.ModuleID
WHERE pm.PageID = :PageID
AND pm.PublishBeginDate <= :CurrentDate
ORDER BY pm.ModuleOrder;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":PageID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				},
				new SqliteParameter(":CurrentDate", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = DateTime.UtcNow
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetMyPageModules(int siteId)
		{
			var commandText = @"
SELECT
	m.ModuleID As ModuleID,
	m.SiteID As SiteID,
	m.ModuleDefID As ModuleDefID,
	m.ModuleTitle As ModuleTitle,
	m.AllowMultipleInstancesOnMyPage As AllowMultipleInstancesOnMyPage,
	m.Icon As ModuleIcon,
	m.IncludeInSearch As IncludeInSearch,
	md.Icon As FeatureIcon,
	md.FeatureName As FeatureName,
	md.ResourceFile As ResourceFile
FROM mp_Modules m
	JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
WHERE m.SiteID = :SiteID
AND m.AvailableForMyPage = 1
ORDER BY m.ModuleTitle;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
		}


		public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
		{
			var commandText = @"
SELECT 
	m.ModuleID As ModuleID,
	m.ModuleTitle As ModuleTitle,
	m.AuthorizedEditRoles As AuthorizedEditRoles,
	m.EditUserID As EditUserID,
	m.IncludeInSearch As IncludeInSearch,
	p.Url As Url,
	p.PageName As PageName,
	p.UseUrl As UseUrl,
	p.PageID As PageID,
	p.EditRoles As EditRoles
FROM mp_Modules m
JOIN mp_PageModules pm ON m.ModuleID = pm.ModuleID
JOIN mp_Pages p ON pm.PageID = p.PageID
JOIN mp_ModuleDefinitions md ON m.ModuleDefID = md.ModuleDefID
WHERE md.Guid = :FeatureGuid
AND m.SiteId = :SiteID
ORDER BY
	p.PageName,
	m.ModuleTitle;";

			var commandParameters = new SqliteParameter[]
			{
				new SqliteParameter(":SiteID", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new SqliteParameter(":FeatureGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = featureGuid.ToString()
				}
			};

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
				commandText,
				commandParameters
			);
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
LEFT OUTER JOIN mp_Users u ON m.CreatedByUserID = u.UserID;";

			return SqliteHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				commandText
			);
		}
	}
}
