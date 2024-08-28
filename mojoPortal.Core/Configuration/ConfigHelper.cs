using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace mojoPortal.Core.Configuration;

public static class ConfigHelper
{
	public static bool GetBoolProperty(string key, bool defaultValue) => GetBoolSettingFromContext(key, defaultValue);

	public static bool GetBoolProperty(string key, bool defaultValue, bool bypassContext)
	{
		if (bypassContext)
		{
			return GetBoolPropertyFromConfig(key, defaultValue);
		}

		return GetBoolSettingFromContext(key, defaultValue);
	}

	private static bool GetBoolSettingFromContext(string key, bool defaultValue)
	{
		if (HttpContext.Current is null)
		{
			return GetBoolPropertyFromConfig(key, defaultValue);
		}

		bool setting;

		if (HttpContext.Current.Items[key] is null)
		{
			setting = GetBoolPropertyFromConfig(key, defaultValue);

			HttpContext.Current.Items[key] = setting;
		}
		else
		{
			setting = (bool)HttpContext.Current.Items[key];
		}

		return setting;
	}

	private static bool GetBoolPropertyFromConfig(string key, bool defaultValue)
	{
		if (ConfigurationManager.AppSettings[key] == null)
		{
			return defaultValue;
		}

		if (string.Equals(ConfigurationManager.AppSettings[key], "true", StringComparison.InvariantCultureIgnoreCase))
		{
			return true;
		}

		if (string.Equals(ConfigurationManager.AppSettings[key], "false", StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}

		return defaultValue;
	}

	public static string GetStringProperty(string key, string defaultValue) => GetStringSettingFromContext(key, defaultValue);

	public static string GetStringProperty(string key, string defaultValue, bool bypassContext)
	{
		if (bypassContext)
		{
			return GetStringPropertyFromConfig(key, defaultValue);
		}

		return GetStringSettingFromContext(key, defaultValue);
	}

	private static string GetStringPropertyFromConfig(string key, string defaultValue) => ConfigurationManager.AppSettings[key] ?? defaultValue;

	private static string GetStringSettingFromContext(string key, string defaultValue)
	{
		if (HttpContext.Current == null)
		{
			return GetStringPropertyFromConfig(key, defaultValue);
		}

		string setting;

		if (HttpContext.Current.Items[key] == null)
		{
			setting = GetStringPropertyFromConfig(key, defaultValue);

			HttpContext.Current.Items[key] = setting;
		}
		else
		{
			setting = HttpContext.Current.Items[key].ToString();
		}

		return setting;
	}

	public static int GetIntProperty(string key, int defaultValue)
	{
		return int.TryParse(ConfigurationManager.AppSettings[key], out int setting) ? setting : defaultValue;
	}

	public static long GetLongProperty(string key, long defaultValue)
	{
		return long.TryParse(ConfigurationManager.AppSettings[key], out long setting) ? setting : defaultValue;
	}

	public static Unit GetUnitProperty(string key, Unit defaultValue)
	{
		if (ConfigurationManager.AppSettings[key] != null)
		{
			return Unit.Parse(ConfigurationManager.AppSettings[key], CultureInfo.InvariantCulture);
		}

		return defaultValue;
	}

	public static bool GetSiteConfigSetting(int siteId, string key, bool webConfigSetting)
	{
		if (siteId < 1)
		{
			return webConfigSetting;
		}

		return GetBoolProperty(Invariant($"Site{siteId}-{key}"), webConfigSetting);
	}

	public static string GetSiteConfigSetting(int siteId, string key, string webConfigSetting)
	{
		if (siteId < 1)
		{
			return webConfigSetting;
		}

		return GetStringProperty(Invariant($"Site{siteId}-{key}"), webConfigSetting);
	}

	public static int GetSiteConfigSetting(int siteId, string key, int webConfigSetting)
	{
		if (siteId < 1)
		{
			return webConfigSetting;
		}

		return GetIntProperty(Invariant($"Site{siteId}-{key}"), webConfigSetting);
	}

	public static long GetSiteConfigSetting(int siteId, string key, long webConfigSetting)
	{
		if (siteId < 1)
		{
			return webConfigSetting;
		}

		return GetLongProperty(Invariant($"Site{siteId}-{key}"), webConfigSetting);
	}

	public static Unit GetSiteConfigSetting(int siteId, string key, Unit webConfigSetting)
	{
		if (siteId < 1)
		{
			return webConfigSetting;
		}

		return GetUnitProperty(Invariant($"Site{siteId}-{key}"), webConfigSetting);
	}

	public static MachineKeySection GetMachineKeySection()
	{
		return (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
	}
}