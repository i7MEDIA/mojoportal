

create or replace function public.drop_type
(
	varchar(100) --: typename
) returns int4 
as '
declare
	_typename alias for $1;
	_rowcount int4;

begin

_rowcount := 0;
perform 1 from pg_class where
	  relname = _typename limit 1;
	
if found then
	EXECUTE ''DROP TYPE '' || _typename || '' CASCADE;'';
	GET DIAGNOSTICS _rowcount = ROW_COUNT;
end if;
return _rowcount; 
end'
security definer language plpgsql;

CREATE OR REPLACE FUNCTION public.monthname(timestamptz)
  RETURNS varchar(10) AS
'
declare
	_date alias for $1;
	_month int;
	_monthname varchar(10);
begin
    _month := date_part(''month'', _date);
    _monthname := ''January'';
    if _month = 2 then
        _monthname := ''February'';
    end if;
    if _month = 3 then
        _monthname := ''March'';
    end if;
    if _month = 4 then
        _monthname := ''April'';
    end if;
    if _month = 5 then
        _monthname := ''May'';
    end if;
    if _month = 6 then
        _monthname := ''June'';
    end if;
    if _month = 7 then
        _monthname := ''July'';
    end if;
    if _month = 8 then
        _monthname := ''August'';
    end if;
    if _month = 9 then
        _monthname := ''September'';
    end if;
    if _month = 10 then
        _monthname := ''October'';
    end if;
    if _month = 11 then
        _monthname := ''November'';
    end if;
    if _month = 12 then
        _monthname := ''December'';
    end if;
    return _monthname;    
end;'
  LANGUAGE 'plpgsql' VOLATILE SECURITY DEFINER;



--  4/17/2005 add Calendar Events 

CREATE SEQUENCE mp_calendarevents_itemid_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


CREATE TABLE mp_calendarevents (
    itemid integer DEFAULT nextval('"mp_calendarevents_itemid_seq"'::text) NOT NULL,
    moduleid integer NOT NULL,
    title character varying(255),
    description text,
    imagename character varying(100),
    eventdate timestamp without time zone,
    starttime timestamp without time zone,
    endtime timestamp without time zone,
    createddate timestamp without time zone,
    userid integer DEFAULT 0 NOT NULL
);

CREATE UNIQUE INDEX mp_calendarevents_idxitemid_idx ON mp_calendarevents USING btree (itemid);

ALTER TABLE ONLY mp_calendarevents
    ADD CONSTRAINT mp_calendarevents_pkey PRIMARY KEY (itemid);

ALTER TABLE ONLY mp_calendarevents
    ADD CONSTRAINT fk_calendarevents_modules_fk FOREIGN KEY (moduleid) REFERENCES mp_modules(moduleid) ON DELETE CASCADE;


select setval('mp_calendarevents_itemid_seq', (select max(itemid) from mp_calendarevents))
	where(select max(itemid) from mp_calendarevents) > 0;




-- Create Stored Procedures

create or replace function mp_calendarevents_delete 
(
	int --:itemid $1
) returns int4 
as '
/*
Author:   			
Created: 			4/17/2005
Last Modified: 			4/17/2005
*/	
	
	delete from mp_calendarevents
	where itemid = $1;
	
	select 1;'
security definer language sql;
grant execute on function mp_calendarevents_delete (
	int --:itemid $1
) to public;


select drop_type('mp_calendarevents_select_one_type');
CREATE TYPE public.mp_calendarevents_select_one_type as (
	itemid int4,
	moduleid int4,
	title varchar(255),
	description text,
	imagename varchar(100),
	eventdate timestamp,
	starttime timestamp,
	endtime timestamp,
	createddate timestamp,
	userid int4
);


select drop_type('mp_calendarevents_select_bydate_type');
CREATE TYPE public.mp_calendarevents_select_bydate_type as (
	itemid int4,
	moduleid int4,
	title varchar(255),
	description text,
	imagename varchar(100),
	eventdate timestamp,
	starttime timestamp,
	endtime timestamp,
	createddate timestamp,
	userid int4
);


create or replace function mp_calendarevents_select_one (
	int --:itemid $1
	
) returns setof mp_calendarevents_select_one_type
as '
/*
Author:   			
Created: 			4/17/2005
Last Modified: 			4/17/2005
*/

select
        itemid,
        moduleid,
        title,
        description,
        imagename,
        eventdate,
        starttime,
        endtime,
        createddate,
        userid
from
        mp_calendarevents
        
where
        itemid = $1;'
security definer language sql;
grant execute on function mp_calendarevents_select_one (
	int --:itemid $1
	
) to public;


create or replace function mp_calendarevents_select_bydate(
	int, --:moduleid $1
	varchar(20), --:begindate $2
	varchar(20)  --:enddate $3
) returns setof mp_calendarevents_select_bydate_type
as '
/*
Author:   			
Created: 			4/17/2005
Last Modified: 			4/17/2005
*/

select
        itemid,
        moduleid,
        title,
        description,
        imagename,
        eventdate,
        starttime,
        endtime,
        createddate,
        userid
from
        mp_calendarevents
        
where   moduleid = $1
	AND ($2 = '''' OR eventdate >= $2)
        AND ($3 = '''' OR eventdate <= $3)

order by eventdate, starttime;'
		
		
security definer language sql;
grant execute on function mp_calendarevents_select_bydate(
	int, --:moduleid $1
	varchar(20), --:begindate $2
	varchar(20)  --:enddate $3
) to public;


create or replace function mp_calendarevents_insert(
	int, --:moduleid $1
	varchar(255), --:title $2
	text, --:description $3
	varchar(100), --:imagename $4
	timestamp, --:eventdate $5
	timestamp, --:starttime $6
	timestamp, --:endtime $7
	int --:userid $8
) returns int4
as '
/*
Author:   			
Created: 			4/17/2005
Last Modified: 			4/17/2005

*/

insert into 	mp_calendarevents
(				
                moduleid,
                title,
                description,
                imagename,
                eventdate,
                starttime,
                endtime,
                createddate,
                userid
) 
values 
(				
                $1, --:moduleid
                $2, --:title
                $3, --:description
                $4, --:imagename
                $5, --:eventdate
                $6, --:starttime
                $7, --:endtime
                current_timestamp(3), --:createddate
                $8 --:userid
);

select cast(currval(''mp_calendarevents_itemid_seq'') as int4);'
security definer language sql;
grant execute on function mp_calendarevents_insert(
	int, --:moduleid $1
	varchar(255), --:title $2
	text, --:description $3
	varchar(100), --:imagename $4
	timestamp, --:eventdate $5
	timestamp, --:starttime $6
	timestamp, --:endtime $7
	int --:userid $8
) to public;

create or replace function mp_calendarevents_update(
	int, --:itemid $1
	int, --:moduleid $2
	varchar(255), --:title $3
	text, --:description $4
	varchar(100), --:imagename $5
	timestamp, --:eventdate $6
	timestamp, --:starttime $7
	timestamp, --:endtime $8
	int --:userid $9
) returns int
as '
/*
Author:   			
Created: 			4/17/2005
Last Modified: 			4/17/2005
*/

update 		mp_calendarevents

set
            moduleid = $2, --:moduleid
            title = $3, --:title
            description = $4, --:description
            imagename = $5, --:imagename
            eventdate = $6, --:eventdate
            starttime = $7, --:starttime
            endtime = $8, --:endtime
            userid = $9 --:userid
            
where
            itemid = $1; --:itemid

select 1;'
security definer language sql;
grant execute on function mp_calendarevents_update(
	int, --:itemid $1
	int, --:moduleid $2
	varchar(255), --:title $3
	text, --:description $4
	varchar(100), --:imagename $5
	timestamp, --:eventdate $6
	timestamp, --:starttime $7
	timestamp, --:endtime $8
	int --:userid $9
) to public;





-- End Calendar Events


drop function drop_type
(
	varchar(100) --: typename
);


    


