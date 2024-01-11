using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBHtmlContent
{

	public static int AddHtmlContent(
		Guid itemGuid,
		Guid moduleGuid,
		int moduleId,
		string title,
		string excerpt,
		string body,
		string moreLink,
		int sortOrder,
		DateTime beginDate,
		DateTime endDate,
		DateTime createdDate,
		int userId,
		Guid userGuid,
		bool excludeFromRecentContent)
	{
		int exclude = 0;
		if (excludeFromRecentContent) { exclude = 1; }

		string sqlCommand = @"
INSERT INTO 
    mp_HtmlContent (
        ModuleID, 
        Title, 
        Excerpt, 
        Body, 
        MoreLink, 
        SortOrder, 
        BeginDate, 
        EndDate, 
        CreatedDate, 
        UserID, 
        ItemGuid, 
        ModuleGuid, 
        ExcludeFromRecentContent, 
        UserGuid, 
        LastModUserGuid, 
        LastModUtc 
    )
VALUES (
    ?ModuleID, 
    ?Title, 
    ?Excerpt, 
    ?Body, 
    ?MoreLink, 
    ?SortOrder, 
    ?BeginDate, 
    ?EndDate, 
    ?CreatedDate, 
    ?UserID, 
    ?ItemGuid, 
    ?ModuleGuid, 
    ?ExcludeFromRecentContent, 
    ?UserGuid, 
    ?UserGuid, 
    ?CreatedDate 
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

			new("?Excerpt", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = excerpt
			},

			new("?Body", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = body
			},

			new("?MoreLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moreLink
			},

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
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
			},

			new("?ExcludeFromRecentContent", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = exclude
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}


	public static bool UpdateHtmlContent(
		int itemId,
		int moduleId,
		string title,
		string excerpt,
		string body,
		string moreLink,
		int sortOrder,
		DateTime beginDate,
		DateTime endDate,
		DateTime lastModUtc,
		Guid lastModUserGuid,
		bool excludeFromRecentContent)
	{
		int exclude = 0;
		if (excludeFromRecentContent) { exclude = 1; }

		string sqlCommand = @"
UPDATE 
    mp_HtmlContent 
SET  
    BeginDate = ?BeginDate  , 
    EndDate = ?EndDate  , 
    Title = ?Title  , 
    Excerpt = ?Excerpt  , 
    Body = ?Body , 
    MoreLink = ?MoreLink,   
    ExcludeFromRecentContent = ?ExcludeFromRecentContent,   
    LastModUserGuid = ?LastModUserGuid, 
    LastModUtc = ?LastModUtc 
WHERE 
    ItemID = ?ItemID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Excerpt", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = excerpt
			},

			new("?Body", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = body
			},

			new("?MoreLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moreLink
			},

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?LastModUserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUserGuid.ToString()
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?ExcludeFromRecentContent", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = exclude
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool DeleteHtmlContent(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_HtmlContent 
WHERE ItemID = ?ItemID;";

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
		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		string sqlCommand = @"       
DELETE FROM 
    mp_ContentHistory 
WHERE 
    ContentGuid 
IN (
    SELECT ItemGuid 
    FROM mp_HtmlContent 
    WHERE ModuleID = ?ModuleID 
);";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		string sqlCommand1 = @"
DELETE FROM 
    mp_ContentRating 
WHERE ContentGuid 
IN (
    SELECT ItemGuid 
    FROM mp_HtmlContent 
    WHERE ModuleID = ?ModuleID 
);";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams);

		string sqlCommand2 = @"
DELETE FROM mp_HtmlContent 
WHERE ModuleID = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand2.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteBySite(int siteId)
	{
		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		string sqlCommand = @"        
DELETE FROM 
    mp_ContentHistory 
WHERE ContentGuid 
IN (
    SELECT ItemGuid 
    FROM mp_HtmlContent 
    WHERE ModuleID 
    IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
);";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		string sqlCommand1 = @"
DELETE FROM 
    mp_ContentRating 
WHERE ContentGuid 
IN (
    SELECT ItemGuid 
    FROM mp_HtmlContent 
    WHERE ModuleID 
    IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    )
);";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams);

		string sqlCommand2 = @"
DELETE FROM 
    mp_HtmlContent 
WHERE ModuleID 
IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand2.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader GetHtmlForMetaWeblogApi(int siteId)
	{
		string sqlCommand = @"
SELECT  
    pm.*, 
    m.ModuleTitle, 
    m.AuthorizedEditRoles, 
    m.IsGlobal, 
    h.Body, 
    h.ItemID, 
    h.ItemGuid, 
    h.LastModUserGuid, 
    h.LastModUtc, 
    p.PageGuid, 
    p.ParentID, 
    p.ParentGuid, 
    p.PageName, 
    p.UseUrl, 
    p.Url, 
    p.EditRoles, 
    p.PageOrder, 
    p.EnableComments, 
    p.IsPending, 
    pp.PageName As ParentName 
FROM
    mp_PageModules pm 
JOIN 
    mp_Modules m 
ON 
    pm.ModuleID = m.ModuleID 
LEFT OUTER JOIN 
    mp_HtmlContent h 
ON 
    h.ModuleID = m.ModuleID 
JOIN 
    mp_ModuleDefinitions md 
ON 
    md.ModuleDefID = m.ModuleDefID 
JOIN 
    mp_Pages p 
ON 
    pm.PageID = p.PageID 
LEFT OUTER JOIN 
    mp_Pages pp 
ON 
    pp.PageID = p.ParentID 
WHERE p.SiteID = ?SiteID  
AND 
    md.Guid = '881e4e00-93e4-444c-b7b0-6672fb55de10' 
AND 
    pm.PaneName = 'contentpane' 
ORDER BY 
    p.PageName, pm.ModuleOrder;";

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

	public static IDataReader GetHtmlContent(
		int moduleId,
		DateTime beginDate)
	{

		string sqlCommand = @"
SELECT 
    h.*, 
    u1.Name AS CreatedByName, 
    u1.FirstName AS CreatedByFirstName, 
    u1.LastName AS CreatedByLastName, 
    u1.Email AS CreatedByEmail, 
    u2.Name AS LastModByName, 
    u1.AuthorBio, 
    u1.AvatarUrl, 
    COALESCE(u1.UserID, -1) As AuthorUserID, 
    u2.FirstName AS LastModByFirstName, 
    u2.LastName AS LastModByLastName, 
    u2.Email AS LastModByEmail 
FROM
    mp_HtmlContent h 
LEFT OUTER JOIN 
    mp_Users u1 
ON 
    h.UserGuid = u1.UserGuid 
LEFT OUTER JOIN 
    mp_Users u2 
ON 
    h.LastModUserGuid = u2.UserGuid 
WHERE 
    h.ModuleID = ?ModuleID  
AND 
    h.BeginDate <= ?BeginDate  
AND 
    h.EndDate >= ?BeginDate  
ORDER BY 
    h.BeginDate DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetHtmlContentByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT  
    h.*, 
    m.ModuleTitle, 
    m.ViewRoles, 
    m.IncludeInSearch, 
    md.FeatureName, 
    u1.Name AS CreatedByName, 
    u1.FirstName AS CreatedByFirstName, 
    u1.LastName AS CreatedByLastName, 
    u1.Email AS CreatedByEmail, 
    u1.AuthorBio, 
    u1.AvatarUrl, 
    COALESCE(u1.UserID, -1) As AuthorUserID 
FROM 
    mp_HtmlContent h 
JOIN 
    mp_Modules m 
ON 
    h.ModuleID = m.ModuleID 
JOIN 
    mp_ModuleDefinitions md 
ON 
    m.ModuleDefID = md.ModuleDefID 
JOIN 
    mp_PageModules pm 
ON 
    m.ModuleID = pm.ModuleID 
JOIN 
    mp_Pages p 
ON 
    p.PageID = pm.PageID 
LEFT OUTER JOIN 
    mp_Users u1 
ON 
    h.UserGuid = u1.UserGuid 
WHERE 
p.SiteID = ?SiteID 
AND 
    pm.PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}



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





}
