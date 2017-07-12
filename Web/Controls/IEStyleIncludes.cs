//	Author:					
//	Created:				2007-05-13
//	Last Modified:		    2012-06-18
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    public class IEStyleIncludes : WebControl
    {
        //http://msdn.microsoft.com/en-us/library/ms537512(VS.85).aspx

        private bool includeHtml5Script = false;

        public bool IncludeHtml5Script
        {
            get { return includeHtml5Script; }
            set { includeHtml5Script = value; }
        }

        private bool includeIE6 = true;

        /// <summary>
        /// if true will include IE6Specific.css from the skin folder
        /// </summary>
        public bool IncludeIE6
        {
            get { return includeIE6; }
            set { includeIE6 = value; }
        }

        private bool includeIE7 = true;
        /// <summary>
        /// if true will include IE7Specific.css from the skin folder
        /// </summary>
        public bool IncludeIE7
        {
            get { return includeIE7; }
            set { includeIE7 = value; }
        }

        private bool includeIE9 = false;

        /// <summary>
        /// if true will include IE8Specific.css from the skin folder
        /// </summary>
        public bool IncludeIE9
        {
            get { return includeIE9; }
            set { includeIE9 = value; }
        }

        private bool includeIE8 = false;

        /// <summary>
        /// if true will include IE8Specific.css from the skin folder
        /// </summary>
        public bool IncludeIE8
        {
            get { return includeIE8; }
            set { includeIE8 = value; }
        }

        private bool includeIE7Script = false;
        public bool IncludeIE7Script
        {
            get { return includeIE7Script; }
            set { includeIE7Script = value; }
        }


        private bool includeIE8Script = false;
        public bool IncludeIE8Script
        {
            get { return includeIE8Script; }
            set { includeIE8Script = value; }
        }

        private bool includeIETransitionMeta = false;
        /// <summary>
        /// if true includes:
        /// <meta http-equiv="Page-Enter" content="blendTrans(Duration=0)" /><meta http-equiv="Page-Exit" content="blendTrans(Duration=0)" />
        /// </summary>
        public bool IncludeIETransitionMeta
        {
            get { return includeIETransitionMeta; }
            set { includeIETransitionMeta = value; }
        }

        private string ie6CssFile = "IESpecific.css";

        public string IE6CssFile
        {
            get { return ie6CssFile; }
            set { ie6CssFile = value; }
        }

        private string ie7CssFile = "IE7Specific.css";

        public string IE7CssFile
        {
            get { return ie7CssFile; }
            set { ie7CssFile = value; }
        }

        private string ie8CssFile = "IE8Specific.css";

        public string IE8CssFile
        {
            get { return ie8CssFile; }
            set { ie8CssFile = value; }
        }

        private string ie9CssFile = "style.ie9.css";

        public string IE9CssFile
        {
            get { return ie9CssFile; }
            set { ie9CssFile = value; }
        }

        private string overrideCssBaseUrl = string.Empty;

        public string OverrideCssBaseUrl
        {
            get { return overrideCssBaseUrl; }
            set { overrideCssBaseUrl = value; }
        }

        private string cacheBuster = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }
            if (!BrowserHelper.IsIE()) { return; }
            
            bool allowPageOverride = true;
            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = (mojoBasePage)Page;
                allowPageOverride = basePage.AllowSkinOverride;
                cacheBuster = "?sv=" + basePage.SiteInfo.SkinVersion.ToString();
            }

            string skinBaseUrl = overrideCssBaseUrl;
            if (skinBaseUrl.Length == 0) { skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(true, false, Page); }

            string scriptBaseUrl = ResolveUrl("~/ClientScript/");

            if (!WebConfigSettings.CacheCssInBrowser) { cacheBuster += "&amp;cb=" + Guid.NewGuid().ToString(); }

            if (includeHtml5Script)
            {
                writer.Write("\n<!--[if IE]>\n");

                writer.Write("<script src=\"" + scriptBaseUrl + "html5shiv.js?v=2\" type=\"text/javascript\"></script>\n");
                
                writer.Write("\n<![endif]-->");

            }

            // IE 6
            if ((includeIE6)&&(BrowserHelper.IsIE6()))
            {
                writer.Write("\n<!--[if lt IE 7]>\n");
                if (includeIE7Script)
                {
                    writer.Write("<script defer=\"defer\" src=\"" + scriptBaseUrl + "IE7.js\" type=\"text/javascript\"></script>\n");
                }

                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie6CssFile + cacheBuster + "\" type=\"text/css\" id=\"IE6CSS\" />\n");

                writer.Write("<![endif]-->\n");
            }


            if ((includeIE7)&&(BrowserHelper.IsIE7()))
            {
                writer.Write("<!--[if IE 7]>\n");
                if (includeIE8Script)
                {
                    writer.Write("<script defer=\"defer\" src=\"" + scriptBaseUrl + "IE8.js\" type=\"text/javascript\"></script>\n");
                }

                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie7CssFile + cacheBuster + "\" type=\"text/css\" id=\"IE7CSS\" />\n");
                writer.Write("<![endif]-->\n");
            }

            if ((includeIE8)&&(BrowserHelper.IsIE8()))
            
            {
                writer.Write("<!--[if IE 8]>\n");
                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie8CssFile + cacheBuster + "\" type=\"text/css\" id=\"IE8CSS\" />\n");
                writer.Write("<![endif]-->\n");

            }

            if ((includeIE9)&&(BrowserHelper.IsIE9()))
            {
                writer.Write("<!--[if IE 9]>\n");
                writer.Write("<link rel=\"stylesheet\" href=\"" + skinBaseUrl + ie9CssFile + cacheBuster + "\" type=\"text/css\" id=\"IE9CSS\" />\n");
                writer.Write("<![endif]-->\n");

            }

            if (includeIETransitionMeta)
            {
                writer.Write("\n<meta http-equiv=\"Page-Enter\" content=\"blendTrans(Duration=0)\" /><meta http-equiv=\"Page-Exit\" content=\"blendTrans(Duration=0)\" />");
            }

                

        }

        
    }
}
