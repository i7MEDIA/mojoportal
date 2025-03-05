using System;
using System.Configuration.Provider;

namespace mojoPortal.SearchIndex;
public class LuceneSettingsProviderCollection : ProviderCollection
{
	public override void Add(ProviderBase provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not LuceneSettingsProvider)
		{
			throw new ArgumentException("The provider parameter must be of type LuceneSettingsProvider.");
		}

		base.Add(provider);
	}

	new public LuceneSettingsProvider this[string name] => (LuceneSettingsProvider)base[name];

	public void CopyTo(LuceneSettingsProvider[] array, int index) => base.CopyTo(array, index);
}