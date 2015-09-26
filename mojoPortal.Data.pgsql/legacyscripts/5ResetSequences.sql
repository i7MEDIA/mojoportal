select setval('mp_blogcomments_blogcommentid_seq', (select max(blogcommentid) from mp_blogcomments))
	where(select max(blogcommentid) from mp_blogcomments) > 0;

select setval('mp_blogs_itemid_seq', (select max(itemid) from mp_blogs))
	where(select max(itemid) from mp_blogs) > 0;

select setval('mp_forumposts_postid_seq', (select max(postid) from mp_forumposts))
	where(select max(postid) from mp_forumposts) > 0;

select setval('mp_forums_itemid_seq', (select max(itemid) from mp_forums))
	where(select max(itemid) from mp_forums) > 0;

select setval('mp_forumthreads_threadid_seq', (select max(threadid) from mp_forumthreads))
	where(select max(threadid) from mp_forumthreads) > 0;

select setval('mp_forumthreadsubscriptions_threadsubscriptionid_seq', (select max(threadsubscriptionid) from mp_forumthreadsubscriptions))
	where(select max(threadsubscriptionid) from mp_forumthreadsubscriptions) > 0;

select setval('mp_galleryimages_itemid_seq', (select max(itemid) from mp_galleryimages))
	where(select max(itemid) from mp_galleryimages) > 0;

select setval('mp_htmlcontent_itemid_seq', (select max(itemid) from mp_htmlcontent))
	where(select max(itemid) from mp_htmlcontent) > 0;

select setval('mp_links_itemid_seq', (select max(itemid) from mp_links))
	where(select max(itemid) from mp_links) > 0;

select setval('mp_moduledefinitions_moduledefid_seq', (select max(moduledefid) from mp_moduledefinitions))
	where(select max(moduledefid) from mp_moduledefinitions) > 0;

select setval('mp_moduledefinitionsettings_id_seq', (select max(id) from mp_moduledefinitionsettings))
	where(select max(id) from mp_moduledefinitionsettings) > 0;

select setval('mp_modules_moduleid_seq', (select max(moduleid) from mp_modules))
	where(select max(moduleid) from mp_modules) > 0;

select setval('mp_modulesettings_id_seq', (select max(id) from mp_modulesettings))
	where(select max(id) from mp_modulesettings) > 0;

select setval('mp_pages_pageid_seq', (select max(pageid) from mp_pages))
	where(select max(pageid) from mp_pages) > 0;

select setval('mp_roles_roleid_seq', (select max(roleid) from mp_roles))
	where(select max(roleid) from mp_roles) > 0;

select setval('mp_sharedfilefolders_folderid_seq', (select max(folderid) from mp_sharedfilefolders))
	where(select max(folderid) from mp_sharedfilefolders) > 0;

select setval('mp_sharedfiles_itemid_seq', (select max(itemid) from mp_sharedfiles))
	where(select max(itemid) from mp_sharedfiles) > 0;

select setval('mp_sharedfileshistory_id_seq', (select max(id) from mp_sharedfileshistory))
	where(select max(id) from mp_sharedfileshistory) > 0;

select setval('mp_sitehosts_hostid_seq', (select max(hostid) from mp_sitehosts))
	where(select max(hostid) from mp_sitehosts) > 0;

select setval('mp_sites_siteid_seq', (select max(siteid) from mp_sites))
	where(select max(siteid) from mp_sites) > 0;

select setval('mp_userroles_id_seq', (select max(id) from mp_userroles))
	where(select max(id) from mp_userroles) > 0;

select setval('mp_users_userid_seq', (select max(userid) from mp_users))
	where(select max(userid) from mp_users) > 0;
	
select setval('mp_rssfeeds_itemid_seq', (select max(itemid) from mp_rssfeeds))
	where(select max(itemid) from mp_rssfeeds) > 0;
	
select setval('mp_calendarevents_itemid_seq', (select max(itemid) from mp_calendarevents))
	where(select max(itemid) from mp_calendarevents) > 0;