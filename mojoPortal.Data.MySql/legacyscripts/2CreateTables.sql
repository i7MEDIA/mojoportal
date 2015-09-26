/* 
SQLyog v3.71
Host - bagend : Database - mojolocal
**************************************************************
Server version 4.1.13
*/

/*
Table structure for mp_BlogCategories
*/

CREATE TABLE `mp_BlogCategories` (
  `CategoryID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `Category` varchar(255) NOT NULL default '',
  `PostCount` int(11) NOT NULL default '0',
  PRIMARY KEY  (`CategoryID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_BlogComments
*/

CREATE TABLE `mp_BlogComments` (
  `BlogCommentID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `ItemID` int(11) NOT NULL default '0',
  `Comment` text NOT NULL,
  `Title` varchar(100) default NULL,
  `Name` varchar(100) default NULL,
  `URL` varchar(250) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`BlogCommentID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_BlogItemCategories
*/

CREATE TABLE `mp_BlogItemCategories` (
  `ID` int(11) NOT NULL auto_increment,
  `ItemID` int(11) NOT NULL default '0',
  `CategoryID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_Blogs
*/

CREATE TABLE `mp_Blogs` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `CreatedByUser` varchar(100) NOT NULL default '',
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `Title` varchar(100) default NULL,
  `Excerpt` text NOT NULL,
  `StartDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `IsInNewsletter` tinyint(1) unsigned NOT NULL default '0',
  `Description` text NOT NULL,
  `CommentCount` int(11) NOT NULL default '0',
  `TrackBackCount` int(11) NOT NULL default '0',
  `Category` varchar(50) default NULL,
  `IncludeInFeed` tinyint(1) unsigned default '1',
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_BlogStats
*/

CREATE TABLE `mp_BlogStats` (
  `ModuleID` int(11) NOT NULL default '0',
  `EntryCount` int(11) NOT NULL default '0',
  `CommentCount` int(11) NOT NULL default '0',
  `TrackBackCount` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ModuleID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_CalendarEvents
*/

CREATE TABLE `mp_CalendarEvents` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `Title` varchar(255) NOT NULL default '',
  `Description` text,
  `ImageName` varchar(100) default NULL,
  `EventDate` datetime default NULL,
  `StartTime` datetime default NULL,
  `EndTime` datetime default NULL,
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_ForumPosts
*/

CREATE TABLE `mp_ForumPosts` (
  `PostID` int(11) NOT NULL auto_increment,
  `ThreadID` int(11) NOT NULL default '0',
  `ThreadSequence` int(11) NOT NULL default '1',
  `Subject` varchar(255) NOT NULL default '',
  `PostDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `Approved` tinyint(1) unsigned NOT NULL default '1',
  `UserID` int(11) NOT NULL default '0',
  `SortOrder` int(11) NOT NULL default '100',
  `Post` text NOT NULL,
  PRIMARY KEY  (`PostID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_Forums
*/

CREATE TABLE `mp_Forums` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `CreatedDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `CreatedBy` int(11) NOT NULL default '0',
  `Title` varchar(100) NOT NULL default '',
  `Description` text NOT NULL,
  `IsModerated` tinyint(1) unsigned NOT NULL default '0',
  `IsActive` tinyint(1) unsigned NOT NULL default '0',
  `SortOrder` int(11) NOT NULL default '100',
  `ThreadCount` int(11) NOT NULL default '0',
  `PostCount` int(11) NOT NULL default '0',
  `MostRecentPostDate` datetime default NULL,
  `MostRecentPostUserID` int(11) NOT NULL default '0',
  `PostsPerPage` int(11) NOT NULL default '10',
  `ThreadsPerPage` int(11) default '20',
  `AllowAnonymousPosts` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_ForumSubscriptions
*/

CREATE TABLE `mp_ForumSubscriptions` (
  `SubscriptionID` int(11) NOT NULL auto_increment,
  `ForumID` int(11) NOT NULL default '0',
  `UserID` int(11) NOT NULL default '0',
  `SubscribeDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UnSubscribeDate` datetime default NULL,
  PRIMARY KEY  (`SubscriptionID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_ForumThreads
*/

CREATE TABLE `mp_ForumThreads` (
  `ThreadID` int(11) NOT NULL auto_increment,
  `ForumID` int(11) NOT NULL default '0',
  `ThreadSubject` varchar(255) NOT NULL default '',
  `ThreadDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `TotalViews` int(11) NOT NULL default '0',
  `TotalReplies` int(11) NOT NULL default '0',
  `SortOrder` int(11) NOT NULL default '1000',
  `IsLocked` tinyint(1) unsigned NOT NULL default '0',
  `ForumSequence` int(11) NOT NULL default '1',
  `MostRecentPostDate` datetime default NULL,
  `MostRecentPostUserID` int(11) default NULL,
  `StartedByUserID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ThreadID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_ForumThreadSubscriptions
*/

CREATE TABLE `mp_ForumThreadSubscriptions` (
  `ThreadSubscriptionID` int(11) NOT NULL auto_increment,
  `ThreadID` int(11) NOT NULL default '0',
  `UserID` int(11) NOT NULL default '0',
  `SubscribeDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `UnSubscribeDate` datetime default NULL,
  PRIMARY KEY  (`ThreadSubscriptionID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_FriendlyUrls
*/

CREATE TABLE `mp_FriendlyUrls` (
  `UrlID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '0',
  `FriendlyUrl` varchar(255) NOT NULL default '',
  `RealUrl` varchar(255) NOT NULL default '',
  `IsPattern` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`UrlID`),
  KEY `idxFriendlyUrl` (`FriendlyUrl`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_GalleryImages
*/

CREATE TABLE `mp_GalleryImages` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `DisplayOrder` int(11) NOT NULL default '100',
  `Caption` varchar(255) default NULL,
  `Description` text,
  `MetaDataXml` text,
  `ImageFile` varchar(100) default NULL,
  `WebImageFile` varchar(100) default NULL,
  `ThumbnailFile` varchar(100) default NULL,
  `UploadDate` datetime default NULL,
  `UploadUser` varchar(100) default NULL,
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_HtmlContent
*/

CREATE TABLE `mp_HtmlContent` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `Title` varchar(255) default NULL,
  `Excerpt` text,
  `Body` text NOT NULL,
  `MoreLink` varchar(255) default NULL,
  `SortOrder` int(11) default '500',
  `BeginDate` datetime default NULL,
  `EndDate` datetime default NULL,
  `CreatedDate` datetime default NULL,
  `UserID` int(11) default NULL,
  PRIMARY KEY  (`ItemID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_Links
*/

CREATE TABLE `mp_Links` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `Title` varchar(100) default NULL,
  `Url` varchar(250) default NULL,
  `ViewOrder` int(11) default NULL,
  `Description` text,
  `CreatedDate` datetime default NULL,
  `CreatedBy` int(11) default NULL,
  `Target` varchar(20) default '_blank',
  PRIMARY KEY  (`ItemID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_ModuleDefinitions
*/

CREATE TABLE `mp_ModuleDefinitions` (
  `ModuleDefID` int(11) NOT NULL auto_increment,
  `FeatureName` varchar(255) NOT NULL default '',
  `ControlSrc` varchar(255) NOT NULL default '',
  `SortOrder` int(11) default '500',
  `Icon` varchar(255) NOT NULL default '',
  `IsAdmin` tinyint(3) unsigned default '0',
  `DefaultCacheTime` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ModuleDefID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_ModuleDefinitionSettings
*/

CREATE TABLE `mp_ModuleDefinitionSettings` (
  `ID` int(11) NOT NULL auto_increment,
  `ModuleDefID` int(11) NOT NULL default '0',
  `SettingName` varchar(100) NOT NULL default '',
  `SettingValue` varchar(255) NOT NULL default '',
  `ControlType` varchar(255) default NULL,
  `RegexValidationExpression` text,
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_Modules
*/

CREATE TABLE `mp_Modules` (
  `ModuleID` int(11) NOT NULL auto_increment,
  `ModuleDefID` int(11) NOT NULL default '0',
  `ModuleTitle` varchar(255) default NULL,
  `CacheTime` int(11) NOT NULL default '0',
  `ShowTitle` tinyint(1) unsigned NOT NULL default '1',
  `EditUserID` int(11) NOT NULL default '0',
  `AuthorizedEditRoles` text,
  `SiteID` int(10) unsigned zerofill NOT NULL default '0',
  `AvailableForMyPage` tinyint(1) unsigned default '0',
  `AllowMultipleInstancesOnMyPage` tinyint(1) unsigned default '1',
  `CountOfUseOnMyPage` int(10) unsigned zerofill NOT NULL default '0',
  `Icon` varchar(255) default NULL,
  `CreatedByUserID` int(10) unsigned zerofill NOT NULL default '0',
  `CreatedDate` datetime default NULL,
  PRIMARY KEY  (`ModuleID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_ModuleSettings
*/

CREATE TABLE `mp_ModuleSettings` (
  `ID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `SettingName` varchar(50) NOT NULL default '',
  `SettingValue` varchar(255) NOT NULL default '',
  `ControlType` varchar(255) default NULL,
  `RegexValidationExpression` text,
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_PageModules
*/

CREATE TABLE `mp_PageModules` (
  `PageID` int(11) NOT NULL default '0',
  `ModuleID` int(11) NOT NULL default '0',
  `PaneName` varchar(50) NOT NULL default '',
  `ModuleOrder` int(11) NOT NULL default '0',
  `PublishBeginDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `PublishEndDate` datetime default NULL,
  PRIMARY KEY  (`PageID`,`ModuleID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_Pages
*/

CREATE TABLE `mp_Pages` (
  `PageID` int(11) NOT NULL auto_increment,
  `ParentID` int(11) NOT NULL default '-1',
  `PageOrder` int(11) NOT NULL default '0',
  `SiteID` int(11) NOT NULL default '1',
  `PageName` varchar(50) NOT NULL default '',
  `PageTitle` varchar(255) NULL ,
  `RequireSSL` tinyint(3) unsigned default '0',
  `AllowBrowserCache` bit NOT NULL ,
  `ShowBreadcrumbs` tinyint(3) unsigned default '0',
  `PageKeyWords` varchar(255) default NULL,
  `PageDescription` varchar(255) default NULL,
  `PageEncoding` varchar(255) default NULL,
  `AdditionalMetaTags` varchar(255) default NULL,
  `MenuImage` varchar(50) default NULL,
  `UseUrl` tinyint(3) unsigned NOT NULL default '0',
  `Url` varchar(255) default NULL,
  `OpenInNewWindow` tinyint(3) unsigned NOT NULL default '0',
  `ShowChildPageMenu` tinyint(3) unsigned NOT NULL default '0',
  `AuthorizedRoles` text,
  `EditRoles` text,
  `CreateChildPageRoles` text,
  `ShowChildBreadcrumbs` tinyint(1) unsigned default '0',
  `HideMainMenu` tinyint(1) unsigned default '0',
  `Skin` varchar(100) default NULL,
  `IncludeInMenu` tinyint(1) unsigned default '1',
  PRIMARY KEY  (`PageID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_Roles
*/

CREATE TABLE `mp_Roles` (
  `RoleID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '1',
  `RoleName` varchar(50) NOT NULL default '',
  `DisplayName` varchar(50) default NULL,
  PRIMARY KEY  (`RoleID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_RssFeeds
*/

CREATE TABLE `mp_RssFeeds` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `CreatedDate` datetime default NULL,
  `UserID` int(11) NOT NULL default '0',
  `Author` varchar(100) NOT NULL default '',
  `Url` varchar(255) NOT NULL default '',
  `RssUrl` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SharedFileFolders
*/

CREATE TABLE `mp_SharedFileFolders` (
  `FolderID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `FolderName` varchar(255) NOT NULL default '',
  `ParentID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`FolderID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SharedFiles
*/

CREATE TABLE `mp_SharedFiles` (
  `ItemID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `UploadUserID` int(11) NOT NULL default '0',
  `FriendlyName` varchar(255) NOT NULL default '',
  `ServerFileName` varchar(50) NOT NULL default '',
  `OriginalFileName` varchar(255) default NULL,
  `FileExtension` varchar(20) default NULL,
  `SizeInKB` int(11) NOT NULL default '0',
  `UploadDate` datetime default NULL,
  `FolderID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ItemID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SharedFilesHistory
*/

CREATE TABLE `mp_SharedFilesHistory` (
  `ID` int(11) NOT NULL auto_increment,
  `ItemID` int(11) NOT NULL default '0',
  `ModuleID` int(11) NOT NULL default '0',
  `FriendlyName` varchar(255) NOT NULL default '',
  `ServerFileName` varchar(50) NOT NULL default '',
  `SizeInKB` int(11) NOT NULL default '0',
  `UploadDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `ArchiveDate` datetime NOT NULL default '0000-00-00 00:00:00',
  `OriginalFileName` varchar(255) default NULL,
  `UploadUserID` int(10) unsigned zerofill NOT NULL default '0000000000',
  PRIMARY KEY  (`ID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SiteHosts
*/

CREATE TABLE `mp_SiteHosts` (
  `HostID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '0',
  `HostName` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`HostID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SiteModuleDefinitions
*/

CREATE TABLE `mp_SiteModuleDefinitions` (
  `SiteID` int(11) NOT NULL default '0',
  `ModuleDefID` int(11) NOT NULL default '0',
  `AuthorizedRoles` text,
  PRIMARY KEY  (`SiteID`,`ModuleDefID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SitePaths
*/

CREATE TABLE `mp_SitePaths` (
  `PathID` varchar(36) NOT NULL default '',
  `SiteID` int(11) NOT NULL default '0',
  `Path` varchar(255) NOT NULL default '',
  `LoweredPath` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`PathID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SitePersonalizationAllUsers
*/

CREATE TABLE `mp_SitePersonalizationAllUsers` (
  `PathID` varchar(36) NOT NULL default '',
  `PageSettings` longblob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`PathID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_SitePersonalizationPerUser
*/

CREATE TABLE `mp_SitePersonalizationPerUser` (
  `ID` varchar(36) NOT NULL default '',
  `PathID` varchar(36) NOT NULL default '',
  `UserID` varchar(36) NOT NULL default '',
  `PageSettings` longblob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`ID`)
) ENGINE=MyISAM  ;

/*
Table structure for mp_Sites
*/

CREATE TABLE `mp_Sites` (
  `SiteID` int(11) NOT NULL auto_increment,
  `SiteAlias` varchar(50) default NULL,
  `SiteName` varchar(128) NOT NULL default '',
  `Skin` varchar(255) default NULL,
  `Logo` varchar(50) default NULL,
  `Icon` varchar(50) default NULL,
  `AllowNewRegistration` tinyint(3) unsigned default '1',
  `AllowUserSkins` tinyint(3) unsigned default '0',
  `UseSecureRegistration` tinyint(3) unsigned default '0',
  `UseSSLOnAllPages` tinyint(3) unsigned default '0',
  `DefaultPageKeyWords` varchar(255) default NULL,
  `DefaultPageDescription` varchar(255) default NULL,
  `DefaultPageEncoding` varchar(255) default NULL,
  `DefaultAdditionalMetaTags` varchar(255) default NULL,
  `IsServerAdminSite` tinyint(1) unsigned NOT NULL default '0',
  `AllowPageSkins` tinyint(1) unsigned default '1',
  `AllowHideMenuOnPages` tinyint(1) unsigned default '1',
  `DefaultFriendlyUrlPatternenum` varchar(50) default 'PageNameWithDotASPX',
  `EditorSkin` varchar(50) default 'normal',
  `AllowUserFullNameChange` tinyint(1) unsigned default '0',
  `UseEmailForLogin` tinyint(1) unsigned default '1',
  `ReallyDeleteUsers` tinyint(1) unsigned default '1',
  `UseLdapAuth` tinyint(1) unsigned default '0',
  `AutoCreateLdapUserOnFirstLogin` tinyint(1) unsigned default '1',
  `LdapServer` varchar(255) default NULL,
  `LdapPort` int(11) default '389',
  `LdapDomain` varchar(255) default NULL,
  `LdapRootDN` varchar(255) default NULL,
  `LdapUserDNKey` varchar(10) NOT NULL default 'uid',
  `SiteGuid` varchar(36) default NULL,
  `AllowPasswordRetrieval` tinyint(1) unsigned default '1',
  `AllowPasswordReset` tinyint(1) unsigned default NULL,
  `RequiresQuestionAndAnswer` tinyint(1) unsigned default '1',
  `MaxInvalidPasswordAttempts` int(11) default '5',
  `PasswordAttemptWindowMinutes` int(11) default '5',
  `RequiresUniqueEmail` tinyint(1) unsigned default '1',
  `PasswordFormat` tinyint(1) unsigned default '0',
  `MinRequiredPasswordLength` int(11) default '4',
  `MinRequiredNonAlphanumericCharacters` int(11) default '0',
  `PasswordStrengthRegularExpression` text,
  `DefaultEmailFromAddress` varchar(100) default NULL,
  `EnableMyPageFeature` tinyint(1) unsigned default '0',
  PRIMARY KEY  (`SiteID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_UserProperties
*/

CREATE TABLE `mp_UserProperties` (
 `PropertyID` varchar(36) NOT NULL, 
 `UserGuid` varchar(36) NOT NULL,
 `PropertyName` varchar(255) NULL,
 `PropertyValueString` text NULL,
 `PropertyValueBinary` LongBlob NULL,
 `LastUpdatedDate` datetime NOT NULL,
 `IsLazyLoaded` bit NOT NULL,
 PRIMARY KEY (`PropertyID`)   
) ENGINE=MyISAM ;

/*
Table structure for mp_UserRoles
*/

CREATE TABLE `mp_UserRoles` (
  `ID` int(11) NOT NULL auto_increment,
  `UserID` int(11) NOT NULL default '0',
  `RoleID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB  ;

/*
Table structure for mp_Users
*/

CREATE TABLE `mp_Users` (
  `UserID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '1',
  `Name` varchar(50) NOT NULL default '',
  `Password` varchar(128) default NULL,
  `Email` varchar(100) NOT NULL default '',
  `Gender` char(1) default NULL,
  `ProfileApproved` tinyint(1) unsigned NOT NULL default '1',
  `ApprovedForForums` tinyint(1) unsigned NOT NULL default '1',
  `Trusted` tinyint(1) unsigned NOT NULL default '0',
  `DisplayInMemberList` tinyint(1) unsigned NOT NULL default '1',
  `WebSiteURL` varchar(100) default NULL,
  `Country` varchar(100) default NULL,
  `State` varchar(100) default NULL,
  `Occupation` varchar(100) default NULL,
  `Interests` varchar(100) default NULL,
  `MSN` varchar(50) default NULL,
  `Yahoo` varchar(50) default NULL,
  `AIM` varchar(50) default NULL,
  `ICQ` varchar(50) default NULL,
  `TotalPosts` int(11) NOT NULL default '0',
  `AvatarUrl` varchar(255) default 'blank.gif',
  `AvatarType` int(11) default NULL,
  `TimeOffsetHours` int(11) default '0',
  `Signature` varchar(255) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserGuid` varchar(36) default NULL,
  `Skin` varchar(100) default NULL,
  `LoginName` varchar(50) default NULL,
  `IsDeleted` tinyint(1) unsigned default '0',
  `LoweredEmail` varchar(100) default NULL,
  `RegisterConfirmGuid` varchar(36) default '00000000-0000-0000-0000-000000000000',
  `PasswordQuestion` varchar(255) default NULL,
  `PasswordAnswer` varchar(255) default NULL,
  `LastActivityDate` datetime default NULL,
  `LastLoginDate` datetime default NULL,
  `LastPasswordChangedDate` datetime default NULL,
  `LastLockoutDate` datetime default NULL,
  `FailedPasswordAttemptCount` int(11) default '0',
  `FailedPasswordAttemptWindowStart` datetime default NULL,
  `FailedPasswordAnswerAttemptCount` int(11) default '0',
  `FailedPasswordAnswerAttemptWindowStart` datetime default NULL,
  `IsLockedOut` tinyint(1) unsigned default '0',
  `MobilePIN` varchar(16) default NULL,
  `PasswordSalt` varchar(128) default NULL,
  `Comment` text,
  PRIMARY KEY  (`UserID`),
  KEY `idxEmail` (`Email`),
  KEY `Name` (`Name`)
) ENGINE=InnoDB  ;

CREATE TABLE `mp_WebParts` (
 `WebPartID` varchar(36) NOT NULL, 
 `SiteID` int(11) NOT NULL,
 `Title` varchar(255) NOT NULL,
 `Description` varchar(255) NOT NULL,
 `ImageUrl` varchar(255) NULL,
 `ClassName` varchar(255) NOT NULL,
 `AssemblyName` varchar(255) NOT NULL,
 `AvailableForMyPage` tinyint(1) unsigned NOT NULL default '0',
 `AllowMultipleInstancesOnMyPage` tinyint(1) unsigned NOT NULL default '1',
 `AvailableForContentSystem` tinyint(1) unsigned NOT NULL default '0',
 `CountOfUseOnMyPage` int(10) unsigned zerofill NOT NULL default '0',
 PRIMARY KEY (`WebPartID`)     
) ENGINE=MyISAM ;

CREATE TABLE `mp_UserPages` (
 `UserPageID` varchar(36) NOT NULL, 
 `SiteID` int(11) NOT NULL,
 `UserGuid` varchar(36) NOT NULL,
 `PageName` varchar(255) NOT NULL,
 `PagePath` varchar(255) NOT NULL,
 `PageOrder` int(11) NOT NULL default '0',
 PRIMARY KEY (`UserPageID`)   
) ENGINE=MyISAM ;


CREATE TABLE `mp_SchemaVersion` (
 `ApplicationID` varchar(36) NOT NULL, 
 `ApplicationName` varchar(255) NOT NULL,
 `Major` int(11) NOT NULL default '0',
 `Minor` int(11) NOT NULL default '0',
 `Build` int(11) NOT NULL default '0',
 `Revision` int(11) NOT NULL default '0',
 PRIMARY KEY (`ApplicationID`)   
) ENGINE=MyISAM ;

CREATE TABLE `mp_SchemaScriptHistory` (
 `ID` int(11) NOT NULL auto_increment, 
 `ApplicationID` varchar(36) NOT NULL,
 `ScriptFile` varchar(255) NOT NULL,
 `RunTime` datetime NOT NULL,
 `ErrorOccurred` tinyint(1) unsigned NOT NULL,
 `ErrorMessage` text NULL,
 `ScriptBody` text NULL,
 PRIMARY KEY (`ID`)    
) ENGINE=MyISAM ;



/*

CREATE TABLE `mp_PrivateMessages` (
 `MessageID` varchar(36) NOT NULL, 
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
 `SentDate` datetime NULL,
 PRIMARY KEY (`MessageID`)   
) ENGINE=MyISAM ;


CREATE TABLE `mp_PrivateMessagePriority` (
 `PriorityID` varchar(36) NOT NULL, 
 `Priority` varchar(50) NOT NULL,
 PRIMARY KEY (`PriorityID`)   
) ENGINE=MyISAM ;


CREATE TABLE `mp_PrivateMessageAttachments` (
 `AttachmentID` varchar(36) NOT NULL, 
 `MessageID` varchar(36) NOT NULL,
 `OriginalFileName` varchar(255) NOT NULL,
 `ServerFileName` varchar(50) NOT NULL,
 `CreatedDate` datetime NOT NULL,
 PRIMARY KEY (`AttachmentID`)   
) ENGINE=MyISAM ;

*/