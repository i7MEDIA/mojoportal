//  Author:                     
//  Created:                    2008-11-12
//	Last Modified:              2008-11-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using log4net;

namespace mojoPortal.Business.WebHelpers
{
    /// <summary>
    ///  
    /// </summary>
    public class DoNothingSitePreDeleteHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingSitePreDeleteHandler));

        public DoNothingSitePreDeleteHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            
            // do nothing
            log.Debug("DoNothingSitePreDeleteHandler called for site " + siteId.ToString(CultureInfo.InvariantCulture));
        }
    }
}
