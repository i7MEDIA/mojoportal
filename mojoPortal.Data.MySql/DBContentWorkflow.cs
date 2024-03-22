using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBContentWorkflow
{

	/// <summary>
	/// Inserts a row in the mp_ContentWorkflow table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="createdDateUtc"> createdDateUtc </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="status"> status </param>
	/// <param name="contentText"> contentText </param>
	/// <param name="customData"> customData </param>
	/// <param name="customReferenceNumber"> customReferenceNumber </param>
	/// <param name="customReferenceGuid"> customReferenceGuid </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid moduleGuid,
		Guid userGuid,
		DateTime createdDateUtc,
		string contentText,
		string customData,
		int customReferenceNumber,
		Guid customReferenceGuid,
		string status)
	{
		string sqlCommand = @"
INSERT INTO 
    mp_ContentWorkflow (
        Guid, 
        SiteGuid, 
        ModuleGuid, 
        CreatedDateUtc, 
        UserGuid, 
        LastModUserGuid, 
        LastModUtc, 
        Status, 
        ContentText, 
        CustomData, 
        CustomReferenceNumber, 
        CustomReferenceGuid 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?CreatedDateUtc, 
    ?UserGuid, 
    ?LastModUserGuid, 
    ?LastModUtc, 
    ?Status, 
    ?ContentText, 
    ?CustomData, 
    ?CustomReferenceNumber, 
    ?CustomReferenceGuid 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?CreatedDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDateUtc
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastModUserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDateUtc
			},

			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
			},

			new("?ContentText", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = contentText
			},

			new("?CustomData", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = customData
			},

			new("?CustomReferenceNumber", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = customReferenceNumber
			},

			new("?CustomReferenceGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = customReferenceGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_ContentWorkflow table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="lastModUserGuid"> lastModUserGuid </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <param name="status"> status </param>
	/// <param name="contentText"> contentText </param>
	/// <param name="customData"> customData </param>
	/// <param name="customReferenceNumber"> customReferenceNumber </param>
	/// <param name="customReferenceGuid"> customReferenceGuid </param>
	/// <returns>bool</returns>
	public static int Update(
		Guid guid,
		Guid lastModUserGuid,
		DateTime lastModUtc,
		string contentText,
		string customData,
		int customReferenceNumber,
		Guid customReferenceGuid,
		string status)
	{
		string sqlCommand = @"
UPDATE 
    mp_ContentWorkflow 
SET  
    LastModUserGuid = ?LastModUserGuid, 
    LastModUtc = ?LastModUtc, 
    Status = ?Status, 
    ContentText = ?ContentText, 
    CustomData = ?CustomData, 
    CustomReferenceNumber = ?CustomReferenceNumber, 
WHERE  
    Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
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

			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
			},

			new("?ContentText", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = contentText
			},

			new("?CustomData", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = customData
			},

			new("?CustomReferenceNumber", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = customReferenceNumber
			},

			new("?CustomReferenceGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = customReferenceGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_ContentWorkflowAuditHistory 
WHERE ModuleGuid = ?ModuleGuid ;
DELETE FROM mp_ContentWorkflow 
WHERE ModuleGuid = ?ModuleGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentWorkflowAuditHistory 
WHERE 
	ContentWorkflowGuid IN (SELECT Guid FROM mp_ContentWorkflow WHERE SiteGuid = ?SiteGuid) ;
DELETE FROM 
	mp_ContentWorkflow 
WHERE 
	SiteGuid = ?SiteGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader GetWorkInProgress(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT 
	cw.*, 
	m.ModuleID, 
	m.ModuleTitle, 
	createdBy.Name as CreatedByUserName, 
	createdBy.LoginName as CreatedByUserLogin, 
	createdBy.Email as CreatedByUserEmail, 
	createdBy.FirstName as CreatedByFirstName, 
	createdBy.LastName as CreatedByLastName, 
	createdBy.UserID as CreatedByUserID, 
	createdBy.AvatarUrl as CreatedByAvatar, 
	createdBy.AuthorBio as CreatedByAuthorBio, 
	modifiedBy.Name as ModifiedByUserName, 
	modifiedBy.FirstName as ModifiedByFirstName, 
	modifiedBy.LastName as ModifiedByLastName, 
	modifiedBy.LoginName as ModifiedByUserLogin, 
	modifiedBy.Email as ModifiedByUserEmail, 
	cwah.Notes as Notes, 
	cwah.CreatedDateUtc as RecentActionOn, 
	recentActionBy.Name as RecentActionByUserName, 
	recentActionBy.LoginName as RecentActionByUserLogin, 
	recentActionBy.Email as RecentActionByUserEmail 
FROM 
	mp_ContentWorkflow cw 
JOIN 
	mp_Modules m 
ON 
	cw.ModuleGuid = m.Guid 
LEFT OUTER JOIN 
	mp_Users createdBy 
ON 
	createdBy.UserGuid = cw.UserGuid 
LEFT OUTER JOIN 
	mp_Users modifiedBy 
ON 
	modifiedBy.UserGuid = cw.LastModUserGuid 
LEFT OUTER JOIN 
	mp_ContentWorkflowAuditHistory cwah 
ON 
	cwah.ContentWorkflowGuid = cw.Guid 
AND 
	cwah.Active = 1 
LEFT OUTER JOIN 
	mp_Users recentActionBy 
ON 
	recentActionBy.UserGuid = cwah.UserGuid 
WHERE 
	cw.ModuleGuid = ?ModuleGuid 
AND 
	cw.Status NOT IN ('Cancelled','Approved') 
ORDER BY 
	CreatedDateUtc DESC ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
	};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetWorkInProgress(Guid moduleGuid, string status)
	{
		string sqlCommand = @"
SELECT 
	cw.*, 
	m.ModuleID, 
	m.ModuleTitle, 
	createdBy.Name as CreatedByUserName, 
	createdBy.LoginName as CreatedByUserLogin, 
	createdBy.Email as CreatedByUserEmail, 
	createdBy.FirstName as CreatedByFirstName, 
	createdBy.LastName as CreatedByLastName, 
	createdBy.UserID as CreatedByUserID, 
	createdBy.AvatarUrl as CreatedByAvatar, 
	createdBy.AuthorBio as CreatedByAuthorBio, 
	modifiedBy.Name as ModifiedByUserName, 
	modifiedBy.FirstName as ModifiedByFirstName, 
	modifiedBy.LastName as ModifiedByLastName, 
	modifiedBy.LoginName as ModifiedByUserLogin, 
	modifiedBy.Email as ModifiedByUserEmail, 
	cwah.Notes as Notes, 
	cwah.CreatedDateUtc as RecentActionOn, 
	recentActionBy.Name as RecentActionByUserName, 
	recentActionBy.LoginName as RecentActionByUserLogin, 
	recentActionBy.Email as RecentActionByUserEmail 
FROM 
	mp_ContentWorkflow cw 
JOIN 
	mp_Modules m 
ON 
	cw.ModuleGuid = m.Guid 
LEFT OUTER JOIN 
	mp_Users createdBy 
ON 
	createdBy.UserGuid = cw.UserGuid 
LEFT OUTER JOIN 
	mp_Users modifiedBy 
ON 
	modifiedBy.UserGuid = cw.LastModUserGuid 
LEFT OUTER JOIN 
	mp_ContentWorkflowAuditHistory cwah 
ON 
	cwah.ContentWorkflowGuid = cw.Guid 
AND 
	cwah.Active = 1 
LEFT OUTER JOIN 
	mp_Users recentActionBy 
ON 
	recentActionBy.UserGuid = cwah.UserGuid 
WHERE 
	cw.ModuleGuid = ?ModuleGuid 
AND 
	cw.Status = ?Status 
ORDER BY 
	CreatedDateUtc DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},


			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int GetWorkInProgressCountByPage(Guid pageGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM 
	mp_ContentWorkflow cw 
JOIN 
	mp_PageModules pm 
ON 
	pm.ModuleGuid = cw.ModuleGuid 
WHERE 
	pm.PageGuid = ?PageGuid 
AND 
	cw.Status Not In ('Cancelled','Approved');";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
	{
		Guid result = Guid.Empty;

		var arParams = new List<MySqlParameter>
		{
			new("?ContentWorkflowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentWorkflowGuid.ToString()
			}
		};


		string sqlCommand = @"
SELECT UserGuid 
FROM mp_ContentWorkflowAuditHistory 
WHERE ContentWorkflowGuid = ?ContentWorkflowGuid 
AND NewStatus = 'AwaitingApproval' 
ORDER BY CreatedDateUtc DESC 
LIMIT 1;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				result = new Guid(reader[0].ToString());
			}
		}

		return result;
	}

	public static int GetCount(Guid siteGuid, string status)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ContentWorkflow 
WHERE SiteGuid = ?SiteGuid 
AND Status = ?Status;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPage(
		Guid siteGuid,
		string status,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteGuid, status);

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
SELECT 
	cw.*, 
	m.ModuleID, 
	m.ModuleTitle, 
	createdBy.Name as CreatedByUserName, 
	createdBy.LoginName as CreatedByUserLogin, 
	createdBy.Email as CreatedByUserEmail, 
	cwah.Notes as Notes, 
	cwah.CreatedDateUtc as RecentActionOn, 
	recentActionBy.Name as RecentActionByUserName, 
	recentActionBy.LoginName as RecentActionByUserLogin, 
	recentActionBy.Email as RecentActionByUserEmail 
FROM 
	mp_ContentWorkflow cw  
JOIN 
	mp_Modules m 
ON 
	cw.ModuleGuid = m.Guid 
LEFT OUTER JOIN 
	mp_Users createdBy 
ON 
	createdBy.UserGuid = cw.UserGuid 
LEFT OUTER JOIN 
	mp_ContentWorkflowAuditHistory cwah 
ON 
	cwah.ContentWorkflowGuid = cw.Guid 
AND 
	cwah.Active = 1 
LEFT OUTER JOIN 
	mp_Users recentActionBy 
ON 
	recentActionBy.UserGuid = cwah.UserGuid 
WHERE 
	cw.SiteGuid = ?SiteGuid 
AND 
	cw.Status = ?Status 
ORDER BY  
	cw.CreatedDateUtc DESC 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
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
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetPageInfoForPage(
		Guid siteGuid,
		string status,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
	p.PageID, 
	p.PageGuid, 
	p.PageName, 
	p.UseUrl, 
	p.Url As PageUrl, 
	cw.Guid as WorkflowGuid 
FROM 
	mp_ContentWorkflow cw  
JOIN 
	mp_Modules m 
ON 
	cw.ModuleGuid = m.Guid 
JOIN 
	mp_PageModules pm 
ON 
	pm.ModuleID = m.ModuleID 
JOIN 
	mp_Pages p 
ON 
	pm.PageID = p.PageID 
WHERE 
	cw.SiteGuid = ?SiteGuid 
AND 
	cw.Status = ?Status 
ORDER BY  
	cw.CreatedDateUtc DESC 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?Status", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
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
			},
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int CreateAuditHistory(
		Guid guid,
		Guid workflowGuid,
		Guid moduleGuid,
		Guid userGuid,
		DateTime createdDateUtc,
		string status,
		string notes,
		bool active)
	{
		#region Bit Conversion

		int intActive = 0;
		if (active) { intActive = 1; }

		#endregion

		string sqlCommand = @"
INSERT INTO 
	mp_ContentWorkflowAuditHistory(
		Guid, 
		ContentWorkflowGuid, 
		ModuleGuid, 
		UserGuid, 
		CreatedDateUtc, 
		NewStatus, 
		Notes, 
		Active 
	) 
VALUES (
	?Guid, 
	?ContentWorkflowGuid, 
	?ModuleGuid, 
	?UserGuid, 
	?CreatedDateUtc, 
	?NewStatus, 
	?Notes, 
	?Active 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?ContentWorkflowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = workflowGuid.ToString()
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

			new("?CreatedDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDateUtc
			},

			new("?NewStatus", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = status
			},

			new("?Notes", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = notes
			},

			new("?Active", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intActive
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	public static bool DeactivateAudit(Guid moduleGuid)
	{
		string sqlCommand = @"
UPDATE mp_ContentWorkflowAuditHistory 
SET Active = 0 
WHERE ModuleGuid = ?ModuleGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}



}
