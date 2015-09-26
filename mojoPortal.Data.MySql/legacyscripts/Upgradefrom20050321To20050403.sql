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
) TYPE=MyISAM;



/*
Table data for mojoportaldev.mp_ModuleDefinitions
*/



INSERT INTO `mp_ModuleDefinitions` (FeatureName, ControlSrc, SortOrder, IsAdmin)
VALUES ('RSS Feed Aggregator','Modules/RSSAggregator.ascx',500,0);

SET @RSSFeedModDefID = LAST_INSERT_ID();

INSERT INTO `mp_SiteModuleDefinitions` VALUES (1,@RSSFeedModDefID);

/*
Table data for mojoportaldev.mp_ModuleDefinitionSettings
*/


INSERT INTO `mp_ModuleDefinitionSettings` VALUES (25,@RSSFeedModDefID,'RSSFeedListLabelSetting','Feeds','TextBox',NULL);
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (26,@RSSFeedModDefID,'RSSFeedFeedListCacheTimeoutSetting','240','TextBox','^[1-9][0-9]{0,4}$');
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (27,@RSSFeedModDefID,'RSSFeedEntryCacheTimeoutSetting','120','TextBox','^[1-9][0-9]{0,4}$');
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (28,@RSSFeedModDefID,'RSSFeedFeedListColumnsSetting','1','TextBox','^[1-9][0-9]{0,2}$');
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (29,@RSSFeedModDefID,'RSSFeedMaxDayCountSetting','90','TextBox','^[1-9][0-9]{0,3}$');
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (30,@RSSFeedModDefID,'RSSFeedMaxPostsPerFeedSetting','20','TextBox','^[1-9][0-9]{0,3}$');
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (31,@RSSFeedModDefID,'RSSFeedShowAggregateFeedLink','true','CheckBox',NULL);
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (32,@RSSFeedModDefID,'RSSFeedShowIndividualFeedLinks','true','CheckBox',NULL);
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (33,@RSSFeedModDefID,'RSSFeedShowFeedListLeftSetting','false','CheckBox',NULL);
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (34,@RSSFeedModDefID,'RSSFeedShowFeedListRightSetting','true','CheckBox',NULL);
INSERT INTO `mp_ModuleDefinitionSettings` VALUES (35,@RSSFeedModDefID,'RSSFeedUseFeedListAsFilterSetting','false','CheckBox',NULL);

