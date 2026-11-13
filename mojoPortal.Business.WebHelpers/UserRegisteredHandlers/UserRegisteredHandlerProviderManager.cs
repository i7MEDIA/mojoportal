#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers.UserRegisteredHandlers;

public sealed class UserRegisteredHandlerProviderManager
{
	private static readonly object _lock = new();
	private static readonly ILog _log = LogManager.GetLogger(typeof(UserRegisteredHandlerProviderManager));
	private static readonly List<UserRegisteredHandlerProvider> _registeredProviders = [];
	private static UserRegisteredHandlerProviderCollection? _providerCollection;


	public static void RegisterProvider(UserRegisteredHandlerProvider provider)
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


	private static bool ContainsType(UserRegisteredHandlerProviderCollection collection, Type type)
	{
		foreach (UserRegisteredHandlerProvider existing in collection)
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
			if (_providerCollection is null)
			{
				return;
			}

			var collection = new UserRegisteredHandlerProviderCollection();

			foreach (var provider in _registeredProviders)
			{
				if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
				{
					collection.Add(provider);
				}
			}

			try
			{
				var config = UserRegisteredHandlerProviderConfig.GetConfig();

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
							var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(UserRegisteredHandlerProvider));

							if (
								p is UserRegisteredHandlerProvider provider &&
								collection[provider.Name] is null &&
								!ContainsType(collection, provider.GetType()))
							{
								collection.Add(p);
							}
						}
						catch (Exception ex)
						{
							_log.Warn($"Skipping legacy UserRegisteredHandlerProvider '{providerSettings.Name}': {ex.Message}");
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


	public static UserRegisteredHandlerProviderCollection Providers
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
