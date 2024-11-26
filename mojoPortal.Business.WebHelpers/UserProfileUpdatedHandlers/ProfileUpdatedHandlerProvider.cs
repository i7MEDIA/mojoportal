//  Author:                     /Huw Reddick

using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;

public abstract class ProfileUpdatedHandlerProvider : ProviderBase
{
	public abstract void ProfileUpdatedHandler(object sender, ProfileUpdatedEventArgs e);
}
