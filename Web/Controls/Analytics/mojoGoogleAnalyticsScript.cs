///	Author:				
///	Created:			2008-08-11
///	Last Modified:		2010-03-17
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
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls;
using Resources;


namespace mojoPortal.Web.UI
{
    public class mojoGoogleAnalyticsScript : GoogleAnalyticsScript
    {
        public const string mojoPageTrackerName = "mojoPageTracker";
        private SiteSettings siteSettings = null;
        private bool didInit = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            DoInit();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (HttpContext.Current == null) { return; }
            if (!didInit) { DoInit(); }
            base.OnPreRender(e);
        }

        private void DoInit()
        {
            if (HttpContext.Current == null) { return; }

            this.EnableViewState = false;

            didInit = true;
            // we need a known tracker name so we can call it from various places
            TrackerName = mojoPageTrackerName;
            
            MemberType = WebConfigSettings.GoogleAnalyticsMemberLabel;
            MemberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAnonymous;
            

            // lets always label Admins as admins, regardless whether they are also customers
            if (WebUser.IsAdminOrContentAdmin)
            {
                MemberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAdmin;
            }
            else
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    if ((siteUser != null) && (siteUser.TotalRevenue > 0))
                    {
                        MemberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeCustomer;
                    }
                    else
                    {
                        MemberLabel = WebConfigSettings.GoogleAnalyticsMemberTypeAuthenticated;
                    }

                }
            }

            TrackPageLoadTime = WebConfigSettings.TrackPageLoadTimeInGoogleAnalytics;
            LogToLocalServer = WebConfigSettings.LogGoogleAnalyticsDataToLocalWebLog;

            if (WebConfigSettings.GoogleAnalyticsScriptOverrideUrl.Length > 0)
            {
                OverrideScriptUrl = WebConfigSettings.GoogleAnalyticsScriptOverrideUrl;
            }

            // do some setup before the base control runs
            // let Web.config setting trump site settings. this meets my needs where I want to track the demo site but am letting people login as admin
            // this way if the remove or change it in site settings it still uses my profile id
            if (ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"] != null)
            {
                GoogleAnalyticsProfileId = ConfigurationManager.AppSettings["GoogleAnalyticsProfileId"].ToString();
                return;

            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if ((siteSettings != null) && (siteSettings.GoogleAnalyticsAccountCode.Length > 0))
            {
                GoogleAnalyticsProfileId = siteSettings.GoogleAnalyticsAccountCode;

            }
        }
    }
}
