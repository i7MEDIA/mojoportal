// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Created:			        2007-06-06
//	Last Modified:              2010-10-24
// 
// 2007/06/06  Alexander Yushchenko: created this control from part of /Secure/Login.aspx.cs refactoring
// 2007-08-24   fixed bug where message was not displayed if
// email confirmation needed or account locked. Also added logic to
// send another confirmation email if user tries to login and
// confirmation is needed.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserSignInHandlers;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using log4net;
using Resources;
using mojoPortal.Core.Helpers;

namespace mojoPortal.Web.UI
{
   
    public class SiteLogin : Login
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiteLogin));
        private readonly SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        private readonly string siteRoot = SiteUtils.GetNavigationSiteRoot();
        //private HiddenField hdnReturnUrl = null;

        private bool setRedirectUrl = true;

        public bool SetRedirectUrl
        {
            get { return setRedirectUrl; }
            set { setRedirectUrl = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(SiteLogin_Load);
            this.LoginError += new EventHandler(SiteLogin_LoginError);
            this.LoggingIn += new LoginCancelEventHandler(SiteLogin_LoggingIn);
            this.LoggedIn += new EventHandler(SiteLogin_LoggedIn);

            this.CreateUserText = Resource.SignInRegisterLinkText;
            this.CreateUserUrl = siteRoot + "/Secure/Register.aspx";
            this.FailureText = ResourceHelper.GetMessageTemplate("LoginFailedMessage.config");
            this.LoginButtonText = Resource.SignInLinkText;
            this.PasswordRecoveryText = Resource.SignInSendPasswordButton;
            this.PasswordRecoveryUrl = siteRoot + "/Secure/RecoverPassword.aspx";
            this.RememberMeText = Resource.SignInSendRememberMeLabel;
            this.RememberMeSet = WebConfigSettings.ForcePersistentAuthCheckboxChecked;

#if !NET35
            this.RenderOuterTable = false;
            this.CssClass = string.Empty;
#endif

            //HookupSignInEventHandlers();

            //hdnReturnUrl = new HiddenField();
            //hdnReturnUrl.ID = "hdnReturnUrl";
            //this.Controls.Add(hdnReturnUrl);

            //CaptchaControl captcha = (CaptchaControl)this.FindControl("captcha");
            //if (captcha != null)
            //{
            //    if(siteSettings.RequireCaptchaOnLogin)
            //    {
            //        captcha.ProviderName = siteSettings.CaptchaProvider;
            //        captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
            //        captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
            //    }
            //}

            
        }


        void SiteLogin_Load(object sender, EventArgs e)
        {
            

            if (!Page.IsPostBack)
            {
                ViewState["LoginErrorCount"] = 0;

                 if (Page.Request.UrlReferrer != null)
                {
                    string urlReferrer = Page.Request.UrlReferrer.ToString();
                    if ((urlReferrer.StartsWith(siteRoot)) || (urlReferrer.StartsWith(siteRoot.Replace("https://", "http://"))))
                    {
                        ViewState["ReturnUrl"] = urlReferrer;
                        //log.Info(hdnReturnUrl.Value);
                    }
                }

                string returnUrlParam = Page.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
                    string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
                    if (
                        ((redirectUrl.StartsWith("/")) && (!(redirectUrl.StartsWith("//")))) 
                        || (redirectUrl.StartsWith(siteRoot)) 
                        || (redirectUrl.StartsWith(siteRoot.Replace("https://", "http://"))))
                    {
                        ViewState["ReturnUrl"] = redirectUrl;
                    }
                }
               
            }

            if (setRedirectUrl) { this.DestinationPageUrl = GetRedirectPath(); }

            if (WebConfigSettings.DebugLoginRedirect)
            {
                log.Info("Login redirect url was " + this.DestinationPageUrl + " for Site Root " + siteRoot);
            }

        }


        protected void SiteLogin_LoginError(object sender, EventArgs e)
        {
            int errorCount = (int)ViewState["LoginErrorCount"] + 1;
            ViewState["LoginErrorCount"] = errorCount;

            if ((siteSettings != null)
                && (!siteSettings.UseLdapAuth)
                && (siteSettings.PasswordFormat != 1)
                && (siteSettings.AllowPasswordRetrieval)
                && (errorCount >= siteSettings.MaxInvalidPasswordAttempts)
                && (this.PasswordRecoveryUrl != String.Empty)
                )
            {
                WebUtils.SetupRedirect(this, this.PasswordRecoveryUrl);
            }
        }


        void SiteLogin_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            if (siteSettings.RequireCaptchaOnLogin)
            {
                CaptchaControl captcha = (CaptchaControl)this.FindControl("captcha");
                if (captcha != null)
                {
                   // if (!captcha.Captcha.IsValid)
                    if (!captcha.IsValid)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

            }

            SiteUser siteUser = new SiteUser(siteSettings, this.UserName);
            if (siteUser.UserId > -1)
            {
                if (siteSettings.UseSecureRegistration && siteUser.RegisterConfirmGuid != Guid.Empty)
                {
                    //this.FailureText = Resource.LoginUnconfirmedEmailMessage;
                    Label lblFailure = (Label)this.FindControl("FailureText");
                    if (lblFailure != null)
                    {
                        lblFailure.Visible = true;
                        lblFailure.Text = Resource.LoginUnconfirmedEmailMessage;

                    }
                    // send email with confirmation link that will approve profile
                    Notification.SendRegistrationConfirmationLink(
                        SiteUtils.GetSmtpSettings(),
                        ResourceHelper.GetMessageTemplate("RegisterConfirmEmailMessage.config"),
                        siteSettings.DefaultEmailFromAddress,
                        siteSettings.DefaultFromEmailAlias,
                        siteUser.Email,
                        siteSettings.SiteName,
                        WebUtils.GetSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
                        siteUser.RegisterConfirmGuid.ToString());

                    // user has not confirmed
                    e.Cancel = true;
                    return;
                }

                if (siteUser.IsDeleted)
                {
                    //this.FailureText = Resource.LoginAccountLockedMessage;
                    Label lblFailure = (Label)this.FindControl("FailureText");
                    if (lblFailure != null)
                    {
                        lblFailure.Visible = true;
                        lblFailure.Text = ResourceHelper.GetMessageTemplate("LoginFailedMessage.config");
                    }

                    e.Cancel = true;
                    return;
                }

                if (siteUser.IsLockedOut)
                {
                    //this.FailureText = Resource.LoginAccountLockedMessage;
                    Label lblFailure = (Label)this.FindControl("FailureText");
                    if (lblFailure != null)
                    {
                        lblFailure.Visible = true;
                        lblFailure.Text = Resource.LoginAccountLockedMessage;
                    }

                    e.Cancel = true;
                    return;
                }

                if ((siteSettings.RequireApprovalBeforeLogin)&&(!siteUser.ApprovedForLogin))
                {
                    //this.FailureText = Resource.LoginAccountLockedMessage;
                    Label lblFailure = (Label)this.FindControl("FailureText");
                    if (lblFailure != null)
                    {
                        lblFailure.Visible = true;
                        lblFailure.Text = Resource.LoginNotApprovedMessage;
                    }

                    e.Cancel = true;
                    return;
                }

                if (siteSettings.MaxInvalidPasswordAttempts > 0)
                {
                    if (siteUser.FailedPasswordAttemptCount >= siteSettings.MaxInvalidPasswordAttempts)
                    {
                        if (siteUser.FailedPasswordAttemptWindowStart.AddMinutes(siteSettings.PasswordAttemptWindowMinutes) > DateTime.UtcNow)
                        {
                            
                            //this.FailureText = Resource.LoginAccountLockedMessage;
                            Label lblFailure = (Label)this.FindControl("FailureText");
                            if (lblFailure != null)
                            {
                                lblFailure.Visible = true;
                                lblFailure.Text = Resource.AccountLockedTemporarilyDueToPasswordFailures;
                            }
                            e.Cancel = true;
                            return;
                        }
                    }

                }

            }
        }

        


        protected void SiteLogin_LoggedIn(object sender, EventArgs e)
        {
            if (siteSettings == null) return;

            SiteUser siteUser = new SiteUser(siteSettings, this.UserName);

            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                string cookieName = "siteguid" + siteSettings.SiteGuid;
                CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), this.RememberMeSet);
            }

            if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
            {
                SiteUtils.SetSkinCookie(siteUser);
            }

            if (siteUser.UserGuid == Guid.Empty) return;

            // track user ip address
            try
            {
                UserLocation userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address());
                userLocation.SiteGuid = siteSettings.SiteGuid;
                userLocation.Hostname = Page.Request.UserHostName;
                userLocation.Save();
            }
            catch (Exception ex)
            {
                log.Error(SiteUtils.GetIP4Address(), ex);
            }


            UserSignInEventArgs u = new UserSignInEventArgs(siteUser);
            OnUserSignIn(u);
            
        }

        #region Events

        //private void HookupSignInEventHandlers()
        //{
        //    // this is a hook so that custom code can be fired when pages are created
        //    // implement a PageCreatedEventHandlerPovider and put a config file for it in
        //    // /Setup/ProviderConfig/pagecreatedeventhandlers
        //    try
        //    {
        //        foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
        //        {
        //            this.UserSignIn += handler.UserSignInEventHandler;
        //        }
        //    }
        //    catch (TypeInitializationException ex)
        //    {
        //        log.Error(ex);
        //    }

        //}

        //public event UserSignInEventHandler UserSignIn;

        protected void OnUserSignIn(UserSignInEventArgs e)
        {
            foreach (UserSignInHandlerProvider handler in UserSignInHandlerProviderManager.Providers)
            {
                handler.UserSignInEventHandler(null, e);
            }
            
            //if (UserSignIn != null)
            //{
            //    UserSignIn(this, e);
            //}
        }

        #endregion


        private string GetRedirectPath()
        {
            string redirectPath = WebConfigSettings.PageToRedirectToAfterSignIn;

            if (redirectPath.Length > 0) { return redirectPath; }

            string defaultRedirect = siteRoot;
            if (
                (!siteSettings.IsServerAdminSite)
                && (WebConfigSettings.UseFolderBasedMultiTenants)
                && (WebConfigSettings.AppendDefaultPageToFolderRootUrl)
                )
            {
                defaultRedirect += "/Default.aspx";
            }

            //if (redirectPath.EndsWith(".aspx")) { return redirectPath; }

            if (ViewState["ReturnUrl"] != null)
            {
                redirectPath = ViewState["ReturnUrl"].ToString();
            }

            if (String.IsNullOrEmpty(redirectPath) ||
                redirectPath.Contains("AccessDenied") ||
                redirectPath.Contains("Login") ||
                redirectPath.Contains("SignIn") ||
                redirectPath.Contains("ConfirmRegistration.aspx") ||
                redirectPath.Contains("OpenIdRpxHandler.aspx") ||
                redirectPath.Contains("RecoverPassword.aspx") ||
                redirectPath.Contains("Register")
                )
            {
                redirectPath = defaultRedirect;
            }

            if (Page.Request.Params["r"] == "h") { redirectPath = defaultRedirect; }

            if (WebHelper.IsSecureRequest())
            {
                if (redirectPath.StartsWith("http:"))
                {
                    redirectPath = redirectPath.Replace("http:", "https:");
                }
            }

            return redirectPath;
        }

    }
}
