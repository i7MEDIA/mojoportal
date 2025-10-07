using System;
using System.Configuration.Provider;

namespace mojoPortal.SearchIndex;

public class IndexBuilderProviderCollection : ProviderCollection
{
	public override void Add(ProviderBase provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not IndexBuilderProvider)
		{
			throw new ArgumentException("The provider parameter must be of type IndexBuilderProvider.");
		}

		base.Add(provider);
	}

	new public IndexBuilderProvider this[string name] => (IndexBuilderProvider)base[name];

	public void CopyTo(IndexBuilderProvider[] array, int index) => base.CopyTo(array, index);
}
