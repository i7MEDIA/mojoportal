
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

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowStatisticsSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc = _latin1 'Modules/BlogModule.ascx';

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowFeedLinksSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc = _latin1 'Modules/BlogModule.ascx';

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowAddFeedLinksSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc =  _latin1 'Modules/BlogModule.ascx';


