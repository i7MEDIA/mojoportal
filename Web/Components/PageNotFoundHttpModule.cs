using System;
using System.Web;
using log4net;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web;

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

		// ReSharper disable once RedundantJumpStatement
		if (!(sender is HttpApplication app)) return;

		//HttpApplication app = sender as HttpApplication;

		//// ReSharper disable once RedundantJumpStatement
		//if (app == null) return;
	}

	void app_Error(object sender, EventArgs e)
	{
		if (!(sender is HttpApplication app)) { return; }

		// don't handle 404 errors for images, javascript files, and web services

		var fileExtToSkip = WebConfigSettings.ExtensionsToSkipIn404Handler.SplitOnCharAndTrim('|');

		foreach (var ext in fileExtToSkip)
		{
			if (app.Request.Path.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}
		}

		//if (
		//             (app.Request.Path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase))
		//                 || (app.Request.Path.EndsWith(".ashx", StringComparison.InvariantCultureIgnoreCase))
		//                 )
		//         {
		//             // don't handle 404 errors for images and javascript files and web services
		//             return;

		//         }

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
				if (((HttpException)ex).GetHttpCode() == 404)
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

					if (WebConfigSettings.Log404Errors)
						log.Info("Referrer(" + exceptionReferrer + ")  PageNotFoundHttpModule handled error.", ex);

					app.Server.ClearError();

					// this solves the IIS 7 issue where the standard 404 page was returned
					//http://www.west-wind.com/weblog/posts/745738.aspx
					app.Context.Response.TrySkipIisCustomErrors = true;

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
					if (WebConfigSettings.Log404HandlerExceptions)
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

	public void Dispose() { }
}
