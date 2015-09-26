///	Created:			    2009-04-15
///	Last Modified:		    2009-05-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
    public static class WindowsLiveHelper
    {

        public static WindowsLiveLogin GetWindowsLiveLogin()
        {
            if (!WebConfigSettings.EnableWindowsLiveAuthentication) { return null; }

            string WindowsLiveSecurityAlgorithm = "wsignin1.0";
            bool forceDelAuthNonProvisioned = true;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return null; }

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string privacyPolicyUrl;
            if (siteSettings.PrivacyPolicyUrl.StartsWith("http"))
            {
                privacyPolicyUrl = siteSettings.PrivacyPolicyUrl;
            }
            else
            {
                privacyPolicyUrl = siteRoot + siteSettings.PrivacyPolicyUrl;
            }

            string returnUrl = siteRoot + "/Secure/WindowsLiveAuthHandler.aspx";
            string appId = siteSettings.WindowsLiveAppId;
            string appKey = siteSettings.WindowsLiveKey;

            if (SiteUtils.SslIsAvailable())
            {
                if (returnUrl.StartsWith("http://"))
                {
                    returnUrl = returnUrl.Replace("http://", "https://");
                }
            }

            // I use this forthe demo site since I let people log in admin but don't want them to change the setting in siteSettigns
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                appId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (appId.Length == 0) { appId = siteSettings.WindowsLiveAppId; }
            }

            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"] != null)
            {
                appKey = ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"];
                if (appKey.Length == 0) { appKey = siteSettings.WindowsLiveKey; }
            }


            if (
                (appId.Length > 0)
                && (appKey.Length > 0)
                )
            {
                WindowsLiveLogin windowsLive = new WindowsLiveLogin(
                    appId,
                    appKey,
                    WindowsLiveSecurityAlgorithm,
                    forceDelAuthNonProvisioned,
                    privacyPolicyUrl,
                    returnUrl);

                windowsLive.AppName = siteSettings.SiteName;
                if (siteSettings.AppLogoForWindowsLive.Length > 0)
                {
                    windowsLive.AppLogoUrl = siteRoot + siteSettings.AppLogoForWindowsLive;
                }

                return windowsLive;

            }




            return null;
        }
    }
}
