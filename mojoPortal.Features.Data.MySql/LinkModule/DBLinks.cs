using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBLinks
{
	public static int AddLink(
		Guid itemGuid,
		Guid moduleGuid,
		int moduleId,
		string title,
		string url,
		int viewOrder,
		string description,
		DateTime createdDate,
		int createdBy,
		string target,
		Guid userGuid)
	{

		string sqlCommand = @"
INSERT INTO mp_Links (
    ModuleID, 
    Title, 
    Url, 
    ViewOrder, 
    Description, 
    CreatedDate, 
    Target, 
    CreatedBy, 
    ItemGuid, 
    ModuleGuid, 
    UserGuid 
) 
VALUES (
    ?ModuleID, 
    ?Title, 
    ?Url, 
    ?ViewOrder, 
    ?Description, 
    ?CreatedDate, 
    ?Target, 
    ?CreatedBy, 
    ?ItemGuid, 
    ?ModuleGuid, 
    ?UserGuid 
); 
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Url", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?ViewOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = viewOrder
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new("?Target", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = target
			},

			new("?CreatedBy", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy
			},

			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}


	public static bool UpdateLink(
		int itemId,
		int moduleId,
		string title,
		string url,
		int viewOrder,
		string description,
		DateTime createdDate,
		string target,
		int createdBy)
	{

		string sqlCommand = @"
UPDATE 
    mp_Links 
SET  
    ModuleID = ?ModuleID, 
    Title = ?Title, 
    Url = ?Url, 
    ViewOrder = ?ViewOrder, 
    Description = ?Description, 
    CreatedDate = ?CreatedDate, 
    Target = ?Target,
    CreatedBy = ?CreatedBy 
WHERE 
    ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Url", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?ViewOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = viewOrder
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new("?Target", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = target
			},

			new("?CreatedBy", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static IDataReader GetLinks(int moduleId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Links 
WHERE ModuleID = ?ModuleID 
ORDER BY ViewOrder, Title ;";

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

	public static IDataReader GetLinksByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT 
    ce.*, 
    m.ModuleTitle, 
    m.ViewRoles, 
    md.FeatureName 
FROM 
    mp_Links ce 
JOIN 
    mp_Modules m 
ON ce.ModuleID = m.ModuleID 
JOIN 
    mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
JOIN 
    mp_PageModules pm 
ON m.ModuleID = pm.ModuleID 
JOIN 
    mp_Pages p 
ON p.PageID = pm.PageID 
WHERE 
    p.SiteID = ?SiteID 
AND 
    pm.PageID = ?PageID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
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

	public static IDataReader GetLink(int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Links 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	public static bool DeleteLink(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_Links 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByModule(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_Links 
WHERE ModuleID = ?moduleId";

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

	public static bool DeleteBySite(int siteId)
	{
		string sqlCommand = @"
DELETE FROM 
    mp_Links 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static int GetCount(int moduleId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Links 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPage(
		int moduleId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{

		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(moduleId);

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

		string sqlCommand = @"
SELECT * 
FROM mp_Links  
WHERE ModuleID = ?ModuleID 
ORDER BY ViewOrder, Title  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}



}
