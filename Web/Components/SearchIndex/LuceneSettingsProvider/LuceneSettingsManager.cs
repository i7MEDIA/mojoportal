using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.SearchIndex;

public sealed class LuceneSettingsManager
{
	private static bool isInitialized = false;

	static LuceneSettingsManager()
	{
		if (!isInitialized)
		{
			Initialize();
		}
	}

	private static void Initialize()
	{
		var config = LuceneSettingsConfiguration.GetConfig();

		if (config.DefaultProvider is null
			|| config.Providers is null
			|| config.Providers.Count < 1
			)
		{
			throw new ProviderException("You must specify a valid default provider.");
		}


		Providers = [];

		ProvidersHelper.InstantiateProviders(config.Providers, Providers, typeof(LuceneSettingsProvider));

		Providers.SetReadOnly();
		
		Provider = Providers[config.DefaultProvider];

		isInitialized = true;
	}

	public static LuceneSettingsProvider Provider { get; private set; }

	public static LuceneSettingsProviderCollection Providers { get; private set; }
}