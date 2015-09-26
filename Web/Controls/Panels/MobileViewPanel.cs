//	Created:			    Joe Davis   2013-03-30
//	Last Modified:		    Joe Davis   2013-03-30
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Code originally derived from the mojoPortal.Web.AutoHidePanel

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this is useful when you need a div container but you don't want it to render if the users browser is not mobile 
    /// </summary>
    public class MobileViewPanel : BasePanel
    {
        private bool isMobileDevice = false;
        private bool useMobileSkin = false;

        /// <summary>
        /// If set to true, this control will only render when a mobile skin is set in Site Settings. Default is false.
        /// </summary>
        public bool RequireMobileSkin
        {
            get { return requireMobileSkin; }
            set { requireMobileSkin = value; }
        }

        private bool requireMobileSkin = false;

        protected override void OnPreRender(EventArgs e)
        {
            if (HttpContext.Current == null)
            {
                base.OnPreRender(e);
                return;
            }

            isMobileDevice = SiteUtils.IsMobileDevice();
            useMobileSkin = SiteUtils.UseMobileSkin();

            if ((!isMobileDevice) //if it is not a mobile device, hide the panel
                || (requireMobileSkin && !useMobileSkin)) //if we are requiring a mobile skin but one is not set, hide the panel
            {
                Visible = false;
            }

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (Visible)
            {
                base.Render(writer);
            }
        }
    }
}
