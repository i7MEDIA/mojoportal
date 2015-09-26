// Author:					Joe Audette
// Created:					2014-08-10
// Last Modified:			2014-08-10
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Data.SqlServerCe;
using mojoPortal.Data;

namespace mojoPortal.Data
{
	public static class DBUserLogins
    {

        public static bool Create(string loginProvider, string providerKey, string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_UserLogins (");
            sqlCommand.Append("LoginProvider ,");
            sqlCommand.Append("ProviderKey, ");
            sqlCommand.Append("UserId ");
            sqlCommand.Append(") ");

            sqlCommand.Append("VALUES (");
            sqlCommand.Append("@LoginProvider, ");
            sqlCommand.Append("@ProviderKey, ");
            sqlCommand.Append("@UserId ");
            sqlCommand.Append(")");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = loginProvider;

            arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = providerKey;

            arParams[2] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
	
		}
	
	
		public static bool Delete(
			string loginProvider, 
			string providerKey, 
			string userId) 
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = @LoginProvider AND ");
			sqlCommand.Append("ProviderKey = @ProviderKey AND ");
			sqlCommand.Append("UserId = @UserId "); 
			sqlCommand.Append(";");
		
			SqlCeParameter[] arParams = new SqlCeParameter[3];
			
			arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
			arParams[0].Direction = ParameterDirection.Input;	
			arParams[0].Value = loginProvider;
				
			arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
			arParams[1].Direction = ParameterDirection.Input;	
			arParams[1].Value = providerKey;
				
			arParams[2] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
			arParams[2].Direction = ParameterDirection.Input;	
			arParams[2].Value = userId;
				
			int rowsAffected = SqlHelper.ExecuteNonQuery(
				ConnectionString.GetConnectionString(), 
				CommandType.Text,
				sqlCommand.ToString(), 
				arParams);
				
			return (rowsAffected > -1);
	
		}

        public static bool DeleteByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("UserId = @UserId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_UserLogins ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("UserId IN (SELECT UserGuid FROM mp_Users WHERE SiteGuid = @SiteGuid) ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader Find(string loginProvider, string providerKey)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_UserLogins ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("LoginProvider = @LoginProvider AND ");
			sqlCommand.Append("ProviderKey = @ProviderKey  ");
			
			sqlCommand.Append(";");
	
			SqlCeParameter[] arParams = new SqlCeParameter[2];
			
			arParams[0] = new SqlCeParameter("@LoginProvider", SqlDbType.NVarChar, 128);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = loginProvider;
				
			arParams[1] = new SqlCeParameter("@ProviderKey", SqlDbType.NVarChar, 128);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = providerKey;
				
			return SqlHelper.ExecuteReader(
				ConnectionString.GetConnectionString(), 
				CommandType.Text,
				sqlCommand.ToString(), 
				arParams);
				
		}

        public static IDataReader GetByUser(string userId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_UserLogins ");
            sqlCommand.Append("WHERE ");          
            sqlCommand.Append("UserId = @UserId  ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserId", SqlDbType.NVarChar, 128);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userId;

            return SqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }
		
		
		
		
	
		
		
	}
}
