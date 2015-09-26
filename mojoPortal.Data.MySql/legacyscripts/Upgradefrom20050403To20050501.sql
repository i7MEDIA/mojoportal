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
) TYPE=MyISAM;

/*
Table data for mojoportaldev.mp_ModuleDefinitions
*/



INSERT INTO `mp_ModuleDefinitions` (FeatureName, ControlSrc, SortOrder, IsAdmin)
VALUES ('Event Calendar','Modules/EventCalendar.ascx',500,0);

SET @NewModDefID = LAST_INSERT_ID();

INSERT INTO `mp_SiteModuleDefinitions` VALUES (1,@NewModDefID);



/*
Table data for mojoportaldev.mp_ModuleDefinitionSettings
*/

/*
--No special setting for this new module but below is the syntax example of how to add module definition settings

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedListLabelSetting','Feeds','TextBox',NULL);

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedFeedListCacheTimeoutSetting','240','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
IVALUES (@NewModDefID,'RSSFeedEntryCacheTimeoutSetting','120','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedFeedListColumnsSetting','1','TextBox','^[1-9][0-9]{0,2}$');

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedMaxDayCountSetting','90','TextBox','^[1-9][0-9]{0,3}$');

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedMaxPostsPerFeedSetting','20','TextBox','^[1-9][0-9]{0,3}$');


INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedShowAggregateFeedLink','true','CheckBox',NULL);

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedShowIndividualFeedLinks','true','CheckBox',NULL);

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedShowFeedListLeftSetting','false','CheckBox',NULL);


INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedShowFeedListRightSetting','true','CheckBox',NULL);

INSERT INTO `mp_ModuleDefinitionSettings` (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
VALUES (@NewModDefID,'RSSFeedUseFeedListAsFilterSetting','false','CheckBox',NULL);

*/

