// Author:				
// Created:			    2008-05-19
// Last Modified:	    2014-07-04
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
using System.Collections.Generic;
using System.Text;
using mojoPortal.Web.Framework;

namespace mojoPortal.Net
{
    
    public class ForumNotificationInfo
    {
        public ForumNotificationInfo()
        { }

        private string subjectTemplate = string.Empty;
        private string bodyTemplate = string.Empty;
        private string forumOnlyTemplate = string.Empty;
        private string threadOnlyTemplate = string.Empty;
        private string moderatorTemplate = string.Empty;
        private string fromEmail = string.Empty;
        private string fromAlias = string.Empty;
        private string siteName = string.Empty;
        private string moduleName = string.Empty;
        private string forumName = string.Empty;
        private string subject = string.Empty;
        private string messageBody = string.Empty;
        private DataSet subscribers = null;
        private string messageLink = string.Empty;
        private string unsubscribeForumThreadLink = string.Empty;
        private string unsubscribeForumLink = string.Empty;
        private SmtpSettings smtpSettings = null;
        private List<string> moderatorEmailAddresses = null;

        private int threadId = -1;

        public int ThreadId
        {
            get { return threadId; }
            set { threadId = value; }
        }

        private int postId = -1;

        public int PostId
        {
            get { return postId; }
            set { postId = value; }
        }

        public List<string> ModeratorEmailAddresses
        {
            get { return moderatorEmailAddresses; }
            set { moderatorEmailAddresses = value; }
        }

        public SmtpSettings SmtpSettings
        {
            get { return smtpSettings; }
            set { smtpSettings = value; }
        }

        public string SubjectTemplate
        {
            get { return subjectTemplate; }
            set { subjectTemplate = value; }
        }

        public string BodyTemplate
        {
            get { return bodyTemplate; }
            set { bodyTemplate = value; }
        }

        public string ForumOnlyTemplate
        {
            get { return forumOnlyTemplate; }
            set { forumOnlyTemplate = value; }
        }

        public string ThreadOnlyTemplate
        {
            get { return threadOnlyTemplate; }
            set { threadOnlyTemplate = value; }
        }

        public string ModeratorTemplate
        {
            get { return moderatorTemplate; }
            set { moderatorTemplate = value; }
        }

        public string FromEmail
        {
            get { return fromEmail; }
            set { fromEmail = value; }
        }

        public string FromAlias
        {
            get { return fromAlias; }
            set { fromAlias = value; }
        }

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        public string ForumName
        {
            get { return forumName; }
            set { forumName = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string MessageBody
        {
            get { return messageBody; }
            set { messageBody = value; }
        }

        public DataSet Subscribers
        {
            get { return subscribers; }
            set { subscribers = value; }
        }

        public string MessageLink
        {
            get { return messageLink; }
            set { messageLink = value; }
        }

        public string UnsubscribeForumThreadLink
        {
            get { return unsubscribeForumThreadLink; }
            set { unsubscribeForumThreadLink = value; }
        }

        public string UnsubscribeForumLink
        {
            get { return unsubscribeForumLink; }
            set { unsubscribeForumLink = value; }
        }
        
    }
}
