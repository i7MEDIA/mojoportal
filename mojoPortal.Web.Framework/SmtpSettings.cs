using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// Author:				
    /// Created:			2008-09-12
    /// Last Modified:		2008-09-12
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public class SmtpSettings
    {
        public SmtpSettings()
        { }

        private string user = string.Empty;
        private string password = string.Empty;
        private string server = string.Empty;
        private int port = 25;
        private bool requiresAuthentication = false;
        private bool useSsl = false;
        private string preferredEncoding = string.Empty;

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public bool RequiresAuthentication
        {
            get { return requiresAuthentication; }
            set { requiresAuthentication = value; }
        }

        public bool UseSsl
        {
            get { return useSsl; }
            set { useSsl = value; }
        }

        public string PreferredEncoding
        {
            get { return preferredEncoding; }
            set { preferredEncoding = value; }
        }

        public bool IsValid
        {
            get
            {
                if (server.Length == 0) { return false; }

                if (requiresAuthentication)
                {
                    if (user.Length == 0) { return false; }
                    if (password.Length == 0) { return false; }
                }

                return true;
            }

        }

    }
}
