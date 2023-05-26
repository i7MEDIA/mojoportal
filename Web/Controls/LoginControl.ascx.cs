using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
	[Themeable(true)]
    public partial class LoginControl : UserControl
    {
        //Constituent controls inside LoginControl
        private SiteLabel lblUserID;
        private SiteLabel lblEmail;
        private SiteLabel lblRegisterPrompt;
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
		public bool SetFocus { get; set; } = false;

		public bool SetRedirectUrl { get; set; } = true;

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

            LoginCtrl.SetRedirectUrl = SetRedirectUrl;

            lblUserID = (SiteLabel)this.LoginCtrl.FindControl("lblUserID");
            lblEmail = (SiteLabel)this.LoginCtrl.FindControl("lblEmail");
            txtUserName = (TextBox)this.LoginCtrl.FindControl("UserName");
            txtPassword = (TextBox)this.LoginCtrl.FindControl("Password");
            chkRememberMe = (CheckBox)this.LoginCtrl.FindControl("RememberMe");
            btnLogin = (mojoButton)this.LoginCtrl.FindControl("Login");
            lnkRecovery = (HyperLink)this.LoginCtrl.FindControl("lnkPasswordRecovery");
            lnkExtraLink = (HyperLink)this.LoginCtrl.FindControl("lnkRegisterExtraLink");
			lblRegisterPrompt = (SiteLabel)this.LoginCtrl.FindControl("lblRegisterPrompt"); 

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

            if (siteSettings.UseEmailForLogin && !siteSettings.UseLdapAuth)
            {
                if (!WebConfigSettings.AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin)
                {
					EmailValidator regexEmail = new()
					{
						ControlToValidate = txtUserName.ID,
						ErrorMessage = Resource.LoginFailedInvalidEmailFormatMessage
					};
					this.LoginCtrl.Controls.Add(regexEmail);
                }
				
                this.lblUserID.Visible = false;
                txtUserName.Attributes.Add("placeholder", Resource.SignInEmailLabel);
			}
            else
            {
                this.lblEmail.Visible = false;
				txtUserName.Attributes.Add("placeholder", Resource.ManageUsersLoginNameLabel);
			}

            txtPassword.Attributes.Add("placeholder", Resource.SignInPasswordLabel);

			if (SetFocus) { txtUserName.Focus(); }

			lnkRecovery.Visible = (siteSettings.AllowPasswordRetrieval || siteSettings.AllowPasswordReset) && (!siteSettings.UseLdapAuth ||
                                                                           (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap));

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

            lblRegisterPrompt.ConfigKey = "SignInRegisterPrompt";

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