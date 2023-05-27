// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// 
// 3/13/2005  added handler in Application_BeginRequest 
// for db404 error which is raised if pageid doesn't exist for siteid	
// 
// 6/22/2005  added log4net error logging	
// 11/30/2005
// 1/16/2006 JA added VirtualPathProvider
// 1/29/2006 added Windows Auth support from Haluk Eryuksel
// 2/4/2006  added mojoSetup 
// 11/8/2006  added tracking user activity time in Application_EndRequest
// 12/3/2006 added tracking of session count
// 1/29/2007 added upgrade check to error handling
// 2/9/2007 added rethrow unhandled error
// 3/15/2007 refactor usercount increment
// 2007/04/26 swap Principal in authenticate request
// 2007-08-04 removed upgrade logic, its all done in Setup/Default.aspx now
// 2007-09-20 added option to force a specific culture
// 2009-06-24 some cleanup
// 2009-11-20 use config settings for keepalivetask settings
// 2011-03-14 added logic for .NET 4 to enable memory and excepton monitoring
// 2011-08-05  refactored end request user activity tracking
// 2014-07-11 added updated routing for web api and mvc
// 2019-04-04 SystemInfoCaching
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.App_Start;
using mojoPortal.Web.Caching;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Optimization;
using mojoPortal.Web.Routing;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security;
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

namespace mojoPortal.Web
{
	//http://haacked.com/archive/2010/05/16/three-hidden-extensibility-gems-in-asp-net-4.aspx/
	//public class WebInitializer
	//{
	//    public static void Initialize()
	//    {
	//        // Whatever can we do here?
	//    }

	//}

	public class Global : HttpApplication 
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Global));

		private static bool debugLog = log.IsDebugEnabled;

		public static bool RegisteredVirtualThemes { get; private set; } = false;


		public static SkinConfigManager SkinConfigManager { get; private set; }
		public static SkinConfig SkinConfig { get; private set; }

		public static Dictionary<string,int> SiteHostMap { get; } = new Dictionary<string,int>();

		// this changes everytime the app starts and the token is required when calling /Services/FileService.ashx
		// to help mitigate against xsrf attacks
		//private static Guid fileSystemToken = Guid.NewGuid();

		public static Guid FileSystemToken
		{
			get 
			{ 
				//2012-04-25 changed from application variable to cached item
				// since application variable won't work well in a web farm
				// return fileSystemToken; 
				// still this will be a problem in a small cluster if not using a distributed cache shared by the nodes
				return GetFileSystemToken();
			}
		}

		private static Guid GetFileSystemToken()
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			Guid siteGuid = Guid.NewGuid();
			if (siteSettings != null) siteGuid = siteSettings.SiteGuid;
			
			string cacheKey = "fileSystemToken" + siteGuid.ToString();
		
			DateTime absoluteExpiration = DateTime.Now.AddHours(1);

			try
			{
				string g = CacheManager.Cache.Get<string>(cacheKey, absoluteExpiration, () =>
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

		// this changes everytime the app starts and is used for rss feed autodiscovery links so it will notredirect to feedburner
		// after each app restart the variable will change so that after the user is subscribed it will begin redirecting to feedburner if using feedburner
		private static Guid feedRedirectBypassToken = Guid.NewGuid();

		public static Guid FeedRedirectBypassToken
		{
			get { return feedRedirectBypassToken; }
		}

		private const string  RequestExceptionKey = "__RequestExceptionKey";

		private static bool appDomainMonitoringEnabled = false;

		public static bool AppDomainMonitoringEnabled
		{
			get { return appDomainMonitoringEnabled; }
		}

		private static bool firstChanceExceptionMonitoringEnabled = false;

		public static bool FirstChanceExceptionMonitoringEnabled
		{
			get { return firstChanceExceptionMonitoringEnabled; }
		}

		protected void Application_Start(Object sender, EventArgs e)
		{
			try
			{
				ServicePointManager.SecurityProtocol |= ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
			}
			catch (System.NotSupportedException)
			{
				log.Error("Application_Start could not set the chosen security protocol.");
			}

			if (WebConfigSettings.EnableVirtualPathProviders)
			{
				try
				{
					log.Info(Resource.ApplicationStartEventMessage);
					RegisterVirtualPathProvider();

				}
				catch (MissingMethodException ex)
				{   // this is broken on mono, not implemented 2006-02-04
					log.Error("Application_Start Could not register VirtualPathProvider, missing method in Mono", ex);

				}
				catch (SecurityException se)
				{
					// must not be running in full trust
					log.Error("Application_Start Could not register VirtualPathProvider, this error is expected when running in Medium trust or lower", se);

				}
				catch (UnauthorizedAccessException ae)
				{
					// must not be running in full trust
					log.Error("Application_Start Could not register VirtualPathProvider, this error is expected when running in Medium trust or lower", ae);

				}
			}

			AutoMapperConfig.Configure();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

			ViewEngines.Engines.Clear();

            mojoViewEngine engine = new mojoViewEngine();
            //engine.AddViewLocationFormat("~/Data/Sites/{2}/skins/Views/{1}/{0}.cshtml");
            //engine.AddViewLocationFormat("~/Data/Sites/{2}/skins/Views/{1}/{0}.vbhtml");

            // Add a shared location too, as the lines above are controller specific
            //engine.AddPartialViewLocationFormat("~/Data/Sites/{1}/skins/Views/{0}.cshtml");
            //engine.AddPartialViewLocationFormat("~/Data/Sites/{1}/skins/Views/{0}.vbhtml");

            ViewEngines.Engines.Add(engine);

            //var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            //razorEngine.ViewLocationFormats =
            //    razorEngine.ViewLocationFormats.Concat(new string[] {
            //        "~/MyVeryOwn/{1}/{0}.cshtml",
            //        "~/MyVeryOwn/{0}.cshtml"
            //        // add other folders here (if any)
            //    }).ToArray();

            AreaRegistration.RegisterAllAreas();
            RouteRegistrar.RegisterRoutes(RouteTable.Routes);

			//System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
			//System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());
			//System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new DateTimeSiteTimeZoneBinder());


			CreateSystemInfoCache();

			StartOrResumeTasks();
			PurgeOldLogEvents();

//#if !NET35
//            appDomainMonitoringEnabled = WebConfigSettings.AppDomainMonitoringEnabled;
//            firstChanceExceptionMonitoringEnabled = WebConfigSettings.FirstChanceExceptionMonitoringEnabled;
//            try
//            {
//                SetupMonitoring();
//            }
//            catch (MethodAccessException)
//            {
//                log.Info("Failed to setup application monitoring, not allowed under medium trust.");
//                appDomainMonitoringEnabled = false;
//                firstChanceExceptionMonitoringEnabled = false;
//            }

			//#endif

//#if!NET35 && !NET40

			if (WebConfigSettings.UnobtrusiveValidationMode == "WebForms")
			{
				if(WebConfigSettings.ForceEmptyJQueryScriptReference)
				{
					// since we already have jquery loaded
					// we need to fool scriptmanager by loading a fake version here
					// to avoid an error http://stackoverflow.com/questions/12065228/asp-net-4-5-web-forms-unobtrusive-validation-jquery-issue
					ScriptResourceDefinition jQuery = new ScriptResourceDefinition();
					jQuery.Path = "~/ClientScript/empty.js"; 
					ScriptManager.ScriptResourceMapping.AddDefinition("jquery", jQuery);
				}
				
			}
			//#endif




		}

		private void CreateSystemInfoCache()
		{
			//string cacheKey = "systemInfoCache";

			//DateTime absoluteExpiration = DateTime.Now.AddHours(1);

			//try
			//{
			//	SystemInfo systemInfo = CacheManager.Cache.Get<SystemInfo>(cacheKey, absoluteExpiration, () =>
			//	{
			//		// This is the anonymous function which gets called if the data is not in the cache.
			//		// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
			//		SystemInfo info = new SystemInfo()
			//		{
			//			SiteCount = SiteSettings.SiteCount(),
			//			SiteMappings = SiteSettings.Host
			//		};
			//	});
			//}
			//catch (Exception ex)
			//{
			//	log.Error("failed to create systemInfoCache", ex);
			//}
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

		//public static void RegisterRoutes(RouteCollection routes)
		//{
		//    routes.Clear();
			
		//    RoutingHandler.Configure(routes);

		//}

//#if!NET35
//        private void SetupMonitoring()
//        {
			
//            if (appDomainMonitoringEnabled)
//            {
//                AppDomain.MonitoringIsEnabled = true;
//            }
//            if (firstChanceExceptionMonitoringEnabled)
//            {
//                AppDomain.CurrentDomain.FirstChanceException += (object source, FirstChanceExceptionEventArgs e) =>
//                {
//                    if (HttpContext.Current == null)// If no context available, ignore it
//                        return;
//                    if (HttpContext.Current.Items[RequestExceptionKey] == null)
//                        HttpContext.Current.Items[RequestExceptionKey] = new RequestException { Exceptions = new List<Exception>() };
//                    (HttpContext.Current.Items[RequestExceptionKey] as RequestException).Exceptions.Add(e.Exception);
//                };
//            }
//        }

//        private void CaptureMonitoringData()
//        {
//            if (!firstChanceExceptionMonitoringEnabled) { return; }

//            if (Context.Items[RequestExceptionKey] != null)
//            {
//                //Only add the request if atleast one exception is raised
//                var reqExc = Context.Items[RequestExceptionKey] as RequestException;
//                reqExc.Url = Request.Url.AbsoluteUri;
//                Application.Lock();
//                if (Application["AllExc"] == null)
//                    Application["AllExc"] = new List<RequestException>();
//                (Application["AllExc"] as List<RequestException>).Add(reqExc);
//                Application.UnLock();
//            }
//        }

//#endif


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
						if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
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


		protected void Application_End(Object sender, EventArgs e)
		{
			if (log.IsInfoEnabled) log.Info("------Application Stopped------");
		}

		
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			var siteCount = SiteSettings.SiteCount();

			//http://stackoverflow.com/questions/1340643/how-to-enable-ip-address-logging-with-log4net
			log4net.ThreadContext.Properties["ip"] = SiteUtils.GetIP4Address();
			log4net.ThreadContext.Properties["culture"] = CultureInfo.CurrentCulture.ToString();
			if ((HttpContext.Current != null)&&(HttpContext.Current.Request != null))
			{
				if (WebConfigSettings.LogFullUrls)
				{
					log4net.ThreadContext.Properties["url"] = HttpContext.Current.Request.Url.ToString();
				}
				else
				{
					log4net.ThreadContext.Properties["url"] = HttpContext.Current.Request.RawUrl;
				}
			}

			//http://www.troyhunt.com/2011/11/owasp-top-10-for-net-developers-part-9.html

			if ((siteCount > 0 && SiteUtils.SslIsAvailable() && WebConfigSettings.ForceSslOnAllPages) || (siteCount == 0 && WebConfigSettings.SslisAvailable))
			{
				//if we have sites (not a new install) and SSL is avail and forced, we want to force all pages to SSL
				//OR if we don't have any sites (is a new install) and SSL is avail, we want to force to SSL, which would really 
				//    only force it for default.aspx which then redirects to setup/default.aspx because there are no sites (see EnsurePageAndSite() in default.aspx)
				switch (Request.Url.Scheme)
				{
					case "https":
						if (WebConfigSettings.UseHSTSHeader)  Response.AddHeader("Strict-Transport-Security", WebConfigSettings.HSTSHeaders );
						break;

					case "http":
						string path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
						Response.Status = "301 Moved Permanently";
						Response.AddHeader("Location", path);
						Response.Cache.SetNoStore();
						Response.Cache.SetCacheability(HttpCacheability.NoCache);
						Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
						break;
				}
			}
			//moved RegisterBundles here so it can properly check the request for SSL. Can't do that when called from Application_Start
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			if (siteCount > 0 && SkinConfigManager == null)
			{
				SkinConfigManager = new SkinConfigManager();
				SkinConfig = SkinConfigManager.GetConfig();
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			

//#if!NET35
//            CaptureMonitoringData();
//#endif

			

			// this is needed otherwise tasks queued on a background thread report these values as they were on the last request handled by the thread
			// so this eliminates the carry over of these settings when a thread is re-used for something other than a web request.
			log4net.ThreadContext.Properties["ip"] = null;
			log4net.ThreadContext.Properties["culture"] = null;
			log4net.ThreadContext.Properties["url"] = null;

			if (HttpContext.Current == null) { return; }
			if (HttpContext.Current.Request == null) { return; }
			if (HttpContext.Current.Items == null) { return; }
			if (!HttpContext.Current.Request.IsAuthenticated) { return; }
			if (!WebConfigSettings.TrackAuthenticatedRequests) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".png")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".gif")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".jpg")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".jpeg")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".svg")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".css")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".axd")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".js")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".ico")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive(".ashx")) { return; }
			if (HttpContext.Current.Request.Path.ContainsCaseInsensitive("setup/default.aspx")) { return; }
			
			// update user activity at the end of each request
			// but only if the siteUser is already in the HttpContext
			// we don't want to lookup the user for little ajax requests
			// unless we have to for security checks
			if (HttpContext.Current.Items["CurrentUser"] != null)
			{
				SiteUtils.TrackUserActivity();
			}
			
		}

	   



		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			//2009-05-30, moved this functionality to /Components/AuthHandlerHttpModule.cs

			
		}


		protected void Application_AuthorizeRequest(Object sender, EventArgs e)
		{
			//if (log.IsDebugEnabled) log.Debug("Global.asax.cs Application_AuthorizeRequest" );
		}


		protected void Application_Error(Object sender, EventArgs e)
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
					log.Error(" Referrer(" + exceptionReferrer + ")", ex);
					return;
				}

				if (ex is HttpException)
				{
					exceptionIpAddress = SiteUtils.GetIP4Address();

					if (log4net.ThreadContext.Properties["ip"] == null)
					{
						log4net.ThreadContext.Properties["ip"] = exceptionIpAddress;
					}

					log.Error(exceptionIpAddress + " " + exceptionUrl + " Referrer(" + exceptionReferrer + ") useragent " + exceptionUserAgent, ex);
					return;

				}

				if (ex.Message == "File does not exist.")
				{

					log.Error(" Referrer(" + exceptionReferrer + ") useragent " + exceptionUserAgent, ex);
					return;
				}



				log.Error(" Referrer(" + exceptionReferrer + ") useragent " + exceptionUserAgent, ex);

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
									IDisposable disposable = prng as IDisposable;
									if (disposable != null) { disposable.Dispose(); }

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

		

		protected void Session_Start(Object sender, EventArgs e)
		{
			if (debugLog) log.Debug("------ Session Started ------");
			IncrementUserCount();
		}


		protected void Session_End(Object sender, EventArgs e)
		{
			if (debugLog) log.Debug("------ Session Ended ------");
			DecrementUserCount();
		}


		private void IncrementUserCount()
		{
			String key = WebUtils.GetHostName() + "_onlineCount";
			if (Session != null)
			{
				Session["onlinecountkey"] = key;
			}
			if (debugLog) log.Debug("IncrementUserCount key was " + key);

			Application.Lock();
			Application[key] = Application[key] == null ? 1 : (int)Application[key] + 1;
			Application.UnLock();
		}

		private void DecrementUserCount()
		{
			if (Session != null)
			{
				if (Session["onlinecountkey"] != null)
				{
					String key = Session["onlinecountkey"].ToString();
					if (key.Length > 0)
					{
						if (log.IsDebugEnabled) log.Debug("DecrementUserCount key was " + key);

						Application.Lock();
						int newCount = Application[key] == null ? 0 : (int)Application[key] - 1;
						Application[key] = newCount > 0 ? newCount : 0;
						Application.UnLock();
					}
				}
			}
		}
	}
}


