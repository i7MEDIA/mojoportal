/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2009-10-31
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
    
    public static class DBLetterSendLog
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
        /// Inserts a row in the mp_LetterSendLog table. Returns new integer id.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterGuid,
            Guid userGuid,
            Guid subscribeGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {
            #region Bit Conversion

            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSendLog (");
            sqlCommand.Append("LetterGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("SubscribeGuid, ");
            sqlCommand.Append("EmailAddress, ");
            sqlCommand.Append("UTC, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":LetterGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":SubscribeGuid, ");
            sqlCommand.Append(":EmailAddress, ");
            sqlCommand.Append(":UTC, ");
            sqlCommand.Append(":ErrorOccurred, ");
            sqlCommand.Append(":ErrorMessage );");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":EmailAddress", DbType.String, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = emailAddress;

            arParams[3] = new SqliteParameter(":UTC", DbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = uTC;

            arParams[4] = new SqliteParameter(":ErrorOccurred", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intErrorOccurred;

            arParams[5] = new SqliteParameter(":ErrorMessage", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = errorMessage;

            arParams[6] = new SqliteParameter(":SubscribeGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = subscribeGuid.ToString();


            int newID = 0;
            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(GetConnectionString(), sqlCommand.ToString(), arParams).ToString());
            return newID;

        }


        /// <summary>
        /// Updates a row in the mp_LetterSendLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="emailAddress"> emailAddress </param>
        /// <param name="uTC"> uTC </param>
        /// <param name="errorOccurred"> errorOccurred </param>
        /// <param name="errorMessage"> errorMessage </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            Guid letterGuid,
            Guid userGuid,
            string emailAddress,
            DateTime uTC,
            bool errorOccurred,
            string errorMessage)
        {
            #region Bit Conversion

            int intErrorOccurred;
            if (errorOccurred)
            {
                intErrorOccurred = 1;
            }
            else
            {
                intErrorOccurred = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_LetterSendLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LetterGuid = :LetterGuid, ");
            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("EmailAddress = :EmailAddress, ");
            sqlCommand.Append("UTC = :UTC, ");
            sqlCommand.Append("ErrorOccurred = :ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage = :ErrorMessage ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = :RowID ;");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":RowID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            arParams[1] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterGuid.ToString();

            arParams[2] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new SqliteParameter(":EmailAddress", DbType.String, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = emailAddress;

            arParams[4] = new SqliteParameter(":UTC", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = uTC;

            arParams[5] = new SqliteParameter(":ErrorOccurred", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intErrorOccurred;

            arParams[6] = new SqliteParameter(":ErrorMessage", DbType.Object);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = errorMessage;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_LetterSendLog table for the letterGuid. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = :LetterGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString(); ;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(), 
                sqlCommand.ToString(), 
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE LetterInfoGuid = :LetterInfoGuid) ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString(); ;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE LetterInfoGuid IN ");
            sqlCommand.Append("(SELECT LetterInfoGuid FROM mp_LetterInfo WHERE SiteGuid = :SiteGuid)) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString(); ;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
            int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = :RowID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_LetterSendLog table.
        /// </summary>
        public static IDataReader GetByLetter(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = :LetterGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSendLog table.
        /// </summary>
        public static int GetCount(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = :LetterGuid ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid letterGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(letterGuid);

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
            sqlCommand.Append("FROM	mp_LetterSendLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = :LetterGuid ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString()
                + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }



    }
}
