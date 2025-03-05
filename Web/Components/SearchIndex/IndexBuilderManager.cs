using System;
using System.Configuration.Provider;
using System.Web.Configuration;
using log4net;

namespace mojoPortal.SearchIndex;

public sealed class IndexBuilderManager
{
	//private static bool isInitialized = false;
	//private static Exception initializationException;
	private static object initializationLock = new object();
	private static readonly ILog log = LogManager.GetLogger(typeof(IndexBuilderManager));
	private static IndexBuilderProviderCollection providerCollection;

	public static IndexBuilderProviderCollection Providers
	{
		get
		{
			if (providerCollection == null)
			{
				Initialize();
			}

			return providerCollection;
		}
	}
	static IndexBuilderManager() => Initialize();
	private static void Initialize()
	{
		providerCollection = [];

		try
		{
			IndexBuilderConfiguration config = IndexBuilderConfiguration.GetConfig();

			if (config != null)
			{
				if (config.Providers == null || config.Providers.Count < 1)
				{
					throw new ProviderException("No IndexBuilderProvider found.");
				}

				ProvidersHelper.InstantiateProviders(config.Providers, providerCollection, typeof(IndexBuilderProvider));
			}
			else
			{
				// config was null, not a good thing
				log.Error("IndexBuilderConfiguration could not be loaded so empty provider collection was returned");
			}
		}
		catch (NullReferenceException ex)
		{
			log.Error(ex);
		}
		catch (TypeInitializationException ex)
		{
			log.Error(ex);
		}
		catch (ProviderException ex)
		{
			log.Error(ex);
		}

		providerCollection.SetReadOnly();
	}
}
