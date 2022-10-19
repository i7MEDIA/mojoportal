/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2012-08-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[7];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":friendlyurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            arParams[2] = new NpgsqlParameter(":realurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = realUrl;

            arParams[3] = new NpgsqlParameter(":ispattern", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = isPattern;

            arParams[4] = new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageGuid.ToString();

            arParams[5] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = siteGuid.ToString();

            arParams[6] = new NpgsqlParameter(":itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = itemGuid.ToString();

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_insert(:siteid,:friendlyurl,:realurl,:ispattern,:pageguid,:siteguid,:itemguid)",
                arParams));

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
            NpgsqlParameter[] arParams = new NpgsqlParameter[6];
            
            arParams[0] = new NpgsqlParameter(":urlid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            arParams[1] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new NpgsqlParameter(":friendlyurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyUrl;

            arParams[3] = new NpgsqlParameter(":realurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = realUrl;

            arParams[4] = new NpgsqlParameter(":ispattern", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = isPattern;

            arParams[5] = new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageGuid.ToString();

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_update(:urlid,:siteid,:friendlyurl,:realurl,:ispattern,:pageguid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteFriendlyUrl(int urlId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":urlid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_delete(:urlid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByPageId(int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = "%pageid=" + pageId.ToString() + "%"; 
            arParams[0].Value = "%pageid=" + pageId.ToString(CultureInfo.InvariantCulture); 

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_deletebypageid(:pageid)",
                arParams));

            return (rowsAffected > 0);

        }

        public static bool DeleteByPageGuid(Guid pageGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM	mp_friendlyurls ");
            sqlCommand.Append("WHERE pageguid = :pageguid ");
            sqlCommand.Append("");

            int rowsAffected = 0;

            // using scopes the connection and will close it /destroy it when it goes out of scope
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString.GetWriteConnectionString()))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sqlCommand.ToString(), conn))
                {
                    command.Parameters.Add(new NpgsqlParameter(":pageguid", DbType.StringFixedLength, 36));
                    command.Prepare();
                    command.Parameters[0].Value = pageGuid.ToString();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return (rowsAffected > 0);
        }

        public static IDataReader GetFriendlyUrl(int urlId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":urlid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = urlId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_selectone(:urlid)",
                arParams);

        }

        public static DataTable GetByHostName(string hostName)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("UrlID", typeof(int));
            dt.Columns.Add("FriendlyUrl", typeof(string));
            dt.Columns.Add("RealUrl", typeof(string));
            dt.Columns.Add("IsPattern", typeof(bool));

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = hostName;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_selectbyhost(:hostname)",
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

        public static DataTable GetBySite(int siteId)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("UrlID", typeof(int));
            dt.Columns.Add("FriendlyUrl", typeof(string));
            dt.Columns.Add("RealUrl", typeof(string));
            dt.Columns.Add("IsPattern", typeof(bool));

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_selectbysiteid(:siteid)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":hostname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = hostName;

            arParams[1] = new NpgsqlParameter(":friendlyurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_selectbyurl(:hostname,:friendlyurl)",
                arParams);


        }

        public static IDataReader GetFriendlyUrl(int siteId, String friendlyUrl)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":friendlyurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_selectbysiteurl(:siteid,:friendlyurl)",
                arParams);


        }

        public static bool Exists(
            int siteId,
            string friendlyUrl)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":friendlyurl", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = friendlyUrl;

            int count = 0;

            count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_friendlyurls_exists(:siteid,:friendlyurl)",
                arParams));

            return (count > 0);

        }

        public static int GetCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_friendlyurls ");
            sqlCommand.Append("WHERE siteid = :siteid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("* ");
            
            sqlCommand.Append("FROM	mp_friendlyurls  ");

            sqlCommand.Append("WHERE siteid = :siteid ");

            sqlCommand.Append("ORDER BY	friendlyurl ");

            //sqlCommand.Append("LIMIT " + pageSize.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append("LIMIT  :pagesize" );

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_FriendlyUrls table.
        /// </summary>
        public static int GetCount(int siteId, string searchTerm)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_friendlyurls ");
            sqlCommand.Append("WHERE siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("friendlyurl LIKE :searchterm ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":searchterm", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchTerm + "%";

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("* ");

            sqlCommand.Append("FROM	mp_friendlyurls  ");

            sqlCommand.Append("WHERE siteid = :siteid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("friendlyurl LIKE :searchterm ");
            sqlCommand.Append("ORDER BY	friendlyurl ");

            //sqlCommand.Append("LIMIT " + pageSize.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":searchterm", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + searchTerm + "%";

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
