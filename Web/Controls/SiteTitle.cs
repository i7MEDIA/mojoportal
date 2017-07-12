/// Author:				    
/// Created:			    2004-08-26
///	Last Modified:		    2012-09-24
/// 
/// 2007-04-13   Alexander Yushchenko: made it WebControl instead of UserControl.
/// 2007-07-05   Alexander Yushchenko: added option to render as a simple heading.
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    
    public class SiteTitle : WebControl
    {
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

        private string extraAttributes = string.Empty;
        public string ExtraAttributes
        {
            get { return extraAttributes; }
            set { extraAttributes = value; }
        }

        private bool useLink = true;
        public bool UseLink
        {
            get { return useLink; }
            set { useLink = value; }
        }

        private bool includeStandardClasses = true;
        public bool IncludeStandardClasses
        {
            get { return includeStandardClasses; }
            set { includeStandardClasses = value; }
        }


        private string element = "h1";
        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        private bool useElement = true;
        public bool UseElement
        {
            get { return useElement; }
            set { useElement = value; }
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

            string titleText = (overrideTitle.Length > 0) ? overrideTitle : siteSettings.SiteName;
            string titleMarkup;

            if (useLink)
            {
                string urlToUse;
                if (overrideTitle.Length > 0 && overrideUrl.Length > 0)
                {
                    urlToUse = (overrideUrl.StartsWith("~/") && siteSettings.SiteFolderName.Length > 0)
                                   ? SiteUtils.GetNavigationSiteRoot() + overrideUrl.Replace("~/", "/")
                                   : overrideUrl;
                }
                else
                {
                    if (siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX)
                    {
                        urlToUse = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx";
                    }
                    else
                    {
                        urlToUse = SiteUtils.GetNavigationSiteRoot();
                    }
                }
                titleMarkup = String.Format(CultureInfo.InvariantCulture, "<a class='siteheading' href='{0}'>{1}</a>",
                                            Page.ResolveUrl(urlToUse), titleText);
            }
            else
            {
                titleMarkup = titleText;
            }

            if (useElement)
            {
                writer.Write("<");
                writer.Write(element);
                if (includeStandardClasses || (CssClass.Length > 0))
                {
                    writer.Write(" class='");
                    if (includeStandardClasses)
                    {
                        writer.Write("art-Logo-name art-logo-name siteheading ");
                    }
                    writer.Write(CssClass);
                    writer.Write("'");
                }

                if (extraAttributes.Length > 0)
                {
                    writer.Write(extraAttributes);
                }

                writer.Write(">");
            }

            writer.Write(titleMarkup);

            if (useElement)
            {
                writer.Write("</");
                writer.Write(element);
                writer.Write(">");
            }

        }
        
    }
}
