
DROP TABLE "mp_userproperties";

CREATE TABLE "mp_userproperties" (
	"propertyid" varchar(36) NOT NULL, 
    "userguid" varchar(36) NOT NULL,
    "propertyname" varchar(255) NULL,
    "propertyvaluestring" text NULL,
    "propertyvaluebinary" bytea NULL,
    "lastupdateddate" date NOT NULL,
    "islazyloaded" bool NOT NULL DEFAULT false
	);
	
CREATE UNIQUE INDEX "mp_userproperties_pkey"
  ON "mp_userproperties"("propertyid"); 


BEGIN;
LOCK mp_pages;
ALTER TABLE mp_pages ADD COLUMN pagetitle character varying(255);
ALTER TABLE mp_pages ADD COLUMN allowbrowsercache bool;
UPDATE mp_pages SET allowbrowsercache = true;
ALTER TABLE mp_pages ALTER COLUMN allowbrowsercache SET DEFAULT true;
COMMIT;

BEGIN;
LOCK mp_moduledefinitions;
ALTER TABLE mp_moduledefinitions ADD COLUMN defaultcachetime integer;
UPDATE mp_moduledefinitions SET defaultcachetime = 0;
ALTER TABLE mp_moduledefinitions ALTER COLUMN defaultcachetime SET DEFAULT 0;
ALTER TABLE mp_moduledefinitions ALTER COLUMN defaultcachetime SET NOT NULL;
COMMIT;

