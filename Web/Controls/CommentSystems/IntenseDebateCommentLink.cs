//  Author:                     
//  Created:                    2009-09-06
//	Last Modified:              2009-09-06
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
    public class IntenseDebateCommentLink : WebControl
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

        private string protocol = "http";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (accountId.Length == 0) { Visible = false; return; }

            SetupVariableScript();

        }

        protected override void Render(HtmlTextWriter writer)
        {

            if (accountId.Length == 0) { Visible = false; return; }

            //SetupVariableScript();

            if (postUrl.Length > 0) { RenderLink(writer); }

        }

        private void RenderLink(HtmlTextWriter writer)
        {
            writer.Write("<script type=\"text/javascript\"> ");
            if (postId.Length > 0)
            {
                writer.Write("idcomments_post_id = '" + postId + "'; ");
            }
            writer.Write("idcomments_post_url = '" + postUrl + "'; ");
            writer.Write("</script>");
            writer.Write("<script type=\"text/javascript\" src=\"" + protocol + "://www.intensedebate.com/js/genericLinkWrapperV2.js\"></script>");
        }

        private void SetupVariableScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type=\"text/javascript\"> ");

            script.Append("var idcomments_acct = '" + accountId + "'; ");
            script.Append("var idcomments_post_id; ");
            script.Append("var idcomments_post_url; ");
            

            script.Append("\n</script>");

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "intensedvars",
                script.ToString());

            //Page.ClientScript.RegisterStartupScript(
            //    typeof(Page),
            //    "intensedstart",
            //    "<script type=\"text/javascript\" src=\"http://www.intensedebate.com/js/genericLinkWrapperV2.js\"></script>");


        }

    }
}
