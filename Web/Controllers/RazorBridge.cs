using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using log4net;
using mojoPortal.Web.Controllers;


//Reference: http://stackoverflow.com/a/1074061/626911
namespace mojoPortal.Web.Components
{
	public class RazorBridge
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(RazorBridge));

		private static HttpContextWrapper httpCtx;
		private static RouteData rt;
		private static ControllerContext ctx;
		private static ViewEngineResult veResult;
		private static IView view;
		private static ViewContext vctx;


		private static void PrepareContexts(string partialName, object model, string controller, ViewDataDictionary viewData)
		{
			//get a wrapper for the legacy WebForm context
			httpCtx = new HttpContextWrapper(HttpContext.Current);

			//create a mock route that points to the empty controller
			rt = new RouteData();
			rt.Values.Add("controller", controller);

			Controller theController = new BaseController();

			theController.ViewData = viewData;

			//create a controller context for the route and http context
			ctx = new ControllerContext(new RequestContext(httpCtx, rt), theController);

			//find the partial view using the viewengine
			veResult = ViewEngines.Engines.FindPartialView(ctx, partialName);

			if (veResult != null)
			{
				view = veResult.View;
			}

			//create a view context and assign the model
			if (view != null)
			{
				vctx = new ViewContext(
					ctx, view,
					new ViewDataDictionary { Model = model },
					new TempDataDictionary(),
					httpCtx.Response.Output
				);
			}
		}


		public static void ReleaseView(string viewName, string controller = "BaseController")
		{
			PrepareContexts(viewName, new { }, controller, new ViewDataDictionary());

			if (veResult != null)
			{
				veResult.ViewEngine.ReleaseView(ctx, view);
			}
		}


		public static void FindPartialView(string partialName, object model, string controller = "BaseController")
		{
			PrepareContexts(partialName, model, controller, new ViewDataDictionary());

			mojoViewEngine mve = new mojoViewEngine();
			veResult = mve.FindPartialView(ctx, partialName, false); //find view without cache
		}


		public static void RenderPartial(string partialName, object model, string controller = "BaseController")
		{
			PrepareContexts(partialName, model, controller, new ViewDataDictionary());
			//render the partial view
			view.Render(vctx, HttpContext.Current.Response.Output);
		}

		public static string RenderPartialToString(string partialName, object model, ViewDataDictionary viewData, string controller = "BaseController")
		{
			PrepareContexts(partialName, model, controller, viewData);

			if (view == null)
			{
				PrepareContexts(partialName.Substring(0, partialName.Substring(partialName.IndexOf("--")).Length + 2), model, controller, viewData);
			}

			using (var sw = new StringWriter())
			{
				try
				{
					veResult.View.Render(vctx, sw);
				}
				catch (HttpException)
				{
					FindPartialView(partialName, model, controller);

					vctx = new ViewContext(
						ctx, veResult.View,
						new ViewDataDictionary { Model = model },
						new TempDataDictionary(),
						httpCtx.Response.Output
					);

					veResult.View.Render(vctx, sw);
				}

				return sw.GetStringBuilder().ToString();
			}
		}

		public static string RenderPartialToString(string partialName, object model, string controller = "BaseController")
		{
			var viewData = new ViewDataDictionary();
			return RenderPartialToString(partialName, model, viewData, controller);
		}


		public static string RenderPartialToString(RazorBridgePartialModel model)
		{
			try
			{
				return RenderPartialToString(model.CustomTemplate, model.Data, model.Controller);
			}
			catch (HttpException ex)
			{
				if (model.Page != null)
				{
					log.ErrorFormat(
						$"Chosen layout ({{0}}) for {model.CustomTemplate} was not found in skin {{1}}. Perhaps it is in a different skin. Error was: \n{{2}}",
						model.Data,
						SiteUtils.GetSkinBaseUrl(true, model.Page),
						ex
					);
				}
				else
				{
					log.ErrorFormat(
						$"Chosen layout ({{0}}) for {model.CustomTemplate} was not found. Perhaps it is in a different skin. Error was: \n{{1}}",
						model.Data,
						ex
					);
				}

				return RenderPartialToString(model.DefaultTemplate, model.Data, model.Controller);
			}
		}
	}


	public class RazorBridgePartialModel
	{
		public string Controller { get; set; }
		public string CustomTemplate { get; set; }
		public string DefaultTemplate { get; set; }
		public object Data { get; set; }
		public Page Page { get; set; } = null;
	}
}
