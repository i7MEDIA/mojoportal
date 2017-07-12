/// Author:		        
/// Created:            2007-08-23
/// Last Modified:      2009-10-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;
using DotNetOpenAuth.OpenId.Extensions.ProviderAuthenticationPolicy;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web.UI.Pages
{

    public partial class RegisterWithOpenId : NonCmsBasePage
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(RegisterWithOpenId));

        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private Collection<mojoProfilePropertyDefinition> 
            requiredProfileProperties = new Collection<mojoProfilePropertyDefinition>();

        private string openidCookieName;
        private string openIdEmailCookieName;
        private string openIdFullNameCookieName;

        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO: get this working in Mono, maby just need to compile
            // sources for needed libs on Mono

            if (
                (!WebConfigSettings.EnableOpenIdAuthentication)
                || (!siteSettings.AllowOpenIdAuth)
                || (!siteSettings.AllowNewRegistration)
                )
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            // overriding base page default here
            this.EnableViewState = true;

            LoadSettings();
            PopulateLabels();

            if (Request.IsAuthenticated)
            {
                pnlRegisterWrapper.Visible = false;
                pnlAuthenticated.Visible = true;
                return;
            }

            PopulateRequiredProfileControls();

        }


        /// <summary>
        /// This event fires upon successfull authentication from
        /// open id provider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OpenIdLogin1_LoggedIn(object sender, OpenIdEventArgs e)
        {
            // prevent the base control from 
            // setting forms auth cookie for us
            // we'll login after further verification
            e.Cancel = true;

            Guid userGuid = SiteUser.GetUserGuidFromOpenId(
                siteSettings.SiteId,
                e.ClaimedIdentifier.ToString());

            
            
            

            if (userGuid == Guid.Empty)
            { 
                // this is expected result for new users
                // user not found in db by open id
                DoNewUserLogic(e);
            }
            else
            {
                // user found so just login
                DoExistingUserLogic(userGuid);
            }


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

                }
                else
                {
                    // create user automagically since we have all 
                    // the needed data
                    string loginName
                        = SecurityHelper.RemoveMarkup(e.ClaimedIdentifier.ToString().Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("/", string.Empty));

                    CreateUser(
                        e.ClaimedIdentifier.ToString(),
                        claim.Email,
                        loginName,
                        SecurityHelper.RemoveMarkup(claim.FullName));
                    
                    return;

                }
            }
            else
            {
                // prompt user to enter needed fields

                CookieHelper.SetSecureCookie(openidCookieName, e.ClaimedIdentifier.ToString());
                if (
                    (claim != null)
                    &&(claim.Email != null)
                    && (claim.Email.Length > 3)
                    && (Email.IsValidEmailAddressSyntax(claim.Email))
                    )
                {
                    CookieHelper.SetSecureCookie(openIdEmailCookieName, claim.Email);
                    divEmailInput.Visible = false;
                    divEmailDisplay.Visible = true;
                    litEmail.Text = claim.Email;
                }
                else
                {
                    divEmailInput.Visible = true;
                    divEmailDisplay.Visible = false;
                }

                if (
                    (claim != null)
                    && (claim.FullName != null)
                    && (claim.FullName.Length > 0)
                    )
                {
                    CookieHelper.SetSecureCookie(openIdFullNameCookieName, SecurityHelper.RemoveMarkup(claim.FullName));
                }
                
                pnlNeededProfileProperties.Visible = true;
                pnlOpenID.Visible = false;
                if (e.ClaimedIdentifier != null)
                {
                    litOpenIDURI.Text = e.ClaimedIdentifier.ToString();
                }
                //PopulateRequiredProfileControls();
                //DisplayResults(e);
                litInfoNeededMessage.Text = Resource.OpenIDAdditionalInfoNeededMessage;
                

            }


        }

        

        void btnCreateUser_Click(object sender, EventArgs e)
        {
            Page.Validate("profile");
            if (Page.IsValid)
            {
                //PopulateRequiredProfileControls();
                string openID = CookieHelper.GetSecureCookieValue(openidCookieName);
                string email = txtEmail.Text;

                if (
                    (CookieHelper.CookieExists(openIdEmailCookieName))
                    &&(email.Length == 0)
                    )
                {
                    email = CookieHelper.GetSecureCookieValue(openIdEmailCookieName);
                }

                if (openID.Length == 0) return;

                string loginName
                    = openID.Replace("http://", string.Empty).Replace("https://", string.Empty).Replace("/", string.Empty);

                string name = loginName;
                if (CookieHelper.CookieExists(openIdFullNameCookieName))
                {
                    name = CookieHelper.GetSecureCookieValue(openIdFullNameCookieName);
                }

                if (SiteUser.EmailExistsInDB(siteSettings.SiteId, email))
                {
                    lblError.Text = Resource.RegisterDuplicateEmailMessage;
                }
                else
                {
                    CreateUser(openID, email, SecurityHelper.RemoveMarkup(loginName), SecurityHelper.RemoveMarkup(name));
                }

            }
        }

        private void CreateUser(
            string openId,
            string email,
            string loginName,
            string name)
        {
            SiteUser newUser = new SiteUser(siteSettings);
            newUser.Email = email;

            if (loginName.Length > 50) loginName = loginName.Substring(0, 50);

            int i = 1;
            while (SiteUser.LoginExistsInDB(
                siteSettings.SiteId, loginName))
            {
                loginName += i.ToString();
                if (loginName.Length > 50) loginName = loginName.Remove(40, 1);
                i++;
                
            }
            if ((name == null) || (name.Length == 0)) name = loginName;
            newUser.LoginName = loginName;
            newUser.Name = name;
            //newUser.Password = SiteUser.CreateRandomPassword(7);
            mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
            newUser.Password = mojoMembership.EncodePassword(siteSettings, newUser, SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars));
            newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
            newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
            newUser.OpenIdUri = openId;
            newUser.Save();
            if (siteSettings.UseSecureRegistration)
            {
                newUser.SetRegistrationConfirmationGuid(Guid.NewGuid());
            }

            mojoProfileConfiguration profileConfig
                = mojoProfileConfiguration.GetConfig();

            // set default values first
            foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                mojoProfilePropertyDefinition.SavePropertyDefault(
                    newUser, propertyDefinition);
            }

            foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                if ((propertyDefinition.RequiredForRegistration)||(propertyDefinition.ShowOnRegistration))
                {
                    mojoProfilePropertyDefinition.SaveProperty(
                        newUser,
                        pnlRequiredProfileProperties,
                        propertyDefinition,
                        timeOffset,
                        timeZone);
                }
            }

            // track user ip address
            UserLocation userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address());
            userLocation.SiteGuid = siteSettings.SiteGuid;
            userLocation.Hostname = Page.Request.UserHostName;
            userLocation.Save();

            UserRegisteredEventArgs u = new UserRegisteredEventArgs(newUser);
            OnUserRegistered(u);

            CacheHelper.ClearMembershipStatisticsCache();

            NewsletterHelper.ClaimExistingSubscriptions(newUser);

            DoUserLogin(newUser);

        }

        #region Events

        private void HookupRegistrationEventHandlers()
        {
            // this is a hook so that custom code can be fired when pages are created
            // implement a PageCreatedEventHandlerPovider and put a config file for it in
            // /Setup/ProviderConfig/pagecreatedeventhandlers
            try
            {
                foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
                {
                    this.UserRegistered += handler.UserRegisteredHandler;
                }
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }

        }

        public event UserRegistreredEventHandler UserRegistered;

        protected void OnUserRegistered(UserRegisteredEventArgs e)
        {
            if (UserRegistered != null)
            {
                UserRegistered(this, e);
            }
        }

        #endregion

        private void DoUserLogin(SiteUser siteUser)
        {
            if (
                (siteSettings.UseSecureRegistration)
                && (siteUser.RegisterConfirmGuid != Guid.Empty)
                )
            {
                Notification.SendRegistrationConfirmationLink(
                    SiteUtils.GetSmtpSettings(),
                    ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                    siteSettings.DefaultEmailFromAddress,
                    siteSettings.DefaultFromEmailAlias,
                    siteUser.Email,
                    siteSettings.SiteName,
                    SiteRoot + "/ConfirmRegistration.aspx?ticket=" +
                    siteUser.RegisterConfirmGuid.ToString());

                lblError.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
                pnlNeededProfileProperties.Visible = false;
                pnlOpenID.Visible = false;
            }
            else
            {
                if (siteUser.IsLockedOut)
                {
                    lblError.Text = Resource.LoginAccountLockedMessage;
                }
                else
                {
                    if (siteSettings.UseEmailForLogin)
                    {
                        FormsAuthentication.SetAuthCookie(
                            siteUser.Email, true);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(
                            siteUser.LoginName, true);
                    }

                    WebUtils.SetupRedirect(this, SiteRoot + "/Secure/UserProfile.aspx");
                }
            }
        }

        

        private void DoExistingUserLogic(Guid userGuid)
        { 
            // really shouldn't hit this code unless the user has
            // forgotten that they already registered or is
            // purposefully trying to register again with the same account
            // if allowed just login and send to profile
            SiteUser user = new SiteUser(siteSettings, userGuid);
            DoUserLogin(user);
           
        }

        private bool IsValidForUserCreation(OpenIdEventArgs e, ClaimsResponse claim)
        {
            if (e == null) return false;
            if (claim == null) return false;
            if (e.ClaimedIdentifier == null) { return false; }

            if (String.IsNullOrEmpty(claim.Email)) { return false; }
            if (String.IsNullOrEmpty(claim.FullName)) { return false; }

            if (!Email.IsValidEmailAddressSyntax(claim.Email)) return false;

            // if custom profile fields are required
            // must pass them on to registration page
            mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                if (profileConfig.HasRequiredCustomProperties()) return false;
            }

            return true;

        }


        void OpenIdLogin1_Failed(object sender, OpenIdEventArgs e)
        {
            lblLoginFailed.Visible = true;
        }

       
        protected void OpenIdLogin1_Canceled(object sender, OpenIdEventArgs e)
        {
            lblLoginCanceled.Visible = true;
        }


        private void PopulateRequiredProfileControls()
        {
            foreach (mojoProfilePropertyDefinition propertyDefinition in requiredProfileProperties)
            {
                mojoProfilePropertyDefinition.SetupPropertyControl(
                    this,
                    pnlRequiredProfileProperties,
                    propertyDefinition,
                    timeOffset,
                    timeZone,
                    SiteRoot);
            }
            
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterWithOpenIDLink);

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionOpenIDRegistrationPageFormat, siteSettings.SiteName);

            litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;

            lblError.Text = string.Empty;
            lblLoginFailed.Text = Resource.OpenIDRegistrationFailedMessage;
            lblLoginCanceled.Text = Resource.OpenIDRegistrationCanceledMessage;
            OpenIdLogin1.ButtonText = Resource.OpenIDRegisterButton;
            OpenIdLogin1.ButtonToolTip = Resource.OpenIDRegisterButton;
            OpenIdLogin1.ExamplePrefix = Resource.OpenIDExamplePrefix;
            OpenIdLogin1.ExampleUrl = Resource.OpenIDExampleUrl;
            OpenIdLogin1.ButtonFontBold = true;
            EmailRequired.ErrorMessage = Resource.RegisterEmailRequiredMessage;

            Literal agreement = new Literal();
            agreement.Text = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
            divAgreement.Controls.Add(agreement);
            btnCreateUser.Text = Resource.RegisterButton;

            regexEmail.ErrorMessage = Resource.RegisterEmailRegexMessage;
        }

        /// <summary>
        /// This is only for testing/development to see the response 
        /// from open id provider
        /// </summary>
        /// <param name="e"></param>
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

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            mojoProfileConfiguration profileConfig 
                = mojoProfileConfiguration.GetConfig();

            foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                if ((propertyDefinition.RequiredForRegistration)||(propertyDefinition.ShowOnRegistration))
                {
                    requiredProfileProperties.Add(propertyDefinition);
                }
            }

            openidCookieName = "openid" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
            openIdEmailCookieName = "openidemail" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
            openIdFullNameCookieName = "openidname" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            AddClassToBody("registeropenidpage");


        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            this.AppendQueryStringToAction = false;
            this.btnCreateUser.Click += new EventHandler(btnCreateUser_Click);
            this.OpenIdLogin1.LoggedIn += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_LoggedIn);
  
            OpenIdLogin1.Failed += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Failed);
            this.OpenIdLogin1.Canceled += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Canceled);

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

            //SuppressPageMenu();
            if (WebConfigSettings.HideMenusOnRegisterPage) { SuppressAllMenus(); }
            HookupRegistrationEventHandlers();
        }

        


        
        
    }
}
