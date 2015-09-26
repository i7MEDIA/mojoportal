// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2013-12-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBPageSettings
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Pages ");
            sqlCommand.Append("(");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("PageName, ");
            sqlCommand.Append("PageTitle, ");
            sqlCommand.Append("AuthorizedRoles, ");
            sqlCommand.Append("EditRoles, ");
            sqlCommand.Append("CreateChildPageRoles, ");
            sqlCommand.Append("RequireSSL, ");
            sqlCommand.Append("AllowBrowserCache, ");
            sqlCommand.Append("ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords, ");
            sqlCommand.Append("PageDescription, ");
            sqlCommand.Append("PageEncoding, ");
            sqlCommand.Append("AdditionalMetaTags, ");
            sqlCommand.Append("MenuImage, ");
            sqlCommand.Append("UseUrl, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("OpenInNewWindow, ");
            sqlCommand.Append("ShowChildPageMenu, ");
            sqlCommand.Append("ShowChildBreadCrumbs, ");
            sqlCommand.Append("Skin, ");
            sqlCommand.Append("HideMainMenu, ");
            sqlCommand.Append("IncludeInMenu, ");
            sqlCommand.Append("ChangeFrequency, ");
            sqlCommand.Append("SiteMapPriority, ");
            sqlCommand.Append("LastModifiedUTC, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("ParentGuid, ");
            sqlCommand.Append("HideAfterLogin, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc, ");
            sqlCommand.Append("IncludeInSiteMap, ");
            sqlCommand.Append("IsClickable, ");
            sqlCommand.Append("ShowHomeCrumb, ");
            sqlCommand.Append("DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles, ");
            sqlCommand.Append("IsPending, ");
            sqlCommand.Append("CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap, ");
            sqlCommand.Append("EnableComments, ");

            sqlCommand.Append("IncludeInChildSiteMap, ");
            sqlCommand.Append("ExpandOnSiteMap, ");
            sqlCommand.Append("PubTeamId, ");
            sqlCommand.Append("PublishMode, ");
            sqlCommand.Append("BodyCssClass, ");
            sqlCommand.Append("MenuCssClass, ");
            sqlCommand.Append("MenuDesc, ");
            sqlCommand.Append("CreateChildDraftRoles, ");

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

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ParentID, ");
            sqlCommand.Append("@PageOrder, ");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@PageName, ");
            sqlCommand.Append("@PageTitle, ");
            sqlCommand.Append("@AuthorizedRoles, ");
            sqlCommand.Append("@EditRoles, ");
            sqlCommand.Append("@CreateChildPageRoles, ");
            sqlCommand.Append("@RequireSSL, ");
            sqlCommand.Append("@AllowBrowserCache, ");
            sqlCommand.Append("@ShowBreadcrumbs, ");
            sqlCommand.Append("@PageKeyWords, ");
            sqlCommand.Append("@PageDescription, ");
            sqlCommand.Append("@PageEncoding, ");
            sqlCommand.Append("@AdditionalMetaTags, ");
            sqlCommand.Append("@MenuImage, ");
            sqlCommand.Append("@UseUrl, ");
            sqlCommand.Append("@Url, ");
            sqlCommand.Append("@OpenInNewWindow, ");
            sqlCommand.Append("@ShowChildPageMenu, ");
            sqlCommand.Append("@ShowChildBreadCrumbs, ");
            sqlCommand.Append("@Skin, ");
            sqlCommand.Append("@HideMainMenu, ");
            sqlCommand.Append("@IncludeInMenu, ");
            sqlCommand.Append("@ChangeFrequency, ");
            sqlCommand.Append("@SiteMapPriority, ");
            sqlCommand.Append("@LastModifiedUTC, ");
            sqlCommand.Append("@PageGuid, ");
            sqlCommand.Append("@ParentGuid, ");
            sqlCommand.Append("@HideAfterLogin, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@CompiledMeta, ");
            sqlCommand.Append("@CompiledMetaUtc, ");
            sqlCommand.Append("@IncludeInSiteMap, ");
            sqlCommand.Append("@IsClickable, ");
            sqlCommand.Append("@ShowHomeCrumb, ");
            sqlCommand.Append("@DraftEditRoles, ");
            sqlCommand.Append("@DraftApprovalRoles, ");
            sqlCommand.Append("@IsPending, ");
            sqlCommand.Append("@CanonicalOverride, ");
            sqlCommand.Append("@IncludeInSearchMap, ");
            sqlCommand.Append("@EnableComments, ");

            sqlCommand.Append("@IncludeInChildSiteMap, ");
            sqlCommand.Append("@ExpandOnSiteMap, ");
            sqlCommand.Append("@PubTeamId, ");
            sqlCommand.Append("@PublishMode, ");
            sqlCommand.Append("@BodyCssClass, ");
            sqlCommand.Append("@MenuCssClass, ");
            sqlCommand.Append("@MenuDesc, ");
            sqlCommand.Append("@CreateChildDraftRoles, ");

            sqlCommand.Append("@LinkRel, ");
            sqlCommand.Append("@PageHeading, ");
            sqlCommand.Append("@ShowPageHeading, ");
            sqlCommand.Append("@PubDateUtc, ");

            sqlCommand.Append("@PCreatedUtc, ");
            sqlCommand.Append("@PCreatedBy, ");
            sqlCommand.Append("@PCreatedFromIp, ");
            sqlCommand.Append("@PLastModUtc, ");
            sqlCommand.Append("@PLastModBy, ");
            sqlCommand.Append("@PLastModFromIp ");


            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[60];

            arParams[0] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentId;

            arParams[1] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            arParams[2] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            arParams[3] = new SqlCeParameter("@PageName", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageName;

            arParams[4] = new SqlCeParameter("@PageTitle", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageTitle;

            arParams[5] = new SqlCeParameter("@AuthorizedRoles", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = authorizedRoles;

            arParams[6] = new SqlCeParameter("@EditRoles", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = editRoles;

            arParams[7] = new SqlCeParameter("@CreateChildPageRoles", SqlDbType.NText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createChildPageRoles;

            arParams[8] = new SqlCeParameter("@RequireSSL", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = requireSsl;

            arParams[9] = new SqlCeParameter("@AllowBrowserCache", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = allowBrowserCache;

            arParams[10] = new SqlCeParameter("@ShowBreadcrumbs", SqlDbType.Bit);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = showBreadcrumbs;

            arParams[11] = new SqlCeParameter("@PageKeyWords", SqlDbType.NVarChar, 1000);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = pageKeyWords;

            arParams[12] = new SqlCeParameter("@PageDescription", SqlDbType.NVarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = pageDescription;

            arParams[13] = new SqlCeParameter("@PageEncoding", SqlDbType.NVarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = pageEncoding;

            arParams[14] = new SqlCeParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = additionalMetaTags;

            arParams[15] = new SqlCeParameter("@MenuImage", SqlDbType.NVarChar, 50);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = menuImage;

            arParams[16] = new SqlCeParameter("@UseUrl", SqlDbType.Bit);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = useUrl;

            arParams[17] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = url;

            arParams[18] = new SqlCeParameter("@OpenInNewWindow", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = openInNewWindow;

            arParams[19] = new SqlCeParameter("@ShowChildPageMenu", SqlDbType.Bit);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = showChildPageMenu;

            arParams[20] = new SqlCeParameter("@ShowChildBreadCrumbs", SqlDbType.Bit);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = showChildPageBreadcrumbs;

            arParams[21] = new SqlCeParameter("@Skin", SqlDbType.NVarChar, 100);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = skin;

            arParams[22] = new SqlCeParameter("@HideMainMenu", SqlDbType.Bit);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = hideMainMenu;

            arParams[23] = new SqlCeParameter("@IncludeInMenu", SqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = includeInMenu;

            arParams[24] = new SqlCeParameter("@ChangeFrequency", SqlDbType.NVarChar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new SqlCeParameter("@SiteMapPriority", SqlDbType.NVarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new SqlCeParameter("@LastModifiedUTC", SqlDbType.DateTime);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = pageGuid;

            arParams[28] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = parentGuid;

            arParams[29] = new SqlCeParameter("@HideAfterLogin", SqlDbType.Bit);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = hideAfterLogin;

            arParams[30] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = siteGuid;

            arParams[31] = new SqlCeParameter("@CompiledMeta", SqlDbType.NText);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = compiledMeta;

            arParams[32] = new SqlCeParameter("@CompiledMetaUtc", SqlDbType.DateTime);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = compiledMetaUtc;

            arParams[33] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = includeInSiteMap;

            arParams[34] = new SqlCeParameter("@IsClickable", SqlDbType.Bit);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = isClickable;

            arParams[35] = new SqlCeParameter("@ShowHomeCrumb", SqlDbType.Bit);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = showHomeCrumb;

            arParams[36] = new SqlCeParameter("@DraftEditRoles", SqlDbType.NText);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = draftEditRoles;

            arParams[37] = new SqlCeParameter("@IsPending", SqlDbType.Bit);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = isPending;

            arParams[38] = new SqlCeParameter("@CanonicalOverride", SqlDbType.NVarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = canonicalOverride;

            arParams[39] = new SqlCeParameter("@IncludeInSearchMap", SqlDbType.Bit);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = includeInSearchMap;

            arParams[40] = new SqlCeParameter("@EnableComments", SqlDbType.Bit);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = enableComments;

            arParams[41] = new SqlCeParameter("@CreateChildDraftRoles", SqlDbType.NText);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = createChildDraftRoles;

            arParams[42] = new SqlCeParameter("@IncludeInChildSiteMap", SqlDbType.Bit);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = includeInChildSiteMap;

            arParams[43] = new SqlCeParameter("@PubTeamId", SqlDbType.UniqueIdentifier);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = pubTeamId;

            arParams[44] = new SqlCeParameter("@BodyCssClass", SqlDbType.NVarChar, 50);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = bodyCssClass;

            arParams[45] = new SqlCeParameter("@MenuCssClass", SqlDbType.NVarChar, 50);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = menuCssClass;

            arParams[46] = new SqlCeParameter("@ExpandOnSiteMap", SqlDbType.Bit);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = expandOnSiteMap;

            arParams[47] = new SqlCeParameter("@PublishMode", SqlDbType.Int);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = publishMode;


            arParams[48] = new SqlCeParameter("@PCreatedUtc", SqlDbType.DateTime);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new SqlCeParameter("@PCreatedBy", SqlDbType.UniqueIdentifier);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = createdBy;

            arParams[50] = new SqlCeParameter("@PCreatedFromIp", SqlDbType.NVarChar, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = createdFromIp;

            arParams[51] = new SqlCeParameter("@PLastModUtc", SqlDbType.DateTime);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = DateTime.UtcNow;

            arParams[52] = new SqlCeParameter("@PLastModBy", SqlDbType.UniqueIdentifier);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = createdBy;

            arParams[53] = new SqlCeParameter("@PLastModFromIp", SqlDbType.NVarChar, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = createdFromIp;

            arParams[54] = new SqlCeParameter("@MenuDesc", SqlDbType.NText);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = menuDescription;

            arParams[55] = new SqlCeParameter("@DraftApprovalRoles", SqlDbType.NText);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = draftApprovalRoles;

            arParams[56] = new SqlCeParameter("@LinkRel", SqlDbType.NVarChar, 20);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = linkRel;

            arParams[57] = new SqlCeParameter("@PageHeading", SqlDbType.NVarChar, 255);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = pageHeading;

            arParams[58] = new SqlCeParameter("@ShowPageHeading", SqlDbType.Bit);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = showPageHeading;

            arParams[59] = new SqlCeParameter("@PubDateUtc", SqlDbType.DateTime);
            arParams[59].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[59].Value = DBNull.Value;
            }
            else
            {
                arParams[59].Value = pubDateUtc;
            }
            


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


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
            DateTime pubDateUtc
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ParentID = @ParentID, ");
            sqlCommand.Append("PageOrder = @PageOrder, ");
            
            sqlCommand.Append("PageName = @PageName, ");
            sqlCommand.Append("PageTitle = @PageTitle, ");
            sqlCommand.Append("AuthorizedRoles = @AuthorizedRoles, ");
            sqlCommand.Append("EditRoles = @EditRoles, ");
            sqlCommand.Append("CreateChildPageRoles = @CreateChildPageRoles, ");
            sqlCommand.Append("RequireSSL = @RequireSSL, ");
            sqlCommand.Append("AllowBrowserCache = @AllowBrowserCache, ");
            sqlCommand.Append("ShowBreadcrumbs = @ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords = @PageKeyWords, ");
            sqlCommand.Append("PageDescription = @PageDescription, ");
            sqlCommand.Append("PageEncoding = @PageEncoding, ");
            sqlCommand.Append("AdditionalMetaTags = @AdditionalMetaTags, ");
            sqlCommand.Append("MenuImage = @MenuImage, ");
            sqlCommand.Append("UseUrl = @UseUrl, ");
            sqlCommand.Append("Url = @Url, ");
            sqlCommand.Append("OpenInNewWindow = @OpenInNewWindow, ");
            sqlCommand.Append("ShowChildPageMenu = @ShowChildPageMenu, ");
            sqlCommand.Append("ShowChildBreadCrumbs = @ShowChildBreadCrumbs, ");
            sqlCommand.Append("Skin = @Skin, ");
            sqlCommand.Append("HideMainMenu = @HideMainMenu, ");
            sqlCommand.Append("IncludeInMenu = @IncludeInMenu, ");
            sqlCommand.Append("ChangeFrequency = @ChangeFrequency, ");
            sqlCommand.Append("SiteMapPriority = @SiteMapPriority, ");
            sqlCommand.Append("LastModifiedUTC = @LastModifiedUTC, ");
            sqlCommand.Append("ParentGuid = @ParentGuid, ");
            sqlCommand.Append("HideAfterLogin = @HideAfterLogin, ");
            sqlCommand.Append("CompiledMeta = @CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc = @CompiledMetaUtc, ");
            sqlCommand.Append("IncludeInSiteMap = @IncludeInSiteMap, ");
            sqlCommand.Append("IsClickable = @IsClickable, ");
            sqlCommand.Append("ShowHomeCrumb = @ShowHomeCrumb, ");
            sqlCommand.Append("DraftEditRoles = @DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles = @DraftApprovalRoles, ");
            sqlCommand.Append("IsPending = @IsPending, ");
            sqlCommand.Append("CanonicalOverride = @CanonicalOverride, ");
            sqlCommand.Append("IncludeInSearchMap = @IncludeInSearchMap, ");
            sqlCommand.Append("EnableComments = @EnableComments, ");

            sqlCommand.Append("IncludeInChildSiteMap = @IncludeInChildSiteMap, ");
            sqlCommand.Append("ExpandOnSiteMap = @ExpandOnSiteMap, ");
            
            sqlCommand.Append("PubTeamId = @PubTeamId, ");
            sqlCommand.Append("PublishMode = @PublishMode, ");
            sqlCommand.Append("BodyCssClass = @BodyCssClass, ");
            sqlCommand.Append("MenuCssClass = @MenuCssClass, ");
            sqlCommand.Append("MenuDesc = @MenuDesc, ");
            sqlCommand.Append("CreateChildDraftRoles = @CreateChildDraftRoles, ");

            sqlCommand.Append("LinkRel = @LinkRel, ");
            sqlCommand.Append("PageHeading = @PageHeading, ");
            sqlCommand.Append("ShowPageHeading = @ShowPageHeading, ");
            sqlCommand.Append("PubDateUtc = @PubDateUtc, ");

            sqlCommand.Append("PCreatedUtc = @PCreatedUtc, ");
            sqlCommand.Append("PCreatedBy = @PCreatedBy, ");
            sqlCommand.Append("PLastModUtc = @PLastModUtc, ");
            sqlCommand.Append("PLastModBy = @PLastModBy, ");
            sqlCommand.Append("PLastModFromIp = @PLastModFromIp ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[58];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageOrder;

            arParams[3] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteId;

            arParams[4] = new SqlCeParameter("@PageName", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageName;

            arParams[5] = new SqlCeParameter("@PageTitle", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageTitle;

            arParams[6] = new SqlCeParameter("@AuthorizedRoles", SqlDbType.NText);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = authorizedRoles;

            arParams[7] = new SqlCeParameter("@EditRoles", SqlDbType.NText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = editRoles;

            arParams[8] = new SqlCeParameter("@CreateChildPageRoles", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createChildPageRoles;

            arParams[9] = new SqlCeParameter("@RequireSSL", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = requireSsl;

            arParams[10] = new SqlCeParameter("@AllowBrowserCache", SqlDbType.Bit);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = allowBrowserCache;

            arParams[11] = new SqlCeParameter("@ShowBreadcrumbs", SqlDbType.Bit);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = showBreadcrumbs;

            arParams[12] = new SqlCeParameter("@PageKeyWords", SqlDbType.NVarChar, 1000);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = pageKeyWords;

            arParams[13] = new SqlCeParameter("@PageDescription", SqlDbType.NVarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = pageDescription;

            arParams[14] = new SqlCeParameter("@PageEncoding", SqlDbType.NVarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pageEncoding;

            arParams[15] = new SqlCeParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = additionalMetaTags;

            arParams[16] = new SqlCeParameter("@MenuImage", SqlDbType.NVarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = menuImage;

            arParams[17] = new SqlCeParameter("@UseUrl", SqlDbType.Bit);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = useUrl;

            arParams[18] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = url;

            arParams[19] = new SqlCeParameter("@OpenInNewWindow", SqlDbType.Bit);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = openInNewWindow;

            arParams[20] = new SqlCeParameter("@ShowChildPageMenu", SqlDbType.Bit);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = showChildPageMenu;

            arParams[21] = new SqlCeParameter("@ShowChildBreadCrumbs", SqlDbType.Bit);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = showChildPageBreadcrumbs;

            arParams[22] = new SqlCeParameter("@Skin", SqlDbType.NVarChar, 100);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = skin;

            arParams[23] = new SqlCeParameter("@HideMainMenu", SqlDbType.Bit);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = hideMainMenu;

            arParams[24] = new SqlCeParameter("@IncludeInMenu", SqlDbType.Bit);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = includeInMenu;

            arParams[25] = new SqlCeParameter("@ChangeFrequency", SqlDbType.NVarChar, 20);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = changeFrequency;

            arParams[26] = new SqlCeParameter("@SiteMapPriority", SqlDbType.NVarChar, 10);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = siteMapPriority;

            arParams[27] = new SqlCeParameter("@LastModifiedUTC", SqlDbType.DateTime);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = DateTime.UtcNow;

            arParams[28] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = parentGuid;

            arParams[29] = new SqlCeParameter("@HideAfterLogin", SqlDbType.Bit);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = hideAfterLogin;

            arParams[30] = new SqlCeParameter("@CompiledMeta", SqlDbType.NText);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = compiledMeta;

            arParams[31] = new SqlCeParameter("@CompiledMetaUtc", SqlDbType.DateTime);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = compiledMetaUtc;

            arParams[32] = new SqlCeParameter("@IncludeInSiteMap", SqlDbType.Bit);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = includeInSiteMap;

            arParams[33] = new SqlCeParameter("@IsClickable", SqlDbType.Bit);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = isClickable;

            arParams[34] = new SqlCeParameter("@ShowHomeCrumb", SqlDbType.Bit);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = showHomeCrumb;

            arParams[35] = new SqlCeParameter("@DraftEditRoles", SqlDbType.NText);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = draftEditRoles;

            arParams[36] = new SqlCeParameter("@IsPending", SqlDbType.Bit);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = isPending;

            arParams[37] = new SqlCeParameter("@CanonicalOverride", SqlDbType.NVarChar, 255);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = canonicalOverride;

            arParams[38] = new SqlCeParameter("@IncludeInSearchMap", SqlDbType.Bit);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = includeInSearchMap;

            arParams[39] = new SqlCeParameter("@EnableComments", SqlDbType.Bit);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = enableComments;

            arParams[40] = new SqlCeParameter("@CreateChildDraftRoles", SqlDbType.NText);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = createChildDraftRoles;

            arParams[41] = new SqlCeParameter("@IncludeInChildSiteMap", SqlDbType.Bit);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = includeInChildSiteMap;

            arParams[42] = new SqlCeParameter("@PubTeamId", SqlDbType.UniqueIdentifier);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = pubTeamId;

            arParams[43] = new SqlCeParameter("@BodyCssClass", SqlDbType.NVarChar, 50);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = bodyCssClass;

            arParams[44] = new SqlCeParameter("@MenuCssClass", SqlDbType.NVarChar, 50);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = menuCssClass;

            arParams[45] = new SqlCeParameter("@ExpandOnSiteMap", SqlDbType.Bit);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = expandOnSiteMap;

            arParams[46] = new SqlCeParameter("@PublishMode", SqlDbType.Int);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = publishMode;


            arParams[47] = new SqlCeParameter("@PCreatedUtc", SqlDbType.DateTime);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = createdUtc;

            arParams[48] = new SqlCeParameter("@PCreatedBy", SqlDbType.UniqueIdentifier);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = createdBy;

            arParams[49] = new SqlCeParameter("@PLastModUtc", SqlDbType.DateTime);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = DateTime.UtcNow;

            arParams[50] = new SqlCeParameter("@PLastModBy", SqlDbType.UniqueIdentifier);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = lastModBy;

            arParams[51] = new SqlCeParameter("@PLastModFromIp", SqlDbType.NVarChar, 36);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = lastModFromIp;

            arParams[52] = new SqlCeParameter("@MenuDesc", SqlDbType.NText);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = menuDescription;

            arParams[53] = new SqlCeParameter("@DraftApprovalRoles", SqlDbType.NText);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = draftApprovalRoles;

            arParams[54] = new SqlCeParameter("@LinkRel", SqlDbType.NVarChar, 20);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = linkRel;

            arParams[55] = new SqlCeParameter("@PageHeading", SqlDbType.NVarChar, 255);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = pageHeading;

            arParams[56] = new SqlCeParameter("@ShowPageHeading", SqlDbType.Bit);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = showPageHeading;

            arParams[57] = new SqlCeParameter("@PubDateUtc", SqlDbType.DateTime);
            arParams[57].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[57].Value = DBNull.Value;
            }
            else
            {
                arParams[57].Value = pubDateUtc;
            }

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("LastModifiedUTC = @LastModifiedUTC ");
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@LastModifiedUTC", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModifiedUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("PageOrder = @PageOrder ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeletePage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool CleanupOrphans()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET ParentGuid = '00000000-0000-0000-0000-000000000000', ");
            sqlCommand.Append("ParentID = -1 ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ParentID <> -1 ");
            sqlCommand.Append("AND ParentID NOT IN (SELECT PageID FROM mp_Pages) ");
            sqlCommand.Append(";");

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return (rowsAffected > -1);

        }

        public static IDataReader GetPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  p.*, ");

            sqlCommand.Append("u1.[Name] As CreatedByName, ");
            sqlCommand.Append("u1.Email As CreatedByEmail, ");
            sqlCommand.Append("u1.FirstName As CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName As CreatedByLastName, ");
            sqlCommand.Append("u2.[Name] As LastModByName, ");
            sqlCommand.Append("u2.Email As LastModByEmail, ");
            sqlCommand.Append("u2.FirstName As LastModByFirstName, ");
            sqlCommand.Append("u2.LastName As LastModByLastName ");

            sqlCommand.Append("FROM	mp_Pages p ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u1 ");
            sqlCommand.Append("ON	p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON	p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("p.PageGuid = @PageGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetPageGuidFromID(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE PageID = @PageID ");
            sqlCommand.Append(" ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            Guid pageGuid = Guid.Empty;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {

                if (reader.Read())
                {
                    pageGuid = new Guid(reader["PageGuid"].ToString());
                }
            }

            return pageGuid;
        }

        public static IDataReader GetPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) p.*, ");

            sqlCommand.Append("u1.[Name] As CreatedByName, ");
            sqlCommand.Append("u1.Email As CreatedByEmail, ");
            sqlCommand.Append("u1.FirstName As CreatedByFirstName, ");
            sqlCommand.Append("u1.LastName As CreatedByLastName, ");
            sqlCommand.Append("u2.[Name] As LastModByName, ");
            sqlCommand.Append("u2.Email As LastModByEmail, ");
            sqlCommand.Append("u2.FirstName As LastModByFirstName, ");
            sqlCommand.Append("u2.LastName As LastModByLastName ");

            sqlCommand.Append("FROM	mp_Pages p ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u1 ");
            sqlCommand.Append("ON	p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON	p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("(p.PageID = @PageID OR @PageID = -1) ");
            sqlCommand.Append("AND p.SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY p.ParentID, p.PageOrder ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY ParentID, PageOrder, PageName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(ParentID = @ParentID OR @ParentID = -2) ");
            sqlCommand.Append("ORDER BY PageName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetPendingCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND IsPending = 1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ") p.*, ");
            sqlCommand.Append("COALESCE(wip.Total,0) as WipCount ");

            sqlCommand.Append("FROM	mp_Pages p  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("(SELECT	Count(*) as Total, pm.PageGuid ");
            sqlCommand.Append("FROM	mp_PageModules pm ");
            sqlCommand.Append("JOIN mp_ContentWorkflow cw ");
            sqlCommand.Append("ON cw.ModuleGuid = pm.ModuleGuid ");
            sqlCommand.Append("WHERE [Status] Not In ('Cancelled','Approved') ");
            sqlCommand.Append("GROUP BY pm.PageGuid ) as wip ");
            sqlCommand.Append("ON wip.PageGuid = p.PageGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND p.IsPending = 1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("p.PageName  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ParentID = @ParentID ");
            sqlCommand.Append("ORDER BY PageOrder ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentPageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetNextPageOrder(
                int siteId,
                int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(PageOrder), -1) + 2 ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ParentID = @ParentID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCount(int siteId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ((IsPending = 0) OR (@IncludePending = 1)) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@IncludePending", SqlDbType.Bit);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = includePending;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountChildPages(int pageId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ParentID = @PageID ");
            sqlCommand.Append("AND ((IsPending = 0) OR (@IncludePending = 1)) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@IncludePending", SqlDbType.Bit);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = includePending;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Pages  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ((IsPending = 0) OR (@IncludePending = 1)) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ParentID, PageName ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@IncludePending", SqlDbType.Bit);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = includePending;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
