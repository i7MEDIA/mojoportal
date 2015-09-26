BEGIN;
LOCK mp_links;
ALTER TABLE mp_links ADD COLUMN target character varying(20);

UPDATE mp_links SET target = '_blank';


ALTER TABLE mp_links ALTER COLUMN target SET DEFAULT '_blank';
ALTER TABLE mp_links ALTER COLUMN target SET NOT NULL;


COMMIT;


INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (2,'LinksShowDeleteIconSetting','false','CheckBox',NULL);