//  Author:                     
//  Created:                    2009-07-22
//	Last Modified:              2009-12-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
//using log4net;

namespace mojoPortal.Web
{
    /// <summary>
    ///  Handles deletion of core data for a site.
    /// </summary>
    public class CoreDataSitePreDeleteHandler : SitePreDeleteHandlerProvider
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(CoreDataSitePreDeleteHandler));

        public CoreDataSitePreDeleteHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
           
            SiteSettings siteSettings = new SiteSettings(siteId);
            CommerceReport.DeleteBySite(siteSettings.SiteGuid);
            FileAttachment.DeleteBySite(siteSettings.SiteGuid);
            EmailSendLog.DeleteBySite(siteSettings.SiteGuid);
            EmailTemplate.DeleteBySite(siteSettings.SiteGuid);
            ContentHistory.DeleteBySite(siteSettings.SiteGuid);
            ContentWorkflow.DeleteBySite(siteSettings.SiteGuid);
           
            ContentMetaRespository metaRepository = new ContentMetaRespository();
            metaRepository.DeleteBySite(siteSettings.SiteGuid);

            
           
           
        }
    }
}
