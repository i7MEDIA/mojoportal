//	Author:				
//	Created:			2010-03-15
//	Last Modified:		2013-12-31
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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls.google;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// 
    /// 
    /// This control is used in conjunction with AnalyticsAsyncTopScript and in replacement for mojoGoogleAnalyticsScript
    /// if you want to use the async loading approach described here:
    /// http://code.google.com/apis/analytics/docs/tracking/asyncUsageGuide.html#SplitSnippet
    /// 
    /// Remove mojoGoogleAnalyticsScript from your layout.master and replace with AnalyticsAsyncBottomScript just before the closing form element
    /// Add AnalyticsAsyncTopScript to layout.master just below the opening body element
    /// </summary>
    public class AnalyticsAsyncBottomScript : WebControl
    {
        private SiteSettings siteSettings = null;
        private string googleAnalyticsProfileId = string.Empty;
        private string overrideScriptUrl = string.Empty;


		/// <summary>
		/// This control should no longer be used and will be removed in a future version.
		/// Google Analytics completely changed and this control only ever support GA.
		/// This control has been replaced with logic in ScriptLoader.
		/// </summary>
		public bool Disable { get; set; } = true;

		/// <summary>
		/// if you want to host the script locally put the path for the src=
		/// </summary>
		public string OverrideScriptUrl
        {
            get { return overrideScriptUrl; }
            set { overrideScriptUrl = value; }
        }

        private bool useUniversal = false; // false for backward compat but set to true in layout.master of newer skins

        public bool UseUniversal
        {
            get { return useUniversal; }
            set { useUniversal = value; }
        }

        

        /// <summary>
        /// legacy
        /// </summary>
        /// <param name="writer"></param>
        private void SetupMainScript(HtmlTextWriter writer)
        {
            writer.Write("\n<script type=\"text/javascript\"> ");
            writer.Write("\n");
            writer.Write("(function() {");
            writer.Write("\n");
            writer.Write("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; ");
            writer.Write("\n");

            if (overrideScriptUrl.Length > 0)
            {
                writer.Write("ga.src = '" + overrideScriptUrl + "';");
            }
            else
            {
                writer.Write("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            }

            writer.Write("\n");
            writer.Write("(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(ga);");

            writer.Write("\n");
            writer.Write("})();");

            writer.Write("\n");
            writer.Write("</script>");

        }

        protected override void OnInit(EventArgs e)
        {
			base.OnInit(e);

            if (HttpContext.Current == null) { return; }

            this.EnableViewState = false;

            if (WebConfigSettings.GoogleAnalyticsScriptOverrideUrl.Length > 0)
            {
                overrideScriptUrl = WebConfigSettings.GoogleAnalyticsScriptOverrideUrl;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (siteSettings.GoogleAnalyticsAccountCode.Length > 0))
            {
                googleAnalyticsProfileId = siteSettings.GoogleAnalyticsAccountCode;

            }

            // let Web.config setting trump site settings. this meets my needs where I want to track the demo site but am letting people login as admin
            // this way if the remove or change it in site settings it still uses my profile id
            if (ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"] != null)
            {
                googleAnalyticsProfileId = ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"].ToString();
            }

            if (!googleAnalyticsProfileId.StartsWith("UA") || !useUniversal)
            {
                Disable = true;
            }
		}

		protected override void OnLoad(EventArgs e)
        {
			if (Disable) return;

			base.OnLoad(e);

            if (WebConfigSettings.GoogleAnalyticsRolesToExclude.Length > 0)
            {
                if (WebUser.IsInRoles(WebConfigSettings.GoogleAnalyticsRolesToExclude))
                {
                    Visible = false; //prevent rendering
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
			if (Disable) return;

			if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (googleAnalyticsProfileId.Length == 0) { return; }

                if (!useUniversal) // for Universal analytics we don't need this bottom control to do anything, it is all in the top one
                {
                    SetupMainScript(writer);
                }
            }
        }
    }
}