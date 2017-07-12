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
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Data;
using System.Configuration;

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_Insert", 60); 
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            sph.DefineSqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorizedRoles);
            sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@DraftApprovalRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftApprovalRoles); 
            sph.DefineSqlParameter("@CreateChildPageRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, createChildPageRoles);
            sph.DefineSqlParameter("@CreateChildDraftRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, createChildDraftRoles);
            sph.DefineSqlParameter("@RequireSSL", SqlDbType.Bit, ParameterDirection.Input, requireSsl);
            sph.DefineSqlParameter("@ShowBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showBreadcrumbs);
            sph.DefineSqlParameter("@ShowChildPageBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showChildPageBreadcrumbs);
            sph.DefineSqlParameter("@PageKeyWords", SqlDbType.NVarChar, 1000, ParameterDirection.Input, pageKeyWords);
            sph.DefineSqlParameter("@PageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageDescription);
            sph.DefineSqlParameter("@PageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageEncoding);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@UseUrl", SqlDbType.Bit, ParameterDirection.Input, useUrl);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@ShowChildPageMenu", SqlDbType.Bit, ParameterDirection.Input, showChildPageMenu);
            sph.DefineSqlParameter("@HideMainMenu", SqlDbType.Bit, ParameterDirection.Input, hideMainMenu);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@IncludeInMenu", SqlDbType.Bit, ParameterDirection.Input, includeInMenu);
            sph.DefineSqlParameter("@MenuImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, menuImage);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@AllowBrowserCache", SqlDbType.Bit, ParameterDirection.Input, allowBrowserCache);
            sph.DefineSqlParameter("@ChangeFrequency", SqlDbType.NVarChar, 20, ParameterDirection.Input, changeFrequency);
            sph.DefineSqlParameter("@SiteMapPriority", SqlDbType.NVarChar, 10, ParameterDirection.Input, siteMapPriority);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@HideAfterLogin", SqlDbType.Bit, ParameterDirection.Input, hideAfterLogin);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@CompiledMetaUtc", SqlDbType.DateTime, ParameterDirection.Input, compiledMetaUtc);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@IsClickable", SqlDbType.Bit, ParameterDirection.Input, isClickable);
            sph.DefineSqlParameter("@ShowHomeCrumb", SqlDbType.Bit, ParameterDirection.Input, showHomeCrumb);
            sph.DefineSqlParameter("@IsPending", SqlDbType.Bit, ParameterDirection.Input, isPending);
            sph.DefineSqlParameter("@CanonicalOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, canonicalOverride);
            sph.DefineSqlParameter("@IncludeInSearchMap", SqlDbType.Bit, ParameterDirection.Input, includeInSearchMap);
            sph.DefineSqlParameter("@EnableComments", SqlDbType.Bit, ParameterDirection.Input, enableComments);
            sph.DefineSqlParameter("@IncludeInChildSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInChildSiteMap);
            sph.DefineSqlParameter("@ExpandOnSiteMap", SqlDbType.Bit, ParameterDirection.Input, expandOnSiteMap);
            sph.DefineSqlParameter("@PubTeamId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pubTeamId);
            sph.DefineSqlParameter("@BodyCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, bodyCssClass);
            sph.DefineSqlParameter("@MenuCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, menuCssClass);
            sph.DefineSqlParameter("@PublishMode", SqlDbType.Int, ParameterDirection.Input, publishMode);

            sph.DefineSqlParameter("@PCreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@PCreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@PCreatedFromIp", SqlDbType.NVarChar, 36, ParameterDirection.Input, createdFromIp);

            sph.DefineSqlParameter("@PLastModUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@PLastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@PLastModFromIp", SqlDbType.NVarChar, 36, ParameterDirection.Input, createdFromIp);
            sph.DefineSqlParameter("@MenuDesc", SqlDbType.NVarChar, -1, ParameterDirection.Input, menuDescription);

            sph.DefineSqlParameter("@LinkRel", SqlDbType.NVarChar, 20, ParameterDirection.Input, linkRel);
            sph.DefineSqlParameter("@PageHeading", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageHeading);
            sph.DefineSqlParameter("@ShowPageHeading", SqlDbType.Bit, ParameterDirection.Input, showPageHeading);
            if (pubDateUtc == DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@PubDateUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@PubDateUtc", SqlDbType.DateTime, ParameterDirection.Input, pubDateUtc);
            }
            

            int newID = Convert.ToInt32(sph.ExecuteScalar());
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_Update", 58);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            sph.DefineSqlParameter("@PageName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageName);
            sph.DefineSqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorizedRoles);
            sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@DraftApprovalRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftApprovalRoles); //JOE DAVIS
            sph.DefineSqlParameter("@CreateChildPageRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, createChildPageRoles);
            sph.DefineSqlParameter("@CreateChildDraftRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, createChildDraftRoles);
            sph.DefineSqlParameter("@RequireSSL", SqlDbType.Bit, ParameterDirection.Input, requireSsl);
            sph.DefineSqlParameter("@ShowBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showBreadcrumbs);
            sph.DefineSqlParameter("@ShowChildPageBreadcrumbs", SqlDbType.Bit, ParameterDirection.Input, showChildPageBreadcrumbs);
            sph.DefineSqlParameter("@PageKeyWords", SqlDbType.NVarChar, 1000, ParameterDirection.Input, pageKeyWords);
            sph.DefineSqlParameter("@PageDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageDescription);
            sph.DefineSqlParameter("@PageEncoding", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageEncoding);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, 255, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@UseUrl", SqlDbType.Bit, ParameterDirection.Input, useUrl);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@ShowChildPageMenu", SqlDbType.Bit, ParameterDirection.Input, showChildPageMenu);
            sph.DefineSqlParameter("@HideMainMenu", SqlDbType.Bit, ParameterDirection.Input, hideMainMenu);
            sph.DefineSqlParameter("@Skin", SqlDbType.NVarChar, 100, ParameterDirection.Input, skin);
            sph.DefineSqlParameter("@IncludeInMenu", SqlDbType.Bit, ParameterDirection.Input, includeInMenu);
            sph.DefineSqlParameter("@MenuImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, menuImage);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@AllowBrowserCache", SqlDbType.Bit, ParameterDirection.Input, allowBrowserCache);
            sph.DefineSqlParameter("@ChangeFrequency", SqlDbType.NVarChar, 20, ParameterDirection.Input, changeFrequency);
            sph.DefineSqlParameter("@SiteMapPriority", SqlDbType.NVarChar, 10, ParameterDirection.Input, siteMapPriority);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@HideAfterLogin", SqlDbType.Bit, ParameterDirection.Input, hideAfterLogin);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@CompiledMetaUtc", SqlDbType.DateTime, ParameterDirection.Input, compiledMetaUtc);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@IsClickable", SqlDbType.Bit, ParameterDirection.Input, isClickable);
            sph.DefineSqlParameter("@ShowHomeCrumb", SqlDbType.Bit, ParameterDirection.Input, showHomeCrumb);
            sph.DefineSqlParameter("@IsPending", SqlDbType.Bit, ParameterDirection.Input, isPending);
            sph.DefineSqlParameter("@CanonicalOverride", SqlDbType.NVarChar, 255, ParameterDirection.Input, canonicalOverride);
            sph.DefineSqlParameter("@IncludeInSearchMap", SqlDbType.Bit, ParameterDirection.Input, includeInSearchMap);
            sph.DefineSqlParameter("@EnableComments", SqlDbType.Bit, ParameterDirection.Input, enableComments);
            sph.DefineSqlParameter("@IncludeInChildSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInChildSiteMap);
            sph.DefineSqlParameter("@ExpandOnSiteMap", SqlDbType.Bit, ParameterDirection.Input, expandOnSiteMap);
            sph.DefineSqlParameter("@PubTeamId", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pubTeamId);
            sph.DefineSqlParameter("@BodyCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, bodyCssClass);
            sph.DefineSqlParameter("@MenuCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, menuCssClass);
            sph.DefineSqlParameter("@PublishMode", SqlDbType.Int, ParameterDirection.Input, publishMode);

            sph.DefineSqlParameter("@PCreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@PCreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@PLastModUtc", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@PLastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            sph.DefineSqlParameter("@PLastModFromIp", SqlDbType.NVarChar, 36, ParameterDirection.Input, lastModFromIp);
            sph.DefineSqlParameter("@MenuDesc", SqlDbType.NVarChar, -1, ParameterDirection.Input, menuDescription);

            sph.DefineSqlParameter("@LinkRel", SqlDbType.NVarChar, 20, ParameterDirection.Input, linkRel);
            sph.DefineSqlParameter("@PageHeading", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageHeading);
            sph.DefineSqlParameter("@ShowPageHeading", SqlDbType.Bit, ParameterDirection.Input, showPageHeading);
            if (pubDateUtc == DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@PubDateUtc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@PubDateUtc", SqlDbType.DateTime, ParameterDirection.Input, pubDateUtc);
            }
            

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_UpdateTimeStamp", 2);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@LastModifiedUTC", SqlDbType.DateTime, ParameterDirection.Input, lastModifiedUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdatePageOrder(int pageId, int pageOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_UpdatePageOrder", 2);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeletePage(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_Delete", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool CleanupOrphans()
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Pages_CleanupOrphans", 0);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }



        public static IDataReader GetPage(Guid pageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectOne", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPageList(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SiteSettings_GetPageList", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectChildPagesAlpha", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            return sph.ExecuteReader();
        }

        public static int GetPendingCount(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectPendingPageCount", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPendingPageListPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectPendingPageListPage", 3);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetChildPages(int siteId, int parentPageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectChildPages", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentPageId);
            return sph.ExecuteReader();
        }

        //public static IDataReader GetBreadcrumbs(int pageId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_Pages_GetBreadcrumbs", 1);
        //    sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
        //    return sph.ExecuteReader();
        //}

        public static int GetNextPageOrder(
                int siteId,
                int parentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_GetNextPageOrder", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }

        public static int GetCount(int siteId, bool includePending)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_CountBySite", 2);

            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@IncludePending", SqlDbType.Bit, ParameterDirection.Input, includePending);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static int GetCountChildPages(int pageId, bool includePending)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_CountChildPages", 2);

            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@IncludePending", SqlDbType.Bit, ParameterDirection.Input, includePending);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageOfPages(
            int siteId, 
            bool includePending,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Pages_SelectPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@IncludePending", SqlDbType.Bit, ParameterDirection.Input, includePending);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


    }
}
