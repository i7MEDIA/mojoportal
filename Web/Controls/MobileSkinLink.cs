//	Created:			    2011-06-08
//	Last Modified:		    2011-06-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.		

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
    public class MobileSkinLink : WebControl
    {

       
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

        private string overrideMobileText = string.Empty;
        public string OverrideMobileText
        {
            get { return overrideMobileText; }
            set { overrideMobileText = value; }
        }

        private string overrideNonMobileText = string.Empty;
        public string OverrideNonMobileText
        {
            get { return overrideNonMobileText; }
            set { overrideNonMobileText = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CssClass.Length == 0) CssClass = "sitelink mobileskinlink";
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
            if ((!WebConfigSettings.AllowMobileSkinForNonMobile) && (!SiteUtils.IsMobileDevice()))
            { 
                return; 
            } //only render for mobile devices

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            if ((siteSettings.MobileSkin.Length == 0)&&(WebConfigSettings.MobilePhoneSkin.Length == 0)) { return; }

            if (renderAsListItem)
            {
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);
            }

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            // this page will toggle themobile skin cookie and redirect to the root
            writer.WriteAttribute("href", SiteUtils.GetNavigationSiteRoot() + "/Redirect.aspx?tm=1"); //tm is for toggle mobile skin
            writer.Write(HtmlTextWriter.TagRightChar);

            bool useMobile = true;
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                useMobile = basePage.UseMobileSkin;
            }

            if (useMobile)
            {
                if (overrideNonMobileText.Length > 0)
                {
                    writer.WriteEncodedText(overrideNonMobileText);
                }
                else
                {
                    writer.WriteEncodedText(Resource.NonMobileLink);
                }
            }
            else
            {
                if (overrideMobileText.Length > 0)
                {
                    writer.WriteEncodedText(overrideMobileText);
                }
                else
                {
                    writer.WriteEncodedText(Resource.MobileSkinLink);
                }
               
            }
            writer.WriteEndTag("a");

            if (renderAsListItem) writer.WriteEndTag("li");

        }

    }
}