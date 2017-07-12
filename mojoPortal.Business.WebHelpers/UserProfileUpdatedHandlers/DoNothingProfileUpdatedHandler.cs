//  Author:                     /Huw Reddick
//  Created:                    2012-07-09
//	Last Modified:              2012-07-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using log4net;


namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public class DoNothingProfileUpdatedHandler : ProfileUpdatedHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(DoNothingProfileUpdatedHandler));

        public DoNothingProfileUpdatedHandler()
        { }

        public override void ProfileUpdatedHandler(object sender, ProfileUpdatedEventArgs e)
        {
            //if (sender == null) return;
            if (e == null) return;
            if (e.SiteUser == null) return;
            
            // do nothing
            if (e.UpdatedByAdmin)
            {
                log.Debug("DoNothingProfileUpdatedHandler called - an admin updated user " + e.SiteUser.Email);
            }
            else
            {
                log.Debug("DoNothingProfileUpdatedHandler called for user " + e.SiteUser.Email);
            }
        }

    }
}
