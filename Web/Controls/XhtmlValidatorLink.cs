///	Created:			    2007-05-23
///	Last Modified:		    2009-07-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    
    public class XhtmlValidatorLink : WebControl
    {
        private bool useImage = true;

        public bool UseImage
        {
            get { return useImage; }
            set { useImage = value; }
        }

        private string excludePagesCsv = string.Empty;

        public string ExcludePagesCsv
        {
            get { return excludePagesCsv; }
            set { excludePagesCsv = value; }
        }

        private bool html5 = false;

        public bool Html5
        {
            get { return html5; }
            set { html5 = value; }
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
                if (WebConfigSettings.DisableHtmlValidatorLink) { return; }

                bool show = true;

                if (excludePagesCsv.Length > 0)
                {
                    string[] excludedPages = excludePagesCsv.Split(',');
                    foreach (string exclude in excludedPages)
                    {
                        if (Page.Request.RawUrl.Contains(exclude)) { show = false; }

                    }
                }


                if (!show) { return; }

                string urlToUse = "http://validator.w3.org/check?uri=referer";

                string innerMarkup = "XHTML 1.0";
                string title = "Valid XHTML 1.0 Transitional";

                if (html5) 
                { 
                    innerMarkup = "HTML 5";
                    title = "Valid HTML 5";
                }

                if (useImage)
                {
                    if (html5)
                    {
                        innerMarkup = "<img  src='"
                            + Page.ResolveUrl("~/Data/SiteImages/valid-html5.png")
                            + "' alt='" + title + "' />";

                    }
                    else
                    {
                        innerMarkup = "<img  src='"
                            + Page.ResolveUrl("~/Data/SiteImages/valid-xhtml10.png")
                            + "' alt='" + title + "' />";
                    }

                }

                string css = string.Empty;
                if (CssClass.Length > 0) css = " class='" + CssClass + "' ";

                writer.Write(string.Format(
                                 "<a rel='nofollow' href='{0}' "
                                 + css
                                 + " title='" + title + "'>"
                                 + innerMarkup
                                 + "</a>",
                                 urlToUse));

                
            }

        }

    }
}
