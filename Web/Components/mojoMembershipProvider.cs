using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace mojoPortal.Web
{
	public class mojoMembershipProvider : MembershipProvider
	{
		#region Private Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(mojoMembershipProvider));

		private string description = string.Empty;
		private string name = string.Empty;
		private string applicationName = string.Empty;

		private const int LoginnameMaxlength = 50;
		private const int EmailMaxlength = 100;
		private const int PasswordquestionMaxlength = 255;
		private const int PasswordanswerMaxlength = 255;

		protected bool saltFirst = true;
		protected int saltLength = 128;

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

				return siteSettings != null ? siteSettings.SiteName : applicationName;
			}
			set { applicationName = value; }
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

				return siteSettings != null ? siteSettings.AllowPasswordReset : false;
			}
		}

		public override bool EnablePasswordRetrieval
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.AllowPasswordRetrieval : false;
			}
		}

		public override int MaxInvalidPasswordAttempts
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.MaxInvalidPasswordAttempts : 5;
			}
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.MinRequiredNonAlphanumericCharacters : 0;
			}
		}

		public override int MinRequiredPasswordLength
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.MinRequiredPasswordLength : 4;
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

				return siteSettings != null ? siteSettings.PasswordAttemptWindowMinutes : 5;
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

				return siteSettings != null ? siteSettings.PasswordStrengthRegularExpression : string.Empty;
			}
		}

		public override bool RequiresQuestionAndAnswer
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.RequiresQuestionAndAnswer : true;
			}
		}

		public override bool RequiresUniqueEmail
		{
			get
			{
				SiteSettings siteSettings = GetSiteSettings();

				return siteSettings != null ? siteSettings.RequiresUniqueEmail : true;
			}
		}

		#endregion


		#region Private Methods

		private MembershipUser CreateMembershipUserFromSiteUser(SiteUser siteUser)
		{
			SiteSettings siteSettings = GetSiteSettings();

			if (siteUser == null || siteUser.UserId <= 0)
			{
				return null;
			}

			if (siteSettings.UseEmailForLogin)
			{
				return new MembershipUser(
					name,
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
					siteUser.LastLockoutDate
				);
			}
			else
			{
				return new MembershipUser(
					name,
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
					siteUser.LastLockoutDate
				);
			}
		}

		#endregion


		#region Public Methods

		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
			this.name = nameof(mojoMembershipProvider);
			description = "mojoPortal Membership Provider";
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
				username == null ||
				username == string.Empty ||
				oldPassword == null ||
				oldPassword == string.Empty ||
				newPassword == null ||
				newPassword == string.Empty
			)
			{
				return result;
			}

			SiteSettings siteSettings = GetSiteSettings();
			if (siteSettings == null)
			{ return result; }

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
					throw new ArgumentException(ResourceHelper.GetMessageTemplate("PasswordDoesntMatchRegularExpressionMessage.config"));
				}
			}

			var e = new ValidatePasswordEventArgs(username, newPassword, false);

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

			var siteUser = new SiteUser(siteSettings, username);

			if (siteUser.UserId == -1)
			{
				return result;
			}

			if (
				(MembershipPasswordFormat)siteSettings.PasswordFormat == MembershipPasswordFormat.Hashed &&
				!siteSettings.UseLdapAuth
			)
			{
				if (siteUser.Password == EncodePassword(oldPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed))
				{
					siteUser.PasswordSalt = CreateSaltKey();
					siteUser.Password = EncodePassword(newPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);
					siteUser.MustChangePwd = false;
					siteUser.PasswordFormat = siteSettings.PasswordFormat;

					result = siteUser.Save();
				}
			}
			else if ((MembershipPasswordFormat)siteSettings.PasswordFormat == MembershipPasswordFormat.Encrypted)
			{
				if (siteUser.Password == EncodePassword(oldPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Encrypted))
				{
					siteUser.PasswordSalt = CreateSaltKey();
					siteUser.Password = EncodePassword(newPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Encrypted);
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
			string newPasswordAnswer
		)
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
				string.IsNullOrEmpty(userName) ||
				password == null ||
				string.IsNullOrEmpty(newPasswordQuestion) ||
				string.IsNullOrEmpty(newPasswordAnswer) ||
				newPasswordQuestion.Length > PasswordquestionMaxlength ||
				newPasswordAnswer.Length > PasswordanswerMaxlength
			)
			{
				return false;
			}

			SiteSettings siteSettings = GetSiteSettings();

			if (siteSettings == null)
			{
				return false;
			}

			var siteUser = new SiteUser(siteSettings, userName);

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
			out MembershipCreateStatus status
		)
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


			if (string.IsNullOrEmpty(userName) || userName.Length > LoginnameMaxlength)
			{
				status = MembershipCreateStatus.InvalidUserName;

				return null;
			}

			if (string.IsNullOrEmpty(email) || email.Length > EmailMaxlength)
			{
				status = MembershipCreateStatus.InvalidEmail;

				return null;
			}

			if (string.IsNullOrEmpty(password))
			{
				status = MembershipCreateStatus.InvalidPassword;

				return null;
			}

			if (siteSettings.RequiresQuestionAndAnswer)
			{
				if (
					string.IsNullOrEmpty(passwordQuestion) ||
					passwordQuestion.Length > PasswordquestionMaxlength
				)
				{
					status = MembershipCreateStatus.InvalidQuestion;

					return null;
				}

				if (
					string.IsNullOrEmpty(passwordAnswer) ||
					passwordAnswer.Length > PasswordanswerMaxlength
				)
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

					if (existingUser != null && !existingUser.IsDeleted)
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

			var e = new ValidatePasswordEventArgs(userName, password, true);

			OnValidatingPassword(e);

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
				siteUser.PasswordSalt = CreateSaltKey();
				password = EncodePassword(password, siteUser.PasswordSalt, PasswordFormat);
			}

			siteUser.Password = password;
			siteUser.ApprovedForLogin = !siteSettings.RequireApprovalBeforeLogin;
			siteUser.PasswordFormat = siteSettings.PasswordFormat;

			var created = siteUser.Save();

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
					var registerConfirmGuid = Guid.NewGuid();
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
			if (HttpContext.Current != null)
			{
				var returnUrlParam = HttpContext.Current.Request.Params.Get("returnurl");

				if (!string.IsNullOrEmpty(returnUrlParam))
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

			var result = false;
			SiteSettings siteSettings = GetSiteSettings();

			// we are ignoring deleteAllRelatedData
			// on purpose because whether to really delete or just flag as deleted
			// is determined by the siteSettings.ReallyDeleteUsers setting
			if (userName != null && siteSettings != null)
			{
				var siteUser = new SiteUser(siteSettings, userName);

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
			out int totalRecords
		)
		{
			/*
			 * Returns a MembershipUserCollection containing MembershipUser objects representing 
			 * users whose e-mail addresses match the emailToMatch input parameter. Wildcard syntax 
			 * is data source-dependent. MembershipUser objects in the MembershipUserCollection are 
			 * sorted by e-mail address. If FindUsersByEmail finds no matching users, it returns an empty 
			 * MembershipUserCollection.
			 */
			SiteSettings siteSettings = GetSiteSettings();
			var users = new MembershipUserCollection();

			totalRecords = 0;

			using (IDataReader reader = SiteUser.GetUserByEmail(siteSettings.SiteId, emailToMatch))
			{
				while (reader.Read())
				{
					var user = new MembershipUser(
						name,
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
						Convert.ToDateTime(reader["LastLockoutDate"], CultureInfo.InvariantCulture)
					);

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
			out int totalRecords
		)
		{
			SiteSettings siteSettings = GetSiteSettings();
			var users = new MembershipUserCollection();

			totalRecords = 0;

			using (IDataReader reader = SiteUser.GetUserByLoginName(
				siteSettings.SiteId,
				usernameToMatch,
				siteSettings.UseLdapAuth &&
				siteSettings.AllowDbFallbackWithLdap &&
				siteSettings.AllowEmailLoginWithLdapDbFallback
			))
			{
				while (reader.Read())
				{
					var user = new MembershipUser(
						name,
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
						Convert.ToDateTime(reader["LastLockoutDate"], CultureInfo.InvariantCulture)
					);

					users.Add(user);
					totalRecords += 1;
				}
			}

			return users;
		}


		public override MembershipUserCollection GetAllUsers(
			int pageIndex,
			int pageSize,
			out int totalRecords
		)
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
			var users = new MembershipUserCollection();

			totalRecords = 0;

			if (siteSettings != null)
			{
				var siteUserPage = SiteUser.GetPage(
					siteSettings.SiteId,
					pageIndex,
					pageSize,
					string.Empty,
					0,
					"display",
					out _
				);

				foreach (SiteUser siteUser in siteUserPage)
				{
					var user = new MembershipUser(
						name,
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
						siteUser.LastLockoutDate
					);

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

			var siteSettings = GetSiteSettings();

			if (!siteSettings.AllowPasswordRetrieval)
			{
				throw new MojoMembershipException(ResourceHelper.GetMessageTemplate("PasswordRetrievalNotEnabledMessage.config"));
			}


			if (userName != null && siteSettings != null)
			{
				var siteUser = new SiteUser(siteSettings, userName);

				if (siteUser.UserId > -1)
				{
					if (siteUser.IsLockedOut)
					{
						throw new MembershipPasswordException(ResourceHelper.GetMessageTemplate("UserAccountLockedMessage.config"));
					}

					if (siteUser.IsDeleted)
					{
						throw new MembershipPasswordException(ResourceHelper.GetMessageTemplate("UserNotFoundMessage.config"));
					}

					bool okToGetPassword = false;
					if (siteSettings.RequiresQuestionAndAnswer)
					{
						if (passwordAnswer != null && PasswordAnswerIsMatch(passwordAnswer, siteUser.PasswordAnswer))
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

					if (okToGetPassword)
					{
						if (siteSettings.RequirePasswordChangeOnResetRecover)
						{
							siteUser.MustChangePwd = true;

							siteUser.Save();
						}

						switch (PasswordFormat)
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

								var newPassword = SiteUser.CreateRandomPassword(siteSettings.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);

								siteUser.PasswordSalt = CreateSaltKey();
								siteUser.Password = EncodePassword(newPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);
								siteUser.PasswordFormat = siteSettings.PasswordFormat;
								//after the new random password is emailed to the user we can force him to change it again immediately after he logs in
								siteUser.MustChangePwd = siteSettings.RequirePasswordChangeOnResetRecover;
								// needed if we are sending a link for automatic login and force to change password instead of sending the random one by email
								// will be cleared to Guid.Empty when password is changed
								siteUser.PasswordResetGuid = Guid.NewGuid();

								siteUser.Save();

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

			return suppliedAnswer == actualAnswer;
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

			if (siteSettings != null && providerUserKey != null)
			{
				SiteUser siteUser = null;

				if (providerUserKey is Guid userGuid)
				{
					siteUser = new SiteUser(siteSettings, userGuid);
				}

				if (providerUserKey is int userId)
				{
					siteUser = new SiteUser(siteSettings, userId);
				}

				if (siteUser?.UserId > 0)
				{
					if (siteUser.IsDeleted)
					{
						return null;
					}

					if (userIsOnline)
					{
						siteUser.UpdateLastActivityTime();
					}

					return CreateMembershipUserFromSiteUser(siteUser);
				}
			}

			return null;
		}


		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			/*
			 * Takes, as input, a user name or user ID (the method is overloaded) and a 
			 * Boolean value indicating whether to update the user's LastActivityDate to 
			 * show that the user is currently online. GetUser returns a MembershipUser object 
			 * representing the specified user. If the user name or user ID is invalid (that is, if 
			 * it doesn't represent a registered user) GetUser returns null (Nothing in Visual Basic).
			 */
			SiteSettings siteSettings = GetSiteSettings();

			if (
				siteSettings != null &&
				username != null &&
				username.Length > 0
			)
			{
				var siteUser = new SiteUser(siteSettings, username);

				if (siteUser.UserId > 0)
				{
					if (siteUser.IsDeleted)
					{
						return null;
					}

					if (userIsOnline)
					{
						siteUser.UpdateLastActivityTime();
					}

					return CreateMembershipUserFromSiteUser(siteUser);
				}
			}

			return null;
		}


		public override string GetUserNameByEmail(string email)
		{
			SiteSettings siteSettings = GetSiteSettings();

			return siteSettings != null && email != null && email.Length > 5
				? SiteUser.GetUserNameFromEmail(siteSettings.SiteId, email)
				: string.Empty;
		}


		public override string ResetPassword(string userName, string passwordAnswer)
		{
			/*
			 * Takes, as input, a user name and a password answer and replaces the user's current password 
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

			string newPassword = null;

			if (userName != null && siteSettings != null)
			{
				var siteUser = new SiteUser(siteSettings, userName);

				if (siteUser.UserId > -1)
				{
					if (siteUser.IsLockedOut)
					{
						throw new MembershipPasswordException(ResourceHelper.GetMessageTemplate("UserAccountLockedMessage.config"));
					}

					bool okToResetPassword;

					if (siteSettings.RequiresQuestionAndAnswer)
					{
						if (passwordAnswer != null && passwordAnswer == siteUser.PasswordAnswer)
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
								siteUser.PasswordSalt = CreateSaltKey();
								siteUser.Password = EncodePassword(newPassword, siteUser.PasswordSalt, PasswordFormat);

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

			var result = false;

			if (siteSettings != null && userName != null && userName.Length > 0)
			{
				var siteUser = new SiteUser(siteSettings, userName);

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

			if (siteSettings != null && user != null)
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
						user.PasswordQuestion != null &&
						user.PasswordQuestion.Length > 0 &&
						user.PasswordQuestion != siteUser.PasswordQuestion
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

			if (siteSettings == null)
			{
				return false;
			}

			if (string.IsNullOrEmpty(userName))
			{
				return false;
			}

			if (string.IsNullOrEmpty(password))
			{
				return false;
			}

			var result = false;

			if (siteSettings.UseEmailForLogin && userName.Length > EmailMaxlength)
			{
				return result;
			}

			if (!siteSettings.UseEmailForLogin && userName.Length > LoginnameMaxlength)
			{
				return result;
			}

			SiteUser siteUser = GetSiteUser(siteSettings, userName);

			if (siteUser != null && siteUser.IsLockedOut && WebConfigSettings.ReturnFalseInValidateUserIfAccountLocked)
			{
				return false;
			}

			if (siteUser != null && siteUser.IsDeleted && WebConfigSettings.ReturnFalseInValidateUserIfAccountDeleted)
			{
				return false;
			}

			if (siteSettings.UseLdapAuth)
			{
				var user = SiteUser.LoginLDAP(siteSettings, userName, password, out SiteUser createdUser);

				if (!string.IsNullOrEmpty(user))
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
				}
				else if (siteSettings.AllowDbFallbackWithLdap && siteUser != null)
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
			}
			else
			{
				if (
					siteSettings.MaxInvalidPasswordAttempts > 0 &&
					siteUser != null &&
					siteUser.UserGuid != Guid.Empty
				)
				{
					siteUser.IncrementPasswordAttempts(siteSettings);
				}

				if (WebConfigSettings.LogFailedLoginAttempts)
				{
					log.Info("failed login attempt for user " + userName);
				}
			}

			return result;
		}


		private SiteUser GetSiteUser(SiteSettings siteSettings, string login)
		{
			var siteUser = new SiteUser(siteSettings, login);

			if (siteUser.UserGuid != Guid.Empty && siteUser.SiteId == siteSettings.SiteId)
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
			if (siteUser == null)
			{
				return false;
			}

			if (string.IsNullOrEmpty(providedPassword))
			{
				return false;
			}

			var isValid = false;
			var didUpdatePassword = false;

			switch (PasswordFormat)
			{
				case MembershipPasswordFormat.Clear:
					isValid = ClearTextPasswordIsValid(siteSettings, siteUser, providedPassword);

					break;

				case MembershipPasswordFormat.Encrypted:
					isValid = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

					// this is to support older installations from before we used salt
					if (isValid && siteUser.PasswordSalt.Length == 0)
					{
						// user is valid but he doesn't have a salt
						// generate a random salt and update the siteuser password to encrypted with salt
						siteUser.PasswordSalt = CreateSaltKey();
						siteUser.Password = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Encrypted);

						siteUser.Save();
					}

					break;

				case MembershipPasswordFormat.Hashed:

					isValid = HashedSha512PasswordIsValid(siteSettings, siteUser, providedPassword);

					if (!isValid && WebConfigSettings.CheckMD5PasswordHashAsFallback)
					{
						// previously we were using md5 so we need to check against that
						// and if valid re-hash it with sha512
						isValid = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);

						if (isValid)
						{
							// update user to sha512 hash with random salt
							// then set didUpdatePassword to true so we don't do it again below
							siteUser.PasswordSalt = CreateSaltKey();
							siteUser.Password = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);

							siteUser.Save();

							didUpdatePassword = true;
						}
					}

					// this is to support older installations from before we used salt
					if (
						isValid &&
						!didUpdatePassword &&
						siteUser.PasswordSalt.Length == 0
					)
					{
						// generate a random salt and update the siteuser password to encrypted with salt
						siteUser.PasswordSalt = CreateSaltKey();
						siteUser.Password = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);

						siteUser.Save();
					}

					break;
			}

			if (!isValid && WebConfigSettings.CheckAllPasswordFormatsOnAuthFailure)
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

				bool isValidByAlternateFormat;

				switch (PasswordFormat)
				{
					case MembershipPasswordFormat.Clear:

						isValidByAlternateFormat = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

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

							if (!isValidByAlternateFormat && WebConfigSettings.CheckMD5PasswordHashAsFallback)
							{
								isValidByAlternateFormat = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);
							}
						}

						if (isValidByAlternateFormat)
						{
							//current format is encrypted but user was validated with another format so we need to encrypt his password
							siteUser.PasswordSalt = CreateSaltKey();
							siteUser.Password = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Encrypted);

							siteUser.Save();

							isValid = true;
						}

						break;


					case MembershipPasswordFormat.Hashed:

						isValidByAlternateFormat = ClearTextPasswordIsValid(siteSettings, siteUser, providedPassword);

						if (!isValidByAlternateFormat)
						{
							isValidByAlternateFormat = EncryptedPasswordIsValid(siteSettings, siteUser, providedPassword);

							if (!isValidByAlternateFormat && WebConfigSettings.CheckMD5PasswordHashAsFallback)
							{
								isValidByAlternateFormat = HashedMd5PasswordIsValid(siteSettings, siteUser, providedPassword);
							}
						}

						if (isValidByAlternateFormat)
						{
							//current format is hashed but user was validated with another format so we need to hash his password
							siteUser.PasswordSalt = CreateSaltKey();
							siteUser.Password = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);

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
			return providedPassword == siteUser.Password;
		}


		private bool EncryptedPasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
		{
			var encryptedPassword = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Encrypted);

			return encryptedPassword == siteUser.Password;
		}


		private bool HashedSha512PasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
		{
			var sha512Hash = EncodePassword(providedPassword, siteUser.PasswordSalt, MembershipPasswordFormat.Hashed);

			return sha512Hash == siteUser.Password;
		}


		// legacy support for upgrades
		private bool HashedMd5PasswordIsValid(SiteSettings siteSettings, SiteUser siteUser, string providedPassword)
		{
			string md5Hash = GetMD5Hash(siteUser.PasswordSalt + providedPassword);

			return md5Hash == siteUser.Password;
		}


		public virtual string EncodePassword(string password, string saltKey = "", SiteSettings siteSettings = null)
		{
			return EncodePassword(password, saltKey, (MembershipPasswordFormat)siteSettings.PasswordFormat);
		}


		public virtual string EncodePassword(SiteSettings site, SiteUser siteUser, string password)
		{
			MembershipPasswordFormat passwordFormat = (MembershipPasswordFormat)site.PasswordFormat;

			if (passwordFormat != MembershipPasswordFormat.Clear)
			{
				siteUser.PasswordSalt = CreateSaltKey();
			}

			return EncodePassword(password, siteUser.PasswordSalt, passwordFormat);
		}


		public virtual string EncodePassword(string password, string saltKey = "", MembershipPasswordFormat passwordFormat = MembershipPasswordFormat.Clear)
		{
			var saltedPassword = saltFirst ? string.Concat(saltKey, password) : string.Concat(password, saltKey);

			switch (passwordFormat)
			{
				default:
				case MembershipPasswordFormat.Clear:
					return saltedPassword;

				case MembershipPasswordFormat.Hashed:
					return CreatePasswordHash(saltedPassword);

				case MembershipPasswordFormat.Encrypted:
					return EncryptPassword(saltedPassword);
			}
		}


		public virtual string UnencodePassword(string password, MembershipPasswordFormat passwordFormat)
		{
			switch (passwordFormat)
			{
				default:
				case MembershipPasswordFormat.Clear:
					return password;

				case MembershipPasswordFormat.Hashed:
					throw new ProviderException("Can't decrypt hashed password");

				case MembershipPasswordFormat.Encrypted:
					return DecryptPassword(password);
			}
		}


		protected virtual string EncryptPassword(string password)
		{
			var bIn = Encoding.Unicode.GetBytes(password);
			var bRet = EncryptPassword(bIn); // Inherited method uses web.config machine key settings

			return Convert.ToBase64String(bRet);
		}


		protected virtual string DecryptPassword(string password)
		{
			var bIn = Convert.FromBase64String(password);
			var bRet = DecryptPassword(bIn); // Inherited method uses web.config machine key settings

			return bRet == null ? null : Encoding.Unicode.GetString(bRet);
		}


		private string GetMD5Hash(string cleanText)
		{
			if (string.IsNullOrEmpty(cleanText))
			{
				return string.Empty;
			}

			using (var hasher = new MD5CryptoServiceProvider())
			{
				var clearBytes = new UnicodeEncoding().GetBytes(cleanText);
				var hashedBytes = hasher.ComputeHash(clearBytes);

				return BitConverter.ToString(hashedBytes);
			}
		}


		protected virtual string CreateSaltKey()
		{
			return SiteUser.CreateRandomPassword(saltLength, WebConfigSettings.PasswordGeneratorChars);
		}


		protected virtual string CreatePasswordHash(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}

			using (var hasher = new SHA512CryptoServiceProvider())
			{
				var clearBytes = new UnicodeEncoding().GetBytes(text);
				var hashedBytes = hasher.ComputeHash(clearBytes);

				return BitConverter.ToString(hashedBytes);
			}
		}



		#endregion


		#region Protected Methods

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


		private void ChangeFromClearTextPasswordsToEncrypted(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var oldPassword = row["Pwd"].ToString();
					var salt = CreateSaltKey();
					var password = EncodePassword(oldPassword, salt, MembershipPasswordFormat.Encrypted);

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


		private void ChangeFromClearTextPasswordsToHashed(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var salt = CreateSaltKey();
					var password = EncodePassword(row["Pwd"].ToString(), salt, MembershipPasswordFormat.Hashed);

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


		private void ChangeFromEncryptedPasswordsToClearText(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var oldPassword = row["Pwd"].ToString();
					var salt = row["PasswordSalt"].ToString();
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


		private void ChangeFromEncryptedPasswordsToHashed(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var oldPassword = row["Pwd"].ToString();
					var salt = row["PasswordSalt"].ToString();
					string clearPassword;

					if (salt.Length > 0)
					{
						clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted).Replace(salt, string.Empty);
					}
					else
					{
						clearPassword = UnencodePassword(oldPassword, MembershipPasswordFormat.Encrypted);
					}

					salt = CreateSaltKey();
					var hashedPassword = EncodePassword(clearPassword, salt, MembershipPasswordFormat.Hashed);

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


		private void ChangeFromHashedPasswordsToClearText(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			//Hashed to cleartext - replace password with random password
			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var newPassword = SiteUser.CreateRandomPassword(site.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);

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


		private void ChangeFromHashedPasswordsToEncrypted(object objSiteSettings)
		{
			if (!(objSiteSettings is SiteSettings site))
			{
				return;
			}

			//Hashed to encrypted - replace passwords with random passwords then encrypt them
			DataTable dtUsers = SiteUser.GetUserListForPasswordFormatChange(site.SiteId);

			foreach (DataRow row in dtUsers.Rows)
			{
				try
				{
					var userId = Convert.ToInt32(row["UserID"]);
					var newPassword = SiteUser.CreateRandomPassword(site.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars);
					var salt = CreateSaltKey();
					var password = EncodePassword(newPassword, salt, MembershipPasswordFormat.Encrypted);

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


		#region Obsolete Methods

		/// <summary>
		/// If you call this method from custom code, you need to concatenate passwordsalt + password before passing it into this method
		/// </summary>
		/// <param name="password">The Password to be encoded.</param>
		/// <param name="siteSettings"></param>
		/// <returns>Return an encoded password using the site's password format settings.</returns>
		[Obsolete("Use EncodePassword(string password, string saltKey, SiteSettings siteSettings) instead.", true)]
		public string EncodePassword(string password, SiteSettings siteSettings)
		{
			return EncodePassword(password, (MembershipPasswordFormat)siteSettings.PasswordFormat);
		}


		/// <summary>
		/// If you call this method from custom code, you need to concatenate passwordsalt + password before passing it into this method
		/// </summary>
		/// <param name="password">The Password to be encoded.</param>
		/// <param name="passwordFormat"></param>
		/// <returns>Return an encoded password using specified password format.</returns>
		[Obsolete("Use EncodePassword(string password, string saltKey, MembershipPasswordFormat passwordFormat) instead.", true)]
		public string EncodePassword(string password, MembershipPasswordFormat passwordFormat)
		{
			if (passwordFormat == MembershipPasswordFormat.Clear)
			{
				return password;
			}

			if (passwordFormat == MembershipPasswordFormat.Hashed)
			{
				return CreatePasswordHash(password);
			}

			return EncryptPassword(password);
		}

		#endregion
	}
}
