using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBFriendlyUrl
{
	/// <summary>
	/// Inserts a row in the mp_FriendlyUrls table. Returns new integer id.
	/// </summary>
	/// <param name="itemGuid"> itemGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="pageGuid"> pageGuid </param>
	/// <param name="siteID"> siteID </param>
	/// <param name="friendlyUrl"> friendlyUrl </param>
	/// <param name="realUrl"> realUrl </param>
	/// <param name="isPattern"> isPattern </param>
	/// <returns>int</returns>
	public static int AddFriendlyUrl(
		Guid itemGuid,
		Guid siteGuid,
		Guid pageGuid,
		int siteId,
		string friendlyUrl,
		string realUrl,
		bool isPattern)
	{

		int intIsPattern;
		if (isPattern)
		{
			intIsPattern = 1;
		}
		else
		{
			intIsPattern = 0;
		}


		string sqlCommand = @"
INSERT INTO mp_FriendlyUrls ( 
        SiteID, 
        FriendlyUrl, 
        RealUrl, 
        IsPattern, 
        PageGuid, 
        SiteGuid, 
        ItemGuid 
    )
VALUES ( 
    ?SiteID, 
    ?FriendlyUrl, 
    ?RealUrl, 
    ?IsPattern, 
    ?PageGuid, 
    ?SiteGuid, 
    ?ItemGuid 
);";

		sqlCommand += "SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?FriendlyUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyUrl
			},

			new("?RealUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = realUrl
			},

			new("?IsPattern", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsPattern
			},

			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			}
		};


		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams).ToString());

		return newID;

	}

	public static bool UpdateFriendlyUrl(
		int urlId,
		int siteId,
		Guid pageGuid,
		string friendlyUrl,
		string realUrl,
		bool isPattern)
	{
		int intIsPattern = 0;
		if (isPattern)
		{
			intIsPattern = 1;
		}

		string sqlCommand = @"

UPDATE 
    mp_FriendlyUrls 
SET  
    SiteID = ?SiteID, 
    FriendlyUrl = ?FriendlyUrl, 
    RealUrl = ?RealUrl, 
    PageGuid = ?PageGuid, 
    IsPattern = ?IsPattern 
WHERE  
    UrlID = ?UrlID;";

		var arParams = new List<MySqlParameter>
		{
			new("?UrlID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = urlId
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?FriendlyUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyUrl
			},

			new("?RealUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = realUrl
			},

			new("?IsPattern", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsPattern
			},

			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;

	}


	public static bool DeleteFriendlyUrl(int urlId)
	{
		string sqlCommand = @"
DELETE FROM mp_FriendlyUrls 
WHERE UrlID = ?UrlID;";

		var arParams = new List<MySqlParameter>
		{
			new("?UrlID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = urlId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand, arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteByPageId(int pageId)
	{
		string sqlCommand = @"
DELETE FROM mp_FriendlyUrls 
WHERE RealUrl LIKE '%pageid=" + pageId.ToString() + "' ;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand);

		return rowsAffected > 0;

	}

	public static bool DeleteByPageGuid(Guid pageGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_FriendlyUrls 
WHERE PageGuid = ?PageGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(), sqlCommand, arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetFriendlyUrl(int urlId)
	{
		string sqlCommand = @"
SELECT * 
FROM	mp_FriendlyUrls 
WHERE	UrlID = ?UrlID;";

		var arParams = new List<MySqlParameter>
		{
			new("?UrlID", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = urlId }
		};

		return CommandHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, arParams);
	}


	public static DataTable GetByHostName(string hostName)
	{

		var dt = new DataTable();
		int siteId = 1;

		dt.Columns.Add("UrlID", typeof(int));
		dt.Columns.Add("FriendlyUrl", typeof(string));
		dt.Columns.Add("RealUrl", typeof(string));
		dt.Columns.Add("IsPattern", typeof(bool));


		string sqlCommand = @"
SELECT mp_SiteHosts.SiteID 
FROM mp_SiteHosts 
WHERE mp_SiteHosts.HostName = ?HostName;";

		var arParams = new List<MySqlParameter>
		{
			new("?HostName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostName
			}
		};


		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams))
		{
			if (reader.Read())
			{
				siteId = Convert.ToInt32(reader["SiteID"]);
			}
		}

		string sqlCommand1 = @"
SELECT  * 
FROM	mp_FriendlyUrls 
WHERE	SiteID = ?SiteID;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand1.ToString(),
			arParams1))
		{
			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["UrlID"] = reader["UrlID"];
				row["FriendlyUrl"] = reader["FriendlyUrl"];
				row["RealUrl"] = reader["RealUrl"];
				row["IsPattern"] = reader["IsPattern"];
				dt.Rows.Add(row);
			}
		}

		return dt;
	}

	public static DataTable GetBySite(int siteId)
	{

		var dt = new DataTable();

		dt.Columns.Add("UrlID", typeof(int));
		dt.Columns.Add("FriendlyUrl", typeof(string));
		dt.Columns.Add("RealUrl", typeof(string));
		dt.Columns.Add("IsPattern", typeof(bool));

		string sqlCommand = @"
SELECT * 
FROM mp_FriendlyUrls 
WHERE SiteID = ?SiteID 
ORDER BY FriendlyUrl;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = siteId }
		};


		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams))
		{
			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["UrlID"] = reader["UrlID"];
				row["FriendlyUrl"] = reader["FriendlyUrl"];
				row["RealUrl"] = reader["RealUrl"];
				row["IsPattern"] = reader["IsPattern"];
				dt.Rows.Add(row);
			}
		}

		return dt;
	}

	public static IDataReader GetByUrl(string hostName, string friendlyUrl)
	{
		string sqlCommand = @"
SELECT * 
FROM 
	mp_FriendlyUrls 
WHERE 
	SiteID = COALESCE(
		(SELECT SiteID From mp_SiteHosts WHERE HostName = ?HostName LIMIT 1),
		(SELECT SiteID From mp_Sites ORDER BY SiteID LIMIT 1)
	) 
AND FriendlyUrl = ?FriendlyUrl;";

		var arParams = new List<MySqlParameter>
		{
			new("?HostName", MySqlDbType.VarChar, 255){ Direction = ParameterDirection.Input, Value = hostName },
			new("?FriendlyUrl", MySqlDbType.VarChar, 255){ Direction = ParameterDirection.Input, Value = friendlyUrl }
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetFriendlyUrl(int siteId, String friendlyUrl)
	{

		string sqlCommand = @"
SELECT	* 
FROM	mp_FriendlyUrls 
WHERE	SiteID = ?SiteID 
	AND FriendlyUrl = ?FriendlyUrl;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = siteId },
			new("?FriendlyUrl", MySqlDbType.VarChar, 255){ Direction = ParameterDirection.Input, Value = friendlyUrl }
		};

		return CommandHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_FriendlyUrls table.
	/// </summary>
	public static int GetCount(int siteId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_FriendlyUrls 
WHERE SiteID = ?SiteID;";

		var param = new MySqlParameter("?SiteID", MySqlDbType.Int32)
		{
			Direction = ParameterDirection.Input,
			Value = siteId
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			param));
	}


	/// <summary>
	/// Gets a page of data from the mp_FriendlyUrls table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteId);

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
FROM mp_FriendlyUrls  
WHERE SiteID = ?SiteID 
ORDER BY FriendlyUrl 
LIMIT ?Offset, ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = siteId },
			new("?Offset", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound },
			new("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize }
		};

		return CommandHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_FriendlyUrls table.
	/// </summary>
	public static int GetCount(int siteId, string searchTerm)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_FriendlyUrls 
WHERE SiteID = ?SiteID 
AND FriendlyUrl LIKE ?SearchTerm;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SearchTerm", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchTerm + "%"
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));
	}

	public static IDataReader GetPage(
		int siteId,
		string searchTerm,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteId, searchTerm);

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
FROM mp_FriendlyUrls  
WHERE SiteID = ?SiteID 
AND FriendlyUrl LIKE ?SearchTerm 
ORDER BY FriendlyUrl 
LIMIT ?Offset, ?PageSize;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SearchTerm", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = "%" + searchTerm + "%"
			},

			new("?Offset", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);

	}
}