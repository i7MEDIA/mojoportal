/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2013-12-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using Npgsql;


namespace mojoPortal.Data
{
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
            string pageEncoding,
            string additionalMetaTags,
            bool useUrl,
            string url,
            bool openInNewWindow,
            bool showChildPageMenu,
            bool hideMainMenu,
            bool includeInMenu,
            String menuImage,
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
            int publishMode,
            Guid createdBy,
            string createdFromIp,
            string menuDescription,
            string linkRel,
            string pageHeading,
            bool showPageHeading,
            DateTime pubDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_pages (");
            sqlCommand.Append("parentid, ");
            sqlCommand.Append("pageorder, ");
            sqlCommand.Append("siteid, ");
            sqlCommand.Append("pagename, ");
            sqlCommand.Append("pagetitle, ");
            sqlCommand.Append("authorizedroles, ");
            sqlCommand.Append("editroles, ");
            sqlCommand.Append("drafteditroles, ");
            sqlCommand.Append("draftapprovalroles, ");
            sqlCommand.Append("createchildpageroles, ");
            sqlCommand.Append("createchilddraftroles, ");
            sqlCommand.Append("requiressl, ");
            sqlCommand.Append("allowbrowsercache, ");
            sqlCommand.Append("showbreadcrumbs, ");
            sqlCommand.Append("pagekeywords, ");
            sqlCommand.Append("pagedescription, ");
            sqlCommand.Append("pageencoding, ");
            sqlCommand.Append("additionalmetatags, ");
            sqlCommand.Append("menuimage, ");
            sqlCommand.Append("useurl, ");
            sqlCommand.Append("url, ");
            sqlCommand.Append("openinnewwindow, ");
            sqlCommand.Append("showchildpagemenu, ");
            sqlCommand.Append("showchildbreadcrumbs, ");
            sqlCommand.Append("skin, ");
            sqlCommand.Append("hidemainmenu, ");
            sqlCommand.Append("includeinmenu, ");
            sqlCommand.Append("changefrequency, ");
            sqlCommand.Append("sitemappriority, ");
            sqlCommand.Append("lastmodifiedutc, ");
            sqlCommand.Append("pageguid, ");
            sqlCommand.Append("parentguid, ");
            sqlCommand.Append("hideafterlogin, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("compiledmeta, ");
            sqlCommand.Append("compiledmetautc, ");
            sqlCommand.Append("includeinsitemap, ");
            sqlCommand.Append("isclickable, ");
            sqlCommand.Append("ispending, ");
            

            sqlCommand.Append("includeinchildsitemap, ");
            sqlCommand.Append("expandonsitemap, ");
            
            sqlCommand.Append("pubteamid, ");
            sqlCommand.Append("publishmode, ");
            sqlCommand.Append("bodycssclass, ");
            sqlCommand.Append("menucssclass, ");
            sqlCommand.Append("menudesc, ");
            sqlCommand.Append("canonicaloverride, ");
            sqlCommand.Append("includeinsearchmap, ");
            sqlCommand.Append("enablecomments, ");
            sqlCommand.Append("showhomecrumb, ");

            sqlCommand.Append("linkrel, ");
            sqlCommand.Append("pageheading, ");
            sqlCommand.Append("showpageheading, ");
            sqlCommand.Append("pubdateutc, ");

            sqlCommand.Append("pcreatedutc, ");
            sqlCommand.Append("pcreatedby, ");
            sqlCommand.Append("pcreatedfromip, ");
            sqlCommand.Append("plastmodutc, ");
            sqlCommand.Append("plastmodby, ");
            sqlCommand.Append("plastmodfromip ");

            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":parentid, ");
            sqlCommand.Append(":pageorder, ");
            sqlCommand.Append(":siteid, ");
            sqlCommand.Append(":pagename, ");
            sqlCommand.Append(":pagetitle, ");
            sqlCommand.Append(":authorizedroles, ");
            sqlCommand.Append(":editroles, ");
            sqlCommand.Append(":drafteditroles, ");
            sqlCommand.Append(":draftapprovalroles, ");
            sqlCommand.Append(":createchildpageroles, ");
            sqlCommand.Append(":createchilddraftroles, ");
            sqlCommand.Append(":requiressl, ");
            sqlCommand.Append(":allowbrowsercache, ");
            sqlCommand.Append(":showbreadcrumbs, ");
            sqlCommand.Append(":pagekeywords, ");
            sqlCommand.Append(":pagedescription, ");
            sqlCommand.Append(":pageencoding, ");
            sqlCommand.Append(":additionalmetatags, ");
            sqlCommand.Append(":menuimage, ");
            sqlCommand.Append(":useurl, ");
            sqlCommand.Append(":url, ");
            sqlCommand.Append(":openinnewwindow, ");
            sqlCommand.Append(":showchildpagemenu, ");
            sqlCommand.Append(":showchildbreadcrumbs, ");
            sqlCommand.Append(":skin, ");
            sqlCommand.Append(":hidemainmenu, ");
            sqlCommand.Append(":includeinmenu, ");
            sqlCommand.Append(":changefrequency, ");
            sqlCommand.Append(":sitemappriority, ");
            sqlCommand.Append(":lastmodifiedutc, ");
            sqlCommand.Append(":pageguid, ");
            sqlCommand.Append(":parentguid, ");
            sqlCommand.Append(":hideafterlogin, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":compiledmeta, ");
            sqlCommand.Append(":compiledmetautc, ");
            sqlCommand.Append(":includeinsitemap, ");
            sqlCommand.Append(":isclickable, ");
            sqlCommand.Append(":ispending, ");

            sqlCommand.Append(":includeinchildsitemap, ");
            sqlCommand.Append(":expandonsitemap, ");
            sqlCommand.Append(":pubteamid, ");
            sqlCommand.Append(":publishmode, ");
            sqlCommand.Append(":bodycssclass, ");
            sqlCommand.Append(":menucssclass, ");
            sqlCommand.Append(":menudesc, ");
            sqlCommand.Append(":canonicaloverride, ");
            sqlCommand.Append(":includeinsearchmap, ");
            sqlCommand.Append(":enablecomments, ");

            sqlCommand.Append(":showhomecrumb, ");

            sqlCommand.Append(":linkrel, ");
            sqlCommand.Append(":pageheading, ");
            sqlCommand.Append(":showpageheading, ");
            sqlCommand.Append(":pubdateutc, ");

            sqlCommand.Append(":pcreatedutc, ");
            sqlCommand.Append(":pcreatedby, ");
            sqlCommand.Append(":pcreatedfromip, ");
            sqlCommand.Append(":plastmodutc, ");
            sqlCommand.Append(":plastmodby, ");
            sqlCommand.Append(":plastmodfromip ");

            sqlCommand.Append(")");

            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_pages_pageid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[60];

            arParams[0] = new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentId;

            arParams[1] = new NpgsqlParameter(":pageorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            arParams[2] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            arParams[3] = new NpgsqlParameter(":pagename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageName;

            arParams[4] = new NpgsqlParameter(":pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageTitle;

            arParams[5] = new NpgsqlParameter(":authorizedroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = authorizedRoles;

            arParams[6] = new NpgsqlParameter(":editroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = editRoles;

            arParams[7] = new NpgsqlParameter(":createchildpageroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createChildPageRoles;

            arParams[8] = new NpgsqlParameter(":requiressl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = requireSsl;

            arParams[9] = new NpgsqlParameter(":allowbrowsercache", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowBrowserCache;

            arParams[10] = new NpgsqlParameter(":showbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = showBreadcrumbs;

            arParams[11] = new NpgsqlParameter(":pagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 1000);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = pageKeyWords;

            arParams[12] = new NpgsqlParameter(":pagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = pageDescription;

            arParams[13] = new NpgsqlParameter(":pageencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = pageEncoding;

            arParams[14] = new NpgsqlParameter(":additionalmetatags", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = additionalMetaTags;

            arParams[15] = new NpgsqlParameter(":menuimage", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = menuImage;

            arParams[16] = new NpgsqlParameter(":useurl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = useUrl;

            arParams[17] = new NpgsqlParameter(":url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = url;

            arParams[18] = new NpgsqlParameter(":openinnewwindow", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = openInNewWindow;

            arParams[19] = new NpgsqlParameter(":showchildpagemenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = showChildPageMenu;

            arParams[20] = new NpgsqlParameter(":showchildbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = showChildPageBreadcrumbs;

            arParams[21] = new NpgsqlParameter(":skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = skin;

            arParams[22] = new NpgsqlParameter(":hidemainmenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = hideMainMenu;

            arParams[23] = new NpgsqlParameter(":includeinmenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = includeInMenu;

            arParams[24] = new NpgsqlParameter(":changefrequency", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new NpgsqlParameter(":sitemappriority", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new NpgsqlParameter(":lastmodifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = pageGuid.ToString();

            arParams[28] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = parentGuid.ToString();

            arParams[29] = new NpgsqlParameter(":hideafterlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = hideAfterLogin;

            arParams[30] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = siteGuid.ToString();

            arParams[31] = new NpgsqlParameter(":compiledmeta", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = compiledMeta;

            arParams[32] = new NpgsqlParameter(":compiledmetautc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = compiledMetaUtc;

            arParams[33] = new NpgsqlParameter(":includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = includeInSiteMap;

            arParams[34] = new NpgsqlParameter(":isclickable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = isClickable;

            arParams[35] = new NpgsqlParameter(":showhomecrumb", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = showHomeCrumb;

            arParams[36] = new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = draftEditRoles;

            arParams[37] = new NpgsqlParameter(":ispending", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = isPending;

            arParams[38] = new NpgsqlParameter(":canonicaloverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = canonicalOverride;

            arParams[39] = new NpgsqlParameter(":includeinsearchmap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = includeInSearchMap;

            arParams[40] = new NpgsqlParameter(":enablecomments", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = enableComments;

            arParams[41] = new NpgsqlParameter(":createchilddraftroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = createChildDraftRoles;

            arParams[42] = new NpgsqlParameter(":includeinchildsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = includeInChildSiteMap;

            arParams[43] = new NpgsqlParameter(":pubteamid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubTeamId.ToString();

            arParams[44] = new NpgsqlParameter(":bodycssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = bodyCssClass;

            arParams[45] = new NpgsqlParameter(":menucssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = menuCssClass;

            arParams[46] = new NpgsqlParameter(":expandonsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = expandOnSiteMap;

            arParams[47] = new NpgsqlParameter(":publishmode", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = publishMode;


            arParams[48] = new NpgsqlParameter(":pcreatedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new NpgsqlParameter(":pcreatedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = createdBy.ToString();

            arParams[50] = new NpgsqlParameter(":pcreatedfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = createdFromIp;

            arParams[51] = new NpgsqlParameter(":plastModutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = DateTime.UtcNow;

            arParams[52] = new NpgsqlParameter(":plastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = createdBy.ToString();

            arParams[53] = new NpgsqlParameter(":plastmodfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = createdFromIp;

            arParams[54] = new NpgsqlParameter(":menudesc", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = menuDescription;

            arParams[55] = new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = draftApprovalRoles;

            arParams[56] = new NpgsqlParameter(":linkrel", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = linkRel;

            arParams[57] = new NpgsqlParameter(":pageheading", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = pageHeading;

            arParams[58] = new NpgsqlParameter(":showpageheading", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = showPageHeading;

            arParams[59] = new NpgsqlParameter(":pubdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[59].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[59].Value = DBNull.Value;
            }
            else
            {
                arParams[59].Value = pubDateUtc;
            }
            

            

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
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
            string pageEncoding,
            string additionalMetaTags,
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
            int publishMode,
            DateTime createdUtc,
            Guid createdBy,
            Guid lastModBy,
            string lastModFromIp,
            string menuDescription,
            string linkRel,
            string pageHeading,
            bool showPageHeading,
            DateTime pubDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_pages ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("parentid = :parentid, ");
            sqlCommand.Append("pageorder = :pageorder, ");
            //sqlCommand.Append("siteid = :siteid, ");
            sqlCommand.Append("pagename = :pagename, ");
            sqlCommand.Append("pagetitle = :pagetitle, ");
            sqlCommand.Append("authorizedroles = :authorizedroles, ");
            sqlCommand.Append("editroles = :editroles, ");
            sqlCommand.Append("drafteditroles = :drafteditroles, ");
            sqlCommand.Append("draftapprovalroles = :draftapprovalroles, ");
            sqlCommand.Append("createchildpageroles = :createchildpageroles, ");
            sqlCommand.Append("createchilddraftroles = :createchilddraftroles, ");
            sqlCommand.Append("requiressl = :requiressl, ");
            sqlCommand.Append("allowbrowsercache = :allowbrowsercache, ");
            sqlCommand.Append("showbreadcrumbs = :showbreadcrumbs, ");
            sqlCommand.Append("pagekeywords = :pagekeywords, ");
            sqlCommand.Append("pagedescription = :pagedescription, ");
            sqlCommand.Append("pageencoding = :pageencoding, ");
            sqlCommand.Append("additionalmetatags = :additionalmetatags, ");
            sqlCommand.Append("menuimage = :menuimage, ");
            sqlCommand.Append("useurl = :useurl, ");
            sqlCommand.Append("url = :url, ");
            sqlCommand.Append("openinnewwindow = :openinnewwindow, ");
            sqlCommand.Append("showchildpagemenu = :showchildpagemenu, ");
            sqlCommand.Append("showchildbreadcrumbs = :showchildbreadcrumbs, ");
            sqlCommand.Append("skin = :skin, ");
            sqlCommand.Append("hidemainmenu = :hidemainmenu, ");
            sqlCommand.Append("includeinmenu = :includeinmenu, ");
            sqlCommand.Append("changefrequency = :changefrequency, ");
            sqlCommand.Append("sitemappriority = :sitemappriority, ");
            sqlCommand.Append("lastmodifiedutc = :lastmodifiedutc, ");
            //sqlCommand.Append("pageguid = :pageguid, ");
            sqlCommand.Append("parentguid = :parentguid, ");
            sqlCommand.Append("hideafterlogin = :hideafterlogin, ");
            //sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("compiledmeta = :compiledmeta, ");
            sqlCommand.Append("compiledmetautc = :compiledmetautc, ");
            sqlCommand.Append("includeinsitemap = :includeinsitemap, ");
            sqlCommand.Append("isclickable = :isclickable, ");
            sqlCommand.Append("ispending = :ispending, ");

            sqlCommand.Append("canonicaloverride = :canonicaloverride, ");
            sqlCommand.Append("includeinsearchmap = :includeinsearchmap, ");
            sqlCommand.Append("enablecomments = :enablecomments, ");

            sqlCommand.Append("includeinchildsitemap = :includeinchildsitemap, ");
            sqlCommand.Append("expandonsitemap = :expandonsitemap, ");
            
            sqlCommand.Append("pubteamid = :pubteamid, ");
            sqlCommand.Append("publishmode = :publishmode, ");
            sqlCommand.Append("bodycssclass = :bodycssclass, ");
            sqlCommand.Append("menucssclass = :menucssclass, ");
            sqlCommand.Append("menudesc = :menudesc, ");
            sqlCommand.Append("showhomecrumb = :showhomecrumb, ");

            sqlCommand.Append("linkrel = :linkrel, ");
            sqlCommand.Append("pageheading = :pageheading, ");
            sqlCommand.Append("showpageheading = :showpageheading, ");
            sqlCommand.Append("pubdateutc = :pubdateutc, ");

            sqlCommand.Append("pcreatedutc = :pcreatedutc, ");
            sqlCommand.Append("pcreatedby = :pcreatedby, ");
            sqlCommand.Append("plastmodutc = :plastmodutc, ");
            sqlCommand.Append("plastmodby = :plastmodby, ");
            sqlCommand.Append("plastmodfromip = :plastmodfromip ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("pageid = :pageid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[57];

            arParams[0] = new NpgsqlParameter(":pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new NpgsqlParameter(":parentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new NpgsqlParameter(":pageorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageOrder;

            arParams[3] = new NpgsqlParameter(":pagename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageName;

            arParams[4] = new NpgsqlParameter(":pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageTitle;

            arParams[5] = new NpgsqlParameter(":authorizedroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = authorizedRoles;

            arParams[6] = new NpgsqlParameter(":editroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = editRoles;

            arParams[7] = new NpgsqlParameter(":createchildpageroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createChildPageRoles;

            arParams[8] = new NpgsqlParameter(":requiressl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = requireSsl;

            arParams[9] = new NpgsqlParameter(":allowbrowsercache", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowBrowserCache;

            arParams[10] = new NpgsqlParameter(":showbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = showBreadcrumbs;

            arParams[11] = new NpgsqlParameter(":pagekeywords", NpgsqlTypes.NpgsqlDbType.Varchar, 1000);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = pageKeyWords;

            arParams[12] = new NpgsqlParameter(":pagedescription", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = pageDescription;

            arParams[13] = new NpgsqlParameter(":pageencoding", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = pageEncoding;

            arParams[14] = new NpgsqlParameter(":additionalmetatags", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = additionalMetaTags;

            arParams[15] = new NpgsqlParameter(":menuimage", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = menuImage;

            arParams[16] = new NpgsqlParameter(":useurl", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = useUrl;

            arParams[17] = new NpgsqlParameter(":url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = url;

            arParams[18] = new NpgsqlParameter(":openinnewwindow", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = openInNewWindow;

            arParams[19] = new NpgsqlParameter(":showchildpagemenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = showChildPageMenu;

            arParams[20] = new NpgsqlParameter(":showchildbreadcrumbs", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = showChildPageBreadcrumbs;

            arParams[21] = new NpgsqlParameter(":skin", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = skin;

            arParams[22] = new NpgsqlParameter(":hidemainmenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = hideMainMenu;

            arParams[23] = new NpgsqlParameter(":includeinmenu", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = includeInMenu;

            arParams[24] = new NpgsqlParameter(":changefrequency", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new NpgsqlParameter(":sitemappriority", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new NpgsqlParameter(":lastmodifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = parentGuid.ToString();

            arParams[28] = new NpgsqlParameter(":hideafterlogin", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = hideAfterLogin;

            arParams[29] = new NpgsqlParameter(":compiledmeta", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = compiledMeta;

            arParams[30] = new NpgsqlParameter(":compiledmetautc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = compiledMetaUtc;

            arParams[31] = new NpgsqlParameter(":includeinsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = includeInSiteMap;

            arParams[32] = new NpgsqlParameter(":isclickable", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = isClickable;

            arParams[33] = new NpgsqlParameter(":showhomecrumb", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = showHomeCrumb;

            arParams[34] = new NpgsqlParameter(":drafteditroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = draftEditRoles;

            arParams[35] = new NpgsqlParameter(":ispending", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = isPending;

            arParams[36] = new NpgsqlParameter(":canonicaloverride", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = canonicalOverride;

            arParams[37] = new NpgsqlParameter(":includeinsearchmap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = includeInSearchMap;

            arParams[38] = new NpgsqlParameter(":enablecomments", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = enableComments;

            arParams[39] = new NpgsqlParameter(":createchilddraftroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = createChildDraftRoles;

            arParams[40] = new NpgsqlParameter(":includeinchildsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = includeInChildSiteMap;

            arParams[41] = new NpgsqlParameter(":pubteamid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubTeamId.ToString();

            arParams[42] = new NpgsqlParameter(":bodycssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = bodyCssClass;

            arParams[43] = new NpgsqlParameter(":menucssclass", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = menuCssClass;

            arParams[44] = new NpgsqlParameter(":expandonsitemap", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = expandOnSiteMap;

            arParams[45] = new NpgsqlParameter(":publishmode", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = publishMode;


            arParams[46] = new NpgsqlParameter(":pcreatedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = createdUtc;

            arParams[47] = new NpgsqlParameter(":pcreatedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = createdBy.ToString();

            arParams[48] = new NpgsqlParameter(":plastModutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new NpgsqlParameter(":plastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = lastModBy.ToString();

            arParams[50] = new NpgsqlParameter(":plastmodfromip", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = lastModFromIp;

            arParams[51] = new NpgsqlParameter(":menudesc", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = menuDescription;

            arParams[52] = new NpgsqlParameter(":draftapprovalroles", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = draftApprovalRoles;

            arParams[53] = new NpgsqlParameter(":linkrel", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = linkRel;

            arParams[54] = new NpgsqlParameter(":pageheading", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = pageHeading;

            arParams[55] = new NpgsqlParameter(":showpageheading", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = showPageHeading;

            arParams[56] = new NpgsqlParameter(":pubdateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[56].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[56].Value = DBNull.Value;
            }
            else
            {
                arParams[56].Value = pubDateUtc;
            }

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
}
