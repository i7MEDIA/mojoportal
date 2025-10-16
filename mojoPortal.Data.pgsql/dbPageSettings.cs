using System;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data;

public static class DBPageSettings
{
	public static int Create(
		int siteId,
		int parentId,
		string pageName,
		string pageTitle,
		string skin,
		int pageOrder,
		string authorizedRoles,
		string editRoles,
		string draftEditRoles,
		string draftApprovalRoles,
		string createChildPageRoles,
		string createChildDraftRoles,
		bool requireSsl,
		bool allowBrowserCache,
		bool showBreadcrumbs,
		bool showChildPageBreadcrumbs,
		string pageKeyWords,
		string pageDescription,
		bool useUrl,
		string url,
		bool openInNewWindow,
		bool showChildPageMenu,
		bool hideMainMenu,
		bool includeInMenu,
		string menuImage,
		string changeFrequency,
		string siteMapPriority,
		Guid pageGuid,
		Guid parentGuid,
		bool hideAfterLogin,
		Guid siteGuid,
		string compiledMeta,
		DateTime compiledMetaUtc,
		bool includeInSiteMap,
		bool isClickable,
		bool showHomeCrumb,
		bool isPending,
		string canonicalOverride,
		bool includeInSearchMap,
		bool enableComments,
		bool includeInChildSiteMap,
		bool expandOnSiteMap,
		Guid pubTeamId,
		string bodyCssClass,
		string menuCssClass,
		Guid createdBy,
		string createdFromIp,
		string menuDescription,
		string linkRel,
		string pageHeading,
		bool showPageHeading,
		DateTime pubDateUtc,
		string styleSets)
	{
		var sqlCommand = """
            INSERT INTO mp_pages (
                parentid, 
                pageorder, 
                siteid, 
                pagename, 
                pagetitle, 
                authorizedroles, 
                editroles, 
                drafteditroles, 
                draftapprovalroles, 
                createchildpageroles, 
                createchilddraftroles, 
                requiressl, 
                allowbrowsercache, 
                showbreadcrumbs, 
                pagekeywords, 
                pagedescription, 
                menuimage, 
                useurl, 
                url, 
                openinnewwindow, 
                showchildpagemenu, 
                showchildbreadcrumbs, 
                skin, 
                hidemainmenu, 
                includeinmenu, 
                changefrequency, 
                sitemappriority, 
                lastmodifiedutc, 
                pageguid, 
                parentguid, 
                hideafterlogin, 
                siteguid, 
                compiledmeta, 
                compiledmetautc, 
                includeinsitemap, 
                isclickable, 
                ispending, 
                includeinchildsitemap, 
                expandonsitemap, 
                pubteamid, 
                bodycssclass, 
                menucssclass, 
                menudesc, 
                canonicaloverride, 
                includeinsearchmap, 
                enablecomments, 
                showhomecrumb, 
                linkrel, 
                pageheading, 
                showpageheading, 
                pubdateutc, 
                pcreatedutc, 
                pcreatedby, 
                pcreatedfromip, 
                plastmodutc, 
                plastmodby, 
                plastmodfromip, 
                stylesets 
            )
            VALUES (
                :parentid, 
                :pageorder, 
                :siteid, 
                :pagename, 
                :pagetitle, 
                :authorizedroles, 
                :editroles, 
                :drafteditroles, 
                :draftapprovalroles, 
                :createchildpageroles, 
                :createchilddraftroles, 
                :requiressl, 
                :allowbrowsercache, 
                :showbreadcrumbs, 
                :pagekeywords, 
                :pagedescription, 
                :pageencoding, 
                :additionalmetatags, 
                :menuimage, 
                :useurl, 
                :url, 
                :openinnewwindow, 
                :showchildpagemenu, 
                :showchildbreadcrumbs, 
                :skin, 
                :hidemainmenu, 
                :includeinmenu, 
                :changefrequency, 
                :sitemappriority, 
                :lastmodifiedutc, 
                :pageguid, 
                :parentguid, 
                :hideafterlogin, 
                :siteguid, 
                :compiledmeta, 
                :compiledmetautc, 
                :includeinsitemap, 
                :isclickable, 
                :ispending, 
                :includeinchildsitemap, 
                :expandonsitemap, 
                :pubteamid, 
                :bodycssclass, 
                :menucssclass, 
                :menudesc, 
                :canonicaloverride, 
                :includeinsearchmap, 
                :enablecomments, 
                :showhomecrumb, 
                :linkrel, 
                :pageheading, 
                :showpageheading, 
                :pubdateutc, 
                :pcreatedutc, 
                :pcreatedby, 
                :pcreatedfromip, 
                :plastmodutc, 
                :plastmodby, 
                :plastmodfromip, 
                :stylesets 
            );
                SELECT CURRVAL('mp_pages_pageid_seq')
         """;

		NpgsqlParameter[] arParams =
		[
			new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = parentId },
			new NpgsqlParameter(":pageorder", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageOrder },
			new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = siteId },
			new NpgsqlParameter(":pagename", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageName },
			new NpgsqlParameter(":pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageTitle },
			new NpgsqlParameter(":authorizedroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = authorizedRoles },
			new NpgsqlParameter(":editroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = editRoles },
			new NpgsqlParameter(":createchildpageroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = createChildPageRoles },
			new NpgsqlParameter(":requiressl", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = requireSsl },
			new NpgsqlParameter(":allowbrowsercache", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = allowBrowserCache },
			new NpgsqlParameter(":showbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showBreadcrumbs },
			new NpgsqlParameter(":pagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 1000) { Direction = ParameterDirection.Input, Value = pageKeyWords },
			new NpgsqlParameter(":pagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageDescription },
			new NpgsqlParameter(":menuimage", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = menuImage },
			new NpgsqlParameter(":useurl", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = useUrl },
			new NpgsqlParameter(":url", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = url },
			new NpgsqlParameter(":openinnewwindow", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = openInNewWindow },
			new NpgsqlParameter(":showchildpagemenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showChildPageMenu },
			new NpgsqlParameter(":showchildbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showChildPageBreadcrumbs },
			new NpgsqlParameter(":skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = skin },
			new NpgsqlParameter(":hidemainmenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = hideMainMenu },
			new NpgsqlParameter(":includeinmenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInMenu },
			new NpgsqlParameter(":changefrequency", NpgsqlTypes.NpgsqlDbType.Varchar, 20) { Direction = ParameterDirection.Input, Value = changeFrequency },
			new NpgsqlParameter(":sitemappriority", NpgsqlTypes.NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = siteMapPriority },
			new NpgsqlParameter(":lastmodifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = pageGuid.ToString() },
			new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = parentGuid.ToString() },
			new NpgsqlParameter(":hideafterlogin", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = hideAfterLogin },
			new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new NpgsqlParameter(":compiledmeta", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = compiledMeta },
			new NpgsqlParameter(":compiledmetautc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = compiledMetaUtc },
			new NpgsqlParameter(":includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInSiteMap },
			new NpgsqlParameter(":isclickable", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isClickable },
			new NpgsqlParameter(":showhomecrumb", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showHomeCrumb },
			new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = draftEditRoles },
			new NpgsqlParameter(":ispending", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isPending },
			new NpgsqlParameter(":canonicaloverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = canonicalOverride },
			new NpgsqlParameter(":includeinsearchmap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInSearchMap },
			new NpgsqlParameter(":enablecomments", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = enableComments },
			new NpgsqlParameter(":createchilddraftroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = createChildDraftRoles },
			new NpgsqlParameter(":includeinchildsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInChildSiteMap },
			new NpgsqlParameter(":pubteamid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = pubTeamId.ToString() },
			new NpgsqlParameter(":bodycssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = bodyCssClass },
			new NpgsqlParameter(":menucssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = menuCssClass },
			new NpgsqlParameter(":expandonsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = expandOnSiteMap },
			new NpgsqlParameter(":pcreatedutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new NpgsqlParameter(":pcreatedby", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = createdBy.ToString() },
			new NpgsqlParameter(":pcreatedfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36) { Direction = ParameterDirection.Input, Value = createdFromIp },
			new NpgsqlParameter(":plastModutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new NpgsqlParameter(":plastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = createdBy.ToString() },
			new NpgsqlParameter(":plastmodfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36) { Direction = ParameterDirection.Input, Value = createdFromIp },
			new NpgsqlParameter(":menudesc", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = menuDescription },
			new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = draftApprovalRoles },
			new NpgsqlParameter(":linkrel", NpgsqlTypes.NpgsqlDbType.Varchar, 20) { Direction = ParameterDirection.Input, Value = linkRel },
			new NpgsqlParameter(":pageheading", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageHeading },
			new NpgsqlParameter(":showpageheading", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showPageHeading },
			new NpgsqlParameter(":pubdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = pubDateUtc == DateTime.MaxValue ? DBNull.Value : pubDateUtc },
			new NpgsqlParameter(":stylesets", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = styleSets },
			];


		int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
		CommandType.Text,
		sqlCommand,
		arParams));

		return newID;
	}

	public static bool UpdatePage(
		int siteId,
		int pageId,
		int parentId,
		string pageName,
		string pageTitle,
		string skin,
		int pageOrder,
		string authorizedRoles,
		string editRoles,
		string draftEditRoles,
		string draftApprovalRoles,
		string createChildPageRoles,
		string createChildDraftRoles,
		bool requireSsl,
		bool allowBrowserCache,
		bool showBreadcrumbs,
		bool showChildPageBreadcrumbs,
		string pageKeyWords,
		string pageDescription,
		bool useUrl,
		string url,
		bool openInNewWindow,
		bool showChildPageMenu,
		bool hideMainMenu,
		bool includeInMenu,
		String menuImage,
		string changeFrequency,
		string siteMapPriority,
		Guid parentGuid,
		bool hideAfterLogin,
		string compiledMeta,
		DateTime compiledMetaUtc,
		bool includeInSiteMap,
		bool isClickable,
		bool showHomeCrumb,
		bool isPending,
		string canonicalOverride,
		bool includeInSearchMap,
		bool enableComments,
		bool includeInChildSiteMap,
		bool expandOnSiteMap,
		Guid pubTeamId,
		string bodyCssClass,
		string menuCssClass,
		Guid lastModBy,
		string lastModFromIp,
		string menuDescription,
		string linkRel,
		string pageHeading,
		bool showPageHeading,
		DateTime pubDateUtc,
		string styleSets)
	{
		var sqlCommand = """
			UPDATE mp_pages 
			SET  
				parentid = :parentid, 
				pageorder = :pageorder, 
				pagename = :pagename, 
				pagetitle = :pagetitle, 
				authorizedroles = :authorizedroles, 
				editroles = :editroles, 
				drafteditroles = :drafteditroles, 
				draftapprovalroles = :draftapprovalroles, 
				createchildpageroles = :createchildpageroles, 
				createchilddraftroles = :createchilddraftroles, 
				requiressl = :requiressl, 
				allowbrowsercache = :allowbrowsercache, 
				showbreadcrumbs = :showbreadcrumbs, 
				pagekeywords = :pagekeywords, 
				pagedescription = :pagedescription, 
				menuimage = :menuimage, 
				useurl = :useurl, 
				url = :url, 
				openinnewwindow = :openinnewwindow, 
				showchildpagemenu = :showchildpagemenu, 
				showchildbreadcrumbs = :showchildbreadcrumbs, 
				skin = :skin, 
				hidemainmenu = :hidemainmenu, 
				includeinmenu = :includeinmenu, 
				changefrequency = :changefrequency, 
				sitemappriority = :sitemappriority, 
				lastmodifiedutc = :lastmodifiedutc, 
				parentguid = :parentguid, 
				hideafterlogin = :hideafterlogin, 
				compiledmeta = :compiledmeta, 
				compiledmetautc = :compiledmetautc, 
				includeinsitemap = :includeinsitemap, 
				isclickable = :isclickable, 
				ispending = :ispending, 
				canonicaloverride = :canonicaloverride, 
				includeinsearchmap = :includeinsearchmap, 
				enablecomments = :enablecomments, 
				includeinchildsitemap = :includeinchildsitemap, 
				expandonsitemap = :expandonsitemap, 
				pubteamid = :pubteamid, 
				bodycssclass = :bodycssclass, 
				menucssclass = :menucssclass, 
				menudesc = :menudesc, 
				showhomecrumb = :showhomecrumb, 
				linkrel = :linkrel, 
				pageheading = :pageheading, 
				showpageheading = :showpageheading, 
				pubdateutc = :pubdateutc, 
				pcreatedutc = :pcreatedutc, 
				pcreatedby = :pcreatedby, 
				plastmodutc = :plastmodutc, 
				plastmodby = :plastmodby, 
				plastmodfromip = :plastmodfromip, 
				stylesets = :stylesets 

			WHERE pageid = :pageid;
		""";

		NpgsqlParameter[] arParams =
		 [
			new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageId },
			new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = parentId },
			new NpgsqlParameter(":pageorder", NpgsqlTypes.NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageOrder },
			new NpgsqlParameter(":pagename", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageName },
			new NpgsqlParameter(":pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageTitle },
			new NpgsqlParameter(":authorizedroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = authorizedRoles },
			new NpgsqlParameter(":editroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = editRoles },
			new NpgsqlParameter(":createchildpageroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = createChildPageRoles },
			new NpgsqlParameter(":requiressl", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = requireSsl },
			new NpgsqlParameter(":allowbrowsercache", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = allowBrowserCache },
			new NpgsqlParameter(":showbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showBreadcrumbs },
			new NpgsqlParameter(":pagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 1000) { Direction = ParameterDirection.Input, Value = pageKeyWords },
			new NpgsqlParameter(":pagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageDescription },
			new NpgsqlParameter(":menuimage", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = menuImage },
			new NpgsqlParameter(":useurl", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = useUrl },
			new NpgsqlParameter(":url", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = url },
			new NpgsqlParameter(":openinnewwindow", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = openInNewWindow },
			new NpgsqlParameter(":showchildpagemenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showChildPageMenu },
			new NpgsqlParameter(":showchildbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showChildPageBreadcrumbs },
			new NpgsqlParameter(":skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = skin },
			new NpgsqlParameter(":hidemainmenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = hideMainMenu },
			new NpgsqlParameter(":includeinmenu", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInMenu },
			new NpgsqlParameter(":changefrequency", NpgsqlTypes.NpgsqlDbType.Varchar, 20) { Direction = ParameterDirection.Input, Value = changeFrequency },
			new NpgsqlParameter(":sitemappriority", NpgsqlTypes.NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = siteMapPriority },
			new NpgsqlParameter(":lastmodifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = parentGuid.ToString() },
			new NpgsqlParameter(":hideafterlogin", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = hideAfterLogin },
			new NpgsqlParameter(":compiledmeta", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = compiledMeta },
			new NpgsqlParameter(":compiledmetautc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = compiledMetaUtc },
			new NpgsqlParameter(":includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInSiteMap },
			new NpgsqlParameter(":isclickable", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isClickable },
			new NpgsqlParameter(":showhomecrumb", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showHomeCrumb },
			new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = draftEditRoles },
			new NpgsqlParameter(":ispending", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isPending },
			new NpgsqlParameter(":canonicaloverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = canonicalOverride },
			new NpgsqlParameter(":includeinsearchmap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInSearchMap },
			new NpgsqlParameter(":enablecomments", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = enableComments },
			new NpgsqlParameter(":createchilddraftroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = createChildDraftRoles },
			new NpgsqlParameter(":includeinchildsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeInChildSiteMap },
			new NpgsqlParameter(":pubteamid", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = pubTeamId.ToString() },
			new NpgsqlParameter(":bodycssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = bodyCssClass },
			new NpgsqlParameter(":menucssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = menuCssClass },
			new NpgsqlParameter(":expandonsitemap", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = expandOnSiteMap },
			new NpgsqlParameter(":plastModutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new NpgsqlParameter(":plastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36) { Direction = ParameterDirection.Input, Value = lastModBy.ToString() },
			new NpgsqlParameter(":plastmodfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36) { Direction = ParameterDirection.Input, Value = lastModFromIp },
			new NpgsqlParameter(":menudesc", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = menuDescription },
			new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = draftApprovalRoles },
			new NpgsqlParameter(":linkrel", NpgsqlTypes.NpgsqlDbType.Varchar, 20) { Direction = ParameterDirection.Input, Value = linkRel },
			new NpgsqlParameter(":pageheading", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = pageHeading },
			new NpgsqlParameter(":showpageheading", NpgsqlTypes.NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = showPageHeading },
			new NpgsqlParameter(":pubdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp) { Direction = ParameterDirection.Input, Value = pubDateUtc == DateTime.MaxValue ? DBNull.Value : pubDateUtc },
			new NpgsqlParameter(":stylesets", NpgsqlTypes.NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = styleSets },
		];
		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
		CommandType.Text,
		sqlCommand.ToString(),
		arParams);

		return (rowsAffected > -1);
	}

	public static bool UpdateTimestamp(
		int pageId,
		DateTime lastModifiedUtc)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = pageId;

		arParams[1] = new NpgsqlParameter(":lastmodifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = lastModifiedUtc;

		int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_pages_updatetimestamp(:pageid,:lastmodifiedutc)",
			arParams));

		return (rowsAffected > -1);

	}

	public static bool UpdatePageOrder(int pageId, int pageOrder)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = pageId;

		arParams[1] = new NpgsqlParameter(":pageorder", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = pageOrder;

		int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_pages_updatepageorder(:pageid,:pageorder)",
			arParams));

		return (rowsAffected > -1);

	}

	public static bool DeletePage(int pageId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = pageId;

		int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_pages_delete(:pageid)",
			arParams));

		return (rowsAffected > -1);

	}

	public static bool CleanupOrphans()
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("UPDATE mp_pages ");
		sqlCommand.Append("SET parentid = -1, parentguid = '00000000-0000-0000-0000-000000000000' ");
		sqlCommand.Append("WHERE parentid <> -1 AND parentid NOT IN (SELECT pageid FROM mp_pages ) ");
		sqlCommand.Append("");

		int rowsAffected = 0;

		// using scopes the connection and will close it /destroy it when it goes out of scope
		using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString.GetWriteConnectionString()))
		{
			conn.Open();
			using (NpgsqlCommand command = new NpgsqlCommand(sqlCommand.ToString(), conn))
			{
				//command.Parameters.Add(new NpgsqlParameter(":pageguid", DbType.StringFixedLength, 36));
				command.Prepare();
				//command.Parameters[0].Value = pageGuid.ToString();
				rowsAffected = command.ExecuteNonQuery();
			}
		}

		return (rowsAffected > 0);

	}

	public static IDataReader GetPage(Guid pageGuid)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = pageGuid.ToString();

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT p.*, ");

		sqlCommand.Append("u1.name As createdbyname, ");
		sqlCommand.Append("u1.email As createdbyemail, ");
		sqlCommand.Append("u1.firstname As createdbyfirstname, ");
		sqlCommand.Append("u1.lastname As createdbylastname, ");
		sqlCommand.Append("u2.name As lastmodbyname, ");
		sqlCommand.Append("u2.email As lastmodbyemail, ");
		sqlCommand.Append("u2.firstname As lastmodbyfirstname, ");
		sqlCommand.Append("u2.lastname As lastmodbylastname ");

		sqlCommand.Append("FROM	mp_pages p ");

		sqlCommand.Append("LEFT OUTER JOIN	mp_users u1 ");
		sqlCommand.Append("ON p.pcreatedby = u1.userguid ");

		sqlCommand.Append("LEFT OUTER JOIN	mp_users u2 ");
		sqlCommand.Append("ON p.plastmodby = u2.userguid ");

		sqlCommand.Append("WHERE p.pageguid = :pageguid ");
		sqlCommand.Append("LIMIT 1 ; ");

		return NpgsqlHelper.ExecuteReader(ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);

		//return NpgsqlHelper.ExecuteReader(ConnectionString.GetReadConnectionString(),
		//    CommandType.StoredProcedure,
		//    "mp_pages_selectonebyguid(:pageguid)",
		//    arParams);

	}

	public static IDataReader GetPage(int siteId, int pageId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = pageId;

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT p.*, ");

		sqlCommand.Append("u1.name As createdbyname, ");
		sqlCommand.Append("u1.email As createdbyemail, ");
		sqlCommand.Append("u1.firstname As createdbyfirstname, ");
		sqlCommand.Append("u1.lastname As createdbylastname, ");
		sqlCommand.Append("u2.name As lastmodbyname, ");
		sqlCommand.Append("u2.email As lastmodbyemail, ");
		sqlCommand.Append("u2.firstname As lastmodbyfirstname, ");
		sqlCommand.Append("u2.lastname As lastmodbylastname ");

		sqlCommand.Append("FROM	mp_pages p ");

		sqlCommand.Append("LEFT OUTER JOIN	mp_users u1 ");
		sqlCommand.Append("ON p.pcreatedby = u1.userguid ");

		sqlCommand.Append("LEFT OUTER JOIN	mp_users u2 ");
		sqlCommand.Append("ON p.plastmodby = u2.userguid ");

		sqlCommand.Append("WHERE (p.pageid = :pageid OR :pageid = -1)  ");
		sqlCommand.Append("AND p.siteid = :siteid  ");
		sqlCommand.Append("ORDER BY p.parentid, p.pageorder  ");
		sqlCommand.Append("LIMIT 1 ; ");

		return NpgsqlHelper.ExecuteReader(
		   ConnectionString.GetReadConnectionString(),
		   CommandType.Text,
		   sqlCommand.ToString(),
		   arParams);

		//return NpgsqlHelper.ExecuteReader(
		//    ConnectionString.GetReadConnectionString(),
		//    CommandType.StoredProcedure,
		//    "mp_pages_selectone(:siteid,:pageid)",
		//    arParams);

	}

	public static IDataReader GetChildPages(int siteId, int parentPageId)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("parentid = :parentid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("siteid = :siteid ");
		sqlCommand.Append("ORDER BY ");
		sqlCommand.Append("pageorder ");
		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = parentPageId;

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);


	}



	public static int GetNextPageOrder(
		int siteId,
		int parentId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = parentId;

		int pageOrder = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_pages_getnextpageorder(:siteid,:parentid)",
			arParams));

		return pageOrder;

	}


	public static IDataReader GetPageList(int siteId)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("siteid = :siteid ");
		sqlCommand.Append("ORDER BY	parentid, pageorder, pagename ;");
		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);


	}

	/// <summary>
	/// parentid = -1 means root level pages
	/// parentid = -2 means get all pages regardless of parent
	/// </summary>
	/// <param name="siteId"></param>
	/// <param name="parentId"></param>
	/// <returns></returns>
	public static IDataReader GetChildPagesSortedAlphabetic(int siteId, int parentId)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  * ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("siteid = :siteid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("(parentid = :parentid OR :parentid = -2) ");
		sqlCommand.Append("ORDER BY	pagename ");
		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = parentId;

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetPageListForAdmin(int siteId)
	{
		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.StoredProcedure,
			"mp_pages_selectlist(:siteid)",
			arParams);
	}

	public static int GetPendingCount(Guid siteGuid)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  Count(*) ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("siteguid = :siteguid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("ispending = true ");

		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[1];

		arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteGuid;

		return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPendingPageListPage(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetPendingCount(siteGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}



		NpgsqlParameter[] arParams = new NpgsqlParameter[3];

		arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteGuid;

		arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = pageSize;

		arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = pageLowerBound;

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT ");
		sqlCommand.Append("	p.*, ");
		sqlCommand.Append("COALESCE(wip.total,0) as wipcount ");

		sqlCommand.Append("FROM	mp_pages p  ");

		sqlCommand.Append("LEFT OUTER JOIN ");
		sqlCommand.Append("(");

		sqlCommand.Append("SELECT Count(*) as total, ");
		sqlCommand.Append("pm.pageguid ");

		sqlCommand.Append("FROM ");
		sqlCommand.Append("mp_pagemodules pm ");

		sqlCommand.Append("JOIN ");
		sqlCommand.Append("mp_contentworkflow cw ");
		sqlCommand.Append("ON ");
		sqlCommand.Append("cw.moduleguid = pm.moduleguid ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("cw.status Not In ('Cancelled','Approved') ");
		sqlCommand.Append("GROUP BY ");
		sqlCommand.Append("pm.pageguid ");

		sqlCommand.Append(") AS wip ");
		sqlCommand.Append("ON ");
		sqlCommand.Append("wip.pageguid = p.pageguid ");

		sqlCommand.Append("WHERE p.siteguid = :siteguid  ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("p.ispending = true ");
		sqlCommand.Append("ORDER BY  ");
		sqlCommand.Append("p.pagename ");


		sqlCommand.Append("LIMIT  :pagesize");

		if (pageNumber > 1)
			sqlCommand.Append(" OFFSET :pageoffset ");

		sqlCommand.Append(";");

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);


	}

	public static int GetCount(int siteId, bool includePending)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  Count(*) ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("siteid = :siteid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("((ispending = false) OR (:includepending = true)) ");

		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":includepending", NpgsqlTypes.NpgsqlDbType.Boolean);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = includePending;

		return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams));

	}

	public static int GetCountChildPages(int pageId, bool includePending)
	{
		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT  Count(*) ");
		sqlCommand.Append("FROM	mp_pages ");
		sqlCommand.Append("WHERE ");
		sqlCommand.Append("parentid = :pageid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("((ispending = false) OR (:includepending = true)) ");

		sqlCommand.Append(";");

		NpgsqlParameter[] arParams = new NpgsqlParameter[2];

		arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = pageId;

		arParams[1] = new NpgsqlParameter(":includepending", NpgsqlTypes.NpgsqlDbType.Boolean);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = includePending;

		return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPageOfPages(
		int siteId,
		bool includePending,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteId, includePending);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}



		NpgsqlParameter[] arParams = new NpgsqlParameter[4];

		arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[0].Direction = ParameterDirection.Input;
		arParams[0].Value = siteId;

		arParams[1] = new NpgsqlParameter(":includepending", NpgsqlTypes.NpgsqlDbType.Boolean);
		arParams[1].Direction = ParameterDirection.Input;
		arParams[1].Value = includePending;

		arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[2].Direction = ParameterDirection.Input;
		arParams[2].Value = pageSize;

		arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
		arParams[3].Direction = ParameterDirection.Input;
		arParams[3].Value = pageLowerBound;

		StringBuilder sqlCommand = new StringBuilder();
		sqlCommand.Append("SELECT	* ");
		sqlCommand.Append("FROM	mp_pages  ");

		sqlCommand.Append("WHERE ");
		sqlCommand.Append("siteid = :siteid ");
		sqlCommand.Append("AND ");
		sqlCommand.Append("((ispending = false) OR (:includepending = true)) ");

		sqlCommand.Append("ORDER BY ");
		sqlCommand.Append("parentid, pagename ");

		sqlCommand.Append("LIMIT  :pagesize");

		if (pageNumber > 1)
			sqlCommand.Append(" OFFSET :pageoffset ");

		sqlCommand.Append(";");

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			arParams);

	}

}
