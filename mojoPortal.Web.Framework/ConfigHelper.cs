using System;
using System.Web.UI.WebControls;

using Config = mojoPortal.Core.Configuration;


namespace mojoPortal.Web.Framework
{
	[Obsolete("Use mojoPortal.Core.Configuration.ConfigHelper")]
	public static class ConfigHelper
	{
		public static bool GetBoolProperty(string key, bool defaultValue)
		{
			return Config.ConfigHelper.GetBoolProperty(key, defaultValue);
		}

		public static bool GetBoolProperty(string key, bool defaultValue, bool byPassContext)
		{
			return Config.ConfigHelper.GetBoolProperty(key, defaultValue, byPassContext);
		}

		public static string GetStringProperty(string key, string defaultValue)
		{
			return Config.ConfigHelper.GetStringProperty(key, defaultValue);
		}

		public static int GetIntProperty(string key, int defaultValue)
		{
			return Config.ConfigHelper.GetIntProperty(key, defaultValue);
		}

		public static long GetLongProperty(string key, long defaultValue)
		{
			return Config.ConfigHelper.GetLongProperty(key, defaultValue);
		}

		public static Unit GetUnit(string key, Unit defaultValue)
		{
			return Config.ConfigHelper.GetUnitProperty(key, defaultValue);
		}
	}
}
