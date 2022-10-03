using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Features
{
	public class SitePreDeleteFeedsHandler : SitePreDeleteHandlerProvider
	{
		public SitePreDeleteFeedsHandler()
		{ }


		public override void DeleteSiteContent(int siteId)
		{
			RssFeed.DeleteBySite(siteId);
		}
	}
}
