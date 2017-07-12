// Author:					
// Created:					2009-02-22
// Last Modified:			2012-08-30
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
using mojoPortal.Data;

namespace mojoPortal.Data
{

    public static class DBEmailTemplate
    {
        


        /// <summary>
        /// Inserts a row in the mp_EmailTemplate table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime createdUtc,
            Guid lastModBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_Insert", 15);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@TextBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, textBody);
            sph.DefineSqlParameter("@HtmlBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, htmlBody);
            sph.DefineSqlParameter("@HasHtml", SqlDbType.Bit, ParameterDirection.Input, hasHtml);
            sph.DefineSqlParameter("@IsEditable", SqlDbType.Bit, ParameterDirection.Input, isEditable);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_EmailTemplate table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, subject);
            sph.DefineSqlParameter("@TextBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, textBody);
            sph.DefineSqlParameter("@HtmlBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, htmlBody);
            sph.DefineSqlParameter("@HasHtml", SqlDbType.Bit, ParameterDirection.Input, hasHtml);
            sph.DefineSqlParameter("@IsEditable", SqlDbType.Bit, ParameterDirection.Input, isEditable);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_DeleteByFeature", 1);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySpecial1(Guid specialGuid1)
        {
            if (specialGuid1 == Guid.Empty) { return false; }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_DeleteBySpecialGuid1", 1);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySpecial2(Guid specialGuid2)
        {
            if (specialGuid2 == Guid.Empty) { return false; }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_EmailTemplate_DeleteBySpecialGuid2", 1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();

        }

        public static IDataReader Get(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_Select", 5);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);

            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_SelectByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_SelectByFeature", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByModule(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_SelectByModuleSpecial", 3);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);
            return sph.ExecuteReader();

        }

        public static int GetCount(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_GetCount", 5);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);

            return Convert.ToInt32(sph.ExecuteScalar());


        }

        /// <summary>
        /// Gets a count of rows in the mp_EmailTemplate table.
        /// </summary>
        public static int GetCountByModuleAndName(Guid moduleGuid, string name)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_GetCountByModuleAndName", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        /// <summary>
        /// Gets a count of rows in the mp_EmailTemplate table.
        /// </summary>
        public static int GetCountByModuleSpecialAndName(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2, string name)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_GetCountByModuleAndName", 4);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid1);
            sph.DefineSqlParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier, ParameterDirection.Input, specialGuid2);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);

            return Convert.ToInt32(sph.ExecuteScalar());

        }

        public static int GetCountByFeature(Guid siteGuid, Guid featureGuid)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_GetCountByFeature", 2);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            
            return Convert.ToInt32(sph.ExecuteScalar());
            

        }

        public static IDataReader GetPageByFeature(
            Guid siteGuid, 
            Guid featureGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows = GetCountByFeature(siteGuid, featureGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_EmailTemplate_SelectPageByFeature", 4);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        

        

    }

}
