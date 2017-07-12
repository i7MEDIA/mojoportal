/// Author:					
/// Created:				2006-03-24
/// Last Modified:			2011-12-15


using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// I use this to wrap things so they are not displayed 
    /// when using https/ssl, otherwise the user gets browser warnings about
    /// the insecure items in the request
    /// </summary>
    public class InsecurePanel : Panel
    {
        private bool showIfSsl = false;

        /// <summary>
        /// setting this to true basically flips this to a panel that only shows when using SSL
        /// whereas the default false only shows if not using ssl
        /// </summary>
        public bool ShowIfSsl
        {
            get { return showIfSsl; }
            set { showIfSsl = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (HttpContext.Current == null) { return; }


            if (SiteUtils.IsSecureRequest())
            {
                if (!showIfSsl)
                {
                    this.Visible = false;
                }
            }
            else
            {
                if (showIfSsl)
                {
                    this.Visible = false;
                }

            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
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
