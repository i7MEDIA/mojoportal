using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;


public static class DBFileAttachment
{
	/// <summary>
	/// Inserts a row in the mp_FileAttachment table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="itemGuid"> itemGuid </param>
	/// <param name="specialGuid1"> specialGuid1 </param>
	/// <param name="specialGuid2"> specialGuid2 </param>
	/// <param name="serverFileName"> serverFileName </param>
	/// <param name="fileName"> fileName </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdBy"> createdBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		Guid siteGuid,
		Guid moduleGuid,
		Guid itemGuid,
		Guid specialGuid1,
		Guid specialGuid2,
		string serverFileName,
		string fileName,
		string contentTitle,
		long contentLength,
		string contentType,
		DateTime createdUtc,
		Guid createdBy)
	{
		string sqlCommand = @"
INSERT INTO mp_FileAttachment (
        RowGuid, 
        SiteGuid, 
        ModuleGuid, 
        ItemGuid, 
        SpecialGuid1, 
        SpecialGuid2, 
        ServerFileName, 
        FileName, 
        ContentTitle, 
        ContentLength, 
        ContentType, 
        CreatedUtc, 
        CreatedBy 
    )
VALUES (
    ?RowGuid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?ItemGuid, 
    ?SpecialGuid1, 
    ?SpecialGuid2, 
    ?ServerFileName, 
    ?FileName, 
    ?ContentTitle, 
    ?ContentLength, 
    ?ContentType, 
    ?CreatedUtc, 
    ?CreatedBy 
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

			new("?SpecialGuid1", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid1.ToString()
			},

			new("?SpecialGuid2", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid2.ToString()
			},

			new("?ServerFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			},

			new("?FileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fileName
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

			new("?ContentLength", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = contentLength
			},

			new("?ContentType", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = contentType
			},

			new("?ContentTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = contentTitle
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_FileAttachment table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="itemGuid"> itemGuid </param>
	/// <param name="specialGuid1"> specialGuid1 </param>
	/// <param name="specialGuid2"> specialGuid2 </param>
	/// <param name="serverFileName"> serverFileName </param>
	/// <param name="fileName"> fileName </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdBy"> createdBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		string serverFileName,
		string fileName,
		string contentTitle,
		long contentLength,
		string contentType)
	{
		#region Bit Conversion


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_FileAttachment 
SET 
    ServerFileName = ?ServerFileName, 
    FileName = ?FileName, 
    ContentTitle = ?ContentTitle, 
    ContentLenth = ?ContentLenth, 
    ContentType = ?ContentType 
WHERE RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?ServerFileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			},

			new("?FileName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = fileName
			},

			new("?ContentLenth", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = contentLength
			},

			new("?ContentType", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = contentType
			},

			new("?ContentTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = contentTitle
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_FileAttachment table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_FileAttachment 
WHERE RowGuid = ?RowGuid;";

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
	/// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_FileAttachment 
WHERE SiteGuid = ?SiteGuid;";

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
	/// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_FileAttachment 
WHERE ModuleGuid = ?ModuleGuid;";

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
	/// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByItem(Guid itemGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_FileAttachment 
WHERE ItemGuid = ?ItemGuid;";

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_FileAttachment table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_FileAttachment 
WHERE RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with row sfrom the mp_FileAttachment table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader SelectByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_FileAttachment 
WHERE ModuleGuid = ?ModuleGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_FileAttachment table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader SelectByItem(Guid itemGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_FileAttachment 
WHERE ItemGuid = ?ItemGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_FileAttachment table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader SelectBySpecial1(Guid specialGuid1)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_FileAttachment 
WHERE SpecialGuid1 = ?SpecialGuid1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SpecialGuid1", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid1.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_FileAttachment table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader SelectBySpecial2(Guid specialGuid2)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_FileAttachment 
WHERE SpecialGuid2 = ?SpecialGuid2;";

		var arParams = new List<MySqlParameter>
		{
			new("?SpecialGuid2", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid2.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

}
