//  Author:                     
//  Created:                    2008-08-28
//	Last Modified:              2008-08-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;

namespace mojoPortal.Business.WebHelpers.UserSignInHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public class DoNothingUserSignInHandlerProvider : UserSignInHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingUserSignInHandlerProvider));

        public DoNothingUserSignInHandlerProvider()
        { }

        public override void UserSignInEventHandler(object sender, UserSignInEventArgs e)
        {
            if (e == null) return;
            if (e.SiteUser == null) return;
            // do nothing
            log.Debug("DoNothingUserSignInHandlerProvider called for user " + e.SiteUser.Email);
        }
    }
}
