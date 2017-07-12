/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2013-08-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeeds_Insert", 13);

            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Author", SqlDbType.NVarChar, 100, ParameterDirection.Input, author);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, -1, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@RssUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, rssUrl);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@ImageUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageUrl);
            sph.DefineSqlParameter("@FeedType", SqlDbType.NVarChar, 20, ParameterDirection.Input, feedType);
            sph.DefineSqlParameter("@PublishByDefault", SqlDbType.Bit, ParameterDirection.Input, publishByDefault);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);


            int newID = Convert.ToInt32(sph.ExecuteScalar());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeeds_Update", 11);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Author", SqlDbType.NVarChar, 100, ParameterDirection.Input, author);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, -1, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@RssUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, rssUrl);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@ImageUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageUrl);
            sph.DefineSqlParameter("@FeedType", SqlDbType.NVarChar, 20, ParameterDirection.Input, feedType);
            sph.DefineSqlParameter("@PublishByDefault", SqlDbType.Bit, ParameterDirection.Input, publishByDefault);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteRssFeed(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeeds_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeeds_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeeds_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetRssFeed(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RssFeeds_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetFeeds(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RssFeeds_Select", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets an IDataReader with one row from the mp_RssFeedEntries table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static DataTable GetEntries(Guid moduleGuid)
        {


            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RssFeedEntries_SelectByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            //return sph.ExecuteReader();

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

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["FeedId"] = reader["FeedId"];
                    row["FeedName"] = reader["FeedName"];
                    row["PubDate"] = reader["PubDate"];
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_DeleteExpiredByModule", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ExpiredDate", SqlDbType.DateTime, ParameterDirection.Input, expiredDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_DeleteUnPublishedByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByFeed(int feedId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_DeleteUnPublishedByFeed", 1);
            sph.DefineSqlParameter("@FeedId", SqlDbType.Int, ParameterDirection.Input, feedId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByFeed(int feedId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_DeleteByFeed", 1);
            sph.DefineSqlParameter("@FeedId", SqlDbType.Int, ParameterDirection.Input, feedId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_RssFeedEntries table.
        /// </summary>
        public static bool EntryExists(Guid moduleGuid, int entryHash)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RssFeedEntries_GetHashCount", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@EntryHash", SqlDbType.Int, ParameterDirection.Input, entryHash);
            int count =  Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);

        }

        /// <summary>
        /// Gets the most recent cache time for the module
        /// </summary>
        public static DateTime GetLastCacheTime(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_RssFeedEntries_GetLastCacheTime", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);

            DateTime result = DateTime.UtcNow.AddDays(-1);

            using (IDataReader reader = sph.ExecuteReader())
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_Insert", 13);
            sph.DefineSqlParameter("@RowGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, rowGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@FeedGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, feedGuid);
            sph.DefineSqlParameter("@FeedId", SqlDbType.Int, ParameterDirection.Input, feedId);
            sph.DefineSqlParameter("@PubDate", SqlDbType.DateTime, ParameterDirection.Input, pubDate);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Author", SqlDbType.NVarChar, 100, ParameterDirection.Input, author);
            sph.DefineSqlParameter("@BlogUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, blogUrl);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Link", SqlDbType.NVarChar, 255, ParameterDirection.Input, link);
            sph.DefineSqlParameter("@Confirmed", SqlDbType.Bit, ParameterDirection.Input, confirmed);
            sph.DefineSqlParameter("@EntryHash", SqlDbType.Int, ParameterDirection.Input, entryHash);
            sph.DefineSqlParameter("@CachedTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, cachedTimeUtc);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_Update", 8);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Author", SqlDbType.NVarChar, 100, ParameterDirection.Input, author);
            sph.DefineSqlParameter("@BlogUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, blogUrl);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Link", SqlDbType.NVarChar, 255, ParameterDirection.Input, link);
            sph.DefineSqlParameter("@EntryHash", SqlDbType.Int, ParameterDirection.Input, entryHash);
            sph.DefineSqlParameter("@CachedTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, cachedTimeUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_RssFeedEntries_UpdatePublishing", 3);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@EntryHash", SqlDbType.Int, ParameterDirection.Input, entryHash);
            sph.DefineSqlParameter("@Confirmed", SqlDbType.Bit, ParameterDirection.Input, confirmed);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }



    }
}
