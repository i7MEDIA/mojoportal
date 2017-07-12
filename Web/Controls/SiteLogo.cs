// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Author:				    
// Created:			    2004-08-26
//	
// 
// 2007/04/13   Alexander Yushchenko: code refactoring, made it WebControl instead of UserControl.
// 2012/03/16   Joe Davis: added cssClass properties to h1, link and image
// 2014-05-07 JA
// 2015/11/09   Joe Davis: added UseUrl option, default false

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public class SiteLogo : WebControl
    {
        private bool useH1 = false;

        public bool UseH1
        {
            get { return useH1; }
            set { useH1 = value; }
        }

        private string overrideUrl = string.Empty;
        public string OverrideUrl
        {
            get { return overrideUrl; }
            set { overrideUrl = value; }
        }

        private string overrideTitle = string.Empty;
        public string OverrideTitle
        {
            get { return overrideTitle; }
            set { overrideTitle = value; }
        }

        private string overrideImageUrl = string.Empty;
        public string OverrideImageUrl
        {
            get { return overrideImageUrl; }
            set { overrideImageUrl = value; }
        }

        private string imageCssClass = "sitelogo";
        public string ImageCssClass
        {
            get { return imageCssClass; }
            set { imageCssClass = value; }
        }

        private string linkCssClass = string.Empty;
        public string LinkCssClass
        {
            get { return linkCssClass; }
            set { linkCssClass = value; }
        }

        private string h1CssClass = "sitelogo";
        public string H1CssClass
        {
            get { return h1CssClass; }
            set { h1CssClass = value; }
        }

        private bool useUrl = true;
        public bool UseUrl
        {
            get { return useUrl; }
            set { useUrl = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (HttpContext.Current == null)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null || String.IsNullOrEmpty(siteSettings.Logo)) return;

                string urlToUse = "~/";
                string titleToUse = siteSettings.SiteName;
                string imageUrlToUse;

                if (WebConfigSettings.SiteLogoUseMediaFolder)
                {
                    imageUrlToUse = Page.ResolveUrl("~/Data/Sites/")
                    + siteSettings.SiteId.ToString()
                    + "/media/logos/" + siteSettings.Logo;
                }
                else
                {
                    imageUrlToUse = Page.ResolveUrl("~/Data/Sites/")
                    + siteSettings.SiteId.ToString()
                    + "/logos/" + siteSettings.Logo;
                }

                

                string siteRoot = SiteUtils.GetNavigationSiteRoot(siteSettings);

                if (siteSettings.SiteFolderName.Length > 0)
                {
                    //urlToUse = siteSettings.SiteRoot + "/Default.aspx";
                    urlToUse = siteRoot + "/Default.aspx";
                }

                if (useH1)
                {
                    writer.Write("<h1 class='{0}'>", h1CssClass);
                }

                if (overrideUrl.Length > 0)
                {
                    if (siteSettings.SiteFolderName.Length > 0)
                    {
                        overrideUrl = siteRoot + overrideUrl.Replace("~/", "/");
                    }
                    urlToUse = overrideUrl;
                }

                if (overrideImageUrl.Length > 0)
                {
                    imageUrlToUse = Page.ResolveUrl(overrideImageUrl);
                }

                if (overrideTitle.Length > 0) titleToUse = overrideTitle;
                //if (cssClass == string.Empty) cssClass = "sitelogo";
                if (useUrl)
                {
                    writer.Write("<a href='{0}' title='{1}' class='{4}'><img class='{3}' alt='{1}' src='{2}' /></a>",
                        Page.ResolveUrl(urlToUse),
                        titleToUse,
                        imageUrlToUse,
                        imageCssClass,
                        linkCssClass);
                }
                else
                {
                    writer.Write("<img class='{0}' alt='{1}' src='{2}' />",
                        imageCssClass,
                        titleToUse,
                        imageUrlToUse);
                }

                if (useH1)
                {
                    writer.Write("</h1>");
                }
            }

        }
    }
}
