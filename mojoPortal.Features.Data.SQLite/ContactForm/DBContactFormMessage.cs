///							DBContactFormMessage.cs
/// Author:					
/// Created:				2008-03-29
/// Last Modified:			2009-06-22
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;


namespace mojoPortal.Data
{

    public static class DBContactFormMessage
    {
        public static String DBPlatform()
        {
            return "SQLite";
        }

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
        /// Inserts a row in the mp_ContactFormMessage table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="email"> email </param>
        /// <param name="url"> url </param>
        /// <param name="subject"> subject </param>
        /// <param name="message"> message </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIpAddress"> createdFromIpAddress </param>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid moduleGuid,
            string email,
            string url,
            string subject,
            string message,
            DateTime createdUtc,
            string createdFromIpAddress,
            Guid userGuid)
        {
         
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContactFormMessage (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("Message, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedFromIpAddress, ");
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":Email, ");
            sqlCommand.Append(":Url, ");
            sqlCommand.Append(":Subject, ");
            sqlCommand.Append(":Message, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":CreatedFromIpAddress, ");
            sqlCommand.Append(":UserGuid )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":Email", DbType.String, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new SqliteParameter(":Url", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new SqliteParameter(":Message", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new SqliteParameter(":CreatedFromIpAddress", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();


            int rowsAffected = 0;
            rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_ContactFormMessage table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="email"> email </param>
        /// <param name="url"> url </param>
        /// <param name="subject"> subject </param>
        /// <param name="message"> message </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIpAddress"> createdFromIpAddress </param>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            Guid siteGuid,
            Guid moduleGuid,
            string email,
            string url,
            string subject,
            string message,
            DateTime createdUtc,
            string createdFromIpAddress,
            Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_ContactFormMessage ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = :SiteGuid, ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid, ");
            sqlCommand.Append("Email = :Email, ");
            sqlCommand.Append("Url = :Url, ");
            sqlCommand.Append("Subject = :Subject, ");
            sqlCommand.Append("Message = :Message, ");
            sqlCommand.Append("CreatedUtc = :CreatedUtc, ");
            sqlCommand.Append("CreatedFromIpAddress = :CreatedFromIpAddress, ");
            sqlCommand.Append("UserGuid = :UserGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":Email", DbType.String, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new SqliteParameter(":Url", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new SqliteParameter(":Message", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new SqliteParameter(":CreatedFromIpAddress", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the mp_ContactFormMessage table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = :SiteID) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContactFormMessage table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(
            Guid rowGuid)
        {
            //return Common.DBContactFormMessage.GetOne(
            //    rowGuid);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContactFormMessage ");
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
        /// Gets an IDataReader with all rows in the mp_ContactFormMessage table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContactFormMessage ");
            sqlCommand.Append(";");

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);
        }

        /// <summary>
        /// Gets a count of rows in the mp_ContactFormMessage table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ModuleGuid = :ModuleGuid  ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the mp_ContactFormMessage table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleGuid);

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

            sqlCommand.Append("FROM	mp_ContactFormMessage  ");

            sqlCommand.Append("WHERE ModuleGuid = :ModuleGuid  ");

            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture)
                + ", " + pageSize.ToString(CultureInfo.InvariantCulture));

            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
