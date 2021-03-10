using System;
using System.Web.UI;
using System.Web.Security;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{
    public partial class ConfirmRegistration : NonCmsBasePage
    {
        private string winliveCookieName;
        private WindowsLiveLogin windowsLive = null;
        private WindowsLiveLogin.User liveUser = null;
        

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
            
        }


        private Guid registrationConfirmationGuid = Guid.Empty;

        private void Page_Load(object sender, System.EventArgs e)
        {
            LoadSettings();
            GetGuidFromQueryString();
            if (this.registrationConfirmationGuid == Guid.Empty)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            SiteUser siteUser = SiteUser.GetByConfirmationGuid(siteSettings, registrationConfirmationGuid);

            if (SiteUser.ConfirmRegistration(this.registrationConfirmationGuid))
            {
                this.lblMessage.Text = Resources.Resource.RegisterConfirmMessage;
                if (siteUser != null) 
                {
                    NewsletterHelper.VerifyExistingSubscriptions(siteUser);
                    NewsletterHelper.ClaimExistingSubscriptions(siteUser);

                    if ((windowsLive != null) && (liveUser != null)) { HandleWindowsLiveConfirmation(siteUser); }

                }

                AnalyticsAsyncTopScript asyncAnalytics = Page.Master.FindControl("analyticsTop") as AnalyticsAsyncTopScript;
                if (asyncAnalytics != null)
                {
                    asyncAnalytics.PageToTrack = "/RegistrationConfirmed.aspx";
                }
                else
                {
                    mojoGoogleAnalyticsScript analytics = Page.Master.FindControl("mojoGoogleAnalyticsScript1") as mojoGoogleAnalyticsScript;
                    if (analytics != null)
                    {
                        analytics.PageToTrack = "/RegistrationConfirmed.aspx";
                    } 
                }
                
                


            }
            else
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }
        }

        private void HandleWindowsLiveConfirmation(SiteUser siteUser)
        {
            if ((liveUser == null) || (windowsLive == null)) { return; }
            

            if (siteSettings.UseEmailForLogin)
            {
                FormsAuthentication.SetAuthCookie(
                    siteUser.Email, liveUser.UsePersistentCookie);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(
                    siteUser.LoginName, liveUser.UsePersistentCookie);
            }

            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid;
                CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), liveUser.UsePersistentCookie);
            }

            if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
            {
                SiteUtils.SetSkinCookie(siteUser);
            }

            siteUser.UpdateLastLoginTime();

            //WebUtils.SetupRedirect(this, SiteRoot + "/Secure/UserProfile.aspx");


        }

        private void GetGuidFromQueryString()
        {
            this.registrationConfirmationGuid = WebUtils.ParseGuidFromQueryString("ticket", Guid.Empty);

            

        }

        private void LoadSettings()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegistrationConfirmation);
            winliveCookieName = "winliveid" + siteSettings.SiteId.ToInvariantString();

            windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();
            if (windowsLive == null) { return; }

            string winLiveToken = CookieHelper.GetCookieValue(winliveCookieName);
            if (winLiveToken.Length > 0)
            {
                liveUser = windowsLive.ProcessToken(winLiveToken);
            }

            AddClassToBody("confirmregistration");
        }


    }
}
