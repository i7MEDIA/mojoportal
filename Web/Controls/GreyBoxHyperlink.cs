// Author:					
// Created:				    2009-04-08
// Last Modified:			2010-04-27
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
    public class GreyBoxHyperlink : HyperLink
    {
        private bool assumeScriptsAreLoaded = false;
        private string scriptBaseUrl = "~/ClientScript/greybox/";
        private string rel = string.Empty;
        private string dialogCloseText = string.Empty;
        private string clientClick = string.Empty;

        public string ScriptBaseUrl
        {
            get { return scriptBaseUrl; }
            set { scriptBaseUrl = value; }
        }

        /// <summary>
        /// examples:
        /// return GB_show('Google', this.href)
        /// return GB_showCenter('Google', this.href)
        /// return GB_showFullScreen('Google', this.href)
        /// return GB_showPage('Google', this.href)
        /// </summary>
        public string ClientClick
        {
            get { return clientClick; }
            set { clientClick = value; }
        }

        /// <summary>
        /// examples:
        /// gb_page[WIDTH, HEIGHT]
        /// gb_page_fs[]
        /// </summary>
        public string Rel
        {
            get { return rel; }
            set { rel = value; }
        }

        public string DialogCloseText
        {
            get { return dialogCloseText; }
            set { dialogCloseText = value; }
        }

        /// <summary>
        /// If using this on the same page as neatupload with NeatUpload greyboxProgressBar, set this to true because NeatUpload already loads the scripts.
        /// </summary>
        public bool AssumeScriptsAreLoaded
        {
            get { return assumeScriptsAreLoaded; }
            set { assumeScriptsAreLoaded = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            dialogCloseText = Resource.DialogCloseLink;
            SetupScript();
            SetupCss();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            

            if (clientClick.Length > 0)
            {
                this.Attributes.Add("onclick", clientClick);
            }
            else if (rel.Length > 0)
            {
                this.Attributes.Add("rel", rel);
            }
            
           

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            base.Render(writer);


        }

        private void SetupScript()
        {
            if (assumeScriptsAreLoaded) { return; }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "gbVar", "\n<script  type=\"text/javascript\">"
                        + "var GB_ROOT_DIR = '" + Page.ResolveUrl(scriptBaseUrl) + "'; var GBCloseText = '" + dialogCloseText + "';" + " </script>");

            if (Page is mojoBasePage)
            {

                mojoBasePage mojoPage = Page as mojoBasePage;
                mojoPage.ScriptConfig.IncludeGreyBox = true;

            }
            else
            {

                



                //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                //        "GreyBoxJs", "\n<script  src=\""
                //        + Page.ResolveUrl(scriptBaseUrl + "gbcombined.js") + "\" type=\"text/javascript\" ></script>");

                //The commented version above is the preferred syntax, the uncommented one below is the deprecated version.
                //There is a reason we are using the deprecated version, greybox is registered also by NeatUpload using the deprecated
                //syntax, the reason they use the older syntax is because they also support .NET v1.1
                //We use the old syntax here for compatibility with NeatUpload so that it does not get registered more than once
                // on pages that use NeatUpload. Otherwise we would have to always modify our copy of NeatUpload.
                Page.RegisterClientScriptBlock("GreyBoxJs", "\n<script  src=\""
                        + Page.ResolveUrl(scriptBaseUrl + "gbcombined.js") + "\" type=\"text/javascript\" ></script>");
            }


        }

        private void SetupCss()
        {
            if (assumeScriptsAreLoaded) { return; }

            //<link href="/ClientScript/greybox/gb_styles.css" rel="stylesheet" type="text/css" media="all" />
            if (Page.Master == null) { return; }
            

            StyleSheetCombiner style = Page.Master.FindControl("StyleSheetCombiner") as StyleSheetCombiner;
            if (style != null) { style.IncludeGreyBoxCss = true; }
            
            
            //Control head = Page.Master.FindControl("Head1");
            //if (head == null) { return; }

            //try
            //{
            //    if (head.FindControl("gb_styles") == null)
            //    {
            //        Literal cssLink = new Literal();
            //        cssLink.ID = "gb_styles";
            //        cssLink.Text = "\n<link rel='stylesheet' type='text/css' href='"
            //            + Page.ResolveUrl(scriptBaseUrl + "gb_styles.css")
            //             + "' media='all' />";

            //        head.Controls.Add(cssLink);
            //    }
            //}
            //catch (HttpException) { }

         
        }

    }
}
