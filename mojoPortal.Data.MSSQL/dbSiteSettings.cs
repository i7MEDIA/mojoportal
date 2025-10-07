using System;
using System.Data;

namespace mojoPortal.Data
{    
    public static class DBSiteSettings
    {
        public static int Create(
            Guid siteGuid,
            String siteName,
            String skin,
            String logo,
            String icon,
            bool allowNewRegistration,
            bool allowUserSkins,
            bool allowPageSkins,
            bool allowHideMenuOnPages,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            String defaultPageKeywords,
            String defaultPageDescription,
            String defaultPageEncoding,
            String defaultAdditionalMetaTags,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            String ldapServer,
            int ldapPort,
            String ldapDomain,
            String ldapRootDN,
            String ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            String editorSkin,
            String defaultFriendlyUrlPattern,
            bool enableMyPageFeature,
            string editorProvider,
            string datePickerProvider,
            string captchaProvider,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string wordpressApiKey,
            string windowsLiveAppId,
            string windowsLiveKey,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            string gmapApiKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Insert", 46);
            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@Logo", SqlDbType.NVarChar, 50, ParameterDirection.Input, logo);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 50, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@AllowUserSkins", SqlDbType.Bit, ParameterDirection.Input, allowUserSkins);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseSSLOnAllPages", SqlDbType.Bit, ParameterDirection.Input, useSslOnAllPages);
            sph.DefineSqlParameter("@DefaultPageKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageKeywords);
            sph.DefineSqlParameter("@DefaultPageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageDescription);
            sph.DefineSqlParameter("@DefaultPageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageEncoding);
            sph.DefineSqlParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultAdditionalMetaTags);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@AllowPageSkins", SqlDbType.Bit, ParameterDirection.Input, allowPageSkins);
            sph.DefineSqlParameter("@AllowHideMenuOnPages", SqlDbType.Bit, ParameterDirection.Input, allowHideMenuOnPages);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);
            sph.DefineSqlParameter("@EditorSkin", SqlDbType.NVarChar, 50, ParameterDirection.Input, editorSkin);
            sph.DefineSqlParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar, 50, ParameterDirection.Input, defaultFriendlyUrlPattern);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@EnableMyPageFeature", SqlDbType.Bit, ParameterDirection.Input, enableMyPageFeature);
            sph.DefineSqlParameter("@EditorProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, editorProvider);
            sph.DefineSqlParameter("@DatePickerProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, datePickerProvider);
            sph.DefineSqlParameter("@CaptchaProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, captchaProvider);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@WordpressAPIKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, wordpressApiKey);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);

            sph.DefineSqlParameter("@GmapApiKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, gmapApiKey);
            sph.DefineSqlParameter("@ApiKeyExtra1", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra1);
            sph.DefineSqlParameter("@ApiKeyExtra2", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra2);
            sph.DefineSqlParameter("@ApiKeyExtra3", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra3);
            sph.DefineSqlParameter("@ApiKeyExtra4", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra4);
            sph.DefineSqlParameter("@ApiKeyExtra5", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra5);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);

            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int siteId,
            string siteName,
            string skin,
            string logo,
            string icon,
            bool allowNewRegistration,
            bool allowUserSkins,
            bool allowPageSkins,
            bool allowHideMenuOnPages,
            bool useSecureRegistration,
            bool useSslOnAllPages,
            string defaultPageKeywords,
            string defaultPageDescription,
            string defaultPageEncoding,
            string defaultAdditionalMetaTags,
            bool isServerAdminSite,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            int ldapPort,
            String ldapDomain,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool reallyDeleteUsers,
            String editorSkin,
            String defaultFriendlyUrlPattern,
            bool enableMyPageFeature,
            string editorProvider,
            string datePickerProvider,
            string captchaProvider,
            string recaptchaPrivateKey,
            string recaptchaPublicKey,
            string wordpressApiKey,
            string windowsLiveAppId,
            string windowsLiveKey,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            string gmapApiKey,
            string apiKeyExtra1,
            string apiKeyExtra2,
            string apiKeyExtra3,
            string apiKeyExtra4,
            string apiKeyExtra5,
            bool disableDbAuth)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Update", 46);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteName", SqlDbType.NVarChar, 128, ParameterDirection.Input, siteName);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@Logo", SqlDbType.NVarChar, 50, ParameterDirection.Input, logo);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 50, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@AllowUserSkins", SqlDbType.Bit, ParameterDirection.Input, allowUserSkins);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseSSLOnAllPages", SqlDbType.Bit, ParameterDirection.Input, useSslOnAllPages);
            sph.DefineSqlParameter("@DefaultPageKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageKeywords);
            sph.DefineSqlParameter("@DefaultPageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageDescription);
            sph.DefineSqlParameter("@DefaultPageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultPageEncoding);
            sph.DefineSqlParameter("@DefaultAdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, defaultAdditionalMetaTags);
            sph.DefineSqlParameter("@IsServerAdminSite", SqlDbType.Bit, ParameterDirection.Input, isServerAdminSite);
            sph.DefineSqlParameter("@AllowPageSkins", SqlDbType.Bit, ParameterDirection.Input, allowPageSkins);
            sph.DefineSqlParameter("@AllowHideMenuOnPages", SqlDbType.Bit, ParameterDirection.Input, allowHideMenuOnPages);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@ReallyDeleteUsers", SqlDbType.Bit, ParameterDirection.Input, reallyDeleteUsers);
            sph.DefineSqlParameter("@EditorSkin", SqlDbType.NVarChar, 50, ParameterDirection.Input, editorSkin);
            sph.DefineSqlParameter("@DefaultFriendlyUrlPatternEnum", SqlDbType.NVarChar, 50, ParameterDirection.Input, defaultFriendlyUrlPattern);
            sph.DefineSqlParameter("@EnableMyPageFeature", SqlDbType.Bit, ParameterDirection.Input, enableMyPageFeature);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@EditorProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, editorProvider);
            sph.DefineSqlParameter("@DatePickerProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, datePickerProvider);
            sph.DefineSqlParameter("@CaptchaProvider", SqlDbType.NVarChar, 255, ParameterDirection.Input, captchaProvider);
            sph.DefineSqlParameter("@RecaptchaPrivateKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPrivateKey);
            sph.DefineSqlParameter("@RecaptchaPublicKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, recaptchaPublicKey);
            sph.DefineSqlParameter("@WordpressAPIKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, wordpressApiKey);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);
            sph.DefineSqlParameter("@GmapApiKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, gmapApiKey);
            sph.DefineSqlParameter("@ApiKeyExtra1", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra1);
            sph.DefineSqlParameter("@ApiKeyExtra2", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra2);
            sph.DefineSqlParameter("@ApiKeyExtra3", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra3);
            sph.DefineSqlParameter("@ApiKeyExtra4", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra4);
            sph.DefineSqlParameter("@ApiKeyExtra5", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiKeyExtra5);
            sph.DefineSqlParameter("@DisableDbAuth", SqlDbType.Bit, ParameterDirection.Input, disableDbAuth);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateExtendedProperties(
            int siteId,
            bool allowPasswordRetrieval,
            bool allowPasswordReset,
            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            bool requiresUniqueEmail,
            int passwordFormat,
            int minRequiredPasswordLength,
            int minRequiredNonAlphanumericCharacters,
            String passwordStrengthRegularExpression,
            String defaultEmailFromAddress
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_UpdateExtendedProperties", 12);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@AllowPasswordRetrieval", SqlDbType.Bit, ParameterDirection.Input, allowPasswordRetrieval);
            sph.DefineSqlParameter("@AllowPasswordReset", SqlDbType.Bit, ParameterDirection.Input, allowPasswordReset);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int, ParameterDirection.Input, passwordAttemptWindowMinutes);
            sph.DefineSqlParameter("@RequiresUniqueEmail", SqlDbType.Bit, ParameterDirection.Input, requiresUniqueEmail);
            sph.DefineSqlParameter("@PasswordFormat", SqlDbType.Int, ParameterDirection.Input, passwordFormat);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@MinRequiredNonAlphanumericCharacters", SqlDbType.Int, ParameterDirection.Input, minRequiredNonAlphanumericCharacters);
            sph.DefineSqlParameter("@PasswordStrengthRegularExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, passwordStrengthRegularExpression);
            sph.DefineSqlParameter("@DefaultEmailFromAddress", SqlDbType.NVarChar, 100, ParameterDirection.Input, defaultEmailFromAddress);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateRelatedSites(
            int siteId,
            bool allowNewRegistration,
            bool useSecureRegistration,
            bool useLdapAuth,
            bool autoCreateLdapUserOnFirstLogin,
            string ldapServer,
            string ldapDomain,
            int ldapPort,
            string ldapRootDN,
            string ldapUserDNKey,
            bool allowUserFullNameChange,
            bool useEmailForLogin,
            bool allowOpenIdAuth,
            bool allowWindowsLiveAuth,
            bool allowPasswordRetrieval,
            bool allowPasswordReset,
            bool requiresQuestionAndAnswer,
            int maxInvalidPasswordAttempts,
            int passwordAttemptWindowMinutes,
            bool requiresUniqueEmail,
            int passwordFormat,
            int minRequiredPasswordLength,
            int minReqNonAlphaChars,
            string pwdStrengthRegex
            )
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_UpdateRelatedSiteSecurity", 24);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@AllowNewRegistration", SqlDbType.Bit, ParameterDirection.Input, allowNewRegistration);
            sph.DefineSqlParameter("@UseSecureRegistration", SqlDbType.Bit, ParameterDirection.Input, useSecureRegistration);
            sph.DefineSqlParameter("@UseLdapAuth", SqlDbType.Bit, ParameterDirection.Input, useLdapAuth);
            sph.DefineSqlParameter("@AutoCreateLDAPUserOnFirstLogin", SqlDbType.Bit, ParameterDirection.Input, autoCreateLdapUserOnFirstLogin);
            sph.DefineSqlParameter("@LdapServer", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapServer);
            sph.DefineSqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapDomain);
            sph.DefineSqlParameter("@LdapPort", SqlDbType.Int, ParameterDirection.Input, ldapPort);
            sph.DefineSqlParameter("@LdapRootDN", SqlDbType.NVarChar, 255, ParameterDirection.Input, ldapRootDN);
            sph.DefineSqlParameter("@LdapUserDNKey", SqlDbType.NVarChar, 10, ParameterDirection.Input, ldapUserDNKey);
            sph.DefineSqlParameter("@AllowUserFullNameChange", SqlDbType.Bit, ParameterDirection.Input, allowUserFullNameChange);
            sph.DefineSqlParameter("@UseEmailForLogin", SqlDbType.Bit, ParameterDirection.Input, useEmailForLogin);
            sph.DefineSqlParameter("@AllowOpenIDAuth", SqlDbType.Bit, ParameterDirection.Input, allowOpenIdAuth);
            sph.DefineSqlParameter("@AllowWindowsLiveAuth", SqlDbType.Bit, ParameterDirection.Input, allowWindowsLiveAuth);
            sph.DefineSqlParameter("@AllowPasswordRetrieval", SqlDbType.Bit, ParameterDirection.Input, allowPasswordRetrieval);
            sph.DefineSqlParameter("@AllowPasswordReset", SqlDbType.Bit, ParameterDirection.Input, allowPasswordReset);
            sph.DefineSqlParameter("@RequiresQuestionAndAnswer", SqlDbType.Bit, ParameterDirection.Input, requiresQuestionAndAnswer);
            sph.DefineSqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, ParameterDirection.Input, maxInvalidPasswordAttempts);
            sph.DefineSqlParameter("@PasswordAttemptWindowMinutes", SqlDbType.Int, ParameterDirection.Input, passwordAttemptWindowMinutes);
            sph.DefineSqlParameter("@RequiresUniqueEmail", SqlDbType.Bit, ParameterDirection.Input, requiresUniqueEmail);
            sph.DefineSqlParameter("@PasswordFormat", SqlDbType.Int, ParameterDirection.Input, passwordFormat);
            sph.DefineSqlParameter("@MinRequiredPasswordLength", SqlDbType.Int, ParameterDirection.Input, minRequiredPasswordLength);
            sph.DefineSqlParameter("@MinReqNonAlphaChars", SqlDbType.Int, ParameterDirection.Input, minReqNonAlphaChars);
            sph.DefineSqlParameter("@PwdStrengthRegex", SqlDbType.NVarChar, -1, ParameterDirection.Input, pwdStrengthRegex);
           
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateRelatedSitesWindowsLive(
            int siteId,
            string windowsLiveAppId,
            string windowsLiveKey
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_SyncRelatedSitesWinLive", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@WindowsLiveAppID", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveAppId);
            sph.DefineSqlParameter("@WindowsLiveKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, windowsLiveKey);
            
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }




        public static bool Delete(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Sites_Delete", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
           // sph.ExecuteNonQuery();

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

		public static IDataReader GetSiteList()
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectAll", 0);
			return sph.ExecuteReader();
		}


		public static int GetSiteCount()
		{
			Version version;
			var newProc = true;

			using (IDataReader reader = DBPortal.SchemaVersionGetSchemaVersion(new("077e4857-f583-488e-836e-34a4b04be855")))
			{
				if (reader.Read())
				{
					version = new(
						Convert.ToInt32(reader["Major"]),
						Convert.ToInt32(reader["Minor"]),
						Convert.ToInt32(reader["Build"]),
						Convert.ToInt32(reader["Revision"])
					);

					if (version < new Version(2, 9, 1, 0))
					{
						newProc = false;
					}
				}
			}

			// We're doing this so that upgrading a site from a version earlier than 2910 won't incorrectly think that there are no sites.
			if (newProc)
			{
				var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectCount", 0);

				return Convert.ToInt32(sph.ExecuteScalar());
			}
			else
			{
				var sitesFound = 0;

				using IDataReader reader = GetSiteList();

				if (reader != null)
				{
					while (reader.Read())
					{
						sitesFound += 1;
					}
				}

				return sitesFound;
			}
		}


        public static IDataReader GetSite(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOneByHost", 1);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 50, ParameterDirection.Input, hostName);
            return sph.ExecuteReader();

        }


        public static void AddFeature(Guid siteGuid, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteModuleDefinitions_Insert", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);

            sph.ExecuteNonQuery();

        }

        public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteModuleDefinitions_Delete", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.ExecuteNonQuery();
        }


        public static IDataReader GetSite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOne", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetSite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return sph.ExecuteReader();
        }

        

        public static IDataReader GetPageListForAdmin(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectList", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetHostList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_Select", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

		public static IDataReader GetHostList()
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_SelectAll", 0);
			return sph.ExecuteReader();
		}

		public static void AddHost(Guid siteGuid, int siteId, string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteHosts_Insert", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
            sph.ExecuteNonQuery();
        }

        public static void DeleteHost(int hostId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SiteHosts_Delete", 1);
            sph.DefineSqlParameter("@HostID", SqlDbType.Int, ParameterDirection.Input, hostId);
            sph.ExecuteNonQuery();
        }


        public static int CountOtherSites(int currentSiteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_CountOtherSites", 1);
            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetPageOfOtherSites(
            int currentSiteId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = CountOtherSites(currentSiteId);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_SelectPageOtherSites", 3);
            sph.DefineSqlParameter("@CurrentSiteID", SqlDbType.Int, ParameterDirection.Input, currentSiteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        public static int GetSiteIdByHostName(string hostName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHosts_SelectSiteIdByHost", 1);
            sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static int GetSiteIdByFolder(string folderName, bool legacy = false)
        {
			var siteFoldersSelect = "SELECT TOP(1) SiteID FROM mp_SiteFolders WHERE FolderName = @FolderName";

			if (legacy)
			{
				siteFoldersSelect = """
                    	SELECT TOP(1) s.SiteID FROM mp_SiteFolders sf
                            JOIN mp_Sites s ON s.SiteGuid = sf.SiteGuid
                            WHERE sf.FolderName = @FolderName 
                    """;
			}

			var sqlCommand = $"""
                SELECT COALESCE(
                	({siteFoldersSelect}),
                	(SELECT TOP(1) SiteID FROM mp_Sites WHERE IsServerAdminSite = 1 ORDER BY SiteID),
                	(SELECT TOP(1) SiteID FROM mp_Sites ORDER BY SiteID)
                ) AS SiteID
                """;

			var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), sqlCommand, CommandType.Text, 1);
			sph.DefineSqlParameter("@FolderName", SqlDbType.NVarChar, 255, ParameterDirection.Input, folderName);
			
			return Convert.ToInt32(sph.ExecuteScalar());
        }

		public static bool HostNameExists(string hostName)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteHost_Exists", 1);
			sph.DefineSqlParameter("@HostName", SqlDbType.NVarChar, 255, ParameterDirection.Input, hostName);
			int count = Convert.ToInt32(sph.ExecuteScalar());
			return (count > 0);
		}
		public static void UpdateSkinVersionGuidForAllSites()
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Sites_UpdateSkinVersionGuidForAllSites", 1);
			sph.DefineSqlParameter("@NewGuid", SqlDbType.NVarChar, 255, ParameterDirection.Input, Guid.NewGuid());

			sph.ExecuteScalar();
		}
    }
}
