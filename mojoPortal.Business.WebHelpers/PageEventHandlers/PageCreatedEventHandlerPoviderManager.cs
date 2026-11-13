#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers.PageEventHandlers;

public sealed class PageCreatedEventHandlerPoviderManager
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(PageCreatedEventHandlerPoviderManager));
	private static readonly object _lock = new object();
	private static readonly List<PageCreatedEventHandlerPovider> _registeredProviders = [];
	private static PageCreatedEventHandlerPoviderCollection? _providerCollection;


	public static void RegisterProvider(PageCreatedEventHandlerPovider provider)
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


	private static bool ContainsType(PageCreatedEventHandlerPoviderCollection collection, Type type)
	{
		foreach (PageCreatedEventHandlerPovider existing in collection)
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
			if (_providerCollection is not null)
			{
				return;
			}

			var collection = new PageCreatedEventHandlerPoviderCollection();

			foreach (var provider in _registeredProviders)
			{
				if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
				{
					collection.Add(provider);
				}
			}

			try
			{
				var config = PageCreatedEventHandlerPoviderConfig.GetConfig();

				if (config?.Providers != null)
				{
					foreach (ProviderSettings providerSettings in config.Providers)
					{
						if (collection[providerSettings.Name] is not null)
						{
							continue;
						}

						try
						{
							var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(PageCreatedEventHandlerPovider));

							if (
								p is PageCreatedEventHandlerPovider provider &&
								collection[provider.Name] is null &&
								!ContainsType(collection, provider.GetType()))
							{
								collection.Add(provider);
							}
						}
						catch (Exception ex)
						{
							_log.Warn($"Skipping legacy PageCreatedEventHandlerPovider '{providerSettings.Name}': {ex.Message}");
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

	public static PageCreatedEventHandlerPoviderCollection Providers
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
