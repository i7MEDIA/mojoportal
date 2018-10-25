/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2018-10-25
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
            #region Bit Conversion

            int intAllowArchiveView = 0;
            if (allowArchiveView) { intAllowArchiveView = 1; }

            int intProfileOptIn = 0;
            if (profileOptIn) { intProfileOptIn = 1; }

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            int intAllowUserFeedback = 0;
            if (allowUserFeedback) { intAllowUserFeedback = 1; }

            int intAllowAnonFeedback = 0;
            if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

            int intEnableViewAsWebPage = 0;
            if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

            int intEnableSendLog = 0;
            if (enableSendLog) { intEnableSendLog = 1; }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterInfo (");
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
            sqlCommand.Append("UnVerifiedCount, ");
            sqlCommand.Append("AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn, ");
            sqlCommand.Append("SortRank, ");

            sqlCommand.Append("DisplayNameDefault, ");
            sqlCommand.Append("FirstNameDefault, ");
            sqlCommand.Append("LastNameDefault, ");

            sqlCommand.Append("CreatedUTC, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUTC, ");
            sqlCommand.Append("LastModBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?LetterInfoGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?AvailableToRoles, ");
            sqlCommand.Append("?Enabled, ");
            sqlCommand.Append("?AllowUserFeedback, ");
            sqlCommand.Append("?AllowAnonFeedback, ");
            sqlCommand.Append("?FromAddress, ");
            sqlCommand.Append("?FromName, ");
            sqlCommand.Append("?ReplyToAddress, ");
            sqlCommand.Append("?SendMode, ");
            sqlCommand.Append("?EnableViewAsWebPage, ");
            sqlCommand.Append("?EnableSendLog, ");
            sqlCommand.Append("?RolesThatCanEdit, ");
            sqlCommand.Append("?RolesThatCanApprove, ");
            sqlCommand.Append("?RolesThatCanSend, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("0, ");

            sqlCommand.Append("?AllowArchiveView, ");
            sqlCommand.Append("?ProfileOptIn, ");
            sqlCommand.Append("?SortRank, ");

            sqlCommand.Append("?DisplayNameDefault, ");
            sqlCommand.Append("?FirstNameDefault, ");
            sqlCommand.Append("?LastNameDefault, ");

            sqlCommand.Append("?CreatedUTC, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModUTC, ");
            sqlCommand.Append("?LastModBy );");

            MySqlParameter[] arParams = new MySqlParameter[27];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?AvailableToRoles", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new MySqlParameter("?Enabled", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new MySqlParameter("?AllowUserFeedback", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new MySqlParameter("?AllowAnonFeedback", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new MySqlParameter("?FromAddress", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new MySqlParameter("?FromName", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new MySqlParameter("?ReplyToAddress", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new MySqlParameter("?SendMode", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new MySqlParameter("?EnableViewAsWebPage", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new MySqlParameter("?EnableSendLog", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new MySqlParameter("?RolesThatCanEdit", MySqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new MySqlParameter("?RolesThatCanApprove", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new MySqlParameter("?RolesThatCanSend", MySqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new MySqlParameter("?CreatedUTC", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new MySqlParameter("?LastModUTC", MySqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new MySqlParameter("?AllowArchiveView", MySqlDbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new MySqlParameter("?ProfileOptIn", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;

            arParams[24] = new MySqlParameter("?DisplayNameDefault", MySqlDbType.VarChar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = displayNameDefault;

            arParams[25] = new MySqlParameter("?FirstNameDefault", MySqlDbType.VarChar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = firstNameDefault;

            arParams[26] = new MySqlParameter("?LastNameDefault", MySqlDbType.VarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = lastNameDefault;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            #region Bit Conversion

            int intAllowArchiveView = 0;
            if (allowArchiveView) { intAllowArchiveView = 1; }

            int intProfileOptIn = 0;
            if (profileOptIn) { intProfileOptIn = 1; }

            int intEnabled = 0;
            if (enabled) { intEnabled = 1; }

            int intAllowUserFeedback = 0;
            if (allowUserFeedback) { intAllowUserFeedback = 1; }

            int intAllowAnonFeedback = 0;
            if (allowAnonFeedback) { intAllowAnonFeedback = 1; }

            int intEnableViewAsWebPage = 0;
            if (enableViewAsWebPage) { intEnableViewAsWebPage = 1; }

            int intEnableSendLog = 0;
            if (enableSendLog) { intEnableSendLog = 1; }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterInfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid, ");
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("AvailableToRoles = ?AvailableToRoles, ");
            sqlCommand.Append("Enabled = ?Enabled, ");
            sqlCommand.Append("AllowUserFeedback = ?AllowUserFeedback, ");
            sqlCommand.Append("AllowAnonFeedback = ?AllowAnonFeedback, ");
            sqlCommand.Append("FromAddress = ?FromAddress, ");
            sqlCommand.Append("FromName = ?FromName, ");
            sqlCommand.Append("ReplyToAddress = ?ReplyToAddress, ");
            sqlCommand.Append("SendMode = ?SendMode, ");
            sqlCommand.Append("EnableViewAsWebPage = ?EnableViewAsWebPage, ");
            sqlCommand.Append("EnableSendLog = ?EnableSendLog, ");
            sqlCommand.Append("RolesThatCanEdit = ?RolesThatCanEdit, ");
            sqlCommand.Append("RolesThatCanApprove = ?RolesThatCanApprove, ");
            sqlCommand.Append("RolesThatCanSend = ?RolesThatCanSend, ");

            sqlCommand.Append("AllowArchiveView = ?AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn = ?ProfileOptIn, ");
            sqlCommand.Append("SortRank = ?SortRank, ");

            sqlCommand.Append("DisplayNameDefault = ?DisplayNameDefault, ");
            sqlCommand.Append("FirstNameDefault = ?FirstNameDefault, ");
            sqlCommand.Append("LastNameDefault = ?LastNameDefault, ");

            sqlCommand.Append("CreatedUTC = ?CreatedUTC, ");
            sqlCommand.Append("CreatedBy = ?CreatedBy, ");
            sqlCommand.Append("LastModUTC = ?LastModUTC, ");
            sqlCommand.Append("LastModBy = ?LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[27];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?AvailableToRoles", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new MySqlParameter("?Enabled", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new MySqlParameter("?AllowUserFeedback", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new MySqlParameter("?AllowAnonFeedback", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new MySqlParameter("?FromAddress", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new MySqlParameter("?FromName", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new MySqlParameter("?ReplyToAddress", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new MySqlParameter("?SendMode", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new MySqlParameter("?EnableViewAsWebPage", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new MySqlParameter("?EnableSendLog", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new MySqlParameter("?RolesThatCanEdit", MySqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new MySqlParameter("?RolesThatCanApprove", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new MySqlParameter("?RolesThatCanSend", MySqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new MySqlParameter("?CreatedUTC", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new MySqlParameter("?LastModUTC", MySqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new MySqlParameter("?AllowArchiveView", MySqlDbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new MySqlParameter("?ProfileOptIn", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new MySqlParameter("?SortRank", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;

            arParams[24] = new MySqlParameter("?DisplayNameDefault", MySqlDbType.VarChar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = displayNameDefault;

            arParams[25] = new MySqlParameter("?FirstNameDefault", MySqlDbType.VarChar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = firstNameDefault;

            arParams[26] = new MySqlParameter("?LastNameDefault", MySqlDbType.VarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = lastNameDefault;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterInfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SubscriberCount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_LetterSubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid  ");
            sqlCommand.Append("),  ");
            sqlCommand.Append("UnVerifiedCount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_LetterSubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid AND IsVerified = 0  ");
            sqlCommand.Append(")  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = ?LetterInfoGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LetterInfoGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterInfo table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterInfo ");
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
        /// Gets an IDataReader with one row from the mp_LetterInfo table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetOne(
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_LetterInfo ");
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
        /// Gets an IDataReader with all rows in the mp_LetterInfo table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            string sqlCommand = @"SELECT li.*, l.SendClickedUTC
				FROM mp_LetterInfo li
				LEFT JOIN (SELECT LetterInfoGuid, MAX(SendClickedUTC) AS SendClickedUTC FROM mp_Letter GROUP BY LetterInfoGuid) AS l ON l.LetterInfoGuid = li.LetterInfoGuid
				WHERE li.SiteGuid = ?SiteGuid
				ORDER BY SortRank, Title;";
			//sqlCommand.Append("SELECT  * ");
   //         sqlCommand.Append("FROM	mp_LetterInfo ");
   //         sqlCommand.Append("WHERE ");
   //         sqlCommand.Append("SiteGuid = ?SiteGuid ");
   //         sqlCommand.Append("ORDER BY ");
   //         sqlCommand.Append("SortRank, Title ");
   //         sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand,
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
            sqlCommand.Append("SiteGuid = ?SiteGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_LetterInfo  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY SortRank, Title ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString()
                + ", ?PageSize  ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

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
