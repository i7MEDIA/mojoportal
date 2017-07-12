//  Author:                     
//  Created:                    2008-07-03
//	Last Modified:              2008-07-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;

namespace mojoPortal.Business.WebHelpers.UserRegisteredHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public class DoNothingUserRegisteredHandler : UserRegisteredHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingUserRegisteredHandler));

        public DoNothingUserRegisteredHandler()
        { }

        public override void UserRegisteredHandler(object sender, UserRegisteredEventArgs e)
        {
            //if (sender == null) return;
            if (e == null) return;
            if (e.SiteUser == null) return;
            
            // do nothing
            log.Debug("DoNothingUserRegisteredHandler called for new user " + e.SiteUser.Email);
        }
    }
}
