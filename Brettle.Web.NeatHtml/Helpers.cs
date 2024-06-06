using System.Web;

namespace Brettle.Web.NeatHtml;

public class Helpers
{
	public static string ApplyAppPathModifier(string url)
	{
		string appPath = HttpContext.Current.Request.ApplicationPath;
		if (appPath == "/")
		{
			appPath = "";
		}
		string requestUrl = HttpContext.Current.Request.RawUrl;
		string result = HttpContext.Current.Response.ApplyAppPathModifier(url);

		// Workaround Mono XSP bug where ApplyAppPathModifier() doesn't add the session id
		if (requestUrl.StartsWith($"{appPath}/(") && !result.StartsWith($"{appPath}/("))
		{
			if (url.StartsWith("/") && url.StartsWith(appPath))
			{
				url = $"~{url.Remove(0, appPath.Length)}";
			}
			if (url.StartsWith("~/"))
			{
				string[] compsOfPathWithinApp = requestUrl.Substring(appPath.Length).Split('/');
				url = $"{appPath}/{compsOfPathWithinApp[1]}/{url.Substring(2)}";
			}
			result = url;
		}
		return result;
	}
}
