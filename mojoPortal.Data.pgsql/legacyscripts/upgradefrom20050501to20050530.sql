BEGIN;
LOCK mp_pages;
ALTER TABLE mp_pages RENAME authorizedroles to authorizedroles_old;
ALTER TABLE mp_pages ADD COLUMN authorizedroles text;
UPDATE mp_pages SET authorizedroles = authorizedroles_old;
ALTER TABLE mp_pages DROP COLUMN authorizedroles_old;
ALTER TABLE mp_pages ADD COLUMN editroles text;
ALTER TABLE mp_pages ADD COLUMN createchildpageroles text;
ALTER TABLE mp_pages ADD COLUMN showchildbreadcrumbs boolean;
UPDATE mp_pages SET showchildbreadcrumbs = false;
ALTER TABLE mp_pages ALTER COLUMN showchildbreadcrumbs SET DEFAULT false;
ALTER TABLE mp_pages ALTER COLUMN showchildbreadcrumbs SET NOT NULL;
ALTER TABLE mp_pages ADD COLUMN hidemainmenu boolean;
UPDATE mp_pages SET hidemainmenu = false;
ALTER TABLE mp_pages ALTER COLUMN hidemainmenu SET DEFAULT false;
ALTER TABLE mp_pages ALTER COLUMN hidemainmenu SET NOT NULL  ;
ALTER TABLE mp_pages ADD COLUMN skin character varying(100);
COMMIT;




BEGIN;
LOCK mp_modules;
ALTER TABLE mp_modules RENAME authorizededitroles to authorizededitroles_old;
ALTER TABLE mp_modules ADD COLUMN authorizededitroles text;
ALTER TABLE mp_modules ADD COLUMN edituserid integer;

UPDATE mp_modules SET authorizededitroles = authorizededitroles_old;
ALTER TABLE mp_modules DROP COLUMN authorizededitroles_old;
COMMIT;

BEGIN;

UPDATE mp_modules SET edituserid = 0;
ALTER TABLE mp_modules ALTER COLUMN edituserid SET DEFAULT 0;
ALTER TABLE mp_modules ALTER COLUMN edituserid SET NOT NULL;


COMMIT;


BEGIN;
LOCK mp_users;
ALTER TABLE mp_users ADD COLUMN skin character varying(100);
COMMIT;

BEGIN;
LOCK mp_sitemoduledefinitions;
ALTER TABLE mp_sitemoduledefinitions ADD COLUMN authorizedroles text;
COMMIT;



BEGIN;
LOCK mp_blogs;
ALTER TABLE mp_blogs ADD COLUMN includeinfeed boolean;
UPDATE mp_blogs SET includeinfeed = true;
ALTER TABLE mp_blogs ALTER COLUMN includeinfeed SET DEFAULT true;
ALTER TABLE mp_blogs ALTER COLUMN includeinfeed SET NOT NULL;
COMMIT;


BEGIN;
LOCK mp_roles;
ALTER TABLE mp_roles ADD COLUMN displayname character varying(50);
COMMIT;


BEGIN;
LOCK mp_sites;
ALTER TABLE mp_sites ADD COLUMN allowpageskins boolean;
UPDATE mp_sites SET allowpageskins = true;
ALTER TABLE mp_sites ALTER COLUMN allowpageskins SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN allowpageskins SET NOT NULL;
ALTER TABLE mp_sites ADD COLUMN allowhidemenuonpages boolean;
UPDATE mp_sites SET allowhidemenuonpages = true;
ALTER TABLE mp_sites ALTER COLUMN allowhidemenuonpages SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN allowhidemenuonpages SET NOT NULL;
COMMIT;


-- begin data

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (9,'BlogEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (9,'BlogEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (1,'HtmlEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$');

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (1,'HtmlEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$');



INSERT INTO mp_roles (siteid, rolename, displayname)
VALUES (1,'Content Administrators', 'Content Administrators');


UPDATE mp_roles
SET displayname = rolename ;




UPDATE mp_roles
SET rolename = 'Content Publishers',
displayname = 'Content Publishers'
WHERE rolename = 'Content Approvers' ;



INSERT INTO mp_roles (siteid, rolename, displayname)
SELECT siteid, 'Content Administrators', 'Content Administrators'
FROM mp_Sites ;


DROP function mp_sitesettings_getpagelist
(
	int --:siteid $1
);

DROP function mp_pages_selectlist
(
	int --:siteid $1
);

DROP function mp_pages_selectone
(
	int --:pageid $1
);

DROP function mp_pages_selectchildpages
(
	int --:parentid $1
);




    


