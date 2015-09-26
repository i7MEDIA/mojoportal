//	Created:			    2010-01-03
//	Last Modified:		    2010-01-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class DeliciousLink : HyperLink
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string currentUrl = Page.Server.UrlEncode(SiteUtils.GetNavigationSiteRoot() + Page.Request.RawUrl);
            NavigateUrl = "http://delicious.com/save?url=" + currentUrl + "&title=" + Page.Server.UrlEncode(Page.Title);
        }
    }
}
