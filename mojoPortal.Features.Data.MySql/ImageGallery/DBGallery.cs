using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBGallery
{

	public static int AddGalleryImage(
		Guid itemGuid,
		Guid moduleGuid,
		int moduleId,
		int displayOrder,
		string caption,
		string description,
		string metaDataXml,
		string imageFile,
		string webImageFile,
		string thumbnailFile,
		DateTime uploadDate,
		string uploadUser,
		Guid userGuid)
	{


		string sqlCommand = @"
INSERT INTO mp_GalleryImages (
    ModuleID, 
    DisplayOrder, 
    Caption, 
    Description, 
    MetaDataXml, 
    ImageFile, 
    WebImageFile, 
    ThumbnailFile, 
    UploadDate, 
    UploadUser, 
    ItemGuid, 
    ModuleGuid, 
    UserGuid )
     VALUES (
    ?ModuleID, 
    ?DisplayOrder, 
    ?Caption, 
    ?Description, 
    ?MetaDataXml, 
    ?ImageFile, 
    ?WebImageFile, 
    ?ThumbnailFile, 
    ?UploadDate, 
    ?UploadUser, 
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

			new("?DisplayOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = displayOrder
			},

			new("?Caption", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = caption
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?MetaDataXml", MySqlDbType.MediumText)
			{
				Direction = ParameterDirection.Input,
				Value = metaDataXml
			},

			new("?ImageFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = imageFile
			},

			new("?WebImageFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = webImageFile
			},

			new("?ThumbnailFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = thumbnailFile
			},

			new("?UploadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			},

			new("?UploadUser", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUser
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


	public static bool UpdateGalleryImage(
		int itemId,
		int moduleId,
		int displayOrder,
		string caption,
		string description,
		string metaDataXml,
		string imageFile,
		string webImageFile,
		string thumbnailFile,
		DateTime uploadDate,
		string uploadUser)
	{

		string sqlCommand = @"
UPDATE mp_GalleryImages 
SET  
    ModuleID = ?ModuleID, 
    DisplayOrder = ?DisplayOrder, 
    Caption = ?Caption, 
    Description = ?Description, 
    MetaDataXml = ?MetaDataXml, 
    ImageFile = ?ImageFile, 
    WebImageFile = ?WebImageFile, 
    ThumbnailFile = ?ThumbnailFile, 
    UploadDate = ?UploadDate, 
    UploadUser = ?UploadUser 
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

			new("?DisplayOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = displayOrder
			},

			new("?Caption", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = caption
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?MetaDataXml", MySqlDbType.MediumText)
			{
				Direction = ParameterDirection.Input,
				Value = metaDataXml
			},

			new("?ImageFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = imageFile
			},

			new("?WebImageFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = webImageFile
			},

			new("?ThumbnailFile", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = thumbnailFile
			},

			new("?UploadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			},

			new("?UploadUser", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUser
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool DeleteGalleryImage(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_GalleryImages 
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
		string sqlCommand = "DELETE FROM mp_GalleryImages WHERE ModuleID = ?moduleId";

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
		string sqlCommand = "DELETE FROM mp_GalleryImages WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);";

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


	public static IDataReader GetGalleryImage(int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GalleryImages 
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

	public static IDataReader GetAllImages(int moduleId)
	{
		string sqlCommand = @"
SELECT  * 
FROM mp_GalleryImages 
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

	public static IDataReader GetImagesByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT 
    ce.*, 
    m.ModuleTitle, 
    m.ViewRoles, 
    md.FeatureName 
FROM mp_GalleryImages ce 
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
    AND pm.PageID = ?PageID 
    AND pm.PublishBeginDate < now() 
    AND (pm.PublishEndDate IS NULL 
    OR pm.PublishEndDate > now()) ;";

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

	public static DataTable GetThumbsByPage(
		int moduleId,
		int pageNumber,
		int thumbsPerPage)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_GalleryImages 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int totalRows = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		int totalPages = totalRows / thumbsPerPage;
		if (totalRows <= thumbsPerPage)
		{
			totalPages = 1;
		}
		else
		{
			int remainder = 0;
			Math.DivRem(totalRows, thumbsPerPage, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		int offset = thumbsPerPage * (pageNumber - 1);

		string sqlCommand1 = $@"
SELECT  
i.ItemID,  
i.Caption,  
i.ThumbnailFile,  
i.WebImageFile,  
i.ImageFile,  
{totalPages.ToString(CultureInfo.InvariantCulture)} As TotalPages 
FROM mp_GalleryImages i 
WHERE i.ModuleID = ?ModuleID 
ORDER BY i.DisplayOrder, i.ItemID ";

		if (pageNumber > 1)
		{
			sqlCommand1 += $@"LIMIT {offset.ToString(CultureInfo.InvariantCulture)}, {thumbsPerPage.ToString(CultureInfo.InvariantCulture)} ";
		}
		else
		{
			sqlCommand1 += $@"LIMIT {thumbsPerPage.ToString(CultureInfo.InvariantCulture)} ";
		}

		sqlCommand1 += " ; ";


		var arParams1 = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		DataTable dt = new DataTable();
		dt.Columns.Add("ItemID", typeof(int));
		dt.Columns.Add("Caption", typeof(String));
		dt.Columns.Add("ThumbnailFile", typeof(String));
		dt.Columns.Add("WebImageFile", typeof(String));
		dt.Columns.Add("TotalPages", typeof(int));


		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{

			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["ItemID"] = reader["ItemID"];
				row["Caption"] = reader["Caption"];
				row["ThumbnailFile"] = reader["ThumbnailFile"];
				row["WebImageFile"] = reader["WebImageFile"];
				row["TotalPages"] = reader["TotalPages"];

				dt.Rows.Add(row);

			}

		}

		return dt;


	}

	public static DataTable GetWebImageByPage(
		int moduleId,
		int pageNumber)
	{

		string sqlCommand = @"
SELECT Count(*) 
FROM mp_GalleryImages 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int totalRows = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		int pageSize = 1;

		int totalPages = totalRows / pageSize;
		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder = 0;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		int offset = pageSize * (pageNumber - 1);

		string sqlCommand1 = $@"
SELECT  
i.ItemID,  
i.ModuleID,  
{totalPages.ToString(CultureInfo.InvariantCulture)} As TotalPages 
FROM mp_GalleryImages i 
WHERE i.ModuleID = ?ModuleID 
ORDER BY i.DisplayOrder, i.ItemID ";

		if (pageNumber > 1)
		{
			sqlCommand1 += $@"LIMIT {offset.ToString(CultureInfo.InvariantCulture)}, {pageSize.ToString(CultureInfo.InvariantCulture)} ";
		}
		else
		{
			sqlCommand1 += $@"LIMIT {pageSize.ToString(CultureInfo.InvariantCulture)} ";
		}

		sqlCommand += " ; ";

		var arParams1 = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		DataTable dt = new DataTable();
		dt.Columns.Add("ItemID", typeof(int));
		dt.Columns.Add("TotalPages", typeof(int));

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand1.ToString(),
			arParams1))
		{

			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["ItemID"] = reader["ItemID"];
				row["TotalPages"] = reader["TotalPages"];
				dt.Rows.Add(row);
			}

		}

		return dt;

	}




}
