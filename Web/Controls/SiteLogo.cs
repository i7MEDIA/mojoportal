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
		public bool UseH1 { get; set; } = false;
		public string OverrideUrl { get; set; } = string.Empty;
		public string OverrideTitle { get; set; } = string.Empty;
		public string OverrideImageUrl { get; set; } = string.Empty;
		public string ImageCssClass { get; set; } = "sitelogo";
		public string LinkCssClass { get; set; } = string.Empty;
		public string H1CssClass { get; set; } = "sitelogo";
		public bool UseUrl { get; set; } = true;

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
                string titleToUse = siteSettings.SiteName.Replace("\"","'");
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

                if (UseH1)
                {
                    writer.Write("<h1 class='{0}'>", H1CssClass);
                }

                if (OverrideUrl.Length > 0)
                {
                    if (siteSettings.SiteFolderName.Length > 0)
                    {
                        OverrideUrl = siteRoot + OverrideUrl.Replace("~/", "/");
                    }
                    urlToUse = OverrideUrl;
                }

                if (OverrideImageUrl.Length > 0)
                {
                    imageUrlToUse = Page.ResolveUrl(OverrideImageUrl);
                }

                if (OverrideTitle.Length > 0) titleToUse = OverrideTitle;
                //if (cssClass == string.Empty) cssClass = "sitelogo";
                if (UseUrl)
                {
                    writer.Write($"<a href=\"{Page.ResolveUrl(urlToUse)}\" title=\"{titleToUse}\" class=\"{LinkCssClass}\"><img class=\"{ImageCssClass}\" alt=\"{titleToUse}\" src=\"{imageUrlToUse}\" /></a>");
                }
                else
                {
                    writer.Write($"<img class=\"{ImageCssClass}\" alt=\"{titleToUse}\" src=\"{imageUrlToUse}\" />");
                }

                if (UseH1)
                {
                    writer.Write("</h1>");
                }
            }

        }
    }
}
