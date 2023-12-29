using System.IO;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers;

/// <summary>
/// Base controller
/// </summary>
public class BaseController : Controller
{
	public BaseController() { }
	/// <summary>
	/// Render partial view to string
	/// </summary>
	/// <returns>Result</returns>
	public virtual string RenderPartialToString()
	{
		return RenderPartialToString(null, null);
	}
	/// <summary>
	/// Render partial view to string
	/// </summary>
	/// <param name="viewName">View name</param>
	/// <returns>Result</returns>
	public virtual string RenderPartialToString(string viewName)
	{
		return RenderPartialToString(viewName, null);
	}
	/// <summary>
	/// Render partial view to string
	/// </summary>
	/// <param name="model">Model</param>
	/// <returns>Result</returns>
	public virtual string RenderPartialToString(object model)
	{
		return RenderPartialToString(null, model);
	}
	/// <summary>
	/// Render partial view to string
	/// </summary>
	/// <param name="viewName">View name</param>
	/// <param name="model">Model</param>
	/// <returns>Result</returns>
	public virtual string RenderPartialToString(string viewName, object model)
	{
		//Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
		if (string.IsNullOrEmpty(viewName))
		{
			viewName = ControllerContext.RouteData.GetRequiredString("action");
		}

		ViewData.Model = model;

		using var sw = new StringWriter();
		ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
		var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
		viewResult.View.Render(viewContext, sw);

		return sw.GetStringBuilder().ToString();
	}
}