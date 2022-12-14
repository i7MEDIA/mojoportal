//	Created:			    2008-08-18
//	Last Modified:		    2016-01-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// allows you to inclde a javascript file form the skin folder by just its name, the path to the skin folder will be resolved.
    /// </summary>
    public class SkinFolderScript : WebControl
    {
        private string scriptFileName = string.Empty;

        public string ScriptFileName
        {
            get { return scriptFileName; }
            set { scriptFileName = value; }
        }

        private string scriptFullUrl = string.Empty;

        public string ScriptFullUrl
        {
            get { return scriptFullUrl; }
            set { scriptFullUrl = value; }
        }


        private bool useSkinVersion = false;
        /// <summary>
        /// If true the skin version guid will be appended to the script url. This helps for controlling caching of the script.
        /// </summary>
        public bool UseSkinVersion
        {
            get { return useSkinVersion; }
            set { useSkinVersion = value; }
        }

        private bool isStartup = false;

        public bool IsStartup
        {
            get { return isStartup; }
            set { isStartup = value; }
        }

        private bool addToCombinedScript = false;

        public bool AddToCombinedScript
        {
            get { return addToCombinedScript; }
            set { addToCombinedScript = value; }
        }

        private string visibleRoles = string.Empty;
        /// <summary>
        /// a semi colon separated list of role names
        /// Admins;Content Administrators
        /// </summary>
        public string VisibleRoles
        {
            get { return visibleRoles; }
            set { visibleRoles = value; }
        }

        private string visibleUrls = string.Empty;
        /// <summary>
        /// a comma separated list of relative urls where the script file should be used
        /// if specified then the link will only be rendered if the current Request.RawUrl contains on of the specified values
        /// /Admin,/HtmlEdit.aspx would add the css only on pages in the Admin folder and on the HtmlEdit.aspx page in the root
        /// </summary>
        public string VisibleUrls
        {
            get { return visibleUrls; }
            set { visibleUrls = value; }
        }

        private bool renderInPlace = false;
        /// <summary>
        /// if true script will be rendered in same location as SkinFolderScript control
        /// </summary>
        public bool RenderInPlace
        {
            get { return renderInPlace; }
            set { renderInPlace = value; }
        }

        private string scriptRefFormat = "<script src=\"{0}\" type=\"text/javascript\" data-loader=\"skinfolderscript\"></script>";
        public string ScriptRefFormat
        {
            get { return scriptRefFormat; }
            set { scriptRefFormat = "\n" + value; }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (scriptFileName.Length == 0) { return; }

            if (ShouldRender() && !renderInPlace)
            { 
                SetupScript();
            }
            
        }

        private void SetupScript()
        {
            if (scriptFullUrl.Length > 0)
            {
                if (isStartup)
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        typeof(Page),
                        ClientID, 
                        string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptFullUrl + GetSkinVersionGuidQueryParam(scriptFullUrl)),
                        false);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(
                        this,
                        typeof(Page),
                        ClientID, 
                        string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptFullUrl + GetSkinVersionGuidQueryParam(scriptFullUrl)),
                        false);
                }
                return;
               
            }

            if (scriptFileName.Length == 0) { return; }
            string scriptUrl = SiteUtils.DetermineSkinBaseUrl(true, WebConfigSettings.UseFullUrlsForSkins, Page) + scriptFileName + GetSkinVersionGuidQueryParam(scriptFileName);

            if (isStartup)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    typeof(Page),
                    scriptFileName, 
                    string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptUrl),
                    false);
            }
            else
            {
                if (addToCombinedScript && (scriptUrl.StartsWith("/")) && (Page is mojoBasePage))
                {
                    mojoBasePage basePage = Page as mojoBasePage;
                    basePage.ScriptConfig.AddPathScriptReference(scriptUrl);

                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(
                        this,
                        typeof(Page),
                        scriptFileName, 
                        string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptUrl),
                        false);
                }
            }
           


        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (!renderInPlace || !ShouldRender()) { return; }
            
            
            if (scriptFullUrl.Length > 0)
            {
                writer.Write(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptFullUrl + GetSkinVersionGuidQueryParam(scriptFullUrl)));
            }
            else
            {
                string scriptUrl = SiteUtils.DetermineSkinBaseUrl(true, WebConfigSettings.UseFullUrlsForSkins, Page) + scriptFileName + GetSkinVersionGuidQueryParam(scriptFileName);
                writer.Write(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptUrl));
            }
           
        }

        protected bool ShouldRender()
        {
            if (visibleRoles.Length > 0)
            {
                if (!WebUser.IsInRoles(visibleRoles))
                {
                    return false;
                }
            }

            if (visibleUrls.Length > 0)
            {
                bool match = false;
                List<string> allowedUrls = visibleUrls.SplitOnChar(',');
                foreach (string u in allowedUrls)
                {
                    //Page.AppRelativeVirtualPath will match for things like blog posts where the friendly url is something like /my-cool-post which
                    //is then mapped to the /Blog/ViewPost.aspx page. So, one could use /Blog/ViewPost.aspx in the AllowedUrls property to render
                    //a script on blog post pages.
                    if (Page.AppRelativeVirtualPath.ContainsCaseInsensitive(u)) { match = true; }

                    //Page.Request.RawUrl is the url used for the request, as in the example above '/my-cool-post'
                    if (Page.Request.RawUrl.ContainsCaseInsensitive(u)) { match = true; }
                }

                if (!match) { return false; }

            }

            return true;
            
        }

        private string GetSkinVersionGuidQueryParam(string url)
        {
            if (useSkinVersion)
            {
                string sep = url.IndexOf('?') >= 0 ? "&" : "?";
                
                return sep + CacheHelper.GetCurrentSiteSettings().SkinVersion.ToString();
            }

            return string.Empty;
            

        }
    }
}
