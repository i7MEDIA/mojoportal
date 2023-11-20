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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

			sqlCommand.Append("INSERT INTO mp_ContentMeta (");

			sqlCommand.Append("Guid, ");
			sqlCommand.Append("SiteGuid, ");
			sqlCommand.Append("ModuleGuid, ");
			sqlCommand.Append("ContentGuid, ");
			sqlCommand.Append("Name, ");
			sqlCommand.Append("NameProperty, ");
			sqlCommand.Append("Scheme, ");
			sqlCommand.Append("LangCode, ");
			sqlCommand.Append("Dir, ");
			sqlCommand.Append("MetaContent, ");
			sqlCommand.Append("ContentProperty, ");
			sqlCommand.Append("SortRank, ");
			sqlCommand.Append("CreatedUtc, ");
			sqlCommand.Append("CreatedBy, ");
			sqlCommand.Append("LastModUtc, ");
			sqlCommand.Append("LastModBy )");

			sqlCommand.Append(" VALUES (");

			sqlCommand.Append("?Guid, ");
			sqlCommand.Append("?SiteGuid, ");
			sqlCommand.Append("?ModuleGuid, ");
			sqlCommand.Append("?ContentGuid, ");
			sqlCommand.Append("?Name, ");
			sqlCommand.Append("?NameProperty, ");
			sqlCommand.Append("?Scheme, ");
			sqlCommand.Append("?LangCode, ");
			sqlCommand.Append("?Dir, ");
			sqlCommand.Append("?MetaContent, ");
			sqlCommand.Append("?ContentProperty, ");
			sqlCommand.Append("?SortRank, ");
			sqlCommand.Append("?CreatedUtc, ");
			sqlCommand.Append("?CreatedBy, ");
			sqlCommand.Append("?LastModUtc, ");
			sqlCommand.Append("?LastModBy )");
			sqlCommand.Append(";");

			var sqlParams = new List<MySqlParameter>()
			{
				new("?Guid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
				new("?SiteGuid", MySqlDbType.VarChar, 36) {Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new("?ModuleGuid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = moduleGuid.ToString() },
				new("?ContentGuid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = contentGuid.ToString() },
				new("?Name", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = name },
				new("?Scheme", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = scheme },
				new("?LangCode", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = langCode },
				new("?Dir", MySqlDbType.VarChar, 3) { Direction = ParameterDirection.Input, Value = dir},
				new("?MetaContent", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = metaContent},
				new("?SortRank", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortRank },
				new("?CreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc},
				new("?CreatedBy", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = createdBy.ToString()},
				new("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = createdUtc },
				new("?LastModBy", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = createdBy.ToString() },
				new("?NameProperty", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = nameProperty },
				new("?ContentProperty", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = contentProperty }
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

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
			var sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ContentMeta ");
			sqlCommand.Append("SET  ");

			sqlCommand.Append("Name = ?Name, ");
			sqlCommand.Append("NameProperty = ?NameProperty, ");
			sqlCommand.Append("Scheme = ?Scheme, ");
			sqlCommand.Append("LangCode = ?LangCode, ");
			sqlCommand.Append("Dir = ?Dir, ");
			sqlCommand.Append("MetaContent = ?MetaContent, ");
			sqlCommand.Append("ContentProperty = ?ContentProperty, ");
			sqlCommand.Append("SortRank = ?SortRank, ");
			sqlCommand.Append("LastModUtc = ?LastModUtc, ");
			sqlCommand.Append("LastModBy = ?LastModBy ");

			sqlCommand.Append("WHERE  ");

			sqlCommand.Append("Guid = ?Guid ");
			sqlCommand.Append(";");

			var sqlParams = new List<MySqlParameter>()
			{
				new("?Guid", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
				new("?Name", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = name },
				new("?Scheme", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = scheme },
				new("?LangCode", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = langCode },
				new("?Dir", MySqlDbType.VarChar, 3) { Direction = ParameterDirection.Input, Value = dir},
				new("?MetaContent", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = metaContent},
				new("?SortRank", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortRank },
				new("?LastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = lastModUtc },
				new("?LastModBy", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = lastModBy.ToString() },
				new("?NameProperty", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = nameProperty },
				new("?ContentProperty", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = contentProperty }
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray()
			);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Deletes a row from the mp_ContentMeta table. Returns true if row deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("Guid = ?Guid ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SiteGuid = ?SiteGuid ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteGuid.ToString();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleGuid.ToString();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByContent(Guid contentGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ContentGuid = ?ContentGuid ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ContentGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentMeta table.
		/// </summary>
		/// <param name="guid"> guid </param>
		public static IDataReader GetOne(Guid guid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("Guid = ?Guid ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
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
			sqlCommand.Append("FROM	mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ContentGuid = ?ContentGuid ");
			sqlCommand.Append("ORDER BY ");
			sqlCommand.Append("SortRank ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ContentGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
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
			sqlCommand.Append("SELECT COALESCE(MAX(SortRank),1) ");
			sqlCommand.Append("FROM	mp_ContentMeta ");
			sqlCommand.Append("WHERE ContentGuid = ?ContentGuid");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ContentGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand.ToString(),
					arParams
				)
			);
		}
	}
}