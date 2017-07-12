//  Author:                     
//  Created:                    2008-09-04
//	Last Modified:              2011-07-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using log4net;
using Resources;


namespace mojoPortal.Web
{
    /// <summary>
    ///  
    /// </summary>
    public class NotifyAdminUserRegisteredHandler : UserRegisteredHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(NotifyAdminUserRegisteredHandler));

        public NotifyAdminUserRegisteredHandler()
        { }

        public override void UserRegisteredHandler(object sender, UserRegisteredEventArgs e)
        {
            if (e == null) return;
            if (e.SiteUser == null) return;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (
                (!siteSettings.RequireApprovalBeforeLogin)
                &&(siteSettings.EmailAdressesForUserApprovalNotification.Length == 0)
                ) { return; }

            log.Debug("NotifyAdminUserRegisteredHandler called for new user " + e.SiteUser.Email);

            if (HttpContext.Current == null) { return; }

            CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();

            string subjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        "NotifyAdminofNewUserRegistationSubject.config");

            string textBodyTemplate = ResourceHelper.GetMessageTemplate(defaultCulture,
                        "NotifyAdminofNewUserRegistationMessage.config");

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

            //lookup admin users and send notification email with link to manage user
            List<string> adminEmails;
            if (siteSettings.EmailAdressesForUserApprovalNotification.Length > 0)
            {
                adminEmails = siteSettings.EmailAdressesForUserApprovalNotification.SplitOnChar(',');
            }
            else
            {
                adminEmails = SiteUser.GetEmailAddresses(siteSettings.SiteId, "Admins;");
            }

            //foreach (DataRow row in admins.Rows)
            foreach(string email in adminEmails)
            {
                if (WebConfigSettings.EmailAddressesToExcludeFromAdminNotifications.IndexOf(email, StringComparison.InvariantCultureIgnoreCase) > -1) { continue; }

                //EmailMessageTask messageTask = new EmailMessageTask(smtpSettings);
                //messageTask.EmailFrom = siteSettings.DefaultEmailFromAddress;
                //messageTask.EmailFromAlias = siteSettings.DefaultFromEmailAlias;
                //messageTask.EmailTo = email;
                //messageTask.Subject = string.Format(defaultCulture, subjectTemplate, e.SiteUser.Email, siteRoot);

                string manageUserLink = siteRoot + "/Admin/ManageUsers.aspx?userid="
                    + e.SiteUser.UserId.ToInvariantString();

                //messageTask.TextBody = string.Format(defaultCulture, textBodyTemplate, siteSettings.SiteName, siteRoot, manageUserLink);
                //messageTask.SiteGuid = siteSettings.SiteGuid;
                //messageTask.QueueTask();

                Email.Send(
                        smtpSettings,
                        siteSettings.DefaultEmailFromAddress,
                        siteSettings.DefaultFromEmailAlias,
                        string.Empty,
                        email,
                        string.Empty,
                        string.Empty,
                        string.Format(defaultCulture, subjectTemplate, e.SiteUser.Email, siteRoot),
                        string.Format(defaultCulture, textBodyTemplate, siteSettings.SiteName, siteRoot, manageUserLink),
                        false,
                        Email.PriorityNormal);

            }

            //WebTaskManager.StartOrResumeTasks();


        }


    }
}
