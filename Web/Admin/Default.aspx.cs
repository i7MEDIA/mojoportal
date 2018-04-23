///	Created:			    2009-04-16
///	Last Modified:		    2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.AdminUI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // this is just a redirect for /Admin/
            //using GetRelativeNavigationSiteRoot so this will work in folder child sites
            //GetNavigationSiteRoot will use CacheHelper.GetCurrentSiteSettings but if user has already been to the main site, they can sometimes get the wrong site settings AHHHH!!!!
            string siteRoot = SiteUtils.GetRelativeNavigationSiteRoot();
            string redirectUrl = string.Empty;

            if (Request.IsAuthenticated)
            {
				WebUtils.SetupRedirect(this, siteRoot + "/Admin/AdminMenu.aspx");
				return;
            }
            else
            {
				SiteUtils.RedirectToLoginPage(this, siteRoot + "/Admin/AdminMenu.aspx");
				return;
            }
        }
    }
}
