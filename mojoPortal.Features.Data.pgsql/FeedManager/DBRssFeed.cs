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
using System.Data;
using System.Text;
using Npgsql;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_rssfeeds (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("createddate, ");
            sqlCommand.Append("userid, ");
            sqlCommand.Append("author, ");
            sqlCommand.Append("url, ");
            sqlCommand.Append("rssurl, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("lastmoduserguid, ");
            sqlCommand.Append("lastmodutc, ");
            sqlCommand.Append("imageurl, ");
            sqlCommand.Append("feedtype, ");
            sqlCommand.Append("sortrank, ");
            sqlCommand.Append("publishbydefault )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":createddate, ");
            sqlCommand.Append(":userid, ");
            sqlCommand.Append(":author, ");
            sqlCommand.Append(":url, ");
            sqlCommand.Append(":rssurl, ");
            sqlCommand.Append(":itemguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":lastmoduserguid, ");
            sqlCommand.Append(":lastmodutc, ");
            sqlCommand.Append(":imageurl, ");
            sqlCommand.Append(":feedtype, ");
            sqlCommand.Append(":sortrank, ");
            sqlCommand.Append(":publishbydefault )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_rssfeeds_itemid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[15];
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = createdUtc;

            arParams[2] = new NpgsqlParameter("userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            arParams[3] = new NpgsqlParameter("author", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = author;

            arParams[4] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new NpgsqlParameter("rssurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rssUrl;

            arParams[6] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = itemGuid.ToString();

            arParams[7] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = moduleGuid.ToString();

            arParams[8] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userGuid.ToString();

            arParams[9] = new NpgsqlParameter("lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            arParams[10] = new NpgsqlParameter("lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdUtc;

            arParams[11] = new NpgsqlParameter("imageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = imageUrl;

            arParams[12] = new NpgsqlParameter("feedtype", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = feedType;

            arParams[13] = new NpgsqlParameter("publishbydefault", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = publishByDefault;

            arParams[14] = new NpgsqlParameter("sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = sortRank;


            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_rssfeeds ");
            sqlCommand.Append("SET  ");
          
            sqlCommand.Append("author = :author, ");
            sqlCommand.Append("url = :url, ");
            sqlCommand.Append("rssurl = :rssurl, ");
            sqlCommand.Append("lastmoduserguid = :lastmoduserguid, ");
            sqlCommand.Append("lastmodutc = :lastmodutc, ");
            sqlCommand.Append("imageurl = :imageurl, ");
            sqlCommand.Append("feedtype = :feedtype, ");
            sqlCommand.Append("sortrank = :sortrank, ");
            sqlCommand.Append("publishbydefault = :publishbydefault ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("author", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = author;

            arParams[3] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new NpgsqlParameter("rssurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rssUrl;

            arParams[5] = new NpgsqlParameter("lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModUserGuid.ToString();

            arParams[6] = new NpgsqlParameter("lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModUtc;

            arParams[7] = new NpgsqlParameter("imageurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = imageUrl;

            arParams[8] = new NpgsqlParameter("feedtype", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = feedType;

            arParams[9] = new NpgsqlParameter("publishbydefault", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = publishByDefault;

            arParams[10] = new NpgsqlParameter("sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = sortRank;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
           
        }

        public static bool DeleteRssFeed(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_rssfeeds_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("feedid IN (SELECT itemid FROM mp_rssfeeds WHERE moduleid  = :moduleid);");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_rssfeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid  = :moduleid ;");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteBySite(int siteId)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("feedid IN (SELECT itemid FROM mp_rssfeeds WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid));");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_rssfeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ;");

            rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetRssFeed(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_rssfeeds ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



        public static IDataReader GetFeeds(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  f.*, ");
            sqlCommand.Append(" (SELECT COUNT(*) FROM mp_rssfeedentries e WHERE e.feedid = f.itemid) AS totalentries ");
            sqlCommand.Append("FROM	mp_rssfeeds f ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("f.moduleid = :moduleid ");
            sqlCommand.Append("ORDER BY f.sortrank, f.author ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
           
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_RssFeedEntries table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static DataTable GetEntries(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("f.author As feedname, ");
            sqlCommand.Append("e.* ");

            sqlCommand.Append("FROM	mp_rssfeedentries e ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_rssfeeds f ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("e.feedid = f.itemid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("e.moduleguid = :moduleguid ");
            sqlCommand.Append("ORDER BY e.pubdate DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            IDataReader reader =  NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(" AND pubdate < :expireddate ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter("expireddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = expiredDate;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(" AND confirmed = false ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
        public static bool DeleteUnPublishedEntriesByFeed(int feedId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("feedid = :feedid ");
            sqlCommand.Append(" AND confirmed = false ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("feedid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = feedId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("feedid = :feedid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("feedid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = feedId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("FROM	mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(" AND entryhash = :entryhash ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter("entryhash", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = entryHash;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT  cachedtimeutc ");
            sqlCommand.Append("FROM	mp_rssfeedentries ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append("ORDER BY cachedtimeutc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            DateTime result = DateTime.UtcNow.AddDays(-1);

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("INSERT INTO mp_rssfeedentries (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("feedguid, ");
            sqlCommand.Append("feedid, ");
            sqlCommand.Append("pubdate, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("author, ");
            sqlCommand.Append("blogurl, ");
            sqlCommand.Append("description, ");
            sqlCommand.Append("link, ");
            sqlCommand.Append("confirmed, ");
            sqlCommand.Append("entryhash, ");
            sqlCommand.Append("cachedtimeutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":feedguid, ");
            sqlCommand.Append(":feedid, ");
            sqlCommand.Append(":pubdate, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":author, ");
            sqlCommand.Append(":blogurl, ");
            sqlCommand.Append(":description, ");
            sqlCommand.Append(":link, ");
            sqlCommand.Append(":confirmed, ");
            sqlCommand.Append(":entryhash, ");
            sqlCommand.Append(":cachedtimeutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[13];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            arParams[2] = new NpgsqlParameter("feedguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = feedGuid.ToString();

            arParams[3] = new NpgsqlParameter("feedid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = feedId;

            arParams[4] = new NpgsqlParameter("pubdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pubDate;

            arParams[5] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = title;

            arParams[6] = new NpgsqlParameter("author", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = author;

            arParams[7] = new NpgsqlParameter("blogurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = blogUrl;

            arParams[8] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = description;

            arParams[9] = new NpgsqlParameter("link", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = link;

            arParams[10] = new NpgsqlParameter("confirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = confirmed;

            arParams[11] = new NpgsqlParameter("entryhash", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = entryHash;

            arParams[12] = new NpgsqlParameter("cachedtimeutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = cachedTimeUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_rssfeedentries ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("title = :title, ");
            sqlCommand.Append("author = :author, ");
            sqlCommand.Append("blogurl = :blogurl, ");
            sqlCommand.Append("description = :description, ");
            sqlCommand.Append("link = :link, ");
            sqlCommand.Append("cachedtimeutc = :cachedtimeutc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(" AND entryhash = :entryhash ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[8];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter("author", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = author;

            arParams[3] = new NpgsqlParameter("blogurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = blogUrl;

            arParams[4] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new NpgsqlParameter("link", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = link;

            arParams[6] = new NpgsqlParameter("entryhash", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = entryHash;

            arParams[7] = new NpgsqlParameter("cachedtimeutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = cachedTimeUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_rssfeedentries ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("confirmed = :confirmed ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(" AND entryhash = :entryhash ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter("confirmed", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = confirmed;

            arParams[2] = new NpgsqlParameter("entryhash", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = entryHash;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

    }
}
