// Author:					
// Created:				2006-03-24
// Last Modified:			2011-02-27
// 2011-02-27 copied from mojoPortal.Web.Controls project so we could use SiteUtils.IsSecureRequest instead of Page.Request.IsSecureConnection


using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// I use this to wrap things so they are not displayed 
    /// when using https/ssl, otherwise the user gets browser warnings about
    /// the insecure items in the request
    /// </summary>
    public class InsecurePanel : Panel
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            if (Page.Request.IsSecureConnection) this.Visible = false;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            base.Render(writer);
        }
    }
}
