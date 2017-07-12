/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2012-07-20
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
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
    public static class DBLetter
    {

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
            #region Bit Conversion

            int intIsApproved;
            if (isApproved)
            {
                intIsApproved = 1;
            }
            else
            {
                intIsApproved = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Letter (");
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
            sqlCommand.Append(" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?LetterGuid, ");
            sqlCommand.Append("?LetterInfoGuid, ");
            sqlCommand.Append("?Subject, ");
            sqlCommand.Append("?HtmlBody, ");
            sqlCommand.Append("?TextBody, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedUTC, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?LastModUTC, ");
            sqlCommand.Append("?IsApproved, ");
            sqlCommand.Append("?ApprovedBy, ");
            sqlCommand.Append("0 ");
            sqlCommand.Append(" );");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new MySqlParameter("?HtmlBody", MySqlDbType.LongText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new MySqlParameter("?TextBody", MySqlDbType.LongText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdBy.ToString();

            arParams[6] = new MySqlParameter("?CreatedUTC", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModBy.ToString();

            arParams[8] = new MySqlParameter("?LastModUTC", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new MySqlParameter("?IsApproved", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsApproved;

            arParams[10] = new MySqlParameter("?ApprovedBy", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = approvedBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            #region Bit Conversion

            int intIsApproved;
            if (isApproved)
            {
                intIsApproved = 1;
            }
            else
            {
                intIsApproved = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Letter ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid, ");
            sqlCommand.Append("Subject = ?Subject, ");
            sqlCommand.Append("HtmlBody = ?HtmlBody, ");
            sqlCommand.Append("TextBody = ?TextBody, ");
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("LastModUTC = ?LastModUTC, ");
            sqlCommand.Append("IsApproved = ?IsApproved, ");
            sqlCommand.Append("ApprovedBy = ?ApprovedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = ?LetterGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new MySqlParameter("?HtmlBody", MySqlDbType.LongText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new MySqlParameter("?TextBody", MySqlDbType.LongText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModBy.ToString();

            arParams[6] = new MySqlParameter("?LastModUTC", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModUtc;

            arParams[7] = new MySqlParameter("?IsApproved", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intIsApproved;

            arParams[8] = new MySqlParameter("?ApprovedBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = approvedBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("LetterGuid = ?LetterGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

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
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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
            sqlCommand.Append("SendClickedUTC = ?SendClickedUTC ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = ?LetterGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new MySqlParameter("?SendClickedUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendClickedUtc;

            
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("SendStartedUTC = ?SendStartedUTC ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = ?LetterGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new MySqlParameter("?SendStartedUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendStartedUtc;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("SendCompleteUTC = ?SendCompleteUTC, ");
            sqlCommand.Append("SendCount = ?SendCount ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterGuid = ?LetterGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new MySqlParameter("?SendCompleteUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendCompleteUtc;

            arParams[2] = new MySqlParameter("?SendCount", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sendCount;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("LetterGuid = ?LetterGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ");
            sqlCommand.Append("AND SendClickedUTC IS NOT NULL ");
            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ");
            sqlCommand.Append("AND SendClickedUTC IS NULL ");
            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Letter  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ");
            sqlCommand.Append("AND SendClickedUTC IS NOT NULL ");
            sqlCommand.Append("ORDER BY SendClickedUTC DESC ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Letter  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ");
            sqlCommand.Append("AND SendClickedUTC IS NULL ");
            sqlCommand.Append("ORDER BY CreatedUTC ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


    }
}
