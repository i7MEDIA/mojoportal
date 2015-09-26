
ALTER TABLE mp_Sites ADD COLUMN SiteGuid varchar(36);
UPDATE mp_Sites SET SiteGuid = '00000000-0000-0000-0000-000000000000';

ALTER TABLE mp_Sites ADD COLUMN AllowPasswordRetrieval tinyint(1) unsigned;
UPDATE mp_Sites SET AllowPasswordRetrieval = 1;

ALTER TABLE mp_Sites ADD COLUMN AllowPasswordReset tinyint(1) unsigned;
UPDATE mp_Sites SET AllowPasswordReset = 1;

ALTER TABLE mp_Sites ADD COLUMN RequiresQuestionAndAnswer tinyint(1) unsigned;
UPDATE mp_Sites SET RequiresQuestionAndAnswer = 1;

ALTER TABLE mp_Sites ADD COLUMN MaxInvalidPasswordAttempts integer DEFAULT 5;
UPDATE mp_Sites SET MaxInvalidPasswordAttempts = 5;

ALTER TABLE mp_Sites ADD COLUMN PasswordAttemptWindowMinutes integer DEFAULT 5;
UPDATE mp_Sites SET PasswordAttemptWindowMinutes = 5;

ALTER TABLE mp_Sites ADD COLUMN RequiresUniqueEmail tinyint(1) unsigned;
UPDATE mp_Sites SET RequiresUniqueEmail = 1;

ALTER TABLE mp_Sites ADD COLUMN PasswordFormat tinyint(1) unsigned;
UPDATE mp_Sites SET PasswordFormat = 0;

ALTER TABLE mp_Sites ADD COLUMN MinRequiredPasswordLength integer DEFAULT 4;
UPDATE mp_Sites SET MinRequiredPasswordLength = 4;

ALTER TABLE mp_Sites ADD COLUMN MinRequiredNonAlphanumericCharacters integer DEFAULT 0;
UPDATE mp_Sites SET MinRequiredNonAlphanumericCharacters = 0;

ALTER TABLE mp_Sites ADD COLUMN PasswordStrengthRegularExpression text;

ALTER TABLE mp_Sites ADD COLUMN DefaultEmailFromAddress varchar(100);
UPDATE mp_Sites SET DefaultEmailFromAddress = 'noreply@yoursite.com';

ALTER TABLE mp_Sites ADD COLUMN EnableMyPageFeature tinyint(1) unsigned DEFAULT 0;
UPDATE mp_Sites SET EnableMyPageFeature = 0;

-- need to do this with a .net utility and set to new guid
-- UPDATE mp_Users UserGuid = '00000000-0000-0000-0000-000000000000';

ALTER TABLE mp_Users ADD COLUMN LoweredEmail varchar(100);
ALTER TABLE mp_Users ADD COLUMN RegisterConfirmGuid varchar(36) DEFAULT '00000000-0000-0000-0000-000000000000';
UPDATE mp_Users SET RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000';

ALTER TABLE mp_Users ADD COLUMN PasswordQuestion varchar(255);
UPDATE mp_Users SET PasswordQuestion = 'what color is blue';

ALTER TABLE mp_Users ADD COLUMN PasswordAnswer varchar(255);
UPDATE mp_Users SET PasswordAnswer = 'blue';

ALTER TABLE mp_Users ADD COLUMN LastActivityDate datetime;
ALTER TABLE mp_Users ADD COLUMN LastLoginDate datetime;
ALTER TABLE mp_Users ADD COLUMN LastPasswordChangedDate datetime;
ALTER TABLE mp_Users ADD COLUMN LastLockoutDate datetime;

ALTER TABLE mp_Users ADD COLUMN FailedPasswordAttemptCount integer DEFAULT 0;
UPDATE mp_Users SET FailedPasswordAttemptCount = 0;

ALTER TABLE mp_Users ADD COLUMN FailedPasswordAttemptWindowStart datetime;

ALTER TABLE mp_Users ADD COLUMN FailedPasswordAnswerAttemptCount integer DEFAULT 0;
UPDATE mp_Users SET FailedPasswordAnswerAttemptCount = 0;

ALTER TABLE mp_Users ADD COLUMN FailedPasswordAnswerAttemptWindowStart datetime;

ALTER TABLE mp_Users ADD COLUMN IsLockedOut tinyint(1) unsigned;
UPDATE mp_Users SET IsLockedOut = 0;

ALTER TABLE mp_Users ADD COLUMN MobilePIN varchar(16);
ALTER TABLE mp_Users ADD COLUMN PasswordSalt varchar(128);
ALTER TABLE mp_Users ADD COLUMN Comment text;

ALTER TABLE mp_SharedFilesHistory ADD COLUMN OriginalFileName varchar(255);

ALTER TABLE mp_SharedFilesHistory ADD COLUMN UploadUserID INTEGER ZEROFILL NOT NULL DEFAULT 0;

ALTER TABLE mp_Pages ADD COLUMN IncludeInMenu tinyint(1) unsigned DEFAULT 1;
UPDATE mp_Pages SET IncludeInMenu = 1;

CREATE TABLE `mp_SitePaths` (
  `PathID` varchar(36) NOT NULL default '',
  `SiteID` int(11) NOT NULL default '0',
  `Path` varchar(255) NOT NULL default '',
  `LoweredPath` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`PathID`)
) ENGINE=MyISAM;

CREATE TABLE `mp_SitePersonalizationAllUsers` (
  `PathID` varchar(36) NOT NULL default '',
  `PageSettings` longblob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`PathID`)
) ENGINE=MyISAM;

CREATE TABLE `mp_SitePersonalizationPerUser` (
  `ID` varchar(36) NOT NULL default '',
  `PathID` varchar(36) NOT NULL default '',
  `UserID` varchar(36) NOT NULL default '',
  `PageSettings` longblob NOT NULL,
  `LastUpdate` datetime NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`ID`)
) ENGINE=MyISAM ;

CREATE TABLE `mp_UserProperties` (
  `UserID` int(11) NOT NULL default '0',
  `PropertyName` varchar(255) default NULL,
  `PropertyNames` text NOT NULL,
  `PropertyValuesString` text,
  `PropertyValuesBinary` longblob,
  `LastUpdateDate` datetime NOT NULL default '0000-00-00 00:00:00'
) ENGINE=MyISAM ;

CREATE TABLE `mp_PageModules` (                                                                                                                                                                                                                                                                                                                                                                            
  `PageID` int(11) NOT NULL default '0',                                                                                                                                                                                                                                                                                                                                                                   
  `ModuleID` int(11) NOT NULL default '0',                                                                                                                                                                                                                                                                                                                                                                 
  `PaneName` varchar(50) NOT NULL default '',                                                                                                                                                                                                                                                                                                                                                              
  `ModuleOrder` int(11) NOT NULL default '0',                                                                                                                                                                                                                                                                                                                                                              
  `PublishBeginDate` datetime NOT NULL default '0000-00-00 00:00:00',                                                                                                                                                                                                                                                                                                                                      
  `PublishEndDate` datetime default NULL,                                                                                                                                                                                                                                                                                                                                                                  
  PRIMARY KEY  (`PageID`,`ModuleID`)                                                                                                                                                                                                                                                                                                                                                                       
) ENGINE=MyISAM;

INSERT INTO mp_PageModules (PageID, ModuleID, PaneName, ModuleOrder, PublishBeginDate )

SELECT PageID, ModuleID, PaneName, ModuleOrder, now()
FROM mp_Modules;

ALTER TABLE mp_Modules ADD COLUMN SiteID INTEGER ZEROFILL NOT NULL DEFAULT 0;

ALTER TABLE mp_Modules ADD COLUMN AvailableForMyPage tinyint(1) unsigned DEFAULT 1;
UPDATE mp_Modules SET AvailableForMyPage = 0;

ALTER TABLE mp_Modules ADD COLUMN CreatedByUserID INTEGER ZEROFILL NOT NULL DEFAULT 0;
UPDATE mp_Modules SET CreatedByUserID = 1;

ALTER TABLE mp_Modules ADD COLUMN CreatedDate DateTime;
UPDATE mp_Modules SET CreatedDate = now();

UPDATE mp_Modules
SET SiteID = (
				SELECT SiteID 
				FROM mp_Pages
				WHERE mp_Pages.PageID = mp_Modules.PageID
			);
		


ALTER TABLE mp_Modules DROP COLUMN PageID ;
ALTER TABLE mp_Modules DROP COLUMN ModuleOrder ;
ALTER TABLE mp_Modules DROP COLUMN PaneName ;



-- added 6/4/2006

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
 PRIMARY KEY (`WebPartID`)    
) ENGINE=MyISAM ;

ALTER TABLE mp_ModuleDefinitions ADD COLUMN Icon varchar(255);

ALTER TABLE mp_Modules ADD COLUMN Icon varchar(255);

ALTER TABLE mp_Modules ADD COLUMN AllowMultipleInstancesOnMyPage tinyint(1) unsigned DEFAULT 1;
UPDATE mp_Modules SET AllowMultipleInstancesOnMyPage = 0;

ALTER TABLE mp_Modules ADD COLUMN CountOfUseOnMyPage INTEGER ZEROFILL NOT NULL DEFAULT 0;

ALTER TABLE mp_WebParts ADD COLUMN CountOfUseOnMyPage INTEGER ZEROFILL NOT NULL DEFAULT 0;

CREATE TABLE `mp_UserPages` (
 `UserPageID` varchar(36) NOT NULL, 
 `SiteID` int(11) NOT NULL,
 `UserGuid` varchar(36) NOT NULL,
 `PageName` varchar(255) NOT NULL,
 `PagePath` varchar(255) NOT NULL,
 `PageOrder` int(11) NOT NULL default '0',
 PRIMARY KEY (`UserPageID`)   
) ENGINE=MyISAM ;

ALTER TABLE `mp_Users` CHANGE `Password` `Password` VARCHAR( 128 )  DEFAULT NULL 