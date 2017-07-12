// Author:					
// Created:					2009-03-01
// Last Modified:			2009-03-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
    public static class DBEmailSendQueue
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_EmailSendQueue (");
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
            sqlCommand.Append("CreatedUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":SpecialGuid1, ");
            sqlCommand.Append(":SpecialGuid2, ");
            sqlCommand.Append(":FromAddress, ");
            sqlCommand.Append(":ReplyTo, ");
            sqlCommand.Append(":ToAddress, ");
            sqlCommand.Append(":CcAddress, ");
            sqlCommand.Append(":BccAddress, ");
            sqlCommand.Append(":Subject, ");
            sqlCommand.Append(":TextBody, ");
            sqlCommand.Append(":HtmlBody, ");
            sqlCommand.Append(":Type, ");
            sqlCommand.Append(":DateToSend, ");
            sqlCommand.Append(":CreatedUtc )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[17];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1.ToString();

            arParams[5] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2.ToString();

            arParams[6] = new SqliteParameter(":FromAddress", DbType.String, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = fromAddress;

            arParams[7] = new SqliteParameter(":ReplyTo", DbType.String, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = replyTo;

            arParams[8] = new SqliteParameter(":ToAddress", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = toAddress;

            arParams[9] = new SqliteParameter(":CcAddress", DbType.String, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ccAddress;

            arParams[10] = new SqliteParameter(":BccAddress", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = bccAddress;

            arParams[11] = new SqliteParameter(":Subject", DbType.String, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = subject;

            arParams[12] = new SqliteParameter(":TextBody", DbType.Object);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = textBody;

            arParams[13] = new SqliteParameter(":HtmlBody", DbType.Object);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = htmlBody;

            arParams[14] = new SqliteParameter(":Type", DbType.String, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = type;

            arParams[15] = new SqliteParameter(":DateToSend", DbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = dateToSend;

            arParams[16] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdUtc;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

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
            sqlCommand.Append("DateToSend >= :CurrentTime ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":CurrentTime", DbType.DateTime);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = currentTime;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
