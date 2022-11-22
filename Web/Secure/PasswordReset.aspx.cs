// Author:					
// Created:					2011-01-07
// Last Modified:			2011-01-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using AjaxControlToolkit;
using Resources;



namespace mojoPortal.Web.UI.Pages
{

    public partial class PasswordResetPage : NonCmsBasePage
    {
        private SiteUser siteUser = null;
        private string redirectUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            
            LoadSettings();

            if (!Request.IsAuthenticated)
            {
                WebUtils.SetupRedirect(this, redirectUrl);
                return;
            }

            // this page is only for user who must change password
            // the regular ChangePassword.aspx uses the ChangePassword Wizard which requires the user to enter the current password
            if ((siteUser == null) || (!siteUser.MustChangePwd))
            {
                WebUtils.SetupRedirect(this, redirectUrl);
                return;

            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }

        void btnChangePassword_Click(object sender, EventArgs e)
        {
            Page.Validate("ChangePassword1");
            if (Page.IsValid)
            {
                siteUser.PasswordResetGuid = Guid.Empty;
                mojoMembershipProvider m = Membership.Provider as mojoMembershipProvider;
                siteUser.Password = m.EncodePassword(siteSettings, siteUser, txtNewPassword.Text);
                siteUser.MustChangePwd = false;
                siteUser.Save();
                siteUser.UpdateLastPasswordChangeTime();

                WebUtils.SetupRedirect(this, redirectUrl);
                return;

            }


            

        }

        void NewPasswordRulesValidator_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            CustomValidator validator = source as CustomValidator;
            validator.ErrorMessage = string.Empty;

            if (args.Value.Length < Membership.MinRequiredPasswordLength)
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += Resource.ChangePasswordMinimumLengthWarning
                    + Membership.MinRequiredPasswordLength.ToInvariantString() + "<br />";
            }

            if (!HasEnoughNonAlphaNumericCharacters(args.Value))
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += Resource.ChangePasswordMinNonAlphanumericCharsWarning
                    + Membership.MinRequiredNonAlphanumericCharacters.ToInvariantString() + "<br />";

            }

            mojoMembershipProvider m = Membership.Provider as mojoMembershipProvider;
            if (siteUser.Password == m.EncodePassword(txtNewPassword.Text, siteUser.PasswordSalt, siteSettings))
            {
                args.IsValid = false;
                validator.ErrorMessage += Resource.ChangePasswordNewMatchesOldWarning + "<br />";
            }

           

        }

        private bool HasEnoughNonAlphaNumericCharacters(string newPassword)
        {
            bool result = false;
            string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] passwordChars = newPassword.ToCharArray();
            int nonAlphaNumericCharCount = 0;
            foreach (char c in passwordChars)
            {
                if (!alphanumeric.Contains(c.ToString()))
                {
                    nonAlphaNumericCharCount += 1;
                }
            }

            if (nonAlphaNumericCharCount >= Membership.MinRequiredNonAlphanumericCharacters)
            {
                result = true;
            }

            return result;
        }

        


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PasswordReset);

            btnChangePassword.Text = Resource.ChangePasswordButton;
            NewPasswordCompare.ErrorMessage = Resource.ChangePasswordMustMatchConfirmMessage;
            ConfirmNewPasswordRequired.ErrorMessage = Resource.ChangePasswordConfirmPasswordRequiredMessage;
            NewPasswordRequired.ErrorMessage = Resource.ChangePasswordNewPasswordRequired;
            NewPasswordRegex.ErrorMessage = Resource.ChangePasswordPasswordRegexFailureMessage;
            if (siteSettings.PasswordRegexWarning.Length > 0)
            {
                NewPasswordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
            }

            NewPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;
            if (Membership.PasswordStrengthRegularExpression.Length == 0)
            {
                NewPasswordRegex.Enabled = false;
            }

            //if (siteSettings.ShowPasswordStrengthOnRegistration)
            //{
               
            //    passwordStrengthChecker.Enabled = true;
            //    passwordStrengthChecker.RequiresUpperAndLowerCaseCharacters = true;
            //    passwordStrengthChecker.MinimumLowerCaseCharacters = WebConfigSettings.PasswordStrengthMinimumLowerCaseCharacters;
            //    passwordStrengthChecker.MinimumUpperCaseCharacters = WebConfigSettings.PasswordStrengthMinimumUpperCaseCharacters;
            //    passwordStrengthChecker.MinimumSymbolCharacters = siteSettings.MinRequiredNonAlphanumericCharacters;
            //    passwordStrengthChecker.PreferredPasswordLength = siteSettings.MinRequiredPasswordLength;
            //    passwordStrengthChecker.PrefixText = Resource.PasswordStrengthPrefix;
            //    passwordStrengthChecker.TextStrengthDescriptions = Resource.PasswordStrengthDescriptions;
            //    passwordStrengthChecker.CalculationWeightings = WebConfigSettings.PasswordStrengthCalculationWeightings;

            //    try
            //    {
            //        passwordStrengthChecker.StrengthIndicatorType = (StrengthIndicatorTypes)Enum.Parse(typeof(StrengthIndicatorTypes), WebConfigSettings.PasswordStrengthIndicatorType, true);
            //    }
            //    catch (ArgumentException)
            //    {
            //        passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
            //    }
            //    catch (OverflowException)
            //    {
            //        passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
            //    }

            //    try
            //    {
            //        passwordStrengthChecker.DisplayPosition = (DisplayPosition)Enum.Parse(typeof(DisplayPosition), WebConfigSettings.PasswordStrengthDisplayPosition, true);
            //    }
            //    catch (ArgumentException)
            //    {
            //        passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
            //    }
            //    catch (OverflowException)
            //    {
            //        passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
            //    }
                

            //}

        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();

            string returnUrlParam = Page.Request.Params.Get("returnurl");
            if (!String.IsNullOrEmpty(returnUrlParam))
            {
                returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
                string requestedRedirectUrl = Page.ResolveUrl(SecurityHelper.RemoveMarkup(Page.Server.UrlDecode(returnUrlParam)));
                if (requestedRedirectUrl.StartsWith("/"))
                {
                    redirectUrl = requestedRedirectUrl;
                }
            }
            else
            {
                redirectUrl = SiteRoot;
                if (
                    (!siteSettings.IsServerAdminSite)
                    && (WebConfigSettings.UseFolderBasedMultiTenants)
                    && (WebConfigSettings.AppendDefaultPageToFolderRootUrl)
                    )
                {
                    redirectUrl += "/Default.aspx";
                }
            }

            

            AddClassToBody("passwordreset");
        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            IsChangePasswordPage = true; //used to prevent an infinite redirect loop when forcing redirect to change password in mojoBasePage
            this.Load += new EventHandler(this.Page_Load);
            NewPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
            btnChangePassword.Click += new EventHandler(btnChangePassword_Click);

            if (WebConfigSettings.HideMenusOnChangePasswordPage) { SuppressAllMenus(); }

        }

        

        #endregion
    }
}
