/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2018-10-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using Npgsql;
using System.Collections.Generic;
using NpgsqlTypes;


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
            string description)
        {
            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Input,
                    Value = siteId
                },
                new NpgsqlParameter(":rolename", NpgsqlTypes.NpgsqlDbType.Text, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleName
                },
                new NpgsqlParameter(":displayName", NpgsqlTypes.NpgsqlDbType.Text, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = displayName
                },
                new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Text, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = description
                },
                new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
                {
                    Direction = ParameterDirection.Input,
                    Value = siteGuid.ToString()
                },
                new NpgsqlParameter(":roleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleGuid.ToString()
                }
        };
            
            

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
                    CommandType.StoredProcedure,
                    "mp_roles_insert(:siteid,:rolename,:displayName,:description,:siteguid,:roleguid)",
                    sqlParams.ToArray()).ToString());

            return newID;

        }

        public static bool Update(int roleId, string displayname, string description)
        {
            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Input,
                    Value = roleId
                },
             
                 new NpgsqlParameter(":displayname", NpgsqlTypes.NpgsqlDbType.Text, 50)
                {
                    Direction = ParameterDirection.Input,
                    Value = displayname
                },
                new NpgsqlParameter(":description", NpgsqlTypes.NpgsqlDbType.Text, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = description
                },


            };


            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_update(:roleid,:displayname,:description)",
                sqlParams.ToArray()).ToString());
            return (rowsAffected > -1);
        }

        public static bool Delete(int roleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_delete(:roleid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteUserRoles(int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;
           
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userroles_deleteuserroles(:userid)",
                arParams));

            return (rowsAffected > -1);
        }

        public static IDataReader GetById(int roleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_selectone(:roleid)",
                arParams);
        }

        public static IDataReader GetByName(int siteId, string roleName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":rolename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_selectbyname(:siteid,:rolename)",
                arParams);
        }

        public static bool Exists(int siteId, string roleName)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":rolename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleName;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_roleexists(:siteid,:rolename)",
                arParams));

            return (count > 0);

        }

        public static IDataReader GetSiteRoles(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.description, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid, ");
            sqlCommand.Append("COUNT(ur.userid) As membercount ");

            sqlCommand.Append("FROM	mp_roles r ");

            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON ur.roleid = r.roleid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("r.siteid = :siteid ");

            sqlCommand.Append("GROUP BY ");
            sqlCommand.Append("r.roleid, ");
            sqlCommand.Append("r.siteid, ");
            sqlCommand.Append("r.rolename, ");
            sqlCommand.Append("r.displayname, ");
            sqlCommand.Append("r.siteguid, ");
            sqlCommand.Append("r.roleguid ");

            sqlCommand.Append("ORDER BY r.displayname ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }


		public static IDataReader GetRolesByUsername(string username, int siteId)
		{
			var sqlCommand = @"
SELECT
	r.roleid,
	r.siteid,
	r.rolename,
	r.displayname,
	r.siteguid,
	r.roleguid,
	r.description
FROM mp_userroles ur
INNER JOIN mp_users u ON ur.userid = u.userid
INNER JOIN mp_roles r ON ur.roleid = r.roleid
WHERE u.siteid = :siteid
AND (u.loginname = :username OR u.email = :username);";

			var arParams = new NpgsqlParameter[]
			{
				new(":siteid", NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = siteId
				},
				new(":username", NpgsqlDbType.Text, 225)
				{
					Direction = ParameterDirection.Input,
					Value = username.ToLower(),
				},
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams
			);
		}


		public static IDataReader GetRoleMembers(int roleId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("mp_userroles.userid, ");
            sqlCommand.Append("mp_users.name, ");
            sqlCommand.Append("mp_users.loginname, ");
            sqlCommand.Append("mp_users.email ");

            sqlCommand.Append("FROM	mp_userroles ");
            sqlCommand.Append("INNER JOIN mp_users ");
            sqlCommand.Append("ON mp_users.userid = mp_userroles.userid ");

            sqlCommand.Append("WHERE mp_userroles.roleid = :roleid ; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            return NpgsqlHelper.ExecuteReader(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams);

        }

        public static int GetCountOfUsersNotInRole(int siteId, int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");
            
            sqlCommand.Append("FROM	mp_users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");
            sqlCommand.Append("AND ur.roleid IS NULL  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
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
            sqlCommand.Append("u.userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.loginname ");

            sqlCommand.Append("FROM	mp_users u ");
            sqlCommand.Append("LEFT OUTER JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");
            sqlCommand.Append("AND ur.roleid IS NULL  ");
            sqlCommand.Append("ORDER BY u.name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams);
           
            
        }

        public static int GetCountOfUsersInRole(int siteId, int roleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");

            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("JOIN mp_userroles ur ");
            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
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
            sqlCommand.Append("u.userid, ");
            sqlCommand.Append("u.name, ");
            sqlCommand.Append("u.email, ");
            sqlCommand.Append("u.loginname ");

            sqlCommand.Append("FROM	mp_users u ");

            sqlCommand.Append("JOIN mp_userroles ur ");

            sqlCommand.Append("ON u.userid = ur.userid ");
            sqlCommand.Append("AND ur.roleid = :roleid ");

            sqlCommand.Append("WHERE u.siteid = :siteid  ");

            sqlCommand.Append("ORDER BY u.name  ");
            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = roleId;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams);


        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_roles_selectrolesuserisnotin(:siteid,:userid)",
                arParams);
        }

        public static bool AddUser(
            int roleId,
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];
            
            arParams[0] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;

            arParams[2] = new NpgsqlParameter(":roleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = roleGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();
           
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userroles_insert(:roleid,:userid,:roleguid,:userguid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool RemoveUser(int roleId, int userId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter(":roleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = roleId;

            arParams[1] = new NpgsqlParameter(":userid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userId;
            
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_userroles_delete(:roleid,:userid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static int GetCountOfSiteRoles(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("COUNT(*) ");
            sqlCommand.Append("FROM	mp_roles ");
            sqlCommand.Append("WHERE siteid = :siteid  ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams));
        }

    }
}
