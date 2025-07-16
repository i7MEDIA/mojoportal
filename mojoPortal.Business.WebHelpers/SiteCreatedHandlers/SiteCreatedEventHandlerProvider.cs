using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;

public abstract class SiteCreatedEventHandlerProvider : ProviderBase
{
	public abstract void SiteCreatedHandler(object sender, SiteCreatedEventArgs e);
}
