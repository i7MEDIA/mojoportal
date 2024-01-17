using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBWebPartContent
{

	public static int AddWebPart(
		Guid webPartId,
		Guid siteGuid,
		int siteId,
		string title,
		string description,
		string imageUrl,
		string className,
		string assemblyName,
		bool availableForMyPage,
		bool allowMultipleInstancesOnMyPage,
		bool availableForContentSystem)
	{

		#region Bit Conversion

		int intAvailableForMyPage;
		if (availableForMyPage)
		{
			intAvailableForMyPage = 1;
		}
		else
		{
			intAvailableForMyPage = 0;
		}

		int intAllowMultipleInstancesOnMyPage;
		if (allowMultipleInstancesOnMyPage)
		{
			intAllowMultipleInstancesOnMyPage = 1;
		}
		else
		{
			intAllowMultipleInstancesOnMyPage = 0;
		}

		int intAvailableForContentSystem;
		if (availableForContentSystem)
		{
			intAvailableForContentSystem = 1;
		}
		else
		{
			intAvailableForContentSystem = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_WebParts (
    WebPartID, 
    SiteGuid, 
    SiteID, 
    Title, 
    Description, 
    ImageUrl, 
    ClassName, 
    AssemblyName, 
    AvailableForMyPage, 
    AllowMultipleInstancesOnMyPage, 
    AvailableForContentSystem 
) 
VALUES (
    ?WebPartID, 
    ?SiteGuid, 
    ?SiteID, 
    ?Title, 
    ?Description, 
    ?ImageUrl, 
    ?ClassName, 
    ?AssemblyName, 
    ?AvailableForMyPage, 
    ?AllowMultipleInstancesOnMyPage, 
    ?AvailableForContentSystem 
);
SELECT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?WebPartID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = webPartId.ToString()
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageUrl
			},

			new("?ClassName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = className
			},

			new("?AssemblyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = assemblyName
			},

			new("?AvailableForMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAvailableForMyPage
			},

			new("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowMultipleInstancesOnMyPage
			},

			new("?AvailableForContentSystem", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAvailableForContentSystem
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}

		};

		int rowsAffected = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return rowsAffected;

	}


	public static bool UpdateWebPart(
		Guid webPartId,
		int siteId,
		string title,
		string description,
		string imageUrl,
		string className,
		string assemblyName,
		bool availableForMyPage,
		bool allowMultipleInstancesOnMyPage,
		bool availableForContentSystem)
	{
		#region Bit Conversion

		int intAvailableForMyPage;
		if (availableForMyPage)
		{
			intAvailableForMyPage = 1;
		}
		else
		{
			intAvailableForMyPage = 0;
		}

		int intAllowMultipleInstancesOnMyPage;
		if (allowMultipleInstancesOnMyPage)
		{
			intAllowMultipleInstancesOnMyPage = 1;
		}
		else
		{
			intAllowMultipleInstancesOnMyPage = 0;
		}

		int intAvailableForContentSystem;
		if (availableForContentSystem)
		{
			intAvailableForContentSystem = 1;
		}
		else
		{
			intAvailableForContentSystem = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE mp_WebParts 
SET  
    SiteID = ?SiteID, 
    Title = ?Title, 
    Description = ?Description, 
    ImageUrl = ?ImageUrl, 
    ClassName = ?ClassName, 
    AssemblyName = ?AssemblyName, 
    AvailableForMyPage = ?AvailableForMyPage, 
    AllowMultipleInstancesOnMyPage = ?AllowMultipleInstancesOnMyPage, 
    AvailableForContentSystem = ?AvailableForContentSystem 
WHERE  
    WebPartID = ?WebPartID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?WebPartID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = webPartId.ToString()
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?ImageUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = imageUrl
			},

			new("?ClassName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = className
			},

			new("?AssemblyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = assemblyName
			},

			new("?AvailableForMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAvailableForMyPage
			},

			new("?AllowMultipleInstancesOnMyPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowMultipleInstancesOnMyPage
			},

			new("?AvailableForContentSystem", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAvailableForContentSystem
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
	{
		string sqlCommand = @"
UPDATE mp_WebParts 
SET CountOfUseOnMyPage = CountOfUseOnMyPage  + " + increment.ToString() + " WHERE WebPartID = ?WebPartID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?WebPartID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = webPartId.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static bool DeleteWebPart(Guid webPartId)
	{
		string sqlCommand = @"
DELETE FROM mp_WebParts 
WHERE WebPartID = ?WebPartID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?WebPartID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = webPartId.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetWebPart(Guid webPartId)
	{
		string sqlCommand = @"
SELECT  * 
FROM	mp_WebParts 
WHERE 
WebPartID = ?WebPartID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?WebPartID", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = webPartId.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader SelectBySite(int siteId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_WebParts 
WHERE SiteID = ?SiteID 
AND AvailableForContentSystem = 1 
ORDER BY Title, ClassName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static DataTable SelectPage(
		int siteId,
		int pageNumber,
		int pageSize,
		bool sortByClassName,
		bool sortByAssemblyName)
	{
		int totalRows = OnlyCount(siteId);
		int totalPages = totalRows / pageSize;
		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			int remainder = 0;
			Math.DivRem(totalRows, pageSize, out remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		int offset = pageSize * (pageNumber - 1);

		string sqlCommand = @"
SELECT 
    m.*, 
    " + totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages FROM mp_WebParts m  ";


		if (sortByClassName)
		{
			sqlCommand += " ORDER BY m.ClassName, m.Title ";
		}
		else if (sortByAssemblyName)
		{
			sqlCommand += " ORDER BY m.AssemblyName, m.Title ";
		}
		else
		{
			sqlCommand += " ORDER BY m.Title, m.ClassName ";
		}

		if (pageNumber > 1)
		{
			sqlCommand += "LIMIT " + offset.ToString(CultureInfo.InvariantCulture) + ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}
		else
		{
			sqlCommand += "LIMIT " + pageSize.ToString(CultureInfo.InvariantCulture) + " ";
		}

		sqlCommand += " ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};



		DataTable dt = new DataTable();
		dt.Columns.Add("WebPartID", typeof(String));
		dt.Columns.Add("Title", typeof(String));
		dt.Columns.Add("Description", typeof(String));
		dt.Columns.Add("ImageUrl", typeof(String));
		dt.Columns.Add("ClassName", typeof(String));
		dt.Columns.Add("AssemblyName", typeof(String));
		dt.Columns.Add("AvailableForMyPage", typeof(bool));
		dt.Columns.Add("AllowMultipleInstancesOnMyPage", typeof(bool));
		dt.Columns.Add("AvailableForContentSystem", typeof(bool));
		dt.Columns.Add("TotalPages", typeof(int));

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["WebPartID"] = reader["WebPartID"].ToString();
				row["Title"] = reader["Title"];
				row["Description"] = reader["Description"];
				row["ImageUrl"] = reader["ImageUrl"];
				row["ClassName"] = reader["ClassName"];
				row["AssemblyName"] = reader["AssemblyName"];
				row["AvailableForMyPage"] = reader["AvailableForMyPage"];
				row["AllowMultipleInstancesOnMyPage"] = reader["AllowMultipleInstancesOnMyPage"];
				row["AvailableForContentSystem"] = reader["AvailableForContentSystem"];
				row["TotalPages"] = reader["TotalPages"];

				dt.Rows.Add(row);

			}

		}

		return dt;
	}

	public static bool Exists(Int32 siteId, String className, String assemblyName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_WebParts 
WHERE SiteID = ?SiteID 
AND ClassName = ?ClassName 
AND AssemblyName = ?AssemblyName ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ClassName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = className
			},

			new("?AssemblyName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = assemblyName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return (count > 0);

	}

	public static IDataReader GetWebPartsForMyPage(int siteId)
	{
		string sqlCommand = @"
(SELECT   
    m.ModuleID, 
    m.SiteID, 
    m.ModuleDefID, 
    m.ModuleTitle , 
    m.AllowMultipleInstancesOnMyPage, 
    m.CountOfUseOnMyPage , 
    m.Icon As ModuleIcon, 
    md.Icon As FeatureIcon, 
    md.FeatureName, 
    md.ResourceFile, 
    0 As IsAssembly, 
     ''  As WebPartID 
FROM mp_Modules m 
JOIN mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
WHERE m.SiteID = ?SiteID 
AND m.AvailableForMyPage = 1  ) 
UNION 
(SELECT   
    -1 As ModuleID, 
    w.SiteID, 
    0 As MuduleDefID, 
      w.Title As ModuleTitle , 
    w.AllowMultipleInstancesOnMyPage, 
    w.CountOfUseOnMyPage , 
    w.ImageUrl As ModuleIcon, 
    w.ImageUrl As FeatureIcon, 
    w.Description As FeatureName, 
    'Resource' As ResourceFile, 
    1 As IsAssembly, 
    w.WebPartID 
FROM mp_WebParts w 
WHERE w.SiteID = ?SiteID 
AND w.AvailableForMyPage = 1 )
ORDER BY ModuleTitle  ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);


	}

	public static IDataReader GetMostPopular(int siteId, int numberToGet)
	{
		string sqlCommand = @"
(SELECT    
    m.ModuleID, 
    m.SiteID, 
    m.ModuleDefID, 
     m.ModuleTitle , 
    m.AllowMultipleInstancesOnMyPage, 
    m.CountOfUseOnMyPage , 
    m.Icon As ModuleIcon, 
    md.Icon As FeatureIcon, 
    md.FeatureName, 
    md.ResourceFile, 
    0 As IsAssembly, 
    '' As WebPartID 
FROM mp_Modules m 
JOIN mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
WHERE m.SiteID = ?SiteID 
AND m.AvailableForMyPage = 1 )
UNION 
(SELECT    
    0 As ModuleID, 
    w.SiteID, 
    0 As ModuleDefID, 
     w.Title As ModuleTitle , 
    w.AllowMultipleInstancesOnMyPage, 
    w.CountOfUseOnMyPage , 
    w.ImageUrl As ModuleIcon, 
    w.ImageUrl As FeatureIcon, 
    w.Description As FeatureName, 
    'Resource' As ResourceFile, 
    1 As IsAssembly, 
    w.WebPartID 
FROM mp_WebParts w 
WHERE w.SiteID = ?SiteID 
AND w.AvailableForMyPage = 1 )
ORDER BY ModuleTitle  
LIMIT " + numberToGet.ToString() + " ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);


	}

	public static int Count(int siteId)
	{
		int count = OnlyCount(siteId);
		count += DBModule.CountForMyPage(siteId);
		return count;

	}

	public static int OnlyCount(int siteId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_WebParts m  
WHERE m.SiteID = ?SiteID 
AND m.AvailableForMyPage = 1 ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count;

	}




}
