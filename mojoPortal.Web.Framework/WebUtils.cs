// Created:					2004-07-14
// Last Modified:			2019-04-04
// 
// 4/30/2005	Dean Brettle Provided a better handling of proxy settings
//				in generating the base path for site links
// 02/05/2007  Alexander Yushchenko added TryLoadRequestParam functions
//				
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Core.Configuration;
using mojoPortal.Core.Helpers;
namespace mojoPortal.Web.Framework
{
	/// <summary>
	/// Utility functions
	/// </summary>
	public static class WebUtils
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebUtils));
        private static bool debugLog = log.IsDebugEnabled;

        public static string GetRequestBody(this HttpRequest request)
        {
            using (StreamReader reader = new StreamReader(request.InputStream))
            {
                string encodedString = reader.ReadToEnd();
                return HttpUtility.UrlDecode(encodedString);
            }
        }

        public static string GetApplicationRoot()
        {
            if (HttpContext.Current == null) return String.Empty;

            string applicationRoot = HttpContext.Current.Items["applicationRoot"] as string;
            if (applicationRoot == null)
            {
                applicationRoot = CalculateApplicationRoot();
                if (applicationRoot != null)
                    HttpContext.Current.Items["applicationRoot"] = applicationRoot;
            }
            return applicationRoot;

           
        }

        public static bool IsRequestForStaticFile(string requestPath)
        {
            if (string.IsNullOrWhiteSpace(requestPath.Replace("/", ""))) { return false; }

            if (requestPath.ContainsCaseInsensitive(".aspx")) { return false; }
            if (requestPath.ContainsCaseInsensitive(".ashx")) { return false; }
            if (requestPath.ContainsCaseInsensitive(".svc")) { return false; }

            var staticExtensions = AppConfig.StaticFileExtensions;

            var extIdx = requestPath.LastIndexOf('.');


            var foo = staticExtensions.SplitOnCharAndTrim('|').Contains(requestPath.Substring(extIdx > -1 ? extIdx : 0, extIdx > -1 ? requestPath.Length - extIdx : requestPath.Length));

            return foo;

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
            if (HttpContext.Current == null) return String.Empty;
            if (protocol == null) protocol = string.Empty;

            string host = HttpContext.Current.Items["host" + protocol] as string;
            if (host == null)
            {
                host = DetermineHost(protocol);
                if (host != null)
                    HttpContext.Current.Items["host" + protocol] = host;
            }
            return host;


        }

        // Returns hostname[:port] to use when constructing the site root URL.
        private static string DetermineHost(string protocol)
        {
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            //if (debugLog)
            //{
            //    log.Debug("ServerName= " + serverName);
            //    log.Debug("serverPort= " + serverPort);

            //}

            // Most proxies add an X-Forwarded-Host header which contains the original Host header
            // including any non-default port.

            string forwardedHosts = HttpContext.Current.Request.Headers["X-Forwarded-Host"];
            if (forwardedHosts != null)
            {
                // If the request passed thru multiple proxies, they will be separated by commas.
                // We only care about the first one.
                string forwardedHost = forwardedHosts.Split(',')[0];

                //if (debugLog) { log.Debug("forwardedHost= " + forwardedHost); }

                string[] serverAndPort = forwardedHost.Split(':');
                serverName = serverAndPort[0];

                //if (debugLog)
                //{
                //    log.Debug("serverName after split from serverAndPort = " + serverName);

                //}

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
                //if (debugLog)
                //{
                //    log.Debug("protocol == https , serverPort == 80");
                //}

                bool mapAlternateSSLPort = ConfigHelper.GetBoolProperty("MapAlternateSSLPort", false);
                if (mapAlternateSSLPort)
                {
                    string alternatSSLPort = ConfigurationManager.AppSettings["AlternateSSLPort"];
                    if (alternatSSLPort != null)
                    {
                        serverPort = alternatSSLPort;
                    }
                }

            }

            string host = serverName;

            //if (debugLog)
            //{
            //    log.Debug("host= " + host);

            //}


            if (serverPort != null)
            {
                bool mapAlternatePort = ConfigHelper.GetBoolProperty("MapAlternatePort", false);
                if (mapAlternatePort)
                {
                    host += ":" + serverPort;
                }
            }

            return host;
        }


        public static string GetSiteRoot()
        {
            if (HttpContext.Current == null) return string.Empty;

            string siteRoot = HttpContext.Current.Items["siteRoot"] as string;
            if (siteRoot == null)
            {
                siteRoot = DetermineSiteRoot();
                if (siteRoot != null)
                    HttpContext.Current.Items["siteRoot"] = siteRoot;
            }
            return siteRoot;


        }

        public static string GetRelativeSiteRoot()
        {
            if (HttpContext.Current == null) return string.Empty;

            string siteRoot = HttpContext.Current.Items["relativesiteRoot"] as string;
            if (siteRoot == null)
            {
                siteRoot = GetApplicationRoot();
                if (siteRoot != null)
                    HttpContext.Current.Items["relativesiteRoot"] = siteRoot;
            }
            return siteRoot;


        }

        public static string GetHostRoot()
        {
            string protocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
            string host = GetHost(protocol);
            return protocol + "://" + host;
        }

        private static string DetermineSiteRoot()
        {
            string protocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
            string host = GetHost(protocol);
            return protocol + "://" + host + GetApplicationRoot();
        }

        

        public static string GetSecureSiteRoot()
        {
            string protocol = "https";
            string host = GetHost(protocol);
            return protocol + "://" + host + GetApplicationRoot();
        }

        public static string GetSecureHostRoot()
        {
            string protocol = "https";
            string host = GetHost(protocol);
            return protocol + "://" + host;
        }

        public static string GetInSecureSiteRoot()
        {
            string protocol = "http";
            string host = GetHostName();
            return protocol + "://" + host + GetApplicationRoot();
        }

        public static string GetInSecureHostRoot()
        {
            string protocol = "http";
            string host = GetHostName();
            return protocol + "://" + host;
        }

        public static string GetHostName()
        {
            if (HttpContext.Current == null) return String.Empty;

            
            string hostname = HttpContext.Current.Items["hostname"] as string;
            if (hostname == null)
            {
                hostname = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                if (hostname != null)
                    HttpContext.Current.Items["hostname"] = hostname;
            }
            return hostname;

            //return HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
            //return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        }

        public static string GetVirtualRoot()
        {
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

            return "/" + serverName + GetApplicationRoot();
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
            if (!rawUrl.Contains("?")) return string.Empty;
            if (rawUrl.IndexOf("?") == rawUrl.Length -1) return string.Empty;
            return rawUrl.Substring(rawUrl.IndexOf("?"), rawUrl.Length - rawUrl.IndexOf("?"));

        }

        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            if (!rawUrl.Contains("?")) return rawUrl;
            return rawUrl.Substring(0, rawUrl.IndexOf("?"));

        }



        public static string BuildQueryString(string leaveOutParam)
        {
            if (String.IsNullOrEmpty(leaveOutParam)) return String.Empty;

            string queryString = HttpContext.Current.Request.QueryString.ToString();

            return BuildQueryString(queryString, leaveOutParam);
            
        }

        public static string BuildQueryString(string queryString, string leaveOutParam)
        {
            if (String.IsNullOrEmpty(leaveOutParam)) return String.Empty;
            if (String.IsNullOrEmpty(queryString)) return String.Empty;

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
            if (String.IsNullOrEmpty(leaveOutParam)) return String.Empty;

            string queryString = HttpContext.Current.Request.QueryString.ToString();
            return RemoveQueryStringParam(queryString, leaveOutParam);
            
        }

        public static string RemoveQueryStringParam(string queryString, string leaveOutParam)
        {
            if (String.IsNullOrEmpty(leaveOutParam)) return String.Empty;
            if (String.IsNullOrEmpty(queryString)) return String.Empty;
            
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
                

                HttpWebResponse webResponse
                    = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse != null)
                {
                    using (StreamReader responseStream =
                       new StreamReader(webResponse.GetResponseStream()))
                    {
                        html = responseStream.ReadToEnd();
                        
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("WebUtils.GetHtmlFromWeb error scraping html from web.", ex);

            }

            return html;

        }


        //public static string RemoveQueryStringParam(string leaveOutParam)
        //{
        //    if (String.IsNullOrEmpty(leaveOutParam)) return String.Empty;

        //    string queryString = HttpContext.Current.Request.Url.ToString();
        //    queryString = queryString.Remove(0, queryString.IndexOf(".aspx") + 5);
        //    if (queryString.Length > 0)
        //    {
        //        if (queryString.StartsWith("?"))
        //        {
        //            queryString = queryString.Remove(0, 1);
        //        }
        //        if (!queryString.StartsWith("&"))
        //        {
        //            queryString = "&" + queryString;
        //        }
        //        int indexOfLeaveOutParam = queryString.IndexOf("&" + leaveOutParam);
        //        if (indexOfLeaveOutParam > -1)
        //        {
        //            string stringBeforeLeaveOutParam = queryString.Substring(0, indexOfLeaveOutParam);
        //            string stringAfterLeaveOutParam = queryString.Substring(indexOfLeaveOutParam + leaveOutParam.Length);
        //            if (stringAfterLeaveOutParam.Length > 0)
        //            {
        //                if (!stringAfterLeaveOutParam.StartsWith("&"))
        //                {
        //                    if (stringAfterLeaveOutParam.IndexOf("&") > -1)
        //                    {
        //                        stringAfterLeaveOutParam =
        //                            stringAfterLeaveOutParam.Substring(stringAfterLeaveOutParam.IndexOf("&"));
        //                    }
        //                    else
        //                    {
        //                        stringAfterLeaveOutParam = "";
        //                    }
        //                }
        //            }
        //            queryString = stringBeforeLeaveOutParam + stringAfterLeaveOutParam;
        //        }

        //        if (queryString.StartsWith("&"))
        //        {
        //            queryString = queryString.Remove(0, 1);
        //        }
        //    }
        //    return queryString;
        //}

        


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

            if (redirectUrl != null)
            {
                HttpContext.Current.Response.Redirect(redirectUrl, false);

                if (control != null)
                {
                    control.EnableViewState = false;
                    control.Visible = false;
                }

                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

        }


        public static bool NullToFalse(Object o)
        {
            if (o == null) return false;
            if (o is bool) return (bool)o;
            if (o is int) return ((int)o > 0);

            string str = o.ToString();
            if ((string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase)) || (str == "1")) return true;
            if ((string.Equals(str, "false", StringComparison.InvariantCultureIgnoreCase)) || (str == "0")) return false;

            return false;
        }

        public static bool NullToTrue(Object o)
        {
            if (o == null) return true;
            if (o is bool) return (bool)o;
            if (o is int) return ((int)o > 0);

            string str = o.ToString();
            if ((string.Equals(str, "true", StringComparison.InvariantCultureIgnoreCase)) || (str == "1")) return true;
            if ((string.Equals(str, "false", StringComparison.InvariantCultureIgnoreCase)) || (str == "0")) return false;

            return true;
        }

        public static Int32 ParseInt32FromHashtable(
            Hashtable settings,
            String key, 
            Int32 defaultIfNotFound)
        {
            int returnValue = defaultIfNotFound;
            if (settings == null) { return returnValue; }
            
            if (
                (key != null)
                &&(settings.Contains(key))
                && (!Int32.TryParse(settings[key].ToString(),NumberStyles.Any,CultureInfo.InvariantCulture,out returnValue))
                )
            {
                returnValue = defaultIfNotFound;
            }
            
            return returnValue;

        }

        public static double ParseDoubleFromHashtable(
            Hashtable settings,
            String key,
            double defaultIfNotFound)
        {
            double returnValue = defaultIfNotFound;

            if (
                (key != null)
                && (settings.Contains(key))
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

        public static bool ParseBoolFromHashtable(
            Hashtable settings,
            String key,
            bool defaultIfNotFound)
        {
            bool returnValue = defaultIfNotFound;

            if (
                (settings != null)
                &&(key != null)
                && (settings.Contains(key))
                )
            {
                if (string.Equals(settings[key].ToString(),"true", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            return returnValue;

        }

        public static Guid ParseGuidFromHashTable(
            Hashtable settings,
            String key, 
            Guid defaultIfNotFoundOrInvalid)
        {
            Guid returnValue = defaultIfNotFoundOrInvalid;

            if (
                (settings != null)
                && (key != null)
                && (settings.Contains(key))
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
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null) { return defaultIfNotFound; }
            if (HttpContext.Current.Request.QueryString[paramName] == null) { return defaultIfNotFound; }

            DateTime result = defaultIfNotFound;

            DateTimeFormatInfo dtfi = CultureInfo.InvariantCulture.DateTimeFormat;
            DateTime.TryParse(HttpContext.Current.Request.QueryString[paramName], dtfi, DateTimeStyles.AdjustToUniversal, out result);
            
            return result;
        }

        public static Int32 ParseInt32FromQueryString(String paramName, Int32 defaultIfNotFoundOrInvalid)
        {
            Int32 paramValue;
            if (TryLoadRequestParam(paramName, out paramValue)) return paramValue;
            return defaultIfNotFoundOrInvalid;
        }

        public static Int32 ParseInt32FromQueryString(String paramName, bool onlyUseQueryString, Int32 defaultIfNotFoundOrInvalid)
        {
            //added this 2009-04-18, when using query string param userid, it was conflicting with cookie params from windows live
            Int32 paramValue;
            if (TryLoadRequestParam(paramName, onlyUseQueryString, out paramValue)) return paramValue;
            return defaultIfNotFoundOrInvalid;
        }

        //public static DateTime ParseDateFromQueryString(String paramName, DateTime defaultIfNotFoundOrInvalid)
        //{
        //    DateTime paramValue;
        //    if (TryLoadRequestParam(paramName, out paramValue)) return paramValue;
        //    return defaultIfNotFoundOrInvalid;
        //}

        public static Guid ParseGuidFromQueryString(String paramName, Guid defaultIfNotFoundOrInvalid)
        {
            Guid paramValue;
            if (TryLoadRequestParam(paramName, out paramValue)) return paramValue;
            return defaultIfNotFoundOrInvalid;
        }

        public static string ParseStringFromQueryString(string paramName, string defaultIfNotFoundOrInvalid)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null) { return defaultIfNotFoundOrInvalid; }

            string val = HttpContext.Current.Request.QueryString[paramName];
            if (String.IsNullOrEmpty(val)) { return defaultIfNotFoundOrInvalid; }
            return val;
        }

        public static bool ParseBoolFromQueryString(string paramName, bool defaultIfNotFoundOrInvalid)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null) { return defaultIfNotFoundOrInvalid; }
            if (HttpContext.Current.Request.QueryString[paramName] == null) { return defaultIfNotFoundOrInvalid; }

            string val = HttpContext.Current.Request.QueryString[paramName];
            if (String.IsNullOrEmpty(val)) { return defaultIfNotFoundOrInvalid; }

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
            T paramValue;
            if (TryLoadRequestParam(paramName, out paramValue)) return paramValue;
            return defaultIfNotFoundOrInvalid;
        }


        #region TryLoadRequestParam functions

        /// <summary>
        /// Loads Int32 parameter from query string.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        public static bool TryLoadRequestParam(String paramName, out Int32 paramValue)
        {
            bool onlyUseQueryString = false;
            return TryLoadRequestParam(paramName, onlyUseQueryString, out paramValue);

            
        }

        public static bool TryLoadRequestParam(String paramName, bool onlyUseQueryString, out Int32 paramValue)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null)
            {
                paramValue = default(Int32);
                return false;
            }

            if (onlyUseQueryString)
            {
                return Int32.TryParse(HttpContext.Current.Request.QueryString[paramName],
                                 NumberStyles.Integer, CultureInfo.InvariantCulture, out paramValue);
            }

            return Int32.TryParse(HttpContext.Current.Request.Params[paramName],
                                  NumberStyles.Integer, CultureInfo.InvariantCulture, out paramValue);
        }

        /// <summary>
        /// Loads String parameter from query string.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        public static bool TryLoadRequestParam(String paramName, out String paramValue)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null)
            {
                paramValue = default(String);
                return false;
            }

            paramValue = HttpContext.Current.Request.Params[paramName];
            return (paramValue != null);
        }

        /// <summary>
        /// Loads DateTime parameter from query string.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        public static bool TryLoadRequestParam(String paramName, out DateTime paramValue)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null)
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
        public static bool TryLoadRequestParam(String paramName, out Guid paramValue)
        {
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null)
            {
                paramValue = default(Guid);
                return false;
            }

            if (String.IsNullOrEmpty(HttpContext.Current.Request.Params[paramName]))
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
            if (String.IsNullOrEmpty(paramName)) throw new ArgumentNullException("ParamName");
            if (HttpContext.Current == null)
            {
                ParamValue = default(T);
                return false;
            }

            string paramValue = HttpContext.Current.Request.Params[paramName];
            if (paramValue == null)
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
                return originalUrl;

            // *** Absolute path - just return
            if (IsAbsolutePath(originalUrl))
                return originalUrl;

            // *** We don't start with the '~' -> we don't process the Url
            if (!originalUrl.StartsWith("~"))
                return originalUrl;

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
                return serverUrl;

            // *** Is it already an absolute Url?
            if (IsAbsolutePath(serverUrl))
                return serverUrl;

            string newServerUrl = ResolveUrl(serverUrl);
            Uri result = new Uri(HttpContext.Current.Request.Url, newServerUrl);

            if (!forceHttps)
                return result.ToString();
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
                return true;

            return false;
        }

    }
}
