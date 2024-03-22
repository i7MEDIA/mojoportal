using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace mojoPortal.Data;


public static class DBCommerceReport
{

	/// <summary>
	/// Inserts a row in the mp_CommerceReport table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="moduleTitle"> moduleTitle </param>
	/// <param name="orderGuid"> orderGuid </param>
	/// <param name="itemGuid"> itemGuid </param>
	/// <param name="itemName"> itemName </param>
	/// <param name="quantity"> quantity </param>
	/// <param name="price"> price </param>
	/// <param name="subTotal"> subTotal </param>
	/// <param name="orderDateUtc"> orderDateUtc </param>
	/// <param name="paymentMethod"> paymentMthod </param>
	/// <param name="iPAddress"> iPAddress </param>
	/// <param name="adminOrderLink"> adminOrderLink </param>
	/// <param name="userOrderLink"> userOrderLink </param>
	/// <param name="rowCreatedUtc"> rowCreatedUtc </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		Guid siteGuid,
		Guid userGuid,
		Guid featureGuid,
		Guid moduleGuid,
		string moduleTitle,
		Guid orderGuid,
		Guid itemGuid,
		string itemName,
		int quantity,
		decimal price,
		decimal subTotal,
		DateTime orderDateUtc,
		string paymentMethod,
		string iPAddress,
		string adminOrderLink,
		string userOrderLink,
		DateTime rowCreatedUtc,
		bool includeInAggregate)
	{
		string sqlCommand = @"
INSERT INTO mp_CommerceReport (
    RowGuid,
    SiteGuid,
    UserGuid,
    FeatureGuid,
    ModuleGuid,
    ModuleTitle,
    OrderGuid,
    ItemGuid,
    ItemName,
    Quantity,
    Price,
    SubTotal,
    OrderDateUtc,
    PaymentMethod,
    IPAddress,
    AdminOrderLink,
    UserOrderLink,
    IncludeInAggregate,
    RowCreatedUtc 
)
VALUES (
    ?RowGuid,
    ?SiteGuid,
    ?UserGuid,
    ?FeatureGuid,
    ?ModuleGuid,
    ?ModuleTitle,
    ?OrderGuid,
    ?ItemGuid,
    ?ItemName,
    ?Quantity,
    ?Price,
    ?SubTotal,
    ?OrderDateUtc,
    ?PaymentMethod,
    ?IPAddress,
    ?AdminOrderLink,
    ?UserOrderLink,
    ?IncludeInAggregate,
    ?RowCreatedUtc 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
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

			new("?ModuleTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moduleTitle
			},

			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			},

			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			},

			new("?ItemName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = itemName
			},

			new("?Quantity", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = quantity
			},

			new("?Price", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = price
			},

			new("?SubTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = subTotal
			},

			new("?OrderDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = orderDateUtc
			},

			new("?PaymentMethod", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paymentMethod
			},

			new("?IPAddress", MySqlDbType.VarChar, 250)
			{
				Direction = ParameterDirection.Input,
				Value = iPAddress
			},

			new("?AdminOrderLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = adminOrderLink
			},

			new("?UserOrderLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = userOrderLink
			},

			new("?RowCreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = rowCreatedUtc
			},
		};


		if (includeInAggregate)
		{
			arParams.Add(new("?IncludeInAggregate", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = 1
			});
		}
		else
		{
			arParams.Add(new("?IncludeInAggregate", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = 0
			});
		}

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
WHERE 
RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
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

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
WHERE 
	UserGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
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

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
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

	/// <summary>
	/// Deletes a row from the mp_CommerceReport table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByOrder(Guid orderGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReport 
WHERE 
OrderGuid = ?OrderGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			}
	};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static IDataReader GetSalesByYearByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT 
	YEAR(OrderDateUtc) As Y,  
	SUM(SubTotal) As Sales, 
	SUM(Quantity) As Units 
FROM 
	mp_CommerceReport 
WHERE 
	ModuleGuid = ?ModuleGuid AND IncludeInAggregate = 1 
GROUP BY YEAR(OrderDateUtc) 
ORDER BY YEAR(OrderDateUtc) desc;";


		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSalesByYearMonthBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT 
	YEAR(OrderDateUtc) As Y,  
	MONTH(OrderDateUtc) As M, 
	SUM(SubTotal) As Sales, 
	SUM(Quantity) As Units 
FROM 
	mp_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid AND IncludeInAggregate = 1 
GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) 
ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc;";


		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSalesByYearMonthByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT 
	YEAR(OrderDateUtc) As Y,  
	MONTH(OrderDateUtc) As M, 
	SUM(SubTotal) As Sales, 
	SUM(Quantity) As Units 
FROM 
	mp_CommerceReport 
WHERE 
	ModuleGuid = ?ModuleGuid AND IncludeInAggregate = 1 
GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) 
ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc;";


		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSalesByYearMonthByItem(Guid itemGuid)
	{
		string sqlCommand = @"
SELECT 
	YEAR(OrderDateUtc) As Y,  
	MONTH(OrderDateUtc) As M, 
	SUM(SubTotal) As Sales, 
	SUM(Quantity) As Units 
FROM 
	mp_CommerceReport 
WHERE 
	ItemGuid = ?ItemGuid AND IncludeInAggregate = 1 
GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) 
ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc;";


		var arParams = new List<MySqlParameter>
		{
			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetSalesByYearMonthByUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT 
	YEAR(OrderDateUtc) As Y,  
	MONTH(OrderDateUtc) As M, 
	SUM(SubTotal) As Sales, 
	SUM(Quantity) As Units 
FROM 
	mp_CommerceReport 
WHERE 
	UserGuid = ?UserGuid AND IncludeInAggregate = 1 
GROUP BY YEAR(OrderDateUtc), MONTH(OrderDateUtc) 
ORDER BY YEAR(OrderDateUtc) desc, MONTH(OrderDateUtc) desc;";


		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static IDataReader GetSalesGroupedByModule(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT  
	ModuleGuid, 
	ModuleTitle, 
	SUM(SubTotal) AS Sales, 
	SUM(Quantity) As Units 
FROM	mp_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid AND IncludeInAggregate = 1 
GROUP BY ModuleGuid, ModuleTitle 
ORDER BY SUM(SubTotal) DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
	};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}
	public static IDataReader GetSalesGroupedByUser(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT  
	c.UserGuid AS UserGuid, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Name, 'deleted user') AS Name, 
	u.LoginName AS LoginName, 
	u.Email AS Email, 
	SUM(SubTotal) AS Sales 
FROM	mp_CommerceReport c 
LEFT OUTER JOIN mp_Users u ON c.UserGuid = u.UserGuid 
WHERE 
	c.SiteGuid = ?SiteGuid 
	AND c.IncludeInAggregate = 1 
GROUP BY  
	c.UserGuid, 
	u.UserID, 
	u.Name, 
	u.LoginName, 
	u.Email 
ORDER BY SUM(SubTotal) DESC LIMIT 20;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
		};




		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	public static IDataReader GetItemSummary(Guid itemGuid)
	{
		string sqlCommand = @"
SELECT  
	SiteGuid, 
	ModuleGuid, 
	ItemGuid, 
	ModuleTitle, 
	ItemName, 
	SUM(SubTotal) AS Revenue 
FROM	
	mp_CommerceReport 
WHERE 
	ItemGuid = ?ItemGuid 
	AND IncludeInAggregate = 1 
GROUP BY SiteGuid, ModuleGuid, ItemGuid, ModuleTitle, ItemName;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetItemRevenueBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT  
	ModuleTitle, 
	ItemName, 
	ItemGuid, 
	SUM(Quantity) AS UnitsSold, 
	SUM(SubTotal) AS Revenue 
FROM	
	mp_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid 
	AND IncludeInAggregate = 1 
GROUP BY ModuleTitle, ItemName, ItemGuid 
ORDER BY ModuleTitle, SUM(SubTotal) DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetItemRevenueByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT  
	ModuleTitle, 
	ItemName, 
	ItemGuid, 
	SUM(Quantity) AS UnitsSold, 
	SUM(SubTotal) AS Revenue 
FROM	
	mp_CommerceReport 
WHERE 
	ModuleGuid = ?ModuleGuid 
	AND IncludeInAggregate = 1 
GROUP BY ModuleTitle, ItemName, ItemGuid 
ORDER BY SUM(SubTotal) DESC 
LIMIT 20;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetItemRevenueByUser(Guid userGuid)
	{
		string sqlCommand = @"
SELECT  
	ModuleTitle, 
	ItemName, 
	ItemGuid, 
	SUM(Quantity) AS UnitsSold, 
	SUM(SubTotal) AS Revenue 
FROM	
	mp_CommerceReport 
WHERE 
	UserGuid = ?UserGuid 
	AND IncludeInAggregate = 1 
GROUP BY 
	ModuleTitle, ItemName, ItemGuid 
ORDER BY 
	ModuleTitle, SUM(SubTotal) DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static decimal GetAllTimeRevenueBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT  
	SUM(SubTotal) 
FROM	
	mp_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid 
	AND IncludeInAggregate = 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
	};



		decimal result = 0;

		try
		{

			result = Convert.ToDecimal(CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams), CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException) { }

		return result;

	}

	public static decimal GetAllTimeRevenueByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT  
	SUM(SubTotal) 
FROM	
	mp_CommerceReport 
WHERE 
	ModuleGuid = ?ModuleGuid 
	AND IncludeInAggregate = 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			}
		};



		decimal result = 0;

		try
		{

			result = Convert.ToDecimal(CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams), CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException) { }

		return result;

	}

	/// <summary>
	/// Gets a count of rows in the mp_CommerceReport table.
	/// </summary>
	public static int GetDistinctItemCountByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM (
	SELECT DISTINCT 
		ItemGuid 
	FROM 
		mp_CommerceReport 
	WHERE 
		ModuleGuid = ?ModuleGuid
	) 
AS s;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			}
	};



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a page of data from the mp_CommerceReport table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetItemsPageByModule(
		Guid moduleGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetDistinctItemCountByModule(moduleGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT 
	ModuleTitle, 
	ItemName, 
	ItemGuid, 
	SUM(Quantity) AS UnitsSold, 
	SUM(SubTotal) AS Revenue 
FROM	
	mp_CommerceReport  
WHERE 
	ModuleGuid = ?ModuleGuid 
GROUP BY 
	ModuleTitle, ItemName, ItemGuid   
ORDER BY 
	ModuleTitle, ItemName   
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";



		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets a count of rows in the mp_CommerceReport table.
	/// </summary>
	public static int GetItemCountByUser(Guid siteGuid, Guid userGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM	 
	p_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid 
	AND UserGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid
			}
	};



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a page of data from the mp_CommerceReport table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetUserItemsPage(
		Guid siteGuid,
		Guid userGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetItemCountByUser(siteGuid, userGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT 
	* 
FROM	
	mp_CommerceReport  
WHERE 
	SiteGuid = ?SiteGuid AND UserGuid = ?UserGuid 
ORDER BY 
	OrderDateUtc DESC   
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	/// <summary>
	/// Gets a count of rows in the mp_CommerceReport table.
	/// </summary>
	public static int GetDistinctUserItemCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT 
	Count(*) 
FROM	 
	(SELECT DISTINCT 
		UserGuid 
	FROM 
		mp_CommerceReport 
WHERE 
	SiteGuid = ?SiteGuid) a;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			}
	};



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the mp_CommerceReport table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetUserItemPageBySite(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetDistinctUserItemCount(siteGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT 
	c.UserGuid AS UserGuid, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Name, 'deleted user') AS Name, 
	u.LoginName AS LoginName, 
	u.Email AS Email, 
	SUM(c.SubTotal) AS Revenue 
FROM	
	mp_CommerceReport c  
LEFT OUTER JOIN 
	mp_Users u ON	c.UserGuid = u.UserGuid 
WHERE 
	c.SiteGuid = ?SiteGuid 
GROUP BY   
	c.UserGuid, 
	u.UserID, 
	u.Name, 
	u.LoginName, 
	u.Email 
ORDER BY 
	u.Name   
LIMIT ?PageSize 1 ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Inserts a row in the mp_CommerceReportOrders table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="orderGuid"> orderGuid </param>
	/// <param name="billingFirstName"> billingFirstName </param>
	/// <param name="billingLastName"> billingLastName </param>
	/// <param name="billingCompany"> billingCompany </param>
	/// <param name="billingAddress1"> billingAddress1 </param>
	/// <param name="billingAddress2"> billingAddress2 </param>
	/// <param name="billingSuburb"> billingSuburb </param>
	/// <param name="billingCity"> billingCity </param>
	/// <param name="billingPostalCode"> billingPostalCode </param>
	/// <param name="billingState"> billingState </param>
	/// <param name="billingCountry"> billingCountry </param>
	/// <param name="paymentMethod"> paymentMethod </param>
	/// <param name="subTotal"> subTotal </param>
	/// <param name="taxTotal"> taxTotal </param>
	/// <param name="shippingTotal"> shippingTotal </param>
	/// <param name="orderTotal"> orderTotal </param>
	/// <param name="orderDateUtc"> orderDateUtc </param>
	/// <param name="adminOrderLink"> adminOrderLink </param>
	/// <param name="userOrderLink"> userOrderLink </param>
	/// <param name="rowCreatedUtc"> rowCreatedUtc </param>
	/// <returns>int</returns>
	public static int CreateOrder(
		Guid rowGuid,
		Guid siteGuid,
		Guid featureGuid,
		Guid moduleGuid,
		Guid userGuid,
		Guid orderGuid,
		string billingFirstName,
		string billingLastName,
		string billingCompany,
		string billingAddress1,
		string billingAddress2,
		string billingSuburb,
		string billingCity,
		string billingPostalCode,
		string billingState,
		string billingCountry,
		string paymentMethod,
		decimal subTotal,
		decimal taxTotal,
		decimal shippingTotal,
		decimal orderTotal,
		DateTime orderDateUtc,
		string adminOrderLink,
		string userOrderLink,
		DateTime rowCreatedUtc,
		bool includeInAggregate)
	{
		string sqlCommand = @"
INSERT INTO 
	mp_CommerceReportOrders(
	RowGuid, 
	SiteGuid, 
	FeatureGuid, 
	ModuleGuid, 
	UserGuid, 
	OrderGuid, 
	BillingFirstName, 
	BillingLastName, 
	BillingCompany, 
	BillingAddress1, 
	BillingAddress2, 
	BillingSuburb, 
	BillingCity, 
	BillingPostalCode, 
	BillingState, 
	BillingCountry, 
	PaymentMethod, 
	SubTotal, 
	TaxTotal, 
	ShippingTotal, 
	OrderTotal, 
	OrderDateUtc, 
	AdminOrderLink, 
	UserOrderLink, 
	IncludeInAggregate, 
	RowCreatedUtc 
)
VALUES (
	?RowGuid, 
	?SiteGuid, 
	?FeatureGuid, 
	?ModuleGuid, 
	?UserGuid, 
	?OrderGuid, 
	?BillingFirstName, 
	?BillingLastName, 
	?BillingCompany, 
	?BillingAddress1, 
	?BillingAddress2, 
	?BillingSuburb, 
	?BillingCity, 
	?BillingPostalCode, 
	?BillingState, 
	?BillingCountry, 
	?PaymentMethod, 
	?SubTotal, 
	?TaxTotal, 
	?ShippingTotal, 
	?OrderTotal, 
	?OrderDateUtc, 
	?AdminOrderLink, 
	?UserOrderLink, 
	?IncludeInAggregate, 
	?RowCreatedUtc 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
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

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			},

			new("?BillingFirstName", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = billingFirstName
			},

			new("?BillingLastName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = billingLastName
			},

			new("?BillingCompany", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingCompany
			},

			new("?BillingAddress1", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingAddress1
			},

			new("?BillingAddress2", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingAddress2
			},

			new("?BillingSuburb", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingSuburb
			},

			new("?BillingCity", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingCity
			},

			new("?BillingPostalCode", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = billingPostalCode
			},

			new("?BillingState", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingState
			},

			new("?BillingCountry", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = billingCountry
			},

			new("?PaymentMethod", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paymentMethod
			},

			new("?SubTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = subTotal
			},

			new("?TaxTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = taxTotal
			},

			new("?ShippingTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = shippingTotal
			},

			new("?OrderTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = orderTotal
			},

			new("?OrderDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = orderDateUtc
			},

			new("?AdminOrderLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = adminOrderLink
			},

			new("?UserOrderLink", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = userOrderLink
			},

			new("?RowCreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = rowCreatedUtc
			},

			new("?IncludeInAggregate", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input
			}
		};

		if (includeInAggregate)
		{
			arParams.Add(new("?IncludeInAggregate", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = 1
			});
		}
		else
		{
			arParams.Add(new("?IncludeInAggregate", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = 0
			});
		}

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}

	/// <summary>
	/// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrder(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_CommerceReportOrders 
WHERE 
	RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes  from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrdersBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_CommerceReportOrders 
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

	/// <summary>
	/// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrdersByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_CommerceReportOrders 
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

	/// <summary>
	/// Deletes from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrdersByOrder(Guid orderGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_CommerceReportOrders 
WHERE 
	OrderGuid = ?OrderGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			}
	};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrdersByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_CommerceReportOrders 
WHERE 
FeatureGuid = ?FeatureGuid 
;";

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

	/// <summary>
	/// Deletes a row from the mp_CommerceReportOrders table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteOrdersByUser(Guid userGuid)
	{
		string sqlCommand = @"
DELETE FROM 
	mp_CommerceReportOrders 
WHERE 
	serGuid = ?UserGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},
	};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static bool MoveOrder(
		Guid orderGuid,
		Guid newUserGuid)
	{

		string sqlCommand0 = @"
UPDATE 
	mp_CommerceReport 
SET  
	UserGuid = ?UserGuid 
WHERE  
	OrderGuid = ?OrderGuid;";

		var arParams0 = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = newUserGuid.ToString()
			},

			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand0.ToString(),
			arParams0);

		string sqlCommand1 = @"
UPDATE 
	mp_CommerceReportOrders 
SET  
	UserGuid = ?UserGuid 
WHERE  
	OrderGuid = ?OrderGuid;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = newUserGuid.ToString()
			},

			new("?OrderGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = orderGuid.ToString()
			}
		};



		CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand1.ToString(),
			arParams1);

		return rowsAffected > -1;

	}

}
