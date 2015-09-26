//	Created:			    2010-03-25
//	Last Modified:		    2010-03-25
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    public class PrivacyPageLink : HyperLink
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage == null) { Visible = false; return; }
            if (basePage.SiteInfo == null) { Visible = false; return; }
            if (basePage.SiteInfo.PrivacyPolicyUrl.Length == 0) { Visible = false; return; }

            NavigateUrl = basePage.SiteRoot + basePage.SiteInfo.PrivacyPolicyUrl;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Text.Length == 0)
            {
                Text = Resource.PrivacyPolicy;
            }
        }
    }
}
