// Author:				
// Created:			    2005-06-01
// Last Modified:		2017-06-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System.Linq;
namespace mojoPortal.Web
{
	
	public class UrlRewriter : IHttpModule
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(UrlRewriter));
        private static bool debugLog = log.IsDebugEnabled;

		public void Init(HttpApplication app)
		{
			app.BeginRequest += new EventHandler(this.UrlRewriter_BeginRequest);
		}

		public void Dispose() {}

		protected  void UrlRewriter_BeginRequest(object sender, EventArgs e)
		{
            if (sender == null) return;

            HttpApplication app = (HttpApplication)sender;

			if (!WebConfigSettings.UseUrlReWritingForStaticFiles && (WebUtils.IsRequestForStaticFile(app.Request.Path)
					|| app.Request.Path.EndsWith("csshandler.ashx", StringComparison.InvariantCultureIgnoreCase)
					|| app.Request.Path.EndsWith("CaptchaImage.ashx", StringComparison.InvariantCultureIgnoreCase)
					|| app.Request.Path.EndsWith("/Data/", StringComparison.InvariantCultureIgnoreCase)
					|| app.Request.Path.StartsWith("/Data/", StringComparison.InvariantCultureIgnoreCase)))
			{
				return;
			}

            if (WebConfigSettings.UseUrlReWriting)
            {
                try
                {
                    RewriteUrl(app);
                }
                catch (InvalidOperationException ex)
                {
                    log.Error(ex);
                }
                catch (System.Data.Common.DbException ex)
                {
                    log.Error(ex);
                }
                catch (Exception ex)
                {
                    // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                    if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                    log.Error(ex);
                }
            }
		}

        private static void RewriteUrl(HttpApplication app)
        {
            if (app == null) return;

            string requestPath = app.Request.Path;
            
            bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
            string virtualFolderName;
			bool setClientFilePath = true;

			if (useFolderForSiteDetection)
            {
                virtualFolderName = VirtualFolderEvaluator.VirtualFolderName();
				if (virtualFolderName.Length > 0)
				{
					virtualFolderName = "/" + virtualFolderName;

					setClientFilePath = false;

					if (requestPath.StartsWith(virtualFolderName) && requestPath.Length > virtualFolderName.Length)
					{
						var v = requestPath.Split('/');
						var w = v.Distinct().ToArray();
						if (v.Count() > w.Count())
						{
							WebUtils.SetupRedirect(new System.Web.UI.Control(), String.Join("/", w));
							return;
						}
					}
					//requestPath = requestPath.Replace("/default.aspx", "");
					if (requestPath.EndsWith(virtualFolderName) || requestPath.EndsWith(virtualFolderName + "/"))
					{
						DoRewrite(app, realPageName: "default.aspx", setClientFilePath: setClientFilePath);
						return;
					}
				}
            }
            else
            {
                virtualFolderName = string.Empty;
            }
			
            // Remove extended information after path, such as for Web services 
            // or bogus /default.aspx/default.aspx
            string pathInfo = app.Request.PathInfo;
            if (pathInfo != string.Empty)
            {
                requestPath = requestPath.Substring(0, requestPath.Length - pathInfo.Length);
            }

            // 2006-01-25 : David Neal : Updated URL checking, Fixes for sites where mojoPortal 
            // is running at the root and for bogus default document URLs
            // Get the relative target URL without the application root
            string appRoot = WebUtils.GetApplicationRoot();

            if (requestPath.Length == appRoot.Length) { return; }

            string targetUrl = requestPath.Substring(appRoot.Length + 1);
            //if (targetUrl.Length == 0) return;
            if(StringHelper.IsCaseInsensitiveMatch(targetUrl, "default.aspx"))return;
            if (useFolderForSiteDetection)
            {
				if (!targetUrl.StartsWith("/")) targetUrl = "/" + targetUrl;

				if (targetUrl.StartsWith(virtualFolderName + "/"))
				{

                    // 2009-03-01 Kris reported a bug where folder site using /er for the folder
                    // was making an incorrect targetUrl 
                    // this url from an edit link in feed manager http://localhost/er/FeedManager/FeedEdit.aspx?mid=54&pageid=34
                    // was getting changed to http://localhost/er/FeedManagFeedEdit.aspx?mid=54&pageid=34 causig a 404
                    // caused by this commented line
                    //targetUrl = targetUrl.Replace(virtualFolderName + "/", string.Empty);
                    //fixed by changing to this
                    targetUrl = targetUrl.Remove(0, virtualFolderName.Length + 1).Replace("default.aspx", "");
					if (targetUrl.Length == 0) return;
				}
			}


			if (!WebConfigSettings.Disable301Redirector)
            {
                try
                {
                    // check if the requested url is supposed to redirect
                    string redirectUrl =string.Empty;

                    // false by default, but option to do this requested by Romaric Fabre
                    if (WebConfigSettings.IncludeParametersIn301RedirectLookup)
                    {
                        redirectUrl = GetRedirectUrl(targetUrl + "?" + app.Request.QueryString.ToString());
                    }

                    if (redirectUrl.Length == 0) { redirectUrl = GetRedirectUrl(targetUrl); }

                    if (redirectUrl.Length > 0)
                    {
                        Do301Redirect(app, redirectUrl);
                        return;
                    }
                }
                catch (NullReferenceException ex)
                {
                    // this can happen on a new installation so we catch and log it
                    log.Error(ex);
                }
            }

            
            FriendlyUrl friendlyUrl = null;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //this will happen on a new installation
            if (siteSettings == null) { return; }
            
            if (useFolderForSiteDetection && (virtualFolderName.Length > 0))
            {
                friendlyUrl = new FriendlyUrl(siteSettings.SiteId, targetUrl);
            }
            else
            {
                if (siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                {
                    //when using extensionless urls we consistently store them without a trailing slash
                    if (targetUrl.EndsWith("/"))
                    {

                        targetUrl = targetUrl.Substring(0, targetUrl.Length - 1);
                        setClientFilePath = false;
                    }
                }

                if (WebConfigSettings.AlwaysUrlEncode)
                {
                    friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), HttpUtility.UrlEncode(targetUrl));

                    //in case existing pages are not url encoded since this setting was added 2009-11-15, try again without encoding
                    
                    if (!friendlyUrl.FoundFriendlyUrl) 
                    {
                        if (WebConfigSettings.RetryUnencodedOnUrlNotFound)
                        {
                            friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), targetUrl);
                        }
                    }
                    
                }
                else
                {
                    friendlyUrl = new FriendlyUrl(WebUtils.GetHostName(), targetUrl);
                }
            }

            if (friendlyUrl == null || !friendlyUrl.FoundFriendlyUrl)
            {
                if (
                (useFolderForSiteDetection)
                && (virtualFolderName.Length > 0)
                &&(requestPath.Contains(virtualFolderName + "/"))
                )
                {
                    SiteUtils.TrackUrlRewrite();

                    //2009-03-01 same bug as above
                    //string pathToUse = requestPath.Replace(virtualFolderName + "/", string.Empty);
                    string pathToUse = requestPath.Remove(0, virtualFolderName.Length + 1);

                    // this is a flag that can be used to detect if the url was already rewritten if you need to run a custom url rewriter after this one
                    // you should only rewrite urls that were not rewritten by mojoPortal url rewriter
                    app.Context.Items["mojoPortaLDidRewriteUrl"] = true;

                    // added 2012-06-08
                    //http://stackoverflow.com/questions/353541/iis7-rewritepath-and-iis-log-files
                    //http://stackoverflow.com/questions/4061227/any-way-to-detect-classic-and-integrated-application-pool-in-code
                    //http://msdn.microsoft.com/en-us/library/system.web.httpruntime.usingintegratedpipeline%28v=vs.90%29.aspx
                    if ((SiteUtils.UsingIntegratedPipeline()) && (WebConfigSettings.UseTransferRequestForUrlReWriting))
                    {
                        string q = app.Request.QueryString.ToString();
                        if ((q.Length > 0) && (!q.StartsWith("?")))
                        {
                            q = "?" + q;
                        }
						if (!pathToUse.StartsWith("/")) pathToUse = "/" + pathToUse;
                        app.Context.Server.TransferRequest(pathToUse + q, true);

                    }
                    else
                    {
                        //previous logic
                        app.Context.RewritePath(
                            pathToUse,
                            string.Empty,
                            app.Request.QueryString.ToString(),
                            setClientFilePath);
                    }

                }
                else
                {
                    if (
                        (targetUrl.Length > 1)
                        &&(!targetUrl.Contains("."))
                        )
                    {
                        // this is a flag that will be detected in our pagenotfoundhttpmodule
                        // so we can handle 404 for extensionless urls
                        app.Context.Items["UrlNotFound"] = true;
                    }
                    return;
                }


            }

            string queryStringToUse = string.Empty;
            string realPageName = string.Empty;

            
            if (friendlyUrl.RealUrl.IndexOf('?') > 0)
            {
                realPageName = friendlyUrl.RealUrl.Substring(0, friendlyUrl.RealUrl.IndexOf('?'));
                queryStringToUse = friendlyUrl.RealUrl.Substring(friendlyUrl.RealUrl.IndexOf('?') + 1);
            }
            else // Added by Christian Fredh 10/30/2006
            {
                realPageName = friendlyUrl.RealUrl;
            }

            if (debugLog) { log.Debug("Rewriting URL to " + friendlyUrl.RealUrl); }


            if ((realPageName != null) && (!String.IsNullOrEmpty(realPageName)))
            {
                if (queryStringToUse == null)
                {
                    queryStringToUse = String.Empty;
                }

                StringBuilder originalQueryString = new StringBuilder();

                // get any additional params besides pageid
                string separator = string.Empty;
                foreach (string key in app.Request.QueryString.AllKeys)
                {
                    if (key != "pageid")
                    {
                        //originalQueryString.Append( separator + key + "="
                        //    + app.Request.QueryString.Get(key));

                        //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11718~1#post48771

                        originalQueryString.Append(separator + key + "="
                            + HttpUtility.UrlEncode(app.Request.QueryString.Get(key), Encoding.UTF8));
                        
                        if(separator.Length == 0)separator = "&";
                    }
                }

                if (originalQueryString.Length > 0)
                {
                    if (queryStringToUse.Length == 0)
                    {
                        queryStringToUse = originalQueryString.ToString();
                    }
                    else
                    {
                        queryStringToUse += "&" + originalQueryString.ToString();
                    }
                }

                DoRewrite(app, queryStringToUse, realPageName, setClientFilePath);
            }
        }

        //private static string SanitizeChildSiteFolderPath(string path, string siteFolder)
        //{
        //    //string[] siteFolders = path.Split(new string[] { "/" + path + "/" }, StringSplitOptions.None);

        //    return String.Join("/", path.SplitOnCharAndTrim('/').Distinct());
        //}

        private static void DoRewrite(HttpApplication app, string queryStringToUse = "", string realPageName = "", bool setClientFilePath = true)
        {
                SiteUtils.TrackUrlRewrite();
                //log.Info("re-writing to " + realPageName);

                // this is a flag that can be used to detect if the url was already rewritten if you need to run a custom url rewriter after this one
                // you should only rewrite urls that were not rewritten by mojoPortal url rewriter
                app.Context.Items["mojoPortaLDidRewriteUrl"] = true;

                // added 2012-06-08
                //http://stackoverflow.com/questions/353541/iis7-rewritepath-and-iis-log-files
                //http://stackoverflow.com/questions/4061227/any-way-to-detect-classic-and-integrated-application-pool-in-code
                //http://msdn.microsoft.com/en-us/library/system.web.httpruntime.usingintegratedpipeline%28v=vs.90%29.aspx
                if ((SiteUtils.UsingIntegratedPipeline()) && (WebConfigSettings.UseTransferRequestForUrlReWriting))
                {
                    if((queryStringToUse.Length > 0)&&(!queryStringToUse.StartsWith("?")))
                    {
                        queryStringToUse = "?" + queryStringToUse;
                    }
                    app.Context.Server.TransferRequest(realPageName + queryStringToUse, true);

                }
                else
                {  //previous logic
                    app.Context.RewritePath(realPageName, string.Empty, queryStringToUse, setClientFilePath);
                }
            }

        /// <summary>
        /// note the expected targetUrl and returned url are not fully qualified, but relative without a /
        /// </summary>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        private static string GetRedirectUrl(string targetUrl)
        {
            //lookup if this url is to be redirected, if found return the new url
            string newUrl = string.Empty;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            using (IDataReader reader = RedirectInfo.GetBySiteAndUrl(siteSettings.SiteId, targetUrl))
            {
                if (reader.Read())
                {
                    newUrl = reader["NewUrl"].ToString();
                }
            }

            return newUrl;
        }

        private static void Do301Redirect(HttpApplication app,  string newUrl)
        {
            //add web.config options to allow setting a cache timeout?
            //https://www.mojoportal.com/Forums/Thread.aspx?thread=9947&mid=34&pageid=5&ItemID=5&pagenumber=1#post41411

            string siteRoot = SiteUtils.GetNavigationSiteRoot();

            // false by default
            if (WebConfigSettings.AllowExternal301Redirects)
            {
                if (!newUrl.StartsWith("http"))
                {
                    newUrl = siteRoot + "/" + newUrl;
                }

            }
            else
            {
                newUrl = siteRoot + "/" + newUrl;
            }

            app.Context.Response.Status = "301 Moved Permanently";
            if (WebConfigSettings.PassQueryStringFor301Redirects)
            {
                app.Context.Response.AddHeader("Location", newUrl + app.Request.Url.Query);
            }
            else
            {
                app.Context.Response.AddHeader("Location", newUrl);
            }

            if (WebConfigSettings.DisableCacheFor301Redirects)
            {
                app.Context.Response.Cache.SetNoStore();
                app.Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                app.Context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                
            }
            else if (WebConfigSettings.SetExplicitCacheFor301Redirects)
            {
                int daysToCache = WebConfigSettings.CacheDurationInDaysFor301Redirects;
                if (daysToCache > 365) { daysToCache = 365; }
                TimeSpan cachDuration = TimeSpan.FromDays(daysToCache);
                app.Context.Response.Cache.SetCacheability(HttpCacheability.Public);
                app.Context.Response.Cache.SetExpires(DateTime.Now.Add(cachDuration));
                app.Context.Response.Cache.SetMaxAge(cachDuration);

            }

        }
	}
}
