using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace mojoPortal.Data
{
    
    public static class DBRoles
    {

        public static int RoleCreate(
            Guid roleGuid,
            Guid siteGuid,
            int siteId,
            string roleName,
            string displayName,
            string description
        )
        {
            var sqlCommand = @"
            INSERT INTO mp_Roles (
                SiteID, 
                RoleName, 
                DisplayName, 
                Description, 
                SiteGuid, 
                RoleGuid )
            VALUES (
                ?SiteID,
                ?RoleName,
                ?DisplayName,
                ?Description,
                ?SiteGuid,
                ?RoleGuid 
            );
            SELECT LAST_INSERT_ID();";

            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?SiteID", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = siteId
                },

                new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleName
                },
                new MySqlParameter("?DisplayName", MySqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = displayName
                },
                new MySqlParameter("?Description", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = description
                },
                new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
                {
                    Direction = ParameterDirection.Input,
                    Value = siteGuid.ToString()
                },
                new MySqlParameter("?RoleGuid", MySqlDbType.VarChar, 36)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleGuid.ToString()
                }
            };

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());

            return newID;
        }

        public static bool Update(int roleId, string displayName, string description)
        {
            var sqlCommand = @"
                UPDATE mp_Roles ( 
                SET DisplayName = ?DisplayName,
                SET Description = ?Description  
                WHERE RoleID = ?RoleID);  ";


            var sqlParams = new List<MySqlParameter>
            {
                new MySqlParameter("?RoleId", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleId
                },
                new MySqlParameter("?DisplayName", MySqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = displayName
                },
                new MySqlParameter("?Description", MySqlDbType.VarChar, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = description
                },
            };

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                sqlParams.ToArray());

            return (rowsAffected > 0);
        }

        public static bool Delete(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Roles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID AND RoleName <> 'Admins' AND RoleName <> 'Content Administrators' AND RoleName <> 'Authenticated Users' AND RoleName <> 'Role Admins'  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool DeleteUserRoles(int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = ?UserID  ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetById(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByName(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND RoleName = ?RoleName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool Exists(int siteId, string roleName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID AND RoleName = ?RoleName ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleName", MySqlDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GetSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, "); 
            sqlCommand.Append("r.Description, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid, ");
            sqlCommand.Append("COUNT(ur.UserID) As MemberCount ");

            sqlCommand.Append("FROM	mp_Roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON ur.RoleID = r.RoleID ");

            sqlCommand.Append("WHERE r.SiteID = ?SiteID  ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.RoleID, ");
            sqlCommand.Append("r.SiteID, ");
            sqlCommand.Append("r.RoleName, ");
            sqlCommand.Append("r.DisplayName, ");
            sqlCommand.Append("r.SiteGuid, ");
            sqlCommand.Append("r.RoleGuid ");

            sqlCommand.Append("ORDER BY r.DisplayName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetRoleMembers(int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_UserRoles.UserID, ");
            sqlCommand.Append("mp_Users.Name, ");
            sqlCommand.Append("mp_Users.LoginName, ");
            sqlCommand.Append("mp_Users.Email ");

            sqlCommand.Append("FROM	mp_UserRoles ");
            sqlCommand.Append("INNER JOIN mp_Users ");
            sqlCommand.Append("ON mp_Users.UserID = mp_UserRoles.UserID ");

            sqlCommand.Append("WHERE mp_UserRoles.RoleID = ?RoleID ; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountOfUsersNotInRole(int siteId, int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");
            
            sqlCommand.Append("FROM	mp_Users u ");
            
            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID NOT IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");
  
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetUsersNotInRole(
            int siteId,
            int roleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountOfUsersNotInRole(siteId, roleId);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");
            
            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID NOT IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");
  
            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountOfUsersInRole(int siteId, int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetUsersInRole(
            int siteId,
            int roleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountOfUsersInRole(siteId, roleId);

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.Name, ");
            sqlCommand.Append("u.Email, ");
            sqlCommand.Append("u.LoginName ");

            sqlCommand.Append("FROM	mp_Users u ");

            sqlCommand.Append("WHERE u.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("u.UserID IN (");
            sqlCommand.Append("SELECT UserID FROM mp_UserRoles ");
            sqlCommand.Append("WHERE RoleID = ?RoleID ");
            sqlCommand.Append(")");

            sqlCommand.Append("ORDER BY u.Name  ");

            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT r.* ");
            sqlCommand.Append("FROM	mp_Roles r ");
            sqlCommand.Append("LEFT OUTER JOIN mp_UserRoles ur ");
            sqlCommand.Append("ON r.RoleID = ur.RoleID ");
            sqlCommand.Append("AND ur.UserID = ?UserID ");

            sqlCommand.Append("WHERE r.SiteID = ?SiteID  ");
            sqlCommand.Append("AND ur.UserID IS NULL  ");
            sqlCommand.Append("ORDER BY r.DisplayName  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            //MS SQL proc checks that no matching record exists, may need to check that
            //here 
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserRoles (UserID, RoleID, RoleGuid, UserGuid) ");
            sqlCommand.Append("VALUES ( ?UserID , ?RoleID, ?RoleGuid, ?UserGuid); ");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new MySqlParameter("?RoleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = roleGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool RemoveUser(int roleId, int userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserRoles ");
            sqlCommand.Append("WHERE UserID = ?UserID  ");
            sqlCommand.Append("AND RoleID = ?RoleID  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?RoleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("Count(*) ");
            sqlCommand.Append("FROM	mp_Roles ");
            sqlCommand.Append("WHERE SiteID = ?SiteID  ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

    }
}
