using mojoPortal.Business;
using mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;
using mojoPortal.Web.Caching;

namespace mojoPortal.Web.Components.EventHandlers.SiteCreated;

public class UpdateSiteCountCacheEventHandler : SiteCreatedEventHandlerProvider
{
	public override void SiteCreatedHandler(object sender, SiteCreatedEventArgs e)
	{
		CacheManager.Cache.InvalidateCacheItem("SiteCount");
	}
}
