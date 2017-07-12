/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2013-04-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    
    public static class DBHtmlContent
    {

        public static int AddHtmlContent(
            Guid itemGuid,
            Guid moduleGuid,
            int moduleId,
            string title,
            string excerpt,
            string body,
            string moreLink,
            int sortOrder,
            DateTime beginDate,
            DateTime endDate,
            DateTime createdDate,
            int userId,
            Guid userGuid,
            bool excludeFromRecentContent)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_HtmlContent_Insert", 14);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Excerpt", SqlDbType.NVarChar, -1, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Body", SqlDbType.NVarChar, -1, ParameterDirection.Input, body);
            sph.DefineSqlParameter("@MoreLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, moreLink);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@ExcludeFromRecentContent", SqlDbType.Bit, ParameterDirection.Input, excludeFromRecentContent);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateHtmlContent(
          int itemId,
          int moduleId,
          string title,
          string excerpt,
          string body,
          string moreLink,
          int sortOrder,
          DateTime beginDate,
          DateTime endDate,
          DateTime lastModUtc,
          Guid lastModUserGuid,
          bool excludeFromRecentContent)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_HtmlContent_Update", 12);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Excerpt", SqlDbType.NVarChar, -1, ParameterDirection.Input, excerpt);
            sph.DefineSqlParameter("@Body", SqlDbType.NVarChar, -1, ParameterDirection.Input, body);
            sph.DefineSqlParameter("@MoreLink", SqlDbType.NVarChar, 255, ParameterDirection.Input, moreLink);
            sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@ExcludeFromRecentContent", SqlDbType.Bit, ParameterDirection.Input, excludeFromRecentContent);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        //public static bool ApproveHtmlContent(
        //    Guid moduleGuid,
        //    int itemId,
        //    Guid siteGuid,
        //    Guid approvalUserGuid,
        //    DateTime approvalUtc)
        //{

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_HtmlContent_Approve", 5);

        //    sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
        //    sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
        //    sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
        //    sph.DefineSqlParameter("@ApprovalUserGuid", SqlDbType.UniqueIdentifier, 255, ParameterDirection.Input, approvalUserGuid);
        //    sph.DefineSqlParameter("@ApprovalUtc", SqlDbType.DateTime, ParameterDirection.Input, approvalUtc);

        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > -1);
        //}

        public static bool DeleteHtmlContent(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_HtmlContent_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_HtmlContent_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_HtmlContent_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetHtmlContent(int moduleId, int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_HtmlContent_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetHtmlForMetaWeblogApi(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_HtmlContent_SelectForMetaWeblogApi", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetHtmlContent(
                int moduleId,
                DateTime beginDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_HtmlContent_Select", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, beginDate);
            return sph.ExecuteReader();
        }

        public static IDataReader GetHtmlContentByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_HtmlContent_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        

    }
}
