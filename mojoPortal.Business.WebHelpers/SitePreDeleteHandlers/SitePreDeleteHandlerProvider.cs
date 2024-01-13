using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers;

/// <summary>
/// Features may be installed separately from the core of mojoPortal and may have foriegn key
/// relationships that can cause errors if a site is deleted. Feature implementors should either implement
/// cascading deletes for their feature content or implement a SitePreDeleteHandlerProvider
/// that deletes all data for the feature related to the the passed in siteId.
/// These providers will be called just prior to actual site deletion.
/// 
/// It should be noted that by default site deletion is disabled in Web.config to prevent accidental
/// deletion of a site by users. It should be enabled only temporarily to delete a site on purpose then set back to diabled.
/// 
/// The provider implementations can assume that the deletion is intentional.
///  
/// </summary>
public abstract class SitePreDeleteHandlerProvider : ProviderBase
{
	public abstract void DeleteSiteContent(int siteId);
}
