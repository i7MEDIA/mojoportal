//	Created:			    2011-06-08
//	Last Modified:		    2011-06-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.		

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    /// primary purpose of this page is a few things where we need to set a cookie and redirect
    /// for example to allow a mobile phone user to choose if they want to view the site using the main skin instead of the mobile skin
    /// 
    /// </summary>
    public partial class RedirectPage : Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessSkinCookie();
            Response.Redirect(SiteUtils.GetNavigationSiteRoot() + "/Default.aspx"); // the default.aspx is needed to work with folder based child sites
        }

        private void ProcessSkinCookie()
        {
            //toggle
            if (SiteUtils.IsMobileDevice())
            {
                if (CookieHelper.CookieExists(SiteUtils.MobileUseFullViewCookieName))
                {
                    CookieHelper.ExpireCookie(SiteUtils.MobileUseFullViewCookieName);
                }
                else
                {
                    CookieHelper.SetCookie(SiteUtils.MobileUseFullViewCookieName, "y");
                }
            }
            else
            {
                if (CookieHelper.CookieExists(SiteUtils.NonMobileUseMobileViewCookieName))
                {
                    CookieHelper.ExpireCookie(SiteUtils.NonMobileUseMobileViewCookieName);
                }
                else
                {
                    CookieHelper.SetCookie(SiteUtils.NonMobileUseMobileViewCookieName, "y");
                }
            }
        }
    }
}