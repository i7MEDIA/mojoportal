//  Author:                     
//  Created:                    2008-06-27
//	Last Modified:              2008-06-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;

namespace mojoPortal.Business.WebHelpers.PageEventHandlers
{
    /// <summary>
    /// The only purpose of this class is because there must be at least one
    /// provider in a provider collection
    ///  
    /// </summary>
    public class DoNothingPageCreatedEventHandler : PageCreatedEventHandlerPovider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingPageCreatedEventHandler));

        public DoNothingPageCreatedEventHandler()
        { }

        public override void PageCreatedHandler(object sender, PageCreatedEventArgs e)
        {
            if (sender == null) return;

            PageSettings page = sender as PageSettings;
            // do nothing
            log.Debug("DoNothingPageCreatedEventHandler handled PageCreated event for " + page.PageName);
        }
    }
}
