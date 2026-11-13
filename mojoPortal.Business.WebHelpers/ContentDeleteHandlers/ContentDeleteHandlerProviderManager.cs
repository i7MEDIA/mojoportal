#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers;

public sealed class ContentDeleteHandlerProviderManager
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(ContentDeleteHandlerProviderManager));
	private static readonly object _lock = new();
	private static readonly List<ContentDeleteHandlerProvider> _registeredProviders = [];
	private static ContentDeleteHandlerProviderCollection? _providerCollection;


	public static void RegisterProvider(ContentDeleteHandlerProvider provider)
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


	private static bool ContainsType(ContentDeleteHandlerProviderCollection collection, Type type)
	{
		foreach (ContentDeleteHandlerProvider existing in collection)
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

			var collection = new ContentDeleteHandlerProviderCollection();

			foreach (var provider in _registeredProviders)
			{
				if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
				{
					collection.Add(provider);
				}
			}

			try
			{
				var config = ContentDeleteHandlerProviderConfig.GetConfig();

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
							var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(ContentDeleteHandlerProvider));

							if (
								p is ContentDeleteHandlerProvider provider &&
								provider is not null &&
								collection[provider.Name] is null)
							{
								collection.Add(provider);
							}
						}
						catch (Exception ex)
						{
							_log.Warn($"Skipping legacy ContentDeleteHandlerProvider '{providerSettings.Name}': {ex.Message}");
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


	public static ContentDeleteHandlerProviderCollection Providers
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
