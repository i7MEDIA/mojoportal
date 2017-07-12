//  Author:                     
//  Created:                    2012-06-12
//	Last Modified:              2012-06-13
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://developers.facebook.com/docs/reference/javascript/
    /// </summary>
    public class FacebookSdk : WebControl
    {
        private SiteSettings siteSettings = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;
        }

        private bool alwaysRender = false;

        public bool AlwaysRender
        {
            get { return alwaysRender; }
            set { alwaysRender = value; }
        }

        private bool addInit = false;

        public bool AddInit
        {
            get { return addInit; }
            set { addInit = value; }
        }

        private string facebookAppId = string.Empty;

        public string FacebookAppId
        {
            get { return facebookAppId; }
            set { facebookAppId = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);

            if (!ShouldRender()) { return; }

            StringBuilder s = new StringBuilder();

            s.Append("\n<div id=\"fb-root\"></div>");
            s.Append("\n <script type='text/javascript'>\n ");

            if ((addInit) &&(facebookAppId.Length > 0))
            {
                s.Append("window.fbAsyncInit = function() {");
                s.Append("FB.init({");
                s.Append("appId:'" + facebookAppId + "'");
                s.Append(",channelUrl:'" + GetChannelUrl() + "'");
                s.Append(",status:true");
                s.Append(",cookie:true");
                s.Append(",xfbml:true");
                s.Append("}); ");


                s.Append("}; ");
            }

            s.Append("\n (function(d, s, id) {");
            s.Append("\n var js, fjs = d.getElementsByTagName(s)[0];");
            s.Append("\n if (d.getElementById(id)) return;");
            s.Append("\n js = d.createElement(s); js.id = id;");

            // the use of // ensures the protocol adapts for ssl
            s.Append("\n js.src = \"//connect.facebook.net/en_US/all.js#xfbml=1&appId=" + facebookAppId + "\";");

            s.Append("\n fjs.parentNode.insertBefore(js, fjs);");
            s.Append("\n }(document, 'script', 'facebook-jssdk'));</script>");

            writer.Write(s.ToString());
        }

        private string GetChannelUrl()
        {
            return WebUtils.ResolveServerUrl(SiteUtils.GetNavigationSiteRoot() + "/channel.ashx").Replace("https://", "//").Replace("http://", "//");
        }

        private bool ShouldRender()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return false; }

            facebookAppId = siteSettings.FacebookAppId;
            if (WebConfigSettings.FacebookAppId.Length > 0)
            {
                facebookAppId = WebConfigSettings.FacebookAppId;
            }

            if (facebookAppId.Length == 0) { return false; }

            if (alwaysRender) { return true; }

            if (Page is NonCmsBasePage) { return false; }

            if(Page is CmsPage)
            {
                if (siteSettings.CommentProvider != "facebook") { return false; }

                PageSettings currentPage = CacheHelper.GetCurrentPage();
                if (currentPage == null) { return false; }
                if (currentPage.EnableComments)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            


            return false;
        }

    }
}