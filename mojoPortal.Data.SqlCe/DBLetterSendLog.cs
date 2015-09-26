// Author:					Joe Audette
// Created:					2010-04-05
// Last Modified:			2010-04-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBLetterSendLog
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSendLog ");
            sqlCommand.Append("(");
            sqlCommand.Append("LetterGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("EmailAddress, ");
            sqlCommand.Append("UTC, ");
            sqlCommand.Append("ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage, ");
            sqlCommand.Append("SubscribeGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@LetterGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@EmailAddress, ");
            sqlCommand.Append("@UTC, ");
            sqlCommand.Append("@ErrorOccurred, ");
            sqlCommand.Append("@ErrorMessage, ");
            sqlCommand.Append("@SubscribeGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@EmailAddress", SqlDbType.NVarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = emailAddress;

            arParams[3] = new SqlCeParameter("@UTC", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = uTC;

            arParams[4] = new SqlCeParameter("@ErrorOccurred", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = errorOccurred;

            arParams[5] = new SqlCeParameter("@ErrorMessage", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = errorMessage;

            arParams[6] = new SqlCeParameter("@SubscribeGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = subscribeGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterSendLog ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LetterGuid = @LetterGuid, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("EmailAddress = @EmailAddress, ");
            sqlCommand.Append("UTC = @UTC, ");
            sqlCommand.Append("ErrorOccurred = @ErrorOccurred, ");
            sqlCommand.Append("ErrorMessage = @ErrorMessage ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowID = @RowID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@RowID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            arParams[1] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterGuid;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@EmailAddress", SqlDbType.NVarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = emailAddress;

            arParams[4] = new SqlCeParameter("@UTC", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = uTC;

            arParams[5] = new SqlCeParameter("@ErrorOccurred", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = errorOccurred;

            arParams[6] = new SqlCeParameter("@ErrorMessage", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = errorMessage;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = @RowID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSendLog ");
            sqlCommand.Append("WHERE LetterGuid IN (SELECT LetterGuid FROM mp_Letter WHERE ");
            sqlCommand.Append("LetterInfoGuid IN (SELECT LetterInfoGuid FROM mp_LetterInfo WHERE SiteGuid = @SiteGuid)) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSendLog table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(int rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowID = @RowID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") * ");

            sqlCommand.Append("FROM	mp_LetterSendLog  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("EmailAddress  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



    }
}
