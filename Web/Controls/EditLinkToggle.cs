// Author:					
// Created:				    2013-02-23
// Last Modified:		    2013-02-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class EditLinkToggle : WebControl
    {
        private PageSettings currentPage = null;

        private string text = string.Empty;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private bool renderAnchor = true;
        
        public bool RenderAnchor
        {
            get { return renderAnchor; }
            set { renderAnchor = value; }
        }

        private string beforeAnchor = string.Empty;

        public string BeforeAnchor
        {
            get { return beforeAnchor; }
            set { beforeAnchor = value; }
        }

        private string afterAnchor = string.Empty;

        public string AfterAnchor
        {
            get { return afterAnchor; }
            set { afterAnchor = value; }
        }

        private string beforeText = string.Empty;

        public string BeforeText
        {
            get { return beforeText; }
            set { beforeText = value; }
        }

        private string afterText = string.Empty;

        public string AfterText
        {
            get { return afterText; }
            set { afterText = value; }
        }

        private string clickTargetSelector = string.Empty;

        public string ClickTargetSelector
        {
            get { return clickTargetSelector; }
            set { clickTargetSelector = value; }
        }

        

        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (WebConfigSettings.DisablejQuery) { return; } //this depends on jquery

            if (!(Page is CmsPage)) { return; }

            currentPage = CacheHelper.GetCurrentPage();

            if (currentPage == null) { return; }
            if ((!WebUser.IsInRoles(currentPage.EditRoles)) && (!WebUser.IsInRoles(currentPage.DraftEditOnlyRoles))) { return; }

            if((!renderAnchor)&&(clickTargetSelector.Length == 0)) { return; }

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("function ToggleEditLinks(isClick)");
            script.Append("{ ");

            script.Append("var editLinksState = Get_Cookie('editLinksState'); ");
            script.Append("if (isClick)");
            script.Append("{ ");
            script.Append("if (editLinksState === null || editLinksState === 'visible') ");
            script.Append("{ ");
            script.Append("Set_Cookie('editLinksState', 'hidden'); ");
            script.Append(" ToggleEditLinks(false); ");

            script.Append("}"); // end linkstate check
            script.Append("else if (editLinksState === 'hidden')");
            script.Append("{ ");
            script.Append("Set_Cookie('editLinksState', 'visible'); ");
            script.Append("ToggleEditLinks(false); ");

            script.Append("}");// end else if
            script.Append("}");// end if isClick
            script.Append("else");
            script.Append("{");

            script.Append("switch (editLinksState)");
            script.Append("{");

            script.Append("case 'hidden': ");
            script.Append("$('.modulelinks, .ModuleEditLink, a.inlineedittoggle').hide(); ");
            //script.Append("$('.ModuleEditLink').hide(); ");
            //script.Append("$('a.inlineedittoggle').hide(); ");

            //script.Append("$('div[contenteditable=\"true\"]').attr('contenteditable', false); ");

            script.Append("break; ");

            script.Append("case 'visible': ");
            script.Append("$('.modulelinks, .ModuleEditLink, a.inlineedittoggle').show(); ");
            //script.Append("$('.ModuleEditLink').show(); ");
            //script.Append("$('a.inlineedittoggle').show(); ");

            //script.Append("$('div[contenteditable=\"false\"]').attr('contenteditable', true); ");

            script.Append("break; ");

            script.Append("case null: ");
            script.Append("$('.modulelinks').show(); ");
            script.Append("$('.ModuleEditLink').show(); ");
            script.Append("$('a.inlineedittoggle').show(); ");
            script.Append("Set_Cookie('editLinksState', 'visible'); ");
            script.Append("break; ");


            script.Append("}"); //end switch


            script.Append("}"); //end else

            script.Append("}"); //end function

            script.Append("\n</script>");

            ScriptManager.RegisterClientScriptBlock(
                this,
                typeof(Page),
                "editlinktogglemain",
                script.ToString(),
                false);

            script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function() {");

            // Check editLinksState cookie and hide if set to 'hidden'
            script.Append(" ToggleEditLinks(false);");

            if (clickTargetSelector.Length > 0)
            {
                script.Append(" $('" + clickTargetSelector + "').click(function() { ToggleEditLinks(true); return false;});");
            }
            else
            {
                script.Append(" $('#" + ClientID + "').click(function() { ToggleEditLinks(true); return false; });");
            }

            script.Append("});");

            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                "editlinktogglestartup",
                script.ToString(),
                false);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            
            if (HttpContext.Current == null) { return; }

            if (!(Page is CmsPage)) { return; }

            if (currentPage == null) { return; }
            if ((!WebUser.IsInRoles(currentPage.EditRoles)) && (!WebUser.IsInRoles(currentPage.DraftEditOnlyRoles))) { return; }

            
            if (!renderAnchor) { return; }
            if (WebConfigSettings.DisablejQuery) { return; } //this depends on jquery

            if (beforeAnchor.Length > 0) { writer.Write(beforeAnchor); }

            writer.Write("<a id='");
            writer.Write(ClientID);
            writer.Write("' rel='nofollow' href='#'");
            if (CssClass.Length > 0)
            {
                writer.Write(" class='" + CssClass + "'");
            }
            writer.Write(">");

            if (text.Length > 0)
            {
                if (beforeText.Length > 0) { writer.Write(beforeText); }
                writer.Write(text);
                if (afterText.Length > 0) { writer.Write(afterText); }
            }

            writer.Write("</a>");

            if (afterAnchor.Length > 0) { writer.Write(afterAnchor); }

        }
    }
}