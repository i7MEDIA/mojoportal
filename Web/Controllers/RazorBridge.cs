using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using mojoPortal.Web.Controllers;
using System;

//Reference: http://stackoverflow.com/a/1074061/626911
namespace mojoPortal.Web.Components
{
    public class RazorBridge
    {
        private static HttpContextWrapper httpCtx;
        private static RouteData rt;
        private static ControllerContext ctx;
        private static ViewEngineResult veResult;
        private static IView view;
        private static ViewContext vctx;

        private static void PrepareContexts(string partialName, object model, string controller)
        {
            //get a wrapper for the legacy WebForm context
            httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

            //create a mock route that points to the empty controller
            rt = new RouteData();
            rt.Values.Add("controller", controller);

            //create a controller context for the route and http context
            ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new BaseController());

            //find the partial view using the viewengine
            veResult = ViewEngines.Engines.FindPartialView(ctx, partialName);
            if (veResult != null)
            {
                view = veResult.View;
            }

            //create a view context and assign the model
            if (view != null)
            {
                vctx = new ViewContext(ctx, view,
                    new ViewDataDictionary { Model = model },
                    new TempDataDictionary(), httpCtx.Response.Output);
            }
        }

        public static void ReleaseView(string viewName, string controller = "BaseController")
        {
            PrepareContexts(viewName, new { }, controller);

            if (veResult != null)
            {
                veResult.ViewEngine.ReleaseView(ctx, view);
            }
        }

        public static void FindPartialView(string partialName, object model, string controller = "BaseController")
        {
            PrepareContexts(partialName, model, controller);

            mojoViewEngine mve = new mojoViewEngine();
            veResult = mve.FindPartialView(ctx, partialName, false); //find view without cache
        }

        public static void RenderPartial(string partialName, object model, string controller = "BaseController")
        {
            PrepareContexts(partialName, model, controller);
            //render the partial view
            view.Render(vctx, System.Web.HttpContext.Current.Response.Output);
        }

        public static string RenderPartialToString(string partialName, object model, string controller = "BaseController")
        {
            PrepareContexts(partialName, model, controller);

            if (view == null)
            {
                PrepareContexts(partialName.Substring(0, partialName.Substring(partialName.IndexOf("--")).Length + 2), model, controller);
            }

            using (var sw = new StringWriter())
            {
                try
                {
                    veResult.View.Render(vctx, sw);
                }
                catch (HttpException ex)
                {
                    FindPartialView(partialName, model, controller);
                    vctx = new ViewContext(ctx, veResult.View,     
                        new ViewDataDictionary { Model = model },
                        new TempDataDictionary(), httpCtx.Response.Output);
                    veResult.View.Render(vctx, sw);
                }

                //try
                //{
                //    viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(ctx, partialName);
                //    viewContext = new ViewContext(ctx, viewResult.View, vctx.ViewData, vctx.TempData, sw);
                //    viewResult.View.Render(viewContext, sw);

                //}
                //catch (Exception ex)
                //{
                //    if (ex is NullReferenceException || ex is HttpException)
                //    {
                //        //ReleaseView(partialName, controller);

                //        mojoViewEngine mve = new mojoViewEngine();
                //        viewResult = mve.FindPartialView(ctx, partialName, false); //find view without cache
                //        viewContext = new ViewContext(ctx, viewResult.View, vctx.ViewData, vctx.TempData, sw);
                //        viewResult.View.Render(viewContext, sw);
                //    }
                //    else
                //    {
                //        throw;
                //    }

                //}


                return sw.GetStringBuilder().ToString();
            }
        }

    }

    

    


}
