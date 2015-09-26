

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
) TYPE=MyISAM;


INSERT INTO `mp_ModuleDefinitions` (FeatureName,ControlSrc,SortOrder,IsAdmin)
VALUES ('Friendly Url Manager','Admin/FriendlyUrlManager.ascx',700,0);


INSERT INTO `mp_SiteModuleDefinitions` (SiteID,ModuleDefID)

SELECT		1, ModuleDefID
FROM		mp_ModuleDefinitions
WHERE		FeatureName = 'Friendly Url Manager';






INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType)
 
VALUES (9,'BlogShowCalendarSetting','false','CheckBox');

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogShowArchiveSetting','true','CheckBox',NULL);

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogShowCategoriesSetting','true','CheckBox',Null);

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogNavigationOnRightSetting','true','CheckBox',Null);

CREATE TABLE `mp_BlogCategories` (
  `CategoryID` int(11) NOT NULL auto_increment,
  `ModuleID` int(11) NOT NULL default '0',
  `Category` varchar(255) NOT NULL default '',
  `PostCount` int(11) NOT NULL default '0',
  PRIMARY KEY  (`CategoryID`)
) TYPE=MyISAM;


CREATE TABLE `mp_BlogItemCategories` (
  `ID` int(11) NOT NULL auto_increment,
  `ItemID` int(11) NOT NULL default '0',
  `CategoryID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ID`)
) TYPE=MyISAM;

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogMonthsToShow';

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogCategoriesToShow';

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogEditorWidthSetting';

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'HtmlEditorWidthSetting';




DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogMonthsToShow';

DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogCategoriesToShow';

DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogEditorWidthSetting';

DELETE FROM mp_ModuleSettings WHERE SettingName = 'HtmlEditorWidthSetting';



