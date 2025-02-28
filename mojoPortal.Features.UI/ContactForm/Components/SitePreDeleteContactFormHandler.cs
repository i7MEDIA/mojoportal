using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Features;

public class SitePreDeleteContactFormHandler : SitePreDeleteHandlerProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SitePreDeleteContactFormHandler));

	public SitePreDeleteContactFormHandler()
	{ }

	public override void DeleteSiteContent(int siteId)
	{
		ContactFormMessage.DeleteBySite(siteId);
	}
}
