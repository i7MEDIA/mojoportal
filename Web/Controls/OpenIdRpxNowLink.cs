// Author:		        
// Created:             2009-05-14
// Last Modified:       2011-12-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a wrapper control for the rpx widget
    /// </summary>
    public class OpenIdRpxNowLink : HyperLink
    {
        private SiteSettings siteSettings = null;
        private string realm = string.Empty;
        private string tokenUrl = string.Empty;
        private bool embed = true;
        private bool useOverlay = false;
        private string siteRoot = string.Empty;
        private string overrideText = string.Empty;

        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        public bool Embed
        {
            get { return embed; }
            set { embed = value; }
        }

        public bool UseOverlay
        {
            get { return useOverlay; }
            set { useOverlay = value; }
        }

        private bool useOldImplementation = false;

        private bool autoDetectReturnUrl = false;
        public bool AutoDetectReturnUrl
        {
            get { return autoDetectReturnUrl; }
            set { autoDetectReturnUrl = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Visible) { return; }

            base.OnLoad(e);

            useOldImplementation = WebConfigSettings.OpenIdRpxUseOldImplementation;

            LoadSettings();
            SetupScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (useOldImplementation)
            {
                RenderOld(writer);
                return;
            }

            if (string.IsNullOrEmpty(realm)) { return; }
            if (string.IsNullOrEmpty(tokenUrl)) { return; }

            if (embed)
            {
                //writer.Write("<iframe src=\"");
                //writer.Write("https://" + realm + "/openid/embed?token_url=" + Page.Server.UrlEncode(tokenUrl)
                //    + "&amp;language_preference=" + GetLanguageCode() + "\"");
                //writer.Write(" scrolling=\"no\" frameBorder=\"no\" style=\"width:400px;height:240px;\"></iframe>");

                writer.Write("<div id=\"janrainEngageEmbed\"></div>");

            }
            else
            {
                string linkText = Resource.OpenIDLoginButton;
                if (overrideText.Length > 0) { linkText = overrideText; }
                //base.Render(writer);
                writer.Write("<a class=\"janrainEngage\" href=\"#\">" + linkText + "</a>");
                
            }


        }

        private void SetupScript()
        {
            if (useOldImplementation)
            {
                SetupScriptOld();
                return;
            }

            if (siteSettings == null) { return; }
            if (string.IsNullOrEmpty(realm)) { return; }
            if (string.IsNullOrEmpty(tokenUrl)) { return; }

            StringBuilder script = new StringBuilder();
            script.Append(Environment.NewLine);

            

            script.Append(Environment.NewLine);

            script.Append("<script type=\"text/javascript\">");

            script.Append(Environment.NewLine);
            script.Append("(function() { ");

            script.Append(Environment.NewLine);

            script.Append("if (typeof window.janrain !== 'object') window.janrain = {}; ");

            //script.Append(Environment.NewLine);

            script.Append("if (typeof window.janrain.settings !== 'object') window.janrain.settings = {}; ");

            script.Append(Environment.NewLine);
            script.Append(Environment.NewLine);

            script.Append("janrain.settings.tokenUrl = '" + tokenUrl + "';");

            script.Append(Environment.NewLine);
            script.Append(Environment.NewLine);

            script.Append("function isReady() { janrain.ready = true; };");

            script.Append(Environment.NewLine);

            script.Append("if (document.addEventListener) {");

            script.Append(Environment.NewLine);

            script.Append("document.addEventListener(\"DOMContentLoaded\", isReady, false);");

            script.Append(Environment.NewLine);

            script.Append("} else {");

            script.Append(Environment.NewLine);

            script.Append("window.attachEvent('onload', isReady);");

            script.Append(Environment.NewLine);

            script.Append("}");

            script.Append(Environment.NewLine);
            script.Append(Environment.NewLine);

            script.Append("var e = document.createElement('script');");

            script.Append(Environment.NewLine);

            script.Append("e.type = 'text/javascript';");

            script.Append(Environment.NewLine);

            script.Append("e.id = 'janrainAuthWidget';");

            script.Append(Environment.NewLine);
            script.Append(Environment.NewLine);

            script.Append("if (document.location.protocol === 'https:') {");

            script.Append(Environment.NewLine);

            script.Append("e.src = 'https://rpxnow.com/js/lib/" + realm + "/engage.js';");

            script.Append(Environment.NewLine);

            script.Append("} else {");

            script.Append(Environment.NewLine);

            script.Append("e.src = 'http://widget-cdn.rpxnow.com/js/lib/" + realm + "/engage.js';");

            script.Append(Environment.NewLine);

            script.Append("}");

            script.Append(Environment.NewLine);
            script.Append(Environment.NewLine);

            script.Append("var s = document.getElementsByTagName('script')[0];");

            script.Append(Environment.NewLine);

            script.Append("s.parentNode.insertBefore(e, s);");

            script.Append(Environment.NewLine);

            //script.Append("alert(erpx.src);");

            script.Append("})();");

            script.Append(Environment.NewLine);

            script.Append("</script>");

            //Literal litScript = new Literal();
            //litScript.ID = "janrainsetup";
            //litScript.Text = script.ToString();
            //Page.Header.Controls.Add(litScript);

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "janrainsignin",
                script.ToString(),
                false);

            //if (SiteUtils.IsSecureRequest())
            //{
            //    Page.ClientScript.RegisterStartupScript(typeof(Page),
            //            "rpxmain", "\n<script  src=\"https://rpxnow.com/js/lib/" + realm + "/engage.js\" type=\"text/javascript\" ></script>");
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(typeof(Page),
            //            "rpxmain", "\n<script  src=\"http://widget-cdn.rpxnow.com/js/lib/" + realm + "/engage.js\" type=\"text/javascript\" ></script>");
            //}


            //Page.ClientScript.RegisterStartupScript(typeof(Page),
            //        "rpxmain", "\n<script  src=\"https://rpxnow.com/openid/v2/widget\" type=\"text/javascript\" ></script>");

            //string overlay = "false";
            //if (useOverlay) { overlay = "true"; }

            //Page.ClientScript.RegisterStartupScript(typeof(Page),
            //        "rpxsetup", "\n<script  type=\"text/javascript\">"
            //        + " RPXNOW.token_url = \"" + tokenUrl
            //        + "\"; RPXNOW.realm = \"" + realm
            //        + "\"; RPXNOW.overlay = " + overlay + "; RPXNOW.language_preference = '"
            //        + GetLanguageCode() + "'; </script>");

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            //if (siteSettings.SiteRoot.Length > 0)
            //{
            //    siteRoot = siteSettings.SiteRoot;
            //}
            //else
            //{
                siteRoot = SiteUtils.GetNavigationSiteRoot();
            //}

            tokenUrl = siteRoot + "/Secure/OpenIdRpxHandler.aspx";

            string returnUrl = SiteUtils.GetReturnUrlParam(Page, siteRoot);
            if(returnUrl.Length > 0)
            {
                tokenUrl += "?returnurl=" + Page.Server.UrlEncode(returnUrl);
            }
            else if (autoDetectReturnUrl)
            {
                tokenUrl += "?returnurl=" + Page.Server.UrlEncode(WebUtils.ResolveServerUrl(Page.Request.RawUrl));
            }


            realm = siteSettings.RpxNowApplicationName;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    realm = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            if (!useOldImplementation)
            {
                realm = realm.Replace(".rpxnow.com", string.Empty);
            }

            this.Text = Resource.OpenIDLoginButton;

            if (overrideText.Length > 0) { this.Text = overrideText; }

            this.CssClass = "rpxnow openid_login";
            this.Attributes.Add("onclick", "return false;");
            this.NavigateUrl = "https://" + realm + ".rpxnow.com/openid/v2/signin?token_url=" + Page.Server.UrlEncode(tokenUrl);

        }


        



        //old implementation


        protected void RenderOld(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(realm)) { return; }
            if (string.IsNullOrEmpty(tokenUrl)) { return; }

            if (embed)
            {
                writer.Write("<iframe src=\"");
                writer.Write("https://" + realm + "/openid/embed?token_url=" + Page.Server.UrlEncode(tokenUrl)
                    + "&amp;language_preference=" + GetLanguageCode() + "\"");
                writer.Write(" scrolling=\"no\" frameBorder=\"no\" style=\"width:400px;height:240px;\"></iframe>");


            }
            else
            {
                base.Render(writer);
            }


        }

        private void SetupScriptOld()
        {
            if (siteSettings == null) { return; }
            if (string.IsNullOrEmpty(realm)) { return; }
            if (string.IsNullOrEmpty(tokenUrl)) { return; }

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "rpxmain", "\n<script  src=\"https://rpxnow.com/openid/v2/widget\" type=\"text/javascript\" ></script>");

            string overlay = "false";
            if (useOverlay) { overlay = "true"; }

            Page.ClientScript.RegisterStartupScript(typeof(Page),
                    "rpxsetup", "\n<script  type=\"text/javascript\">"
                    + " RPXNOW.token_url = \"" + tokenUrl
                    + "\"; RPXNOW.realm = \"" + realm
                    + "\"; RPXNOW.overlay = " + overlay + "; RPXNOW.language_preference = '"
                    + GetLanguageCode() + "'; </script>");

        }

        //private void LoadSettings()
        //{
        //    siteSettings = CacheHelper.GetCurrentSiteSettings();
        //    if (siteSettings == null) { return; }

        //    if (siteSettings.SiteRoot.Length > 0)
        //    {
        //        siteRoot = siteSettings.SiteRoot;
        //    }
        //    else
        //    {
        //        siteRoot = SiteUtils.GetNavigationSiteRoot();
        //    }

        //    tokenUrl = siteRoot + "/Secure/OpenIdRpxHandler.aspx";

        //    realm = siteSettings.RpxNowApplicationName;

        //    if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
        //    {
        //        if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
        //        {
        //            realm = WebConfigSettings.OpenIdRpxApplicationName;
        //        }

        //    }

        //    this.Text = Resource.OpenIDLoginButton;

        //    if (overrideText.Length > 0) { this.Text = overrideText; }

        //    this.CssClass = "rpxnow openid_login";
        //    this.Attributes.Add("onclick", "return false;");
        //    this.NavigateUrl = "https://" + realm + ".rpxnow.com/openid/v2/signin?token_url=" + Page.Server.UrlEncode(tokenUrl);

        //}

        private string GetLanguageCode()
        {
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                    // these are the supported languages as of 2009-05-15

                case "ro":
                    return "ro";

                case "hu":
                    return "hu";

                case "it":
                    return "it";

                case "sv":
                    return "sv-SE";

                case "ja":
                    return "ja";

                case "bg":
                    return "bg";

                case "pl":
                    return "pl";

                case "ru":
                    return "ru";

                case "de":
                    return "de";

                case "pt":
                    return "pt-BR";

                case "vi":
                    return "vi";

                case "cs":
                    return "cs";

                case "zh":
                    return "zh";

                case "fr":
                    return "fr";

                case "sr":
                    return "sr";

                case "nl":
                    return "nl";

                case "ko":
                    return "ko";

                case "el":
                    return "el";

                case "es":
                    return "es";

                case "da":
                    return "da";

              
            }

            return "en";
        }


    }
}
