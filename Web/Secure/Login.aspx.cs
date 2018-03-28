// Author:				        
// Created:			            2004-10-22
// Last Modified:               2018-03-28
// 
// 2007/06/06  Alexander Yushchenko: moved login logic into new SiteLogin control, refactoring
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
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.Controls;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{
    public partial class LoginPage : NonCmsBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginPage));

        //Constituent controls inside LoginControl
        //private SiteLabel lblUserID;
        //private SiteLabel lblEmail;
      
        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;
        private string returnUrlCookieName = string.Empty;
		

        //private TextBox txtUserName;
        //private CheckBox chkRememberMe;
        //private mojoButton btnLogin;
        //private HyperLink lnkRecovery;
        //private HyperLink lnkExtraLink;
        //private TextBox txtPassword;
        //private Panel divCaptcha = null;
        //private CaptchaControl captcha = null;
		



        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.AppendQueryStringToAction = false;

            rpxApiKey = siteSettings.RpxNowApiKey;
            rpxApplicationName = siteSettings.RpxNowApplicationName;
            //pnlOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
                }

            }

            returnUrlCookieName = "returnurl" + siteSettings.SiteId.ToInvariantString();


#if !MONO

            if ((WebConfigSettings.EnableOpenIdAuthentication) && (siteSettings.AllowOpenIdAuth))
            {
                pnlOpenID.Visible = true;
                OpenIdLoginControl oidLogin = (OpenIdLoginControl)Page.LoadControl("~/Controls/OpenIDLoginControl.ascx");
                oidLogin.ID = "oidLogin";
                pnlOpenID.Controls.Add(oidLogin);

                AddClassToBody("openid");
            }

            if (rpxApiKey.Length > 0)
            {
                if ((!WebConfigSettings.DisableRpxAuthentication))
                {
                    pnlOpenID.Visible = true;
                    OpenIdRpxNowLink rpxNowLink = new OpenIdRpxNowLink();
                    pnlOpenID.Controls.Add(rpxNowLink);

                    AddClassToBody("janrain");
                    
                }
            }

            //SiteLogin LoginCtrl = (SiteLogin)login1.FindControl("LoginCtrl");
            //Panel divCaptcha = (Panel)LoginCtrl.FindControl("divCaptcha");
            //CaptchaControl captcha = (CaptchaControl)LoginCtrl.FindControl("captcha");
            //if (!siteSettings.RequireCaptchaOnLogin)
            //{
            //    if (divCaptcha != null) { divCaptcha.Visible = false; }
            //    if (captcha != null) { captcha.Captcha.Enabled = false; }
            //}
            //else
            //{
            //    captcha.ProviderName = siteSettings.CaptchaProvider;
            //    captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
            //    captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;

            //}

            
#endif

            //DotNetOpenAuth does not work on mono as of 2009-05-15 so only option is rpxnow.com
#if MONO
            if (rpxApiKey.Length > 0)
            {
                OpenIdRpxNowLink rpxNowLink = new OpenIdRpxNowLink();
                pnlOpenID.Controls.Add(rpxNowLink);
            }

#endif

            if (WebConfigSettings.HideMenusOnLoginPage) { SuppressAllMenus(); }
            
        }


        private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (Request.IsAuthenticated)
            {
                string returnUrlParam = Page.Request.Params.Get("returnurl");

                if (!String.IsNullOrEmpty(returnUrlParam) && !returnUrlParam.ToLower().Contains("/accessdenied.aspx"))
                {
                    returnUrlParam = SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam));


                    string redirectUrl = Page.ResolveUrl(returnUrlParam);
                    if (
                        ((redirectUrl.StartsWith("/")) && (!(redirectUrl.StartsWith("//")))) 
                        || (redirectUrl.StartsWith(SiteRoot)) 
                        || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
                    {
                        WebUtils.SetupRedirect(this, returnUrlParam);
                        return;
                    }
                }

                // user is logged in
                WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
                return;
            }

            //lblUserID = (SiteLabel)this.LoginCtrl.FindControl("lblUserID");
            //lblEmail = (SiteLabel)this.LoginCtrl.FindControl("lblEmail");
            //txtUserName = (TextBox)this.LoginCtrl.FindControl("UserName");
            //txtPassword = (TextBox)this.LoginCtrl.FindControl("Password");
            //chkRememberMe = (CheckBox)this.LoginCtrl.FindControl("RememberMe");
            //btnLogin = (mojoButton)this.LoginCtrl.FindControl("Login");
            //lnkRecovery = (HyperLink)this.LoginCtrl.FindControl("lnkPasswordRecovery");
            //lnkExtraLink = (HyperLink)this.LoginCtrl.FindControl("lnkRegisterExtraLink");

            //divCaptcha = (Panel)LoginCtrl.FindControl("divCaptcha");
            //captcha = (CaptchaControl)LoginCtrl.FindControl("captcha");
            //if (!siteSettings.RequireCaptchaOnLogin)
            //{
            //    if (divCaptcha != null) { divCaptcha.Visible = false; }
            //    if (captcha != null) { captcha.Captcha.Enabled = false; }
            //}
            //else
            //{
            //    captcha.ProviderName = siteSettings.CaptchaProvider;
            //    captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
            //    captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;

            //}
            

            PopulateLabels();
            

            string wlAppId = siteSettings.WindowsLiveAppId;
            if (ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"] != null)
            {
                wlAppId = ConfigurationManager.AppSettings["GlobalWindowsLiveAppId"];
                if (wlAppId.Length == 0) { wlAppId = siteSettings.WindowsLiveAppId; }
            }

            pnlWindowsLive.Visible 
                = WebConfigSettings.EnableWindowsLiveAuthentication 
                && siteSettings.AllowWindowsLiveAuth
                && (wlAppId.Length > 0);

            divLiteralOr.Visible = (pnlOpenID.Visible && pnlWindowsLive.Visible);

            login1.SetFocus = true;

            //if ((siteSettings.UseEmailForLogin)&&(!siteSettings.UseLdapAuth))
            //{
            //    if (!WebConfigSettings.AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin)
            //    {
            //        RegularExpressionValidator regexEmail = new RegularExpressionValidator();
            //        regexEmail.ControlToValidate = txtUserName.ID;
            //        //regexEmail.ValidationExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";
            //        regexEmail.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;
            //        regexEmail.ErrorMessage = Resource.LoginFailedInvalidEmailFormatMessage;
            //        this.LoginCtrl.Controls.Add(regexEmail);
            //    }

            //}

            if (siteSettings.LoginInfoTop.Length > 0)
            {
                pnlTopContent.Visible = true;
                litTopContent.Text = siteSettings.LoginInfoTop;
            }

            if (siteSettings.LoginInfoBottom.Length > 0)
            {
                pnlBottomContent.Visible = true;
                litBottomContent.Text = siteSettings.LoginInfoBottom;
            }


            SetupReturnUrlCookie();

            if (siteSettings.DisableDbAuth) { pnlStandardLogin.Visible = false; }
            

        }

        private void SetupReturnUrlCookie()
        {
            if (Page.IsPostBack) { return; }
            
            string returnUrl = string.Empty;

            if (Page.Request.UrlReferrer != null)
            {
                string urlReferrer = Page.Request.UrlReferrer.ToString();
                if ((urlReferrer.StartsWith(SiteRoot)) || (urlReferrer.StartsWith(SiteRoot.Replace("https://", "http://"))))
                {
                    returnUrl = urlReferrer;

                }
            }

            string returnUrlParam = Page.Request.Params.Get("returnurl");
            if (!String.IsNullOrEmpty(returnUrlParam))
            {
                returnUrlParam = SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam));
                string redirectUrl = Page.ResolveUrl(returnUrlParam);
                //string redirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(returnUrlParam));
                if ((redirectUrl.StartsWith(SiteRoot)) || (redirectUrl.StartsWith(SiteRoot.Replace("https://", "http://"))))
                {
                    returnUrl = redirectUrl;
                }
            }

            
            if (returnUrl.Length > 0) { CookieHelper.SetCookie(returnUrlCookieName, returnUrl); }

            

        }


        private void PopulateLabels()
		{
            
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.LoginLink);

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                Resource.MetaDescriptionSignInPageFormat, siteSettings.SiteName);
            
            //if (siteSettings.UseEmailForLogin && !siteSettings.UseLdapAuth)
            //{
            //    this.lblUserID.Visible = false;
            //}
            //else
            //{
            //    this.lblEmail.Visible = false;
            //}

            //txtUserName.Focus();

            //lnkRecovery.Visible = (siteSettings.AllowPasswordRetrieval && (!siteSettings.UseLdapAuth ||
            //                                                               (siteSettings.UseLdapAuth && WebConfigSettings.UseLDAPFallbackAuthentication)));

            //lnkRecovery.NavigateUrl = this.LoginCtrl.PasswordRecoveryUrl;
            //lnkRecovery.Text = this.LoginCtrl.PasswordRecoveryText;

            //lnkExtraLink.NavigateUrl = SiteRoot + "/Secure/Register.aspx";
            //lnkExtraLink.Text = Resource.RegisterLink;
            //lnkExtraLink.Visible = siteSettings.AllowNewRegistration;

            //string returnUrlParam = Page.Request.Params.Get("returnurl");
            //if (!String.IsNullOrEmpty(returnUrlParam))
            //{
            //    //string redirectUrl = returnUrlParam;
            //    lnkExtraLink.NavigateUrl += "?returnurl=" + returnUrlParam;
            //}

            //chkRememberMe.Visible = WebConfigSettings.AllowPersistentLoginCookie;
            //chkRememberMe.Text = this.LoginCtrl.RememberMeText;

            //btnLogin.Text = this.LoginCtrl.LoginButtonText;
            //SiteUtils.SetButtonAccessKey(btnLogin, AccessKeys.LoginAccessKey);

            litOr.Text = Resource.LiteralOr;

            AddClassToBody("loginpage");
        }

    }
}
