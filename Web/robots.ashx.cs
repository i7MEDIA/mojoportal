using System.IO;
using System.Text;
using System.Web;

namespace mojoPortal.Web;

/// <summary>
/// If you route/rewrite requests for robots.txt to robots.ashx then you can use a different robots file for https
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class RobotsHandler : IHttpHandler
{

	public void ProcessRequest(HttpContext context)
	{
		RenderRobots(context);
	}

	private void RenderRobots(HttpContext context)
	{
		string robotsFile;
		if (Core.Helpers.WebHelper.IsSecureRequest())
		{
			robotsFile = context.Server.MapPath(WebConfigSettings.RobotsSslConfigFile); //robots.ssl.config by default
		}
		else
		{
			robotsFile = context.Server.MapPath(WebConfigSettings.RobotsConfigFile); //robots.config by default
		}

		if (!File.Exists(robotsFile)) return;

		context.Response.ContentType = "text/plain";
		Encoding encoding = new UTF8Encoding();
		context.Response.ContentEncoding = encoding;
		var file = new FileInfo(robotsFile);

		using StreamReader sr = file.OpenText();
		context.Response.Write(sr.ReadToEnd());

	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
}
