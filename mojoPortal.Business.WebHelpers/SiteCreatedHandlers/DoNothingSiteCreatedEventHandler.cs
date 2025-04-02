using log4net;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers;

/// <summary>
/// The only purpose of this class is because there must be at least one
/// provider in a provider collection
/// </summary>
public class DoNothingSiteCreatedEventHandler : SiteCreatedEventHandlerProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(DoNothingSiteCreatedEventHandler));


	public DoNothingSiteCreatedEventHandler()
	{ }


	public override void SiteCreatedHandler(object sender, SiteCreatedEventArgs e)
	{
		if (e.Site == null)
		{
			return;
		}


		// do nothing
		log.Debug($"DoNothingSiteCreatedEventHandler handled SiteCreated event for {e.Site.SiteName}");
	}
}
