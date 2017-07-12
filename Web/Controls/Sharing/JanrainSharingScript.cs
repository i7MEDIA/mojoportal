//	Author:                 
//  Created:			    2012-07-01
//	Last Modified:		    2012-07-06
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
using System.Text;
using mojoPortal.Web.UI.Pages;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://developers.janrain.com/documentation/widgets/social-sharing-widget/users-guide/get-the-code/
    /// </summary>
    public class JanrainSharingScript : WebControl
    {
        private StringBuilder script = null;

        private string nonSecureScriptUrl = string.Empty;

        public string NonSecureScriptUrl
        {
            get { return nonSecureScriptUrl; }
            set { nonSecureScriptUrl = value; }
        }

        private string secureScriptUrl = string.Empty;

        public string SecureScriptUrl
        {
            get { return secureScriptUrl; }
            set { secureScriptUrl = value; }
        }

        private string urlToShare = string.Empty;

        public string UrlToShare
        {
            get { return urlToShare; }
            set { urlToShare = value; }
        }

        private string message = string.Empty;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description = string.Empty;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private bool renderOnNonCmsBasePage = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnableViewState = false;
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRender()) { return; }

            script = new StringBuilder();

            script.Append("\n <script type=\"text/javascript\">");

            script.Append("\n (function() {");

            script.Append("if (typeof window.janrain !== 'object') window.janrain = {}; ");
            script.Append("if (typeof window.janrain.settings !== 'object') window.janrain.settings = {}; ");
            script.Append("if (typeof window.janrain.settings.share !== 'object') window.janrain.settings.share = {}; ");
            script.Append("if (typeof window.janrain.settings.packages !== 'object') janrain.settings.packages = []; ");
            script.Append("janrain.settings.packages.push('share'); ");

            if (message.Length > 0)
            {
                script.Append("janrain.settings.share.message = \"" + message.HtmlEscapeQuotes() + "\"; ");
            }

            if (title.Length > 0)
            {
                script.Append("janrain.settings.share.title = \"" + title.HtmlEscapeQuotes() + "\"; ");
            }

            if (urlToShare.Length > 0)
            {
                script.Append("janrain.settings.share.url = \"" + urlToShare + "\"; ");
            }

            if (description.Length > 0)
            {
                script.Append("janrain.settings.share.description = \"" + description.HtmlEscapeQuotes() + "\"; ");
            }


            script.Append("\n function isReady() { janrain.ready = true; }; ");
            script.Append("if (document.addEventListener) { ");
            script.Append("document.addEventListener(\"DOMContentLoaded\", isReady, false); ");
            script.Append("} else { ");
            script.Append("window.attachEvent('onload', isReady); ");
            script.Append("} ");

            script.Append("var e = document.createElement('script'); ");
            script.Append("e.type = 'text/javascript'; ");
            script.Append("e.id = 'janrainWidgets'; ");

            script.Append("if (document.location.protocol === 'https:') { ");
            script.Append("e.src = '" + secureScriptUrl + "'; ");
            script.Append("} else { ");
            script.Append("e.src = '" + nonSecureScriptUrl + "'; ");
            script.Append("} ");

            script.Append("var s = document.getElementsByTagName('script')[0]; ");
            script.Append("s.parentNode.insertBefore(e, s); ");




            script.Append("\n })();");

            script.Append("\n </script>");

        }

        private bool ShouldRender()
        {
            if (nonSecureScriptUrl.Length == 0) { return false; }
            if (secureScriptUrl.Length == 0) { return false; }
            if ((Page is NonCmsBasePage) && (!renderOnNonCmsBasePage)) { return false; }

            if (Page is LoginPage) { return false; }

            //Register
            //RegisterWithOpenId
            //RegisterWithWindowsLiveId
            //UpdateOpenIdPage
            //UserProfile
            //ChangePassword
            //OpenIdRpxHandlerPage
            //PasswordResetPage
            //RecoverPassword

            return true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            if (script != null)
            {
                writer.Write(script.ToString());
            }
        }
    }
}