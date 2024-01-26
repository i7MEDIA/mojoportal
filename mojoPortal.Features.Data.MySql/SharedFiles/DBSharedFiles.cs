using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBSharedFiles
{
	public static int AddSharedFileFolder(
		Guid folderGuid,
		Guid moduleGuid,
		Guid parentGuid,
		int moduleId,
		string folderName,
		int parentId,
		string viewRoles
	)
	{
		string sqlCommand = @"
INSERT INTO mp_SharedFileFolders (
	ModuleID,
	FolderName,
	ParentID,
	ModuleGuid,
	FolderGuid,
	ParentGuid,
	ViewRoles
) 
VALUES (
	?ModuleID,
	?FolderName,
	?ParentID,
	?ModuleGuid,
	?FolderGuid,
	?ParentGuid,
	?ViewRoles
); 
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?FolderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = folderGuid.ToString()
			},

			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;
	}


	public static bool UpdateSharedFileFolder(
		int folderId,
		int moduleId,
		string folderName,
		int parentId,
		Guid parentGuid,
		string viewRoles
	)
	{

		string sqlCommand = @"
UPDATE 
	mp_SharedFileFolders 
SET  
	ModuleID = ?ModuleID, 
	FolderName = ?FolderName, 
	ParentID = ?ParentID, 
	ParentGuid = ?ParentGuid, 
	ViewRoles = ?ViewRoles 
WHERE 
	FolderID = ?FolderID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			},

			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			},

			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;
	}


	public static bool DeleteSharedFileFolder(int folderId)
	{
		string sqlCommand = @"
DELETE FROM mp_SharedFileFolders 
WHERE FolderID = ?FolderID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}


	public static IDataReader GetSharedFileFolder(int folderId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SharedFileFolders 
WHERE FolderID = ?FolderID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	public static IDataReader GetSharedModuleFolders(int moduleId)
	{
		string sqlCommand = @"
SELECT 
	sff.*, 
	(SELECT COALESCE(SUM(sf.SizeInKB),0) 
FROM 
	mp_SharedFiles sf 
WHERE 
	sf.FolderID = sff.FolderID) As SizeInKB 
FROM 
	mp_SharedFileFolders sff 
WHERE 
	sff.ModuleID = ?ModuleID 
ORDER BY 
	sff.FolderName ;";

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


	public static IDataReader GetSharedFolders(int moduleId, int parentId)
	{
		string sqlCommand = @"
SELECT 
	sff.*, 
	(SELECT COALESCE(SUM(sf.SizeInKB),0) 
FROM 
	mp_SharedFiles sf 
WHERE 
	sf.FolderID = sff.FolderID) As SizeInKB 
FROM 
	mp_SharedFileFolders sff 
WHERE 
	sff.ModuleID = ?ModuleID 
AND 
	sff.ParentID = ?ParentID 
ORDER BY sff.FolderName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	public static int AddSharedFile(
		Guid itemGuid,
		Guid moduleGuid,
		Guid userGuid,
		Guid folderGuid,
		int moduleId,
		int uploadUserId,
		string friendlyName,
		string originalFileName,
		string serverFileName,
		int sizeInKB,
		DateTime uploadDate,
		int folderId,
		string description,
		string viewRoles
	)
	{
		string sqlCommand = @"
INSERT INTO mp_SharedFiles (
	ModuleID, 
	UploadUserID, 
	FriendlyName, 
	OriginalFileName, 
	ServerFileName, 
	SizeInKB, 
	UploadDate, 
	FolderID, 
	ItemGuid, 
	ModuleGuid, 
	UserGuid, 
	Description, 
	DownloadCount, 
	FolderGuid, 
	ViewRoles
) 
VALUES (
	?ModuleID, 
	?UploadUserID, 
	?FriendlyName, 
	?OriginalFileName, 
	?ServerFileName, 
	?SizeInKB, 
	?UploadDate, 
	?FolderID, 
	?ItemGuid, 
	?ModuleGuid, 
	?UserGuid, 
	?Description, 
	0, 
	?FolderGuid, 
	?ViewRoles
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UploadUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			},

			new("?FriendlyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			},

			new("?OriginalFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			},

			new("?ServerFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			},


			new("?SizeInKB", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			},

			new("?UploadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			},

			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
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

			new("?FolderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = folderGuid.ToString()
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;
	}


	public static bool UpdateSharedFile(
		int itemId,
		int moduleId,
		int uploadUserId,
		string friendlyName,
		string originalFileName,
		string serverFileName,
		int sizeInKB,
		DateTime uploadDate,
		int folderId,
		Guid folderGuid,
		Guid userGuid,
		string description,
		string viewRoles
	)
	{
		string sqlCommand = @"
UPDATE mp_SharedFiles 
SET 
	ModuleID = ?ModuleID, 
	UploadUserID = ?UploadUserID, 
	FriendlyName = ?FriendlyName, 
	OriginalFileName = ?OriginalFileName, 
	ServerFileName = ?ServerFileName, 
	SizeInKB = ?SizeInKB, 
	UploadDate = ?UploadDate, 
	FolderID = ?FolderID, 
	UserGuid = ?UserGuid, 
	Description = ?Description, 
	FolderGuid = ?FolderGuid, 
	ViewRoles = ?ViewRoles 
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

			new("?UploadUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			},

			new("?FriendlyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			},

			new("?OriginalFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			},

			new("?ServerFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			},

			new("?SizeInKB", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			},

			new("?UploadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			},

			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?FolderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = folderGuid.ToString()
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;
	}


	public static bool IncrementDownloadCount(int itemId)
	{
		string sqlCommand = @"
UPDATE mp_SharedFiles 
SET  DownloadCount = DownloadCount + 1 
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

		return rowsAffected > -1;
	}


	public static bool DeleteSharedFile(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_SharedFiles 
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
DELETE FROM mp_SharedFilesHistory WHERE ModuleID = ?moduleId
DELETE FROM mp_SharedFiles WHERE ModuleID = ?moduleId
DELETE FROM mp_SharedFileFolders WHERE ModuleID = ?moduleId";

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
DELETE FROM mp_SharedFilesHistory 
WHERE ModuleID 
IN (
	SELECT ModuleID 
	FROM mp_Modules 
	WHERE SiteID = ?SiteID
);
DELETE FROM mp_SharedFiles 
WHERE ModuleID 
IN (
	SELECT ModuleID 
	FROM mp_Modules 
	WHERE SiteID = ?SiteID
);
DELETE FROM mp_SharedFileFolders 
WHERE ModuleID 
IN (
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


	public static IDataReader GetSharedFile(int itemId)
	{
		string sqlCommand = @"
SELECT  
	ItemID, 
	ModuleID, 
	UploadUserID, 
	FriendlyName, 
	OriginalFileName, 
	ServerFileName, 
	SizeInKB, 
	UploadDate, 
	FolderID, 
	ItemGuid, 
	FolderGuid, 
	UserGuid, 
	Description, 
	DownloadCount, 
	ModuleGuid, 
	ViewRoles 
FROM mp_SharedFiles 
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


	public static IDataReader GetSharedFiles(int moduleId, int folderId)
	{
		string sqlCommand = @"
SELECT   
	sf.ItemID, 
	sf.ModuleID, 
	sf.UploadUserID, 
	sf.FriendlyName, 
	sf.OriginalFileName, 
	sf.ServerFileName, 
	sf.SizeInKB, 
	sf.UploadDate, 
	sf.FolderID, 
	sf.ItemGuid, 
	sf.FolderGuid, 
	sf.UserGuid, 
	sf.ModuleGuid, 
	sf.Description, 
	sf.DownloadCount, 
	sf.ViewRoles, 
	u.Name As UserName 
FROM mp_SharedFiles sf 
LEFT OUTER JOIN 
	mp_Users u 
ON sf.UploadUserID = u.UserID 
WHERE sf.ModuleID = ?ModuleID 
AND sf.FolderID = ?FolderID 
ORDER BY sf.FriendlyName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?FolderID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}


	public static IDataReader GetSharedFiles(int moduleId)
	{
		string sqlCommand = @"
SELECT   
	sf.ItemID, 
	sf.ModuleID, 
	sf.UploadUserID, 
	sf.FriendlyName, 
	sf.OriginalFileName, 
	sf.ServerFileName, 
	sf.SizeInKB, 
	sf.UploadDate, 
	sf.FolderID, 
	sf.ItemGuid, 
	sf.FolderGuid, 
	sf.UserGuid, 
	sf.ModuleGuid, 
	sf.Description, 
	sf.DownloadCount, 
	sf.ViewRoles 
FROM mp_SharedFiles sf 
WHERE sf.ModuleID = ?ModuleID 
ORDER BY sf.FriendlyName ;";

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


	public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT   
	ce.ItemID, 
	ce.ModuleID, 
	ce.UploadUserID, 
	ce.FriendlyName, 
	ce.OriginalFileName, 
	ce.ServerFileName, 
	ce.SizeInKB, 
	ce.UploadDate, 
	ce.FolderID, 
	ce.ItemGuid, 
	ce.FolderGuid, 
	ce.UserGuid, 
	ce.ModuleGuid, 
	ce.Description, 
	ce.DownloadCount, 
	ce.ViewRoles, 
	m.ModuleTitle, 
	m.ViewRoles, 
	md.FeatureName 
FROM mp_SharedFiles ce 
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
WHERE p.SiteID = ?SiteID 
AND pm.PageID = ?PageID ;";

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


	public static bool AddHistory(
		Guid itemGuid,
		Guid moduleGuid,
		Guid userGuid,
		int itemId,
		int moduleId,
		string friendlyName,
		string originalFileName,
		string serverFileName,
		int sizeInKB,
		DateTime uploadDate,
		int uploadUserId,
		DateTime archiveDate,
		string viewRoles
	)
	{
		string sqlCommand = @"
INSERT INTO mp_SharedFilesHistory (
	ItemID, 
	ModuleID, 
	FriendlyName, 
	OriginalFileName, 
	ServerFileName, 
	SizeInKB, 
	UploadDate, 
	UploadUserID, 
	ArchiveDate, 
	ItemGuid, 
	ModuleGuid, 
	UserGuid, 
	ViewRoles
) 
VALUES (
	?ItemID, 
	?ModuleID, 
	?FriendlyName, 
	?OriginalFileName, 
	?ServerFileName, 
	?SizeInKB, 
	?UploadDate, 
	?UploadUserID, 
	?ArchiveDate, 
	?ItemGuid, 
	?ModuleGuid, 
	?UserGuid, 
	?ViewRoles
); 
SELECT LAST_INSERT_ID();";

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

			new("?FriendlyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			},

			new("?OriginalFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			},

			new("?ServerFileName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			},

			new("?SizeInKB", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			},

			new("?UploadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			},

			new("?UploadUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			},

			new("?ArchiveDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = archiveDate
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

			new("?ViewRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID > 0;
	}


	public static bool DeleteHistory(int id)
	{
		string sqlCommand = @"
DELETE FROM mp_SharedFilesHistory 
WHERE ID = ?ID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = id
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}


	public static bool DeleteHistoryByItemID(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_SharedFilesHistory 
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


	public static IDataReader GetHistory(int moduleId, int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SharedFilesHistory 
WHERE ModuleID = ?ModuleID 
AND ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

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


	public static IDataReader GetHistoryByModule(int moduleId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SharedFilesHistory 
WHERE ModuleID = ?ModuleID ;";

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


	public static IDataReader GetHistoryFile(int id)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SharedFilesHistory 
WHERE ID = ?ID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = id
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}
}