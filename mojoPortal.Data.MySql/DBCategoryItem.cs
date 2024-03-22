using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBCategoryItem
{
	/// <summary>
	/// Inserts a row in the mp_CategoryItem table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="itemGuid"> itemGuid </param>
	/// <param name="categoryGuid"> categoryGuid </param>
	/// <param name="extraGuid"> extraGuid </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid featureGuid,
		Guid moduleGuid,
		Guid itemGuid,
		Guid categoryGuid,
		Guid extraGuid)
	{

		string sqlCommand = @"
INSERT INTO mp_CategoryItem (
	Guid, 
	SiteGuid, 
	FeatureGuid, 
	ModuleGuid, 
	ItemGuid, 
	CategoryGuid, 
	ExtraGuid 
)
	VALUES (
	?Guid, 
	?SiteGuid, 
	?FeatureGuid, 
	?ModuleGuid, 
	?ItemGuid, 
	?CategoryGuid, 
	?ExtraGuid 
);";

		var arParams = new List<MySqlParameter>
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

			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			},

			new("?CategoryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = categoryGuid.ToString()
			},

			new("?ExtraGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = extraGuid.ToString()
			}

		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}




	/// <summary>
	/// Deletes a row from the mp_CategoryItem table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByItem(Guid itemGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
WHERE 
	ItemGuid = ?ItemGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			}

		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteByExtraGuid(Guid extraGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
WHERE 
	ExtraGuid = ?ExtraGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ExtraGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = extraGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteByCategory(Guid categoryGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
WHERE 
	CategoryGuid = ?CategoryGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?CategoryGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = categoryGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CategoryItem 
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}
}
