using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Core.Configuration
{
	public static class ConfigHelper
	{
		public static bool GetBoolProperty(string key, bool defaultValue)
		{
			return GetBoolSettingFromContext(key, defaultValue);
		}

		public static bool GetBoolProperty(string key, bool defaultValue, bool byPassContext)
		{
			if (byPassContext) return GetBoolPropertyFromConfig(key, defaultValue);
			return GetBoolSettingFromContext(key, defaultValue);
		}

		private static bool GetBoolSettingFromContext(string key, bool defaultValue)
		{
			if (HttpContext.Current == null)
			{
				return GetBoolPropertyFromConfig(key, defaultValue);
			}

			bool setting;

			if (HttpContext.Current.Items[key] == null)
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
			if (ConfigurationManager.AppSettings[key] == null) return defaultValue;
			if (string.Equals(ConfigurationManager.AppSettings[key], "true", StringComparison.InvariantCultureIgnoreCase)) return true;
			if (string.Equals(ConfigurationManager.AppSettings[key], "false", StringComparison.InvariantCultureIgnoreCase)) return false;

			return defaultValue;
		}

		public static string GetStringProperty(string key, string defaultValue)
		{
			return GetStringSettingFromContext(key, defaultValue);
		}

		private static string GetStringPropertyFromConfig(string key, string defaultValue)
		{
			if (ConfigurationManager.AppSettings[key] == null) return defaultValue;
			return ConfigurationManager.AppSettings[key];
		}

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
	}
}
