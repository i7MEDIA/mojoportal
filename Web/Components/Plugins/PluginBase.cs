#nullable enable
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PageEventHandlers;
using mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
using mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.Web.Plugins;

public abstract class PluginBase : IPlugin
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(PluginBase));

	public abstract Guid Id { get; }
	public abstract string Name { get; }
	public virtual string LegacyApplicationName => Name;
	public abstract Version Version { get; }
	public virtual int Order => 100;


	public virtual void Initialize()
	{
		RegisterResourceManagers();
		RegisterProviders();
	}


	/// <summary>
	/// Automatically discovers and registers strongly-typed ResourceManagers compiled into the plugin assembly.
	/// </summary>
	protected virtual void RegisterResourceManagers()
	{
		var assembly = GetType().Assembly;

		foreach (var type in GetLoadableTypes(assembly))
		{
			if (!type.IsClass)
			{
				continue;
			}

			var rmProp = type.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

			if (rmProp is not null && typeof(ResourceManager).IsAssignableFrom(rmProp.PropertyType))
			{
				try
				{
					if (rmProp.GetValue(null, null) is ResourceManager rm)
					{
						ResourceHelper.RegisterPluginResourceManager(type.Name, rm);
					}
				}
				catch (Exception ex)
				{
					_log.Warn($"Failed to register ResourceManager for {type.FullName}", ex);
				}
			}
		}
	}


	/// <summary>
	/// Automatically discovers and registers provider implementations in this plugin's assembly,
	/// eliminating the need for XML files in Setup/ProviderConfig/.
	/// Override if you need custom provider initialization or ordering.
	/// </summary>
	protected virtual void RegisterProviders()
	{
		var assembly = GetType().Assembly;
		var emptyConfig = new NameValueCollection();

		foreach (var type in GetLoadableTypes(assembly))
		{
			if (type.IsAbstract || type.IsInterface || type.GetConstructor(Type.EmptyTypes) is null)
			{
				continue;
			}

			try
			{
				if (typeof(IndexBuilderProvider).IsAssignableFrom(type))
				{
					var provider = (IndexBuilderProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					IndexBuilderManager.RegisterProvider(provider);
				}
				else if (typeof(ContentDeleteHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (ContentDeleteHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					ContentDeleteHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(SitePreDeleteHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (SitePreDeleteHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					SitePreDeleteHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(PageCreatedEventHandlerPovider).IsAssignableFrom(type))
				{
					var provider = (PageCreatedEventHandlerPovider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					PageCreatedEventHandlerPoviderManager.RegisterProvider(provider);
				}
				else if (typeof(SiteCreatedEventHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (SiteCreatedEventHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					SiteCreatedEventHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(UserRegisteredHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (UserRegisteredHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					UserRegisteredHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(UserSignInHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (UserSignInHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					UserSignInHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(ProfileUpdatedHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (ProfileUpdatedHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					ProfileUpdatedHandlerProviderManager.RegisterProvider(provider);
				}
				else if (typeof(UserPreDeleteHandlerProvider).IsAssignableFrom(type))
				{
					var provider = (UserPreDeleteHandlerProvider)Activator.CreateInstance(type);
					provider.Initialize(GetProviderName(type), emptyConfig);
					UserPreDeleteHandlerProviderManager.RegisterProvider(provider);
				}
			}
			catch (Exception ex)
			{
				_log.Error($"Failed to register provider {type.FullName} in plugin {Name}", ex);
			}
		}
	}


	protected virtual string GetProviderName(Type providerType) => providerType.Name;


	protected static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
	{
		if (assembly is null)
		{
			return [];
		}

		try
		{
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			return ex.Types.Where(t => t is not null);
		}
		catch
		{
			return [];
		}
	}

	public virtual void RegisterRoutes(RouteCollection routes) { }

	public virtual void RegisterWebApi(HttpConfiguration config) { }

	public virtual void RegisterGlobalFilters(GlobalFilterCollection filters) { }

	public virtual IEnumerable<string> GetViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public virtual IEnumerable<string> GetPartialViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public virtual IEnumerable<string> GetMasterLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public virtual IEnumerable<ContentFeature> GetFeatures() => [];
}
