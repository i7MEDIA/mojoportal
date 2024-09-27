using System;
using System.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace mojoPortal.Web;

public static class WebConfigSettings
{
	public static bool UseSiteIdAppThemesInMediumTrust => ConfigHelper.GetBoolProperty("UseSiteIdAppThemesInMediumTrust", false);

	public static bool EnableOpenIdAuthentication => ConfigHelper.GetBoolProperty("EnableOpenIDAuthentication", true);

	public static bool DisableRpxAuthentication => ConfigHelper.GetBoolProperty("DisableRpxAuthentication", false);

	public static bool ShowLegacyOpenIDSelector => ConfigHelper.GetBoolProperty("ShowLegacyOpenIDSelector", false);

	public static bool XmlUseMediaFolder => ConfigHelper.GetBoolProperty("XMLUseMediaFolder", false);

	public static bool SiteLogoUseMediaFolder => ConfigHelper.GetBoolProperty("SiteLogoUseMediaFolder", false);

	public static bool HtmlFragmentUseMediaFolder => ConfigHelper.GetBoolProperty("HtmlFragmentUseMediaFolder", false);

	public static bool DisableGoogleTranslate => ConfigHelper.GetBoolProperty("DisableGoogleTranslate", false);

	public static bool UseOpenIdRpxSettingsFromWebConfig => ConfigHelper.GetBoolProperty("UseOpenIdRpxSettingsFromWebConfig", false);

	/// <summary>
	/// If true then we will store our userGuid in the rpx server and allow multiple open id identifiers to be used
	/// with a single mojoportal account
	/// </summary>
	public static bool OpenIdRpxUseMappings => ConfigHelper.GetBoolProperty("OpenIdRpxUseMappings", true);

	// 2013-05-26 changed from true to false not sure why it was true
	public static bool OpenIdRpxUseOldImplementation => ConfigHelper.GetBoolProperty("OpenIdRpxUseOldImplementation", false);

	public static string OpenIdRpxApiKey => ConfigHelper.GetStringProperty("OpenIdRpxApiKey", "");

	/// <summary>
	/// to support proxy servers where the client ip may be passed in a custom server variable
	/// </summary>
	public static string ClientIpServerVariable => ConfigHelper.GetStringProperty("ClientIpServerVariable", "");

	/// <summary>
	/// to suport proxy servers which may use non standard variables
	/// http://www.mojoportal.com/Forums/Thread.aspx?thread=7424&mid=34&pageid=5&ItemID=5&pagenumber=1#post30680
	/// </summary>
	public static string RemoteHostServerVariable => ConfigHelper.GetStringProperty("RemoteHostServerVariable", "REMOTE_HOST");

	public static string OpenIdRpxApplicationName => ConfigHelper.GetStringProperty("OpenIdRpxApplicationName", "");

	public static bool EnableWindowsLiveAuthentication => ConfigHelper.GetBoolProperty("EnableWindowsLiveAuthentication", false);

	public static bool HideDisableDbAuthenticationSetting => ConfigHelper.GetBoolProperty("HideDisableDbAuthenticationSetting", false);

	public static bool GloballyDisableMemberUseOfWindowsLiveMessenger => ConfigHelper.GetBoolProperty("GloballyDisableMemberUseOfWindowsLiveMessenger", true);

	public static bool TestLiveMessengerDelegation => ConfigHelper.GetBoolProperty("TestLiveMessengerDelegation", false);

	public static bool DebugWindowsLive => ConfigHelper.GetBoolProperty("DebugWindowsLive", false);

	public static bool DebugLoginRedirect => ConfigHelper.GetBoolProperty("DebugLoginRedirect", false);

	public static bool EnableTaskQueueTestLinks => ConfigHelper.GetBoolProperty("EnableTaskQueueTestLinks", false);

	public static bool DisableSetup => ConfigHelper.GetBoolProperty("DisableSetup", false);

	public static string ContentPagesToSkip => ConfigHelper.GetStringProperty("ContentPagesToSkip", string.Empty);

	public static string PaymentGatewayConfigFileName => ConfigHelper.GetStringProperty("PaymentGatewayConfigFileName", "mojoPaymentGateway.config");

	/// <summary>
	/// Allows using one set of Commerce Settings on all tenants
	/// </summary>
	public static bool CommerceUseGlobalSettings => ConfigHelper.GetBoolProperty("CommerceUseGlobalSettings", false);

	public static bool CommerceGlobalIs503TaxExempt => ConfigHelper.GetBoolProperty("CommerceGlobalIs503TaxExempt", false);

	public static bool CommerceGlobalUseTestMode => ConfigHelper.GetBoolProperty("CommerceGlobalUseTestMode", false);

	public static string CommerceGlobalPrimaryGateway => ConfigHelper.GetStringProperty("CommerceGlobalPrimaryGateway", string.Empty);

	public static string CommerceGlobalAuthorizeNetProductionAPILogin => ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetProductionAPILogin", string.Empty);

	public static string CommerceGlobalAuthorizeNetProductionAPITransactionKey => ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetProductionAPITransactionKey", string.Empty);

	public static string CommerceGlobalAuthorizeNetSandboxAPILogin => ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetSandboxAPILogin", string.Empty);

	public static string CommerceGlobalAuthorizeNetSandboxAPITransactionKey => ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetSandboxAPITransactionKey", string.Empty);

	public static string CommerceGlobalPlugNPayProductionAPIPublisherName => ConfigHelper.GetStringProperty("CommerceGlobalPlugNPayProductionAPIPublisherName", string.Empty);

	public static string CommerceGlobalPlugNPayProductionAPIPublisherPassword => ConfigHelper.GetStringProperty("CommerceGlobalPlugNPayProductionAPIPublisherPassword", string.Empty);

	public static string CommerceGlobalPlugNPaySandboxAPIPublisherName => ConfigHelper.GetStringProperty("CommerceGlobalPlugNPaySandboxAPIPublisherName", string.Empty);

	public static string CommerceGlobalPlugNPaySandboxAPIPublisherPassword => ConfigHelper.GetStringProperty("CommerceGlobalPlugNPaySandboxAPIPublisherPassword", string.Empty);

	public static string CommerceGlobalPayPalAccountProductionEmailAddress => ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountProductionEmailAddress", string.Empty);

	public static string CommerceGlobalPayPalAccountProductionPDTId => ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountProductionPDTId", string.Empty);

	public static string CommerceGlobalPayPalAccountSandboxEmailAddress => ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountSandboxEmailAddress", string.Empty);

	public static string CommerceGlobalPayPalAccountSandboxPDTId => ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountSandboxPDTId", string.Empty);

	public static bool CommerceGlobalUsePayPalStandard => ConfigHelper.GetBoolProperty("CommerceGlobalUsePayPalStandard", true);

	public static string CommerceGlobalPayPalStandardProductionUrl => ConfigHelper.GetStringProperty("CommerceGlobalPayPalStandardProductionUrl", string.Empty);

	public static string CommerceGlobalPayPalStandardSandboxUrl => ConfigHelper.GetStringProperty("CommerceGlobalPayPalStandardSandboxUrl", string.Empty);

	public static string CommerceGlobalPayPalProductionAPIUsername => ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPIUsername", string.Empty);

	public static string CommerceGlobalPayPalProductionAPIPassword => ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPIPassword", string.Empty);

	public static string CommerceGlobalPayPalProductionAPISignature => ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPISignature", string.Empty);

	public static string CommerceGlobalPayPalSandboxAPIUsername => ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPIUsername", string.Empty);

	public static string CommerceGlobalPayPalSandboxAPIPassword => ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPIPassword", string.Empty);

	public static string CommerceGlobalPayPalSandboxAPISignature => ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPISignature", string.Empty);

	public static string CommerceGlobalGoogleProductionMerchantID => ConfigHelper.GetStringProperty("CommerceGlobalGoogleProductionMerchantID", string.Empty);

	public static string CommerceGlobalGoogleProductionMerchantKey => ConfigHelper.GetStringProperty("CommerceGlobalGoogleProductionMerchantKey", string.Empty);

	public static string CommerceGlobalGoogleSandboxMerchantID => ConfigHelper.GetStringProperty("CommerceGlobalGoogleSandboxMerchantID", string.Empty);

	public static string CommerceGlobalGoogleSandboxMerchantKey => ConfigHelper.GetStringProperty("CommerceGlobalGoogleSandboxMerchantKey", string.Empty);

	public static string CommerceGlobalWorldPayInstallationId => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayInstallationId", string.Empty);

	public static string CommerceGlobalWorldPayMerchantCode => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayMerchantCode", string.Empty);

	public static string CommerceGlobalWorldPayMd5Secret => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayMd5Secret", string.Empty);

	public static string CommerceGlobalWorldPayResponsePassword => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayResponsePassword", string.Empty);

	public static string CommerceGlobalWorldPayShopperResponseTemplate => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayShopperResponseTemplate", string.Empty);

	public static string CommerceGlobalWorldPayShopperCancellationResponseTemplate => ConfigHelper.GetStringProperty("CommerceGlobalWorldPayShopperCancellationResponseTemplate", string.Empty);

	public static bool CommerceGlobalWorldPayProduceShopperResponse => ConfigHelper.GetBoolProperty("CommerceGlobalWorldPayProduceShopperResponse", true);

	public static bool CommerceGlobalWorldPayProduceShopperCancellationResponse => ConfigHelper.GetBoolProperty("CommerceGlobalWorldPayProduceShopperCancellationResponse", true);

	public static bool SetupTryAnywayIfFailedAlterSchemaTest => ConfigHelper.GetBoolProperty("SetupTryAnywayIfFailedAlterSchemaTest", false);

	public static bool MaskPasswordsInUserAdmin => ConfigHelper.GetBoolProperty("MaskPasswordsInUserAdmin", true);

	public static string MemberListUrl => ConfigHelper.GetStringProperty("MemberListUrl", "/MemberList.aspx");

	public static string MemberListOverrideLinkText => ConfigHelper.GetStringProperty("MemberListOverrideLinkText", string.Empty);

	public static bool ShowEmailInMemberList => ConfigHelper.GetBoolProperty("ShowEmailInMemberList", false);

	public static bool ShowPurgeUserLocationsInUserManagement => ConfigHelper.GetBoolProperty("ShowPurgeUserLocationsInUserManagement", true);

	public static bool ShowForumUnsubscribeLinkInUserManagement => ConfigHelper.GetBoolProperty("ShowForumUnsubscribeLinkInUserManagement", true);

	public static bool ShowRevenueInForums => ConfigHelper.GetBoolProperty("ShowRevenueInForums", false);

	public static bool GetAlphaPagerCharsFromResourceFile => ConfigHelper.GetBoolProperty("GetAlphaPagerCharsFromResourceFile", false);

	public static string AlphaPagerChars => ConfigHelper.GetStringProperty("AlphaPagerChars", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

	public static bool AdaptHtmlDirectionToCulture => ConfigHelper.GetBoolProperty("AdaptHtmlDirectionToCulture", false);

	public static bool AddLangToHtmlElement => ConfigHelper.GetBoolProperty("AddLangToHtmlElement", true);

	/*
	 * Menu hiding options. As of version 2.9.1, these are in the skin/config/config.json file
	 * control used in the theme.skin.
	 */
	public static bool HideAllMenusOnSiteClosedPage => ConfigHelper.GetBoolProperty("HideAllMenusOnSiteClosedPage", false);

	public static bool HideMenusOnLoginPage => ConfigHelper.GetBoolProperty("HideMenusOnLoginPage", false);

	public static bool HideMenusOnSiteMap => ConfigHelper.GetBoolProperty("HideMenusOnSiteMap", false);

	public static bool HidePageMenusOnSiteMap => ConfigHelper.GetBoolProperty("HidePageMenusOnSiteMap", true);

	public static bool HideMenusOnRegisterPage => ConfigHelper.GetBoolProperty("HideMenusOnRegisterPage", false);

	public static bool HideMenusOnPasswordRecoveryPage => ConfigHelper.GetBoolProperty("HideMenusOnPasswordRecoveryPage", false);

	public static bool HideMenusOnChangePasswordPage => ConfigHelper.GetBoolProperty("HideMenusOnChangePasswordPage", false);

	public static bool HideAllMenusOnProfilePage => ConfigHelper.GetBoolProperty("HideAllMenusOnProfilePage", false);

	public static bool HidePageMenuOnProfilePage => ConfigHelper.GetBoolProperty("HidePageMenuOnProfilePage", true);

	public static bool HidePageMenuOnMemberListPage => ConfigHelper.GetBoolProperty("HidePageMenuOnMemberListPage", true);

	public static bool HidePageViewModeIfNoWorkflowItems => ConfigHelper.GetBoolProperty("HidePageViewModeIfNoWorkflowItems", true);

	public static bool SuppressMenuOnBuiltIn404Page => ConfigHelper.GetBoolProperty("SuppressMenuOnBuiltIn404Page", false);
	public static bool ShowForumPostsInMemberList => ConfigHelper.GetBoolProperty("ShowForumPostsInMemberList", true);

	public static bool DisableLoginInfo => ConfigHelper.GetBoolProperty("DisableLoginInfo", false);

	public static bool ShowLoginNameInMemberList => ConfigHelper.GetBoolProperty("ShowLoginNameInMemberList", false);

	public static bool ShowUserIDInMemberList => ConfigHelper.GetBoolProperty("ShowUserIDInMemberList", false);

	public static bool ShowLeftColumnOnSearchResults => ConfigHelper.GetBoolProperty("ShowLeftColumnOnSearchResults", false);

	public static bool ShowSkinSearchInputOnSearchResults => ConfigHelper.GetBoolProperty("ShowSkinSearchInputOnSearchResults", false);

	public static bool ShowSearchInputOnSiteSettings => ConfigHelper.GetBoolProperty("ShowSearchInputOnSiteSettings", false);

	public static bool ShowRightColumnOnSearchResults => ConfigHelper.GetBoolProperty("ShowRightColumnOnSearchResults", false);

	public static bool ShowModuleTitlesByDefault => ConfigHelper.GetBoolProperty("ShowModuleTitlesByDefault", true);

	public static bool EnableEditingModuleTitleElement => ConfigHelper.GetBoolProperty("EnableEditingModuleTitleElement", false);

	public static string ModuleTitleTag => ConfigHelper.GetStringProperty("ModuleTitleTag", "h2");

	public static bool EnableDeveloperMenuInAdminMenu => ConfigHelper.GetBoolProperty("EnableDeveloperMenuInAdminMenu", false);

	public static int UserAutoCompleteRowsToGet => ConfigHelper.GetIntProperty("UserAutoCompleteRowsToGet", 10);

	public static bool EnableQueryTool => ConfigHelper.GetBoolProperty("EnableQueryTool", false);

	public static string QueryToolOverrideConnectionString => ConfigHelper.GetStringProperty("QueryToolOverrideConnectionString", "");

	public static bool TryToCreateMsSqlDatabase => ConfigHelper.GetBoolProperty("TryToCreateMsSqlDatabase", false);

	public static string QueryToolMsSqlTableSelectSql => ConfigHelper.GetStringProperty("QueryToolMsSqlTableSelectSql", "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE OBJECTPROPERTY(object_id(TABLE_NAME), N'IsUserTable') = 1 ORDER BY TABLE_NAME");

	public static string QueryToolMySqlTableSelectSql => ConfigHelper.GetStringProperty("QueryToolMySqlTableSelectSql", "SELECT DISTINCT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA <> 'mysql' ORDER BY TABLE_NAME;");

	public static string QueryToolPgSqlTableSelectSql => ConfigHelper.GetStringProperty("QueryToolPgSqlTableSelectSql", "SELECT table_name AS TableName FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;");

	public static string QueryToolSqliteTableSelectSql => ConfigHelper.GetStringProperty("QueryToolSqliteTableSelectSql", "SELECT Name As TableName FROM sqlite_master WHERE type = 'table' ORDER BY name;");

	public static bool EnableLogViewer => ConfigHelper.GetBoolProperty("EnableLogViewer", true);

	public static bool UseCultureOverride => ConfigHelper.GetBoolProperty("UseCultureOverride", false);

	public static bool SetUICultureWhenSettingCulture => ConfigHelper.GetBoolProperty("SetUICultureWhenSettingCulture", true);

	public static bool UseCultureForUICulture => ConfigHelper.GetBoolProperty("UseCultureForUICulture", true);

	public static bool UseCustomHandlingForPersianCulture => ConfigHelper.GetBoolProperty("UseCustomHandlingForPersianCulture", false);

	//this fixes some ajax updatepanel issues in webkit
	//http://forums.asp.net/p/1252014/2392110.aspx
	public static bool UseSafariWebKitHack => ConfigHelper.GetBoolProperty("UseSafariWebKitHack", false);

	public static bool UseAjaxFormActionUpdateScript => ConfigHelper.GetBoolProperty("UseAjaxFormActionUpdateScript", true);

	public static string CKEditorSkin => ConfigHelper.GetStringProperty("CKEditor:Skin", "moono-lisa");

	//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12326~1#post51248
	public static bool CKeditorSuppressTitle => ConfigHelper.GetBoolProperty("CKEditor:SuppressTitle", true);

	public static bool CKeditorEncodeBrackets => ConfigHelper.GetBoolProperty("CKeditor:EncodeBrackets", false);

	public static bool UseSkinCssInEditor => ConfigHelper.GetBoolProperty("UseSkinCssInEditor", true);

	/// <summary>
	/// if you populate this setting it should start with a comma since it
	/// will be appended to the skin url which is assigned by default
	/// </summary>
	public static string EditorExtraCssUrlCsv => ConfigHelper.GetStringProperty("EditorExtraCssUrlCsv", string.Empty);

	public static bool DisableTinyMceInlineEditing => ConfigHelper.GetBoolProperty("DisableTinyMceInlineEditing", false);

	public static bool TinyMceInlineSaveOnBlur => ConfigHelper.GetBoolProperty("TinyMceInlineSaveOnBlur", true);

	public static bool TinyMceInlineRemoveAutoSavePlugin => ConfigHelper.GetBoolProperty("TinyMceInlineRemoveAutoSavePlugin", true);

	public static bool TinyMceInlineUseSavePlugin => ConfigHelper.GetBoolProperty("TinyMceInlineUseSavePlugin", false);

	public static string TinyMceSchema => ConfigHelper.GetStringProperty("TinyMCE:Schema", "html5");

	public static string TinyMceBasePath => ConfigHelper.GetStringProperty("TinyMCE:BasePath", "/ClientScript/tinymce641/");

	public static string TinyMceSkin => ConfigHelper.GetStringProperty("TinyMCE:Skin", "default");

	public static string UnobtrusiveValidationMode => ConfigHelper.GetStringProperty("ValidationSettings:UnobtrusiveValidationMode", string.Empty);

	public static bool ForceEmptyJQueryScriptReference => ConfigHelper.GetBoolProperty("ForceEmptyJQueryScriptReference", true);

	public static string jQueryUIAvailableThemes => ConfigHelper.GetStringProperty("jQueryUIAvailableThemes", string.Empty);

	public static bool UseHtml5 => ConfigHelper.GetBoolProperty("UseHtml5", false);

	public static bool DisableViewStateOnSiteMapDataSource => ConfigHelper.GetBoolProperty("DisableViewStateOnSiteMapDataSource", true);

	public static int ChannelFileCacheInDays => ConfigHelper.GetIntProperty("ChannelFileCacheInDays", 365);

	public static bool CacheTimeZoneList => ConfigHelper.GetBoolProperty("CacheTimeZoneList", true);

	public static bool DisableASPThemes => ConfigHelper.GetBoolProperty("DisableASPThemes", false);

	public static bool AllowChangingFriendlyUrlPattern => ConfigHelper.GetBoolProperty("AllowChangingFriendlyUrlPattern", true);

	public static string FriendlyUrlSuggestScript => ConfigHelper.GetStringProperty("FriendlyUrlSuggestScript", "~/ClientScript/friendlyurlsuggest_v3.js");

	public static bool AllowDirectEntryOfUserIdForEditPermission => ConfigHelper.GetBoolProperty("AllowDirectEntryOfUserIdForEditPermission", false);

	public static bool AllowMultipleSites => ConfigHelper.GetBoolProperty("AllowMultipleSites", true);

	public static bool AppendDefaultPageToFolderRootUrl => ConfigHelper.GetBoolProperty("AppendDefaultPageToFolderRootUrl", true);

	public static bool ShowSiteGuidInSiteSettings => ConfigHelper.GetBoolProperty("ShowSiteGuidInSiteSettings", false);

	public static bool ShowSiteIdInSiteList => ConfigHelper.GetBoolProperty("ShowSiteIdInSiteList", true);

	public static int SiteListPageSize => ConfigHelper.GetIntProperty("SiteListPageSize", 30);

	public static int RoleMemberPageSize => ConfigHelper.GetIntProperty("RoleMemberPageSize", 30);

	public static bool EnableSiteSettingsSmtpSettings => ConfigHelper.GetBoolProperty("EnableSiteSettingsSmtpSettings", true);

	public static bool EnforceContentVersioningGlobally => ConfigHelper.GetBoolProperty("EnforceContentVersioningGlobally", false);

	public static bool MaskSmtpPasswordInSiteSettings => ConfigHelper.GetBoolProperty("MaskSmtpPasswordInSiteSettings", true);

	public static bool ShowSmtpEncodingOption => ConfigHelper.GetBoolProperty("ShowSmtpEncodingOption", false);

	public static bool HideGoogleAnalyticsInChildSites => ConfigHelper.GetBoolProperty("HideGoogleAnalyticsInChildSites", false);

	public static string GravatarMaxAllowedRating => ConfigHelper.GetStringProperty("GravatarMaxAllowedRating", "G");

	public static bool ProxyPreventsSSLDetection => ConfigHelper.GetBoolProperty("ProxyPreventsSSLDetection", false);

	public static bool RedirectSslWith301Status => ConfigHelper.GetBoolProperty("RedirectSslWith301Status", false);

	public static bool IsDemoSite => ConfigHelper.GetBoolProperty("IsDemoSite", false);
	/// <summary>
	/// Used to track people using our demo site who try to DOS (denial of service) our demo site by deleting all the pages
	/// We want to ban those ip addresses
	/// </summary>
	public static bool LogIpAddressForContentDeletions => ConfigHelper.GetBoolProperty("LogIpAddressForContentDeletions", false);

	/// <summary>
	/// Used to track users who try to DOS our demo site by changing the admin password so other users cannnot use it
	/// </summary>
	public static bool LogIpAddressForPasswordChanges => ConfigHelper.GetBoolProperty("LogIpAddressForPasswordChanges", false);

	/// <summary>
	/// Used to track users who try to DOS our demo site by changing the admin password so other users cannnot use it
	/// </summary>
	public static bool LogIpAddressForEmailChanges => ConfigHelper.GetBoolProperty("LogIpAddressForEmailChanges", false);

	public static bool LogFailedLoginAttempts => ConfigHelper.GetBoolProperty("LogFailedLoginAttempts", false);

	public static bool LogAllFileServiceRequests => ConfigHelper.GetBoolProperty("LogAllFileServiceRequests", false);

	public static bool LogBlockedRequests => ConfigHelper.GetBoolProperty("LogBlockedRequests", true);

	public static bool FileServiceRejectFishyPosts => ConfigHelper.GetBoolProperty("FileServiceRejectFishyPosts", true);

	public static bool LogCacheActivity => ConfigHelper.GetBoolProperty("LogCacheActivity", false);

	public static bool LogXmlRpcRequests => ConfigHelper.GetBoolProperty("LogXmlRpcRequests", false);

	public static bool LogNewsletterSubscriptions => ConfigHelper.GetBoolProperty("LogNewsletterSubscriptions", false);

	public static bool LogFullUrls => ConfigHelper.GetBoolProperty("LogFullUrls", false);

	public static bool UseSystemLogInsteadOfFileLog => ConfigHelper.GetBoolProperty("UseSystemLogInsteadOfFileLog", false);

	public static bool ShowFileLogInAdditionToSystemLog => ConfigHelper.GetBoolProperty("ShowFileLogInAdditionToSystemLog", false);

	public static int SystemLogPageSize => ConfigHelper.GetIntProperty("SystemLogPageSize", 30);

	public static bool SystemLogSortAscending => ConfigHelper.GetBoolProperty("SystemLogSortAscending", false);

	public static string SystemLogDateTimeFormat => ConfigHelper.GetStringProperty("SystemLogDateTimeFormat", "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff");

	public static bool SystemLogDeleteOldEventsOnApplicationStart => ConfigHelper.GetBoolProperty("SystemLogDeleteOldEventsOnApplicationStart", true);

	public static int SystemLogApplicationStartDeleteOlderThanDays => ConfigHelper.GetIntProperty("SystemLogApplicationStartDeleteOlderThanDays", 10);

	public static bool AllowDeletingChildSites => ConfigHelper.GetBoolProperty("AllowDeletingChildSites", false);

	public static bool DeleteSiteFolderWhenDeletingSites => ConfigHelper.GetBoolProperty("DeleteSiteFolderWhenDeletingSites", false);

	public static bool ShowSkinRestoreButtonInSiteSettings => ConfigHelper.GetBoolProperty("ShowSkinRestoreButtonInSiteSettings", true);
	public static bool ShowCopyNewSkinsButtonInSiteSettings => ConfigHelper.GetBoolProperty("ShowCopyNewSkinsButtonInSiteSettings", true);

	public static bool AllowFileManagerInChildSites => ConfigHelper.GetBoolProperty("AllowFileManagerInChildSites", false);

	public static bool AllowClosingSites => ConfigHelper.GetBoolProperty("AllowClosingSites", true);

	public static string RolesThatCanAccessClosedSites => ConfigHelper.GetStringProperty("RolesThatCanAccessClosedSites", "Admins;Content Administrators;");

	public static string RolesThatAlwaysViewMobileContent => ConfigHelper.GetStringProperty("RolesThatAlwaysViewMobileContent", string.Empty);

	public static string ClosedPageUrl => ConfigHelper.GetStringProperty("ClosedPageUrl", "/closed.aspx");

	public static string UserNameValidationExpression => ConfigHelper.GetStringProperty("UserNameValidationExpression", string.Empty);

	public static string UserNameValidationWarning => ConfigHelper.GetStringProperty("UserNameValidationWarning", string.Empty);

	/// <summary>
	/// for backward compatibility this is true but for new installations this is false in the user.config.sample file so it uses the newer method
	/// main problem with the legacy solution is that it doe snot work in medium trust hosting
	/// </summary>
	public static bool UseLegacyCryptoHelper => ConfigHelper.GetBoolProperty("UseLegacyCryptoHelper", true);

	public static bool AllowUserProfilePage => ConfigHelper.GetBoolProperty("AllowUserProfilePage", true);

	public static string PrivateProfileRelativeUrl => ConfigHelper.GetStringProperty("PrivateProfileRelativeUrl", "/Secure/UserProfile.aspx");

	public static string PublicProfileRelativeUrl => ConfigHelper.GetStringProperty("PublicProfileRelativeUrl", "/ProfileView.aspx");
	public static bool AllowPasswordFormatChange => ConfigHelper.GetBoolProperty("AllowPasswordFormatChange", true);

	public static bool AllowCaseInsensitivePasswordQuestionAnswer => ConfigHelper.GetBoolProperty("AllowCaseInsensitivePasswordQuestionAnswer", false);

	/// <summary>
	/// this is mainly used so I can disable it on the demo site
	/// </summary>
	public static bool AllowRequiringPasswordChange => ConfigHelper.GetBoolProperty("AllowRequiringPasswordChange", true);

	public static bool LockAccountOnMaxPasswordAnswerTries => ConfigHelper.GetBoolProperty("LockAccountOnMaxPasswordAnswerTries", false);

	public static bool DisableAutoCompleteOnLogin => ConfigHelper.GetBoolProperty("DisableAutoCompleteOnLogin", false);


	/// <summary>
	/// List of semi-colon separated numeric values used to determine the weighting of a strength characteristic. There must be 4 values specified which must total 100. 
	/// The default weighting values are defined as 50;15;15;20. This corresponds to password length is 50% of the strength calculation, 
	/// Numeric criteria is 15% of strength calculation, casing criteria is 15% of calculation, and symbol criteria is 20% of calculation. 
	/// So the format is 'A;B;C;D' where A = length weighting, B = numeric weighting, C = casing weighting, D = symbol weighting.
	/// </summary>
	public static string PasswordStrengthCalculationWeightings => ConfigHelper.GetStringProperty("PasswordStrengthCalculationWeightings", "25;25;25;25");

	/// <summary>
	/// valid options are RightSide, LeftSide, AboveLeft, AboveRight, BelowLeft, BelowRight
	/// </summary>
	public static string PasswordStrengthDisplayPosition => ConfigHelper.GetStringProperty("PasswordStrengthDisplayPosition", "RightSide");

	/// <summary>
	/// valid options are BarIndicator and Text
	/// </summary>
	public static string PasswordStrengthIndicatorType => ConfigHelper.GetStringProperty("PasswordStrengthIndicatorType", "Text");

	public static int PasswordStrengthMinimumLowerCaseCharacters => ConfigHelper.GetIntProperty("PasswordStrengthMinimumLowerCaseCharacters", 1);

	public static int PasswordStrengthMinimumUpperCaseCharacters => ConfigHelper.GetIntProperty("PasswordStrengthMinimumUpperCaseCharacters", 1);

	public static int MinUserNameLength => ConfigHelper.GetIntProperty("MinUserNameLength", 6);

	public static bool PreEncryptRolesForCookie => ConfigHelper.GetBoolProperty("PreEncryptRolesForCookie", false);

	public static bool Return404StatusForCryptoError => ConfigHelper.GetBoolProperty("Return404StatusForCryptoError", true);

	/// <summary>
	/// 0 = clear, 1= hashed, 2= encrypted
	/// </summary>
	public static int InitialSitePasswordFormat
		//changed default to hashed 2009-02-25
		// changed default to encrypted 2009-05-08
		//http://www.mojoportal.com/Forums/Thread.aspx?thread=2902&mid=34&pageid=5&ItemID=5&pagenumber=1
		// changed back to clear text 2009-05-25 because of too many support requests where people end up
		// getting the admin user locked out
		=> ConfigHelper.GetIntProperty("InitialSitePasswordFormat", 0);

	public static bool AllowPasswordFormatChangeInChildSites => ConfigHelper.GetBoolProperty("AllowPasswordFormatChangeInChildSites", false);

	public static bool CheckMD5PasswordHashAsFallback => ConfigHelper.GetBoolProperty("CheckMD5PasswordHashAsFallback", true);

	public static bool CheckAllPasswordFormatsOnAuthFailure => ConfigHelper.GetBoolProperty("CheckAllPasswordFormatsOnAuthFailure", false);

	/// <summary>
	/// we default this to false because we already handle locked users in SiteLogin.cs by cancellinng and showing a specific message indicating the account is locked
	/// whereas if we just return fale from membership provider the message will be more generic authentication failed message
	/// </summary>
	public static bool ReturnFalseInValidateUserIfAccountLocked => ConfigHelper.GetBoolProperty("MembershipProvider:ReturnFalseInValidateUserIfAccountLocked", false);

	public static bool ReturnFalseInValidateUserIfAccountDeleted => ConfigHelper.GetBoolProperty("MembershipProvider:ReturnFalseInValidateUserIfAccountDeleted", true);

	public static bool AllowNewRegistrationToActivateDeletedAccountWithSameEmail => ConfigHelper.GetBoolProperty("MembershipProvider:AllowNewRegistrationToActivateDeletedAccountWithSameEmail", true);

	public static string PasswordGeneratorChars => ConfigHelper.GetStringProperty("PasswordGeneratorChars", "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$");

	public static bool ShowSystemInformationInChildSiteAdminMenu => ConfigHelper.GetBoolProperty("ShowSystemInformationInChildSiteAdminMenu", true);

	public static bool ShowConnectionErrorOnSetup => ConfigHelper.GetBoolProperty("ShowConnectionErrorOnSetup", false);

	public static bool NotifyUsersOnAccountApproval => ConfigHelper.GetBoolProperty("NotifyUsersOnAccountApproval", true);

	/// <summary>
	/// a comma separated list of email addresses to exclude when sending
	/// administrative emails including registration notifications and content workflow submissions
	/// this is for when you have admin user accounts that you do not want to receive these emails
	/// </summary>
	public static string EmailAddressesToExcludeFromAdminNotifications => ConfigHelper.GetStringProperty("EmailAddressesToExcludeFromAdminNotifications", string.Empty);

	/// <summary>
	/// if a value is specified then these settings will be used instead of the standard site smtp settings
	/// </summary>
	public static string NewsletterSmtpServer => ConfigHelper.GetStringProperty("Newsletter:SmtpServer", string.Empty);

	public static int NewsletterSmtpServerPort => ConfigHelper.GetIntProperty("Newsletter:SmtpServerPort", 25);

	public static string NewsletterSmtpUser => ConfigHelper.GetStringProperty("Newsletter:SmtpUser", string.Empty);

	public static string NewsletterSmtpUserPassword => ConfigHelper.GetStringProperty("Newsletter:SmtpUserPassword", string.Empty);

	public static bool NewsletterSmtpRequiresAuthentication => ConfigHelper.GetBoolProperty("Newsletter:SmtpRequiresAuthentication", false);

	public static bool NewsletterSmtpUseSsl => ConfigHelper.GetBoolProperty("Newsletter:SmtpUseSsl", false);

	public static string NewsletterSmtpPreferredEncoding => ConfigHelper.GetStringProperty("Newsletter:SmtpPreferredEncoding", string.Empty);

	public static bool NewsletterTestMode => ConfigHelper.GetBoolProperty("NewsletterTestMode", false);

	public static bool GuessEmailForWindowsAuth => ConfigHelper.GetBoolProperty("GuessEmailForWindowsAuth", false);

	public static string WindowsAuthDomainExtension => ConfigHelper.GetStringProperty("WindowsAuthDomainExtension", ".com");

	public static bool UseFolderBasedMultiTenants => ConfigurationManager.AppSettings["UseFoldersInsteadOfHostnamesForMultipleSites"] != null
				? ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false)
				: ConfigHelper.GetBoolProperty("UseFolderBasedMultiTenants", false);

	public static bool UseSiteNameForRootBreadcrumb => ConfigHelper.GetBoolProperty("UseSiteNameForRootBreadcrumb", false);

	public static bool UseRelatedSiteMode => AppConfig.RelatedSiteModeEnabled;

	public static bool UseSameContentFolderForRelatedSiteMode => AppConfig.RelatedSiteModeShareContentFolder;

	public static int RelatedSiteID => AppConfig.RelatedSiteID;

	public static bool RelatedSiteModeHideRoleManagerInChildSites => AppConfig.RelatedSiteModeHideRoleManagerInChildSites;

	public static bool UseUrlReWriting => ConfigHelper.GetBoolProperty("UseUrlReWriting", true);

	/// <summary>
	/// setting this to true solves a problem where IIS logs were not showing the re-written/requested url
	/// http://stackoverflow.com/questions/353541/iis7-rewritepath-and-iis-log-files
	/// </summary>
	public static bool UseTransferRequestForUrlReWriting => ConfigHelper.GetBoolProperty("UseTransferRequestForUrlReWriting", true);

	public static bool UseUrlReWritingForStaticFiles => ConfigHelper.GetBoolProperty("UseUrlReWritingForStaticFiles", false);

	public static bool DisableUseOfPassedInDateForMetaWeblogApi => ConfigHelper.GetBoolProperty("DisableUseOfPassedInDateForMetaWeblogApi", false);

	public static string FacebookAppId => ConfigHelper.GetStringProperty("FacebookAppId", string.Empty);

	public static bool DisableFacebookLikeButton => ConfigHelper.GetBoolProperty("DisableFacebookLikeButton", false);

	public static bool DisableHelpSystem => ConfigHelper.GetBoolProperty("DisableHelpSystem", false);

	public static bool DisableWorkflowNotification => ConfigHelper.GetBoolProperty("DisableWorkflowNotification", false);

	public static bool RenderModulePanel => ConfigHelper.GetBoolProperty("RenderModulePanel", true);

	public static bool HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins => ConfigHelper.GetBoolProperty("HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins", false);

	public static bool HideModuleSettingsDeleteButtonFromNonAdmins => ConfigHelper.GetBoolProperty("HideModuleSettingsDeleteButtonFromNonAdmins", false);

	public static bool Disable301Redirector => ConfigHelper.GetBoolProperty("Disable301Redirector", false);

	public static bool IncludeParametersIn301RedirectLookup => ConfigHelper.GetBoolProperty("IncludeParametersIn301RedirectLookup", false);

	public static bool AllowExternal301Redirects => ConfigHelper.GetBoolProperty("AllowExternal301Redirects", false);

	public static bool EnableRouting => ConfigHelper.GetBoolProperty("EnableRouting", true);

	public static bool AddDefaultMvcRoute => ConfigHelper.GetBoolProperty("AddDefaultMvcRoute", false);

	public static string RouteConfigPath => ConfigHelper.GetStringProperty("RouteConfigPath", "~/Setup/RouteRegistrars/");

	public static bool EnableVirtualPathProviders => ConfigHelper.GetBoolProperty("EnableVirtualPathProviders", true);


	public static bool PassQueryStringFor301Redirects => ConfigHelper.GetBoolProperty("PassQueryStringFor301Redirects", false);

	public static bool DisableCacheFor301Redirects => ConfigHelper.GetBoolProperty("DisableCacheFor301Redirects", false);

	public static bool SetExplicitCacheFor301Redirects => ConfigHelper.GetBoolProperty("SetExplicitCacheFor301Redirects", true);

	public static int CacheDurationInDaysFor301Redirects => ConfigHelper.GetIntProperty("CacheDurationInDaysFor301Redirects", 5);

	public static bool DisableTaskQueue => ConfigHelper.GetBoolProperty("DisableTaskQueue", false);

	public static bool DisableBannedIpBlockingModule => ConfigHelper.GetBoolProperty("DisableBannedIpBlockingModule", false);

	public static bool AppDomainMonitoringEnabled => ConfigHelper.GetBoolProperty("AppDomainMonitoringEnabled", false);

	public static bool FirstChanceExceptionMonitoringEnabled => ConfigHelper.GetBoolProperty("FirstChanceExceptionMonitoringEnabled", false);

	public static bool UsePerSiteTaskQueue => ConfigHelper.GetBoolProperty("UsePerSiteTaskQueue", false);

	public static string mojoCombinedScriptVersionParam => ConfigHelper.GetStringProperty("mojoCombinedScriptVersionParam", "v3");

	public static string BingMapsVersion => ConfigHelper.GetStringProperty("BingMapsVersion", "6.3");

	public static bool UseGoogleCDN => ConfigHelper.GetBoolProperty("UseGoogleCDN", true);

	public static string GoogleCDNJQueryBaseUrl => ConfigHelper.GetStringProperty("GoogleCDNJQueryBaseUrl", "//ajax.googleapis.com/ajax/libs/jquery/");

	public static string GoogleCDNJQueryUIBaseUrl => ConfigHelper.GetStringProperty("GoogleCDNJQueryUIBaseUrl", "//ajax.googleapis.com/ajax/libs/jqueryui/");

	public static bool BundlesUseCdn => ConfigHelper.GetBoolProperty("BundlesUseCdn", true);

	public static bool BundlesForceOptimization => ConfigHelper.GetBoolProperty("BundlesForceOptimization", true);

	public static bool DisableAjaxToolkitBundlesAndScriptReferences => ConfigHelper.GetBoolProperty("DisableAjaxToolkitBundlesAndScriptReferences", true);

	public static string GoogleCDNjQueryVersion => ConfigHelper.GetStringProperty("GoogleCDNjQueryVersion", "1.9.1");

	public static string GoogleCDNjQueryUIVersion => ConfigHelper.GetStringProperty("GoogleCDNjQueryUIVersion", "1.10.2");

	public static string jQueryBasePath => ConfigHelper.GetStringProperty("jQueryBasePath", "~/ClientScript/jquery142/");

	public static string jQueryUIBasePath => ConfigHelper.GetStringProperty("jQueryUIBasePath", "~/ClientScript/jqueryui182/");

	public static string TimePickerScriptUrl => ConfigHelper.GetStringProperty("TimePickerScriptUrl", "~/ClientScript/jqmojo/jquery-ui-timepicker-addonv3.js");

	public static string TimePickerScriptLocaleBaseUrl => ConfigHelper.GetStringProperty("TimePickerScriptLocaleBaseUrl", "~/ClientScript/jqmojo/timepicker-i18n/");

	/// <summary>
	/// In IIS Integrated mode if you want to use the App Keep alive feature you need to specify
	/// the root url of your site in this setting like http://yoursiteroot/Default.aspx
	/// </summary>
	public static string AppKeepAliveUrl => ConfigHelper.GetStringProperty("AppKeepAliveUrl", string.Empty);

	/// <summary>
	/// If true the MeatContentControl will render the content type meta tag. Set to false if you would rather specify it in the layout.master
	/// </summary>
	public static bool AutoSetContentType => ConfigHelper.GetBoolProperty("AutoSetContentType", true);

	public static string ContentMimeType => ConfigHelper.GetStringProperty("ContentMimeType", "application/xhtml+xml");

	public static string ContentEncoding => ConfigHelper.GetStringProperty("ContentEncoding", "utf-8");

	public static bool UseAppKeepAlive => ConfigHelper.GetBoolProperty("UseAppKeepAlive", false);

	public static int AppKeepAliveSleepMinutes => ConfigHelper.GetIntProperty("AppKeepAliveSleepMinutes", 10);

	public static int AppKeepAliveMaxRunTimeMinutes => ConfigHelper.GetIntProperty("AppKeepAliveMaxRunTimeMinutes", 720);

	public static bool AssignNewPagesParentPageSkinByDefault => ConfigHelper.GetBoolProperty("AssignNewPagesParentPageSkinByDefault", true);

	public static bool AllowAnonymousUsersToViewMemberList => ConfigHelper.GetBoolProperty("AllowAnonymousUsersToViewMemberList", false);

	public static bool AutoGenerateAndHideUserNamesWhenUsingEmailForLogin => ConfigHelper.GetBoolProperty("AutoGenerateAndHideUserNamesWhenUsingEmailForLogin", false);

	public static bool DisablePageViewStateByDefault => ConfigHelper.GetBoolProperty("DisablePageViewStateByDefault", false);

	public static string CacheProviderType => ConfigHelper.GetStringProperty("Cache:ProviderType", "mojoPortal.Web.Caching.MemoryCacheAdapter, mojoPortal.Web");

	public static string DistributedCacheServers => ConfigHelper.GetStringProperty("Cache:DistributedCacheServers", "localhost:22223");

	public static string AzureCacheSecurityMode => ConfigHelper.GetStringProperty("Cache:AzureCacheSecurityMode", "Message");

	public static string AzureCacheAuthorizationInfo => ConfigHelper.GetStringProperty("Cache:AzureCacheAuthorizationInfo", string.Empty);

	public static bool AzureCacheUseSsl => ConfigHelper.GetBoolProperty("Cache:AzureCacheUseSsl", true);

	public static bool DisableGlobalContent => ConfigHelper.GetBoolProperty("DisableGlobalContent", false);

	public static bool RequireContentTitle => ConfigHelper.GetBoolProperty("RequireContentTitle", true);

	public static bool RequireChangeDefaultContentTitle => ConfigHelper.GetBoolProperty("RequireChangeDefaultContentTitle", true);

	public static bool PrePopulateDefaultContentTitle => ConfigHelper.GetBoolProperty("PrePopulateDefaultContentTitle", true);

	public static bool UsePageContentWizard => ConfigHelper.GetBoolProperty("UsePageContentWizard", true);

	public static bool DisableContentCache => ConfigHelper.GetBoolProperty("DisableContentCache", true);

	public static bool UseCacheDependencyFiles => ConfigHelper.GetBoolProperty("UseCacheDependencyFiles", false);

	public static bool RedirectHomeFromSetupPagesWhenSystemIsUpToDate => ConfigHelper.GetBoolProperty("RedirectHomeFromSetupPagesWhenSystemIsUpToDate", true);

	public static bool AutoSuggestFriendlyUrls => ConfigHelper.GetBoolProperty("AutoSuggestFriendlyUrls", true);

	public static bool AutoSuggestFriendlyUrlsOnPageNameChanges => ConfigHelper.GetBoolProperty("AutoSuggestFriendlyUrlsOnPageNameChanges", true);

	public static bool DisableMetaWeblogApi => ConfigHelper.GetBoolProperty("DisableMetaWeblogApi", false);

	public static bool DisableEditingPagesInMetaWeblogApi => ConfigHelper.GetBoolProperty("DisableEditingPagesInMetaWeblogApi", false);

	public static bool DisableDeletingPagesInMetaWeblogApi => ConfigHelper.GetBoolProperty("DisableDeletingPagesInMetaWeblogApi", false);

	public static bool AutoGeneratePageMetaDescriptionForMetaweblogNewPages => ConfigHelper.GetBoolProperty("AutoGeneratePageMetaDescriptionForMetaweblogNewPages", true);

	public static int MetaweblogGeneratedMetaDescriptionMaxLength => ConfigHelper.GetIntProperty("MetaweblogGeneratedMetaDescriptionMaxLength", 165);

	public static bool AppendDateToBlogUrls => ConfigHelper.GetBoolProperty("AppendDateToBlogUrls", false);

	public static bool AllowForcingPreferredHostName => ConfigHelper.GetBoolProperty("AllowForcingPreferredHostName", false);

	public static bool Use301RedirectWhenEnforcingPreferredHostName => ConfigHelper.GetBoolProperty("Use301RedirectWhenEnforcingPreferredHostName", true);

	public static bool RedirectToRootWhenEnforcingPreferredHostName => ConfigHelper.GetBoolProperty("RedirectToRootWhenEnforcingPreferredHostName", false);

	public static int SessionKeepAliveFrequencyOverrideMinutes => ConfigHelper.GetIntProperty("SessionKeepAliveFrequencyOverrideMinutes", -1);

	public static bool ForceSingleSessionPerUser => ConfigHelper.GetBoolProperty("ForceSingleSessionPerUser", false);

	public static bool EnforceRequirePasswordChanges => ConfigHelper.GetBoolProperty("EnforcRequirePasswordChanges", true);

	/// <summary>
	/// You should not call this directly, instead use SiteUtils.SslIsAvailable()
	/// we now support separate ssl settings per site with Web.config like this:
	/// Site1-SSLIsAvailable, Site2-SSLIsAvailable etc, and trhe siteutils method resolves this
	/// </summary>
	public static bool SslisAvailable => ConfigHelper.GetBoolProperty("SSLIsAvailable", false);

	public static bool ForceSslOnAllPages => ConfigHelper.GetBoolProperty("ForceSslOnAllPages", true);

	public static bool RequireSslForRoleCookie => ConfigHelper.GetBoolProperty("RequireSslForRoleCookie", true);

	public static bool ForceHttpForCanonicalUrlsThatDontRequireSsl => ConfigHelper.GetBoolProperty("ForceHttpForCanonicalUrlsThatDontRequireSsl", false);

	public static bool ShowWarningWhenSslIsAvailableButNotUsedWithLoginModule => ConfigHelper.GetBoolProperty("ShowWarningWhenSslIsAvailableButNotUsedWithLoginModule", true);

	public static bool IsRunningInRootSite => ConfigHelper.GetBoolProperty("IsRunningInRootSite", true);

	public static bool ForceRegexOnEmailValidator => ConfigHelper.GetBoolProperty("ForceRegexOnEmailValidator", false);

	public static bool UseHSTSHeader => ConfigHelper.GetBoolProperty("UseHSTSHeader", false);

	public static string HSTSHeaders => ConfigHelper.GetStringProperty("HSTSHeaders", "max-age= 63072000;");

	public static bool ClearSslOnNonSecurePages => ConfigHelper.GetBoolProperty("ClearSslOnNonSecurePages", false);

	/// <summary>
	/// If the current request is using https, then a relative url for all menu items will resolve to https
	/// This setting enables chaning to fully qualified urls in the menus to avoid this
	/// which in turn avoids an unneeded redirect to enforce or clear ssl
	/// </summary>
	public static bool ResolveFullUrlsForMenuItemProtocolDifferences => ConfigHelper.GetBoolProperty("ResolveFullUrlsForMenuItemProtocolDifferences", false);

	/// <summary>
	/// The title element of an html page should not exceed 65 chars.
	/// Ideally you should set this to true
	/// </summary>
	public static bool AutoTruncatePageTitles => ConfigHelper.GetBoolProperty("AutoTruncatePageTitles", false);

	public static bool AutomaticallyAddCanonicalUrlToCmsPages => ConfigHelper.GetBoolProperty("AutomaticallyAddCanonicalUrlToCmsPages", true);

	public static bool ShowRebuildReportsButton => ConfigHelper.GetBoolProperty("ShowRebuildReportsButton", false);

	public static bool UseShortcutKeys => ConfigHelper.GetBoolProperty("UseShortcutKeys", false);

	public static string AdminImage => ConfigHelper.GetStringProperty("AdminImage", "key.png");

	public static string PageTreeImage => ConfigHelper.GetStringProperty("PageTreeImage", "shelf_double_down.png");

	public static int ParentPageDialogExpansionDepth => ConfigHelper.GetIntProperty("ParentPageDialogExpansionDepth", 0);

	public static string EditContentImage => ConfigHelper.GetStringProperty("EditContentImage", "edit.png");

	public static string EditPageFeaturesImage => ConfigHelper.GetStringProperty("EditPageFeaturesImage", "edit_cover.png");

	public static string EditPageSettingsImage => ConfigHelper.GetStringProperty("EditPageSettingsImage", "page-settings-icon.png");

	public static string EditPropertiesImage => ConfigHelper.GetStringProperty("EditPropertiesImage", "cog-icon.png");

	public static string DeleteLinkImage => ConfigHelper.GetStringProperty("DeleteLinkImage", "delete.png");

	public static string RSSImageFileName => ConfigHelper.GetStringProperty("RSSImageFileName", "feed.png");

	public static string NewThreadImage => ConfigHelper.GetStringProperty("NewThreadImage", "messages_new.png");

	public static string ForumThreadImage => ConfigHelper.GetStringProperty("ForumThreadImage", "messages_chat.png");

	public static bool UseIconsForAdminLinks => ConfigHelper.GetBoolProperty("UseIconsForAdminLinks", true);

	public static bool UsePageImagesInSiteMap => ConfigHelper.GetBoolProperty("UsePageImagesInSiteMap", false);

	public static bool TreatChildPageIndexAsSiteMap => ConfigHelper.GetBoolProperty("TreatChildPageIndexAsSiteMap", false);

	public static bool UseTextLinksForFeatureSettings => ConfigHelper.GetBoolProperty("UseTextLinksForFeatureSettings", true);

	public static bool Log404HandlerExceptions
	{
		get
		{
			if (ConfigurationManager.AppSettings["Log404HandlerExceptions"] != null)
			{
				return ConfigHelper.GetBoolProperty("Log404HandlerExceptions", false);
			}
			//backwards compatibility with old setting
			return ConfigHelper.GetBoolProperty("LogErrorsFrom404Handler", false);
		}
	}
	public static bool Log404Errors => ConfigHelper.GetBoolProperty("Log404Errors", true);

	public static bool LogRedirectsToPreferredHostName => ConfigHelper.GetBoolProperty("LogRedirectsToPreferredHostName", false);

	public static bool TrackAuthenticatedRequests => ConfigHelper.GetBoolProperty("TrackAuthenticatedRequests", true);

	public static bool TrackIPForAuthenticatedRequests => ConfigHelper.GetBoolProperty("TrackIPForAuthenticatedRequests", false);

	public static bool SiteStatisticsShowMemberStatisticsDefault => ConfigHelper.GetBoolProperty("SiteStatistics_ShowMemberStatistics_Default", true);

	public static bool SiteStatisticsShowOnlineStatisticsDefault => ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineStatistics_Default", true);

	public static bool SiteStatisticsShowOnlineMembersDefault => ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineMembers_Default", true);

	public static bool DisableFileManager => ConfigHelper.GetBoolProperty("DisableFileManager", false);

	public static bool FileDialogEnableDragDrop => ConfigHelper.GetBoolProperty("FileDialogEnableDragDrop", true);

	public static bool ShowFileManagerLink => ConfigHelper.GetBoolProperty("ShowFileManagerLink", true);

	public static bool ShowServerPathInFileManager => ConfigHelper.GetBoolProperty("ShowServerPathInFileManager", true);

	public static bool DisableSearchIndex => ConfigHelper.GetBoolProperty("DisableSearchIndex", false);

	public static bool IndexPageKeywordsWithHtmlArticleContent => ConfigHelper.GetBoolProperty("IndexPageKeywordsWithHtmlArticleContent", false);

	public static bool DisableOpenSearchAutoDiscovery => ConfigHelper.GetBoolProperty("DisableOpenSearchAutoDiscovery", false);

	public static bool ShowModuleTitleInSearchResultLink => ConfigHelper.GetBoolProperty("ShowModuleTitleInSearchResultLink", false);


	/// <summary>
	/// generally we should not include the page meta because it can result in duplicate results
	/// one for each instance of html content on the page because they all use the same page meta from the parent page.
	/// since page meta should reflect the content of the page it is sufficient to just index the content
	/// </summary>
	public static bool IndexPageMeta => ConfigHelper.GetBoolProperty("IndexPageMeta", false);

	/// <summary>
	/// in a cluster, only one node should have this set to true
	/// clusters are only supported if they share a common file system (as of 2009-09-25)
	/// and in this configuration we should let just one node be responsible for indexing the content.
	/// </summary>
	public static bool IsSearchIndexingNode => ConfigHelper.GetBoolProperty("IsSearchIndexingNode", true);

	/// <summary>
	/// disabled by default for backawards compatibility with existing indexes.
	/// if you set this to true in Web.config/user.config you should rebuild the index
	/// http://www.mojoportal.com/rebuilding-the-search-index.aspx
	/// </summary>
	public static bool EnableSearchResultsHighlighting => ConfigHelper.GetBoolProperty("EnableSearchResultsHighlighting", false);

	/// <summary>
	/// disabled by default for backawards compatibility with existing indexes.
	/// if you set this to false in Web.config/user.config you should rebuild the index
	/// http://www.mojoportal.com/rebuilding-the-search-index.aspx
	/// </summary>
	public static bool DisableSearchFeatureFilters => ConfigHelper.GetBoolProperty("DisableSearchFeatureFilters", true);

	public static string SearchableFeatureGuidsToExclude => ConfigHelper.GetStringProperty("SearchableFeatureGuidsToExclude", string.Empty);

	///
	///2013-08-20 JA added this to support add on products on the demo site
	/// so I can add css needed to demo the add on features without having to add/maintain it in every skin
	/// whereas customers would typically add the css that ships with the feature to their skin and list it in style.config
	///  example format ~/Data/gcss/
	/// 
	public static string GlobalAddOnStyleFolder => ConfigHelper.GetStringProperty("GlobalAddOnStyleFolder", string.Empty);

	public static bool AllowMobileSkinForNonMobile => ConfigHelper.GetBoolProperty("AllowMobileSkinForNonMobile", false);

	public static string MobilePhoneSkin => ConfigHelper.GetStringProperty("MobilePhoneSkin", string.Empty);

	public static string SkinsToExcludeFromSkinList => ConfigHelper.GetStringProperty("SkinsToExcludeFromSkinList", "printerfriendly");

	public static string MobileDetectionExcludeUrlsCsv => ConfigHelper.GetStringProperty("MobileDetectionExcludeUrlsCsv", string.Empty);

	//http://googlewebmastercentral.blogspot.com/2012/11/giving-tablet-users-full-sized-web.html
	//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11092~1#post46239
	// Android phones can be differentiated by Android; Mobile;

	public static string MobilePhoneUserAgents => ConfigHelper.GetStringProperty("MobilePhoneUserAgents", "iphone,ipod,iemobile,android;blackberry");

	public static bool DisableYUI => ConfigHelper.GetBoolProperty("DisableYUI", false);

	public static bool DisableZedGraph => false;

	public static bool IncludeFaqContentTemplate => ConfigHelper.GetBoolProperty("IncludeFaqContentTemplate", true);

	public static bool IncludejQueryAccordionContentTemplate => ConfigHelper.GetBoolProperty("IncludejQueryAccordionContentTemplate", true);

	public static bool IncludejQueryAccordionNoHeightContentTemplate => ConfigHelper.GetBoolProperty("IncludejQueryAccordionNoHeightContentTemplate", true);

	public static bool IncludejQueryTabsContentTemplate => ConfigHelper.GetBoolProperty("IncludejQueryTabsContentTemplate", true);

	public static bool Include2ColumnsOver1ColumnTemplate => ConfigHelper.GetBoolProperty("Include2ColumnsOver1ColumnTemplate", true);

	public static bool Include3ColumnsOver1ColumnTemplate => ConfigHelper.GetBoolProperty("Include3ColumnsOver1ColumnTemplate", true);

	public static bool Include4ColumnsOver1ColumnTemplate => ConfigHelper.GetBoolProperty("Include4ColumnsOver1ColumnTemplate", true);

	public static bool RedirectToNewPageOnCreationGlobalDefault => ConfigHelper.GetBoolProperty("RedirectToNewPageOnCreationGlobalDefault", true);

	public static bool EnableDragDropPageLayout => ConfigHelper.GetBoolProperty("EnableDragDropPageLayout", false);

	public static bool OpenSearchDownloadLinksInNewWindow => ConfigHelper.GetBoolProperty("OpenSearchDownloadLinksInNewWindow", true);

	public static bool DisablejQuery => ConfigHelper.GetBoolProperty("DisablejQuery", false);

	public static bool DisableEditArea => ConfigHelper.GetBoolProperty("DisableEditArea", false);

	public static bool DisableEditAreaForCssEditor => ConfigHelper.GetBoolProperty("DisableEditAreaForCssEditor", false);

	public static bool DisablejQueryUI => ConfigHelper.GetBoolProperty("DisablejQueryUI", false);

	public static bool DisableExternalCommentSystems => ConfigHelper.GetBoolProperty("DisableExternalCommentSystems", false);

	public static bool DisableBlogRssMetaLink => ConfigHelper.GetBoolProperty("DisableBlogRssMetaLink", false);

	public static bool UseSSLForFeedLinks => ConfigHelper.GetBoolProperty("UseSSLForFeedLinks", true);

	/// <summary>
	/// if true disables /Services/RecentContentRss.aspx
	/// </summary>
	public static bool DisableRecentContentFeed => ConfigHelper.GetBoolProperty("DisableRecentContentFeed", false);

	public static string RecentContentChannelDescription => ConfigHelper.GetStringProperty("RecentContentChannelDescription", "Recent Content");

	public static string RecentContentChannelCopyright => ConfigHelper.GetStringProperty("RecentContentChannelCopyright", string.Empty);

	public static string RecentContentChannelNotifyEmail => ConfigHelper.GetStringProperty("RecentContentChannelNotifyEmail", string.Empty);

	public static int RecentContentFeedTimeToLive => ConfigHelper.GetIntProperty("RecentContentFeedTimeToLive", 10);

	public static int RecentContentFeedCacheTimeInMinutes => ConfigHelper.GetIntProperty("RecentContentFeedCacheTimeInMinutes", 10);

	/// <summary>
	/// by defult 30 days which means the feed will show all searchable site content
	/// with a modified date newer than 30 days ago
	/// </summary>
	public static int RecentContentFeedMaxDaysOld => ConfigHelper.GetIntProperty("RecentContentFeedMaxDaysOld", 30);

	/// <summary>
	/// default of 10 items will be returned but the feed takes a param n=5 to get 5 etc
	/// </summary>
	public static int RecentContentDefaultItemsToRetrieve => ConfigHelper.GetIntProperty("RecentContentDefaultItemsToRetrieve", 10);

	/// <summary>
	/// the upper limit of allowed results in the recent content feed to prevent heavy requests
	/// </summary>
	public static int RecentContentMaxItemsToRetrieve => ConfigHelper.GetIntProperty("RecentContentMaxItemsToRetrieve", 60);

	public static bool AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin => ConfigHelper.GetBoolProperty("AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin", false);

	public static bool EnableNewsletter => ConfigHelper.GetBoolProperty("EnableNewsletter", true);

	public static bool EnableContentWorkflow => ConfigHelper.GetBoolProperty("EnableContentWorkflow", true);

	public static bool EnableAjaxControlPasswordStrength => ConfigHelper.GetBoolProperty("EnableAjaxControlPasswordStrength", true);

	public static bool WorkflowShowPublishForUnSubmittedDraft => ConfigHelper.GetBoolProperty("WorkflowShowPublishForUnSubmittedDraft", false);

	public static bool Use3LevelContentWorkflow => ConfigHelper.GetBoolProperty("Use3LevelContentWorkflow", false);

	public static string RolesAllowedToUseWorkflowAdminPages => ConfigHelper.GetStringProperty("RoleseAllowedToUseWorkflowAdminPages", string.Empty);

	public static bool PromptBeforeUnsubscribeNewsletter => ConfigHelper.GetBoolProperty("PromptBeforeUnsubscribeNewsletter", false);

	public static bool EnableBlogSiteMap => ConfigHelper.GetBoolProperty("EnableBlogSiteMap", true);

	public static bool EnforcePageSettingsInChildPageSiteMapModule => ConfigHelper.GetBoolProperty("EnforcePageSettingsInChildPageSiteMapModule", false);

	public static bool HideMasterPageChildSiteMapWhenUsingModule => ConfigHelper.GetBoolProperty("HideMasterPageChildSiteMapWhenUsingModule", true);

	public static bool EnableWoopraGlobally => ConfigHelper.GetBoolProperty("EnableWoopraGlobally", false);

	public static bool DisableWoopraGlobally => ConfigHelper.GetBoolProperty("DisableWoopraGlobally", false);

	public static bool MapAlternatePort => ConfigHelper.GetBoolProperty("MapAlternatePort", true);

	public static bool MapAlternateSSLPort => ConfigHelper.GetBoolProperty("MapAlternateSSLPort", true);

	public static bool ShowAlternateSearchIfConfigured => ConfigHelper.GetBoolProperty("ShowAlternateSearchIfConfigured", false);

	public static int SearchResultsPageSize => ConfigHelper.GetIntProperty("SearchResultsPageSize", 30);

	public static int SearchResultsFragmentSize => ConfigHelper.GetIntProperty("SearchResultsFragmentSize", 500);

	public static int SearchMaxClauseCount => ConfigHelper.GetIntProperty("SearchMaxClauseCount", 1024);

	public static int ContentCatalogPageSize => ConfigHelper.GetIntProperty("ContentCatalogPageSize", 30);

	public static int UrlManagerPageSize => ConfigHelper.GetIntProperty("UrlManagerPageSize", 30);

	public static int RedirectManagerPageSize => ConfigHelper.GetIntProperty("RedirectManagerPageSize", 30);

	public static int ContentStyleTemplatePageSize => ConfigHelper.GetIntProperty("ContentStyleTemplatePageSize", 30);

	public static int ContentTemplatePageSize => ConfigHelper.GetIntProperty("ContentTemplatePageSize", 5);

	public static bool ContentTemplateShowBodyInAdminList => ConfigHelper.GetBoolProperty("ContentTemplateShowBodyInAdminList", true);

	public static bool AddSystemStyleTemplatesAboveSiteTemplates => ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesAboveSiteTemplates", true);

	public static bool AddSystemStyleTemplatesBelowSiteTemplates => ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesBelowSiteTemplates", false);

	public static int ContentRatingListPageSize => ConfigHelper.GetIntProperty("ContentRatingListPageSize", 30);

	public static int MemberListPageSize => ConfigHelper.GetIntProperty("MemberListPageSize", 30);

	public static int NewsletterArchivePageSize => ConfigHelper.GetIntProperty("NewsletterArchivePageSize", 30);

	public static int NewsletterMaxToSendPerMinute => ConfigHelper.GetIntProperty("NewsletterMaxToSendPerMinute", 0);

	public static bool NewsletterEnforceCanSpam => ConfigHelper.GetBoolProperty("NewsletterEnforceCanSpam", true);

	public static bool NewsletterUseHtmlEmailConfirmation => ConfigHelper.GetBoolProperty("NewsletterUseHtmlEmailConfirmation", false);

	public static bool NewsletterAutoSubscribeUsersCreatedByAdmin => ConfigHelper.GetBoolProperty("NewsletterAutoSubscribeUsersCreatedByAdmin", true);

	public static bool NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers => ConfigHelper.GetBoolProperty("NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers", true);

	public static bool NewsletterRequireVerification => ConfigHelper.GetBoolProperty("NewsletterRequireVerification", true);

	/// <summary>
	/// Since we send a verification email for anonymous subscribers we have to consider the possibility that a bot will
	/// submit the same email addresses over and over frequently. We don't want to be spamming people with the verification email
	/// so if we have an existing unverified subscription and it gets submitted again we will only re-send the verification
	/// if this many days have passed since it was last submitted, default is 5 days.
	/// This way in case the user lost the original verification or his email was unavailable for some reason
	/// he can get a new opportunity to confirm by submitting again.
	/// </summary>
	public static int NewsletterReVerifcationAfterDays => ConfigHelper.GetIntProperty("NewsletterReVerifcationAfterDays", 5);


	public static int MinutesBetweenAnonymousRatings => ConfigHelper.GetIntProperty("MinutesBetweenAnonymousRatings", 5);

	public static int NumberOfWebPartsToShowInMiniCatalog => ConfigHelper.GetIntProperty("NumberOfWebPartsToShowInMiniCatalog", 15);

	public static int WebPageInfoCacheMinutes => ConfigHelper.GetIntProperty("WebPageInfoCacheMinutes", 20);

	public static int SiteSettingsCacheDurationInSeconds => ConfigHelper.GetIntProperty("SiteSettingsCacheDurationInSeconds", 120);

	public static Guid InternalFeedSecurityBypassKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"] != null)
			{
				var sGuid = ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"];
				if (sGuid.Length == 36)
				{
					try
					{
						var g = new Guid(sGuid);
						return g;
					}
					catch (FormatException)
					{
					}
				}
			}

			return Guid.Empty;
		}
	}

	public static string PasswordRecoveryEmailTemplateFileNamePattern => ConfigHelper.GetStringProperty("PasswordRecoveryEmailTemplateFileNamePattern", "PasswordEmailMessage.config");

	/// <summary>
	/// possible values, MD5 (default), SHA256, SHA512
	/// for future use
	/// currently we can only use MD5 because the password field in the db is only nvarchar(128)
	/// SHA256 requires 256 bits and SHA512 requires 512 bits
	/// we will need to change to ntext (to support SQL 2000)
	/// </summary>
	public static string HashedPasswordCryptoType => ConfigHelper.GetStringProperty("HashedPasswordCryptoType", "MD5");

	public static string HashedPasswordRecoveryEmailTemplateFileNamePattern => ConfigHelper.GetStringProperty("HashedPasswordRecoveryEmailTemplateFileNamePattern", "HashedPasswordEmailMessage.config");

	public static string RolesThatCannotBeDeleted => ConfigHelper.GetStringProperty("RolesThatCannotBeDeleted", string.Empty);

	public static string DefaultContentTemplateAllowedRoles => ConfigHelper.GetStringProperty("DefaultContentTemplateAllowedRoles", "All Users;");

	public static string RecaptchaPrivateKey => ConfigHelper.GetStringProperty("RecaptchaPrivateKey", string.Empty);

	public static string RecaptchaPublicKey => ConfigHelper.GetStringProperty("RecaptchaPublicKey", string.Empty);

	public static string RecaptchaHCaptcha
	{
		get
		{
			if (ConfigurationManager.AppSettings["RecaptchaPublicKey"] != null)
			{
				return ConfigurationManager.AppSettings["RecaptchaHCaptcha"];
			}
			return "recaptcha";
		}
	}

	public static string ReCaptchaDefaultVerifyUrl => ConfigHelper.GetStringProperty("reCaptcha:DefaultVerifyUrl", "http://www.google.com/recaptcha/api/siteverify");

	public static string HCaptchaDefaultVerifyUrl => ConfigHelper.GetStringProperty("hCaptcha:DefaultVerifyUrl", "https://hcaptcha.com/siteverify");

	public static string ReCaptchaDefaultClientScriptUrl => ConfigHelper.GetStringProperty("reCaptcha:DefaultClientScriptUrl", "https://www.google.com/recaptcha/api.js");

	public static string HCaptchaDefaultClientScriptUrl => ConfigHelper.GetStringProperty("hCaptcha:DefaultClientScriptUrl", "https://hcaptcha.com/1/api.js");

	public static string ReCaptchaDefaultParam => ConfigHelper.GetStringProperty("reCaptcha:DefaultParam", "g-recaptcha");

	public static string HCaptchaDefaultParam => ConfigHelper.GetStringProperty("hCaptcha:DefaultParam", "h-captcha");

	public static string ReCaptchaDefaultTheme => ConfigHelper.GetStringProperty("reCaptcha:DefaultTheme", "light");

	public static string HCaptchaDefaultTheme => ConfigHelper.GetStringProperty("hCaptcha:DefaultTheme", "light");

	public static string ReCaptchaDefaultResponseField => ConfigHelper.GetStringProperty("reCaptcha:DefaultResponseField", "g-recaptcha-response");

	public static string HCaptchaDefaultResponseField => ConfigHelper.GetStringProperty("HCaptcha:DefaultResponseField", "h-captcha-response");

	public static bool UseRawUrlForCmsPageLoginRedirects => ConfigHelper.GetBoolProperty("UseRawUrlForCmsPageLoginRedirects", false);

	public static bool UseAlternateFileManagerAsDefault => ConfigHelper.GetBoolProperty("UseAlternateFileManagerAsDefault", false);

	public static bool ForceLowerCaseForUploadedFiles => ConfigHelper.GetBoolProperty("ForceLowerCaseForUploadedFiles", true);

	public static bool ForceLegacyFileUpload => ConfigHelper.GetBoolProperty("ForceLegacyFileUpload", false);

	public static bool ForceLowerCaseForFolderCreation => ConfigHelper.GetBoolProperty("ForceLowerCaseForFolderCreation", true);

	public static bool ForceAdminsToUseMediaFolder => ConfigHelper.GetBoolProperty("ForceAdminsToUseMediaFolder", true);

	public static bool AllowAdminsToUseDataFolder => ConfigHelper.GetBoolProperty("AllowAdminsToUseDataFolder", false);

	public static bool AllowRoleAdminsToCreateContentManagers => ConfigHelper.GetBoolProperty("AllowRoleAdminsToCreateContentManagers", true);

	public static bool EnforceSiteIdInModuleWrapper => ConfigHelper.GetBoolProperty("EnforceSiteIdInModuleWrapper", true);

	public static bool UseFullUrlsForSkins => ConfigHelper.GetBoolProperty("UseFullUrlsForSkins", false);

	public static bool UseClosestAsciiCharsForUrls => ConfigHelper.GetBoolProperty("UseClosestAsciiCharsForUrls", true);

	public static bool AlwaysUrlEncode => ConfigHelper.GetBoolProperty("AlwaysUrlEncode", false);

	public static bool RetryUnencodedOnUrlNotFound => ConfigHelper.GetBoolProperty("RetryUnencodedOnUrlNotFound", false);

	public static bool ForceFriendlyUrlsToLowerCase => ConfigHelper.GetBoolProperty("ForceFriendlyUrlsToLowerCase", true);

	public static bool SetMaintainScrollPositionOnPostBackTrueOnCmsPages => ConfigHelper.GetBoolProperty("SetMaintainScrollPositionOnPostBackTrueOnCmsPages", false);

	public static bool FileSystemIsWritable => ConfigHelper.GetBoolProperty("FileSystemIsWritable", true);

	public static bool FileManagerOverwriteFiles => ConfigHelper.GetBoolProperty("FileManagerOverwriteFiles", true);

	/// <summary>
	/// defaults to -1 which means not specified which means use the global default from the httpRuntime element
	/// which defaults to 110 seconds
	/// for large downloads you may need to set a number here higher than 110
	/// </summary>
	public static int DownloadScriptTimeout => ConfigHelper.GetIntProperty("DownloadScriptTimeout", -1);

	public static bool ImageGalleryUseMediaFolder => ConfigHelper.GetBoolProperty("ImageGalleryUseMediaFolder", true);

	public static string ImageCropperWrapperDivStyle => ConfigHelper.GetStringProperty("ImageCropperWrapperDivStyle", "width:100%;height:100%;overflow-x:scroll;overflow-y:visible;");

	public static long UserFolderDiskQuotaInMegaBytes => ConfigHelper.GetLongProperty("UserFolderDiskQuotaInMegaBytes", 300);

	public static long MediaFolderDiskQuotaInMegaBytes => ConfigHelper.GetLongProperty("MediaFolderDiskQuotaInMegaBytes", 6000);

	public static long AdminDiskQuotaInMegaBytes => ConfigHelper.GetLongProperty("AdminDiskQuotaInMegaBytes", 12000);

	public static long UserFolderMaxSizePerFileInMegaBytes => ConfigHelper.GetLongProperty("UserFolderMaxSizePerFileInMegaBytes", 10);

	public static long MediaFolderMaxSizePerFileInMegaBytes => ConfigHelper.GetLongProperty("MediaFolderMaxSizePerFileInMegaBytes", 30);

	public static long AdminMaxSizePerFileInMegaBytes => ConfigHelper.GetLongProperty("AdminMaxSizePerFileInMegaBytes", 2048);

	public static int UserFolderMaxNumberOfFiles => ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFiles", 1000);

	public static int MediaFolderMaxNumberOfFiles => ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFiles", 10000);

	public static int AdminMaxNumberOfFiles => ConfigHelper.GetIntProperty("AdminMaxNumberOfFiles", 100000);

	public static int UserFolderMaxNumberOfFolders => ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFolders", 50);

	public static int MediaFolderMaxNumberOfFolders => ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFolders", 500);

	public static int AdminMaxNumberOfFolders => ConfigHelper.GetIntProperty("AdminMaxNumberOfFolders", 1000);

	public static int MaxSkinFilesToUploadAtOnce => ConfigHelper.GetIntProperty("MaxSkinFilesToUploadAtOnce", 10);

	public static int MaxFileManagerFilesToUploadAtOnce => ConfigHelper.GetIntProperty("MaxFileManagerFilesToUploadAtOnce", 20);

	public static bool RequireFileSystemServiceToken => ConfigHelper.GetBoolProperty("RequireFileSystemServiceToken", true);

	public static bool AllowFileEditInFileManager => ConfigHelper.GetBoolProperty("AllowFileEditInFileManager", true);

	public static bool AllowEditingSkins => ConfigHelper.GetBoolProperty("AllowEditingSkins", true);

	public static bool UseFailSafeMasterPageOnError => ConfigHelper.GetBoolProperty("UseFailSafeMasterPageOnError", true);

	public static bool AllowEditingSkinsInChildSites => ConfigHelper.GetBoolProperty("AllowEditingSkinsInChildSites", true);

	public static bool CheckFishyReferrer => ConfigHelper.GetBoolProperty("CheckFishyReferer", true);

	public static bool LogFishyReferrer => ConfigHelper.GetBoolProperty("LogFishyReferer", true);

	/// <summary>
	/// this is mainly needed so I can prevent people from changing the mobile skin on the demo site
	/// </summary>
	public static bool AllowSettingMobileSkinInChildSites => ConfigHelper.GetBoolProperty("AllowSettingMobileSkinInChildSites", true);

	public static string DefaultInitialSkin => ConfigHelper.GetStringProperty("DefaultInitialSkin", "framework");


	public static string AllowedSkinFileExtensions => ConfigHelper.GetStringProperty("AllowedSkinFileExtensions", ".master|.skin|.css|.jpg|.jpeg|.png|.gif|.ico|.txt|.config|.js|.webm|.weba|.webp|.html|.xml|.less|.eot|.otf|.woff|.ttf|.svg|.cshtml");

	public static bool DebugSkinImporter => ConfigHelper.GetBoolProperty("DebugSkinImporter", false);

	public static bool IncludeContentStylesWithSkinExport => ConfigHelper.GetBoolProperty("IncludeContentStylesWithSkinExport", false);

	public static string ImageFileExtensions => ConfigHelper.GetStringProperty("ImageFileExtensions", ".gif|.jpg|.jpeg|.png|.svg|.webp");

	public static string AllowedMediaFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AllowedMediaFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AllowedMediaFileExtensions"].ToLower();
			}
			// default value
			return $"{ImageFileExtensions}|{AudioFileExtensions}|{VideoFileExtensions}";
		}
	}

	public static string AudioFileExtensions => ConfigHelper.GetStringProperty("AudioFileExtensions", ".wmv|.mp3|.m4a|.m4v|.oga|.webma|.webm|.wav|.asf|.asx").ToLower();

	public static string VideoFileExtensions => ConfigHelper.GetStringProperty("VideoFileExtensions", ".wmv|.mp4|.m4v|.ogv|.webmv|.webm|.avi|.mov|.mpeg|.mpg").ToLower();

	public static string JPlayerAudioFileExtensions => ConfigHelper.GetStringProperty("JPlayerAudioFileExtensions", ".mp3|.m4a|.mp4|.oga|.webma|.webm|.wav").ToLower();

	public static string JPlayerVideoFileExtensions => ConfigHelper.GetStringProperty("JPlayerVideoFileExtensions", ".m4v|.mp4|.ogv|.webmv|.webm|.ogg").ToLower();

	public static string JPlayerBasePath => ConfigHelper.GetStringProperty("JPlayerBasePath", "~/ClientScript/jplayer-2-5-0/");

	public static string AllowedUploadFileExtensions => ConfigHelper.GetStringProperty("AllowedUploadFileExtensions", ".gif|.jpg|.jpeg|.svg|.png|.wmv|.mp3|.mp4|.tif|.asf|.asx|.avi|.mov|.mpeg|.mpg|.zip|.pdf|.doc|.docx|.xls|.xlsx|.ppt|.pptx|.csv|.txt").ToLower();

	public static string AllowedLessPriveledgedUserUploadFileExtensions => ConfigHelper.GetStringProperty("AllowedLessPriveledgedUserUploadFileExtensions", ".gif|.jpg|.jpeg|.png|.svg|.zip").ToLower();

	public static string mojoFileSystemConfigFileName => ConfigHelper.GetStringProperty("mojoFileSystemConfigFileName", "mojoFileSystem.config");

	public static string FileSystemProvider => ConfigHelper.GetStringProperty("FileSystemProvider", "DiskFileSystemProvider");

	public static System.Drawing.Color DefaultResizeBackgroundColor
	{
		get
		{
			if (ConfigurationManager.AppSettings["DefaultResizeHexBackgroundColor"] != null)
			{
				return System.Drawing.ColorTranslator.FromHtml(ConfigurationManager.AppSettings["DefaultResizeHexBackgroundColor"]);
			}

			if (ConfigurationManager.AppSettings["DefaultResizeBackgroundColor"] != null)
			{
				return System.Drawing.Color.FromName(ConfigurationManager.AppSettings["DefaultResizeBackgroundColor"]);
			}

			return System.Drawing.Color.White;
		}
	}


	/// <summary>
	/// if a user is in a role that allows both uploading and deleting then they will have access to the main file manager
	/// in some cases you may want to allow users who can only upload to user specific folders to delete files from the editor file browser
	/// without giving them access to the general File Manager, to do that you could set this to true
	/// </summary>
	public static bool AllowDeletingFilesFromUserFolderWithoutDeleteRole => ConfigHelper.GetBoolProperty("AllowDeletingFilesFromUserFolderWithoutDeleteRole", false);

	public static bool EnableInlineEditing => ConfigHelper.GetBoolProperty("EnableInlineEditing", true);

	public static string CKEditorBasePath
	{
		get
		{
			string defaultPath = "~/ClientScript/ckeditor4112/";
			string path = ConfigHelper.GetStringProperty("CKEditor:BasePath", defaultPath);

			return string.IsNullOrWhiteSpace(path) ? defaultPath : path;
		}
	}

	public static string CKEditorConfigPath
	{
		get
		{
			string defaultPath = "~/ClientScript/ckeditor-mojoconfig.js";
			string path = ConfigHelper.GetStringProperty("CKEditor:ConfigPath", defaultPath);

			return string.IsNullOrWhiteSpace(path) ? defaultPath : path;
		}
	}

	public static string CKEditorFullWithTemplatesToolbarDefinition => ConfigHelper.GetStringProperty("CKEditor:FullWithTemplatesToolbarDefinition", "[['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],['Undo','Redo','-','Find','Replace','Bold','Italic','Underline','Strike'],'/',['Blockquote','Styles'],['NumberedList','BulletedList'],['Link','Unlink','Anchor'],['Templates','Image','oembed','Table','HorizontalRule','Smiley','SpecialChar'],];");

	public static bool ResizeEditorUploadedImages => ConfigHelper.GetBoolProperty("ResizeEditorUploadedImages", true);

	public static bool KeepFullSizeEditorUploadedImages => ConfigHelper.GetBoolProperty("KeepFullSizeEditorUploadedImages", false);

	public static bool KeepFullSizeImagesDroppedInEditor => ConfigHelper.GetBoolProperty("KeepFullSizeImagesDroppedInEditor", true);

	public static int ResizeImageDefaultMaxWidth => ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550);

	public static int ResizeImageDefaultMaxHeight => ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550);

	public static bool AvatarsCanOnlyBeUploadedByAdmin => ConfigHelper.GetBoolProperty("AvatarsCanOnlyBeUploadedByAdmin", false);

	public static bool ForceSquareAvatars => ConfigHelper.GetBoolProperty("ForceSquareAvatars", true);

	public static int AvatarMaxOriginalWidth => ConfigHelper.GetIntProperty("AvatarMaxOriginalWidth", 800);

	public static int AvatarMaxOriginalHeight => ConfigHelper.GetIntProperty("AvatarMaxOriginalHeight", 800);

	public static int AvatarMaxWidth => ConfigHelper.GetIntProperty("AvatarMaxWidth", 90);

	public static int AvatarMaxHeight => ConfigHelper.GetIntProperty("AvatarMaxHeight", 90);

	public static string DefaultBlankAvatarPath => ConfigHelper.GetStringProperty("DefaultBlankAvatarPath", "~/Data/SiteImages/1x1.gif");

	public static string UrlRegex => ConfigHelper.GetStringProperty("UrlRegex", @"\b(([\w-]+://?|www[.])[^\s()<>]+(?:\([\w\d]+\)|([^[:punct:]\s]|/)))");

	public static string CustomEmailRegex => ConfigHelper.GetStringProperty("CustomEmailRegex", string.Empty);

	public static string CustomEmailRegexWarning => ConfigHelper.GetStringProperty("CustomEmailRegexWarning", string.Empty);

	/// <summary>
	/// not recommended to set true but it was requested to support this
	/// if true the remember me checkbox will be hidden and forcibly checked
	/// which results in a persistent auth cookie upon login
	/// </summary>
	public static bool ForcePersistentAuthCheckboxChecked => ConfigHelper.GetBoolProperty("ForcePersistentAuthCheckboxChecked", false);

	public static string PageTreeRelativeUrl => ConfigHelper.GetStringProperty("PageTreeRelativeUrl", $"{AdminDirectoryLocation}/PageManager.aspx");

	public static string ContentPublishPageRelativeUrl => ConfigHelper.GetStringProperty("ContentPublishPageRelativeUrl", $"{AdminDirectoryLocation}/ContentManager.aspx");

	public static string FileDialogRelativeUrl => ConfigHelper.GetStringProperty("FileDialogRelativeUrl", "/FileManager");

	/// <summary>
	/// The filesystem location of the Admin directory. This is not configurable yet but will be to allow renaming of the Admin directory
	/// to obfuscate it.
	/// </summary>
	public static string AdminDirectoryLocation => "/Admin";

	public static bool RedirectRegistrationPageToCustomPage => ConfigHelper.GetBoolProperty("RedirectRegistrationPageToCustomPage", false);

	public static bool UseRedirectInSignInModule => ConfigHelper.GetBoolProperty("UseRedirectInSignInModule", true);

	public static string PageToRedirectToAfterRegistration => ConfigHelper.GetStringProperty("PageToRedirectToAfterRegistration", string.Empty);

	public static string CKEditorH1Mapping => ConfigHelper.GetStringProperty("CKEditorH1Mapping", "h3");

	public static string CKEditorH2Mapping => ConfigHelper.GetStringProperty("CKEditorH2Mapping", "h4");

	public static string CKEditorH3Mapping => ConfigHelper.GetStringProperty("CKEditorH3Mapping", "h5");

	public static string EditorTemplatesPath => ConfigHelper.GetStringProperty("EditorTemplatesPath", "~/ClientScript/mojo-editor-templates.xml");

	public static string EditAreaBasePath => ConfigHelper.GetStringProperty("EditAreaBasePath", "~/ClientScript/edit_area0811/");

	public static string RequestApprovalImage => ConfigHelper.GetStringProperty("RequestApprovalImage", "~/Data/SiteImages/glasses.png");

	public static string ApproveContentImage => ConfigHelper.GetStringProperty("ApproveContentImage", "~/Data/SiteImages/done_cover.png");

	public static string PublishContentImage => ConfigHelper.GetStringProperty("PublishContentImage", "~/Data/SiteImages/plus.png");

	public static string RejectContentImage => ConfigHelper.GetStringProperty("RejectContentImage", "~/Data/SiteImages/minus_circle.png");

	public static string CancelContentChangesImage => ConfigHelper.GetStringProperty("CancelContentChangesImage", "~/Data/SiteImages/x-circle.png");

	public static string RobotsConfigFile => ConfigHelper.GetStringProperty("RobotsConfigFile", "~/robots.config");

	public static string RobotsSslConfigFile => ConfigHelper.GetStringProperty("RobotsSslConfigFile", "~/robots.ssl.config");

	public static bool ShowRebuildSearchIndexButtonToAdmins => ConfigHelper.GetBoolProperty("ShowRebuildSearchIndexButtonToAdmins", false);

	public static bool ShowCustomProfilePropertiesAboveManadotoryRegistrationFields => ConfigHelper.GetBoolProperty("ShowCustomProfilePropertiesAboveManadotoryRegistrationFields", false);

	public static bool AllowUserThreadBrowsing => ConfigHelper.GetBoolProperty("AllowUserThreadBrowsing", true);

	public static bool ShowUseUrlSettingInPageSettings => ConfigHelper.GetBoolProperty("ShowUseUrlSettingInPageSettings", false);

	public static string SetupHeaderConfigPath => ConfigHelper.GetStringProperty("SetupHeaderConfigPath", "~/Setup/SetupHeader.config");

	public static string SetupHeaderConfigPathRtl => ConfigHelper.GetStringProperty("SetupHeaderConfigPathRtl", "~/Setup/SetupHeader-rtl.config");

	public static string SetupFooterConfigPath => ConfigHelper.GetStringProperty("SetupFooterConfigPath", "~/Setup/SetupFooter.config");

	public static string SetupFooterConfigPathRtl => ConfigHelper.GetStringProperty("SetupFooterConfigPathRtl", "~/Setup/SetupFooter-rtl.config");

	public static string DefaultCountry => ConfigHelper.GetStringProperty("DefaultCountry", "US");

	public static bool AlwaysShowAltPanesInPublishDialog => ConfigHelper.GetBoolProperty("AlwaysShowAltPanesInPublishDialog", false);

	public static int TooManyPagesForDropdownList => ConfigHelper.GetIntProperty("TooManyPagesForDropdownList", 150);

	public static int TooManyRolesForPageSettings => ConfigHelper.GetIntProperty("TooManyRolesForPageSettings", 20);

	public static int TooManyRolesForModuleSettings => ConfigHelper.GetIntProperty("TooManyRolesForModuleSettings", 20);

	public static int TooManyRolesForManageUserPage => ConfigHelper.GetIntProperty("TooManyRolesForManageUserPage", 20);

	public static int TooManyPagesForGridEvents => ConfigHelper.GetIntProperty("TooManyPagesForGridEvents", 150);

	/// <summary>
	/// valid options: TitleOnly, SitePlusTitle, TitlePlusSite
	/// generally you should not call this directly but use the corresponding method in SiteUtils
	/// </summary>
	public static string PageTitleFormatName => ConfigHelper.GetStringProperty("PageTitleFormatName", "SitePlusTitle");


	/// <summary>
	/// used to separate the site and page title when PageTitleFormatName is SitePlusTitle or TitlePlusSite
	/// generally you should not call this directly but use the corresponding method in SiteUtils
	/// </summary>
	public static string PageTitleSeparatorString => ConfigHelper.GetStringProperty("PageTitleSeparatorString", " - ");


	/// <summary>
	/// if true will process the cms page override title with the same format as used for autogenerated titles based on the page name and site name and format settings
	/// </summary>
	public static bool FormatOverridePageTitle => ConfigHelper.GetBoolProperty("FormatOverridePageTitle", true);

	public static string GoogleMapsAPIKey => ConfigHelper.GetStringProperty("GoogleMapsAPIKey", string.Empty);

	/// <summary>
	/// valid options are internal, google, bing
	/// using google or bing requires a bingapi id or a google custom search id
	/// </summary>
	public static string PrimarySearchEngine => ConfigHelper.GetStringProperty("PrimarySearchEngine", "");

	public static string BingAPIId => ConfigHelper.GetStringProperty("BingAPIId", "");

	public static string GoogleCustomSearchId => ConfigHelper.GetStringProperty("GoogleCustomSearchId", "");

	public static string CustomSearchDomain => ConfigHelper.GetStringProperty("CustomSearchDomain", "");
	public static int BingSearchPageSize => ConfigHelper.GetIntProperty("BingSearchPageSize", 30);

	public static string BingApiUrl => ConfigHelper.GetStringProperty("BingApiUrl", "https://api.datamarket.azure.com/Bing/SearchWeb/");

	public static string Custom404Page => ConfigHelper.GetStringProperty("Custom404Page", "~/PageNotFound.aspx");

	public static bool EnableGoogle404Enhancement => ConfigHelper.GetBoolProperty("EnableGoogle404Enhancement", true);

	public static string ExtensionsToSkipIn404Handler
	{
		get
		{
			if (ConfigurationManager.AppSettings["ExtensionsToSkipIn404Handler"] != null)
			{
				return ConfigurationManager.AppSettings["ExtensionsToSkipIn404Handler"].ToLower();
			}
			// default value
			return $"{ImageFileExtensions}|{AudioFileExtensions}|{VideoFileExtensions}.js|.css|.ashx|.axd".ToLower();
		}
	}

	public static bool GoogleAnalyticsForceUniversal => ConfigHelper.GetBoolProperty("GoogleAnalyticsForceUniversal", false);

	public static string GoogleAnalyticsRolesToExclude => ConfigHelper.GetStringProperty("GoogleAnalyticsRolesToExclude", "");

	public static string GoogleAnalyticsMemberTypeAnonymous => ConfigHelper.GetStringProperty("GoogleAnalyticsMemberTypeAnonymous", "anonymous");

	public static string GoogleAnalyticsMemberLabel => ConfigHelper.GetStringProperty("GoogleAnalyticsMemberLabel", "member-type");

	public static string GoogleAnalyticsSectionLabel => ConfigHelper.GetStringProperty("GoogleAnalyticsSectionLabel", "section");

	public static string GoogleAnalyticsMemberTypeAuthenticated => ConfigHelper.GetStringProperty("GoogleAnalyticsMemberTypeAuthenticated", "member");

	public static string GoogleAnalyticsMemberTypeCustomer => ConfigHelper.GetStringProperty("GoogleAnalyticsMemberTypeCustomer", "customer");

	public static string GoogleAnalyticsMemberTypeAdmin => ConfigHelper.GetStringProperty("GoogleAnalyticsMemberTypeAdmin", "admin");

	public static string BodyElementClientIDMode => ConfigHelper.GetStringProperty("BodyElementClientIDMode", "Static");

	public static string HeadElementClientIDMode => ConfigHelper.GetStringProperty("HeadElementClientIDMode", "Static");

	/// <summary>
	/// Allows for automatic creation of MachineKey during Setup. 
	/// FALSE by default, set to TRUE in user.config.sample
	/// Requires web user to have write permissions to root of website installation directory
	/// </summary>
	public static bool TryEnsureCustomMachineKeyOnSetup => ConfigHelper.GetBoolProperty("TryEnsureCustomMachineKeyOnSetup", false);

	public static bool AllowUpdateCheck => ConfigHelper.GetBoolProperty("AllowUpdateCheck", true);

	public static bool SecurityAdvisorLogTLSCheckResponse => ConfigHelper.GetBoolProperty("SecurityAdvisorLogTLSCheckResponse", false);

	// Supported values are: MD5, SHA1, HMACSHA256, HMACSHA385, HMACSHA512
	public static string MachineKeyValidationAlgorithm => ConfigHelper.GetStringProperty("MachineKeyValidationAlgorithm", "HMACSHA256");

	// Supported values are: AES, DES, 3DES
	public static string MachineKeyDecryptionAlgorithm => ConfigHelper.GetStringProperty("MachineKeyDecryptionAlgorithm", "3DES");

	/// <summary>
	/// calls to this method should be made inside a try catch log
	/// we don't expect the Web.config file to be writable in general but it usually is on a new 
	/// installation so we can go ahead and try to update to a custom machine key
	/// </summary>
	public static void EnsureCustomMachineKey()
	{
		var securityAdvisor = new SecurityAdvisor();

		if (securityAdvisor.UsingCustomMachineKey())
		{
			return; // already using a custom key
		}

		var webConfigPath = HostingEnvironment.MapPath("~/Web.config");

		var xmlConfig = XmlHelper.GetXmlDocument(webConfigPath);

		var xmlMachineKey = xmlConfig.SelectSingleNode("/configuration/location/system.web/machineKey");

		xmlMachineKey ??= xmlConfig.SelectSingleNode("/configuration/system.web/machineKey");

		var (validationKey, decryptionKey, _, _) = SiteUtils.GenerateRandomMachineKey();

		XmlAttribute attrib = xmlMachineKey.Attributes["validationKey"];

		attrib.InnerText = validationKey;
		attrib = xmlMachineKey.Attributes["decryptionKey"];
		attrib.InnerText = decryptionKey;

		var writer = new XmlTextWriter(webConfigPath, null)
		{
			Formatting = Formatting.Indented
		};

		xmlConfig.WriteTo(writer);

		writer.Flush();
		writer.Close();
	}
}