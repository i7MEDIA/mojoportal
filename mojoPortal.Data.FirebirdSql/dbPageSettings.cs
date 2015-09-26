/// Author:					Joe Audette
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
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    
    public static class DBPageSettings
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
            DateTime pubDateUtc)
        {

            #region Bit Conversion

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }

            int intExpandOnSiteMap = 0;
            if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

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


            int inthideAfterLogin = 0;
            if (hideAfterLogin)
            {
                inthideAfterLogin = 1;
            }
            
            int intRequireSSL = 0;
            if (requireSsl)
            {
                intRequireSSL = 1;
            }
           
            int intAllowBrowserCache = 0;
            if (allowBrowserCache)
            {
                intAllowBrowserCache = 1;
            }
            
            int intShowBreadcrumbs = 0;
            if (showBreadcrumbs)
            {
                intShowBreadcrumbs = 1;
            }
            
            int intUseUrl = 0;
            if (useUrl)
            {
                intUseUrl = 1;
            }
            
            int intOpenInNewWindow = 0;
            if (openInNewWindow)
            {
                intOpenInNewWindow = 1;
            }
            

            int intShowChildPageMenu = 0;


            int intShowChildBreadCrumbs = 0;


            int intHideMainMenu = 0;
            if (hideMainMenu)
            {
                intHideMainMenu = 1;
            }
           

            int intIncludeInMenu = 0;
            if (includeInMenu)
            {
                intIncludeInMenu = 1;
            }

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
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

            FbParameter[] arParams = new FbParameter[60];

            arParams[0] = new FbParameter(":ParentID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentId;

            arParams[1] = new FbParameter(":PageOrder", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            arParams[2] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteId;

            arParams[3] = new FbParameter(":PageName", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageName;

            arParams[4] = new FbParameter(":PageTitle", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageTitle;

            arParams[5] = new FbParameter(":AuthorizedRoles", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = authorizedRoles;

            arParams[6] = new FbParameter(":EditRoles", FbDbType.VarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = editRoles;

            arParams[7] = new FbParameter(":CreateChildPageRoles", FbDbType.VarChar);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createChildPageRoles;

            arParams[8] = new FbParameter(":CreateChildDraftRoles", FbDbType.VarChar);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createChildDraftRoles;

            arParams[9] = new FbParameter(":RequireSSL", FbDbType.SmallInt);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intRequireSSL;

            arParams[10] = new FbParameter(":AllowBrowserCache", FbDbType.SmallInt);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intAllowBrowserCache;

            arParams[11] = new FbParameter(":ShowBreadcrumbs", FbDbType.SmallInt);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intShowBreadcrumbs;

            arParams[12] = new FbParameter(":PageKeyWords", FbDbType.VarChar, 1000);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = pageKeyWords;

            arParams[13] = new FbParameter(":PageDescription", FbDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = pageDescription;

            arParams[14] = new FbParameter(":PageEncoding", FbDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = pageEncoding;

            arParams[15] = new FbParameter(":AdditionalMetaTags", FbDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = additionalMetaTags;

            arParams[16] = new FbParameter(":MenuImage", FbDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = menuImage;

            arParams[17] = new FbParameter(":UseUrl", FbDbType.SmallInt);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = intUseUrl;

            arParams[18] = new FbParameter(":Url", FbDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = url;

            arParams[19] = new FbParameter(":OpenInNewWindow", FbDbType.SmallInt);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intOpenInNewWindow;

            arParams[20] = new FbParameter(":ShowChildPageMenu", FbDbType.SmallInt);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = intShowChildPageMenu;

            arParams[21] = new FbParameter(":ShowChildBreadCrumbs", FbDbType.SmallInt);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = intShowChildBreadCrumbs;

            arParams[22] = new FbParameter(":Skin", FbDbType.VarChar, 100);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = skin;

            arParams[23] = new FbParameter(":HideMainMenu", FbDbType.SmallInt);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intHideMainMenu;

            arParams[24] = new FbParameter(":IncludeInMenu", FbDbType.SmallInt);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = intIncludeInMenu;

            arParams[25] = new FbParameter(":ChangeFrequency", FbDbType.VarChar, 20);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = changeFrequency;

            arParams[26] = new FbParameter(":SiteMapPriority", FbDbType.VarChar, 10);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = siteMapPriority;

            arParams[27] = new FbParameter(":LastModifiedUTC", FbDbType.TimeStamp);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = DateTime.UtcNow;

            arParams[28] = new FbParameter(":PageGuid", FbDbType.Char, 36);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = pageGuid.ToString();

            arParams[29] = new FbParameter(":ParentGuid", FbDbType.Char, 36);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = parentGuid.ToString();

            arParams[30] = new FbParameter(":HideAfterLogin", FbDbType.SmallInt);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = inthideAfterLogin;

            arParams[31] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = siteGuid.ToString();

            arParams[32] = new FbParameter(":CompiledMeta", FbDbType.VarChar);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = compiledMeta;

            arParams[33] = new FbParameter(":CompiledMetaUtc", FbDbType.TimeStamp);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = compiledMetaUtc;

            arParams[34] = new FbParameter(":IncludeInSiteMap", FbDbType.SmallInt);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intincludeInSiteMap;

            arParams[35] = new FbParameter(":IsClickable", FbDbType.SmallInt);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intisClickable;

            arParams[36] = new FbParameter(":ShowHomeCrumb", FbDbType.SmallInt);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intshowHomeCrumb;

            arParams[37] = new FbParameter(":DraftEditRoles", FbDbType.VarChar);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = draftEditRoles;

            arParams[38] = new FbParameter(":IsPending", FbDbType.SmallInt);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intIsPending;

            arParams[39] = new FbParameter(":CanonicalOverride", FbDbType.VarChar, 255);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = canonicalOverride;

            arParams[40] = new FbParameter(":IncludeInSearchMap", FbDbType.SmallInt);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intIncludeInSearchMap;

            arParams[41] = new FbParameter(":EnableComments", FbDbType.SmallInt);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = intenableComments;

            arParams[42] = new FbParameter(":IncludeInChildSiteMap", FbDbType.SmallInt);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intIncludeInChildSiteMap;

            arParams[43] = new FbParameter(":ExpandOnSiteMap", FbDbType.SmallInt);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = intExpandOnSiteMap;

            arParams[44] = new FbParameter(":PubTeamId", FbDbType.Char, 36);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = pubTeamId.ToString();

            arParams[45] = new FbParameter(":BodyCssClass", FbDbType.VarChar, 50);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = bodyCssClass;

            arParams[46] = new FbParameter(":MenuCssClass", FbDbType.VarChar, 50);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = menuCssClass;

            arParams[47] = new FbParameter(":PublishMode", FbDbType.Integer);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = publishMode;


            arParams[48] = new FbParameter(":PCreatedUtc", FbDbType.TimeStamp);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new FbParameter(":PCreatedBy", FbDbType.Char, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = createdBy;

            arParams[50] = new FbParameter(":PCreatedFromIp", FbDbType.VarChar, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = createdFromIp;

            arParams[51] = new FbParameter(":PLastModUtc", FbDbType.TimeStamp);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = DateTime.UtcNow;

            arParams[52] = new FbParameter(":PLastModBy", FbDbType.Char, 36);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = createdBy;

            arParams[53] = new FbParameter(":PLastModFromIp", FbDbType.VarChar, 36);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = createdFromIp;

            arParams[54] = new FbParameter(":MenuDesc", FbDbType.VarChar);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = menuDescription;

            arParams[55] = new FbParameter(":DraftApprovalRoles", FbDbType.VarChar);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = draftApprovalRoles;

            arParams[56] = new FbParameter(":LinkRel", FbDbType.VarChar, 20);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = linkRel;

            arParams[57] = new FbParameter(":PageHeading", FbDbType.VarChar, 255);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = pageHeading;

            arParams[58] = new FbParameter(":ShowPageHeading", FbDbType.Integer);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = intShowPageHeading;

            arParams[59] = new FbParameter(":PubDateUtc", FbDbType.TimeStamp);
            arParams[59].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[59].Value = DBNull.Value;
            }
            else
            {
                arParams[59].Value = pubDateUtc;
            }
            


            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_PAGES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
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
            string menuImage,
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

            #region bit conversion

            int intIncludeInSearchMap = 0;
            if (includeInSearchMap) { intIncludeInSearchMap = 1; }

            int intExpandOnSiteMap = 0;
            if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

            int intenableComments = 0;
            if (enableComments) { intenableComments = 1; }


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

            int inthideAfterLogin;
            if (hideAfterLogin)
            {
                inthideAfterLogin = 1;
            }
            else
            {
                inthideAfterLogin = 0;
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

            int intIsPending = 0;
            if (isPending)
            {
                intIsPending = 1;
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
            sqlCommand.Append("SET PageOrder = @PageOrder , ");
            sqlCommand.Append("ParentID = @ParentID,  ");
            sqlCommand.Append("PageName = @PageName  , ");
            sqlCommand.Append("PageTitle = @PageTitle  , ");
            sqlCommand.Append("AuthorizedRoles = @AuthorizedRoles  , ");

            sqlCommand.Append("EditRoles = @EditRoles  , ");
            sqlCommand.Append("DraftEditRoles = @DraftEditRoles  , ");
            sqlCommand.Append("DraftApprovalRoles = @DraftApprovalRoles  , ");
            sqlCommand.Append("CreateChildPageRoles = @CreateChildPageRoles  , ");
            sqlCommand.Append("CreateChildDraftRoles = @CreateChildDraftRoles  , ");

            sqlCommand.Append("RequireSSL = @RequireSSL , ");
            sqlCommand.Append("AllowBrowserCache = @AllowBrowserCache , ");
            sqlCommand.Append("ShowBreadcrumbs = @ShowBreadcrumbs, ");
            sqlCommand.Append("PageKeyWords = @PageKeyWords , ");
            sqlCommand.Append("PageDescription = @PageDescription , ");
            sqlCommand.Append("PageEncoding = @PageEncoding , ");
            sqlCommand.Append("AdditionalMetaTags = @AdditionalMetaTags,  ");
            sqlCommand.Append("UseUrl = @UseUrl,  ");
            sqlCommand.Append("Url = @Url,  ");
            sqlCommand.Append("OpenInNewWindow = @OpenInNewWindow,  ");
            sqlCommand.Append("ShowChildPageMenu = @ShowChildPageMenu,  ");
            sqlCommand.Append("ShowChildBreadcrumbs = @ShowChildPageBreadcrumbs,  ");
            sqlCommand.Append("HideMainMenu = @HideMainMenu,  ");
            sqlCommand.Append("Skin = @Skin,  ");
            sqlCommand.Append("MenuImage = @MenuImage,  ");
            sqlCommand.Append("IncludeInMenu = @IncludeInMenu,  ");
            sqlCommand.Append("ChangeFrequency = @ChangeFrequency,  ");
            sqlCommand.Append("SiteMapPriority = @SiteMapPriority,  ");
            sqlCommand.Append("LastModifiedUTC = @LastModifiedUTC,  ");
            sqlCommand.Append("ParentGuid = @ParentGuid,  ");
            sqlCommand.Append("HideAfterLogin = @HideAfterLogin,  ");

            sqlCommand.Append("CanonicalOverride = @CanonicalOverride,  ");
            sqlCommand.Append("IncludeInSearchMap = @IncludeInSearchMap,  ");
            sqlCommand.Append("EnableComments = @EnableComments,  ");

            sqlCommand.Append("IncludeInSiteMap = @IncludeInSiteMap,  ");
            sqlCommand.Append("IsClickable = @IsClickable,  ");
            sqlCommand.Append("ShowHomeCrumb = @ShowHomeCrumb,  ");

            sqlCommand.Append("IsPending = @IsPending,  ");
            sqlCommand.Append("IncludeInChildSiteMap = @IncludeInChildSiteMap,  ");
            sqlCommand.Append("ExpandOnSiteMap = @ExpandOnSiteMap,  ");
            sqlCommand.Append("PubTeamId = @PubTeamId,  ");

            sqlCommand.Append("PublishMode = @PublishMode,  ");

            sqlCommand.Append("BodyCssClass = @BodyCssClass,  ");
            sqlCommand.Append("MenuCssClass = @MenuCssClass,  ");

            sqlCommand.Append("CompiledMeta = @CompiledMeta, ");
            sqlCommand.Append("CompiledMetaUtc = @CompiledMetaUtc, ");
            sqlCommand.Append("MenuDesc = @MenuDesc, ");

            sqlCommand.Append("LinkRel = @LinkRel, ");
            sqlCommand.Append("PageHeading = @PageHeading, ");
            sqlCommand.Append("ShowPageHeading = @ShowPageHeading, ");
            sqlCommand.Append("PubDateUtc = @PubDateUtc, ");

            sqlCommand.Append("PCreatedUtc = @PCreatedUtc,  ");
            sqlCommand.Append("PCreatedBy = @PCreatedBy,  ");
            sqlCommand.Append("PLastModUtc = @PLastModUtc,  ");
            sqlCommand.Append("PLastModBy = @PLastModBy, ");
            sqlCommand.Append("PLastModFromIp = @PLastModFromIp ");



            sqlCommand.Append("WHERE PageID = @PageID ;");

            FbParameter[] arParams = new FbParameter[57];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@ParentID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            arParams[2] = new FbParameter("@PageName", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageName;

            arParams[3] = new FbParameter("@PageOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new FbParameter("@AuthorizedRoles", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = authorizedRoles;

            arParams[5] = new FbParameter("@PageKeyWords", FbDbType.VarChar,1000);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageKeyWords;

            arParams[6] = new FbParameter("@PageDescription", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = pageDescription;

            arParams[7] = new FbParameter("@PageEncoding", FbDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = pageEncoding;

            arParams[8] = new FbParameter("@AdditionalMetaTags", FbDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = additionalMetaTags;

            arParams[9] = new FbParameter("@RequireSSL", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ssl;

            arParams[10] = new FbParameter("@ShowBreadcrumbs", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = show;

            arParams[11] = new FbParameter("@UseUrl", FbDbType.Integer);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = u;

            arParams[12] = new FbParameter("@Url", FbDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = url;

            arParams[13] = new FbParameter("@OpenInNewWindow", FbDbType.Integer);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = nw;

            arParams[14] = new FbParameter("@ShowChildPageMenu", FbDbType.Integer);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = cm;

            arParams[15] = new FbParameter("@EditRoles", FbDbType.VarChar);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = editRoles;

            arParams[16] = new FbParameter("@CreateChildPageRoles", FbDbType.VarChar);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createChildPageRoles;

            arParams[17] = new FbParameter("@ShowChildPageBreadcrumbs", FbDbType.Integer);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = cb;

            arParams[18] = new FbParameter("@HideMainMenu", FbDbType.Integer);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hm;

            arParams[19] = new FbParameter("@Skin", FbDbType.VarChar, 100);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = skin;

            arParams[20] = new FbParameter("@IncludeInMenu", FbDbType.Integer);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = inMenu;

            arParams[21] = new FbParameter("@MenuImage", FbDbType.VarChar, 50);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = menuImage;

            arParams[22] = new FbParameter("@PageTitle", FbDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = pageTitle;

            arParams[23] = new FbParameter("@AllowBrowserCache", FbDbType.SmallInt);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = allowBrowserCache;

            arParams[24] = new FbParameter("@ChangeFrequency", FbDbType.VarChar, 20);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = changeFrequency;

            arParams[25] = new FbParameter("@SiteMapPriority", FbDbType.VarChar, 10);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = siteMapPriority;

            arParams[26] = new FbParameter("@LastModifiedUTC", FbDbType.TimeStamp);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = DateTime.UtcNow;

            arParams[27] = new FbParameter("@ParentGuid", FbDbType.Char, 36);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = parentGuid.ToString();

            arParams[28] = new FbParameter("@HideAfterLogin", FbDbType.SmallInt);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = inthideAfterLogin;

            arParams[29] = new FbParameter("@CompiledMeta", FbDbType.VarChar);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = compiledMeta;

            arParams[30] = new FbParameter("@CompiledMetaUtc", FbDbType.TimeStamp);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = compiledMetaUtc;

            arParams[31] = new FbParameter("@IncludeInSiteMap", FbDbType.SmallInt);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intincludeInSiteMap;

            arParams[32] = new FbParameter("@IsClickable", FbDbType.SmallInt);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intisClickable;

            arParams[33] = new FbParameter("@ShowHomeCrumb", FbDbType.SmallInt);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intshowHomeCrumb;

            arParams[34] = new FbParameter("@DraftEditRoles", FbDbType.VarChar);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = draftEditRoles;

            arParams[35] = new FbParameter("@IsPending", FbDbType.SmallInt);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intIsPending;

            arParams[36] = new FbParameter("@CanonicalOverride", FbDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = canonicalOverride;

            arParams[37] = new FbParameter("@IncludeInSearchMap", FbDbType.SmallInt);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intIncludeInSearchMap;

            arParams[38] = new FbParameter("@EnableComments", FbDbType.SmallInt);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intenableComments;

            arParams[39] = new FbParameter("@CreateChildDraftRoles", FbDbType.VarChar);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = createChildDraftRoles;

            arParams[40] = new FbParameter("@IncludeInChildSiteMap", FbDbType.SmallInt);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intIncludeInChildSiteMap;

            arParams[41] = new FbParameter("@PubTeamId", FbDbType.Char, 36);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = pubTeamId.ToString();

            arParams[42] = new FbParameter("@BodyCssClass", FbDbType.VarChar, 50);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = bodyCssClass;

            arParams[43] = new FbParameter("@MenuCssClass", FbDbType.VarChar, 50);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = menuCssClass;

            arParams[44] = new FbParameter("@ExpandOnSiteMap", FbDbType.SmallInt);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = intExpandOnSiteMap;

            arParams[45] = new FbParameter("@PublishMode", FbDbType.Integer);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = publishMode;

            arParams[46] = new FbParameter("@PCreatedUtc", FbDbType.TimeStamp);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = createdUtc;

            arParams[47] = new FbParameter("@PCreatedBy", FbDbType.Char, 36);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = createdBy;

            
            arParams[48] = new FbParameter("@PLastModUtc", FbDbType.TimeStamp);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = DateTime.UtcNow;

            arParams[49] = new FbParameter("@PLastModBy", FbDbType.Char, 36);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = lastModBy;

            arParams[50] = new FbParameter("@PLastModFromIp", FbDbType.VarChar, 36);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = lastModFromIp;

            arParams[51] = new FbParameter("@MenuDesc", FbDbType.VarChar);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = menuDescription;

            arParams[52] = new FbParameter("@DraftApprovalRoles", FbDbType.VarChar);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = draftApprovalRoles;

            arParams[53] = new FbParameter("@LinkRel", FbDbType.VarChar, 20);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = linkRel;

            arParams[54] = new FbParameter("@PageHeading", FbDbType.VarChar, 255);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = pageHeading;

            arParams[55] = new FbParameter("@ShowPageHeading", FbDbType.Integer);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = intShowPageHeading;

            arParams[56] = new FbParameter("@PubDateUtc", FbDbType.TimeStamp);
            arParams[56].Direction = ParameterDirection.Input;
            if (pubDateUtc == DateTime.MaxValue)
            {
                arParams[56].Value = DBNull.Value;
            }
            else
            {
                arParams[56].Value = pubDateUtc;
            }
            

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            sqlCommand.Append("WHERE SiteID = @SiteID AND ParentID = @ParentID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ParentID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            int nextPageOrder = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams)) + 2;
            if (nextPageOrder == 1)
            {
                nextPageOrder = 3;
            }

            return nextPageOrder;

        }

        public static IDataReader GetPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1 p.*, ");

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
            sqlCommand.Append("ON	p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON	p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE p.PageGuid = @PageGuid   ");

            sqlCommand.Append("  ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPage(int siteId, int pageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1 p.*, ");

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
            sqlCommand.Append("ON	p.PCreatedBy = u1.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u2 ");
            sqlCommand.Append("ON	p.PLastModBy = u2.UserGuid ");

            sqlCommand.Append("WHERE (p.PageID = @PageID OR @PageID = -1)  ");
            sqlCommand.Append("AND p.SiteID = @SiteID  ");

            sqlCommand.Append("ORDER BY p.ParentID, p.PageOrder  ");
            sqlCommand.Append("  ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE ParentID = @ParentPageID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("ORDER BY PageOrder; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ParentPageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentPageId;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("SET LastModifiedUTC = @LastModifiedUTC  ");
            sqlCommand.Append("WHERE PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@LastModifiedUTC", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModifiedUtc;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Pages ");
            sqlCommand.Append("SET PageOrder = @PageOrder  ");
            sqlCommand.Append("WHERE PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@PageOrder", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeletePage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Pages ");
            sqlCommand.Append("WHERE PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                null);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_Pages table.
        /// </summary>
        public static int GetPendingCount(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE SiteGuid = @SiteGuid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsPending = 1 ");
            //sqlCommand.Append("PageID IN ");
            //sqlCommand.Append("( ");
            //sqlCommand.Append("SELECT pm.PageID FROM mp_PageModules pm ");
            //sqlCommand.Append("JOIN mp_ContentWorkflow cw ");
            //sqlCommand.Append("ON cw.ModuleGuid = pm.ModuleGuid ");
            //sqlCommand.Append("WHERE Status Not In ('Cancelled','Approved') ");
            //sqlCommand.Append(")");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	p.*, ");
            sqlCommand.Append("COALESCE(wip.Total,0) as WipCount ");

            sqlCommand.Append("FROM	mp_Pages p  ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("(");

            sqlCommand.Append("SELECT Count(*) as Total, ");
            sqlCommand.Append("pm.PageGuid ");

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

            sqlCommand.Append("WHERE p.SiteGuid = @SiteGuid  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("p.IsPending = 1 ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("p.PageName ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


        public static IDataReader GetPageList(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");

            sqlCommand.Append("FROM	mp_Pages ");

            sqlCommand.Append("WHERE SiteID = @SiteID  ");

            sqlCommand.Append("ORDER BY	ParentID, PageOrder, PageName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
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

            sqlCommand.Append("WHERE  ");

            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(ParentID = @ParentID OR @ParentID = -2) ");

            sqlCommand.Append("ORDER BY	PageName ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ParentID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }


        /// <summary>
        /// Gets a count of rows in the mp_Pages table.
        /// </summary>
        public static int GetCount(int siteId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (@IncludePending = 1)) ");
           
            sqlCommand.Append(";");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@IncludePending", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountChildPages(int pageId, bool includePending)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ParentID = @PageID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (@IncludePending = 1)) ");

            sqlCommand.Append(";");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@IncludePending", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_Pages  ");

            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((IsPending = 0) OR (@IncludePending = 1)) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ParentID, PageName ");

            sqlCommand.Append("	; ");

            int intIncludePending = 0;
            if (includePending) { intIncludePending = 1; }

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@IncludePending", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIncludePending;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
