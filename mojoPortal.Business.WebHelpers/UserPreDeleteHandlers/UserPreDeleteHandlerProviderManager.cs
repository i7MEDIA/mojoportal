#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace mojoPortal.Business.WebHelpers
{
	public sealed class UserPreDeleteHandlerProviderManager
	{
		private static readonly object _lock = new();
		private static readonly ILog _log = LogManager.GetLogger(typeof(UserPreDeleteHandlerProviderManager));
		private static readonly List<UserPreDeleteHandlerProvider> _registeredProviders = [];
		private static UserPreDeleteHandlerProviderCollection? _providerCollection;


		public static void RegisterProvider(UserPreDeleteHandlerProvider provider)
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


		private static bool ContainsType(UserPreDeleteHandlerProviderCollection collection, Type type)
		{
			foreach (UserPreDeleteHandlerProvider existing in collection)
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

				var collection = new UserPreDeleteHandlerProviderCollection();

				foreach (var provider in _registeredProviders)
				{
					if (collection[provider.Name] is null && !ContainsType(collection, provider.GetType()))
					{
						collection.Add(provider);
					}
				}

				try
				{
					var config = UserPreDeleteHandlerProviderConfig.GetConfig();

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
								var p = ProvidersHelper.InstantiateProvider(providerSettings, typeof(UserPreDeleteHandlerProvider));

								if (
									p is UserPreDeleteHandlerProvider provider &&
									collection[provider.Name] is null &&
									!ContainsType(collection, provider.GetType()))
								{
									collection.Add(provider);
								}
							}
							catch (Exception ex)
							{
								_log.Warn($"Skipping legacy UserPreDeleteHandlerProvider '{providerSettings.Name}': {ex.Message}");
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


		public static UserPreDeleteHandlerProviderCollection Providers
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
}
