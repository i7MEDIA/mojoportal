// Author:					
// Created:					2010-12-11
// Last Modified:			2014-08-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class LoginControl : UserControl
    {

        //Constituent controls inside LoginControl
        private SiteLabel lblUserID;
        private SiteLabel lblEmail;
        private TextBox txtUserName;
        private CheckBox chkRememberMe;
        private mojoButton btnLogin;
        private HyperLink lnkRecovery;
        private HyperLink lnkExtraLink;
        private TextBox txtPassword;
        private Panel divCaptcha = null;
        private CaptchaControl captcha = null;

        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;

        private bool setFocus = false;
        public bool SetFocus
        {
            get { return setFocus; }
            set { setFocus = value; }
        }

        private bool setRedirectUrl = true;

        public bool SetRedirectUrl
        {
            get { return setRedirectUrl; }
            set { setRedirectUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.IsAuthenticated)
            {
                // user is logged in
                this.Visible = false;
                return;
            }

            LoadSettings();
            PopulateLabels();

            PopulateControls();

        }

        private void PopulateControls()
        {
            if (siteSettings == null) { return; }
            if (siteSettings.DisableDbAuth) { this.Visible = false; return; }

            LoginCtrl.SetRedirectUrl = setRedirectUrl;

            lblUserID = (SiteLabel)this.LoginCtrl.FindControl("lblUserID");
            lblEmail = (SiteLabel)this.LoginCtrl.FindControl("lblEmail");
            txtUserName = (TextBox)this.LoginCtrl.FindControl("UserName");
            txtPassword = (TextBox)this.LoginCtrl.FindControl("Password");
            chkRememberMe = (CheckBox)this.LoginCtrl.FindControl("RememberMe");
            btnLogin = (mojoButton)this.LoginCtrl.FindControl("Login");
            lnkRecovery = (HyperLink)this.LoginCtrl.FindControl("lnkPasswordRecovery");
            lnkExtraLink = (HyperLink)this.LoginCtrl.FindControl("lnkRegisterExtraLink");
            
            if (WebConfigSettings.DisableAutoCompleteOnLogin)
            {
                txtUserName.AutoCompleteType = AutoCompleteType.Disabled;
                txtPassword.AutoCompleteType = AutoCompleteType.Disabled;
            }

            divCaptcha = (Panel)LoginCtrl.FindControl("divCaptcha");
            captcha = (CaptchaControl)LoginCtrl.FindControl("captcha");
            if (!siteSettings.RequireCaptchaOnLogin)
            {
                if (divCaptcha != null) { divCaptcha.Visible = false; }
                if (captcha != null) { captcha.Captcha.Enabled = false; }
            }
            else
            {
                captcha.ProviderName = siteSettings.CaptchaProvider;
                captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
                captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;

            }

            if ((siteSettings.UseEmailForLogin) && (!siteSettings.UseLdapAuth))
            {
                if (!WebConfigSettings.AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin)
                {
                    EmailValidator regexEmail = new EmailValidator();
                    regexEmail.ControlToValidate = txtUserName.ID;
                    regexEmail.ErrorMessage = Resource.LoginFailedInvalidEmailFormatMessage;
                    this.LoginCtrl.Controls.Add(regexEmail);
                }

            }

            if (siteSettings.UseEmailForLogin && !siteSettings.UseLdapAuth)
            {
                this.lblUserID.Visible = false;
            }
            else
            {
                this.lblEmail.Visible = false;
            }

            if (setFocus) { txtUserName.Focus(); }

            lnkRecovery.Visible = ((siteSettings.AllowPasswordRetrieval ||siteSettings.AllowPasswordReset) && (!siteSettings.UseLdapAuth ||
                                                                           (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap)));

            lnkRecovery.NavigateUrl = this.LoginCtrl.PasswordRecoveryUrl;
            lnkRecovery.Text = this.LoginCtrl.PasswordRecoveryText;

            lnkExtraLink.NavigateUrl = siteRoot + "/Secure/Register.aspx";
            lnkExtraLink.Text = Resource.RegisterLink;
            lnkExtraLink.Visible = siteSettings.AllowNewRegistration;

            string returnUrlParam = Page.Request.Params.Get("returnurl");
            if (!String.IsNullOrEmpty(returnUrlParam))
            {
                //string redirectUrl = returnUrlParam;
                lnkExtraLink.NavigateUrl += "?returnurl=" + SecurityHelper.RemoveMarkup(returnUrlParam);
            }

            chkRememberMe.Visible = siteSettings.AllowPersistentLogin;
            chkRememberMe.Text = this.LoginCtrl.RememberMeText;

            if (WebConfigSettings.ForcePersistentAuthCheckboxChecked)
            {
                chkRememberMe.Checked = true;
                chkRememberMe.Visible = false;
            }

            btnLogin.Text = this.LoginCtrl.LoginButtonText;
            //SiteUtils.SetButtonAccessKey(btnLogin, AccessKeys.LoginAccessKey);

            

        }

        private void PopulateLabels()
        {


        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteRoot = SiteUtils.GetNavigationSiteRoot();

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);

            //if (Request.IsAuthenticated)
            //{
            //    // user is logged in
            //    this.Visible = false;
            //    return;
            //}

            //LoadSettings();
            //PopulateLabels();

            //PopulateControls();

            
        }
    }
}