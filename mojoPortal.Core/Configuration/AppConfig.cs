using System;
using System.Collections.Generic;
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
	}
}
