//	Created:			    2012-04-23
//	Last Modified:		    2012-04-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Renders literal text of the Company Name from Site Settings
    /// example usage: <portal:CompanyNameLiteral id="fax1" runat="server" />
    /// </summary>
    public class CompanyNameLiteral : Literal
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            Text = siteSettings.CompanyName;

        }
    }
}