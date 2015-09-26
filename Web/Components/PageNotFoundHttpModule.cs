// Author:				Joe Audette
// Created:			    2008-12-12
// Last Modified:		2012-05-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Configuration;
using log4net;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class PageNotFoundHttpModule : IHttpModule
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(PageNotFoundHttpModule));

        //private const string aspnet404ErrorMarker = " does not exist.";
        //private const string mono404ErrorMarker = " was not found.";
        //private const string aspnet404StackTraceMarker = "System.Web.UI.Util.CheckVirtualFileExists(VirtualPath virtualPath)";
        //private const string mono404StackTraceMarker = "at System.Web.Compilation.BuildManager.AssertVirtualPathExists";

        private const string fallBack404Content = "<html><head>Page Not Found</head><body>sorry page not found</body></html>";
        private const string webFormInitScript = "<script type=\"text/javascript\">Sys.Application.add_load(function() { var form = Sys.WebForms.PageRequestManager.getInstance()._form; form._initialAction = form.action = window.location.href; }); </script>";
        private const string openingForm = "<form name=\"aspnetForm\" method=\"post\" action=\"PageNotFound.aspx\" id=\"aspnetForm\">";
        private const string closingFormTag = "</form>";


        public void Init(HttpApplication app)
        {
            app.BeginRequest += new EventHandler(app_BeginRequest);
            app.Error += new EventHandler(app_Error);
        }

        void app_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app == null) { return; }

            if (WebConfigSettings.DetectPageNotFoundForExtensionlessUrls)
            {
                if ((app.Context.Items["UrlNotFound"] != null) && (Convert.ToBoolean(app.Context.Items["UrlNotFound"]) == true))
                {
                    log.Info("handled page not found for url " + app.Context.Request.Url.ToString());
                    if (WebConfigSettings.Custom404Page.Length > 0)
                    {
                        app.Server.Transfer(WebConfigSettings.Custom404Page);
                    }
                    else
                    {
                        app.Server.Transfer("~/PageNotFound.aspx");
                    }
                }
            }
        }

        void app_Error(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app == null) { return; }

            if (
                (app.Request.Path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase))
                    || (app.Request.Path.EndsWith(".ashx", StringComparison.InvariantCultureIgnoreCase))
                    )
            {
                // don't handle 404 errors for images and javascript files and web services
                return;

            }

            Exception ex = null;

            try
            {
                Exception rawException = app.Server.GetLastError();
                if (rawException != null)
                {
                    if (rawException.InnerException != null)
                    {
                        ex = rawException.InnerException;
                    }
                    else
                    {
                        ex = rawException;
                    }
                }

                // too bad 404 errors don't throw FileNotFoundException, this is ugly but works
                if (ex is HttpException) 
                {
                    //if (
                    //    (ex.Message.Contains(aspnet404ErrorMarker))
                    //    || (ex.Message.Contains(mono404ErrorMarker))
                    //    || (ex.StackTrace.Contains(aspnet404StackTraceMarker))
                    //    || (ex.StackTrace.Contains(mono404StackTraceMarker))
                    //)
                    if(((HttpException)(ex)).GetHttpCode() == 404)
                    {
                        string exceptionReferrer = string.Empty;

                        if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UrlReferrer != null)
                        {
                            exceptionReferrer = HttpContext.Current.Request.UrlReferrer.ToString();
                        }
                        else
                        {
                            exceptionReferrer = "none";
                        }

                        log.Error("Referrer(" + exceptionReferrer + ")  PageNotFoundHttpModule handled error.", ex);

                        app.Server.ClearError();
#if !MONO
                        // this solves the IIS 7 issue where the standard 404 page was returned
                        //http://www.west-wind.com/weblog/posts/745738.aspx
                        app.Context.Response.TrySkipIisCustomErrors = true;
#endif
                        //app.Context.Response.StatusCode = 404;
                        //app.Context.Response.Write(GetCustom404Html());
                        //app.Context.Response.End();
                        if (WebConfigSettings.Custom404Page.Length > 0)
                        {
                            app.Server.Transfer(WebConfigSettings.Custom404Page);
                        }
                        else
                        {
                            app.Server.Transfer("~/PageNotFound.aspx");
                        }
                    }
                    else
                    {
                        if (WebConfigSettings.LogErrorsFrom404Handler)
                        {
                            log.Info("PageNotFoundHttpModule ignoring error ", ex);
                        }
                    }

                }

            }
            catch (Exception ex2)
            {
                log.Info("PageNotFoundHttpModule swallowed error", ex2);
            }
        }

        

        //private string GetCustom404Html()
        //{
        //    try
        //    {
        //        if (ConfigurationManager.AppSettings["Custom404Page"] == null) { return fallBack404Content; }

        //        string custom404PageUrl = SiteUtils.GetNavigationSiteRoot() + ConfigurationManager.AppSettings["Custom404Page"];

        //        string html = WebUtils.GetHtmlFromWeb(custom404PageUrl + "?c=" + CultureInfo.CurrentCulture.Name);

        //        if (html.Contains(webFormInitScript))
        //        {
        //            html = html.Replace(webFormInitScript, string.Empty);
        //        }

        //        if (html.Contains(openingForm))
        //        {
        //            html = html.Replace(openingForm, string.Empty);
        //        }

        //        if (html.Contains(closingFormTag))
        //        {
        //            html = html.Replace(closingFormTag, string.Empty);
        //        }

        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("PageNotFoundHttpModule raised an error trying to get Custom 404 page content.", ex);

        //    }

        //    return fallBack404Content;
        //}

        

        public void Dispose() { }
    }
}
