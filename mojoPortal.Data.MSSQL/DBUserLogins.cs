// Author:					
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

namespace mojoPortal.Data
{
	
	public static class DBUserLogins
    {


		/// <summary>
		/// Inserts a row in the mp_UserLogins table. Returns new integer id.
		/// </summary>
		/// <returns>int</returns>
        public static bool Create(string loginProvider, string providerKey, string userId) 
		{
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_Insert", 3);
            sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
			sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);

			//int newID = Convert.ToInt32(sph.ExecuteScalar());
			//return newID;
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes a row from the mp_UserLogins table. Returns true if row deleted.
		/// </summary>
		/// <param name="loginProvider"> loginProvider </param>
		/// <param name="providerKey"> providerKey </param>
		/// <param name="userId"> userId </param>
		/// <returns>bool</returns>
		public static bool Delete(string loginProvider, string providerKey, string userId) 
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_Delete", 3);
			sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
			sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);
			
		}

        public static bool DeleteByUser(string userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_DeleteByUser", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_UserLogins_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static IDataReader Find(string loginProvider, string providerKey)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserLogins_Find", 2);
            sph.DefineSqlParameter("@LoginProvider", SqlDbType.NVarChar, 128, ParameterDirection.Input, loginProvider);
            sph.DefineSqlParameter("@ProviderKey", SqlDbType.NVarChar, 128, ParameterDirection.Input, providerKey);
            return sph.ExecuteReader();

        }
		
		
		public static IDataReader GetByUser(string  userId)  
		{
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_UserLogins_SelectByUser", 1);
			sph.DefineSqlParameter("@UserId", SqlDbType.NVarChar, 128, ParameterDirection.Input, userId);
			return sph.ExecuteReader();
			
		}
		
        
	
	}

}
