//	Author:				
//	Created:			2010-06-06
//	Last Modified:		2012-12-26
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
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public class GoogleCustomSearchControl : WebControl
    {
        private string customSearchId = string.Empty;

        public string CustomSearchId
        {
            get { return customSearchId; }
            set { customSearchId = value; }
        }

        private string divId = "cse";

        public string DivId
        {
            get { return divId; }
            set { divId = value; }
        }

        private string loadingText = string.Empty;

        public string LoadingText
        {
            get { return loadingText; }
            set { loadingText = value; }
        }

        private bool includeGoogleCustomSearchCss = false;
        public bool IncludeGoogleCustomSearchCss
        {
            get { return includeGoogleCustomSearchCss; }
            set { includeGoogleCustomSearchCss = value; }
        }

        private bool useV1 = false;
        public bool UseV1
        {
            get { return useV1; }
            set { useV1 = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Visible) { return; }

            if (useV1)
            {
                LoadV1();
            }
            else
            {
                LoadV2();
            }
            
        }

        private void LoadV2()
        {
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                basePage.ScriptConfig.IncludeGoogleSearchV2 = true;
                basePage.ScriptConfig.GoogleSearchV2Id = SiteUtils.GetGoogleCustomSearchId();
            }
        }

        private void LoadV1()
        {
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                basePage.ScriptConfig.IncludeGoogleSearch = true;
                basePage.StyleCombiner.IncludeGoogleCustomSearchCss = includeGoogleCustomSearchCss;
            }

            if (customSearchId.Length == 0) { customSearchId = SiteUtils.GetGoogleCustomSearchId(); }

            if (loadingText.Length == 0) { loadingText = Resource.Loading; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (useV1)
            {
                RenderV1(writer);
            }
            else
            {
                RenderV2(writer);
            }
        }

        private bool useHtml5 = true;
        public bool UseHtml5
        {
            get { return useHtml5; }
            set { useHtml5 = value; }
        }

        private void RenderV2(HtmlTextWriter writer)
        {
            if (useHtml5)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "gcse-search");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
            else
            {
                writer.Write("<gcse:search></gcse:search>");
            }

        }

        private void RenderV1(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(customSearchId)) { return; }
            if (string.IsNullOrEmpty(divId)) { return; }

            writer.Write("<div id=\"" + divId + "\" style=\"width: 100%;\">" + loadingText + "</div>");

            writer.Write("<script type=\"text/javascript\">");
            writer.Write("google.setOnLoadCallback(function() {");
            writer.Write("var customSearchControl = new google.search.CustomSearchControl('" + customSearchId + "'); ");
            writer.Write("customSearchControl.setResultSetSize(google.search.Search.FILTERED_CSE_RESULTSET); ");
            writer.Write("customSearchControl.draw('cse'); ");

            if (!Page.IsPostBack)
            {
                if (HttpContext.Current.Request.QueryString["q"] != null && HttpContext.Current.Request.QueryString["q"].Length > 0)
                {
                    writer.Write("customSearchControl.execute('" + SecurityHelper.SanitizeHtml(HttpContext.Current.Request.QueryString["q"]) + "');");
                }
            }

            writer.Write("}, true); ");

            writer.Write("</script>");
        }
    }
}