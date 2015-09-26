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
    
    public class SilktideSiteScoreLink : WebControl
    {
        //http%3A%2F%2Fwww.mojoportal.com
        private string urlToCheck = string.Empty;
        public string UrlToCheck
        {
            get { return urlToCheck; }
            set { urlToCheck = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (urlToCheck.Length == 0) urlToCheck = WebUtils.GetSiteRoot();

            string linkUrl = "http://www.silktide.com/report.php?url="
                + Page.Server.UrlEncode(urlToCheck);

            string imageMarkup = "<img src='http://sitescore.silktide.com/index.php?siteScoreUrl="
                + Page.Server.UrlEncode(urlToCheck)
                + "alt='Silktide Sitescore for this website' style='border: 0' />";

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off")
                {
                     writer.Write(string.Format(CultureInfo.InvariantCulture,
                             "<a href='{0}' class='"
                             + CssClass
                             + "'>{1}</a>",
                             linkUrl, imageMarkup));
                }
            }


           
        }

    }
}