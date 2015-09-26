// Author:					Joe Audette
// Created:					2010-04-05
// Last Modified:			2012-11-05
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
    public static class DBLetterInfo
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterInfo ");
            sqlCommand.Append("(");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("AvailableToRoles, ");
            sqlCommand.Append("Enabled, ");
            sqlCommand.Append("AllowUserFeedback, ");
            sqlCommand.Append("AllowAnonFeedback, ");
            sqlCommand.Append("FromAddress, ");
            sqlCommand.Append("FromName, ");
            sqlCommand.Append("ReplyToAddress, ");
            sqlCommand.Append("SendMode, ");
            sqlCommand.Append("EnableViewAsWebPage, ");
            sqlCommand.Append("EnableSendLog, ");
            sqlCommand.Append("RolesThatCanEdit, ");
            sqlCommand.Append("RolesThatCanApprove, ");
            sqlCommand.Append("RolesThatCanSend, ");
            sqlCommand.Append("SubscriberCount, ");
            sqlCommand.Append("CreatedUTC, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUTC, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn, ");
            sqlCommand.Append("SortRank, ");

            sqlCommand.Append("DisplayNameDefault, ");
            sqlCommand.Append("FirstNameDefault, ");
            sqlCommand.Append("LastNameDefault, ");

            sqlCommand.Append("UnVerifiedCount ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@AvailableToRoles, ");
            sqlCommand.Append("@Enabled, ");
            sqlCommand.Append("@AllowUserFeedback, ");
            sqlCommand.Append("@AllowAnonFeedback, ");
            sqlCommand.Append("@FromAddress, ");
            sqlCommand.Append("@FromName, ");
            sqlCommand.Append("@ReplyToAddress, ");
            sqlCommand.Append("@SendMode, ");
            sqlCommand.Append("@EnableViewAsWebPage, ");
            sqlCommand.Append("@EnableSendLog, ");
            sqlCommand.Append("@RolesThatCanEdit, ");
            sqlCommand.Append("@RolesThatCanApprove, ");
            sqlCommand.Append("@RolesThatCanSend, ");
            sqlCommand.Append("@SubscriberCount, ");
            sqlCommand.Append("@CreatedUTC, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModUTC, ");
            sqlCommand.Append("@LastModBy, ");
            sqlCommand.Append("@AllowArchiveView, ");
            sqlCommand.Append("@ProfileOptIn, ");
            sqlCommand.Append("@SortRank, ");

            sqlCommand.Append("@DisplayNameDefault, ");
            sqlCommand.Append("@FirstNameDefault, ");
            sqlCommand.Append("@LastNameDefault, ");

            sqlCommand.Append("@UnVerifiedCount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[29];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@AvailableToRoles", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new SqlCeParameter("@Enabled", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = enabled;

            arParams[6] = new SqlCeParameter("@AllowUserFeedback", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowUserFeedback;

            arParams[7] = new SqlCeParameter("@AllowAnonFeedback", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = allowAnonFeedback;

            arParams[8] = new SqlCeParameter("@FromAddress", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new SqlCeParameter("@FromName", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new SqlCeParameter("@ReplyToAddress", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new SqlCeParameter("@SendMode", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new SqlCeParameter("@EnableViewAsWebPage", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = enableViewAsWebPage;

            arParams[13] = new SqlCeParameter("@EnableSendLog", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = enableSendLog;

            arParams[14] = new SqlCeParameter("@RolesThatCanEdit", SqlDbType.NText);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new SqlCeParameter("@RolesThatCanApprove", SqlDbType.NText);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new SqlCeParameter("@RolesThatCanSend", SqlDbType.NText);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new SqlCeParameter("@SubscriberCount", SqlDbType.Int);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = 0;

            arParams[18] = new SqlCeParameter("@CreatedUTC", SqlDbType.DateTime);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdUtc;

            arParams[19] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = createdBy;

            arParams[20] = new SqlCeParameter("@LastModUTC", SqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModUtc;

            arParams[21] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = lastModBy;

            arParams[22] = new SqlCeParameter("@AllowArchiveView", SqlDbType.Bit);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = allowArchiveView;

            arParams[23] = new SqlCeParameter("@ProfileOptIn", SqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = profileOptIn;

            arParams[24] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = sortRank;

            arParams[25] = new SqlCeParameter("@UnVerifiedCount", SqlDbType.Int);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = 0;

            arParams[26] = new SqlCeParameter("@DisplayNameDefault", SqlDbType.NVarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = displayNameDefault;

            arParams[27] = new SqlCeParameter("@FirstNameDefault", SqlDbType.NVarChar, 50);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = firstNameDefault;

            arParams[28] = new SqlCeParameter("@LastNameDefault", SqlDbType.NVarChar, 50);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = lastNameDefault;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterInfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("AvailableToRoles = @AvailableToRoles, ");
            sqlCommand.Append("Enabled = @Enabled, ");
            sqlCommand.Append("AllowUserFeedback = @AllowUserFeedback, ");
            sqlCommand.Append("AllowAnonFeedback = @AllowAnonFeedback, ");
            sqlCommand.Append("FromAddress = @FromAddress, ");
            sqlCommand.Append("FromName = @FromName, ");
            sqlCommand.Append("ReplyToAddress = @ReplyToAddress, ");
            sqlCommand.Append("SendMode = @SendMode, ");
            sqlCommand.Append("EnableViewAsWebPage = @EnableViewAsWebPage, ");
            sqlCommand.Append("EnableSendLog = @EnableSendLog, ");
            sqlCommand.Append("RolesThatCanEdit = @RolesThatCanEdit, ");
            sqlCommand.Append("RolesThatCanApprove = @RolesThatCanApprove, ");
            sqlCommand.Append("RolesThatCanSend = @RolesThatCanSend, ");
            sqlCommand.Append("LastModUTC = @LastModUTC, ");
            sqlCommand.Append("LastModBy = @LastModBy, ");
            sqlCommand.Append("AllowArchiveView = @AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn = @ProfileOptIn, ");

            sqlCommand.Append("DisplayNameDefault = @DisplayNameDefault, ");
            sqlCommand.Append("FirstNameDefault = @FirstNameDefault, ");
            sqlCommand.Append("LastNameDefault = @LastNameDefault, ");

            sqlCommand.Append("SortRank = @SortRank ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[27];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@AvailableToRoles", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new SqlCeParameter("@Enabled", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = enabled;

            arParams[6] = new SqlCeParameter("@AllowUserFeedback", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowUserFeedback;

            arParams[7] = new SqlCeParameter("@AllowAnonFeedback", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = allowAnonFeedback;

            arParams[8] = new SqlCeParameter("@FromAddress", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new SqlCeParameter("@FromName", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new SqlCeParameter("@ReplyToAddress", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new SqlCeParameter("@SendMode", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new SqlCeParameter("@EnableViewAsWebPage", SqlDbType.Bit);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = enableViewAsWebPage;

            arParams[13] = new SqlCeParameter("@EnableSendLog", SqlDbType.Bit);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = enableSendLog;

            arParams[14] = new SqlCeParameter("@RolesThatCanEdit", SqlDbType.NText);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new SqlCeParameter("@RolesThatCanApprove", SqlDbType.NText);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new SqlCeParameter("@RolesThatCanSend", SqlDbType.NText);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new SqlCeParameter("@CreatedUTC", SqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy;

            arParams[19] = new SqlCeParameter("@LastModUTC", SqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy;

            arParams[21] = new SqlCeParameter("@AllowArchiveView", SqlDbType.Bit);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = allowArchiveView;

            arParams[22] = new SqlCeParameter("@ProfileOptIn", SqlDbType.Bit);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = profileOptIn;

            arParams[23] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;

            arParams[24] = new SqlCeParameter("@DisplayNameDefault", SqlDbType.NVarChar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = displayNameDefault;

            arParams[25] = new SqlCeParameter("@FirstNameDefault", SqlDbType.NVarChar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = firstNameDefault;

            arParams[26] = new SqlCeParameter("@LastNameDefault", SqlDbType.NVarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = lastNameDefault;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Updates the subscriber count on a row in the mp_LetterInfo table. Returns true if row updated.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool UpdateSubscriberCount(Guid letterInfoGuid)
        {
            int subscriberCount = GetSubscriberCount(letterInfoGuid);
            int unverifiedSubscriberCount = GetUnverifiedSubscriberCount(letterInfoGuid);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterInfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SubscriberCount = @SubscriberCount, ");
            sqlCommand.Append("UnVerifiedCount = @UnVerifiedCount ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@SubscriberCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = subscriberCount;

            arParams[2] = new SqlCeParameter("@UnVerifiedCount", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = unverifiedSubscriberCount;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        private static int GetSubscriberCount(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
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

        private static int GetUnverifiedSubscriberCount(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND IsVerified = 0 ");
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
        /// Deletes a row from the mp_LetterInfo table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterInfo ");
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
        /// Gets an IDataReader with one row from the mp_LetterInfo table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetOne(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterInfo ");
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
        /// Gets an IDataReader with all rows in the mp_LetterInfo table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY SortRank, [Title] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterInfo table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterInfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid);

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

            sqlCommand.Append("FROM	mp_LetterInfo  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY SortRank, [Title] ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



    }
}
