using System;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents an RSS or Atom Feed
/// </summary>
/// 
public class RssFeed
{
	public static Guid FeatureGuid => new("5ef82464-e2d3-4982-8dfd-01e1afa615f9");

	#region Constructors

	public RssFeed()
	{ }

	public RssFeed(int moduleId) => ModuleId = moduleId;

	public RssFeed(int moduleId, int itemId)
	{
		ModuleId = moduleId;
		if (itemId > 0)
		{
			GetRssFeed(itemId);
		}
	}

	#endregion

	#region Properties

	public Guid ItemGuid { get; private set; } = Guid.Empty;
	public Guid ModuleGuid { get; set; } = Guid.Empty;
	public Guid UserGuid { get; set; } = Guid.Empty;
	public Guid LastModUserGuid { get; set; } = Guid.Empty;
	public int ItemId { get; set; } = -1;
	public int ModuleId { get; set; } = -1;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime LastModUtc { get; set; } = DateTime.UtcNow;
	public int UserId { get; set; } = -1;
	public string Author { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public string RssUrl { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public string FeedType { get; set; } = "Rss";
	public bool PublishByDefault { get; set; } = false;
	public int SortRank { get; set; } = 500;

	#endregion

	#region Private Methods

	private void GetRssFeed(int itemId)
	{
		using (IDataReader reader = DBRssFeed.GetRssFeed(itemId))
		{
			if (reader.Read())
			{
				ItemId = Convert.ToInt32(reader["ItemID"]);
				ModuleId = Convert.ToInt32(reader["ModuleID"]);
				CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
				UserId = Convert.ToInt32(reader["UserID"]);
				Author = reader["Author"].ToString();
				Url = reader["Url"].ToString();
				RssUrl = reader["RssUrl"].ToString();

				ImageUrl = reader["ImageUrl"].ToString();
				FeedType = reader["FeedType"].ToString();
				PublishByDefault = Convert.ToBoolean(reader["PublishByDefault"]);

				ItemGuid = new Guid(reader["ItemGuid"].ToString());
				ModuleGuid = new Guid(reader["ModuleGuid"].ToString());

				string user = reader["UserGuid"].ToString();
				if (user.Length == 36) UserGuid = new Guid(user);

				user = reader["LastModUserGuid"].ToString();
				if (user.Length == 36) LastModUserGuid = new Guid(user);

				if (reader["LastModUtc"] != DBNull.Value)
					LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

				SortRank = Convert.ToInt32(reader["SortRank"]);

			}

		}

	}

	private bool Create()
	{
		int newID = -1;
		ItemGuid = Guid.NewGuid();
		CreatedDate = DateTime.UtcNow;
		LastModUtc = CreatedDate;

		newID = DBRssFeed.AddRssFeed(
			ItemGuid,
			ModuleGuid,
			UserGuid,
			ModuleId,
			UserId,
			Author,
			Url,
			RssUrl,
			CreatedDate,
			ImageUrl,
			FeedType,
			PublishByDefault,
			SortRank);

		ItemId = newID;


		return (newID > -1);

	}

	private bool Update()
	{
		LastModUtc = DateTime.UtcNow;

		bool result = DBRssFeed.UpdateRssFeed(
			ItemId,
			ModuleId,
			Author,
			Url,
			RssUrl,
			LastModUserGuid,
			LastModUtc,
			ImageUrl,
			FeedType,
			PublishByDefault,
			SortRank);



		return result;

	}



	#endregion

	public bool Save()
	{
		if (ItemId > 0)
		{
			return Update();
		}
		else
		{
			return Create();
		}
	}


	#region Static Methods

	public static bool DeleteFeed(int itemId)
	{
		return DBRssFeed.DeleteRssFeed(itemId);
	}

	public static DataTable GetFeeds(int moduleId)
	{
		var dataTable = new DataTable();
		dataTable.Columns.Add("ItemID", typeof(int));
		dataTable.Columns.Add("ItemGuid", typeof(Guid));
		dataTable.Columns.Add("ModuleID", typeof(int));
		dataTable.Columns.Add("ModuleGuid", typeof(Guid));
		dataTable.Columns.Add("Author", typeof(string));
		dataTable.Columns.Add("Url", typeof(string));
		dataTable.Columns.Add("RssUrl", typeof(string));
		dataTable.Columns.Add("ImageUrl", typeof(string));
		dataTable.Columns.Add("FeedType", typeof(string));
		dataTable.Columns.Add("PublishByDefault", typeof(bool));
		dataTable.Columns.Add("TotalEntries", typeof(int));

		using (IDataReader reader = DBRssFeed.GetFeeds(moduleId))
		{
			while (reader.Read())
			{
				DataRow row = dataTable.NewRow();
				row["ItemID"] = reader["ItemID"];
				row["ItemGuid"] = new Guid(reader["ItemGuid"].ToString());
				row["ModuleID"] = reader["ModuleID"];
				row["ModuleGuid"] = new Guid(reader["ModuleGuid"].ToString());
				row["Author"] = reader["Author"];
				row["Url"] = reader["Url"];
				row["RssUrl"] = reader["RssUrl"];
				row["ImageUrl"] = reader["ImageUrl"];
				row["FeedType"] = reader["FeedType"];
				row["PublishByDefault"] = Convert.ToBoolean(reader["PublishByDefault"]);
				row["TotalEntries"] = Convert.ToInt32(reader["TotalEntries"]);

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
		return DBRssFeed.DeleteExpiredEntriesByModule(moduleGuid, expiredDate);
	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteUnPublishedEntriesByModule(Guid moduleGuid)
	{
		return DBRssFeed.DeleteUnPublishedEntriesByModule(moduleGuid);
	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="feedId"> feedId </param>
	/// <returns>bool</returns>
	public static bool DeleteUnPublishedEntriesByFeed(int feedId)
	{
		return DBRssFeed.DeleteUnPublishedEntriesByFeed(feedId);
	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="feedId"> feedId </param>
	/// <returns>bool</returns>
	public static bool DeleteEntriesByFeed(int feedId)
	{
		return DBRssFeed.DeleteEntriesByFeed(feedId);
	}

	/// <summary>
	/// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteEntriesByModule(Guid moduleGuid)
	{
		return DBRssFeed.DeleteEntriesByModule(moduleGuid);
	}

	public static bool DeleteByModule(int moduleId)
	{
		return DBRssFeed.DeleteByModule(moduleId);
	}

	public static bool DeleteBySite(int siteId)
	{
		return DBRssFeed.DeleteBySite(siteId);
	}

	/// <summary>
	/// Gets a count of rows in the mp_RssFeedEntries table.
	/// </summary>
	public static bool EntryExists(Guid moduleGuid, int entryHash)
	{
		return DBRssFeed.EntryExists(moduleGuid, entryHash);
	}

	/// <summary>
	/// Gets the most recent cache time for the module
	/// </summary>
	public static DateTime GetLastCacheTime(Guid moduleGuid)
	{
		return DBRssFeed.GetLastCacheTime(moduleGuid);
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
		return DBRssFeed.CreateEntry(
			rowGuid,
			moduleGuid,
			feedGuid,
			feedId,
			pubDate,
			title,
			author,
			blogUrl,
			description,
			link,
			confirmed,
			entryHash,
			cachedTimeUtc);

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
	public static bool UpdateEntry(
		Guid moduleGuid,
		string title,
		string author,
		string blogUrl,
		string description,
		string link,
		int entryHash,
		DateTime cachedTimeUtc)
	{
		return DBRssFeed.UpdateEntry(
			moduleGuid,
			title,
			author,
			blogUrl,
			description,
			link,
			entryHash,
			cachedTimeUtc);
	}

	/// <summary>
	/// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="entryHash"> entryHash </param>
	public static void Publish(Guid moduleGuid, int entryHash)
	{
		DBRssFeed.UpdatePublishing(moduleGuid, true, entryHash);
	}

	/// <summary>
	/// Updates a row in the mp_RssFeedEntries table. Returns true if row updated.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="entryHash"> entryHash </param>
	public static void UnPublish(Guid moduleGuid, int entryHash)
	{
		DBRssFeed.UpdatePublishing(moduleGuid, false, entryHash);
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_RssFeedEntries table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static DataTable GetEntries(Guid moduleGuid)
	{
		return DBRssFeed.GetEntries(moduleGuid);
	}

	#endregion
}