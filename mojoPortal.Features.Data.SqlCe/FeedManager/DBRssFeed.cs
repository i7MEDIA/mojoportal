// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2013-08-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBRssFeed
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_RssFeeds ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("UserID, ");
            sqlCommand.Append("Author, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("RssUrl, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LastModUserGuid, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("ImageUrl, ");
            sqlCommand.Append("FeedType, ");
            sqlCommand.Append("PublishByDefault, ");
            sqlCommand.Append("SortRank ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@UserID, ");
            sqlCommand.Append("@Author, ");
            sqlCommand.Append("@Url, ");
            sqlCommand.Append("@RssUrl, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@LastModUserGuid, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@ImageUrl, ");
            sqlCommand.Append("@FeedType, ");
            sqlCommand.Append("@PublishByDefault, ");
            sqlCommand.Append("@SortRank ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[15];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new SqlCeParameter("@UserID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            arParams[3] = new SqlCeParameter("@Author", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = author;

            arParams[4] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqlCeParameter("@RssUrl", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rssUrl;

            arParams[6] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = itemGuid;

            arParams[7] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = moduleGuid;

            arParams[8] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userGuid;

            arParams[9] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid;

            arParams[10] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdUtc;

            arParams[11] = new SqlCeParameter("@ImageUrl", SqlDbType.NVarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = imageUrl;

            arParams[12] = new SqlCeParameter("@FeedType", SqlDbType.NVarChar, 20);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = feedType;

            arParams[13] = new SqlCeParameter("@PublishByDefault", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = publishByDefault;

            arParams[14] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = sortRank;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_RssFeeds ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Author = @Author, ");
            sqlCommand.Append("Url = @Url, ");
            sqlCommand.Append("RssUrl = @RssUrl, ");
            sqlCommand.Append("LastModUserGuid = @LastModUserGuid, ");
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("ImageUrl = @ImageUrl, ");
            sqlCommand.Append("FeedType = @FeedType, ");
            sqlCommand.Append("PublishByDefault = @PublishByDefault, ");
            sqlCommand.Append("SortRank = @SortRank ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@Author", SqlDbType.NVarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = author;

            arParams[3] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new SqlCeParameter("@RssUrl", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rssUrl;

            arParams[5] = new SqlCeParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModUserGuid;

            arParams[6] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModUtc;

            arParams[7] = new SqlCeParameter("@ImageUrl", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = imageUrl;

            arParams[8] = new SqlCeParameter("@FeedType", SqlDbType.NVarChar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = feedType;

            arParams[9] = new SqlCeParameter("@PublishByDefault", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = publishByDefault;

            arParams[10] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = sortRank;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteRssFeed(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeedID IN (SELECT ItemID FROM mp_RssFeeds WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID));");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetRssFeed(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_RssFeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetFeeds(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  f.*, ");
            sqlCommand.Append("COALESCE(s2.TotalEntries, 0) AS TotalEntries ");
            //sqlCommand.Append(" (SELECT COUNT(*) FROM mp_RssFeedEntries e WHERE e.FeedId = f.ItemID) As TotalEntries ");

            sqlCommand.Append("FROM	mp_RssFeeds f ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT FeedId, Count(*) As TotalEntries ");
            sqlCommand.Append("FROM mp_RssFeedEntries ");
            sqlCommand.Append("GROUP BY FeedId ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.FeedId = f.ItemID ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("f.ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY f.SortRank, f.Author ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetEntries(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("f.Author As FeedName, ");
            sqlCommand.Append("e.* ");

            sqlCommand.Append("FROM	mp_RssFeedEntries e ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_RssFeeds f ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.FeedID = f.ItemID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY e.PubDate DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
                    row["PubDate"] = Convert.ToDateTime(reader["PubDate"]);
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(" AND PubDate < @ExpiredDate ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@ExpiredDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = expiredDate;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(" AND Confirmed = 0 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteUnPublishedEntriesByFeed(int feedId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeedId = @FeedId ");
            sqlCommand.Append(" AND Confirmed = 0 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeedId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = feedId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_RssFeedEntries table. Returns true if row deleted.
        /// </summary>
        /// <param name="feedId"> feedId </param>
        /// <returns>bool</returns>
        public static bool DeleteEntriesByFeed(int feedId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeedId = @FeedId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeedId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = feedId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets a count of rows in the mp_RssFeedEntries table.
        /// </summary>
        public static bool EntryExists(Guid moduleGuid, int entryHash)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(" AND EntryHash = @EntryHash ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@entryHash", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = entryHash;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);
        }

        /// <summary>
        /// Gets the most recent cache time for the module
        /// </summary>
        public static DateTime GetLastCacheTime(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1)  CachedTimeUtc ");
            sqlCommand.Append("FROM	mp_RssFeedEntries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY CachedTimeUtc DESC ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            DateTime result = DateTime.UtcNow.AddDays(-1);

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_RssFeedEntries ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("FeedGuid, ");
            sqlCommand.Append("FeedId, ");
            sqlCommand.Append("PubDate, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Author, ");
            sqlCommand.Append("BlogUrl, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Link, ");
            sqlCommand.Append("Confirmed, ");
            sqlCommand.Append("EntryHash, ");
            sqlCommand.Append("CachedTimeUtc ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@FeedGuid, ");
            sqlCommand.Append("@FeedId, ");
            sqlCommand.Append("@PubDate, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Author, ");
            sqlCommand.Append("@BlogUrl, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@Link, ");
            sqlCommand.Append("@Confirmed, ");
            sqlCommand.Append("@EntryHash, ");
            sqlCommand.Append("@CachedTimeUtc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[13];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid;

            arParams[2] = new SqlCeParameter("@FeedGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = feedGuid;

            arParams[3] = new SqlCeParameter("@FeedId", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = feedId;

            arParams[4] = new SqlCeParameter("@PubDate", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pubDate;

            arParams[5] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = title;

            arParams[6] = new SqlCeParameter("@Author", SqlDbType.NVarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = author;

            arParams[7] = new SqlCeParameter("@BlogUrl", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = blogUrl;

            arParams[8] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = description;

            arParams[9] = new SqlCeParameter("@Link", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = link;

            arParams[10] = new SqlCeParameter("@Confirmed", SqlDbType.Bit);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = confirmed;

            arParams[11] = new SqlCeParameter("@EntryHash", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = entryHash;

            arParams[12] = new SqlCeParameter("@CachedTimeUtc", SqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cachedTimeUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_RssFeedEntries ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Author = @Author, ");
            sqlCommand.Append("BlogUrl = @BlogUrl, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("Link = @Link, ");
            sqlCommand.Append("CachedTimeUtc = @CachedTimeUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(" AND EntryHash = @EntryHash ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[8];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Author", SqlDbType.NVarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = author;

            arParams[3] = new SqlCeParameter("@BlogUrl", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = blogUrl;

            arParams[4] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqlCeParameter("@Link", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = link;

            arParams[6] = new SqlCeParameter("@EntryHash", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = entryHash;

            arParams[7] = new SqlCeParameter("@CachedTimeUtc", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = cachedTimeUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_RssFeedEntries ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Confirmed = @Confirmed ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(" AND EntryHash = @EntryHash ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@Confirmed", SqlDbType.Bit);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = confirmed;

            arParams[2] = new SqlCeParameter("@EntryHash", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = entryHash;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

    }
}
