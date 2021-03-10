/// Last Modified:		        2007-01-19 by aleyush
/// 2007-08-25
/// 2008-11-17 by 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web;

namespace mojoPortal.Web.UI.Pages 
{

	
    public partial class Logoff : Page 
	{
        const string WindowsLiveSecurityAlgorithm = "wsignin1.0";
        //private bool forceDelAuthNonProvisioned = true;

        protected void Page_Load(object sender, EventArgs e) 
		{
            DoLogout();
        }

        private void DoLogout()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            string winliveCookieName = "winliveid"
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            string roleCookieName = SiteUtils.GetRoleCookieName(siteSettings);

            HttpCookie roleCookie = new HttpCookie(roleCookieName, string.Empty);
            roleCookie.Expires = DateTime.Now.AddMinutes(1);
            roleCookie.Path = "/";
            Response.Cookies.Add(roleCookie);

            HttpCookie displayNameCookie = new HttpCookie("DisplayName", string.Empty);
            displayNameCookie.Expires = DateTime.Now.AddMinutes(1);
            displayNameCookie.Path = "/";
            Response.Cookies.Add(displayNameCookie);

            // apparently we need this here for folder sites using windows auth
            //https://www.mojoportal.com/Forums/EditPost.aspx?thread=13195&forumid=2&mid=34&pageid=5&pagenumber=1
            CookieHelper.ExpireCookie("siteguid" + siteSettings.SiteGuid);
           
            if (WebConfigSettings.UseFolderBasedMultiTenants && !WebConfigSettings.UseRelatedSiteMode)
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid.ToString();

                HttpCookie siteCookie = new HttpCookie(cookieName, string.Empty);
                siteCookie.Expires = DateTime.Now.AddMinutes(1);
                siteCookie.Path = "/";
                Response.Cookies.Add(siteCookie);

                CookieHelper.ExpireCookie("siteguid" + siteSettings.SiteGuid);

            }
            else
            {
                FormsAuthentication.SignOut();
            }

            string winLiveToken = CookieHelper.GetCookieValue(winliveCookieName);
            WindowsLiveLogin.User liveUser = null;
            if (winLiveToken.Length > 0)
            {
                WindowsLiveLogin windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();

                try
                {
                    liveUser = windowsLive.ProcessToken(winLiveToken);
                    if (liveUser != null)
                    {
                        Response.Redirect(windowsLive.GetLogoutUrl());
                        Response.End();

                    }
                }
                catch (InvalidOperationException)
                {
                }

            }

            try
            {
                if (Session != null)
				{
					Session.Clear();
					Session.Abandon();
				}
            }
            catch (HttpException) { }

            string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx";

            if (!siteSettings.UseSslOnAllPages)
            {
                if (redirectUrl.StartsWith("https:"))
                {
                    redirectUrl = redirectUrl.Replace("https:", "http:");
                }
            }

            WebUtils.SetupRedirect(this, redirectUrl);
        


        }
    }
}
