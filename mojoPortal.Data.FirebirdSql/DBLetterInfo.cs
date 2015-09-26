/// Author:					Joe Audette
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
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBLetterInfo
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[28];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@AvailableToRoles", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new FbParameter("@Enabled", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new FbParameter("@AllowUserFeedback", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new FbParameter("@AllowAnonFeedback", FbDbType.SmallInt);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new FbParameter("@FromAddress", FbDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new FbParameter("@FromName", FbDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new FbParameter("@ReplyToAddress", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new FbParameter("@SendMode", FbDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new FbParameter("@EnableViewAsWebPage", FbDbType.SmallInt);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new FbParameter("@EnableSendLog", FbDbType.SmallInt);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new FbParameter("@RolesThatCanEdit", FbDbType.VarChar);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new FbParameter("@RolesThatCanApprove", FbDbType.VarChar);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new FbParameter("@RolesThatCanSend", FbDbType.VarChar);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new FbParameter("@CreatedUTC", FbDbType.TimeStamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new FbParameter("@CreatedBy", FbDbType.Char, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new FbParameter("@LastModUTC", FbDbType.TimeStamp);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new FbParameter("@LastModBy", FbDbType.Char, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new FbParameter("@AllowArchiveView", FbDbType.SmallInt);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new FbParameter("@ProfileOptIn", FbDbType.SmallInt);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new FbParameter("@SendMode", FbDbType.Integer);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sendMode;

            arParams[24] = new FbParameter("@SortRank", FbDbType.Integer);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = sortRank;

            arParams[25] = new FbParameter("@DisplayNameDefault", FbDbType.VarChar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = displayNameDefault;

            arParams[26] = new FbParameter("@FirstNameDefault", FbDbType.VarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = firstNameDefault;

            arParams[27] = new FbParameter("@LastNameDefault", FbDbType.VarChar, 50);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = lastNameDefault;


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
            sqlCommand.Append("0, ");
            sqlCommand.Append("0, ");

            sqlCommand.Append("@AllowArchiveView, ");
            sqlCommand.Append("@ProfileOptIn, ");
            sqlCommand.Append("@SortRank, ");

            sqlCommand.Append("@DisplayNameDefault, ");
            sqlCommand.Append("@FirstNameDefault, ");
            sqlCommand.Append("@LastNameDefault, ");

            sqlCommand.Append("@CreatedUTC, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModUTC, ");
            sqlCommand.Append("@LastModBy )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            sqlCommand.Append("AllowArchiveView = @AllowArchiveView, ");
            sqlCommand.Append("ProfileOptIn = @ProfileOptIn, ");
            sqlCommand.Append("SortRank = @SortRank, ");

            sqlCommand.Append("DisplayNameDefault = @DisplayNameDefault, ");
            sqlCommand.Append("FirstNameDefault = @FirstNameDefault, ");
            sqlCommand.Append("LastNameDefault = @LastNameDefault, ");

            sqlCommand.Append("CreatedUTC = @CreatedUTC, ");
            sqlCommand.Append("CreatedBy = @CreatedBy, ");
            sqlCommand.Append("LastModUTC = @LastModUTC, ");
            sqlCommand.Append("LastModBy = @LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[28];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@AvailableToRoles", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new FbParameter("@Enabled", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intEnabled;

            arParams[6] = new FbParameter("@AllowUserFeedback", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intAllowUserFeedback;

            arParams[7] = new FbParameter("@AllowAnonFeedback", FbDbType.SmallInt);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAllowAnonFeedback;

            arParams[8] = new FbParameter("@FromAddress", FbDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new FbParameter("@FromName", FbDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new FbParameter("@ReplyToAddress", FbDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new FbParameter("@SendMode", FbDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new FbParameter("@EnableViewAsWebPage", FbDbType.SmallInt);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intEnableViewAsWebPage;

            arParams[13] = new FbParameter("@EnableSendLog", FbDbType.SmallInt);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intEnableSendLog;

            arParams[14] = new FbParameter("@RolesThatCanEdit", FbDbType.VarChar);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new FbParameter("@RolesThatCanApprove", FbDbType.VarChar);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new FbParameter("@RolesThatCanSend", FbDbType.VarChar);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new FbParameter("@CreatedUTC", FbDbType.TimeStamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new FbParameter("@CreatedBy", FbDbType.Char, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new FbParameter("@LastModUTC", FbDbType.TimeStamp);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new FbParameter("@LastModBy", FbDbType.Char, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new FbParameter("@AllowArchiveView", FbDbType.SmallInt);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intAllowArchiveView;

            arParams[22] = new FbParameter("@ProfileOptIn", FbDbType.SmallInt);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = intProfileOptIn;

            arParams[23] = new FbParameter("@SendMode", FbDbType.Integer);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sendMode;

            arParams[24] = new FbParameter("@SortRank", FbDbType.Integer);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = sortRank;

            arParams[25] = new FbParameter("@DisplayNameDefault", FbDbType.VarChar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = displayNameDefault;

            arParams[26] = new FbParameter("@FirstNameDefault", FbDbType.VarChar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = firstNameDefault;

            arParams[27] = new FbParameter("@LastNameDefault", FbDbType.VarChar, 50);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = lastNameDefault;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid  ");
            sqlCommand.Append("),");
            sqlCommand.Append("UnVerifiedCount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_LetterSubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid AND IsVerified = 0  ");
            sqlCommand.Append(")  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            
            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SiteGuid = @SiteGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

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
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SortRank, Title ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_LetterInfo  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY SortRank, Title ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@PageNumber", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new FbParameter("@PageSize", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



    }
}
