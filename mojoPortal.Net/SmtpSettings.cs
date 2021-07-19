// Author:				
// Created:			2008-09-12
// Last Modified:		2019-11-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


namespace mojoPortal.Net
{

	public class SmtpSettings
    {
        public SmtpSettings()
        { }

		public string User { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string Server { get; set; } = string.Empty;

		public int Port { get; set; } = 25;

		public bool RequiresAuthentication { get; set; } = false;

		public bool UseSsl { get; set; } = false;

		public string PreferredEncoding { get; set; } = string.Empty;

		public bool AddBulkMailHeader { get; set; } = false;
		public string AdditionalHeaders { get; set; } = string.Empty;
		public bool IsValid
        {
            get
            {
                if (Server.Length == 0) { return false; }

                if (RequiresAuthentication)
                {
                    if (User.Length == 0) { return false; }
                    if (Password.Length == 0) { return false; }
                }

                return true;
            }

        }

    }
}
