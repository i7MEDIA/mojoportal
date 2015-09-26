

BEGIN;
LOCK mp_sites;
ALTER TABLE mp_sites ADD COLUMN siteguid character varying(36);
UPDATE mp_sites SET siteguid = '00000000-0000-0000-0000-000000000000';

ALTER TABLE mp_sites ADD COLUMN allowpasswordretrieval boolean;
UPDATE mp_sites SET allowpasswordretrieval = true;
ALTER TABLE mp_sites ALTER COLUMN allowpasswordretrieval SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN allowpasswordretrieval SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN allowpasswordreset boolean;
UPDATE mp_sites SET allowpasswordreset = true;
ALTER TABLE mp_sites ALTER COLUMN allowpasswordreset SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN allowpasswordreset SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN requiresquestionandanswer boolean;
UPDATE mp_sites SET requiresquestionandanswer = true;
ALTER TABLE mp_sites ALTER COLUMN requiresquestionandanswer SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN requiresquestionandanswer SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN requiresuniqueemail boolean;
UPDATE mp_sites SET requiresuniqueemail = true;
ALTER TABLE mp_sites ALTER COLUMN requiresuniqueemail SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN requiresuniqueemail SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN maxinvalidpasswordattempts integer;
UPDATE mp_sites SET maxinvalidpasswordattempts = 5;
ALTER TABLE mp_sites ALTER COLUMN maxinvalidpasswordattempts SET DEFAULT 5;
ALTER TABLE mp_sites ALTER COLUMN maxinvalidpasswordattempts SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN passwordattemptwindowminutes integer;
UPDATE mp_sites SET passwordattemptwindowminutes = 5;
ALTER TABLE mp_sites ALTER COLUMN passwordattemptwindowminutes SET DEFAULT 5;
ALTER TABLE mp_sites ALTER COLUMN passwordattemptwindowminutes SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN passwordformat integer;
UPDATE mp_sites SET passwordformat = 0;
ALTER TABLE mp_sites ALTER COLUMN passwordformat SET DEFAULT 0;
ALTER TABLE mp_sites ALTER COLUMN passwordformat SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN minrequiredpasswordlength integer;
UPDATE mp_sites SET minrequiredpasswordlength = 4;
ALTER TABLE mp_sites ALTER COLUMN minrequiredpasswordlength SET DEFAULT 4;
ALTER TABLE mp_sites ALTER COLUMN minrequiredpasswordlength SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN minrequirednonalphanumericcharacters integer;
UPDATE mp_sites SET minrequirednonalphanumericcharacters = 0;
ALTER TABLE mp_sites ALTER COLUMN minrequirednonalphanumericcharacters SET DEFAULT 0;
ALTER TABLE mp_sites ALTER COLUMN minrequirednonalphanumericcharacters SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN passwordstrengthregularexpression text;

ALTER TABLE mp_sites ADD COLUMN defaultemailfromaddress character varying(100);
UPDATE mp_sites SET defaultemailfromaddress = 'noreply@yoursite.com';

ALTER TABLE mp_sites ADD COLUMN enablemypagefeature boolean;
UPDATE mp_sites SET enablemypagefeature = false;
ALTER TABLE mp_sites ALTER COLUMN enablemypagefeature SET DEFAULT false;
ALTER TABLE mp_sites ALTER COLUMN enablemypagefeature SET NOT NULL;


COMMIT;

--

BEGIN;

LOCK mp_users;

ALTER TABLE mp_users ADD COLUMN loweredemail character varying(100);

ALTER TABLE mp_users ADD COLUMN passwordquestion character varying(255);
UPDATE mp_users SET passwordquestion = 'what color is blue';

ALTER TABLE mp_users ADD COLUMN passwordanswer character varying(255);
UPDATE mp_users SET passwordanswer = 'blue';

ALTER TABLE mp_users ADD COLUMN registerconfirmguid character varying(36);
UPDATE mp_users SET registerconfirmguid = '00000000-0000-0000-0000-000000000000';
ALTER TABLE mp_users ALTER COLUMN registerconfirmguid SET DEFAULT '00000000-0000-0000-0000-000000000000';


ALTER TABLE mp_users ADD COLUMN lastactivitydate timestamp without time zone;
ALTER TABLE mp_users ADD COLUMN lastlogindate timestamp without time zone;
ALTER TABLE mp_users ADD COLUMN lastpasswordchangeddate timestamp without time zone;
ALTER TABLE mp_users ADD COLUMN lastlockoutdate timestamp without time zone;
ALTER TABLE mp_users ADD COLUMN failedpasswordattemptwindowstart timestamp without time zone;
ALTER TABLE mp_users ADD COLUMN failedpasswordanswerattemptwindowstart timestamp without time zone;

ALTER TABLE mp_users ADD COLUMN islockedout boolean;
UPDATE mp_users SET islockedout = false;
ALTER TABLE mp_users ALTER COLUMN islockedout SET DEFAULT false;
ALTER TABLE mp_users ALTER COLUMN islockedout SET NOT NULL;

ALTER TABLE mp_users ADD COLUMN failedpasswordattemptcount integer;
UPDATE mp_users SET failedpasswordattemptcount = 0;
ALTER TABLE mp_users ALTER COLUMN failedpasswordattemptcount SET DEFAULT 0;
ALTER TABLE mp_users ALTER COLUMN failedpasswordattemptcount SET NOT NULL;

ALTER TABLE mp_users ADD COLUMN failedpasswordanswerattemptcount integer;
UPDATE mp_users SET failedpasswordanswerattemptcount = 0;
ALTER TABLE mp_users ALTER COLUMN failedpasswordanswerattemptcount SET DEFAULT 0;
ALTER TABLE mp_users ALTER COLUMN failedpasswordanswerattemptcount SET NOT NULL;

ALTER TABLE mp_users ADD COLUMN mobilepin character varying(16);
ALTER TABLE mp_users ADD COLUMN passwordsalt character varying(128);
ALTER TABLE mp_users ADD COLUMN comment text;



COMMIT;

CREATE TABLE mp_sitepaths (
    pathid character varying(36) NOT NULL,
    siteid integer NOT NULL,
    path character varying(255) NOT NULL,
    loweredpath character varying(255) NOT NULL
);

CREATE UNIQUE INDEX mp_sitepaths_idxpthid_idx ON mp_sitepaths USING btree (pathid);

CREATE TABLE mp_sitepersonalizationallusers (
    pathid character varying(36) NOT NULL,
    pagesettings bytea NOT NULL,
    lastupdate timestamp without time zone NOT NULL
);

CREATE UNIQUE INDEX mp_sitepersonalizationallusers_idxpthid_idx ON mp_sitepersonalizationallusers USING btree (pathid);



CREATE TABLE mp_sitepersonalizationperuser (
    id character varying(36) NOT NULL,
    pathid character varying(36) NOT NULL,
    userid character varying(36) NOT NULL,
    pagesettings bytea NOT NULL,
    lastupdate timestamp without time zone NOT NULL
);

CREATE UNIQUE INDEX mp_sitepersonalizationperuser_idxid_idx ON mp_sitepersonalizationperuser USING btree (id);

CREATE TABLE mp_userproperties (
    userid integer NOT NULL,
    propertyname character varying(255) NULL,
    propertynames text NOT NULL,
    propertyvaluesstring text,
    propertyvaluesbinary bytea,
    lastupdatedate timestamp without time zone 
);


CREATE TABLE mp_pagemodules (
    pageid integer NOT NULL,
    moduleid integer NOT NULL,
    panename character varying(50) NULL,
    moduleorder integer NOT NULL,
    publishbegindate timestamp without time zone NOT NULL,
    publishenddate timestamp without time zone 
);

CREATE UNIQUE INDEX mp_pagemodules_pkey
	ON public.mp_pagemodules(pageid, moduleid);
	
INSERT INTO mp_pagemodules (pageid, moduleid, panename, moduleorder, publishbegindate )

SELECT pageid, moduleid, panename, moduleorder, current_timestamp(3)
FROM mp_modules;



BEGIN;

LOCK mp_modules;

ALTER TABLE mp_modules ADD COLUMN siteid integer;
UPDATE mp_modules 

SET siteid = (
				SELECT siteid 
				FROM mp_pages
				WHERE mp_pages.pageid = mp_modules.pageid
			);


ALTER TABLE mp_modules ALTER COLUMN siteid SET DEFAULT 0;
ALTER TABLE mp_modules ALTER COLUMN siteid SET NOT NULL;

ALTER TABLE mp_modules ADD COLUMN availableformypage boolean;
UPDATE mp_modules SET availableformypage = false;
ALTER TABLE mp_modules ALTER COLUMN availableformypage SET DEFAULT false;
ALTER TABLE mp_modules ALTER COLUMN availableformypage SET NOT NULL;

ALTER TABLE mp_modules ADD COLUMN createdbyuserid integer;
UPDATE mp_modules SET createdbyuserid = 1;

ALTER TABLE mp_modules ADD COLUMN createddate timestamp without time zone;
UPDATE mp_modules SET createddate = current_timestamp(3);

ALTER TABLE mp_modules DROP COLUMN pageid ;
ALTER TABLE mp_modules DROP COLUMN moduleorder ;
ALTER TABLE mp_modules DROP COLUMN panename ;


COMMIT;

BEGIN;
LOCK mp_pages;

ALTER TABLE mp_pages ADD COLUMN includeinmenu boolean;
UPDATE mp_pages SET includeinmenu = true;
ALTER TABLE mp_pages ALTER COLUMN includeinmenu SET DEFAULT true;
ALTER TABLE mp_pages ALTER COLUMN includeinmenu SET NOT NULL;

COMMIT;

-- added 6/3/2006

CREATE TABLE "mp_webparts" (
    "webpartid" varchar(36) NOT NULL, 
    "siteid" int4 NOT NULL,
    "title" varchar(255) NOT NULL,
    "description" varchar(255) NOT NULL,
    "imageurl" varchar(255) NULL,
    "classname" varchar(255) NOT NULL,
    "assemblyname" varchar(255) NOT NULL,
    "availableformypage" bool NOT NULL DEFAULT false,
    "allowmultipleinstancesonmypage" bool NOT NULL DEFAULT true,
    "availableforcontentsystem" bool NOT NULL DEFAULT false
);
	
CREATE UNIQUE INDEX "mp_webparts_pkey"
  ON "mp_webparts"("webpartid");   
  
ALTER TABLE mp_moduledefinitions ADD COLUMN icon character varying(255);

ALTER TABLE mp_modules ADD COLUMN icon character varying(255);

ALTER TABLE mp_modules ADD COLUMN allowmultipleinstancesonmypage boolean;
UPDATE mp_modules SET allowmultipleinstancesonmypage = true;
ALTER TABLE mp_modules ALTER COLUMN allowmultipleinstancesonmypage SET DEFAULT true;
ALTER TABLE mp_modules ALTER COLUMN allowmultipleinstancesonmypage SET NOT NULL;

-- added 6/18/2006

ALTER TABLE mp_modules ADD COLUMN countofuseonmypage integer;
UPDATE mp_modules SET countofuseonmypage = 0;
ALTER TABLE mp_modules ALTER COLUMN countofuseonmypage SET DEFAULT 0;
ALTER TABLE mp_modules ALTER COLUMN countofuseonmypage SET NOT NULL;

CREATE TABLE "mp_userpages" (
  	"userpageid" varchar(36) NOT NULL, 
      "siteid" int4 NOT NULL,
      "userguid" varchar(36) NOT NULL,
      "pagename" varchar(255) NOT NULL,
      "pagepath" varchar(255) NOT NULL,
      "pageorder" int4 NOT NULL
  	);
  	
  CREATE UNIQUE INDEX "mp_userpages_pkey"
  ON "mp_userpages"("userpageid"); 
  
  
ALTER TABLE mp_webparts ADD COLUMN countofuseonmypage integer;
UPDATE mp_webparts SET countofuseonmypage = 0;
ALTER TABLE mp_webparts ALTER COLUMN countofuseonmypage SET DEFAULT 0;
ALTER TABLE mp_webparts ALTER COLUMN countofuseonmypage SET NOT NULL;


ALTER TABLE "mp_users" ALTER COLUMN "password" TYPE varchar(128);

/*



*/