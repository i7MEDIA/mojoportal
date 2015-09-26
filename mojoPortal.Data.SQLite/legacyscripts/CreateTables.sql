
/*
Table structure for mp_BlogCategories
*/

CREATE TABLE `mp_BlogCategories` (
  `CategoryID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `Category` varchar(255) NOT NULL default '',
  `PostCount` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_BlogComments
*/

CREATE TABLE `mp_BlogComments` (
  `BlogCommentID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `ItemID` INTEGER NOT NULL default '0',
  `Comment` text NOT NULL,
  `Title` varchar(100) default NULL,
  `Name` varchar(100) default NULL,
  `URL` varchar(250) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00'
);

/*
Table structure for mp_BlogItemCategories
*/

CREATE TABLE `mp_BlogItemCategories` (
  `ID` INTEGER PRIMARY KEY,
  `ItemID` INTEGER NOT NULL default '0',
  `CategoryID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_Blogs
*/

CREATE TABLE `mp_Blogs` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `CreatedByUser` varchar(100) NOT NULL default '',
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `Title` varchar(100) default NULL,
  `Excerpt` text NOT NULL,
  `StartDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `IsInNewsletter` INTEGER NOT NULL default '0',
  `Description` text NOT NULL,
  `CommentCount` INTEGER NOT NULL default '0',
  `TrackBackCount` INTEGER NOT NULL default '0',
  `Category` varchar(50) default NULL,
  `IncludeInFeed` INTEGER default '1',
  `AllowCommentsForDays` INTEGER default '0'
);

/*
Table structure for mp_BlogStats
*/

CREATE TABLE `mp_BlogStats` (
  `ModuleID` INTEGER NOT NULL default '0',
  `EntryCount` INTEGER NOT NULL default '0',
  `CommentCount` INTEGER NOT NULL default '0',
  `TrackBackCount` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_CalendarEvents
*/

CREATE TABLE `mp_CalendarEvents` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `Title` varchar(255) NOT NULL default '',
  `Description` text,
  `ImageName` varchar(100) default NULL,
  `EventDate` datetime default NULL,
  `StartTime` datetime default NULL,
  `EndTime` datetime default NULL,
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_ForumPosts
*/

CREATE TABLE `mp_ForumPosts` (
  `PostID` INTEGER PRIMARY KEY,
  `ThreadID` INTEGER NOT NULL default '0',
  `ThreadSequence` INTEGER NOT NULL default '1',
  `Subject` varchar(255) NOT NULL default '',
  `PostDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `Approved` INTEGER NOT NULL default '1',
  `UserID` INTEGER NOT NULL default '0',
  `SortOrder` INTEGER NOT NULL default '100',
  `Post` text NOT NULL
);

/*
Table structure for mp_Forums
*/

CREATE TABLE `mp_Forums` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `CreatedBy` INTEGER NOT NULL default '0',
  `Title` varchar(100) NOT NULL default '',
  `Description` text NOT NULL,
  `IsModerated` INTEGER NOT NULL default '0',
  `IsActive` INTEGER NOT NULL default '0',
  `SortOrder` INTEGER NOT NULL default '100',
  `ThreadCount` INTEGER NOT NULL default '0',
  `PostCount` INTEGER NOT NULL default '0',
  `MostRecentPostDate` datetime default NULL,
  `MostRecentPostUserID` INTEGER NOT NULL default '0',
  `PostsPerPage` INTEGER NOT NULL default '10',
  `ThreadsPerPage` INTEGER default '20',
  `AllowAnonymousPosts` INTEGER NOT NULL default '1'
);

/*
Table structure for mp_ForumSubscriptions
*/

CREATE TABLE `mp_ForumSubscriptions` (
  `SubscriptionID` INTEGER PRIMARY KEY,
  `ForumID` INTEGER NOT NULL default '0',
  `UserID` INTEGER NOT NULL default '0',
  `SubscribeDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UnSubscribeDate` datetime default NULL
);

/*
Table structure for mp_ForumThreads
*/

CREATE TABLE `mp_ForumThreads` (
  `ThreadID` INTEGER PRIMARY KEY,
  `ForumID` INTEGER NOT NULL default '0',
  `ThreadSubject` varchar(255) NOT NULL default '',
  `ThreadDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `TotalViews` INTEGER NOT NULL default '0',
  `TotalReplies` INTEGER NOT NULL default '0',
  `SortOrder` INTEGER NOT NULL default '1000',
  `IsLocked` INTEGER NOT NULL default '0',
  `ForumSequence` INTEGER NOT NULL default '1',
  `MostRecentPostDate` datetime default NULL,
  `MostRecentPostUserID` INTEGER default NULL,
  `StartedByUserID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_ForumThreadSubscriptions
*/

CREATE TABLE `mp_ForumThreadSubscriptions` (
  `ThreadSubscriptionID` INTEGER PRIMARY KEY,
  `ThreadID` INTEGER NOT NULL default '0',
  `UserID` INTEGER NOT NULL default '0',
  `SubscribeDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UnSubscribeDate` datetime default NULL
);

/*
Table structure for mp_FriendlyUrls
*/

CREATE TABLE `mp_FriendlyUrls` (
  `UrlID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '0',
  `FriendlyUrl` varchar(255) NOT NULL default '',
  `RealUrl` varchar(255) NOT NULL default '',
  `IsPattern` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_GalleryImages
*/

CREATE TABLE `mp_GalleryImages` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `DisplayOrder` INTEGER NOT NULL default '100',
  `Caption` varchar(255) default NULL,
  `Description` text,
  `MetaDataXml` text,
  `ImageFile` varchar(100) default NULL,
  `WebImageFile` varchar(100) default NULL,
  `ThumbnailFile` varchar(100) default NULL,
  `UploadDate` datetime default NULL,
  `UploadUser` varchar(100) default NULL
);

/*
Table structure for mp_HtmlContent
*/

CREATE TABLE `mp_HtmlContent` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `Title` varchar(255) default NULL,
  `Excerpt` text,
  `Body` text NOT NULL,
  `MoreLink` varchar(255) default NULL,
  `SortOrder` INTEGER default '500',
  `BeginDate` datetime default NULL,
  `EndDate` datetime default NULL,
  `CreatedDate` datetime default NULL,
  `UserID` INTEGER default NULL
);

/*
Table structure for mp_Links
*/

CREATE TABLE `mp_Links` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `Title` varchar(100) default NULL,
  `Url` varchar(250) default NULL,
  `ViewOrder` INTEGER default NULL,
  `Description` text,
  `CreatedDate` datetime default NULL,
  `CreatedBy` INTEGER default NULL,
  `Target` varchar(20) default '_blank'
);

/*
Table structure for mp_ModuleDefinitions
*/

CREATE TABLE `mp_ModuleDefinitions` (
  `ModuleDefID` INTEGER PRIMARY KEY,
  `Guid` char(36) NOT NULL,
  `FeatureName` varchar(255) NOT NULL default '',
  `ControlSrc` varchar(255) NOT NULL default '',
  `SortOrder` INTEGER default '500',
  `DefaultCacheTime` INTEGER NOT NULL default '0',
  `Icon` varchar(255) NOT NULL default '',
  `IsAdmin` INTEGER default '0'
);

/*
Table structure for mp_ModuleDefinitionSettings
*/

CREATE TABLE `mp_ModuleDefinitionSettings` (
  `ID` INTEGER PRIMARY KEY,
  `ModuleDefID` INTEGER NOT NULL default '0',
  `FeatureGuid` char(36) NOT NULL,
  `ResourceFile` varchar(255) NULL,
  `SettingName` varchar(100) NOT NULL default '',
  `SettingValue` varchar(255) NOT NULL default '',
  `ControlType` varchar(255) default NULL,
  `RegexValidationExpression` text
);

/*
Table structure for mp_Modules
*/

CREATE TABLE `mp_Modules` (
  `ModuleID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '0',
  `ModuleDefID` INTEGER NOT NULL default '0',
  `ModuleTitle` varchar(255) default NULL,
  `CacheTime` INTEGER NOT NULL default '0',
  `ShowTitle` INTEGER NOT NULL default '1',
  `EditUserID` INTEGER default NULL,
  `AuthorizedEditRoles` text,
  `AvailableForMyPage` INTEGER NOT NULL default'0',
  `AllowMultipleInstancesOnMyPage` INTEGER NOT NULL default'1',
  `CountOfUseOnMyPage` INTEGER NOT NULL default '0',
  `Icon` varchar(255) NOT NULL default '',
  `CreatedByUserID` INTEGER NOT NULL default '0',
  `CreatedDate` datetime default NULL 
);

/*
Table structure for mp_ModuleSettings
*/

CREATE TABLE `mp_ModuleSettings` (
  `ID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `SettingName` varchar(50) NOT NULL default '',
  `SettingValue` varchar(255) NOT NULL default '',
  `ControlType` varchar(255) default NULL,
  `RegexValidationExpression` text
  );

/*
Table structure for mp_PageModules
*/

CREATE TABLE `mp_PageModules` (
 `RowID` INTEGER NOT NULL PRIMARY KEY,
  `PageID` INTEGER NOT NULL ,
  `ModuleID` INTEGER NOT NULL ,
  `PaneName` varchar(50) NOT NULL default '',
  `ModuleOrder` INTEGER NOT NULL default '0',
  `PublishBeginDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `PublishEndDate` datetime default NULL
) ;


/*
Table structure for mp_Pages
*/

CREATE TABLE `mp_Pages` (
  `PageID` INTEGER PRIMARY KEY,
  `ParentID` INTEGER NOT NULL default '-1',
  `PageOrder` INTEGER NOT NULL default '0',
  `SiteID` INTEGER NOT NULL default '1',
  `PageName` varchar(50) NOT NULL default '',
  `PageTitle` varchar(255) NULL ,
  `RequireSSL` INTEGER default '0',
  `AllowBrowserCache` INTEGER default '1',
  `ShowBreadcrumbs` INTEGER default '0',
  `PageKeyWords` varchar(255) default NULL,
  `PageDescription` varchar(255) default NULL,
  `PageEncoding` varchar(255) default NULL,
  `AdditionalMetaTags` varchar(255) default NULL,
  `MenuImage` varchar(50) default NULL,
  `UseUrl` INTEGER NOT NULL default '0',
  `Url` varchar(255) default NULL,
  `OpenInNewWindow` INTEGER NOT NULL default '0',
  `ShowChildPageMenu` INTEGER NOT NULL default '0',
  `AuthorizedRoles` text,
  `EditRoles` text,
  `CreateChildPageRoles` text,
  `ShowChildBreadCrumbs` INTEGER default '0',
  `HideMainMenu` INTEGER default '0',
  `Skin` varchar(100) default NULL,
  `IncludeInMenu` INTEGER NOT NULL default '1'
);

/*
Table structure for mp_Roles
*/

CREATE TABLE `mp_Roles` (
  `RoleID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '1',
  `RoleName` varchar(50) NOT NULL default '',
  `DisplayName` varchar(50) default NULL
);

/*
Table structure for mp_RssFeeds
*/

CREATE TABLE `mp_RssFeeds` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `CreatedDate` datetime default NULL,
  `UserID` INTEGER NOT NULL default '0',
  `Author` varchar(100) NOT NULL default '',
  `Url` varchar(255) NOT NULL default '',
  `RssUrl` varchar(255) NOT NULL default ''
);

/*
Table structure for mp_SharedFileFolders
*/

CREATE TABLE `mp_SharedFileFolders` (
  `FolderID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `FolderName` varchar(255) NOT NULL default '',
  `ParentID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_SharedFiles
*/

CREATE TABLE `mp_SharedFiles` (
  `ItemID` INTEGER PRIMARY KEY,
  `ModuleID` INTEGER NOT NULL default '0',
  `UploadUserID` INTEGER NOT NULL default '0',
  `FriendlyName` varchar(255) NOT NULL default '',
  `ServerFileName` varchar(50) NOT NULL default '',
  `OriginalFileName` varchar(255) default NULL,
  `FileExtension` varchar(20) default NULL,
  `SizeInKB` INTEGER NOT NULL default '0',
  `UploadDate` datetime default NULL,
  `FolderID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_SharedFilesHistory
*/

CREATE TABLE `mp_SharedFilesHistory` (
  `ID` INTEGER PRIMARY KEY,
  `ItemID` INTEGER NOT NULL default '0',
  `ModuleID` INTEGER NOT NULL default '0',
  `FriendlyName` varchar(255) NOT NULL default '',
  `ServerFileName` varchar(50) NOT NULL default '',
  `SizeInKB` INTEGER NOT NULL default '0',
  `UploadDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `ArchiveDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `OriginalFileName` varchar(255) default NULL,
  `UploadUserID` INTEGER NOT NULL default '1'
);

/*
Table structure for mp_SiteHosts
*/

CREATE TABLE `mp_SiteHosts` (
  `HostID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '0',
  `HostName` varchar(255) NOT NULL default ''
);

/*
Table structure for mp_SiteModuleDefinitions
*/

CREATE TABLE `mp_SiteModuleDefinitions` (
  `SiteID` INTEGER NOT NULL default '0',
  `ModuleDefID` INTEGER NOT NULL default '0',
  `AuthorizedRoles` text
);

/*
Table structure for mp_SitePaths
*/

CREATE TABLE `mp_SitePaths` (
  `PathID` varchar(36) NOT NULL default '',
  `SiteID` INTEGER NOT NULL default '0',
  `Path` varchar(255) NOT NULL default '',
  `LoweredPath` varchar(255) NOT NULL default ''
);


/*
Table structure for mp_SitePersonalizationAllUsers
*/

CREATE TABLE `mp_SitePersonalizationAllUsers` (
  `PathID` varchar(36) NOT NULL PRIMARY KEY,
  `PageSettings` blob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00'
) ;

/*
Table structure for mp_SitePersonalizationPerUser
*/

CREATE TABLE `mp_SitePersonalizationPerUser` (
  `ID` varchar(36) NOT NULL PRIMARY KEY,
  `PathID` varchar(36) NOT NULL default '',
  `UserID` varchar(36) NOT NULL default '',
  `PageSettings` blob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00'
);



/*
Table structure for mp_Sites
*/

CREATE TABLE `mp_Sites` (
  `SiteID` INTEGER PRIMARY KEY,
  `SiteAlias` varchar(50) default NULL,
  `SiteName` varchar(128) NOT NULL default '',
  `Skin` varchar(255) default NULL,
  `Logo` varchar(50) default NULL,
  `Icon` varchar(50) default NULL,
  `AllowNewRegistration` INTEGER default '1',
  `AllowUserSkins` INTEGER default '0',
  `UseSecureRegistration` INTEGER default '0',
  `UseSSLOnAllPages` INTEGER default '0',
  `DefaultPageKeyWords` varchar(255) default NULL,
  `DefaultPageDescription` varchar(255) default NULL,
  `DefaultPageEncoding` varchar(255) default NULL,
  `DefaultAdditionalMetaTags` varchar(255) default NULL,
  `IsServerAdminSite` INTEGER NOT NULL default '0',
  `AllowPageSkins` INTEGER default '1',
  `AllowHideMenuOnPages` INTEGER default '1',
  `DefaultFriendlyUrlPatternenum` varchar(50) default 'PageNameWithDotASPX',
  `EditorProvider` varchar(255) default '',
  `EditorSkin` varchar(50) default 'normal',
  `AllowUserFullNameChange` INTEGER default '0',
  `UseEmailForLogin` INTEGER default '1',
  `ReallyDeleteUsers` INTEGER default '1',
  `UseLdapAuth` INTEGER default '0',
  `AutoCreateLdapUserOnFirstLogin` INTEGER default '1',
  `LdapServer` varchar(255) default NULL,
  `LdapPort` INTEGER default '389',
  `LdapRootDN` varchar(255) default NULL,
  `LdapDomain` varchar(255) default NULL,
  `LdapUserDNKey` varchar(10) NOT NULL default 'uid',
  `SiteGuid` varchar(36) default NULL,
  `AllowPasswordRetrieval` INTEGER default '1',
  `AllowPasswordReset` INTEGER default '1',
  `RequiresQuestionAndAnswer` INTEGER default '1',
  `MaxInvalidPasswordAttempts` int(11) default '5',
  `PasswordAttemptWindowMinutes` INTEGER default '5',
  `RequiresUniqueEmail` INTEGER default '1',
  `PasswordFormat` INTEGER default '0',
  `MinRequiredPasswordLength` INTEGER default '4',
  `MinReqNonAlphaChars` INTEGER default '0',
  `PwdStrengthRegex` text,
  `DefaultEmailFromAddress` varchar(100) default NULL,
  `EnableMyPageFeature` INTEGER default '0'
);

/*
Table structure for mp_UserProperties
*/

CREATE TABLE `mp_UserProperties` (
 `PropertyID` varchar(36) NOT NULL PRIMARY KEY, 
 `UserGuid` varchar(36) NOT NULL,
 `PropertyName` varchar(255) NOT NULL,
 `PropertyValueString` text NULL,
 `PropertyValueBinary` longblob NULL,
 `LastUpdatedDate` datetime NOT NULL,
 `IsLazyLoaded` INTEGER NOT NULL
);

/*
Table structure for mp_UserRoles
*/

CREATE TABLE `mp_UserRoles` (
  `ID` INTEGER PRIMARY KEY,
  `UserID` INTEGER NOT NULL default '0',
  `RoleID` INTEGER NOT NULL default '0'
);

/*
Table structure for mp_Users
*/
CREATE TABLE `mp_Users` (
  `UserID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '1',
  `Name` varchar(50) NOT NULL default '',
  `Password` varchar(128) default NULL,
  `Email` varchar(100) NOT NULL default '',
  `Gender` char(1) default NULL,
  `ProfileApproved` INTEGER NOT NULL default '1',
  `ApprovedForForums` INTEGER NOT NULL default '1',
  `Trusted` INTEGER NOT NULL default '0',
  `DisplayInMemberList` INTEGER NOT NULL default '1',
  `WebSiteURL` varchar(100) default NULL,
  `Country` varchar(100) default NULL,
  `State` varchar(100) default NULL,
  `Occupation` varchar(100) default NULL,
  `Interests` varchar(100) default NULL,
  `MSN` varchar(50) default NULL,
  `Yahoo` varchar(50) default NULL,
  `AIM` varchar(50) default NULL,
  `ICQ` varchar(50) default NULL,
  `TotalPosts` INTEGER NOT NULL default '0',
  `AvatarUrl` varchar(255) default 'blank.gif',
  `AvatarType` INTEGER default NULL,
  `TimeOffsetHours` INTEGER default '0',
  `Signature` varchar(255) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserGuid` varchar(36) default NULL,
  `Skin` varchar(100) default NULL,
  `LoginName` varchar(50) default NULL,
  `IsDeleted` INTEGER default '0',
  `LoweredEmail` varchar(100) default NULL,
  `RegisterConfirmGuid` varchar(36) default '00000000-0000-0000-0000-000000000000',
  `PasswordQuestion` varchar(255) default NULL,
  `PasswordAnswer` varchar(255) default NULL,
  `LastActivityDate` datetime default NULL,
  `LastLoginDate` datetime default NULL,
  `LastPasswordChangedDate` datetime default NULL,
  `LastLockoutDate` datetime default NULL,
  `FailedPasswordAttemptCount` INTEGER default '0',
  `FailedPwdAttemptWindowStart` datetime default NULL,
  `FailedPwdAnswerAttemptCount` INTEGER default '0',
  `FailedPwdAnswerWindowStart` datetime default NULL,
  `IsLockedOut` INTEGER default '0',
  `MobilePIN` varchar(16) default NULL,
  `PasswordSalt` varchar(128) default NULL,
  `Comment` text
);

-- added 6/4/2006

CREATE TABLE `mp_WebParts` (
 `WebPartID` varchar(36) NOT NULL PRIMARY KEY, 
 `SiteID` INTEGER NOT NULL,
 `Title` varchar(255) NOT NULL,
 `Description` varchar(255) NOT NULL,
 `ImageUrl` varchar(255) NULL,
 `ClassName` varchar(255) NOT NULL,
 `AssemblyName` varchar(255) NOT NULL,
 `AvailableForMyPage` INTEGER NOT NULL default '0',
 `AllowMultipleInstancesOnMyPage` INTEGER NOT NULL default '1',
 `AvailableForContentSystem` INTEGER NOT NULL default '0',
 `CountOfUseOnMyPage` INTEGER NOT NULL default '0'
);

CREATE TABLE `mp_UserPages` (
 `UserPageID` varchar(36) NOT NULL PRIMARY KEY, 
 `SiteID` INTEGER NOT NULL,
 `UserGuid` varchar(36) NOT NULL,
 `PageName` varchar(255) NOT NULL,
 `PagePath` varchar(255) NOT NULL,
 `PageOrder` INTEGER NOT NULL default '0'
);

CREATE TABLE `mp_SchemaVersion` (
 `ApplicationID` varchar(36) NOT NULL PRIMARY KEY, 
 `ApplicationName` varchar(255) NOT NULL,
 `Major` INTEGER NOT NULL default '0',
 `Minor` INTEGER NOT NULL default '0',
 `Build` INTEGER NOT NULL default '0',
 `Revision` INTEGER NOT NULL default '0'
);

CREATE TABLE `mp_SchemaScriptHistory` (
 `ID` INTEGER NOT NULL PRIMARY KEY, 
 `ApplicationID` varchar(36) NOT NULL,
 `ScriptFile` varchar(255) NOT NULL,
 `RunTime` datetime NOT NULL,
 `ErrorOccurred` INTEGER NOT NULL,
 `ErrorMessage` text NULL,
 `ScriptBody` text NULL
);

CREATE TABLE `mp_SiteFolders` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `SiteGuid` varchar(36) NOT NULL,
 `FolderName` varchar(255) NOT NULL
);

/*
CREATE TABLE `mp_PrivateMessages` (
 `MessageID` varchar(36) NOT NULL PRIMARY KEY, 
 `FromUser` varchar(36) NOT NULL,
 `PriorityID` varchar(36) NULL,
 `Subject` varchar(255) NOT NULL,
 `Body` Text NOT NULL,
 `ToCSVList` Text NULL,
 `CcCSVList` Text NULL,
 `BccCSVList` Text NULL,
 `ToCSVLabels` Text NULL,
 `CcCSVLabels` Text NULL,
 `BccCSVLabels` Text NULL,
 `CreatedDate` datetime NOT NULL,
 `SentDate` datetime NULL
);

CREATE TABLE `mp_PrivateMessagePriority` (
 `PriorityID` varchar(36) NOT NULL PRIMARY KEY, 
 `Priority` varchar(50) NOT NULL
);

CREATE TABLE `mp_PrivateMessageAttachments` (
 `AttachmentID` varchar(36) NOT NULL PRIMARY KEY, 
 `MessageID` varchar(36) NOT NULL,
 `OriginalFileName` varchar(255) NOT NULL,
 `ServerFileName` varchar(50) NOT NULL,
 `CreatedDate` datetime NOT NULL
);

*/

