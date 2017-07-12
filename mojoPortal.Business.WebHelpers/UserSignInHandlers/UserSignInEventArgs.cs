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

using System;

namespace mojoPortal.Business.WebHelpers.UserSignInHandlers
{
    public delegate void UserSignInEventHandler(object sender, UserSignInEventArgs e);

    /// <summary>
    ///  
    /// </summary>
    public class UserSignInEventArgs : EventArgs
    {
        private SiteUser _siteUser = null;

        public SiteUser SiteUser
        {
            get { return _siteUser; }
        }

        public UserSignInEventArgs(SiteUser siteUser)
        {
            _siteUser = siteUser;
        }
    }
}
