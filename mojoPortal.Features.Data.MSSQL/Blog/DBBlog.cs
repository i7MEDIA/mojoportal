// Author:
// Created:       2007-11-03
// Last Modified: 2017-07-18
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
using System.Globalization;

namespace mojoPortal.Data
{

	public static class DBBlog
    {
        /// <summary>
        /// gets top 20 related posts ordered by created date desc
        /// based on categories of current post itemid
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static IDataReader GetRelatedPosts(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectRelated", 2);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

		public static IDataReader GetBlogs(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_Select", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogsForFeed(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectForFeed", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogsForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectRecentPostsForMetaWeblogApi", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogCategoriesForMetaWeblogApi(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectRecentPostCategoriesForMetaWeblogApi", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static int GetCountClosed(
            int moduleId,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCountClosed", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountClosed(moduleId, currentTime);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectClosed", 4);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetAttachmentsForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectAttachmentsForClosed", 4);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetCategoriesForClosed(
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectCategoriesForClosed", 4);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }


        public static int GetCountOfDrafts(
            int moduleId,
            Guid userGuid,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCountOfDrafts", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetPageOfDrafts(
            int moduleId,
            Guid userGuid,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountOfDrafts(moduleId, userGuid, currentTime);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectPageOfDrafts", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

		public static int GetCount(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCount", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());
          
        }

        public static IDataReader GetPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(moduleId, beginDate, currentTime);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectPage", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetAttachmentsForPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectAttachmentsForPage", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetAttachmentsForPage(
            int moduleId,
            int categoryId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectAttachmentsForPageByCategory", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetAttachmentsForPage(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetAttachmentsForPagePersian(month, year, moduleId, currentTime, pageNumber, pageSize);
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectAttachmentsForPageByMonth", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetAttachmentsForPagePersian(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectAttachmentsForPageByMonthPersian", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetCategoriesForPage(
            int moduleId,
            DateTime beginDate,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectCategoriesForPage", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountByCategory(
            int moduleId,
            int categoryId, 
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCountByCategory", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetCategoriesForPage(
            int moduleId,
            int categoryId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectCategoriesForPageByCategory", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetEntriesByCategory(
            int moduleId, 
            int categoryId, 
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountByCategory(moduleId, categoryId, currentTime);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectPageByCategory", 5);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }


        public static IDataReader GetEntriesByCategory(int moduleId, int categoryId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectByCategory", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogsForSiteMap(int siteId, DateTime currentUtcDateTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectForSiteMap", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@CurrentUtcDateTime", SqlDbType.DateTime, ParameterDirection.Input, currentUtcDateTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogsForNewsMap(int siteId, DateTime utcThresholdTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectForNewsMap", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UtcThresholdTime", SqlDbType.DateTime, ParameterDirection.Input, utcThresholdTime);
            return sph.ExecuteReader();
        }

        public static IDataReader GetDrafts(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectDrafts", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        public static IDataReader GetBlogsByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogStats(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogStats_Select", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }


        public static IDataReader GetBlogMonthArchive(int moduleId, DateTime currentTime)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetBlogMonthArchiveForPersian(moduleId, currentTime);
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectArchiveByMonth", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// By A.Samarian 2009-09-11
        /// special handling to support Persian Calendar 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetBlogMonthArchiveForPersian(int moduleId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectArchiveByMonth_Persian", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static int GetCountByMonth(
            int month,
            int year,
            int moduleId,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCountByMonth", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());

        }
        
        public static IDataReader GetBlogEntriesByMonth(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetBlogEntriesByMonthPersian(month, year, moduleId, currentTime, pageNumber, pageSize, out totalPages);
            }

            totalPages = 1;
            int totalRows = GetCountByMonth(month, year, moduleId, currentTime);

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



            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectPageByMonth", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountByMonthPersian(
            int month,
            int year,
            int moduleId,
            DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_GetCountByMonthPersian", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static IDataReader GetBlogEntriesByMonthPersian(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountByMonthPersian(month, year, moduleId, currentTime);

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



            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectPageByMonthPersian", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

    
        public static IDataReader GetCategoriesForPage(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetCategoriesForPagePersian(month, year, moduleId, currentTime, pageNumber, pageSize);
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectCategoriesForPageByMonth", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetCategoriesForPagePersian(
            int month,
            int year,
            int moduleId,
            DateTime currentTime,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blogs_SelectCategoriesForPageByMonthPersian", 6);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }


        public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId, DateTime currentTime)
        {
            if (CultureInfo.CurrentCulture.Name == "fa-IR")
            {
                return GetBlogEntriesByMonthPersian(month, year, moduleId, currentTime);
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectByMonth", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// By A.Samarian 2009-09-11
        /// special handling to support Persian Calendar 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetBlogEntriesByMonthPersian(int month, int year, int moduleId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectByMonth_Persian", 4);
            sph.DefineSqlParameter("@Month", SqlDbType.Int, ParameterDirection.Input, month);
            sph.DefineSqlParameter("@Year", SqlDbType.Int, ParameterDirection.Input, year);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        


        public static IDataReader GetSingleBlog(int itemId, DateTime currentTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Blog_SelectOne", 2);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            return sph.ExecuteReader();
        }

        public static bool DeleteBlog(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static int AddBlog(
            Guid blogGuid,
            Guid moduleGuid,
            int moduleId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid userGuid,
            DateTime createdDate,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished,
            string subTitle,
            DateTime endDate,
            bool approved,
            Guid approvedBy,
            DateTime approvedDate,
            bool showAuthorName,
            bool showAuthorAvatar,
            bool showAuthorBio,
            bool includeInSearch,
            bool useBingMap,
            string mapHeight,
            string mapWidth,
            bool showMapOptions,
            bool showZoomTool,
            bool showLocationInfo,
            bool useDrivingDirections,
            string mapType,
            int mapZoom,
            bool showDownloadLink,
            bool includeInSiteMap,
            bool excludeFromRecentContent,

            bool includeInNews,
            string pubName,
            string pubLanguage,
            string pubAccess,
            string pubGenres,
            string pubKeyWords,
            string pubGeoLocations,
            string pubStockTickers,
            string headlineImageUrl,
            bool includeImageInExcerpt,
			bool includeImageInPost
			)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_Insert", 52);

            sph.DefineSqlParameter("@BlogGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, blogGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@Heading", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NVarChar, -1, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Location", SqlDbType.NVarChar, -1, ParameterDirection.Input, location);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@IsInNewsletter", SqlDbType.Bit, ParameterDirection.Input, isInNewsletter);
            sph.DefineSqlParameter("@IncludeInFeed", SqlDbType.Bit, ParameterDirection.Input, includeInFeed);
            sph.DefineSqlParameter("@AllowCommentsForDays", SqlDbType.Int, ParameterDirection.Input, allowCommentsForDays);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@ItemUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemUrl);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);

            sph.DefineSqlParameter("@SubTitle", SqlDbType.NVarChar, 500, ParameterDirection.Input, subTitle);
            if (endDate < DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            }
            else
            {
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            sph.DefineSqlParameter("@Approved", SqlDbType.Bit, ParameterDirection.Input, approved);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);
            if (approvedDate < DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@ApprovedDate", SqlDbType.DateTime, ParameterDirection.Input, approvedDate);
            }
            else
            {
                sph.DefineSqlParameter("@ApprovedDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }


            sph.DefineSqlParameter("@ShowAuthorName", SqlDbType.Bit, ParameterDirection.Input, showAuthorName);
            sph.DefineSqlParameter("@ShowAuthorAvatar", SqlDbType.Bit, ParameterDirection.Input, showAuthorAvatar);
            sph.DefineSqlParameter("@ShowAuthorBio", SqlDbType.Bit, ParameterDirection.Input, showAuthorBio);
            sph.DefineSqlParameter("@IncludeInSearch", SqlDbType.Bit, ParameterDirection.Input, includeInSearch);
            sph.DefineSqlParameter("@UseBingMap", SqlDbType.Bit, ParameterDirection.Input, useBingMap);
            sph.DefineSqlParameter("@MapHeight", SqlDbType.NVarChar, 10, ParameterDirection.Input, mapHeight);
            sph.DefineSqlParameter("@MapWidth", SqlDbType.NVarChar, 10, ParameterDirection.Input, mapWidth);
            sph.DefineSqlParameter("@ShowMapOptions", SqlDbType.Bit, ParameterDirection.Input, showMapOptions);
            sph.DefineSqlParameter("@ShowZoomTool", SqlDbType.Bit, ParameterDirection.Input, showZoomTool);
            sph.DefineSqlParameter("@ShowLocationInfo", SqlDbType.Bit, ParameterDirection.Input, showLocationInfo);
            sph.DefineSqlParameter("@UseDrivingDirections", SqlDbType.Bit, ParameterDirection.Input, useDrivingDirections);
            sph.DefineSqlParameter("@MapType", SqlDbType.NVarChar, 20, ParameterDirection.Input, mapType);
            sph.DefineSqlParameter("@MapZoom", SqlDbType.Int, ParameterDirection.Input, mapZoom);
            sph.DefineSqlParameter("@ShowDownloadLink", SqlDbType.Bit, ParameterDirection.Input, showDownloadLink);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@ExcludeFromRecentContent", SqlDbType.Bit, ParameterDirection.Input, excludeFromRecentContent);

            sph.DefineSqlParameter("@IncludeInNews", SqlDbType.Bit, ParameterDirection.Input, includeInNews);
            sph.DefineSqlParameter("@PubName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubName);
            sph.DefineSqlParameter("@PubLanguage", SqlDbType.NVarChar, 7, ParameterDirection.Input, pubLanguage);
            sph.DefineSqlParameter("@PubAccess", SqlDbType.NVarChar, 20, ParameterDirection.Input, pubAccess);
            sph.DefineSqlParameter("@PubGenres", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubGenres);
            sph.DefineSqlParameter("@PubKeyWords", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubKeyWords);
            sph.DefineSqlParameter("@PubGeoLocations", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubGeoLocations);
            sph.DefineSqlParameter("@PubStockTickers", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubStockTickers);
            sph.DefineSqlParameter("@HeadlineImageUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, headlineImageUrl);
			sph.DefineSqlParameter("@IncludeImageInExcerpt", SqlDbType.Bit, ParameterDirection.Input, includeImageInExcerpt);
			sph.DefineSqlParameter("@IncludeImageInPost", SqlDbType.Bit, ParameterDirection.Input, includeImageInPost);

			//if you added/removed parameters, be sure to change the newID line below to match the index of the ItemID parameter
			sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.InputOutput, null);

			sph.ExecuteNonQuery();

			//if you added/removed parameters, be sure this is using the correct index for the ItemID parameter.
            int newID = Convert.ToInt32(sph.Parameters[51].Value);
            return newID;
        }

        public static bool UpdateBlog(
            int moduleId,
            int itemId,
            string userName,
            string title,
            string excerpt,
            string description,
            DateTime startDate,
            bool isInNewsletter,
            bool includeInFeed,
            int allowCommentsForDays,
            string location,
            Guid lastModUserGuid,
            DateTime lastModUtc,
            string itemUrl,
            string metaKeywords,
            string metaDescription,
            string compiledMeta,
            bool isPublished,
            string subTitle,
            DateTime endDate,
            bool approved,
            Guid approvedBy,
            DateTime approvedDate,
            bool showAuthorName,
            bool showAuthorAvatar,
            bool showAuthorBio,
            bool includeInSearch,
            bool useBingMap,
            string mapHeight,
            string mapWidth,
            bool showMapOptions,
            bool showZoomTool,
            bool showLocationInfo,
            bool useDrivingDirections,
            string mapType,
            int mapZoom,
            bool showDownloadLink,
            bool includeInSiteMap,
            bool excludeFromRecentContent,

            bool includeInNews,
            string pubName,
            string pubLanguage,
            string pubAccess,
            string pubGenres,
            string pubKeyWords,
            string pubGeoLocations,
            string pubStockTickers,
            string headlineImageUrl,
            bool includeImageInExcerpt,
			bool includeImageInPost
		)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_Update", 50);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 100, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@Heading", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NVarChar, -1, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@IsInNewsletter", SqlDbType.Bit, ParameterDirection.Input, isInNewsletter);
            sph.DefineSqlParameter("@IncludeInFeed", SqlDbType.Bit, ParameterDirection.Input, includeInFeed);
            sph.DefineSqlParameter("@AllowCommentsForDays", SqlDbType.Int, ParameterDirection.Input, allowCommentsForDays);
            sph.DefineSqlParameter("@Location", SqlDbType.NVarChar, -1, ParameterDirection.Input, location);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@ItemUrl", SqlDbType.NVarChar, 512, ParameterDirection.Input, itemUrl);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@SubTitle", SqlDbType.NVarChar, 500, ParameterDirection.Input, subTitle);

            if (endDate < DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            }
            else
            {
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            sph.DefineSqlParameter("@Approved", SqlDbType.Bit, ParameterDirection.Input, approved);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedBy);

            if (approvedDate < DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@ApprovedDate", SqlDbType.DateTime, ParameterDirection.Input, approvedDate);
            }
            else
            {
                sph.DefineSqlParameter("@ApprovedDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }

            sph.DefineSqlParameter("@ShowAuthorName", SqlDbType.Bit, ParameterDirection.Input, showAuthorName);
            sph.DefineSqlParameter("@ShowAuthorAvatar", SqlDbType.Bit, ParameterDirection.Input, showAuthorAvatar);
            sph.DefineSqlParameter("@ShowAuthorBio", SqlDbType.Bit, ParameterDirection.Input, showAuthorBio);
            sph.DefineSqlParameter("@IncludeInSearch", SqlDbType.Bit, ParameterDirection.Input, includeInSearch);
            sph.DefineSqlParameter("@UseBingMap", SqlDbType.Bit, ParameterDirection.Input, useBingMap);
            sph.DefineSqlParameter("@MapHeight", SqlDbType.NVarChar, 10, ParameterDirection.Input, mapHeight);
            sph.DefineSqlParameter("@MapWidth", SqlDbType.NVarChar, 10, ParameterDirection.Input, mapWidth);
            sph.DefineSqlParameter("@ShowMapOptions", SqlDbType.Bit, ParameterDirection.Input, showMapOptions);
            sph.DefineSqlParameter("@ShowZoomTool", SqlDbType.Bit, ParameterDirection.Input, showZoomTool);
            sph.DefineSqlParameter("@ShowLocationInfo", SqlDbType.Bit, ParameterDirection.Input, showLocationInfo);
            sph.DefineSqlParameter("@UseDrivingDirections", SqlDbType.Bit, ParameterDirection.Input, useDrivingDirections);
            sph.DefineSqlParameter("@MapType", SqlDbType.NVarChar, 20, ParameterDirection.Input, mapType);
            sph.DefineSqlParameter("@MapZoom", SqlDbType.Int, ParameterDirection.Input, mapZoom);
            sph.DefineSqlParameter("@ShowDownloadLink", SqlDbType.Bit, ParameterDirection.Input, showDownloadLink);
            sph.DefineSqlParameter("@IncludeInSiteMap", SqlDbType.Bit, ParameterDirection.Input, includeInSiteMap);
            sph.DefineSqlParameter("@ExcludeFromRecentContent", SqlDbType.Bit, ParameterDirection.Input, excludeFromRecentContent);
            sph.DefineSqlParameter("@IncludeInNews", SqlDbType.Bit, ParameterDirection.Input, includeInNews);
            sph.DefineSqlParameter("@PubName", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubName);
            sph.DefineSqlParameter("@PubLanguage", SqlDbType.NVarChar, 7, ParameterDirection.Input, pubLanguage);
            sph.DefineSqlParameter("@PubAccess", SqlDbType.NVarChar, 20, ParameterDirection.Input, pubAccess);
            sph.DefineSqlParameter("@PubGenres", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubGenres);
            sph.DefineSqlParameter("@PubKeyWords", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubKeyWords);
            sph.DefineSqlParameter("@PubGeoLocations", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubGeoLocations);
            sph.DefineSqlParameter("@PubStockTickers", SqlDbType.NVarChar, 255, ParameterDirection.Input, pubStockTickers);
            sph.DefineSqlParameter("@HeadlineImageUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, headlineImageUrl);
			sph.DefineSqlParameter("@IncludeImageInExcerpt", SqlDbType.Bit, ParameterDirection.Input, includeImageInExcerpt);
			sph.DefineSqlParameter("@IncludeImageInPost", SqlDbType.Bit, ParameterDirection.Input, includeImageInPost);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Blog_UpdateCommentCount", 2);
            sph.DefineSqlParameter("@BlogGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, blogGuid);
            sph.DefineSqlParameter("@CommentCount", SqlDbType.Int, ParameterDirection.Input, commentCount);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool AddBlogComment(
          int moduleId,
          int itemId,
          string name,
          string title,
          string url,
          string comment,
                DateTime dateCreated)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogComment_Insert", 7);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 100, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@URL", SqlDbType.NVarChar, 200, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Comment", SqlDbType.NVarChar, -1, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@DateCreated", SqlDbType.DateTime, ParameterDirection.Input, dateCreated);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }


        public static bool DeleteAllCommentsForBlog(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogComments_DeleteByPost", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool UpdateCommentStats(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogStats_UpdateCommentCount", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
        }

        public static bool UpdateEntryStats(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogStats_UpdateEntryStats", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            
        }

        public static bool DeleteBlogComment(int commentId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogComment_Delete", 1);
            sph.DefineSqlParameter("@BlogCommentID", SqlDbType.Int, ParameterDirection.Input, commentId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }


        public static IDataReader GetBlogComments(int moduleId, int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogComments_Select", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static int AddBlogCategory(
          int moduleId,
          string category)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogCategories_Insert", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Category", SqlDbType.NVarChar, 255, ParameterDirection.Input, category);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateBlogCategory(
          int categoryId,
          string category)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogCategories_Update", 2);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            sph.DefineSqlParameter("@Category", SqlDbType.NVarChar, 255, ParameterDirection.Input, category);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteCategory(int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogCategories_Delete", 1);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetCategory(int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogCategories_SelectOne", 1);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetCategories(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogCategories_SelectByModule", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        public static IDataReader GetCategoriesList(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogCategories_SelectListByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static int AddBlogItemCategory(
          int itemId,
          int categoryId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogItemCategories_Insert", 2);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@CategoryID", SqlDbType.Int, ParameterDirection.Input, categoryId);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool DeleteItemCategories(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_BlogItemCategories_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetBlogItemCategories(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_BlogItemCategories_SelectByItem", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }
    }
}