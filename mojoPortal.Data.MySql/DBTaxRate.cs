using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBTaxRate
{
	/// <summary>
	/// Inserts a row in the mp_TaxRate table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="geoZoneGuid"> geoZoneGuid </param>
	/// <param name="taxClassGuid"> taxClassGuid </param>
	/// <param name="priority"> priority </param>
	/// <param name="rate"> rate </param>
	/// <param name="description"> description </param>
	/// <param name="created"> created </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="modifiedBy"> modifiedBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid geoZoneGuid,
		Guid taxClassGuid,
		int priority,
		decimal rate,
		string description,
		DateTime created,
		Guid createdBy,
		DateTime lastModified,
		Guid modifiedBy)
	{

		string sqlCommand = @"
INSERT INTO mp_TaxRate (
    Guid, 
    SiteGuid, 
    GeoZoneGuid, 
    TaxClassGuid, 
    Priority, 
    Rate, 
    Description, 
    Created, 
    CreatedBy, 
    LastModified, 
    ModifiedBy 
) 
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?GeoZoneGuid, 
    ?TaxClassGuid, 
    ?Priority, 
    ?Rate, 
    ?Description, 
    ?Created, 
    ?CreatedBy, 
    ?LastModified, 
    ?ModifiedBy 
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

			new("?GeoZoneGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = geoZoneGuid.ToString()
			},

			new("?TaxClassGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = taxClassGuid.ToString()
			},

			new("?Priority", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = priority
			},

			new("?Rate", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = rate
			},

			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?Created", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = created
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?LastModified", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModified
			},

			new("?ModifiedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = modifiedBy.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_TaxRate table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="geoZoneGuid"> geoZoneGuid </param>
	/// <param name="taxClassGuid"> taxClassGuid </param>
	/// <param name="priority"> priority </param>
	/// <param name="rate"> rate </param>
	/// <param name="description"> description </param>
	/// <param name="created"> created </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="modifiedBy"> modifiedBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		Guid geoZoneGuid,
		Guid taxClassGuid,
		int priority,
		decimal rate,
		string description,
		DateTime lastModified,
		Guid modifiedBy)
	{

		string sqlCommand = @"
UPDATE mp_TaxRate 
SET  
    GeoZoneGuid = ?GeoZoneGuid, 
    TaxClassGuid = ?TaxClassGuid, 
    Priority = ?Priority, 
    Rate = ?Rate, 
    Description = ?Description, 
    LastModified = ?LastModified, 
    ModifiedBy = ?ModifiedBy 
WHERE Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?GeoZoneGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = geoZoneGuid.ToString()
			},

			new("?TaxClassGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = taxClassGuid.ToString()
			},

			new("?Priority", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = priority
			},

			new("?Rate", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = rate
			},

			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?LastModified", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModified
			},

			new("?ModifiedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = modifiedBy.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_TaxRate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_TaxRate 
WHERE Guid = ?Guid ;";

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
	/// Gets an IDataReader with one row from the mp_TaxRate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaxRate 
WHERE Guid = ?Guid ;";

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_TaxRate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetTaxRates(
		Guid siteGuid,
		Guid geoZoneGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_TaxRate 
WHERE SiteGuid = ?SiteGuid 
AND GeoZoneGuid = ?GeoZoneGuid 
ORDER BY Priority ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?GeoZoneGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = geoZoneGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Inserts a row in the mp_TaxRateHistory table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="taxRateGuid"> taxRateGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="geoZoneGuid"> geoZoneGuid </param>
	/// <param name="taxClassGuid"> taxClassGuid </param>
	/// <param name="priority"> priority </param>
	/// <param name="rate"> rate </param>
	/// <param name="description"> description </param>
	/// <param name="created"> created </param>
	/// <param name="createdBy"> createdBy </param>
	/// <param name="lastModified"> lastModified </param>
	/// <param name="modifiedBy"> modifiedBy </param>
	/// <param name="logTime"> logTime </param>
	/// <returns>int</returns>
	public static int AddHistory(
		Guid guid,
		Guid taxRateGuid,
		Guid siteGuid,
		Guid geoZoneGuid,
		Guid taxClassGuid,
		int priority,
		decimal rate,
		string description,
		DateTime created,
		Guid createdBy,
		DateTime lastModified,
		Guid modifiedBy,
		DateTime logTime)
	{


		string sqlCommand = @"
INSERT INTO mp_TaxRateHistory (
    Guid, 
    TaxRateGuid, 
    SiteGuid, 
    GeoZoneGuid, 
    TaxClassGuid, 
    Priority, 
    Rate, 
    Description, 
    Created, 
    CreatedBy, 
    LastModified, 
    ModifiedBy, 
    LogTime 
) 
VALUES (
    ?Guid, 
    ?TaxRateGuid, 
    ?SiteGuid, 
    ?GeoZoneGuid, 
    ?TaxClassGuid, 
    ?Priority, 
    ?Rate, 
    ?Description, 
    ?Created, 
    ?CreatedBy, 
    ?LastModified, 
    ?ModifiedBy, 
    ?LogTime 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?TaxRateGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = taxRateGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?GeoZoneGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = geoZoneGuid.ToString()
			},

			new("?TaxClassGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = taxClassGuid.ToString()
			},

			new("?Priority", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = priority
			},

			new("?Rate", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = rate
			},

			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?Created", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = created
			},

			new("?CreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy.ToString()
			},

			new("?LastModified", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModified
			},

			new("?ModifiedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = modifiedBy.ToString()
			},

			new("?LogTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = logTime
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	///// <summary>
	///// Gets a count of rows in the mp_TaxRate table.
	///// </summary>
	//public static int GetCount()
	//{
	//    string sqlCommand = @"
	//    sqlCommand.Append("SELECT  Count(*) ");
	//    sqlCommand.Append("FROM	mp_TaxRate ");
	//    sqlCommand.Append(";");

	//    return Convert.ToInt32(CommandHelper.ExecuteScalar(
	//        ConnectionString.GetReadConnectionString(),
	//        sqlCommand.ToString(),
	//        null));
	//}

	///// <summary>
	///// Gets a page of data from the mp_TaxRate table.
	///// </summary>
	///// <param name="pageNumber">The page number.</param>
	///// <param name="pageSize">Size of the page.</param>
	///// <param name="totalPages">total pages</param>
	//public static IDataReader GetPage(
	//    int pageNumber,
	//    int pageSize,
	//    out int totalPages)
	//{
	//    int pageLowerBound = (pageSize * pageNumber) - pageSize;
	//    totalPages = 1;
	//    int totalRows = GetCount();

	//    if (pageSize > 0) totalPages = totalRows / pageSize;

	//    if (totalRows <= pageSize)
	//    {
	//        totalPages = 1;
	//    }
	//    else
	//    {
	//        int remainder;
	//        Math.DivRem(totalRows, pageSize, out remainder);
	//        if (remainder > 0)
	//        {
	//            totalPages += 1;
	//        }
	//    }

	//    string sqlCommand = @"
	//    sqlCommand.Append("SELECT	* ");
	//    sqlCommand.Append("FROM	mp_TaxRate  ");
	//    //sqlCommand.Append("WHERE  ");
	//    //sqlCommand.Append("ORDER BY  ");
	//    //sqlCommand.Append("  ");
	//    sqlCommand.Append("LIMIT ?PageSize ");

	//    if (pageNumber > 1)
	//    {
	//        sqlCommand.Append("OFFSET ?OffsetRows ");
	//    }

	//    sqlCommand.Append(";");

	//    var arParams = new List<MySqlParameter>

	//    new("?PageSize", MySqlDbType.Int32) {
	//    Direction = ParameterDirection.Input,
	//    Value = pageSize

	//    new("?OffsetRows", MySqlDbType.Int32) {
	//    Direction = ParameterDirection.Input,
	//    Value = pageLowerBound

	//    return CommandHelper.ExecuteReader(
	//        ConnectionString.GetReadConnectionString(),
	//        sqlCommand.ToString(),
	//        arParams);
	//}

}
