// Author:					
// Created:					2009-03-08
// Last Modified:			2009-03-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class EmailSendLog
    {
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
            DBEmailSendLog.Create(
                guid,
                siteGuid,
                moduleGuid,
                specialGuid1,
                specialGuid2,
                toAddress,
                ccAddress,
                bccAddress,
                subject,
                textBody,
                htmlBody,
                type,
                sentUtc,
                fromAddress,
                replyTo,
                userGuid);
        

        }

        /// <summary>
        /// Deletes a row from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBEmailSendLog.Delete(guid);

        }

        /// <summary>
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBEmailSendLog.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
        /// </summary>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBEmailSendLog.DeleteByModule(moduleGuid);
        }


    }
}
