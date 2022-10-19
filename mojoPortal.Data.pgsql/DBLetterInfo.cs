/// Author:					
/// Created:				2007-12-27
/// Last Modified:			2018-10-25
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using Npgsql;

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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_letterinfo (");
            sqlCommand.Append("letterinfoguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("description, ");
            sqlCommand.Append("availabletoroles, ");
            sqlCommand.Append("enabled, ");
            sqlCommand.Append("allowuserfeedback, ");
            sqlCommand.Append("allowanonfeedback, ");
            sqlCommand.Append("fromaddress, ");
            sqlCommand.Append("fromname, ");
            sqlCommand.Append("replytoaddress, ");
            sqlCommand.Append("sendmode, ");
            sqlCommand.Append("enableviewaswebpage, ");
            sqlCommand.Append("enablesendlog, ");
            sqlCommand.Append("rolesthatcanedit, ");
            sqlCommand.Append("rolesthatcanapprove, ");
            sqlCommand.Append("rolesthatcansend, ");
            sqlCommand.Append("subscribercount, ");
            sqlCommand.Append("unverifiedcount, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("createdby, ");
            sqlCommand.Append("lastmodutc, ");
            sqlCommand.Append("lastmodby, ");
            sqlCommand.Append("allowarchiveview, ");
            sqlCommand.Append("profileoptin, ");

            sqlCommand.Append("displaynamedefault, ");
            sqlCommand.Append("firstnamedefault, ");
            sqlCommand.Append("lastnamedefault, ");


            sqlCommand.Append("sortrank )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":letterinfoguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":description, ");
            sqlCommand.Append(":availabletoroles, ");
            sqlCommand.Append(":enabled, ");
            sqlCommand.Append(":allowuserfeedback, ");
            sqlCommand.Append(":allowanonfeedback, ");
            sqlCommand.Append(":fromaddress, ");
            sqlCommand.Append(":fromname, ");
            sqlCommand.Append(":replytoaddress, ");
            sqlCommand.Append(":sendmode, ");
            sqlCommand.Append(":enableviewaswebpage, ");
            sqlCommand.Append(":enablesendlog, ");
            sqlCommand.Append(":rolesthatcanedit, ");
            sqlCommand.Append(":rolesthatcanapprove, ");
            sqlCommand.Append(":rolesthatcansend, ");
            sqlCommand.Append(":subscribercount, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":createdby, ");
            sqlCommand.Append(":lastmodutc, ");
            sqlCommand.Append(":lastmodby, ");
            sqlCommand.Append(":allowarchiveview, ");
            sqlCommand.Append(":profileoptin, ");

            sqlCommand.Append(":displaynamedefault, ");
            sqlCommand.Append(":firstnamedefault, ");
            sqlCommand.Append(":lastnamedefault, ");

            sqlCommand.Append(":sortrank ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[28];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new NpgsqlParameter(":availabletoroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new NpgsqlParameter(":enabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = enabled;

            arParams[6] = new NpgsqlParameter(":allowuserfeedback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowUserFeedback;

            arParams[7] = new NpgsqlParameter(":allowanonfeedback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = allowAnonFeedback;

            arParams[8] = new NpgsqlParameter(":fromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new NpgsqlParameter(":fromname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new NpgsqlParameter(":replytoaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new NpgsqlParameter(":sendmode", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new NpgsqlParameter(":enableviewaswebpage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = enableViewAsWebPage;

            arParams[13] = new NpgsqlParameter(":enablesendlog", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = enableSendLog;

            arParams[14] = new NpgsqlParameter(":rolesthatcanedit", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new NpgsqlParameter(":rolesthatcanapprove", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new NpgsqlParameter(":rolesthatcansend", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new NpgsqlParameter(":subscribercount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = 0;

            arParams[18] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdUtc;

            arParams[19] = new NpgsqlParameter(":createdby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = createdBy.ToString();

            arParams[20] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModUtc;

            arParams[21] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = lastModBy.ToString();

            arParams[22] = new NpgsqlParameter(":allowarchiveview", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = allowArchiveView;

            arParams[23] = new NpgsqlParameter(":profileoptin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = profileOptIn;

            arParams[24] = new NpgsqlParameter(":sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = sortRank;

            arParams[25] = new NpgsqlParameter(":displaynamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = displayNameDefault;

            arParams[26] = new NpgsqlParameter(":firstnamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = firstNameDefault;

            arParams[27] = new NpgsqlParameter(":lastnamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = lastNameDefault;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_letterinfo ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("title = :title, ");
            sqlCommand.Append("description = :description, ");
            sqlCommand.Append("availabletoroles = :availabletoroles, ");
            sqlCommand.Append("enabled = :enabled, ");
            sqlCommand.Append("allowuserfeedback = :allowuserfeedback, ");
            sqlCommand.Append("allowanonfeedback = :allowanonfeedback, ");
            sqlCommand.Append("fromaddress = :fromaddress, ");
            sqlCommand.Append("fromname = :fromname, ");
            sqlCommand.Append("replytoaddress = :replytoaddress, ");
            sqlCommand.Append("sendmode = :sendmode, ");
            sqlCommand.Append("enableviewaswebpage = :enableviewaswebpage, ");
            sqlCommand.Append("enablesendlog = :enablesendlog, ");
            sqlCommand.Append("rolesthatcanedit = :rolesthatcanedit, ");
            sqlCommand.Append("rolesthatcanapprove = :rolesthatcanapprove, ");
            sqlCommand.Append("rolesthatcansend = :rolesthatcansend, ");
            
            sqlCommand.Append("createdutc = :createdutc, ");
            sqlCommand.Append("createdby = :createdby, ");
            sqlCommand.Append("lastmodutc = :lastmodutc, ");
            sqlCommand.Append("lastmodby = :lastmodby, ");
            sqlCommand.Append("allowarchiveview = :allowarchiveview, ");
            sqlCommand.Append("profileoptin = :profileoptin, ");

            sqlCommand.Append("displaynamedefault = :displaynamedefault, ");
            sqlCommand.Append("firstnamedefault = :firstnamedefault, ");
            sqlCommand.Append("lastnamedefault = :lastnamedefault, ");

            sqlCommand.Append("sortrank = :sortrank ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[27];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new NpgsqlParameter(":availabletoroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = availableToRoles;

            arParams[5] = new NpgsqlParameter(":enabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = enabled;

            arParams[6] = new NpgsqlParameter(":allowuserfeedback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowUserFeedback;

            arParams[7] = new NpgsqlParameter(":allowanonfeedback", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = allowAnonFeedback;

            arParams[8] = new NpgsqlParameter(":fromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = fromAddress;

            arParams[9] = new NpgsqlParameter(":fromname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = fromName;

            arParams[10] = new NpgsqlParameter(":replytoaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = replyToAddress;

            arParams[11] = new NpgsqlParameter(":sendmode", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = sendMode;

            arParams[12] = new NpgsqlParameter(":enableviewaswebpage", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = enableViewAsWebPage;

            arParams[13] = new NpgsqlParameter(":enablesendlog", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = enableSendLog;

            arParams[14] = new NpgsqlParameter(":rolesthatcanedit", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = rolesThatCanEdit;

            arParams[15] = new NpgsqlParameter(":rolesthatcanapprove", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = rolesThatCanApprove;

            arParams[16] = new NpgsqlParameter(":rolesthatcansend", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = rolesThatCanSend;

            arParams[17] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new NpgsqlParameter(":createdby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = createdBy.ToString();

            arParams[19] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = lastModUtc;

            arParams[20] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastModBy.ToString();

            arParams[21] = new NpgsqlParameter(":allowarchiveview", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = allowArchiveView;

            arParams[22] = new NpgsqlParameter(":profileoptin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = profileOptIn;

            arParams[23] = new NpgsqlParameter(":sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank;

            arParams[24] = new NpgsqlParameter(":displaynamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = displayNameDefault;

            arParams[25] = new NpgsqlParameter(":firstnamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = firstNameDefault;

            arParams[26] = new NpgsqlParameter(":lastnamedefault", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = lastNameDefault;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_letterInfo ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("subscribercount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_lettersubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid  ");
            sqlCommand.Append("),  ");

            sqlCommand.Append("unverifiedcount = (  ");
            sqlCommand.Append("SELECT COUNT(*) ");
            sqlCommand.Append("FROM mp_lettersubscribe  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid AND isverified = false  ");
            sqlCommand.Append(")  ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ;");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_letterinfo_delete(:letterinfoguid)",
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterInfo table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetOne(Guid letterInfoGuid)
        {
            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            //arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = letterInfoGuid.ToString();

            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_letterinfo_select_one(:letterinfoguid)",
            //    arParams);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_letterinfo ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            //sqlCommand.Append("ORDER BY ");
           // sqlCommand.Append("sortrank, title ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_LetterInfo table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
			string sqlCommand = @"SELECT li.*, l.sendclickedutc
				FROM mp_letterinfo li
				LEFT JOIN (SELECT letterinfoguid, MAX(sendclickedutc) AS sendclickedutc FROM mp_Letter GROUP BY letterinfoguid) AS l ON l.letterinfoguid = li.letterinfoguid
				WHERE li.siteguid = :siteguid
				ORDER BY sortrank, title;";

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            

        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterInfo table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_letterinfo_count(:siteguid)",
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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_letterinfo  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append("ORDER BY sortrank, title ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            //totalPages = 1;
            //int totalRows
            //    = GetCount(siteGuid);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            //if (totalRows <= pageSize)
            //{
            //    totalPages = 1;
            //}
            //else
            //{
            //    int remainder;
            //    Math.DivRem(totalRows, pageSize, out remainder);
            //    if (remainder > 0)
            //    {
            //        totalPages += 1;
            //    }
            //}

            //NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            //arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = siteGuid.ToString();

            //arParams[1] = new NpgsqlParameter(":pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[1].Direction = ParameterDirection.Input;
            //arParams[1].Value = pageNumber;

            //arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[2].Direction = ParameterDirection.Input;
            //arParams[2].Value = pageSize;

            //return NpgsqlHelper.ExecuteReader(
            //    ConnectionString.GetReadConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_letterinfo_selectpage(:siteguid,:pagenumber,:pagesize)",
            //    arParams);

        }


    }
}
