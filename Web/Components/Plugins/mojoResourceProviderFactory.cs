#nullable enable
using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Web.Compilation;

namespace mojoPortal.Web.Plugins;

public sealed class mojoResourceProviderFactory : ResourceProviderFactory
{
	private static readonly ConcurrentDictionary<string, IResourceProvider> _globalProviders = new(StringComparer.OrdinalIgnoreCase);

	public override IResourceProvider CreateGlobalResourceProvider(string classKey)
	{
		if (string.IsNullOrWhiteSpace(classKey))
		{
			return new mojoGlobalResourceProvider(string.Empty);
		}

		return _globalProviders.GetOrAdd(classKey, key => new mojoGlobalResourceProvider(key));
	}

	public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
	{
		return new mojoLocalResourceProvider(virtualPath);
	}


	private sealed class mojoGlobalResourceProvider : IResourceProvider
	{
		private readonly string _classKey;
		private ResourceManager? _appGlobalResourceManager;
		private bool _appGlobalResourceNotFound;
		private readonly object _syncLock = new();

		public mojoGlobalResourceProvider(string classKey)
		{
			_classKey = classKey;
		}

		public object? GetObject(string resourceKey, CultureInfo? culture)
		{
			if (string.IsNullOrWhiteSpace(_classKey) || string.IsNullOrWhiteSpace(resourceKey))
			{
				return null;
			}

			var effectiveCulture = culture ?? CultureInfo.CurrentUICulture;

			// 1. Check standard ~/App_GlobalResources/ compiled assembly first
			if (!_appGlobalResourceNotFound)
			{
				var rm = EnsureAppGlobalResourceManager();

				if (rm is not null)
				{
					try
					{
						var val = rm.GetObject(resourceKey, effectiveCulture);

						if (val is not null)
						{
							return val;
						}
					}
					catch (MissingManifestResourceException)
					{
						_appGlobalResourceNotFound = true;
					}
					catch
					{ }
				}
			}

			// 2. Fallback to plugin resources (embedded ResourceManager or ~/Plugins/<Plugin>/App_GlobalResources/*.resx)
			return ResourceHelper.GetPluginResourceString(_classKey, resourceKey, effectiveCulture);
		}

		public IResourceReader ResourceReader => EmptyResourceReader.Instance;

		private ResourceManager? EnsureAppGlobalResourceManager()
		{
			if (_appGlobalResourceManager is not null || _appGlobalResourceNotFound)
			{
				return _appGlobalResourceManager;
			}

			lock (_syncLock)
			{
				if (_appGlobalResourceManager is not null || _appGlobalResourceNotFound)
				{
					return _appGlobalResourceManager;
				}

				try
				{
					var asm = Assembly.Load("App_GlobalResources");

					if (asm is not null)
					{
						var rm = new ResourceManager($"Resources.{_classKey}", asm)
						{
							IgnoreCase = true
						};

						_appGlobalResourceManager = rm;
						return rm;
					}
				}
				catch
				{
					// App_GlobalResources may not be loaded or does not contain this resource
				}

				return null;
			}
		}
	}


	private sealed class mojoLocalResourceProvider : IResourceProvider
	{
		private readonly string _virtualPath;

		public mojoLocalResourceProvider(string virtualPath)
		{
			_virtualPath = PluginFileResolver.ResolveVirtualPath(virtualPath);
		}

		public object? GetObject(string resourceKey, CultureInfo? culture)
		{
			return null;
		}

		public IResourceReader ResourceReader => EmptyResourceReader.Instance;
	}


	private sealed class EmptyResourceReader : IResourceReader
	{
		public static readonly EmptyResourceReader Instance = new();

		public void Close()
		{ }

		public void Dispose()
		{ }

		public IDictionaryEnumerator GetEnumerator() => new Hashtable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
