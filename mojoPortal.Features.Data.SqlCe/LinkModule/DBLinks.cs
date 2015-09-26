// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2010-07-05
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
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBLinks
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int AddLink(
            Guid itemGuid,
            Guid moduleGuid,
            int moduleId,
            string title,
            string url,
            int viewOrder,
            string description,
            DateTime createdDate,
            int createdBy,
            string target,
            Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Links ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("Target, ");
            sqlCommand.Append("ViewOrder, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("CreatedDate, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Url, ");
            sqlCommand.Append("@Target, ");
            sqlCommand.Append("@ViewOrder, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@CreatedDate, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = url;

            arParams[3] = new SqlCeParameter("@Target", SqlDbType.NVarChar, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = target;

            arParams[4] = new SqlCeParameter("@ViewOrder", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = viewOrder;

            arParams[5] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = description;

            arParams[6] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDate;

            arParams[7] = new SqlCeParameter("@CreatedBy", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy;

            arParams[8] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid;

            arParams[9] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid;

            arParams[10] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


        }

        public static bool UpdateLink(
          int itemId,
          int moduleId,
          string title,
          string url,
          int viewOrder,
          string description,
          DateTime createdDate,
          string target,
          int createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Links ");
            sqlCommand.Append("SET  ");
            //sqlCommand.Append("ModuleID = @ModuleID, ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Url = @Url, ");
            sqlCommand.Append("Target = @Target, ");
            sqlCommand.Append("ViewOrder = @ViewOrder, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("CreatedDate = @CreatedDate, ");
            sqlCommand.Append("CreatedBy = @CreatedBy ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[9];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Url", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new SqlCeParameter("@Target", SqlDbType.NVarChar, 20);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = target;

            arParams[5] = new SqlCeParameter("@ViewOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = viewOrder;

            arParams[6] = new SqlCeParameter("@Description", SqlDbType.NVarChar);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new SqlCeParameter("@CreatedDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdDate;

            arParams[8] = new SqlCeParameter("@CreatedBy", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteLink(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Links ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Links ");
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

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Links WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetLink(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Links ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetLinks(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Links ");

            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY ViewOrder, Title ;");

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

        public static IDataReader GetLinksByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_Links ce ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ce.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

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

        /// <summary>
        /// Gets a count of rows in the mp_Links table.
        /// </summary>
        public static int GetCount(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Links ");
            sqlCommand.Append("WHERE ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            int moduleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleId);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_Links  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("ViewOrder, Title  ");

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

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


    }
}
