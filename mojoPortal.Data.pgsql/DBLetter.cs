/// Author:					
/// Created:				2007-12-27
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
using System.Data;
using System.Text;
using Npgsql;

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            
            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new NpgsqlParameter(":subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new NpgsqlParameter(":htmlbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new NpgsqlParameter(":textbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new NpgsqlParameter(":createdby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdBy.ToString();

            arParams[6] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModBy.ToString();

            arParams[8] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new NpgsqlParameter(":isapproved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = isApproved;

            arParams[10] = new NpgsqlParameter(":approvedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = approvedBy.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_insert(:letterguid,:letterinfoguid,:subject,:htmlbody,:textbody,:createdby,:createdutc,:lastmodby,:lastmodutc,:isapproved,:approvedby)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[9];
            
            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new NpgsqlParameter(":subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new NpgsqlParameter(":htmlbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = htmlBody;

            arParams[4] = new NpgsqlParameter(":textbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = textBody;

            arParams[5] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModBy.ToString();

            arParams[6] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModUtc;

            arParams[7] = new NpgsqlParameter(":isapproved", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = isApproved;

            arParams[8] = new NpgsqlParameter(":approvedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = approvedBy.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_update(:letterguid,:letterinfoguid,:subject,:htmlbody,:textbody,:lastmodby,:lastmodutc,:isapproved,:approvedby)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_delete(:letterguid)",
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
            sqlCommand.Append("DELETE FROM mp_letter ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":sendclickedutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendClickedUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_updatesendclicked(:letterguid,:sendclickedutc)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":sendstartedutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendStartedUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_updatesendstarted(:letterguid,:sendstartedutc)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":sendcompleteutc", NpgsqlTypes.NpgsqlDbType.Date);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = sendCompleteUtc;

            arParams[2] = new NpgsqlParameter(":sendcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = sendCount;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_updatesendsomplete(:letterguid,:sendcompleteutc,:sendcount)",
                arParams);

            return (rowsAffected > -1);

        }




        /// <summary>
        /// Gets an IDataReader with one row from the mp_Letter table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static IDataReader GetOne(Guid letterGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_select_one(:letterguid)",
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Letter table.
        /// </summary>
        public static IDataReader GetAll(Guid letterInfoGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_select_all(:letterinfoguid)",
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCount(Guid letterInfoGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_count(:letterinfoguid)",
                arParams));

           

        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCountOfDrafts(Guid letterInfoGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_countdrafts(:letterinfoguid)",
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
            totalPages = 1;
            int totalRows
                = GetCount(letterInfoGuid);

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_selectpage(:letterinfoguid,:pagenumber,:pagesize)",
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
            totalPages = 1;
            int totalRows
                = GetCountOfDrafts(letterInfoGuid);

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letter_selectdraftspage(:letterinfoguid,:pagenumber,:pagesize)",
                arParams);

        }



    }
}
