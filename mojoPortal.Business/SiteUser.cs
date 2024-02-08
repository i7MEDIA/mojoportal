using log4net;
using mojoPortal.Business.Properties;
using mojoPortal.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace mojoPortal.Business;

[Serializable]
public class SiteUser
{
	#region Constructors

	public SiteUser()
	{ }


	public SiteUser(SiteSettings settings)
	{
		if (settings != null)
		{
			siteSettings = settings;
			SiteId = siteSettings.SiteId;
			SiteGuid = siteSettings.SiteGuid;
			timeZoneId = siteSettings.TimeZoneId;
			passwordFormat = siteSettings.PasswordFormat;

			if (UseRelatedSiteMode)
			{
				SiteId = RelatedSiteID;
			}
		}
	}


	public SiteUser(SiteSettings settings, bool overridRelatedSiteMode)
	{
		if (settings != null)
		{
			siteSettings = settings;
			SiteId = siteSettings.SiteId;
			SiteGuid = siteSettings.SiteGuid;
			timeZoneId = siteSettings.TimeZoneId;

			if (UseRelatedSiteMode && !overridRelatedSiteMode)
			{
				SiteId = RelatedSiteID;
			}
		}
	}


	public SiteUser(SiteSettings settings, int userId)
	{
		if (settings != null)
		{
			siteSettings = settings;
			SiteGuid = siteSettings.SiteGuid;
			SiteId = siteSettings.SiteId;
			timeZoneId = siteSettings.TimeZoneId;

			if (UseRelatedSiteMode)
			{
				SiteId = RelatedSiteID;
			}

			GetUser(userId);
		}
	}


	public SiteUser(SiteSettings settings, string login)
	{
		if (settings != null)
		{
			siteSettings = settings;
			SiteGuid = siteSettings.SiteGuid;
			SiteId = siteSettings.SiteId;
			timeZoneId = siteSettings.TimeZoneId;

			if (UseRelatedSiteMode)
			{
				SiteId = RelatedSiteID;
			}

			GetUser(login);
		}

	}


	public SiteUser(SiteSettings settings, Guid userGuid)
	{
		if (settings != null)
		{
			siteSettings = settings;
			SiteId = siteSettings.SiteId;
			timeZoneId = siteSettings.TimeZoneId;

			if (UseRelatedSiteMode)
			{
				SiteId = RelatedSiteID;
			}

			GetUser(userGuid);
		}
	}

	#endregion


	#region Private Properties

	private static readonly ILog log = LogManager.GetLogger(typeof(SiteUser));

	[NonSerialized]
	private readonly SiteSettings siteSettings;
	private DateTime dateCreated = DateTime.UtcNow;
	private bool isDeleted = false;

	private string passwordQuestion = string.Empty;
	private string passwordAnswer = string.Empty;
	private DateTime lastActivityDate = DateTime.MinValue;
	private DateTime lastLoginDate = DateTime.MinValue;
	private DateTime lastPasswordChangedDate = DateTime.MinValue;
	private DateTime lastLockoutDate = DateTime.MinValue;
	private int failedPasswordAttemptCount;
	private DateTime failedPasswordAttemptWindowStart = DateTime.MinValue;
	private int failedPasswordAnswerAttemptCount;
	private DateTime failedPasswordAnswerAttemptWindowStart = DateTime.MinValue;
	private bool isLockedOut = false;
	private string mobilePIN = string.Empty;
	private string passwordSalt = string.Empty;
	private string comment = string.Empty;
	private Guid registerConfirmGuid = Guid.Empty;

	private bool nonLazyLoadedProfilePropertiesLoaded = false;
	private DataTable profileProperties = null;
	private ArrayList userRoles = new ArrayList();
	private bool rolesLoaded = false;

	private string openIDURI = string.Empty;
	private string windowsLiveID = string.Empty;
	private decimal totalRevenue = 0;
	private bool mustChangePwd = false;

	private string timeZoneId = "Central Standard Time"; //default
	private string newEmail = string.Empty;
	private Guid emailChangeGuid = Guid.Empty;
	private string editorPreference = string.Empty; // use site default

	private Guid passwordResetGuid = Guid.Empty;
	private bool rolesChanged = false;

	private string authorBio = string.Empty;
	private bool allowPasswordChange = true;
	private bool readonlyUserProfile = false;

	private string passwordHash = string.Empty; // used for asp.net identity pwd field is still used for membership

	/// <summary>
	/// since aspnet identity doesn't directly support salt
	/// we have to store the hash, the salt and the format in a single field
	/// so our custom hasher will have it
	/// http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
	/// https://aspnetidentity.codeplex.com/workitem/2333
	/// </summary>
	public string PasswordHash
	{
		get
		{
			if (passwordHash.Length == 0)
			{
				return $"{Password}|{passwordSalt}|{passwordFormat}";
			}

			return passwordHash;
		}
		set { passwordHash = value; }
	}

	// 0 = plain text, 1 = hashed, 2 = encrypted
	private int passwordFormat = 0;
	public int PasswordFormat
	{
		get { return passwordFormat; }
		set { passwordFormat = value; }
	}

	private string securityStamp = string.Empty;
	public string SecurityStamp
	{
		get { return securityStamp; }
		set { securityStamp = value; }
	}

	private bool emailConfirmed = false;
	public bool EmailConfirmed
	{
		get { return emailConfirmed; }
		set { emailConfirmed = value; }
	}

	private string phoneNumber = string.Empty;
	public string PhoneNumber
	{
		get { return phoneNumber; }
		set { phoneNumber = value; }
	}

	private bool phoneNumberConfirmed = false;
	public bool PhoneNumberConfirmed
	{
		get { return phoneNumberConfirmed; }
		set { phoneNumberConfirmed = value; }
	}

	private bool twoFactorEnabled = false;
	public bool TwoFactorEnabled
	{
		get { return twoFactorEnabled; }
		set { twoFactorEnabled = value; }
	}

	private DateTime? lockoutEndDateUtc = null;
	public DateTime? LockoutEndDateUtc
	{
		get { return lockoutEndDateUtc; }
		set { lockoutEndDateUtc = value; }
	}

	private DateTime dateOfBirth = DateTime.MinValue;
	public DateTime DateOfBirth
	{
		get { return dateOfBirth; }
		set { dateOfBirth = value; }
	}

	private static bool UseRelatedSiteMode
	{
		get
		{
			if (
				ConfigurationManager.AppSettings["UseRelatedSiteMode"] != null &&
				ConfigurationManager.AppSettings["UseRelatedSiteMode"] == "true"
			)
			{
				return true;
			}

			return false;
		}
	}

	private static int RelatedSiteID
	{
		get
		{
			int result = 1;

			if (ConfigurationManager.AppSettings["RelatedSiteID"] != null)
			{
				int.TryParse(ConfigurationManager.AppSettings["RelatedSiteID"], out result);
			}

			return result;
		}
	}

	#endregion


	#region Public Properties

	public int UserId { get; set; } = -1;

	public decimal TotalRevenue
	{
		get { return totalRevenue; }
	}

	public Guid SiteGuid { get; set; } = Guid.Empty;

	public int SiteId { get; set; } = -1;

	public bool IsDeleted
	{
		get { return isDeleted; }
	}

	public Guid UserGuid { get; set; } = Guid.Empty;

	public Guid RegisterConfirmGuid
	{
		get { return registerConfirmGuid; }
	}

	public string Name { get; set; } = string.Empty;

	public string LoginName { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public string AuthorBio
	{
		get { return authorBio; }
		set { authorBio = value; }
	}

	public string NewEmail
	{
		get { return newEmail; }
		set { newEmail = value; }
	}

	public string EditorPreference
	{
		get { return editorPreference; }
		set { editorPreference = value; }
	}

	public Guid EmailChangeGuid
	{
		get { return emailChangeGuid; }
		set { emailChangeGuid = value; }
	}

	public string Password { get; set; } = string.Empty;

	public bool MustChangePwd
	{
		get { return mustChangePwd; }
		set { mustChangePwd = value; }
	}

	public bool RolesChanged
	{
		get { return rolesChanged; }
		set { rolesChanged = value; }
	}

	public string Gender { get; set; } = string.Empty;

	public bool ProfileApproved { get; set; } = true;

	// 2010-12-19 changed public name from ApprovedForForums
	// the property was previously intended for forums but was never used there
	// the field name in the db will remain the same but the purpose of this property 
	// is now for requiring approval of newly registered users before they can login
	// if that is required by siteSettings
	public bool ApprovedForLogin { get; set; } = true;
	public bool Trusted { get; set; } = false;
	public bool DisplayInMemberList { get; set; } = true;
	public string WebSiteUrl { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public string Occupation { get; set; } = string.Empty;
	public string Interests { get; set; } = string.Empty;
	public string MSN { get; set; } = string.Empty;
	public string Yahoo { get; set; } = string.Empty;
	public string AIM { get; set; } = string.Empty;
	public string ICQ { get; set; } = string.Empty;
	public int TotalPosts { get; set; } = 0;
	public string AvatarUrl { get; set; } = string.Empty;

	public string TimeZoneId
	{
		get { return timeZoneId; }
		set { timeZoneId = value; }
	}

	/*
	 * Originally implemented TimeOffsetHours as int in db
	 * but need double so using ad hoc property
	 * 
	 */
	private double timeOffsetHours = 0;
	public double TimeOffsetHours
	{
		get
		{
			timeOffsetHours = Convert.ToDouble(GetProperty("TimeOffsetHours", SettingsSerializeAs.String), CultureInfo.InvariantCulture);

			return timeOffsetHours;
		}
		set
		{
			timeOffsetHours = value;
			SetProperty("TimeOffsetHours", value, SettingsSerializeAs.String);
		}
	}

	public bool EnableLiveMessengerOnProfile
	{
		get
		{
			object o = GetProperty("EnableLiveMessengerOnProfile", SettingsSerializeAs.String);

			if (o != null)
			{
				return Convert.ToBoolean(o, CultureInfo.InvariantCulture);
			}

			return false;
		}
		set
		{
			SetProperty("EnableLiveMessengerOnProfile", value, SettingsSerializeAs.String);
		}
	}

	public string LiveMessengerId
	{
		get
		{
			object o = GetProperty("LiveMessengerId", SettingsSerializeAs.String);

			if (o != null)
			{
				return o.ToString();
			}

			return string.Empty;
		}
		set
		{
			SetProperty("LiveMessengerId", value, SettingsSerializeAs.String);
		}
	}

	public string LiveMessengerDelegationToken
	{
		get
		{
			object o = GetProperty("LiveMessengerDelegationToken", SettingsSerializeAs.String);

			if (o != null)
			{
				return o.ToString();
			}

			return string.Empty;
		}
		set
		{
			SetProperty("LiveMessengerDelegationToken", value, SettingsSerializeAs.String);
		}
	}

	public string Signature { get; set; } = string.Empty;

	public DateTime DateCreated
	{
		get { return dateCreated; }
	}

	public string Skin { get; set; } = string.Empty;
	public string LoweredEmail { get; private set; } = string.Empty;

	public string PasswordQuestion
	{
		get { return passwordQuestion; }
		set { passwordQuestion = value; }
	}

	public string PasswordAnswer
	{
		get { return passwordAnswer; }
		set { passwordAnswer = value; }
	}

	public DateTime LastActivityDate
	{
		get { return lastActivityDate; }
	}

	public DateTime LastLoginDate
	{
		get { return lastLoginDate; }
	}

	public DateTime LastPasswordChangedDate
	{
		get { return lastPasswordChangedDate; }
	}

	public DateTime LastLockoutDate
	{
		get { return lastLockoutDate; }
	}

	public int FailedPasswordAttemptCount
	{
		get { return failedPasswordAttemptCount; }
	}

	public DateTime FailedPasswordAttemptWindowStart
	{
		get { return failedPasswordAttemptWindowStart; }
	}

	public int FailedPasswordAnswerAttemptCount
	{
		get { return failedPasswordAnswerAttemptCount; }
	}

	public DateTime FailedPasswordAnswerAttemptWindowStart
	{
		get { return failedPasswordAnswerAttemptWindowStart; }
	}

	public bool IsLockedOut
	{
		get { return isLockedOut; }
	}

	public string MobilePin
	{
		get { return mobilePIN; }
		set { mobilePIN = value; }
	}

	public string PasswordSalt
	{
		get { return passwordSalt; }
		set { passwordSalt = value; }
	}

	public string Comment
	{
		get { return comment; }
		set { comment = value; }
	}

	public string OpenIdUri
	{
		get { return openIDURI; }
		set { openIDURI = value; }
	}

	public string WindowsLiveId
	{
		get { return windowsLiveID; }
		set { windowsLiveID = value; }
	}

	public Guid PasswordResetGuid
	{
		get { return passwordResetGuid; }
		set { passwordResetGuid = value; }
	}

	#endregion


	#region Private Methods

	private void GetUser(string loginInfo)
	{
		IDataReader reader;

		if (
			siteSettings.UseEmailForLogin &&
			!siteSettings.UseLdapAuth &&
			loginInfo.Contains("@")
		)
		{
			using (reader = DBSiteUser.GetSingleUser(SiteId, loginInfo))
			{
				GetUser(reader);
			}
		}
		else
		{
			using (
				reader = DBSiteUser.GetSingleUserByLoginName(
					SiteId,
					loginInfo,
					siteSettings.UseLdapAuth &&
					siteSettings.AllowDbFallbackWithLdap &&
					siteSettings.AllowEmailLoginWithLdapDbFallback
				)
			)
			{
				GetUser(reader);
			}
		}
	}


	private void GetUser(int userId)
	{
		using IDataReader reader = DBSiteUser.GetSingleUser(userId, SiteId);

		GetUser(reader);
	}


	private void GetUser(Guid userGuid)
	{
		using IDataReader reader = DBSiteUser.GetSingleUser(userGuid, SiteId);

		GetUser(reader);
	}

	private void GetUser(IDataReader reader)
	{
		if (reader.Read())
		{
			UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
			SiteId = Convert.ToInt32(reader["SiteID"], CultureInfo.InvariantCulture);

			try
			{
				totalRevenue = Convert.ToDecimal(reader["TotalRevenue"], CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{ }

			Name = reader["Name"].ToString();
			LoginName = reader["LoginName"].ToString();
			Email = reader["Email"].ToString();
			Password = reader["Pwd"].ToString();
			Gender = reader["Gender"].ToString().Trim();

			if (reader["ProfileApproved"] != DBNull.Value)
			{
				ProfileApproved = Convert.ToBoolean(reader["ProfileApproved"]);
			}

			if (reader["ApprovedForForums"] != DBNull.Value)
			{
				ApprovedForLogin = Convert.ToBoolean(reader["ApprovedForForums"]);
			}

			if (reader["Trusted"] != DBNull.Value)
			{
				Trusted = Convert.ToBoolean(reader["Trusted"]);
			}

			if (reader["DisplayInMemberList"] != DBNull.Value)
			{
				DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
			}

			WebSiteUrl = reader["WebSiteURL"].ToString();
			Country = reader["Country"].ToString();
			State = reader["State"].ToString();
			Occupation = reader["Occupation"].ToString();
			Interests = reader["Interests"].ToString();
			MSN = reader["MSN"].ToString();
			Yahoo = reader["Yahoo"].ToString();
			AIM = reader["AIM"].ToString();
			ICQ = reader["ICQ"].ToString();
			TotalPosts = Convert.ToInt32(reader["TotalPosts"], CultureInfo.InvariantCulture);
			AvatarUrl = reader["AvatarUrl"].ToString();

			timeOffsetHours = Convert.ToInt32(reader["TimeOffsetHours"], CultureInfo.InvariantCulture);
			Signature = reader["Signature"].ToString();

			try
			{
				dateCreated = Convert.ToDateTime(reader["DateCreated"]);
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("DateCreated was invalid", ex);
				}
			}

			try
			{
				UserGuid = new Guid(reader["UserGuid"].ToString());
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("UserGuid was invalid", ex);
				}
			}

			Skin = reader["Skin"].ToString();

			if (reader["IsDeleted"] != DBNull.Value)
			{
				isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
			}

			if (reader["IsLockedOut"] != DBNull.Value)
			{
				isLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
			}

			LoweredEmail = reader["LoweredEmail"].ToString();
			passwordQuestion = reader["PasswordQuestion"].ToString();
			passwordAnswer = reader["PasswordAnswer"].ToString();

			try
			{
				if (reader["LastActivityDate"] != DBNull.Value)
				{
					lastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("LastActivityDate was invalid", ex);
				}
			}

			try
			{
				if (reader["LastLoginDate"] != DBNull.Value)
				{
					lastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("LastLoginDate was invalid", ex);
				}
			}

			try
			{
				if (reader["LastPasswordChangedDate"] != DBNull.Value)
				{
					lastPasswordChangedDate = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("LastPasswordChangedDate was invalid", ex);
				}
			}

			try
			{
				if (reader["LastLockoutDate"] != DBNull.Value)
				{
					lastLockoutDate = Convert.ToDateTime(reader["LastLockoutDate"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("LastLockoutDate was invalid", ex);
				}
			}

			try
			{
				if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
				{
					failedPasswordAttemptCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"], CultureInfo.InvariantCulture);
				}
			}
			catch (InvalidCastException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("FailedPasswordAttemptCount was invalid", ex);
				}
			}

			try
			{
				if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
				{
					failedPasswordAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAttemptWindowStart"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("FailedPwdAttemptWindowStart was invalid", ex);
				}
			}

			try
			{
				if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
				{
					failedPasswordAnswerAttemptCount = Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"], CultureInfo.InvariantCulture);
				}
			}
			catch (InvalidCastException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("FailedPwdAnswerAttemptCount was invalid", ex);
				}
			}

			try
			{
				if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
				{
					failedPasswordAnswerAttemptWindowStart = Convert.ToDateTime(reader["FailedPwdAnswerWindowStart"]);
				}
			}
			catch (FormatException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("FailedPwdAnswerWindowStart was invalid", ex);
				}
			}

			mobilePIN = reader["MobilePIN"].ToString();
			passwordSalt = reader["PasswordSalt"].ToString();
			comment = reader["Comment"].ToString();

			var confirmGuidString = reader["RegisterConfirmGuid"].ToString();

			if (confirmGuidString.Length == 36)
			{
				registerConfirmGuid = new Guid(confirmGuidString);
			}

			openIDURI = reader["OpenIDURI"].ToString();
			windowsLiveID = reader["WindowsLiveID"].ToString();
			SiteGuid = new Guid(reader["SiteGuid"].ToString());
			mustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);

			emailChangeGuid = new Guid(reader["EmailChangeGuid"].ToString());
			newEmail = reader["NewEmail"].ToString();
			timeZoneId = reader["TimeZoneId"].ToString();
			editorPreference = reader["EditorPreference"].ToString();
			FirstName = reader["FirstName"].ToString();
			LastName = reader["LastName"].ToString();

			passwordResetGuid = new Guid(reader["PasswordResetGuid"].ToString());

			rolesChanged = Convert.ToBoolean(reader["RolesChanged"]);

			authorBio = reader["AuthorBio"].ToString();

			if (reader["DateOfBirth"] != DBNull.Value)
			{
				dateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
			}

			emailConfirmed = Convert.ToBoolean(reader["EmailConfirmed"]);
			passwordFormat = Convert.ToInt32(reader["PwdFormat"], CultureInfo.InvariantCulture);
			passwordHash = reader["PasswordHash"].ToString();
			securityStamp = reader["SecurityStamp"].ToString();
			phoneNumber = reader["PhoneNumber"].ToString();
			phoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
			twoFactorEnabled = Convert.ToBoolean(reader["TwoFactorEnabled"]);

			if (reader["LockoutEndDateUtc"] != DBNull.Value)
			{
				lockoutEndDateUtc = Convert.ToDateTime(reader["LockoutEndDateUtc"]);
			}
		}
	}


	private bool Create()
	{
		UserGuid = Guid.NewGuid();
		dateCreated = DateTime.UtcNow;

		var newID = DBSiteUser.AddUser(
			SiteGuid,
			SiteId,
			Name,
			LoginName,
			Email,
			Password,
			passwordSalt,
			UserGuid,
			dateCreated,
			mustChangePwd,
			FirstName,
			LastName,
			timeZoneId,
			dateOfBirth,
			emailConfirmed,
			passwordFormat,
			passwordHash,
			securityStamp,
			phoneNumber,
			phoneNumberConfirmed,
			twoFactorEnabled,
			lockoutEndDateUtc
		);

		var userCreated = newID > 0;

		if (userCreated)
		{
			UserId = newID;
			dateCreated = DateTime.Now;
			lastPasswordChangedDate = DateTime.Now;

			// not all properties are added on insert so update
			Update();

			Role.AddUserToDefaultRoles(this);
		}

		return userCreated;
	}


	private bool Update()
	{
		// timeOffset is now a double and stored in profile system
		var legacyTimeOffset = 0;

		var userUpdated = DBSiteUser.UpdateUser(
			UserId,
			Name,
			LoginName,
			Email,
			Password,
			passwordSalt,
			Gender,
			ProfileApproved,
			ApprovedForLogin,
			Trusted,
			DisplayInMemberList,
			WebSiteUrl,
			Country,
			State,
			Occupation,
			Interests,
			MSN,
			Yahoo,
			AIM,
			ICQ,
			AvatarUrl,
			Signature,
			Skin,
			Email.ToLower(),
			passwordQuestion,
			passwordAnswer,
			comment,
			legacyTimeOffset,
			openIDURI,
			windowsLiveID,
			mustChangePwd,
			FirstName,
			LastName,
			timeZoneId,
			editorPreference,
			newEmail,
			emailChangeGuid,
			passwordResetGuid,
			rolesChanged,
			authorBio,
			dateOfBirth,
			emailConfirmed,
			passwordFormat,
			passwordHash,
			securityStamp,
			phoneNumber,
			phoneNumberConfirmed,
			twoFactorEnabled,
			lockoutEndDateUtc
		);

		return userUpdated;
	}


	private void LoadRoles()
	{
		using IDataReader reader = GetRolesByUser(SiteId, UserId);

		while (reader.Read())
		{
			userRoles.Add(reader["RoleName"].ToString().Trim());
		}

		rolesLoaded = true;
	}

	#endregion


	#region Public Methods

	public bool Save()
	{
		if (UserId > -1)
		{
			return Update();
		}
		else
		{
			return Create();
		}
	}


	public bool IsValid()
	{
		return GetRuleViolations().Count() == 0;
	}


	public IEnumerable<RuleViolation> GetRuleViolations()
	{
		//this is not used yet but gives a model for validation when/if we ever implement MVC

		if (string.IsNullOrEmpty(LoginName))
		{
			yield return new RuleViolation(Resources.SiteUserLoginRequired);
		}

		if (string.IsNullOrEmpty(Email))
		{
			yield return new RuleViolation(Resources.SiteUserEmailIsRequired);
		}

		if (!RegexHelper.IsValidEmailAddressSyntax(Email))
		{
			yield return new RuleViolation(Resources.SiteUserInvalidEmailFormat);
		}

		yield break;
	}


	public bool DeleteUser()
	{
		if (siteSettings.ReallyDeleteUsers)
		{
			var subscriptions = new SubscriberRepository();

			List<LetterSubscriber> subs = subscriptions.GetListByUser(siteSettings.SiteGuid, UserGuid);

			foreach (LetterSubscriber subscription in subs)
			{
				subscriptions.Delete(subscription);
				LetterInfo.UpdateSubscriberCount(subscription.LetterInfoGuid);
			}

			UserLocation.DeleteByUser(UserGuid);
			UserPage.DeleteByUser(UserGuid);
			DBSiteUser.DeletePropertiesByUser(UserGuid);

			var result = DBSiteUser.DeleteUser(UserId);

			if (result)
			{
				Role.DeleteUserRoles(UserId);
			}

			return result;
		}
		else
		{
			return DBSiteUser.FlagAsDeleted(UserId);
		}
	}

	public bool UndeleteUser()
	{
		return DBSiteUser.FlagAsNotDeleted(UserId);
	}

	public bool UpdateLastActivityTime()
	{
		return DBSiteUser.UpdateLastActivityTime(UserGuid, DateTime.UtcNow);
	}

	public bool UpdateLastLoginTime()
	{
		return DBSiteUser.UpdateLastLoginTime(UserGuid, DateTime.UtcNow);
	}

	public bool LockoutAccount()
	{
		return DBSiteUser.AccountLockout(UserGuid, DateTime.UtcNow);
	}

	public bool UnlockAccount()
	{
		return DBSiteUser.AccountClearLockout(UserGuid);
	}

	public bool UpdatePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
	{
		return DBSiteUser.UpdatePasswordQuestionAndAnswer(UserGuid, newQuestion, newAnswer);
	}

	public bool UpdateLastPasswordChangeTime()
	{
		return DBSiteUser.UpdateLastPasswordChangeTime(UserGuid, DateTime.UtcNow);
	}

	public void IncrementPasswordAttempts(SiteSettings siteSettings)
	{
		if (siteSettings == null)
		{
			return;
		}

		if (
			failedPasswordAttemptCount == 0 ||
			failedPasswordAttemptWindowStart.AddMinutes(siteSettings.PasswordAttemptWindowMinutes) < DateTime.UtcNow
		)
		{
			failedPasswordAttemptWindowStart = DateTime.UtcNow;
			DBSiteUser.UpdateFailedPasswordAttemptStartWindow(UserGuid, failedPasswordAttemptWindowStart);
			failedPasswordAttemptCount = 1;
		}
		else
		{
			failedPasswordAttemptCount += 1;
		}

		DBSiteUser.UpdateFailedPasswordAttemptCount(UserGuid, failedPasswordAttemptCount);
	}


	public void IncrementPasswordAnswerAttempts(SiteSettings siteSettings)
	{
		if (siteSettings != null)
		{
			if (
				failedPasswordAnswerAttemptCount == 0 ||
				FailedPasswordAnswerAttemptWindowStart.AddMinutes(siteSettings.PasswordAttemptWindowMinutes) < DateTime.UtcNow
			)
			{
				failedPasswordAnswerAttemptWindowStart = DateTime.UtcNow;

				DBSiteUser.UpdateFailedPasswordAnswerAttemptStartWindow(UserGuid, failedPasswordAnswerAttemptWindowStart);

				failedPasswordAnswerAttemptCount = 1;
			}
			else
			{
				failedPasswordAnswerAttemptCount += 1;
			}

			DBSiteUser.UpdateFailedPasswordAnswerAttemptCount(UserGuid, failedPasswordAnswerAttemptCount);
		}
	}


	public bool SetRegistrationConfirmationGuid(Guid registrationGuid)
	{
		if (registrationGuid == Guid.Empty)
		{
			// empty guid indicates already confirmed registration
			// static ConfirmRegistration is the only method that should be allowed to
			// this method is locking the account in addition to setting the registration
			// Guid. If the correct guid is passed on the ConfirmRegistration.aspx 
			// it will unlock the account. best not to lock an account with an empty guid
			// because known guids can easily be entered in the url

			return false;
		}

		registerConfirmGuid = registrationGuid;

		return DBSiteUser.SetRegistrationConfirmationGuid(UserGuid, registrationGuid);
	}


	public bool IsInRoles(string semicolonSeparatedRoleNames)
	{
		if (string.IsNullOrEmpty(semicolonSeparatedRoleNames))
		{
			return false;
		}

		if (!rolesLoaded)
		{
			LoadRoles();
		}

		foreach (string roleToCheck in semicolonSeparatedRoleNames.Split([';']))
		{
			foreach (string userRole in userRoles)
			{
				int compareResult = string.Compare(roleToCheck.Trim(), userRole, true, CultureInfo.InvariantCulture);

				if (compareResult == 0)
				{
					return true;
				}
			}
		}

		return false;
	}


	#region mojoProfile methods

	public void SetProperty(string propertyName, string propertyValue)
	{
		SetProperty(propertyName, propertyValue, SettingsSerializeAs.String);
	}


	public void SetProperty(
		string propertyName,
		object propertyValue,
		SettingsSerializeAs serializeAs
	)
	{
		bool lazyLoad = false;

		SetProperty(propertyName, propertyValue, serializeAs, lazyLoad);
	}


	public void SetProperty(
		string propertyName,
		object propertyValue,
		SettingsSerializeAs serializeAs,
		bool lazyLoad
	)
	{
		switch (propertyName)
		{
			//the properties identified in the case statements
			//were already implemented on SiteUser
			//before the exstensible profile was implemented

			case "UserID":
				//no change allowed through profile

				break;

			case "SiteID":
				//no change allowed through profile

				break;

			case "IsDeleted":
				//no change allowed through profile

				break;

			case "UserGuid":
				//no change allowed through profile

				break;

			case "RegisterConfirmGuid":
				//no change allowed through profile

				break;

			case "TimeZoneId":
				if (propertyValue is string)
				{
					TimeZoneId = propertyValue.ToString();

					Save();
				}

				break;

			case "FirstName":
				if (propertyValue is string)
				{
					FirstName = propertyValue.ToString();

					Save();
				}

				break;

			case "LastName":
				if (propertyValue is string)
				{
					LastName = propertyValue.ToString();

					Save();
				}

				break;

			case "PhoneNumber":
				if (propertyValue is string)
				{
					PhoneNumber = propertyValue.ToString();

					Save();
				}

				break;

			case "AuthorBio":
				if (propertyValue is string)
				{
					AuthorBio = propertyValue.ToString();

					Save();
				}

				break;

			case "Name":
				if (propertyValue is string)
				{
					Name = propertyValue.ToString();

					Save();
				}

				break;

			case "LoginName":
				//no change allowed through profile

				break;

			case "Email":
				//no change allowed through profile

				break;

			case "Password":
				//no change allowed through profile

				break;

			case "Gender":
				if (propertyValue is string)
				{
					Gender = propertyValue.ToString();

					Save();
				}
				break;

			case "ProfileApproved":
				//no change allowed through profile

				break;

			case "ApprovedForForums":
				//no change allowed through profile

				break;

			case "Trusted":
				//no change allowed through profile

				break;

			case "DisplayInMemberList":
				//no change allowed through profile

				break;

			case "WebSiteUrl":
				if (propertyValue is string)
				{
					WebSiteUrl = propertyValue.ToString();

					if (WebSiteUrl.Length > 100)
					{
						WebSiteUrl = WebSiteUrl.Substring(0, 100);
					}

					Save();
				}
				break;

			case "Country":
				if (propertyValue is string)
				{
					Country = propertyValue.ToString();

					if (Country.Length > 100)
					{
						Country = Country.Substring(0, 100);
					}

					Save();
				}
				break;

			case "State":
				if (propertyValue is string)
				{
					State = propertyValue.ToString();

					if (State.Length > 100)
					{
						State = State.Substring(0, 100);
					}

					Save();
				}
				break;

			case "Occupation":
				if (propertyValue is string)
				{
					Occupation = propertyValue.ToString();

					if (Occupation.Length > 100)
					{
						Occupation = Occupation.Substring(0, 100);
					}

					Save();
				}
				break;

			case "Interests":
				if (propertyValue is string)
				{
					Interests = propertyValue.ToString();

					if (Interests.Length > 100)
					{
						Interests = Interests.Substring(0, 100);
					}

					Save();
				}
				break;

			case "MSN":
				if (propertyValue is string)
				{
					MSN = propertyValue.ToString();

					if (MSN.Length > 50)
					{
						MSN = MSN.Substring(0, 50);
					}

					Save();
				}
				break;

			case "Yahoo":
				if (propertyValue is string)
				{
					Yahoo = propertyValue.ToString();

					if (Yahoo.Length > 50)
					{
						Yahoo = Yahoo.Substring(0, 50);
					}

					Save();
				}
				break;

			case "AIM":
				if (propertyValue is string)
				{
					AIM = propertyValue.ToString();

					if (AIM.Length > 50)
					{
						AIM = AIM.Substring(0, 50);
					}

					Save();
				}
				break;

			case "ICQ":
				if (propertyValue is string)
				{
					ICQ = propertyValue.ToString();

					if (ICQ.Length > 50)
					{
						ICQ = ICQ.Substring(0, 50);
					}

					Save();
				}
				break;

			case "TotalPosts":
				//no change allowed through profile

				break;

			case "AvatarUrl":
				//no change allowed through profile

				break;

			case "Signature":
				if (propertyValue is string)
				{
					Signature = propertyValue.ToString();

					Save();
				}

				break;

			case "DateCreated":
				//no change allowed through profile

				break;

			case "Skin":
				//no change allowed through profile

				break;

			case "LoweredEmail":
				//no change allowed through profile

				break;

			case "PasswordQuestion":
				//no change allowed through profile

				break;

			case "PasswordAnswer":
				//no change allowed through profile

				break;

			case "LastActivityDate":
				//no change allowed through profile

				break;

			default:
				// this is for properties added to config
				// that were not previously implemented on SiteUser

				bool propertyExists = DBSiteUser.PropertyExists(UserGuid, propertyName);

				if (!propertyExists)
				{
					CreateProperty(propertyName, propertyValue, serializeAs, lazyLoad);

					if (!lazyLoad)
					{
						CreatePropertyLocalInstance(propertyName, propertyValue, serializeAs);
					}
				}
				else
				{
					UpdateProperty(propertyName, propertyValue, serializeAs, lazyLoad);

					if (!lazyLoad)
					{
						UpdatePropertyLocalInstance(propertyName, propertyValue, serializeAs);
					}
				}

				break;
		}
	}


	private void CreateProperty(string propertyName, object propertyValue, SettingsSerializeAs serializeAs, bool lazyLoad)
	{
		Guid propertyID = Guid.NewGuid();

		if (serializeAs == SettingsSerializeAs.String)
		{
			// currently only serializing to string
			DBSiteUser.CreateProperty(
				propertyID,
				UserGuid,
				propertyName,
				propertyValue.ToString(),
				null,
				DateTime.UtcNow,
				lazyLoad
			);
		}
	}


	private void UpdateProperty(
		string propertyName,
		object propertyValue,
		SettingsSerializeAs serializeAs,
		bool lazyLoad
	)
	{
		if (serializeAs == SettingsSerializeAs.String)
		{
			// currently only serializing to string
			DBSiteUser.UpdateProperty(
				UserGuid,
				propertyName,
				propertyValue.ToString(),
				null,
				DateTime.UtcNow,
				lazyLoad
			);
		}
	}


	private void CreatePropertyLocalInstance(
		string propertyName,
		object propertyValue,
		SettingsSerializeAs serializeAs
	)
	{
		if (
			profileProperties != null &&
			nonLazyLoadedProfilePropertiesLoaded
		)
		{
			DataRow row = profileProperties.NewRow();
			row["UserGuid"] = UserGuid.ToString();
			row["PropertyName"] = propertyName;

			switch (serializeAs)
			{
				case SettingsSerializeAs.String:
					row["PropertyValueString"] = propertyValue.ToString();
					break;

				default:
					row["PropertyValueBinary"] = propertyValue;
					break;

			}

			profileProperties.Rows.Add(row);
		}
	}


	private void UpdatePropertyLocalInstance(
		string propertyName,
		object propertyValue,
		SettingsSerializeAs serializeAs
	)
	{
		if (
			profileProperties != null &&
			nonLazyLoadedProfilePropertiesLoaded
		)
		{
			foreach (DataRow row in profileProperties.Rows)
			{
				if (row["PropertyName"].ToString() == propertyName)
				{
					switch (serializeAs)
					{
						case SettingsSerializeAs.String:
							row["PropertyValueString"] = propertyValue.ToString();
							break;

						default:
							row["PropertyValueBinary"] = propertyValue;
							break;

					}

					return;
				}
			}
		}
	}


	public object GetProperty(string propertyName, SettingsSerializeAs serializeAs)
	{
		bool lazyLoad = false;

		return GetProperty(propertyName, serializeAs, lazyLoad);
	}


	public string GetCustomPropertyAsString(string propertyName)
	{
		object o = GetProperty(propertyName, SettingsSerializeAs.String, false);

		if (o == null)
		{
			return string.Empty;
		}

		return o.ToString();
	}

	public object GetProperty(
		string propertyName,
		SettingsSerializeAs serializeAs,
		bool lazyLoad
	)
	{
		switch (propertyName)
		{
			//the properties identified in the case statements
			//were already implemented on SiteUser
			//before the exstensible profile was implemented
			//so we just return the existing property for those
			//other properties are handled by the bottom default case
			//so arbitrary properties can be configured

			case "UserID":
				return UserId;

			case "SiteID":
				return SiteId;

			case "IsDeleted":
				return IsDeleted;

			case "UserGuid":
				return UserGuid;

			case "RegisterConfirmGuid":
				return RegisterConfirmGuid;

			case "FirstName":
				return FirstName;

			case "LastName":
				return LastName;

			case "PhoneNumber":
				return PhoneNumber;

			case "AuthorBio":
				return AuthorBio;

			case "DateOfBirth":
				if (DateOfBirth == DateTime.MinValue)
				{
					return string.Empty;
				}

				return DateOfBirth;

			case "Name":
				return Name;

			case "LoginName":
				return LoginName;

			case "Email":
				return Email;

			case "Password":
				// don't return the password from the profile object
				return string.Empty;

			case "Gender":
				return Gender;

			case "ProfileApproved":
				return ProfileApproved;

			case "ApprovedForLogin":
				return ApprovedForLogin;

			case "Trusted":
				return Trusted;

			case "DisplayInMemberList":
				return DisplayInMemberList;

			case "WebSiteUrl":
				return WebSiteUrl;

			case "Country":
				return Country;

			case "State":
				return State;

			case "Occupation":
				return Occupation;

			case "Interests":
				return Interests;

			case "MSN":
				return MSN;

			case "Yahoo":
				return Yahoo;

			case "AIM":
				return AIM;

			case "ICQ":
				return ICQ;

			case "TotalPosts":
				return TotalPosts;

			case "AvatarUrl":
				return AvatarUrl;

			case "TimeOffsetHours":
				EnsureNonLazyLoadedProperties();
				object objTimeOffset = GetNonLazyLoadedProperty(propertyName, serializeAs);

				if (objTimeOffset != null && objTimeOffset.ToString().Length > 0)
				{
					return objTimeOffset;
				}
				else
				{
					string timeOffset = "-5.00";

					if (ConfigurationManager.AppSettings["PreferredGreenwichMeantimeOffset"] != null)
					{
						timeOffset = ConfigurationManager.AppSettings["PreferredGreenwichMeantimeOffset"];
					}

					return timeOffset;
				}

			case "Signature":
				return Signature;

			case "DateCreated":
				return DateCreated;

			case "Skin":
				return Skin;

			case "LoweredEmail":
				return LoweredEmail;

			case "PasswordQuestion":
				return PasswordQuestion;

			case "PasswordAnswer":
				return PasswordAnswer;

			case "LastActivityDate":
				return LastActivityDate;

			default:
				// this is for properties added to config
				//that were not previously implemented on SiteUser
				if (!lazyLoad)
				{
					EnsureNonLazyLoadedProperties();

					return GetNonLazyLoadedProperty(propertyName, serializeAs);
				}
				else
				{
					// lazyLoaded Properties are either seldom used or expensive
					// and therefore only loaded from the db as needed
					return GetLazyLoadedProperty(propertyName, serializeAs);
				}
		}
	}


	private void EnsureNonLazyLoadedProperties()
	{
		if (
			profileProperties == null ||
			!nonLazyLoadedProfilePropertiesLoaded
		)
		{
			profileProperties = DBSiteUser.GetNonLazyLoadedPropertiesForUser(UserGuid);
		}
	}


	private object GetNonLazyLoadedProperty(string propertyName, SettingsSerializeAs serializeAs)
	{
		if (
			profileProperties != null &&
			profileProperties.Rows.Count > 0
		)
		{
			switch (serializeAs)
			{
				case SettingsSerializeAs.String:

					foreach (DataRow row in profileProperties.Rows)
					{
						if (row["PropertyName"].ToString() == propertyName)
						{
							//return row["PropertyValueString"].ToString(CultureInfo.InvariantCulture);
							return row["PropertyValueString"];
						}
					}

					break;

				default:
					foreach (DataRow row in profileProperties.Rows)
					{
						if (row["PropertyName"].ToString() == propertyName)
						{
							return row["PropertyValueBinary"];
						}
					}

					break;
			}
		}

		return null;
	}


	private object GetLazyLoadedProperty(string propertyName, SettingsSerializeAs serializeAs)
	{
		object result = null;

		using (IDataReader reader = DBSiteUser.GetLazyLoadedProperty(UserGuid, propertyName))
		{
			if (reader.Read())
			{
				switch (serializeAs)
				{
					case SettingsSerializeAs.String:

						if (reader["PropertyName"].ToString() == propertyName)
						{
							result = reader["PropertyValueString"].ToString();
						}

						break;

					default:

						if (reader["PropertyName"].ToString() == propertyName)
						{
							result = reader["PropertyValueBinary"];
						}

						break;
				}
			}
		}

		return result;
	}

	#endregion

	#endregion


	#region Static Methods

	public static bool FlagAsNotDeleted(int userId)
	{
		return DBSiteUser.FlagAsNotDeleted(userId);
	}

	public static bool UpdatePasswordAndSalt(
		int userId,
		int passwordFormat,
		string password,
		string passwordSalt
	)
	{
		return DBSiteUser.UpdatePasswordAndSalt(userId, passwordFormat, password, passwordSalt);
	}


	public static SiteUser GetByEmail(SiteSettings siteSettings, string email)
	{
		if (siteSettings == null)
		{
			return null;
		}

		if (string.IsNullOrEmpty(email))
		{
			return null;
		}

		var siteUser = new SiteUser();
		var siteId = siteSettings.SiteId;

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetSingleUser(siteId, email);

		siteUser.GetUser(reader);

		if (siteUser.UserGuid != Guid.Empty)
		{
			return siteUser;
		}

		return null;
	}


	public static SiteUser GetByLoginName(
		SiteSettings siteSettings,
		string userName,
		bool allowEmailFallback
	)
	{
		if (siteSettings == null)
		{
			return null;
		}

		if (string.IsNullOrEmpty(userName))
		{
			return null;
		}

		var siteUser = new SiteUser();
		var siteId = siteSettings.SiteId;

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = GetUserByLoginName(siteId, userName, allowEmailFallback);

		siteUser.GetUser(reader);

		if (siteUser.UserGuid != Guid.Empty)
		{
			return siteUser;
		}

		return null;
	}


	public static SiteUser GetByConfirmationGuid(SiteSettings siteSettings, Guid confirmGuid)
	{
		if (siteSettings == null)
		{
			return null;
		}

		if (confirmGuid == Guid.Empty)
		{
			return null;
		}

		var siteUser = new SiteUser();

		using IDataReader reader = DBSiteUser.GetUserByRegistrationGuid(siteSettings.SiteId, confirmGuid);

		siteUser.GetUser(reader);

		if (siteUser.UserGuid != Guid.Empty)
		{
			return siteUser;
		}

		return null;
	}


	public static bool ConfirmRegistration(Guid registrationGuid)
	{
		if (registrationGuid == Guid.Empty)
		{

			return false;
		}

		return DBSiteUser.ConfirmRegistration(Guid.Empty, registrationGuid);
	}


	public static IDataReader GetUserList(int siteId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetUserList(siteId);
	}


	public static DataTable GetUserListForPasswordFormatChange(int siteId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var dt = new DataTable();

		dt.Columns.Add("UserID", typeof(int));
		dt.Columns.Add("PasswordSalt", typeof(string));
		dt.Columns.Add("Pwd", typeof(string));

		using IDataReader reader = DBSiteUser.GetUserList(siteId);

		while (reader.Read())
		{
			var row = dt.NewRow();

			row["UserID"] = reader["UserID"];
			row["PasswordSalt"] = reader["PasswordSalt"];
			row["Pwd"] = reader["Pwd"];

			dt.Rows.Add(row);
		}

		return dt;
	}


	public static int UserCount(int siteId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.UserCount(siteId);
	}


	public static int UserCount(
		int siteId,
		string userNameBeginsWith,
		string nameFilterMode
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.UserCount(siteId, userNameBeginsWith, nameFilterMode);
	}


	public static int UsersOnlineSinceCount(int siteId, DateTime sinceTime)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.CountOnlineSince(siteId, sinceTime);
	}


	public static IDataReader GetUsersOnlineSince(int siteId, DateTime sinceTime)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetUsersOnlineSince(siteId, sinceTime);
	}


	public static IDataReader GetUsersTop50OnlineSince(int siteId, DateTime sinceTime)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetTop50UsersOnlineSince(siteId, sinceTime);
	}


	public static DataTable GetRoleMembers(int roleId)
	{
		DataTable dataTable = new DataTable();

		dataTable.Columns.Add("UserID", typeof(int));
		dataTable.Columns.Add("Name", typeof(string));
		dataTable.Columns.Add("Email", typeof(string));
		dataTable.Columns.Add("LoginName", typeof(string));

		using IDataReader reader = Role.GetRoleMembers(roleId);

		while (reader.Read())
		{
			var row = dataTable.NewRow();

			row["UserID"] = reader["UserID"];
			row["Name"] = reader["Name"];
			row["Email"] = reader["Email"];
			row["LoginName"] = reader["LoginName"];

			dataTable.Rows.Add(row);
		}

		return dataTable;
	}


	public static List<string> GetEmailAddresses(int siteId, string roleNamesSeparatedBySemiColons)
	{
		var emailAddresses = new List<string>();
		var roleIds = Role.GetRoleIds(siteId, roleNamesSeparatedBySemiColons);

		foreach (int roleId in roleIds)
		{
			using IDataReader reader = Role.GetRoleMembers(roleId);

			while (reader.Read())
			{
				var email = reader["Email"].ToString().ToLower();

				if (!emailAddresses.Contains(email))
				{
					emailAddresses.Add(email);
				}
			}
		}

		return emailAddresses;
	}


	/// <summary>
	/// Gets Data for a user registration by month chart. Fields Y, M, Label, Users
	/// Label is just a concat of Year-Month, Users is the count.
	/// </summary>
	/// <param name="siteId"></param>
	/// <returns></returns>
	public static IDataReader GetUserCountByYearMonth(int siteId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetUserCountByYearMonth(siteId);
	}


	public static List<SiteUser> GetByIPAddress(Guid siteGuid, string ipv4Address)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteGuid = Guid.Empty;
		}

		using IDataReader reader = UserLocation.GetUsersByIPAddress(siteGuid, ipv4Address);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	public static List<SiteUser> GetPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string beginsWith,
		int sortMode,
		string nameFilterMode,
		out int totalPages
	)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetUserListPage(
			siteId,
			pageNumber,
			pageSize,
			beginsWith,
			sortMode,
			nameFilterMode,
			out totalPages
		);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	public static List<SiteUser> GetUserSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		out int totalPages
	)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetUserSearchPage(
			siteId,
			pageNumber,
			pageSize,
			searchInput,
			sortMode,
			out totalPages
		);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	public static List<SiteUser> GetUserAdminSearchPage(
		int siteId,
		int pageNumber,
		int pageSize,
		string searchInput,
		int sortMode,
		out int totalPages
	)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetUserAdminSearchPage(
			siteId,
			pageNumber,
			pageSize,
			searchInput,
			sortMode,
			out totalPages
		);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	public static List<SiteUser> GetPageLockedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages
	)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetPageLockedUsers(
			siteId,
			pageNumber,
			pageSize,
			out totalPages
		);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	public static List<SiteUser> GetNotApprovedUsers(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages
	)
	{
		var userList = new List<SiteUser>();

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		using IDataReader reader = DBSiteUser.GetPageNotApprovedUsers(
			siteId,
			pageNumber,
			pageSize,
			out totalPages
		);

		while (reader.Read())
		{
			var user = new SiteUser();

			PopulateFromReaderRow(user, reader);

			userList.Add(user);
		}

		return userList;
	}


	private static void PopulateFromReaderRow(SiteUser user, IDataReader reader)
	{
		user.UserId = Convert.ToInt32(reader["UserID"]);
		user.SiteId = Convert.ToInt32(reader["SiteID"]);

		if (UseRelatedSiteMode)
		{
			user.SiteId = RelatedSiteID;
		}

		user.Name = reader["Name"].ToString();
		user.LoginName = reader["LoginName"].ToString();

		user.FirstName = reader["FirstName"].ToString();
		user.LastName = reader["LastName"].ToString();

		user.Email = reader["Email"].ToString();
		user.LoweredEmail = reader["LoweredEmail"].ToString();
		user.Password = reader["Pwd"].ToString();
		user.passwordQuestion = reader["PasswordQuestion"].ToString();
		user.passwordAnswer = reader["PasswordAnswer"].ToString();
		user.Gender = reader["Gender"].ToString();

		if (reader["ProfileApproved"] != DBNull.Value)
		{
			user.ProfileApproved = Convert.ToBoolean(reader["ProfileApproved"]);
		}

		if (reader["ApprovedForForums"] != DBNull.Value)
		{
			user.ApprovedForLogin = Convert.ToBoolean(reader["ApprovedForForums"]);
		}

		if (reader["Trusted"] != DBNull.Value)
		{
			user.Trusted = Convert.ToBoolean(reader["Trusted"]);
		}

		if (reader["DisplayInMemberList"] != DBNull.Value)
		{
			user.DisplayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
		}

		if (reader["IsDeleted"] != DBNull.Value)
		{
			user.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
		}

		if (reader["IsLockedOut"] != DBNull.Value)
		{
			user.isLockedOut = Convert.ToBoolean(reader["IsLockedOut"]);
		}

		user.WebSiteUrl = reader["WebSiteURL"].ToString();
		user.Country = reader["Country"].ToString();
		user.State = reader["State"].ToString();
		user.Occupation = reader["Occupation"].ToString();
		user.Interests = reader["Interests"].ToString();
		user.MSN = reader["MSN"].ToString();
		user.Yahoo = reader["Yahoo"].ToString();
		user.AIM = reader["AIM"].ToString();
		user.ICQ = reader["ICQ"].ToString();

		if (reader["TotalPosts"] != DBNull.Value)
		{
			user.TotalPosts = Convert.ToInt32(reader["TotalPosts"]);
		}

		user.AvatarUrl = reader["AvatarUrl"].ToString();
		user.Signature = reader["Signature"].ToString();

		if (reader["DateCreated"] != DBNull.Value)
		{
			user.dateCreated = Convert.ToDateTime(reader["DateCreated"]);
		}

		user.UserGuid = new Guid(reader["UserGuid"].ToString());
		user.Skin = reader["Skin"].ToString();

		if (reader["LastActivityDate"] != DBNull.Value)
		{
			user.lastActivityDate = Convert.ToDateTime(reader["LastActivityDate"]);
		}

		if (reader["LastLoginDate"] != DBNull.Value)
		{
			user.lastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]);
		}

		if (reader["LastPasswordChangedDate"] != DBNull.Value)
		{
			user.lastPasswordChangedDate = Convert.ToDateTime(reader["LastPasswordChangedDate"]);
		}

		if (reader["LastLockoutDate"] != DBNull.Value)
		{
			user.lastLockoutDate = Convert.ToDateTime(reader["LastLockoutDate"]);
		}

		if (reader["FailedPasswordAttemptCount"] != DBNull.Value)
		{
			user.failedPasswordAttemptCount = Convert.ToInt32(reader["FailedPasswordAttemptCount"]);
		}

		if (reader["FailedPwdAttemptWindowStart"] != DBNull.Value)
		{
			user.failedPasswordAttemptWindowStart
				= Convert.ToDateTime(reader["FailedPwdAttemptWindowStart"]);
		}

		if (reader["FailedPwdAnswerAttemptCount"] != DBNull.Value)
		{
			user.failedPasswordAnswerAttemptCount
				= Convert.ToInt32(reader["FailedPwdAnswerAttemptCount"]);
		}

		if (reader["FailedPwdAnswerWindowStart"] != DBNull.Value)
		{
			user.failedPasswordAnswerAttemptWindowStart
				= Convert.ToDateTime(reader["FailedPwdAnswerWindowStart"]);
		}

		user.passwordSalt = reader["PasswordSalt"].ToString();
		user.comment = reader["Comment"].ToString();
		user.mustChangePwd = Convert.ToBoolean(reader["MustChangePwd"]);
		user.rolesChanged = Convert.ToBoolean(reader["RolesChanged"]);
	}


	public static IDataReader GetUserByEmail(int siteId, string email)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetSingleUser(siteId, email);
	}


	public static IDataReader GetUserByLoginName(
		int siteId,
		string loginName,
		bool allowEmailFallback
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetSingleUserByLoginName(siteId, loginName, allowEmailFallback);
	}


	public static IDataReader GetRolesByUser(int siteId, int userId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetRolesByUser(siteId, userId);
	}


	public static string[] GetRoles(SiteSettings siteSettings, string identity)
	{
		var siteUser = new SiteUser(siteSettings, identity);

		return [.. GetRoles(siteUser)];
	}


	public static IList<string> GetRoles(SiteUser siteUser)
	{
		var siteId = siteUser.SiteId;

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var userRoles = new List<string>();

		using IDataReader reader = DBSiteUser.GetRolesByUser(siteId, siteUser.UserId);

		while (reader.Read())
		{
			userRoles.Add(reader["RoleName"].ToString());
		}

		return userRoles;
	}


	public static string Login(
		SiteSettings siteSettings,
		string loginId,
		string password
	)
	{
		var siteId = siteSettings.SiteId;

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		if (siteSettings.UseEmailForLogin)
		{
			string foundUser = DBSiteUser.LoginByEmail(siteSettings.SiteId, loginId, password);

			if (foundUser != string.Empty)
			{
				return foundUser;
			}
			else
			{
				return DBSiteUser.Login(siteId, loginId, password);
			}
		}
		else
		{
			return DBSiteUser.Login(siteId, loginId, password);
		}
	}


	public static string LoginLDAP(
		SiteSettings siteSettings,
		string loginId,
		string password,
		out SiteUser userCreatedForLdap
	)
	{
		userCreatedForLdap = null;

		var siteId = siteSettings.SiteId;

		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		// if using ldap we don't login by email
		var user = LdapHelper.LdapLogin(siteSettings.SiteLdapSettings, loginId, password);

		if (user != null)
		{
			var existsInDB = LoginExistsInDB(siteId, loginId);

			if (existsInDB)
			{
				return user.CommonName;
			}
			else
			{
				if (siteSettings.AutoCreateLdapUserOnFirstLogin)
				{
					userCreatedForLdap = new SiteUser(siteSettings);

					if (user.FirstName.Length > 0 && user.LastName.Length > 0)
					{
						userCreatedForLdap.Name = user.FirstName + " " + user.LastName;
					}
					else
					{
						userCreatedForLdap.Name = user.CommonName;
					}

					userCreatedForLdap.LoginName = loginId;
					userCreatedForLdap.Email = user.Email;
					// This password would be used during pre-LDAP fallback authentication, or if the site 
					// was changed back from LDAP to standard db authentication, so we need to populate 
					// it with something strong and unguessable.
					userCreatedForLdap.Password = CreateRandomPassword(12, string.Empty);
					userCreatedForLdap.Save();

					return user.CommonName;
				}
				else
				{
					return string.Empty;
				}
			}
		}
		else
		{
			return string.Empty;
		}
	}


	public static bool IncrementTotalPosts(int userId)
	{
		return DBSiteUser.IncrementTotalPosts(userId);
	}


	public static bool DecrementTotalPosts(int userId)
	{
		return DBSiteUser.DecrementTotalPosts(userId);
	}


	public static IDataReader GetSmartDropDownData(
		int siteId,
		string query,
		int rowsToGet
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetSmartDropDownData(siteId, query, rowsToGet);
	}


	public static IDataReader EmailLookup(
		int siteId,
		string query,
		int rowsToGet
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.EmailLookup(siteId, query, rowsToGet);
	}


	public static bool EmailExistsInDB(int siteId, string email)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var found = false;

		using IDataReader r = DBSiteUser.GetSingleUser(siteId, email);

		while (r.Read())
		{
			found = true;
		}

		return found;
	}


	public static bool EmailExistsInDB(
		int siteId,
		int userId,
		string email
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var found = false;

		using IDataReader r = DBSiteUser.GetSingleUser(siteId, email);

		while (r.Read())
		{
			var foundId = Convert.ToInt32(r["UserID"]);

			found = foundId != userId;

			if (found)
			{
				return found;
			}
		}

		return found;
	}


	public static bool LoginExistsInDB(int siteId, string loginName)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var found = false;

		using IDataReader r = DBSiteUser.GetSingleUserByLoginName(siteId, loginName, false);

		while (r.Read())
		{
			found = true;
		}

		return found;
	}


	public static string CreateRandomPassword(int length, string allowedPasswordChars)
	{
		if (length == 0)
		{
			length = 7;
		}

		char[] allowedChars;

		if (string.IsNullOrEmpty(allowedPasswordChars))
		{
			allowedChars = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$".ToCharArray();
		}
		else
		{
			allowedChars = allowedPasswordChars.ToCharArray();
		}

		var passwordChars = new char[length];
		var seedBytes = new byte[4];
		var crypto = new RNGCryptoServiceProvider();

		crypto.GetBytes(seedBytes);

		var seed =
			(seedBytes[0] & 0x7f) << 24 |
			seedBytes[1] << 16 |
			seedBytes[2] << 8 |
			seedBytes[3];

		Random random = new(seed);

		for (int i = 0; i < length; i++)
		{
			passwordChars[i] = allowedChars[random.Next(0, allowedChars.Length)];
		}

		return new string(passwordChars);
	}


	public static string GetUserNameFromEmail(int siteId, string email)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		var result = string.Empty;

		if (email != null && email.Length > 0 && siteId > 0)
		{
			var comma = string.Empty;

			using IDataReader reader = DBSiteUser.GetSingleUser(siteId, email);

			while (reader.Read())
			{
				result += comma + reader["LoginName"].ToString();
				comma = ", ";
			}
		}

		return result;
	}


	public static int GetNewestUserId(int siteId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetNewestUserId(siteId);
	}


	public static SiteUser GetNewestUser(SiteSettings siteSettings)
	{
		var userID = GetNewestUserId(siteSettings.SiteId);
		var siteUser = new SiteUser(siteSettings, userID);

		if (siteUser.UserId == userID)
		{
			return siteUser;
		}

		return null;
	}


	public static int CountUsersByRegistrationDateRange(
		int siteId,
		DateTime beginDate,
		DateTime endDate
	)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.CountUsersByRegistrationDateRange(siteId, beginDate, endDate);
	}


	public static Guid GetUserGuidFromOpenId(int siteId, string openIdUri)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetUserGuidFromOpenId(siteId, openIdUri);
	}


	public static Guid GetUserGuidFromWindowsLiveId(int siteId, string windowsLiveId)
	{
		if (UseRelatedSiteMode)
		{
			siteId = RelatedSiteID;
		}

		return DBSiteUser.GetUserGuidFromWindowsLiveId(siteId, windowsLiveId);
	}


	public static void UpdateTotalRevenue(Guid userGuid)
	{
		DBSiteUser.UpdateTotalRevenue(userGuid);
	}


	/// <summary>
	/// updates the total revenue for all users
	/// </summary>
	public static void UpdateTotalRevenue()
	{
		DBSiteUser.UpdateTotalRevenue();
	}


	public static SiteUser GetByGuid(SiteSettings siteSettings, Guid userGuid)
	{
		if (siteSettings == null)
		{
			return null;
		}

		var user = new SiteUser(siteSettings, userGuid);

		if (user.UserId > -1 && user.SiteGuid == siteSettings.SiteGuid)
		{
			return user;
		}

		return null;
	}

	#endregion
}
