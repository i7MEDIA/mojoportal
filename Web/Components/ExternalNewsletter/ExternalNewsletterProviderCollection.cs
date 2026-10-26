using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.ExternalNewsletter
{
    public class ExternalNewsletterProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider), "The provider parameter cannot be null.");

            if (!(provider is ExternalNewsletterProvider))
                throw new ArgumentException("The provider parameter must be of type ExternalNewsletterProvider.", nameof(provider));

            base.Add(provider);
        }

        new public ExternalNewsletterProvider this[string name]
        {
            get { return (ExternalNewsletterProvider)base[name]; }
        }

        public void CopyTo(ExternalNewsletterProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
