// Author:					Joe Audette
// Created:					2010-04-02
// Last Modified:			2010-04-02
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
    public static class DBContentMetaLink
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_ContentMetaLink table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="rel"> rel </param>
        /// <param name="href"> href </param>
        /// <param name="hrefLang"> hrefLang </param>
        /// <param name="rev"> rev </param>
        /// <param name="type"> type </param>
        /// <param name="media"> media </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid contentGuid,
            string rel,
            string href,
            string hrefLang,
            string rev,
            string type,
            string media,
            int sortRank,
            DateTime createdUtc,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_ContentMetaLink ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ContentGuid, ");
            sqlCommand.Append("Rel, ");
            sqlCommand.Append("Href, ");
            sqlCommand.Append("HrefLang, ");
            sqlCommand.Append("Rev, ");
            sqlCommand.Append("Type, ");
            sqlCommand.Append("Media, ");
            sqlCommand.Append("SortRank, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@ContentGuid, ");
            sqlCommand.Append("@Rel, ");
            sqlCommand.Append("@Href, ");
            sqlCommand.Append("@HrefLang, ");
            sqlCommand.Append("@Rev, ");
            sqlCommand.Append("@Type, ");
            sqlCommand.Append("@Media, ");
            sqlCommand.Append("@SortRank, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@LastModBy ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[15];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = contentGuid;

            arParams[4] = new SqlCeParameter("@Rel", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rel;

            arParams[5] = new SqlCeParameter("@Href", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = href;

            arParams[6] = new SqlCeParameter("@HrefLang", SqlDbType.NVarChar, 10);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = hrefLang;

            arParams[7] = new SqlCeParameter("@Rev", SqlDbType.NVarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = rev;

            arParams[8] = new SqlCeParameter("@Type", SqlDbType.NVarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = type;

            arParams[9] = new SqlCeParameter("@Media", SqlDbType.NVarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = media;

            arParams[10] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = sortRank;

            arParams[11] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdUtc;

            arParams[12] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdBy;

            arParams[13] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_ContentMetaLink table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="rel"> rel </param>
        /// <param name="href"> href </param>
        /// <param name="hrefLang"> hrefLang </param>
        /// <param name="rev"> rev </param>
        /// <param name="type"> type </param>
        /// <param name="media"> media </param>
        /// <param name="sortRank"> sortRank </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string rel,
            string href,
            string hrefLang,
            string rev,
            string type,
            string media,
            int sortRank,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_ContentMetaLink ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Rel = @Rel, ");
            sqlCommand.Append("Href = @Href, ");
            sqlCommand.Append("HrefLang = @HrefLang, ");
            sqlCommand.Append("Rev = @Rev, ");
            sqlCommand.Append("Type = @Type, ");
            sqlCommand.Append("Media = @Media, ");
            sqlCommand.Append("SortRank = @SortRank, ");
           
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("LastModBy = @LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Rel", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = rel;

            arParams[2] = new SqlCeParameter("@Href", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = href;

            arParams[3] = new SqlCeParameter("@HrefLang", SqlDbType.NVarChar, 10);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = hrefLang;

            arParams[4] = new SqlCeParameter("@Rev", SqlDbType.NVarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rev;

            arParams[5] = new SqlCeParameter("@Type", SqlDbType.NVarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = type;

            arParams[6] = new SqlCeParameter("@Media", SqlDbType.NVarChar, 50);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = media;

            arParams[7] = new SqlCeParameter("@SortRank", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = sortRank;

            arParams[8] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_ContentMetaLink table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
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

        /// <summary>
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="siteGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
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
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="moduleGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
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

        /// <summary>
        /// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
        /// </summary>
        /// <param name="contentGuid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContentMetaLink table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
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

        /// <summary>
        /// Gets an IDataReader with rows from the mp_ContentMetaLink table.
        /// </summary>
        /// <param name="contentGuid"> guid </param>
        public static IDataReader GetByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SortRank ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetMaxSortRank(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  COALESCE(MAX(SortRank), 1) ");
            sqlCommand.Append("FROM	mp_ContentMetaLink ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContentGuid = @ContentGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ContentGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

    }
}
