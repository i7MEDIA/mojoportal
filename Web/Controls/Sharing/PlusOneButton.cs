//	Created:			    2011-06-02
//	Last Modified:		    2011-07-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://code.google.com/apis/+1button/
    /// http://www.google.com/support/forum/p/Webmasters/thread?tid=4e6057528875e135&hl=en&fid=4e6057528875e1350004a512314acb2c
    /// http://googlewebmastercentral.blogspot.com/2011/07/1-button-now-faster.html
    /// </summary>
    public class PlusOneButton : WebControl
    {
        private string widgetSize = "standard";

        /// <summary>
        /// supported values are small, medium, standard, tall
        /// </summary>
        public string WidgetSize
        {
            get { return widgetSize; }
            set { widgetSize = value; }
        }

        private bool showCount = true;

        public bool ShowCount
        {
            get { return showCount; }
            set { showCount = value; }
        }

        private string targetUrl = string.Empty;

        /// <summary>
        /// leave blank for the current page url
        /// </summary>
        public string TargetUrl
        {
            get { return targetUrl; }
            set { targetUrl = value; }
        }

        private string callback = string.Empty;

        public string Callback
        {
            get { return callback; }
            set { callback = value; }
        }

        private string lang = "en-US";

        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        private bool useHtml5Syntax = true;

        public bool UseHtml5Syntax
        {
            get { return useHtml5Syntax; }
            set { useHtml5Syntax = value; }
        }

        //http://googlewebmastercentral.blogspot.com/2011/07/1-button-now-faster.html
        private bool useAsyncScript = true;

        public bool UseAsyncScript
        {
            get { return useAsyncScript; }
            set { useAsyncScript = value; }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (!Visible) { return; }

            if (useAsyncScript)
            {
                string script = "<script type=\"text/javascript\">\n (function() {\n var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;\npo.src = 'https://apis.google.com/js/plusone.js';\nvar s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);\n })();\n</script>";
                Page.ClientScript.RegisterStartupScript(typeof(Page),
                             "gplusone", script, false);
            }
            else
            {

                Page.ClientScript.RegisterStartupScript(typeof(Page),
                             "gplusone", "\n<script type=\"text/javascript\" src=\""
                             + "https://apis.google.com/js/plusone.js" + "\">{\"lang\":\"" + lang + "\"}</script>",false);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (useHtml5Syntax)
            {
                RenderHtml5(writer);
            }
            else
            {
                RenderInvalidElement(writer);
            }

            

        }

        private void RenderHtml5(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "g-plusone");

            switch (widgetSize)
            {
                case "small":
                    writer.WriteAttribute("data-size", "small");
                    break;

                case "medium":
                    writer.WriteAttribute("data-size", "medium");
                    break;

                case "tall":
                    writer.WriteAttribute("data-size", "tall");
                    break;

                case "standard":
                default:
                    writer.WriteAttribute("data-size", "standard");
                    break;
            }

            if (!showCount)
            {
                writer.WriteAttribute("data-count", "false");
            }

            if (targetUrl.Length > 0)
            {
                writer.WriteAttribute("data-href", targetUrl);
            }

            if (callback.Length > 0)
            {
                writer.WriteAttribute("data-callback", callback);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteEndTag("div");

        }

        private void RenderInvalidElement(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("g:plusone");
            writer.WriteAttribute("class", "g-plusone");

            switch (widgetSize)
            {
                case "small":
                    writer.WriteAttribute("size", "small");
                    break;

                case "medium":
                    writer.WriteAttribute("size", "medium");
                    break;

                case "tall":
                    writer.WriteAttribute("size", "tall");
                    break;

                case "standard":
                default:
                    writer.WriteAttribute("size", "standard");
                    break;
            }

            if (!showCount)
            {
                writer.WriteAttribute("count", "false");
            }

            if (targetUrl.Length > 0)
            {
                writer.WriteAttribute("href", targetUrl);
            }

            if (callback.Length > 0)
            {
                writer.WriteAttribute("callback", callback);
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteEndTag("g:plusone");

        }


    }
}