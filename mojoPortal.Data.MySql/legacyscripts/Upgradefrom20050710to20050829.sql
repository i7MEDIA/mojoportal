

CREATE TABLE `mp_LinkCategories` (
  `CategoryID` int(10) unsigned NOT NULL auto_increment,
  `ParentId` int(11) NOT NULL default '0',
  `Name` varchar(50) NOT NULL default '',
  PRIMARY KEY  (`CategoryID`),
  UNIQUE KEY `IX_linkcategories_name` (`Name`),
  KEY `IX_linkcategories_parentid` (`ParentId`)
) ENGINE=MyISAM;


#----------------------------
# Records for table mp_linkcategories
#----------------------------
insert  into mp_LinkCategories values (1, -1, 'Root');



CREATE TABLE `mp_Link_LinkCategories` (
  `RowID` int(11) NOT NULL auto_increment,
  `LinkID` int(11) NOT NULL default '0',
  `CategoryID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`RowID`),
  UNIQUE KEY `IX_link_linkcategories` (`LinkID`,`CategoryID`)
) ENGINE=MyISAM;



ALTER TABLE mp_Links ADD COLUMN Target VARCHAR(20);
ALTER TABLE mp_Links ALTER COLUMN Target  SET DEFAULT '_blank';



UPDATE mp_Links
SET Target = '_blank' 
WHERE Target IS NULL;



INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (2,'LinksShowDeleteIconSetting','false','CheckBox', NULL);

