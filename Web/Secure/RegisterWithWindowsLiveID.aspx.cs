/// Author:		        
/// Created:            2007-08-24
/// Last Modified:      2010-04-08
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
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web.UI.Pages
{

    public partial class RegisterWithWindowsLiveId : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RegisterWithWindowsLiveId));

        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private Collection<mojoProfilePropertyDefinition>
            requiredProfileProperties = new Collection<mojoProfilePropertyDefinition>();

        private string winliveCookieName;
        private WindowsLiveLogin windowsLive = null;
        private WindowsLiveLogin.User liveUser = null;
        private string protocol = "http://";

        protected string Protocol
        {
            get { return protocol; }
        }
        private string windowsLiveAppId;
        protected string WindowsLiveAppId
        {
            get { return windowsLiveAppId; }
        }

        private bool persistCookie = false;
        private Guid userGuid = Guid.Empty;
        

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.AppendQueryStringToAction = false;
            this.btnCreateUser.Click += new EventHandler(btnCreateUser_Click);
            //SuppressPageMenu();
            if (WebConfigSettings.HideMenusOnRegisterPage) { SuppressAllMenus(); }

            HookupRegistrationEventHandlers();
        }

        

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (
                (!WebConfigSettings.EnableWindowsLiveAuthentication)
                || (!siteSettings.AllowWindowsLiveAuth)
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

            if (windowsLive == null)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            PopulateLabels();

            if (Request.IsAuthenticated)
            {
                pnlRegisterWrapper.Visible = false;
                pnlAuthenticated.Visible = true;
                return;
            }

            PopulateRequiredProfileControls();
            if (liveUser != null)
            {
                pnlWindowsLiveLogin.Visible = false;
                pnlWindowsLiveRegister.Visible = true;
            }
            else
            {
                pnlWindowsLiveLogin.Visible = true;
                pnlWindowsLiveRegister.Visible = false;
            }

        }

        void btnCreateUser_Click(object sender, EventArgs e)
        {
            if (!IsValidForNewUser()) return;
            
            

            if (liveUser == null)
            {
                // TO DO: show error

                return;
            }
            else
            {
                Guid userGuid = SiteUser.GetUserGuidFromWindowsLiveId(
                    siteSettings.SiteId,liveUser.Id);

                persistCookie = liveUser.UsePersistentCookie;

                if (userGuid == Guid.Empty)
                {
                    CreateUser(liveUser.Id);
                }
                else
                {
                    // shouldn't get here but if we do
                    WebUtils.SetupRedirect(this, "Login.aspx");

                }
            }
           
           
        }

        private void CreateUser(string windowsLiveId)
        {
            SiteUser newUser = new SiteUser(siteSettings);
            newUser.WindowsLiveId = windowsLiveId;
            newUser.Name = SecurityHelper.RemoveMarkup(txtUserName.Text);
            newUser.LoginName = newUser.Name;
            newUser.Email = txtEmail.Text;
            mojoMembershipProvider mojoMembership = (mojoMembershipProvider)Membership.Provider;
            newUser.Password = mojoMembership.EncodePassword(siteSettings, newUser, SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars));
            //newUser.Password = SiteUser.CreateRandomPassword(7);
            newUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
            newUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
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
#if!MONO
                // we are using the new TimeZoneInfo list but it doesn't work under Mono
                // this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
                if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
#endif
                mojoProfilePropertyDefinition.SavePropertyDefault(
                    newUser, propertyDefinition);
            }

            foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
#if!MONO
                // we are using the new TimeZoneInfo list but it doesn't work under Mono
                // this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
                if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
#endif
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
                    WebUtils.GetSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
                    siteUser.RegisterConfirmGuid.ToString());

                lblError.Text = Resource.RegistrationRequiresEmailConfirmationMessage;
                pnlWindowsLiveRegister.Visible = false;

                
                
            }
            else
            {
                if (siteUser.IsLockedOut)
                {
                    lblError.Text = Resource.LoginAccountLockedMessage;
                    pnlWindowsLiveRegister.Visible = false;
                }
                else
                {
                    if (siteSettings.UseEmailForLogin)
                    {
                        FormsAuthentication.SetAuthCookie(
                            siteUser.Email, persistCookie);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(
                            siteUser.LoginName, persistCookie);
                    }

                    if (WebConfigSettings.UseFolderBasedMultiTenants)
                    {
                        string cookieName = "siteguid" + siteSettings.SiteGuid;
                        CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), persistCookie);
                    }

                    if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
                    {
                        SiteUtils.SetSkinCookie(siteUser);
                    }

                    siteUser.UpdateLastLoginTime();


                    WebUtils.SetupRedirect(this, SiteRoot + "/Secure/UserProfile.aspx");
                }
            }
        }

        private bool IsValidForNewUser()
        {
            bool result = true;
            Page.Validate("profile");
            if (!Page.IsValid) return false;

            if (SiteUser.LoginExistsInDB(siteSettings.SiteId, txtUserName.Text))
            {
                result = false;
                lblError.Text = Resource.RegisterDuplicateUserNameMessage;
            }

            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
            {
                result = false;
                lblError.Text += " " + Resource.RegisterDuplicateEmailMessage;
            }

            return result;
        }

        private void PopulateRequiredProfileControls()
        {
            foreach (mojoProfilePropertyDefinition propertyDefinition in requiredProfileProperties)
            {
#if!MONO
                // we are using the new TimeZoneInfo list but it doesn't work under Mono
                // this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
                if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
#endif
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
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RegisterWithWindowsLiveLink);

            litAlreadyAuthenticated.Text = Resource.AlreadyRegisteredMessage;

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionWidnowsLiveRegistrationPageFormat, siteSettings.SiteName);

            litInstructions.Text = Resource.WindowsLiveSignInBeforeRegisterMessage;
            btnCreateUser.Text = Resource.RegisterButton;
            litInfoNeededMessage.Text = Resource.WindowsLiveRegistrationRequiresAdditionalInfo;

            string termsOfUse = siteSettings.RegistrationAgreement;
            if (termsOfUse.Length == 0)
            {
                termsOfUse = ResourceHelper.GetMessageTemplate("RegisterLicense.config");
            }

            if (termsOfUse.Length > 0)
            {
                Literal agreement = new Literal();
                agreement.Text = termsOfUse;
                divAgreement.Controls.Add(agreement);
            }

        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            if (SiteUtils.SslIsAvailable()) protocol = "https://";

            mojoProfileConfiguration profileConfig
                = mojoProfileConfiguration.GetConfig();

            foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
            {
                if ((propertyDefinition.RequiredForRegistration)||(propertyDefinition.ShowOnRegistration))
                {
                    requiredProfileProperties.Add(propertyDefinition);
                }
            }

            winliveCookieName = "winliveid" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            windowsLive = WindowsLiveHelper.GetWindowsLiveLogin();
            if (windowsLive == null) { return; }

            windowsLiveAppId = windowsLive.AppId;

            string winLiveToken = CookieHelper.GetCookieValue(winliveCookieName);
            if (winLiveToken.Length > 0)
            {
                liveUser = windowsLive.ProcessToken(winLiveToken);
            }


            AddClassToBody("registerwinlivepage");

            

        }

    }
}
