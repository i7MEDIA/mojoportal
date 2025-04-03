using log4net;
using System;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;

public sealed class SiteCreatedEventHandlerProviderManager
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SiteCreatedEventHandlerProviderManager));
	private static SiteCreatedEventHandlerProviderCollection providerCollection;


	public static SiteCreatedEventHandlerProviderCollection Providers
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


	static SiteCreatedEventHandlerProviderManager()
	{
		Initialize();
	}


	private static void Initialize()
	{
		//needs to be initialized here, not as default
		providerCollection = [];

		try
		{
			var config = SiteCreatedEventHandlerProviderConfig.GetConfig();

			if (config != null)
			{
				if (
					config.Providers == null ||
					config.Providers.Count < 1
				)
				{
					throw new ProviderException("No SiteCreatedEventHandlerProviderCollection found.");
				}

				ProvidersHelper.InstantiateProviders(
					config.Providers,
					providerCollection,
					typeof(SiteCreatedEventHandlerProvider)
				);
			}
			else
			{
				// config was null, not a good thing
				log.Error("SiteCreatedHandlerConfig could not be loaded so empty provider collection was returned");
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
