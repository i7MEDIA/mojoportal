// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2010-04-04
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
    public static class DBEmailSendQueue
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

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
            sqlCommand.Append("INSERT INTO mp_EmailSendQueue ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("SpecialGuid1, ");
            sqlCommand.Append("SpecialGuid2, ");
            sqlCommand.Append("FromAddress, ");
            sqlCommand.Append("ReplyTo, ");
            sqlCommand.Append("ToAddress, ");
            sqlCommand.Append("CcAddress, ");
            sqlCommand.Append("BccAddress, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("Type, ");
            sqlCommand.Append("DateToSend, ");
            sqlCommand.Append("CreatedUtc ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@SpecialGuid1, ");
            sqlCommand.Append("@SpecialGuid2, ");
            sqlCommand.Append("@FromAddress, ");
            sqlCommand.Append("@ReplyTo, ");
            sqlCommand.Append("@ToAddress, ");
            sqlCommand.Append("@CcAddress, ");
            sqlCommand.Append("@BccAddress, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@TextBody, ");
            sqlCommand.Append("@HtmlBody, ");
            sqlCommand.Append("@Type, ");
            sqlCommand.Append("@DateToSend, ");
            sqlCommand.Append("@CreatedUtc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[17];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1;

            arParams[5] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2;

            arParams[6] = new SqlCeParameter("@FromAddress", SqlDbType.NVarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = fromAddress;

            arParams[7] = new SqlCeParameter("@ReplyTo", SqlDbType.NVarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = replyTo;

            arParams[8] = new SqlCeParameter("@ToAddress", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = toAddress;

            arParams[9] = new SqlCeParameter("@CcAddress", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ccAddress;

            arParams[10] = new SqlCeParameter("@BccAddress", SqlDbType.NVarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = bccAddress;

            arParams[11] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subject;

            arParams[12] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = textBody;

            arParams[13] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = htmlBody;

            arParams[14] = new SqlCeParameter("@Type", SqlDbType.NVarChar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = type;

            arParams[15] = new SqlCeParameter("@DateToSend", SqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = dateToSend;

            arParams[16] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_EmailSendQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_EmailSendQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("DateToSend <= @CurrentTime ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = currentTime;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
