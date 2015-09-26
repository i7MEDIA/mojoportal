///	Created:			    2007-04-26
///	Last Modified:		    2009-01-04
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

namespace mojoPortal.Web.UI
{
    public class CssValidatorLink : WebControl
    {
        private bool useImage = true;

        public bool UseImage
        {
            get { return useImage; }
            set { useImage = value; }
        }

        

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (WebConfigSettings.DisableCssValidatorLink) { return; }
                
                string urlToUse = "http://jigsaw.w3.org/css-validator/validator?uri="
                    + SiteUtils.GetStyleSheetUrl(Page);

                string innerMarkup = "CSS";
                if (useImage)
                {
                    innerMarkup = "<img  src='"
                        + Page.ResolveUrl("~/Data/SiteImages/vcss.png")
                        + "' alt='Valid CSS' />";

                }

                string css = string.Empty;
                if (CssClass.Length > 0) css = " class='" + CssClass + "' ";

                writer.Write(string.Format(CultureInfo.InvariantCulture,
                                 "<a rel='nofollow' href='{0}' "
                                 + css
                                 + " title='Valid CSS'>"
                                 + innerMarkup
                                 + "</a>",
                                 urlToUse));
            }
        }

    }
}
