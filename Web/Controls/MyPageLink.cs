///	Created:			    2006-05-20
///	Last Modified:		    2010-07-30
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    
    public class MyPageLink : WebControl
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

        private SiteSettings siteSettings;

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

#if !MONO

            if (HttpContext.Current == null) { return; }
            if (!WebConfigSettings.MyPageIsInstalled) { return; }
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            if (!siteSettings.EnableMyPageFeature) { return; }

            if (WebUser.IsInRoles(siteSettings.RolesThatCanViewMyPage))
            {
                if (renderAsListItem)
                {
                    //writer.Write("<li class='" + listItemCSS + "'>");
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

                string urlToUse;
                if (WebConfigSettings.UseSslForMyPage)
                {
                    urlToUse = SiteUtils.GetSecureNavigationSiteRoot() + "/MyPage.aspx";
                }
                else
                {
                    urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + "/MyPage.aspx";
                    //if ((SiteUtils.IsSecureRequest()) && (!siteSettings.UseSslOnAllPages))
                    //{
                    //    urlToUse = urlToUse.Replace("https", "http");
                    //}
                }

                if (CssClass.Length == 0) CssClass = "sitelink";


                writer.WriteBeginTag("a");
                writer.WriteAttribute("class", CssClass);
                //writer.WriteAttribute("title", Resource.MyPageLink);
                writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteEncodedText(Resource.MyPageLink);
                writer.WriteEndTag("a");



                if (renderAsListItem) writer.WriteEndTag("li");



            }



#endif

        }

    }
}


