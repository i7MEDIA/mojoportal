// Author:					
// Created:				    2007-11-03
// Last Modified:			2013-04-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBModule
    {
       
       

        public static int AddModule(
            int pageId,
            int siteId,
            Guid siteGuid,
            int moduleDefId,
            int moduleOrder,
            string paneName,
            string moduleTitle,
            string viewRoles,
            string authorizedEditRoles,
            string draftEditRoles,
            string draftApprovalRoles, 
            int cacheTime,
            bool showTitle,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            String icon,
            int createdByUserId,
            DateTime createdDate,
            Guid guid,
            Guid featureGuid,
            bool hideFromAuthenticated,
            bool hideFromUnauthenticated,
            string headElement,
            int publishMode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_Insert", 24); //JOE DAVIS
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@ModuleOrder", SqlDbType.Int, ParameterDirection.Input, moduleOrder);
            sph.DefineSqlParameter("@PaneName", SqlDbType.NVarChar, 50, ParameterDirection.Input, paneName);
            sph.DefineSqlParameter("@ModuleTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, moduleTitle);
            sph.DefineSqlParameter("@AuthorizedEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorizedEditRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@DraftApprovalRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftApprovalRoles); //JOE DAVIS
            sph.DefineSqlParameter("@CacheTime", SqlDbType.Int, ParameterDirection.Input, cacheTime);
            sph.DefineSqlParameter("@ShowTitle", SqlDbType.Bit, ParameterDirection.Input, showTitle);
            sph.DefineSqlParameter("@AvailableForMyPage", SqlDbType.Bit, ParameterDirection.Input, availableForMyPage);
            sph.DefineSqlParameter("@CreatedByUserID", SqlDbType.Int, ParameterDirection.Input, createdByUserId);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit, ParameterDirection.Input, allowMultipleInstancesOnMyPage);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@HideFromAuth", SqlDbType.Bit, ParameterDirection.Input, hideFromAuthenticated);
            sph.DefineSqlParameter("@HideFromUnAuth", SqlDbType.Bit, ParameterDirection.Input, hideFromUnauthenticated);
            sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
            sph.DefineSqlParameter("@HeadElement", SqlDbType.NVarChar, 25, ParameterDirection.Input, headElement);
            sph.DefineSqlParameter("@PublishMode", SqlDbType.Int, ParameterDirection.Input, publishMode);

            int newID = Convert.ToInt32(sph.ExecuteScalar());

            return newID;
        }

        public static bool UpdateModule(
            int moduleId,
            int moduleDefId,
            string moduleTitle,
            string viewRoles,
            string authorizedEditRoles,
            string draftEditRoles,
            string draftApprovalRoles, 
            int cacheTime,
            bool showTitle,
            int editUserId,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            String icon,
            bool hideFromAuthenticated,
            bool hideFromUnauthenticated,
            bool includeInSearch,
            bool isGlobal,
            string headElement,
            int publishMode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_Update", 19); 
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@ModuleTitle", SqlDbType.NVarChar, ParameterDirection.Input, moduleTitle);
            sph.DefineSqlParameter("@AuthorizedEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, authorizedEditRoles);
            sph.DefineSqlParameter("@DraftEditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftEditRoles);
            sph.DefineSqlParameter("@DraftApprovalRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, draftApprovalRoles); //JOE DAVIS
            sph.DefineSqlParameter("@CacheTime", SqlDbType.Int, ParameterDirection.Input, cacheTime);
            sph.DefineSqlParameter("@ShowTitle", SqlDbType.Bit, ParameterDirection.Input, showTitle);
            sph.DefineSqlParameter("@EditUserID", SqlDbType.Int, ParameterDirection.Input, editUserId);
            sph.DefineSqlParameter("@AvailableForMyPage", SqlDbType.Bit, ParameterDirection.Input, availableForMyPage);
            sph.DefineSqlParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit, ParameterDirection.Input, allowMultipleInstancesOnMyPage);
            sph.DefineSqlParameter("@Icon", SqlDbType.NVarChar, 255, ParameterDirection.Input, icon);
            sph.DefineSqlParameter("@HideFromAuth", SqlDbType.Bit, ParameterDirection.Input, hideFromAuthenticated);
            sph.DefineSqlParameter("@HideFromUnAuth", SqlDbType.Bit, ParameterDirection.Input, hideFromUnauthenticated);
            sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
            sph.DefineSqlParameter("@IncludeInSearch", SqlDbType.Bit, ParameterDirection.Input, includeInSearch);
            sph.DefineSqlParameter("@IsGlobal", SqlDbType.Bit, ParameterDirection.Input, isGlobal);
            sph.DefineSqlParameter("@HeadElement", SqlDbType.NVarChar, 25, ParameterDirection.Input, headElement);
            sph.DefineSqlParameter("@PublishMode", SqlDbType.Int, ParameterDirection.Input, publishMode);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateModuleOrder(int pageId, int moduleId, int moduleOrder, string paneName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_UpdateModuleOrder", 4);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@ModuleOrder", SqlDbType.Int, ParameterDirection.Input, moduleOrder);
            sph.DefineSqlParameter("@PaneName", SqlDbType.NVarChar, 50, ParameterDirection.Input, paneName);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_UpdatePage", 3);
            sph.DefineSqlParameter("@OldPageID", SqlDbType.Int, ParameterDirection.Input, oldPageId);
            sph.DefineSqlParameter("@NewPageID", SqlDbType.Int, ParameterDirection.Input, newPageId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool Publish(
            Guid pageGuid,
            Guid moduleGuid,
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            if (PageModuleExists(moduleId, pageId))
            {
                return PageModuleUpdate(
                    moduleId,
                    pageId,
                    paneName,
                    moduleOrder,
                    publishBeginDate,
                    publishEndDate);
            }
            else
            {
                return PageModuleInsert(
                    pageGuid,
                    moduleGuid,
                    moduleId,
                    pageId,
                    paneName,
                    moduleOrder,
                    publishBeginDate,
                    publishEndDate);

            }

        }

        public static bool PageModuleExists(int moduleId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PageModule_Exists", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static DataTable PageModuleGetByModule(int moduleId)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("PaneName", typeof(string));
            dataTable.Columns.Add("ModuleOrder", typeof(int));
            dataTable.Columns.Add("PublishBeginDate", typeof(DateTime));
            dataTable.Columns.Add("PublishEndDate", typeof(DateTime));

            using (IDataReader reader = PageModuleGetReaderByModule(moduleId))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["PageID"] = reader["PageID"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["PaneName"] = reader["PaneName"];
                    row["ModuleOrder"] = reader["ModuleOrder"];
                    row["PublishBeginDate"] = reader["PublishBeginDate"];
                    row["PublishEndDate"] = reader["PublishEndDate"];

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;

        }

        public static IDataReader PageModuleGetReaderByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PageModule_SelectByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static IDataReader PageModuleGetReaderByPage(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PageModule_SelectByPage", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static bool PageModuleInsert(
            Guid pageGuid,
            Guid moduleGuid,
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_PageModules_Insert", 8);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@PaneName", SqlDbType.NVarChar, 50, ParameterDirection.Input, paneName);
            sph.DefineSqlParameter("@ModuleOrder", SqlDbType.Int, ParameterDirection.Input, moduleOrder);
            sph.DefineSqlParameter("@PublishBeginDate", SqlDbType.DateTime, ParameterDirection.Input, publishBeginDate);
            sph.DefineSqlParameter("@PublishEndDate", SqlDbType.DateTime, ParameterDirection.Input, (publishEndDate == DateTime.MinValue) ? (object)DBNull.Value : (object)publishEndDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool PageModuleUpdate(
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PageModules_Update", 6);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@PaneName", SqlDbType.NVarChar, 50, ParameterDirection.Input, paneName);
            sph.DefineSqlParameter("@ModuleOrder", SqlDbType.Int, ParameterDirection.Input, moduleOrder);
            sph.DefineSqlParameter("@PublishBeginDate", SqlDbType.DateTime, ParameterDirection.Input, publishBeginDate);
            sph.DefineSqlParameter("@PublishEndDate", SqlDbType.DateTime, ParameterDirection.Input, (publishEndDate == DateTime.MinValue) ? (object)DBNull.Value : (object)publishEndDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool PageModuleDeleteByPage(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_PageModules_DeleteByPage", 1);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_UpdateCountOfUseOnMyPage", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@Increment", SqlDbType.Int, ParameterDirection.Input, increment);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_Delete", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteModuleInstance(int moduleId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Modules_DeleteInstance", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectOne", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectOneByGuid", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetModule(int moduleId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectOneByPage", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPageModules(int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectByPage", 2);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        
        public static IDataReader GetMyPageModules(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectForMyPage", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectBySite", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
		/// Gets a count of rows in the mp_Modules table.
		/// </summary>
        public static int GetCount(int siteId, int moduleDefId, string title)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_GetCount", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static DataTable SelectPage(
            int siteId,
            int moduleDefId, 
            string title,
            int pageNumber,
            int pageSize,
            bool sortByModuleType,
            bool sortByAuthor,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCount(siteId, moduleDefId, title);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectPage", 7);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@SortByModuleType", SqlDbType.Bit, ParameterDirection.Input, sortByModuleType);
            sph.DefineSqlParameter("@SortByAuthor", SqlDbType.Bit, ParameterDirection.Input, sortByAuthor);
            

            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("ModuleTitle", typeof(String));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ResourceFile", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));
            dt.Columns.Add("AuthorizedEditRoles", typeof(String));
            dt.Columns.Add("CreatedBy", typeof(String));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("UseCount", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["FeatureName"] = reader["FeatureName"];
                    row["ResourceFile"] = reader["ResourceFile"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    row["AuthorizedEditRoles"] = reader["AuthorizedEditRoles"];
                    row["CreatedBy"] = reader["CreatedBy"];
                    row["CreatedDate"] = reader["CreatedDate"];
                    row["UseCount"] = reader["UseCount"];

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static int GetCountByFeature(int moduleDefId)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_CountByFeature", 1);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);

            return Convert.ToInt32(sph.ExecuteScalar());

        }


        public static int GetGlobalCount(int siteId, int moduleDefId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_GetGlobalCount", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static DataTable SelectGlobalPage(
            int siteId,
            int moduleDefId,
            int pageId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetGlobalCount(siteId, moduleDefId, pageId);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectGlobalPage", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ModuleDefID", SqlDbType.Int, ParameterDirection.Input, moduleDefId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            


            DataTable dt = new DataTable();
            dt.Columns.Add("ModuleID", typeof(int));
            dt.Columns.Add("ModuleTitle", typeof(String));
            dt.Columns.Add("FeatureName", typeof(String));
            dt.Columns.Add("ResourceFile", typeof(String));
            dt.Columns.Add("ControlSrc", typeof(String));
            dt.Columns.Add("AuthorizedEditRoles", typeof(String));
            dt.Columns.Add("CreatedBy", typeof(String));
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("UseCount", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["FeatureName"] = reader["FeatureName"];
                    row["ResourceFile"] = reader["ResourceFile"];
                    row["ControlSrc"] = reader["ControlSrc"];
                    row["AuthorizedEditRoles"] = reader["AuthorizedEditRoles"];
                    row["CreatedBy"] = reader["CreatedBy"];
                    row["CreatedDate"] = reader["CreatedDate"];
                    row["UseCount"] = reader["UseCount"];

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

		public static IDataReader GetGlobalContent(int siteId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Modules_SelectGlobalContent", 1);
			sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
			return sph.ExecuteReader();
		}
	}
}
