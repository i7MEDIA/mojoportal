using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mojoPortal.Core.Configuration
{
	public static class AppConfig
	{
		/// <summary>
		/// this can be used to detect a secure request in a proxied environment when the mere presence of a specific server variable indicates a secure connection
		/// for example this can be used with IIS AAR (Application Request Routing Module) where the presence of a server variable named HTTP_X_ARR_SSL indicates a secure request
		/// So you would add this to user.config  <add key="SecureConnectionServerVariableForPresenceCheck" value="HTTP_X_ARR_SSL"/>
		/// This setting is checked in WebHelper.IsSecureRequest();
		/// </summary>
		public static string SecureConnectionServerVariableForPresenceCheck
		{
			get
			{
				var prop = ConfigHelper.GetStringProperty("SecureConnectionServerVariableForPresenceCheck", string.Empty);
				if (!string.IsNullOrWhiteSpace(prop))
				{
					return prop;
				}
				return string.Empty;
			}
		}

		/// <summary>
		/// use this if you need to check a custom server variable for a specific value to determine a secure request
		/// you must also set the value for SecureConnectionServerVariableSecureValue that corresponds to a secure request
		/// </summary>
		public static string SecureConnectionServerVariableForValueCheck
		{
			get
			{
				var prop = ConfigHelper.GetStringProperty("SecureConnectionServerVariableForValueCheck", string.Empty);
				if (!string.IsNullOrWhiteSpace(prop))
				{
					return prop;
				}
				return string.Empty;
			}
		}

		public static string SecureConnectionServerVariableSecureValue
		{
			get
			{
				var prop = ConfigHelper.GetStringProperty("SecureConnectionServerVariableSecureValue", string.Empty);
				if (!string.IsNullOrWhiteSpace(prop))
				{
					return prop;
				}
				return string.Empty;
			}
		}

		public static string StaticFileExtensions
		{
			get
			{
				var defaultVal = ".asf|.asx|.avi|.css|.csv|.doc|.docx|.gif|.htm|.html|.ico|.jpeg|.jpg|.js|.json|.less|.m4a|.m4v|.mov|.mp3|.mp4|.mpeg|.mpg|.oga|.ogg|.ogv|.pdf|.png|.pps|.ppt|.pptx|.svg|.tif|.ttf|.txt|.wav|.webm|.webma|.webmv|.webp|.wmv|.woff|.xls|.xlsx|.xml|.zip";
				return ConfigHelper.GetStringProperty("StaticFileExtensions", defaultVal);
			}
		}

		public static string EditorTemplatesOrder
		{
			get
			{
				var defaultVal = "site,skin,system";
				return ConfigHelper.GetStringProperty("EditorTemplatesOrder", defaultVal);
			}
		}

		public static string JQueryVersion
		{
			get
			{
				return ConfigHelper.GetStringProperty("JQueryVersion", "3.6.0");
			}
		}

		public static string JQueryUIVersion
		{
			get
			{
				return ConfigHelper.GetStringProperty("JQueryUIVersion", "~/Scripts/");
			}
		}

		public static string JQueryBasePath
		{
			get
			{
				return ConfigHelper.GetStringProperty("JQueryPath", "~/Scripts/");
			}
		}

		public static string GoogleAnalyticsInitScript
		{
			get
			{
				return ConfigHelper.GetStringProperty("GoogleAnalyticsInitScript", "~/ClientScript/GA4-gtag.js");
			}
		}

		public static string GoogleAnalyticsScript
		{
			get
			{
				return ConfigHelper.GetStringProperty("GoogleAnalyticsScript", "https://www.googletagmanager.com/gtag/js?id=");
			}
		}

		public static bool EnableUploads
		{
			get
			{
				return ConfigHelper.GetBoolProperty("EnableUploads", true);
			}
		}
	}
}
