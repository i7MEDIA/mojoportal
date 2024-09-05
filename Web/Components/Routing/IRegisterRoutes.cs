using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.Web.Routing;

public interface IRegisterRoutes
{
	void Register(HttpConfiguration config);
	void RegisterRoutes(RouteCollection routes);
	void RegisterGlobalFilters(GlobalFilterCollection filters);
}