using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web;

public static class CustomHtmlHelper
{
	public static IHtmlString FormatAttribute(this HtmlHelper helper, string format = "{0}", string str = "")
	{		
		if (string.IsNullOrWhiteSpace(str))
		{
			return new MvcHtmlString(string.Empty);
		}
		return new MvcHtmlString(string.Format(format, str.Trim()));
	}
}