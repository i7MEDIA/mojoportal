using System.Web;

namespace Brettle.Web.NeatHtml;

public class Helpers
{
	public static string ApplyAppPathModifier(string url)
	{
		var appPath = HttpContext.Current.Request.ApplicationPath ?? string.Empty;
		if (appPath == "/")
		{
			appPath = string.Empty;
		}
		var requestUrl = HttpContext.Current.Request.RawUrl;
		var result = HttpContext.Current.Response.ApplyAppPathModifier(url);

		// Workaround Mono XSP bug where ApplyAppPathModifier() doesn't add the session id
		if (requestUrl.StartsWith($"{appPath}/(") && !result.StartsWith($"{appPath}/("))
		{
			if (url.StartsWith("/") && url.StartsWith(appPath))
			{
				url = $"~{url.Remove(0, appPath.Length)}";
			}
			if (url.StartsWith("~/"))
			{
				var compsOfPathWithinApp = requestUrl.Substring(appPath.Length).Split('/');
				url = $"{appPath}/{compsOfPathWithinApp[1]}/{url.Substring(2)}";
			}
			result = url;
		}
		return result;
	}
}
