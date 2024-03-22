using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySqlConnector;

namespace mojoPortal.Data;

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

		#region bool conversion

		byte intDisableDbAuth = 0;
		if (disableDbAuth) { intDisableDbAuth = 1; }

		byte oidauth;
		if (allowOpenIdAuth)
		{
			oidauth = 1;
		}
		else
		{
			oidauth = 0;
		}

		byte winliveauth;
		if (allowWindowsLiveAuth)
		{
			winliveauth = 1;
		}
		else
		{
			winliveauth = 0;
		}

		byte uldapp;
		if (useLdapAuth)
		{
			uldapp = 1;
		}
		else
		{
			uldapp = 0;
		}

		byte autoldapp;
		if (autoCreateLdapUserOnFirstLogin)
		{
			autoldapp = 1;
		}
		else
		{
			autoldapp = 0;
		}

		byte allowNameChange;
		if (allowUserFullNameChange)
		{
			allowNameChange = 1;
		}
		else
		{
			allowNameChange = 0;
		}

		byte emailForLogin;
		if (useEmailForLogin)
		{
			emailForLogin = 1;
		}
		else
		{
			emailForLogin = 0;
		}

		byte deleteUsers;
		if (reallyDeleteUsers)
		{
			deleteUsers = 1;
		}
		else
		{
			deleteUsers = 0;
		}

		byte allowNew;
		if (allowNewRegistration)
		{
			allowNew = 1;
		}
		else
		{
			allowNew = 0;
		}

		byte allowSkins;
		if (allowUserSkins)
		{
			allowSkins = 1;
		}
		else
		{
			allowSkins = 0;
		}

		byte secure;
		if (useSecureRegistration)
		{
			secure = 1;
		}
		else
		{
			secure = 0;
		}

		byte ssl;
		if (useSslOnAllPages)
		{
			ssl = 1;
		}
		else
		{
			ssl = 0;
		}

		byte adminSite;
		if (isServerAdminSite)
		{
			adminSite = 1;
		}
		else
		{
			adminSite = 0;
		}

		byte pageSkins;
		if (allowPageSkins)
		{
			pageSkins = 1;
		}
		else
		{
			pageSkins = 0;
		}

		byte allowHide;
		if (allowHideMenuOnPages)
		{
			allowHide = 1;
		}
		else
		{
			allowHide = 0;
		}

		byte enableMy;
		if (enableMyPageFeature)
		{
			enableMy = 1;
		}
		else
		{
			enableMy = 0;
		}

		#endregion

		string sqlCommand = @"
INSERT INTO mp_Sites ( 
        SiteName, 
        Skin, 
        Logo, 
        Icon, 
        AllowNewRegistration, 
        AllowUserSkins, 
        UseSecureRegistration, 
        EnableMyPageFeature, 
        UseSSLOnAllPages, 
        DefaultPageKeywords, 
        DefaultPageDescription, 
        DefaultPageEncoding, 
        DefaultAdditionalMetaTags, 
        IsServerAdminSite, 
        AllowPageSkins, 
        AllowHideMenuOnPages, 
        UseLdapAuth, 
        AutoCreateLDAPUserOnFirstLogin, 
        LdapServer, 
        LdapPort, 
        LdapDomain, 
        LdapRootDN, 
        LdapUserDNKey, 
        AllowUserFullNameChange, 
        UseEmailForLogin, 
        ReallyDeleteUsers, 
        EditorProvider, 
        EditorSkin, 
        DatePickerProvider, 
        CaptchaProvider, 
        RecaptchaPrivateKey, 
        RecaptchaPublicKey, 
        WordpressAPIKey, 
        WindowsLiveAppID, 
        WindowsLiveKey, 
        AllowOpenIDAuth, 
        AllowWindowsLiveAuth, 
        GmapApiKey, 
        ApiKeyExtra1, 
        ApiKeyExtra2, 
        ApiKeyExtra3, 
        ApiKeyExtra4, 
        ApiKeyExtra5, 
        DisableDbAuth, 
        DefaultFriendlyUrlPatternEnum, 
        SiteGuid 
    )
VALUES (
    ?SiteName , 
    ?Skin , 
    ?Logo, 
    ?Icon, 
    ?AllowNewRegistration, 
    ?AllowUserSkins, 
    ?UseSecureRegistration, 
    ?EnableMyPageFeature, 
    ?UseSSLOnAllPages, 
    ?DefaultPageKeywords, 
    ?DefaultPageDescription, 
    ?DefaultPageEncoding, 
    ?DefaultAdditionalMetaTags, 
    ?IsServerAdminSite, 
    ?AllowPageSkins, 
    ?AllowHideMenuOnPages, 
    ?UseLdapAuth, 
    ?AutoCreateLDAPUserOnFirstLogin, 
    ?LdapServer, 
    ?LdapPort, 
    ?LdapDomain, 
    ?LdapRootDN, 
    ?LdapUserDNKey, 
    ?AllowUserFullNameChange, 
    ?UseEmailForLogin, 
    ?ReallyDeleteUsers, 
    ?EditorProvider, 
    ?EditorSkin, 
    ?DatePickerProvider, 
    ?CaptchaProvider, 
    ?RecaptchaPrivateKey, 
    ?RecaptchaPublicKey, 
    ?WordpressAPIKey, 
    ?WindowsLiveAppID, 
    ?WindowsLiveKey, 
    ?AllowOpenIDAuth, 
    ?AllowWindowsLiveAuth, 
    ?GmapApiKey, 
    ?ApiKeyExtra1, 
    ?ApiKeyExtra2, 
    ?ApiKeyExtra3, 
    ?ApiKeyExtra4, 
    ?ApiKeyExtra5, 
    ?DisableDbAuth, 
    ?DefaultFriendlyUrlPattern, 
    ?SiteGuid 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = siteName
			},

			new("?IsServerAdminSite", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = adminSite
			},

			new("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},

			new("?Logo", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = logo
			},

			new("?Icon", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?AllowNewRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNew
			},

			new("?AllowUserSkins", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowSkins
			},

			new("?UseSecureRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = secure
			},

			new("?EnableMyPageFeature", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = enableMy
			},

			new("?UseSSLOnAllPages", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ssl
			},

			new("?DefaultPageKeywords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageKeywords
			},

			new("?DefaultPageDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageDescription
			},

			new("?DefaultPageEncoding", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageEncoding
			},

			new("?DefaultAdditionalMetaTags", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultAdditionalMetaTags
			},

			new("?AllowPageSkins", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSkins
			},

			new("?AllowHideMenuOnPages", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowHide
			},

			new("?UseLdapAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uldapp
			},

			new("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = autoldapp
			},

			new("?LdapServer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapServer
			},

			new("?LdapPort", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ldapPort
			},

			new("?LdapRootDN", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapRootDN
			},

			new("?LdapUserDNKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapUserDNKey
			},

			new("?AllowUserFullNameChange", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNameChange
			},

			new("?UseEmailForLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = emailForLogin
			},

			new("?ReallyDeleteUsers", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = deleteUsers
			},

			new("?EditorSkin", MySqlDbType.VarChar, 50)
			{
					Direction = ParameterDirection.Input,
					Value = editorSkin
			},

			new("?DefaultFriendlyUrlPattern", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = defaultFriendlyUrlPattern
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?LdapDomain", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapDomain
			},

			new("?EditorProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = editorProvider
			},

			new("?DatePickerProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = datePickerProvider
			},

			new("?CaptchaProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = captchaProvider
			},

			new("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = recaptchaPrivateKey
			},

			new("?RecaptchaPublicKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = recaptchaPublicKey
			},

			new("?WordpressAPIKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = wordpressApiKey
			},

			new("?WindowsLiveAppID", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveAppId
			},

			new("?WindowsLiveKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveKey
			},

			new("?AllowOpenIDAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = oidauth
			},

			new("?AllowWindowsLiveAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = winliveauth
			},

			new("?GmapApiKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = gmapApiKey
			},

			new("?ApiKeyExtra1", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra1
			},

			new("?ApiKeyExtra2", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra2
			},

			new("?ApiKeyExtra3", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra3
			},

			new("?ApiKeyExtra4", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra4
			},

			new("?ApiKeyExtra5", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra5
			},

			new("?DisableDbAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intDisableDbAuth
			}
		};


		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams).ToString());

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

		#region bool conversion

		byte intDisableDbAuth = 0;
		if (disableDbAuth) { intDisableDbAuth = 1; }

		byte oidauth = 0;
		if (allowOpenIdAuth) { oidauth = 1; }


		byte winliveauth = 0;
		if (allowWindowsLiveAuth)
		{
			winliveauth = 1;
		}


		byte uldapp = 0;
		if (useLdapAuth)
		{
			uldapp = 1;
		}


		byte autoldapp = 0;
		if (autoCreateLdapUserOnFirstLogin)
		{
			autoldapp = 1;
		}


		byte allowNameChange = 0;
		if (allowUserFullNameChange)
		{
			allowNameChange = 1;
		}


		byte emailForLogin = 0;
		if (useEmailForLogin)
		{
			emailForLogin = 1;
		}


		byte deleteUsers = 0;
		if (reallyDeleteUsers)
		{
			deleteUsers = 1;
		}


		byte allowNew = 0;
		if (allowNewRegistration)
		{
			allowNew = 1;
		}


		byte allowSkins = 0;
		if (allowUserSkins)
		{
			allowSkins = 1;
		}


		byte secure = 0;
		if (useSecureRegistration)
		{
			secure = 1;
		}


		byte ssl = 0;
		if (useSslOnAllPages)
		{
			ssl = 1;
		}


		byte adminSite = 0;
		if (isServerAdminSite)
		{
			adminSite = 1;
		}


		byte pageSkins = 0;
		if (allowPageSkins)
		{
			pageSkins = 1;
		}

		byte allowHide = 0;
		if (allowHideMenuOnPages)
		{
			allowHide = 1;
		}


		byte enableMy = 0;
		if (enableMyPageFeature)
		{
			enableMy = 1;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_Sites 
SET 
    SiteName = ?SiteName, 
    IsServerAdminSite = ?IsServerAdminSite, 
    Skin = ?Skin, 
    Logo = ?Logo, 
    Icon = ?Icon, 
    AllowNewRegistration = ?AllowNewRegistration, 
    AllowUserSkins = ?AllowUserSkins, 
    AllowPageSkins = ?AllowPageSkins, 
    AllowHideMenuOnPages = ?AllowHideMenuOnPages, 
    UseSecureRegistration = ?UseSecureRegistration, 
    EnableMyPageFeature = ?EnableMyPageFeature, 
    UseSSLOnAllPages = ?UseSSLOnAllPages, 
    UseLdapAuth = ?UseLdapAuth, 
    AutoCreateLDAPUserOnFirstLogin = ?AutoCreateLDAPUserOnFirstLogin, 
    LdapServer = ?LdapServer, 
    LdapPort = ?LdapPort, 
    LdapDomain = ?LdapDomain, 
    LdapRootDN = ?LdapRootDN, 
    LdapUserDNKey = ?LdapUserDNKey, 
    AllowUserFullNameChange = ?AllowUserFullNameChange, 
    UseEmailForLogin = ?UseEmailForLogin, 
    ReallyDeleteUsers = ?ReallyDeleteUsers, 
    EditorSkin = ?EditorSkin, 
    EditorProvider = ?EditorProvider, 
    DefaultFriendlyUrlPatternEnum = ?DefaultFriendlyUrlPattern, 
    DatePickerProvider = ?DatePickerProvider, 
    CaptchaProvider = ?CaptchaProvider, 
    RecaptchaPrivateKey = ?RecaptchaPrivateKey, 
    RecaptchaPublicKey = ?RecaptchaPublicKey, 
    WordpressAPIKey = ?WordpressAPIKey, 
    WindowsLiveAppID = ?WindowsLiveAppID, 
    WindowsLiveKey = ?WindowsLiveKey, 
    AllowOpenIDAuth = ?AllowOpenIDAuth, 
    AllowWindowsLiveAuth = ?AllowWindowsLiveAuth, 
    GmapApiKey = ?GmapApiKey, 
    ApiKeyExtra1 = ?ApiKeyExtra1, 
    ApiKeyExtra2 = ?ApiKeyExtra2, 
    ApiKeyExtra3 = ?ApiKeyExtra3, 
    ApiKeyExtra4 = ?ApiKeyExtra4, 
    ApiKeyExtra5 = ?ApiKeyExtra5, 
    DisableDbAuth = ?DisableDbAuth, 
    DefaultPageKeywords = ?DefaultPageKeywords, 
    DefaultPageDescription = ?DefaultPageDescription, 
    DefaultPageEncoding = ?DefaultPageEncoding, 
    DefaultAdditionalMetaTags = ?DefaultAdditionalMetaTags 
WHERE 
    SiteID = ?SiteID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?SiteName", MySqlDbType.VarChar, 128)
			{
				Direction = ParameterDirection.Input,
				Value = siteName
			},

			new("?IsServerAdminSite", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = adminSite
			},

			new("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},

			new("?Logo", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = logo
			},

			new("?Icon", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = icon
			},

			new("?AllowNewRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNew
			},

			new("?AllowUserSkins", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowSkins
			},

			new("?UseSecureRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = secure
			},

			new("?EnableMyPageFeature", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = enableMy
			},

			new("?UseSSLOnAllPages", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ssl
			},

			new("?DefaultPageKeywords", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageKeywords
			},

			new("?DefaultPageDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageDescription
			},

			new("?DefaultPageEncoding", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultPageEncoding
			},

			new("?DefaultAdditionalMetaTags", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = defaultAdditionalMetaTags
			},

			new("?AllowPageSkins", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSkins
			},

			new("?AllowHideMenuOnPages", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowHide
			},

			new("?UseLdapAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uldapp
			},

			new("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = autoldapp
			},

			new("?LdapServer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapServer
			},

			new("?LdapPort", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ldapPort
			},

			new("?LdapRootDN", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapRootDN
			},

			new("?LdapUserDNKey", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = ldapUserDNKey
			},

			new("?AllowUserFullNameChange", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNameChange
			},

			new("?UseEmailForLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = emailForLogin
			},

			new("?ReallyDeleteUsers", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = deleteUsers
			},

			new("?EditorSkin", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = editorSkin
			},

			new("?DefaultFriendlyUrlPattern", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = defaultFriendlyUrlPattern
			},

			new("?LdapDomain", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapDomain
			},

			new("?EditorProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = editorProvider
			},

			new("?DatePickerProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = datePickerProvider
			},

			new("?CaptchaProvider", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = captchaProvider
			},

			new("?RecaptchaPrivateKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = recaptchaPrivateKey
			},

			new("?RecaptchaPublicKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = recaptchaPublicKey
			},

			new("?WordpressAPIKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = wordpressApiKey
			},

			new("?WindowsLiveAppID", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveAppId
			},

			new("?WindowsLiveKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveKey
			},

			new("?AllowOpenIDAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = oidauth
			},

			new("?AllowWindowsLiveAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = winliveauth
			},

			new("?GmapApiKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = gmapApiKey
			},

			new("?ApiKeyExtra1", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra1
			},

			new("?ApiKeyExtra2", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra2
			},

			new("?ApiKeyExtra3", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra3
			},

			new("?ApiKeyExtra4", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra4
			},

			new("?ApiKeyExtra5", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = apiKeyExtra5
			},

			new("?DisableDbAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intDisableDbAuth
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;
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

		#region bool conversion

		byte oidauth = 0;
		if (allowOpenIdAuth)
		{
			oidauth = 1;
		}


		byte winliveauth = 0;
		if (allowWindowsLiveAuth)
		{
			winliveauth = 1;
		}


		byte uldapp = 0;
		if (useLdapAuth)
		{
			uldapp = 1;
		}


		byte autoldapp = 0;
		if (autoCreateLdapUserOnFirstLogin)
		{
			autoldapp = 1;
		}


		byte allowNameChange = 0;
		if (allowUserFullNameChange)
		{
			allowNameChange = 1;
		}


		byte emailForLogin = 0;
		if (useEmailForLogin)
		{
			emailForLogin = 1;
		}

		byte allowNew = 0;
		if (allowNewRegistration)
		{
			allowNew = 1;
		}


		byte secure = 0;
		if (useSecureRegistration)
		{
			secure = 1;
		}

		int intAllowPasswordRetrieval = 0;
		if (allowPasswordRetrieval)
		{
			intAllowPasswordRetrieval = 1;
		}

		int intAllowPasswordReset = 0;
		if (allowPasswordReset)
		{
			intAllowPasswordReset = 1;
		}

		int intRequiresQuestionAndAnswer = 0;
		if (requiresQuestionAndAnswer)
		{
			intRequiresQuestionAndAnswer = 1;
		}

		int intRequiresUniqueEmail = 0;
		if (requiresUniqueEmail)
		{
			intRequiresUniqueEmail = 1;
		}

		#endregion

		string sqlCommand = @"
UPDATE mp_Sites 
SET  
    AllowNewRegistration = ?AllowNewRegistration, 
    UseSecureRegistration = ?UseSecureRegistration, 
    UseLdapAuth = ?UseLdapAuth, 
    AutoCreateLDAPUserOnFirstLogin = ?AutoCreateLDAPUserOnFirstLogin, 
    LdapServer = ?LdapServer, 
    LdapPort = ?LdapPort, 
    LdapDomain = ?LdapDomain, 
    LdapRootDN = ?LdapRootDN, 
    LdapUserDNKey = ?LdapUserDNKey, 
    AllowUserFullNameChange = ?AllowUserFullNameChange, 
    UseEmailForLogin = ?UseEmailForLogin, 
    AllowOpenIDAuth = ?AllowOpenIDAuth, 
    AllowWindowsLiveAuth = ?AllowWindowsLiveAuth, 
    AllowPasswordRetrieval = ?AllowPasswordRetrieval, 
    AllowPasswordReset = ?AllowPasswordReset, 
    RequiresQuestionAndAnswer = ?RequiresQuestionAndAnswer, 
    MaxInvalidPasswordAttempts = ?MaxInvalidPasswordAttempts, 
    PasswordAttemptWindowMinutes = ?PasswordAttemptWindowMinutes, 
    RequiresUniqueEmail = ?RequiresUniqueEmail, 
    PasswordFormat = ?PasswordFormat, 
    MinRequiredPasswordLength = ?MinRequiredPasswordLength, 
    MinReqNonAlphaChars = ?MinReqNonAlphaChars, 
    PwdStrengthRegex = ?PwdStrengthRegex 
WHERE 
    SiteID <> ?SiteID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?AllowNewRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNew
			},

			new("?UseSecureRegistration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = secure
			},

			new("?UseLdapAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = uldapp
			},

			new("?AutoCreateLDAPUserOnFirstLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = autoldapp
			},

			new("?LdapServer", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapServer
			},

			new("?LdapPort", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ldapPort
			},

			new("?LdapRootDN", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapRootDN
			},

			new("?LdapUserDNKey", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = ldapUserDNKey
			},

			new("?AllowUserFullNameChange", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowNameChange
			},

			new("?UseEmailForLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = emailForLogin
			},

			new("?LdapDomain", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ldapDomain
			},

			new("?AllowOpenIDAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = oidauth
			},

			new("?AllowWindowsLiveAuth", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = winliveauth
			},

			new("?AllowPasswordRetrieval", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowPasswordRetrieval
			},

			new("?AllowPasswordReset", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowPasswordReset
			},

			new("?RequiresQuestionAndAnswer", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequiresQuestionAndAnswer
			},

			new("?MaxInvalidPasswordAttempts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = maxInvalidPasswordAttempts
			},

			new("?PasswordAttemptWindowMinutes", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAttemptWindowMinutes
			},

			new("?RequiresUniqueEmail", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequiresUniqueEmail
			},

			new("?PasswordFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = passwordFormat
			},

			new("?MinRequiredPasswordLength", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minRequiredPasswordLength
			},

			new("?MinReqNonAlphaChars", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minReqNonAlphaChars
			},

			new("?PwdStrengthRegex", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = pwdStrengthRegex
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;


	}

	public static bool UpdateRelatedSitesWindowsLive(
		int siteId,
		string windowsLiveAppId,
		string windowsLiveKey
		)
	{
		string sqlCommand = @"
UPDATE 
    mp_Sites 
SET 
    WindowsLiveAppID = ?WindowsLiveAppID, 
    WindowsLiveKey = ?WindowsLiveKey 
WHERE 
    SiteID <> ?SiteID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?WindowsLiveAppID", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveAppId
			},

			new("?WindowsLiveKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = windowsLiveKey
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


		return rowsAffected > 0;

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
		#region bit conversion

		byte allowRetrieval;
		if (allowPasswordRetrieval)
		{
			allowRetrieval = 1;
		}
		else
		{
			allowRetrieval = 0;
		}

		byte allowReset;
		if (allowPasswordReset)
		{
			allowReset = 1;
		}
		else
		{
			allowReset = 0;
		}

		byte requiresQA;
		if (requiresQuestionAndAnswer)
		{
			requiresQA = 1;
		}
		else
		{
			requiresQA = 0;
		}

		byte requiresEmail;
		if (requiresUniqueEmail)
		{
			requiresEmail = 1;
		}
		else
		{
			requiresEmail = 0;
		}

		#endregion




		string sqlCommand = @"
UPDATE 
    mp_Sites 
SET 
    AllowPasswordRetrieval = ?AllowPasswordRetrieval, 
    AllowPasswordReset = ?AllowPasswordReset, 
    RequiresQuestionAndAnswer = ?RequiresQuestionAndAnswer, 
    MaxInvalidPasswordAttempts = ?MaxInvalidPasswordAttempts, 
    PasswordAttemptWindowMinutes = ?PasswordAttemptWindowMinutes, 
    RequiresUniqueEmail = ?RequiresUniqueEmail, 
    PasswordFormat = ?PasswordFormat, 
    MinRequiredPasswordLength = ?MinRequiredPasswordLength, 
    MinReqNonAlphaChars = ?MinRequiredNonAlphanumericCharacters, 
    PwdStrengthRegex = ?PasswordStrengthRegularExpression, 
    DefaultEmailFromAddress = ?DefaultEmailFromAddress 
WHERE 
    SiteID = ?SiteID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?AllowPasswordRetrieval", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowRetrieval
			},

			new("?AllowPasswordReset", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowReset
			},

			new("?RequiresQuestionAndAnswer", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = requiresQA
			},

			new("?MaxInvalidPasswordAttempts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = maxInvalidPasswordAttempts
			},

			new("?PasswordAttemptWindowMinutes", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = passwordAttemptWindowMinutes
			},

			new("?RequiresUniqueEmail", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = requiresEmail
			},

			new("?PasswordFormat", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = passwordFormat
			},

			new("?MinRequiredPasswordLength", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minRequiredPasswordLength
			},

			new("?PasswordStrengthRegularExpression", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = passwordStrengthRegularExpression
			},

			new("?DefaultEmailFromAddress", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = defaultEmailFromAddress
			},

			new("?MinRequiredNonAlphanumericCharacters", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = minRequiredNonAlphanumericCharacters
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool Delete(int siteId)
	{
		string sqlCommand = @"
DELETE FROM mp_WebParts WHERE SiteID = ?SiteID; 
DELETE FROM mp_PageModules 
WHERE PageID IN (
    SELECT PageID FROM mp_Pages 
    WHERE SiteID = ?SiteID
); 
DELETE FROM mp_ModuleSettings WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);   
DELETE FROM mp_HtmlContent WHERE ModuleID IN (
    SELECT ModuleID 
    FROM mp_Modules 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_Modules WHERE SiteID = ?SiteID; 
DELETE FROM mp_SiteModuleDefinitions WHERE SiteID = ?SiteID; 
DELETE FROM mp_UserProperties WHERE UserGuid IN (
    SELECT UserGuid 
    FROM mp_Users 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_UserRoles WHERE UserID IN (
    SELECT UserID 
    FROM mp_Users 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_UserLocation WHERE UserGuid IN (
    SELECT UserGuid 
    FROM mp_Users 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_FriendlyUrls WHERE SiteID = ?SiteID; 
DELETE FROM mp_UserPages WHERE SiteID = ?SiteID; 
DELETE FROM mp_Users WHERE SiteID = ?SiteID; 
DELETE FROM mp_Pages WHERE SiteID = ?SiteID; 
DELETE FROM mp_Roles WHERE SiteID = ?SiteID; 
DELETE FROM mp_SiteHosts WHERE SiteID = ?SiteID; 
DELETE FROM mp_SitePersonalizationAllUsers WHERE PathID IN (
    SELECT PathID 
    FROM mp_SitePaths 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_SitePersonalizationPerUser WHERE PathID IN (
    SELECT PathID 
    FROM mp_SitePaths 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_SitePaths WHERE SiteID = ?SiteID; 
DELETE FROM mp_SiteFolders WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_SiteSettingsEx WHERE SiteID = ?SiteID; 
DELETE FROM mp_LetterSendLog   
WHERE LetterGuid IN (
    SELECT LetterGuid 
    FROM mp_Letter   
    WHERE LetterInfoGuid 
    IN (
        SELECT LetterInfoGuid   
        FROM mp_LetterInfo   
        WHERE SiteGuid IN (
            SELECT SiteGuid 
            FROM mp_Sites WHERE SiteID = ?SiteID
        ) 
    )
);
DELETE FROM mp_LetterSubscribeHx   
WHERE LetterInfoGuid IN (
    SELECT LetterInfoGuid   
    FROM mp_LetterInfo   
    WHERE SiteGuid IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )   
);
DELETE FROM mp_LetterSubscribe  
WHERE LetterInfoGuid IN (
    SELECT LetterInfoGuid   
    FROM mp_LetterInfo   
    WHERE SiteGuid 
    IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )   
);
DELETE FROM mp_Letter   
WHERE LetterInfoGuid IN (
    SELECT LetterInfoGuid   
    FROM mp_LetterInfo   
    WHERE SiteGuid IN (
        SELECT SiteGuid 
        FROM mp_Sites 
        WHERE SiteID = ?SiteID
    )   
);
DELETE FROM mp_LetterHtmlTemplate WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_LetterInfo WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_PaymentLog WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_GoogleCheckoutLog WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_PayPalLog WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_RedirectList WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_TaskQueue WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
WHERE SiteID = ?SiteID
);
DELETE FROM mp_TaxClass WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_TaxRateHistory WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_TaxRate WHERE SiteGuid IN (
    SELECT SiteGuid 
    FROM mp_Sites 
    WHERE SiteID = ?SiteID
);
DELETE FROM mp_Sites 
WHERE SiteID = ?SiteID  ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static bool HasFeature(int siteId, int moduleDefId)
	{
		string sqlCommand = @"
SELECT Count(*) FROM mp_SiteModuleDefinitions 
WHERE SiteID = ?SiteID 
AND ModuleDefID = ?ModuleDefID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}

	public static void AddFeature(Guid siteGuid, Guid featureGuid)
	{
		int siteId = GetSiteIDFromGuid(siteGuid);
		int moduleDefId = DBModuleDefinition.GetModuleDefinitionIDFromGuid(featureGuid);

		if (HasFeature(siteId, moduleDefId)) return;

		string sqlCommand = @"
INSERT INTO mp_SiteModuleDefinitions ( 
    SiteID, 
    ModuleDefID, 
    SiteGuid, 
    FeatureGuid, 
    AuthorizedRoles 
) 
VALUES ( 
    ?SiteID, 
    ?ModuleDefID, 
    ?SiteGuid, 
    ?FeatureGuid, 
    'All Users' 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}

	public static void RemoveFeature(Guid siteGuid, Guid featureGuid)
	{
		int siteId = GetSiteIDFromGuid(siteGuid);
		int moduleDefId = DBModuleDefinition.GetModuleDefinitionIDFromGuid(featureGuid);


		string sqlCommand = @"
DELETE FROM mp_SiteModuleDefinitions 
WHERE SiteID = ?SiteID AND ModuleDefID = ?ModuleDefID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ModuleDefID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleDefId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader GetHostList(int siteId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SiteHosts 
WHERE SiteID = ?SiteID;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetHostList()
	{
		string sqlCommand = @"SELECT * FROM mp_SiteHosts order by HostName;";
		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand);
	}

	public static void AddHost(Guid siteGuid, int siteId, string hostName)
	{
		string sqlCommand = @"
INSERT INTO mp_SiteHosts ( 
    SiteID, 
    SiteGuid, 
    HostName 
) 
VALUES ( 
    ?SiteID, 
    ?SiteGuid, 
    ?HostName 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?HostName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostName
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);


	}

	public static void DeleteHost(int hostId)
	{
		string sqlCommand = @"
DELETE FROM mp_SiteHosts WHERE HostID = ?HostID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?HostID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = hostId
			}
		};

		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSiteList()
	{
		string sqlCommand = @"
SELECT * FROM	mp_Sites ORDER BY SiteName ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString());
	}

	public static int GetFirstSiteID()
	{
		string sqlCommand = @"
SELECT COALESCE(SiteID, -1) 
FROM mp_Sites 
ORDER BY SiteID LIMIT 1 ;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString()));
	}

	public static Guid GetSiteGuidFromID(int siteId)
	{
		string sqlCommand = @"
SELECT SiteGuid 
FROM mp_Sites 
WHERE SiteID = ?SiteID 
ORDER BY SiteName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		Guid siteGuid = Guid.Empty;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				siteGuid = new Guid(reader["SiteGuid"].ToString());
			}
		}

		return siteGuid;
	}

	public static int GetSiteIDFromGuid(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT SiteID 
FROM mp_Sites 
WHERE SiteGuid = ?SiteGuid 
ORDER BY SiteName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		int siteID = -1;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				siteID = Convert.ToInt32(reader["SiteID"].ToString(), CultureInfo.InvariantCulture);
			}
		}

		return siteID;
	}

	public static IDataReader GetSite(int siteId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Sites 
WHERE SiteID = ?SiteID 
ORDER BY SiteName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetSite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Sites 
WHERE SiteGuid = ?SiteGuid 
ORDER BY SiteName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}



	public static IDataReader GetSite(string hostName)
	{


		var arParams = new List<MySqlParameter>
		{
			new("?HostName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostName
			}
		};

		int siteId = -1;

		string sqlCommand = @"
SELECT mp_SiteHosts.SiteID 
FROM mp_SiteHosts 
WHERE mp_SiteHosts.HostName = ?HostName ;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				siteId = Convert.ToInt32(reader["SiteID"]);
			}
		}

		string sqlCommand1 = @"
SELECT * 
FROM	mp_Sites 
WHERE SiteID = ?SiteID OR ?SiteID = -1 
ORDER BY	SiteID 
LIMIT 1 ;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString(),
			arParams1);

	}


	public static IDataReader GetPageListForAdmin(int siteId)
	{
		string sqlCommand = @"
SELECT  
    PageID, 
    ParentID, 
    PageOrder, 
    PageName 
FROM 
    mp_Pages 
WHERE 
    SiteID = ?SiteID 
ORDER BY 
    ParentID, PageName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static int CountOtherSites(int currentSiteId)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_Sites 
WHERE SiteID <> ?CurrentSiteID 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?CurrentSiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = currentSiteId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPageOfOtherSites(
		int currentSiteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

		string sqlCommand = @"
SELECT * 
FROM mp_Sites  
WHERE SiteID <> ?CurrentSiteID 
ORDER BY SiteName  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?CurrentSiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = currentSiteId
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}


	public static int GetSiteIdByHostName(string hostName)
	{
		int siteId = -1;



		var arParams = new List<MySqlParameter>
		{
			new("?HostName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostName
			}
		};

		string sqlCommand = @"
SELECT SiteID 
FROM mp_SiteHosts 
WHERE HostName = ?HostName ;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				siteId = Convert.ToInt32(reader["SiteID"]);
			}
		}

		if (siteId == -1)
		{
			string sqlCommand1 = @"
SELECT SiteID 
FROM mp_Sites 
ORDER BY SiteID 
LIMIT 1 ;";

			using IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString());
			if (reader.Read())
			{
				siteId = Convert.ToInt32(reader["SiteID"]);
			}

		}

		return siteId;

	}

	public static int GetSiteIdByFolder(string folderName)
	{
		int siteId = -1;

		var arParams = new List<MySqlParameter>
		{
			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			}
		};

		string sqlCommand = @"
SELECT COALESCE(s.SiteID, -1) AS SiteID 
FROM mp_SiteFolders sf 
JOIN mp_Sites s 
ON 
sf.SiteGuid = s.SiteGuid 
WHERE sf.FolderName = ?FolderName 
ORDER BY s.SiteID 
;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				siteId = Convert.ToInt32(reader["SiteID"]);
			}
		}

		if (siteId == -1)
		{
			string sqlCommand1 = @"
SELECT SiteID 
FROM mp_Sites 
ORDER BY SiteID 
LIMIT 1 ;";

			using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand1.ToString()))
			{
				if (reader.Read())
				{
					siteId = Convert.ToInt32(reader["SiteID"]);
				}
			}

		}

		return siteId;

	}

	public static bool HostNameExists(string hostName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SiteHosts 
WHERE HostName = ?HostName ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?HostName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = hostName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}

	public static void UpdateSkinVersionGuidForAllSites()
	{
		var sqlCommand = $@"UPDATE mp_SiteSettingsEx
				SET KeyValue = ?NewGuid
				WHERE KeyName = 'SkinVersion'
				AND GroupName = 'Settings';";

		List<MySqlParameter> sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?NewGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = Guid.NewGuid().ToString()
				}
			};

		CommandHelper.ExecuteScalar(
			ConnectionString.GetWrite(),
			sqlCommand,
			sqlParams.ToArray());
	}

}
