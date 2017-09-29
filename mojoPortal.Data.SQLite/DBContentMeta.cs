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

using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

namespace mojoPortal.Data
{
	public static class DBContentMeta
	{
		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

			if (connectionString == "defaultdblocation")
			{
				connectionString = "version=3,URI=file:" + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");
			}

			return connectionString;
		}


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

			sqlCommand.Append(":Guid, ");
			sqlCommand.Append(":SiteGuid, ");
			sqlCommand.Append(":ModuleGuid, ");
			sqlCommand.Append(":ContentGuid, ");
			sqlCommand.Append(":Name, ");
			sqlCommand.Append(":NameProperty, ");
			sqlCommand.Append(":Scheme, ");
			sqlCommand.Append(":LangCode, ");
			sqlCommand.Append(":Dir, ");
			sqlCommand.Append(":MetaContent, ");
			sqlCommand.Append(":ContentProperty, ");
			sqlCommand.Append(":SortRank, ");
			sqlCommand.Append(":CreatedUtc, ");
			sqlCommand.Append(":CreatedBy, ");
			sqlCommand.Append(":LastModUtc, ");
			sqlCommand.Append(":LastModBy )");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[16];

			arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = siteGuid.ToString();

			arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleGuid.ToString();

			arParams[3] = new SqliteParameter(":ContentGuid", DbType.String, 36);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = contentGuid.ToString();

			arParams[4] = new SqliteParameter(":Name", DbType.String, 255);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = name;

			arParams[5] = new SqliteParameter(":Scheme", DbType.String, 255);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = scheme;

			arParams[6] = new SqliteParameter(":LangCode", DbType.String, 10);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = langCode;

			arParams[7] = new SqliteParameter(":Dir", DbType.String, 3);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = dir;

			arParams[8] = new SqliteParameter(":MetaContent", DbType.Object);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = metaContent;

			arParams[9] = new SqliteParameter(":SortRank", DbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = sortRank;

			arParams[10] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = createdUtc;

			arParams[11] = new SqliteParameter(":CreatedBy", DbType.String, 36);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = createdBy.ToString();

			arParams[12] = new SqliteParameter(":LastModUtc", DbType.DateTime);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = createdUtc;

			arParams[13] = new SqliteParameter(":LastModBy", DbType.String, 36);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = createdBy.ToString();

			arParams[14] = new SqliteParameter(":NameProperty", DbType.String, 255);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = name;

			arParams[15] = new SqliteParameter(":ContentProperty", DbType.String, 255);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = name;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
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
			string contentProerty,
			int sortRank,
			DateTime lastModUtc,
			Guid lastModBy)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ContentMeta ");
			sqlCommand.Append("SET  ");

			sqlCommand.Append("Name = :Name, ");
			sqlCommand.Append("NameProperty = :NameProperty, ");
			sqlCommand.Append("Scheme = :Scheme, ");
			sqlCommand.Append("LangCode = :LangCode, ");
			sqlCommand.Append("Dir = :Dir, ");
			sqlCommand.Append("MetaContent = :MetaContent, ");
			sqlCommand.Append("ContentProperty = :ContentProperty, ");
			sqlCommand.Append("SortRank = :SortRank, ");
			sqlCommand.Append("LastModUtc = :LastModUtc, ");
			sqlCommand.Append("LastModBy = :LastModBy ");

			sqlCommand.Append("WHERE  ");

			sqlCommand.Append("Guid = :Guid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[11];

			arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			arParams[1] = new SqliteParameter(":Name", DbType.String, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = name;

			arParams[2] = new SqliteParameter(":Scheme", DbType.String, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = scheme;

			arParams[3] = new SqliteParameter(":LangCode", DbType.String, 10);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = langCode;

			arParams[4] = new SqliteParameter(":Dir", DbType.String, 3);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = dir;

			arParams[5] = new SqliteParameter(":MetaContent", DbType.Object);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = metaContent;

			arParams[6] = new SqliteParameter(":SortRank", DbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = sortRank;

			arParams[7] = new SqliteParameter(":LastModUtc", DbType.DateTime);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = lastModUtc;

			arParams[8] = new SqliteParameter(":LastModBy", DbType.String, 36);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = lastModBy.ToString();

			arParams[9] = new SqliteParameter(":NameProperty", DbType.String, 255);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = name;

			arParams[10] = new SqliteParameter(":ContentProperty", DbType.String, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = name;

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
			sqlCommand.Append("DELETE FROM mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("Guid = :Guid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
			sqlCommand.Append("SiteGuid = :SiteGuid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteGuid.ToString();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
			sqlCommand.Append("ModuleGuid = :ModuleGuid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleGuid.ToString();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
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
			sqlCommand.Append("ContentGuid = :ContentGuid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return (rowsAffected > 0);
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentMeta table.
		/// </summary>
		public static IDataReader GetOne(Guid guid)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_ContentMeta ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("Guid = :Guid ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = guid.ToString();

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
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
			sqlCommand.Append("ContentGuid = :ContentGuid ");
			sqlCommand.Append("ORDER BY ");
			sqlCommand.Append("SortRank ");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return SqliteHelper.ExecuteReader(
				GetConnectionString(),
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
			sqlCommand.Append("WHERE ContentGuid = :ContentGuid");
			sqlCommand.Append(";");

			SqliteParameter[] arParams = new SqliteParameter[1];

			arParams[0] = new SqliteParameter(":ContentGuid", DbType.String, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = contentGuid.ToString();

			return Convert.ToInt32(
				SqliteHelper.ExecuteScalar(
					GetConnectionString(),
					sqlCommand.ToString(),
					arParams
				)
			);
		}
	}
}