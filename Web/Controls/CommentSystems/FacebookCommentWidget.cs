//  Author:                     
//  Created:                    2012-06-12
//	Last Modified:              2012-06-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://developers.facebook.com/docs/reference/javascript/
    /// </summary>
    public class FacebookCommentWidget : Panel
    {
        //HtmlGenericControl body = null;

        private string dataHref = string.Empty;
        public string DataHref
        {
            get { return dataHref; }
            set { dataHref = value; }
        }

        private int postsToShow = 10;

        public int PostsToShow
        {
            get { return postsToShow; }
            set { postsToShow = value; }
        }

        private int widgetWidth = 470;

        public int WidgetWidth
        {
            get { return widgetWidth; }
            set { widgetWidth = value; }
        }

        private bool autoDetectUrl = false;
        public bool AutoDetectUrl
        {
            get { return autoDetectUrl; }
            set { autoDetectUrl = value; }
        }

        private string colorScheme = "light";
        /// <summary>
        /// options light or dark
        /// </summary>
        public string ColorScheme
        {
            get { return colorScheme; }
            set { colorScheme = value; }
        }

        //private string protocol = "http";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;
            //body = Page.Master.FindControl("Body") as HtmlGenericControl;

            if (!CanRender())
            {
                Visible = false;
                return;
            }

            if ((dataHref.Length == 0) && (autoDetectUrl))
            {
                dataHref = WebUtils.ResolveServerUrl(Page.Request.RawUrl);
            }

            //SetupScript();

            CssClass = "fb-comments";

            Attributes.Add("data-href", dataHref);
            Attributes.Add("data-num-posts", postsToShow.ToInvariantString());
            Attributes.Add("data-width", widgetWidth.ToInvariantString());
            Attributes.Add("data-colorscheme", colorScheme);
        }

        //private void SetupScript()
        //{
        //    if (ScriptIsAlreadyLoaded()) { return; }

        //    StringBuilder s = new StringBuilder();

        //    s.Append("\n<div id=\"fb-root\"></div>");
        //    s.Append("\n <script type='text/javscript'>\n (function(d, s, id) {");
        //    s.Append("\n var js, fjs = d.getElementsByTagName(s)[0];");
        //    s.Append("\n if (d.getElementById(id)) return;");
        //    s.Append("\n js = d.createElement(s); js.id = id;");
        //    s.Append("\n js.src = \"//connect.facebook.net/en_US/all.js#xfbml=1\";");
        //    s.Append("\n fjs.parentNode.insertBefore(js, fjs);");
        //    s.Append("\n }(document, 'script', 'facebook-jssdk'));</script>");
            

        //    Literal litScript = new Literal();
        //    litScript.ID = mainScriptId;
        //    litScript.EnableViewState = false;
        //    litScript.Text = s.ToString();
        //    //Page.Form.Controls.AddAt(0, litScript);
        //    body.Controls.Add(litScript);

        //}

        //private string mainScriptId = "fbcommentsmain";

        //private bool ScriptIsAlreadyLoaded()
        //{
        //    foreach (Control c in Page.Form.Controls)
        //    {
        //        if (c.ID == mainScriptId) { return true; }
        //    }


        //    return false;
        //}

        private bool CanRender()
        {
            if ((dataHref.Length == 0) && (!autoDetectUrl)) { return false; }

            
            //if (body == null) { return false; }

            return true;
        }
    }
}