using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;


namespace mojoPortal.Data;

public static class DBBlog
{

	public static IDataReader GetBlogs(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime)
	{
		string sqlCommand = @"

SELECT SettingValue 
FROM mp_ModuleSettings 
WHERE SettingName = 'BlogEntriesToShowSetting' 
AND ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsToShow = int.Parse(ConfigurationManager.AppSettings["DefaultBlogPageSize"]);

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams))
		{
			if (reader.Read())
			{
				try
				{
					rowsToShow = Convert.ToInt32(reader["SettingValue"]);
				}
				catch { }

			}
		}

		string sqlCommand1 = $@"
SELECT b.*, 
u.Name, 
u.LoginName, 
u.Email 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND ?BeginDate >= b.StartDate  
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime  
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
ORDER BY  b.StartDate DESC  
LIMIT {rowsToShow.ToString()} ;";

		var arParams1 = new List<MySqlParameter>
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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1);

	}

	/// <summary>
	/// gets top 20 related posts ordered by created date desc
	/// based on categories of current post itemid
	/// </summary>
	/// <param name="itemId"></param>
	/// <returns></returns>
	public static IDataReader GetRelatedPosts(int itemId)
	{
		string sqlCommand = @"           
SELECT DISTINCT 
    b.ItemID, 
    b.ItemUrl, 
    b.Heading 
FROM mp_Blogs b 
JOIN (SELECT ItemID, CategoryID FROM mp_BlogItemCategories) bc1 
ON bc1.ItemID = b.ItemID 
JOIN (SELECT ItemID, CategoryID FROM mp_BlogItemCategories WHERE ItemID = ?ItemID) bc2 
ON bc1.CategoryID = bc2.CategoryID 
WHERE b.ItemID <> ?ItemID 
AND b.StartDate <= ?CurrentTime  
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
ORDER BY  b.StartDate DESC  
LIMIT 20 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static IDataReader GetBlogsForFeed(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime)
	{
		string sqlCommand = @"

SELECT SettingValue 
FROM mp_ModuleSettings 
WHERE SettingName = 'MaxFeedItems' 
AND ModuleID = ?ModuleID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsToShow = 20;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams))
		{
			if (reader.Read())
			{
				try
				{
					rowsToShow = Convert.ToInt32(reader["SettingValue"]);
				}
				catch { }

			}
		}

		string sqlCommand1 = $@"
SELECT 
    b.*, 
    u.Name, 
    u.LoginName, 
    u.Email 
FROM 
    mp_Blogs b 
LEFT OUTER JOIN	
    mp_Users u 
ON 
    b.UserGuid = u.UserGuid 
WHERE 
    b.ModuleID = ?ModuleID  
AND 
    ?BeginDate >= b.StartDate  
AND 
    b.IsPublished = 1 
AND 
    b.StartDate <= ?CurrentTime  
AND 
    (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
ORDER BY  
    b.StartDate DESC  
LIMIT 
    {rowsToShow.ToString()}; ";

		var arParams1 = new List<MySqlParameter>
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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1);

	}

	public static IDataReader GetBlogsForMetaWeblogApi(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT SettingValue 
FROM mp_ModuleSettings 
WHERE SettingName = 'MaxMetaweblogRecentItems' 
AND ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsToShow = 100;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams))
		{
			if (reader.Read())
			{
				try
				{
					rowsToShow = Convert.ToInt32(reader["SettingValue"]);
				}
				catch { }

			}
		}

		string sqlCommand1 = $@"
SELECT b.*, 
    u.Name, 
    u.LoginName, 
    u.Email 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND ?BeginDate >= b.StartDate  
AND b.StartDate <= ?CurrentTime  
ORDER BY  b.StartDate DESC  
LIMIT {rowsToShow.ToString()}; ";

		var arParams1 = new List<MySqlParameter>
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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1);

	}

	public static IDataReader GetBlogCategoriesForMetaWeblogApi(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT SettingValue 
FROM mp_ModuleSettings 
WHERE SettingName = 'MaxMetaweblogRecentItems' 
AND ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsToShow = 100;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams))
		{
			if (reader.Read())
			{
				try
				{
					rowsToShow = Convert.ToInt32(reader["SettingValue"]);
				}
				catch { }

			}
		}

		string sqlCommand1 = $@"
SELECT 
bic.ID, 
bic.ItemID, 
bic.CategoryID, 
bc.Category 
FROM mp_BlogItemCategories bic 
JOIN	mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
JOIN ( 
SELECT b.*, 
u.Name, 
u.LoginName, 
u.Email 
FROM	mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND ?BeginDate >= b.StartDate  
AND b.StartDate <= ?CurrentTime  
ORDER BY  b.StartDate DESC  
LIMIT {rowsToShow.ToString()} ) b 
ON b.ItemID = bic.ItemID 
ORDER BY bc.Category ; ";

		var arParams1 = new List<MySqlParameter>
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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1);

	}

	public static int GetCountClosed(
		int moduleId,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND EndDate < ?CurrentTime ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			 ConnectionString.GetRead(),
			 sqlCommand,
			 arParams));

	}

	public static IDataReader GetClosed(
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountClosed(moduleId, currentTime);

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
SELECT b.*, 
COALESCE(u.UserID, -1) AS UserID, 
u.Name, 
u.FirstName, 
u.LastName, 
u.LoginName, 
u.Email, 
u.AvatarUrl, 
u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND b.EndDate < ?CurrentTime  
ORDER BY  b.StartDate DESC  
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

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);


	}

	public static IDataReader GetAttachmentsForClosed(
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.*, 
    b2.ShowDownloadLink 
FROM mp_FileAttachment bic 
JOIN mp_Blogs b2 
ON 
    b2.BlogGuid = bic.ItemGuid 
JOIN ( 
    SELECT 
        b.* 
    FROM 
        mp_Blogs b 
    WHERE 
        b.ModuleID = ?ModuleID  
    AND 
        b.EndDate < ?CurrentTime  
    ORDER BY  
        b.StartDate DESC  
    LIMIT 
        ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.BlogGuid = bic.ItemGuid ";

		sqlCommand += "; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetCategoriesForClosed(
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.ID, 
    bic.ItemID, 
    bic.CategoryID, 
    bc.Category 
FROM mp_BlogItemCategories bic 
JOIN mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
JOIN	( 
SELECT 
    b.*, 
    u.Name, 
    u.LoginName, 
    u.Email 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND b.EndDate < ?CurrentTime  
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += "b ON b.ItemID = bic.ItemID ";

		sqlCommand += "ORDER BY bc.Category; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static int GetCountOfDrafts(
		int moduleId,
		Guid userGuid,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND (?UserGuid = '00000000-0000-0000-0000-000000000000' OR UserGuid  = ?UserGuid)  
AND ((StartDate > ?CurrentTime) OR (IsPublished = 0)) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			 ConnectionString.GetRead(),
			 sqlCommand,
			 arParams));

	}

	public static IDataReader GetPageOfDrafts(
		int moduleId,
		Guid userGuid,
		DateTime currentTime,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountOfDrafts(moduleId, userGuid, currentTime);

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
    b.*, 
    COALESCE(u.UserID, -1) AS UserID, 
        u.Name, 
        u.FirstName, 
        u.LastName, 
        u.LoginName, 
        u.Email, 
        u.AvatarUrl, 
        u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND (?UserGuid = '00000000-0000-0000-0000-000000000000' OR b.UserGuid  = ?UserGuid)  
AND ((b.StartDate > ?CurrentTime) OR (b.IsPublished = 0)) 
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ; ";
		}

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);


	}


	public static int GetCount(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND ?BeginDate >= StartDate  
AND IsPublished = 1 
AND StartDate <= ?CurrentTime  
AND (EndDate IS NULL OR EndDate > ?CurrentTime) ;";

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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			 ConnectionString.GetRead(),
			 sqlCommand,
			 arParams));

	}

	public static IDataReader GetPage(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(moduleId, beginDate, currentTime);

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
    b.*, 
    COALESCE(u.UserID, -1) AS UserID, 
        u.Name, 
        u.FirstName, 
        u.LastName, 
        u.LoginName, 
        u.Email, 
        u.AvatarUrl, 
        u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND ?BeginDate >= b.StartDate  
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime  
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
ORDER BY  b.StartDate DESC  
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

			new("?BeginDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = beginDate
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);


	}

	public static IDataReader GetAttachmentsForPage(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.*, 
    b2.ShowDownloadLink 
FROM mp_FileAttachment bic 
JOIN mp_Blogs b2 
ON 
b2.BlogGuid = bic.ItemGuid 
JOIN ( 
SELECT b.* 
FROM mp_Blogs b 
WHERE b.ModuleID = ?ModuleID  
AND ?BeginDate >= b.StartDate  
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime  
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.BlogGuid = bic.ItemGuid ; ";

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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetAttachmentsForPage(
		int moduleId,
		int categoryId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.*, 
    b2.ShowDownloadLink 
FROM mp_FileAttachment bic 
JOIN mp_Blogs b2 
ON 
b2.BlogGuid = bic.ItemGuid 
JOIN ( 
SELECT b.* 
FROM mp_Blogs b 
JOIN mp_BlogItemCategories bic2 
ON b.ItemID = bic2.ItemID 
WHERE b.ModuleID = ?ModuleID   
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND  bic2.CategoryID = ?CategoryID   
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.BlogGuid = bic.ItemGuid ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetAttachmentsForPage(
		int month,
		int year,
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.*, 
    b2.ShowDownloadLink 
FROM mp_FileAttachment bic 
JOIN mp_Blogs b2 
ON b2.BlogGuid = bic.ItemGuid 
JOIN	( 
    SELECT b.* 
    FROM mp_Blogs b 
    WHERE b.ModuleID = ?ModuleID  
    AND b.IsPublished = 1 
    AND b.StartDate <= ?CurrentTime 
    AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
    AND DATE_FORMAT(b.StartDate, '%Y') = ?Year  
    AND MONTH(b.StartDate) = ?Month  
    ORDER BY b.StartDate DESC  
    LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.BlogGuid = bic.ItemGuid ; ";


		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Year", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = year
			},

			new("?Month", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = month
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetCategoriesForPage(
		int moduleId,
		DateTime beginDate,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.ID, 
    bic.ItemID, 
    bic.CategoryID, 
    bc.Category 
FROM mp_BlogItemCategories bic 
JOIN mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
JOIN ( 
    SELECT 
        b.*, 
        u.Name, 
        u.LoginName, 
        u.Email 
    FROM mp_Blogs b 
    LEFT OUTER JOIN	mp_Users u 
    ON b.UserGuid = u.UserGuid 
    WHERE b.ModuleID = ?ModuleID  
    AND ?BeginDate >= b.StartDate  
    AND b.IsPublished = 1 
    AND b.StartDate <= ?CurrentTime  
    AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
    ORDER BY  b.StartDate DESC  
    LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.ItemID = bic.ItemID ";

		sqlCommand += "ORDER BY bc.Category; ";

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
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static int GetCountByMonth(
		int month,
		int year,
		int moduleId,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND IsPublished = 1 
AND StartDate <= ?CurrentTime 
AND (EndDate IS NULL OR EndDate > ?CurrentTime)  
AND DATE_FORMAT(StartDate, '%Y') = ?Year  
AND MONTH(StartDate)  = ?Month  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Year", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = year
			},

			new("?Month", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = month
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			 ConnectionString.GetRead(),
			 sqlCommand,
			 arParams));

	}

	public static IDataReader GetBlogEntriesByMonth(
		int month,
		int year,
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByMonth(month, year, moduleId, currentTime);

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
    .*, 
    COALESCE(u.UserID, -1) AS UserID, 
    u.Name, 
    u.FirstName, 
    u.LastName, 
    u.LoginName, 
    u.Email, 
    u.AvatarUrl, 
    u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND DATE_FORMAT(b.StartDate, '%Y') = ?Year  
AND MONTH(b.StartDate)  = ?Month  
ORDER BY b.StartDate DESC 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ; ";
		}

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Year", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = year
			},

			new("?Month", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = month
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetCategoriesForPage(
		int month,
		int year,
		int moduleId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.ID, 
    bic.ItemID, 
    bic.CategoryID, 
    bc.Category 
FROM mp_BlogItemCategories bic 
JOIN mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
JOIN ( 
SELECT b.*, 
u.Name, 
u.LoginName, 
u.Email 
FROM	mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ModuleID = ?ModuleID  
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND DATE_FORMAT(b.StartDate, '%Y') = ?Year  
AND MONTH(b.StartDate)  = ?Month  
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.ItemID = bic.ItemID ";

		sqlCommand += "ORDER BY bc.Category; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Year", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = year
			},

			new("?Month", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = month
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}


	public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND IsPublished = 1 
AND StartDate <= ?CurrentTime 
AND (EndDate IS NULL OR EndDate > ?CurrentTime)  
AND DATE_FORMAT(StartDate, '%Y') = ?Year  
AND MONTH(StartDate)  = ?Month  
ORDER BY StartDate DESC ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Year", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = year
			},

			new("?Month", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = month
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static int GetCountByCategory(
		int moduleId,
		int categoryId,
		DateTime currentTime)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Blogs b 
JOIN mp_BlogItemCategories bic 
ON b.ItemID = bic.ItemID 
WHERE b.ModuleID = ?ModuleID  
AND  bic.CategoryID = ?CategoryID   
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime  
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime) ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			 ConnectionString.GetRead(),
			 sqlCommand,
			 arParams));

	}

	public static IDataReader GetEntriesByCategory(
		int moduleId,
		int categoryId,
		DateTime currentTime,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByCategory(moduleId, categoryId, currentTime);

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
    b.*, 
    COALESCE(u.UserID, -1) AS UserID, 
    u.Name, 
    u.FirstName, 
    u.LastName, 
    u.LoginName, 
    u.Email, 
    u.AvatarUrl, 
    u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
JOIN mp_BlogItemCategories bic 
ON b.ItemID = bic.ItemID 
WHERE b.ModuleID = ?ModuleID   
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND  bic.CategoryID = ?CategoryID   
ORDER BY b.StartDate DESC   
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ; ";
		}



		// categoryid is uint in db so don't allow -1
		if (categoryId <= -1) { categoryId = 1; }

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);



	}

	public static IDataReader GetCategoriesForPage(
		int moduleId,
		int categoryId,
		DateTime currentTime,
		int pageNumber,
		int pageSize)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;

		string sqlCommand = @"
SELECT 
    bic.ID, 
    bic.ItemID, 
    bic.CategoryID, 
    bc.Category 
FROM mp_BlogItemCategories bic 
JOIN mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
JOIN ( 
SELECT b.* 
FROM mp_Blogs b 
JOIN mp_BlogItemCategories bic2 
ON b.ItemID = bic2.ItemID 
WHERE b.ModuleID = ?ModuleID   
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND  bic2.CategoryID = ?CategoryID   
ORDER BY  b.StartDate DESC  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ") b ON b.ItemID = bic.ItemID ";

		sqlCommand += "ORDER BY bc.Category; ";


		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
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
			sqlCommand,
			arParams);

	}

	public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
	{
		string sqlCommand = @"
SELECT b.* 
FROM mp_Blogs b 
JOIN mp_BlogItemCategories bic 
ON b.ItemID = bic.ItemID 
WHERE b.ModuleID = ?ModuleID   
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentTime 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentTime)  
AND  bic.CategoryID = ?CategoryID   
ORDER BY b.StartDate DESC ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);



	}


	public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
	{
		string sqlCommand = @"
SELECT  
    b.ItemUrl, 
    b.LastModUtc, 
    b.ItemID, 
    b.ModuleID, 
    pm.PageID 
FROM mp_Blogs b 
JOIN mp_Modules m 
ON b.ModuleID = m.ModuleID 
JOIN mp_PageModules pm 
ON b.ModuleID = pm.ModuleID 
WHERE 
    m.SiteID = ?SiteID 
    AND b.IncludeInSiteMap = 1 
    AND b.IsPublished = 1 
    AND b.StartDate <= ?CurrentDateTime  
    AND (b.EndDate IS NULL OR b.EndDate > ?CurrentDateTime)  
    AND b.ItemUrl <> ''  
ORDER BY  b.StartDate DESC  ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?CurrentDateTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentUtcDateTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

	public static IDataReader GetBlogsForNewsMap(int siteId, DateTime utcThresholdTime)
	{
		string sqlCommand = @"
SELECT  
    b.ItemUrl, 
    b.LastModUtc, 
    b.ItemID, 
    b.ModuleID, 
    b.HeadlineImageUrl, 
    b.PubAccess, 
    b.PubGenres, 
    b.PubGeoLocations, 
    b.PubKeyWords, 
    b.PubLanguage,
    b.PubName, 
    b.PubStockTickers, 
    b.StartDate,
    b.Title, 
    b.Heading, 
    pm.PageID 
FROM mp_Blogs b 
JOIN mp_Modules m 
ON b.ModuleID = m.ModuleID 
JOIN mp_PageModules pm 
ON b.ModuleID = pm.ModuleID 
WHERE 
    m.SiteID = ?SiteID 
    AND b.IncludeInNews = 1 
    AND b.IsPublished = 1 
    AND b.StartDate >= ?UtcThresholdTime  
    AND b.ItemUrl <> ''  
ORDER BY  b.StartDate DESC  ;  ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?UtcThresholdTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = utcThresholdTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}


	public static IDataReader GetDrafts(
		int moduleId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND ((StartDate > ?BeginDate) OR (IsPublished = 0))  
ORDER BY  StartDate DESC  ;  ";

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
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}





	public static IDataReader GetBlogsByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT 
    b.*, 
    m.ModuleTitle, 
    m.ViewRoles, 
    md.FeatureName, 
    COALESCE(u.UserID, -1) AS UserID, 
    u.Name, 
    u.FirstName, 
    u.LastName, 
    u.LoginName, 
    u.Email, 
    u.AvatarUrl 
FROM mp_Blogs b 
JOIN mp_Modules m 
ON b.ModuleID = m.ModuleID 
JOIN mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
JOIN mp_PageModules pm 
ON m.ModuleID = pm.ModuleID 
JOIN mp_Pages p 
ON p.PageID = pm.PageID 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE p.SiteID = ?SiteID 
AND pm.PageID = ?PageID ; ";

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
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}


	public static IDataReader GetBlogStats(int moduleId)
	{
		string sqlCommand = @"
SELECT  
ModuleID, 
EntryCount, 
CommentCount 
FROM mp_BlogStats 
WHERE ModuleID = ?ModuleID  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}


	public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
	{
		string sqlCommand = @"
SELECT  
    MONTH(StartDate) AS `Month`, 
    DATE_FORMAT(StartDate, '%M') AS `MonthName`, 
    YEAR(StartDate) AS `Year`, 
    1 AS `Day`, 
    count(*) AS `Count` 
FROM mp_Blogs 
WHERE ModuleID = ?ModuleID  
AND IsPublished = 1 
AND StartDate <= ?CurrentDate
GROUP BY 
    `Year`, 
    `Month`, 
    `MonthName`  
ORDER BY 	
    `Year` desc, 
    `Month` desc ; ";

		var sqlParams = new List<MySqlParameter>()
			{
				new("?ModuleID", MySqlDbType.Int32)
				{   Direction = ParameterDirection.Input,
					Value = moduleId
				},

				new("?CurrentDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = currentTime
				}
			};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			sqlParams.ToArray());
	}

	public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
	{


		string sqlCommand = @"
SELECT  
    b.*, (
        SELECT b2.ItemUrl 
        FROM mp_Blogs b2 
        WHERE b2.IsPublished = 1 
        AND b2.StartDate <= ?CurrentTime 
        AND (
            b2.EndDate IS NULL 
            OR b2.EndDate > ?CurrentTime
            ) 
        AND (
            b2.StartDate > b.StartDate
        ) 
        AND b2.ModuleID = b.ModuleID 
        AND b2.ItemUrl IS NOT NULL 
        AND (b2.ItemUrl <> '') 
        ORDER BY b2.StartDate LIMIT 1 
    ) 
    AS NextPost, (
        SELECT b4.Title 
        FROM mp_Blogs b4 
        WHERE b4.IsPublished = 1 
        AND b4.StartDate <= ?CurrentTime 
        AND (b4.EndDate IS NULL 
        OR b4.EndDate > ?CurrentTime) 
        AND (b4.StartDate > b.StartDate) 
        AND b4.ModuleID = b.ModuleID 
        AND b4.ItemUrl IS NOT NULL 
        AND (b4.ItemUrl <> '') 
        ORDER BY b4.StartDate LIMIT 1 
    ) AS NextPostTitle, 
    COALESCE(
        (
            SELECT b6.ItemID 
            FROM mp_Blogs b6 
            WHERE b6.IsPublished = 1 
            AND b6.StartDate <= ?CurrentTime 
            AND (b6.EndDate IS NULL 
            OR b6.EndDate > ?CurrentTime) 
            AND (b6.StartDate > b.StartDate) 
            AND b6.ModuleID = b.ModuleID  
            ORDER BY b6.StartDate LIMIT 1 
        ),-1
    ) AS NextItemID, (
        SELECT b3.ItemUrl 
        FROM mp_Blogs b3 
        WHERE b3.IsPublished = 1 
        AND b3.StartDate <= ?CurrentTime 
        AND (b3.EndDate IS NULL OR b3.EndDate > ?CurrentTime) 
        AND (b3.StartDate < b.StartDate) 
        AND b3.ModuleID = b.ModuleID AND b3.ItemUrl IS NOT NULL 
        AND (b3.ItemUrl <> '') 
        ORDER BY b3.StartDate DESC LIMIT 1 
    ) AS PreviousPost, (
        SELECT b5.Title 
        FROM mp_Blogs b5 
        WHERE b5.IsPublished = 1 
        AND b5.StartDate <= ?CurrentTime 
        AND (b5.EndDate IS NULL 
        OR b5.EndDate > ?CurrentTime) 
        AND (b5.StartDate < b.StartDate) 
        AND b5.ModuleID = b.ModuleID 
        AND b5.ItemUrl IS NOT NULL 
        AND (b5.ItemUrl <> '') 
        ORDER BY b5.StartDate DESC LIMIT 1 
    ) AS PreviousPostTitle,  
    COALESCE(
        (
            SELECT b7.ItemID 
            FROM mp_Blogs b7 
            WHERE b7.IsPublished = 1 
            AND b7.StartDate <= ?CurrentTime 
            AND (b7.EndDate IS NULL OR b7.EndDate > ?CurrentTime) 
            AND (b7.StartDate < b.StartDate) 
            AND b7.ModuleID = b.ModuleID  
            ORDER BY b7.StartDate DESC LIMIT 1 
        ),-1
    ) AS PreviousItemID,  
        COALESCE(u.UserID, -1) AS UserID, 
        u.Name, 
        u.FirstName, 
        u.LastName, 
        u.LoginName, 
        u.Email, 
        u.AvatarUrl, 
        u.AuthorBio 
FROM mp_Blogs b 
LEFT OUTER JOIN	mp_Users u 
ON b.UserGuid = u.UserGuid 
WHERE b.ItemID = ?ItemID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = currentTime
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}



	public static bool DeleteBlog(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_Blogs 
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
			ConnectionString.GetWrite(),
			sqlCommand,
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
DELETE FROM mp_BlogItemCategories 
WHERE ItemID IN (
    SELECT ItemID 
    FROM mp_Blogs 
    WHERE ModuleID = ?ModuleID 
) ;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		string sqlCommand1 = @"
DELETE FROM mp_FriendlyUrls 
WHERE PageGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID = ?ModuleID 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams);

		string sqlCommand2 = @"
DELETE FROM mp_ContentHistory 
WHERE ContentGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID = ?ModuleID 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand2.ToString(),
			arParams);

		string sqlCommand3 = @"
DELETE FROM mp_ContentRating 
WHERE ContentGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID = ?ModuleID 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand3.ToString(),
			arParams);

		string sqlCommand4 = @"
DELETE FROM mp_BlogCategories 
WHERE ModuleID = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand4.ToString(),
			arParams);

		string sqlCommand5 = @"
DELETE FROM mp_BlogStats 
WHERE ModuleID = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand5.ToString(),
			arParams);

		string sqlCommand6 = @"
DELETE FROM mp_BlogComments 
WHERE ModuleID = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand6.ToString(),
			arParams);

		string sqlCommand7 = @"
DELETE FROM mp_Blogs 
WHERE ModuleID = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand7.ToString(),
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
DELETE FROM mp_BlogItemCategories 
WHERE ItemID IN (
    SELECT ItemID 
    FROM mp_Blogs 
    WHERE ModuleID IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
) ;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		string sqlCommand1 = @"
DELETE FROM mp_FriendlyUrls 
WHERE PageGuid IN (
    SELECT ModuleGuid 
    FROM mp_Blogs 
    WHERE ModuleID IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams);

		string sqlCommand2 = @"
DELETE FROM mp_FriendlyUrls 
WHERE PageGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand2.ToString(),
			arParams);

		string sqlCommand3 = @"
DELETE FROM mp_ContentHistory 
WHERE ContentGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand3.ToString(),
			arParams);

		string sqlCommand4 = @"
DELETE FROM mp_ContentRating 
WHERE ContentGuid IN (
    SELECT BlogGuid 
    FROM mp_Blogs 
    WHERE ModuleID IN (
        SELECT ModuleID 
        FROM mp_Modules 
        WHERE SiteID = ?SiteID
    ) 
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand4.ToString(),
			arParams);

		string sqlCommand5 = @"
DELETE FROM mp_BlogCategories 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand5.ToString(),
			arParams);

		string sqlCommand6 = @"
DELETE FROM mp_BlogStats 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand6.ToString(),
			arParams);

		string sqlCommand7 = @"
DELETE FROM mp_BlogComments 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand7.ToString(),
			arParams);

		string sqlCommand8 = @"
DELETE FROM mp_Blogs 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand8.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static int AddBlog(
		Guid blogGuid,
		Guid moduleGuid,
		int moduleId,
		string userName,
		string title,
		string excerpt,
		string description,
		DateTime startDate,
		bool isInNewsletter,
		bool includeInFeed,
		int allowCommentsForDays,
		string location,
		Guid userGuid,
		DateTime createdDate,
		string itemUrl,
		string metaKeywords,
		string metaDescription,
		string compiledMeta,
		bool isPublished,
		string subTitle,
		DateTime endDate,
		bool approved,
		Guid approvedBy,
		DateTime approvedDate,
		bool showAuthorName,
		bool showAuthorAvatar,
		bool showAuthorBio,
		bool includeInSearch,
		bool useBingMap,
		string mapHeight,
		string mapWidth,
		bool showMapOptions,
		bool showZoomTool,
		bool showLocationInfo,
		bool useDrivingDirections,
		string mapType,
		int mapZoom,
		bool showDownloadLink,
		bool includeInSiteMap,
		bool excludeFromRecentContent,

		bool includeInNews,
		string pubName,
		string pubLanguage,
		string pubAccess,
		string pubGenres,
		string pubKeyWords,
		string pubGeoLocations,
		string pubStockTickers,
		string headlineImageUrl,
		bool includeImageInExcerpt,
			bool includeImageInPost
		)
	{

		#region bit conversion

		string inNews;
		if (isInNewsletter)
		{
			inNews = "1";
		}
		else
		{
			inNews = "0";
		}

		string inFeed;
		if (includeInFeed)
		{
			inFeed = "1";
		}
		else
		{
			inFeed = "0";
		}

		string isPub;
		if (isPublished)
		{
			isPub = "1";
		}
		else
		{
			isPub = "0";
		}

		int intApproved = 0;
		if (approved) { intApproved = 1; }

		int intshowAuthorName = 0;
		if (showAuthorName) { intshowAuthorName = 1; }

		int intshowAuthorAvatar = 0;
		if (showAuthorAvatar) { intshowAuthorAvatar = 1; }

		int intshowAuthorBio = 0;
		if (showAuthorBio) { intshowAuthorBio = 1; }

		int intincludeInSearch = 0;
		if (includeInSearch) { intincludeInSearch = 1; }

		int intuseBingMap = 0;
		if (useBingMap) { intuseBingMap = 1; }

		int intshowMapOptions = 0;
		if (showMapOptions) { intshowMapOptions = 1; }

		int intshowZoomTool = 0;
		if (showZoomTool) { intshowZoomTool = 1; }

		int intshowLocationInfo = 0;
		if (showLocationInfo) { intshowLocationInfo = 1; }

		int intuseDrivingDirections = 0;
		if (useDrivingDirections) { intuseDrivingDirections = 1; }

		int intshowDownloadLink = 0;
		if (showDownloadLink) { intshowDownloadLink = 1; }

		int intincludeInSiteMap = 0;
		if (includeInSiteMap) { intincludeInSiteMap = 1; }

		int intExcludeRecent = 0;
		if (excludeFromRecentContent) { intExcludeRecent = 1; }

		int intincludeInNews = 0;
		if (includeInNews) { intincludeInNews = 1; }

		int intincludeImageInExcerpt = 0;
		if (includeImageInExcerpt) { intincludeImageInExcerpt = 1; }

		int intincludeImageInPost = 0;
		if (includeImageInPost) { intincludeImageInPost = 1; }


		#endregion

		string sqlCommand = @"
INSERT INTO mp_Blogs (  
    ModuleID, 
    CreatedByUser, 
    CreatedDate, 
    Heading, 
    Abstract, 
    Description, 
    StartDate, 
    AllowCommentsForDays, 
    BlogGuid, 
    ModuleGuid, 
    Location, 
    UserGuid, 
    LastModUserGuid, 
    LastModUtc, 
    ItemUrl, 
    MetaKeywords, 
    MetaDescription, 
    CompiledMeta, 
    SubTitle, 
    EndDate, 
    Approved, 
    ApprovedDate, 
    ApprovedBy, 
    ShowAuthorName, 
    ShowAuthorAvatar, 
    ShowAuthorBio, 
    IncludeInSearch, 
    IncludeInSiteMap, 
    UseBingMap, 
    MapHeight, 
    MapWidth, 
    ShowMapOptions, 
    ShowZoomTool, 
    ShowLocationInfo, 
    UseDrivingDirections, 
    MapType, 
    MapZoom, 
    ShowDownloadLink, 
    ExcludeFromRecentContent, 
    IncludeInNews, 
    PubName, 
    PubLanguage, 
    PubAccess, 
    PubGenres, 
    PubKeyWords, 
    PubGeoLocations, 
    PubStockTickers, 
    HeadlineImageUrl, 
    IncludeImageInExcerpt, 
    IncludeImageInPost, 
    IsInNewsletter, 
    IsPublished, 
    IncludeInFeed 
)
VALUES (
    ?ModuleID , 
    ?UserName  , 
    ?CreatedDate, 
    ?Heading , 
    ?Abstract , 
    ?Description  , 
    ?StartDate , 
    ?AllowCommentsForDays , 
    ?BlogGuid, 
    ?ModuleGuid, 
    ?Location, 
    ?UserGuid, 
    ?UserGuid, 
    ?CreatedDate, 
    ?ItemUrl, 
    ?MetaKeywords, 
    ?MetaDescription, 
    ?CompiledMeta, 
    ?SubTitle, 
    ?EndDate, 
    ?Approved, 
    ?ApprovedDate, 
    ?ApprovedBy, 
    ?ShowAuthorName, 
    ?ShowAuthorAvatar, 
    ?ShowAuthorBio, 
    ?IncludeInSearch, 
    ?IncludeInSiteMap, 
    ?UseBingMap, 
    ?MapHeight, 
    ?MapWidth, 
    ?ShowMapOptions, 
    ?ShowZoomTool, 
    ?ShowLocationInfo, 
    ?UseDrivingDirections, 
    ?MapType, 
    ?MapZoom, 
    ?ShowDownloadLink, 
    ?ExcludeFromRecentContent, 
    ?IncludeInNews, 
    ?PubName, 
    ?PubLanguage, 
    ?PubAccess, 
    ?PubGenres, 
    ?PubKeyWords, 
    ?PubGeoLocations, 
    ?PubStockTickers, 
    ?HeadlineImageUrl, 
    ?IncludeImageInExcerpt, 
    ?IncludeImageInPost, 
    " + inNews + ", " + isPub + " " + inFeed + " SELECT LAST_INSERT_ID()" +
") ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = userName
			},

			new("?Heading", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Abstract", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = excerpt
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?StartDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = startDate
			},

			new("?AllowCommentsForDays", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowCommentsForDays
			},

			new("?BlogGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = blogGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Location", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = location
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdDate
			},

			new("?ItemUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = itemUrl
			},

			new("?MetaKeywords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = metaKeywords
			},

			new("?MetaDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = metaDescription
			},

			new("?CompiledMeta", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMeta
			},

			new("?SubTitle", MySqlDbType.VarChar, 500)
			{
				Direction = ParameterDirection.Input,
				Value = subTitle
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?Approved", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intApproved
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			},

			new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?ShowAuthorName", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorName
			},

			new("?ShowAuthorAvatar", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorAvatar
			},

			new("?ShowAuthorBio", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorBio
			},

			new("?IncludeInSearch", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSearch
			},

			new("?IncludeInSiteMap", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSiteMap
			},

			new("?UseBingMap", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intuseBingMap
			},

			new("?MapHeight", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = mapHeight
			},

			new("?MapWidth", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = mapWidth
			},

			new("?ShowMapOptions", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowMapOptions
			},

			new("?ShowZoomTool", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowZoomTool
			},

			new("?ShowLocationInfo", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowLocationInfo
			},

			new("?UseDrivingDirections", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intuseDrivingDirections
			},

			new("?MapType", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = mapType
			},

			new("?MapZoom", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = mapZoom
			},

			new("?ShowDownloadLink", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowDownloadLink
			},

			new("?ExcludeFromRecentContent", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intExcludeRecent
			},

			new("?IncludeInNews", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInNews
			},

			new("?PubName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubName
			},

			new("?PubLanguage", MySqlDbType.VarChar, 7)
			{
				Direction = ParameterDirection.Input,
				Value = pubLanguage
			},

			new("?PubAccess", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = pubAccess
			},

			new("?PubGenres", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubGenres
			},

			new("?PubKeyWords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubKeyWords
			},

			new("?PubGeoLocations", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubGeoLocations
			},

			new("?PubStockTickers", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubStockTickers
			},

			new("?HeadlineImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = headlineImageUrl
			},

			new("?IncludeImageInExcerpt", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeImageInExcerpt
			},

			new("?IncludeImageInPost", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeImageInPost
			}
		};


		if (endDate < DateTime.MaxValue)
		{
			arParams.Add(new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			});
		}
		else
		{
			arParams.Add(new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		if (endDate < DateTime.MaxValue)
		{
			arParams.Add(new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = approvedDate
			});
		}
		else
		{
			arParams.Add(new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
		ConnectionString.GetWrite(),
		sqlCommand,
		arParams).ToString());

		string sqlCommand1 = @"
SELECT count(*) 
FROM mp_BlogStats 
WHERE ModuleID = ?ModuleID ;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowCount = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1).ToString());

		if (rowCount > 0)
		{
			string sqlCommand2 = @"
UPDATE mp_BlogStats 
SET EntryCount = EntryCount + 1 
WHERE ModuleID = ?ModuleID ;";

			var arParams2 = new List<MySqlParameter>
			{
				new("?ModuleID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				}
			};

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand2.ToString(),
				arParams2);


		}
		else
		{
			string sqlCommand3 = @"
INSERT INTO mp_BlogStats(
    ModuleGuid, 
    ModuleID, 
    EntryCount, 
    CommentCount, 
    TrackBackCount
) 
VALUES (
    ?ModuleGuid, 
    ?ModuleID, 
    1, 
    0, 
    0
); ";

			var arParams3 = new List<MySqlParameter>
			{
				new("?ModuleID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},

				new("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand3.ToString(),
				arParams3);


		}

		return newID;

	}




	public static bool UpdateBlog(
		int moduleId,
		int itemId,
		string userName,
		string title,
		string excerpt,
		string description,
		DateTime startDate,
		bool isInNewsletter,
		bool includeInFeed,
		int allowCommentsForDays,
		string location,
		Guid lastModUserGuid,
		DateTime lastModUtc,
		string itemUrl,
		string metaKeywords,
		string metaDescription,
		string compiledMeta,
		bool isPublished,
		string subTitle,
		DateTime endDate,
		bool approved,
		Guid approvedBy,
		DateTime approvedDate,
		bool showAuthorName,
		bool showAuthorAvatar,
		bool showAuthorBio,
		bool includeInSearch,
		bool useBingMap,
		string mapHeight,
		string mapWidth,
		bool showMapOptions,
		bool showZoomTool,
		bool showLocationInfo,
		bool useDrivingDirections,
		string mapType,
		int mapZoom,
		bool showDownloadLink,
		bool includeInSiteMap,
		bool excludeFromRecentContent,

		bool includeInNews,
		string pubName,
		string pubLanguage,
		string pubAccess,
		string pubGenres,
		string pubKeyWords,
		string pubGeoLocations,
		string pubStockTickers,
		string headlineImageUrl,
		bool includeImageInExcerpt,
			bool includeImageInPost
		)
	{

		#region bit conversion

		string inNews;
		if (isInNewsletter)
		{
			inNews = "1";
		}
		else
		{
			inNews = "0";
		}

		string inFeed;
		if (includeInFeed)
		{
			inFeed = "1";
		}
		else
		{
			inFeed = "0";
		}

		string isPub;
		if (isPublished)
		{
			isPub = "1";
		}
		else
		{
			isPub = "0";
		}

		int intApproved = 0;
		if (approved) { intApproved = 1; }

		int intshowAuthorName = 0;
		if (showAuthorName) { intshowAuthorName = 1; }

		int intshowAuthorAvatar = 0;
		if (showAuthorAvatar) { intshowAuthorAvatar = 1; }

		int intshowAuthorBio = 0;
		if (showAuthorBio) { intshowAuthorBio = 1; }

		int intincludeInSearch = 0;
		if (includeInSearch) { intincludeInSearch = 1; }

		int intuseBingMap = 0;
		if (useBingMap) { intuseBingMap = 1; }

		int intshowMapOptions = 0;
		if (showMapOptions) { intshowMapOptions = 1; }

		int intshowZoomTool = 0;
		if (showZoomTool) { intshowZoomTool = 1; }

		int intshowLocationInfo = 0;
		if (showLocationInfo) { intshowLocationInfo = 1; }

		int intuseDrivingDirections = 0;
		if (useDrivingDirections) { intuseDrivingDirections = 1; }

		int intshowDownloadLink = 0;
		if (showDownloadLink) { intshowDownloadLink = 1; }

		int intincludeInSiteMap = 0;
		if (includeInSiteMap) { intincludeInSiteMap = 1; }

		int intExcludeRecent = 0;
		if (excludeFromRecentContent) { intExcludeRecent = 1; }

		int intincludeInNews = 0;
		if (includeInNews) { intincludeInNews = 1; }

		int intincludeImageInExcerpt = 0;
		if (includeImageInExcerpt) { intincludeImageInExcerpt = 1; }

		int intincludeImageInPost = 0;
		if (includeImageInPost) { intincludeImageInPost = 1; }

		#endregion

		string sqlCommand = $@"
UPDATE 
    mp_Blogs 
SET  
    Heading = ?Heading,
    Abstract = ?Abstract,
    Description = ?Description,
    IsInNewsletter = {inNews},
    IncludeInFeed = {inFeed},
    IsPublished {isPub},
    Description = ?Description,
    AllowCommentsForDays = ?AllowCommentsForDays,
    Location = ?Location,
    MetaKeywords = ?MetaKeywords,
    MetaDescription = ?MetaDescription,
    CompiledMeta = ?CompiledMeta,
    SubTitle = ?SubTitle,
    EndDate = ?EndDate,
    Approved = ?Approved,
    ApprovedDate = ?ApprovedDate,
    ApprovedBy = ?ApprovedBy,
    ShowAuthorName = ?ShowAuthorName,
    ShowAuthorAvatar = ?ShowAuthorAvatar,
    ShowAuthorBio = ?ShowAuthorBio,
    IncludeInSearch = ?IncludeInSearch,
    IncludeInSiteMap = ?IncludeInSiteMap,
    UseBingMap = ?UseBingMap,
    MapHeight = ?MapHeight,
    MapWidth = ?MapWidth,
    ShowMapOptions = ?ShowMapOptions,
    ShowZoomTool = ?ShowZoomTool,
    ShowLocationInfo = ?ShowLocationInfo,
    UseDrivingDirections = ?UseDrivingDirections,
    MapType = ?MapType,
    MapZoom = ?MapZoom,
    ShowDownloadLink = ?ShowDownloadLink,
    ExcludeFromRecentContent = ?ExcludeFromRecentContent,
    IncludeInNews = ?IncludeInNews,
    PubName = ?PubName,
    PubLanguage = ?PubLanguage,
    PubAccess = ?PubAccess,
    PubGenres = ?PubGenres,
    PubKeyWords = ?PubKeyWords,
    PubGeoLocations = ?PubGeoLocations,
    PubStockTickers = ?PubStockTickers,
    HeadlineImageUrl = ?HeadlineImageUrl,
    IncludeImageInExcerpt = ?IncludeImageInExcerpt,
    IncludeImageInPost = ?IncludeImageInPost,
    ItemUrl = ?ItemUrl,
    LastModUserGuid = ?LastModUserGuid,
    LastModUtc = ?LastModUtc,
    StartDate = ?StartDate 
WHERE 
    ItemID = {itemId.ToString()} ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?UserName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = userName
			},

			new("?Heading", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Abstract", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = excerpt
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?StartDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = startDate
			},

			new("?AllowCommentsForDays", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowCommentsForDays
			},

			new("?Location", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = location
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

			new("?ItemUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = itemUrl
			},

			new("?MetaKeywords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = metaKeywords
			},

			new("?MetaDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = metaDescription
			},

			new("?CompiledMeta", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMeta
			},

			new("?SubTitle", MySqlDbType.VarChar, 500)
			{
				Direction = ParameterDirection.Input,
				Value = subTitle
			},

			new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?Approved", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intApproved
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			},

			new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?ShowAuthorName", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorName
			},

			new("?ShowAuthorAvatar", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorAvatar
			},

			new("?ShowAuthorBio", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowAuthorBio
			},

			new("?IncludeInSearch", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSearch
			},

			new("?IncludeInSiteMap", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSiteMap
			},

			new("?UseBingMap", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intuseBingMap
			},

			new("?MapHeight", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = mapHeight
			},

			new("?MapWidth", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = mapWidth
			},

			new("?ShowMapOptions", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowMapOptions
			},

			new("?ShowZoomTool", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowZoomTool
			},

			new("?ShowLocationInfo", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowLocationInfo
			},

			new("?UseDrivingDirections", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intuseDrivingDirections
			},

			new("?MapType", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = mapType
			},

			new("?MapZoom", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = mapZoom
			},

			new("?ShowDownloadLink", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intshowDownloadLink
			},

			new("?ExcludeFromRecentContent", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intExcludeRecent
			},

			new("?IncludeInNews", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInNews
			},

			new("?PubName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubName
			},

			new("?PubLanguage", MySqlDbType.VarChar, 7)
			{
				Direction = ParameterDirection.Input,
				Value = pubLanguage
			},

			new("?PubAccess", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = pubAccess
			},

			new("?PubGenres", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubGenres
			},

			new("?PubKeyWords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubKeyWords
			},

			new("?PubGeoLocations", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubGeoLocations
			},

			new("?PubStockTickers", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pubStockTickers
			},

			new("?HeadlineImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = headlineImageUrl
			},

			new("?IncludeImageInExcerpt", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeImageInExcerpt
			},

			new("?IncludeImageInPost", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeImageInPost
			}
		};


		if (endDate < DateTime.MaxValue)
		{
			arParams.Add(new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = endDate
			});
		}
		else
		{
			arParams.Add(new("?EndDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		if (endDate < DateTime.MaxValue)
		{
			arParams.Add(new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = approvedDate
			});
		}
		else
		{
			arParams.Add(new("?ApprovedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
	{
		string sqlCommand = @"
UPDATE 
    mp_Blogs 
SET 
    CommentCount = ?CommentCount 
WHERE 
    BlogGuid = ?BlogGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?BlogGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = blogGuid.ToString()
			},

			new("?CommentCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = commentCount
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;

	}


	public static bool AddBlogComment(
		int moduleId,
		int itemId,
		string name,
		string title,
		string url,
		string comment,
		DateTime dateCreated)
	{
		string sqlCommand = @"
INSERT INTO 
    mp_BlogComments (
        ModuleID, 
        ItemID, 
        Name, 
        Title, 
        URL, 
        Comment, 
        DateCreated
)
VALUES (
    ?ModuleID,
    ?ItemID,
    ?Name,
    ?Title,
    ?URL,
    ?Comment,
    ?DateCreated
);";

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
			},

			new("?Name", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Title", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?URL", MySqlDbType.VarChar, 200)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?Comment", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = comment
			},

			new("?DateCreated", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = dateCreated
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		string sqlCommand1 = @"
Update mp_Blogs 
SET CommentCount = CommentCount + 1 
WHERE ModuleID = ?ModuleID 
AND ItemID = ?ItemID ;";

		var arParams1 = new List<MySqlParameter>
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

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams1);

		string sqlCommand2 = @"
Update mp_BlogStats 
SET CommentCount = CommentCount + 1 
WHERE ModuleID = ?ModuleID  ;";

		var arParams2 = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand2.ToString(),
			arParams2);

		return rowsAffected > 0;

	}

	public static bool DeleteAllCommentsForBlog(int itemId)
	{
		string sqlCommand = @"
DELETE 
FROM mp_BlogComments 
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
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;


	}

	public static bool UpdateCommentStats(int moduleId)
	{
		string sqlCommand = @"
UPDATE 
    mp_BlogStats 
SET CommentCount = (
    SELECT COUNT(*) 
    FROM mp_BlogComments 
    WHERE ModuleID = ?ModuleID
) 
WHERE 
    ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;


	}

	public static bool UpdateEntryStats(int moduleId)
	{
		string sqlCommand = @"
UPDATE mp_BlogStats 
SET EntryCount = (
    SELECT COUNT(*) 
    FROM mp_Blogs 
    WHERE ModuleID = ?ModuleID
) 
WHERE ModuleID = ?ModuleID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;


	}


	public static bool DeleteBlogComment(int blogCommentId)
	{
		string sqlCommand = @"
SELECT ModuleID, ItemID 
FROM mp_BlogComments 
WHERE BlogCommentID = ?BlogCommentID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?BlogCommentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = blogCommentId
			}
		};

		int moduleId = -1;
		int itemId = -1;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams))
		{

			if (reader.Read())
			{
				moduleId = (int)reader["ModuleID"];
				itemId = (int)reader["ItemID"];
			}
		}

		string sqlCommand1 = @"
DELETE FROM mp_BlogComments 
WHERE BlogCommentID = ?BlogCommentID;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?BlogCommentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = blogCommentId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams1);

		if (moduleId > -1)
		{
			string sqlCommand2 = @"
UPDATE mp_Blogs 
SET CommentCount = CommentCount - 1 
WHERE ModuleID = ?ModuleID 
AND ItemID = ?ItemID ;
UPDATE mp_BlogStats 
SET CommentCount = CommentCount - 1 
WHERE ModuleID = ?ModuleID ;";

			var arParams2 = new List<MySqlParameter>
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

			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand2.ToString(),
				arParams);


			return rowsAffected > 0;


		}

		return rowsAffected > 0;

	}


	public static IDataReader GetBlogComments(int moduleId, int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_BlogComments 
WHERE ModuleID = ?ModuleID 
AND ItemID = ?ItemID 
ORDER BY BlogCommentID, DateCreated DESC ;";

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
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}


	public static int AddBlogCategory(
		int moduleId,
		string category)
	{

		string sqlCommand = @"
INSERT INTO mp_BlogCategories (
    ModuleID, 
    Category
)
VALUES (
    ?ModuleID , 
    ?Category   
);    
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?Category", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = category
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams).ToString());

		return newID;

	}

	public static bool UpdateBlogCategory(
		int categoryId,
		string category)
	{

		string sqlCommand = @"
UPDATE mp_BlogCategories 
SET Category = ?Category 
WHERE CategoryID = ?CategoryID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			},

			new("?Category", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = category
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteCategory(int categoryId)
	{
		string sqlCommand = @"
DELETE FROM mp_BlogItemCategories 
WHERE CategoryID = ?CategoryID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		string sqlCommand1 = @"
DELETE FROM mp_BlogCategories 
WHERE CategoryID = ?CategoryID ;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams);

		return rowsAffected > 0;

	}



	public static IDataReader GetCategory(int categoryId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_BlogCategories 
WHERE CategoryID = ?CategoryID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetCategories(int moduleId)
	{
		string sqlCommand = @"
SELECT 
    bc.CategoryID, 
    bc.Category, 
    COUNT(bic.ItemID) As PostCount 
FROM mp_BlogCategories bc 
JOIN mp_BlogItemCategories bic 
ON bc.CategoryID = bic.CategoryID 
JOIN mp_Blogs b 
ON b.ItemID = bic.ItemID 
WHERE b.ModuleID = ?ModuleID 
AND b.IsPublished = 1 
AND b.StartDate <= ?CurrentDate 
AND (b.EndDate IS NULL OR b.EndDate > ?CurrentDate) 
GROUP BY bc.CategoryID, bc.Category 
ORDER BY bc.Category; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?CurrentDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetCategoriesList(int moduleId)
	{
		string sqlCommand = @"
SELECT 
    bc.CategoryID, 
    bc.Category, COUNT(bic.ItemID) As PostCount 
FROM mp_BlogCategories bc 
LEFT OUTER JOIN	mp_BlogItemCategories bic 
ON bc.CategoryID = bic.CategoryID 
WHERE bc.ModuleID = ?ModuleID  
GROUP BY bc.CategoryID, bc.Category; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static int AddBlogItemCategory(
		int itemId,
		int categoryId)
	{

		string sqlCommand = @"
INSERT INTO mp_BlogItemCategories (
    ItemID,
    CategoryID
)
VALUES (
    ?ItemID,
    ?CategoryID
);
SELECT LAST_INSERT_ID();";


		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?CategoryID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = categoryId
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams).ToString());

		return newID;

	}


	public static bool DeleteItemCategories(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_BlogItemCategories 
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
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader GetBlogItemCategories(int itemId)
	{
		string sqlCommand = @"
SELECT 
    bic.ItemID,
    bic.CategoryID,
    bc.Category 
FROM mp_BlogItemCategories bic 
JOIN mp_BlogCategories bc 
ON bc.CategoryID = bic.CategoryID 
WHERE bic.ItemID = ?ItemID 
ORDER BY bc.Category;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}






}
