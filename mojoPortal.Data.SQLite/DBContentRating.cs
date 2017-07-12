/// Author:					
/// Created:				2008-10-06
/// Last Modified:			2008-10-06
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Configuration;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{   
    
    public static class DBContentRating
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }

        /// <summary>
        /// Inserts a row in the mp_ContentRating table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="rating"> rating </param>
        /// <param name="comments"> comments </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid contentGuid,
            Guid userGuid,
            string emailAddress,
            int rating,
            string comments,
            string ipAddress,
            DateTime createdUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentRating (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ContentGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("EmailAddress, ");
            sqlCommand.Append("Rating, ");
            sqlCommand.Append("Comments, ");
            sqlCommand.Append("IpAddress, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ContentGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":EmailAddress, ");
            sqlCommand.Append(":Rating, ");
            sqlCommand.Append(":Comments, ");
            sqlCommand.Append(":IpAddress, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":CreatedUtc )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[9];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = contentGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":EmailAddress", DbType.String, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = emailAddress;

            arParams[5] = new SqliteParameter(":Rating", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rating;

            arParams[6] = new SqliteParameter(":Comments", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = comments;

            arParams[7] = new SqliteParameter(":IpAddress", DbType.String, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = ipAddress;

            arParams[8] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            
            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_ContentRating table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="rating"> rating </param>
        /// <param name="comments"> comments </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            string emailAddress,
            int rating,
            string comments,
            string ipAddress,
            DateTime lastModUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ContentRating ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("EmailAddress = :EmailAddress, ");
            sqlCommand.Append("Rating = :Rating, ");
            sqlCommand.Append("Comments = :Comments, ");
            sqlCommand.Append("IpAddress = :IpAddress, ");
            sqlCommand.Append("LastModUtc = :LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[4] = new SqliteParameter(":EmailAddress", DbType.String, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = emailAddress;

            arParams[5] = new SqliteParameter(":Rating", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rating;

            arParams[6] = new SqliteParameter(":Comments", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = comments;

            arParams[7] = new SqliteParameter(":IpAddress", DbType.String, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = ipAddress;

            arParams[9] = new SqliteParameter(":LastModUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModUtc;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentRating table. Returns true if row deleted.
        /// </summary>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        public static IDataReader GetOneByContentAndUser(Guid contentGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append("AND UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();


            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid"> contentGuid </param>
        public static IDataReader GetStatsByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(AVG(Rating),0) As CurrentRating, ");
            sqlCommand.Append("Count(*) As TotalRatings ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountByContentAndUser(Guid contentGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append("AND UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));


        }

        /// <summary>
        /// Gets a count of rows in the mp_ContentRating table.
        /// </summary>
        public static int GetCountOfRatingsSince(Guid contentGuid, string ipAddress, DateTime beginUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContentRating ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append(" AND UserGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND IpAddress = :IpAddress ");
            sqlCommand.Append(" AND CreatedUtc > :BeginUtc ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new SqliteParameter(":IpAddress", DbType.String, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = ipAddress;

            arParams[2] = new SqliteParameter(":BeginUtc", DbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginUtc;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_ContentRating table.
        /// </summary>
        /// <param name="contentGuid">contentGuid</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid contentGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByContent(contentGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_ContentRating  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = :ContentGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
