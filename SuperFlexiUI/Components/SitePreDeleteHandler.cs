/// Author:                     i7MEDIA
/// Created:                    2015-03-05
///	Last Modified:              2015-03-05
/// You must not remove this notice, or any other, from this software.
///  

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using SuperFlexiBusiness;

namespace SuperFlexiUI
{
    public class SitePreDeleteSuperFlexiHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SitePreDeleteSuperFlexiHandler));

        public SitePreDeleteSuperFlexiHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {

            SiteSettings siteSettings = new SiteSettings(siteId);
            ItemFieldValue.DeleteBySite(siteSettings.SiteGuid);
            Item.DeleteBySite(siteSettings.SiteGuid);
            Field.DeleteBySite(siteSettings.SiteGuid);
        }
    }
}
