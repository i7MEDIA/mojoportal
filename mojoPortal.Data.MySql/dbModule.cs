using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace mojoPortal.Data;

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

		string sqlCommand = @"
INSERT INTO 
	mp_Modules (
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
		IncludeInSearch, 
		IsGlobal, 
		PublishMode, 
		HeadElement, 
		SiteGuid 
	)
VALUES (
	?SiteID, 
	?ModuleDefID, 
	?ModuleTitle, 
	?ViewRoles, 
	?AuthorizedEditRoles, 
	?DraftEditRoles, 
	?DraftApprovalRoles, 
	?CacheTime, 
	?ShowTitle, 
	?AvailableForMyPage, 
	?AllowMultipleInstancesOnMyPage, 
	?Icon, 
	?CreatedByUserID, 
	?CreatedDate, 
	0, 
	?Guid, 
	?FeatureGuid, 
	?HideFromAuth, 
	?HideFromUnAuth, 
	1, 
	0, 
	?PublishMode, 
	?HeadElement, 
	?SiteGuid 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?ModuleTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moduleTitle
			},

			new("?AuthorizedEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = authorizedEditRoles
			},

			new("?CacheTime", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cacheTime
			},

			new("?ShowTitle", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowTitle
			},

			new("?AvailableForMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = myAvailable
			},

			new("?CreatedByUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = createdByUserId
			},

			new ("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new ("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowMultiple
			},

			new("?Icon", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?HideFromAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inthideFromAuthenticated
			},

			new("?HideFromUnAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inthideFromUnauthenticated
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			},

			new("?DraftEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftEditRoles
			},

			new("?HeadElement", MySqlDbType.VarChar, 25)
			{
				Direction = ParameterDirection.Input,
				Value = headElement
			},

			new("?PublishMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = publishMode
			},

			new("?DraftApprovalRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftApprovalRoles
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		if ((newID > -1) && (pageId > -1))
		{
			string sqlCommand1 = @"
INSERT INTO 
	mp_PageModules (
		PageID, 
		ModuleID, 
		PageGuid, 
		ModuleGuid, 
		ModuleOrder, 
		PaneName, 
		PublishBeginDate 
	) 
VALUES (
	?PageID, 
	?ModuleID, 
	(SELECT PageGuid FROM mp_Pages WHERE PageID = ?PageID LIMIT 1), 
	(SELECT Guid FROM mp_Modules WHERE ModuleID = ?ModuleID LIMIT 1), 
	?ModuleOrder, 
	?PaneName, 
	?PublishBeginDate 
);";

			var arParams1 = new List<MySqlParameter>
			{
				new("?PageID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = pageId
				},

				new("?ModuleID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = newID
				},

				new("?ModuleOrder", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleOrder
				},

				new("?PaneName", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = paneName
				},

				new("?PublishBeginDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = DateTime.UtcNow.AddMinutes(-30)
				}
			};

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand1.ToString(),
				arParams1);

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

		string sqlCommand = @"
UPDATE 
	mp_Modules 
SET  
	ModuleDefID = ?ModuleDefID, 
	ModuleTitle = ?ModuleTitle, 
	ViewRoles = ?ViewRoles, 
	AuthorizedEditRoles = ?AuthorizedEditRoles, 
	DraftEditRoles = ?DraftEditRoles, 
	DraftApprovalRoles = ?DraftApprovalRoles, 
	CacheTime = ?CacheTime, 
	ShowTitle = ?ShowTitle, 
	EditUserID = ?EditUserID, 
	HideFromAuth = ?HideFromAuth, 
	HideFromUnAuth = ?HideFromUnAuth, 
	AvailableForMyPage = ?AvailableForMyPage, 
	AllowMultipleInstancesOnMyPage = ?AllowMultipleInstancesOnMyPage, 
	IncludeInSearch = ?IncludeInSearch, 
	IsGlobal = ?IsGlobal, 
	PublishMode = ?PublishMode, 
	HeadElement = ?HeadElement, 
	Icon = ?Icon 
WHERE  
	ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?ModuleTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moduleTitle
			},

			new("?AuthorizedEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = authorizedEditRoles
			},

			new("?CacheTime", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cacheTime
			},

			new("?ShowTitle", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowTitle
			},

			new("?EditUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = editUserId
			},

			new("?AvailableForMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = myAvailable
			},

			new("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowMultiple
			},

			new("?Icon", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?HideFromAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inthideFromAuthenticated
			},

			new("?HideFromUnAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inthideFromUnauthenticated
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			},

			new("?DraftEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftEditRoles
			},

			new("?IncludeInSearch", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInSearch
			},

			new("?IsGlobal", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsGlobal
			},

			new("?HeadElement", MySqlDbType.VarChar, 25)
			{
				Direction = ParameterDirection.Input,
				Value = headElement
	},

			new("?PublishMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = publishMode
			},

			new("?DraftApprovalRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftApprovalRoles
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateModuleOrder(
		int pageId,
		int moduleId,
		int moduleOrder,
		string paneName)
	{
		string sqlCommand = @"
UPDATE mp_PageModules 
SET ModuleOrder = ?ModuleOrder , 
PaneName = ?PaneName  
WHERE ModuleID = ?ModuleID 
AND PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?ModuleOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleOrder
			},

			new("?PaneName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paneName
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool DeleteModule(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_PageModules 
WHERE ModuleID = ?ModuleID ;
DELETE FROM mp_Modules 
WHERE ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteModuleInstance(int moduleId, int pageId)
	{
		string sqlCommand = @"
DELETE FROM mp_PageModules 
WHERE ModuleID = ?ModuleID AND PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool PageModuleDeleteByPage(int pageId)
	{
		string sqlCommand = @"
DELETE FROM mp_PageModules 
WHERE PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool PageModuleExists(int moduleId, int pageId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_PageModules 
WHERE ModuleID = ?ModuleID AND PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return (count > 0);

	}

	public static DataTable PageModuleGetByModule(int moduleId)
	{
		DataTable dataTable = new();
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
		string sqlCommand = @"
SELECT 
	pm.*, 
	m.ModuleTitle, 
	p.PageName, 
	p.UseUrl, 
	p.Url 
FROM
	mp_PageModules pm 
JOIN 
	mp_Modules m 
ON 
	pm.ModuleID = m.ModuleID 
JOIN 
	mp_Pages p 
ON 
	pm.PageID = p.PageID 
WHERE 
	pm.ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader PageModuleGetReaderByPage(int pageId)
	{
		string sqlCommand = @"
SELECT 
	pm.*, 
	m.ModuleTitle, 
	p.PageName, 
	p.UseUrl, 
	p.Url 
FROM
	mp_PageModules pm 
JOIN 
	mp_Modules m 
ON 
	pm.ModuleID = m.ModuleID 
JOIN 
	p_Pages p 
ON 
	pm.PageID = p.PageID 
WHERE 
	pm.PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
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

		string sqlCommand = @"
INSERT INTO 
	mp_PageModules (
		PageGuid, 
		ModuleGuid, 
		ModuleID, 
		PageID, 
		PaneName, 
		ModuleOrder, 
		PublishBeginDate, 
		PublishEndDate 
	) 
VALUES (
	?PageGuid, 
	?ModuleGuid, 
	?ModuleID, 
	?PageID, 
	?PaneName, 
	?ModuleOrder, 
	?PublishBeginDate, 
	?PublishEndDate 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
				{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?PaneName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paneName
			},

			new("?ModuleOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleOrder
			},

			new("?PublishBeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = publishBeginDate
			},

			new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		if (publishEndDate != DateTime.MinValue)
		{
			arParams.Add(new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = publishEndDate
			});
		}
		else
		{
			arParams.Add(new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool PageModuleUpdate(
		int moduleId,
		int pageId,
		String paneName,
		int moduleOrder,
		DateTime publishBeginDate,
		DateTime publishEndDate)
	{
		string sqlCommand = @"
UPDATE 
	mp_PageModules 
SET  
	PaneName = ?PaneName, 
	ModuleOrder = ?ModuleOrder, 
	PublishBeginDate = ?PublishBeginDate, 
	PublishEndDate = ?PublishEndDate 
WHERE 
	ModuleID = ?ModuleID AND PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?PaneName", MySqlDbType.VarChar, 50
			) {
				Direction = ParameterDirection.Input,
				Value = paneName
			},

			new("?ModuleOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleOrder
			},

			new("?PublishBeginDate", MySqlDbType.DateTime) {
				Direction = ParameterDirection.Input,
				Value = publishBeginDate
			},

			new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			}
		};

		if (publishEndDate != DateTime.MinValue)
		{
			arParams.Add(new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = publishEndDate
			});
		}
		else
		{
			arParams.Add(new("?PublishEndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
	{

		string sqlCommand = @"
UPDATE mp_Modules 
SET  CountOfUseOnMyPage = CountOfUseOnMyPage + ?Increment 
WHERE ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Increment", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = increment
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
	{
		string sqlCommand = @"
UPDATE mp_PageModules 
SET PageID = ?NewPageID, 
PageGuid = (SELECT PageGuid FROM mp_Pages WHERE PageID = ?NewPageID LIMIT 1)
WHERE ModuleID = ?ModuleID AND PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = oldPageId
			},

			new("?NewPageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = newPageId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static int CountNonAdminModules(int siteId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Modules m  
JOIN mp_ModuleDefinitions md 
ON md.ModuleDefID = m.ModuleDefID 
WHERE m.SiteID = ?SiteID 
AND md.IsAdmin = 0";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static int CountForMyPage(int siteId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Modules m  
WHERE m.SiteID = ?SiteID AND m.AvailableForMyPage = 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}

	public static int GetCount(int siteId, int moduleDefId, string title)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Modules m  
JOIN mp_ModuleDefinitions md 
ON md.ModuleDefID = m.ModuleDefID 
WHERE m.SiteID = ?SiteID 
AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) 
AND md.IsAdmin = 0 ";

		if (title.Length > 0)
		{
			sqlCommand += "AND (m.ModuleTitle LIKE CONCAT('%', ?Title,'%')) ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

	public static int GetCountByFeature(int moduleDefId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Modules   
WHERE  
ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
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
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		int offset = pageSize * (pageNumber - 1);

		string sqlCommand = @"
SELECT 
	m.*, 
	md.FeatureName, 
	md.ControlSrc, 
	md.ResourceFile, 
	u.Name As CreatedBy, (
		SELECT COUNT(pm.PageID) 
		FROM mp_PageModules pm 
		WHERE pm.ModuleID = m.ModuleID
	) 
AS 
	UseCount  
FROM 
	mp_Modules m  
JOIN 
	mp_ModuleDefinitions md 
ON 
	md.ModuleDefID = m.ModuleDefID 
LEFT OUTER JOIN 
	mp_Users u 
ON 
	m.CreatedByUserID = u.UserID 
WHERE 
	m.SiteID = ?SiteID 
AND (
		(m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)
	)";

		if (title.Length > 0)
		{
			sqlCommand += "AND (m.ModuleTitle LIKE CONCAT('%',?Title,'%')) ";
		}

		sqlCommand += "AND md.IsAdmin = 0 ";

		if (sortByModuleType)
		{
			sqlCommand += "ORDER BY md.FeatureName, m.ModuleTitle ";
		}

		else if (sortByAuthor)
		{
			sqlCommand += "ORDER BY u.Name, m.ModuleTitle ";
		}

		else
		{
			sqlCommand += "ORDER BY m.ModuleTitle, md.FeatureName ";
		}

		if (pageNumber > 1)
		{
			sqlCommand += "LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
				+ ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}
		else
		{
			sqlCommand += "LIMIT "
				+ pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}

		sqlCommand += " ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = moduleDefId
			},

			new("?Title", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = title
			}
		};

		DataTable dt = new();
		dt.Columns.Add("ModuleID", typeof(int));
		dt.Columns.Add("ModuleTitle", typeof(String));
		dt.Columns.Add("FeatureName", typeof(String));
		dt.Columns.Add("ResourceFile", typeof(String));
		dt.Columns.Add("ControlSrc", typeof(String));
		dt.Columns.Add("AuthorizedEditRoles", typeof(String));
		dt.Columns.Add("CreatedBy", typeof(String));
		dt.Columns.Add("CreatedDate", typeof(DateTime));
		dt.Columns.Add("UseCount", typeof(int));

		using (IDataReader reader = CommandHelper.ExecuteReader(
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
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Modules m  
JOIN mp_ModuleDefinitions md 
ON md.ModuleDefID = m.ModuleDefID 
WHERE m.SiteID = ?SiteID 
AND ((m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)) 
AND m.IsGlobal = 1 
AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = ?PageID);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = moduleDefId
			},

			new("?PageID", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = pageId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
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
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		int offset = pageSize * (pageNumber - 1);

		string sqlCommand = @"

SELECT m.*, 
md.FeatureName, 
md.ControlSrc, 
md.ResourceFile, 
u.Name As CreatedBy, (
	SELECT COUNT(pm.PageID) 
	FROM mp_PageModules pm 
	WHERE pm.ModuleID = m.ModuleID
) 
AS UseCount  
FROM mp_Modules m  
JOIN mp_ModuleDefinitions md 
ON md.ModuleDefID = m.ModuleDefID 
LEFT OUTER JOIN mp_Users u 
ON m.CreatedByUserID = u.UserID 
WHERE 
m.SiteID = ?SiteID 
AND (
	(m.ModuleDefID = ?ModuleDefID) OR (?ModuleDefID = -1)
) 
AND m.IsGlobal = 1 
AND m.ModuleID NOT IN (
	SELECT ModuleID 
	FROM mp_PageModules 
	WHERE PageID = ?PageID
) 
ORDER BY m.ModuleTitle, md.FeatureName";

		if (pageNumber > 1)
		{
			sqlCommand += "LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
				+ ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}
		else
		{
			sqlCommand += "LIMIT " + pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}

		sqlCommand += " ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = moduleDefId
			},

			new("?PageID", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = pageId
			}
		};

		DataTable dt = new();
		dt.Columns.Add("ModuleID", typeof(int));
		dt.Columns.Add("ModuleTitle", typeof(String));
		dt.Columns.Add("FeatureName", typeof(String));
		dt.Columns.Add("ResourceFile", typeof(String));
		dt.Columns.Add("ControlSrc", typeof(String));
		dt.Columns.Add("AuthorizedEditRoles", typeof(String));
		dt.Columns.Add("CreatedBy", typeof(String));
		dt.Columns.Add("CreatedDate", typeof(DateTime));
		dt.Columns.Add("UseCount", typeof(int));

		using (IDataReader reader = CommandHelper.ExecuteReader(
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
		string sqlCommand = @"
SELECT  
	m.*,  
	md.ControlSrc,  
	md.FeatureName 
FROM 
	mp_Modules m 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
WHERE 
	m.ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32) {
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT  
	m.*,  
	md.ControlSrc,  
	md.FeatureName  
FROM 
	mp_Modules m 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
WHERE 
	m.Guid = ?ModuleGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader GetModule(int moduleId, int pageId)
	{
		string sqlCommand = @"
SELECT  
	m.*,  
	pm.PageID,  
	pm.ModuleOrder,  
	pm.PaneName,  
	pm.PublishBeginDate,  
	pm.PublishEndDate,  
	md.ControlSrc,  
	md.FeatureName  
FROM 
	mp_Modules m 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
JOIN 
	mp_PageModules pm 
ON 
	m.ModuleID = pm.ModuleID 
WHERE 
	pm.PageID = ?PageID 
 AND 
	m.ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetPageModules(int pageId)
	{
		string sqlCommand = @"
SELECT   
	m.*, 
	pm.PageID,   
	pm.ModuleOrder,   
	pm.PaneName,   
	pm.PublishBeginDate,   
	pm.PublishEndDate,   
	md.ControlSrc, 
	md.FeatureName, 
	md.Guid AS FeatureGuid 
FROM 
	mp_Modules m 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
JOIN 
	mp_PageModules pm 
ON 
	m.ModuleID = pm.ModuleID 
WHERE 
	pm.PageID = ?PageID 
AND 
	pm.PublishBeginDate <= ?NowDate 
AND (
		pm.PublishEndDate IS NULL 
		OR pm.PublishEndDate > ?NowDate
	) 
ORDER BY pm.ModuleOrder;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?NowDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetMyPageModules(int siteId)
	{
		string sqlCommand = @"
SELECT   
	m.ModuleID, 
	m.SiteID, 
	m.ModuleDefID, 
	m.ModuleTitle, 
	m.HideFromAuth, 
	m.HideFromUnAuth, 
	m.AllowMultipleInstancesOnMyPage, 
	m.Icon As ModuleIcon, 
	md.Icon As FeatureIcon, 
	md.FeatureName 
FROM 
	mp_Modules m 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
WHERE 
	m.SiteID = ?SiteID 
AND 
	m.AvailableForMyPage = 1 
ORDER BY 
	m.ModuleTitle;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
	{
		string sqlCommand = @"
SELECT   
	m.ModuleID, 
	m.ModuleTitle, 
	m.AuthorizedEditRoles, 
	m.EditUserID, 
	p.Url, 
	p.PageName, 
	p.UseUrl, 
	p.PageID, 
	p.EditRoles 
FROM 
	mp_Modules m 
JOIN 
	mp_PageModules pm 
ON 
	m.ModuleID = pm.ModuleID 
JOIN 
	mp_Pages p 
ON 
	pm.PageID = p.PageID 
JOIN 
	mp_ModuleDefinitions md 
ON 
	m.ModuleDefID = md.ModuleDefID 
WHERE 
	md.Guid = ?FeatureGuid 
AND 
	m.SiteId = ?SiteID 
ORDER BY 
	p.PageName, m.ModuleTitle; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	public static IDataReader GetGlobalContent(int siteId)
	{
		var commandText = @"
SELECT 
	m.*,
	md.FeatureName,
	md.ControlSrc,
	md.ResourceFile,
	u.Name As CreatedBy,
	u.UserID AS CreatedById,(
		SELECT COUNT(pm.PageID)
		FROM mp_PageModules pm
		WHERE pm.ModuleID = m.ModuleID
	) 
	AS UseCount
FROM 
	mp_Modules m
JOIN 
	mp_ModuleDefinitions md ON md.ModuleDefID = m.ModuleDefID
LEFT OUTER JOIN 
	mp_Users u ON m.CreatedByUserID = u.UserID";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			commandText
		);
	}
}
