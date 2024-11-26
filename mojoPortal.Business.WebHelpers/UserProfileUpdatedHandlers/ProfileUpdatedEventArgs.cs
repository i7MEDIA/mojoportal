using System;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;

public delegate void ProfileUpdateEventHandler(object sender, UserRegisteredEventArgs e);

public class ProfileUpdatedEventArgs : EventArgs
{
	public SiteUser SiteUser { get; } = null;

	public bool UpdatedByAdmin { get; } = false;

	public ProfileUpdatedEventArgs(SiteUser siteUser, bool updatedByAdmin)
	{
		SiteUser = siteUser;
		UpdatedByAdmin = updatedByAdmin;
	}
}
