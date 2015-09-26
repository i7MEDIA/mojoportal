

BEGIN;
LOCK mp_sites;
ALTER TABLE mp_sites ADD COLUMN ldapdomain character varying(255);

COMMIT;



