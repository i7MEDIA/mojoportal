using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBEmailTemplate
{


	/// <summary>
	/// Inserts a row in the mp_EmailTemplate table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="specialGuid1"> specialGuid1 </param>
	/// <param name="specialGuid2"> specialGuid2 </param>
	/// <param name="name"> name </param>
	/// <param name="subject"> subject </param>
	/// <param name="textBody"> textBody </param>
	/// <param name="htmlBody"> htmlBody </param>
	/// <param name="hasHtml"> hasHtml </param>
	/// <param name="isEditable"> isEditable </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid siteGuid,
		Guid featureGuid,
		Guid moduleGuid,
		Guid specialGuid1,
		Guid specialGuid2,
		string name,
		string subject,
		string textBody,
		string htmlBody,
		bool hasHtml,
		bool isEditable,
		DateTime createdUtc,
		Guid lastModBy)
	{
		#region Bit Conversion

		int intHasHtml = 0;
		if (hasHtml)
		{
			intHasHtml = 1;
		}

		int intIsEditable = 0;
		if (isEditable)
		{
			intIsEditable = 1;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO mp_EmailTemplate (
        Guid, 
        SiteGuid, 
        FeatureGuid, 
        ModuleGuid, 
        SpecialGuid1, 
        SpecialGuid2, 
        Name, 
        Subject, 
        TextBody, 
        HtmlBody, 
        HasHtml, 
        IsEditable, 
        CreatedUtc, 
        LastModUtc, 
        LastModBy 
    ) 
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?FeatureGuid, 
    ?ModuleGuid, 
    ?SpecialGuid1, 
    ?SpecialGuid2, 
    ?Name, 
    ?Subject, 
    ?TextBody, 
    ?HtmlBody, 
    ?HasHtml, 
    ?IsEditable, 
    ?CreatedUtc, 
    ?LastModUtc, 
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

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?TextBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = textBody
			},

			new("?HtmlBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = htmlBody
			},

			new("?HasHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intHasHtml
			},

			new("?IsEditable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsEditable
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

			new("?LastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;


	}

	/// <summary>
	/// Updates a row in the mp_EmailTemplate table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="name"> name </param>
	/// <param name="subject"> subject </param>
	/// <param name="textBody"> textBody </param>
	/// <param name="htmlBody"> htmlBody </param>
	/// <param name="hasHtml"> hasHtml </param>
	/// <param name="isEditable"> isEditable </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		string name,
		string subject,
		string textBody,
		string htmlBody,
		bool hasHtml,
		bool isEditable,
		DateTime lastModUtc,
		Guid lastModBy)
	{
		#region Bit Conversion

		int intHasHtml = 0;
		if (hasHtml)
		{
			intHasHtml = 1;
		}

		int intIsEditable = 0;
		if (isEditable)
		{
			intIsEditable = 1;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_EmailTemplate 
SET  
    Name = ?Name, 
    Subject = ?Subject, 
    TextBody = ?TextBody, 
    HtmlBody = ?HtmlBody, 
    HasHtml = ?HasHtml, 
    IsEditable = ?IsEditable, 
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

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?TextBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = textBody
			},

			new("?HtmlBody", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = htmlBody
			},

			new("?HasHtml", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intHasHtml
			},

			new("?IsEditable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsEditable
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
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 
WHERE Guid = ?Guid;";

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

	/// <summary>
	/// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 
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
	/// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 
WHERE FeatureGuid = ?FeatureGuid;";

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
	/// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 
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
	/// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySpecial1(Guid specialGuid1)
	{
		if (specialGuid1 == Guid.Empty) { return false; }

		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 
WHERE SpecialGuid1 = ?SpecialGuid1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SpecialGuid1", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid1.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes from the mp_EmailTemplate table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySpecial2(Guid specialGuid2)
	{
		if (specialGuid2 == Guid.Empty) { return false; }

		string sqlCommand = @"
DELETE FROM mp_EmailTemplate 

WHERE 
SpecialGuid2 = ?SpecialGuid2 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?SpecialGuid2", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid2.ToString()
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_EmailTemplate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT *
FROM mp_EmailTemplate
WHERE Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader Get(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_EmailTemplate 
WHERE SiteGuid = ?SiteGuid 
AND (?FeatureGuid = '00000000-0000-0000-0000-000000000000' OR FeatureGuid = ?FeatureGuid) 
AND (?ModuleGuid = '00000000-0000-0000-0000-000000000000' OR ModuleGuid = ?ModuleGuid) 
AND (?SpecialGuid1 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid1 = ?SpecialGuid1) 
AND (?SpecialGuid2 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid2 = ?SpecialGuid2) 
ORDER BY Name;";

		var arParams = new List<MySqlParameter>
		{
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

			new("?SpecialGuid1", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid1.ToString()
			},

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

	/// <summary>
	/// Gets an IDataReader with one row from the mp_EmailTemplate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_EmailTemplate 
WHERE ModuleGuid = ?ModuleGuid 
ORDER BY Name;";

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
	/// Gets an IDataReader with one row from the mp_EmailTemplate table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByModule(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_EmailTemplate 
WHERE ModuleGuid = ?ModuleGuid 
AND SpecialGuid1 = ?SpecialGuid1 
AND SpecialGuid2 = ?SpecialGuid2 
ORDER BY Name;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
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
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_EmailTemplate 
WHERE SiteGuid = ?SiteGuid 
AND FeatureGuid = ?FeatureGuid 
ORDER BY Name;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}

	public static int GetCount(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_EmailTemplate 
WHERE SiteGuid = ?SiteGuid 
AND (?FeatureGuid = '00000000-0000-0000-0000-000000000000' OR FeatureGuid = ?FeatureGuid) 
AND (?ModuleGuid = '00000000-0000-0000-0000-000000000000' OR ModuleGuid = ?ModuleGuid) 
AND (?SpecialGuid1 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid1 = ?SpecialGuid1) 
AND (?SpecialGuid2 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid2 = ?SpecialGuid2);";

		var arParams = new List<MySqlParameter>
		{
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

			new("?SpecialGuid1", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid1.ToString()
			},

			new("?SpecialGuid2", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = specialGuid2.ToString()
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a count of rows in the mp_EmailTemplate table.
	/// </summary>
	public static int GetCountByModuleAndName(Guid moduleGuid, string name)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_EmailTemplate 
WHERE ModuleGuid = ?ModuleGuid 
AND Name = ?Name;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			}
		};




		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	/// <summary>
	/// Gets a count of rows in the mp_EmailTemplate table.
	/// </summary>
	public static int GetCountByModuleSpecialAndName(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2, string name)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_EmailTemplate 
WHERE ModuleGuid = ?ModuleGuid 
AND SpecialGuid1 = ?SpecialGuid1 
AND SpecialGuid2 = ?SpecialGuid2 
AND Name = ?Name;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
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

			new("?Name", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = name
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	public static int GetCountByFeature(Guid siteGuid, Guid featureGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_EmailTemplate 
WHERE SiteGuid = ?SiteGuid 
AND FeatureGuid = ?FeatureGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			}
		};


		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));

	}

	public static IDataReader GetPageByFeature(
		Guid siteGuid,
		Guid featureGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByFeature(siteGuid, featureGuid);

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
FROM	mp_EmailTemplate  
WHERE 
SiteGuid = ?SiteGuid 
AND FeatureGuid = ?FeatureGuid 
ORDER BY  
Name  
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

			new("?FeatureGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = featureGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new ("?OffsetRows", MySqlDbType.Int32)
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

}
