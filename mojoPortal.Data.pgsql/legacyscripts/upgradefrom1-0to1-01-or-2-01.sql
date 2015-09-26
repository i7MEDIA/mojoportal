
    
BEGIN;
LOCK mp_sites;


ALTER TABLE mp_sites ADD COLUMN ldapuserdnkey character varying(10);
UPDATE mp_sites SET ldapuserdnkey = 'uid';
ALTER TABLE mp_sites ALTER COLUMN ldapuserdnkey SET DEFAULT 'uid';
ALTER TABLE mp_sites ALTER COLUMN ldapuserdnkey SET NOT NULL;


COMMIT;

--- There are 2 rows with the same id in mp_moduledefinitionsettings.  That will cause a problem
--- when we try to make the id column a primary key.  So, we move on of the rows to a new id.  We could
--- probably safely delete it but this way we can get it back easily if we need it.  For details, see:
--- http://www.mojoportal.com/ForumThreadView.aspx?thread=1373&mid=34&pageid=5&ItemID=2
UPDATE mp_moduledefinitionsettings SET id = 0 WHERE id = 16 AND settingname = 'GalleryCompactModeSetting';


/*
DECLARE _moduledefid int;
INSERT INTO mp_moduledefinitions (featurename, controlsrc, sortorder, isadmin)

VALUES ('Html Fragment Include','Modules/HtmlFragmentInclude.ascx',500,false);

_moduledefid := cast(currval(''mp_moduledefinitions_moduledefid_seq'') as int4);

insert into mp_sitemoduledefinitions (siteid, moduledefid)
values (1,_moduledefid);

*/
