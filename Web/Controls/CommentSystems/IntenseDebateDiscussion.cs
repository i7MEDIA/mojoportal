//  Author:                     
//  Created:                    2009-09-06
//	Last Modified:              2011-06-29
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    public class IntenseDebateDiscussion : WebControl
    {
        private string accountId = string.Empty;

        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        private string postId = string.Empty;

        public string PostId
        {
            get { return postId; }
            set { postId = value; }
        }

        private string postUrl = string.Empty;

        public string PostUrl
        {
            get { return postUrl; }
            set { postUrl = value; }
        }

        /// <summary>
        /// this allows override by theme
        /// </summary>
        private bool disable = false;

        public bool Disable
        {
            get { return disable; }
            set { disable = value; }
        }

        private string protocol = "http";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (disable || (accountId.Length == 0)) { Visible = false; return; }

            SetupVariableScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (disable || (accountId.Length == 0)) { Visible = false; return; }

           // SetupVariableScript();

            RenderThread(writer); 

        }

        private void RenderThread(HtmlTextWriter writer)
        {
            

            writer.Write("<div class=\"cmwrapper\">");
            writer.Write("<span id=\"IDCommentsPostTitle\" style=\"display:none\"></span>");
            
            writer.Write("<script type='text/javascript' src='" + protocol + "://www.intensedebate.com/js/genericCommentWrapperV2.js'></script>");

            writer.Write("</div>");
        }

        private void SetupVariableScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type=\"text/javascript\">");

            script.Append(" var idcomments_acct = '" + accountId + "'; ");

            if (postId.Length > 0)
            {
                script.Append(" var idcomments_post_id = '" + postId + "'; ");
            }
            else
            {
                script.Append(" var idcomments_post_id; ");
            }

            if (postUrl.Length > 0)
            {
                script.Append(" var idcomments_post_url = '" + postUrl + "'; ");
            }
            else
            {
                script.Append(" var idcomments_post_url; ");
            }


            script.Append("\n</script>");

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "intensedvars",
                script.ToString(), false);


        }

    }
}
