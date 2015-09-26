using System;


namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers
{
    public delegate void ProfileUpdateEventHandler(object sender, UserRegisteredEventArgs e);

    public class ProfileUpdatedEventArgs : EventArgs
    {
        private SiteUser _siteUser = null;

        public SiteUser SiteUser
        {
            get { return _siteUser; }
        }

        private bool _updatedByAdmin = false;

        public bool UpdatedByAdmin
        {
            get { return _updatedByAdmin; }
        }

        public ProfileUpdatedEventArgs(SiteUser siteUser, bool updatedByAdmin)
        {
            _siteUser = siteUser;
            _updatedByAdmin = updatedByAdmin;
        }
    }
}
