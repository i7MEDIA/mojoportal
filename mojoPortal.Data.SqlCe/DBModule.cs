// Author:					Joe Audette
// Created:					2010-04-05
// Last Modified:			2013-04-23
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
    public static class DBModule
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Modules ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("ModuleDefID, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("AuthorizedEditRoles, ");
            sqlCommand.Append("CacheTime, ");
            sqlCommand.Append("ShowTitle, ");
            sqlCommand.Append("EditUserID, ");
            sqlCommand.Append("AvailableForMyPage, ");
            sqlCommand.Append("AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("Icon, ");
            sqlCommand.Append("CreatedByUserID, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("CountOfUseOnMyPage, ");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("EditUserGuid, ");
            sqlCommand.Append("HideFromUnAuth, ");
            sqlCommand.Append("HideFromAuth, ");
            sqlCommand.Append("ViewRoles, ");
            sqlCommand.Append("DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles, ");
            sqlCommand.Append("IncludeInSearch, ");
            sqlCommand.Append("HeadElement, ");
            sqlCommand.Append("PublishMode, ");
            sqlCommand.Append("IsGlobal ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@ModuleDefID, ");
            sqlCommand.Append("@ModuleTitle, ");
            sqlCommand.Append("@AuthorizedEditRoles, ");
            sqlCommand.Append("@CacheTime, ");
            sqlCommand.Append("@ShowTitle, ");
            sqlCommand.Append("@EditUserID, ");
            sqlCommand.Append("@AvailableForMyPage, ");
            sqlCommand.Append("@AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("@Icon, ");
            sqlCommand.Append("@CreatedByUserID, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@CountOfUseOnMyPage, ");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@EditUserGuid, ");
            sqlCommand.Append("@HideFromUnAuth, ");
            sqlCommand.Append("@HideFromAuth, ");
            sqlCommand.Append("@ViewRoles, ");
            sqlCommand.Append("@DraftEditRoles, ");
            sqlCommand.Append("@DraftApprovalRoles, ");
            sqlCommand.Append("1, ");
            sqlCommand.Append("@HeadElement, ");
            sqlCommand.Append("@PublishMode, ");
            sqlCommand.Append("0 ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[24];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@ModuleTitle", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleTitle;

            arParams[3] = new SqlCeParameter("@AuthorizedEditRoles", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = authorizedEditRoles;

            arParams[4] = new SqlCeParameter("@CacheTime", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cacheTime;

            arParams[5] = new SqlCeParameter("@ShowTitle", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = showTitle;

            arParams[6] = new SqlCeParameter("@EditUserID", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = -1;

            arParams[7] = new SqlCeParameter("@AvailableForMyPage", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = availableForMyPage;

            arParams[8] = new SqlCeParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultipleInstancesOnMyPage;

            arParams[9] = new SqlCeParameter("@Icon", SqlDbType.NVarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = icon;

            arParams[10] = new SqlCeParameter("@CreatedByUserID", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdByUserId;

            arParams[11] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdDate;

            arParams[12] = new SqlCeParameter("@CountOfUseOnMyPage", SqlDbType.Int);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = 0;

            arParams[13] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = guid;

            arParams[14] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = featureGuid;

            arParams[15] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = siteGuid;

            arParams[16] = new SqlCeParameter("@EditUserGuid", SqlDbType.UniqueIdentifier);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = Guid.Empty;

            arParams[17] = new SqlCeParameter("@HideFromUnAuth", SqlDbType.Bit);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = hideFromUnauthenticated;

            arParams[18] = new SqlCeParameter("@HideFromAuth", SqlDbType.Bit);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = hideFromAuthenticated;

            arParams[19] = new SqlCeParameter("@ViewRoles", SqlDbType.NText);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = viewRoles;

            arParams[20] = new SqlCeParameter("@DraftEditRoles", SqlDbType.NText);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = draftEditRoles;

            arParams[21] = new SqlCeParameter("@HeadElement", SqlDbType.NVarChar, 25);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = headElement;

            arParams[22] = new SqlCeParameter("@PublishMode", SqlDbType.Int);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = publishMode;

            arParams[23] = new SqlCeParameter("@DraftApprovalRoles", SqlDbType.NText);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = draftApprovalRoles;

            

            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            if (pageId > -1)
            {
                Guid pageGuid = GetPageGuid(pageId);
                PageModuleInsert(
                    pageGuid,
                    featureGuid,
                    newId,
                    pageId,
                    paneName,
                    moduleOrder,
                    createdDate,
                    DateTime.MinValue);

            }

            return newId;


        }

        private static Guid GetPageGuid(int pageId)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  PageGuid ");
            sqlCommand.Append("FROM	mp_Pages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            object o = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            string strGuid = string.Empty;
            if (o != null) { strGuid = o.ToString(); }

            if ((strGuid != null) && (strGuid.Length == 36))
            {
                return new Guid(strGuid);
            }

            return Guid.Empty;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Modules ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("ModuleDefID = @ModuleDefID, ");
            sqlCommand.Append("ModuleTitle = @ModuleTitle, ");
            sqlCommand.Append("AuthorizedEditRoles = @AuthorizedEditRoles, ");
            sqlCommand.Append("CacheTime = @CacheTime, ");
            sqlCommand.Append("ShowTitle = @ShowTitle, ");
            sqlCommand.Append("EditUserID = @EditUserID, ");
            sqlCommand.Append("AvailableForMyPage = @AvailableForMyPage, ");
            sqlCommand.Append("AllowMultipleInstancesOnMyPage = @AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("Icon = @Icon, ");
            sqlCommand.Append("HideFromUnAuth = @HideFromUnAuth, ");
            sqlCommand.Append("HideFromAuth = @HideFromAuth, ");
            sqlCommand.Append("ViewRoles = @ViewRoles, ");
            sqlCommand.Append("DraftEditRoles = @DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles = @DraftApprovalRoles, ");
            sqlCommand.Append("IncludeInSearch = @IncludeInSearch, ");
            sqlCommand.Append("HeadElement = @HeadElement, ");
            sqlCommand.Append("PublishMode = @PublishMode, ");
            sqlCommand.Append("IsGlobal = @IsGlobal ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[19];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ModuleTitle", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleTitle;

            arParams[2] = new SqlCeParameter("@AuthorizedEditRoles", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = authorizedEditRoles;

            arParams[3] = new SqlCeParameter("@CacheTime", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = cacheTime;

            arParams[4] = new SqlCeParameter("@ShowTitle", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = showTitle;

            arParams[5] = new SqlCeParameter("@EditUserID", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = editUserId;

            arParams[6] = new SqlCeParameter("@AvailableForMyPage", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = availableForMyPage;

            arParams[7] = new SqlCeParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = allowMultipleInstancesOnMyPage;

            arParams[8] = new SqlCeParameter("@Icon", SqlDbType.NVarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = icon;

            arParams[9] = new SqlCeParameter("@HideFromUnAuth", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = hideFromUnauthenticated;

            arParams[10] = new SqlCeParameter("@HideFromAuth", SqlDbType.Bit);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = hideFromAuthenticated;

            arParams[11] = new SqlCeParameter("@ViewRoles", SqlDbType.NText);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = viewRoles;

            arParams[12] = new SqlCeParameter("@DraftEditRoles", SqlDbType.NText);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = draftEditRoles;

            arParams[13] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = moduleDefId;

            arParams[14] = new SqlCeParameter("@IncludeInSearch", SqlDbType.Bit);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = includeInSearch;

            arParams[15] = new SqlCeParameter("@IsGlobal", SqlDbType.Bit);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = isGlobal;

            arParams[16] = new SqlCeParameter("@HeadElement", SqlDbType.NVarChar, 25);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = headElement;

            arParams[17] = new SqlCeParameter("@PublishMode", SqlDbType.Int);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = publishMode;

            arParams[18] = new SqlCeParameter("@DraftApprovalRoles", SqlDbType.NText);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = draftApprovalRoles;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool UpdateModuleOrder(int pageId, int moduleId, int moduleOrder, string paneName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PageModules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PaneName = @PaneName, ");
            sqlCommand.Append("ModuleOrder = @ModuleOrder ");
           

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID AND ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@PaneName", SqlDbType.NVarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = paneName;

            arParams[3] = new SqlCeParameter("@ModuleOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleOrder;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
        {
            Guid pageGuid = DBPageSettings.GetPageGuidFromID(newPageId);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PageModules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PageID = @NewPageID, ");
            sqlCommand.Append("PageGuid = @PageGuid) ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @OldPageID AND ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@OldPageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = oldPageId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@NewPageID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = newPageId;

            arParams[3] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_PageModules ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID AND ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  pm.*, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.Url ");

            sqlCommand.Append("FROM	mp_PageModules pm ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader PageModuleGetReaderByPage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  pm.*, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.Url ");

            sqlCommand.Append("FROM	mp_PageModules pm ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.PageID = @PageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_PageModules ");
            sqlCommand.Append("(");
            sqlCommand.Append("PaneName, ");
            sqlCommand.Append("ModuleOrder, ");
            sqlCommand.Append("PublishBeginDate, ");
            if (publishEndDate > DateTime.MinValue)
            {
                sqlCommand.Append("PublishEndDate, ");
            }
            sqlCommand.Append("PageID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("ModuleGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@PaneName, ");
            sqlCommand.Append("@ModuleOrder, ");
            sqlCommand.Append("@PublishBeginDate, ");
            if (publishEndDate > DateTime.MinValue)
            {
                sqlCommand.Append("@PublishEndDate, ");
            }
            sqlCommand.Append("@PageID, ");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@PageGuid, ");
            sqlCommand.Append("@ModuleGuid ");
            sqlCommand.Append(")");
           
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[8];

            arParams[0] = new SqlCeParameter("@PaneName", SqlDbType.NVarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = paneName;

            arParams[1] = new SqlCeParameter("@ModuleOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleOrder;

            arParams[2] = new SqlCeParameter("@PublishBeginDate", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = publishBeginDate;

            arParams[3] = new SqlCeParameter("@PublishEndDate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            if (publishEndDate > DateTime.MinValue)
            {
                arParams[3].Value = publishEndDate;
            }
            else
            {
                arParams[3].Value = DBNull.Value;
            }

            arParams[4] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageGuid;

            arParams[5] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = moduleGuid;

            arParams[6] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = pageId;

            arParams[7] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = moduleId;


            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool PageModuleUpdate(
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PageModules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PaneName = @PaneName, ");
            sqlCommand.Append("ModuleOrder = @ModuleOrder, ");
            sqlCommand.Append("PublishBeginDate = @PublishBeginDate, ");
            sqlCommand.Append("PublishEndDate = @PublishEndDate ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageID = @PageID AND ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@PaneName", SqlDbType.NVarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = paneName;

            arParams[3] = new SqlCeParameter("@ModuleOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleOrder;

            arParams[4] = new SqlCeParameter("@PublishBeginDate", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = publishBeginDate;

            arParams[5] = new SqlCeParameter("@PublishEndDate", SqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            if (publishEndDate > DateTime.MinValue)
            {
                arParams[5].Value = publishEndDate;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

     
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool PageModuleDeleteByPage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageID = @PageID  ");
            
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

        public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Modules ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("CountOfUseOnMyPage = CountOfUseOnMyPage + @Increment ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@Increment", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = increment;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            
            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Modules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteModuleInstance(int moduleId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageID = @PageID AND ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  m.*, ");
            
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  m.*, ");
            
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.[Guid] = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModule(int moduleId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  m.*, ");
            sqlCommand.Append("pm.PageID, ");
            sqlCommand.Append("pm.ModuleOrder, ");
            sqlCommand.Append("pm.PaneName, ");
            sqlCommand.Append("pm.PublishBeginDate, ");
            sqlCommand.Append("pm.PublishEndDate, ");
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.PageID = @PageID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("pm.ModuleID = @ModuleID ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageModules(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  m.*, ");
            sqlCommand.Append("pm.PageID, ");
            sqlCommand.Append("pm.ModuleOrder, ");
            sqlCommand.Append("pm.PaneName, ");
            sqlCommand.Append("pm.PublishBeginDate, ");
            sqlCommand.Append("pm.PublishEndDate, ");
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.Guid AS FeatureGuid ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.PageID = @PageID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("pm.PublishBeginDate <= @CurrentTime ");
            sqlCommand.Append("AND	( ");
            sqlCommand.Append("(pm.PublishEndDate IS NULL) ");
            sqlCommand.Append("OR ");
            sqlCommand.Append("(pm.PublishEndDate > @CurrentTime) ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("pm.ModuleOrder ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMyPageModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.SiteID, ");
            sqlCommand.Append("m.ModuleDefID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("m.HideFromUnAuth, ");
            sqlCommand.Append("m.HideFromAuth, ");
            sqlCommand.Append("m.Icon As ModuleIcon, ");
            sqlCommand.Append("md.Icon As FeatureIcon, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.AvailableForMyPage = 1 ");

            sqlCommand.Append("ORDER BY m.ModuleTitle ");

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

        public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AuthorizedEditRoles, ");
            sqlCommand.Append("m.EditUserID, ");
            sqlCommand.Append("p.Url, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.EditRoles ");
            

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN mp_Pages p ");
            sqlCommand.Append("ON pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("md.[Guid] = @FeatureGuid ");

            sqlCommand.Append("ORDER BY p.PageName, m.ModuleTitle ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;


            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(int siteId, int moduleDefId, string title)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            sqlCommand.Append("AND ((m.ModuleTitle LIKE '%' + @Title + '%') OR (@Title = '')) ");
            sqlCommand.Append("AND md.IsAdmin = 0 ");


            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountByFeature(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Modules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");

            sqlCommand.Append("m.*, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("u.[Name] As CreatedBy, ");
            sqlCommand.Append("COALESCE(c.UseCount, 0) As UseCount ");

            sqlCommand.Append("FROM	mp_Modules m  ");

            sqlCommand.Append("LEFT JOIN (SELECT ModuleID, COUNT(*) As UseCount FROM mp_PageModules GROUP BY ModuleID) as c ");
            sqlCommand.Append("ON m.ModuleID = c.ModuleID ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON m.CreatedByUserID = u.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            sqlCommand.Append("AND ((m.ModuleTitle LIKE '%' + @Title + '%') OR (@Title = '')) ");
            sqlCommand.Append("AND md.IsAdmin = 0 ");

            sqlCommand.Append("ORDER BY  ");
            if(sortByAuthor)
            {
                sqlCommand.Append("u.[Name], m.ModuleTitle  ");
            }
            else
            {
                sqlCommand.Append("m.ModuleTitle  ");
            }

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            if (sortByAuthor)
            {
                sqlCommand.Append("t1.[Name], t1.ModuleTitle  ");
            }
            else
            {
                sqlCommand.Append("t1.ModuleTitle  ");
            }
            sqlCommand.Append("DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY  ");
            if (sortByAuthor)
            {
                sqlCommand.Append("t2.[Name], t2.ModuleTitle  ");
            }
            else
            {
                sqlCommand.Append("t2.ModuleTitle  ");
            }
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
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

        public static int GetGlobalCount(int siteId, int moduleDefId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            
            sqlCommand.Append("AND m.IsGlobal = 1 ");
            sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = @PageID) ");


            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static DataTable SelectGlobalPage(
            int siteId,
            int moduleDefId,
            int pageId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");

            sqlCommand.Append("m.*, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("u.[Name] As CreatedBy, ");
            sqlCommand.Append("COALESCE(c.UseCount, 0) As UseCount ");

            sqlCommand.Append("FROM	mp_Modules m  ");

            sqlCommand.Append("LEFT JOIN (SELECT ModuleID, COUNT(*) As UseCount FROM mp_PageModules GROUP BY ModuleID) as c ");
            sqlCommand.Append("ON m.ModuleID = c.ModuleID ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON m.CreatedByUserID = u.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
           
            sqlCommand.Append("AND m.IsGlobal = 1 ");
            sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = @PageID) ");

            sqlCommand.Append("ORDER BY  ");
            
             sqlCommand.Append("m.ModuleTitle  ");
            

            sqlCommand.Append(") AS t1 ");
            sqlCommand.Append("ORDER BY  ");
            
            sqlCommand.Append("t1.ModuleTitle  ");
            
            sqlCommand.Append("DESC ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            sqlCommand.Append("ORDER BY  ");
            
            sqlCommand.Append("t2.ModuleTitle  ");
            
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@ModuleDefID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageId;

            
            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
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

    }
}
