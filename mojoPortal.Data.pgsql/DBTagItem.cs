// Author:					Joe Audette
// Created:					2011-10-29
// Last Modified:			2012-08-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.IO;
using Npgsql;
using NpgsqlTypes;
using mojoPortal.Data; 

namespace mojoPortal.Data
{
    public static class DBTagItem
    {
       
        /// <summary>
        /// Inserts a row in the mp_TagItem table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="tagGuid"> tagGuid </param>
        /// <param name="extraGuid"> extraGuid </param>
        /// <param name="taggedBy"> taggedBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid tagGuid,
            Guid extraGuid,
            Guid taggedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_tagitem (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("tagguid, ");
            sqlCommand.Append("extraguid, ");
            sqlCommand.Append("taggedby )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":itemguid, ");
            sqlCommand.Append(":tagguid, ");
            sqlCommand.Append(":extraguid, ");
            sqlCommand.Append(":taggedby ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[8];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid.ToString();

            arParams[3] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = itemGuid.ToString();

            arParams[5] = new NpgsqlParameter("tagguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tagGuid.ToString();

            arParams[6] = new NpgsqlParameter("extraguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = extraGuid.ToString();

            arParams[7] = new NpgsqlParameter("taggedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = taggedBy.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


       
        /// <summary>
        /// Deletes a row from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("itemguid = :itemguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByExtraGuid(Guid extraGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("extraguid = :extraguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("extraguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = extraGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }


        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByTag(Guid tagGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("tagguid = :tagguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("tagguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = tagGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("featureguid = :featureguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes rows from the mp_TagItem table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tagitem ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

    }
}
