using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBCategory
{


	/// <summary>
	/// Inserts a row in the mp_Category table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="parentGuid"> parentGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="category"> category </param>
	/// <param name="description"> description </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdBy"> createdBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid parentGuid,
		Guid siteGuid,
		Guid featureGuid,
		Guid moduleGuid,
		string category,
		string description,
		DateTime createdUtc,
		Guid createdBy)
	{

		string sqlCommand = @"
INSERT INTO mp_Category (
	Guid, 
	ParentGuid, 
	SiteGuid, 
	FeatureGuid, 
	ModuleGuid, 
	Category, 
	Description, 
	ItemCount, 
	CreatedUtc, 
	CreatedBy, 
	ModifiedUtc, 
	ModifiedBy 
)
 VALUES (
	?Guid, 
	?ParentGuid, 
	?SiteGuid, 
	?FeatureGuid, 
	?ModuleGuid, 
	?Category, 
	?Description, 
	?ItemCount, 
	?CreatedUtc, 
	?CreatedBy, 
	?ModifiedUtc, 
	?ModifiedBy 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
			Direction = ParameterDirection.Input,
			Value = guid.ToString()
			},

			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
			Direction = ParameterDirection.Input,
			Value = parentGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
			Direction = ParameterDirection.Input,
			Value = siteGuid.ToString()
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
			Direction = ParameterDirection.Input,
			Value = featureGuid.ToString()
			},

			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
			Direction = ParameterDirection.Input,
			Value = moduleGuid.ToString()
			},

			new("?Category", MySqlDbType.VarChar, 255)
			{
			Direction = ParameterDirection.Input,
			Value = category
			},

			new("?Description", MySqlDbType.Text)
			{
			Direction = ParameterDirection.Input,
			Value = description
			},

			new("?ItemCount", MySqlDbType.Int32)
			{
			Direction = ParameterDirection.Input,
			Value = 0
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

			new("?ModifiedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?ModifiedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_Category table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="parentGuid"> parentGuid </param>
	/// <param name="category"> category </param>
	/// <param name="description"> description </param>
	/// <param name="modifiedUtc"> modifiedUtc </param>
	/// <param name="modifiedBy"> modifiedBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		Guid parentGuid,
		string category,
		string description,
		DateTime modifiedUtc,
		Guid modifiedBy)
	{

		string sqlCommand = @"
UPDATE mp_Category
SET 
	ParentGuid = ?ParentGuid,
	Category = ?Category,
	Description = ?Description,
	ModifiedUtc = ?ModifiedUtc,
	ModifiedBy = ?ModifiedBy
WHERE 
	Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{

			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new ("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},

			new("?Category", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = category
			},

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ModifiedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = modifiedUtc
			},

			new("?ModifiedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = modifiedBy.ToString()
			},
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateItemCount(Guid guid)
	{

		string sqlCommand = @"
UPDATE mp_Category 
SET  
	ItemCount = ( 
		SELECT Count(*) 
		FROM 
			mp_CategoryItem 
		WHERE  
			CategoryGuid = ?Guid
		) 
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
			arParams);

		return rowsAffected > -1;


	}

	/// <summary>
	/// Deletes a row from the mp_Category table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_Category 
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
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_Category table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeletByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Category 
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
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_Category table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeletByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Category 
WHERE 
	FeatureGuid = ?FeatureGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes rows from the mp_Category table. Returns true if rows deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeletBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Category 
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
			arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_Category table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_Category 
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
			arParams);

	}

	public static IDataReader GetByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_Category 
WHERE 
	ModuleGuid = ?ModuleGuid 
ORDER BY 
	Category;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets a count of rows in the mp_Category table.
	/// </summary>
	public static int GetCountByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_Category 
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



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

}
