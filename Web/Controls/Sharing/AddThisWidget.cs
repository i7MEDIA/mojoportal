//	Created:			    2011-09-21
//	Last Modified:		    2016-01-05
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
    public class AddThisWidget : WebControl
    {

        //private string protocol = "http";
        private string addThisBaseUrl = "//s7.addthis.com/js/300/addthis_widget.js";
        public string AddThisBaseUrl
        {
            get { return addThisBaseUrl; }
            set { addThisBaseUrl = value; }
        }

        private bool hideOnNonCmsBasePage = true;
        public bool HideOnNonCmsBasePage
        {
            get { return hideOnNonCmsBasePage; }
            set { hideOnNonCmsBasePage = value; }
        }

        private SiteSettings siteSettings = null;

        private string accountId = string.Empty;

        /// <summary>
        /// Your addthis.com username.
        /// If this is not set the control will not render.
        /// </summary>
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        private string urlToShare = string.Empty;

        public string UrlToShare
        {
            get { return urlToShare; }
            set { urlToShare = value; }
        }

        private string titleOfUrlToShare = string.Empty;

        public string TitleOfUrlToShare
        {
            get { return titleOfUrlToShare; }
            set { titleOfUrlToShare = value; }
        }

        private bool renderAsToolbar = true;

        public bool RenderAsToolbar
        {
            get { return renderAsToolbar; }
            set { renderAsToolbar = value; }
        }

        //private string toolbarMarkup = "<div class='addthis_toolbox addthis_default_style '>\n<a class='addthis_button_facebook_like' fb:like:layout='button_count'></a>\n<a class='addthis_button_tweet'></a>\n<a class='addthis_button_google_plusone' g:plusone:size='medium'></a>\n<a class='addthis_counter addthis_pill_style'></a>\n</div>";

        // updated on 1/5/2016 to use "newest" addthis tools available on that date.
        private string toolbarMarkup = "<div class='addthis_sharing_toolbox' data-url='{0}' data-title='{1}'></div>";

        // this is the equivalent of the old and uses addthis_toolbox
        //<div addthis:url='{0}' addthis:title='{1}' class='addthis_toolbox addthis_default_style addthis_32x32_style'><a class='addthis_button_preferred_1'></a><a class='addthis_button_preferred_2'></a><a class='addthis_button_preferred_4'></a><a class='addthis_button_compact'></a><a class='addthis_counter addthis_bubble_style'></a></div>

        public string ToolbarMarkup
        {
            get { return toolbarMarkup; }
            set { toolbarMarkup = value; }
        }

        //private string toolbarMarkupWithId = "<div id='{0}' class='addthis_toolbox addthis_default_style '>\n<a class='addthis_button_facebook_like' fb:like:layout='button_count'></a>\n<a class='addthis_button_tweet'></a>\n<a class='addthis_button_google_plusone' g:plusone:size='medium'></a>\n<a class='addthis_counter addthis_pill_style'></a>\n</div>";

        //private string toolbarMarkupWithId = "<div id='{0}' data-url='{1}' data-title='{2}' class='addthis_toolbox addthis_default_style addthis_32x32_style'><a class='addthis_button_preferred_1'></a><a class='addthis_button_preferred_2'></a><a class='addthis_button_preferred_4'></a><a class='addthis_button_compact'></a><a class='addthis_counter addthis_bubble_style'></a></div>";

        
        //public string ToolbarMarkupWithId
        //{
        //    get { return toolbarMarkupWithId; }
        //    set { toolbarMarkupWithId = value; }
        //}

        private string langCode = string.Empty;

        public string LangCode
        {
            get { return langCode; }
            set { langCode = value; }
        }

        private bool use508Compliant = false;

        public bool Use508Compliant
        {
            get { return use508Compliant; }
            set { use508Compliant = value; }
        }

        private bool useAnalyticsIntegration = true;

        private string analyticsId = string.Empty;

        public bool UseAnalyticsIntegration
        {
            get { return useAnalyticsIntegration; }
            set { useAnalyticsIntegration = value; }
        }

        private bool renderOnSecurePages = true;

        public bool RenderOnSecurePages
        {
            get { return renderOnSecurePages; }
            set { renderOnSecurePages = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (HttpContext.Current == null) { return; }
            if (hideOnNonCmsBasePage)
            {
                if (Page is NonCmsBasePage) { return; }
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            if (accountId.Length == 0)
            {
                accountId = siteSettings.AddThisDotComUsername;
            }
            analyticsId = siteSettings.GoogleAnalyticsAccountCode;

            if (SiteUtils.IsSecureRequest()) 
            { 
                //protocol = "https";
                if (!renderOnSecurePages) { Visible = false; }
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }

            if ((Page.Response.StatusCode == 404) || (Page.Response.StatusCode == 410))
            {
                return;
            }

            if (accountId.Length == 0)
            {
                this.Visible = false;
                return;
            }


            SetupMainScript();

            SetupGlobalConfigScript();

            //if (urlToShare.Length > 0)
            //{
            //    SetupInstanceScript();
            //}
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);

            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if(!Visible) { return; }

            //if (urlToShare.Length > 0)
            //{
                writer.Write(string.Format(toolbarMarkup, UrlToShare, TitleOfUrlToShare));
            //}
            //else
            //{
            //    writer.Write(toolbarMarkup);
            //}

        }

        //private void SetupInstanceScript()
        //{
        //    StringBuilder script = new StringBuilder();
        //    script.Append("<script type=\"text/javascript\">\n");

        //    //script.Append("addthis.toolbox('#" + ClientID + "',{},");
        //    //script.Append("{url:'" + urlToShare + "',title:'" + titleOfUrlToShare + "'}");

        //    //script.Append(");");

        //    script.Append("$('#" + ClientID + "').attr('addthis:url','" + urlToShare + "');");
        //    script.Append("$('#" + ClientID + "').attr('addthis:title','" + titleOfUrlToShare + "');");

        //    script.Append("\n");
        //    script.Append(" </script>");


        //    /*ScriptManager.RegisterStartupScript(
        //        this,
        //        typeof(AddThisWidget),
        //        UniqueID,
        //        script.ToString(),
        //        false);*/
        //}

        private void SetupGlobalConfigScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            script.Append("var addthis_config = {");

            script.Append("pubid:'" + accountId + "'");

            if (langCode.Length > 0)
            {
                script.Append(",ui_language:'" + langCode + "'");
            }

            if (use508Compliant)
            {
                script.Append(",ui_508_compliant: true");
            }

            if (useAnalyticsIntegration && analyticsId.Length > 0)
            {
                script.Append(",data_ga_tracker:'" + analyticsId + "'");
            }

            script.Append("}");

            script.Append("\n");
            script.Append(" </script>");


            ScriptManager.RegisterClientScriptBlock(
                this,
                typeof(AddThisWidget),
                "addthisconfig",
                script.ToString(),
                false);

        }

        private void SetupMainScript()
        {

            ScriptManager.RegisterClientScriptBlock(
                    this,
                    typeof(AddThisWidget),
                    "addthiswidget", "\n<script type=\"text/javascript\" src=\""
                    + addThisBaseUrl + "#pubid=" + accountId
                    + "\" async></script>", false);
        }
    }
}