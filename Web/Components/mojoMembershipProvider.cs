// Author:             
// Created:            2006-01-19
// Last Modified:      2012-03-19

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    
    public class mojoMembershipProvider : MembershipProvider
    {
        
        #region Private Properties

        private static readonly log4net.ILog log
            = log4net.LogManager.GetLogger(typeof(mojoMembershipProvider));

        private String description = String.Empty;
        private String name = String.Empty;
        private String applicationName = String.Empty;
      
        private const int LoginnameMaxlength = 50;
        private const int EmailMaxlength = 100;
        private const int PasswordquestionMaxlength = 255;
        private const int PasswordanswerMaxlength = 255;
        private const int PasswordSize = 14;
        //private MachineKeySection machineKey;


        #endregion

        private static SiteSettings GetSiteSettings()
        {
            if (WebConfigSettings.UseRelatedSiteMode)
            {
                return CacheHelper.GetSiteSettings(WebConfigSettings.RelatedSiteID);
            }

            return CacheHelper.GetCurrentSiteSettings();
        }

        #region Public Properties

        public override string ApplicationName
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.SiteName;
                return applicationName; 
            }
            set { applicationName = value;}
        }

        public override string Description
        {
            get { return description; }
        }

        public override bool EnablePasswordReset
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.AllowPasswordReset;

                return false; 
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.AllowPasswordRetrieval;
                
                return false; 
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.MaxInvalidPasswordAttempts;

                return 5; 
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.MinRequiredNonAlphanumericCharacters;

                return 0; 
            }
        }

        public override int MinRequiredPasswordLength
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.MinRequiredPasswordLength;
                return 4; 
            }
        }

        public override string Name
        {
            get { return name; }
        }

        public override int PasswordAttemptWindow
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.PasswordAttemptWindowMinutes;

                return 5; 
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null)
                {
                    return (MembershipPasswordFormat)siteSettings.PasswordFormat;
                }

                return MembershipPasswordFormat.Clear; 
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.PasswordStrengthRegularExpression;

                return string.Empty; 
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.RequiresQuestionAndAnswer;

                return true; 
            }
        }

        public override bool RequiresUniqueEmail
        {
            get 
            {
                SiteSettings siteSettings = GetSiteSettings();
                if (siteSettings != null) return siteSettings.RequiresUniqueEmail;

                return true; 
            }
        }


        #endregion

        #region Private Methods


        private MembershipUser CreateMembershipUserFromSiteUser(SiteUser siteUser)
        {
            SiteSettings siteSettings = GetSiteSettings();
            if ((siteUser == null) || (siteUser.UserId <= 0)) return null;

            if (siteSettings.UseEmailForLogin)
            {
                return new MembershipUser(
                    this.name,
                    siteUser.Email,
                    siteUser.UserGuid,
                    siteUser.Email,
                    siteUser.PasswordQuestion,
                    siteUser.Comment,
                    siteUser.ProfileApproved,
                    siteUser.IsLockedOut,
                    siteUser.DateCreated,
                    siteUser.LastLoginDate,
                    siteUser.LastActivityDate,
                    siteUser.LastPasswordChangedDate,
                    siteUser.LastLockoutDate);
            }
            else
            {
                return new MembershipUser(
                    this.name,
                    siteUser.LoginName,
                    siteUser.UserGuid,
                    siteUser.Email,
                    siteUser.PasswordQuestion,
                    siteUser.Comment,
                    siteUser.ProfileApproved,
                    siteUser.IsLockedOut,
                    siteUser.DateCreated,
                    siteUser.LastLoginDate,
                    siteUser.LastActivityDate,
                    siteUser.LastPasswordChangedDate,
                    siteUser.LastLockoutDate);
            }
        }


        #endregion

        #region Public Methods

        public override void Initialize(string name, NameValueCollection config)
        {
            
            base.Initialize(name, config);
            this.name = "mojoMembershipProvider";
            this.description = "mojoPortal Membership Provider";

        }

        

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            /*
             * Takes, as input, a user name, a password (the user's current password), and a 
             * new password and updates the password in the membership data source. 
             * ChangePassword returns true if the password was updated successfully. Otherwise, 
             * it returns false. Before changing a password, ChangePassword calls the provider's 
             * virtual OnValidatingPassword method to validate the new password. It then 
             * changes the password or cancels the action based on the outcome of the call. If the 
             * user name, password, new password, or password answer is not valid, 
             * ChangePassword does not throw an exception; it simply returns false. Following a 
             * successful password change, ChangePassword updates the user's 
             * LastPasswordChangedDate.
             */

            bool result = false;

            if (
                (username == null) || (username == String.Empty)
                || (oldPassword == null) || (oldPassword == String.Empty)
                || (newPassword == null) || (newPassword == String.Empty)
                )
            {
                return result;
            }

            SiteSettings siteSettings = GetSiteSettings();
            if (siteSettings == null) { return result; }

            if (newPassword.Length < siteSettings.MinRequiredPasswordLength)
            {
                throw new ArgumentException(ResourceHelper.GetMessageTemplate("PasswordNotLongEnoughMessage.config"));
            }

            int countNonAlphanumericCharacters = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    countNonAlphanumericCharacters++;
                }
            }

            if (countNonAlphanumericCharacters < siteSettings.MinRequiredNonAlphanumericCharacters)
            {
                throw new ArgumentException(ResourceHelper.GetMessageTemplate("PasswordRequiresMoreNonAlphanumericCharactersMessage.config"));
            }

            if (siteSettings.PasswordStrengthRegularExpression.Length > 0)
            {
                if (!Regex.IsMatch(newPassword, siteSettings.PasswordStrengthRegularExpression))
                {
                    throw new ArgumentException(
                        ResourceHelper.GetMessageTemplate("PasswordDoesntMatchRegularExpressionMessage.config"));
                }
            }

            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);

            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                else
                {
                    throw new ArgumentException("The custom password validation failed.");
                }
            }

            SiteUser siteUser = new SiteUser(siteSettings, username);
            if (siteUser.UserId == -1) { return result; }

            if (
                ((MembershipPasswordFormat)siteSettings.PasswordFormat == MembershipPasswordFormat.Hashed)
                && (!siteSettings.UseLdapAuth)
                )
            {
                if (siteUser.Password == EncodePassword(siteUser.PasswordSalt + oldPassword,MembershipPasswordFormat.Hashed))
                {
                    siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    siteUser.Password = EncodePassword(siteUser.PasswordSalt + newPassword, MembershipPasswordFormat.Hashed);
                    siteUser.MustChangePwd = false;
                    siteUser.PasswordFormat = siteSettings.PasswordFormat;
                    result = siteUser.Save();
                }
            }
            else if ((MembershipPasswordFormat)siteSettings.PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                if (siteUser.Password == EncodePassword(siteUser.PasswordSalt + oldPassword, MembershipPasswordFormat.Encrypted))
                {
                    siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    siteUser.Password = EncodePassword(siteUser.PasswordSalt + newPassword, MembershipPasswordFormat.Encrypted);
                    siteUser.MustChangePwd = false;
                    siteUser.PasswordFormat = siteSettings.PasswordFormat;
                    result = siteUser.Save();
                }
            }
            else if ((MembershipPasswordFormat)siteSettings.PasswordFormat == MembershipPasswordFormat.Clear)
            {
                if (siteUser.Password == oldPassword)
                {
                    siteUser.Password = newPassword;
                    siteUser.MustChangePwd = false;
                    siteUser.PasswordFormat = siteSettings.PasswordFormat;
                    result = siteUser.Save();
                }
            }

            if (result)
            {
                if (WebConfigSettings.LogIpAddressForPasswordChanges)
                {
                   log.Info("password for user " + siteUser.Name + " was changed from ip address " + SiteUtils.GetIP4Address());
                }

                siteUser.UpdateLastPasswordChangeTime();
            }

            
            return result;
           
        }

        public override bool ChangePasswordQuestionAndAnswer(
            string userName,
            string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            /*
             * 	Takes, as input, a user name, password, password question, and password answer and 
             * updates 
             * the password question and answer in the data source if the user name and password are valid. 
             * This method 
             * returns true if the password question and answer are successfully updated. Otherwise, 
             * it returns false. 
             * ChangePasswordQuestionAndAnswer returns false if either the user name or password is invalid.
             */
            
            if (
                String.IsNullOrEmpty(userName)
                || (password == null)
                || String.IsNullOrEmpty(newPasswordQuestion)
                || String.IsNullOrEmpty(newPasswordAnswer)
                || (newPasswordQuestion.Length > PasswordquestionMaxlength)
                || (newPasswordAnswer.Length > PasswordanswerMaxlength)
                )
            {
                return false;
            }

            SiteSettings siteSettings = GetSiteSettings();
            if (siteSettings == null) { return false; }
            
            SiteUser siteUser = new SiteUser(siteSettings, userName);
            if (siteUser.UserId > -1 && ValidateUser(userName, password))
            {
                return siteUser.UpdatePasswordQuestionAndAnswer(newPasswordQuestion, newPasswordAnswer);
            }
            
            return false;
        }


        public override MembershipUser CreateUser(
            string userName,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            /*
             * Takes, as input, a user name, password, e-mail address, and other information and adds 
             * a new 
             * user to the membership data source. CreateUser returns a MembershipUser object 
             * representing the 
             * newly created user. It also accepts an out parameter  that returns a 
             * MembershipCreateStatus value indicating whether the user was successfully created or, 
             * if the user 
             * was not created, the reason why. If the user was not created, CreateUser returns null. 
             * Before creating a new user, 
             * CreateUser calls the provider's virtual OnValidatingPassword method to validate the 
             * supplied password. 
             * It then creates the user or cancels the action based on the outcome of the call.
             */

            SiteSettings siteSettings = GetSiteSettings();
            if (siteSettings == null)
            {
                status = MembershipCreateStatus.UserRejected;
                return null;
            }

            if ((siteSettings.UseEmailForLogin) && (WebConfigSettings.AutoGenerateAndHideUserNamesWhenUsingEmailForLogin))
            {
                userName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, email);
            }


            if (String.IsNullOrEmpty(userName) || userName.Length > LoginnameMaxlength)
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if (String.IsNullOrEmpty(email) || email.Length > EmailMaxlength)
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (String.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (siteSettings.RequiresQuestionAndAnswer)
            {
                if (String.IsNullOrEmpty(passwordQuestion)
                    || passwordQuestion.Length > PasswordquestionMaxlength)
                {
                    status = MembershipCreateStatus.InvalidQuestion;
                    return null;
                }

                if (String.IsNullOrEmpty(passwordAnswer)
                    || passwordAnswer.Length > PasswordanswerMaxlength)
                {
                    status = MembershipCreateStatus.InvalidAnswer;
                    return null;
                }
            }

            SiteUser existingUser = null;

            // this can return true if there is an existing user even if that iser is flagged as deleted
            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, email))
            {
                if (WebConfigSettings.AllowNewRegistrationToActivateDeletedAccountWithSameEmail)
                {
                    existingUser = SiteUser.GetByEmail(siteSettings, email);
                    if ((existingUser != null) && (!existingUser.IsDeleted))
                    {
                        // if it isn't a deleted account set it back to null
                        // we can't let a new registration assum this user
                        existingUser = null;
                    }
                }

                if (existingUser == null)
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }
            }

            // this can return true if there is an existing user even if that iser is flagged as deleted
            // however just because someone chose the same login name doesn't mean it is the same person
            if (SiteUser.LoginExistsInDB(siteSettings.SiteId, userName))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (password.Length < MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }


            int nonAlphaNumericCharactersUsedCount = 0;

            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i))
                {
                    nonAlphaNumericCharactersUsedCount++;
                }
            }

            if (nonAlphaNumericCharactersUsedCount < siteSettings.MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }


            if (siteSettings.PasswordStrengthRegularExpression.Length > 0)
            {
                if (!Regex.IsMatch(password, siteSettings.PasswordStrengthRegularExpression))
                {
                    status = MembershipCreateStatus.InvalidPassword;
                    return null;
                }
            }


            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(userName, password, true);
            this.OnValidatingPassword(e);

            if (e.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            SiteUser siteUser;
            if (existingUser != null)
            {
                siteUser = existingUser;  
            }
            else
            {
                siteUser = new SiteUser(siteSettings);
            }
            siteUser.Name = userName;
            siteUser.LoginName = userName;
            siteUser.Email = email;
            siteUser.PasswordQuestion = passwordQuestion;
            siteUser.PasswordAnswer = passwordAnswer;
            siteUser.ProfileApproved = isApproved;

            if (PasswordFormat != MembershipPasswordFormat.Clear)
            {
                siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                password = EncodePassword(siteUser.PasswordSalt + password, PasswordFormat);
            }

            siteUser.Password = password;
            siteUser.ApprovedForLogin = !siteSettings.RequireApprovalBeforeLogin;
            siteUser.PasswordFormat = siteSettings.PasswordFormat;
            bool created = siteUser.Save();

            if (existingUser != null)
            {
                // was flagged as deleted
                // need to unflag
                SiteUser.FlagAsNotDeleted(siteUser.UserId);

            }


            if (created)
            {
                if (siteSettings.UseSecureRegistration)
                {
                    Guid registerConfirmGuid = Guid.NewGuid();
                    siteUser.SetRegistrationConfirmationGuid(registerConfirmGuid);

                    // send email with confirmation link that will approve profile
                    Notification.SendRegistrationConfirmationLink(
                        SiteUtils.GetSmtpSettings(),
                        ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "RegisterConfirmEmailMessage.config"),
                        siteSettings.DefaultEmailFromAddress,
                        siteSettings.DefaultFromEmailAlias,
                        siteUser.Email,
                        siteSettings.SiteName,
                        SiteUtils.GetNavigationSiteRoot() + "/ConfirmRegistration.aspx?ticket=" +
                        registerConfirmGuid.ToString()
                        + "&returnurl=" + GetReturnUrl()
                        );
                }
                else
                {
                    NewsletterHelper.ClaimExistingSubscriptions(siteUser);
                }

                status = MembershipCreateStatus.Success;
                return CreateMembershipUserFromSiteUser(siteUser);
            }
            else
            {
                status = MembershipCreateStatus.UserRejected;
                return null;
            }
        }

        private string GetReturnUrl()
        {
            if(System.Web.HttpContext.Current != null)
            {
                string returnUrlParam = System.Web.HttpContext.Current.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    return returnUrlParam;
                }
            }

            return "/";
        }


        public override bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            /*
             * 	Takes, as input, a user name and deletes that user from the membership data source. DeleteUser returns 
             * true if the user was successfully deleted. Otherwise, it returns false. DeleteUser takes a third parameter-a Boolean 
             * named deleteAllRelatedData-that specifies whether related data for that user should be deleted also. 
             * If deleteAllRelatedData is true, DeleteUser should delete role data, profile data, and all other data associated 
             * with that user.
             */
            bool result = false;
            SiteSettings siteSettings = GetSiteSettings();
            // we are ignoring deleteAllRelatedData
            // on purpose because whether to really delete or just flag as deleted
            // is determined by the siteSettings.ReallyDeleteUsers setting

            if ((userName != null) && (siteSettings != null))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                if (siteUser.UserId > -1)
                {
                    result = siteUser.DeleteUser();

                }
            }

            return result;

            
        }

        public override MembershipUserCollection FindUsersByEmail(
            string emailToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            /*
             * Returns a MembershipUserCollection containing MembershipUser objects representing 
             * users whose e-mail addresses match the emailToMatch input parameter. Wildcard syntax 
             * is data source-dependent. MembershipUser objects in the MembershipUserCollection are 
             * sorted by e-mail address. If FindUsersByEmail finds no matching users, it returns an empty 
             * MembershipUserCollection.
             */
            SiteSettings siteSettings = GetSiteSettings();
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            if (
                (siteSettings != null)
                && (emailToMatch != null)
                && (emailToMatch.Length > 5)
                )
            {

            }

            using(IDataReader reader = SiteUser.GetUserByEmail(siteSettings.SiteId, emailToMatch))
            {
                while (reader.Read())
                {
                    MembershipUser user = new MembershipUser(
                        this.name,
                        reader["LoginName"].ToString(),
                        reader["UserGuid"],
                        reader["Email"].ToString(),
                        reader["PasswordQuestion"].ToString(),
                        reader["Comment"].ToString(),
                        Convert.ToBoolean(reader["ProfileApproved"], CultureInfo.InvariantCulture),
                        Convert.ToBoolean(reader["IsLockedOut"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["DateCreated"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastLoginDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastActivityDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastPasswordChangedDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastLockoutDate"], CultureInfo.InvariantCulture));

                    users.Add(user);
                    totalRecords += 1;

                }
            }
            
            return users;
        }

        public override MembershipUserCollection FindUsersByName(
            string usernameToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;
            SiteSettings siteSettings = GetSiteSettings();

            if (
                (siteSettings != null)
                && (usernameToMatch != null)
                && (usernameToMatch.Length > 0)
                )
            {

            }

            using(IDataReader reader = SiteUser.GetUserByLoginName(
                siteSettings.SiteId, 
                usernameToMatch,
                (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap && siteSettings.AllowEmailLoginWithLdapDbFallback)
                ))
            {
                while (reader.Read())
                {
                    MembershipUser user = new MembershipUser(
                        this.name,
                        reader["LoginName"].ToString(),
                        reader["UserGuid"],
                        reader["Email"].ToString(),
                        reader["PasswordQuestion"].ToString(),
                        reader["Comment"].ToString(),
                        Convert.ToBoolean(reader["ProfileApproved"], CultureInfo.InvariantCulture),
                        Convert.ToBoolean(reader["IsLockedOut"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["DateCreated"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastLoginDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastActivityDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastPasswordChangedDate"], CultureInfo.InvariantCulture),
                        Convert.ToDateTime(reader["LastLockoutDate"], CultureInfo.InvariantCulture));

                    users.Add(user);
                    totalRecords += 1;

                }
            }
            

            return users;

        }

        public override MembershipUserCollection GetAllUsers(
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            /*
             Returns a MembershipUserCollection containing MembershipUser objects representing all registered users. 
             * If there are no registered users, GetAllUsers returns an empty MembershipUserCollection. The results returned 
             * by GetAllUsers are constrained by the pageIndex and pageSize input parameters. pageSize specifies the 
             * maximum number of MembershipUser objects to return. pageIndex identifies which page of results to return. 
             * Page indexes are 0-based. GetAllUsers also takes an out parameter (in Visual Basic, ByRef) named totalRecords 
             * that, on return, holds a count of all registered users.
             */

            SiteSettings siteSettings = GetSiteSettings();
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;

            if (siteSettings != null)  
            {
                int totalPages = 1;

                List<SiteUser> siteUserPage = SiteUser.GetPage(
                    siteSettings.SiteId,
                    pageIndex,
                    pageSize,
                    string.Empty,
                    0,
					"display",
                    out totalPages);

                foreach(SiteUser siteUser in siteUserPage)
                {
                    MembershipUser user = new MembershipUser(
                       this.name,
                       siteUser.LoginName,
                       siteUser.UserGuid,
                       siteUser.Email,
                       siteUser.PasswordQuestion,
                       siteUser.Comment,
                       siteUser.ProfileApproved,
                       siteUser.IsLockedOut,
                       siteUser.DateCreated,
                       siteUser.LastLoginDate,
                       siteUser.LastActivityDate,
                       siteUser.LastPasswordChangedDate,
                       siteUser.LastLockoutDate);

                    users.Add(user);
                }

                totalRecords = SiteUser.UserCount(siteSettings.SiteId);
               

            }

            return users;

        }

        public override int GetNumberOfUsersOnline()
        {
            int result = 0;

            /*
             Returns a count of users that are currently online-that is, whose LastActivityDate is greater 
             * than the current date and time minus the value of the membership service's UserIsOnlineTimeWindow 
             * property, which can be read from Membership.UserIsOnlineTimeWindow. UserIsOnlineTimeWindow 
             * specifies a time in minutes and is set using the <membership> element's 
             * userIsOnlineTimeWindow attribute.
             */
            SiteSettings siteSettings = GetSiteSettings();
            if (siteSettings != null)
            {
                DateTime sinceTime = DateTime.UtcNow.AddMinutes(-Membership.UserIsOnlineTimeWindow);
                result = SiteUser.UsersOnlineSinceCount(siteSettings.SiteId, sinceTime);
            }

            return result;
        }

       

        public override string GetPassword(string userName, string passwordAnswer)
        {
            /*
             * Takes, as input, a user name and a password answer and returns that user's password. 
             * If the user name is not valid, GetPassword throws a ProviderException. Before retrieving 
             * a password, GetPassword verifies that EnablePasswordRetrieval is true. 
             * If EnablePasswordRetrieval is false, GetPassword throws a NotSupportedException. 
             * If EnablePasswordRetrieval is true but the password format is hashed, GetPassword 
             * throws a ProviderException since hashed passwords cannot, by definition, be retrieved. 
             * A membership provider should also throw a ProviderException from Initialize if 
             * EnablePasswordRetrieval is true but the password format is hashed. GetPassword also 
             * checks the value of the RequiresQuestionAndAnswer property before retrieving a password. 
             * If RequiresQuestionAndAnswer is true, GetPassword compares the supplied password 
             * answer to the stored password answer and throws a MembershipPasswordException if 
             * the two don't match. GetPassword also throws a MembershipPasswordException if the 
             * user whose password is being retrieved is currently locked out.
             */

            SiteSettings siteSettings = GetSiteSettings();

            if (!siteSettings.AllowPasswordRetrieval)
            {
                throw new MojoMembershipException(
                    ResourceHelper.GetMessageTemplate("PasswordRetrievalNotEnabledMessage.config")
                    );
            }

            
            if ((userName != null) && (siteSettings != null))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                if (siteUser.UserId > -1)
                {
                    if (siteUser.IsLockedOut)
                    {
                        throw new MembershipPasswordException(
                            ResourceHelper.GetMessageTemplate("UserAccountLockedMessage.config"));
                    }

                    if (siteUser.IsDeleted)
                    {
                        throw new MembershipPasswordException(
                            ResourceHelper.GetMessageTemplate("UserNotFoundMessage.config"));
                    }

                    bool okToGetPassword = false;
                    if (siteSettings.RequiresQuestionAndAnswer)
                    {
                        if ((passwordAnswer != null) && (PasswordAnswerIsMatch(passwordAnswer, siteUser.PasswordAnswer)))
                        {
                            okToGetPassword = true;
                        }
                        else
                        {
                            if (siteSettings.MaxInvalidPasswordAttempts > 0)
                            {
                                siteUser.IncrementPasswordAnswerAttempts(siteSettings);

                                if (WebConfigSettings.LockAccountOnMaxPasswordAnswerTries)
                                {
                                    if (siteUser.FailedPasswordAnswerAttemptCount >= siteSettings.MaxInvalidPasswordAttempts)
                                    {
                                        siteUser.LockoutAccount();
                                    }
                                }

                            }
                        }

                    }
                    else
                    {
                        okToGetPassword = true;
                    }

                    if(okToGetPassword)
                    {
                        if (siteSettings.RequirePasswordChangeOnResetRecover)
                        {
                            siteUser.MustChangePwd = true;
                            siteUser.Save();
                        }

                        switch(PasswordFormat)
                        {
                            case MembershipPasswordFormat.Clear:

                                
                                return siteUser.Password;
                                
                            case MembershipPasswordFormat.Encrypted:

                                try
                                {
                                    if (siteUser.PasswordSalt.Length > 0)
                                    {
                                        return UnencodePassword(siteUser.Password, MembershipPasswordFormat.Encrypted).Replace(siteUser.PasswordSalt, string.Empty);
                                    }
                                    else
                                    {
                                        return UnencodePassword(siteUser.Password, MembershipPasswordFormat.Encrypted);
                                    }
                                }
                                catch (FormatException ex)
                                {
                                    log.Error(ex);
                                    throw new MembershipPasswordException("failure retrieving password");
                                }

                            case MembershipPasswordFormat.Hashed:
                                
                                string newPassword = SiteUser.CreateRandomPassword(siteSettings.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);


                                siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                                siteUser.Password = EncodePassword(siteUser.PasswordSalt + newPassword, MembershipPasswordFormat.Hashed);
                                siteUser.PasswordFormat = siteSettings.PasswordFormat;

                                //after the new random password is emailed to the user we can force him to change it again immediately after he logs in
                                siteUser.MustChangePwd = siteSettings.RequirePasswordChangeOnResetRecover; 

                                // needed if we are sending a link for automatic login and force to change password instead of sending the random one by email
                                // will be cleared to Guid.Empty when password is changed
                                siteUser.PasswordResetGuid = Guid.NewGuid(); 
                                siteUser.Save();
                                //siteUser.UnlockAccount();
                                return newPassword;

                                
                        }

                    }
                    else
                    {
                        return null;
                    }
                    
                }
                else
                {
                    throw new ProviderException(ResourceHelper.GetMessageTemplate("UserNotFoundMessage.config"));

                }

            }

            return null;


        }

        private bool PasswordAnswerIsMatch(string suppliedAnswer, string actualAnswer)
        {
            if (WebConfigSettings.AllowCaseInsensitivePasswordQuestionAnswer)
            {
                return string.Equals(suppliedAnswer, actualAnswer, StringComparison.InvariantCultureIgnoreCase);
            }

            return (suppliedAnswer == actualAnswer);

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            /*
             * Takes, as input, a user name or user ID (the method is overloaded) and a Boolean value 
             * indicating whether to update the user's LastActivityDate to show that the user is currently online. 
             * GetUser returns a MembershipUser object representing the specified user. If the user name or 
             * user ID is invalid (that is, if it doesn't represent a registered user) GetUser returns null (Nothing in Visual Basic).
             */
            SiteSettings siteSettings = GetSiteSettings();
            if ((siteSettings != null)&&(providerUserKey != null))
            {
                SiteUser siteUser = null;
                if (providerUserKey is Guid)
                {
                    siteUser = new SiteUser(siteSettings, (Guid)providerUserKey);
                    if (siteUser.UserId > 0)
                    {
                        if (siteUser.IsDeleted) { return null; }

                        if (userIsOnline)
                        {
                            siteUser.UpdateLastActivityTime();
                        }
                        return this.CreateMembershipUserFromSiteUser(siteUser);
                    }

                }

                if (providerUserKey is int)
                {
                    siteUser = new SiteUser(siteSettings, (int)providerUserKey);
                    if (siteUser.UserId > 0)
                    {
                        if (siteUser.IsDeleted) { return null; }

                        if (userIsOnline)
                        {
                            siteUser.UpdateLastActivityTime();
                        }
                        return this.CreateMembershipUserFromSiteUser(siteUser);
                    }
                }

            }



            return null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            /*
             * 	Takes, as input, a user name or user ID (the method is overloaded) and a 
             * Boolean value indicating whether to update the user's LastActivityDate to 
             * show that the user is currently online. GetUser returns a MembershipUser object 
             * representing the specified user. If the user name or user ID is invalid (that is, if 
             * it doesn't represent a registered user) GetUser returns null (Nothing in Visual Basic).
             */
            SiteSettings siteSettings = GetSiteSettings();

            if ((siteSettings != null) && (username != null)&&(username.Length > 0))
            {
                SiteUser siteUser = null;
                siteUser = new SiteUser(siteSettings, username);
                if (siteUser.UserId > 0)
                {
                    if (siteUser.IsDeleted) { return null; }

                    if (userIsOnline)
                    {
                        siteUser.UpdateLastActivityTime();
                    }
                    return this.CreateMembershipUserFromSiteUser(siteUser);
                }  
            }

            return null;

        }


        public override string GetUserNameByEmail(string email)
        {
            SiteSettings siteSettings = GetSiteSettings();
            if ((siteSettings != null)&&(email != null)&&(email.Length >  5))
            {
                return SiteUser.GetUserNameFromEmail(siteSettings.SiteId, email);
            }
            return String.Empty;
        }

        public override string ResetPassword(string userName, string passwordAnswer)
        {
            /*
            Takes, as input, a user name and a password answer and replaces the user's current password 
             * with a new, random password. ResetPassword then returns the new password. A 
             * convenient mechanism for generating a random password is the 
             * Membership.GeneratePassword method. If the user name is not valid, ResetPassword 
             * throws a ProviderException. ResetPassword also checks the value of the 
             * RequiresQuestionAndAnswer property before resetting a password. If 
             * RequiresQuestionAndAnswer is true, ResetPassword compares the supplied password 
             * answer to the stored password answer and throws a MembershipPasswordException if 
             * the two don't match. Before resetting a password, ResetPassword verifies that 
             * EnablePasswordReset is true. If EnablePasswordReset is false, ResetPassword throws 
             * a NotSupportedException. If the user whose password is being changed is currently 
             * locked out, ResetPassword throws a MembershipPasswordException. Before resetting a 
             * password, ResetPassword calls the provider's virtual OnValidatingPassword method to 
             * validate the new password. It then resets the password or cancels the action based on 
             * the outcome of the call. If the new password is invalid, ResetPassword throws a 
             * ProviderException. Following a successful password reset, ResetPassword updates the 
             * user's LastPasswordChangedDate.
            */
            SiteSettings siteSettings = GetSiteSettings();

            if (!siteSettings.AllowPasswordReset)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            String newPassword = null;

            if ((userName != null) && (siteSettings != null))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                if (siteUser.UserId > -1)
                {
                    
                    if (siteUser.IsLockedOut)
                    {
                        throw new MembershipPasswordException(
                            ResourceHelper.GetMessageTemplate("UserAccountLockedMessage.config"));
                    }

                    bool okToResetPassword = false;
                    if (siteSettings.RequiresQuestionAndAnswer)
                    {
                        if ((passwordAnswer != null) && (passwordAnswer == siteUser.PasswordAnswer))
                        {
                            okToResetPassword = true;
                        }
                        else
                        {
                            // if wrong answer or user is locked out
                            throw new MembershipPasswordException(ResourceHelper.GetMessageTemplate("PasswordWrongAnswerToQuestionMessage.config"));
                        }

                    }
                    else
                    {
                        okToResetPassword = true;
                    }

                    if (okToResetPassword)
                    {
                       
                        newPassword = SiteUser.CreateRandomPassword(siteSettings.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);

                        switch (PasswordFormat)
                        {
                            case MembershipPasswordFormat.Clear:
                                siteUser.Password = newPassword;
                                break;
                            default:
                                siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                                siteUser.Password = EncodePassword(siteUser.PasswordSalt + newPassword, PasswordFormat);
                                break;
                        }
                        
                        siteUser.MustChangePwd = siteSettings.RequirePasswordChangeOnResetRecover;
                        siteUser.PasswordFormat = siteSettings.PasswordFormat;
                        siteUser.Save();

                        siteUser.UpdateLastPasswordChangeTime();

                    }
                }
                else
                {
                    throw new ProviderException(ResourceHelper.GetMessageTemplate("UserNotFoundMessage.config"));

                }

            }

            return newPassword;

        }

        

        public override bool UnlockUser(string userName)
        {
            /*
             Unlocks (that is, restores login privileges for) the specified user. UnlockUser returns true if the 
             * user is successfully unlocked. Otherwise, it returns false. If the user is already unlocked, 
             * UnlockUser simply returns true.
             */
            SiteSettings siteSettings = GetSiteSettings();

            bool result = false;
            if ((siteSettings != null) && (userName != null) && (userName.Length > 0))
            {
                SiteUser siteUser = new SiteUser(siteSettings, userName);
                if (siteUser.UserId > 0)
                {
                    if (!siteUser.IsLockedOut)
                    {
                        result = true;
                    }
                    else
                    {
                        result = siteUser.UnlockAccount();
                    }

                }

            }

            return result;

        }

        public override void UpdateUser(MembershipUser user)
        {
            /*
            Takes, as input, a MembershipUser object representing a registered user and updates the 
             * information stored for 
             that user in the membership data source. If any of the input submitted in the MembershipUser 
             * object is not valid, 
             UpdateUser throws a ProviderException. Note that UpdateUser is not obligated to allow all 
             * the data that 
             can be encapsulated in a MembershipUser object to be updated in the data source.
             
             */
            SiteSettings siteSettings = GetSiteSettings();

            if((siteSettings != null)&&(user != null))
            {
                SiteUser siteUser;
                if (siteSettings.UseEmailForLogin)
                {
                    siteUser = new SiteUser(siteSettings, user.Email);
                }
                else
                {
                    siteUser = new SiteUser(siteSettings, user.UserName);
                }
                if (siteUser.UserId > 0)
                {
                    siteUser.Comment = user.Comment;
                    siteUser.Email = user.Email;
                    if (!siteSettings.UseEmailForLogin)
                    {
                        siteUser.LoginName = user.UserName;
                    }
                    siteUser.ProfileApproved = user.IsApproved;
                    
                    if (
                        (user.PasswordQuestion != null)
                        &&(user.PasswordQuestion.Length > 0)
                        &&(user.PasswordQuestion != siteUser.PasswordQuestion)
                        )
                    {
                        siteUser.PasswordQuestion = user.PasswordQuestion;
                       
                    }
                    
                    siteUser.Save();
                    

                    if (user.LastActivityDate > siteUser.LastActivityDate)
                    {
                        siteUser.UpdateLastActivityTime();
                    }
                    
                }

            }
        }

        public override bool ValidateUser(string userName, string password)
        {
            /*
             Takes, as input, a user name and a password and verifies that they are valid-that is, that 
             * the membership 
             * data source contains a matching user name and password. ValidateUser returns true if the 
             * user name and 
             * password are valid, if the user is approved (that is, if MembershipUser.IsApproved is true), 
             * and if the user 
             * isn't currently locked out. Otherwise, it returns false. Following a successful validation, 
             * ValidateUser updates 
             * the user's LastLoginDate and fires an AuditMembershipAuthenticationSuccess Web event. 
             * Following a failed validation, 
             * it fires an AuditMembershipAuthenticationFailure Web event.
          
             */
       
            SiteSettings siteSettings = GetSiteSettings();

            if (siteSettings == null) { return false; }

            

            if (string.IsNullOrEmpty(userName)) { return false; }
            if (string.IsNullOrEmpty(password)) { return false; }

            bool result = false;

           

            if (
                (siteSettings.UseEmailForLogin)
                && (userName.Length > EmailMaxlength)
                )
            {
                return result;
            }

            if (
                (!siteSettings.UseEmailForLogin)
                && (userName.Length > LoginnameMaxlength)
                )
            {
                return result;
            }

            //previous implementation

            //SiteUser siteUser = null;
            //string encPassword = EncodePassword(password, siteSettings);

            //string user;
            //if (!siteSettings.UseLdapAuth || (siteSettings.UseLdapAuth && WebConfigSettings.UseLDAPFallbackAuthentication))
            //{
            //    user = SiteUser.Login(siteSettings, userName, encPassword);
            //    if ((user != null) && (user != String.Empty))
            //    {
            //        result = true;
            //        siteUser = new SiteUser(siteSettings, userName);
            //    }
            //    else if (!siteSettings.UseLdapAuth)
            //    {
            //        // need to create the user here so we can increment the failed password attmpt count below
            //        siteUser = new SiteUser(siteSettings, userName);
            //    }
            //}
            //if (siteSettings.UseLdapAuth && siteUser == null)
            //{
            //    user = SiteUser.LoginLDAP(siteSettings, userName, password, out siteUser);
            //    if ((user != null) && (user != String.Empty))
            //    {
            //        result = true;
            //        if (siteUser != null)
            //        {
            //            //we just auto created a user who was validated against LDAP, but did not exist as a site user
            //            NewsletterHelper.ClaimExistingSubscriptions(siteUser);
            //            UserRegisteredEventArgs u = new UserRegisteredEventArgs(siteUser);
            //            OnUserRegistered(u);
            //        }
            //        else
            //        {
            //            siteUser = new SiteUser(siteSettings, userName);
            //        }
            //    }
            //}

            SiteUser siteUser = GetSiteUser(siteSettings, userName);

            if ((siteUser != null) && (siteUser.IsLockedOut) && (WebConfigSettings.ReturnFalseInValidateUserIfAccountLocked))
            {
                return false;
            }

            if ((siteUser != null) && (siteUser.IsDeleted) && (WebConfigSettings.ReturnFalseInValidateUserIfAccountDeleted))
            {
                return false;
            }

            if (siteSettings.UseLdapAuth)
            {
                SiteUser createdUser = null;
                string user = SiteUser.LoginLDAP(siteSettings, userName, password, out createdUser);
                if (!(string.IsNullOrEmpty(user)))
                {
                    result = true;
                    if (createdUser != null)
                    {
                        //we just auto created a user who was validated against LDAP, but did not exist as a site user
                        siteUser = createdUser;
                        // lets make sure to use the right password encoding, the auto creation assigned a random one but did not encode it
                        siteUser.Password = EncodePassword(siteSettings, siteUser, siteUser.Password);
                        siteUser.Save();
                        NewsletterHelper.ClaimExistingSubscriptions(siteUser);
                        UserRegisteredEventArgs u = new UserRegisteredEventArgs(siteUser);
                        OnUserRegistered(u);
                    }
                    //else
                    //{
                    //    siteUser = new SiteUser(siteSettings, userName);
                    //}
                }
                else if((siteSettings.AllowDbFallbackWithLdap)&&(siteUser != null))
                {
                    // ldap auth failed but we did find a matching user in the db
                    // and we are allowing db users in addition to ldap
                    // so validate the db way
                    result = PasswordIsValid(siteSettings, siteUser, password);
                }

            }
            else
            {
                result = PasswordIsValid(siteSettings, siteUser, password);
            }
           

            if (result)
            {
                siteUser.UpdateLastLoginTime();

                //PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_SUCCESS);

                // this raises an error for some reason just by raisng the event
                // maybe because there is no handler for it

                //mojoWebAuthenticationSuccessAuditEvent webSuccess
                //    = new mojoWebAuthenticationSuccessAuditEvent(
                //            null,
                //            this,
                //            WebEventCodes.AuditMembershipAuthenticationSuccess,
                //            userName);

                //webSuccess.Raise();

            }
            else
            {

                if (
                    (siteSettings.MaxInvalidPasswordAttempts > 0)
                    && (siteUser != null)
                    && (siteUser.UserGuid != Guid.Empty))
                {
                    siteUser.IncrementPasswordAttempts(siteSettings);

                }

                if (WebConfigSettings.LogFailedLoginAttempts)
                {
                    log.Info("failed login attempt for user " + userName);
                }

                //PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_FAIL);

                //mojoWebAuthenticationFailureAuditEvent webEvent
                //    = new mojoWebAuthenticationFailureAuditEvent(
                //            null,
                //            this,
                //            WebEventCodes.AuditMembershipAuthenticationFailure,
                //            userName);

                //webEvent.Raise();

            }

            return result;
        }

        private SiteUser GetSiteUser(SiteSettings siteSettings, string login)
        {
            SiteUser siteUser = new SiteUser(siteSettings, login);

            if ((siteUser.UserGuid != Guid.Empty) && (siteUser.SiteId == siteSettings.SiteId))
            {
                return siteUser;
            }

            return null;
        }

        private void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }
        }

        private bool PasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
        {
            if (siteUser == null) { return false; }
            if (string.IsNullOrEmpty(providedPassword)) { return false; }

            bool isValid = false;
            bool didUpdatePassword = false;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    isValid = ClearTextPasswordIsValid(siteSettings, siteUser, providedPassword);
                    break;

                case MembershipPasswordFormat.Encrypted:

                    isValid = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

                    // this is to support older installations from before we used salt
                    if ((isValid) && (siteUser.PasswordSalt.Length == 0))
                    {   // user is valid but he doesn't have a salt
                        // generate a random salt and update the siteuser password to encrypted with salt
                        siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                        byte[] bIn = Encoding.Unicode.GetBytes(siteUser.PasswordSalt + providedPassword);
                        byte[] bRet = EncryptPassword(bIn);
                        siteUser.Password = Convert.ToBase64String(bRet);
                        siteUser.Save();

                    }

                    break;

                case MembershipPasswordFormat.Hashed:

                    isValid = HashedSha512PasswordIsValid(siteSettings, siteUser, providedPassword);

                    if ((!isValid) && (WebConfigSettings.CheckMD5PasswordHashAsFallback))
                    {
                        // previously we were using md5 so we need to check against that
                        // and if valid re-hash it with sha512
                        isValid = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);

                        if (isValid)
                        {
                            // update user to sha512 hash with random salt
                            // then set didUpdatePassword to true so we don't do it again below
                            siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                            siteUser.Password = GetSHA512Hash(siteUser.PasswordSalt + providedPassword);
                            siteUser.Save();
                            didUpdatePassword = true;

                        }

                    }

                    // this is to support older installations from before we used salt
                    if (
                        (isValid)
                        &&(!didUpdatePassword)
                        &&(siteUser.PasswordSalt.Length == 0)
                        )
                    {
                        // generate a random salt and update the siteuser password to encrypted with salt
                        siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                        siteUser.Password = GetSHA512Hash(siteUser.PasswordSalt + providedPassword);
                        siteUser.Save();

                    }


                    break;

            }

            if ((!isValid) && (WebConfigSettings.CheckAllPasswordFormatsOnAuthFailure)) 
            {
                // CheckAllPasswordFormatsOnAuthFailure is false by default so this code will not execute unless you change 
                // it to true by adding it to web.config or user.config 
                // <add key="CheckAllPasswordFormatsOnAuthFailure" value="true" />

                // Its purpose if true is to rescue a site
                // from a failed password format conversion. Consider what might happen if changing password formats does not 
                // complete on all users. We queue it onto a background thread but if there are a very large number of rows
                // it is possible that the app may be recycled before it completes if someone touches web.config for example
                // or if memory limits on the app pool are reached, it could leave the database in a state where some users 
                // are in the new password format and some in the old format and therefore cannot login
                // so this is a safety valve that can be enabled to fallback and check other formats and if
                // the user can be validated with another format then update him to the current format

                bool isValidByAlternateFormat = false;

                switch (PasswordFormat)
                {
                    case MembershipPasswordFormat.Clear:

                        isValidByAlternateFormat = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

                        if(!isValidByAlternateFormat)
                        {
                            isValidByAlternateFormat = HashedSha512PasswordIsValid(siteSettings, siteUser, providedPassword);

                            if((!isValidByAlternateFormat)&&(WebConfigSettings.CheckMD5PasswordHashAsFallback))
                            {
                                isValidByAlternateFormat = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);
                            }
                        }

                        if (isValidByAlternateFormat)
                        {
                            //current format is clear but user validated with another format so we need to update him to clear
                            siteUser.PasswordSalt = string.Empty;
                            siteUser.Password = providedPassword;
                            siteUser.Save();
                            isValid = true;
                        }

                        break;

                    case MembershipPasswordFormat.Encrypted:

                        isValidByAlternateFormat = ClearTextPasswordIsValid(siteSettings, siteUser, providedPassword);

                        if (!isValidByAlternateFormat)
                        {
                            isValidByAlternateFormat = HashedSha512PasswordIsValid(siteSettings, siteUser, providedPassword);

                            if ((!isValidByAlternateFormat) && (WebConfigSettings.CheckMD5PasswordHashAsFallback))
                            {
                                isValidByAlternateFormat = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);
                            }
                        }

                        if (isValidByAlternateFormat)
                        {
                            //current format is encrypted but user was validated with another format so we need to encrypt his password
                            siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                            siteUser.Password = EncodePassword(siteUser.PasswordSalt + providedPassword, MembershipPasswordFormat.Encrypted);
                            siteUser.Save();
                            isValid = true;
                        }

                        break;


                    case MembershipPasswordFormat.Hashed:

                        isValidByAlternateFormat = ClearTextPasswordIsValid(siteSettings, siteUser, providedPassword);

                        if (!isValidByAlternateFormat)
                        {
                            isValidByAlternateFormat = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

                            if ((!isValidByAlternateFormat) && (WebConfigSettings.CheckMD5PasswordHashAsFallback))
                            {
                                isValidByAlternateFormat = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);
                            }
                        }

                        if (isValidByAlternateFormat)
                        {
                            //current format is hashed but user was validated with another format so we need to hash his password
                            siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                            siteUser.Password = EncodePassword(siteUser.PasswordSalt + providedPassword, MembershipPasswordFormat.Hashed);
                            siteUser.Save();
                            isValid = true;
                        }

                        break;
                }

            }


            return isValid;
        }

        private bool ClearTextPasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
        {
            if (providedPassword == siteUser.Password) { return true; }

            return false;
        }

        private bool EncryptedPasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
        {
            byte[] bIn = Encoding.Unicode.GetBytes(siteUser.PasswordSalt + providedPassword);
            byte[] bRet = EncryptPassword(bIn);

            string encryptedPassword = Convert.ToBase64String(bRet);

            if (encryptedPassword == siteUser.Password) { return true; }

            return false;
        }

        private bool HashedSha512PasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
        {
            string sha512Hash = GetSHA512Hash(siteUser.PasswordSalt + providedPassword);

            if (sha512Hash == siteUser.Password) {  return true; }

            return false;
        }

        // legacy support for upgrades
        private bool HashedMd5PasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
        {
            string md5Hash = GetMD5Hash(siteUser.PasswordSalt + providedPassword);

            if (md5Hash == siteUser.Password) { return true; }

            return false;
        }

        /// <summary>
        /// If you call this method from custom code, you need to concatenate passwordsalt + password before passing it into this method
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="site"></param>
        /// <returns>encoded password</returns>
        public string EncodePassword(string pass, SiteSettings site)
        {
           // if (site.UseLdapAuth) { return pass; }
            return EncodePassword(pass, (MembershipPasswordFormat) site.PasswordFormat);
        }

        public string EncodePassword(SiteSettings site, SiteUser siteUser, string pass)
        {
            MembershipPasswordFormat passwordFormat = (MembershipPasswordFormat)site.PasswordFormat;

            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    //do nothing
                    break;

                default:
                    siteUser.PasswordSalt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    pass = siteUser.PasswordSalt + pass;
                    break;
            }

            return EncodePassword(pass, passwordFormat);
        }

        
        public string EncodePassword(string pass, MembershipPasswordFormat passwordFormat)
        {
            if (passwordFormat == MembershipPasswordFormat.Clear)
            {
                return pass;
            }

            if (passwordFormat == MembershipPasswordFormat.Hashed)
            {
                return GetSHA512Hash(pass);
                
            }

            // else encrypted
            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bRet = EncryptPassword(bIn);

            return Convert.ToBase64String(bRet);
        }


        public string UnencodePassword(string pass, MembershipPasswordFormat passwordFormat)
        {
            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    return pass;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Can't decrypt hashed password");
                default:
                    byte[] bIn = Convert.FromBase64String(pass);
                    byte[] bRet = DecryptPassword(bIn);
                    
                    if (bRet == null)
                        return null;

                    return Encoding.Unicode.GetString(bRet);
            }
        }



        private string GetMD5Hash(string cleanText)
        {
            if(string.IsNullOrEmpty(cleanText)){ return string.Empty;}

            using (MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider())
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);
                Byte[] hashedBytes = hasher.ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);

            }

        }

        private string GetSHA512Hash(string cleanText)
        {
            if (string.IsNullOrEmpty(cleanText)) { return string.Empty; }

            using (SHA512CryptoServiceProvider hasher = new SHA512CryptoServiceProvider())
            {
                Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanText);
                Byte[] hashedBytes = hasher.ComputeHash(clearBytes);

                return BitConverter.ToString(hashedBytes);

            }

        }

        

        #endregion

        #region Protected Methods

        




        //protected override byte[] DecryptPassword(byte[] encodedPassword)
        //{
        //    /*
        //     * Takes, as input, a byte array containing an encrypted password and returns a byte array 
        //     * containing the password in plaintext form. The default implementation in MembershipProvider 
        //     * decrypts the password using <machineKey>'s decryptionKey, but throws an exception if the 
        //     * decryption key is autogenerated. Override only if you want to customize the decryption process. 
        //     * Do not call the base class's DecryptPassword method if you override this method.
        //     */

        //    return base.DecryptPassword(encodedPassword);

       
        //}

        //protected override byte[] EncryptPassword(byte[] password)
        //{
        //    /*
        //     * 	Takes, as input, a byte array containing a plaintext password and returns a byte array containing the 
        //     * password in encrypted form. The default implementation in MembershipProvider encrypts the password 
        //     * using <machineKey>'s decryptionKey, but throws an exception if the decryption key is autogenerated. 
        //     * Override only if you want to customize the encryption process. Do not call the base class's EncryptPassword 
        //     * method if you override this method.
        //     */
        //    return base.EncryptPassword(password);
        //}

        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            /*
             * Virtual method called when a password is created. The default implementation in MembershipProvider 
             * fires a ValidatingPassword event, so be sure to call the base class's OnValidatingPassword method if 
             * you override this method. The ValidatingPassword event allows applications to apply additional tests to 
             * passwords by registering event handlers. A custom provider's CreateUser, ChangePassword, and ResetPassword 
             * methods (in short, all methods that record new passwords) should call this method.
             */

            base.OnValidatingPassword(e);
        }



        #endregion


        public void ChangeUserPasswordFormat(SiteSettings siteSettings, int oldPasswordFormat)
        {
            /*
             * 
             Cleartext change to encrypted - encrypt plain passwords for exisitng
             Cleartext change to hashed - hash passwords for exisiting users
             Encrypted changed to cleartext - decrypt passwords
             Encrypted change to hashed - decrypt then hash passwords
             Hashed to cleartext - replace password with random password
             Hashed to encrypted - replace passwords with random passwords then encrypt them
             */

            switch (oldPasswordFormat)
            {
                case (int)MembershipPasswordFormat.Clear:

                    switch (siteSettings.PasswordFormat)
                    {
                        case (int)MembershipPasswordFormat.Encrypted:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromClearTextPasswordsToEncrypted), siteSettings);

                            break;

                        case (int)MembershipPasswordFormat.Hashed:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromClearTextPasswordsToHashed), siteSettings);

                            break;

                    }

                    break;

                case (int)MembershipPasswordFormat.Encrypted:

                    switch (siteSettings.PasswordFormat)
                    {
                        case (int)MembershipPasswordFormat.Clear:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromEncryptedPasswordsToClearText), siteSettings);

                            break;

                        case (int)MembershipPasswordFormat.Hashed:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromEncryptedPasswordsToHashed), siteSettings);

                            break;

                    }

                    break;


                case (int)MembershipPasswordFormat.Hashed:

                    switch (siteSettings.PasswordFormat)
                    {
                        case (int)MembershipPasswordFormat.Encrypted:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromHashedPasswordsToEncrypted), siteSettings);

                            break;

                        case (int)MembershipPasswordFormat.Clear:
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ChangeFromHashedPasswordsToClearText), siteSettings);

                            break;

                    }

                    break;


            }

        }


        private void ChangeFromClearTextPasswordsToEncrypted(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);
            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string oldPassword = row["Pwd"].ToString();
                    string salt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    string password = EncodePassword(salt + oldPassword, MembershipPasswordFormat.Encrypted);

                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Encrypted, password, salt);

                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }
            }
        }


        private void ChangeFromClearTextPasswordsToHashed(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);
            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string salt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    string password = GetSHA512Hash(salt + row["Pwd"].ToString());

                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Hashed, password, salt);

                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }
            }
        }


        private void ChangeFromEncryptedPasswordsToClearText(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);
            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string oldPassword = row["Pwd"].ToString();
                    string salt = row["PasswordSalt"].ToString();
                    string clearPassword;
                    if (salt.Length > 0)
                    {
                        clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted).Replace(salt, string.Empty);
                    }
                    else
                    {
                        clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted);
                    }

                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Clear, clearPassword, string.Empty);
           
                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }
            }
        }


        private void ChangeFromEncryptedPasswordsToHashed(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);
            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string oldPassword = row["Pwd"].ToString();
                    string salt = row["PasswordSalt"].ToString();
                    string clearPassword;
                    if (salt.Length > 0)
                    {
                        clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted).Replace(salt, string.Empty);
                    }
                    else
                    {
                        clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted);
                    }

                    salt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    string hashedPassword = GetSHA512Hash(salt + clearPassword);

                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Hashed, hashedPassword, salt);

                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }
            }
        }


        private void ChangeFromHashedPasswordsToClearText(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            //Hashed to cleartext - replace password with random password
            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);
            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string newPassword = SiteUser.CreateRandomPassword(site.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);
                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Clear, newPassword, string.Empty);

                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }
            }

        }


        private void ChangeFromHashedPasswordsToEncrypted(Object objSiteSettings)
        {
            SiteSettings site = objSiteSettings as SiteSettings;
            if (site == null) return;

            //Hashed to encrypted - replace passwords with random passwords then encrypt them
            DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

            foreach (DataRow row in dtUsers.Rows)
            {
                try
                {
                    int userId = Convert.ToInt32(row["UserID"]);
                    string newPassword = SiteUser.CreateRandomPassword(site.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);
                    string salt = SiteUser.CreateRandomPassword(128, WebConfigSettings.PasswordGeneratorChars);
                    string password = EncodePassword(salt + newPassword, MembershipPasswordFormat.Encrypted);

                    SiteUser.UpdatePasswordAndSalt(userId, (int)MembershipPasswordFormat.Encrypted, password, salt);

                }
                catch (Exception ex)
                {
                    // I don't like catching a general exception here but since this gets queued 
                    //on a different thread best to log anything that goes wrong
                    log.Error("ChangeFromClearTextPasswordsToEncrypted", ex);
                }

            }
        }
    }
}
