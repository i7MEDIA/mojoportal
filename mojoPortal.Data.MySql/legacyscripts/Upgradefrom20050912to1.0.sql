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
) TYPE=MyISAM;


ALTER TABLE mp_Sites ADD COLUMN DefaultFriendlyUrlPatternenum VARCHAR(50);
UPDATE mp_Sites SET DefaultFriendlyUrlPatternenum = 'PageNameWithDotASPX';
ALTER TABLE mp_Sites ALTER COLUMN DefaultFriendlyUrlPatternenum SET DEFAULT 'PageNameWithDotASPX';


ALTER TABLE mp_Sites ADD COLUMN EditorSkin VARCHAR(50);
UPDATE mp_Sites SET EditorSkin = 'normal';
ALTER TABLE mp_Sites ALTER COLUMN EditorSkin SET DEFAULT 'normal';

ALTER TABLE mp_Sites ADD COLUMN AllowUserFullNameChange tinyint(1) unsigned;
UPDATE mp_Sites SET AllowUserFullNameChange = 0;
ALTER TABLE mp_Sites ALTER COLUMN AllowUserFullNameChange SET DEFAULT 0;

ALTER TABLE mp_Sites ADD COLUMN UseEmailForLogin tinyint(1) unsigned;
UPDATE mp_Sites SET UseEmailForLogin = 1;
ALTER TABLE mp_Sites ALTER COLUMN UseEmailForLogin SET DEFAULT 1;

ALTER TABLE mp_Sites ADD COLUMN ReallyDeleteUsers tinyint(1) unsigned;
UPDATE mp_Sites SET ReallyDeleteUsers = 1;
ALTER TABLE mp_Sites ALTER COLUMN ReallyDeleteUsers SET DEFAULT 1;


ALTER TABLE mp_Sites ADD COLUMN UseLdapAuth tinyint(1) unsigned;
UPDATE mp_Sites SET UseLdapAuth = 0;
ALTER TABLE mp_Sites ALTER COLUMN UseLdapAuth SET DEFAULT 0;

ALTER TABLE mp_Sites ADD COLUMN AutoCreateLdapUserOnFirstLogin tinyint(1) unsigned;
UPDATE mp_Sites SET AutoCreateLdapUserOnFirstLogin = 1;
ALTER TABLE mp_Sites ALTER COLUMN AutoCreateLdapUserOnFirstLogin SET DEFAULT 1;

ALTER TABLE mp_Sites ADD COLUMN LdapServer VARCHAR(255);
ALTER TABLE mp_Sites ADD COLUMN LdapPort int;
UPDATE mp_Sites SET LdapPort = 389;
ALTER TABLE mp_Sites ALTER COLUMN LdapPort SET DEFAULT 389;
ALTER TABLE mp_Sites ADD COLUMN LdapRootDN VARCHAR(255);


ALTER TABLE mp_Users ADD COLUMN LoginName VARCHAR(50);
ALTER TABLE mp_Users ADD COLUMN IsDeleted tinyint(1) unsigned;
UPDATE mp_Users SET IsDeleted = 0, LoginName = name;
ALTER TABLE mp_Users ALTER COLUMN isdeleted SET DEFAULT 0;


INSERT INTO `mp_ModuleDefinitions` (FeatureName, ControlSrc, SortOrder, IsAdmin)
VALUES ('Html Fragment Include','Modules/HtmlFragmentInclude.ascx',500,0);