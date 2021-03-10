/// Author:		        
/// Created:            2007-08-17
/// Last Modified:      2009-05-15
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
using System.Web.UI;
using log4net;
using mojoPortal.Web.Framework;
#if !MONO
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
#endif
using System.Web.Security;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Configuration;
using Resources;

namespace mojoPortal.Web
{
    
    public partial class OpenIdLoginControl : UserControl
    {
        private SiteSettings siteSettings;
        private string siteRoot = string.Empty;
        private static readonly ILog log
            = LogManager.GetLogger(typeof(OpenIdLoginControl));

        private string returnUrlCookieName;

        protected void Page_Load(object sender, EventArgs e)
        {
#if !MONO
            
            if (!WebConfigSettings.EnableOpenIdAuthentication)
            {
                this.Visible = false;
                return;
            }

            LoadSettings();

            PopulateLabels();

            

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
                    string urlReferrer = Page.Request.UrlReferrer.ToString();
                    if ((urlReferrer.StartsWith(siteRoot)) || (urlReferrer.StartsWith(siteRoot.Replace("https://", "http://"))))
                    {
                        returnUrl = urlReferrer;
                    }

                    
                }

                string returnUrlParam = Page.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
                    string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
                    if ((redirectUrl.StartsWith(siteRoot)) || (redirectUrl.StartsWith(siteRoot.Replace("https://", "http://"))))
                    {
                        returnUrl = redirectUrl;
                    }
                }

                //string returnUrlParam = Page.Request.Params.Get("returnurl");
                //if (!String.IsNullOrEmpty(returnUrlParam))
                //{



                //    returnUrl = Page.ResolveUrl(Page.Server.UrlDecode(returnUrlParam));
                //}

                if (returnUrl.Length > 0)
                {
                    CookieHelper.SetCookie(returnUrlCookieName, returnUrl);
                }
            }


#endif
        }

        
#if !MONO
        protected void OpenIdLogin1_LoggedIn(object sender, OpenIdEventArgs e)
        {
            // prevent the base control from doing forms auth for us
            e.Cancel = true;

            Guid userGuid = SiteUser.GetUserGuidFromOpenId(
                siteSettings.SiteId, 
                e.ClaimedIdentifier.ToString());

            if (userGuid == Guid.Empty)
            {
                // if enough info is available auto create user
                DoNewUserLogic(e);
            }
            else
            {
                DoExistingUserLogic(userGuid);
            }

        }

        private void DoExistingUserLogic(Guid userGuid)
        {
            // user found so login if allowed
            SiteUser user = new SiteUser(siteSettings, userGuid);

            if (
                (siteSettings.UseSecureRegistration)
                && (user.RegisterConfirmGuid != Guid.Empty)
                )
            {
                Notification.SendRegistrationConfirmationLink(
                    SiteUtils.GetSmtpSettings(),
                    ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                    siteSettings.DefaultEmailFromAddress,
                    siteSettings.DefaultFromEmailAlias,
                    user.Email,
                    siteSettings.SiteName,
                    WebUtils.GetSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
                    user.RegisterConfirmGuid.ToString());

                lblError.Text = Resource.LoginUnconfirmedEmailMessage;
                log.Info("User " + user.Name + " tried to login but email address is not confirmed.");

                return;
            }

            if (user.IsLockedOut)
            {
                lblError.Text = Resource.LoginAccountLockedMessage;
                log.Info("User " + user.Name + " tried to login but account is locked.");

                return;
            }


            if (siteSettings.UseEmailForLogin)
            {
                FormsAuthentication.SetAuthCookie(
                    user.Email, true);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(
                    user.LoginName, true);
            }

            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid;
                CookieHelper.SetCookie(cookieName, user.UserGuid.ToString(), true);
            }

            user.UpdateLastLoginTime();

            // track user ip address
            UserLocation userLocation = new UserLocation(user.UserGuid, SiteUtils.GetIP4Address());
            userLocation.SiteGuid = siteSettings.SiteGuid;
            userLocation.Hostname = Page.Request.UserHostName;
            userLocation.Save();

            string redirectUrl = GetRedirectPath();
            CookieHelper.ExpireCookie(returnUrlCookieName);

            UserSignInEventArgs u = new UserSignInEventArgs(user);
            OnUserSignIn(u);

            WebUtils.SetupRedirect(this, redirectUrl);
            return;

        }

        private void DoNewUserLogic(OpenIdEventArgs e)
        {
            if (e == null) { return; }

            ClaimsResponse claim = e.Response.GetExtension<ClaimsResponse>();
            if (claim == null) { return; }

            if (IsValidForUserCreation(e, claim))
            {
                if (SiteUser.EmailExistsInDB(siteSettings.SiteId, claim.Email))
                {
                    // show message that user should login and associate 
                    // their open id account on their profile page.
                    lblError.Text = Resource.OpenIDRegisterUserEmailExistsMessage;
                    return;
                }
                else
                {
                    // create user automagically since we have all 
                    // the needed data
                    SiteUser newUser = new SiteUser(siteSettings);
                    newUser.Email = claim.Email;
                    newUser.Name = claim.FullName;
                    string loginName = newUser.Name.Replace(" ", ".").ToLower();
                    if (loginName.Length > 50) loginName = loginName.Substring(0, 50);

                    if (SiteUser.LoginExistsInDB(
                        siteSettings.SiteId, loginName))
                    {
                        loginName = e.ClaimedIdentifier.ToString().Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("/", string.Empty);
                        if (loginName.Length > 50) loginName = loginName.Substring(0, 50);

                        int i = 1;
                        while (SiteUser.LoginExistsInDB(
                            siteSettings.SiteId, loginName))
                        {
                            loginName += i.ToString();
                            if (loginName.Length > 50) loginName = loginName.Remove(40, 1);
                            i++;

                        }

                    }

                    newUser.LoginName = loginName;
                    newUser.Password = SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars);
                    newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
                    newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
                    newUser.OpenIdUri = e.ClaimedIdentifier.ToString();
                    newUser.Save();
                    if (siteSettings.UseSecureRegistration)
                    {
                        newUser.SetRegistrationConfirmationGuid(Guid.NewGuid());
                    }


                    // track user ip address
                    UserLocation userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address());
                    userLocation.SiteGuid = siteSettings.SiteGuid;
                    userLocation.Hostname = Page.Request.UserHostName;
                    userLocation.Save();

                    if (
                        (siteSettings.UseSecureRegistration)
                        && (newUser.RegisterConfirmGuid != Guid.Empty)
                        )
                    {
                        Notification.SendRegistrationConfirmationLink(
                            SiteUtils.GetSmtpSettings(),
                            ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                            siteSettings.DefaultEmailFromAddress,
                            siteSettings.DefaultFromEmailAlias,
                            newUser.Email,
                            siteSettings.SiteName,
                            WebUtils.GetSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
                            newUser.RegisterConfirmGuid.ToString());

                        lblError.Text = Resource.LoginUnconfirmedEmailMessage;
                        log.Info("Automatically created User " + newUser.Name + " on login from open id. Tried to login but email address is not confirmed.");

                        return;
                    }

                    if (siteSettings.UseEmailForLogin)
                    {
                        FormsAuthentication.SetAuthCookie(
                            newUser.Email, true);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(
                            newUser.LoginName, true);
                    }

                    if (WebConfigSettings.UseFolderBasedMultiTenants)
                    {
                        string cookieName = "siteguid" + siteSettings.SiteGuid;
                        CookieHelper.SetCookie(cookieName, newUser.UserGuid.ToString(), true);
                    }

                    newUser.UpdateLastLoginTime();

                    string redirectUrl = GetRedirectPath();
                    CookieHelper.ExpireCookie(returnUrlCookieName);
                    WebUtils.SetupRedirect(this, redirectUrl);
                    return;

                }


            }
            else
            {
                // user not found
                // required fields not available from open id
                // redirect to register page?
                // Or show message with Link to
                // register page
                string registerLinkHref = siteRoot
                    + "/Secure/RegisterWithOpenID.aspx";

                litNotRegisteredYetMessage.Text
                    = string.Format(
                    Resource.OpenIDMustRegisterBeforeLoginMesage,
                    registerLinkHref);

            }

        }

        private void HookupSignInEventHandlers()
        {
            // this is a hook so that custom code can be fired when pages are created
            // implement a PageCreatedEventHandlerPovider and put a config file for it in
            // /Setup/ProviderConfig/pagecreatedeventhandlers
            try
            {
                foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
                {
                    this.UserSignIn += handler.UserSignInEventHandler;
                }
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }

        }

        public event UserSignInEventHandler UserSignIn;

        protected void OnUserSignIn(UserSignInEventArgs e)
        {
            if (UserSignIn != null)
            {
                UserSignIn(this, e);
            }
        }

        private bool IsValidForUserCreation(OpenIdEventArgs e, ClaimsResponse claim)
        {
            bool result = true;

            if (claim == null) { return false; }

            if (String.IsNullOrEmpty(claim.Email)) { return false; }
            if (String.IsNullOrEmpty(claim.FullName)) { return false; }

            if (!Email.IsValidEmailAddressSyntax(claim.Email)) { return false; }

            mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            if (profileConfig.HasRequiredCustomProperties()) { result = false; }

            return result;

        }

        void OpenIdLogin1_Canceled(object sender, OpenIdEventArgs e)
        {
            lblLoginCanceled.Visible = true;
        }

        void OpenIdLogin1_Failed(object sender, OpenIdEventArgs e)
        {
            lblLoginFailed.Visible = true;
            if (e.Response.Exception is System.Net.WebException)
            {
                lblError.Text = Resource.OpenIDServerConnectionErrorMessage;
            }
            else
                log.Error(e.Response.Exception);

        }

       

        private string GetRedirectPath()
        {
            string redirectPath = string.Empty;
            if (CookieHelper.CookieExists(returnUrlCookieName))
            {
                redirectPath = CookieHelper.GetCookieValue(returnUrlCookieName);
            }
            if (String.IsNullOrEmpty(redirectPath) ||
                redirectPath.Contains("AccessDenied") ||
                redirectPath.Contains("Login") ||
                redirectPath.Contains("SignIn") ||
                redirectPath.Contains("ConfirmRegistration.aspx") ||
                redirectPath.Contains("Register")
                )
                return SiteUtils.GetNavigationSiteRoot();

            return redirectPath;
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteRoot = SiteUtils.GetNavigationSiteRoot();
            returnUrlCookieName = "ret" 
                + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

        }

        private void PopulateLabels()
        {
            lblLoginFailed.Text = Resource.OpenIDLoginFailedMessage;
            lblLoginCanceled.Text = Resource.OpenIDLoginCanceledMessage;

            OpenIdLogin1.ButtonText = Resource.OpenIDLoginButton;
            OpenIdLogin1.ButtonToolTip = Resource.OpenIDLoginButton;
            OpenIdLogin1.ExamplePrefix = Resource.OpenIDExamplePrefix;
            OpenIdLogin1.ExampleUrl = Resource.OpenIDExampleUrl;
            //OpenIdLogin1.ButtonFontBold = true;

            lblError.Text = string.Empty;
            
        }

        private void DisplayResults(OpenIdEventArgs e, ClaimsResponse claim)
        {
            litResult.Text = "uri: " + e.ClaimedIdentifier.ToString() + "<br />"
                + "Full Name: " + claim.FullName + "<br />"
                + "Nickname: " + claim.Nickname + "<br />"
                + "Birthdate: " + claim.BirthDate.ToString() + "<br />"
                + "Country: " + claim.Country + "<br />"
                + "Culture: " + claim.Culture.ToString() + "<br />"
                + "Gender: " + claim.Gender + "<br />"
                + "Language: " + claim.Language + "<br />"
                + "MailAddress: " + claim.MailAddress + "<br />"
                + "Postal Code: " + claim.PostalCode + " <br />"
                + "Email: " + claim.Email + "<br />"
                + "TimeZone: " + claim.TimeZone + "<br />";


        }


#endif

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
#if !MONO
            

            OpenIdLogin1.LoggedIn += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_LoggedIn);
            OpenIdLogin1.Failed += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Failed);
            OpenIdLogin1.Canceled += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Canceled);

            string navigationRoot = SiteUtils.GetNavigationSiteRoot();
            OpenIdLogin1.RealmUrl = navigationRoot;
            OpenIdLogin1.ReturnToUrl = navigationRoot + "/Secure/Login.aspx";

            OpenIdLogin1.RequestEmail = DemandLevel.Request;
            OpenIdLogin1.RequestFullName = DemandLevel.Request;
            OpenIdLogin1.RequestTimeZone = DemandLevel.Request;
            OpenIdLogin1.RequestGender = DemandLevel.Request;
            OpenIdLogin1.RequestLanguage = DemandLevel.Request;
            OpenIdLogin1.RequestNickname = DemandLevel.Request;
            OpenIdLogin1.RequestPostalCode = DemandLevel.Request;
            OpenIdLogin1.RequestCountry = DemandLevel.Request;
            OpenIdLogin1.RequestBirthDate = DemandLevel.Request;
            OpenIdLogin1.EnableRequestProfile = true;

            HookupSignInEventHandlers();

#endif
#if MONO
            this.Controls.Clear();
#endif

        }

        

    }
}