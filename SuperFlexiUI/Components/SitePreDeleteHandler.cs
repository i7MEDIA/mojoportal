//using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using SuperFlexiBusiness;

namespace SuperFlexiUI;

public class SitePreDeleteSuperFlexiHandler : SitePreDeleteHandlerProvider
{
	//private static readonly ILog log = LogManager.GetLogger(typeof(SitePreDeleteSuperFlexiHandler));

	public SitePreDeleteSuperFlexiHandler()
	{ }

	public override void DeleteSiteContent(int siteId)
	{
		var siteSettings = new SiteSettings(siteId);
		ItemFieldValue.DeleteBySite(siteSettings.SiteGuid);
		Item.DeleteBySite(siteSettings.SiteGuid);
		Field.DeleteBySite(siteSettings.SiteGuid);
	}
}