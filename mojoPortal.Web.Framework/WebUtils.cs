using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Core.Configuration;

namespace mojoPortal.Web.Framework;

/// <summary>
/// Utility functions
/// </summary>
public static class WebUtils
{
	private static readonly ILog log = LogManager.GetLogger(typeof(WebUtils));
	private static bool debugLog = log.IsDebugEnabled;

	public static string GetRequestBody(this HttpRequest request)
	{
		using var reader = new StreamReader(request.InputStream);
		string encodedString = reader.ReadToEnd();
		return HttpUtility.UrlDecode(encodedString);
	}

	public static string GetApplicationRoot()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		if (HttpContext.Current.Items["applicationRoot"] is not string applicationRoot)
		{
			applicationRoot = CalculateApplicationRoot();
			if (applicationRoot is not null)
			{
				HttpContext.Current.Items["applicationRoot"] = applicationRoot;
			}
		}
		return applicationRoot;
	}

	public static bool IsRequestForStaticFile(string requestPath)
	{
		if (
			string.IsNullOrWhiteSpace(requestPath.Replace("/", "")) ||
			requestPath.Contains(".aspx", StringComparison.OrdinalIgnoreCase) ||
			requestPath.Contains(".ashx", StringComparison.OrdinalIgnoreCase) ||
			requestPath.Contains(".svc", StringComparison.OrdinalIgnoreCase)
		)
		{
			return false;
		}

		var staticExtensions = AppConfig.StaticFileExtensions;

		var extIdx = requestPath.LastIndexOf('.');


		var result = staticExtensions.SplitOnCharAndTrim('|').Contains(requestPath.Substring(extIdx > -1 ? extIdx : 0, extIdx > -1 ? requestPath.Length - extIdx : requestPath.Length));

		return result;
	}

	private static string CalculateApplicationRoot()
	{
		//if (debugLog) { log.Debug("ApplicationPath is " + HttpContext.Current.Request.ApplicationPath); }

		if (HttpContext.Current.Request.ApplicationPath.Length == 1)
		{
			return string.Empty;
		}
		else
		{
			return HttpContext.Current.Request.ApplicationPath;
		}
	}

	private static string GetHost(string protocol)
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		protocol ??= string.Empty;

		if (HttpContext.Current.Items["host" + protocol] is not string host)
		{
			host = DetermineHost(protocol);
			if (host is not null)
			{
				HttpContext.Current.Items["host" + protocol] = host;
			}
		}
		return host;
	}

	// Returns hostname[:port] to use when constructing the site root URL.
	private static string DetermineHost(string protocol)
	{
		string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
		if (debugLog)
		{
			log.Debug("ServerName= " + serverName);
			log.Debug("serverPort= " + serverPort);
		}

		// Most proxies add an X-Forwarded-Host header which contains the original Host header
		// including any non-default port.

		string forwardedHosts = HttpContext.Current.Request.Headers["X-Forwarded-Host"];
		if (forwardedHosts is not null)
		{
			// If the request passed thru multiple proxies, they will be separated by commas.
			// We only care about the first one.
			string forwardedHost = forwardedHosts.Split(',')[0];

			if (debugLog)
			{
				log.Debug("forwardedHost= " + forwardedHost);
			}

			string[] serverAndPort = forwardedHost.Split(':');
			serverName = serverAndPort[0];

			if (debugLog)
			{
				log.Debug("serverName after split from serverAndPort = " + serverName);

			}

			serverPort = null;
			if (serverAndPort.Length > 1)
			{
				serverPort = serverAndPort[1];
				if (debugLog)
				{
					log.Debug("serverPort after split from serverAndPort = " + serverPort);

				}
			}
		}

		// Only include a port if it is not the default for the protocol and MapAlternatePort = true
		// in the config file.
		if ((protocol == "http" && serverPort == "80")
			|| (protocol == "https" && serverPort == "443"))
		{
			serverPort = null;
		}

		// added to fix issue reported by user running normal on port 80 but ssl on port 472
		if (protocol == "https" && serverPort == "80")
		{
			if (debugLog)
			{
				log.Debug("protocol == https , serverPort == 80");
			}

			bool mapAlternateSSLPort = Core.Configuration.ConfigHelper.GetBoolProperty("MapAlternateSSLPort", false);
			if (mapAlternateSSLPort)
			{
				string alternatSSLPort = ConfigurationManager.AppSettings["AlternateSSLPort"];
				if (alternatSSLPort is not null)
				{
					serverPort = alternatSSLPort;
				}
			}

		}

		string host = serverName;

		if (debugLog)
		{
			log.Debug($"host= {host}");
		}

		if (serverPort is not null)
		{
			bool mapAlternatePort = Core.Configuration.ConfigHelper.GetBoolProperty("MapAlternatePort", false);
			if (mapAlternatePort)
			{
				host += $":{serverPort}";
			}
		}

		return host;
	}

	public static string GetSiteRoot()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		if (HttpContext.Current.Items["siteRoot"] is not string siteRoot)
		{
			siteRoot = DetermineSiteRoot();
			if (siteRoot is not null)
			{
				HttpContext.Current.Items["siteRoot"] = siteRoot;
			}
		}
		return siteRoot;
	}

	public static string GetRelativeSiteRoot()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		if (HttpContext.Current.Items["relativesiteRoot"] is not string siteRoot)
		{
			siteRoot = GetApplicationRoot();
			if (siteRoot is not null)
			{
				HttpContext.Current.Items["relativesiteRoot"] = siteRoot;
			}
		}
		return siteRoot;
	}

	public static string GetHostRoot()
	{
		string protocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
		string host = GetHost(protocol);
		return $"{protocol}://{host}";
	}

	private static string DetermineSiteRoot()
	{
		string protocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
		string host = GetHost(protocol);
		return $"{protocol}://{host}{GetApplicationRoot()}";
	}

	public static string GetSecureSiteRoot()
	{
		string protocol = "https";
		string host = GetHost(protocol);
		return $"{protocol}://{host}{GetApplicationRoot()}";
	}

	public static string GetSecureHostRoot()
	{
		string protocol = "https";
		string host = GetHost(protocol);
		return $"{protocol}://{host}";
	}

	public static string GetInSecureSiteRoot()
	{
		string protocol = "http";
		string host = GetHostName();
		return $"{protocol}://{host}{GetApplicationRoot()}";
	}

	public static string GetInSecureHostRoot()
	{
		string protocol = "http";
		string host = GetHostName();
		return $"{protocol}://{host}";
	}

	public static string GetHostName()
	{
		if (HttpContext.Current is null)
		{
			return string.Empty;
		}

		if (HttpContext.Current.Items["hostname"] is not string hostname)
		{
			hostname = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
			if (hostname is not null)
			{
				HttpContext.Current.Items["hostname"] = hostname;
			}
		}
		return hostname;
	}

	public static string GetVirtualRoot()
	{
		string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		return $"/{serverName}{GetApplicationRoot()}";
	}

	public static void ForceSsl()
	{
		bool.TryParse(ConfigurationManager.AppSettings["ProxyPreventsSSLDetection"], out bool proxyPreventsSSLDetection);
		// proxyPreventsSSLDetection is false if parsing failed for any reason

		if (!proxyPreventsSSLDetection)
		{
			string url = HttpContext.Current.Request.Url.ToString();
			if (url.StartsWith("http:"))
			{
				HttpContext.Current.Response.Redirect("https" + url.Remove(0, 4), false);
			}
		}
	}

	public static void ClearSsl()
	{
		string url = HttpContext.Current.Request.Url.ToString();
		if (url.StartsWith("https:"))
		{
			HttpContext.Current.Response.Redirect("http" + url.Remove(0, 5), false);
		}
	}

	public static string GetQueryString(string rawUrl)
	{
		if (!rawUrl.Contains("?"))
		{
			return string.Empty;
		}

		if (rawUrl.IndexOf("?") == rawUrl.Length - 1)
		{
			return string.Empty;
		}

		return rawUrl.Substring(rawUrl.IndexOf("?"), rawUrl.Length - rawUrl.IndexOf("?"));
	}

	public static string GetUrlWithoutQueryString(string rawUrl)
	{
		if (!rawUrl.Contains("?"))
		{
			return rawUrl;
		}
		return rawUrl.Substring(0, rawUrl.IndexOf("?"));
	}

	public static string BuildQueryString(string leaveOutParam)
	{
		if (string.IsNullOrEmpty(leaveOutParam))
		{
			return string.Empty;
		}

		string queryString = HttpContext.Current.Request.QueryString.ToString();

		return BuildQueryString(queryString, leaveOutParam);
	}

	public static string BuildQueryString(string queryString, string leaveOutParam)
	{
		if (string.IsNullOrEmpty(leaveOutParam))
		{
			return string.Empty;
		}

		if (string.IsNullOrEmpty(queryString))
		{
			return string.Empty;
		}

		if (queryString.Length > 0)
		{
			if (queryString.StartsWith("?"))
			{
				queryString = queryString.Remove(0, 1);
			}
			if (!queryString.StartsWith("&"))
			{
				queryString = "&" + queryString;
			}
			int indexOfLeaveOutParam = queryString.IndexOf("&" + leaveOutParam);
			if (indexOfLeaveOutParam > -1)
			{
				string leavoutParamValue = HttpContext.Current.Request.Params.Get(leaveOutParam);
				string stringBeforeLeaveOutParam = queryString.Substring(0, indexOfLeaveOutParam);
				string stringAfterLeaveOutParam =
					queryString.Substring(indexOfLeaveOutParam + leaveOutParam.Length + 1 + leavoutParamValue.Length);
				if (stringAfterLeaveOutParam.Length > 0)
				{
					if (!stringAfterLeaveOutParam.StartsWith("&"))
					{
						if (stringAfterLeaveOutParam.IndexOf("&") > -1)
						{
							stringAfterLeaveOutParam =
								stringAfterLeaveOutParam.Substring(stringAfterLeaveOutParam.IndexOf("&"));
						}
						else
						{
							stringAfterLeaveOutParam = "";
						}
					}
				}
				queryString = stringBeforeLeaveOutParam + stringAfterLeaveOutParam;
			}
		}
		return queryString;
	}

	public static string RemoveQueryStringParam(string leaveOutParam)
	{
		if (string.IsNullOrEmpty(leaveOutParam))
		{
			return string.Empty;
		}

		string queryString = HttpContext.Current.Request.QueryString.ToString();
		return RemoveQueryStringParam(queryString, leaveOutParam);
	}

	public static string RemoveQueryStringParam(string queryString, string leaveOutParam)
	{
		if (string.IsNullOrEmpty(leaveOutParam))
		{
			return string.Empty;
		}

		if (string.IsNullOrEmpty(queryString))
		{
			return string.Empty;
		}

		if (queryString.Length > 0)
		{
			if (queryString.StartsWith("?"))
			{
				queryString = queryString.Remove(0, 1);
			}
			if (!queryString.StartsWith("&"))
			{
				queryString = "&" + queryString;
			}
			int indexOfLeaveOutParam = queryString.IndexOf("&" + leaveOutParam);
			if (indexOfLeaveOutParam > -1)
			{
				string stringBeforeLeaveOutParam = queryString.Substring(0, indexOfLeaveOutParam);
				string stringAfterLeaveOutParam = queryString.Substring(indexOfLeaveOutParam + leaveOutParam.Length);
				if (stringAfterLeaveOutParam.Length > 0)
				{
					if (!stringAfterLeaveOutParam.StartsWith("&"))
					{
						if (stringAfterLeaveOutParam.IndexOf("&") > -1)
						{
							stringAfterLeaveOutParam =
								stringAfterLeaveOutParam.Substring(stringAfterLeaveOutParam.IndexOf("&"));
						}
						else
						{
							stringAfterLeaveOutParam = "";
						}
					}
				}
				queryString = stringBeforeLeaveOutParam + stringAfterLeaveOutParam;
			}

			if (queryString.StartsWith("&"))
			{
				queryString = queryString.Remove(0, 1);
			}
		}
		return queryString;
	}

	public static string GetHtmlFromWeb(string url)
	{
		string html = string.Empty;

		try
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = "GET";
			webRequest.Timeout = 5000; // 5 seconds


			HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

			if (webResponse is not null)
			{
				using var responseStream = new StreamReader(webResponse.GetResponseStream());
				html = responseStream.ReadToEnd();
			}
		}
		catch (Exception ex)
		{
			log.Error("WebUtils.GetHtmlFromWeb error scraping html from web.", ex);
		}

		return html;
	}

	public static void SetupRedirect(Control control, string redirectUrl)
	{
		// the reason for using this overload of Response.Redirect:
		//  Response.Redirect("somepage.aspx", false);
		// as opposed to this
		//  Response.Redirect("somepage.aspx");
		// is that by default true is used which causes Response.End to be invoked automatically
		// this can increase the number of ThreadWasAboutToAbortExceptions and impact performance
		// in some scenarios
		// however using the overlaod with 2 params also allows the current thread to continue processing
		// after redirection which is not neccessarily desireable and can have some security implications.
		// If using redirection to enforce security, like redirecting to the  AccessDenied page, it is best to always
		// use plain old Response.Redirect("somepage.aspx");
		// so that the response is ended immediately.
		// When redirecting after an update maximum performance can be achieved using
		// Response.Redirect("somepage.aspx", false);
		// You should call SiteUtils.UseGracefulThreadEnding after your redirect using
		// Response.Redirect("somepage.aspx", false);
		// in order to gracefully end processing in the most secure fashion possible

		// for more info about the above see
		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspp/html/monitor_perf.asp

		if (redirectUrl is not null)
		{
			HttpContext.Current.Response.Redirect(redirectUrl, false);

			if (control is not null)
			{
				control.EnableViewState = false;
				control.Visible = false;
			}

			HttpContext.Current.ApplicationInstance.CompleteRequest();
			return;
		}
	}

	public static bool NullToFalse(object o)
	{
		if (o is null)
		{
			return false;
		}

		if (o is bool v)
		{
			return v;
		}

		if (o is int v1)
		{
			return v1 > 0;
		}

		string str = o.ToString();
		if (string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase) || (str == "1"))
		{
			return true;
		}

		if (string.Equals(str, "false", StringComparison.InvariantCultureIgnoreCase) || (str == "0"))
		{
			return false;
		}

		return false;
	}

	public static bool NullToTrue(object o)
	{
		if (o is null)
		{
			return true;
		}
		if (o is bool v)
		{
			return v;
		}
		if (o is int v1)
		{
			return v1 > 0;
		}

		string str = o.ToString();
		if (string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase) || (str == "1"))
		{
			return true;
		}
		if (string.Equals(str, "false", StringComparison.InvariantCultureIgnoreCase) || (str == "0"))
		{
			return false;
		}

		return true;
	}

	public static string ParseStringFromHashtable(Hashtable settings, string key, string defaultIfNotFound = "")
	{
		if (settings.Contains(key))
		{
			return settings[key].ToString().Trim();
		}
		return defaultIfNotFound;
	}

	public static int ParseInt32FromHashtable(Hashtable settings, string key, int defaultIfNotFound)
	{
		int returnValue = defaultIfNotFound;
		if (settings is null) { return returnValue; }

		if ((key is not null)
			&& settings.Contains(key)
			&& (!int.TryParse(settings[key].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out returnValue))
			)
		{
			returnValue = defaultIfNotFound;
		}

		return returnValue;
	}

	public static double ParseDoubleFromHashtable(Hashtable settings, string key, double defaultIfNotFound)
	{
		double returnValue = defaultIfNotFound;

		if ((key is not null)
			&& settings.Contains(key)
			&& (!double.TryParse(
			settings[key].ToString(),
			NumberStyles.Any,
			CultureInfo.InvariantCulture,
			out returnValue))
			)
		{
			returnValue = defaultIfNotFound;
		}

		return returnValue;
	}

	public static bool ParseBoolFromHashtable(Hashtable settings, string key, bool defaultIfNotFound)
	{
		bool returnValue = defaultIfNotFound;

		if (
			(settings is not null)
			&& (key is not null)
			&& settings.Contains(key)
			)
		{
			if (string.Equals(settings[key].ToString(), "true", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}

		return returnValue;
	}

	public static Guid ParseGuidFromHashTable(Hashtable settings, string key, Guid defaultIfNotFoundOrInvalid)
	{
		Guid returnValue = defaultIfNotFoundOrInvalid;

		if (
			(settings is not null)
			&& (key is not null)
			&& settings.Contains(key)
			)
		{
			string foundSetting = settings[key].ToString();
			if (foundSetting.Length == 36)
			{
				returnValue = new Guid(foundSetting);
			}
		}
		return returnValue;
	}

	public static DateTime ParseDateFromQueryString(string paramName, DateTime defaultIfNotFound)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}

		if (HttpContext.Current is null) { return defaultIfNotFound; }
		if (HttpContext.Current.Request.QueryString[paramName] is null) { return defaultIfNotFound; }

		DateTime result = defaultIfNotFound;

		DateTimeFormatInfo dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
		DateTime.TryParse(HttpContext.Current.Request.QueryString[paramName], dtfi, DateTimeStyles.AdjustToUniversal, out result);

		return result;
	}

	public static int ParseInt32FromQueryString(string paramName, int defaultIfNotFoundOrInvalid)
	{
		if (TryLoadRequestParam(paramName, out int paramValue))
		{
			return paramValue;
		}
		return defaultIfNotFoundOrInvalid;
	}

	public static int ParseInt32FromQueryString(string paramName, bool onlyUseQueryString, int defaultIfNotFoundOrInvalid)
	{
		//added this 2009-04-18, when using query string param userid, it was conflicting with cookie params from windows live
		if (TryLoadRequestParam(paramName, onlyUseQueryString, out int paramValue))
		{
			return paramValue;
		}
		return defaultIfNotFoundOrInvalid;
	}

	//public static DateTime ParseDateFromQueryString(String paramName, DateTime defaultIfNotFoundOrInvalid)
	//{
	//    DateTime paramValue;
	//    if (TryLoadRequestParam(paramName, out paramValue)) return paramValue;
	//    return defaultIfNotFoundOrInvalid;
	//}

	public static Guid ParseGuidFromQueryString(string paramName, Guid defaultIfNotFoundOrInvalid)
	{
		if (TryLoadRequestParam(paramName, out Guid paramValue))
		{
			return paramValue;
		}
		return defaultIfNotFoundOrInvalid;
	}

	public static string ParseStringFromQueryString(string paramName, string defaultIfNotFoundOrInvalid)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			return defaultIfNotFoundOrInvalid;
		}

		string val = HttpContext.Current.Request.QueryString[paramName];
		if (string.IsNullOrEmpty(val))
		{
			return defaultIfNotFoundOrInvalid;
		}

		return val.RemoveMarkup();
	}

	public static bool ParseBoolFromQueryString(string paramName, bool defaultIfNotFoundOrInvalid)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			return defaultIfNotFoundOrInvalid;
		}
		if (HttpContext.Current.Request.QueryString[paramName] is null)
		{
			return defaultIfNotFoundOrInvalid;
		}

		string val = HttpContext.Current.Request.QueryString[paramName];
		if (string.IsNullOrEmpty(val))
		{
			return defaultIfNotFoundOrInvalid;
		}

		bool result = defaultIfNotFoundOrInvalid;

		bool.TryParse(val, out result);

		return result;

	}

	/// <summary>
	/// Loads parameter of a given type from query string.
	/// Returns the given value if operation failed.
	/// </summary>
	public static T LoadOptionalRequestParam<T>(string paramName, T defaultIfNotFoundOrInvalid)
	{
		if (TryLoadRequestParam(paramName, out T paramValue))
		{
			return paramValue;
		}
		return defaultIfNotFoundOrInvalid;
	}

	#region TryLoadRequestParam functions

	/// <summary>
	/// Loads Int32 parameter from query string.
	/// A return value indicates whether the operation succeeded.
	/// </summary>
	public static bool TryLoadRequestParam(string paramName, out int paramValue)
	{
		bool onlyUseQueryString = false;
		return TryLoadRequestParam(paramName, onlyUseQueryString, out paramValue);
	}

	public static bool TryLoadRequestParam(string paramName, bool onlyUseQueryString, out int paramValue)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			paramValue = default;
			return false;
		}

		if (onlyUseQueryString)
		{
			return int.TryParse(HttpContext.Current.Request.QueryString[paramName],
							 NumberStyles.Integer, CultureInfo.InvariantCulture, out paramValue);
		}

		return int.TryParse(HttpContext.Current.Request.Params[paramName],
							  NumberStyles.Integer, CultureInfo.InvariantCulture, out paramValue);
	}

	/// <summary>
	/// Loads String parameter from query string.
	/// A return value indicates whether the operation succeeded.
	/// </summary>
	public static bool TryLoadRequestParam(string paramName, out string paramValue)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			paramValue = default(string);
			return false;
		}

		paramValue = HttpContext.Current.Request.Params[paramName];
		return (paramValue is not null);
	}

	/// <summary>
	/// Loads DateTime parameter from query string.
	/// A return value indicates whether the operation succeeded.
	/// </summary>
	public static bool TryLoadRequestParam(string paramName, out DateTime paramValue)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			paramValue = default(DateTime);
			return false;
		}

		return DateTime.TryParse(HttpContext.Current.Request.Params[paramName],
								 CultureInfo.InvariantCulture, DateTimeStyles.None, out paramValue);
	}

	/// <summary>
	/// Loads Guid parameter from query string.
	/// A return value indicates whether the operation succeeded.
	/// </summary>
	public static bool TryLoadRequestParam(string paramName, out Guid paramValue)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			paramValue = default(Guid);
			return false;
		}

		if (string.IsNullOrEmpty(HttpContext.Current.Request.Params[paramName]))
		{
			paramValue = default(Guid);
			return false;
		}

		try
		{
			paramValue = new Guid(HttpContext.Current.Request.Params[paramName]);
			return true;
		}
		catch (FormatException)
		{
			paramValue = default(Guid);
			return false;
		}
	}

	/// <summary>
	/// Loads parameter of a given type from query string.
	/// A return value indicates whether the operation succeeded.
	/// </summary>
	public static bool TryLoadRequestParam<T>(string paramName, out T ParamValue)
	{
		if (string.IsNullOrEmpty(paramName))
		{
			throw new ArgumentNullException("ParamName");
		}
		if (HttpContext.Current is null)
		{
			ParamValue = default(T);
			return false;
		}

		string paramValue = HttpContext.Current.Request.Params[paramName];
		if (paramValue is null)
		{
			ParamValue = default(T);
			return false;
		}

		try
		{
			ParamValue = (T)Convert.ChangeType(paramValue, typeof(T));
		}
		catch (InvalidCastException)
		{
			ParamValue = default(T);
			return false;
		}

		return true;
	}

	#endregion


	/// <summary>
	/// Returns a site relative HTTP path from a partial path starting out with a ~.
	/// Same syntax that ASP.Net internally supports but this method can be used
	/// outside of the Page framework.
	/// 
	/// Works like Control.ResolveUrl including support for ~ syntax
	/// but returns an absolute URL.
	/// </summary>
	/// <param name="originalUrl">Any Url including those starting with ~</param>
	/// <returns>relative url</returns>
	public static string ResolveUrl(string originalUrl)
	{
		if (string.IsNullOrEmpty(originalUrl))
		{
			return originalUrl;
		}

		// *** Absolute path - just return
		if (IsAbsolutePath(originalUrl))
		{
			return originalUrl;
		}

		// *** We don't start with the '~' -> we don't process the Url
		if (!originalUrl.StartsWith("~"))
		{
			return originalUrl;
		}

		// *** Fix up path for ~ root app dir directory
		// VirtualPathUtility blows up if there is a 
		// query string, so we have to account for this.
		int queryStringStartIndex = originalUrl.IndexOf('?');
		if (queryStringStartIndex != -1)
		{
			string queryString = originalUrl.Substring(queryStringStartIndex);
			string baseUrl = originalUrl.Substring(0, queryStringStartIndex);

			return string.Concat(
				VirtualPathUtility.ToAbsolute(baseUrl),
				queryString);
		}
		else
		{
			return VirtualPathUtility.ToAbsolute(originalUrl);
		}

	}

	/// <summary>
	/// This method returns a fully qualified absolute server Url which includes
	/// the protocol, server, port in addition to the server relative Url.
	/// 
	/// Works like Control.ResolveUrl including support for ~ syntax
	/// but returns an absolute URL.
	/// </summary>
	public static string ResolveServerUrl(string serverUrl, bool forceHttps)
	{
		if (string.IsNullOrEmpty(serverUrl))
		{
			return serverUrl;
		}

		// *** Is it already an absolute Url?
		if (IsAbsolutePath(serverUrl))
		{
			return serverUrl;
		}

		string newServerUrl = ResolveUrl(serverUrl);
		Uri result = new Uri(HttpContext.Current.Request.Url, newServerUrl);

		if (!forceHttps)
		{
			return result.ToString();
		}
		else
			return ForceUriToHttps(result).ToString();

	}

	/// <summary>
	/// This method returns a fully qualified absolute server Url which includes
	/// the protocol, server, port in addition to the server relative Url.
	/// 
	/// It work like Page.ResolveUrl, but adds these to the beginning.
	/// This method is useful for generating Urls for AJAX methods
	/// </summary>
	/// <param name="serverUrl">Any Url, either App relative or fully qualified</param>
	/// <returns></returns>
	public static string ResolveServerUrl(string serverUrl)
	{
		bool forceHttps = WebHelper.IsSecureRequest();
		return ResolveServerUrl(serverUrl, forceHttps);
	}

	/// <summary>
	/// Forces the Uri to use https
	/// </summary>
	private static Uri ForceUriToHttps(Uri uri)
	{
		// ** Re-write Url using builder.
		UriBuilder builder = new UriBuilder(uri);
		builder.Scheme = Uri.UriSchemeHttps;
		builder.Port = 443;

		return builder.Uri;
	}

	private static bool IsAbsolutePath(string originalUrl)
	{
		// *** Absolute path - just return
		int IndexOfSlashes = originalUrl.IndexOf("://");
		int IndexOfQuestionMarks = originalUrl.IndexOf("?");

		if (IndexOfSlashes > -1 &&
			 (IndexOfQuestionMarks < 0 ||
			  (IndexOfQuestionMarks > -1 && IndexOfQuestionMarks > IndexOfSlashes)
			  )
			)
		{
			return true;
		}

		return false;
	}

}
