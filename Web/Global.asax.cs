using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.App_Start;
using mojoPortal.Web.Caching;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Optimization;
using mojoPortal.Web.Routing;
using mojoPortal.Web.Security;
using Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;

//using mojoPortal.Web.ModelBinders;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace mojoPortal.Web;

//http://haacked.com/archive/2010/05/16/three-hidden-extensibility-gems-in-asp-net-4.aspx/
//public class WebInitializer
//{
//	public static void Initialize()
//	{
//		// Whatever can we do here?
//	}
//}

public class Global : HttpApplication
{
	#region Private Fields

	private static readonly ILog log = LogManager.GetLogger(typeof(Global));
	private static bool debugLog = log.IsDebugEnabled;
	private const string RequestExceptionKey = "__RequestExceptionKey";

	#endregion


	#region Public Methods

	public static bool RegisteredVirtualThemes { get; private set; } = false;
	public static SkinConfigManager SkinConfigManager { get; private set; }
	public static SkinConfig SkinConfig { get; private set; }
	public static ConcurrentDictionary<string, int> SiteHostMap { get; } = [];
	// this changes everytime the app starts and is used for rss feed autodiscovery links so it will notredirect to feedburner
	// after each app restart the variable will change so that after the user is subscribed it will begin redirecting to feedburner if using feedburner
	public static Guid FeedRedirectBypassToken { get; } = Guid.NewGuid();
	public static bool AppDomainMonitoringEnabled { get; } = false;
	public static bool FirstChanceExceptionMonitoringEnabled { get; } = false;
	// this changes everytime the app starts and the token is required when calling /Services/FileService.ashx
	// to help mitigate against xsrf attacks.
	// 2012-04-25 changed from application variable to cached item since application
	// variable won't work well in a web farm return fileSystemToken; still this will
	// be a problem in a small cluster if not using a distributed cache shared by the nodes.
	public static Guid FileSystemToken => GetFileSystemToken();
	public static OidcService OidcService { get; private set; }

	#endregion


	protected void Application_Start(object sender, EventArgs e)
	{
		#region Configure OAuth/OpenID Connect

		if (AppConfig.OAuth.Configured)
		{
			OidcService = new OidcService(AppConfig.OAuth);
		}

		#endregion

		try
		{
			ServicePointManager.SecurityProtocol |= ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
		}
		catch (NotSupportedException)
		{
			log.Error("Application_Start could not set the chosen security protocol.");
		}

		if (WebConfigSettings.EnableVirtualPathProviders)
		{
			log.Info(Resource.ApplicationStartEventMessage);
			HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());
			RegisteredVirtualThemes = true;
		}

		AutoMapperConfig.Configure();

		AreaRegistration.RegisterAllAreas();
		GlobalConfiguration.Configure(WebApiConfig.Register);
		FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		ViewEngines.Engines.Clear();

		var engine = new mojoViewEngine();
		ViewEngines.Engines.Add(engine);
		AreaRegistration.RegisterAllAreas();
		RouteRegistrar.RegisterRoutes(RouteTable.Routes);

		StartOrResumeTasks();
		PurgeOldLogEvents();

		if (WebConfigSettings.UnobtrusiveValidationMode == "WebForms")
		{
			if (WebConfigSettings.ForceEmptyJQueryScriptReference)
			{
				// since we already have jquery loaded
				// we need to fool scriptmanager by loading a fake version here
				// to avoid an error http://stackoverflow.com/questions/12065228/asp-net-4-5-web-forms-unobtrusive-validation-jquery-issue
				ScriptResourceDefinition jQuery = new ScriptResourceDefinition();
				jQuery.Path = "~/ClientScript/empty.js";
				ScriptManager.ScriptResourceMapping.AddDefinition("jquery", jQuery);
			}
		}
	}


	protected void Application_End(object sender, EventArgs e)
	{
		if (log.IsInfoEnabled) log.Info("------Application Stopped------");
	}


	protected void Application_BeginRequest(object sender, EventArgs e)
	{
		var siteCount = SiteSettings.SiteCount();

		//http://stackoverflow.com/questions/1340643/how-to-enable-ip-address-logging-with-log4net
		ThreadContext.Properties["ip"] = SiteUtils.GetIP4Address();
		ThreadContext.Properties["culture"] = CultureInfo.CurrentCulture.ToString();

		if (HttpContext.Current?.Request != null)
		{
			if (WebConfigSettings.LogFullUrls)
			{
				ThreadContext.Properties["url"] = HttpContext.Current.Request.Url.ToString();
			}
			else
			{
				ThreadContext.Properties["url"] = HttpContext.Current.Request.RawUrl;
			}
		}

		HandleRedirects(siteCount);

		//moved RegisterBundles here so it can properly check the request for SSL. Can't do that when called from Application_Start
		BundleConfig.RegisterBundles(BundleTable.Bundles);

		try
		{
			if (siteCount > 0)
			{
				SkinConfigManager ??= new SkinConfigManager();
				SkinConfig = SkinConfigManager.GetConfig();
			}
		}
		catch (IndexOutOfRangeException ex)
		{
			//this can happen if we're upgrading from an old version of mojoPortal which doesn't have some Page attributes
			log.Error($"Cannot get skin config because of missing page attributes,\r\n{ex}");
		}

		if (AppConfig.SanitizeQueryStrings)
		{
			var originalQueryStrings = new Dictionary<string, string>();
			var sanitizedQueryStrings = new Dictionary<string, string>();

			foreach (string key in Request.QueryString.Keys)
			{
				if (key is not null)
				{
					originalQueryStrings.Add(key, Request.QueryString[key]);
					sanitizedQueryStrings.Add(key, UrlEncode(Request.QueryString[key]));
				}
			}

			// we added the original query strings this way to ensure both strings were handled the same way so there is less of a chance of 
			// inconsequential differences causing our check below to true when it doesn't need to be true
			// someone could do a bit more research/testing on this in the future to see if it's actually necessary.
			var originalQueryString = originalQueryStrings.ToDelimitedString();
			var sanitizedQueryString = sanitizedQueryStrings.ToDelimitedString();

			//we don't want to rewrite anything if there weren't any changes
			if (sanitizedQueryStrings.Count > 0 && originalQueryString != sanitizedQueryString)
			{
				Context.RewritePath(Request.Path, Request.PathInfo, sanitizedQueryStrings.ToDelimitedString());
			}
		}
	}


	protected void Application_EndRequest(object sender, EventArgs e)
	{
		// this is needed otherwise tasks queued on a background thread report these values as they were on the last request handled by the thread
		// so this eliminates the carry over of these settings when a thread is re-used for something other than a web request.
		ThreadContext.Properties["ip"] = null;
		ThreadContext.Properties["culture"] = null;
		ThreadContext.Properties["url"] = null;

		if (
			HttpContext.Current == null ||
			HttpContext.Current.Request == null ||
			HttpContext.Current.Items == null ||
			!HttpContext.Current.Request.IsAuthenticated ||
			!WebConfigSettings.TrackAuthenticatedRequests ||
			HttpContext.Current.Request.Path.Contains(".png", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".gif", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".jpg", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".jpeg", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".svg", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".css", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".axd", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".js", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".ico", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains(".ashx", StringComparison.OrdinalIgnoreCase) ||
			HttpContext.Current.Request.Path.Contains("setup/default.aspx", StringComparison.OrdinalIgnoreCase)
		)
		{
			return;
		}

		// update user activity at the end of each request
		// but only if the siteUser is already in the HttpContext
		// we don't want to lookup the user for little ajax requests
		// unless we have to for security checks
		if (HttpContext.Current.Items["CurrentUser"] != null)
		{
			SiteUtils.TrackUserActivity();
		}
	}


	protected void Application_Error(object sender, EventArgs e)
	{
		bool errorObtained = false;
		Exception ex = null;

		try
		{
			Exception rawException = Server.GetLastError();
			if (rawException != null)
			{
				errorObtained = true;
				if (rawException.InnerException != null)
				{
					ex = rawException.InnerException;
				}
				else
				{
					ex = rawException;
				}
			}
		}
		catch { }

		string exceptionUrl = string.Empty;
		string exceptionIpAddress = string.Empty;
		string exceptionUserAgent = string.Empty;
		string exceptionReferrer = "none";

		if (HttpContext.Current != null)
		{
			if (HttpContext.Current.Request != null)
			{
				exceptionUrl = HttpContext.Current.Request.RawUrl;
				//exceptionIpAddress = SiteUtils.GetIP4Address();
				if (HttpContext.Current.Request.UrlReferrer != null)
				{
					exceptionReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
				}

				if (HttpContext.Current.Request.UserAgent != null)
				{
					exceptionUserAgent = HttpContext.Current.Request.UserAgent;
				}
			}
		}

		if (errorObtained)
		{
			if (ex is UnauthorizedAccessException)
			{
				// swallow this for medium trust?
				log.Error($" Referrer({exceptionReferrer})", ex);
				return;
			}

			if (ex is HttpException)
			{
				exceptionIpAddress = SiteUtils.GetIP4Address();

				if (log4net.ThreadContext.Properties["ip"] == null)
				{
					log4net.ThreadContext.Properties["ip"] = exceptionIpAddress;
				}

				log.Error($"{exceptionIpAddress} {exceptionUrl} Referrer({exceptionReferrer}) useragent {exceptionUserAgent}", ex);
				return;
			}

			if (ex.Message == "File does not exist.")
			{
				log.Error($" Referrer({exceptionReferrer}) useragent {exceptionUserAgent}", ex);
				return;
			}

			log.Error($" Referrer({exceptionReferrer}) useragent {exceptionUserAgent}", ex);

			if (ex is System.Security.Cryptography.CryptographicException)
			{
				// hopefully this is a fix for 
				//http://visualstudiomagazine.com/articles/2010/09/14/aspnet-security-hack.aspx
				// at this time the exploit is not fully disclosed but seems they use the 500 status code 
				// so returning 404 instead may block it
				if (WebConfigSettings.Return404StatusForCryptoError)
				{
					Server.ClearError();
					try
					{
						if (HttpContext.Current != null)
						{
							if (HttpContext.Current.Response != null)
							{
								HttpContext.Current.Response.Clear();

								// add a random delay
								byte[] delay = new byte[1];
								RandomNumberGenerator prng = new RNGCryptoServiceProvider();
								prng.GetBytes(delay);
								Thread.Sleep((int)delay[0]);
								IDisposable disposable = prng;
								disposable?.Dispose();

								HttpContext.Current.Response.StatusCode = 404;
								HttpContext.Current.Response.End();

								log.Info("crypto error trapped and returned a 404 instead of 500 for security reasons");
							}
						}
					}
					catch (HttpException)
					{ }
				}
			}
		}
	}


	protected void Session_Start(object sender, EventArgs e)
	{
		if (debugLog) log.Debug("------ Session Started ------");
		IncrementUserCount();
	}


	protected void Session_End(object sender, EventArgs e)
	{
		if (debugLog) log.Debug("------ Session Ended ------");
		DecrementUserCount();
	}


	#region Private Methods

	private void HandleRedirects(int siteCount)
	{
		if (WebConfigSettings.AllowForcingPreferredHostName)
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			var useHttps = siteCount > 0 &&
				SiteUtils.SslIsAvailable(siteSettings) ||
				siteCount == 0 &&
				WebConfigSettings.SslisAvailable;
			var protocol = useHttps ? "https://" : "http://";
			string redirectUrl = null;
			bool doRedirect = false;

			if (useHttps)
			{
				switch (Request.Url.Scheme)
				{
					case "https":
						if (WebConfigSettings.UseHSTSHeader)
						{
							Response.AddHeader("Strict-Transport-Security", WebConfigSettings.HSTSHeaders);
						}

						break;

					case "http":
						doRedirect = true;
						redirectUrl = $"https://{Request.Url.Host}{Request.Url.PathAndQuery}";

						break;
				}
			}

			if (siteSettings is not null && !string.IsNullOrWhiteSpace(siteSettings.PreferredHostName))
			{
				var requestedHostName = WebUtils.GetHostName();

				if (siteSettings.PreferredHostName != requestedHostName)
				{
					doRedirect = true;

					var serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

					if (!string.IsNullOrWhiteSpace(serverPort) && (serverPort == "80" || serverPort == "443"))
					{
						serverPort = string.Empty;
					}
					else
					{
						serverPort = $":{serverPort}";
					}

					if (WebConfigSettings.RedirectToRootWhenEnforcingPreferredHostName)
					{
						redirectUrl = protocol + siteSettings.PreferredHostName + serverPort;
					}
					else
					{
						redirectUrl = protocol + siteSettings.PreferredHostName + serverPort + Request.RawUrl;
					}

					if (WebConfigSettings.LogRedirectsToPreferredHostName)
					{
						log.Info($"received a request for hostname {requestedHostName}{serverPort}{Request.RawUrl}, redirecting to preferred host name {redirectUrl}");
					}
				}
			}

			if (doRedirect)
			{
				if (WebConfigSettings.Use301RedirectWhenEnforcingPreferredHostName)
				{
					Response.Status = "301 Moved Permanently";
					Response.AddHeader("Location", redirectUrl);
					Response.Cache.SetNoStore();
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
					Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

					return;
				}

				Response.Redirect(redirectUrl, true);

				return;
			}
		}
	}


	private void PurgeOldLogEvents()
	{
		if (!WebConfigSettings.UseSystemLogInsteadOfFileLog) { return; }
		if (!WebConfigSettings.SystemLogDeleteOldEventsOnApplicationStart) { return; }

		try
		{
			DateTime cutoffDate = DateTime.UtcNow.AddDays(-WebConfigSettings.SystemLogApplicationStartDeleteOlderThanDays);

			SystemLog.DeleteOlderThan(cutoffDate);
		}
		catch (Exception ex)
		{
			log.Error(ex);
		}
	}


	private void StartOrResumeTasks()
	{
		// NOTE: In IIS 7 using integrated mode, HttpContext.Current will always be null in Application_Start
		// http://weblogs.asp.net/jgaylord/archive/2008/09/04/iis7-integrated-mode-and-global-asax.aspx
		if (WebConfigSettings.UseAppKeepAlive)
		{
			AppKeepAliveTask keepAlive;
			try
			{
				try
				{
					if ((HttpContext.Current is not null) && (HttpContext.Current.Request is not null))
					{
						keepAlive = new AppKeepAliveTask
						{
							UrlToRequest = WebUtils.GetSiteRoot(),
							MaxRunTimeMinutes = WebConfigSettings.AppKeepAliveMaxRunTimeMinutes,
							MinutesToSleep = WebConfigSettings.AppKeepAliveSleepMinutes
						};
						keepAlive.QueueTask();
					}
				}
				catch (HttpException)
				{
					//this error will be thrown when using IIS 7 Integrated pipeline mode
					//since we have no context.Request to get the site root, in IIS 7 Integrated pipeline mode
					//we need to use an additional config setting to get the url to request for keep alive 
					if (WebConfigSettings.AppKeepAliveUrl.Length > 0)
					{
						keepAlive = new AppKeepAliveTask
						{
							UrlToRequest = WebConfigSettings.AppKeepAliveUrl,
							MaxRunTimeMinutes = WebConfigSettings.AppKeepAliveMaxRunTimeMinutes,
							MinutesToSleep = WebConfigSettings.AppKeepAliveSleepMinutes
						};
						keepAlive.QueueTask();
					}
				}
			}
			catch (Exception ex)
			{
				// if a new installation the table will not exist yet so just log and swallow
				log.Error(ex);
			}
		}

		WebTaskManager.StartOrResumeTasks(true);
	}


	private void RegisterVirtualPathProvider()
	{
		// had to move this into its own method
		// in less than full trust it blows up even with a try catch if present in 
		// Application_Start, moving into a separate method works with a try catch
		HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());
		RegisteredVirtualThemes = true;
	}


	private static Guid GetFileSystemToken()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var siteGuid = Guid.NewGuid();

		if (siteSettings is not null)
		{
			siteGuid = siteSettings.SiteGuid;
		}

		var cacheKey = "fileSystemToken" + siteGuid;
		var absoluteExpiration = DateTime.Now.AddHours(1);

		try
		{
			var g = CacheManager.Cache.Get(cacheKey, absoluteExpiration, () =>
			{
				// This is the anonymous function which gets called if the data is not in the cache.
				// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.

				return Guid.NewGuid().ToString();
			});

			return new Guid(g);
		}
		catch (Exception ex)
		{
			log.Error("failed to get fileSystemToken from cache", ex);

			return Guid.NewGuid();
		}
	}


	private void IncrementUserCount()
	{
		var key = WebUtils.GetHostName() + "_onlineCount";

		if (Session != null)
		{
			Session["onlinecountkey"] = key;
		}

		if (debugLog)
		{
			log.Debug($"IncrementUserCount key was {key}");
		}

		Application.Lock();
		Application[key] = Application[key] == null ? 1 : (int)Application[key] + 1;
		Application.UnLock();
	}


	private void DecrementUserCount()
	{
		if (Session != null && Session["onlinecountkey"] != null)
		{
			var key = Session["onlinecountkey"].ToString();

			if (key.Length > 0)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("DecrementUserCount key was " + key);
				}

				Application.Lock();

				var newCount = Application[key] == null ? 0 : (int)Application[key] - 1;

				Application[key] = newCount > 0 ? newCount : 0;
				Application.UnLock();
			}
		}
	}

	#endregion
}
