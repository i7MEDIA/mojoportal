

CREATE SEQUENCE mp_friendlyurls_urlid_seq
    START 2
    INCREMENT 1
    MAXVALUE 9223372036854775807
    MINVALUE 1
    CACHE 1;



CREATE TABLE mp_friendlyurls (
    urlid integer DEFAULT nextval('"mp_friendlyurls_urlid_seq"'::text) NOT NULL,
    siteid integer NOT NULL,
    friendlyurl character varying(255),
    realurl character varying(255),
    ispattern boolean
);


CREATE UNIQUE INDEX mp_friendlyurls_idxurlid_idx ON mp_friendlyurls USING btree (urlid);

CREATE INDEX mp_friendlyurls_idxfriendlyurl_idx ON mp_friendlyurls USING btree (friendlyurl);

CREATE INDEX mp_users_idxname_idx ON mp_users USING btree (name);
CREATE INDEX mp_users_idxemail_idx ON mp_users USING btree (email);

ALTER TABLE ONLY mp_friendlyurls
    ADD CONSTRAINT mp_friendlyurls_pkey PRIMARY KEY (urlid);
    



INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype)
 
VALUES (9,'BlogShowCalendarSetting','false','CheckBox');

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (9,'BlogShowArchiveSetting','true','CheckBox', NULL);

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (9,'BlogShowCategoriesSetting','true','CheckBox',NULL);

INSERT INTO mp_moduledefinitionsettings (moduledefid, settingname, settingvalue, controltype, regexvalidationexpression)
 
VALUES (9,'BlogNavigationOnRightSetting','true','CheckBox',NULL);




DELETE FROM mp_moduledefinitionsettings WHERE settingname = 'BlogMonthsToShow';

DELETE FROM mp_moduledefinitionsettings WHERE settingname = 'BlogCategoriesToShow';

DELETE FROM mp_moduledefinitionsettings WHERE settingname = 'BlogEditorWidthSetting';

DELETE FROM mp_moduledefinitionsettings WHERE settingname = 'HtmlEditorWidthSetting';




DELETE FROM mp_modulesettings WHERE settingname = 'BlogMonthsToShow';

DELETE FROM mp_modulesettings WHERE settingname = 'BlogCategoriesToShow';

DELETE FROM mp_modulesettings WHERE settingname = 'BlogEditorWidthSetting';

DELETE FROM mp_modulesettings WHERE settingname = 'HtmlEditorWidthSetting';






CREATE SEQUENCE mp_blogcategories_categoryid_seq
    START 2
    INCREMENT 1
    MAXVALUE 9223372036854775807
    MINVALUE 1
    CACHE 1;



CREATE TABLE mp_blogcategories (
    categoryid integer DEFAULT nextval('"mp_blogcategories_categoryid_seq"'::text) NOT NULL,
    moduleid integer NOT NULL,
    category character varying(255)
);


CREATE SEQUENCE mp_blogitemcategories_id_seq
    START 2
    INCREMENT 1
    MAXVALUE 9223372036854775807
    MINVALUE 1
    CACHE 1;



CREATE TABLE mp_blogitemcategories (
    id integer DEFAULT nextval('"mp_blogitemcategories_id_seq"'::text) NOT NULL,
    itemid integer NOT NULL,
    categoryid integer NOT NULL
    
);





