///	Created:			    2005-03-24
///	Last Modified:		    2012-06-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    
    public class RegisterLink : WebControl
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

        private string overrideUrl = string.Empty;
        /// <summary>
        /// allows linking to a custom registration page
        /// can also be done from Web.config with <add key="CustomRegistrationPage" value="/path/to/your/customregistration.aspx"/>
        /// but the setting on this control takes precedence
        /// </summary>
        public string OverrideUrl
        {
            get { return overrideUrl; }
            set { overrideUrl = value; }
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


        private bool autoDetectReturnUrl = true;

        public bool AutoDetectReturnUrl
        {
            get { return autoDetectReturnUrl; }
            set { autoDetectReturnUrl = value; }
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
            if (CssClass.Length == 0) CssClass = "sitelink";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings == null) || (!siteSettings.AllowNewRegistration) || (siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap)) { return; }
            if (Page.Request.IsAuthenticated) { return; }

            string siteRoot;
            if (SiteUtils.SslIsAvailable())
            {
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            }
            else
            {
                siteRoot = SiteUtils.GetRelativeNavigationSiteRoot();
            }

            string urlToUse = siteRoot + GetUrlToUse();

            if (siteSettings.DisableDbAuth)
            {
                if ((!siteSettings.AllowOpenIdAuth) && (!siteSettings.AllowWindowsLiveAuth)) { return; } //everything is disabled so render nothing

                if (!siteSettings.AllowOpenIdAuth)
                {
                    urlToUse = siteRoot + "/Secure/RegisterWithWindowsLiveID.aspx";
                }

                if ((!siteSettings.AllowWindowsLiveAuth) && (siteSettings.RpxNowApiKey.Length == 0))
                {
                    urlToUse = siteRoot + "/Secure/RegisterWithOpenID.aspx";
                }
            }

            if (UseLeftSeparator) writer.Write("<span class='accent'>|</span> ");

            if (renderAsListItem)
            {
                //writer.Write("<li class='" + listItemCSS + "'>");
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }



            if (SiteUtils.SslIsAvailable())
            {
                urlToUse = urlToUse.Replace("http:", "https:");
            }

            string returnUrlParam = string.Empty;

            if ((autoDetectReturnUrl)&&(!(Page is mojoPortal.Web.UI.Pages.Register)))
            {
                returnUrlParam = Page.Server.UrlEncode(Page.Request.RawUrl);
            }
            else
            {
                returnUrlParam = Page.Request.Params.Get("returnurl");
                returnUrlParam = SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam));
                if((Page is mojoPortal.Web.UI.Pages.Register))
                {
                    // no need to render it on register page
                    returnUrlParam = string.Empty;
                }

                if ((Page is mojoPortal.Web.UI.Pages.ConfirmRegistration))
                {
                    // no need to render it on register page
                    returnUrlParam = string.Empty;
                }
            }


            if (!string.IsNullOrEmpty(returnUrlParam))
            {
                urlToUse += "?returnurl=" + returnUrlParam;
            }

            //writer.Write(string.Format(
            //                 " <a href='{0}' title='{1}' class='"
            //                 + CssClass + "'>{1}</a>",
            //                 Page.ResolveUrl(urlToUse),
            //                 Resource.RegisterLink));

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);

            if (useRelNoFollow)
            {
                writer.WriteAttribute("rel", "nofollow");
            }

            //writer.WriteAttribute("title", Resource.RegisterLink);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            if (overrideText.Length > 0)
            {
                writer.WriteEncodedText(overrideText);
            }
            else
            {
                writer.WriteEncodedText(Resource.RegisterLink);
            }
            writer.WriteEndTag("a");



            if (renderAsListItem) writer.WriteEndTag("li");

        }

        private string GetUrlToUse()
        {
            string result = "/Secure/Register.aspx";
            if (WebConfigSettings.CustomRegistrationPage.Length > 0)
            {
                result = WebConfigSettings.CustomRegistrationPage;
            }

            if(overrideUrl.Length > 0)
            {
                result = overrideUrl;
            }

            return result;
        }

    }
}
