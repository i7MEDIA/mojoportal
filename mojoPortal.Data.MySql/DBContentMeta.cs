using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

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
		string sqlCommand = @"
INSERT INTO 
	mp_ContentMeta (
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
	?Guid, 
	?SiteGuid, 
	?ModuleGuid, 
	?ContentGuid, 
	?Name, 
	?NameProperty, 
	?Scheme, 
	?LangCode, 
	?Dir, 
	?MetaContent, 
	?ContentProperty, 
	?SortRank, 
	?CreatedUtc, 
	?CreatedBy, 
	?LastModUtc, 
	?LastModBy 
	);";

		var sqlParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Scheme", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = scheme
			},

			new("?LangCode", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = langCode
			},

			new("?Dir", MySqlDbType.VarChar, 3)
			{
				Direction = ParameterDirection.Input,
				Value = dir
			},

			new("?MetaContent", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = metaContent
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?NameProperty", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = nameProperty
			},

			new("?ContentProperty", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = contentProperty
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
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
		string sqlCommand = @"
UPDATE 
	mp_ContentMeta 
SET  
	Name = ?Name, 
	NameProperty = ?NameProperty, 
	Scheme = ?Scheme, 
	LangCode = ?LangCode, 
	Dir = ?Dir, 
	MetaContent = ?MetaContent, 
	ContentProperty = ?ContentProperty, 
	SortRank = ?SortRank, 
	LastModUtc = ?LastModUtc, 
	LastModBy = ?LastModBy 
WHERE  
	Guid = ?Guid;";

		var sqlParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Scheme", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = scheme
			},

			new("?LangCode", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = langCode
			},

			new("?Dir", MySqlDbType.VarChar, 3)
			{
				Direction = ParameterDirection.Input,
				Value = dir
			},

			new("?MetaContent", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = metaContent
			},

			new("?SortRank", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortRank
			},

			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			},

			new("?NameProperty", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = nameProperty
			},

			new("?ContentProperty", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = contentProperty
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			sqlParams.ToArray()
		);

		return rowsAffected > -1;
	}


	/// <summary>
	/// Deletes a row from the mp_ContentMeta table. Returns true if row deleted.
	/// </summary>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_ContentMeta 
WHERE 
	Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
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
		string sqlCommand = @"
DELETE FROM 
	mp_ContentMeta 
WHERE 
	SiteGuid = ?SiteGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}

	};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
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
		string sqlCommand = @"
DELETE FROM 
	mp_ContentMeta 
WHERE 
	ModuleGuid = ?ModuleGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}

		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
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
		string sqlCommand = @"
DELETE FROM 
	mp_ContentMeta 
WHERE 
	ContentGuid = ?ContentGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			}

	};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams
		);

		return rowsAffected > 0;
	}


	/// <summary>
	/// Gets an IDataReader with one row from the mp_ContentMeta table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT 
	* 
FROM 
	mp_ContentMeta 
WHERE 
	Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};



		return CommandHelper.ExecuteReader(
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
		string sqlCommand = @"
SELECT 
	* 
FROM 
	mp_ContentMeta 
WHERE 
	ContentGuid = ?ContentGuid 
ORDER BY 
	SortRank;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			}

		};



		return CommandHelper.ExecuteReader(
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
		string sqlCommand = @"
SELECT 
	COALESCE(MAX(SortRank),1) 
FROM 
	mp_ContentMeta 
WHERE 
	ContentGuid = ?ContentGui;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			}

		};



		return Convert.ToInt32(
			CommandHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams
			)
		);
	}
}