// Author:					
// Created:					2009-02-23
// Last Modified:			2012-08-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    public static class DBEmailSendQueue
    {
       
        /// <summary>
        /// Inserts a row in the mp_EmailSendQueue table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="replyTo"> replyTo </param>
        /// <param name="toAddress"> toAddress </param>
        /// <param name="ccAddress"> ccAddress </param>
        /// <param name="bccAddress"> bccAddress </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="type"> type </param>
        /// <param name="dateToSend"> dateToSend </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid userGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string fromAddress,
            string replyTo,
            string toAddress,
            string ccAddress,
            string bccAddress,
            string subject,
            string textBody,
            string htmlBody,
            string type,
            DateTime dateToSend,
            DateTime createdUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_emailsendqueue (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("specialguid1, ");
            sqlCommand.Append("specialguid2, ");
            sqlCommand.Append("fromaddress, ");
            sqlCommand.Append("replyto, ");
            sqlCommand.Append("toaddress, ");
            sqlCommand.Append("ccaddress, ");
            sqlCommand.Append("bccaddress, ");
            sqlCommand.Append("subject, ");
            sqlCommand.Append("textbody, ");
            sqlCommand.Append("htmlbody, ");
            sqlCommand.Append("type, ");
            sqlCommand.Append("datetosend, ");
            sqlCommand.Append("createdutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":specialguid1, ");
            sqlCommand.Append(":specialguid2, ");
            sqlCommand.Append(":fromaddress, ");
            sqlCommand.Append(":replyto, ");
            sqlCommand.Append(":toaddress, ");
            sqlCommand.Append(":ccaddress, ");
            sqlCommand.Append(":bccaddress, ");
            sqlCommand.Append(":subject, ");
            sqlCommand.Append(":textbody, ");
            sqlCommand.Append(":htmlbody, ");
            sqlCommand.Append(":type, ");
            sqlCommand.Append(":datetosend, ");
            sqlCommand.Append(":createdutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[17];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new NpgsqlParameter(":specialguid1", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1.ToString();

            arParams[5] = new NpgsqlParameter(":specialguid2", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2.ToString();

            arParams[6] = new NpgsqlParameter(":fromaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = fromAddress;

            arParams[7] = new NpgsqlParameter(":replyto", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = replyTo;

            arParams[8] = new NpgsqlParameter(":toaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = toAddress;

            arParams[9] = new NpgsqlParameter(":ccaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ccAddress;

            arParams[10] = new NpgsqlParameter(":bccaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = bccAddress;

            arParams[11] = new NpgsqlParameter(":subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subject;

            arParams[12] = new NpgsqlParameter(":textbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = textBody;

            arParams[13] = new NpgsqlParameter(":htmlbody", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = htmlBody;

            arParams[14] = new NpgsqlParameter(":type", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = type;

            arParams[15] = new NpgsqlParameter(":datetosend", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = dateToSend;

            arParams[16] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Deletes a row from the mp_EmailSendQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_emailsendqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_EmailSendQueue table where DateToSend >= CurrentTime.
        /// </summary>
        /// <param name="currentTime"> currentTime </param>
        public static IDataReader GetEmailToSend(DateTime currentTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_emailsendqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("datetosend >= :currenttime ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":currenttime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = currentTime;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
