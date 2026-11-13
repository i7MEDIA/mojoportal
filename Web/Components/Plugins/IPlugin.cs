#nullable enable
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.Web.Plugins;

public interface IPlugin
{
	/// <summary>
	/// Unique application/plugin identifier (maps to mp_SchemaVersion.ApplicationID).
	/// </summary>
	Guid Id { get; }

	/// <summary>
	/// Human-readable plugin name (maps to mp_SchemaVersion.ApplicationName).
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Optional legacy application folder name in ~/Setup/applications/ (if different from Name)
	/// used to prevent duplicate execution if legacy setup scripts were not deleted on upgrade.
	/// </summary>
	string LegacyApplicationName { get; }

	/// <summary>
	/// Current version of the plugin. Compared against mp_SchemaVersion during Setup.
	/// </summary>
	Version Version { get; }

	/// <summary>
	/// Execution order for route and view registration. Lower numbers run first (higher priority).
	/// Can be overridden in ~/Plugins/plugins.config or ~/Plugins/<PluginName>/plugin.config.
	/// </summary>
	int Order { get; }

	/// <summary>
	/// Called during application startup after all plugins are discovered and sorted by Order.
	/// </summary>
	void Initialize();

	/// <summary>
	/// Register MVC 5 routes.
	/// </summary>
	void RegisterRoutes(RouteCollection routes);

	/// <summary>
	/// Register Web API 2.0 routes and configuration.
	/// </summary>
	void RegisterWebApi(HttpConfiguration config);

	/// <summary>
	/// Register MVC 5 global filters.
	/// </summary>
	void RegisterGlobalFilters(GlobalFilterCollection filters);

	/// <summary>
	/// Return custom Razor view location formats for this plugin.
	/// </summary>
	IEnumerable<string> GetViewLocationFormats(string pluginFolderName);

	/// <summary>
	/// Return custom Razor partial-view location formats for this plugin.
	/// </summary>
	IEnumerable<string> GetPartialViewLocationFormats(string pluginFolderName);

	/// <summary>
	/// Return custom Razor master/layout location formats for this plugin.
	/// </summary>
	IEnumerable<string> GetMasterLocationFormats(string pluginFolderName);

	/// <summary>
	/// Return the ContentFeature definitions (and their ContentFeatureSettings) provided by this plugin.
	/// Replaces legacy FeatureDefinitions/*.config XML files.
	/// </summary>
	IEnumerable<ContentFeature> GetFeatures();
}
