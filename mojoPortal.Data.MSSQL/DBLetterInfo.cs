/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2012-11-05
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
    
    public static class DBLetterInfo
    {
        

        /// <summary>
        /// Inserts a row in the mp_LetterInfo table. Returns rows affected count.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="availableToRoles"> availableToRoles </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="allowUserFeedback"> allowUserFeedback </param>
        /// <param name="allowAnonFeedback"> allowAnonFeedback </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="fromName"> fromName </param>
        /// <param name="replyToAddress"> replyToAddress </param>
        /// <param name="sendMode"> sendMode </param>
        /// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
        /// <param name="enableSendLog"> enableSendLog </param>
        /// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
        /// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
        /// <param name="rolesThatCanSend"> rolesThatCanSend </param>
        /// <param name="createdUTC"> createdUTC </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid letterInfoGuid,
            Guid siteGuid,
            string title,
            string description,
            string availableToRoles,
            bool enabled,
            bool allowUserFeedback,
            bool allowAnonFeedback,
            string fromAddress,
            string fromName,
            string replyToAddress,
            int sendMode,
            bool enableViewAsWebPage,
            bool enableSendLog,
            string rolesThatCanEdit,
            string rolesThatCanApprove,
            string rolesThatCanSend,
            DateTime createdUtc,
            Guid createdBy,
            DateTime lastModUtc,
            Guid lastModBy,
            bool allowArchiveView,
            bool profileOptIn,
            int sortRank,
            string displayNameDefault,
            string firstNameDefault,
            string lastNameDefault)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterInfo_Insert", 27);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@AvailableToRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, availableToRoles);
            sph.DefineSqlParameter("@Enabled", SqlDbType.Bit, ParameterDirection.Input, enabled);
            sph.DefineSqlParameter("@AllowUserFeedback", SqlDbType.Bit, ParameterDirection.Input, allowUserFeedback);
            sph.DefineSqlParameter("@AllowAnonFeedback", SqlDbType.Bit, ParameterDirection.Input, allowAnonFeedback);
            sph.DefineSqlParameter("@FromAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, fromAddress);
            sph.DefineSqlParameter("@FromName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fromName);
            sph.DefineSqlParameter("@ReplyToAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, replyToAddress);
            sph.DefineSqlParameter("@SendMode", SqlDbType.Int, ParameterDirection.Input, sendMode);
            sph.DefineSqlParameter("@EnableViewAsWebPage", SqlDbType.Bit, ParameterDirection.Input, enableViewAsWebPage);
            sph.DefineSqlParameter("@EnableSendLog", SqlDbType.Bit, ParameterDirection.Input, enableSendLog);
            sph.DefineSqlParameter("@RolesThatCanEdit", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanEdit);
            sph.DefineSqlParameter("@RolesThatCanApprove", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanApprove);
            sph.DefineSqlParameter("@RolesThatCanSend", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanSend);
            sph.DefineSqlParameter("@CreatedUTC", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);

            sph.DefineSqlParameter("@AllowArchiveView", SqlDbType.Bit, ParameterDirection.Input, allowArchiveView);
            sph.DefineSqlParameter("@ProfileOptIn", SqlDbType.Bit, ParameterDirection.Input, profileOptIn);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);

            sph.DefineSqlParameter("@DisplayNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, displayNameDefault);
            sph.DefineSqlParameter("@FirstNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, firstNameDefault);
            sph.DefineSqlParameter("@LastNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, lastNameDefault);


            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Updates a row in the mp_LetterInfo table. Returns true if row updated.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="availableToRoles"> availableToRoles </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="allowUserFeedback"> allowUserFeedback </param>
        /// <param name="allowAnonFeedback"> allowAnonFeedback </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="fromName"> fromName </param>
        /// <param name="replyToAddress"> replyToAddress </param>
        /// <param name="sendMode"> sendMode </param>
        /// <param name="enableViewAsWebPage"> enableViewAsWebPage </param>
        /// <param name="enableSendLog"> enableSendLog </param>
        /// <param name="rolesThatCanEdit"> rolesThatCanEdit </param>
        /// <param name="rolesThatCanApprove"> rolesThatCanApprove </param>
        /// <param name="rolesThatCanSend"> rolesThatCanSend </param>
        /// <param name="createdUTC"> createdUTC </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUTC"> lastModUTC </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid letterInfoGuid,
            Guid siteGuid,
            string title,
            string description,
            string availableToRoles,
            bool enabled,
            bool allowUserFeedback,
            bool allowAnonFeedback,
            string fromAddress,
            string fromName,
            string replyToAddress,
            int sendMode,
            bool enableViewAsWebPage,
            bool enableSendLog,
            string rolesThatCanEdit,
            string rolesThatCanApprove,
            string rolesThatCanSend,
            DateTime createdUtc,
            Guid createdBy,
            DateTime lastModUtc,
            Guid lastModBy,
            bool allowArchiveView,
            bool profileOptIn,
            int sortRank,
            string displayNameDefault,
            string firstNameDefault,
            string lastNameDefault)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterInfo_Update", 27);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@AvailableToRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, availableToRoles);
            sph.DefineSqlParameter("@Enabled", SqlDbType.Bit, ParameterDirection.Input, enabled);
            sph.DefineSqlParameter("@AllowUserFeedback", SqlDbType.Bit, ParameterDirection.Input, allowUserFeedback);
            sph.DefineSqlParameter("@AllowAnonFeedback", SqlDbType.Bit, ParameterDirection.Input, allowAnonFeedback);
            sph.DefineSqlParameter("@FromAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, fromAddress);
            sph.DefineSqlParameter("@FromName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fromName);
            sph.DefineSqlParameter("@ReplyToAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, replyToAddress);
            sph.DefineSqlParameter("@SendMode", SqlDbType.Int, ParameterDirection.Input, sendMode);
            sph.DefineSqlParameter("@EnableViewAsWebPage", SqlDbType.Bit, ParameterDirection.Input, enableViewAsWebPage);
            sph.DefineSqlParameter("@EnableSendLog", SqlDbType.Bit, ParameterDirection.Input, enableSendLog);
            sph.DefineSqlParameter("@RolesThatCanEdit", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanEdit);
            sph.DefineSqlParameter("@RolesThatCanApprove", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanApprove);
            sph.DefineSqlParameter("@RolesThatCanSend", SqlDbType.NVarChar, -1, ParameterDirection.Input, rolesThatCanSend);
            sph.DefineSqlParameter("@CreatedUTC", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);

            sph.DefineSqlParameter("@AllowArchiveView", SqlDbType.Bit, ParameterDirection.Input, allowArchiveView);
            sph.DefineSqlParameter("@ProfileOptIn", SqlDbType.Bit, ParameterDirection.Input, profileOptIn);
            sph.DefineSqlParameter("@SortRank", SqlDbType.Int, ParameterDirection.Input, sortRank);

            sph.DefineSqlParameter("@DisplayNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, displayNameDefault);
            sph.DefineSqlParameter("@FirstNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, firstNameDefault);
            sph.DefineSqlParameter("@LastNameDefault", SqlDbType.NVarChar, 50, ParameterDirection.Input, lastNameDefault);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Updates the subscriber count on a row in the mp_LetterInfo table. Returns true if row updated.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool UpdateSubscriberCount(Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterInfo_UpdateSubscriberCount", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_LetterInfo table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_LetterInfo_Delete", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        
        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterInfo table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetOne(
            Guid letterInfoGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfo_SelectOne", 1);
            sph.DefineSqlParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, letterInfoGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterInfo table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfo_GetCount", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_LetterInfo table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfo_SelectAll", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the mp_LetterInfo table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(siteGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_LetterInfo_SelectPage", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


    }
}
