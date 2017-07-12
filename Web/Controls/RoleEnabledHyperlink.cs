// Author:		        /Joe Davis
// Created:             2012-09-23
// Last Modified:       2014-04-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business.WebHelpers;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class RoleEnabledHyperlink : HyperLink
    {
        private string relativeUrl = string.Empty;
        public string RelativeUrl 
        { 
            get { return relativeUrl; } 
            set { relativeUrl = value; } 
        }

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

        private string allowedRoles = string.Empty;
        public string AllowedRoles 
        { 
            get { return allowedRoles; } 
            set { allowedRoles = value; } 
        }

        private bool useSSL = false;
        public bool UseSSL 
        { 
            get { return useSSL; } 
            set { useSSL = value; } 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (relativeUrl.Length > 0)
            {
                if (useSSL && SiteUtils.SslIsAvailable())
                {
                    NavigateUrl = SiteUtils.GetSecureNavigationSiteRoot() + relativeUrl;
                }
                else
                {
                    NavigateUrl = SiteUtils.GetNavigationSiteRoot() + relativeUrl;
                }
            }

            if ((allowedRoles.Length > 0) && (!WebUser.IsInRoles(allowedRoles))) { Visible = false; }

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if ((allowedRoles.Length > 0) &&(!WebUser.IsInRoles(allowedRoles))) { return; }

            if (renderAsListItem)
            {
                writer.Write("<li");
                if (listItemCSS.Length > 0)
                {
                    writer.Write(" class='" + listItemCSS + "'");
                }
                writer.Write(">");
            }

            base.Render(writer);

            if (renderAsListItem)
            {
                writer.Write("</li>");
            }
        }
        

    }
}