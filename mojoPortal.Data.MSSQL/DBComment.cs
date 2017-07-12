// Author:					
// Created:					2012-08-10
// Last Modified:			2012-08-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{

    public static class DBComments
    {
        
        /// <summary>
        /// Inserts a row in the mp_Comments table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="parentGuid"> parentGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid parentGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid contentGuid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime createdUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_Insert", 17);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@UserComment", SqlDbType.NVarChar, -1, ParameterDirection.Input, userComment);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@UserEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, userEmail);
            sph.DefineSqlParameter("@UserUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, userUrl);
            sph.DefineSqlParameter("@UserIp", SqlDbType.NVarChar, 50, ParameterDirection.Input, userIp);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@ModerationStatus", SqlDbType.TinyInt, ParameterDirection.Input, moderationStatus);
            sph.DefineSqlParameter("@ModeratedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moderatedBy);
            sph.DefineSqlParameter("@ModerationReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, moderationReason);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_Comments table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime lastModUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_Update", 12);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@UserComment", SqlDbType.NVarChar, -1, ParameterDirection.Input, userComment);
            sph.DefineSqlParameter("@UserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, userName);
            sph.DefineSqlParameter("@UserEmail", SqlDbType.NVarChar, 100, ParameterDirection.Input, userEmail);
            sph.DefineSqlParameter("@UserUrl", SqlDbType.NVarChar, 255, ParameterDirection.Input, userUrl);
            sph.DefineSqlParameter("@UserIp", SqlDbType.NVarChar, 50, ParameterDirection.Input, userIp);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@ModerationStatus", SqlDbType.TinyInt, ParameterDirection.Input, moderationStatus);
            sph.DefineSqlParameter("@ModeratedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moderatedBy);
            sph.DefineSqlParameter("@ModerationReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, moderationReason);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_DeleteByContent", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByParent(Guid parentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Comments_DeleteByParent", 1);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentAsc(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_SelectByContentAsc", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentDesc(Guid contentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_SelectByContentDesc", 1);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentAsc(Guid parentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_SelectByParentAsc", 1);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentDesc(Guid parentGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_SelectByParentDesc", 1);
            sph.DefineSqlParameter("@ParentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, parentGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the mp_Comments table.
        /// </summary>
        public static int GetCount(Guid contentGuid, int moderationStatus)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_CountByContent", 2);
            sph.DefineSqlParameter("@ContentGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, contentGuid);
            sph.DefineSqlParameter("@ModerationStatus", SqlDbType.TinyInt, ParameterDirection.Input, moderationStatus);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static int GetCountByModule(Guid moduleGuid, int moderationStatus)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_CountByModule", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModerationStatus", SqlDbType.TinyInt, ParameterDirection.Input, moderationStatus);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static int GetCountBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Comments_CountBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
     

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        

       

    }

}
