#nullable enable
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;

namespace mojoPortal.Web.Framework;

public static class ResourceHelper
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(ResourceHelper));
	private static readonly ConcurrentDictionary<string, ResourceManager> _pluginResourceManagers = new(StringComparer.OrdinalIgnoreCase);
	private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _pluginResxCache = new(StringComparer.OrdinalIgnoreCase);
	private static readonly List<string> _pluginDirectories = [];
	private static readonly object _lock = new();


	public static void RegisterPluginResourceManager(string resourceFile, ResourceManager resourceManager)
	{
		if (string.IsNullOrWhiteSpace(resourceFile) || resourceManager is null)
		{
			return;
		}

		_pluginResourceManagers[resourceFile] = resourceManager;
	}


	public static void RegisterPluginDirectory(string pluginPhysicalPath)
	{
		if (string.IsNullOrWhiteSpace(pluginPhysicalPath) || !Directory.Exists(pluginPhysicalPath))
		{
			return;
		}

		lock (_lock)
		{
			if (!_pluginDirectories.Contains(pluginPhysicalPath))
			{
				_pluginDirectories.Add(pluginPhysicalPath);
			}
		}
	}


	public static string? GetPluginResourceString(string resourceFile, string resourceKey, CultureInfo? culture = null)
	{
		if (string.IsNullOrWhiteSpace(resourceFile) || string.IsNullOrWhiteSpace(resourceKey))
		{
			return null;
		}

		culture ??= CultureInfo.CurrentUICulture;

		if (_pluginResourceManagers.TryGetValue(resourceFile, out var rm))
		{
			try
			{
				var val = rm.GetString(resourceKey, culture);

				if (val is not null)
				{
					return val;
				}
			}
			catch { }
		}

		// Fallback to reading .resx files from ~/Plugins/<PluginName>/App_GlobalResources/ in memory (read-only safe)
		string[] candidateNames;

		if (culture is not null && !string.IsNullOrEmpty(culture.Name))
		{
			candidateNames = [
				$"{resourceFile}.{culture.Name}.resx",
				$"{resourceFile}.{culture.TwoLetterISOLanguageName}.resx",
				$"{resourceFile}.resx"
			];
		}
		else
		{
			candidateNames = [$"{resourceFile}.resx"];
		}

		List<string> dirs;

		lock (_lock)
		{
			dirs = [.. _pluginDirectories];
		}

		foreach (var pluginDir in dirs)
		{
			var resxDir = Path.Combine(pluginDir, "App_GlobalResources");

			if (!Directory.Exists(resxDir))
			{
				continue;
			}

			foreach (var candidate in candidateNames)
			{
				var resxPath = Path.Combine(resxDir, candidate);

				if (!File.Exists(resxPath))
				{
					continue;
				}

				var dict = _pluginResxCache.GetOrAdd(resxPath, LoadResxDictionary);

				if (dict is not null && dict.TryGetValue(resourceKey, out var value))
				{
					return value;
				}
			}
		}

		return null;
	}


	private static Dictionary<string, string> LoadResxDictionary(string resxPath)
	{
		var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		try
		{
			var doc = XDocument.Load(resxPath);

			if (doc.Root is not null)
			{
				foreach (var dataNode in doc.Root.Elements("data"))
				{
					var nameAttr = dataNode.Attribute("name")?.Value;
					var valElem = dataNode.Element("value")?.Value;

					if (!string.IsNullOrEmpty(nameAttr) && valElem is not null)
					{
						result[nameAttr!] = valElem;
					}
				}
			}
		}
		catch (Exception ex)
		{
			_log.Warn($"Failed to read plugin resource file {resxPath}", ex);
		}

		return result;
	}


	#region Help System

	/// <summary>
	/// when posssible you should use SiteUtils.GetDefaultUICulture() in preference to this method
	/// </summary>
	/// <returns></returns>
	public static CultureInfo GetDefaultUICulture()
	{
		try
		{
			// reading config sections not allowed in Medium trust so catch SecurityException
			var globalizationSection = ConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;

			if (globalizationSection is not null)
			{
				if (globalizationSection.Culture.Contains(":"))
				{
					// this is tryinbg to get UiCulture and expects this format
					// <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
					var cultureString = globalizationSection.Culture.Substring(
						globalizationSection.Culture.LastIndexOf(":") + 1,
						globalizationSection.Culture.Length - (globalizationSection.Culture.LastIndexOf(":") + 1));

					return new CultureInfo(cultureString);
				}
				else
				{
					return new CultureInfo(globalizationSection.Culture);
				}
			}
		}
		catch (ArgumentException ex)
		{
			_log.Info("ResourceHelper swallowed exception", ex);
		}
		catch (System.Security.SecurityException ex)
		{
			_log.Info("ResourceHelper swallowed exception", ex);
		}

		return new CultureInfo("en-US"); //this is the default culture in Web.config
	}


	/// <summary>
	/// when posssible you should use SiteUtils.GetDefaultCulture() in preference to this method
	/// </summary>
	/// <returns></returns>
	public static CultureInfo GetDefaultCulture()
	{
		try
		{
			// reading config sections not allowed in Medium trust so catch SecurityException
			var globalizationSection = ConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;

			if (globalizationSection is not null)
			{
				if (globalizationSection.Culture.Contains(":"))
				{
					// this is trying to get culture not uiculture and expects this format 
					// <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
					var cultureString = globalizationSection.Culture.Substring(
						globalizationSection.Culture.IndexOf(":") + 1,
						(globalizationSection.Culture.Length - (globalizationSection.Culture.IndexOf(":") + 1)));

					return new CultureInfo(cultureString);
				}
				else
				{
					return new CultureInfo(globalizationSection.Culture);
				}
			}
		}
		catch (ArgumentException ex)
		{
			_log.Info("ResourceHelper swallowed exception", ex);
		}
		catch (System.Security.SecurityException ex)
		{
			_log.Info("ResourceHelper swallowed exception", ex);
		}

		return new CultureInfo("en-US"); //this is the default culture in Web.config
	}


	/// <summary>
	/// Gets a CultureInfo object suitable for parsing or formatting a specific currency
	/// </summary>
	/// <param name="currencyISOCode"></param>
	/// <returns></returns>
	public static CultureInfo GetCurrencyCulture(string currencyISOCode)
	{
		if (string.IsNullOrEmpty(currencyISOCode))
		{
			return GetDefaultCulture();
		}

		//this helper caches the lookup list
		var currencyCulture = CurrencyHelper.CultureInfoFromCurrencyISO(currencyISOCode);

		if (currencyCulture is not null)
		{
			return currencyCulture;
		}

		// this is based on web.config globalization section, en-US
		return GetDefaultCulture();

	}


	public static string GetHelpFileText(string helpKey)
	{
		if (string.IsNullOrEmpty(helpKey))
		{
			return string.Empty;
		}

		var fileExtension = ".config";
		var helpFolder = GetHelpFolder();
		var helpFile = GetFullResourceFilePath(CultureInfo.CurrentUICulture, helpFolder, helpKey + fileExtension);
		var message = string.Empty;

		if (File.Exists(helpFile))
		{
			var file = new FileInfo(helpFile);
			var sr = file.OpenText();

			message = sr.ReadToEnd();

			sr.Close();

			return message;
		}

		// Fallback to plugin Data/HelpFiles folders
		List<string> dirs;

		lock (_lock)
		{
			dirs = [.. _pluginDirectories];
		}

		foreach (var pluginDir in dirs)
		{
			var pluginHelpFolder = Path.Combine(pluginDir, "Data", "HelpFiles") + Path.DirectorySeparatorChar;

			if (!Directory.Exists(pluginHelpFolder))
			{
				continue;
			}

			var pluginHelpFile = GetFullResourceFilePath(CultureInfo.CurrentUICulture, pluginHelpFolder, helpKey + fileExtension);

			if (File.Exists(pluginHelpFile))
			{
				return File.ReadAllText(pluginHelpFile);
			}
		}

		return message;
	}


	public static void SetHelpFileText(string helpKey, string helpText)
	{
		if (string.IsNullOrEmpty(helpKey) || string.IsNullOrEmpty(helpText))
		{
			return;
		}

		var culture = CultureInfo.CurrentUICulture.Name;
		var helpFolder = GetHelpFolder();
		var helpFile = $"{helpFolder}{culture}-{helpKey}.config";

		if (!IsPathSafe(helpFolder, helpFile))
		{
			_log.Error("helpKey attempted directory traversal.");

			throw new Exception("helpKey attempted directory traversal.");
		}

		var streamWriter = File.CreateText(helpFile);

		streamWriter.Write(helpText);
		streamWriter.Close();
	}


	public static string GetHelpFolder()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/HelpFiles") + Path.DirectorySeparatorChar;
	}


	public static string GetFullResourceFilePath(CultureInfo cultureInfo, string folder, string filename)
	{
		var path = GetResourceFilePath(cultureInfo, folder, filename);

		if (!string.IsNullOrWhiteSpace(path))
		{
			return path;
		}

		path = $"{folder}en-US-{filename}";

		if (!IsPathSafe(folder, path))
		{
			_log.Error("Filename attempted directory traversal.");

			throw new Exception("Filename attempted directory traversal.");
		}

		return path;
	}


	public static string GetResourceFilePath(CultureInfo cultureInfo, string folder, string filename)
	{
		if (cultureInfo is null || cultureInfo.LCID.Equals(CultureInfo.InvariantCulture.LCID))
		{
			return string.Empty;
		}

		var path = $"{folder}{cultureInfo.Name}-{filename}";

		if (!IsPathSafe(folder, path))
		{
			_log.Error("Filename attempted directory traversal.");

			throw new Exception("Filename attempted directory traversal.");
		}

		if (File.Exists(path))
		{
			return path;
		}

		// Fallback to the default culture
		path = $"{folder}en-US-{filename}";

		if (!IsPathSafe(folder, path))
		{
			_log.Error("Filename attempted directory traversal.");

			throw new Exception("Filename attempted directory traversal.");
		}

		return path;
	}

	#endregion


	#region Message Templates

	public static string GetMessageTemplate(string templateFile) => GetMessageTemplate(CultureInfo.CurrentUICulture, templateFile);


	public static string GetMessageTemplate(CultureInfo cultureInfo, string templateFile)
	{
		if (string.IsNullOrEmpty(templateFile))
		{
			return string.Empty;
		}

		var templateFolder = GetMessageTemplateFolder();
		var messageFile = GetFullResourceFilePath(cultureInfo, templateFolder, templateFile);

		if (File.Exists(messageFile))
		{
			var message = File.ReadAllText(messageFile);

			return message;
		}
		else
		{
			List<string> dirs;

			lock (_lock)
			{
				dirs = [.. _pluginDirectories];
			}

			foreach (var pluginDir in dirs)
			{
				var pluginTemplateFolder = Path.Combine(pluginDir, "Data", "MessageTemplates") + Path.DirectorySeparatorChar;

				if (!Directory.Exists(pluginTemplateFolder))
				{
					continue;
				}

				var pluginMessageFile = GetFullResourceFilePath(cultureInfo, pluginTemplateFolder, templateFile);

				if (File.Exists(pluginMessageFile))
				{
					return File.ReadAllText(pluginMessageFile);
				}
			}

			return string.Empty;
		}
	}


	public static string GetMessageTemplateFolder()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/MessageTemplates") + Path.DirectorySeparatorChar;
	}

	#endregion


	public static string GetResourceString(string resourceFile, string resourceKey) =>
		GetResourceString(resourceFile, resourceKey, false);


	public static string GetResourceString(
		string resourceFile,
		string resourceKey,
		bool useConfigOverrides) => GetResourceString(
			resourceFile,
			resourceKey,
			null,
			useConfigOverrides);


	public static string GetResourceString(
		string resourceFile,
		string resourceKey,
		CultureInfo? culture,
		bool useConfigOverrides)
	{
		if (HttpContext.Current is null)
		{
			return resourceKey;
		}

		if (resourceFile.Length == 0)
		{
			resourceFile = "Resource";
		}

		if (ConfigurationManager.AppSettings[resourceKey] is not null)
		{
			return ConfigurationManager.AppSettings[resourceKey];
		}

		try
		{
			object resource;

			if (culture is not null)
			{
				resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey, culture);
			}
			else
			{
				resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey);
			}

			if (resource is not null)
			{
				return resource.ToString();
			}
		}
		catch { }

		var pluginResource = GetPluginResourceString(resourceFile, resourceKey, culture);

		if (pluginResource is not null)
		{
			return pluginResource;
		}

		return resourceKey;
	}


	public static string FormatCategoryLinkText(string category, int postCount)
	{
		if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
		{
			return $"({postCount}) {category}";
		}

		return $"{category} ({postCount})";
	}


	private static bool IsPathSafe(string basePath, string path)
	{
		var nomalizedBase = basePath.EndsWith(Path.DirectorySeparatorChar.ToString()) ?
			basePath :
			basePath + Path.DirectorySeparatorChar;
		var fullPath = Path.GetFullPath(path);

		if (!fullPath.StartsWith(nomalizedBase, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		return true;
	}
}
