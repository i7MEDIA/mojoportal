

ALTER TABLE mp_Pages ADD COLUMN AuthorizedRoles_old varchar(255);
UPDATE mp_Pages SET AuthorizedRoles_old = AuthorizedRoles;
ALTER TABLE mp_Pages DROP COLUMN AuthorizedRoles;
ALTER TABLE mp_Pages ADD COLUMN AuthorizedRoles text;
UPDATE mp_Pages SET AuthorizedRoles = AuthorizedRoles_old;
ALTER TABLE mp_Pages DROP COLUMN AuthorizedRoles_old;
ALTER TABLE mp_Pages ADD COLUMN EditRoles text;
ALTER TABLE mp_Pages ADD COLUMN CreateChildPageRoles text;
ALTER TABLE mp_Pages ADD COLUMN ShowChildBreadcrumbs tinyint(1) unsigned ;
UPDATE mp_Pages SET ShowChildBreadcrumbs =0 ;
ALTER TABLE mp_Pages ALTER COLUMN ShowChildBreadcrumbs SET DEFAULT 0;
ALTER TABLE mp_Pages ADD COLUMN HideMainMenu tinyint(1) unsigned;
UPDATE mp_Pages SET HideMainMenu = 0;
ALTER TABLE mp_Pages ALTER COLUMN HideMainMenu SET DEFAULT 0;
ALTER TABLE mp_Pages ADD COLUMN Skin varchar(100);

ALTER TABLE mp_Modules ADD COLUMN AuthorizedEditRoles_old varchar(255);
UPDATE mp_Modules SET AuthorizedEditRoles_old = AuthorizedEditRoles;
ALTER TABLE mp_Modules  DROP COLUMN AuthorizedEditRoles;
ALTER TABLE mp_Modules ADD COLUMN AuthorizedEditRoles text;
UPDATE mp_Modules SET AuthorizedEditRoles = AuthorizedEditRoles_old;
ALTER TABLE mp_Modules  DROP COLUMN AuthorizedEditRoles_old;
ALTER TABLE mp_Modules ADD COLUMN EditUserID integer DEFAULT 0;
UPDATE mp_Modules SET EditUserID = 0;


ALTER TABLE mp_Users ADD COLUMN Skin varchar(100);

ALTER TABLE mp_SiteModuleDefinitions ADD COLUMN AuthorizedRoles text;



ALTER TABLE mp_Blogs ADD COLUMN IncludeInFeed tinyint(1) unsigned DEFAULT 1;
UPDATE mp_Blogs SET IncludeInFeed = 1;

ALTER TABLE mp_Roles ADD COLUMN DisplayName varchar(50);


ALTER TABLE mp_Sites ADD COLUMN AllowPageSkins tinyint(1) unsigned;
UPDATE mp_Sites SET AllowPageSkins = 1;
ALTER TABLE mp_Sites ALTER COLUMN AllowPageSkins SET DEFAULT 1;

ALTER TABLE mp_Sites ADD COLUMN AllowHideMenuOnPages tinyint(1) unsigned;
UPDATE mp_Sites SET AllowHideMenuOnPages = 1;
ALTER TABLE mp_Sites ALTER COLUMN AllowHideMenuOnPages SET DEFAULT 1;







INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (1,'HtmlEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (1,'HtmlEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_Roles (SiteID, RoleName, DisplayName)
VALUES (1,'Content Administrators', 'Content Administrators');


UPDATE mp_Roles
SET DisplayName = RoleName ;




UPDATE mp_Roles
SET RoleName = 'Content Publishers',
DisplayName = 'Content Publishers'
WHERE RoleName = 'Content Approvers' ;



INSERT INTO mp_Roles (SiteID, RoleName, DisplayName)
SELECT SiteID, 'Content Administrators', 'Content Administrators'
FROM mp_Sites ;

UPDATE mp_ModuleSettings

