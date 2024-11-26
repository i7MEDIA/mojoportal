//  Author:                     /Huw Reddick

using log4net;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;

public class DoNothingProfileUpdatedHandler : ProfileUpdatedHandlerProvider
{
	private static readonly ILog log = LogManager.GetLogger(typeof(DoNothingProfileUpdatedHandler));

	public DoNothingProfileUpdatedHandler() { }

	public override void ProfileUpdatedHandler(object sender, ProfileUpdatedEventArgs e)
	{
		if (e == null) return;
		if (e.SiteUser == null) return;

		// do nothing
		if (e.UpdatedByAdmin)
		{
			log.Debug($"DoNothingProfileUpdatedHandler called - an admin updated user {e.SiteUser.Email}");
		}
		else
		{
			log.Debug($"DoNothingProfileUpdatedHandler called for user {e.SiteUser.Email}");
		}
	}
}
