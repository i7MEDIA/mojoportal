using System;
using mojoPortal.Business;

namespace mojoPortal.Web.ExternalNewsletter
{
    public class ExternalNewsletterInfo : LetterInfo
    {
        public string ProviderName { get; set; } = string.Empty;
        public string ExternalListId { get; set; } = string.Empty;

        public ExternalNewsletterInfo()
        {
            AllowArchiveView = false;
            Enabled = true;
            AvailableToRoles = "All Users;";
        }
    }
}
