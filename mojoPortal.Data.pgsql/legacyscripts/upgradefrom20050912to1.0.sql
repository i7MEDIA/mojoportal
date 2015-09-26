--
-- Name: mp_forumsubscriptions_subscriptionid_seq; Type: SEQUENCE; Schema: public; Owner: mojo
--

CREATE SEQUENCE mp_forumsubscriptions_subscriptionid_seq
    START 1
    INCREMENT 1
    MAXVALUE 9223372036854775807
    MINVALUE 1
    CACHE 1;


--
-- Name: mp_forumsubscriptions; Type: TABLE; Schema: public; Owner: mojo
--

CREATE TABLE mp_forumsubscriptions (
    subscriptionid integer DEFAULT nextval('"mp_forumsubscriptions_subscriptionid_seq"'::text) NOT NULL,
    forumid integer NOT NULL,
    userid integer NOT NULL,
    subscribedate timestamp without time zone DEFAULT ('now'::text)::timestamp(3) with time zone NOT NULL,
    unsubscribedate timestamp without time zone
);


ALTER TABLE ONLY mp_forumsubscriptions
    ADD CONSTRAINT mp_forumsubscriptions_pkey PRIMARY KEY (subscriptionid);



-- added 10/22/2005
    
BEGIN;
LOCK mp_sites;


ALTER TABLE mp_sites ADD COLUMN defaultfriendlyurlpatternenum character varying(50);
UPDATE mp_sites SET defaultfriendlyurlpatternenum = 'PageNameWithDotASPX';
ALTER TABLE mp_sites ALTER COLUMN defaultfriendlyurlpatternenum SET DEFAULT 'PageNameWithDotASPX';
ALTER TABLE mp_sites ALTER COLUMN defaultfriendlyurlpatternenum SET NOT NULL;


ALTER TABLE mp_sites ADD COLUMN editorskin character varying(50);
UPDATE mp_sites SET editorskin = 'normal';
ALTER TABLE mp_sites ALTER COLUMN editorskin SET DEFAULT 'normal';
ALTER TABLE mp_sites ALTER COLUMN editorskin SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN allowuserfullnamechange boolean;
UPDATE mp_sites SET allowuserfullnamechange = false;
ALTER TABLE mp_sites ALTER COLUMN allowuserfullnamechange SET DEFAULT false;
ALTER TABLE mp_sites ALTER COLUMN allowuserfullnamechange SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN useemailforlogin boolean;
UPDATE mp_sites SET useemailforlogin = true;
ALTER TABLE mp_sites ALTER COLUMN useemailforlogin SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN useemailforlogin SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN reallydeleteusers boolean;
UPDATE mp_sites SET reallydeleteusers = true;
ALTER TABLE mp_sites ALTER COLUMN reallydeleteusers SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN reallydeleteusers SET NOT NULL;


ALTER TABLE mp_sites ADD COLUMN useldapauth boolean;
UPDATE mp_sites SET useldapauth = false;
ALTER TABLE mp_sites ALTER COLUMN useldapauth SET DEFAULT false;
ALTER TABLE mp_sites ALTER COLUMN useldapauth SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN autocreateldapuseronfirstlogin boolean;
UPDATE mp_sites SET autocreateldapuseronfirstlogin = true;
ALTER TABLE mp_sites ALTER COLUMN autocreateldapuseronfirstlogin SET DEFAULT true;
ALTER TABLE mp_sites ALTER COLUMN autocreateldapuseronfirstlogin SET NOT NULL;

ALTER TABLE mp_sites ADD COLUMN ldapserver character varying(255);
ALTER TABLE mp_sites ADD COLUMN ldapport integer;
UPDATE mp_sites SET ldapport = 389;
ALTER TABLE mp_sites ALTER COLUMN ldapport SET DEFAULT 389;
ALTER TABLE mp_sites ALTER COLUMN ldapport SET NOT NULL;
ALTER TABLE mp_sites ADD COLUMN ldaprootdn character varying(255);



COMMIT;




BEGIN;
LOCK mp_users;

ALTER TABLE mp_users ADD COLUMN loginname character varying(50);

ALTER TABLE mp_users ADD COLUMN isdeleted boolean;
UPDATE mp_users SET isdeleted = false, loginname = name;
ALTER TABLE mp_users ALTER COLUMN isdeleted SET DEFAULT false;
ALTER TABLE mp_users ALTER COLUMN isdeleted SET NOT NULL;

COMMIT;

/*
DECLARE _moduledefid int;
INSERT INTO mp_moduledefinitions (featurename, controlsrc, sortorder, isadmin)

VALUES ('Html Fragment Include','Modules/HtmlFragmentInclude.ascx',500,false);

_moduledefid := cast(currval(''mp_moduledefinitions_moduledefid_seq'') as int4);

insert into mp_sitemoduledefinitions (siteid, moduledefid)
values (1,_moduledefid);

*/