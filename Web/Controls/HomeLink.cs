///	Created:			    2007-04-13
///	Last Modified:		    2011-03-07
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
    public class HomeLink : WebControl
    {
        // these separator properties are deprecated
        // it is recommended not to use these properties
        // but instead to use mojoPortal.Web.Controls.SeparatorControl
        private bool useLeftSeparator = false;
        /// <summary>
        /// deprecated
        /// </summary>
        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

        private string overrideUrl = string.Empty;
        public string OverrideUrl
        {
            get { return overrideUrl; }
            set { overrideUrl = value; }
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

        private string anonymousListItemCSS = "firstnav";
        public string AnonymousListItemCss
        {
            get { return anonymousListItemCSS; }
            set { anonymousListItemCSS = value; }
        }

        private bool useSiteTitle = false;
        public bool UseSiteTitle
        {
            get { return useSiteTitle; }
            set { useSiteTitle = value; }
        }

        private string overrideText = string.Empty;
        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            EnableViewState = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);

           
        }

        private void DoRender(HtmlTextWriter writer)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return;

            if (renderAsListItem)
            {
                if (Page.Request.IsAuthenticated)
                {
                    //writer.Write("<li class='" + listItemCSS + "'>");
                    writer.WriteBeginTag("li");
                    writer.WriteAttribute("class", listItemCSS);
                    writer.Write(HtmlTextWriter.TagRightChar);

                }
                else
                {
                    //writer.Write("<li class='" + anonymousListItemCSS + "'>");
                    writer.WriteBeginTag("li");
                    writer.WriteAttribute("class", anonymousListItemCSS);
                    writer.Write(HtmlTextWriter.TagRightChar);
                }
            }

            if (UseLeftSeparator) writer.Write("<span class='accent'>|</span>");

            string urlToUse = SiteUtils.GetRelativeNavigationSiteRoot();

            //if ((!siteSettings.UseSslOnAllPages) && (SiteUtils.IsSecureRequest()))
            //{ 
            //    urlToUse = urlToUse.Replace("https", "http"); 
            //}

            if (CssClass.Length == 0) CssClass = "sitelink homelink";




            if (overrideUrl.Length > 0)
            {
                urlToUse = urlToUse + overrideUrl.Replace("~/", "/");

            }
            else
            {
                if (siteSettings.SiteFolderName.Length > 0)
                {
                    urlToUse += "/Default.aspx";
                }
            }

            if (urlToUse.Length == 0) { urlToUse = "/"; }

            if (useSiteTitle)
            {

                writer.WriteBeginTag("a");
                writer.WriteAttribute("class", CssClass);
                //writer.WriteAttribute("title", siteSettings.SiteName);
                writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteEncodedText(siteSettings.SiteName);
                writer.WriteEndTag("a");

            }
            else
            {


                writer.WriteBeginTag("a");
                writer.WriteAttribute("class", CssClass);
                //writer.WriteAttribute("title", Resource.HomePageLink);
                writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
                writer.Write(HtmlTextWriter.TagRightChar);
                if (overrideText.Length > 0)
                {
                    writer.WriteEncodedText(overrideText);
                }
                else
                {
                    writer.WriteEncodedText(Resource.HomePageLink);
                }
                writer.WriteEndTag("a");

            }

            if (renderAsListItem) writer.WriteEndTag("li");

        }

    }
}
