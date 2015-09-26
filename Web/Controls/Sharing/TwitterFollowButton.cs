//	Created:			    2011-06-02
//	Last Modified:		    2011-09-23
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
using Resources;

namespace mojoPortal.Web.UI
{
    public class TwitterFollowButton : WebControl
    {
        // https://twitter.com/about/resources/followbutton

        private string protocol = "http";

        private string userToFollow = string.Empty;

        public string UserToFollow
        {
            get { return userToFollow; }
            set { userToFollow = value; }
        }

        private string lang = "en";

        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        private bool showCount = true;

        public bool ShowCount
        {
            get { return showCount; }
            set { showCount = value; }
        }

        private string buttonColor = "blue";

        public string ButtonColor
        {
            get { return buttonColor; }
            set { buttonColor = value; }
        }

        private string textColor = string.Empty;

        public string TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        private string linkColor = string.Empty;

        public string LinkColor
        {
            get { return linkColor; }
            set { linkColor = value; }
        }

        private string widgetWidth = string.Empty;

        public string WidgetWidth
        {
            get { return widgetWidth; }
            set { widgetWidth = value; }
        }

        private string align = string.Empty;

        public string Align
        {
            get { return align; }
            set { align = value; }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (!Visible) { return; }

            if (userToFollow.Length == 0) { return; }

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                         "twitterwidgets", "\n<script src=\""
                         + protocol + "://platform.twitter.com/widgets.js" + "\" type=\"text/javascript\"></script>");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (userToFollow.Length == 0) { return; }

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", "twitter-follow-button");

            writer.WriteAttribute("href", "http://twitter.com/" + userToFollow);

            if (!showCount)
            {
                writer.WriteAttribute("data-show-count", "false");
            }

            if (!string.Equals(lang, "en", StringComparison.InvariantCultureIgnoreCase))
            {
                writer.WriteAttribute("data-lang", lang);
            }

            if (!string.Equals(buttonColor, "blue", StringComparison.InvariantCultureIgnoreCase))
            {
                writer.WriteAttribute("data-button", buttonColor);
            }

            if (textColor.Length > 0)
            {
                writer.WriteAttribute("data-text-color", textColor);
            }

            if (linkColor.Length > 0)
            {
                writer.WriteAttribute("data-link-color", linkColor);
            }

            if (widgetWidth.Length > 0)
            {
                writer.WriteAttribute("data-width", widgetWidth);
            }

            if (align.Length > 0)
            {
                writer.WriteAttribute("data-align", align);
            }
            
            

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(string.Format(Resource.TwitterFollowFormat, userToFollow));

            writer.WriteEndTag("a");
        }
    }
}