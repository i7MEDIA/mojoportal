using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Plugins.HtmlContent.Components;

public class SitePreDeleteHtmlHandler : SitePreDeleteHandlerProvider
{
	public SitePreDeleteHtmlHandler()
	{ }

	public override void DeleteSiteContent(int siteId)
	{
		var repository = new HtmlRepository();
		repository.DeleteBySite(siteId);
	}
}
