using System.Collections.Generic;
using System.Collections.Specialized;

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
        //public NameValueCollection AdditionalHeaders { get; set; } = new NameValueCollection();
        public List<SmtpHeader> AdditionalHeaders { get; set; } = new List<SmtpHeader>();
        public string SenderHeader { get; set; } = string.Empty;
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
