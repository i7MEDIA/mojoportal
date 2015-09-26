// Author:					Joe Audette
// Created:					2010-04-04
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
    public static class DBLetter
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        /// <summary>
        /// Inserts a row in the mp_Letter table. Returns rows affected count.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="subject"> subject </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdUTC"> createdUTC </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="isApproved"> isApproved </param>
        /// <param name="approvedBy"> approvedBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterGuid,
            Guid letterInfoGuid,
            string subject,
            string htmlBody,
            string textBody,
            Guid createdBy,
            DateTime createdUtc,
            Guid lastModBy,
            DateTime lastModUtc,
            bool isApproved,
            Guid approvedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Letter ");
            sqlCommand.Append("(");
            sqlCommand.Append("LetterGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedUTC, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("LastModUTC, ");
            sqlCommand.Append("IsApproved, ");
            sqlCommand.Append("ApprovedBy, ");
            
            sqlCommand.Append("SendCount ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@LetterGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@HtmlBody, ");
            sqlCommand.Append("@TextBody, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@CreatedUTC, ");
            sqlCommand.Append("@LastModBy, ");
            sqlCommand.Append("@LastModUTC, ");
            sqlCommand.Append("@IsApproved, ");
            sqlCommand.Append("@ApprovedBy, ");
            sqlCommand.Append("@SendCount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            arParams[2] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdBy;

            arParams[6] = new SqlCeParameter("@CreatedUTC", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModBy;

            arParams[8] = new SqlCeParameter("@LastModUTC", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new SqlCeParameter("@IsApproved", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isApproved;

            arParams[10] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = approvedBy;

            arParams[11] = new SqlCeParameter("@SendCount", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = 0;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_Letter table. Returns true if row updated.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="subject"> subject </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="isApproved"> isApproved </param>
        /// <param name="approvedBy"> approvedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid letterGuid,
            Guid letterInfoGuid,
            string subject,
            string htmlBody,
            string textBody,
            Guid lastModBy,
            DateTime lastModUtc,
            bool isApproved,
            Guid approvedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Letter ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid, ");
            sqlCommand.Append("Subject = @Subject, ");
            sqlCommand.Append("HtmlBody = @HtmlBody, ");
            sqlCommand.Append("TextBody = @TextBody, ");
            sqlCommand.Append("LastModBy = @LastModBy, ");
            sqlCommand.Append("LastModUTC = @LastModUTC, ");
            sqlCommand.Append("IsApproved = @IsApproved, ");
            sqlCommand.Append("ApprovedBy = @ApprovedBy ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[9];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            arParams[2] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModBy;

            arParams[6] = new SqlCeParameter("@LastModUTC", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModUtc;

            arParams[7] = new SqlCeParameter("@IsApproved", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = isApproved;

            arParams[8] = new SqlCeParameter("@ApprovedBy", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = approvedBy;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        /// <summary>
        /// Deletes a row from the mp_Letter table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Letter ");
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

        /// <summary>
        /// Deletes a row from the mp_Letter table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
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

        /// <summary>
        /// Records the click of the send button in the db
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="sendClickedUtc"> sendClickedUtc </param>
        /// <returns>bool</returns>
        public static bool SendClicked(
            Guid letterGuid,
            DateTime sendClickedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Letter ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("SendClickedUTC = @SendClickedUTC ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@SendClickedUTC", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendClickedUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Records the start of sending in the db
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="sendClickedUtc"> sendClickedUtc </param>
        /// <returns>bool</returns>
        public static bool SendStarted(
            Guid letterGuid,
            DateTime sendStartedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Letter ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("SendStartedUTC = @SendStartedUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@SendStartedUTC", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendStartedUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Records the complete of sending in the db
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <param name="sendClickedUtc"> sendClickedUtc </param>
        /// <returns>bool</returns>
        public static bool SendComplete(
            Guid letterGuid,
            DateTime sendCompleteUtc,
            int sendCount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Letter ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("SendCompleteUTC = @SendCompleteUTC, ");
            sqlCommand.Append("SendCount = @SendCount ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@SendCompleteUTC", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendCompleteUtc;

            arParams[2] = new SqlCeParameter("@SendCount", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sendCount;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Letter table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static IDataReader GetOne(Guid letterGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterGuid = @LetterGuid ");
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

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Letter table.
        /// </summary>
        public static IDataReader GetAll(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCount(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND [SendClickedUTC] IS NOT NULL ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCountOfDrafts(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND [SendClickedUTC] IS NULL ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_Letter table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(letterInfoGuid);

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

            sqlCommand.Append("FROM	mp_Letter  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND [SendClickedUTC] IS NOT NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[SendClickedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a page of data from the mp_Letter table corresponding to unsent letters.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetDrafts(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountOfDrafts(letterInfoGuid);

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

            sqlCommand.Append("FROM	mp_Letter  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND [SendClickedUTC] IS NULL ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("[SendClickedUTC] DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
