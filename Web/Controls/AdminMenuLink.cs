//	Created:			    2010-01-04
//	Last Modified:		    2010-06-04
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
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a convenience link for the Administration menu. The link renders only for those in roles that can use the admin menu
    /// </summary>
    public class AdminMenuLink : HyperLink
    {
        
        private string relativeUrl = "/Admin/AdminMenu.aspx";
        private mojoBasePage basePage = null;

        private bool renderAsListItem = false;
        public bool RenderAsListItem
        {
            get { return renderAsListItem; }
            set { renderAsListItem = value; }
        }

        private string listItemCSS = string.Empty;
        public string ListItemCss
        {
            get { return listItemCSS; }
            set { listItemCSS = value; }
        }

        private bool ShouldRender()
        {
            if (basePage == null) { return false; }
            if (!Page.Request.IsAuthenticated) { return false; }

            // initialize to default values
            ToolTip = Resource.AdminMenuLink;
            if (basePage.UseIconsForAdminLinks)
            {
                ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.AdminImage);
                Text = Resource.AdminMenuLink;
            }
            else
            {
                Text = Resource.AdminLink;
            }

            if (WebUser.IsAdminOrContentAdminOrRoleAdmin) { return true; }

            if (basePage.CurrentPage == null) { return false; }
            if (WebUser.IsInRoles(basePage.CurrentPage.CreateChildPageRoles))
            {
                //overide for non admins
                relativeUrl = WebConfigSettings.PageTreeRelativeUrl;
                ToolTip = Resource.PageTreeTitle;
                if (basePage.UseIconsForAdminLinks)
                {
                    ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.PageTreeImage);
                    Text = Resource.PageTreeLink;
                }
                else
                {
                    Text = Resource.PageListLink;
                }

                return true;
            }

            if (!WebConfigSettings.UseRelatedSiteMode) { return false; }

            if (basePage.SiteInfo == null) { return false; }
            // in related sites mode usersin site eidotrs role can use admin menu
            if (WebUser.IsInRoles(basePage.SiteInfo.SiteRootEditRoles)) { return true; }

            return false;

            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            EnableViewState = false;
            basePage = Page as mojoBasePage;

            Visible = ShouldRender();
            if (!Visible) { return; }

            if (basePage == null) { return; }

            if (CssClass.Length > 0)
            {
                CssClass = "adminlink adminmenulink " + CssClass;
            }
            else
            {
                CssClass = "adminlink adminmenulink";
            }
            //ToolTip = Resource.AdminMenuLink;
            if (SiteUtils.SslIsAvailable())
            {
                NavigateUrl = Page.ResolveUrl(basePage.SiteRoot + relativeUrl);
            }
            else
            {
                NavigateUrl = Page.ResolveUrl(basePage.RelativeSiteRoot + relativeUrl);
            }
            //if (basePage.UseIconsForAdminLinks)
            //{
            //    ImageUrl = Page.ResolveUrl("~/Data/SiteImages/admin.png");
            //    Text = Resource.AdminMenuLink;
            //}
            //else
            //{
            //    Text = Resource.AdminLink;
            //}
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (renderAsListItem)
            {
                if (listItemCSS.Length > 0)
                {
                    writer.Write("<li class='" + listItemCSS + "'>");
                }
                else
                {
                    writer.Write("<li>");
                }
            }

            base.Render(writer);

            if (renderAsListItem)
            {
                writer.Write("</li>");
            }
        }

    }
}
