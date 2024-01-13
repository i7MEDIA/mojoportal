using System;
using System.Configuration;
using System.Web.Hosting;
using System.Xml;
using mojoPortal.Core.Configuration;

namespace mojoPortal.Web;

public static class WebConfigSettings
{
	public static bool UseSiteIdAppThemesInMediumTrust
	{
		get { return ConfigHelper.GetBoolProperty("UseSiteIdAppThemesInMediumTrust", false); }
	}

	public static bool EnableOpenIdAuthentication
	{
		get { return ConfigHelper.GetBoolProperty("EnableOpenIDAuthentication", false); }
	}

	public static bool DisableRpxAuthentication
	{
		get { return ConfigHelper.GetBoolProperty("DisableRpxAuthentication", false); }
	}

	public static bool ShowLegacyOpenIDSelector
	{
		get { return ConfigHelper.GetBoolProperty("ShowLegacyOpenIDSelector", false); }
	}

	public static bool XmlUseMediaFolder
	{
		get { return ConfigHelper.GetBoolProperty("XMLUseMediaFolder", false); }
	}

	public static bool SiteLogoUseMediaFolder
	{
		get { return ConfigHelper.GetBoolProperty("SiteLogoUseMediaFolder", false); }
	}

	public static bool HtmlFragmentUseMediaFolder
	{
		get { return ConfigHelper.GetBoolProperty("HtmlFragmentUseMediaFolder", false); }
	}

	public static bool DisableGoogleTranslate
	{
		get { return ConfigHelper.GetBoolProperty("DisableGoogleTranslate", false); }
	}

	public static bool UseOpenIdRpxSettingsFromWebConfig
	{
		get { return ConfigHelper.GetBoolProperty("UseOpenIdRpxSettingsFromWebConfig", false); }
	}

	/// <summary>
	/// If true then we will store our userGuid in the rpx server and allow multiple open id identifiers to be used
	/// with a single mojoportal account
	/// </summary>
	public static bool OpenIdRpxUseMappings
	{
		get { return ConfigHelper.GetBoolProperty("OpenIdRpxUseMappings", true); }
	}

	// 2013-05-26 changed from true to false not sure why it was true
	public static bool OpenIdRpxUseOldImplementation
	{
		get { return ConfigHelper.GetBoolProperty("OpenIdRpxUseOldImplementation", false); }
	}

	public static string OpenIdRpxApiKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["OpenIdRpxApiKey"] != null)
			{
				return ConfigurationManager.AppSettings["OpenIdRpxApiKey"];
			}
			return string.Empty;
		}
	}

	/// <summary>
	/// to support proxy servers where the client ip may be passed in a custom server variable
	/// </summary>
	public static string ClientIpServerVariable
	{
		get
		{
			if (ConfigurationManager.AppSettings["ClientIpServerVariable"] != null)
			{
				return ConfigurationManager.AppSettings["ClientIpServerVariable"];
			}
			return string.Empty;
		}
	}

	/// <summary>
	/// to suport proxy servers which may use non standard variables
	/// http://www.mojoportal.com/Forums/Thread.aspx?thread=7424&mid=34&pageid=5&ItemID=5&pagenumber=1#post30680
	/// </summary>
	public static string RemoteHostServerVariable
	{
		get
		{
			if (ConfigurationManager.AppSettings["RemoteHostServerVariable"] != null)
			{
				return ConfigurationManager.AppSettings["RemoteHostServerVariable"];
			}
			return "REMOTE_HOST";
		}
	}

	/// <summary>
	/// this can be used to detect a secure request in a proxied environment when the mere presence of a specific server variable indicates a secure connection
	/// for example this can be used with IIS AAR (Application Request Routing Module) where the presence of a server variable named HTTP_X_ARR_SSL indicates a secure request
	/// So you would add this to user.config  <add key="SecureConnectionServerVariableForPresenceCheck" value="HTTP_X_ARR_SSL"/>
	/// This setting is checked in WebHelper.IsSecureRequest();
	/// </summary>
	[Obsolete("Use mojoPortal.Core.Configuration.AppConfig")]
	public static string SecureConnectionServerVariableForPresenceCheck
	{
		get
		{
			return AppConfig.SecureConnectionServerVariableForPresenceCheck;
		}
	}

	/// <summary>
	/// use this if you need to check a custom server variable for a specific value to determine a secure request
	/// you must also set the value for SecureConnectionServerVariableSecureValue that corresponds to a secure request
	/// </summary>
	[Obsolete("Use mojoPortal.Core.Configuration.AppConfig")]
	public static string SecureConnectionServerVariableForValueCheck
	{
		get
		{
			return AppConfig.SecureConnectionServerVariableForValueCheck;
		}
	}

	[Obsolete("Use mojoPortal.Core.Configuration.AppConfig")]
	public static string SecureConnectionServerVariableSecureValue
	{
		get
		{
			return AppConfig.SecureConnectionServerVariableSecureValue;
		}
	}

	public static string OpenIdRpxApplicationName
	{
		get
		{
			if (ConfigurationManager.AppSettings["OpenIdRpxApplicationName"] != null)
			{
				return ConfigurationManager.AppSettings["OpenIdRpxApplicationName"];
			}
			return string.Empty;
		}
	}

	public static bool EnableWindowsLiveAuthentication
	{
		get { return ConfigHelper.GetBoolProperty("EnableWindowsLiveAuthentication", false); }
	}

	public static bool HideDisableDbAuthenticationSetting
	{
		get { return ConfigHelper.GetBoolProperty("HideDisableDbAuthenticationSetting", false); }
	}


	public static bool GloballyDisableMemberUseOfWindowsLiveMessenger
	{
		get { return ConfigHelper.GetBoolProperty("GloballyDisableMemberUseOfWindowsLiveMessenger", true); }
	}

	public static bool TestLiveMessengerDelegation
	{
		get { return ConfigHelper.GetBoolProperty("TestLiveMessengerDelegation", false); }
	}

	public static bool EncodeLiveMessengerToken
	{
		get { return ConfigHelper.GetBoolProperty("EncodeLiveMessengerToken", false); }
	}

	public static bool DebugWindowsLive
	{
		get { return ConfigHelper.GetBoolProperty("DebugWindowsLive", false); }
	}

	public static bool DebugDotLess
	{
		get { return ConfigHelper.GetBoolProperty("DebugDotLess", false); }
	}

	public static bool DebugPayPal
	{
		get { return ConfigHelper.GetBoolProperty("DebugPayPal", false); }
	}

	public static bool DebugLoginRedirect
	{
		get { return ConfigHelper.GetBoolProperty("DebugLoginRedirect", false); }
	}

	public static bool DebugOpenID
	{
		get { return ConfigHelper.GetBoolProperty("DebugOpenID", false); }
	}

	public static bool EnableTaskQueueTestLinks
	{
		get { return ConfigHelper.GetBoolProperty("EnableTaskQueueTestLinks", false); }
	}


	public static bool DisableSetup
	{
		get { return ConfigHelper.GetBoolProperty("DisableSetup", false); }
	}

	public static string ContentPagesToSkip
	{
		get { return ConfigHelper.GetStringProperty("ContentPagesToSkip", string.Empty); }
	}


	public static string PaymentGatewayConfigFileName
	{
		get { return ConfigHelper.GetStringProperty("PaymentGatewayConfigFileName", "mojoPaymentGateway.config"); }
	}

	/// <summary>
	/// for when you want to use the same sommerce settings in any child sites
	/// </summary>
	public static bool CommerceUseGlobalSettings
	{
		get { return ConfigHelper.GetBoolProperty("CommerceUseGlobalSettings", false); }
	}

	public static bool CommerceGlobalIs503TaxExempt
	{
		get { return ConfigHelper.GetBoolProperty("CommerceGlobalIs503TaxExempt", false); }
	}

	public static bool CommerceGlobalUseTestMode
	{
		get { return ConfigHelper.GetBoolProperty("CommerceGlobalUseTestMode", false); }
	}

	public static string CommerceGlobalPrimaryGateway
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPrimaryGateway", string.Empty); }
	}

	public static string CommerceGlobalAuthorizeNetProductionAPILogin
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetProductionAPILogin", string.Empty); }
	}

	public static string CommerceGlobalAuthorizeNetProductionAPITransactionKey
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetProductionAPITransactionKey", string.Empty); }
	}

	public static string CommerceGlobalAuthorizeNetSandboxAPILogin
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetSandboxAPILogin", string.Empty); }
	}

	public static string CommerceGlobalAuthorizeNetSandboxAPITransactionKey
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalAuthorizeNetSandboxAPITransactionKey", string.Empty); }
	}

	public static string CommerceGlobalPlugNPayProductionAPIPublisherName
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPlugNPayProductionAPIPublisherName", string.Empty); }
	}

	public static string CommerceGlobalPlugNPayProductionAPIPublisherPassword
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPlugNPayProductionAPIPublisherPassword", string.Empty); }
	}

	public static string CommerceGlobalPlugNPaySandboxAPIPublisherName
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPlugNPaySandboxAPIPublisherName", string.Empty); }
	}

	public static string CommerceGlobalPlugNPaySandboxAPIPublisherPassword
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPlugNPaySandboxAPIPublisherPassword", string.Empty); }
	}

	public static string CommerceGlobalPayPalAccountProductionEmailAddress
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountProductionEmailAddress", string.Empty); }
	}

	public static string CommerceGlobalPayPalAccountProductionPDTId
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountProductionPDTId", string.Empty); }
	}

	public static string CommerceGlobalPayPalAccountSandboxEmailAddress
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountSandboxEmailAddress", string.Empty); }
	}

	public static string CommerceGlobalPayPalAccountSandboxPDTId
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalAccountSandboxPDTId", string.Empty); }
	}

	public static bool CommerceGlobalUsePayPalStandard
	{
		get { return ConfigHelper.GetBoolProperty("CommerceGlobalUsePayPalStandard", true); }
	}

	public static string CommerceGlobalPayPalStandardProductionUrl
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalStandardProductionUrl", string.Empty); }
	}

	public static string CommerceGlobalPayPalStandardSandboxUrl
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalStandardSandboxUrl", string.Empty); }
	}

	public static string CommerceGlobalPayPalProductionAPIUsername
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPIUsername", string.Empty); }
	}

	public static string CommerceGlobalPayPalProductionAPIPassword
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPIPassword", string.Empty); }
	}

	public static string CommerceGlobalPayPalProductionAPISignature
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalProductionAPISignature", string.Empty); }
	}

	public static string CommerceGlobalPayPalSandboxAPIUsername
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPIUsername", string.Empty); }
	}

	public static string CommerceGlobalPayPalSandboxAPIPassword
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPIPassword", string.Empty); }
	}

	public static string CommerceGlobalPayPalSandboxAPISignature
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalPayPalSandboxAPISignature", string.Empty); }
	}

	public static string CommerceGlobalGoogleProductionMerchantID
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalGoogleProductionMerchantID", string.Empty); }
	}

	public static string CommerceGlobalGoogleProductionMerchantKey
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalGoogleProductionMerchantKey", string.Empty); }
	}

	public static string CommerceGlobalGoogleSandboxMerchantID
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalGoogleSandboxMerchantID", string.Empty); }
	}

	public static string CommerceGlobalGoogleSandboxMerchantKey
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalGoogleSandboxMerchantKey", string.Empty); }
	}

	public static string CommerceGlobalWorldPayInstallationId
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayInstallationId", string.Empty); }
	}

	public static string CommerceGlobalWorldPayMerchantCode
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayMerchantCode", string.Empty); }
	}

	public static string CommerceGlobalWorldPayMd5Secret
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayMd5Secret", string.Empty); }
	}

	public static string CommerceGlobalWorldPayResponsePassword
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayResponsePassword", string.Empty); }
	}

	public static string CommerceGlobalWorldPayShopperResponseTemplate
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayShopperResponseTemplate", string.Empty); }
	}

	public static string CommerceGlobalWorldPayShopperCancellationResponseTemplate
	{
		get { return ConfigHelper.GetStringProperty("CommerceGlobalWorldPayShopperCancellationResponseTemplate", string.Empty); }
	}

	public static bool CommerceGlobalWorldPayProduceShopperResponse
	{
		get { return ConfigHelper.GetBoolProperty("CommerceGlobalWorldPayProduceShopperResponse", true); }
	}

	public static bool CommerceGlobalWorldPayProduceShopperCancellationResponse
	{
		get { return ConfigHelper.GetBoolProperty("CommerceGlobalWorldPayProduceShopperCancellationResponse", true); }
	}

	public static bool SetupTryAnywayIfFailedAlterSchemaTest
	{
		get { return ConfigHelper.GetBoolProperty("SetupTryAnywayIfFailedAlterSchemaTest", false); }
	}

	public static bool DisableDBAdminTool
	{
		get { return ConfigHelper.GetBoolProperty("DisableDBAdminTool", true); }
	}

	public static bool MaskPasswordsInUserAdmin
	{
		get { return ConfigHelper.GetBoolProperty("MaskPasswordsInUserAdmin", true); }
	}

	public static bool ShowProviderListInDBAdminTool
	{
		get { return ConfigHelper.GetBoolProperty("ShowProviderListInDBAdminTool", false); }
	}

	public static string MemberListUrl
	{
		get { return ConfigHelper.GetStringProperty("MemberListUrl", "/MemberList.aspx"); }
	}

	public static string MemberListOverrideLinkText
	{
		get { return ConfigHelper.GetStringProperty("MemberListOverrideLinkText", string.Empty); }
	}

	public static bool ShowEmailInMemberList
	{
		get { return ConfigHelper.GetBoolProperty("ShowEmailInMemberList", false); }
	}

	public static bool ShowPurgeUserLocationsInUserManagement
	{
		get { return ConfigHelper.GetBoolProperty("ShowPurgeUserLocationsInUserManagement", true); }
	}

	public static bool ShowForumUnsubscribeLinkInUserManagement
	{
		get { return ConfigHelper.GetBoolProperty("ShowForumUnsubscribeLinkInUserManagement", true); }
	}

	public static bool ShowRevenueInForums
	{
		get { return ConfigHelper.GetBoolProperty("ShowRevenueInForums", false); }
	}

	public static bool GetAlphaPagerCharsFromResourceFile
	{
		get { return ConfigHelper.GetBoolProperty("GetAlphaPagerCharsFromResourceFile", false); }
	}

	public static string AlphaPagerChars
	{
		get { return ConfigHelper.GetStringProperty("AlphaPagerChars", "ABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
	}

	public static bool AdaptHtmlDirectionToCulture
	{
		get { return ConfigHelper.GetBoolProperty("AdaptHtmlDirectionToCulture", false); }
	}

	public static bool AddLangToHtmlElement
	{
		get { return ConfigHelper.GetBoolProperty("AddLangToHtmlElement", true); }
	}

	/*
	 * Menu hiding options. As of version 2.8, these are in the <portal:CoreDisplaySettings /> 
	 * control used in the theme.skin.
	 */
	public static bool HideAllMenusOnSiteClosedPage
	{
		get { return ConfigHelper.GetBoolProperty("HideAllMenusOnSiteClosedPage", true); }
	}

	public static bool HideMenusOnLoginPage
	{
		get { return ConfigHelper.GetBoolProperty("HideMenusOnLoginPage", true); }
	}

	public static bool HideMenusOnSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("HideMenusOnSiteMap", true); }
	}

	public static bool HidePageMenusOnSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("HidePageMenusOnSiteMap", true); }
	}

	public static bool HideMenusOnRegisterPage
	{
		get { return ConfigHelper.GetBoolProperty("HideMenusOnRegisterPage", true); }
	}

	public static bool HideMenusOnPasswordRecoveryPage
	{
		get { return ConfigHelper.GetBoolProperty("HideMenusOnPasswordRecoveryPage", true); }
	}

	public static bool HideMenusOnChangePasswordPage
	{
		get { return ConfigHelper.GetBoolProperty("HideMenusOnChangePasswordPage", true); }
	}

	public static bool HideAllMenusOnProfilePage
	{
		get { return ConfigHelper.GetBoolProperty("HideAllMenusOnProfilePage", false); }
	}

	public static bool HidePageMenuOnProfilePage
	{
		get { return ConfigHelper.GetBoolProperty("HidePageMenuOnProfilePage", true); }
	}

	public static bool HidePageMenuOnMemberListPage
	{
		get { return ConfigHelper.GetBoolProperty("HidePageMenuOnMemberListPage", true); }
	}

	public static bool HidePageViewModeIfNoWorkflowItems
	{
		get { return ConfigHelper.GetBoolProperty("HidePageViewModeIfNoWorkflowItems", true); }
	}

	public static bool SuppressMenuOnBuiltIn404Page
	{
		get { return ConfigHelper.GetBoolProperty("SuppressMenuOnBuiltIn404Page", true); }
	}

	public static bool ShowForumPostsInMemberList
	{
		get { return ConfigHelper.GetBoolProperty("ShowForumPostsInMemberList", true); }
	}

	public static bool DisableLoginInfo
	{
		get { return ConfigHelper.GetBoolProperty("DisableLoginInfo", false); }
	}

	public static bool ShowLoginNameInMemberList
	{
		get { return ConfigHelper.GetBoolProperty("ShowLoginNameInMemberList", false); }
	}

	public static bool ShowUserIDInMemberList
	{
		get { return ConfigHelper.GetBoolProperty("ShowUserIDInMemberList", false); }
	}

	public static bool ShowLeftColumnOnSearchResults
	{
		get { return ConfigHelper.GetBoolProperty("ShowLeftColumnOnSearchResults", false); }
	}

	public static bool ShowSkinSearchInputOnSearchResults
	{
		get { return ConfigHelper.GetBoolProperty("ShowSkinSearchInputOnSearchResults", false); }
	}

	public static bool ShowSearchInputOnSiteSettings
	{
		get { return ConfigHelper.GetBoolProperty("ShowSearchInputOnSiteSettings", false); }
	}

	public static bool ShowRightColumnOnSearchResults
	{
		get { return ConfigHelper.GetBoolProperty("ShowRightColumnOnSearchResults", false); }
	}

	public static bool ShowModuleTitlesByDefault
	{
		get { return ConfigHelper.GetBoolProperty("ShowModuleTitlesByDefault", true); }
	}

	public static bool EnableEditingModuleTitleElement
	{
		get { return ConfigHelper.GetBoolProperty("EnableEditingModuleTitleElement", false); }
	}

	public static string ModuleTitleTag
	{
		get { return ConfigHelper.GetStringProperty("ModuleTitleTag", "h2"); }
	}

	public static bool EnableDeveloperMenuInAdminMenu
	{
		get { return ConfigHelper.GetBoolProperty("EnableDeveloperMenuInAdminMenu", false); }
	}

	public static int UserAutoCompleteRowsToGet
	{
		get { return ConfigHelper.GetIntProperty("UserAutoCompleteRowsToGet", 10); }
	}

	public static bool EnableQueryTool
	{
		get { return ConfigHelper.GetBoolProperty("EnableQueryTool", false); }
	}

	public static string QueryToolOverrideConnectionString
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolOverrideConnectionString"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolOverrideConnectionString"];
			}
			// default value
			return string.Empty;
		}
	}

	public static bool TryToCreateMsSqlDatabase
	{
		get { return ConfigHelper.GetBoolProperty("TryToCreateMsSqlDatabase", false); }
	}

	public static bool TryToCopySQLiteSeedDatabase
	{
		get { return ConfigHelper.GetBoolProperty("TryToCopySQLiteSeedDatabase", true); }
	}

	public static string QueryToolMsSqlTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolMsSqlTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolMsSqlTableSelectSql"];
			}
			// default value
			return "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE OBJECTPROPERTY(object_id(TABLE_NAME), N'IsUserTable') = 1 ORDER BY TABLE_NAME";
		}
	}

	public static string QueryToolMySqlTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolMySqlTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolMySqlTableSelectSql"];
			}
			// default value
			return "SELECT DISTINCT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA <> 'mysql' ORDER BY TABLE_NAME;";
		}
	}

	public static string QueryToolPgSqlTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolPgSqlTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolPgSqlTableSelectSql"];
			}
			// default value
			return "SELECT table_name AS TableName FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
		}
	}

	public static string QueryToolSqliteTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolSqliteTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolSqliteTableSelectSql"];
			}
			// default value
			return "SELECT Name As TableName FROM sqlite_master WHERE type = 'table' ORDER BY name;";
		}
	}

	public static string QueryToolFirebirdSqlTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolFirebirdSqlTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolFirebirdSqlTableSelectSql"];
			}
			// default value
			return "SELECT DISTINCT TRIM(RDB$RELATION_NAME) AS TableName FROM RDB$RELATION_FIELDS WHERE RDB$SYSTEM_FLAG=0;";
		}
	}

	public static string QueryToolSqAzureTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolSqAzureTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolSqAzureTableSelectSql"];
			}
			// default value
			return "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE OBJECTPROPERTY(object_id(TABLE_NAME), N'IsUserTable') = 1 ORDER BY TABLE_NAME";
		}
	}

	public static string QueryToolSqlCeTableSelectSql
	{
		get
		{
			if (ConfigurationManager.AppSettings["QueryToolSqlCeTableSelectSql"] != null)
			{
				return ConfigurationManager.AppSettings["QueryToolSqlCeTableSelectSql"];
			}
			// default value
			return "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";

			//SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'mp_ModuleSettings' ;
		}
	}

	public static bool EnableLogViewer
	{
		get { return ConfigHelper.GetBoolProperty("EnableLogViewer", true); }
	}

	public static bool UseCultureOverride
	{
		get { return ConfigHelper.GetBoolProperty("UseCultureOverride", false); }
	}

	public static bool SetUICultureWhenSettingCulture
	{
		get { return ConfigHelper.GetBoolProperty("SetUICultureWhenSettingCulture", true); }
	}

	public static bool UseCultureForUICulture
	{
		get { return ConfigHelper.GetBoolProperty("UseCultureForUICulture", true); }
	}

	public static bool UseCustomHandlingForPersianCulture
	{
		get { return ConfigHelper.GetBoolProperty("UseCustomHandlingForPersianCulture", false); }
	}

	//this fixes some ajax updatepanel issues in webkit
	//http://forums.asp.net/p/1252014/2392110.aspx
	public static bool UseSafariWebKitHack
	{
		get { return ConfigHelper.GetBoolProperty("UseSafariWebKitHack", false); }
	}

	public static bool UseAjaxFormActionUpdateScript
	{
		get { return ConfigHelper.GetBoolProperty("UseAjaxFormActionUpdateScript", true); }
	}

	public static bool CombineJavaScript
	{
		get { return ConfigHelper.GetBoolProperty("CombineJavaScript", false); }
	}

	public static string CKEditorSkin
	{
		get { return ConfigHelper.GetStringProperty("CKEditor:Skin", "moono-lisa"); }
	}

	//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12326~1#post51248
	public static bool CKeditorSuppressTitle
	{
		get { return ConfigHelper.GetBoolProperty("CKEditor:SuppressTitle", true); }
	}


	public static bool CKeditorEncodeBrackets
	{
		get { return ConfigHelper.GetBoolProperty("CKeditor:EncodeBrackets", false); }
	}

	public static bool UseSkinCssInEditor
	{
		get { return ConfigHelper.GetBoolProperty("UseSkinCssInEditor", true); }
	}

	public static string EditorCssUrlOverride
	{
		get
		{
			return ConfigHelper.GetStringProperty("EditorCssUrlOverride", string.Empty);
		}
	}

	/// <summary>
	/// if you populate this setting it should start with a comma since it
	/// will be appended to the skin url which is assigned by default
	/// </summary>
	public static string EditorExtraCssUrlCsv
	{
		get
		{
			return ConfigHelper.GetStringProperty("EditorExtraCssUrlCsv", string.Empty);
		}
	}

	public static bool DisableTinyMceInlineEditing
	{
		get { return ConfigHelper.GetBoolProperty("DisableTinyMceInlineEditing", false); }
	}

	public static bool TinyMceInlineSaveOnBlur
	{
		get { return ConfigHelper.GetBoolProperty("TinyMceInlineSaveOnBlur", true); }
	}

	public static bool TinyMceInlineRemoveAutoSavePlugin
	{
		get { return ConfigHelper.GetBoolProperty("TinyMceInlineRemoveAutoSavePlugin", true); }
	}

	public static bool TinyMceInlineUseSavePlugin
	{
		get { return ConfigHelper.GetBoolProperty("TinyMceInlineUseSavePlugin", false); }
	}

	public static string TinyMceSchema
	{
		get
		{
			if (ConfigurationManager.AppSettings["TinyMCE:Schema"] != null)
			{
				return ConfigurationManager.AppSettings["TinyMCE:Schema"];
			}
			return "html5";
		}
	}

	public static string TinyMceBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["TinyMCE:BasePath"] != null)
			{
				return ConfigurationManager.AppSettings["TinyMCE:BasePath"];
			}
			return "/ClientScript/tiny_mce325/";
		}
	}

	public static string TinyMceSkin
	{
		get
		{
			if (ConfigurationManager.AppSettings["TinyMCE:Skin"] != null)
			{
				return ConfigurationManager.AppSettings["TinyMCE:Skin"];
			}
			return "default";
		}
	}

	public static string TimelineBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["TimelineBasePath"] != null)
			{
				return ConfigurationManager.AppSettings["TimelineBasePath"];
			}
			return "~/ClientScript/timeline/";
		}
	}

	public static string UnobtrusiveValidationMode
	{
		get { return ConfigHelper.GetStringProperty("ValidationSettings:UnobtrusiveValidationMode", string.Empty); }
	}

	public static bool ForceEmptyJQueryScriptReference
	{
		get { return ConfigHelper.GetBoolProperty("ForceEmptyJQueryScriptReference", true); }
	}

	public static string jQueryUIAvailableThemes
	{
		get { return ConfigHelper.GetStringProperty("jQueryUIAvailableThemes", string.Empty); }
	}

	public static bool UseHtml5
	{
		get { return ConfigHelper.GetBoolProperty("UseHtml5", false); }
	}

	public static bool DisableViewStateOnSiteMapDataSource
	{
		get { return ConfigHelper.GetBoolProperty("DisableViewStateOnSiteMapDataSource", true); }
	}

	public static bool CombineCSS
	{
		get { return ConfigHelper.GetBoolProperty("CombineCSS", true); }
	}

	public static bool CacheCssOnServer
	{
		get { return ConfigHelper.GetBoolProperty("CacheCssOnServer", true); }
	}

	public static bool CacheCssInBrowser
	{
		get { return ConfigHelper.GetBoolProperty("CacheCssInBrowser", true); }
	}

	/// <summary>
	/// This can easily show the mojoPortal version to nefarious jerks that could then use it to exploit vulnerabilities in the advertised version
	/// </summary>
	public static bool IncludeVersionInCssUrl
	{
		get { return ConfigHelper.GetBoolProperty("IncludeVersionInCssUrl", false); }
	}

	public static int CssCacheInDays
	{
		get { return ConfigHelper.GetIntProperty("CssCacheInDays", 7); }
	}

	public static int ChannelFileCacheInDays
	{
		get { return ConfigHelper.GetIntProperty("ChannelFileCacheInDays", 365); }
	}

	public static bool MinifyCSS
	{
		get { return ConfigHelper.GetBoolProperty("MinifyCSS", false); }
	}

	public static bool CacheTimeZoneList
	{
		get { return ConfigHelper.GetBoolProperty("CacheTimeZoneList", true); }
	}

	public static bool DisableASPThemes
	{
		get { return ConfigHelper.GetBoolProperty("DisableASPThemes", false); }
	}

	public static bool UsingOlderSkins
	{
		get { return ConfigHelper.GetBoolProperty("UsingOlderSkins", false); }
	}

	public static bool MenusAreResponsibleForAddingCss
	{
		get { return ConfigHelper.GetBoolProperty("MenusAreResponsibleForAddingCss", false); }
	}

	public static bool AllowChangingFriendlyUrlPattern
	{
		get { return ConfigHelper.GetBoolProperty("AllowChangingFriendlyUrlPattern", true); }
	}

	public static string FriendlyUrlSuggestScript
	{
		get { return ConfigHelper.GetStringProperty("FriendlyUrlSuggestScript", "~/ClientScript/friendlyurlsuggest_v3.js"); }
	}

	public static bool AllowDirectEntryOfUserIdForEditPermission
	{
		get { return ConfigHelper.GetBoolProperty("AllowDirectEntryOfUserIdForEditPermission", false); }
	}

	public static bool AllowMultipleSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowMultipleSites", true); }
	}

	public static bool AppendDefaultPageToFolderRootUrl
	{
		get { return ConfigHelper.GetBoolProperty("AppendDefaultPageToFolderRootUrl", true); }
	}

	public static bool ShowSiteGuidInSiteSettings
	{
		get { return ConfigHelper.GetBoolProperty("ShowSiteGuidInSiteSettings", false); }
	}

	public static bool ShowSiteIdInSiteList
	{
		get { return ConfigHelper.GetBoolProperty("ShowSiteIdInSiteList", true); }
	}

	public static int SiteListPageSize
	{
		get { return ConfigHelper.GetIntProperty("SiteListPageSize", 30); }
	}

	public static int RoleMemberPageSize
	{
		get { return ConfigHelper.GetIntProperty("RoleMemberPageSize", 30); }
	}

	public static bool EnableSiteSettingsSmtpSettings
	{
		get { return ConfigHelper.GetBoolProperty("EnableSiteSettingsSmtpSettings", true); }
	}

	public static bool EnforceContentVersioningGlobally
	{
		get { return ConfigHelper.GetBoolProperty("EnforceContentVersioningGlobally", false); }
	}

	public static bool MaskSmtpPasswordInSiteSettings
	{
		get { return ConfigHelper.GetBoolProperty("MaskSmtpPasswordInSiteSettings", true); }
	}

	public static bool ShowSmtpEncodingOption
	{
		get { return ConfigHelper.GetBoolProperty("ShowSmtpEncodingOption", false); }
	}

	public static bool HideGoogleAnalyticsInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("HideGoogleAnalyticsInChildSites", false); }
	}

	public static string GravatarMaxAllowedRating
	{
		get
		{
			if (ConfigurationManager.AppSettings["GravatarMaxAllowedRating"] != null)
			{
				return ConfigurationManager.AppSettings["GravatarMaxAllowedRating"];
			}
			// default value
			return "G";
		}
	}

	public static bool OnlyAdminsCanEditCheesyAvatars
	{
		get { return ConfigHelper.GetBoolProperty("OnlyAdminsCanEditCheesyAvatars", false); }
	}

	public static bool MyPageIsInstalled
	{
		get { return ConfigHelper.GetBoolProperty("MyPageIsInstalled", false); }
	}

	public static bool UseSslForMyPage
	{
		get { return ConfigHelper.GetBoolProperty("UseSslForMyPage", false); }
	}

	public static bool UseSslForSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("UseSslForSiteMap", false); }
	}

	public static bool UseSslForSiteOffice
	{
		get { return ConfigHelper.GetBoolProperty("UseSslForSiteOffice", false); }
	}

	public static bool UseSslForMemberList
	{
		get { return ConfigHelper.GetBoolProperty("UseSslForMemberList", false); }
	}

	public static bool ProxyPreventsSSLDetection
	{
		get { return ConfigHelper.GetBoolProperty("ProxyPreventsSSLDetection", false); }
	}

	public static bool RedirectSslWith301Status
	{
		get { return ConfigHelper.GetBoolProperty("RedirectSslWith301Status", false); }
	}

	public static bool IsDemoSite
	{
		get { return ConfigHelper.GetBoolProperty("IsDemoSite", false); }
	}
	/// <summary>
	/// I use this to track people using our demo site who try to DOS (denial of service) our demo site by deleting all the pages
	/// I want to ban those ip addresses
	/// </summary>
	public static bool LogIpAddressForContentDeletions
	{
		get { return ConfigHelper.GetBoolProperty("LogIpAddressForContentDeletions", false); }
	}

	/// <summary>
	/// I use this to track users who try to DOS our demo site by changing the admin password so other users cannnot use it
	/// </summary>
	public static bool LogIpAddressForPasswordChanges
	{
		get { return ConfigHelper.GetBoolProperty("LogIpAddressForPasswordChanges", false); }
	}

	/// <summary>
	/// I use this to track users who try to DOS our demo site by changing the admin password so other users cannnot use it
	/// </summary>
	public static bool LogIpAddressForEmailChanges
	{
		get { return ConfigHelper.GetBoolProperty("LogIpAddressForEmailChanges", false); }
	}

	public static bool LogFailedLoginAttempts
	{
		get { return ConfigHelper.GetBoolProperty("LogFailedLoginAttempts", false); }
	}

	public static bool LogAllFileServiceRequests
	{
		get { return ConfigHelper.GetBoolProperty("LogAllFileServiceRequests", false); }
	}

	public static bool LogBlockedRequests
	{
		get { return ConfigHelper.GetBoolProperty("LogBlockedRequests", true); }
	}

	public static bool LogAllFileManagerActivity
	{
		get { return ConfigHelper.GetBoolProperty("LogAllFileManagerActivity", false); }
	}

	public static bool FileServiceRejectFishyPosts
	{
		get { return ConfigHelper.GetBoolProperty("FileServiceRejectFishyPosts", true); }
	}

	public static bool LogCacheActivity
	{
		get { return ConfigHelper.GetBoolProperty("LogCacheActivity", false); }
	}

	public static bool LogXmlRpcRequests
	{
		get { return ConfigHelper.GetBoolProperty("LogXmlRpcRequests", false); }
	}

	public static bool LogNewsletterSubscriptions
	{
		get { return ConfigHelper.GetBoolProperty("LogNewsletterSubscriptions", false); }
	}

	public static bool LogFullUrls
	{
		get { return ConfigHelper.GetBoolProperty("LogFullUrls", false); }
	}

	public static bool UseSystemLogInsteadOfFileLog
	{
		get { return ConfigHelper.GetBoolProperty("UseSystemLogInsteadOfFileLog", false); }
	}

	public static bool ShowFileLogInAdditionToSystemLog
	{
		get { return ConfigHelper.GetBoolProperty("ShowFileLogInAdditionToSystemLog", false); }
	}

	public static int SystemLogPageSize
	{
		get { return ConfigHelper.GetIntProperty("SystemLogPageSize", 30); }
	}

	public static bool SystemLogSortAscending
	{
		get { return ConfigHelper.GetBoolProperty("SystemLogSortAscending", false); }
	}

	public static string SystemLogDateTimeFormat
	{
		get { return ConfigHelper.GetStringProperty("SystemLogDateTimeFormat", "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff"); }
	}

	public static bool SystemLogDeleteOldEventsOnApplicationStart
	{
		get { return ConfigHelper.GetBoolProperty("SystemLogDeleteOldEventsOnApplicationStart", true); }
	}

	public static int SystemLogApplicationStartDeleteOlderThanDays
	{
		get { return ConfigHelper.GetIntProperty("SystemLogApplicationStartDeleteOlderThanDays", 10); }
	}




	//public static bool EnableSslInChildSites
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("EnableSSLInChildSites", false); }
	//}

	public static bool AllowDeletingChildSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowDeletingChildSites", false); }
	}

	public static bool DeleteSiteFolderWhenDeletingSites
	{
		get { return ConfigHelper.GetBoolProperty("DeleteSiteFolderWhenDeletingSites", false); }
	}

	public static bool ShowSkinRestoreButtonInSiteSettings
	{
		get { return ConfigHelper.GetBoolProperty("ShowSkinRestoreButtonInSiteSettings", true); }
	}
	public static bool ShowCopyNewSkinsButtonInSiteSettings
	{
		get { return ConfigHelper.GetBoolProperty("ShowCopyNewSkinsButtonInSiteSettings", true); }
	}

	public static bool AllowFileManagerInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowFileManagerInChildSites", false); }
	}

	public static bool AllowClosingSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowClosingSites", true); }
	}

	public static string RolesThatCanAccessClosedSites
	{
		get { return ConfigHelper.GetStringProperty("RolesThatCanAccessClosedSites", "Admins;Content Administrators;"); }
	}

	public static string RolesThatAlwaysViewMobileContent
	{
		get { return ConfigHelper.GetStringProperty("RolesThatAlwaysViewMobileContent", string.Empty); }
	}

	public static string ClosedPageUrl
	{
		get { return ConfigHelper.GetStringProperty("ClosedPageUrl", "/closed.aspx"); }
	}


	public static string UserNameValidationExpression
	{
		get
		{
			if (ConfigurationManager.AppSettings["UserNameValidationExpression"] != null)
			{
				return ConfigurationManager.AppSettings["UserNameValidationExpression"];
			}

			return string.Empty;
		}
	}

	public static string UserNameValidationWarning
	{
		get
		{
			if (ConfigurationManager.AppSettings["UserNameValidationWarning"] != null)
			{
				return ConfigurationManager.AppSettings["UserNameValidationWarning"];
			}

			return string.Empty;
		}
	}

	//public static string TestDecryptedValueForDefaultMachineKey
	//{
	//	get
	//	{
	//		if (ConfigurationManager.AppSettings["TestDecryptedValueForDefaultMachineKey"] != null)
	//		{
	//			return ConfigurationManager.AppSettings["TestDecryptedValueForDefaultMachineKey"];
	//		}

	//		return "thisSiteIsSecure123";
	//	}
	//}

	//public static string TestEncryptedValueForDefaultMachineKey
	//{
	//	get
	//	{
	//		if (ConfigurationManager.AppSettings["TestEncryptedValueForDefaultMachineKey"] != null)
	//		{
	//			return ConfigurationManager.AppSettings["TestEncryptedValueForDefaultMachineKey"];
	//		}

	//		return "8qgZUAp4ukDE6U1/aMIHbmmmLk66RUNQb4KXdgnimwSoSMNyrMPqyzJQCrRf2+XQ";
	//	}
	//}

	/// <summary>
	/// for backward compatibility this is true but for new installations this is false in the user.config.sample file so it uses the newer method
	/// main problem with the legacy solution is that it doe snot work in medium trust hosting
	/// </summary>
	public static bool UseLegacyCryptoHelper
	{
		get { return ConfigHelper.GetBoolProperty("UseLegacyCryptoHelper", true); }
	}

	public static bool AllowUserProfilePage
	{
		get { return ConfigHelper.GetBoolProperty("AllowUserProfilePage", true); }
	}

	public static string PrivateProfileRelativeUrl
	{
		get { return ConfigHelper.GetStringProperty("PrivateProfileRelativeUrl", "/Secure/UserProfile.aspx"); }

	}
	public static string PublicProfileRelativeUrl
	{
		get { return ConfigHelper.GetStringProperty("PublicProfileRelativeUrl", "/ProfileView.aspx"); }

	}
	public static bool AllowPasswordFormatChange
	{
		get { return ConfigHelper.GetBoolProperty("AllowPasswordFormatChange", true); }
	}

	public static bool AllowCaseInsensitivePasswordQuestionAnswer
	{
		get { return ConfigHelper.GetBoolProperty("AllowCaseInsensitivePasswordQuestionAnswer", false); }
	}

	/// <summary>
	/// this is mainly used so I can disable it on the demo site
	/// </summary>
	public static bool AllowRequiringPasswordChange
	{
		get { return ConfigHelper.GetBoolProperty("AllowRequiringPasswordChange", true); }
	}

	public static bool LockAccountOnMaxPasswordAnswerTries
	{
		get { return ConfigHelper.GetBoolProperty("LockAccountOnMaxPasswordAnswerTries", false); }
	}

	public static bool DisableAutoCompleteOnLogin
	{
		get { return ConfigHelper.GetBoolProperty("DisableAutoCompleteOnLogin", false); }
	}


	/// <summary>
	/// List of semi-colon separated numeric values used to determine the weighting of a strength characteristic. There must be 4 values specified which must total 100. 
	/// The default weighting values are defined as 50;15;15;20. This corresponds to password length is 50% of the strength calculation, 
	/// Numeric criteria is 15% of strength calculation, casing criteria is 15% of calculation, and symbol criteria is 20% of calculation. 
	/// So the format is 'A;B;C;D' where A = length weighting, B = numeric weighting, C = casing weighting, D = symbol weighting.
	/// </summary>
	public static string PasswordStrengthCalculationWeightings
	{
		get { return ConfigHelper.GetStringProperty("PasswordStrengthCalculationWeightings", "25;25;25;25"); }
	}

	/// <summary>
	/// valid options are RightSide, LeftSide, AboveLeft, AboveRight, BelowLeft, BelowRight
	/// </summary>
	public static string PasswordStrengthDisplayPosition
	{
		get { return ConfigHelper.GetStringProperty("PasswordStrengthDisplayPosition", "RightSide"); }
	}

	/// <summary>
	/// valid options are BarIndicator and Text
	/// </summary>
	public static string PasswordStrengthIndicatorType
	{
		get { return ConfigHelper.GetStringProperty("PasswordStrengthIndicatorType", "Text"); }
	}

	public static int PasswordStrengthMinimumLowerCaseCharacters
	{
		get { return ConfigHelper.GetIntProperty("PasswordStrengthMinimumLowerCaseCharacters", 1); }
	}

	public static int PasswordStrengthMinimumUpperCaseCharacters
	{
		get { return ConfigHelper.GetIntProperty("PasswordStrengthMinimumUpperCaseCharacters", 1); }
	}

	public static int MinUserNameLength
	{
		get { return ConfigHelper.GetIntProperty("MinUserNameLength", 6); }
	}

	public static bool PreEncryptRolesForCookie
	{
		get { return ConfigHelper.GetBoolProperty("PreEncryptRolesForCookie", false); }
	}

	public static bool Return404StatusForCryptoError
	{
		get { return ConfigHelper.GetBoolProperty("Return404StatusForCryptoError", true); }
	}

	/// <summary>
	/// 0 = clear, 1= hashed, 2= encrypted
	/// </summary>
	public static int InitialSitePasswordFormat
	{
		//changed default to hashed 2009-02-25
		// changed default to encrypted 2009-05-08
		//http://www.mojoportal.com/Forums/Thread.aspx?thread=2902&mid=34&pageid=5&ItemID=5&pagenumber=1
		// changed back to clear text 2009-05-25 because of too many support requests where people end up
		// getting the admin user locked out
		get { return ConfigHelper.GetIntProperty("InitialSitePasswordFormat", 0); }
	}

	public static bool AllowPasswordFormatChangeInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowPasswordFormatChangeInChildSites", false); }
	}

	public static bool CheckMD5PasswordHashAsFallback
	{
		get { return ConfigHelper.GetBoolProperty("CheckMD5PasswordHashAsFallback", true); }
	}

	public static bool CheckAllPasswordFormatsOnAuthFailure
	{
		get { return ConfigHelper.GetBoolProperty("CheckAllPasswordFormatsOnAuthFailure", false); }
	}

	/// <summary>
	/// we default this to false because we already handle locked users in SiteLogin.cs by cancellinng and showing a specific message indicating the account is locked
	/// whereas if we just return fale from membership provider the message will be more generic authentication failed message
	/// </summary>
	public static bool ReturnFalseInValidateUserIfAccountLocked
	{
		get { return ConfigHelper.GetBoolProperty("MembershipProvider:ReturnFalseInValidateUserIfAccountLocked", false); }
	}

	public static bool ReturnFalseInValidateUserIfAccountDeleted
	{
		get { return ConfigHelper.GetBoolProperty("MembershipProvider:ReturnFalseInValidateUserIfAccountDeleted", true); }
	}

	public static bool AllowNewRegistrationToActivateDeletedAccountWithSameEmail
	{
		get { return ConfigHelper.GetBoolProperty("MembershipProvider:AllowNewRegistrationToActivateDeletedAccountWithSameEmail", true); }
	}

	public static string PasswordGeneratorChars
	{
		get
		{
			if (ConfigurationManager.AppSettings["PasswordGeneratorChars"] != null)
			{
				return ConfigurationManager.AppSettings["PasswordGeneratorChars"];
			}

			return "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$";
		}
	}

	public static bool ShowSystemInformationInChildSiteAdminMenu
	{
		get { return ConfigHelper.GetBoolProperty("ShowSystemInformationInChildSiteAdminMenu", true); }
	}

	public static bool ShowConnectionErrorOnSetup
	{
		get { return ConfigHelper.GetBoolProperty("ShowConnectionErrorOnSetup", false); }
	}

	public static bool NotifyAdminsOnNewUserRegistration
	{
		get { return ConfigHelper.GetBoolProperty("NotifyAdminsOnNewUserRegistration", false); }
	}

	public static bool NotifyUsersOnAccountApproval
	{
		get { return ConfigHelper.GetBoolProperty("NotifyUsersOnAccountApproval", true); }
	}

	/// <summary>
	/// a comma separated list of email addresses to exclude when sending
	/// administrative emails including registration notifications and content workflow submissions
	/// this is for when you have admin user accounts that you do not want to receive these emails
	/// </summary>
	public static string EmailAddressesToExcludeFromAdminNotifications
	{
		get
		{
			if (ConfigurationManager.AppSettings["EmailAddressesToExcludeFromAdminNotifications"] != null)
			{
				return ConfigurationManager.AppSettings["EmailAddressesToExcludeFromAdminNotifications"];
			}

			return string.Empty;
		}
	}

	/// <summary>
	/// if a value is specified then these settings will be used instead of the standard site smtp settings
	/// </summary>
	public static string NewsletterSmtpServer
	{
		get { return ConfigHelper.GetStringProperty("Newsletter:SmtpServer", string.Empty); }
	}

	public static int NewsletterSmtpServerPort
	{
		get { return ConfigHelper.GetIntProperty("Newsletter:SmtpServerPort", 25); }
	}

	public static string NewsletterSmtpUser
	{
		get { return ConfigHelper.GetStringProperty("Newsletter:SmtpUser", string.Empty); }
	}

	public static string NewsletterSmtpUserPassword
	{
		get { return ConfigHelper.GetStringProperty("Newsletter:SmtpUserPassword", string.Empty); }
	}

	public static bool NewsletterSmtpRequiresAuthentication
	{
		get { return ConfigHelper.GetBoolProperty("Newsletter:SmtpRequiresAuthentication", false); }
	}

	public static bool NewsletterSmtpUseSsl
	{
		get { return ConfigHelper.GetBoolProperty("Newsletter:SmtpUseSsl", false); }
	}

	public static string NewsletterSmtpPreferredEncoding
	{
		get { return ConfigHelper.GetStringProperty("Newsletter:SmtpPreferredEncoding", string.Empty); }
	}

	public static bool NewsletterTestMode
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterTestMode", false); }
	}


	public static bool GuessEmailForWindowsAuth
	{
		get { return ConfigHelper.GetBoolProperty("GuessEmailForWindowsAuth", false); }
	}

	public static string WindowsAuthDomainExtension
	{
		get
		{
			if (ConfigurationManager.AppSettings["WindowsAuthDomainExtension"] != null)
			{
				return ConfigurationManager.AppSettings["WindowsAuthDomainExtension"];
			}

			return ".com";
		}
	}

	//public static bool DisableDotNetOpenMail
	//{
	//	get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("DisableDotNetOpenMail", true); }
	//}

	public static bool ShowHistoryOnUpgradePage
	{
		get { return ConfigHelper.GetBoolProperty("ShowHistoryOnUpgradePage", false); }
	}

	[Obsolete("Replaced by UseFolderBasedMultiTenants")]
	public static bool UseFoldersInsteadOfHostnamesForMultipleSites
	{
		get { return UseFolderBasedMultiTenants; }
	}

	public static bool UseFolderBasedMultiTenants
	{
		get
		{
			return ConfigurationManager.AppSettings["UseFoldersInsteadOfHostnamesForMultipleSites"] != null
				? ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false)
				: ConfigHelper.GetBoolProperty("UseFolderBasedMultiTenants", false);
		}
	}

	public static bool UseSiteNameForRootBreadcrumb
	{
		get { return ConfigHelper.GetBoolProperty("UseSiteNameForRootBreadcrumb", false); }
	}

	public static bool UseRelatedSiteMode
	{
		get { return ConfigHelper.GetBoolProperty("UseRelatedSiteMode", false); }
	}

	public static bool UseSameContentFolderForRelatedSiteMode
	{
		get { return ConfigHelper.GetBoolProperty("UseSameContentFolderForRelatedSiteMode", false); }
	}

	public static bool UseRpxNowForOpenId
	{
		get { return ConfigHelper.GetBoolProperty("UseRpxNowForOpenId", false); }
	}

	public static int RelatedSiteID
	{
		get { return ConfigHelper.GetIntProperty("RelatedSiteID", 1); }
	}

	public static bool RelatedSiteModeHideRoleManagerInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("RelatedSiteModeHideRoleManagerInChildSites", true); }
	}

	public static bool UseUrlReWriting
	{
		get { return ConfigHelper.GetBoolProperty("UseUrlReWriting", true); }
	}

	/// <summary>
	/// setting this to true solves a problem where IIS logs were not showing the re-written/requested url
	/// http://stackoverflow.com/questions/353541/iis7-rewritepath-and-iis-log-files
	/// </summary>
	public static bool UseTransferRequestForUrlReWriting
	{
		get { return ConfigHelper.GetBoolProperty("UseTransferRequestForUrlReWriting", true); }
	}


	// Commented out for issue #70
	// https://github.com/i7MEDIA/mojoportal/issues/70
	//public static bool DetectPageNotFoundForExtensionlessUrls
	//{
	//	get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("DetectPageNotFoundForExtensionlessUrls", false); }
	//}


	public static bool UseUrlReWritingForStaticFiles
	{
		get { return ConfigHelper.GetBoolProperty("UseUrlReWritingForStaticFiles", false); }
	}

	public static bool DisableUseOfPassedInDateForMetaWeblogApi
	{
		get { return ConfigHelper.GetBoolProperty("DisableUseOfPassedInDateForMetaWeblogApi", false); }
	}

	public static string FacebookAppId
	{
		get
		{
			return ConfigHelper.GetStringProperty("FacebookAppId", string.Empty);
		}
	}

	public static bool DisableFacebookLikeButton
	{
		get { return ConfigHelper.GetBoolProperty("DisableFacebookLikeButton", false); }
	}

	public static bool DisableHelpSystem
	{
		get { return ConfigHelper.GetBoolProperty("DisableHelpSystem", false); }
	}

	public static bool DisableWorkflowNotification
	{
		get { return ConfigHelper.GetBoolProperty("DisableWorkflowNotification", false); }
	}

	public static bool RenderModulePanel
	{
		get { return ConfigHelper.GetBoolProperty("RenderModulePanel", true); }
	}

	public static bool HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins
	{
		get { return ConfigHelper.GetBoolProperty("HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins", false); }
	}

	public static bool HideModuleSettingsDeleteButtonFromNonAdmins
	{
		get { return ConfigHelper.GetBoolProperty("HideModuleSettingsDeleteButtonFromNonAdmins", false); }
	}

	public static bool Disable301Redirector
	{
		get { return ConfigHelper.GetBoolProperty("Disable301Redirector", false); }
	}

	public static bool IncludeParametersIn301RedirectLookup
	{
		get { return ConfigHelper.GetBoolProperty("IncludeParametersIn301RedirectLookup", false); }
	}

	public static bool AllowExternal301Redirects
	{
		get { return ConfigHelper.GetBoolProperty("AllowExternal301Redirects", false); }
	}

	public static bool EnableRouting
	{
		get { return ConfigHelper.GetBoolProperty("EnableRouting", true); }
	}

	public static bool AddDefaultMvcRoute
	{
		get { return ConfigHelper.GetBoolProperty("AddDefaultMvcRoute", false); }
	}

	public static string RouteConfigPath
	{
		get { return ConfigHelper.GetStringProperty("RouteConfigPath", "~/Setup/RouteRegistrars/"); }
	}

	public static bool EnableVirtualPathProviders
	{
		get { return ConfigHelper.GetBoolProperty("EnableVirtualPathProviders", true); }
	}


	public static bool PassQueryStringFor301Redirects
	{
		get { return ConfigHelper.GetBoolProperty("PassQueryStringFor301Redirects", false); }
	}

	public static bool DisableCacheFor301Redirects
	{
		get { return ConfigHelper.GetBoolProperty("DisableCacheFor301Redirects", false); }
	}

	public static bool SetExplicitCacheFor301Redirects
	{
		get { return ConfigHelper.GetBoolProperty("SetExplicitCacheFor301Redirects", true); }
	}

	public static int CacheDurationInDaysFor301Redirects
	{
		get { return ConfigHelper.GetIntProperty("CacheDurationInDaysFor301Redirects", 5); }
	}

	public static bool DisableTaskQueue
	{
		get { return ConfigHelper.GetBoolProperty("DisableTaskQueue", false); }
	}

	public static bool DisableBannedIpBlockingModule
	{
		get { return ConfigHelper.GetBoolProperty("DisableBannedIpBlockingModule", false); }
	}

	public static bool AppDomainMonitoringEnabled
	{
		get { return ConfigHelper.GetBoolProperty("AppDomainMonitoringEnabled", false); }
	}

	public static bool FirstChanceExceptionMonitoringEnabled
	{
		get { return ConfigHelper.GetBoolProperty("FirstChanceExceptionMonitoringEnabled", false); }
	}

	public static bool UsePerSiteTaskQueue
	{
		get { return ConfigHelper.GetBoolProperty("UsePerSiteTaskQueue", false); }
	}

	public static string mojoCombinedScriptVersion
	{
		get
		{
			if (ConfigurationManager.AppSettings["mojoCombinedScriptVersion"] != null)
			{
				return ConfigurationManager.AppSettings["mojoCombinedScriptVersion"];
			}
			// default value
			return string.Empty;
		}
	}

	public static string mojoCombinedScriptVersionParam
	{
		get
		{
			if (ConfigurationManager.AppSettings["mojoCombinedScriptVersionParam"] != null)
			{
				return ConfigurationManager.AppSettings["mojoCombinedScriptVersionParam"];
			}
			// default value
			return "v3";
		}
	}

	public static string mojoCombinedScript
	{
		get
		{
			return ConfigHelper.GetStringProperty("mojoCombinedScript", string.Empty);
		}
	}

	public static bool DisableBingStaticMaps
	{
		get { return ConfigHelper.GetBoolProperty("DisableBingStaticMaps", false); }
	}

	public static string BingMapsApiKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["BingMapsApiKey"] != null)
			{
				return ConfigurationManager.AppSettings["BingMapsApiKey"];
			}

			return string.Empty;
		}
	}

	public static string BingMapsVersion
	{
		get
		{
			if (ConfigurationManager.AppSettings["BingMapsVersion"] != null)
			{
				return ConfigurationManager.AppSettings["BingMapsVersion"];
			}
			// default value
			return "6.3";
		}
	}

	// Firefox 4 broke bing map when plotting points like in In Site Analytics Pro
	// found this workaround http://social.msdn.microsoft.com/Forums/en-CA/vemapcontroldev/thread/17efab17-d70c-40b3-9e50-75c65d59385e
	// this setting allows backing it out if the problem is later resolved another way
	public static bool UseBingMapWorkaroundForFirefox4
	{
		get { return ConfigHelper.GetBoolProperty("UseBingMapWorkaroundForFirefox4", true); }
	}

	public static bool UseGoogleCDN
	{
		get { return ConfigHelper.GetBoolProperty("UseGoogleCDN", true); }
	}


	public static string GoogleCDNJQueryBaseUrl
	{
		get { return ConfigHelper.GetStringProperty("GoogleCDNJQueryBaseUrl", "//ajax.googleapis.com/ajax/libs/jquery/"); }
	}

	public static string GoogleCDNJQueryUIBaseUrl
	{
		get { return ConfigHelper.GetStringProperty("GoogleCDNJQueryUIBaseUrl", "//ajax.googleapis.com/ajax/libs/jqueryui/"); }
	}

	public static bool BundlesUseCdn
	{
		get { return ConfigHelper.GetBoolProperty("BundlesUseCdn", true); }
	}

	public static bool BundlesForceOptimization
	{
		get { return ConfigHelper.GetBoolProperty("BundlesForceOptimization", true); }
	}

	public static bool AjaxToolkitUseCdnForBundle
	{
		get { return ConfigHelper.GetBoolProperty("AjaxToolkitUseCdnForBundle", true); }
	}

	public static bool DisableAjaxToolkitBundlesAndScriptReferences
	{
		get { return ConfigHelper.GetBoolProperty("DisableAjaxToolkitBundlesAndScriptReferences", true); }
	}

	public static string GoogleCDNjQueryVersion
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleCDNjQueryVersion"];
			}
			// default value
			return "1.9.1";
		}
	}

	public static string GoogleCDNjQueryUIVersion
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleCDNjQueryUIVersion"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleCDNjQueryUIVersion"];
			}
			// default value
			return "1.10.2";
		}
	}

	public static bool AssumejQueryIsLoaded
	{
		get { return ConfigHelper.GetBoolProperty("AssumejQueryIsLoaded", false); }
	}

	public static string jQueryBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["jQueryBasePath"] != null)
			{
				return ConfigurationManager.AppSettings["jQueryBasePath"];
			}
			// default value
			return "~/ClientScript/jquery142/";
		}
	}

	public static string jQueryUIBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["jQueryUIBasePath"] != null)
			{
				return ConfigurationManager.AppSettings["jQueryUIBasePath"];
			}
			// default value
			return "~/ClientScript/jqueryui182/";
		}
	}

	public static string TimePickerScriptUrl
	{
		get
		{
			if (ConfigurationManager.AppSettings["TimePickerScriptUrl"] != null)
			{
				return ConfigurationManager.AppSettings["TimePickerScriptUrl"];
			}
			// default value
			return "~/ClientScript/jqmojo/jquery-ui-timepicker-addonv3.js";
		}
	}

	public static string TimePickerScriptLocaleBaseUrl
	{
		get
		{
			if (ConfigurationManager.AppSettings["TimePickerScriptLocaleBaseUrl"] != null)
			{
				return ConfigurationManager.AppSettings["TimePickerScriptLocaleBaseUrl"];
			}
			// default value
			return "~/ClientScript/jqmojo/timepicker-i18n/";
		}
	}




	//public static string JQueryMobileCssCdnUrl
	//{
	//    get
	//    {
	//        if (ConfigurationManager.AppSettings["JQueryMobileCssCdnUrl"] != null)
	//        {
	//            return ConfigurationManager.AppSettings["JQueryMobileCssCdnUrl"];
	//        }
	//        // default value
	//        return "http://code.jquery.com/mobile/1.0a2/jquery.mobile-1.0a2.min.css";
	//    }
	//}

	//public static string JQueryMobileJsCdnUrl
	//{
	//    get
	//    {
	//        if (ConfigurationManager.AppSettings["JQueryMobileJsCdnUrl"] != null)
	//        {
	//            return ConfigurationManager.AppSettings["JQueryMobileJsCdnUrl"];
	//        }
	//        // default value
	//        return "http://code.jquery.com/mobile/1.0a2/jquery.mobile-1.0a2.min.js";
	//    }
	//}



	/// <summary>
	/// In IIS 7 Integrated mode if you want to use the App Keep alive feature you need to specify
	/// the root url of your site in this setting like http://yoursiteroot/Default.aspx
	/// </summary>
	public static string AppKeepAliveUrl
	{
		get
		{
			if (ConfigurationManager.AppSettings["AppKeepAliveUrl"] != null)
			{
				return ConfigurationManager.AppSettings["AppKeepAliveUrl"];
			}
			// default value
			return string.Empty;
		}
	}

	/// <summary>
	/// If true the MeatContentControl will render the content type meta tag. Set to false if you would rather specify it in the layout.master
	/// </summary>
	public static bool AutoSetContentType
	{
		get { return ConfigHelper.GetBoolProperty("AutoSetContentType", true); }
	}

	public static string ContentMimeType
	{
		get
		{
			if (ConfigurationManager.AppSettings["ContentMimeType"] != null)
			{
				return ConfigurationManager.AppSettings["ContentMimeType"];
			}
			// default value
			return "application/xhtml+xml";
		}
	}

	public static string ContentEncoding
	{
		get
		{
			if (ConfigurationManager.AppSettings["ContentEncoding"] != null)
			{
				return ConfigurationManager.AppSettings["ContentEncoding"];
			}
			// default value
			return "utf-8";
		}
	}


	public static bool UseAppKeepAlive
	{
		get { return ConfigHelper.GetBoolProperty("UseAppKeepAlive", false); }
	}

	public static int AppKeepAliveSleepMinutes
	{
		get { return ConfigHelper.GetIntProperty("AppKeepAliveSleepMinutes", 10); }
	}

	public static int AppKeepAliveMaxRunTimeMinutes
	{
		get { return ConfigHelper.GetIntProperty("AppKeepAliveMaxRunTimeMinutes", 720); }
	}


	//public static bool AllowPersistentLoginCookie
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("AllowPersistentLoginCookie", true); }
	//}


	public static bool AssignNewPagesParentPageSkinByDefault
	{
		get { return ConfigHelper.GetBoolProperty("AssignNewPagesParentPageSkinByDefault", true); }
	}

	public static bool AllowAnonymousUsersToViewMemberList
	{
		get { return ConfigHelper.GetBoolProperty("AllowAnonymousUsersToViewMemberList", false); }
	}

	public static bool AutoGenerateAndHideUserNamesWhenUsingEmailForLogin
	{
		get { return ConfigHelper.GetBoolProperty("AutoGenerateAndHideUserNamesWhenUsingEmailForLogin", false); }
	}

	public static bool DisablePageViewStateByDefault
	{
		get { return ConfigHelper.GetBoolProperty("DisablePageViewStateByDefault", false); }
	}

	//public static string AzureBlobStorageContainerName
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetStringProperty("AzureBlobStorageContainerName", "Data"); }
	//}



	public static string CacheProviderType
	{
		get { return ConfigHelper.GetStringProperty("Cache:ProviderType", "mojoPortal.Web.Caching.MemoryCacheAdapter, mojoPortal.Web"); }
	}

	public static string DistributedCacheName
	{
		get { return ConfigHelper.GetStringProperty("Cache:DistributedCacheName", "MyCache"); }
	}

	public static string DistributedCacheServers
	{
		get { return ConfigHelper.GetStringProperty("Cache:DistributedCacheServers", "localhost:22223"); }
	}

	public static string AzureCacheSecurityMode
	{
		get { return ConfigHelper.GetStringProperty("Cache:AzureCacheSecurityMode", "Message"); }
	}

	public static string AzureCacheAuthorizationInfo
	{
		get { return ConfigHelper.GetStringProperty("Cache:AzureCacheAuthorizationInfo", string.Empty); }
	}


	public static bool AzureCacheUseSsl
	{
		get { return ConfigHelper.GetBoolProperty("Cache:AzureCacheUseSsl", true); }
	}

	public static bool DisableGlobalContent
	{
		get { return ConfigHelper.GetBoolProperty("DisableGlobalContent", false); }
	}

	public static bool RequireContentTitle
	{
		get { return ConfigHelper.GetBoolProperty("RequireContentTitle", true); }
	}

	public static bool RequireChangeDefaultContentTitle
	{
		get { return ConfigHelper.GetBoolProperty("RequireChangeDefaultContentTitle", true); }
	}

	public static bool PrePopulateDefaultContentTitle
	{
		get { return ConfigHelper.GetBoolProperty("PrePopulateDefaultContentTitle", true); }
	}

	public static bool UsePageContentWizard
	{
		get { return ConfigHelper.GetBoolProperty("UsePageContentWizard", true); }
	}

	public static bool DisableContentCache
	{
		get { return ConfigHelper.GetBoolProperty("DisableContentCache", true); }
	}

	public static bool UseCacheDependencyFiles
	{
		get { return ConfigHelper.GetBoolProperty("UseCacheDependencyFiles", false); }
	}

	public static bool RedirectHomeFromSetupPagesWhenSystemIsUpToDate
	{
		get { return ConfigHelper.GetBoolProperty("RedirectHomeFromSetupPagesWhenSystemIsUpToDate", true); }
	}

	public static bool AutoSuggestFriendlyUrls
	{
		get { return ConfigHelper.GetBoolProperty("AutoSuggestFriendlyUrls", true); }
	}

	public static bool AutoSuggestFriendlyUrlsOnPageNameChanges
	{
		get { return ConfigHelper.GetBoolProperty("AutoSuggestFriendlyUrlsOnPageNameChanges", true); }
	}

	public static bool DisableMetaWeblogApi
	{
		get { return ConfigHelper.GetBoolProperty("DisableMetaWeblogApi", false); }
	}

	public static bool DisableEditingPagesInMetaWeblogApi
	{
		get { return ConfigHelper.GetBoolProperty("DisableEditingPagesInMetaWeblogApi", false); }
	}

	public static bool DisableDeletingPagesInMetaWeblogApi
	{
		get { return ConfigHelper.GetBoolProperty("DisableDeletingPagesInMetaWeblogApi", false); }
	}

	public static bool AutoGeneratePageMetaDescriptionForMetaweblogNewPages
	{
		get { return ConfigHelper.GetBoolProperty("AutoGeneratePageMetaDescriptionForMetaweblogNewPages", true); }
	}

	public static int MetaweblogGeneratedMetaDescriptionMaxLength
	{
		get { return ConfigHelper.GetIntProperty("MetaweblogGeneratedMetaDescriptionMaxLength", 165); }
	}


	public static bool AppendDateToBlogUrls
	{
		get { return ConfigHelper.GetBoolProperty("AppendDateToBlogUrls", false); }
	}

	public static bool AllowForcingPreferredHostName
	{
		get { return ConfigHelper.GetBoolProperty("AllowForcingPreferredHostName", false); }
	}

	public static bool Use301RedirectWhenEnforcingPreferredHostName
	{
		get { return ConfigHelper.GetBoolProperty("Use301RedirectWhenEnforcingPreferredHostName", true); }
	}

	public static bool RedirectToRootWhenEnforcingPreferredHostName
	{
		get { return ConfigHelper.GetBoolProperty("RedirectToRootWhenEnforcingPreferredHostName", false); }
	}

	public static int SessionKeepAliveFrequencyOverrideMinutes
	{
		get { return ConfigHelper.GetIntProperty("SessionKeepAliveFrequencyOverrideMinutes", -1); }
	}

	public static bool ForceSingleSessionPerUser
	{
		get { return ConfigHelper.GetBoolProperty("ForceSingleSessionPerUser", false); }
	}

	public static bool EnforceRequirePasswordChanges
	{
		get { return ConfigHelper.GetBoolProperty("EnforcRequirePasswordChanges", true); }
	}

	/// <summary>
	/// You should not call this directly, instead use SiteUtils.SslIsAvailable()
	/// we now support separate ssl settings per site with Web.config like this:
	/// Site1-SSLIsAvailable, Site2-SSLIsAvailable etc, and trhe siteutils method resolves this
	/// </summary>
	public static bool SslisAvailable
	{
		get { return ConfigHelper.GetBoolProperty("SSLIsAvailable", false); }
	}

	public static bool RequireSslForRoleCookie
	{
		get { return ConfigHelper.GetBoolProperty("RequireSslForRoleCookie", true); }
	}

	public static bool ForceHttpForCanonicalUrlsThatDontRequireSsl
	{
		get { return ConfigHelper.GetBoolProperty("ForceHttpForCanonicalUrlsThatDontRequireSsl", false); }
	}

	public static bool ShowWarningWhenSslIsAvailableButNotUsedWithLoginModule
	{
		get { return ConfigHelper.GetBoolProperty("ShowWarningWhenSslIsAvailableButNotUsedWithLoginModule", true); }
	}

	public static bool IsRunningInRootSite
	{
		get { return ConfigHelper.GetBoolProperty("IsRunningInRootSite", true); }
	}

	public static bool ForceRegexOnEmailValidator
	{
		get { return ConfigHelper.GetBoolProperty("ForceRegexOnEmailValidator", false); }
	}

	/// <summary>
	/// if IIS or apache is set to require ssl for all pages then set thsi to true.
	/// </summary>
	public static bool SslIsRequiredByWebServer
	{
		get { return ConfigHelper.GetBoolProperty("SSLIsRequiredByWebServer", false); }
	}

	public static bool ForceSslInLoginModule
	{
		get { return ConfigHelper.GetBoolProperty("ForceSslInLoginModule", true); }
	}

	public static bool ForceSslOnAllPages
	{
		get { return ConfigHelper.GetBoolProperty("ForceSslOnAllPages", true); }
	}

	public static bool UseHSTSHeader
	{
		get { return ConfigHelper.GetBoolProperty("UseHSTSHeader", false); }
	}

	public static string HSTSHeaders
	{
		get { return ConfigHelper.GetStringProperty("HSTSHeaders", "max-age= 63072000;"); }
	}

	public static bool ClearSslOnNonSecurePages
	{
		get { return ConfigHelper.GetBoolProperty("ClearSslOnNonSecurePages", false); }
	}

	/// <summary>
	/// If the current reqest is using https, then a relative ulr for all menu items will resolve to https
	/// This setting enables chaning to fully qualified urls in the menus to avoid this
	/// which in turn avoids an unneeded redirect to enforce or clear ssl
	/// </summary>
	public static bool ResolveFullUrlsForMenuItemProtocolDifferences
	{
		get { return ConfigHelper.GetBoolProperty("ResolveFullUrlsForMenuItemProtocolDifferences", false); }
	}



	public static bool ForceSslOnProfileView
	{
		get { return ConfigHelper.GetBoolProperty("ForceSslOnProfileView", false); }
	}

	/// <summary>
	/// The title element of an html page should not exceed 65 chars.
	/// Ideally you should set this to true
	/// </summary>
	public static bool AutoTruncatePageTitles
	{
		get { return ConfigHelper.GetBoolProperty("AutoTruncatePageTitles", false); }
	}

	public static bool AutomaticallyAddCanonicalUrlToCmsPages
	{
		get { return ConfigHelper.GetBoolProperty("AutomaticallyAddCanonicalUrlToCmsPages", true); }
	}

	public static bool ShowRebuildReportsButton
	{
		get { return ConfigHelper.GetBoolProperty("ShowRebuildReportsButton", false); }
	}

	public static bool UseShortcutKeys
	{
		get { return ConfigHelper.GetBoolProperty("UseShortcutKeys", false); }
	}

	public static string AdminImage
	{
		get { return ConfigHelper.GetStringProperty("AdminImage", "key.png"); }
	}

	public static string PageTreeImage
	{
		get { return ConfigHelper.GetStringProperty("PageTreeImage", "shelf_double_down.png"); }
	}

	public static int ParentPageDialogExpansionDepth
	{
		get { return ConfigHelper.GetIntProperty("ParentPageDialogExpansionDepth", 0); }
	}

	public static string EditContentImage
	{
		get { return ConfigHelper.GetStringProperty("EditContentImage", "edit.png"); }
	}

	public static string EditPageFeaturesImage
	{
		get { return ConfigHelper.GetStringProperty("EditPageFeaturesImage", "edit_cover.png"); }
	}

	public static string EditPageSettingsImage
	{
		get { return ConfigHelper.GetStringProperty("EditPageSettingsImage", "page-settings-icon.png"); }
	}

	public static string EditPropertiesImage
	{
		get { return ConfigHelper.GetStringProperty("EditPropertiesImage", "cog-icon.png"); }
	}

	public static string DeleteLinkImage
	{
		get { return ConfigHelper.GetStringProperty("DeleteLinkImage", "delete.png"); }
	}

	public static string RSSImageFileName
	{
		get { return ConfigHelper.GetStringProperty("RSSImageFileName", "feed.png"); }
	}




	public static string NewThreadImage
	{
		get { return ConfigHelper.GetStringProperty("NewThreadImage", "messages_new.png"); }
	}

	public static string ForumThreadImage
	{
		get { return ConfigHelper.GetStringProperty("ForumThreadImage", "messages_chat.png"); }
	}



	public static bool UseIconsForAdminLinks
	{
		get { return ConfigHelper.GetBoolProperty("UseIconsForAdminLinks", true); }
	}

	public static bool UsePageImagesInSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("UsePageImagesInSiteMap", false); }
	}

	//public static bool UseMenuTooltipForCustomCss
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("UseMenuTooltipForCustomCss", true); }
	//}


	public static bool TreatChildPageIndexAsSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("TreatChildPageIndexAsSiteMap", false); }
	}


	public static bool UseTextLinksForFeatureSettings
	{
		get { return ConfigHelper.GetBoolProperty("UseTextLinksForFeatureSettings", true); }
	}

	public static bool UseSiteMailFeature
	{
		get { return ConfigHelper.GetBoolProperty("UseSiteMailFeature", false); }
	}

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
	public static bool Log404Errors
	{
		get { return ConfigHelper.GetBoolProperty("Log404Errors", true); }
	}

	public static bool LogRedirectsToPreferredHostName
	{
		get { return ConfigHelper.GetBoolProperty("LogRedirectsToPreferredHostName", false); }
	}


	public static bool TrackAuthenticatedRequests
	{
		get { return ConfigHelper.GetBoolProperty("TrackAuthenticatedRequests", true); }
	}

	public static bool TrackIPForAuthenticatedRequests
	{
		get { return ConfigHelper.GetBoolProperty("TrackIPForAuthenticatedRequests", false); }
	}

	public static string GoogleAnalyticsScriptOverrideUrl
	{
		get { return ConfigHelper.GetStringProperty("GoogleAnalyticsScriptOverrideUrl", string.Empty); }
	}

	public static bool TrackPageLoadTimeInGoogleAnalytics
	{
		get { return ConfigHelper.GetBoolProperty("TrackPageLoadTimeInGoogleAnalytics", false); }
	}

	public static bool LogGoogleAnalyticsDataToLocalWebLog
	{
		get { return ConfigHelper.GetBoolProperty("LogGoogleAnalyticsDataToLocalWebLog", false); }
	}

	public static bool SiteStatisticsShowMemberStatisticsDefault
	{
		get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowMemberStatistics_Default", true); }
	}

	public static bool SiteStatisticsShowOnlineStatisticsDefault
	{
		get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineStatistics_Default", true); }
	}

	public static bool SiteStatisticsShowOnlineMembersDefault
	{
		get { return ConfigHelper.GetBoolProperty("SiteStatistics_ShowOnlineMembers_Default", true); }
	}

	public static bool DisableFileManager
	{
		get { return ConfigHelper.GetBoolProperty("DisableFileManager", false); }
	}

	public static bool FileDialogEnableDragDrop
	{
		get { return ConfigHelper.GetBoolProperty("FileDialogEnableDragDrop", true); }
	}

	public static bool ShowFileManagerLink
	{
		get { return ConfigHelper.GetBoolProperty("ShowFileManagerLink", true); }
	}

	public static bool ShowServerPathInFileManager
	{
		get { return ConfigHelper.GetBoolProperty("ShowServerPathInFileManager", true); }
	}

	public static bool UseGreyBoxProgressForNeatUpload
	{
		get { return ConfigHelper.GetBoolProperty("UseGreyBoxProgressForNeatUpload", false); }
	}

	public static bool DisableSearchIndex
	{
		get { return ConfigHelper.GetBoolProperty("DisableSearchIndex", false); }
	}

	public static bool IndexPageKeywordsWithHtmlArticleContent
	{
		get { return ConfigHelper.GetBoolProperty("IndexPageKeywordsWithHtmlArticleContent", false); }
	}



	public static bool DisableOpenSearchAutoDiscovery
	{
		get { return ConfigHelper.GetBoolProperty("DisableOpenSearchAutoDiscovery", false); }
	}

	public static bool ShowModuleTitleInSearchResultLink
	{
		get { return ConfigHelper.GetBoolProperty("ShowModuleTitleInSearchResultLink", false); }
	}

	/// <summary>
	/// As of version 2.3.0.5, we changed the way role filtering is done in search results.
	/// We set the default to true here so that excisting indexes will not be broken.
	/// Ideaaly uoi will set this to false after rebuilding the index.
	/// Rebuilding the index will make it compatible with he new method of role filtering
	/// which prduces accurate results counts
	/// </summary>
	//public static bool SearchUseBackwardCompatibilityMode
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("SearchUseBackwardCompatibilityMode", true); }
	//}

	/// <summary>
	/// generally we should not include the page meta because it can result in duplicate results
	/// one for each instance of html content on the page because they all use the same page meta from the parent page.
	/// since page meta should reflect the content of the page it is sufficient to just index the content
	/// </summary>
	public static bool IndexPageMeta
	{
		get { return ConfigHelper.GetBoolProperty("IndexPageMeta", false); }
	}





	/// <summary>
	/// in a cluster, only one node should have this set to true
	/// clusters are only supported if they share a common file system (as of 2009-09-25)
	/// and in this configuration we should let just one node be responsible for indexing the content.
	/// </summary>
	public static bool IsSearchIndexingNode
	{
		get { return ConfigHelper.GetBoolProperty("IsSearchIndexingNode", true); }
	}

	/// <summary>
	/// disabled by default for backawards compatibility with existing indexes.
	/// if you set this to true in Web.config/user.config you should rebuild the index
	/// http://www.mojoportal.com/rebuilding-the-search-index.aspx
	/// </summary>
	public static bool EnableSearchResultsHighlighting
	{
		get { return ConfigHelper.GetBoolProperty("EnableSearchResultsHighlighting", false); }
	}

	/// <summary>
	/// disabled by default for backawards compatibility with existing indexes.
	/// if you set this to false in Web.config/user.config you should rebuild the index
	/// http://www.mojoportal.com/rebuilding-the-search-index.aspx
	/// </summary>
	public static bool DisableSearchFeatureFilters
	{
		get { return ConfigHelper.GetBoolProperty("DisableSearchFeatureFilters", true); }
	}

	/// <summary>
	/// disabled by default for backawards compatibility with existing indexes.
	/// if you set this to false in Web.config/user.config you should rebuild the index
	/// http://www.mojoportal.com/rebuilding-the-search-index.aspx
	/// </summary>
	//public static bool SearchIncludeModuleRoleFilters
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("SearchIncludeModuleRoleFilters", false); }
	//}



	public static string SearchableFeatureGuidsToExclude
	{
		get
		{
			if (ConfigurationManager.AppSettings["SearchableFeatureGuidsToExclude"] != null)
			{
				return ConfigurationManager.AppSettings["SearchableFeatureGuidsToExclude"];
			}

			return string.Empty;
		}
	}

	//public static bool UseMobileSpecificSkin
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("UseMobileSpecificSkin", false); }
	//}

	///
	///2013-08-20 JA added this to support add on products on the demo site
	/// so I can add css needed to demo the add on features without having to add/maintain it in every skin
	/// whereas customers would typically add the css that ships with the feature to their skin and list it in style.config
	///  example format ~/Data/gcss/
	/// 
	public static string GlobalAddOnStyleFolder
	{
		get { return ConfigHelper.GetStringProperty("GlobalAddOnStyleFolder", string.Empty); }
	}

	public static bool AllowMobileSkinForNonMobile
	{
		get { return ConfigHelper.GetBoolProperty("AllowMobileSkinForNonMobile", false); }
	}

	public static string MobilePhoneSkin
	{
		get { return ConfigHelper.GetStringProperty("MobilePhoneSkin", string.Empty); }
	}

	public static string SkinsToExcludeFromSkinList
	{
		get { return ConfigHelper.GetStringProperty("SkinsToExcludeFromSkinList", "printerfriendly"); }
	}

	public static string MobileDetectionExcludeUrlsCsv
	{
		get { return ConfigHelper.GetStringProperty("MobileDetectionExcludeUrlsCsv", string.Empty); }
	}

	//http://googlewebmastercentral.blogspot.com/2012/11/giving-tablet-users-full-sized-web.html
	//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11092~1#post46239
	// Android phones can be differentiated by Android; Mobile;

	public static string MobilePhoneUserAgents
	{
		get { return ConfigHelper.GetStringProperty("MobilePhoneUserAgents", "iphone,ipod,iemobile,android;blackberry"); }
	}

	public static bool DisableYUI
	{
		get { return ConfigHelper.GetBoolProperty("DisableYUI", false); }
	}

	public static bool DisableZedGraph
	{
		get { return false; }
		//get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("DisableZedGraph", false); }
	}

	public static bool IncludeFaqContentTemplate
	{
		get { return ConfigHelper.GetBoolProperty("IncludeFaqContentTemplate", true); }
	}

	public static bool IncludejQueryAccordionContentTemplate
	{
		get { return ConfigHelper.GetBoolProperty("IncludejQueryAccordionContentTemplate", true); }
	}

	public static bool IncludejQueryAccordionNoHeightContentTemplate
	{
		get { return ConfigHelper.GetBoolProperty("IncludejQueryAccordionNoHeightContentTemplate", true); }
	}

	public static bool IncludejQueryTabsContentTemplate
	{
		get { return ConfigHelper.GetBoolProperty("IncludejQueryTabsContentTemplate", true); }
	}

	public static bool Include2ColumnsOver1ColumnTemplate
	{
		get { return ConfigHelper.GetBoolProperty("Include2ColumnsOver1ColumnTemplate", true); }
	}

	public static bool Include3ColumnsOver1ColumnTemplate
	{
		get { return ConfigHelper.GetBoolProperty("Include3ColumnsOver1ColumnTemplate", true); }
	}

	public static bool Include4ColumnsOver1ColumnTemplate
	{
		get { return ConfigHelper.GetBoolProperty("Include4ColumnsOver1ColumnTemplate", true); }
	}


	//

	//public static bool AlwaysLoadYuiTabs
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("AlwaysLoadYuiTabs", false); }
	//}

	public static bool RedirectToNewPageOnCreationGlobalDefault
	{
		get { return ConfigHelper.GetBoolProperty("RedirectToNewPageOnCreationGlobalDefault", true); }
	}

	public static bool EnableDragDropPageLayout
	{
		get { return ConfigHelper.GetBoolProperty("EnableDragDropPageLayout", false); }
	}

	public static bool OpenSearchDownloadLinksInNewWindow
	{
		get { return ConfigHelper.GetBoolProperty("OpenSearchDownloadLinksInNewWindow", true); }
	}

	public static bool DisablejQuery
	{
		get { return ConfigHelper.GetBoolProperty("DisablejQuery", false); }
	}

	public static bool DisableEditArea
	{
		get { return ConfigHelper.GetBoolProperty("DisableEditArea", false); }
	}

	public static bool DisableEditAreaForCssEditor
	{
		get { return ConfigHelper.GetBoolProperty("DisableEditAreaForCssEditor", false); }
	}

	public static bool DisablejQueryUI
	{
		get { return ConfigHelper.GetBoolProperty("DisablejQueryUI", false); }
	}

	public static bool DisableExternalCommentSystems
	{
		get { return ConfigHelper.GetBoolProperty("DisableExternalCommentSystems", false); }
	}

	public static bool DisableBlogRssMetaLink
	{
		get { return ConfigHelper.GetBoolProperty("DisableBlogRssMetaLink", false); }
	}
	public static bool UseSSLForFeedLinks
	{
		get { return ConfigHelper.GetBoolProperty("UseSSLForFeedLinks", true); }
	}

	/// <summary>
	/// if true disables /Services/RecentContentRss.aspx
	/// </summary>
	public static bool DisableRecentContentFeed
	{
		get { return ConfigHelper.GetBoolProperty("DisableRecentContentFeed", true); }
	}

	public static string RecentContentChannelDescription
	{
		get { return ConfigHelper.GetStringProperty("RecentContentChannelDescription", "Recent Content"); }
	}

	public static string RecentContentChannelCopyright
	{
		get { return ConfigHelper.GetStringProperty("RecentContentChannelCopyright", string.Empty); }
	}

	public static string RecentContentChannelNotifyEmail
	{
		get { return ConfigHelper.GetStringProperty("RecentContentChannelNotifyEmail", string.Empty); }
	}

	public static int RecentContentFeedTimeToLive
	{
		get { return ConfigHelper.GetIntProperty("RecentContentFeedTimeToLive", 10); }
	}


	public static int RecentContentFeedCacheTimeInMinutes
	{
		get { return ConfigHelper.GetIntProperty("RecentContentFeedCacheTimeInMinutes", 10); }
	}

	/// <summary>
	/// by defult 30 days which means the feed will show all searchable site content
	/// with a modified date newer than 30 days ago
	/// </summary>
	public static int RecentContentFeedMaxDaysOld
	{
		get { return ConfigHelper.GetIntProperty("RecentContentFeedMaxDaysOld", 30); }
	}

	/// <summary>
	/// default of 10 items will be returned but the feed takes a param n=5 to get 5 etc
	/// </summary>
	public static int RecentContentDefaultItemsToRetrieve
	{
		get { return ConfigHelper.GetIntProperty("RecentContentDefaultItemsToRetrieve", 10); }
	}

	/// <summary>
	/// the upper limit of allowed results in the recent content feed to prevent heavy requests
	/// </summary>
	public static int RecentContentMaxItemsToRetrieve
	{
		get { return ConfigHelper.GetIntProperty("RecentContentMaxItemsToRetrieve", 60); }
	}

	public static bool AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin
	{
		get { return ConfigHelper.GetBoolProperty("AllowLoginWithUsernameWhenSiteSettingIsUseEmailForLogin", false); }
	}

	public static bool EnableNewsletter
	{
		get { return ConfigHelper.GetBoolProperty("EnableNewsletter", true); }
	}

	public static bool EnableContentWorkflow
	{
		get { return ConfigHelper.GetBoolProperty("EnableContentWorkflow", true); }
	}

	public static bool EnableAjaxControlPasswordStrength
	{
		get { return ConfigHelper.GetBoolProperty("EnableAjaxControlPasswordStrength", true); }
	}

	public static bool WorkflowShowPublishForUnSubmittedDraft
	{
		get { return ConfigHelper.GetBoolProperty("WorkflowShowPublishForUnSubmittedDraft", false); }
	}



	public static bool Use3LevelContentWorkflow
	{
		get { return ConfigHelper.GetBoolProperty("Use3LevelContentWorkflow", false); }
	}

	public static string RolesAllowedToUseWorkflowAdminPages
	{
		get { return ConfigHelper.GetStringProperty("RoleseAllowedToUseWorkflowAdminPages", string.Empty); }
	}

	public static string RolesAllowedToUseTinyMCESpellChecker
	{
		get { return ConfigHelper.GetStringProperty("RolesAllowedToUseTinyMCESpellChecker", "Authenticated Users"); }
	}

	public static bool PromptBeforeUnsubscribeNewsletter
	{
		get { return ConfigHelper.GetBoolProperty("PromptBeforeUnsubscribeNewsletter", false); }
	}

	public static bool EnableBlogSiteMap
	{
		get { return ConfigHelper.GetBoolProperty("EnableBlogSiteMap", true); }
	}

	public static bool EnforcePageSettingsInChildPageSiteMapModule
	{
		get { return ConfigHelper.GetBoolProperty("EnforcePageSettingsInChildPageSiteMapModule", false); }
	}

	public static bool HideMasterPageChildSiteMapWhenUsingModule
	{
		get { return ConfigHelper.GetBoolProperty("HideMasterPageChildSiteMapWhenUsingModule", true); }
	}

	public static bool EnableWoopraGlobally
	{
		get { return ConfigHelper.GetBoolProperty("EnableWoopraGlobally", false); }
	}

	public static bool DisableWoopraGlobally
	{
		get { return ConfigHelper.GetBoolProperty("DisableWoopraGlobally", false); }
	}

	public static bool UseOfficeFeature
	{
		get { return ConfigHelper.GetBoolProperty("UseOfficeFeature", false); }
	}

	public static bool UseSilverlightSiteOffice
	{
		get { return ConfigHelper.GetBoolProperty("UseSilverlightSiteOffice", false); }
	}


	public static bool AdaptEditorForMobile
	{
		get { return ConfigHelper.GetBoolProperty("AdaptEditorForMobile", false); }
	}

	public static bool ForceTextAreaEditorInMobile
	{
		get { return ConfigHelper.GetBoolProperty("ForceTextAreaEditorInMobile", false); }
	}

	public static bool ForceTinyMCEInSafari
	{
		get { return ConfigHelper.GetBoolProperty("ForceTinyMCEInSafari", false); }
	}

	public static bool ForceTinyMCEInOpera
	{
		get { return ConfigHelper.GetBoolProperty("ForceTinyMCEInOpera", false); }
	}

	public static bool ForcePlainTextInIphone
	{
		get { return ConfigHelper.GetBoolProperty("ForcePlainTextInIphone", true); }
	}

	public static bool ForcePlainTextInIpad
	{
		get { return ConfigHelper.GetBoolProperty("ForcePlainTextInIpad", true); }
	}

	public static bool ForcePlainTextInAndroid
	{
		get { return ConfigHelper.GetBoolProperty("ForcePlainTextInAndroid", true); }
	}


	public static bool MapAlternatePort
	{
		get { return ConfigHelper.GetBoolProperty("MapAlternatePort", true); }
	}

	public static bool MapAlternateSSLPort
	{
		get { return ConfigHelper.GetBoolProperty("MapAlternateSSLPort", true); }
	}

	public static bool ShowAlternateSearchIfConfigured
	{
		get { return ConfigHelper.GetBoolProperty("ShowAlternateSearchIfConfigured", false); }
	}

	public static int SearchResultsPageSize
	{
		get { return ConfigHelper.GetIntProperty("SearchResultsPageSize", 30); }
	}

	public static int SearchResultsFragmentSize
	{
		get { return ConfigHelper.GetIntProperty("SearchResultsFragmentSize", 500); }
	}

	public static int SearchMaxClauseCount
	{
		get { return ConfigHelper.GetIntProperty("SearchMaxClauseCount", 1024); }
	}



	public static int ContentCatalogPageSize
	{
		get { return ConfigHelper.GetIntProperty("ContentCatalogPageSize", 30); }
	}

	public static int UrlManagerPageSize
	{
		get { return ConfigHelper.GetIntProperty("UrlManagerPageSize", 30); }
	}

	public static int RedirectManagerPageSize
	{
		get { return ConfigHelper.GetIntProperty("RedirectManagerPageSize", 30); }
	}

	public static int ContentStyleTemplatePageSize
	{
		get { return ConfigHelper.GetIntProperty("ContentStyleTemplatePageSize", 30); }
	}

	public static int ContentTemplatePageSize
	{
		get { return ConfigHelper.GetIntProperty("ContentTemplatePageSize", 5); }
	}

	public static bool ContentTemplateShowBodyInAdminList
	{
		get { return ConfigHelper.GetBoolProperty("ContentTemplateShowBodyInAdminList", true); }
	}

	[Obsolete("Will be removed when we add skin-based templates. 10/31/2018")]
	public static bool AddSystemContentTemplatesAboveSiteTemplates
	{
		get { return ConfigHelper.GetBoolProperty("AddSystemContentTemplatesAboveSiteTemplates", false); }
	}

	[Obsolete("Will be removed when we add skin-based templates. 10/31/2018")]
	public static bool AddSystemContentTemplatesBelowSiteTemplates
	{
		get { return ConfigHelper.GetBoolProperty("AddSystemContentTemplatesBelowSiteTemplates", false); }
	}

	public static bool AddSystemStyleTemplatesAboveSiteTemplates
	{
		get { return ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesAboveSiteTemplates", true); }
	}

	public static bool AddSystemStyleTemplatesBelowSiteTemplates
	{
		get { return ConfigHelper.GetBoolProperty("AddSystemStyleTemplatesBelowSiteTemplates", false); }
	}

	public static int ContentRatingListPageSize
	{
		get { return ConfigHelper.GetIntProperty("ContentRatingListPageSize", 30); }
	}


	public static int MemberListPageSize
	{
		get { return ConfigHelper.GetIntProperty("MemberListPageSize", 30); }
	}

	public static int NewsletterArchivePageSize
	{
		get { return ConfigHelper.GetIntProperty("NewsletterArchivePageSize", 30); }
	}

	public static int NewsletterMaxToSendPerMinute
	{
		get { return ConfigHelper.GetIntProperty("NewsletterMaxToSendPerMinute", 0); } //0 = unlimited no throttling
	}

	public static bool NewsletterEnforceCanSpam
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterEnforceCanSpam", true); }
	}

	public static bool NewsletterUseHtmlEmailConfirmation
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterUseHtmlEmailConfirmation", false); }
	}

	public static bool NewsletterAutoSubscribeUsersCreatedByAdmin
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterAutoSubscribeUsersCreatedByAdmin", true); }
	}

	public static bool NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterExcludeAllPreviousOptOutsWhenOptingInUsers", true); }
	}

	public static bool NewsletterRequireVerification
	{
		get { return ConfigHelper.GetBoolProperty("NewsletterRequireVerification", true); }
	}

	//public static bool NewsletterShowImportPanel
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("NewsletterShowImportPanel", false); }
	//}

	/// <summary>
	/// Since we send a verification email for anonymous subscribers we have to consider the possibility that a bot will
	/// submit the same email addresses over and over frequently. We don't want to be spamming people with the verification email
	/// so if we have an existing unverified subscription and it gets submitted again we will only re-send the verification
	/// if this many days have passed since it was last submitted, default is 5 days.
	/// This way in case the user lost the original verification or his email was unavailable for some reason
	/// he can get a new opportunity to confirm by submitting again.
	/// </summary>
	public static int NewsletterReVerifcationAfterDays
	{
		get { return ConfigHelper.GetIntProperty("NewsletterReVerifcationAfterDays", 5); }
	}


	public static int MinutesBetweenAnonymousRatings
	{
		get { return ConfigHelper.GetIntProperty("MinutesBetweenAnonymousRatings", 5); }
	}

	public static int NumberOfWebPartsToShowInMiniCatalog
	{
		get { return ConfigHelper.GetIntProperty("NumberOfWebPartsToShowInMiniCatalog", 15); }
	}

	public static int WebPageInfoCacheMinutes
	{
		get { return ConfigHelper.GetIntProperty("WebPageInfoCacheMinutes", 20); }
	}

	public static int SiteSettingsCacheDurationInSeconds
	{
		get { return ConfigHelper.GetIntProperty("SiteSettingsCacheDurationInSeconds", 120); }
	}

	public static Guid InternalFeedSecurityBypassKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"] != null)
			{
				string sGuid = ConfigurationManager.AppSettings["InternalFeedSecurityBypassKey"];
				if (sGuid.Length == 36)
				{
					Guid g = Guid.Empty;
					try
					{
						g = new Guid(sGuid);
						return g;
					}
					catch (FormatException) { }
				}
			}

			return Guid.Empty;
		}
	}

	public static string PasswordRecoveryEmailTemplateFileNamePattern
	{
		get
		{
			if (ConfigurationManager.AppSettings["PasswordRecoveryEmailTemplateFileNamePattern"] != null)
			{
				return ConfigurationManager.AppSettings["PasswordRecoveryEmailTemplateFileNamePattern"];
			}
			// default value
			return "PasswordEmailMessage.config";
		}
	}

	/// <summary>
	/// possible values, MD5 (default), SHA256, SHA512
	/// for future use
	/// currently we can only use MD5 because the password field in the db is only nvarchar(128)
	/// SHA256 requires 256 bits and SHA512 requires 512 bits
	/// we will need to change to ntext (to support SQL 2000)
	/// </summary>
	public static string HashedPasswordCryptoType
	{
		get
		{
			if (ConfigurationManager.AppSettings["HashedPasswordCryptoType"] != null)
			{
				return ConfigurationManager.AppSettings["HashedPasswordCryptoType"];
			}
			// for backward compatibility this is the default
			return "MD5";
		}
	}

	public static string HashedPasswordRecoveryEmailTemplateFileNamePattern
	{
		get
		{
			if (ConfigurationManager.AppSettings["HashedPasswordRecoveryEmailTemplateFileNamePattern"] != null)
			{
				return ConfigurationManager.AppSettings["HashedPasswordRecoveryEmailTemplateFileNamePattern"];
			}
			// default value
			return "HashedPasswordEmailMessage.config";
		}
	}

	public static string RolesThatCannotBeDeleted
	{
		get { return ConfigHelper.GetStringProperty("RolesThatCannotBeDeleted", string.Empty); }
	}

	public static string DefaultContentTemplateAllowedRoles
	{
		get
		{
			if (ConfigurationManager.AppSettings["DefaultContentTemplateAllowedRoles"] != null)
			{
				return ConfigurationManager.AppSettings["DefaultContentTemplateAllowedRoles"];
			}

			return "All Users;";
		}
	}



	public static string RecaptchaPrivateKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["RecaptchaPrivateKey"] != null)
			{
				return ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
			}

			return string.Empty;
		}
	}

	public static string RecaptchaPublicKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["RecaptchaPublicKey"] != null)
			{
				return ConfigurationManager.AppSettings["RecaptchaPublicKey"];
			}

			return string.Empty;
		}
	}

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

	public static string ReCaptchaDefaultVerifyUrl
	{
		get
		{
			return ConfigHelper.GetStringProperty("reCaptcha:DefaultVerifyUrl", "http://www.google.com/recaptcha/api/siteverify");
		}
	}

	public static string HCaptchaDefaultVerifyUrl
	{
		get
		{
			return ConfigHelper.GetStringProperty("hCaptcha:DefaultVerifyUrl", "https://hcaptcha.com/siteverify");
		}
	}

	public static string ReCaptchaDefaultClientScriptUrl
	{
		get
		{
			return ConfigHelper.GetStringProperty("reCaptcha:DefaultClientScriptUrl", "https://www.google.com/recaptcha/api.js");
		}
	}

	public static string HCaptchaDefaultClientScriptUrl
	{
		get
		{
			return ConfigHelper.GetStringProperty("hCaptcha:DefaultClientScriptUrl", "https://hcaptcha.com/1/api.js");
		}
	}

	public static string ReCaptchaDefaultParam
	{
		get
		{
			return ConfigHelper.GetStringProperty("reCaptcha:DefaultParam", "g-recaptcha");
		}
	}

	public static string HCaptchaDefaultParam
	{
		get
		{
			return ConfigHelper.GetStringProperty("hCaptcha:DefaultParam", "h-captcha");
		}
	}

	public static string ReCaptchaDefaultTheme
	{
		get
		{
			return ConfigHelper.GetStringProperty("reCaptcha:DefaultTheme", "light");
		}
	}

	public static string HCaptchaDefaultTheme
	{
		get
		{
			return ConfigHelper.GetStringProperty("hCaptcha:DefaultTheme", "light");
		}
	}

	public static string ReCaptchaDefaultResponseField
	{
		get
		{
			return ConfigHelper.GetStringProperty("reCaptcha:DefaultResponseField", "g-recaptcha-response");
		}
	}

	public static string HCaptchaDefaultResponseField
	{
		get
		{
			return ConfigHelper.GetStringProperty("HCaptcha:DefaultResponseField", "h-captcha-response");
		}
	}

	public static bool UseRawUrlForCmsPageLoginRedirects
	{
		get { return ConfigHelper.GetBoolProperty("UseRawUrlForCmsPageLoginRedirects", false); }
	}

	public static bool UseAlternateFileManagerAsDefault
	{
		get { return ConfigHelper.GetBoolProperty("UseAlternateFileManagerAsDefault", false); }
	}

	public static bool ForceLowerCaseForUploadedFiles
	{
		get { return ConfigHelper.GetBoolProperty("ForceLowerCaseForUploadedFiles", true); }
	}

	public static bool ForceLegacyFileUpload
	{
		get { return ConfigHelper.GetBoolProperty("ForceLegacyFileUpload", false); }
	}

	public static bool ForceLowerCaseForFolderCreation
	{
		get { return ConfigHelper.GetBoolProperty("ForceLowerCaseForFolderCreation", true); }
	}

	public static bool ForceAdminsToUseMediaFolder
	{
		get { return ConfigHelper.GetBoolProperty("ForceAdminsToUseMediaFolder", true); }
	}

	public static bool AllowAdminsToUseDataFolder
	{
		get { return ConfigHelper.GetBoolProperty("AllowAdminsToUseDataFolder", false); }
	}

	public static bool AllowRoleAdminsToCreateContentManagers
	{
		get { return ConfigHelper.GetBoolProperty("AllowRoleAdminsToCreateContentManagers", true); }
	}

	public static bool EnforceSiteIdInModuleWrapper
	{
		get { return ConfigHelper.GetBoolProperty("EnforceSiteIdInModuleWrapper", true); }
	}

	public static bool UseFullUrlsForSkins
	{
		get { return ConfigHelper.GetBoolProperty("UseFullUrlsForSkins", false); }
	}

	public static bool UseClosestAsciiCharsForUrls
	{
		get { return ConfigHelper.GetBoolProperty("UseClosestAsciiCharsForUrls", true); }
	}

	public static bool AlwaysUrlEncode
	{
		get { return ConfigHelper.GetBoolProperty("AlwaysUrlEncode", false); }
	}

	public static bool RetryUnencodedOnUrlNotFound
	{
		get { return ConfigHelper.GetBoolProperty("RetryUnencodedOnUrlNotFound", false); }
	}

	public static bool ForceFriendlyUrlsToLowerCase
	{
		get { return ConfigHelper.GetBoolProperty("ForceFriendlyUrlsToLowerCase", true); }
	}

	public static bool SetMaintainScrollPositionOnPostBackTrueOnCmsPages
	{
		get { return ConfigHelper.GetBoolProperty("SetMaintainScrollPositionOnPostBackTrueOnCmsPages", false); }
	}



	public static bool FileSystemIsWritable
	{
		get { return ConfigHelper.GetBoolProperty("FileSystemIsWritable", true); }
	}

	public static bool FileManagerOverwriteFiles
	{
		get { return ConfigHelper.GetBoolProperty("FileManagerOverwriteFiles", true); }
	}

	/// <summary>
	/// defaults to -1 which means not specified which means use the global default from the httpRuntime element
	/// which defaults to 110 seconds
	/// for large downloads you may need to set a number here higher than 110
	/// </summary>
	public static int DownloadScriptTimeout
	{
		get { return ConfigHelper.GetIntProperty("DownloadScriptTimeout", -1); }
	}

	public static bool ImageGalleryUseMediaFolder
	{
		get { return ConfigHelper.GetBoolProperty("ImageGalleryUseMediaFolder", true); }
	}

	public static string ImageCropperWrapperDivStyle
	{
		get { return ConfigHelper.GetStringProperty("ImageCropperWrapperDivStyle", "width:100%;height:100%;overflow-x:scroll;overflow-y:visible;"); }
	}

	public static long UserFolderDiskQuotaInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("UserFolderDiskQuotaInMegaBytes", 300); }
	}

	public static long MediaFolderDiskQuotaInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("MediaFolderDiskQuotaInMegaBytes", 6000); }
	}

	public static long AdminDiskQuotaInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("AdminDiskQuotaInMegaBytes", 12000); }
	}

	public static long UserFolderMaxSizePerFileInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("UserFolderMaxSizePerFileInMegaBytes", 10); }
	}

	public static long MediaFolderMaxSizePerFileInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("MediaFolderMaxSizePerFileInMegaBytes", 30); }
	}

	public static long AdminMaxSizePerFileInMegaBytes
	{
		get { return ConfigHelper.GetLongProperty("AdminMaxSizePerFileInMegaBytes", 2048); }
	}

	public static int UserFolderMaxNumberOfFiles
	{
		get { return ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFiles", 1000); }
	}

	public static int MediaFolderMaxNumberOfFiles
	{
		get { return ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFiles", 10000); }
	}

	public static int AdminMaxNumberOfFiles
	{
		get { return ConfigHelper.GetIntProperty("AdminMaxNumberOfFiles", 100000); }
	}

	public static int UserFolderMaxNumberOfFolders
	{
		get { return ConfigHelper.GetIntProperty("UserFolderMaxNumberOfFolders", 50); }
	}

	public static int MediaFolderMaxNumberOfFolders
	{
		get { return ConfigHelper.GetIntProperty("MediaFolderMaxNumberOfFolders", 500); }
	}

	public static int AdminMaxNumberOfFolders
	{
		get { return ConfigHelper.GetIntProperty("AdminMaxNumberOfFolders", 1000); }
	}

	public static int MaxSkinFilesToUploadAtOnce
	{
		get { return ConfigHelper.GetIntProperty("MaxSkinFilesToUploadAtOnce", 10); }
	}

	public static int MaxFileManagerFilesToUploadAtOnce
	{
		get { return ConfigHelper.GetIntProperty("MaxFileManagerFilesToUploadAtOnce", 20); }
	}

	public static bool RequireFileSystemServiceToken
	{
		get { return ConfigHelper.GetBoolProperty("RequireFileSystemServiceToken", true); }
	}

	public static bool AllowFileEditInFileManager
	{
		get { return ConfigHelper.GetBoolProperty("AllowFileEditInFileManager", true); }
	}

	public static bool AllowEditingSkins
	{
		get { return ConfigHelper.GetBoolProperty("AllowEditingSkins", true); }
	}

	public static bool UseFailSafeMasterPageOnError
	{
		get { return ConfigHelper.GetBoolProperty("UseFailSafeMasterPageOnError", true); }
	}

	public static bool AllowEditingSkinsInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowEditingSkinsInChildSites", true); }
	}

	public static bool CheckFishyReferrer
	{
		get { return ConfigHelper.GetBoolProperty("CheckFishyReferer", true); }
	}

	public static bool LogFishyReferrer
	{
		get { return ConfigHelper.GetBoolProperty("LogFishyReferer", true); }
	}

	/// <summary>
	/// this is mainly needed so I can prevent people from changing the mobile skin on the demo site
	/// </summary>
	public static bool AllowSettingMobileSkinInChildSites
	{
		get { return ConfigHelper.GetBoolProperty("AllowSettingMobileSkinInChildSites", true); }
	}

	public static string DefaultInitialSkin
	{
		get { return ConfigHelper.GetStringProperty("DefaultInitialSkin", "framework"); }
	}


	public static string AllowedSkinFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AllowedSkinFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AllowedSkinFileExtensions"].ToLower();
			}
			// default value
			return ".master|.skin|.css|.jpg|.jpeg|.png|.gif|.ico|.txt|.config|.js|.webm|.weba|.webp|.html|.xml|.less|.eot|.otf|.woff|.ttf|.svg|.cshtml";
		}
	}

	public static bool DebugSkinImporter
	{
		get { return ConfigHelper.GetBoolProperty("DebugSkinImporter", false); }
	}

	public static bool IncludeContentStylesWithSkinExport
	{
		get { return ConfigHelper.GetBoolProperty("IncludeContentStylesWithSkinExport", false); } //JOE DAVIS
	}

	public static string ImageFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["ImageFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["ImageFileExtensions"].ToLower();
			}

			return ".gif|.jpg|.jpeg|.png|.svg|.webp";
		}
	}

	public static string AllowedMediaFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AllowedMediaFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AllowedMediaFileExtensions"].ToLower();
			}
			// default value
			return ImageFileExtensions + "|" + AudioFileExtensions + "|" + VideoFileExtensions;
		}
	}

	public static string AudioFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AudioFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AudioFileExtensions"].ToLower();
			}
			// default value
			return ".wmv|.mp3|.m4a|.m4v|.oga|.webma|.webm|.wav|.asf|.asx";
		}
	}

	public static string VideoFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["VideoFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["VideoFileExtensions"].ToLower();
			}
			// default value
			return ".wmv|.mp4|.m4v|.ogv|.webmv|.webm|.avi|.mov|.mpeg|.mpg";
		}
	}

	public static string JPlayerAudioFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["JPlayerAudioFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["JPlayerAudioFileExtensions"].ToLower();
			}
			// default value
			return ".mp3|.m4a|.mp4|.oga|.webma|.webm|.wav";
		}
	}

	public static string JPlayerVideoFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["JPlayerVideoFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["JPlayerVideoFileExtensions"].ToLower();
			}
			// default value
			return ".m4v|.mp4|.ogv|.webmv|.webm|.ogg";
		}
	}

	public static string JPlayerBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["JPlayerBasePath"] != null)
			{
				return ConfigurationManager.AppSettings["JPlayerBasePath"];
			}
			// default value
			return "~/ClientScript/jplayer-2-5-0/";
		}
	}

	public static string AllowedUploadFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AllowedUploadFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AllowedUploadFileExtensions"].ToLower();
			}
			// default value
			return ".gif|.jpg|.jpeg|.svg|.png|.wmv|.mp3|.mp4|.tif|.asf|.asx|.avi|.mov|.mpeg|.mpg|.zip|.pdf|.doc|.docx|.xls|.xlsx|.ppt|.pptx|.csv|.txt";
		}
	}



	public static string AllowedLessPriveledgedUserUploadFileExtensions
	{
		get
		{
			if (ConfigurationManager.AppSettings["AllowedLessPriveledgedUserUploadFileExtensions"] != null)
			{
				return ConfigurationManager.AppSettings["AllowedLessPriveledgedUserUploadFileExtensions"].ToLower();
			}
			// default value
			return ".gif|.jpg|.jpeg|.png|.svg|.zip";
		}
	}

	public static string mojoFileSystemConfigFileName
	{
		get { return ConfigHelper.GetStringProperty("mojoFileSystemConfigFileName", "mojoFileSystem.config"); }
	}

	public static string FileSystemProvider
	{
		get
		{
			return ConfigHelper.GetStringProperty("FileSystemProvider", "DiskFileSystemProvider");
		}
	}

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
	public static bool AllowDeletingFilesFromUserFolderWithoutDeleteRole
	{
		get { return ConfigHelper.GetBoolProperty("AllowDeletingFilesFromUserFolderWithoutDeleteRole", false); }
	}

	public static bool EnableInlineEditing
	{
		get { return ConfigHelper.GetBoolProperty("EnableInlineEditing", true); }
	}

	public static string CKEditorBasePath
	{
		get
		{
			string defaultPath = "~/ClientScript/ckeditor4112/";
			string path = ConfigHelper.GetStringProperty("CKEditor:BasePath", defaultPath);

			return String.IsNullOrWhiteSpace(path) ? defaultPath : path;
		}
	}

	public static string CKEditorConfigPath
	{
		get
		{
			string defaultPath = "~/ClientScript/ckeditor-mojoconfig.js";
			string path = ConfigHelper.GetStringProperty("CKEditor:ConfigPath", defaultPath);

			return String.IsNullOrWhiteSpace(path) ? defaultPath : path;
		}
	}

	public static string CKEditorFullWithTemplatesToolbarDefinition
	{
		get { return ConfigHelper.GetStringProperty("CKEditor:FullWithTemplatesToolbarDefinition", "[['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],['Undo','Redo','-','Find','Replace','Bold','Italic','Underline','Strike'],'/',['Blockquote','Styles'],['NumberedList','BulletedList'],['Link','Unlink','Anchor'],['Templates','Image','oembed','Table','HorizontalRule','Smiley','SpecialChar'],];"); }
	}

	public static bool ResizeEditorUploadedImages
	{
		get { return ConfigHelper.GetBoolProperty("ResizeEditorUploadedImages", true); }
	}

	public static bool KeepFullSizeEditorUploadedImages
	{
		get { return ConfigHelper.GetBoolProperty("KeepFullSizeEditorUploadedImages", false); }
	}

	public static bool KeepFullSizeImagesDroppedInEditor
	{
		get { return ConfigHelper.GetBoolProperty("KeepFullSizeImagesDroppedInEditor", true); }
	}

	public static int ResizeImageDefaultMaxWidth
	{
		get { return ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550); }
	}

	public static int ResizeImageDefaultMaxHeight
	{
		get { return ConfigHelper.GetIntProperty("ResizeImageDefaultMaxWidth", 550); }
	}

	public static bool AvatarsCanOnlyBeUploadedByAdmin
	{
		get { return ConfigHelper.GetBoolProperty("AvatarsCanOnlyBeUploadedByAdmin", false); }
	}

	public static bool ForceSquareAvatars
	{
		get { return ConfigHelper.GetBoolProperty("ForceSquareAvatars", true); }
	}

	public static int AvatarMaxOriginalWidth
	{
		get { return ConfigHelper.GetIntProperty("AvatarMaxOriginalWidth", 800); }
	}

	public static int AvatarMaxOriginalHeight
	{
		get { return ConfigHelper.GetIntProperty("AvatarMaxOriginalHeight", 800); }
	}

	public static int AvatarMaxWidth
	{
		get { return ConfigHelper.GetIntProperty("AvatarMaxWidth", 90); }
	}

	public static int AvatarMaxHeight
	{
		get { return ConfigHelper.GetIntProperty("AvatarMaxHeight", 90); }
	}

	public static string DefaultBlankAvatarPath
	{
		get { return ConfigHelper.GetStringProperty("DefaultBlankAvatarPath", "~/Data/SiteImages/1x1.gif"); }
	}

	public static string UrlRegex
	{
		get { return ConfigHelper.GetStringProperty("UrlRegex", @"\b(([\w-]+://?|www[.])[^\s()<>]+(?:\([\w\d]+\)|([^[:punct:]\s]|/)))"); }
	}

	public static string CustomEmailRegex
	{
		get { return ConfigHelper.GetStringProperty("CustomEmailRegex", string.Empty); }
	}

	public static string CustomEmailRegexWarning
	{
		get { return ConfigHelper.GetStringProperty("CustomEmailRegexWarning", string.Empty); }
	}

	/// <summary>
	/// not recommended to set true but it was requested to support this
	/// if true the remember me checkbox will be hidden and forcibly checked
	/// which results in a persistent auth cookie upon login
	/// </summary>
	public static bool ForcePersistentAuthCheckboxChecked
	{
		get { return ConfigHelper.GetBoolProperty("ForcePersistentAuthCheckboxChecked", false); }
	}

	public static string LoginPageRelativeUrl
	{
		get
		{
			if (ConfigurationManager.AppSettings["LoginPageRelativeUrl"] != null)
			{
				return ConfigurationManager.AppSettings["LoginPageRelativeUrl"];
			}
			// default value
			return string.Empty;
		}
	}

	public static string PageTreeRelativeUrl
	{
		get { return ConfigHelper.GetStringProperty("PageTreeRelativeUrl", $"{AdminDirectoryLocation}/PageManager.aspx"); }
	}

	public static string ContentPublishPageRelativeUrl
	{
		get { return ConfigHelper.GetStringProperty("ContentPublishPageRelativeUrl", $"{AdminDirectoryLocation}/ContentManager.aspx"); }
	}

	public static string FileDialogRelativeUrl
	{
		get { return ConfigHelper.GetStringProperty("FileDialogRelativeUrl", "/FileManager"); }
	}

	/// <summary>
	/// The filesystem location of the Admin directory. This is not configurable yet but will be to allow renaming of the Admin directory
	/// to obfuscate it.
	/// </summary>
	public static string AdminDirectoryLocation
	{
		get { return "/Admin"; }
	}





	public static string CustomRegistrationPage
	{
		get
		{
			if (ConfigurationManager.AppSettings["CustomRegistrationPage"] != null)
			{
				return ConfigurationManager.AppSettings["CustomRegistrationPage"];
			}
			// default value
			return string.Empty;
		}
	}



	public static bool RedirectRegistrationPageToCustomPage
	{
		get { return ConfigHelper.GetBoolProperty("RedirectRegistrationPageToCustomPage", false); }
	}

	public static string PageToRedirectToAfterSignIn
	{
		get
		{
			if (ConfigurationManager.AppSettings["PageToRedirectToAfterSignIn"] != null)
			{
				return ConfigurationManager.AppSettings["PageToRedirectToAfterSignIn"];
			}
			// default value
			return string.Empty;
		}
	}

	public static bool UseRedirectInSignInModule
	{
		get { return ConfigHelper.GetBoolProperty("UseRedirectInSignInModule", true); }
	}



	public static string PageToRedirectToAfterRegistration
	{
		get
		{
			if (ConfigurationManager.AppSettings["PageToRedirectToAfterRegistration"] != null)
			{
				return ConfigurationManager.AppSettings["PageToRedirectToAfterRegistration"];
			}
			// default value
			return string.Empty;
		}
	}

	public static string CKEditorH1Mapping
	{
		get { return ConfigHelper.GetStringProperty("CKEditorH1Mapping", "h3"); }
	}

	public static string CKEditorH2Mapping
	{
		get { return ConfigHelper.GetStringProperty("CKEditorH2Mapping", "h4"); }
	}

	public static string CKEditorH3Mapping
	{
		get { return ConfigHelper.GetStringProperty("CKEditorH3Mapping", "h5"); }
	}

	public static string EditorTemplatesPath
	{
		get
		{
			if (ConfigurationManager.AppSettings["EditorTemplatesPath"] != null)
			{
				return ConfigurationManager.AppSettings["EditorTemplatesPath"];
			}
			// default value
			return "~/ClientScript/mojo-editor-templates.xml";
		}
	}

	public static string EditAreaBasePath
	{
		get
		{
			if (ConfigurationManager.AppSettings["EditAreaBasePath"] != null)
			{
				return ConfigurationManager.AppSettings["EditAreaBasePath"];
			}
			// default value
			return "~/ClientScript/edit_area0811/";
		}
	}








	public static string RequestApprovalImage
	{
		get
		{
			if (ConfigurationManager.AppSettings["RequestApprovalImage"] != null)
			{
				return ConfigurationManager.AppSettings["RequestApprovalImage"];
			}
			// default value
			return "~/Data/SiteImages/glasses.png";
		}
	}

	public static string ApproveContentImage
	{
		get
		{
			if (ConfigurationManager.AppSettings["ApproveContentImage"] != null)
			{
				return ConfigurationManager.AppSettings["ApproveContentImage"];
			}
			// default value
			return "~/Data/SiteImages/done_cover.png";
		}
	}

	public static string PublishContentImage
	{
		get
		{
			if (ConfigurationManager.AppSettings["PublishContentImage"] != null)
			{
				return ConfigurationManager.AppSettings["PublishContentImage"];
			}
			// default value
			return "~/Data/SiteImages/plus.png";
		}
	}

	public static string RejectContentImage
	{
		get
		{
			if (ConfigurationManager.AppSettings["RejectContentImage"] != null)
			{
				return ConfigurationManager.AppSettings["RejectContentImage"];
			}
			// default value
			return "~/Data/SiteImages/minus_circle.png";
		}
	}

	public static string CancelContentChangesImage
	{
		get
		{
			if (ConfigurationManager.AppSettings["CancelContentChangesImage"] != null)
			{
				return ConfigurationManager.AppSettings["CancelContentChangesImage"];
			}
			// default value
			return "~/Data/SiteImages/x-circle.png";
		}
	}

	public static string RobotsConfigFile
	{
		get
		{
			if (ConfigurationManager.AppSettings["RobotsConfigFile"] != null)
			{
				return ConfigurationManager.AppSettings["RobotsConfigFile"];
			}
			// default value
			return "~/robots.config";
		}
	}

	public static string RobotsSslConfigFile
	{
		get
		{
			if (ConfigurationManager.AppSettings["RobotsSslConfigFile"] != null)
			{
				return ConfigurationManager.AppSettings["RobotsSslConfigFile"];
			}
			// default value
			return "~/robots.ssl.config";
		}
	}


	public static bool ShowRebuildSearchIndexButtonToAdmins
	{
		get { return ConfigHelper.GetBoolProperty("ShowRebuildSearchIndexButtonToAdmins", false); }
	}

	public static bool ShowCustomProfilePropertiesAboveManadotoryRegistrationFields
	{
		get { return ConfigHelper.GetBoolProperty("ShowCustomProfilePropertiesAboveManadotoryRegistrationFields", false); }
	}


	public static bool AllowUserThreadBrowsing
	{
		get { return ConfigHelper.GetBoolProperty("AllowUserThreadBrowsing", true); }
	}

	//public static bool ShowPageEncoding
	//{
	//	get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("ShowPageEncoding", false); }
	//}

	public static bool ShowUseUrlSettingInPageSettings
	{
		get { return ConfigHelper.GetBoolProperty("ShowUseUrlSettingInPageSettings", false); }
	}


	//public static bool ShowAdditionalMeta
	//{
	//    get { return mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("ShowAdditionalMeta", false); }
	//}

	public static string SetupHeaderConfigPath
	{
		get
		{
			if (ConfigurationManager.AppSettings["SetupHeaderConfigPath"] != null)
			{
				return ConfigurationManager.AppSettings["SetupHeaderConfigPath"];
			}
			// default value
			return "~/Setup/SetupHeader.config";
		}
	}

	public static string SetupHeaderConfigPathRtl
	{
		get
		{
			if (ConfigurationManager.AppSettings["SetupHeaderConfigPathRtl"] != null)
			{
				return ConfigurationManager.AppSettings["SetupHeaderConfigPathRtl"];
			}
			// default value
			return "~/Setup/SetupHeader-rtl.config";
		}
	}

	public static string SetupFooterConfigPath
	{
		get
		{
			if (ConfigurationManager.AppSettings["SetupFooterConfigPath"] != null)
			{
				return ConfigurationManager.AppSettings["SetupFooterConfigPath"];
			}
			// default value
			return "~/Setup/SetupFooter.config";
		}
	}

	public static string SetupFooterConfigPathRtl
	{
		get
		{
			if (ConfigurationManager.AppSettings["SetupFooterConfigPathRtl"] != null)
			{
				return ConfigurationManager.AppSettings["SetupFooterConfigPathRtl"];
			}
			// default value
			return "~/Setup/SetupFooter-rtl.config";
		}
	}

	public static string DefaultCountry
	{
		get
		{
			if (ConfigurationManager.AppSettings["DefaultCountry"] != null)
			{
				return ConfigurationManager.AppSettings["DefaultCountry"];
			}

			return "US";
		}
	}

	public static bool AlwaysShowAltPanesInPublishDialog
	{
		get { return ConfigHelper.GetBoolProperty("AlwaysShowAltPanesInPublishDialog", false); }
	}

	public static int TooManyPagesForDropdownList
	{
		get { return ConfigHelper.GetIntProperty("TooManyPagesForDropdownList", 150); }
	}

	public static int TooManyRolesForPageSettings
	{
		get { return ConfigHelper.GetIntProperty("TooManyRolesForPageSettings", 20); }
	}

	public static int TooManyRolesForModuleSettings
	{
		get { return ConfigHelper.GetIntProperty("TooManyRolesForModuleSettings", 20); }
	}

	public static int TooManyRolesForManageUserPage
	{
		get { return ConfigHelper.GetIntProperty("TooManyRolesForManageUserPage", 20); }
	}

	public static int TooManyPagesForGridEvents
	{
		get { return ConfigHelper.GetIntProperty("TooManyPagesForGridEvents", 150); }
	}

	/// <summary>
	/// valid options: TitleOnly, SitePlusTitle, TitlePlusSite
	/// generally you should not call this directly but use the corresponding method in SiteUtils
	/// </summary>
	public static string PageTitleFormatName
	{
		get
		{
			if (ConfigurationManager.AppSettings["PageTitleFormatName"] != null)
			{
				return ConfigurationManager.AppSettings["PageTitleFormatName"];
			}

			return "SitePlusTitle";
		}
	}

	/// <summary>
	/// used to separate the site and page title when PageTitleFormatName is SitePlusTitle or TitlePlusSite
	/// generally you should not call this directly but use the corresponding method in SiteUtils
	/// </summary>
	public static string PageTitleSeparatorString
	{
		get
		{
			if (ConfigurationManager.AppSettings["PageTitleSeparatorString"] != null)
			{
				return ConfigurationManager.AppSettings["PageTitleSeparatorString"];
			}

			return " - ";
		}
	}

	/// <summary>
	/// if true will process the cms page override title with the same format as used for autogenerated titles based on the page name and site name and format settings
	/// </summary>
	public static bool FormatOverridePageTitle
	{
		get { return ConfigHelper.GetBoolProperty("FormatOverridePageTitle", true); }
	}

	public static string VertigoSlideShowOverrideXmlConfigFile
	{
		get
		{
			if (ConfigurationManager.AppSettings["VertigoSlideShowOverrideXmlConfigFile"] != null)
			{
				return ConfigurationManager.AppSettings["VertigoSlideShowOverrideXmlConfigFile"];
			}

			return string.Empty;
		}
	}

	public static string GoogleMapsAPIKey
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleMapsAPIKey"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleMapsAPIKey"];
			}

			return string.Empty;
		}
	}

	/// <summary>
	/// valid options are internal, google, bing
	/// using google or bing requires a bingapi id or a google custom search id
	/// </summary>
	public static string PrimarySearchEngine
	{
		get
		{
			if (ConfigurationManager.AppSettings["PrimarySearchEngine"] != null)
			{
				return ConfigurationManager.AppSettings["PrimarySearchEngine"];
			}

			return string.Empty;
		}
	}

	public static string BingAPIId
	{
		get
		{
			if (ConfigurationManager.AppSettings["BingAPIId"] != null)
			{
				return ConfigurationManager.AppSettings["BingAPIId"];
			}

			return string.Empty;
		}
	}

	public static string GoogleCustomSearchId
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleCustomSearchId"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleCustomSearchId"];
			}

			return string.Empty;
		}
	}

	public static string CustomSearchDomain
	{
		get
		{
			if (ConfigurationManager.AppSettings["CustomSearchDomain"] != null)
			{
				return ConfigurationManager.AppSettings["CustomSearchDomain"];
			}

			return string.Empty;
		}
	}

	public static int BingSearchPageSize
	{
		get { return ConfigHelper.GetIntProperty("BingSearchPageSize", 30); }
	}

	public static string BingApiUrl
	{
		get
		{
			if (ConfigurationManager.AppSettings["BingApiUrl"] != null)
			{
				return ConfigurationManager.AppSettings["BingApiUrl"];
			}

			return "https://api.datamarket.azure.com/Bing/SearchWeb/";
		}
	}

	public static string Custom404Page
	{
		get
		{
			if (ConfigurationManager.AppSettings["Custom404Page"] != null)
			{
				return ConfigurationManager.AppSettings["Custom404Page"];
			}

			return "~/PageNotFound.aspx";
		}
	}

	public static bool EnableGoogle404Enhancement
	{
		get { return ConfigHelper.GetBoolProperty("EnableGoogle404Enhancement", true); }
	}

	public static string ExtensionsToSkipIn404Handler
	{
		get
		{
			if (ConfigurationManager.AppSettings["ExtensionsToSkipIn404Handler"] != null)
			{
				return ConfigurationManager.AppSettings["ExtensionsToSkipIn404Handler"].ToLower();
			}
			// default value
			return ImageFileExtensions + "|" + AudioFileExtensions + "|" + VideoFileExtensions + ".js|.css|.ashx|.axd";
		}
	}

	public static bool GoogleAnalyticsForceUniversal
	{
		get { return ConfigHelper.GetBoolProperty("GoogleAnalyticsForceUniversal", false); }
	}

	public static string GoogleAnalyticsRolesToExclude
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsRolesToExclude"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsRolesToExclude"];
			}

			return string.Empty;
		}
	}


	public static string GoogleAnalyticsMemberTypeAnonymous
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAnonymous"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAnonymous"];
			}

			return "anonymous";
		}
	}

	public static string GoogleAnalyticsMemberLabel
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberLabel"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsMemberLabel"];
			}

			return "member-type";
		}
	}

	public static string GoogleAnalyticsSectionLabel
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsSectionLabel"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsSectionLabel"];
			}

			return "section";
		}
	}

	public static string GoogleAnalyticsMemberTypeAuthenticated
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAuthenticated"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAuthenticated"];
			}

			return "member";
		}
	}

	public static string GoogleAnalyticsMemberTypeCustomer
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeCustomer"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeCustomer"];
			}

			return "customer";
		}
	}

	public static string GoogleAnalyticsMemberTypeAdmin
	{
		get
		{
			if (ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAdmin"] != null)
			{
				return ConfigurationManager.AppSettings["GoogleAnalyticsMemberTypeAdmin"];
			}

			return "admin";
		}
	}


	public static string BodyElementClientIDMode
	{
		get
		{
			if (ConfigurationManager.AppSettings["BodyElementClientIDMode"] != null)
			{
				return ConfigurationManager.AppSettings["BodyElementClientIDMode"];
			}

			return "Static";
		}
	}

	public static string HeadElementClientIDMode
	{
		get
		{
			if (ConfigurationManager.AppSettings["HeadElementClientIDMode"] != null)
			{
				return ConfigurationManager.AppSettings["HeadElementClientIDMode"];
			}

			return "Static";
		}
	}


	public static string SubSonicProvider
	{
		get
		{
			if (ConfigurationManager.AppSettings["SubSonicProvider"] != null)
			{
				return ConfigurationManager.AppSettings["SubSonicProvider"];
			}

			return string.Empty;
		}
	}

	/// <summary>
	/// this is false by default but true in user.config.sample
	/// so when the web pi installs it it should try to generate a custom machine key
	/// in web.config form setup page if it detects the default machine key
	/// false to prevent it form happening on every developer machine in main Web.config
	/// </summary>
	public static bool TryEnsureCustomMachineKeyOnSetup
	{
		get { return ConfigHelper.GetBoolProperty("TryEnsureCustomMachineKeyOnSetup", false); }
	}

	public static bool AllowUpdateCheck
	{
		get { return ConfigHelper.GetBoolProperty("AllowUpdateCheck", true); }
	}

	public static bool SecurityAdvisorLogTLSCheckResponse
	{
		get { return ConfigHelper.GetBoolProperty("SecurityAdvisorLogTLSCheckResponse", false); }
	}

	// Supported values are: MD5, SHA1, HMACSHA256, HMACSHA385, HMACSHA512
	public static string MachineKeyValidationAlgorithm
	{
		get { return ConfigHelper.GetStringProperty("MachineKeyValidationAlgorithm", "HMACSHA256"); }
	}

	// Supported values are: AES, DES, 3DES
	public static string MachineKeyDecryptionAlgorithm
	{
		get { return ConfigHelper.GetStringProperty("MachineKeyDecryptionAlgorithm", "3DES"); }
	}

	/// <summary>
	/// calls to this method should be made inside a try catch log
	/// we don't expect the Web.config file to be writable in general but it usually is on a new 
	/// installation so we can go ahead and try to update to a custom machine key
	/// </summary>
	public static void EnsureCustomMachineKey()
	{
		SecurityAdvisor securityAdvisor = new SecurityAdvisor();

		if (securityAdvisor.UsingCustomMachineKey())
		{
			return; // already using a custom key
		}

		var webConfigPath = HostingEnvironment.MapPath("~/Web.config");

		var xmlConfig = XmlHelper.GetXmlDocument(webConfigPath);

		XmlNode xmlMachineKey = xmlConfig.SelectSingleNode("/configuration/location/system.web/machineKey");

		if (xmlMachineKey == null)
		{
			xmlMachineKey = xmlConfig.SelectSingleNode("/configuration/system.web/machineKey");
		}

		//string validationKey = SiteUtils.GenerateKey(64);
		//string decryptionKey = SiteUtils.GenerateKey(64);
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
