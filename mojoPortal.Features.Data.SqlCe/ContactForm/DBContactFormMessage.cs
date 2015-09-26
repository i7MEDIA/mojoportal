// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2010-07-04
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
    public static class DBContactFormMessage
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            sqlCommand.Append("INSERT INTO mp_ContactFormMessage ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("Message, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedFromIpAddress, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@Url, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@Message, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedFromIpAddress, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new SqlCeParameter("@Message", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new SqlCeParameter("@CreatedFromIpAddress", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            //sqlCommand.Append("SiteGuid = @SiteGuid, ");
            //sqlCommand.Append("ModuleGuid = @ModuleGuid, ");
            sqlCommand.Append("Email = @Email, ");
            sqlCommand.Append("Url = @Url, ");
            sqlCommand.Append("Subject = @Subject, ");
            sqlCommand.Append("Message = @Message, ");
            //sqlCommand.Append("CreatedUtc = @CreatedUtc, ");
            //sqlCommand.Append("CreatedFromIpAddress = @CreatedFromIpAddress, ");
            sqlCommand.Append("UserGuid = @UserGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new SqlCeParameter("@Message", SqlDbType.NVarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new SqlCeParameter("@CreatedFromIpAddress", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContactFormMessage ");
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

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append(";");

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

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContactFormMessage table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = @RowGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_ContactFormMessage table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_ContactFormMessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_ContactFormMessage  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY  CreatedUtc DESC "); 
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
