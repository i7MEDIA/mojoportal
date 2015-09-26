/// Author:					Joe Audette
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
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBContentRating
    {
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
            FbParameter[] arParams = new FbParameter[9];


            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = contentGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@EmailAddress", FbDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = emailAddress;

            arParams[5] = new FbParameter("@Rating", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rating;

            arParams[6] = new FbParameter("@Comments", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = comments;

            arParams[7] = new FbParameter("@IpAddress", FbDbType.VarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = ipAddress;

            arParams[8] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

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
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ContentGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@EmailAddress, ");
            sqlCommand.Append("@Rating, ");
            sqlCommand.Append("@Comments, ");
            sqlCommand.Append("@IpAddress, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedUtc )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
           
            sqlCommand.Append("EmailAddress = @EmailAddress, ");
            sqlCommand.Append("Rating = @Rating, ");
            sqlCommand.Append("Comments = @Comments, ");
            sqlCommand.Append("IpAddress = @IpAddress, ");
            sqlCommand.Append("LastModUtc = @LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@EmailAddress", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = emailAddress;

            arParams[2] = new FbParameter("@Rating", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = rating;

            arParams[3] = new FbParameter("@Comments", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = comments;

            arParams[4] = new FbParameter("@IpAddress", FbDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = ipAddress;

            arParams[5] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModUtc;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(" AND UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(" AND UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();


            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(" AND UserGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append(" AND IpAddress = @IpAddress ");
            sqlCommand.Append(" AND CreatedUtc > @BeginUtc ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new FbParameter("@IpAddress", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = ipAddress;

            arParams[2] = new FbParameter("@BeginUtc", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginUtc;


            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_ContentRating  ");
            sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ContentGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
