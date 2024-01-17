using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBComments
{
	/// <summary>
	/// Inserts a row in the mp_Comments table. Returns rows affected count.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="parentGuid"> parentGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="featureGuid"> featureGuid </param>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <param name="contentGuid"> contentGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="title"> title </param>
	/// <param name="userComment"> userComment </param>
	/// <param name="userName"> userName </param>
	/// <param name="userEmail"> userEmail </param>
	/// <param name="userUrl"> userUrl </param>
	/// <param name="userIp"> userIp </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="moderationStatus"> moderationStatus </param>
	/// <param name="moderatedBy"> moderatedBy </param>
	/// <param name="moderationReason"> moderationReason </param>
	/// <returns>int</returns>
	public static int Create(
		Guid guid,
		Guid parentGuid,
		Guid siteGuid,
		Guid featureGuid,
		Guid moduleGuid,
		Guid contentGuid,
		Guid userGuid,
		string title,
		string userComment,
		string userName,
		string userEmail,
		string userUrl,
		string userIp,
		DateTime createdUtc,
		byte moderationStatus,
		Guid moderatedBy,
		string moderationReason)
	{
		string sqlCommand = @"
INSERT INTO mp_Comments (
    Guid, 
    ParentGuid, 
    SiteGuid, 
    FeatureGuid, 
    ModuleGuid, 
    ContentGuid, 
    UserGuid, 
    Title, 
    UserComment, 
    UserName, 
    UserEmail, 
    UserUrl, 
    UserIp, 
    CreatedUtc, 
    LastModUtc, 
    ModerationStatus, 
    ModeratedBy, 
    ModerationReason 
    )

    VALUES (
    ?Guid, 
    ?ParentGuid, 
    ?SiteGuid, 
    ?FeatureGuid, 
    ?ModuleGuid, 
    ?ContentGuid, 
    ?UserGuid, 
    ?Title, 
    ?UserComment, 
    ?UserName, 
    ?UserEmail, 
    ?UserUrl, 
    ?UserIp, 
    ?CreatedUtc, 
    ?LastModUtc, 
    ?ModerationStatus, 
    ?ModeratedBy, 
    ?ModerationReason 
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


			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

			new ("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString(),
			},


			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title,
			},


			new("?UserComment", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = userComment
			},


			new("?UserName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userName
			},


			new("?UserEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = userEmail
			},


			new("?UserUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = userUrl
			},


			new("?UserIp", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userIp,
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


			new("?ModerationStatus", MySqlDbType.Int16)
			{
				Direction = ParameterDirection.Input,
				Value = moderationStatus
			},


			new("?ModeratedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moderatedBy.ToString()
			},


			new("?ModerationReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moderationReason
			}

		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_Comments table. Returns true if row updated.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="title"> title </param>
	/// <param name="userComment"> userComment </param>
	/// <param name="userName"> userName </param>
	/// <param name="userEmail"> userEmail </param>
	/// <param name="userUrl"> userUrl </param>
	/// <param name="userIp"> userIp </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <param name="moderationStatus"> moderationStatus </param>
	/// <param name="moderatedBy"> moderatedBy </param>
	/// <param name="moderationReason"> moderationReason </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid guid,
		Guid userGuid,
		string title,
		string userComment,
		string userName,
		string userEmail,
		string userUrl,
		string userIp,
		DateTime lastModUtc,
		byte moderationStatus,
		Guid moderatedBy,
		string moderationReason)
	{
		string sqlCommand = @"
UPDATE mp_Comments 
SET  
	UserGuid = ?UserGuid, 
	Title = ?Title, 
	UserComment = ?UserComment, 
	UserName = ?UserName, 
	UserEmail = ?UserEmail, 
	UserUrl = ?UserUrl, 
	UserIp = ?UserIp, 
	LastModUtc = ?LastModUtc, 
	ModerationStatus = ?ModerationStatus, 
	ModeratedBy = ?ModeratedBy, 
	ModerationReason = ?ModerationReason 
WHERE  
	Guid = ?Guid;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},


			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},


			new("?Title", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},


			new("?UserComment", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = userComment
			},


			new("?UserName", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userName
			},


			new("?UserEmail", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = userEmail
			},


			new("?UserUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = userUrl
			},


			new("?UserIp", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userIp
			},


			new("?LastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModUtc
			},


			new("?ModerationStatus", MySqlDbType.Int16)
			{
				Direction = ParameterDirection.Input,
				Value = moderationStatus
			},


			new("?ModeratedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moderatedBy.ToString()
			},


			new("?ModerationReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = moderationReason
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;
	}

	/// <summary>
	/// Deletes a row from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
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
	/// Deletes rows from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByContent(Guid contentGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
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
			arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes rows from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
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
	/// Deletes rows from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByFeature(Guid featureGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
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
	/// Deletes rows from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByModule(Guid moduleGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
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
	/// Deletes rows from the mp_Comments table. Returns true if row deleted.
	/// </summary>
	/// <param name="guid"> guid </param>
	/// <returns>bool</returns>
	public static bool DeleteByParent(Guid parentGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_Comments 
WHERE 
	ParentGuid = ?ParentGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_Comments table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT  c.*, 
	COALESCE(u.Name, c.UserName) AS PostAuthor, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Email, c.UserEmail) AS AuthorEmail, 
	COALESCE(u.TotalRevenue, 0) AS UserRevenue, 
	COALESCE(u.Trusted, 0) AS Trusted, 
	u.AvatarUrl AS PostAuthorAvatar, 
	COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl
FROM	mp_Comments c 
LEFT OUTER JOIN mp_Users u 
	ON c.UserGuid = u.UserGuid 
WHERE 
	c.Guid = ?Guid;";

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
	/// Gets an IDataReader with rows from the mp_Comments table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByContentAsc(Guid contentGuid)
	{
		string sqlCommand = @"
SELECT  c.*, 
	COALESCE(u.Name, c.UserName) AS PostAuthor, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Email, c.UserEmail) AS AuthorEmail, 
	COALESCE(u.TotalRevenue, 0) AS UserRevenue, 
	COALESCE(u.Trusted, 0) AS Trusted, 
	u.AvatarUrl AS PostAuthorAvatar, 
	COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl 
FROM	mp_Comments c 
LEFT OUTER JOIN mp_Users u ON c.UserGuid = u.UserGuid 
WHERE 
	c.ContentGuid = ?ContentGuid 
ORDER BY c.CreatedUtc;";

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
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_Comments table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByContentDesc(Guid contentGuid)
	{
		string sqlCommand = @"
SELECT  c.*, 
	COALESCE(u.Name, c.UserName) AS PostAuthor, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Email, c.UserEmail) AS AuthorEmail, 
	COALESCE(u.TotalRevenue, 0) AS UserRevenue, 
	COALESCE(u.Trusted, 0) AS Trusted, 
	u.AvatarUrl AS PostAuthorAvatar, 
	COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl 
FROM	mp_Comments c 
LEFT OUTER JOIN mp_Users u ON c.UserGuid = u.UserGuid 
WHERE 
	c.ContentGuid = ?ContentGuid 
ORDER BY c.CreatedUtc DESC;";

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
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_Comments table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByParentAsc(Guid parentGuid)
	{
		string sqlCommand = @"
SELECT  c.*, 
	COALESCE(u.Name, c.UserName) AS PostAuthor, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Email, c.UserEmail) AS AuthorEmail, 
	COALESCE(u.TotalRevenue, 0) AS UserRevenue, 
	COALESCE(u.Trusted, 0) AS Trusted, 
	u.AvatarUrl AS PostAuthorAvatar, 
	COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl 
FROM	mp_Comments c 
LEFT OUTER JOIN mp_Users u ON c.UserGuid = u.UserGuid 
WHERE 
	c.ParentGuid = ?ParentGuid 
ORDER BY c.CreatedUtc 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_Comments table.
	/// </summary>
	/// <param name="guid"> guid </param>
	public static IDataReader GetByParentDesc(Guid parentGuid)
	{
		string sqlCommand = @"
SELECT  c.*, 
	COALESCE(u.Name, c.UserName) AS PostAuthor, 
	COALESCE(u.UserID, -1) AS UserID, 
	COALESCE(u.Email, c.UserEmail) AS AuthorEmail, 
	COALESCE(u.TotalRevenue, 0) AS UserRevenue, 
	COALESCE(u.Trusted, 0) AS Trusted, 
	u.AvatarUrl AS PostAuthorAvatar, 
	COALESCE(c.UserUrl, u.WebSiteURL) AS PostAuthorWebSiteUrl 
FROM	mp_Comments c 
LEFT OUTER JOIN mp_Users u ON c.UserGuid = u.UserGuid 
WHERE 
	c.ParentGuid = ?ParentGuid 
ORDER BY c.CreatedUtc DESC 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			}
	};



		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);
	}

	/// <summary>
	/// Gets a count of rows in the mp_Comments table.
	/// </summary>
	public static int GetCount(Guid contentGuid, int moderationStatus)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_Comments 
WHERE 
	ContentGuid = ?ContentGuid 
AND 
	ModerationStatus = ?ModerationStatus 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?ContentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = contentGuid.ToString()
			},

		new("?ModerationStatus", MySqlDbType.UInt16)
		{
			Direction = ParameterDirection.Input,
			Value = moderationStatus
		}
	};



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	public static int GetCountByModule(Guid moduleGuid, int moderationStatus)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_Comments 
WHERE 
	ModuleGuid = ?ModuleGuid 
AND 
	ModerationStatus = ?ModerationStatus 
;";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			},

			new("?ModerationStatus", MySqlDbType.UInt16)
			{
				Direction = ParameterDirection.Input,
				Value = moderationStatus
			}
	};



		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}

	public static int GetCountBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT  Count(*) 
FROM	mp_Comments 
WHERE 
	?SiteGuid = '00000000-0000-0000-0000-000000000000' 
OR 
	SiteGuid = ?SiteGuid;";

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

}
