///	Created:			    2007-04-26
///	Last Modified:		    2007-11-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    
    public class AccessibilityCynthiaValidatorLink : WebControl
    {


        protected override void Render(HtmlTextWriter writer)
        {

            string urlToUse = "http://www.contentquality.com/mynewtester/cynthia.exe?Url1="
                + Page.Server.UrlEncode(WebUtils.GetSiteRoot()) ;


            writer.Write(string.Format(CultureInfo.InvariantCulture,
                             "<a href='{0}' class='"
                             + CssClass
                             + "' title='Check the accessibility of this page according to U.S. Section 508'>508</a>",
                             urlToUse));
        }

    }
}