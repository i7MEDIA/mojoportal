// Author:					Joe Audette
// Created:					2011-10-29
// Last Modified:			2012-05-29
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
    public static class DBTag
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }



        /// <summary>
        /// Inserts a row in the mp_Tag table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="tag"> tag </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid vocabularyGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            string tag,
            DateTime createdUtc,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Tag ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("VocabularyGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Tag, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy, ");
            sqlCommand.Append("ItemCount ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@VocabularyGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@Tag, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@ModifiedUtc, ");
            sqlCommand.Append("@ModifiedBy, ");
            sqlCommand.Append("@ItemCount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid;

            arParams[4] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid;

            arParams[5] = new SqlCeParameter("@Tag", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tag;

            arParams[6] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy;

            arParams[8] = new SqlCeParameter("@ModifiedUtc", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            arParams[9] = new SqlCeParameter("@ModifiedBy", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy;

            arParams[10] = new SqlCeParameter("@ItemCount", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = 0;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }



        /// <summary>
        /// Updates a row in the mp_Tag table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>  
        /// <param name="tag"> tag </param>
        /// <param name="modifiedUtc"> modifiedUtc </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid vocabularyGuid,
            string tag,
            DateTime modifiedUtc,
            Guid modifiedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Tag ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("VocabularyGuid = @VocabularyGuid, ");
            sqlCommand.Append("Tag = @Tag, ");
            sqlCommand.Append("ModifiedUtc = @ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy = @ModifiedBy ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@VocabularyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid;

            arParams[2] = new SqlCeParameter("@Tag", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = tag;

            arParams[3] = new SqlCeParameter("@ModifiedUtc", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = modifiedUtc;

            arParams[4] = new SqlCeParameter("@ModifiedBy", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = modifiedBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateItemCount(Guid guid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Tag ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ItemCount = @ItemCount ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            int count = DBTagItem.GetCountByTag(guid);

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@ItemCount", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = count;

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }


        /// <summary>
        /// Deletes a row from the mp_Tag table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Tag table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Tag ");
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

        /// <summary>
        /// Gets a count of rows in the mp_Tag table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }


    }
}
