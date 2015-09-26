// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2011-01-19
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
    public static class DBUserPage
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int AddUserPage(
            Guid userPageId,
            Guid siteGuid,
            int siteId,
            Guid userGuid,
            string pageName,
            String pagePath,
            int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserPages ");
            sqlCommand.Append("(");
            sqlCommand.Append("UserPageID, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("PageName, ");
            sqlCommand.Append("PagePath, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("SiteGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@UserPageID, ");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@PageName, ");
            sqlCommand.Append("@PagePath, ");
            sqlCommand.Append("@PageOrder, ");
            sqlCommand.Append("@SiteGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@UserPageID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userPageId;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid;

            arParams[3] = new SqlCeParameter("@PageName", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageName;

            arParams[4] = new SqlCeParameter("@PagePath", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pagePath;

            arParams[5] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = pageOrder;

            arParams[6] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool UpdateUserPage(
            Guid userPageId,
            string pageName,
            int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_UserPages ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("PageName = @PageName, ");
            sqlCommand.Append("PageOrder = @PageOrder ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserPageID = @UserPageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@UserPageID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userPageId;

            arParams[1] = new SqlCeParameter("@PageName", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageName;

            arParams[2] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageOrder;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteUserPage(Guid userPageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserPageID = @UserPageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserPageID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userPageId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetUserPage(Guid userPageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserPageID = @UserPageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserPageID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userPageId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader SelectByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("PageOrder ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetNextPageOrder(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(PageOrder), -1) + 2 ");
            sqlCommand.Append("FROM	mp_UserPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static bool UpdatePageOrder(Guid userPageId, int pageOrder)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_UserPages ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("PageOrder = @PageOrder ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("UserPageID = @UserPageID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@UserPageID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userPageId;

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


    }
}
