using System;


namespace mojoPortal.Business.WebHelpers
{
    public delegate void UserPreDeleteHandler(object sender, UserPreDeleteEventArgs e);

    public class UserPreDeleteEventArgs : EventArgs
    {
        private SiteUser _siteUser = null;

        public SiteUser SiteUser
        {
            get { return _siteUser; }
        }

        private bool _flaggedAsDeletedOnly = false;

        public bool FlaggedAsDeletedOnly
        {
            get { return _flaggedAsDeletedOnly; }
        }

        public UserPreDeleteEventArgs(SiteUser siteUser, bool flaggedAsDeletedOnly)
        {
            _siteUser = siteUser;
            _flaggedAsDeletedOnly = flaggedAsDeletedOnly;
        }
    }
}
