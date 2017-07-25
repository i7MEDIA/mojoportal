using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace mojoPortal.Web.Helpers
{
	// As seen here: https://malvinly.com/2011/02/28/using-web-forms-user-controls-in-an-asp-net-mvc-project/
	public static class UserControlHelper
	{
		public static HtmlString RenderControl<T>(this HtmlHelper helper, string path)
			where T : UserControl
		{
			return RenderControl<T>(helper, path, null);
		}

		public static HtmlString RenderControl<T>(this HtmlHelper helper, string path, Action<T> action)
			where T : UserControl
		{
			Page page = new Page();
			T control = (T)page.LoadControl(path);
			page.Controls.Add(control);

			if (action != null)
				action(control);

			using (StringWriter sw = new StringWriter())
			{
				HttpContext.Current.Server.Execute(page, sw, false);
				return new HtmlString(sw.ToString());
			}
		}
	}

	// Usage
	// =====
	//@(Html.RenderControl<MyCustomControl>("~/Test/MyCustomControl.ascx"))
	//@(Html.RenderControl<MyCustomControl>("~/Test/MyCustomControl.ascx", x => x.Name = "Malvin"))
	//@(Html.RenderControl<MyCustomControl>("~/Test/MyCustomControl.ascx", x =>
	//	{
	//		x.Name = "Malvin";
	//		x.Age = 25;
	//	}))
}