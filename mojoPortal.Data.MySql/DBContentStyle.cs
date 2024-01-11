using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data
{

	public static class DBContentStyle
	{


		/// <summary>
		/// Inserts a row in the mp_ContentStyle table. Returns rows affected count.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="name"> name </param>
		/// <param name="element"> element </param>
		/// <param name="cssClass"> cssClass </param>
		/// <param name="skinName"> skinName </param>
		/// <param name="isActive"> isActive </param>
		/// <param name="createdUtc"> createdUtc </param>
		/// <param name="createdBy"> createdBy </param>
		/// <returns>int</returns>
		public static int Create(
			Guid guid,
			Guid siteGuid,
			string name,
			string element,
			string cssClass,
			string skinName,
			bool isActive,
			DateTime createdUtc,
			Guid createdBy)
		{
			#region Bit Conversion

			int intIsActive = 0;
			if (isActive)
			{
				intIsActive = 1;
			}


			#endregion

			string sqlCommand = @"
INSERT INTO 
    mp_ContentStyle (
        Guid, 
        SiteGuid, 
        Name, 
        Element, 
        CssClass, 
        SkinName, 
        IsActive, 
        CreatedUtc, 
        LastModUtc, 
        CreatedBy, 
        LastModBy 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?Name, 
    ?Element, 
    ?CssClass, 
    ?SkinName, 
    ?IsActive, 
    ?CreatedUtc, 
    ?LastModUtc, 
    ?CreatedBy, 
    ?LastModBy 
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

				new("?Name", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = name
				},

				new("?Element", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = element
				},

				new("?CssClass", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = cssClass
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				},

				new("?IsActive", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intIsActive
				},

				new("?CreatedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},

				new("?LastModUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},

				new("?CreatedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},

				new("?LastModBy", MySqlDbType.VarChar, 36)
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
		/// Updates a row in the mp_ContentStyle table. Returns true if row updated.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="name"> name </param>
		/// <param name="element"> element </param>
		/// <param name="cssClass"> cssClass </param>
		/// <param name="skinName"> skinName </param>
		/// <param name="isActive"> isActive </param>
		/// <param name="lastModUtc"> lastModUtc </param>
		/// <param name="lastModBy"> lastModBy </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid guid,
			Guid siteGuid,
			string name,
			string element,
			string cssClass,
			string skinName,
			bool isActive,
			DateTime lastModUtc,
			Guid lastModBy)
		{
			#region Bit Conversion

			int intIsActive = 0;
			if (isActive)
			{
				intIsActive = 1;
			}

			#endregion

			string sqlCommand = @"
UPDATE 
    mp_ContentStyle 
SET  
    SiteGuid = ?SiteGuid, 
    Name = ?Name, 
    Element = ?Element, 
    CssClass = ?CssClass, 
    SkinName = ?SkinName, 
    IsActive = ?IsActive, 
    LastModUtc = ?LastModUtc, 
    LastModBy = ?LastModBy 
WHERE  
    Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},

				new("?SiteGuid", MySqlDbType.VarChar, 36){
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?Name", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = name
				},

				new("?Element", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = element
				},

				new("?CssClass", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = cssClass
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				},

				new("?IsActive", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intIsActive
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
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > -1;

		}


		/// <summary>
		/// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentStyle 
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
		/// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentStyle 
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
		/// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySkin(Guid siteGuid, string skinName)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentStyle 
WHERE 
    SiteGuid = ?SiteGuid 
    AND SkinName = ?SkinName;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				}
			};


			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Deletes a row from the mp_ContentStyle table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool SetActivationBySkin(Guid siteGuid, string skinName, bool isActive)
		{
			#region Bit Conversion

			int intIsActive = 0;
			if (isActive)
			{
				intIsActive = 1;
			}

			#endregion

			string sqlCommand = @"
UPDATE 
    mp_ContentStyle 
SET  
    IsActive = ?IsActive 
WHERE  
    SiteGuid = ?SiteGuid AND SkinName = ?SkinName;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				},

				new("?IsActive", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = intIsActive
				}
			};


			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > -1;

		}

		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentStyle table.
		/// </summary>
		/// <param name="guid"> guid </param>
		public static IDataReader GetOne(Guid guid)
		{
			string sqlCommand = @"
SELECT  
    * 
FROM 
    mp_ContentStyle 
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

		/// <summary>
		/// Gets an IDataReader with all rows in the mp_ContentStyle table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid)
		{
			string sqlCommand = @"
SELECT  
    * 
FROM 
    mp_ContentStyle 
WHERE 
    SiteGuid = ?SiteGuid 
ORDER BY 
    Name;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};


			return CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the mp_ContentStyle table.
		/// </summary>
		public static IDataReader GetAll(Guid siteGuid, string skinName)
		{
			string sqlCommand = @"
SELECT * 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid
AND SkinName = ?SkinName 
ORDER BY Name;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				}
			};


			return CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the mp_ContentStyle table.
		/// </summary>
		public static IDataReader GetAllActive(Guid siteGuid)
		{
			string sqlCommand = @"
SELECT  * 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid 
AND IsActive = 1 
ORDER BY Name;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString(),
				}
			};


			return CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the mp_ContentStyle table.
		/// </summary>
		public static IDataReader GetAllActive(Guid siteGuid, string skinName)
		{
			string sqlCommand = @"
SELECT  * 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid 
AND SkinName = ?SkinName 
AND IsActive = 1 
ORDER BY Name;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				}
			};


			return CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets a count of rows in the mp_ContentStyle table.
		/// </summary>
		public static int GetCount(Guid siteGuid)
		{
			string sqlCommand = @"
SELECT Count(*) 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};


			return Convert.ToInt32(CommandHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

		}

		/// <summary>
		/// Gets a count of rows in the mp_ContentStyle table.
		/// </summary>
		public static int GetCount(Guid siteGuid, string skinName)
		{
			string sqlCommand = @"
SELECT Count(*) 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid 
AND SkinName = ?SkinName;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
				}
			};


			return Convert.ToInt32(CommandHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

		}

		/// <summary>
		/// Gets a page of data from the mp_ContentStyle table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(
			Guid siteGuid,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;
			totalPages = 1;
			int totalRows = GetCount(siteGuid);

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
SELECT * 
FROM mp_ContentStyle  
WHERE SiteGuid = ?SiteGuid 
ORDER BY Name  
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
					Value = siteGuid.ToString()
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
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets a page of data from the mp_ContentStyle table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(
			Guid siteGuid,
			string skinName,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;
			totalPages = 1;
			int totalRows = GetCount(siteGuid, skinName);

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
SELECT * 
FROM mp_ContentStyle 
WHERE SiteGuid = ?SiteGuid 
AND SkinName = ?SkinName 
ORDER BY  Name 
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
					Value = siteGuid.ToString()
				},

				new("?SkinName", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = skinName
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
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


	}
}
