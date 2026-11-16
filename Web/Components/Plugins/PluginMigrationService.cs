#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI.Pages;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;

namespace mojoPortal.Web.Plugins;

public class PluginMigrationService : IPluginMigrationService
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(PluginMigrationService));
	private static readonly Version _zeroVersion = new(0, 0, 0, 0);

	public void RunMigrations(Action<SetupStatus, string>? writeOutput, bool canAlterSchema)
	{
		var dbPlatform = DatabaseHelper.DBPlatform();

		foreach (var descriptor in PluginLoader.EnabledPlugins)
		{
			var plugin = descriptor.Instance;

			if (plugin is null)
			{
				continue;
			}

			try
			{
				if (canAlterSchema)
				{
					RunPluginSchemaScripts(descriptor, plugin, dbPlatform, writeOutput);
				}

				ConfigurePluginFeatures(plugin, dbPlatform, writeOutput);
			}
			catch (Exception ex)
			{
				_log.Error($"Error migrating plugin '{plugin.Name}' ({descriptor.FolderName}).", ex);

				writeOutput?.Invoke(SetupStatus.Error, $"Error migrating plugin {plugin.Name}: {ex.Message}");
			}
		}
	}


	private static readonly string[] _knownMigratedPluginAppNames =
	[
		"betterimagegallery",
		"blog",
		"childpagesitemap",
		"comments",
		"contactform",
		"custommenu",
		"customscript",
		"eventcalendar",
		"feed-manager",
		"feedmanager",
		"forums",
		"googlemap",
		"googletranslate",
		"htmlcontent",
		"htmlinclude",
		"iframe",
		"imagegallery",
		"kdmediaplayer",
		"list",
		"login",
		"newslettersubscribe",
		"poll",
		"recentcontent",
		"sharedfiles",
		"sitesearch",
		"sitestatistics",
		"superflexi",
		"survey",
		"xmlxsl"
	];


	/// <summary>
	/// Returns the set of application names (both current Name and LegacyApplicationName) handled by plugins
	/// so legacy ~/Setup/applications/ folders are not executed a second time if left behind on upgrade or when plugins are disabled/unloaded.
	/// </summary>
	public static HashSet<string> GetMigratedApplicationNames()
	{
		var set = new HashSet<string>(_knownMigratedPluginAppNames, StringComparer.OrdinalIgnoreCase);

		foreach (var descriptor in PluginLoader.AllPlugins)
		{
			set.Add(descriptor.FolderName);

			if (descriptor.Instance is not null)
			{
				if (!string.IsNullOrWhiteSpace(descriptor.Instance.Name))
				{
					set.Add(descriptor.Instance.Name);
				}
				if (!string.IsNullOrWhiteSpace(descriptor.Instance.LegacyApplicationName))
				{
					set.Add(descriptor.Instance.LegacyApplicationName);
				}
			}
		}

		return set;
	}


	private void RunPluginSchemaScripts(
		PluginDescriptor descriptor,
		IPlugin plugin,
		string dbPlatform,
		Action<SetupStatus, string>? writeOutput)
	{
		var setupRoot = Path.Combine(descriptor.PhysicalPath, "Setup");

		if (!Directory.Exists(setupRoot))
		{
			return;
		}

		var platformFolder = dbPlatform.ToLowerInvariant();
		var installFolder = Path.Combine(setupRoot, "SchemaInstallScripts", platformFolder);
		var upgradeFolder = Path.Combine(setupRoot, "SchemaUpgradeScripts", platformFolder);

		var hasInstallScripts = Directory.Exists(installFolder) && Directory.GetFiles(installFolder, "*.config").Length > 0;
		var hasUpgradeScripts = Directory.Exists(upgradeFolder) && Directory.GetFiles(upgradeFolder, "*.config").Length > 0;

		if (!hasInstallScripts && !hasUpgradeScripts)
		{
			return;
		}

		var (effectiveAppId, currentVersion) = ResolveApplicationIdAndVersion(plugin);
		var targetVersion = plugin.Version;
		var overrideConnectionString = GetOverrideConnectionString(plugin);

		// 1. Initial Schema Install (if not yet installed)
		if (currentVersion <= _zeroVersion && hasInstallScripts)
		{
			var installDirInfo = new DirectoryInfo(installFolder);
			var scriptFiles = installDirInfo.GetFiles("*.config");

			Array.Sort(scriptFiles, UIHelper.CompareFileNames);

			var scriptFile = scriptFiles[scriptFiles.Length - 1];
			var scriptVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

			if (scriptVersion is not null && scriptVersion > currentVersion && (targetVersion is null || scriptVersion <= targetVersion))
			{
				var message = string.Format(SetupResource.InstallingFeature, plugin.Name, scriptFile.Name.Replace(".config", string.Empty));

				writeOutput?.Invoke(SetupStatus.Info, message);

				var errorMessage = DatabaseHelper.RunScript(effectiveAppId, scriptFile, overrideConnectionString);

				if (!string.IsNullOrWhiteSpace(errorMessage))
				{
					writeOutput?.Invoke(SetupStatus.Error, errorMessage);

					return;
				}

				DatabaseHelper.UpdateSchemaVersion(
					effectiveAppId,
					plugin.Name,
					scriptVersion.Major,
					scriptVersion.Minor,
					scriptVersion.Build,
					scriptVersion.Revision);

				DatabaseHelper.AddSchemaScriptHistory(
					effectiveAppId,
					scriptFile.Name,
					DateTime.UtcNow,
					false,
					string.Empty,
					string.Empty);

				currentVersion = scriptVersion;
			}
		}

		// 2. Schema Upgrade Scripts (when currentVersion < plugin.Version)
		if (hasUpgradeScripts && (targetVersion is null || currentVersion < targetVersion))
		{
			var upgradeDirInfo = new DirectoryInfo(upgradeFolder);
			var scriptFiles = upgradeDirInfo.GetFiles("*.config");

			Array.Sort(scriptFiles, UIHelper.CompareFileNames);

			var upgradeHeaderWritten = false;

			foreach (var scriptFile in scriptFiles)
			{
				var scriptVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

				if (scriptVersion is not null &&
					scriptVersion > currentVersion &&
					(targetVersion is null || scriptVersion <= targetVersion))
				{
					if (!upgradeHeaderWritten)
					{
						writeOutput?.Invoke(SetupStatus.Info, string.Format(SetupResource.UpgradingFeature, plugin.Name));
						upgradeHeaderWritten = true;
					}

					writeOutput?.Invoke(SetupStatus.SqlInfo, scriptFile.Name.Replace(".config", string.Empty));

					var errorMessage = DatabaseHelper.RunScript(effectiveAppId, scriptFile, overrideConnectionString);

					if (!string.IsNullOrWhiteSpace(errorMessage))
					{
						writeOutput?.Invoke(SetupStatus.Error, errorMessage);
						return;
					}

					DatabaseHelper.UpdateSchemaVersion(
						effectiveAppId,
						plugin.Name,
						scriptVersion.Major,
						scriptVersion.Minor,
						scriptVersion.Build,
						scriptVersion.Revision);

					DatabaseHelper.AddSchemaScriptHistory(
						effectiveAppId,
						scriptFile.Name,
						DateTime.UtcNow,
						false,
						string.Empty,
						string.Empty);

					currentVersion = scriptVersion;
				}
			}
		}

		// 3. If plugin Version is higher than the highest SQL script version (e.g. code-only release bump),
		// update mp_SchemaVersion so the version tracks IPlugin.Version accurately.
		if (targetVersion is not null && targetVersion > currentVersion && currentVersion > _zeroVersion)
		{
			DatabaseHelper.UpdateSchemaVersion(
				effectiveAppId,
				plugin.Name,
				targetVersion.Major,
				targetVersion.Minor,
				targetVersion.Build,
				targetVersion.Revision);
		}
	}


	private static (Guid appId, Version currentVersion) ResolveApplicationIdAndVersion(IPlugin plugin)
	{
		if (plugin.Id != Guid.Empty)
		{
			var ver = DatabaseHelper.GetSchemaVersion(plugin.Id);

			if (ver > _zeroVersion)
			{
				return (plugin.Id, ver);
			}
		}

		// Check if existing site already recorded schema version under LegacyApplicationName or Name
		if (!string.IsNullOrWhiteSpace(plugin.LegacyApplicationName))
		{
			var legacyId = DatabaseHelper.GetApplicationId(plugin.LegacyApplicationName);
			var legacyVer = DatabaseHelper.GetSchemaVersion(legacyId);

			if (legacyVer > _zeroVersion)
			{
				return (legacyId, legacyVer);
			}
		}

		if (!string.IsNullOrWhiteSpace(plugin.Name) &&
			!string.Equals(plugin.Name, plugin.LegacyApplicationName, StringComparison.OrdinalIgnoreCase))
		{
			var nameId = DatabaseHelper.GetApplicationId(plugin.Name);
			var nameVer = DatabaseHelper.GetSchemaVersion(nameId);

			if (nameVer > _zeroVersion)
			{
				return (nameId, nameVer);
			}
		}

		var finalId = plugin.Id != Guid.Empty ? plugin.Id : DatabaseHelper.GetApplicationId(plugin.Name);

		return (finalId, _zeroVersion);
	}


	private static string? GetOverrideConnectionString(IPlugin plugin)
	{
		if (!string.IsNullOrWhiteSpace(plugin.Name))
		{
			var cs = ConfigHelper.GetStringProperty($"{plugin.Name}_ConnectionString", string.Empty);

			if (!string.IsNullOrWhiteSpace(cs))
			{
				return cs;
			}
		}

		if (!string.IsNullOrWhiteSpace(plugin.LegacyApplicationName))
		{
			var cs = ConfigHelper.GetStringProperty($"{plugin.LegacyApplicationName}_ConnectionString", string.Empty);

			if (!string.IsNullOrWhiteSpace(cs))
			{
				return cs;
			}
		}

		return null;
	}


	private void ConfigurePluginFeatures(
		IPlugin plugin,
		string dbPlatform,
		Action<SetupStatus, string>? writeOutput)
	{
		var features = plugin.GetFeatures();

		if (features is null)
		{
			return;
		}

		foreach (var feature in features)
		{
			if (feature is null || feature.FeatureGuid == Guid.Empty)
			{
				continue;
			}

			if (!string.IsNullOrWhiteSpace(feature.SupportedDatabases) &&
				!feature.SupportedDatabases.ToLowerInvariant().Contains(dbPlatform.ToLowerInvariant()))
			{
				continue;
			}

			if (!PluginFileResolver.FileExists(feature.ControlSource))
			{
				_log.Error($"Could not install feature {feature.FeatureNameReasourceKey} from plugin {plugin.Name}. ControlSource not found: {feature.ControlSource}");
				continue;
			}

			writeOutput?.Invoke(
				SetupStatus.Success,
				string.Format(
					SetupResource.ConfigureFeatureMessage,
					ResourceHelper.GetResourceString(feature.ResourceFile, feature.FeatureNameReasourceKey)
				)
			);

			var moduleDefinition = new ModuleDefinition(feature.FeatureGuid)
			{
				ControlSrc = feature.ControlSource,
				DefaultCacheTime = feature.DefaultCacheTime,
				FeatureName = feature.FeatureNameReasourceKey,
				Icon = feature.Icon,
				IsAdmin = feature.ExcludeFromFeatureList,
				SortOrder = feature.SortOrder,
				ResourceFile = feature.ResourceFile,
				IsCacheable = feature.IsCacheable,
				IsSearchable = feature.IsSearchable,
				SearchListName = feature.SearchListNameResourceKey,
				SupportsPageReuse = feature.SupportsPageReuse,
				DeleteProvider = feature.DeleteProvider,
				PartialView = feature.PartialView,
				SkinFileName = feature.SkinFileName
			};

			moduleDefinition.Save();

			foreach (var featureSetting in feature.Settings)
			{
				ModuleDefinition.UpdateModuleDefinitionSetting(
					moduleDefinition.FeatureGuid,
					moduleDefinition.ModuleDefId,
					featureSetting.ResourceFile,
					featureSetting.GroupNameKey,
					featureSetting.ResourceKey,
					featureSetting.DefaultValue,
					featureSetting.ControlType,
					featureSetting.RegexValidationExpression,
					featureSetting.ControlSrc,
					featureSetting.HelpKey,
					featureSetting.SortOrder,
					featureSetting.Attributes,
					featureSetting.Options,
					featureSetting.Roles,
					featureSetting.ShowToUnauthorized);
			}
		}
	}
}
