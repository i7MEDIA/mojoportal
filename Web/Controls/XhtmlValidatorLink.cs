///	Created:			    2007-05-23
///	Last Modified:		    2019-01-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{

	public class XhtmlValidatorLink : WebControl
    {
		public bool UseImage { get; set; } = true;

		public string ExcludePagesCsv { get; set; } = string.Empty;

		public bool Html5 { get; set; } = false;

		protected override void Render(HtmlTextWriter writer)
        {

			// this is an unnecessary control 
			// if you want a HTML Validator link on your site, place one there with an <a href=

			return;

			//if (this.Site != null && this.Site.DesignMode)
   //         {
   //             // TODO: show a bmp or some other design time thing?
   //             writer.Write("[" + this.ID + "]");
   //         }
   //         else
   //         {
   //             if (WebConfigSettings.DisableHtmlValidatorLink) { return; }

   //             bool show = true;

   //             if (ExcludePagesCsv.Length > 0)
   //             {
   //                 string[] excludedPages = ExcludePagesCsv.Split(',');
   //                 foreach (string exclude in excludedPages)
   //                 {
   //                     if (Page.Request.RawUrl.Contains(exclude)) { show = false; }

   //                 }
   //             }


   //             if (!show) { return; }

   //             string urlToUse = "http://validator.w3.org/check?uri=referer";

   //             string innerMarkup = "XHTML 1.0";
   //             string title = "Valid XHTML 1.0 Transitional";

   //             if (Html5) 
   //             { 
   //                 innerMarkup = "HTML 5";
   //                 title = "Valid HTML 5";
   //             }

   //             if (UseImage)
   //             {
   //                 if (Html5)
   //                 {
   //                     innerMarkup = "<img  src='"
   //                         + Page.ResolveUrl("~/Data/SiteImages/valid-html5.png")
   //                         + "' alt='" + title + "' />";

   //                 }
   //                 else
   //                 {
   //                     innerMarkup = "<img  src='"
   //                         + Page.ResolveUrl("~/Data/SiteImages/valid-xhtml10.png")
   //                         + "' alt='" + title + "' />";
   //                 }

   //             }

   //             string css = string.Empty;
   //             if (CssClass.Length > 0) css = " class='" + CssClass + "' ";

   //             writer.Write(string.Format(
   //                              "<a rel='nofollow' href='{0}' "
   //                              + css
   //                              + " title='" + title + "'>"
   //                              + innerMarkup
   //                              + "</a>",
   //                              urlToUse));

                
   //         }

        }

    }
}
