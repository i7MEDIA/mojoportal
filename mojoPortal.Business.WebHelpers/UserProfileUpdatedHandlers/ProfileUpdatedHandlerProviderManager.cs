#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;

public sealed class ProfileUpdatedHandlerProviderManager
{
	private static readonly object _lock = new();
	private static readonly ILog _log = LogManager.GetLogger(typeof(ProfileUpdatedHandlerProviderManager));
	private static readonly List<ProfileUpdatedHandlerProvider> _registeredProviders = [];
	private static ProfileUpdatedHandlerProviderCollection? _providerCollection;


	public static void RegisterProvider(ProfileUpdatedHandlerProvider provider)
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


	private static bool ContainsType(ProfileUpdatedHandlerProviderCollection collection, Type type)
	{
		foreach (ProfileUpdatedHandlerProvider existing in collection)
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

			var collection = new ProfileUpdatedHandlerProviderCollection();

			foreach (var provider in _registeredProviders)
			{
				if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
				{
					collection.Add(provider);
				}
			}

			try
			{
				var config = ProfileUpdatedHandlerProviderConfig.GetConfig();

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
							var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(ProfileUpdatedHandlerProvider));

							if (
								p is ProfileUpdatedHandlerProvider provider &&
								collection[provider.Name] is null &&
								!ContainsType(collection, provider.GetType()))
							{
								collection.Add(provider);
							}
						}
						catch (Exception ex)
						{
							_log.Warn($"Skipping legacy ProfileUpdatedHandlerProvider '{providerSettings.Name}': {ex.Message}");
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


	public static ProfileUpdatedHandlerProviderCollection Providers
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
