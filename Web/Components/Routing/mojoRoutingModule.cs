using System.Web;
using System.Web.Routing;

namespace mojoPortal.Web.Routing;

public class RoutingModule : UrlRoutingModule
{
	//http://msdn.microsoft.com/en-us/library/system.web.routing.urlroutingmodule.aspx
	//http://www.salient6.com/blog/default.aspx
	public override void PostResolveRequestCache(HttpContextBase context)
	{
		base.PostResolveRequestCache(context);
	}
}
