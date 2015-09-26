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
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    public static class DBTag
    {
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
            sqlCommand.Append("INSERT INTO mp_tag (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("vocabularyguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("tag, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("createdby, ");
            sqlCommand.Append("modifiedutc, ");
            sqlCommand.Append("modifiedby, ");
            sqlCommand.Append("itemcount )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":vocabularyguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":tag, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":createdby, ");
            sqlCommand.Append(":modifiedutc, ");
            sqlCommand.Append(":modifiedby, ");
            sqlCommand.Append(":itemcount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("vocabularyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid.ToString();

            arParams[2] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new NpgsqlParameter("featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new NpgsqlParameter("tag", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tag;

            arParams[6] = new NpgsqlParameter("createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new NpgsqlParameter("createdby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy.ToString();

            arParams[8] = new NpgsqlParameter("modifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            arParams[9] = new NpgsqlParameter("modifiedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy.ToString();

            arParams[10] = new NpgsqlParameter("itemcount", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = 0;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_tag ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("vocabularyguid = :vocabularyguid, ");
            sqlCommand.Append("tag = :tag, ");
            sqlCommand.Append("modifiedutc = :modifiedutc, ");
            sqlCommand.Append("modifiedby = :modifiedby ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter("vocabularyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid.ToString();

            arParams[2] = new NpgsqlParameter("tag", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = tag;

            arParams[3] = new NpgsqlParameter("modifiedutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = modifiedUtc;

            arParams[4] = new NpgsqlParameter("modifiedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = modifiedBy.ToString();

          
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("itemcount = ( ");
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_tagitem ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("tagguid = :guid) ");
            sqlCommand.Append("WHERE  ");
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
        /// Deletes a row from the mp_Tag table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tag ");
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

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tag ");
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


        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tag ");
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

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_tag ");
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

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Tag table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("tag ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }


    }
}
