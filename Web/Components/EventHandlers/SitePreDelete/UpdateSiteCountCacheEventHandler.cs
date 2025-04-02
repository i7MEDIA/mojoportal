using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Caching;

namespace mojoPortal.Web.Components.EventHandlers.SitePreDelete;

public class UpdateSiteCountCacheEventHandler : SitePreDeleteHandlerProvider
{
	public override void DeleteSiteContent(int siteId)
	{
		CacheManager.Cache.InvalidateCacheItem("SiteCount");
	}
}
