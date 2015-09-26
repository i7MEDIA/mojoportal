///	Created:			    2006-08-03
///	Last Modified:		    2010-06-04
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
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This is really the SiteOffice link, I would rename it but it would break existing user skins to do that.
    /// </summary>
    public class MailboxLink : WebControl
    {

        private bool useLeftSeparator = false;
        private string leftSeparatorImageUrl = string.Empty;

        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

        public string LeftSeparatorImageUrl
        {
            get { return leftSeparatorImageUrl; }
            set { leftSeparatorImageUrl = value; }
        }

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = "topnavitem";
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (!WebConfigSettings.UseSiteMailFeature) { return; }

            if ((!Page.Request.IsAuthenticated) && (!WebConfigSettings.UseSilverlightSiteOffice)) { return; }
            
            if (renderAsListItem)
            {
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            if (leftSeparatorImageUrl.Length > 0)
            {
                writer.Write("<img class='accent' src='" + Page.ResolveUrl(leftSeparatorImageUrl) + "' border='0' /> ");
            }
            else
            {
                if (UseLeftSeparator)
                {
                    writer.Write("<span class='accent'>|</span>");
                }
            }

            string urlToUse = "/SiteOffice/Default.aspx";

            if (WebConfigSettings.UseSilverlightSiteOffice)
            {
                urlToUse = "/app.aspx";
            }

            urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + urlToUse;

            //if (SiteUtils.IsSecureRequest())
            //{

            //    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //    if ((siteSettings != null) && (!siteSettings.UseSslOnAllPages) && (!WebConfigSettings.UseSslForSiteOffice))
            //    {
            //        urlToUse = urlToUse.Replace("https","http");
            //    }
            //}

            if (CssClass.Length == 0) CssClass = "sitelink";

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            //writer.WriteAttribute("title", Resource.MailboxLink);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEncodedText(Resource.MailboxLink);
            writer.WriteEndTag("a");

            if (renderAsListItem) writer.WriteEndTag("li");

                
            
        }

    }
}
