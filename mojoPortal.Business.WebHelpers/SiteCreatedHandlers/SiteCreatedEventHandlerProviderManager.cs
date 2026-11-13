#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;

public sealed class SiteCreatedEventHandlerProviderManager
{
	private static readonly object _lock = new();
	private static readonly ILog _log = LogManager.GetLogger(typeof(SiteCreatedEventHandlerProviderManager));
	private static readonly List<SiteCreatedEventHandlerProvider> _registeredProviders = [];
	private static SiteCreatedEventHandlerProviderCollection? _providerCollection;


	public static void RegisterProvider(SiteCreatedEventHandlerProvider provider)
	{
		if (provider is null)
		{
			return;
		}

		lock (_lock)
		{
			if (
				!_registeredProviders.Exists(p => string.Equals(p.Name, provider.Name, StringComparison.OrdinalIgnoreCase) ||
					p.GetType().FullName == provider.GetType().FullName))
			{
				_registeredProviders.Add(provider);
			}

			if (
				_providerCollection is not null &&
				_providerCollection[provider.Name] is null &&
				!ContainsType(_providerCollection, provider.GetType()))
			{
				_providerCollection.Add(provider);
			}
		}
	}


	private static bool ContainsType(SiteCreatedEventHandlerProviderCollection collection, Type type)
	{
		foreach (SiteCreatedEventHandlerProvider existing in collection)
		{
			if (existing.GetType().FullName == type.FullName)
			{
				return true;
			}
		}

		return false;
	}


	private static void Initialize()
	{
		lock (_lock)
		{
			if (_providerCollection is not null) return;

			var collection = new SiteCreatedEventHandlerProviderCollection();

			foreach (var provider in _registeredProviders)
			{
				if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
				{
					collection.Add(provider);
				}
			}

			try
			{
				var config = SiteCreatedEventHandlerProviderConfig.GetConfig();

				if (config?.Providers is not null)
				{
					foreach (ProviderSettings providerSettings in config.Providers)
					{
						if (collection[providerSettings.Name] is not null)
						{
							continue;
						}

						try
						{
							var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(SiteCreatedEventHandlerProvider));

							if (
								p is SiteCreatedEventHandlerProvider provider &&
								collection[provider.Name] is null &&
								!ContainsType(collection, provider.GetType()))
							{
								collection.Add(provider);
							}
						}
						catch (Exception ex)
						{
							_log.Warn($"Skipping legacy SiteCreatedEventHandlerProvider '{providerSettings.Name}': {ex.Message}");
						}
					}
				}
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}

			_providerCollection = collection;
		}
	}


	public static SiteCreatedEventHandlerProviderCollection Providers
	{
		get
		{
			if (_providerCollection is null)
			{
				Initialize();
			}

			return _providerCollection!;
		}
	}
}
