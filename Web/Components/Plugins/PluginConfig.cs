#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace mojoPortal.Web.Plugins;

public class PluginOverrideSetting
{
	public string Name { get; set; } = string.Empty;
	public int? Order { get; set; }
	public bool? Enabled { get; set; }
}


public class PluginConfig
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(PluginConfig));

	public Dictionary<string, PluginOverrideSetting> Overrides { get; } = [with(StringComparer.OrdinalIgnoreCase)];


	public static PluginConfig Load(string pluginsRootPhysicalPath)
	{
		var config = new PluginConfig();

		if (string.IsNullOrWhiteSpace(pluginsRootPhysicalPath) || !Directory.Exists(pluginsRootPhysicalPath))
		{
			return config;
		}

		// 1. Check per-plugin ~/Plugins/<FolderName>/plugin.config
		foreach (var pluginDir in Directory.GetDirectories(pluginsRootPhysicalPath))
		{
			var folderName = Path.GetFileName(pluginDir);
			var perPluginConfigFile = Path.Combine(pluginDir, "plugin.config");

			if (File.Exists(perPluginConfigFile))
			{
				LoadFromFile(config, perPluginConfigFile, folderName);
			}
		}

		// 2. Check global ~/Plugins/plugins.config (takes precedence over per-plugin plugin.config)
		var globalConfigFile = Path.Combine(pluginsRootPhysicalPath, "plugins.config");

		if (File.Exists(globalConfigFile))
		{
			LoadFromFile(config, globalConfigFile, null);
		}

		return config;
	}


	private static void LoadFromFile(PluginConfig config, string filePath, string? defaultPluginName)
	{
		try
		{
			var doc = XDocument.Load(filePath);

			if (doc.Root is null)
			{
				return;
			}

			if (doc.Root.Name.LocalName.Equals("plugin", StringComparison.OrdinalIgnoreCase))
			{
				ParsePluginElement(config, doc.Root, defaultPluginName);

				return;
			}

			foreach (var elem in doc.Root.Elements())
			{
				if (elem.Name.LocalName.Equals("add", StringComparison.OrdinalIgnoreCase) ||
					elem.Name.LocalName.Equals("plugin", StringComparison.OrdinalIgnoreCase))
				{
					ParsePluginElement(config, elem, defaultPluginName);
				}
			}
		}
		catch (Exception ex)
		{
			_log.Warn($"Failed to parse plugin configuration file {filePath}", ex);
		}
	}


	private static void ParsePluginElement(PluginConfig config, XElement elem, string? defaultPluginName)
	{
		var name = elem.Attribute("name")?.Value ?? elem.Attribute("Name")?.Value ?? defaultPluginName ?? string.Empty;

		if (string.IsNullOrWhiteSpace(name))
		{
			return;
		}

		if (!config.Overrides.TryGetValue(name, out var setting))
		{
			setting = new PluginOverrideSetting { Name = name };
			config.Overrides[name] = setting;
		}

		var orderAttr = elem.Attribute("order")?.Value ?? elem.Attribute("Order")?.Value;

		if (!string.IsNullOrWhiteSpace(orderAttr) && int.TryParse(orderAttr, out int orderVal))
		{
			setting.Order = orderVal;
		}

		var enabledAttr = elem.Attribute("enabled")?.Value ?? elem.Attribute("Enabled")?.Value;

		if (!string.IsNullOrWhiteSpace(enabledAttr) && bool.TryParse(enabledAttr, out bool enabledVal))
		{
			setting.Enabled = enabledVal;
		}
	}
}
