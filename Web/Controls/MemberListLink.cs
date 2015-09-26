///	Created:			    2005-03-24
///	Last Modified:		    2013-10-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    
    public class MemberListLink : WebControl
    {
        private SiteSettings siteSettings = null;
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

        private string overrideText = string.Empty;
        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            siteSettings = CacheHelper.GetCurrentSiteSettings();
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
            if (siteSettings == null) { return; }

            if (!WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList)) { return; }


            if (renderAsListItem)
            {
                //writer.Write("<li class='" + listItemCSS + "'>");
                writer.WriteBeginTag("li");
                writer.WriteAttribute("class", listItemCSS);
                writer.Write(HtmlTextWriter.TagRightChar);

            }

            if (UseLeftSeparator) writer.Write("<span class='accent'>|</span>");

            string urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + WebConfigSettings.MemberListUrl;
           
            if (CssClass.Length == 0) CssClass = "sitelink";

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);

    
            if (WebConfigSettings.MemberListOverrideLinkText.Length > 0)
            {
                writer.WriteAttribute("title", Page.Server.HtmlEncode(WebConfigSettings.MemberListOverrideLinkText));
            }
           
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);
            if (overrideText.Length > 0)
            {
                writer.WriteEncodedText(overrideText);
            }
            else
            {
                writer.WriteEncodedText(Resource.MemberListLink);
            }
            writer.WriteEndTag("a");



            if (renderAsListItem) writer.WriteEndTag("li");

        }

    }
}
