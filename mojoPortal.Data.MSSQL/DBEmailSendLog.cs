// Author:					
// Created:					2009-03-01
// Last Modified:			2010-07-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace mojoPortal.Data
{

    public static class DBEmailSendLog
    {
        ///// <summary>
        ///// Gets the connection string for read.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetReadConnectionString()
        //{
        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}

        ///// <summary>
        ///// Gets the connection string for write.
        ///// </summary>
        ///// <returns></returns>
        //private static string GetWriteConnectionString()
        //{
        //    if (ConfigurationManager.AppSettings["MSSQLWriteConnectionString"] != null)
        //    {
        //        return ConfigurationManager.AppSettings["MSSQLWriteConnectionString"];
        //    }

        //    return ConfigurationManager.AppSettings["MSSQLConnectionString"];

        //}

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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailSendLog_Insert", 16);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);
            sph.DefineSqlParameter("@ToAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, toAddress);
            sph.DefineSqlParameter("@CcAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, ccAddress);
            sph.DefineSqlParameter("@BccAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, bccAddress);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@TextBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, textBody);
            sph.DefineSqlParameter("@HtmlBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, htmlBody);
            sph.DefineSqlParameter("@Type", SqlDbType.NVarChar, 50, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@SentUtc", SqlDbType.DateTime, ParameterDirection.Input, sentUtc);
            sph.DefineSqlParameter("@FromAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, fromAddress);
            sph.DefineSqlParameter("@ReplyTo", SqlDbType.NVarChar, 100, ParameterDirection.Input, replyTo);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.ExecuteNonQuery();
           

        }

        /// <summary>
        /// Deletes a row from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailSendLog_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailSendLog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailSendLog_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }




    }
}
