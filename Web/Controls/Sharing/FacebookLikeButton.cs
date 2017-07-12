//  Author:                 
//	Created:			    2010-05-26
//	Last Modified:		    2010-05-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a simple control to implement a Facebook like button
    /// </summary>
    public class FacebookLikeButton : WebControl
    {

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            //if (SiteUtils.IsSecureRequest()) { return; }
            if (!Visible) { return; }
            if (WebConfigSettings.DisableFacebookLikeButton) { return; }

            if (CssClass.Length == 0) { CssClass = "fblikebutton"; }

            if(WrapInDiv)
            writer.Write("<div class='" + CssClass + "'>");

            writer.Write("<iframe ");
            writer.Write("src=\"//www.facebook.com/plugins/like.php?href=");

            if (urlToLike.Length > 0)
            {
                writer.Write(Page.Server.UrlEncode(urlToLike));
            }
            else
            {
                writer.Write(Page.Server.UrlEncode(WebUtils.ResolveServerUrl(Page.Request.RawUrl)));
            }

            switch (Layout)
            {
                case LayoutButtonCount:
                    writer.Write("&amp;layout=button_count");
                    break;
                case LayoutStandard:
                default:
                    writer.Write("&amp;layout=standard");
                    break;
            }

            if (ShowFaces)
            {
                writer.Write("&amp;show_faces=true");
            }
            else
            {
                writer.Write("&amp;show_faces=false");
            }

            writer.Write("&amp;width=" + WidthInPixels.ToInvariantString());
            if (showFaces)
            {
                writer.Write("&amp;height=80");
            }
            else
            {
                writer.Write("&amp;height=35");
            }
            writer.Write("&amp;action=like");

            switch (ColorScheme)
            {
                case ColorSchemDark:
                    writer.Write("&amp;colorscheme=dark\"");
                    break;

                case ColorSchemeLight:
                default:
                    writer.Write("&amp;colorscheme=light\"");
                    break;
            }

            writer.Write(" scrolling=\"no\"");
            writer.Write(" frameborder=\"0\"");
            writer.Write(" allowTransparency=\"true\"");
            if (showFaces)
            {
                if (heightInPixels < 80) { heightInPixels = 80; }
                writer.Write(" style=\"border:none; overflow:hidden;width:" + WidthInPixels.ToInvariantString() + "px; height:" + heightInPixels.ToInvariantString() + "px;\"></iframe>");
            }
            else
            {
                writer.Write(" style=\"border:none; overflow:hidden;width:" + WidthInPixels.ToInvariantString() + "px; height:" + heightInPixels.ToInvariantString() + "px;\"></iframe>");
            }

            if (WrapInDiv)
            writer.Write("</div>");
 
        }

        

        private string urlToLike = string.Empty;
        /// <summary>
        /// by default this control uses the current url or the page for the url to like, leave this blank for the default behavior
        /// or specify a fully qualified url to like
        /// </summary>
        public string UrlToLike
        {
            get { return urlToLike; }
            set { urlToLike = value; }
        }

        private const string ColorSchemeLight = "light";
        private const string ColorSchemDark = "dark";

        private string colorScheme = "light"; 
        /// <summary>
        /// options are light and dark, if you specify something invalid light will be used
        /// </summary>
        public string ColorScheme
        {
            get { return colorScheme; }
            set { colorScheme = value; }
        }

        private const string LayoutStandard = "standard";
        private const string LayoutButtonCount = "button_count";

        private string layout = LayoutStandard;
        /// <summary>
        /// options are standard or button_count,if something invalid is specified standard will be used
        /// </summary>
        public string Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        private bool showFaces = false;

        public bool ShowFaces
        {
            get { return showFaces; }
            set { showFaces = value; }
        }

        private int widthInPixels = 450;

        public int WidthInPixels
        {
            get { return widthInPixels; }
            set { widthInPixels = value; }
        }

        private int heightInPixels = 35;

        public int HeightInPixels
        {
            get { return heightInPixels; }
            set { heightInPixels = value; }
        }

        private bool wrapInDiv = true;
        /// <summary>
        /// if true it wraps the iframe inside a div with a css class that you could use to position the iframe
        /// </summary>
        public bool WrapInDiv
        {
            get { return wrapInDiv; }
            set { wrapInDiv = value; }
        }

    }
}