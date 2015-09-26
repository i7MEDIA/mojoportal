// Author:					Joe Audette
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
// Note moved into separate class file from dbPortal 2007-11-03

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
    /// <summary>
    /// 
    /// </summary>
    public static class DBModule
    {
        
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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
            #region Bit Conversion

            int inthideFromAuthenticated;
            if (hideFromAuthenticated)
            {
                inthideFromAuthenticated = 1;
            }
            else
            {
                inthideFromAuthenticated = 0;
            }

            int inthideFromUnauthenticated;
            if (hideFromUnauthenticated)
            {
                inthideFromUnauthenticated = 1;
            }
            else
            {
                inthideFromUnauthenticated = 0;
            }

            int intShowTitle;
            if (showTitle)
            {
                intShowTitle = 1;
            }
            else
            {
                intShowTitle = 0;
            }

            int intAvailableForMyPage;
            if (availableForMyPage)
            {
                intAvailableForMyPage = 1;
            }
            else
            {
                intAvailableForMyPage = 0;
            }

            int intAllowMultipleInstancesOnMyPage;
            if (allowMultipleInstancesOnMyPage)
            {
                intAllowMultipleInstancesOnMyPage = 1;
            }
            else
            {
                intAllowMultipleInstancesOnMyPage = 0;
            }


            #endregion

            FbParameter[] arParams = new FbParameter[23];

            arParams[0] = new FbParameter(":SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter(":ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter(":ModuleTitle", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleTitle;

            arParams[3] = new FbParameter(":AuthorizedEditRoles", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = authorizedEditRoles;

            arParams[4] = new FbParameter(":CacheTime", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cacheTime;

            arParams[5] = new FbParameter(":ShowTitle", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intShowTitle;

            arParams[6] = new FbParameter(":EditUserID", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = -1;

            arParams[7] = new FbParameter(":AvailableForMyPage", FbDbType.SmallInt);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAvailableForMyPage;

            arParams[8] = new FbParameter(":AllowMultipleInstancesOnMyPage", FbDbType.SmallInt);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intAllowMultipleInstancesOnMyPage;

            arParams[9] = new FbParameter(":Icon", FbDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = icon;

            arParams[10] = new FbParameter(":CreatedByUserID", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdByUserId;

            arParams[11] = new FbParameter(":CreatedDate", FbDbType.TimeStamp);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdDate;

            arParams[12] = new FbParameter(":CountOfUseOnMyPage", FbDbType.Integer);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = 0;

            arParams[13] = new FbParameter(":Guid", FbDbType.Char, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = guid.ToString();

            arParams[14] = new FbParameter(":FeatureGuid", FbDbType.Char, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = featureGuid.ToString();

            arParams[15] = new FbParameter(":SiteGuid", FbDbType.Char, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = siteGuid.ToString();

            arParams[16] = new FbParameter(":HideFromAuth", FbDbType.SmallInt);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = inthideFromAuthenticated;

            arParams[17] = new FbParameter(":HideFromUnAuth", FbDbType.SmallInt);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = inthideFromUnauthenticated;

            arParams[18] = new FbParameter(":ViewRoles", FbDbType.VarChar);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = viewRoles;

            arParams[19] = new FbParameter(":DraftEditRoles", FbDbType.VarChar);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = draftEditRoles;

            arParams[20] = new FbParameter(":DraftApprovalRoles", FbDbType.VarChar);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = draftApprovalRoles;

            arParams[21] = new FbParameter(":HeadElement", FbDbType.VarChar, 25);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = headElement;

            arParams[22] = new FbParameter(":PublishMode", FbDbType.Integer);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = publishMode;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_MODULES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));



            if ((newID > -1) && (pageId > -1))
            {
                StringBuilder sqlCommand = new StringBuilder();
                sqlCommand.Append("INSERT INTO mp_PageModules (");
                sqlCommand.Append("PAGEID,");
                sqlCommand.Append("MODULEID,");
                sqlCommand.Append("PageGuid,");
                sqlCommand.Append("ModuleGuid,");
                sqlCommand.Append("ModuleOrder,");
                sqlCommand.Append("PaneName,");
                sqlCommand.Append("PublishBeginDate");
                sqlCommand.Append(") ");

                sqlCommand.Append(" VALUES (");
                sqlCommand.Append("" + pageId.ToString(CultureInfo.InvariantCulture) + ", ");
                sqlCommand.Append("" + newID.ToString(CultureInfo.InvariantCulture) + ", ");
                sqlCommand.Append("(SELECT FIRST 1 PageGuid FROM mp_Pages WHERE PageID = @PageID), ");
                sqlCommand.Append("(SELECT FIRST 1 Guid FROM mp_Modules WHERE ModuleID = @ModuleID), ");
                sqlCommand.Append("@ModuleOrder, ");
                sqlCommand.Append("@PaneName, ");
                sqlCommand.Append("@PublishBeginDate ");
                sqlCommand.Append("); ");

                FbParameter[] arParams2 = new FbParameter[5];

                arParams2[0] = new FbParameter("@PageID", FbDbType.Integer);
                arParams2[0].Direction = ParameterDirection.Input;
                arParams2[0].Value = pageId;

                arParams2[1] = new FbParameter("@ModuleID", FbDbType.Integer);
                arParams2[1].Direction = ParameterDirection.Input;
                arParams2[1].Value = newID;

                arParams2[2] = new FbParameter("@ModuleOrder", FbDbType.Integer);
                arParams2[2].Direction = ParameterDirection.Input;
                arParams2[2].Value = moduleOrder;

                arParams2[3] = new FbParameter("@PaneName", FbDbType.VarChar, 50);
                arParams2[3].Direction = ParameterDirection.Input;
                arParams2[3].Value = paneName;

                arParams2[4] = new FbParameter("@PublishBeginDate", FbDbType.TimeStamp);
                arParams2[4].Direction = ParameterDirection.Input;
                arParams2[4].Value = DateTime.UtcNow.AddMinutes(-30);

                FBSqlHelper.ExecuteNonQuery(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    arParams2);

            }

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

            #region Bit Conversion

            int inthideFromAuthenticated;
            if (hideFromAuthenticated)
            {
                inthideFromAuthenticated = 1;
            }
            else
            {
                inthideFromAuthenticated = 0;
            }

            int inthideFromUnauthenticated;
            if (hideFromUnauthenticated)
            {
                inthideFromUnauthenticated = 1;
            }
            else
            {
                inthideFromUnauthenticated = 0;
            }

            int intShowTitle;
            if (showTitle)
            {
                intShowTitle = 1;
            }
            else
            {
                intShowTitle = 0;
            }

            int myAvailable;
            if (availableForMyPage)
            {
                myAvailable = 1;
            }
            else
            {
                myAvailable = 0;
            }

            int allowMultiple;
            if (allowMultipleInstancesOnMyPage)
            {
                allowMultiple = 1;
            }
            else
            {
                allowMultiple = 0;
            }

            int intIncludeInSearch = 1; 
            if (!includeInSearch)
            {
                intIncludeInSearch = 0;
            }

            int intIsGlobal = 0;
            if (isGlobal)
            {
                intIsGlobal = 1;
            }
           

            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_Modules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID, ");
            sqlCommand.Append("ModuleTitle = @ModuleTitle, ");
            sqlCommand.Append("ViewRoles = @ViewRoles, ");
            sqlCommand.Append("AuthorizedEditRoles = @AuthorizedEditRoles, ");
            sqlCommand.Append("DraftEditRoles = @DraftEditRoles, ");
            sqlCommand.Append("DraftApprovalRoles = @DraftApprovalRoles, ");
            sqlCommand.Append("CacheTime = @CacheTime, ");
            sqlCommand.Append("ShowTitle = @ShowTitle, ");
            sqlCommand.Append("HideFromAuth = @HideFromAuth, ");
            sqlCommand.Append("HideFromUnAuth = @HideFromUnAuth, ");
            sqlCommand.Append("EditUserID = @EditUserID, ");
            sqlCommand.Append("IncludeInSearch = @IncludeInSearch, ");
            sqlCommand.Append("IsGlobal = @IsGlobal, ");
            sqlCommand.Append("PublishMode = @PublishMode, ");
            sqlCommand.Append("HeadElement = @HeadElement, ");
            sqlCommand.Append("AvailableForMyPage = @AvailableForMyPage, ");
            sqlCommand.Append("AllowMultipleInstancesOnMyPage = @AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("Icon = @Icon ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleID = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[19];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@ModuleTitle", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleTitle;

            arParams[3] = new FbParameter("@AuthorizedEditRoles", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = authorizedEditRoles;

            arParams[4] = new FbParameter("@CacheTime", FbDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = cacheTime;

            arParams[5] = new FbParameter("@ShowTitle", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intShowTitle;

            arParams[6] = new FbParameter("@EditUserID", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = editUserId;

            arParams[7] = new FbParameter("@AvailableForMyPage", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = myAvailable;

            arParams[8] = new FbParameter("@AllowMultipleInstancesOnMyPage", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultiple;

            arParams[9] = new FbParameter("@Icon", FbDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = icon;

            arParams[10] = new FbParameter("@HideFromAuth", FbDbType.SmallInt);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = inthideFromAuthenticated;

            arParams[11] = new FbParameter("@HideFromUnAuth", FbDbType.SmallInt);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = inthideFromUnauthenticated;

            arParams[12] = new FbParameter("@ViewRoles", FbDbType.VarChar);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = viewRoles;

            arParams[13] = new FbParameter("@DraftEditRoles", FbDbType.VarChar);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = draftEditRoles;

            arParams[14] = new FbParameter("@DraftApprovalRoles", FbDbType.VarChar);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = draftApprovalRoles;

            arParams[15] = new FbParameter("@IncludeInSearch", FbDbType.Integer);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intIncludeInSearch;

            arParams[16] = new FbParameter("@IsGlobal", FbDbType.Integer);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intIsGlobal;

            arParams[17] = new FbParameter("@HeadElement", FbDbType.VarChar, 25);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = headElement;

            arParams[18] = new FbParameter("@PublishMode", FbDbType.Integer);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = publishMode;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateModuleOrder(
            int pageId,
            int moduleId,
            int moduleOrder,
            string paneName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PageModules ");
            sqlCommand.Append("SET ModuleOrder = @ModuleOrder , ");
            sqlCommand.Append("PaneName = @PaneName  ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new FbParameter("@ModuleOrder", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleOrder;

            arParams[3] = new FbParameter("@PaneName", FbDbType.VarChar, 50);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = paneName;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_Modules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ;");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteModuleInstance(int moduleId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID AND PageID = @PageID ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool PageModuleDeleteByPage(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_PageModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageID = @PageID ;");

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

        public static bool PageModuleExists(int moduleId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_PageModules ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.ModuleID = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.PageID = p.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.PageID = @PageID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

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
            sqlCommand.Append("INSERT INTO mp_PageModules (");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("PageID, ");
            sqlCommand.Append("PaneName, ");
            sqlCommand.Append("ModuleOrder, ");
            sqlCommand.Append("PublishBeginDate, ");
            sqlCommand.Append("PublishEndDate ");
            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@PageGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@PageID, ");
            sqlCommand.Append("@PaneName, ");
            sqlCommand.Append("@ModuleOrder, ");
            sqlCommand.Append("@PublishBeginDate, ");
            sqlCommand.Append("@PublishEndDate ");

            sqlCommand.Append(") ;");

            FbParameter[] arParams = new FbParameter[8];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new FbParameter("@PaneName", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = paneName;

            arParams[3] = new FbParameter("@ModuleOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleOrder;

            arParams[4] = new FbParameter("@PublishBeginDate", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = publishBeginDate;

            arParams[5] = new FbParameter("@PublishEndDate", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            if (publishEndDate != DateTime.MinValue)
            {
                arParams[5].Value = publishEndDate;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            arParams[6] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = pageGuid.ToString();

            arParams[7] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = moduleGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE ModuleID = @ModuleID AND PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new FbParameter("@PaneName", FbDbType.VarChar, 50);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = paneName;

            arParams[3] = new FbParameter("@ModuleOrder", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleOrder;

            arParams[4] = new FbParameter("@PublishBeginDate", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = publishBeginDate;

            arParams[5] = new FbParameter("@PublishEndDate", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            if (publishEndDate != DateTime.MinValue)
            {
                arParams[5].Value = publishEndDate;
            }
            else
            {
                arParams[5].Value = DBNull.Value;
            }

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Modules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("CountOfUseOnMyPage = CountOfUseOnMyPage + @Increment ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID  ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@Increment", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = increment;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_PageModules ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PageID = @NewPageID, ");
            sqlCommand.Append("PageGuid = (SELECT FIRST 1 PageGuid FROM mp_Pages WHERE PageID = @NewPageID) ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID AND PageID = @PageID ; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = oldPageId;

            arParams[2] = new FbParameter("@NewPageID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = newPageId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static int CountNonAdminModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_Modules m  ");
            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
            sqlCommand.Append("WHERE m.SiteID = @SiteID ");
            sqlCommand.Append("AND md.IsAdmin = 0 ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static int CountForMyPage(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_Modules m  ");
            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static int GetCount(int siteId, int moduleDefId, string title)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_Modules m  ");
            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
            sqlCommand.Append("WHERE m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            if (title.Length > 0)
            {
                sqlCommand.Append("AND (m.ModuleTitle LIKE '%' || @Title || '%') ");
            }
            sqlCommand.Append("AND md.IsAdmin = 0 ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static int GetCountByFeature(int moduleDefId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_Modules   ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleDefID = @ModuleDefID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleDefId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;
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

            

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();


            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append("m.*,  ");
            sqlCommand.Append("md.FeatureName,  ");
            sqlCommand.Append("md.ControlSrc,  ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("COALESCE(u.Name, '') As CreatedBy,  ");
            sqlCommand.Append("(SELECT COUNT(pm.PageID) FROM mp_PageModules pm WHERE pm.ModuleID = m.ModuleID) AS UseCount  ");
            sqlCommand.Append("FROM	mp_Modules m  ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitions md  ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u  ");
            sqlCommand.Append("ON m.CreatedByUserID = u.UserID  ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            if (title.Length > 0)
            {
                sqlCommand.Append("AND (m.ModuleTitle LIKE '%' || @Title || '%') ");
            }
            sqlCommand.Append("AND md.IsAdmin = 0 ");

            if (sortByModuleType)
            {
                sqlCommand.Append("ORDER BY md.FeatureName, m.ModuleTitle ");

            }
            else if (sortByAuthor)
            {
                sqlCommand.Append("ORDER BY u.Name, m.ModuleTitle ");

            }
            else
            {
                sqlCommand.Append("   ORDER BY 	m.ModuleTitle, md.FeatureName ");

            }

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

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

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_Modules m  ");
            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID ");
            sqlCommand.Append("WHERE m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            
            sqlCommand.Append("AND m.IsGlobal = 1 ");
            sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = @PageID) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();


            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append("m.*,  ");
            sqlCommand.Append("md.FeatureName,  ");
            sqlCommand.Append("md.ControlSrc,  ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("COALESCE(u.Name, '') As CreatedBy,  ");
            sqlCommand.Append("(SELECT COUNT(pm.PageID) FROM mp_PageModules pm WHERE pm.ModuleID = m.ModuleID) AS UseCount  ");
            sqlCommand.Append("FROM	mp_Modules m  ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitions md  ");
            sqlCommand.Append("ON md.ModuleDefID = m.ModuleDefID  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u  ");
            sqlCommand.Append("ON m.CreatedByUserID = u.UserID  ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID ");
            sqlCommand.Append("AND ((m.ModuleDefID = @ModuleDefID) OR (@ModuleDefID = -1)) ");
            
            sqlCommand.Append("AND m.IsGlobal = 1 ");
            sqlCommand.Append("AND m.ModuleID NOT IN (SELECT ModuleID FROM mp_PageModules WHERE PageID = @PageID) ");

            sqlCommand.Append("   ORDER BY 	m.ModuleTitle, md.FeatureName ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ModuleDefID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleDefId;

            arParams[2] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageId;

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

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
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


        public static IDataReader GetModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("m.*,  ");
            
            sqlCommand.Append("md.ControlSrc,  ");
            sqlCommand.Append("md.FeatureName  ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.ModuleID = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("m.*,  ");
            
            sqlCommand.Append("md.ControlSrc,  ");
            sqlCommand.Append("md.FeatureName  ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.Guid = @ModuleGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetModule(int moduleId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("m.*,  ");
            sqlCommand.Append("pm.PageID,  ");
            sqlCommand.Append("pm.ModuleOrder,  ");
            sqlCommand.Append("pm.PaneName,  ");
            sqlCommand.Append("pm.PublishBeginDate,  ");
            sqlCommand.Append("pm.PublishEndDate,  ");
            sqlCommand.Append("md.ControlSrc,  ");
            sqlCommand.Append("md.FeatureName  ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE pm.PageID = @PageID ");
            sqlCommand.Append(" AND m.ModuleID = @ModuleID ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageModules(int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.*, ");
            
            sqlCommand.Append("pm.PageID,   ");
            sqlCommand.Append("pm.ModuleOrder,   ");
            sqlCommand.Append("pm.PaneName,   ");
            sqlCommand.Append("pm.PublishBeginDate,   ");
            sqlCommand.Append("pm.PublishEndDate,   ");
            sqlCommand.Append("md.ControlSrc, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.Guid AS FeatureGuid ");

            sqlCommand.Append("FROM	mp_Modules m ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("WHERE pm.PageID = @PageID ");

            sqlCommand.Append("AND pm.PublishBeginDate <= @BeginTime ");
            sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > @BeginTime) ");


            sqlCommand.Append("ORDER BY pm.ModuleOrder ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageId;

            arParams[1] = new FbParameter("@BeginTime", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = DateTime.UtcNow;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetMyPageModules(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.SiteID, ");
            sqlCommand.Append("m.ModuleDefID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.HideFromAuth,  ");
            sqlCommand.Append("m.HideFromUnAuth,  ");
            sqlCommand.Append("m.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("m.Icon As ModuleIcon, ");
            sqlCommand.Append("md.Icon As FeatureIcon, ");
            sqlCommand.Append("md.FeatureName ");
            sqlCommand.Append("FROM	mp_Modules m ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ");
            sqlCommand.Append("ORDER BY m.ModuleTitle ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AuthorizedEditRoles, ");
            sqlCommand.Append("m.EditUserID, ");
            sqlCommand.Append("p.Url, ");
            sqlCommand.Append("p.PageName, ");
            sqlCommand.Append("p.UseUrl, ");
            sqlCommand.Append("p.PageID, ");
            sqlCommand.Append("p.EditRoles ");

            sqlCommand.Append("FROM mp_Modules m ");

            sqlCommand.Append("JOIN mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN mp_Pages p ");
            sqlCommand.Append("ON pm.PageID = p.PageID ");

            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE md.Guid = @FeatureGuid ");
            sqlCommand.Append("AND m.SiteId = @SiteID ");

            sqlCommand.Append("ORDER BY p.PageName, m.ModuleTitle ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }





        

    }
}
