//  Author:                     
//  Created:                    2008-11-12
//	Last Modified:              2009-03-08
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
    ///  Handles deletion html content data for a site.
    /// </summary>
    public class SitePreDeleteHtmlHandler : SitePreDeleteHandlerProvider
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SitePreDeleteHandler));

        public SitePreDeleteHtmlHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            HtmlRepository repository = new HtmlRepository();
            repository.DeleteBySite(siteId);

          
            
            
        }
    }
}
