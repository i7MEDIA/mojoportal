/// Author:		        
/// Created:            2007-08-18
/// Last Modified:      2009-07-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class WindowsLiveLoginControl : UserControl
    {
        private string winliveCookieName;
        private string returnUrlCookieName;
        static WindowsLiveLogin windowsLive = null;
        private string windowsLiveAppId;
        private string protocol = "http://";
        //private string LiveUserId;
        private string siteRoot;
        private SiteSettings siteSettings;
        private string backColor = "white";
        private string foreColor = "black";
        

        public string WindowsLiveAppId
        {
            get { return windowsLiveAppId; }
            
        }

        public string Protocol
        {
            get { return protocol; }

        }

        public string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public string ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (windowsLive == null)
            {
                this.Visible = false;
                return;
            }

          

            if (
                (!WebConfigSettings.EnableWindowsLiveAuthentication)
                ||(!siteSettings.AllowWindowsLiveAuth)
                )
            {
                this.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                string returnUrl = WebConfigSettings.PageToRedirectToAfterSignIn;

                if (returnUrl.EndsWith(".aspx"))
                {
                    CookieHelper.SetCookie(returnUrlCookieName, returnUrl);
                    return;
                }

                if (Page.Request.UrlReferrer != null)
                {
                    returnUrl = Page.Request.UrlReferrer.ToString();
                }

                string returnUrlParam = Page.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    returnUrl = Page.ResolveUrl(Page.Server.UrlDecode(returnUrlParam));
                }
                if (returnUrl.Length > 0)
                {
                    CookieHelper.SetCookie(returnUrlCookieName, returnUrl);
                }
            }

        }


        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteRoot = SiteUtils.GetNavigationSiteRoot();
            winliveCookieName = "winliveid" 
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            returnUrlCookieName = "ret"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            litSignInAddendum.Text = Resource.WindowsLiveSignInAddendum;
            if (SiteUtils.SslIsAvailable()) protocol = "https://";

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"].Trim();
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId.Trim(); }
            }

            if (wlAppId.Length > 0)
            {
                try
                {
                    windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();
                    if (windowsLive == null)
                    {
                        this.Visible = false;
                        return;
                    }
                    windowsLiveAppId = windowsLive.AppId;
                }
                catch (ArgumentException ) 
                {
                    windowsLive = null;
                }
            }

            

        }



        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

        }

    }
}