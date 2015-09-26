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
    public static class DBEmailSendLog
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_EmailSendLog table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="toAddress"> toAddress </param>
        /// <param name="ccAddress"> ccAddress </param>
        /// <param name="bccAddress"> bccAddress </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="type"> type </param>
        /// <param name="sentUtc"> sentUtc </param>
        /// <param name="fromAddress"> fromAddress </param>
        /// <param name="replyTo"> replyTo </param>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>int</returns>
        public static void Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string toAddress,
            string ccAddress,
            string bccAddress,
            string subject,
            string textBody,
            string htmlBody,
            string type,
            DateTime sentUtc,
            string fromAddress,
            string replyTo,
            Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_EmailSendLog ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SpecialGuid1, ");
            sqlCommand.Append("SpecialGuid2, ");
            sqlCommand.Append("ToAddress, ");
            sqlCommand.Append("CcAddress, ");
            sqlCommand.Append("BccAddress, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("Type, ");
            sqlCommand.Append("SentUtc, ");
            sqlCommand.Append("FromAddress, ");
            sqlCommand.Append("ReplyTo, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@SpecialGuid1, ");
            sqlCommand.Append("@SpecialGuid2, ");
            sqlCommand.Append("@ToAddress, ");
            sqlCommand.Append("@CcAddress, ");
            sqlCommand.Append("@BccAddress, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@TextBody, ");
            sqlCommand.Append("@HtmlBody, ");
            sqlCommand.Append("@Type, ");
            sqlCommand.Append("@SentUtc, ");
            sqlCommand.Append("@FromAddress, ");
            sqlCommand.Append("@ReplyTo, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[16];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = specialGuid1;

            arParams[4] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid2;

            arParams[5] = new SqlCeParameter("@ToAddress", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = toAddress;

            arParams[6] = new SqlCeParameter("@CcAddress", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = ccAddress;

            arParams[7] = new SqlCeParameter("@BccAddress", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = bccAddress;

            arParams[8] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = subject;

            arParams[9] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = textBody;

            arParams[10] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = htmlBody;

            arParams[11] = new SqlCeParameter("@Type", SqlDbType.NVarChar, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = type;

            arParams[12] = new SqlCeParameter("@SentUtc", SqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = sentUtc;

            arParams[13] = new SqlCeParameter("@FromAddress", SqlDbType.NVarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = fromAddress;

            arParams[14] = new SqlCeParameter("@ReplyTo", SqlDbType.NVarChar, 100);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = replyTo;

            arParams[15] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = userGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            

        }


        /// <summary>
        /// Deletes a row from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailSendLog ");
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
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailSendLog ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
