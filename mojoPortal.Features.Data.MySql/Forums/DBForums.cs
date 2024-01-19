using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySqlConnector;


namespace mojoPortal.Data;

public static class DBForums
{
	public static int Create(
		Guid forumGuid,
		int moduleId,
		int userId,
		string title,
		string description,
		bool isModerated,
		bool isActive,
		int sortOrder,
		int postsPerPage,
		int threadsPerPage,
		bool allowAnonymousPosts,

		string rolesThatCanPost,
		string rolesThatCanModerate,
		string moderatorNotifyEmail,
		bool includeInGoogleMap,
		bool addNoIndexMeta,
		bool closed,
		bool visible,
		bool requireModeration,
		bool requireModForNotify,
		bool allowTrustedDirectPosts,
		bool allowTrustedDirectNotify
	)
	{
		#region Bit Conversion

		byte moderated = 1;
		if (!isModerated)
		{
			moderated = 0;
		}

		byte active = 1;
		if (!isActive)
		{
			active = 0;
		}

		byte allowAnonymous = 1;
		if (!allowAnonymousPosts)
		{
			allowAnonymous = 0;
		}

		int intIncludeInGoogleMap = includeInGoogleMap ? 1 : 0;
		int intAddNoIndexMeta = addNoIndexMeta ? 1 : 0;
		int intClosed = closed ? 1 : 0;
		int intVisible = visible ? 1 : 0;
		int intRequireModeration = requireModeration ? 1 : 0;
		int intRequireModForNotify = requireModForNotify ? 1 : 0;
		int intAllowTrustedDirectPosts = allowTrustedDirectPosts ? 1 : 0;
		int intAllowTrustedDirectNotify = allowTrustedDirectNotify ? 1 : 0;

		#endregion

		string sqlCommand = @"
INSERT INTO	mp_Forums ( 
	ModuleID, 
	CreatedBy, 
	CreatedDate, 
	Title, 
	Description , 
	IsModerated , 
	IsActive , 
	SortOrder , 
	PostsPerPage , 
	ThreadsPerPage , 
	ForumGuid , 
	RolesThatCanPost, 
	RolesThatCanModerate, 
	ModeratorNotifyEmail, 
	IncludeInGoogleMap, 
	AddNoIndexMeta, 
	Closed, 
	Visible, 
	RequireModeration, 
	RequireModForNotify, 
	AllowTrustedDirectPosts, 
	AllowTrustedDirectNotify, 
	AllowAnonymousPosts 
) 
VALUES (
	?ModuleID , 
	?UserID  , 
	now(), 
	?Title , 
	?Description , 
	?IsModerated , 
	?IsActive , 
	?SortOrder , 
	?PostsPerPage , 
	?ThreadsPerPage , 
	?ForumGuid , 
	?RolesThatCanPost, 
	?RolesThatCanModerate, 
	?ModeratorNotifyEmail, 
	?IncludeInGoogleMap, 
	?AddNoIndexMeta, 
	?Closed, 
	?Visible, 
	?RequireModeration, 
	?RequireModForNotify, 
	?AllowTrustedDirectPosts, 
	?AllowTrustedDirectNotify, 
	?AllowAnonymousPosts 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?Title", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?IsModerated", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moderated
			},

			new("?IsActive", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = active
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?PostsPerPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postsPerPage
			},

			new("?ThreadsPerPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadsPerPage
			},

			new("?AllowAnonymousPosts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowAnonymous
			},

			new("?ForumGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = forumGuid.ToString()
			},

			new("?RolesThatCanPost", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanPost
			},

			new("?RolesThatCanModerate", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanModerate
			},

			new("?ModeratorNotifyEmail", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = moderatorNotifyEmail
			},

			new("?IncludeInGoogleMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInGoogleMap
			},

			new("?AddNoIndexMeta", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAddNoIndexMeta
			},

			new("?Closed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intClosed
			},

			new("?Visible", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intVisible
			},

			new("?RequireModeration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequireModeration
			},

			new("?RequireModForNotify", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequireModForNotify
			},

			new("?AllowTrustedDirectPosts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowTrustedDirectPosts
			},

			new("?AllowTrustedDirectNotify", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowTrustedDirectNotify
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return newID;

	}



	public static bool Update(
		int itemId,
		int userId,
		string title,
		string description,
		bool isModerated,
		bool isActive,
		int sortOrder,
		int postsPerPage,
		int threadsPerPage,
		bool allowAnonymousPosts,
		string rolesThatCanPost,
		string rolesThatCanModerate,
		string moderatorNotifyEmail,
		bool includeInGoogleMap,
		bool addNoIndexMeta,
		bool closed,
		bool visible,
		bool requireModeration,
		bool requireModForNotify,
		bool allowTrustedDirectPosts,
		bool allowTrustedDirectNotify
		)
	{
		#region Bit Conversion

		byte moderated = 1;
		if (!isModerated)
		{
			moderated = 0;
		}

		byte active = 1;
		if (!isActive)
		{
			active = 0;
		}

		byte allowAnonymous = 1;
		if (!allowAnonymousPosts)
		{
			allowAnonymous = 0;
		}

		int intIncludeInGoogleMap = includeInGoogleMap ? 1 : 0;
		int intAddNoIndexMeta = addNoIndexMeta ? 1 : 0;
		int intClosed = closed ? 1 : 0;
		int intVisible = visible ? 1 : 0;
		int intRequireModeration = requireModeration ? 1 : 0;
		int intRequireModForNotify = requireModForNotify ? 1 : 0;
		int intAllowTrustedDirectPosts = allowTrustedDirectPosts ? 1 : 0;
		int intAllowTrustedDirectNotify = allowTrustedDirectNotify ? 1 : 0;

		#endregion

		string sqlCommand = @"
UPDATE 
	mp_Forums 
SET	
	Title = ?Title, 
	Description = ?Description, 
	IsModerated = ?IsModerated, 
	IsActive = ?IsActive, 
	SortOrder = ?SortOrder, 
	PostsPerPage = ?PostsPerPage, 
	ThreadsPerPage = ?ThreadsPerPage, 
	RolesThatCanPost = ?RolesThatCanPost, 
	RolesThatCanModerate = ?RolesThatCanModerate, 
	ModeratorNotifyEmail = ?ModeratorNotifyEmail, 
	IncludeInGoogleMap = ?IncludeInGoogleMap, 
	AddNoIndexMeta = ?AddNoIndexMeta, 
	Closed = ?Closed, 
	Visible = ?Visible, 
	RequireModeration = ?RequireModeration, 
	RequireModForNotify = ?RequireModForNotify, 
	AllowTrustedDirectPosts = ?AllowTrustedDirectPosts, 
	AllowTrustedDirectNotify = ?AllowTrustedDirectNotify, 
	AllowAnonymousPosts = ?AllowAnonymousPosts 
WHERE 
	ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?Title", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = title
			},

			new("?Description", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			},

			new("?IsModerated", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moderated
			},

			new("?IsActive", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = active
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?PostsPerPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postsPerPage
			},

			new("?ThreadsPerPage", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadsPerPage
			},

			new("?AllowAnonymousPosts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = allowAnonymous
			},

			new("?RolesThatCanPost", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanPost
			},

			new("?RolesThatCanModerate", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rolesThatCanModerate
			},

			new("?ModeratorNotifyEmail", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = moderatorNotifyEmail
			},

			new("?IncludeInGoogleMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInGoogleMap
			},

			new("?AddNoIndexMeta", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAddNoIndexMeta
			},

			new("?Closed", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intClosed
			},

			new("?Visible", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intVisible
			},

			new("?RequireModeration", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequireModeration
			},

			new("?RequireModForNotify", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRequireModForNotify
			},

			new("?AllowTrustedDirectPosts", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowTrustedDirectPosts
			},

			new("?AllowTrustedDirectNotify", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intAllowTrustedDirectNotify
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool Delete(int itemId)
	{
		string sqlCommand = @"
DELETE FROM mp_ForumPosts 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID = ?ItemID 
);
DELETE FROM mp_ForumThreadSubscriptions 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID = ?ItemID 
);
DELETE FROM mp_ForumThreads 
WHERE ForumID = ?itemId ;
DELETE FROM mp_ForumSubscriptions 
WHERE ForumID = ?ItemID ;
DELETE FROM mp_Forums 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteByModule(int moduleId)
	{
		string sqlCommand = @"
DELETE FROM mp_ForumPosts 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID IN (
		SELECT ItemID 
		FROM mp_Forums 
		WHERE ModuleID = ?ModuleID
	) 
);
DELETE FROM mp_ForumThreadSubscriptions 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID IN (
		SELECT ItemID 
		FROM mp_Forums 
		WHERE ModuleID = ?ModuleID
	) 
);
DELETE FROM mp_ForumThreads 
WHERE ForumID IN (
	SELECT ItemID 
	FROM mp_Forums 
	WHERE ModuleID = ?ModuleId
);
DELETE FROM mp_ForumSubscriptions 
WHERE ForumID IN (
	SELECT ItemID 
	FROM mp_Forums 
	WHERE ModuleID = ?ModuleID
) ;
DELETE FROM mp_Forums 
WHERE ModuleID = ?moduleId";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static bool DeleteBySite(int siteId)
	{
		string sqlCommand = @"
DELETE FROM mp_ForumPosts 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID IN (
		SELECT ItemID 
		FROM mp_Forums 
		WHERE ModuleID IN (
			SELECT ModuleID 
			FROM mp_Modules 
			WHERE SiteID = ?SiteID
		) 
	) 
);
DELETE FROM mp_ForumThreadSubscriptions 
WHERE ThreadID IN (
	SELECT ThreadID 
	FROM mp_ForumThreads 
	WHERE ForumID IN (
		SELECT ItemID 
		FROM mp_Forums 
		WHERE ModuleID IN (
			SELECT ModuleID 
			FROM mp_Modules 
			WHERE SiteID = ?SiteID
		) 
	) 
);
DELETE FROM mp_ForumThreads 
WHERE ForumID IN (
	SELECT ItemID 
	FROM mp_Forums 
	WHERE ModuleID IN (
		SELECT ModuleID 
		FROM mp_Modules 
		WHERE SiteID = ?SiteID
	) 
);
DELETE FROM mp_ForumSubscriptions 
WHERE ForumID IN (
	SELECT ItemID 
	FROM mp_Forums 
	WHERE ModuleID IN (
		SELECT ModuleID 
		FROM mp_Modules 
		WHERE SiteID = ?SiteID
	) 
);
DELETE FROM mp_Forums 
WHERE ModuleID IN (
	SELECT ModuleID 
	FROM mp_Modules 
	WHERE SiteID = ?SiteID
);";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}



	public static IDataReader GetForums(int moduleId, int userId)
	{

		string sqlCommand = @"
SELECT 
	f.*, 
	u.Name As MostRecentPostUser, 
	s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL As Subscribed, 
	(
		SELECT COUNT(*) 
		FROM mp_ForumSubscriptions fs 
		WHERE fs.ForumID = f.ItemID 
		AND fs.UnSubscribeDate IS NULL
	) 
	As SubscriberCount  
FROM 
	mp_Forums f 
LEFT OUTER JOIN 
	mp_Users u 
ON 
	f.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN	
	mp_ForumSubscriptions s 
ON f.ItemID = s.ForumID 
AND s.UserID = ?UserID 
AND s.UnSubscribeDate IS NULL 
WHERE 
	f.ModuleID	= ?ModuleID 
AND 
	f.IsActive = 1 
ORDER BY 
	f.SortOrder, 
	f.ItemID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ModuleID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetForum(int itemId)
	{
		string sqlCommand = @"
SELECT 
	f.*, 
	u.Name As CreatedByUser, 
	up.Name As MostRecentPostUser 
FROM 
	mp_Forums f 
LEFT OUTER JOIN	
	mp_Users u 
ON 
	f.CreatedBy = u.UserID 
LEFT OUTER JOIN	
	mp_Users up 
ON 
	f.MostRecentPostUserID = up.UserID 
WHERE 
	f.ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static bool IncrementPostCount(
		int forumId,
		int mostRecentPostUserId,
		DateTime mostRecentPostDate)
	{

		string sqlCommand = @"
UPDATE mp_Forums 
SET 
	MostRecentPostDate = ?MostRecentPostDate, 
	MostRecentPostUserID = ?MostRecentPostUserID, 
	PostCount = PostCount + 1 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?MostRecentPostUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostUserId
			},

			new("?MostRecentPostDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostDate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), arParams);

		return rowsAffected > -1;

	}

	public static bool UpdateUserStats(int userId)
	{
		string sqlCommand = @"
UPDATE mp_Users 
SET  
	TotalPosts = (
		SELECT COUNT(*) 
		FROM mp_ForumPosts 
		WHERE mp_ForumPosts.UserID = mp_Users.UserID 
	) ";

		if (userId > -1)
		{
			sqlCommand += "WHERE UserID = ?UserID ;";
		}

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool IncrementPostCount(int forumId)
	{

		string sqlCommand = @"
UPDATE mp_Forums 
SET PostCount = PostCount + 1 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool DecrementPostCount(int forumId)
	{

		string sqlCommand = @"
UPDATE mp_Forums 
SET PostCount = PostCount - 1 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;


	}

	public static bool RecalculatePostStats(int forumId)
	{
		DateTime mostRecentPostDate = DateTime.UtcNow;
		int mostRecentPostUserID = -1;
		int postCount = 0;


		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		string sqlCommand = @"
SELECT 
MostRecentPostDate, 
MostRecentPostUserID 
FROM mp_ForumThreads 
WHERE ForumID = ?ForumID 
ORDER BY MostRecentPostDate DESC 
LIMIT 1 ;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				if (reader["MostRecentPostUserID"] != DBNull.Value)
				{
					mostRecentPostUserID = Convert.ToInt32(reader["MostRecentPostUserID"]);
				}
				if (reader["MostRecentPostDate"] != DBNull.Value)
				{
					mostRecentPostDate = Convert.ToDateTime(reader["MostRecentPostDate"]);
				}
			}
		}

		string sqlCommand1 = @"
SELECT 
COALESCE(SUM(TotalReplies) + COUNT(*),0) As PostCount 
FROM mp_ForumThreads 
WHERE ForumID = ?ForumID ;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand1.ToString(),
			arParams))
		{

			if (reader.Read())
			{
				postCount = Convert.ToInt32(reader["PostCount"]);
			}
		}

		string sqlCommand2 = @"
UPDATE	mp_Forums 
SET	 
MostRecentPostDate = ?MostRecentPostDate, 
MostRecentPostUserID = ?MostRecentPostUserID, 
PostCount = ?PostCount 
WHERE ItemID = ?ForumID ;";

		var arParams3 = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?MostRecentPostDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostDate
			},

			new("?MostRecentPostUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostUserID
			},

			new("?PostCount", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postCount
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand2.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool IncrementThreadCount(int forumId)
	{
		string sqlCommand = @"
UPDATE mp_Forums 
SET	ThreadCount = ThreadCount + 1 
WHERE ItemID = ?ItemID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool DecrementThreadCount(int forumId)
	{

		string sqlCommand = @"
UPDATE mp_Forums 
SET ThreadCount = ThreadCount - 1 
WHERE ItemID = ?ItemID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ItemID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static int GetUserThreadCount(int userId, int siteId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ForumThreads ft 
JOIN mp_Forums f 
ON ft.ForumID = f.ItemID 
JOIN mp_Modules m 
ON f.ModuleID = m.ModuleID 
WHERE m.SiteID = ?SiteID 
AND ft.ThreadID IN (
	Select DISTINCT ThreadID 
	FROM mp_ForumPosts 
	WHERE mp_ForumPosts.UserID = ?UserID
);";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));
	}


	public static IDataReader GetThreadPageByUser(
		int userId,
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetUserThreadCount(userId, siteId);

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

		string sqlCommand = $@"
SELECT	 
	t.*, 
	f.Title As Forum, 
	f.ModuleID, 
	(
		SELECT PageID 
		FROM mp_PageModules 
		WHERE mp_PageModules.ModuleID = f.ModuleID 
		AND (PublishEndDate IS NULL OR PublishEndDate > ?CurrentDate) 
		LIMIT 1
	) As PageID, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	s.Name As StartedBy 
FROM 
	mp_ForumThreads t 
JOIN 
	mp_Forums f 
ON t.ForumID = f.ItemID 
JOIN 
	mp_Modules m 
ON f.ModuleID = m.ModuleID 
LEFT OUTER JOIN	
	mp_Users u 
ON t.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN	
	mp_Users s 
ON t.StartedByUserID = s.UserID 
WHERE 
	m.SiteID = ?SiteID 
AND 
	t.ThreadID IN (
		Select DISTINCT ThreadID 
		FROM mp_ForumPosts 
		WHERE mp_ForumPosts.UserID = ?UserID
	) 
ORDER BY t.MostRecentPostDate DESC 
LIMIT 
	{pageLowerBound.ToString(CultureInfo.InvariantCulture)}, 
	?PageSize ;";


		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new ("?CurrentDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new ("?SiteID", MySqlDbType.Int32)
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



	public static IDataReader GetThreadsForSiteMap(int siteId)
	{
		string sqlCommand = @"
SELECT 
	ft.ThreadID, 
	ft.MostRecentPostDate, 
	f.ModuleID, 
	m.ViewRoles, 
	p.PageID, 
	p.AuthorizedRoles 
FROM mp_ForumThreads ft 
JOIN 
	mp_Forums f 
ON f.ItemID = ft.ForumID 
JOIN 
	mp_Modules m 
ON f.ModuleID = m.ModuleID 
JOIN 
	mp_PageModules pm 
ON pm.ModuleID = m.ModuleID 
JOIN 
	mp_Pages p 
ON p.PageID = pm.PageID 
WHERE p.SiteID = ?SiteID 
AND ft.IncludeInSiteMap = 1 
ORDER BY ft.ThreadID DESC ;";

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


	public static IDataReader GetThreads(int forumId, int pageNumber)
	{

		int pageSize = 1;
		int totalRows = 0;
		using (IDataReader reader = GetForum(forumId))
		{
			if (reader.Read())
			{
				pageSize = Convert.ToInt32(reader["ThreadsPerPage"]);
				totalRows = Convert.ToInt32(reader["ThreadCount"]);
			}
		}

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
	t.*, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	s.Name As StartedBy 
FROM mp_ForumThreads t 
LEFT OUTER JOIN	
	mp_Users u 
ON t.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON t.StartedByUserID = s.UserID 
WHERE t.ForumID = ?ForumID 
ORDER BY 
	t.SortOrder, 
	t.MostRecentPostDate DESC ";

		if (pageNumber > 1)
		{
			sqlCommand += "LIMIT {offset.ToString(CultureInfo.InvariantCulture)}, {pageSize.ToString(CultureInfo.InvariantCulture)} ";
		}
		else
		{
			sqlCommand += "LIMIT {pageSize.ToString(CultureInfo.InvariantCulture)} ";
		}

		sqlCommand += " ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int ForumThreadGetPostCount(int threadId)
	{

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		string sqlCommand = @"
SELECT COUNT(*) FROM mp_ForumPosts WHERE ThreadID = ?ThreadID; ";

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams).ToString());

		return count;

	}

	public static int GetSubscriberCount(int forumId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ForumSubscriptions 
WHERE ForumID = ?ForumID 
AND UnSubscribeDate IS NULL ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

	}


	public static IDataReader GetSubscriberPage(
		int forumId,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetSubscriberCount(forumId);

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
	fs.SubscriptionID, 
	fs.SubscribeDate, 
	u.Name, 
	u.LoginName, 
	u.Email 
FROM mp_ForumSubscriptions fs  
LEFT OUTER JOIN 
	mp_Users u 
ON 
	u.UserID = fs.UserID 
WHERE 
	fs.ForumID = ?ForumID 
AND 
	fs.UnSubscribeDate IS NULL 
ORDER BY  
	u.Name  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
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


	public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
	{
		string sqlCommand = @"=
SELECT COUNT(*) As SubscriptionCount 
FROM mp_ForumSubscriptions  
WHERE ForumID = ?ForumID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int subscriptionCount = 0;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
			}
		}

		int rowsAffected = -1;

		if (subscriptionCount > 0)
		{
			var arParams1 = new List<MySqlParameter>
			{
				new("?ForumID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = forumId
				},

				new("?UserID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = userId
				},

				new("?SubscribeDate", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = DateTime.UtcNow
				}
			};

			string sqlCommand1 = @"
UPDATE mp_ForumSubscriptions 
SET UnSubscribeDate = ?SubscribeDate, 
WHERE ForumID = ?ForumID 
AND UserID = ?UserID 
AND UnSubscribeDate IS NULL ;";

			rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams1);

		}

		var arParams2 = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?SubscribeDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			}
		};

		string sqlCommand2 = @"
INSERT INTO	mp_ForumSubscriptions ( 
	ForumID, 
	UserID, 
	SubGuid, 
	SubscribeDate
) 
VALUES ( 
	?ForumID, 
	?UserID, 
	?SubGuid, 
	?SubscribeDate
) ;";

		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand2.ToString(),
			arParams2);


		return rowsAffected > -1;



	}

	public static bool DeleteSubscription(int subscriptionId)
	{
		string sqlCommand = @"
DELETE FROM mp_ForumSubscriptions 
WHERE SubscriptionID = ?SubscriptionID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SubscriptionID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = subscriptionId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}

	public static IDataReader GetForumSubscription(Guid subGuid)
	{

		string sqlCommand = @"
SELECT * 
FROM mp_ForumSubscriptions 
WHERE SubGuid = ?SubGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);


	}

	public static bool Unsubscribe(Guid subGuid)
	{
		string sqlCommand = @"
UPDATE mp_ForumSubscriptions 
SET UnSubscribeDate = ?UnSubscribeDate 
WHERE SubGuid = ?SubGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			},

			new("?UnSubscribeDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool Unsubscribe(int forumId, int userId)
	{
		string sqlCommand = @"
UPDATE mp_ForumSubscriptions 
SET UnSubscribeDate = ?UnSubscribeDate 
WHERE ForumID = ?ForumID AND UserID = ?UserID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?UnSubscribeDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool UnsubscribeAll(int userId)
	{
		string sqlCommand = @"
UPDATE mp_ForumSubscriptions 
SET UnSubscribeDate = ?UnSubscribeDate 
WHERE UserID = ?UserID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?UnSubscribeDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumSubscriptionExists(int forumId, int userId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ForumSubscriptions 
WHERE ForumID = ?ForumID 
AND UserID = ?UserID 
AND UnSubscribeDate IS NULL ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return (count > 0);

	}

	public static bool ForumThreadSubscriptionExists(int threadId, int userId)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_ForumThreadSubscriptions 
WHERE ThreadID = ?ThreadID 
AND UserID = ?UserID 
AND UnSubscribeDate IS NULL ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return (count > 0);

	}




	public static IDataReader ForumThreadGetThread(int threadId)
	{

		string sqlCommand = @"
SELECT 
	t.*, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	COALESCE(s.Name, 'Guest') As StartedBy, 
	f.PostsPerPage, 
	f.ModuleID 
FROM mp_ForumThreads t 
LEFT OUTER JOIN 
	mp_Users u 
ON t.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON t.StartedByUserID = s.UserID 
JOIN 
	mp_Forums f 
ON f.ItemID = t.ForumID 
WHERE t.ThreadID = ?ThreadID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);


	}

	public static IDataReader ForumThreadGetPost(int postId)
	{

		string sqlCommand = @"
SELECT fp.* 
FROM mp_ForumPosts fp 
WHERE fp.PostID = ?PostID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PostID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static int ForumThreadCreate(
		int forumId,
		string threadSubject,
		int sortOrder,
		bool isLocked,
		int startedByUserId,
		DateTime threadDate,
		Guid threadGuid,
		bool isQuestion,
		bool includeInSiteMap,
		bool setNoIndexMeta,
		string pageTitleOverride,
		int modStatus,
		string threadType)
	{

		byte locked = 1;
		if (!isLocked) { locked = 0; }

		byte isQ = 1;
		if (!isQuestion) { isQ = 0; }

		byte inMap = 1;
		if (!includeInSiteMap) { inMap = 0; }

		byte noIndex = 1;
		if (!setNoIndexMeta) { noIndex = 0; }

		int forumSequence = 1;

		string sqlCommand = @"
SELECT COALESCE(
	Max(ForumSequence) + 1,1
) 
As ForumSequence 
FROM mp_ForumThreads 
WHERE ForumID = ?ForumID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			}
		};

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				forumSequence = Convert.ToInt32(reader["ForumSequence"]);
			}
		}


		string sqlCommand1 = @"
INSERT INTO mp_ForumThreads ( 
	ThreadGuid, 
	ForumID, 
	ThreadSubject, 
	SortOrder, 
	ForumSequence, 
	IsLocked, 
	StartedByUserID, 
	ThreadDate, 
	IsQuestion, 
	IncludeInSiteMap, 
	SetNoIndexMeta, 
	PTitleOverride, 
	ModStatus, 
	ThreadType, 
	MostRecentPostUserID, 
	MostRecentPostDate 
) 
VALUES (
	?ThreadGuid, 
	?ForumID , 
	?ThreadSubject  , 
	?SortOrder, 
	?ForumSequence, 
	?IsLocked , 
	?StartedByUserID , 
	?ThreadDate , 
	?IsQuestion, 
	?IncludeInSiteMap, 
	?SetNoIndexMeta, 
	?PTitleOverride, 
	?ModStatus, 
	?ThreadType, 
	?StartedByUserID , 
	?ThreadDate  
);
SELECT LAST_INSERT_ID();";

		var arParams1 = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?ThreadSubject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = threadSubject
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?IsLocked", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = locked
			},

			new("?StartedByUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = startedByUserId
			},

			new("?ThreadDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = threadDate
			},

			new("?ForumSequence", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumSequence
			},

			new("?ThreadGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = threadGuid.ToString()
			},

			new("?IsQuestion", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = isQ
			},

			new("?IncludeInSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inMap
			},

			new("?SetNoIndexMeta", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = noIndex
			},

			new("?PTitleOverride", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitleOverride
			},

			new("?ModStatus", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = modStatus
			},

			new("?ThreadType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = threadType
			}
		};

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams1).ToString()
		);

		return newID;
	}


	public static bool ForumThreadDelete(int threadId)
	{
		ForumThreadDeletePosts(threadId);
		ForumThreadDeleteSubscriptions(threadId);

		var sqlCommand = $@"
UPDATE mp_Forums 
SET MostRecentPostDate = (
	SELECT MAX(MostRecentPostDate) 
	FROM mp_ForumThreads 
	WHERE ForumID = (
		SELECT ForumID 
		FROM mp_ForumThreads 
		WHERE ThreadID = ?ThreadID
	)
);
DELETE FROM mp_ForumThreads WHERE ThreadID = ?ThreadID;";

		var sqlParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		var rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams
		);

		return rowsAffected > -1;
	}


	public static bool ForumThreadDeletePosts(int threadId)
	{
		var sqlCommand = "DELETE FROM mp_ForumPosts WHERE ThreadID = ?ThreadID ;";

		var sqlParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		var rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams
		);

		return rowsAffected > -1;

	}


	public static bool ForumThreadDeleteSubscriptions(int threadId)
	{
		var sqlCommand = "DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID = ?ThreadID;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams
		);

		return rowsAffected > -1;
	}


	public static bool ForumThreadUpdate(
		int threadId,
		int forumId,
		string threadSubject,
		int sortOrder,
		bool isLocked,
		bool isQuestion,
		bool includeInSiteMap,
		bool setNoIndexMeta,
		string pageTitleOverride,
		int modStatus,
		string threadType,
		Guid assignedTo,
		Guid lockedBy,
		string lockedReason,
		DateTime lockedUtc
	)
	{
		byte locked = 1;
		if (!isLocked) { locked = 0; }

		byte isQ = 1;
		if (!isQuestion) { isQ = 0; }

		byte inMap = 1;
		if (!includeInSiteMap) { inMap = 0; }

		byte noIndex = 1;
		if (!setNoIndexMeta) { noIndex = 0; }

		string sqlCommand = @"
UPDATE mp_ForumThreads 
SET 
	ForumID = ?ForumID, 
	ThreadSubject = ?ThreadSubject, 
	SortOrder = ?SortOrder, 
	IsQuestion = ?IsQuestion, 
	IncludeInSiteMap = ?IncludeInSiteMap, 
	SetNoIndexMeta = ?SetNoIndexMeta, 
	PTitleOverride = ?PTitleOverride, 
	ModStatus = ?ModStatus, 
	ThreadType = ?ThreadType, 
	AssignedTo = ?AssignedTo, 
	LockedBy = ?LockedBy, 
	LockedReason = ?LockedReason, 
	LockedUtc = ?LockedUtc, 
	IsLocked = ?IsLocked 
WHERE ThreadID = ?ThreadID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?ThreadSubject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = threadSubject
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?IsLocked", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = locked
			},

			new("?IsQuestion", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = isQ
			},

			new("?IncludeInSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inMap
			},

			new("?SetNoIndexMeta", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = noIndex
			},

			new("?PTitleOverride", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitleOverride
			},

			new("?ModStatus", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = modStatus
			},

			new("?ThreadType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = threadType
			},

			new("?AssignedTo", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = assignedTo.ToString()
			},

			new("?LockedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lockedBy.ToString()
			},

			new("?LockedReason", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = lockedReason
			},

			new("?LockedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			}
		};


		if (lockedUtc < DateTime.MaxValue)
		{
			arParams.Add(new("?LockedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lockedUtc
			});
		}
		else
		{
			arParams.Add(new("?LockedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumThreadIncrementReplyStats(
		int threadId,
		int mostRecentPostUserId,
		DateTime mostRecentPostDate)
	{
		string sqlCommand = @"
UPDATE mp_ForumThreads 
SET 
	MostRecentPostUserID = ?MostRecentPostUserID, 
	TotalReplies = TotalReplies + 1, 
	MostRecentPostDate = ?MostRecentPostDate 
WHERE ThreadID = ?ThreadID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?MostRecentPostUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostUserId
			},

			new("?MostRecentPostDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = mostRecentPostDate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumThreadDecrementReplyStats(int threadId)
	{
		string sqlCommand = @"
SELECT UserID, PostDate 
FROM mp_ForumPosts 
WHERE ThreadID = ?ThreadID 
ORDER BY PostID DESC ";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		int userId = 0;
		DateTime postDate = DateTime.Now;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				userId = Convert.ToInt32(reader["UserID"]);
				postDate = Convert.ToDateTime(reader["PostDate"]);
			}
		}

		string sqlCommand1 = @"
UPDATE mp_ForumThreads 
SET 
	MostRecentPostUserID = ?MostRecentPostUserID, 
	TotalReplies = TotalReplies - 1, 
	MostRecentPostDate = ?MostRecentPostDate 
WHERE ThreadID = ?ThreadID ;";

		var arParams1 = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?MostRecentPostUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?MostRecentPostDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = postDate
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams1);

		return rowsAffected > -1;

	}

	public static bool ForumThreadUpdateViewStats(int threadId)
	{
		string sqlCommand = @"
UPDATE mp_ForumThreads 
SET TotalViews = TotalViews + 1 
WHERE ThreadID = ?ThreadID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
	{
		int postsPerPage = 10;

		string sqlCommand = @"
SELECT f.PostsPerPage 
FROM mp_ForumThreads ft 
JOIN mp_Forums f 
ON ft.ForumID = f.ItemID 
WHERE ft.ThreadID = ?ThreadID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};



		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
			}
		}

		int currentPageMaxThreadSequence = postsPerPage * pageNumber;
		int beginSequence = 1;
		int endSequence;
		if (currentPageMaxThreadSequence > postsPerPage)
		{
			beginSequence = currentPageMaxThreadSequence - postsPerPage + 1;

		}

		endSequence = beginSequence + postsPerPage;

		string sqlCommand1 = @"
SELECT 
	p.*, 
	ft.ForumID, 
	ft.IsLocked, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	COALESCE(s.Name, 'Guest') As StartedBy, 
	COALESCE(up.Name, 'Guest') As PostAuthor, 
	COALESCE(up.Email, '') As AuthorEmail, 
	COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, 
	COALESCE(up.TotalRevenue, 0) As UserRevenue, 
	COALESCE(up.Trusted, 0) As Trusted, 
	COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, 
	up.WebSiteURL As PostAuthorWebSiteUrl, 
	up.Signature As PostAuthorSignature 
FROM mp_ForumPosts p 
JOIN 
	mp_ForumThreads ft 
ON p.ThreadID = ft.ThreadID 
LEFT OUTER JOIN 
	mp_Users u 
ON ft.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON ft.StartedByUserID = s.UserID 
LEFT OUTER JOIN 
	mp_Users up 
ON up.UserID = p.UserID 
WHERE ft.ThreadID = ?ThreadID 
AND p.ThreadSequence >= ?BeginSequence 
AND p.ThreadSequence <= ?EndSequence 
ORDER BY p.SortOrder, p.ThreadSequence ;";

		// TODO:
		//using 'Guest' here is not culture neutral, need to pass in a label

		var arParams1 = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?PageNumber", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageNumber
			},

			new("?BeginSequence", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = beginSequence
			},

			new("?EndSequence", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = endSequence
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand1.ToString(),
			arParams1);

	}

	public static IDataReader ForumThreadGetPosts(int threadId)
	{

		string sqlCommand = @"
SELECT 
	p.*, 
	ft.ForumID, 
	ft.IsLocked, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	COALESCE(s.Name, 'Guest') As StartedBy, 
	COALESCE(up.Name, 'Guest') As PostAuthor, 
	COALESCE(up.Email, '') As AuthorEmail, 
	COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, 
	COALESCE(up.TotalRevenue, 0) As UserRevenue, 
	COALESCE(up.Trusted, 0) As Trusted, 
	COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, 
	up.WebSiteURL As PostAuthorWebSiteUrl, 
	up.Signature As PostAuthorSignature 
FROM mp_ForumPosts p 
JOIN mp_ForumThreads ft 
ON p.ThreadID = ft.ThreadID 
LEFT OUTER JOIN 
	mp_Users u 
ON ft.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON ft.StartedByUserID = s.UserID 
LEFT OUTER JOIN	
	mp_Users up 
ON up.UserID = p.UserID 
WHERE ft.ThreadID = ?ThreadID 
ORDER BY p.PostID ;";

		// TODO:
		//using 'Guest' here is not culture neutral, need to pass in a label

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
	{

		string sqlCommand = @"
SELECT 
	p.*, 
	ft.ForumID, 
	ft.IsLocked, 
	COALESCE(u.Name, 'Guest') As MostRecentPostUser, 
	COALESCE(s.Name, 'Guest') As StartedBy, 
	COALESCE(up.Name, 'Guest') As PostAuthor, 
	COALESCE(up.Email, '') As AuthorEmail, 
	COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, 
	COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, 
	COALESCE(up.TotalRevenue, 0) As UserRevenue, 
	COALESCE(up.Trusted, 0) As Trusted, 
	up.WebSiteURL As PostAuthorWebSiteUrl, 
	up.Signature As PostAuthorSignature 
FROM mp_ForumPosts p 
JOIN mp_ForumThreads ft 
ON p.ThreadID = ft.ThreadID 
LEFT OUTER JOIN 
	mp_Users u 
ON ft.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON ft.StartedByUserID = s.UserID 
LEFT OUTER JOIN 
	mp_Users up 
ON up.UserID = p.UserID 
WHERE ft.ThreadID = ?ThreadID 
ORDER BY p.ThreadSequence DESC ;";

		// TODO:
		//using 'Guest' here is not culture neutral, need to pass in a label

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT 
	fp.*, 
	ft.MostRecentPostDate, 
	f.ModuleID, 
	f.ItemID, 
	m.ModuleTitle, 
	m.ViewRoles, 
	md.FeatureName 
FROM mp_ForumPosts fp 
JOIN
	mp_ForumThreads ft 
ON fp.ThreadID = ft.ThreadID 
JOIN 
	mp_Forums f 
ON f.ItemID = ft.ForumID 
JOIN 
	mp_Modules m 
ON f.ModuleID = m.ModuleID 
JOIN 
	mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
JOIN 
	mp_PageModules pm 
ON m.ModuleID = pm.ModuleID 
JOIN 
	mp_Pages p 
ON p.PageID = pm.PageID 
WHERE 
p.SiteID = ?SiteID 
AND pm.PageID = ?PageID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
	{
		string sqlCommand = @"
SELECT 
	ft.*, 
	f.ModuleID, 
	f.ItemID, 
	m.ModuleTitle, 
	m.ViewRoles, 
	md.FeatureName 
FROM mp_ForumThreads ft 
JOIN 
	mp_Forums f 
ON f.ItemID = ft.ForumID 
JOIN 
	mp_Modules m 
ON f.ModuleID = m.ModuleID 
JOIN 
	mp_ModuleDefinitions md 
ON m.ModuleDefID = md.ModuleDefID 
JOIN 
	mp_PageModules pm 
ON m.ModuleID = pm.ModuleID 
JOIN 
	mp_Pages p 
ON p.PageID = pm.PageID 
WHERE 
p.SiteID = ?SiteID 
AND pm.PageID = ?PageID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
	{
		string sqlCommand = @"
SELECT 
	fp.*, 
	ft.ThreadSubject, 
	ft.ForumID, 
	p.PageID, 
	p.AuthorizedRoles, 
	m.ModuleID, 
	m.ViewRoles, 
	COALESCE(s.Name,'Guest') as StartedBy, 
	COALESCE(up.Name, 'Guest') as PostAuthor, 
	up.TotalPosts as PostAuthorTotalPosts, 
	up.AvatarUrl as PostAuthorAvatar, 
	up.WebSiteURL as PostAuthorWebSiteUrl, 
	up.Signature as PostAuthorSignature 
FROM mp_ForumPosts fp 
JOIN 
	mp_ForumThreads ft 
ON fp.ThreadID = ft.ThreadID 
JOIN 
	mp_Forums f 
ON ft.ForumID = f.ItemID 
JOIN 
	mp_Modules m 
ON f.ModuleID = m.ModuleID 
JOIN
	mp_PageModules pm 
ON pm.ModuleID = m.ModuleID 
JOIN 
	mp_Pages p 
ON pm.PageID = p.PageID 
LEFT OUTER JOIN 
	mp_Users u 
ON ft.MostRecentPostUserID = u.UserID 
LEFT OUTER JOIN 
	mp_Users s 
ON ft.StartedByUserID = s.UserID 
LEFT OUTER JOIN 
	mp_Users up 
ON up.UserID = fp.UserID 
WHERE p.SiteID = ?SiteID 
AND	(?PageID = -1 OR p.PageID = ?PageID) 
AND	(?ModuleID = -1 OR m.ModuleID = ?ModuleID) 
AND	(?ItemID = -1 OR f.ItemID = ?ItemID) 
AND	(?ThreadID = -1 OR ft.ThreadID = ?ThreadID) 
AND	( (?MaximumDays = -1) 
OR  ((now() - ?MaximumDays) >= fp.PostDate )) 
ORDER BY fp.PostDate DESC ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?PageID", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?ModuleID", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			},

			new("?ItemID", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			},

			new("?ThreadID", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?MaximumDays", SqlDbType.Int)
			{
				Direction = ParameterDirection.Input,
				Value = maximumDays
		}
	};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
	{
		string sqlCommand = @"
SELECT u.Email, 
COALESCE(fts.ThreadSubscriptionID, -1) AS ThreadSubID, 
COALESCE(fs.SubscriptionID, -1) AS ForumSubID, 
COALESCE(fts.SubGuid, '00000000-0000-0000-0000-000000000000') AS ThreadSubGuid, 
COALESCE(fs.SubGuid, '00000000-0000-0000-0000-000000000000') AS ForumSubGuid 
FROM	mp_Users u 
LEFT OUTER JOIN mp_ForumThreadSubscriptions fts 
ON 
fts.UserID = u.UserID 
AND fts.ThreadID = ?ThreadID 
AND fts.UnSubscribeDate IS NULL 
LEFT OUTER JOIN mp_ForumSubscriptions fs 
ON 
fs.UserID = u.UserID 
AND fs.ForumID = ?ForumID 
AND fs.UnSubscribeDate IS NULL 
WHERE ";

		if (!includeCurrentUser)
		{
			sqlCommand += " u.UserID <> ?CurrentPostUserID AND ";
		}

		sqlCommand += @"
(
(fts.ThreadSubscriptionID IS NOT NULL )
OR 
(fs.SubscriptionID IS NOT NULL )
);";

		var arParams = new List<MySqlParameter>
		{
			new("?ForumID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = forumId
			},

			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?CurrentPostUserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = currentPostUserId
			}
		};

		return CommandHelper.ExecuteDataset(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


	public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
	{

		string sqlCommand = @"
SELECT * 
FROM mp_ForumThreadSubscriptions 
WHERE SubGuid = ?SubGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);


	}

	public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
	{
		string sqlCommand = @"
SELECT COUNT(*) As SubscriptionCount 
FROM mp_ForumThreadSubscriptions 
WHERE ThreadID = ?ThreadID 
AND UserID = ?UserID 
AND UnSubscribeDate IS NULL ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int subscriptionCount = 0;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
			}
		}


		int rowsAffected = -1;

		var arParams1 = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};


		if (subscriptionCount > 0)
		{
			string sqlCommand1 = @"
UPDATE mp_ForumThreadSubscriptions 
SET UnSubscribeDate = ?CurrentTime 
WHERE ThreadID = ?ThreadID 
AND UserID = ?UserID 
AND UnSubscribeDate IS NULL ;";

			rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams1);

		}


		string sqlCommand2 = @"
INSERT INTO	mp_ForumThreadSubscriptions ( 
	ThreadID, 
	SubGuid, 
	SubscribeDate, 
	UserID 
) 
VALUES ( 
	?ThreadID, 
	?SubGuid, 
	?CurrentTime, 
	?UserID 
) ;";

		var arParams2 = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};


		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand2.ToString(),
			arParams2);

		return rowsAffected > -1;

	}

	public static bool ForumThreadUnSubscribe(Guid subGuid)
	{
		string sqlCommand = @"
UPDATE mp_ForumThreadSubscriptions 
SET UnSubscribeDate = ?CurrentTime 
WHERE SubGuid = ?SubGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SubGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = subGuid.ToString()
			},

			new("?CurrentTime", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumThreadUNSubscribe(int threadId, int userId)
	{
		string sqlCommand = @"
UPDATE mp_ForumThreadSubscriptions 
SET UnSubscribeDate = now() 
WHERE ThreadID = ?ThreadID 
AND UserID = ?UserID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumThreadUnsubscribeAll(int userId)
	{
		string sqlCommand = @"
UPDATE mp_ForumThreadSubscriptions 
SET UnSubscribeDate = now() 
WHERE UserID = ?UserID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static int ForumPostCreate(
		int threadId,
		string subject,
		string post,
		bool approved,
		int userId,
		DateTime postDate,
		Guid postGuid,
		Guid approvedBy,
		DateTime approvedUtc,
		string userIp,
		bool notificationSent,
		int modStatus)
	{

		byte approve = 1;
		if (!approved)
		{
			approve = 0;
		}

		int intNotificationSent = notificationSent ? 1 : 0;

		string sqlCommand = @"SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM mp_ForumPosts WHERE ThreadID = ?ThreadID ; ";

		var arParams = new List<MySqlParameter>
		{
			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		int threadSequence = 1;

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{
			if (reader.Read())
			{
				threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
			}
		}

		string sqlCommand1 = @"
INSERT INTO mp_ForumPosts ( 
	ThreadID, 
	Subject, 
	Post, 
	PostDate, 
	Approved, 
	UserID, 
	PostGuid, 
	AnswerVotes, 
	ApprovedBy, 
	ApprovedUtc, 
	UserIp, 
	NotificationSent, 
	ModStatus, 
	ThreadSequence 
) 
VALUES (
	?ThreadID , 
	?Subject  , 
	?Post, 
	?PostDate, 
	?Approved , 
	?UserID , 
	?PostGuid, 
	0, 
	?ApprovedBy, 
	?ApprovedUtc, 
	?UserIp, 
	?NotificationSent, 
	?ModStatus, 
	?ThreadSequence  
);
SELECT LAST_INSERT_ID();";

		var arParams1 = new List<MySqlParameter>
		{
			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?Post", MySqlDbType.MediumText)
			{
				Direction = ParameterDirection.Input,
				Value = post
			},

			new("?Approved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = approve
			},

			new("?UserID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = userId
			},

			new("?PostDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = postDate
			},

			new("?ThreadSequence", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadSequence
			},

			new("?PostGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = postGuid.ToString()
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			},

			new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			},

			new("?UserIp", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = userIp
			},

			new("?NotificationSent", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intNotificationSent
			},

			new("?ModStatus", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = modStatus
			},

			new("?ThreadID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadId
			}
		};

		if (approvedUtc > DateTime.MinValue)
		{
			arParams.Add(new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = approvedUtc
			});
		}
		else
		{
			arParams.Add(new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand1.ToString(),
			arParams1).ToString());

		return newID;


	}

	public static bool ForumPostUpdate(
		int postId,
		string subject,
		string post,
		int sortOrder,
		bool approved,
		Guid approvedBy,
		DateTime approvedUtc,
		bool notificationSent,
		int modStatus)
	{
		byte approve = 1;
		if (!approved)
		{
			approve = 0;
		}

		int intNotificationSent = notificationSent ? 1 : 0;

		string sqlCommand = @"
UPDATE mp_ForumPosts 
SET Subject = ?Subject, 
Post = ?Post, 
SortOrder = ?SortOrder, 
NotificationSent = ?NotificationSent, 
ModStatus = ?ModStatus, 
ApprovedBy = ?ApprovedBy, 
ApprovedUtc = ?ApprovedUtc, 
Approved = ?Approved 
WHERE PostID = ?PostID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PostID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postId
			},

			new("?Subject", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = subject
			},

			new("?Post", MySqlDbType.MediumText)
			{
				Direction = ParameterDirection.Input,
				Value = post
			},

			new("?SortOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = sortOrder
			},

			new("?Approved", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = approve
			},

			new("?ApprovedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = approvedBy.ToString()
			},

			new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,

			},
			new("?NotificationSent", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intNotificationSent
			},

			new("?ModStatus", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = modStatus
			}
		};


		if (approvedUtc > DateTime.MinValue)
		{
			arParams.Add(new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = approvedUtc
			});
		}
		else
		{
			arParams.Add(new("?ApprovedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumPostUpdateThreadSequence(
		int postId,
		int threadSequence)
	{
		string sqlCommand = @"
UPDATE mp_ForumPosts 
SET ThreadSequence = ?ThreadSequence 
WHERE PostID = ?PostID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PostID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postId
			},

			new("?ThreadSequence", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = threadSequence
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	public static bool ForumPostDelete(int postId)
	{

		string sqlCommand = @"
DELETE FROM mp_ForumPosts 
WHERE PostID = ?PostID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?PostID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = postId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}






}
