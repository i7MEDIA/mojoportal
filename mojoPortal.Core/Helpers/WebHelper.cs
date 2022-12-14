using System.Web;
using mojoPortal.Core.Configuration;
namespace mojoPortal.Core.Helpers
{
	public static class WebHelper
	{
		/// <summary>
		/// encapsulates checks for a secure connection with configurable server variable checks
		/// </summary>
		public static bool IsSecureRequest()
		{
			if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
			{
				// default this works when the SSL certificate is installed in the site but not when using load balancers or other proxy server
				if (HttpContext.Current.Request.IsSecureConnection) { return true; }

				if (AppConfig.SecureConnectionServerVariableForPresenceCheck.Length > 0)
				{
					if (HttpContext.Current.Request.ServerVariables[AppConfig.SecureConnectionServerVariableForPresenceCheck] != null) { return true; }
				}

				if (AppConfig.SecureConnectionServerVariableForValueCheck.Length > 0 && AppConfig.SecureConnectionServerVariableSecureValue.Length > 0)
				{
					if (HttpContext.Current.Request.ServerVariables[AppConfig.SecureConnectionServerVariableForValueCheck] != null)
					{
						if (HttpContext.Current.Request.ServerVariables[AppConfig.SecureConnectionServerVariableForValueCheck] == AppConfig.SecureConnectionServerVariableSecureValue) { return true; }
					}
				}
			}
			return false;
		}
	}
}
