using System;
using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;


public class SiteCreatedEventHandlerProviderCollection : ProviderCollection
{
	public override void Add(ProviderBase provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not SiteCreatedEventHandlerProvider)
		{
			throw new ArgumentException("The provider parameter must be of type PageCreatedEventHandlerPovider.");
		}

		base.Add(provider);
	}

	new public SiteCreatedEventHandlerProvider this[string name] => (SiteCreatedEventHandlerProvider)base[name];


	public void CopyTo(SiteCreatedEventHandlerProvider[] array, int index) => base.CopyTo(array, index);
}
