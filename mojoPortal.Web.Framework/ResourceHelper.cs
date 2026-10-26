using log4net;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace mojoPortal.Web.Framework;

public static class ResourceHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ResourceHelper));

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
			GlobalizationSection globalizationSection = (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
			if (globalizationSection != null)
			{
				if (globalizationSection.Culture.Contains(":"))
				{
					string cultureString;
					// this is tryinbg to get UiCulture and expects this format
					// <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
					cultureString = globalizationSection.Culture.Substring(globalizationSection.Culture.LastIndexOf(":") + 1, (globalizationSection.Culture.Length - (globalizationSection.Culture.LastIndexOf(":") + 1)));
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
			log.Info("ResourceHelper swallowed exception", ex);
		}
		catch (System.Security.SecurityException ex)
		{
			log.Info("ResourceHelper swallowed exception", ex);
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
			GlobalizationSection globalizationSection = (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
			if (globalizationSection != null)
			{
				if (globalizationSection.Culture.Contains(":"))
				{
					string cultureString;
					// this is trying to get culture not uiculture and expects this format 
					// <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
					cultureString = globalizationSection.Culture.Substring(globalizationSection.Culture.IndexOf(":") + 1, (globalizationSection.Culture.Length - (globalizationSection.Culture.IndexOf(":") + 1)));
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
			log.Info("ResourceHelper swallowed exception", ex);
		}
		catch (System.Security.SecurityException ex)
		{
			log.Info("ResourceHelper swallowed exception", ex);
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
		if (string.IsNullOrEmpty(currencyISOCode)) { return GetDefaultCulture(); }

		//this helper caches the lookup list
		CultureInfo currencyCulture = CurrencyHelper.CultureInfoFromCurrencyISO(currencyISOCode);
		if (currencyCulture != null) { return currencyCulture; }

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
			log.Error("helpKey attempted directory traversal.");

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
			log.Error("Filename attempted directory traversal.");

			throw new Exception("Filename attempted directory traversal.");
		}

		return path;
	}

	public static string GetResourceFilePath(CultureInfo cultureInfo, string folder, string filename)
	{
		if (cultureInfo == null || cultureInfo.LCID.Equals(CultureInfo.InvariantCulture.LCID))
		{
			return string.Empty;
		}

		var path = $"{folder}{cultureInfo.Name}-{filename}";

		if (!IsPathSafe(folder, path))
		{
			log.Error("Filename attempted directory traversal.");

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
			log.Error("Filename attempted directory traversal.");

			throw new Exception("Filename attempted directory traversal.");
		}

		return path;
	}

	#endregion

	#region Message Templates

	public static string GetMessageTemplate(string templateFile)
	{
		return GetMessageTemplate(CultureInfo.CurrentUICulture, templateFile);
	}

	public static string GetMessageTemplate(CultureInfo cultureInfo, string templateFile)
	{
		if (string.IsNullOrEmpty(templateFile))
		{
			return string.Empty;
		}

		string templateFolder = GetMessageTemplateFolder();
		string messageFile;

		messageFile = GetFullResourceFilePath(cultureInfo, templateFolder, templateFile);

		if (File.Exists(messageFile))
		{
			string message = File.ReadAllText(messageFile);

			return message;
		}
		else
		{
			return string.Empty;
		}
	}

	public static string GetMessageTemplateFolder()
	{
		if (HttpContext.Current == null)
		{
			return string.Empty;
		}

		return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/Data/MessageTemplates") + Path.DirectorySeparatorChar;
	}


	#endregion

	public static string GetResourceString(
		string resourceFile,
		string resourceKey)
	{
		bool useConfigOverrides = false;

		return GetResourceString(resourceFile, resourceKey, useConfigOverrides);

	}

	public static string GetResourceString(
		string resourceFile,
		string resourceKey,
		bool useConfigOverrides)
	{
		return GetResourceString(
			resourceFile,
			resourceKey,
			null,
			useConfigOverrides);

	}

	public static string GetResourceString(
		string resourceFile,
		string resourceKey,
		CultureInfo culture,
		bool useConfigOverrides)
	{
		if (HttpContext.Current == null) return resourceKey;
		if (resourceFile.Length == 0) resourceFile = "Resource";

		if (ConfigurationManager.AppSettings[resourceKey] != null)
		{
			return ConfigurationManager.AppSettings[resourceKey];
		}

		try
		{
			object resource;
			if (culture != null)
			{
				resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey, culture);
			}
			else
			{
				resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey);
			}

			if (resource != null) return resource.ToString();
		}
		catch { }

		return resourceKey;

	}

	public static string FormatCategoryLinkText(string category, int postCount)
	{
		if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
		{
			return "(" + postCount.ToString() + ") " + category;
		}

		return category + " (" + postCount.ToString() + ")";
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
