using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Framework
{
    public static class ConfigHelper
    {
        public static bool GetBoolProperty(string key, bool defaultValue)
        {
            //if (ConfigurationManager.AppSettings[key] == null) return defaultValue;

            //if (StringHelper.IsCaseInsensitiveMatch(ConfigurationManager.AppSettings[key], "true")) return true;

            //if (StringHelper.IsCaseInsensitiveMatch(ConfigurationManager.AppSettings[key], "false")) return false;

            //return defaultValue;

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

            if (StringHelper.IsCaseInsensitiveMatch(ConfigurationManager.AppSettings[key], "true")) return true;

            if (StringHelper.IsCaseInsensitiveMatch(ConfigurationManager.AppSettings[key], "false")) return false;

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
            int setting;
            return int.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : defaultValue;
        }

        public static long GetLongProperty(string key, long defaultValue)
        {
            long setting;
            return long.TryParse(ConfigurationManager.AppSettings[key], out setting) ? setting : defaultValue;
        }

        public static Unit GetUnit(string key, Unit defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return Unit.Parse(ConfigurationManager.AppSettings[key], CultureInfo.InvariantCulture);
            }

            return defaultValue;
        }

    }
}
