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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
   
    public static class DBPageSettings
    {
        
        
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }



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
            DateTime pubDateUtc
            )
        {

            #region byte conversion

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

            int intExpandOnSiteMap = 0;
            if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
            }

            int intincludeInSiteMap = 0;
            if (includeInSiteMap)
            {
                intincludeInSiteMap = 1;
            }

            int intisClickable = 0;
            if (isClickable)
            {
                intisClickable = 1;
            }

            int intshowHomeCrumb = 0;
            if (showHomeCrumb)
            {
                intshowHomeCrumb = 1;
            }

            byte hideauth;
            if (hideAfterLogin)
            {
                hideauth = 1;
            }
            else
            {
                hideauth = 0;
            }

            byte ssl;
            if (requireSsl)
            {
                ssl = 1;
            }
            else
            {
                ssl = 0;
            }

            byte show;
            if (showBreadcrumbs)
            {
                show = 1;
            }
            else
            {
                show = 0;
            }

            byte u;
            if (useUrl)
            {
                u = 1;
            }
            else
            {
                u = 0;
            }

            byte nw;
            if (openInNewWindow)
            {
                nw = 1;
            }
            else
            {
                nw = 0;
            }

            byte cm;
            if (showChildPageMenu)
            {
                cm = 1;
            }
            else
            {
                cm = 0;
            }

            byte cb;
            if (showChildPageBreadcrumbs)
            {
                cb = 1;
            }
            else
            {
                cb = 0;
            }

            byte hm;
            if (hideMainMenu)
            {
                hm = 1;
            }
            else
            {
                hm = 0;
            }

            byte inMenu;
            if (includeInMenu)
            {
                inMenu = 1;
            }
            else
            {
                inMenu = 0;
            }

            byte bcache;
            if (allowBrowserCache)
            {
                bcache = 1;
            }
            else
            {
                bcache = 0;
            }

            int intIncludeInChildSiteMap = 0;
            if (includeInChildSiteMap)
            {
                intIncludeInChildSiteMap = 1;
            }

            int intShowPageHeading = 0;
            if (showPageHeading)
            {
                intShowPageHeading = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Pages ( ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("PageName, ");
            sqlCommand.Append("PageTitle, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("AuthorizedRoles, ");
            sqlCommand.Append("EditRoles, ");
            sqlCommand.Append("DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles, ");
            sqlCommand.Append("CreateChildPageRoles, ");
            sqlCommand.Append("CreateChildDraftRoles, ");
            sqlCommand.Append("RequireSSL, ");
            sqlCommand.Append("AllowBrowserCache, ");
            sqlCommand.Append("ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords, ");
            sqlCommand.Append("PageDescription, ");
            sqlCommand.Append("PageEncoding, ");
            sqlCommand.Append("AdditionalMetaTags, ");
            sqlCommand.Append("UseUrl, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("OpenInNewWindow, ");
            sqlCommand.Append("ShowChildPageMenu, ");
            sqlCommand.Append("ShowChildBreadcrumbs, ");
            sqlCommand.Append("HideMainMenu, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("MenuImage, ");
            sqlCommand.Append("IncludeInMenu, ");
            sqlCommand.Append("ChangeFrequency, ");
            sqlCommand.Append("SiteMapPriority, ");
            sqlCommand.Append("LastModifiedUTC, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("ParentGuid, ");
            sqlCommand.Append("HideAfterLogin, ");
            sqlCommand.Append("IsPending, ");
            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("IsClickable, ");
            sqlCommand.Append("ShowHomeCrumb, ");

            sqlCommand.Append("CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap, ");
            sqlCommand.Append("EnableComments, ");

            sqlCommand.Append("IncludeInChildSiteMap, ");
            sqlCommand.Append("ExpandOnSiteMap, ");

            sqlCommand.Append("PubTeamId, ");
            sqlCommand.Append("PublishMode, ");
            sqlCommand.Append("BodyCssClass, ");
            sqlCommand.Append("MenuCssClass, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc, ");
            sqlCommand.Append("MenuDesc, ");

            sqlCommand.Append("LinkRel, ");
            sqlCommand.Append("PageHeading, ");
            sqlCommand.Append("ShowPageHeading, ");
            sqlCommand.Append("PubDateUtc, ");

            sqlCommand.Append("PCreatedUtc, ");
            sqlCommand.Append("PCreatedBy, ");
            sqlCommand.Append("PCreatedFromIp, ");
            sqlCommand.Append("PLastModUtc, ");
            sqlCommand.Append("PLastModBy, ");
            sqlCommand.Append("PLastModFromIp ");

            sqlCommand.Append(")");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append(" :SiteID , ");
            sqlCommand.Append(" :ParentID , ");
            sqlCommand.Append(" :PageName , ");
            sqlCommand.Append(" :PageTitle , ");
            sqlCommand.Append(" :PageOrder , ");
            sqlCommand.Append(" :AuthorizedRoles , ");
            sqlCommand.Append(" :EditRoles , ");
            sqlCommand.Append(":DraftEditRoles, ");
            sqlCommand.Append(":DraftApprovalRoles, ");
            sqlCommand.Append(" :CreateChildPageRoles , ");
            sqlCommand.Append(":CreateChildDraftRoles, ");
            sqlCommand.Append(" :RequireSSL , ");
            sqlCommand.Append(" :AllowBrowserCache, ");
            sqlCommand.Append(" :ShowBreadcrumbs , ");
            sqlCommand.Append(" :PageKeyWords , ");
            sqlCommand.Append(" :PageDescription , ");
            sqlCommand.Append(" :PageEncoding , ");
            sqlCommand.Append(" :AdditionalMetaTags,  ");
            sqlCommand.Append(" :UseUrl,  ");
            sqlCommand.Append(" :Url,  ");
            sqlCommand.Append(" :OpenInNewWindow,  ");
            sqlCommand.Append(" :ShowChildPageMenu,  ");
            sqlCommand.Append(" :ShowChildPageBreadcrumbs,  ");
            sqlCommand.Append(" :HideMainMenu,  ");
            sqlCommand.Append(" :Skin,  ");
            sqlCommand.Append(" :MenuImage,  ");
            sqlCommand.Append(" :IncludeInMenu,  ");
            sqlCommand.Append(" :ChangeFrequency,  ");
            sqlCommand.Append(" :SiteMapPriority,  ");
            sqlCommand.Append(" :LastModifiedUTC,  ");
            sqlCommand.Append(" :PageGuid,  ");
            sqlCommand.Append(" :ParentGuid,  ");
            sqlCommand.Append(":HideAfterLogin, ");
            sqlCommand.Append(":IsPending, ");
            sqlCommand.Append(":IncludeInSiteMap, ");
            sqlCommand.Append(":IsClickable, ");
            sqlCommand.Append(":ShowHomeCrumb, ");

            sqlCommand.Append(":CanonicalOverride, ");
            sqlCommand.Append(":IncludeInSearchMap, ");
            sqlCommand.Append(":EnableComments, ");

            sqlCommand.Append(":IncludeInChildSiteMap, ");
            sqlCommand.Append(":ExpandOnSiteMap, ");
            sqlCommand.Append(":PubTeamId, ");
            sqlCommand.Append(":PublishMode, ");
            sqlCommand.Append(":BodyCssClass, ");
            sqlCommand.Append(":MenuCssClass, ");

            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":CompiledMeta, ");
            sqlCommand.Append(":CompiledMetaUtc, ");
            sqlCommand.Append(":MenuDesc, ");

            sqlCommand.Append(":LinkRel, ");
            sqlCommand.Append(":PageHeading, ");
            sqlCommand.Append(":ShowPageHeading, ");
            sqlCommand.Append(":PubDateUtc, ");

            sqlCommand.Append(":PCreatedUtc, ");
            sqlCommand.Append(":PCreatedBy, ");
            sqlCommand.Append(":PCreatedFromIp, ");
            sqlCommand.Append(":PLastModUtc, ");
            sqlCommand.Append(":PLastModBy, ");
            sqlCommand.Append(":PLastModFromIp ");

            sqlCommand.Append(")");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[60];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new SqliteParameter(":PageName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageName;

            arParams[3] = new SqliteParameter(":PageOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new SqliteParameter(":AuthorizedRoles", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = authorizedRoles;

            arParams[5] = new SqliteParameter(":RequireSSL", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = ssl;


            arParams[6] = new SqliteParameter(":ShowBreadcrumbs", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = show;

            arParams[7] = new SqliteParameter(":PageKeyWords", DbType.String, 1000);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageKeyWords;

            arParams[8] = new SqliteParameter(":PageDescription", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = pageDescription;

            arParams[9] = new SqliteParameter(":PageEncoding", DbType.String, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = pageEncoding;

            arParams[10] = new SqliteParameter(":AdditionalMetaTags", DbType.String, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = additionalMetaTags;

            arParams[11] = new SqliteParameter(":UseUrl", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = u;

            arParams[12] = new SqliteParameter(":Url", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new SqliteParameter(":OpenInNewWindow", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = nw;

            arParams[14] = new SqliteParameter(":ShowChildPageMenu", DbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = cm;

            arParams[15] = new SqliteParameter(":EditRoles", DbType.Object);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = editRoles;

            arParams[16] = new SqliteParameter(":CreateChildPageRoles", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createChildPageRoles;

            arParams[17] = new SqliteParameter(":ShowChildPageBreadcrumbs", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = cb;

            arParams[18] = new SqliteParameter(":HideMainMenu", DbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hm;

            arParams[19] = new SqliteParameter(":Skin", DbType.String, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new SqliteParameter(":IncludeInMenu", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = inMenu;

            arParams[21] = new SqliteParameter(":MenuImage", DbType.String, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = menuImage;

            arParams[22] = new SqliteParameter(":PageTitle", DbType.String, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = pageTitle;

            arParams[23] = new SqliteParameter(":AllowBrowserCache", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = bcache;

            arParams[24] = new SqliteParameter(":ChangeFrequency", DbType.String, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new SqliteParameter(":SiteMapPriority", DbType.String, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new SqliteParameter(":LastModifiedUTC", DbType.DateTime);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new SqliteParameter(":PageGuid", DbType.String, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = pageGuid.ToString();

            arParams[28] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = parentGuid.ToString();

            arParams[29] = new SqliteParameter(":HideAfterLogin", DbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = hideauth;

            arParams[30] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = siteGuid.ToString();

            arParams[31] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = compiledMeta;

            arParams[32] = new SqliteParameter(":CompiledMetaUtc", DbType.DateTime);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = compiledMetaUtc;

            arParams[33] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intincludeInSiteMap;

            arParams[34] = new SqliteParameter(":IsClickable", DbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intisClickable;

            arParams[35] = new SqliteParameter(":ShowHomeCrumb", DbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intshowHomeCrumb;

            arParams[36] = new SqliteParameter(":DraftEditRoles", DbType.Object);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = draftEditRoles;

            arParams[37] = new SqliteParameter(":IsPending", DbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intIsPending;

            arParams[38] = new SqliteParameter(":CanonicalOverride", DbType.String, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = canonicalOverride;

            arParams[39] = new SqliteParameter(":IncludeInSearchMap", DbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intIncludeInSearchMap;

            arParams[40] = new SqliteParameter(":EnableComments", DbType.Int32);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intenableComments;

            arParams[41] = new SqliteParameter(":CreateChildDraftRoles", DbType.Object);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = createChildDraftRoles;

            arParams[42] = new SqliteParameter(":IncludeInChildSiteMap", DbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intIncludeInChildSiteMap;

            arParams[43] = new SqliteParameter(":PubTeamId", DbType.String, 36);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubTeamId.ToString();

            arParams[44] = new SqliteParameter(":BodyCssClass", DbType.String, 50);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = bodyCssClass;

            arParams[45] = new SqliteParameter(":MenuCssClass", DbType.String, 50);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = menuCssClass;

            arParams[46] = new SqliteParameter(":ExpandOnSiteMap", DbType.Int32);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = intExpandOnSiteMap;

            arParams[47] = new SqliteParameter(":PublishMode", DbType.Int32);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = publishMode;

            arParams[48] = new SqliteParameter(":PCreatedUtc", DbType.DateTime);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new SqliteParameter(":PCreatedBy", DbType.String, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = createdBy.ToString();

            arParams[50] = new SqliteParameter(":PCreatedFromIp", DbType.String, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = createdFromIp;

            arParams[51] = new SqliteParameter(":PLastModUtc", DbType.DateTime);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = DateTime.UtcNow;

            arParams[52] = new SqliteParameter(":PLastModBy", DbType.String, 36);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = createdBy.ToString();

            arParams[53] = new SqliteParameter(":PLastModFromIp", DbType.String, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = createdFromIp;

            arParams[54] = new SqliteParameter(":MenuDesc", DbType.Object);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = menuDescription;

            arParams[55] = new SqliteParameter(":DraftApprovalRoles", DbType.Object);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = draftApprovalRoles;

            arParams[56] = new SqliteParameter(":LinkRel", DbType.String, 20);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = linkRel;

            arParams[57] = new SqliteParameter(":PageHeading", DbType.String, 255);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = pageHeading;

            arParams[58] = new SqliteParameter(":ShowPageHeading", DbType.Int32);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = intShowPageHeading;

            arParams[59] = new SqliteParameter(":PubDateUtc", DbType.DateTime);
            arParams[59].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[59].Value = DBNull.Value;
            }
            else
            {
                arParams[59].Value = pubDateUtc;
            }


            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

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

            #region byte conversion

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

            int intExpandOnSiteMap = 0;
            if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
            }

            int intincludeInSiteMap = 0;
            if (includeInSiteMap)
            {
                intincludeInSiteMap = 1;
            }

            int intisClickable = 0;
            if (isClickable)
            {
                intisClickable = 1;
            }

            int intshowHomeCrumb = 0;
            if (showHomeCrumb)
            {
                intshowHomeCrumb = 1;
            }

            byte hideauth;
            if (hideAfterLogin)
            {
                hideauth = 1;
            }
            else
            {
                hideauth = 0;
            }

            byte ssl;
            if (requireSsl)
            {
                ssl = 1;
            }
            else
            {
                ssl = 0;
            }

            byte show;
            if (showBreadcrumbs)
            {
                show = 1;
            }
            else
            {
                show = 0;
            }

            byte u;
            if (useUrl)
            {
                u = 1;
            }
            else
            {
                u = 0;
            }

            byte nw;
            if (openInNewWindow)
            {
                nw = 1;
            }
            else
            {
                nw = 0;
            }

            byte cm;
            if (showChildPageMenu)
            {
                cm = 1;
            }
            else
            {
                cm = 0;
            }

            byte cb;
            if (showChildPageBreadcrumbs)
            {
                cb = 1;
            }
            else
            {
                cb = 0;
            }

            byte hm;
            if (hideMainMenu)
            {
                hm = 1;
            }
            else
            {
                hm = 0;
            }

            byte inMenu;
            if (includeInMenu)
            {
                inMenu = 1;
            }
            else
            {
                inMenu = 0;
            }

            byte bcache;
            if (allowBrowserCache)
            {
                bcache = 1;
            }
            else
            {
                bcache = 0;
            }

            int intIncludeInChildSiteMap = 0;
            if (includeInChildSiteMap)
            {
                intIncludeInChildSiteMap = 1;
            }

            int intShowPageHeading = 0;
            if (showPageHeading)
            {
                intShowPageHeading = 1;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET PageOrder = :PageOrder , ");
            sqlCommand.Append("ParentID = :ParentID,  ");
            sqlCommand.Append("PageName = :PageName  , ");
            sqlCommand.Append("PageTitle = :PageTitle  , ");
            sqlCommand.Append("AuthorizedRoles = :AuthorizedRoles  , ");
            sqlCommand.Append("EditRoles = :EditRoles  , ");
            sqlCommand.Append("DraftEditRoles = :DraftEditRoles  , ");
            sqlCommand.Append("DraftApprovalRoles = :DraftApprovalRoles  , ");
            sqlCommand.Append("CreateChildPageRoles = :CreateChildPageRoles  , ");
            sqlCommand.Append("CreateChildDraftRoles = :CreateChildDraftRoles  , ");

            sqlCommand.Append("RequireSSL = :RequireSSL , ");
            sqlCommand.Append("AllowBrowserCache = :AllowBrowserCache , ");
            sqlCommand.Append("ShowBreadcrumbs = :ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords = :PageKeyWords , ");
            sqlCommand.Append("PageDescription = :PageDescription , ");
            sqlCommand.Append("PageEncoding = :PageEncoding , ");
            sqlCommand.Append("AdditionalMetaTags = :AdditionalMetaTags,  ");
            sqlCommand.Append("UseUrl = :UseUrl,  ");
            sqlCommand.Append("Url = :Url,  ");
            sqlCommand.Append("OpenInNewWindow = :OpenInNewWindow,  ");
            sqlCommand.Append("ShowChildPageMenu = :ShowChildPageMenu,  ");
            sqlCommand.Append("ShowChildBreadcrumbs = :ShowChildPageBreadcrumbs,  ");
            sqlCommand.Append("HideMainMenu = :HideMainMenu,  ");
            sqlCommand.Append("Skin = :Skin,  ");
            sqlCommand.Append("MenuImage = :MenuImage,  ");
            sqlCommand.Append("IncludeInMenu = :IncludeInMenu,  ");
            sqlCommand.Append("ChangeFrequency = :ChangeFrequency,  ");
            sqlCommand.Append("SiteMapPriority = :SiteMapPriority,  ");
            sqlCommand.Append("LastModifiedUTC = :LastModifiedUTC,  ");
            sqlCommand.Append("ParentGuid = :ParentGuid,  ");
            sqlCommand.Append("HideAfterLogin = :HideAfterLogin, ");

            sqlCommand.Append("CanonicalOverride = :CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap = :IncludeInSearchMap, ");
            sqlCommand.Append("EnableComments = :EnableComments, ");

            sqlCommand.Append("IncludeInChildSiteMap = :IncludeInChildSiteMap, ");
            sqlCommand.Append("PubTeamId = :PubTeamId, ");
            sqlCommand.Append("PublishMode = :PublishMode, ");
            sqlCommand.Append("BodyCssClass = :BodyCssClass, ");
            sqlCommand.Append("MenuCssClass = :MenuCssClass, ");

            sqlCommand.Append("IncludeInSiteMap = :IncludeInSiteMap, ");
            sqlCommand.Append("ExpandOnSiteMap = :ExpandOnSiteMap, ");

            sqlCommand.Append("IsClickable = :IsClickable, ");
            sqlCommand.Append("IsPending = :IsPending, ");
            sqlCommand.Append("ShowHomeCrumb = :ShowHomeCrumb, ");
            sqlCommand.Append("CompiledMeta = :CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc = :CompiledMetaUtc, ");
            sqlCommand.Append("MenuDesc = :MenuDesc, ");

            sqlCommand.Append("LinkRel = :LinkRel, ");
            sqlCommand.Append("PageHeading = :PageHeading, ");
            sqlCommand.Append("ShowPageHeading = :ShowPageHeading, ");
            sqlCommand.Append("PubDateUtc = :PubDateUtc, ");

            sqlCommand.Append("PCreatedUtc = :PCreatedUtc, ");
            sqlCommand.Append("PCreatedBy = :PCreatedBy, ");
            sqlCommand.Append("PLastModUtc = :PLastModUtc, ");
            sqlCommand.Append("PLastModBy = :PLastModBy, ");
            sqlCommand.Append("PLastModFromIp = :PLastModFromIp "); 

            sqlCommand.Append("WHERE PageID = :PageID ;");

            SqliteParameter[] arParams = new SqliteParameter[57];

            arParams[0] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new SqliteParameter(":PageName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageName;

            arParams[3] = new SqliteParameter(":PageOrder", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new SqliteParameter(":AuthorizedRoles", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = authorizedRoles;

            arParams[5] = new SqliteParameter(":PageKeyWords", DbType.String, 1000);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageKeyWords;

            arParams[6] = new SqliteParameter(":PageDescription", DbType.String, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = pageDescription;

            arParams[7] = new SqliteParameter(":PageEncoding", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageEncoding;

            arParams[8] = new SqliteParameter(":AdditionalMetaTags", DbType.String, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = additionalMetaTags;

            arParams[9] = new SqliteParameter(":RequireSSL", DbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ssl;

            arParams[10] = new SqliteParameter(":ShowBreadcrumbs", DbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = show;

            arParams[11] = new SqliteParameter(":UseUrl", DbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = u;

            arParams[12] = new SqliteParameter(":Url", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new SqliteParameter(":OpenInNewWindow", DbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = nw;

            arParams[14] = new SqliteParameter(":ShowChildPageMenu", DbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = cm;

            arParams[15] = new SqliteParameter(":EditRoles", DbType.Object);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = editRoles;

            arParams[16] = new SqliteParameter(":CreateChildPageRoles", DbType.Object);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createChildPageRoles;

            arParams[17] = new SqliteParameter(":ShowChildPageBreadcrumbs", DbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = cb;

            arParams[18] = new SqliteParameter(":HideMainMenu", DbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hm;

            arParams[19] = new SqliteParameter(":Skin", DbType.String, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new SqliteParameter(":IncludeInMenu", DbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = inMenu;

            arParams[21] = new SqliteParameter(":MenuImage", DbType.String, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = menuImage;

            arParams[22] = new SqliteParameter(":PageTitle", DbType.String, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = pageTitle;

            arParams[23] = new SqliteParameter(":AllowBrowserCache", DbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = bcache;

            arParams[24] = new SqliteParameter(":ChangeFrequency", DbType.String, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new SqliteParameter(":SiteMapPriority", DbType.String, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new SqliteParameter(":LastModifiedUTC", DbType.DateTime);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = parentGuid.ToString();

            arParams[28] = new SqliteParameter(":HideAfterLogin", DbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = hideauth;

            arParams[29] = new SqliteParameter(":CompiledMeta", DbType.Object);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = compiledMeta;

            arParams[30] = new SqliteParameter(":CompiledMetaUtc", DbType.DateTime);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = compiledMetaUtc;

            arParams[31] = new SqliteParameter(":IncludeInSiteMap", DbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intincludeInSiteMap;

            arParams[32] = new SqliteParameter(":IsClickable", DbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intisClickable;

            arParams[33] = new SqliteParameter(":ShowHomeCrumb", DbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intshowHomeCrumb;

            arParams[34] = new SqliteParameter(":DraftEditRoles", DbType.Object);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = draftEditRoles;

            arParams[35] = new SqliteParameter(":IsPending", DbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intIsPending;

            arParams[36] = new SqliteParameter(":CanonicalOverride", DbType.String, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = canonicalOverride;

            arParams[37] = new SqliteParameter(":IncludeInSearchMap", DbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intIncludeInSearchMap;

            arParams[38] = new SqliteParameter(":EnableComments", DbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intenableComments;

            arParams[39] = new SqliteParameter(":CreateChildDraftRoles", DbType.Object);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = createChildDraftRoles;

            arParams[40] = new SqliteParameter(":IncludeInChildSiteMap", DbType.Int32);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intIncludeInChildSiteMap;

            arParams[41] = new SqliteParameter(":PubTeamId", DbType.String, 36);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubTeamId.ToString();

            arParams[42] = new SqliteParameter(":BodyCssClass", DbType.String, 50);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = bodyCssClass;

            arParams[43] = new SqliteParameter(":MenuCssClass", DbType.String, 50);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = menuCssClass;

            arParams[44] = new SqliteParameter(":ExpandOnSiteMap", DbType.Int32);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = intExpandOnSiteMap;

            arParams[45] = new SqliteParameter(":PublishMode", DbType.Int32);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = publishMode;


            arParams[46] = new SqliteParameter(":PCreatedUtc", DbType.DateTime);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = createdUtc;

            arParams[47] = new SqliteParameter(":PCreatedBy", DbType.String, 36);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = createdBy.ToString();

            arParams[48] = new SqliteParameter(":PLastModUtc", DbType.DateTime);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new SqliteParameter(":PLastModBy", DbType.String, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = lastModBy.ToString();

            arParams[50] = new SqliteParameter(":PLastModFromIp", DbType.String, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = lastModFromIp;

            arParams[51] = new SqliteParameter(":MenuDesc", DbType.Object);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = menuDescription;

            arParams[52] = new SqliteParameter(":DraftApprovalRoles", DbType.Object);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = draftApprovalRoles;

            arParams[53] = new SqliteParameter(":LinkRel", DbType.String, 20);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = linkRel;

            arParams[54] = new SqliteParameter(":PageHeading", DbType.String, 255);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = pageHeading;

            arParams[55] = new SqliteParameter(":ShowPageHeading", DbType.Int32);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = intShowPageHeading;

            arParams[56] = new SqliteParameter(":PubDateUtc", DbType.DateTime);
            arParams[56].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[56].Value = DBNull.Value;
            }
            else
            {
                arParams[56].Value = pubDateUtc;
            }
            


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);



        }

        public static int GetNextPageOrder(
            int siteId,
            int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(MAX(PageOrder),-1) ");
            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE SiteID = :SiteID AND ParentID = :ParentID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            int nextPageOrder = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams)) + 2;
            if (nextPageOrder == 1)
            {
                nextPageOrder = 3;
            }

            return nextPageOrder;

        }

        public static IDataReader GetPageList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE SiteID = :SiteID ");

            sqlCommand.Append("ORDER BY	ParentID, PageOrder, PageName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE SiteID = :SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(ParentID = :ParentID OR :ParentID = -2) ");

            sqlCommand.Append("ORDER BY	PageName ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT p.*, ");

            sqlCommand.Append("u1.Name As CreatedByName, ");
            sqlCommand.Append("u1.Email As CreatedByEmail, ");
            sqlCommand.Append("u1.FirstName As CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName As CreatedByLastName, ");
            sqlCommand.Append("u2.Name As LastModByName, ");
            sqlCommand.Append("u2.Email As LastModByEmail, ");
            sqlCommand.Append("u2.FirstName As LastModByFirstName, ");
            sqlCommand.Append("u2.LastName As LastModByLastName ");

            sqlCommand.Append("FROM	mp_Pages p ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u1 ");
            sqlCommand.Append("ON p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE p.PageGuid = :PageGuid ");
            sqlCommand.Append("LIMIT 1 ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageGuid", DbType.StringFixedLength, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT p.*, ");

            sqlCommand.Append("u1.Name As CreatedByName, ");
            sqlCommand.Append("u1.Email As CreatedByEmail, ");
            sqlCommand.Append("u1.FirstName As CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName As CreatedByLastName, ");
            sqlCommand.Append("u2.Name As LastModByName, ");
            sqlCommand.Append("u2.Email As LastModByEmail, ");
            sqlCommand.Append("u2.FirstName As LastModByFirstName, ");
            sqlCommand.Append("u2.LastName As LastModByLastName ");

            sqlCommand.Append("FROM	mp_Pages p ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u1 ");
            sqlCommand.Append("ON p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE (p.PageID = :PageID OR :PageID = -1)  ");
            sqlCommand.Append("AND p.SiteID = :SiteID  ");
            sqlCommand.Append("ORDER BY p.ParentID, p.PageOrder  ");
            sqlCommand.Append("LIMIT 1 ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE ParentID = :ParentPageID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SiteID = :SiteID ");
            sqlCommand.Append("ORDER BY PageOrder; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":ParentPageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentPageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

       

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET LastModifiedUTC = :LastModifiedUTC  ");
            sqlCommand.Append("WHERE PageID = :PageID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqliteParameter(":LastModifiedUTC", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModifiedUtc;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET PageOrder = :PageOrder  ");
            sqlCommand.Append("WHERE PageID = :PageID ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqliteParameter(":PageOrder", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeletePage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Pages ");
            sqlCommand.Append("WHERE PageID = :PageID ; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool CleanupOrphans()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET ParentID = -1, ParentGuid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("WHERE ParentID <> -1 AND ParentID NOT IN (SELECT PageID FROM mp_Pages) ");
            sqlCommand.Append(";");

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            return (rowsAffected > 0);

        }


        public static int GetPendingCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE SiteGuid = :SiteGuid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsPending = 1 ");
            
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

            sqlCommand.Append("	p.*, ");
            sqlCommand.Append("COALESCE(wip.Total,0) as WipCount ");

            sqlCommand.Append("FROM	mp_Pages p  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("(");

            sqlCommand.Append("SELECT Count(*) as Total, ");
            sqlCommand.Append("pm.PageGuid as PageGuid ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_PageModules pm ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_ContentWorkflow cw ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.ModuleGuid = pm.ModuleGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.Status Not In ('Cancelled','Approved') ");
            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("pm.PageGuid ");

            sqlCommand.Append(") wip ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("wip.PageGuid = p.PageGuid ");

            sqlCommand.Append("WHERE p.SiteGuid = :SiteGuid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("p.IsPending = 1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("p.PageName ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(int siteId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (:IncludePending = 1)) ");

            sqlCommand.Append(";");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":IncludePending", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountChildPages(int pageId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ParentID = :PageID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (:IncludePending = 1)) ");

            sqlCommand.Append(";");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqliteParameter(":IncludePending", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Pages  ");

            sqlCommand.Append("WHERE SiteID = :SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (:IncludePending = 1)) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ParentID, PageName ");

            sqlCommand.Append("LIMIT :PageSize ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET :OffsetRows ");
            }
            sqlCommand.Append(";");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":IncludePending", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            arParams[2] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new SqliteParameter(":OffsetRows", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
