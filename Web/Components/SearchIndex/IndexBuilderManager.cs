#nullable enable
using log4net;
using System;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.SearchIndex;

public sealed class IndexBuilderManager
{
	private static readonly ILog log = LogManager.GetLogger(typeof(IndexBuilderManager));

	private static IndexBuilderProviderCollection? providerCollection;

	public static IndexBuilderProviderCollection Providers
	{
		get
		{
			if (providerCollection == null)
			{
				Initialize();
			}

			return providerCollection!; // We know that calling initialize() will populate the collection
		}
	}


	static IndexBuilderManager() => Initialize();


	private static void Initialize()
	{
		providerCollection = [];

		try
		{
			var config = IndexBuilderConfiguration.GetConfig();

			if (config is not null)
			{
				if (config.Providers is null || config.Providers.Count < 1)
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
