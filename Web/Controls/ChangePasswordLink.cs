///	Created:			    2007/04/25
///	Last Modified:		    2007/04/25
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public class ChangePasswordLink : WebControl
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

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                //writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (Page.Request.IsAuthenticated && Context.User.Identity.AuthenticationType == "Forms")
                {
                    if (renderAsListItem) writer.Write("<li class='" + listItemCSS + "'>");

                    if (UseLeftSeparator)
                    {
                        if (leftSeparatorImageUrl.Length > 0)
                        {
                            writer.Write("<img class='accent' src='" + Page.ResolveUrl(leftSeparatorImageUrl) + "' border='0' />");
                        }
                        else
                        {
                            writer.Write("<span class='accent'>|</span>");
                        }
                    }

                    string urlToUse = "~/Secure/ChangePassword.aspx";
                    if (CssClass.Length == 0) CssClass = "sitelink";
                    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                    if ((siteSettings != null) && (siteSettings.SiteFolderName.Length > 0))
                    {
                        //urlToUse = siteSettings.SiteRoot + "/Secure/ChangePassword.aspx";
                        urlToUse = SiteUtils.GetNavigationSiteRoot(siteSettings) + "/Secure/ChangePassword.aspx";
                    }
                    writer.Write(string.Format(
                                     " <a href='{0}' title='{1}' class='"
                                     + CssClass + "'>{1}</a>",
                                     Page.ResolveUrl(urlToUse),
                                     Resource.ChangePasswordLink));

                    if (renderAsListItem) writer.Write("</li>");
                }
            }
        }
    }
}
