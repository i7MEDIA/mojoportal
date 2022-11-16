// Author:                     
// Created:                    2006-04-27
// Last Modified:              2010-11-08
//
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages
{

    public partial class ChangePassword : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChangePassword));


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.Load += new EventHandler(this.Page_Load);
            ChangePassword1.ChangedPassword += new EventHandler(ChangePassword1_ChangedPassword);

            if (WebConfigSettings.HideMenusOnChangePasswordPage) { SuppressAllMenus(); }
            
        }

        

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            PopulateLabels();
        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ChangePasswordLabel);

            Button changePasswordButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton");
            Button cancelButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CancelPushButton");

            if (changePasswordButton != null)
            {
                changePasswordButton.Text = Resource.ChangePasswordButton;
                SiteUtils.SetButtonAccessKey(changePasswordButton, AccessKeys.ChangePasswordButtonAccessKey);
            }
            else
            {
                log.Debug("couldn't find changepasswordbutton so couldn't set label");
            }

            if (cancelButton != null)
            {
                cancelButton.Text = Resource.ChangePasswordCancelButton;
                SiteUtils.SetButtonAccessKey(cancelButton, AccessKeys.ChangePasswordCancelButtonAccessKey);
            }
            else
            {
                log.Debug("couldn't find cancelbutton so couldn't set label");
            }


            this.ChangePassword1.CancelDestinationPageUrl 
                = SiteUtils.GetNavigationSiteRoot() + "/Secure/UserProfile.aspx";

            this.ChangePassword1.ChangePasswordFailureText
                = Resource.ChangePasswordFailureText;

            CompareValidator newPasswordCompare
                = (CompareValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare");

            if (newPasswordCompare != null)
            {
                newPasswordCompare.ErrorMessage = Resource.ChangePasswordMustMatchConfirmMessage;
            }

            RequiredFieldValidator confirmNewPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired");

            if (confirmNewPasswordRequired != null)
            {
                confirmNewPasswordRequired.ErrorMessage = Resource.ChangePasswordConfirmPasswordRequiredMessage;
            }

            this.ChangePassword1.ContinueDestinationPageUrl 
                = SiteUtils.GetNavigationSiteRoot();

            RequiredFieldValidator newPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired");

            if (newPasswordRequired != null)
            {
                newPasswordRequired.ErrorMessage = Resource.ChangePasswordNewPasswordRequired;
            }

            
            RequiredFieldValidator currentPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired");

            if (currentPasswordRequired != null)
            {
                currentPasswordRequired.ErrorMessage = Resource.ChangePasswordCurrentPasswordRequiredWarning;
            }

            RegularExpressionValidator newPasswordRegex
                = (RegularExpressionValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRegex");

            if (newPasswordRegex != null)
            {
                newPasswordRegex.ErrorMessage = Resource.ChangePasswordPasswordRegexFailureMessage;
                if (siteSettings.PasswordRegexWarning.Length > 0)
                {
                    newPasswordRegex.ErrorMessage = siteSettings.PasswordRegexWarning;
                }

                newPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;

                if (Membership.PasswordStrengthRegularExpression.Length == 0)
                {
                    newPasswordRegex.Visible = false;
                    newPasswordRegex.ValidationGroup = "None";
                }
            }

            CustomValidator newPasswordRulesValidator
                = (CustomValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRulesValidator");
            if (newPasswordRulesValidator != null)
            {
                newPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
            }

            this.ChangePassword1.SuccessTitleText = String.Empty;
            this.ChangePassword1.SuccessText = Resource.ChangePasswordSuccessText;

            //if (siteSettings.ShowPasswordStrengthOnRegistration)
            //{
            //    PasswordStrength passwordStrengthChecker = (PasswordStrength)ChangePassword1.ChangePasswordTemplateContainer.FindControl("passwordStrengthChecker");
            //    if (passwordStrengthChecker != null)
            //    {
            //        passwordStrengthChecker.Enabled = true;
            //        passwordStrengthChecker.RequiresUpperAndLowerCaseCharacters = true;
            //        passwordStrengthChecker.MinimumLowerCaseCharacters = WebConfigSettings.PasswordStrengthMinimumLowerCaseCharacters;
            //        passwordStrengthChecker.MinimumUpperCaseCharacters = WebConfigSettings.PasswordStrengthMinimumUpperCaseCharacters;
            //        passwordStrengthChecker.MinimumSymbolCharacters = siteSettings.MinRequiredNonAlphanumericCharacters;
            //        passwordStrengthChecker.PreferredPasswordLength = siteSettings.MinRequiredPasswordLength;

            //        passwordStrengthChecker.PrefixText = Resource.PasswordStrengthPrefix;
            //        passwordStrengthChecker.TextStrengthDescriptions = Resource.PasswordStrengthDescriptions;
            //        passwordStrengthChecker.CalculationWeightings = WebConfigSettings.PasswordStrengthCalculationWeightings;

            //        try
            //        {
            //            passwordStrengthChecker.StrengthIndicatorType = (StrengthIndicatorTypes)Enum.Parse(typeof(StrengthIndicatorTypes), WebConfigSettings.PasswordStrengthIndicatorType, true);
            //        }
            //        catch (ArgumentException)
            //        {
            //            passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
            //        }
            //        catch (OverflowException)
            //        {
            //            passwordStrengthChecker.StrengthIndicatorType = StrengthIndicatorTypes.Text;
            //        }

            //        try
            //        {
            //            passwordStrengthChecker.DisplayPosition = (DisplayPosition)Enum.Parse(typeof(DisplayPosition), WebConfigSettings.PasswordStrengthDisplayPosition, true);
            //        }
            //        catch (ArgumentException)
            //        {
            //            passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
            //        }
            //        catch (OverflowException)
            //        {
            //            passwordStrengthChecker.DisplayPosition = DisplayPosition.RightSide;
            //        }
            //    }

            //}

            AddClassToBody("changepassword");
            

        }

        void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        {
            //this is needed to prevent a script error in IE8 after the password is changed
            ValidationSummary vSummary = (ValidationSummary)ChangePassword1.ChangePasswordTemplateContainer.FindControl("vSummary");
            if (vSummary != null) { vSummary.Visible = false; }
            if (WebConfigSettings.LogIpAddressForPasswordChanges)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    log.Info("user " + currentUser.Name + " changed their password from ip address " + SiteUtils.GetIP4Address());
                }
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

            TextBox currentPassword
                    = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword");

            TextBox newPassword
                    = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPassword");

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser != null)
            {
                if (currentPassword != null)
                {
                    switch (Membership.Provider.PasswordFormat)
                    {
                        case MembershipPasswordFormat.Clear:
                            if (currentPassword.Text != currentUser.Password)
                            {
                                args.IsValid = false;
                                validator.ErrorMessage
                                    += Resource.ChangePasswordCurrentPasswordIncorrectWarning + "<br />";
                                    
                            }
                            break;

                        case MembershipPasswordFormat.Encrypted:

                            break;

                        case MembershipPasswordFormat.Hashed:

                            break;

                    }
                }

            }

            if ((newPassword != null) && (currentPassword != null))
            {
                if (newPassword.Text == currentPassword.Text)
                {
                    args.IsValid = false;
                    validator.ErrorMessage
                       += Resource.ChangePasswordNewMatchesOldWarning + "<br />";

                }
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
                if(!alphanumeric.Contains(c.ToString()))
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

        
    }
}
