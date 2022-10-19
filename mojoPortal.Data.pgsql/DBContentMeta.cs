// Author:
// Created:       2009-12-02
// Last Modified: 2017-09-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Npgsql;
using System;
using System.Data;
using System.Text;

namespace mojoPortal.Data
{
	public static class DBContentMeta
	{
		/// <summary>
		/// Inserts a row in the mp_ContentMeta table. Returns rows affected count.
		/// </summary>
		/// <returns>int</returns>
		public static int Create(
			Guid guid,
			Guid siteGuid,
			Guid moduleGuid,
			Guid contentGuid,
			string name,
			string nameProperty,
			string scheme,
			string langCode,
			string dir,
			string metaContent,
			string contentProperty,
			int sortRank,
			DateTime createdUtc,
			Guid createdBy)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("INSERT INTO mp_contentmeta (");

			sqlCommand.Append("guid, ");
			sqlCommand.Append("siteguid, ");
			sqlCommand.Append("moduleguid, ");
			sqlCommand.Append("contentguid, ");
			sqlCommand.Append("name, ");
			sqlCommand.Append("nameproperty, ");
			sqlCommand.Append("scheme, ");
			sqlCommand.Append("langcode, ");
			sqlCommand.Append("dir, ");
			sqlCommand.Append("metacontent, ");
			sqlCommand.Append("contentproperty, ");
			sqlCommand.Append("sortrank, ");
			sqlCommand.Append("createdutc, ");
			sqlCommand.Append("createdby, ");
			sqlCommand.Append("lastmodutc, ");
			sqlCommand.Append("lastmodby )");

			sqlCommand.Append(" VALUES (");

			sqlCommand.Append(":guid, ");
			sqlCommand.Append(":siteguid, ");
			sqlCommand.Append(":moduleguid, ");
			sqlCommand.Append(":contentguid, ");
			sqlCommand.Append(":name, ");
			sqlCommand.Append(":nameproperty, ");
			sqlCommand.Append(":scheme, ");
			sqlCommand.Append(":langcode, ");
			sqlCommand.Append(":dir, ");
			sqlCommand.Append(":metacontent, ");
			sqlCommand.Append(":contentproperty, ");
			sqlCommand.Append(":sortrank, ");
			sqlCommand.Append(":createdutc, ");
			sqlCommand.Append(":createdby, ");
			sqlCommand.Append(":lastmodutc, ");
			sqlCommand.Append(":lastmodby ");
			sqlCommand.Append(")");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[16];

			arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = siteGuid.ToString();

			arParams[2] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleGuid.ToString();

			arParams[3] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = contentGuid.ToString();

			arParams[4] = new NpgsqlParameter(":name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = name;

			arParams[5] = new NpgsqlParameter(":scheme", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = scheme;

			arParams[6] = new NpgsqlParameter(":langcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = langCode;

			arParams[7] = new NpgsqlParameter(":dir", NpgsqlTypes.NpgsqlDbType.Varchar, 3);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = dir;

			arParams[8] = new NpgsqlParameter(":metacontent", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = metaContent;

			arParams[9] = new NpgsqlParameter(":sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = sortRank;

			arParams[10] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = createdUtc;

			arParams[11] = new NpgsqlParameter(":createdby", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = createdBy.ToString();

			arParams[12] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = createdUtc;

			arParams[13] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = createdBy.ToString();

			arParams[14] = new NpgsqlParameter(":nameproperty", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = name;

			arParams[15] = new NpgsqlParameter(":contentproperty", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = name;

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams);

			return rowsAffected;
		}


		/// <summary>
		/// Updates a row in the mp_ContentMeta table. Returns true if row updated.
		/// </summary>
		/// <returns>bool</returns>
		public static bool Update(
			Guid guid,
			string name,
			string nameProperty,
			string scheme,
			string langCode,
			string dir,
			string metaContent,
			string contentProperty,
			int sortRank,
			DateTime lastModUtc,
			Guid lastModBy)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_contentmeta ");
			sqlCommand.Append("SET  ");

			sqlCommand.Append("name = :name, ");
			sqlCommand.Append("nameproperty = :nameproperty, ");
			sqlCommand.Append("scheme = :scheme, ");
			sqlCommand.Append("langcode = :langcode, ");
			sqlCommand.Append("dir = :dir, ");
			sqlCommand.Append("metacontent = :metacontent, ");
			sqlCommand.Append("contentproperty = :contentproperty, ");
			sqlCommand.Append("sortrank = :sortrank, ");
			sqlCommand.Append("lastmodutc = :lastmodutc, ");
			sqlCommand.Append("lastmodby = :lastmodby ");

			sqlCommand.Append("WHERE  ");

			sqlCommand.Append("guid = :guid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[11];

			arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			arParams[1] = new NpgsqlParameter(":name", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = name;

			arParams[2] = new NpgsqlParameter(":scheme", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = scheme;

			arParams[3] = new NpgsqlParameter(":langcode", NpgsqlTypes.NpgsqlDbType.Varchar, 10);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = langCode;

			arParams[4] = new NpgsqlParameter(":dir", NpgsqlTypes.NpgsqlDbType.Varchar, 3);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = dir;

			arParams[5] = new NpgsqlParameter(":metacontent", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = metaContent;

			arParams[6] = new NpgsqlParameter(":sortrank", NpgsqlTypes.NpgsqlDbType.Integer);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = sortRank;

			arParams[7] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = lastModUtc;

			arParams[8] = new NpgsqlParameter(":lastmodby", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = lastModBy.ToString();

			arParams[9] = new NpgsqlParameter(":nameproperty", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = name;

			arParams[10] = new NpgsqlParameter(":contentproperty", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = name;

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Deletes a row from the mp_ContentMeta table. Returns true if row deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("guid = :guid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
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
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("siteguid = :siteguid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
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
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("moduleguid = :moduleguid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
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
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByContent(Guid contentGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("contentguid = :contentguid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();
			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentMeta table.
		/// </summary>
		public static IDataReader GetOne(Guid guid)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("guid = :guid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		/// <summary>
		/// Gets an IDataReader with rows from the mp_ContentMeta table.
		/// </summary>
		public static IDataReader GetByContent(Guid contentGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_contentmeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("contentguid = :contentguid ");
			sqlCommand.Append("ORDER BY ");
			sqlCommand.Append("sortrank ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams
			);
		}


		/// <summary>
		/// gets the max sort rank or 1 if null
		/// </summary>
		/// <returns>int</returns>
		public static int GetMaxSortRank(Guid contentGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT COALESCE(MAX(sortrank),1) ");
			sqlCommand.Append("FROM	mp_contentmeta ");
			sqlCommand.Append("WHERE contentguid = :contentguid");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					CommandType.Text,
					sqlCommand.ToString(),
					arParams
				)
			);
		}
	}
}