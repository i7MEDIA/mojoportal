using System;
using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers;

public class SitePreDeleteProviderCollection : ProviderCollection
{
	new public SitePreDeleteHandlerProvider this[string name] => (SitePreDeleteHandlerProvider)base[name];
	public void CopyTo(SitePreDeleteHandlerProvider[] array, int index) => base.CopyTo(array, index); 

	public override void Add(ProviderBase provider)
	{

		if (provider == null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not SitePreDeleteHandlerProvider)
		{
			throw new ArgumentException("The provider parameter must be of type SitePreDeleteHandlerProvider.");
		}

		base.Add(provider);
	}
}