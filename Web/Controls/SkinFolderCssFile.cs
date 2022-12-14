//  Author:                 
//	Created:			    2013-11-23
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
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{

    /// <summary>
    /// this control can be used in layout.master to load an extra css file
    /// you can specify roles for whom the extra css link will be rendered
    /// if left blank then it will always render provided that either CssFileName or CssFullUrl are specified
    /// requested here:
    /// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12248~1#post50892
    /// 
    /// Example usage:
    /// <portal:SkinFolderCssFile id="css1" runat="server" CssFile="admin.css" VisibleRoles="Admins;Content Administrators" />
    /// where admin.css is an extra file in the skin folder that is not combined in the main css
    /// </summary>
    public class SkinFolderCssFile : WebControl
    {
        private string cssFileName = string.Empty;

        public string CssFileName
        {
            get { return cssFileName; }
            set { cssFileName = value; }
        }

        private string cssFullUrl = string.Empty;
        /// <summary>
        /// if specified this is used instead of CssFileName. for using css file that is not in the skin folder
        /// use a protocol relative url //www.somedomain.com/style.css
        /// </summary>
        public string CssFullUrl
        {
            get { return cssFullUrl; }
            set { cssFullUrl = value; }
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
        /// a comma separated list of relative urls where the extra css file shold be used
        /// if specified then the link will only be rendered if the current Request.RawUrl contains on of the specified values
        /// /Admin,/HtmlEdit.aspx would add the css only on pages in the Admin folder and on the HtmlEdit.aspx page in the root
        /// </summary>
        public string VisibleUrls
        {
            get { return visibleUrls; }
            set { visibleUrls = value; }
        }

        private string linkFormat = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" data-loader=\"skinfoldercss\">";

        public string LinkFormat
        {
            get { return linkFormat; }
            set { linkFormat = "\n" + value; }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (cssFileName.Length == 0) { Visible = false; }

            if (visibleRoles.Length > 0)
            {
                if(!WebUser.IsInRoles(visibleRoles))
                {
                    Visible = false;
                }
            }

            if (visibleUrls.Length > 0)
            {
                bool match = false;
                List<string> allowedUrls = visibleUrls.SplitOnChar(',');
                foreach(string u in allowedUrls)
                {
                    //Page.AppRelativeVirtualPath will match for things like blog posts where the friendly url is something like /my-cool-post which
                    //is then mapped to the /Blog/ViewPost.aspx page. So, one could use /Blog/ViewPost.aspx in the AllowedUrls property to render
                    //a css file on blog post pages.
                    if (Page.AppRelativeVirtualPath.ContainsCaseInsensitive(u)) { match = true; }

                    //Page.Request.RawUrl is the url used for the request, as in the example above '/my-cool-post'
                    if (Page.Request.RawUrl.ContainsCaseInsensitive(u)) { match = true;}
                }
                Visible = match;

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            if (!Visible) { return; }

            if(cssFullUrl.Length > 0)
            {
                writer.Write(string.Format(CultureInfo.InvariantCulture, linkFormat, cssFullUrl));
                return;
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            
            string cssUrl = SiteUtils.DetermineSkinBaseUrl(true, WebConfigSettings.UseFullUrlsForSkins, Page)
                + cssFileName + "?v=" + siteSettings.SkinVersion.ToString();

            writer.Write(string.Format(CultureInfo.InvariantCulture, linkFormat, cssUrl));
        }

    }
}