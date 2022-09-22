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
    public class LoginLink : WebControl
    {
        // these separator properties are deprecated
        // it is recommended not to use these properties
        // but instead to use mojoPortal.Web.Controls.SeparatorControl
        private bool useLeftSeparator = false;
        public bool UseLeftSeparator
        {
            get { return useLeftSeparator; }
            set { useLeftSeparator = value; }
        }

        private string leftSeparatorImageUrl = string.Empty;
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

        private string overrideText = string.Empty;
        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        private bool useRelNoFollow = true;

        public bool UseRelNoFollow
        {
            get { return useRelNoFollow; }
            set { useRelNoFollow = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;
            if (Page.Request.IsAuthenticated) { this.Visible = false; return; }
            
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
            if (Page.Request.IsAuthenticated) { return; }

               
            if (renderAsListItem)
            {
                //writer.Write("<li class='" + listItemCSS + "'>");
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            if (UseLeftSeparator)
            {
                if (leftSeparatorImageUrl.Length > 0)
                {
                    writer.Write("<img class='accent' alt='' src='" + Page.ResolveUrl(leftSeparatorImageUrl) + "' border='0' /> ");
                
                
                }
                else
                {
                    writer.Write("<span class='accent'>|</span>");
                }
            }

            string urlToUse = string.Empty;
            

            if (SiteUtils.SslIsAvailable())
            {
                urlToUse = SiteUtils.GetNavigationSiteRoot() + SiteUtils.GetLoginRelativeUrl();
                
                //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                //if ((siteSettings != null) && (siteSettings.SiteFolderName.Length > 0))
                //{
                //    urlToUse = siteSettings.SiteRoot + SiteUtils.GetLoginRelativeUrl();
                //}

                urlToUse = urlToUse.Replace("http:", "https:");
            }
            else
            {
                urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + SiteUtils.GetLoginRelativeUrl();
            }

            //for custom login page
            if (WebConfigSettings.LoginPageRelativeUrl.Length > 0)
            {
                urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + WebConfigSettings.LoginPageRelativeUrl;
            }

            PageSettings currentPage = CacheHelper.GetCurrentPage();
            if (currentPage.HideAfterLogin)
            {
                urlToUse += "?r=h";
            }
            else
            {
                if (
                    !(Page is mojoPortal.Web.UI.Pages.LoginPage)
                    && !(Page is mojoPortal.Web.UI.Pages.ConfirmRegistration)// https://github.com/i7media/mojoportal/issues/7
					&& !(Page is mojoPortal.Web.UI.Pages.RecoverPassword)
					&& !(Page is mojoPortal.Web.UI.Pages.AccessDeniedPage)
					)
                {
                    if (WebConfigSettings.PageToRedirectToAfterSignIn.Length > 0)
                    {
                        urlToUse += "?returnurl=" + Page.Server.UrlEncode(WebConfigSettings.PageToRedirectToAfterSignIn);
                    }
                    else
                    {
                        urlToUse += "?returnurl=" + Page.Server.UrlEncode(Page.Request.RawUrl);
                    }
                    
                }

                if(Page is mojoPortal.Web.UI.Pages.ConfirmRegistration)
                {
                    string returnUrlParam = Page.Request.Params.Get("returnurl");
                    if (!string.IsNullOrEmpty(returnUrlParam))
                    {
                        urlToUse += "?returnurl=" + returnUrlParam;
                        urlToUse = Page.Server.UrlEncode(urlToUse);
                    }
                }
            }

            //writer.Write(string.Format(
            //                 " <a href='{0}' title='{1}' class='"
            //                 + CssClass + "'>{1}</a>",
            //                 Page.ResolveUrl(urlToUse),
            //                 Resource.LoginLink));

            if (CssClass.Length == 0) CssClass = "sitelink";

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            if (useRelNoFollow)
            {
                writer.WriteAttribute("rel", "nofollow");
            }
            //writer.WriteAttribute("title", Resource.LoginLink);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            if (overrideText.Length > 0)
            {
                writer.WriteEncodedText(overrideText);
            }
            else
            {
                writer.WriteEncodedText(Resource.LoginLink);
            }
            writer.WriteEndTag("a");

            if (renderAsListItem) writer.WriteEndTag("li");

        }
    }
}
