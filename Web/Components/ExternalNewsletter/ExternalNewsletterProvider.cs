using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace mojoPortal.Web.ExternalNewsletter
{
    public abstract class ExternalNewsletterProvider : ProviderBase
    {
        public abstract List<ExternalNewsletterInfo> GetAvailableNewsletters(Guid siteGuid);
        public abstract bool Subscribe(ExternalNewsletterInfo newsletter, string email, string firstName = "", string lastName = "");
        public abstract bool Unsubscribe(ExternalNewsletterInfo newsletter, string email);
    }
}
