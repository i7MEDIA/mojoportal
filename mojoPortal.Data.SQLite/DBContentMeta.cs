using Mono.Data.Sqlite;
using System;
using System.Configuration;
using System.Data;
using System.Web.Hosting;

namespace mojoPortal.Data
{
	public static class DBContentMeta
	{
		private static string GetConnectionString()
		{
			string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];

			if (connectionString == "defaultdblocation")
			{
				connectionString = "version=3,URI=file:" + HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");
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
			Guid createdBy
		)
		{
			var sqlCommand = $@"
INSERT INTO mp_ContentMeta (
	Guid,
	SiteGuid,
	ModuleGuid,
	ContentGuid,
	Name,
	NameProperty,
	Scheme,
	LangCode,
	Dir,
	MetaContent,
	ContentProperty,
	SortRank,
	CreatedUtc,
	CreatedBy,
	LastModUtc,
	LastModBy
)
VALUES (
	:Guid,
	:SiteGuid,
	:ModuleGuid,
	:ContentGuid,
	:Name,
	:NameProperty,
	:Scheme,
	:LangCode,
	:Dir,
	:MetaContent,
	:ContentProperty,
	:SortRank,
	:CreatedUtc,
	:CreatedBy,
	:LastModUtc,
	:LastModBy
);";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new SqliteParameter(":ContentGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				},
				new SqliteParameter(":Name", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = name
				},
				new SqliteParameter(":Scheme", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = scheme
				},
				new SqliteParameter(":LangCode", DbType.String, 10)
				{
					Direction = ParameterDirection.Input,
					Value = langCode
				},
				new SqliteParameter(":Dir", DbType.String, 3)
				{
					Direction = ParameterDirection.Input,
					Value = dir
				},
				new SqliteParameter(":MetaContent", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = metaContent
				},
				new SqliteParameter(":SortRank", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortRank
				},
				new SqliteParameter(":CreatedUtc", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new SqliteParameter(":CreatedBy", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new SqliteParameter(":LastModUtc", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},
				new SqliteParameter(":LastModBy", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},
				new SqliteParameter(":NameProperty", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = nameProperty
				},
				new SqliteParameter(":ContentProperty", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = contentProperty
				}
			};

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
			string contentProperty,
			int sortRank,
			DateTime lastModUtc,
			Guid lastModBy
		)
		{
			var sqlCommand = $@"
UPDATE mp_ContentMeta SET
	Name = :Name,
	NameProperty = :NameProperty,
	Scheme = :Scheme,
	LangCode = :LangCode,
	Dir = :Dir,
	MetaContent = :MetaContent,
	ContentProperty = :ContentProperty,
	SortRank = :SortRank,
	LastModUtc = :LastModUtc,
	LastModBy = :LastModBy
WHERE Guid = :Guid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},
				new SqliteParameter(":Name", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = name
				},
				new SqliteParameter(":Scheme", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = scheme
				},
				new SqliteParameter(":LangCode", DbType.String, 10)
				{
					Direction = ParameterDirection.Input,
					Value = langCode
				},
				new SqliteParameter(":Dir", DbType.String, 3)
				{
					Direction = ParameterDirection.Input,
					Value = dir
				},
				new SqliteParameter(":MetaContent", DbType.Object)
				{
					Direction = ParameterDirection.Input,
					Value = metaContent
				},
				new SqliteParameter(":SortRank", DbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortRank
				},
				new SqliteParameter(":LastModUtc", DbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = lastModUtc
				},
				new SqliteParameter(":LastModBy", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = lastModBy.ToString()
				},
				new SqliteParameter(":NameProperty", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = nameProperty
				},
				new SqliteParameter(":ContentProperty", DbType.String, 255)
				{
					Direction = ParameterDirection.Input,
					Value = contentProperty
				}
			};

			var rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > -1;
		}


		/// <summary>
		/// Deletes a row from the mp_ContentMeta table. Returns true if row deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			var sqlCommand = "DELETE FROM mp_ContentMeta WHERE Guid = :Guid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			var sqlCommand = "DELETE FROM mp_ContentMeta WHERE SiteGuid = :SiteGuid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":SiteGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			var sqlCommand = "DELETE FROM mp_ContentMeta WHERE ModuleGuid = :ModuleGuid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":ModuleGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the mp_ContentMeta table. Returns true if rows deleted.
		/// </summary>
		/// <returns>bool</returns>
		public static bool DeleteByContent(Guid contentGuid)
		{
			var sqlCommand = "DELETE FROM mp_ContentMeta WHERE ContentGuid = :ContentGuid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":ContentGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
				GetConnectionString(),
				sqlCommand.ToString(),
				arParams
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentMeta table.
		/// </summary>
		public static IDataReader GetOne(Guid guid)
		{
			var sqlCommand = "SELECT * FROM mp_ContentMeta WHERE Guid = :Guid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":Guid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			};

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
			var sqlCommand = "SELECT * FROM mp_ContentMeta WHERE  ContentGuid = :ContentGuid ORDER BY SortRank;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":ContentGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};

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
			var sqlCommand = "SELECT COALESCE(MAX(SortRank),1) FROM	mp_ContentMeta WHERE ContentGuid = :ContentGuid;";

			var arParams = new SqliteParameter[]
			{
				new SqliteParameter(":ContentGuid", DbType.String, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};

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
