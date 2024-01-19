using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBRssFeed
{

	public static int AddRssFeed(
		Guid itemGuid,
		Guid moduleGuid,
		Guid userGuid,
		int moduleId,
		int userId,
		string author,
		string url,
		string rssUrl,
		DateTime createdUtc,
		string imageUrl,
		string feedType,
		bool publishByDefault,
		int sortRank)
	{
		#region Bit Conversion
		int intPublishByDefault;
		if (publishByDefault)
		{
			intPublishByDefault = 1;
		}
		else
		{
			intPublishByDefault = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_RssFeeds (
    ModuleID, 
    CreatedDate, 
    UserID, 
    Author, 
    Url, 
    RssUrl, 
    ItemGuid, 
    ModuleGuid, 
    UserGuid, 
    LastModUserGuid, 
    LastModUtc, 
    ImageUrl, 
    FeedType, 
    SortRank, 
    PublishByDefault 
)
VALUES (
    ?ModuleID, 
    ?CreatedDate, 
    ?UserID, 
    ?Author, 
    ?Url, 
    ?RssUrl, 
    ?ItemGuid, 
    ?ModuleGuid, 
    ?UserGuid, 
    ?UserGuid, 
    ?CreatedDate, 
    ?ImageUrl, 
    ?FeedType, 
    ?SortRank, 
    ?PublishByDefault )
); 
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?Author", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = author
			},

			new("?Url", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?RssUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = rssUrl
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

			new("?CreatedDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?ImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageUrl
			},

			new("?FeedType", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = feedType
			},

			new("?PublishByDefault", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPublishByDefault
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}


	public static bool UpdateRssFeed(
		int itemId,
		int moduleId,
		string author,
		string url,
		string rssUrl,
		Guid lastModUserGuid,
		DateTime lastModUtc,
		string imageUrl,
		string feedType,
		bool publishByDefault,
		int sortRank)
	{
		#region Bit Conversion

		int intPublishByDefault;

		if (publishByDefault)
		{
			intPublishByDefault = 1;
		}
		else
		{
			intPublishByDefault = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_RssFeeds 
SET  
    ModuleID = ?ModuleID, 
    Author = ?Author, 
    Url = ?Url, 
    RssUrl = ?RssUrl, 
    LastModUserGuid = ?LastModUserGuid, 
    LastModUtc = ?LastModUtc, 
    ImageUrl = ?ImageUrl, 
    FeedType = ?FeedType, 
    SortRank = ?SortRank, 
    PublishByDefault = ?PublishByDefault 
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

			new("?Author", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = author
			},

			new("?Url", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?RssUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = rssUrl
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

			new("?ImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageUrl
			},

			new("?FeedType", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = feedType
			},

			new("?PublishByDefault", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intPublishByDefault
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool DeleteRssFeed(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeeds 
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
		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
WHERE FeedID IN (
    SELECT ItemID 
    FROM mp_RssFeeds 
    WHERE ModuleID  = ?ModuleID
);";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		string sqlCommand1 = @"
DELETE FROM mp_RssFeeds 
WHERE ModuleID  = ?ModuleID ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
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
DELETE FROM mp_RssFeedEntries 
WHERE FeedID IN (
    SELECT ItemID 
    FROM mp_RssFeeds 
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
DELETE FROM mp_RssFeeds 
WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetRssFeed(int itemId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_RssFeeds 
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


	public static IDataReader GetFeeds(int moduleId)
	{
		string sqlCommand = @"
SELECT 
    f.*, 
    (SELECT COUNT(*) 
        FROM mp_RssFeedEntries e 
        WHERE e.FeedId = f.ItemID
    ) 
As TotalEntries 
FROM mp_RssFeeds f 
WHERE f.ModuleID = ?ModuleID 
ORDER BY f.SortRank, f.Author;";

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_RssFeedEntries table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static DataTable GetEntries(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT 
    f.Author As FeedName, 
    e.* 
FROM mp_RssFeedEntries e 
JOIN mp_RssFeeds f 
ON e.FeedID = f.ItemID 
WHERE e.ModuleGuid = ?ModuleGuid 
ORDER BY e.PubDate DESC, e.PubDateMils DESC ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("FeedId", typeof(int));
		dataTable.Columns.Add("FeedName", typeof(string));
		dataTable.Columns.Add("PubDate", typeof(DateTime));
		dataTable.Columns.Add("Author", typeof(string));
		dataTable.Columns.Add("Title", typeof(string));
		dataTable.Columns.Add("Description", typeof(string));
		dataTable.Columns.Add("BlogUrl", typeof(string));
		dataTable.Columns.Add("Link", typeof(string));
		dataTable.Columns.Add("Confirmed", typeof(bool));
		dataTable.Columns.Add("EntryHash", typeof(int));

		using (reader)
		{
			while (reader.Read())
			{
				DataRow row = dataTable.NewRow();
				row["FeedId"] = reader["FeedId"];
				row["FeedName"] = reader["FeedName"];
				row["PubDate"] = Convert.ToDateTime(reader["PubDate"]).AddMilliseconds(Convert.ToInt32(reader["PubDateMils"]));
				row["Author"] = reader["Author"];
				row["Title"] = reader["Title"];
				row["Description"] = reader["Description"];
				row["BlogUrl"] = reader["BlogUrl"];
				row["Link"] = reader["Link"];
				row["Confirmed"] = Convert.ToBoolean(reader["Confirmed"]);
				row["EntryHash"] = reader["EntryHash"];

				dataTable.Rows.Add(row);

			}
		}

		return dataTable;

	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteExpiredEntriesByModule(Guid moduleGuid, DateTime expiredDate)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
WHERE ModuleGuid = ?ModuleGuid 
AND PubDate < ?ExpiredDate ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?ExpiredDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = expiredDate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteEntriesByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
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
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteUnPublishedEntriesByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
WHERE ModuleGuid = ?ModuleGuid 
AND Confirmed = 0 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteUnPublishedEntriesByFeed(int feedId)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
WHERE FeedId = ?FeedId 
AND Confirmed = 0 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeedId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = feedId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="feedId"> feedId </param>
	/// <returns>bool</returns>
	public static bool DeleteEntriesByFeed(int feedId)
	{
		string sqlCommand = @"
DELETE FROM mp_RssFeedEntries 
WHERE FeedId = ?FeedId ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeedId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = feedId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets a count of rows in the mp_RssFeedEntries table.
	/// </summary>
	public static bool EntryExists(Guid moduleGuid, int entryHash)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_RssFeedEntries 
WHERE ModuleGuid = ?ModuleGuid 
AND EntryHash = ?EntryHash;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?EntryHash", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = entryHash
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return (count > 0);
	}


	/// <summary>
	/// Gets the most recent cache time for the module
	/// </summary>
	public static DateTime GetLastCacheTime(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT CachedTimeUtc 
FROM mp_RssFeedEntries 
WHERE ModuleGuid = ?ModuleGuid 
ORDER BY CachedTimeUtc DESC 
LIMIT 1 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};

		DateTime result = DateTime.UtcNow.AddDays(-1);

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				result = Convert.ToDateTime(reader["CachedTimeUtc"]);
			}
		}

		return result;

	}

	/// <summary>
	/// Inserts a row in the mp_RssFeedEntries table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="feedGuid"> feedGuid </param>
	/// <param name="pubDate"> pubDate </param>
	/// <param name="title"> title </param>
	/// <param name="author"> author </param>
	/// <param name="blogUrl"> blogUrl </param>
	/// <param name="description"> description </param>
	/// <param name="link"> link </param>
	/// <param name="confirmed"> confirmed </param>
	/// <param name="entryHash"> entryHash </param>
	/// <param name="cachedTimeUtc"> cachedTimeUtc </param>
	/// <returns>int</returns>
	public static int CreateEntry(
		Guid rowGuid,
		Guid moduleGuid,
		Guid feedGuid,
		int feedId,
		DateTime pubDate,
		string title,
		string author,
		string blogUrl,
		string description,
		string link,
		bool confirmed,
		int entryHash,
		DateTime cachedTimeUtc)
	{
		#region Bit Conversion

		int intConfirmed;
		if (confirmed)
		{
			intConfirmed = 1;
		}
		else
		{
			intConfirmed = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_RssFeedEntries (
    RowGuid, 
    ModuleGuid, 
    FeedGuid, 
    FeedId, 
    PubDate, 
    PubDateMils, 
    Title, 
    Author, 
    BlogUrl, 
    Description, 
    Link, 
    Confirmed, 
    EntryHash, 
    CachedTimeUtc 
)
VALUES (
    ?RowGuid, 
    ?ModuleGuid, 
    ?FeedGuid, 
    ?FeedId, 
    ?PubDate, 
    ?PubDateMils, 
    ?Title, 
    ?Author, 
    ?BlogUrl, 
    ?Description, 
    ?Link, 
    ?Confirmed, 
    ?EntryHash, 
    ?CachedTimeUtc 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?FeedGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = feedGuid.ToString()
			},

			new("?FeedId", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = feedId
			},

			new("?PubDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = pubDate
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Author", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = author
			},

			new("?BlogUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = blogUrl
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?Link", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = link
			},

			new("?Confirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intConfirmed
			},

			new("?EntryHash", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = entryHash
			},

			new("?CachedTimeUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = cachedTimeUtc
			},

			new("?PubDateMils", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pubDate.Millisecond
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		// needed because even PubDateMils milliseconds is not different if rows are processed fast
		// 1 ms sleep to ensure that the next time call will return a unique value
		Thread.Sleep(1);

		return rowsAffected;


	}

	/// <summary>
	/// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="title"> title </param>
	/// <param name="author"> author </param>
	/// <param name="blogUrl"> blogUrl </param>
	/// <param name="description"> description </param>
	/// <param name="link"> link </param>
	/// <param name="entryHash"> entryHash </param>
	/// <param name="cachedTimeUtc"> cachedTimeUtc </param>
	/// <returns>bool</returns>
	public static bool UpdateEnry(
		Guid moduleGuid,
		string title,
		string author,
		string blogUrl,
		string description,
		string link,
		int entryHash,
		DateTime cachedTimeUtc)
	{
		string sqlCommand = @"
UPDATE mp_RssFeedEntries 
SET  
    Title = ?Title, 
    Author = ?Author, 
    BlogUrl = ?BlogUrl, 
    Description = ?Description, 
    Link = ?Link, 
    CachedTimeUtc = ?CachedTimeUtc 
WHERE  
ModuleGuid = ?ModuleGuid 
AND EntryHash = ?EntryHash ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Author", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = author
			},

			new("?BlogUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = blogUrl
			},

			new("?Description", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?Link", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = link
			},

			new("?EntryHash", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = entryHash
			},

			new("?CachedTimeUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = cachedTimeUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="confirmed"> confirmed </param>
	/// <param name="entryHash"> entryHash </param>
	/// <returns>bool</returns>
	public static bool UpdatePublishing(
		Guid moduleGuid,
		bool confirmed,
		int entryHash)
	{
		#region Bit Conversion

		int intConfirmed;
		if (confirmed)
		{
			intConfirmed = 1;
		}
		else
		{
			intConfirmed = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE mp_RssFeedEntries 
SET Confirmed = ?Confirmed 
WHERE ModuleGuid = ?ModuleGuid 
AND EntryHash = ?EntryHash ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Confirmed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intConfirmed
			},

			new("?EntryHash", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = entryHash
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}



}
