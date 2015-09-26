//  Author:                     Joe Davis
//  Created:                    2012-04-02
//	Last Modified:              2012-04-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;

namespace mojoPortal.Business.WebHelpers.SiteCreatedEventHandlers
{
    /// <summary>
    /// The only purpose of this class is because there must be at least one
    /// provider in a provider collection
    ///  
    /// </summary>
    public class DoNothingSiteCreatedEventHandler : SiteCreatedEventHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingSiteCreatedEventHandler));

        public DoNothingSiteCreatedEventHandler()
        { }

        public override void SiteCreatedHandler(object sender, SiteCreatedEventArgs e)
        {
            if (e.Site == null) { return; }
            //if (sender == null) return;

            
            // do nothing
            log.Debug("DoNothingSiteCreatedEventHandler handled SiteCreated event for " + e.Site.SiteName);
        }
    }
}
