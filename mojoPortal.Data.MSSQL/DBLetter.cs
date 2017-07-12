/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2010-07-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_Insert", 11);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@HtmlBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, htmlBody);
            sph.DefineSqlParameter("@TextBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, textBody);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedUTC", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            sph.DefineSqlParameter("@LastModUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@IsApproved", SqlDbType.Bit, ParameterDirection.Input, isApproved);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);

            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_Update", 9);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@HtmlBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, htmlBody);
            sph.DefineSqlParameter("@TextBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, textBody);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            sph.DefineSqlParameter("@LastModUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@IsApproved", SqlDbType.Bit, ParameterDirection.Input, isApproved);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_Letter table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid letterGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_Delete", 1);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_Letter table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_DeleteByLetterInfo", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_UpdateSendClicked", 2);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@SendClickedUTC", SqlDbType.DateTime, ParameterDirection.Input, sendClickedUtc);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_UpdateSendStarted", 2);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@SendStartedUTC", SqlDbType.DateTime, ParameterDirection.Input, sendStartedUtc);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Letter_UpdateSendComplete", 3);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            sph.DefineSqlParameter("@SendCompleteUTC", SqlDbType.DateTime, ParameterDirection.Input, sendCompleteUtc);
            sph.DefineSqlParameter("@SendCount", SqlDbType.Int, ParameterDirection.Input, sendCount);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }




        /// <summary>
        /// Gets an IDataReader with one row from the mp_Letter table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static IDataReader GetOne(Guid letterGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_SelectOne", 1);
            sph.DefineSqlParameter("@LetterGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCount(Guid letterInfoGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_GetCount", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

            
        }

        /// <summary>
        /// Gets a count of rows in the mp_Letter table.
        /// </summary>
        public static int GetCountOfDrafts(Guid letterInfoGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_GetCountDrafts", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Letter table.
        /// </summary>
        public static IDataReader GetAll(Guid letterInfoGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_SelectAll", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            return sph.ExecuteReader();



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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_SelectPage", 3);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Letter_SelectDraftsPage", 3);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }



    }
}
