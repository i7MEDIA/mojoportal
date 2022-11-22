using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Text;


namespace mojoPortal.Data
{
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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO	mp_Forums ( ");
			sqlCommand.Append("ModuleID, ");
			sqlCommand.Append("CreatedBy, ");
			sqlCommand.Append("CreatedDate, ");
			sqlCommand.Append("Title, ");
			sqlCommand.Append("Description , ");
			sqlCommand.Append("IsModerated , ");
			sqlCommand.Append("IsActive , ");
			sqlCommand.Append("SortOrder , ");
			sqlCommand.Append("PostsPerPage , ");
			sqlCommand.Append("ThreadsPerPage , ");
			sqlCommand.Append("ForumGuid , ");

			sqlCommand.Append("RolesThatCanPost, ");
			sqlCommand.Append("RolesThatCanModerate, ");
			sqlCommand.Append("ModeratorNotifyEmail, ");
			sqlCommand.Append("IncludeInGoogleMap, ");
			sqlCommand.Append("AddNoIndexMeta, ");
			sqlCommand.Append("Closed, ");
			sqlCommand.Append("Visible, ");
			sqlCommand.Append("RequireModeration, ");
			sqlCommand.Append("RequireModForNotify, ");
			sqlCommand.Append("AllowTrustedDirectPosts, ");
			sqlCommand.Append("AllowTrustedDirectNotify, ");

			sqlCommand.Append("AllowAnonymousPosts  ");
			sqlCommand.Append(" ) ");

			sqlCommand.Append("VALUES (");

			sqlCommand.Append(" ?ModuleID , ");
			sqlCommand.Append(" ?UserID  , ");
			sqlCommand.Append(" now(), ");
			sqlCommand.Append(" ?Title , ");
			sqlCommand.Append(" ?Description , ");
			sqlCommand.Append(" ?IsModerated , ");
			sqlCommand.Append(" ?IsActive , ");
			sqlCommand.Append(" ?SortOrder , ");
			sqlCommand.Append(" ?PostsPerPage , ");
			sqlCommand.Append(" ?ThreadsPerPage , ");
			sqlCommand.Append(" ?ForumGuid , ");

			sqlCommand.Append("?RolesThatCanPost, ");
			sqlCommand.Append("?RolesThatCanModerate, ");
			sqlCommand.Append("?ModeratorNotifyEmail, ");
			sqlCommand.Append("?IncludeInGoogleMap, ");
			sqlCommand.Append("?AddNoIndexMeta, ");
			sqlCommand.Append("?Closed, ");
			sqlCommand.Append("?Visible, ");
			sqlCommand.Append("?RequireModeration, ");
			sqlCommand.Append("?RequireModForNotify, ");
			sqlCommand.Append("?AllowTrustedDirectPosts, ");
			sqlCommand.Append("?AllowTrustedDirectNotify, ");

			sqlCommand.Append(" ?AllowAnonymousPosts  ");

			sqlCommand.Append(");");
			sqlCommand.Append("SELECT LAST_INSERT_ID();");

			MySqlParameter[] arParams = new MySqlParameter[22];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 100);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = title;

			arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = description;

			arParams[4] = new MySqlParameter("?IsModerated", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = moderated;

			arParams[5] = new MySqlParameter("?IsActive", MySqlDbType.Int32);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = active;

			arParams[6] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = sortOrder;

			arParams[7] = new MySqlParameter("?PostsPerPage", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = postsPerPage;

			arParams[8] = new MySqlParameter("?ThreadsPerPage", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = threadsPerPage;

			arParams[9] = new MySqlParameter("?AllowAnonymousPosts", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = allowAnonymous;

			arParams[10] = new MySqlParameter("?ForumGuid", MySqlDbType.VarChar, 36);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = forumGuid.ToString();

			arParams[11] = new MySqlParameter("?RolesThatCanPost", MySqlDbType.Text);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = rolesThatCanPost;

			arParams[12] = new MySqlParameter("?RolesThatCanModerate", MySqlDbType.Text);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = rolesThatCanModerate;

			arParams[13] = new MySqlParameter("?ModeratorNotifyEmail", MySqlDbType.Text);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = moderatorNotifyEmail;

			arParams[14] = new MySqlParameter("?IncludeInGoogleMap", MySqlDbType.Int32);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = intIncludeInGoogleMap;

			arParams[15] = new MySqlParameter("?AddNoIndexMeta", MySqlDbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = intAddNoIndexMeta;

			arParams[16] = new MySqlParameter("?Closed", MySqlDbType.Int32);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = intClosed;

			arParams[17] = new MySqlParameter("?Visible", MySqlDbType.Int32);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = intVisible;

			arParams[18] = new MySqlParameter("?RequireModeration", MySqlDbType.Int32);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = intRequireModeration;

			arParams[19] = new MySqlParameter("?RequireModForNotify", MySqlDbType.Int32);
			arParams[19].Direction = ParameterDirection.Input;
			arParams[19].Value = intRequireModForNotify;

			arParams[20] = new MySqlParameter("?AllowTrustedDirectPosts", MySqlDbType.Int32);
			arParams[20].Direction = ParameterDirection.Input;
			arParams[20].Value = intAllowTrustedDirectPosts;

			arParams[21] = new MySqlParameter("?AllowTrustedDirectNotify", MySqlDbType.Int32);
			arParams[21].Direction = ParameterDirection.Input;
			arParams[21].Value = intAllowTrustedDirectNotify;



			int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE		mp_Forums ");
			sqlCommand.Append("SET	Title = ?Title, ");
			sqlCommand.Append("Description = ?Description, ");
			sqlCommand.Append("IsModerated = ?IsModerated, ");
			sqlCommand.Append("IsActive = ?IsActive, ");
			sqlCommand.Append("SortOrder = ?SortOrder, ");
			sqlCommand.Append("PostsPerPage = ?PostsPerPage, ");
			sqlCommand.Append("ThreadsPerPage = ?ThreadsPerPage, ");

			sqlCommand.Append("RolesThatCanPost = ?RolesThatCanPost, ");
			sqlCommand.Append("RolesThatCanModerate = ?RolesThatCanModerate, ");
			sqlCommand.Append("ModeratorNotifyEmail = ?ModeratorNotifyEmail, ");
			sqlCommand.Append("IncludeInGoogleMap = ?IncludeInGoogleMap, ");
			sqlCommand.Append("AddNoIndexMeta = ?AddNoIndexMeta, ");
			sqlCommand.Append("Closed = ?Closed, ");
			sqlCommand.Append("Visible = ?Visible, ");
			sqlCommand.Append("RequireModeration = ?RequireModeration, ");
			sqlCommand.Append("RequireModForNotify = ?RequireModForNotify, ");
			sqlCommand.Append("AllowTrustedDirectPosts = ?AllowTrustedDirectPosts, ");
			sqlCommand.Append("AllowTrustedDirectNotify = ?AllowTrustedDirectNotify, ");

			sqlCommand.Append("AllowAnonymousPosts = ?AllowAnonymousPosts ");

			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[20];


			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 100);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = title;

			arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = description;

			arParams[3] = new MySqlParameter("?IsModerated", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = moderated;

			arParams[4] = new MySqlParameter("?IsActive", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = active;

			arParams[5] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = sortOrder;

			arParams[6] = new MySqlParameter("?PostsPerPage", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = postsPerPage;

			arParams[7] = new MySqlParameter("?ThreadsPerPage", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = threadsPerPage;

			arParams[8] = new MySqlParameter("?AllowAnonymousPosts", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = allowAnonymous;

			arParams[9] = new MySqlParameter("?RolesThatCanPost", MySqlDbType.Text);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = rolesThatCanPost;

			arParams[10] = new MySqlParameter("?RolesThatCanModerate", MySqlDbType.Text);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = rolesThatCanModerate;

			arParams[11] = new MySqlParameter("?ModeratorNotifyEmail", MySqlDbType.Text);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = moderatorNotifyEmail;

			arParams[12] = new MySqlParameter("?IncludeInGoogleMap", MySqlDbType.Int32);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = intIncludeInGoogleMap;

			arParams[13] = new MySqlParameter("?AddNoIndexMeta", MySqlDbType.Int32);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = intAddNoIndexMeta;

			arParams[14] = new MySqlParameter("?Closed", MySqlDbType.Int32);
			arParams[14].Direction = ParameterDirection.Input;
			arParams[14].Value = intClosed;

			arParams[15] = new MySqlParameter("?Visible", MySqlDbType.Int32);
			arParams[15].Direction = ParameterDirection.Input;
			arParams[15].Value = intVisible;

			arParams[16] = new MySqlParameter("?RequireModeration", MySqlDbType.Int32);
			arParams[16].Direction = ParameterDirection.Input;
			arParams[16].Value = intRequireModeration;

			arParams[17] = new MySqlParameter("?RequireModForNotify", MySqlDbType.Int32);
			arParams[17].Direction = ParameterDirection.Input;
			arParams[17].Value = intRequireModForNotify;

			arParams[18] = new MySqlParameter("?AllowTrustedDirectPosts", MySqlDbType.Int32);
			arParams[18].Direction = ParameterDirection.Input;
			arParams[18].Value = intAllowTrustedDirectPosts;

			arParams[19] = new MySqlParameter("?AllowTrustedDirectNotify", MySqlDbType.Int32);
			arParams[19].Direction = ParameterDirection.Input;
			arParams[19].Value = intAllowTrustedDirectNotify;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool Delete(int itemId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID = ?ItemID );");
			sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID = ?ItemID );");
			sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID = ?ItemID;");
			sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID = ?ItemID ;");

			sqlCommand.Append("DELETE FROM mp_Forums ");
			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteByModule(int moduleId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = ?ModuleID) );");
			sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = ?ModuleID) );");
			sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = ?ModuleID);");
			sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID = ?ModuleID) ;");
			sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID = ?ModuleID;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static bool DeleteBySite(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ForumPosts WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID)) );");
			sqlCommand.Append("DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID)) );");
			sqlCommand.Append("DELETE FROM mp_ForumThreads WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID));");
			sqlCommand.Append("DELETE FROM mp_ForumSubscriptions WHERE ForumID IN (SELECT ItemID FROM mp_Forums WHERE ModuleID IN  (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID)) ;");
			sqlCommand.Append("DELETE FROM mp_Forums WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}



		public static IDataReader GetForums(int moduleId, int userId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT f.*, ");
			sqlCommand.Append("u.Name As MostRecentPostUser, ");
			sqlCommand.Append("s.SubscribeDate IS NOT NULL AND s.UnSubscribeDate IS NULL As Subscribed, ");
			sqlCommand.Append("(SELECT COUNT(*) FROM mp_ForumSubscriptions fs WHERE fs.ForumID = f.ItemID AND fs.UnSubscribeDate IS NULL) As SubscriberCount  ");

			sqlCommand.Append("FROM	mp_Forums f ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON f.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_ForumSubscriptions s ");
			sqlCommand.Append("ON f.ItemID = s.ForumID AND s.UserID = ?UserID AND s.UnSubscribeDate IS NULL ");

			sqlCommand.Append("WHERE f.ModuleID	= ?ModuleID ");
			sqlCommand.Append("AND f.IsActive = 1 ");

			sqlCommand.Append("ORDER BY		f.SortOrder, f.ItemID ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = moduleId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader GetForum(int itemId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT f.*, ");
			sqlCommand.Append("u.Name As CreatedByUser, ");
			sqlCommand.Append("up.Name As MostRecentPostUser ");
			sqlCommand.Append("FROM	mp_Forums f ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON f.CreatedBy = u.UserID ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
			sqlCommand.Append("ON f.MostRecentPostUserID = up.UserID ");
			sqlCommand.Append("WHERE f.ItemID	= ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = itemId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static bool IncrementPostCount(
			int forumId,
			int mostRecentPostUserId,
			DateTime mostRecentPostDate)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Forums ");
			sqlCommand.Append("SET MostRecentPostDate = ?MostRecentPostDate, ");
			sqlCommand.Append("MostRecentPostUserID = ?MostRecentPostUserID, ");
			sqlCommand.Append("PostCount = PostCount + 1 ");

			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = mostRecentPostUserId;

			arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = mostRecentPostDate;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), arParams);

			return (rowsAffected > -1);

		}

		public static bool UpdateUserStats(int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Users ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("TotalPosts = (SELECT COUNT(*) FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = mp_Users.UserID) ");
			if (userId > -1)
			{
				sqlCommand.Append("WHERE UserID = ?UserID ");
			}
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = userId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool IncrementPostCount(int forumId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Forums ");
			sqlCommand.Append("SET  ");
			sqlCommand.Append("PostCount = PostCount + 1 ");
			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool DecrementPostCount(int forumId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Forums ");
			sqlCommand.Append("SET PostCount = PostCount - 1 ");

			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);


		}

		public static bool RecalculatePostStats(int forumId)
		{
			DateTime mostRecentPostDate = DateTime.UtcNow;
			int mostRecentPostUserID = -1;
			int postCount = 0;


			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT ");
			sqlCommand.Append("MostRecentPostDate, ");
			sqlCommand.Append("MostRecentPostUserID ");
			sqlCommand.Append("FROM mp_ForumThreads ");
			sqlCommand.Append("WHERE ForumID = ?ForumID ");
			sqlCommand.Append("ORDER BY MostRecentPostDate DESC ");
			sqlCommand.Append("LIMIT 1 ;");

			using (IDataReader reader = MySqlHelper.ExecuteReader(
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

			sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT ");
			sqlCommand.Append("COALESCE(SUM(TotalReplies) + COUNT(*),0) As PostCount ");
			sqlCommand.Append("FROM mp_ForumThreads ");
			sqlCommand.Append("WHERE ForumID = ?ForumID ;");

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{

				if (reader.Read())
				{
					postCount = Convert.ToInt32(reader["PostCount"]);
				}
			}

			sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE	mp_Forums ");
			sqlCommand.Append("SET	 ");
			sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate,	 ");
			sqlCommand.Append("MostRecentPostUserID = ?MostRecentPostUserID,	 ");
			sqlCommand.Append("PostCount = ?PostCount	 ");
			sqlCommand.Append("WHERE ItemID = ?ForumID ;");

			arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = mostRecentPostDate;

			arParams[2] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = mostRecentPostUserID;

			arParams[3] = new MySqlParameter("?PostCount", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = postCount;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}


		public static bool IncrementThreadCount(int forumId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE	mp_Forums ");
			sqlCommand.Append("SET	ThreadCount = ThreadCount + 1 ");
			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool DecrementThreadCount(int forumId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_Forums ");
			sqlCommand.Append("SET ThreadCount = ThreadCount - 1 ");

			sqlCommand.Append("WHERE ItemID = ?ItemID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}


		public static int GetUserThreadCount(int userId, int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  Count(*) ");

			sqlCommand.Append("FROM	mp_ForumThreads ft ");

			sqlCommand.Append("JOIN mp_Forums f ");
			sqlCommand.Append("ON ft.ForumID = f.ItemID ");

			sqlCommand.Append("JOIN mp_Modules m ");
			sqlCommand.Append("ON f.ModuleID = m.ModuleID ");


			sqlCommand.Append("WHERE m.SiteID = ?SiteID AND ft.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = ?UserID) ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = userId;

			arParams[1] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = siteId;

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	 ");
			sqlCommand.Append(" t.*, ");
			sqlCommand.Append("f.Title As Forum, ");
			sqlCommand.Append("f.ModuleID, ");
			sqlCommand.Append("(SELECT PageID FROM mp_PageModules WHERE mp_PageModules.ModuleID = f.ModuleID AND (PublishEndDate IS NULL OR PublishEndDate > ?CurrentDate) LIMIT 1) As PageID, ");
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("s.Name As StartedBy ");

			sqlCommand.Append("FROM	mp_ForumThreads t ");

			sqlCommand.Append("JOIN	mp_Forums f ");
			sqlCommand.Append("ON t.ForumID = f.ItemID ");

			sqlCommand.Append("JOIN mp_Modules m ");
			sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
			sqlCommand.Append("ON t.StartedByUserID = s.UserID ");

			sqlCommand.Append("WHERE m.SiteID = ?SiteID AND t.ThreadID IN (Select DISTINCT ThreadID FROM mp_ForumPosts WHERE mp_ForumPosts.UserID = ?UserID) ");

			sqlCommand.Append("ORDER BY	t.MostRecentPostDate DESC  ");

			sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize ");
			sqlCommand.Append(";");


			MySqlParameter[] arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = userId;

			arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageSize;

			arParams[2] = new MySqlParameter("?CurrentDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = DateTime.UtcNow;

			arParams[3] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = siteId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}



		public static IDataReader GetThreadsForSiteMap(int siteId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT ");
			sqlCommand.Append("ft.ThreadID, ");
			sqlCommand.Append("ft.MostRecentPostDate, ");
			sqlCommand.Append("f.ModuleID, ");
			sqlCommand.Append("m.ViewRoles, ");
			sqlCommand.Append("p.PageID, ");
			sqlCommand.Append("p.AuthorizedRoles ");


			sqlCommand.Append("FROM	mp_ForumThreads ft ");

			sqlCommand.Append("JOIN	mp_Forums f ");
			sqlCommand.Append("ON f.ItemID = ft.ForumID ");

			sqlCommand.Append("JOIN	mp_Modules m ");
			sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN	mp_PageModules pm ");
			sqlCommand.Append("ON pm.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN	mp_Pages p ");
			sqlCommand.Append("ON p.PageID = pm.PageID ");


			sqlCommand.Append("WHERE p.SiteID = ?SiteID ");
			sqlCommand.Append("AND ft.IncludeInSiteMap = 1 ");

			sqlCommand.Append("ORDER BY ft.ThreadID DESC ");

			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			return MySqlHelper.ExecuteReader(
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


			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	 ");
			sqlCommand.Append(" t.*, ");
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("s.Name As StartedBy ");
			sqlCommand.Append("FROM	mp_ForumThreads t ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
			sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
			sqlCommand.Append("WHERE t.ForumID = ?ForumID ");

			sqlCommand.Append("ORDER BY t.SortOrder, t.MostRecentPostDate DESC  ");

			if (pageNumber > 1)
			{
				sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
					+ ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}
			else
			{
				sqlCommand.Append("LIMIT "
					+ pageSize.ToString(CultureInfo.InvariantCulture) + " ");
			}

			sqlCommand.Append(" ; ");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static int ForumThreadGetPostCount(int threadId)
		{

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT COUNT(*) FROM mp_ForumPosts WHERE ThreadID = ?ThreadID; ");

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());

			return count;

		}

		public static int GetSubscriberCount(int forumId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  Count(*) ");
			sqlCommand.Append("FROM	mp_ForumSubscriptions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("ForumID = ?ForumID ");
			sqlCommand.Append("AND ");
			sqlCommand.Append("UnSubscribeDate IS NULL");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			return Convert.ToInt32(MySqlHelper.ExecuteScalar(
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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT ");
			sqlCommand.Append("fs.SubscriptionID, ");
			sqlCommand.Append("fs.SubscribeDate, ");
			sqlCommand.Append("u.Name, ");
			sqlCommand.Append("u.LoginName, ");
			sqlCommand.Append("u.Email ");

			sqlCommand.Append("FROM	mp_ForumSubscriptions fs  ");

			sqlCommand.Append("LEFT OUTER JOIN ");
			sqlCommand.Append("mp_Users u ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("u.UserID = fs.UserID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("fs.ForumID = ?ForumID ");
			sqlCommand.Append("AND ");
			sqlCommand.Append("fs.UnSubscribeDate IS NULL ");

			sqlCommand.Append("ORDER BY  ");
			sqlCommand.Append("u.Name  ");


			sqlCommand.Append("LIMIT ?PageSize ");

			if (pageNumber > 1)
			{
				sqlCommand.Append("OFFSET ?OffsetRows ");
			}

			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageSize;

			arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = pageLowerBound;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


		public static bool AddSubscriber(int forumId, int userId, Guid subGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
			sqlCommand.Append("FROM mp_ForumSubscriptions  ");
			sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			int subscriptionCount = 0;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
				}
			}

			sqlCommand = new StringBuilder();

			int rowsAffected = -1;

			if (subscriptionCount > 0)
			{
				arParams = new MySqlParameter[3];

				arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
				arParams[0].Direction = ParameterDirection.Input;
				arParams[0].Value = forumId;

				arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
				arParams[1].Direction = ParameterDirection.Input;
				arParams[1].Value = userId;

				arParams[2] = new MySqlParameter("?SubscribeDate", MySqlDbType.DateTime);
				arParams[2].Direction = ParameterDirection.Input;
				arParams[2].Value = DateTime.UtcNow;

				sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
				sqlCommand.Append("SET UnSubscribeDate = ?SubscribeDate, ");
				//sqlCommand.Append("UnSubscribeDate = Null ");
				sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID AND UnSubscribeDate IS NULL ;");

				rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			}

			arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?SubscribeDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = DateTime.UtcNow;

			arParams[3] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = subGuid.ToString();


			sqlCommand = new StringBuilder();

			sqlCommand.Append("INSERT INTO	mp_ForumSubscriptions ( ");
			sqlCommand.Append("ForumID, ");
			sqlCommand.Append("UserID, ");
			sqlCommand.Append("SubGuid, ");
			sqlCommand.Append("SubscribeDate");
			sqlCommand.Append(") ");
			sqlCommand.Append("VALUES ( ");
			sqlCommand.Append("?ForumID, ");
			sqlCommand.Append("?UserID, ");
			sqlCommand.Append("?SubGuid, ");
			sqlCommand.Append("?SubscribeDate");
			sqlCommand.Append(") ;");

			rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);


			return (rowsAffected > -1);



		}

		public static bool DeleteSubscription(int subscriptionId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ForumSubscriptions ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("SubscriptionID = ?SubscriptionID ");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SubscriptionID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = subscriptionId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > 0);

		}

		public static IDataReader GetForumSubscription(Guid subGuid)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * ");
			sqlCommand.Append("FROM	mp_ForumSubscriptions ");

			sqlCommand.Append("WHERE SubGuid = ?SubGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = subGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);


		}

		public static bool Unsubscribe(Guid subGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = ?UnSubscribeDate ");
			sqlCommand.Append("WHERE SubGuid = ?SubGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = subGuid.ToString();

			arParams[1] = new MySqlParameter("?UnSubscribeDate", MySqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = DateTime.UtcNow;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool Unsubscribe(int forumId, int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = ?UnSubscribeDate ");
			sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?UnSubscribeDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = DateTime.UtcNow;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool UnsubscribeAll(int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = ?UnSubscribeDate ");
			sqlCommand.Append("WHERE UserID = ?UserID ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = userId;

			arParams[1] = new MySqlParameter("?UnSubscribeDate", MySqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = DateTime.UtcNow;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumSubscriptionExists(int forumId, int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM	mp_ForumSubscriptions ");
			sqlCommand.Append("WHERE ForumID = ?ForumID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

			return (count > 0);

		}

		public static bool ForumThreadSubscriptionExists(int threadId, int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT Count(*) ");
			sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams));

			return (count > 0);

		}




		public static IDataReader ForumThreadGetThread(int threadId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	t.*, ");
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
			sqlCommand.Append("f.PostsPerPage, ");
			sqlCommand.Append("f.ModuleID ");

			sqlCommand.Append("FROM	mp_ForumThreads t ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON t.MostRecentPostUserID = u.UserID ");
			sqlCommand.Append("LEFT OUTER JOIN	mp_Users s ");
			sqlCommand.Append("ON t.StartedByUserID = s.UserID ");
			sqlCommand.Append("JOIN	mp_Forums f ");
			sqlCommand.Append("ON f.ItemID = t.ForumID ");
			sqlCommand.Append("WHERE t.ThreadID = ?ThreadID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);


		}

		public static IDataReader ForumThreadGetPost(int postId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	fp.* ");
			sqlCommand.Append("FROM	mp_ForumPosts fp ");
			sqlCommand.Append("WHERE fp.PostID = ?PostID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = postId;

			return MySqlHelper.ExecuteReader(
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

			StringBuilder sqlCommand = new StringBuilder();
			int forumSequence = 1;
			sqlCommand.Append("SELECT COALESCE(Max(ForumSequence) + 1,1) As ForumSequence FROM mp_ForumThreads WHERE ForumID = ?ForumID ; ");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					forumSequence = Convert.ToInt32(reader["ForumSequence"]);
				}
			}


			sqlCommand = new StringBuilder();

			sqlCommand.Append("INSERT INTO mp_ForumThreads ( ");
			sqlCommand.Append("ThreadGuid, ");
			sqlCommand.Append("ForumID, ");
			sqlCommand.Append("ThreadSubject, ");
			sqlCommand.Append("SortOrder, ");
			sqlCommand.Append("ForumSequence, ");
			sqlCommand.Append("IsLocked, ");
			sqlCommand.Append("StartedByUserID, ");
			sqlCommand.Append("ThreadDate, ");

			sqlCommand.Append("IsQuestion, ");
			sqlCommand.Append("IncludeInSiteMap, ");
			sqlCommand.Append("SetNoIndexMeta, ");
			sqlCommand.Append("PTitleOverride, ");

			sqlCommand.Append("ModStatus, ");
			sqlCommand.Append("ThreadType, ");

			sqlCommand.Append("MostRecentPostUserID, ");
			sqlCommand.Append("MostRecentPostDate ");
			sqlCommand.Append(" ) ");

			sqlCommand.Append("VALUES (");
			sqlCommand.Append("?ThreadGuid, ");
			sqlCommand.Append(" ?ForumID , ");
			sqlCommand.Append(" ?ThreadSubject  , ");
			sqlCommand.Append(" ?SortOrder, ");
			sqlCommand.Append(" ?ForumSequence, ");
			sqlCommand.Append(" ?IsLocked , ");
			sqlCommand.Append(" ?StartedByUserID , ");
			sqlCommand.Append(" ?ThreadDate , ");

			sqlCommand.Append("?IsQuestion, ");
			sqlCommand.Append("?IncludeInSiteMap, ");
			sqlCommand.Append("?SetNoIndexMeta, ");
			sqlCommand.Append("?PTitleOverride, ");

			sqlCommand.Append("?ModStatus, ");
			sqlCommand.Append("?ThreadType, ");

			sqlCommand.Append(" ?StartedByUserID , ");
			sqlCommand.Append(" ?ThreadDate  ");
			sqlCommand.Append(");");
			sqlCommand.Append("SELECT LAST_INSERT_ID();");

			arParams = new MySqlParameter[14];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?ThreadSubject", MySqlDbType.VarChar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = threadSubject;

			arParams[2] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = sortOrder;

			arParams[3] = new MySqlParameter("?IsLocked", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = locked;

			arParams[4] = new MySqlParameter("?StartedByUserID", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = startedByUserId;

			arParams[5] = new MySqlParameter("?ThreadDate", MySqlDbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = threadDate;

			arParams[6] = new MySqlParameter("?ForumSequence", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = forumSequence;

			arParams[7] = new MySqlParameter("?ThreadGuid", MySqlDbType.VarChar, 36);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = threadGuid.ToString();

			arParams[8] = new MySqlParameter("?IsQuestion", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = isQ;

			arParams[9] = new MySqlParameter("?IncludeInSiteMap", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = inMap;

			arParams[10] = new MySqlParameter("?SetNoIndexMeta", MySqlDbType.Int32);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = noIndex;

			arParams[11] = new MySqlParameter("?PTitleOverride", MySqlDbType.VarChar, 255);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = pageTitleOverride;

			arParams[12] = new MySqlParameter("?ModStatus", MySqlDbType.Int32);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = modStatus;

			arParams[13] = new MySqlParameter("?ThreadType", MySqlDbType.VarChar, 255);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = threadType;


			int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString()
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

			var sqlParams = new MySqlParameter[]
			{
				new MySqlParameter("?ThreadID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = threadId
				}
			};

			var rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams
			);

			return rowsAffected > -1;
		}


		public static bool ForumThreadDeletePosts(int threadId)
		{
			var sqlCommand = "DELETE FROM mp_ForumPosts WHERE ThreadID = ?ThreadID ;";

			var sqlParams = new MySqlParameter[]
			{
				new MySqlParameter("?ThreadID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = threadId
				}
			};

			var rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams
			);

			return rowsAffected > -1;

		}


		public static bool ForumThreadDeleteSubscriptions(int threadId)
		{
			var sqlCommand = "DELETE FROM mp_ForumThreadSubscriptions WHERE ThreadID = ?ThreadID;";

			var arParams = new MySqlParameter[]
			{
				new MySqlParameter("?ThreadID", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = threadId
				}
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE	 mp_ForumThreads ");
			sqlCommand.Append("SET	ForumID = ?ForumID, ");
			sqlCommand.Append("ThreadSubject = ?ThreadSubject, ");
			sqlCommand.Append("SortOrder = ?SortOrder, ");

			sqlCommand.Append("IsQuestion = ?IsQuestion, ");
			sqlCommand.Append("IncludeInSiteMap = ?IncludeInSiteMap, ");
			sqlCommand.Append("SetNoIndexMeta = ?SetNoIndexMeta, ");
			sqlCommand.Append("PTitleOverride = ?PTitleOverride, ");

			sqlCommand.Append("ModStatus = ?ModStatus, ");
			sqlCommand.Append("ThreadType = ?ThreadType, ");
			sqlCommand.Append("AssignedTo = ?AssignedTo, ");
			sqlCommand.Append("LockedBy = ?LockedBy, ");
			sqlCommand.Append("LockedReason = ?LockedReason, ");
			sqlCommand.Append("LockedUtc = ?LockedUtc, ");

			sqlCommand.Append("IsLocked = ?IsLocked ");


			sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

			MySqlParameter[] arParams = new MySqlParameter[15];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = forumId;

			arParams[2] = new MySqlParameter("?ThreadSubject", MySqlDbType.VarChar, 255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = threadSubject;

			arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = sortOrder;

			arParams[4] = new MySqlParameter("?IsLocked", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = locked;

			arParams[5] = new MySqlParameter("?IsQuestion", MySqlDbType.Int32);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = isQ;

			arParams[6] = new MySqlParameter("?IncludeInSiteMap", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = inMap;

			arParams[7] = new MySqlParameter("?SetNoIndexMeta", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = noIndex;

			arParams[8] = new MySqlParameter("?PTitleOverride", MySqlDbType.VarChar, 255);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = pageTitleOverride;

			arParams[9] = new MySqlParameter("?ModStatus", MySqlDbType.Int32);
			arParams[9].Direction = ParameterDirection.Input;
			arParams[9].Value = modStatus;

			arParams[10] = new MySqlParameter("?ThreadType", MySqlDbType.VarChar, 255);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = threadType;

			arParams[11] = new MySqlParameter("?AssignedTo", MySqlDbType.VarChar, 36);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = assignedTo.ToString();

			arParams[12] = new MySqlParameter("?LockedBy", MySqlDbType.VarChar, 36);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = lockedBy.ToString();

			arParams[13] = new MySqlParameter("?LockedReason", MySqlDbType.VarChar, 100);
			arParams[13].Direction = ParameterDirection.Input;
			arParams[13].Value = lockedReason;

			arParams[14] = new MySqlParameter("?LockedUtc", MySqlDbType.DateTime);
			arParams[14].Direction = ParameterDirection.Input;

			if (lockedUtc < DateTime.MaxValue)
			{
				arParams[14].Value = lockedUtc;
			}
			else
			{
				arParams[14].Value = DBNull.Value;
			}

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadIncrementReplyStats(
			int threadId,
			int mostRecentPostUserId,
			DateTime mostRecentPostDate)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_ForumThreads ");
			sqlCommand.Append("SET MostRecentPostUserID = ?MostRecentPostUserID, ");
			sqlCommand.Append("TotalReplies = TotalReplies + 1, ");
			sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = mostRecentPostUserId;

			arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = mostRecentPostDate;


			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadDecrementReplyStats(int threadId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT UserID, PostDate ");
			sqlCommand.Append("FROM mp_ForumPosts ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID ");
			sqlCommand.Append("ORDER BY PostID DESC ");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			int userId = 0;
			DateTime postDate = DateTime.Now;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
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

			sqlCommand = new StringBuilder();


			sqlCommand.Append("UPDATE mp_ForumThreads ");
			sqlCommand.Append("SET MostRecentPostUserID = ?MostRecentPostUserID, ");
			sqlCommand.Append("TotalReplies = TotalReplies - 1, ");
			sqlCommand.Append("MostRecentPostDate = ?MostRecentPostDate ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

			arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?MostRecentPostUserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?MostRecentPostDate", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = postDate;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadUpdateViewStats(int threadId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_ForumThreads ");
			sqlCommand.Append("SET TotalViews = TotalViews + 1 ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static IDataReader ForumThreadGetPosts(int threadId, int pageNumber)
		{
			int postsPerPage = 10;

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	f.PostsPerPage ");
			sqlCommand.Append("FROM		mp_ForumThreads ft ");
			sqlCommand.Append("JOIN		mp_Forums f ");
			sqlCommand.Append("ON		ft.ForumID = f.ItemID ");
			sqlCommand.Append("WHERE	ft.ThreadID = ?ThreadID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					postsPerPage = Convert.ToInt32(reader["PostsPerPage"]);
				}
			}
			sqlCommand = new StringBuilder();
			int currentPageMaxThreadSequence = postsPerPage * pageNumber;
			int beginSequence = 1;
			int endSequence;
			if (currentPageMaxThreadSequence > postsPerPage)
			{
				beginSequence = currentPageMaxThreadSequence - postsPerPage + 1;

			}

			//sqlCommand.Append("DECLARE @BeginSequence int; ");
			// sqlCommand.Append("DECLARE @EndSequence int; ");
			// sqlCommand.Append("SET @BeginSequence = " + beginSequence.ToString(CultureInfo.InvariantCulture) + " ;");
			//EndSequence = BeginSequence + PostsPerPage - 1;
			endSequence = beginSequence + postsPerPage;
			// sqlCommand.Append("SET @EndSequence = " + endSequence.ToString(CultureInfo.InvariantCulture) + " ;");

			sqlCommand.Append("SELECT	p.*, ");
			sqlCommand.Append("ft.ForumID, ");
			sqlCommand.Append("ft.IsLocked, ");
			// TODO:
			//using 'Guest' here is not culture neutral, need to pass in a label
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
			sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
			sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
			sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
			sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
			sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
			sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
			sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
			sqlCommand.Append("up.Signature As PostAuthorSignature ");

			sqlCommand.Append("FROM	mp_ForumPosts p ");

			sqlCommand.Append("JOIN	mp_ForumThreads ft ");
			sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
			sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
			sqlCommand.Append("ON up.UserID = p.UserID ");

			sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");
			sqlCommand.Append("AND p.ThreadSequence >= ?BeginSequence ");
			sqlCommand.Append("AND p.ThreadSequence <= ?EndSequence ");
			//sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID ;");
			sqlCommand.Append("ORDER BY p.SortOrder, p.ThreadSequence ;");

			arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?PageNumber", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageNumber;

			arParams[2] = new MySqlParameter("?BeginSequence", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = beginSequence;

			arParams[3] = new MySqlParameter("?EndSequence", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = endSequence;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader ForumThreadGetPosts(int threadId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT	p.*, ");
			sqlCommand.Append("ft.ForumID, ");
			sqlCommand.Append("ft.IsLocked, ");
			// TODO:
			//using 'Guest' here is not culture neutral, need to pass in a label
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
			sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
			sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
			sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
			sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
			sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
			sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
			sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
			sqlCommand.Append("up.Signature As PostAuthorSignature ");


			sqlCommand.Append("FROM	mp_ForumPosts p ");

			sqlCommand.Append("JOIN	mp_ForumThreads ft ");
			sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
			sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
			sqlCommand.Append("ON up.UserID = p.UserID ");

			sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");

			//sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
			sqlCommand.Append("ORDER BY p.PostID  ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader ForumThreadGetPostsReverseSorted(int threadId)
		{

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT ");
			sqlCommand.Append("p.*, ");
			//sqlCommand.Append("p.PostID, ");
			//sqlCommand.Append("p.ThreadID, ");
			//sqlCommand.Append("p.ThreadSequence, ");
			//sqlCommand.Append("p.Subject, ");
			//sqlCommand.Append("p.PostDate, ");
			//sqlCommand.Append("p.Approved, ");
			//sqlCommand.Append("p.UserID, ");
			//sqlCommand.Append("p.SortOrder, ");
			//sqlCommand.Append("p.Post, ");

			sqlCommand.Append("ft.ForumID, ");
			sqlCommand.Append("ft.IsLocked, ");
			// TODO:
			//using 'Guest' here is not culture neutral, need to pass in a label
			sqlCommand.Append("COALESCE(u.Name, 'Guest') As MostRecentPostUser, ");
			sqlCommand.Append("COALESCE(s.Name, 'Guest') As StartedBy, ");
			sqlCommand.Append("COALESCE(up.Name, 'Guest') As PostAuthor, ");
			sqlCommand.Append("COALESCE(up.Email, '') As AuthorEmail, ");
			sqlCommand.Append("COALESCE(up.TotalPosts, 0) As PostAuthorTotalPosts, ");
			sqlCommand.Append("COALESCE(up.AvatarUrl, 'blank.gif') As PostAuthorAvatar, ");
			sqlCommand.Append("COALESCE(up.TotalRevenue, 0) As UserRevenue, ");
			sqlCommand.Append("COALESCE(up.Trusted, 0) As Trusted, ");
			sqlCommand.Append("up.WebSiteURL As PostAuthorWebSiteUrl, ");
			sqlCommand.Append("up.Signature As PostAuthorSignature ");

			sqlCommand.Append("FROM	mp_ForumPosts p ");

			sqlCommand.Append("JOIN	mp_ForumThreads ft ");
			sqlCommand.Append("ON p.ThreadID = ft.ThreadID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
			sqlCommand.Append("ON ft.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN mp_Users s ");
			sqlCommand.Append("ON ft.StartedByUserID = s.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN	mp_Users up ");
			sqlCommand.Append("ON up.UserID = p.UserID ");

			sqlCommand.Append("WHERE ft.ThreadID = ?ThreadID ");

			//sqlCommand.Append("ORDER BY	p.SortOrder, p.PostID DESC ;");
			sqlCommand.Append("ORDER BY p.ThreadSequence DESC  ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader ForumThreadGetPostsByPage(int siteId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  fp.*, ");
			sqlCommand.Append("ft.MostRecentPostDate, ");
			sqlCommand.Append("f.ModuleID, ");
			sqlCommand.Append("f.ItemID, ");

			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("m.ViewRoles, ");
			sqlCommand.Append("md.FeatureName ");

			sqlCommand.Append("FROM	mp_ForumPosts fp ");

			sqlCommand.Append("JOIN	mp_ForumThreads ft ");
			sqlCommand.Append("ON fp.ThreadID = ft.ThreadID ");

			sqlCommand.Append("JOIN	mp_Forums f ");
			sqlCommand.Append("ON f.ItemID = ft.ForumID ");

			sqlCommand.Append("JOIN	mp_Modules m ");
			sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("JOIN	mp_PageModules pm ");
			sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

			sqlCommand.Append("JOIN	mp_Pages p ");
			sqlCommand.Append("ON p.PageID = pm.PageID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("p.SiteID = ?SiteID ");
			sqlCommand.Append("AND pm.PageID = ?PageID ");
			sqlCommand.Append(" ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


		public static IDataReader ForumThreadGetThreadsByPage(int siteId, int pageId)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  ft.*, ");

			sqlCommand.Append("f.ModuleID, ");
			sqlCommand.Append("f.ItemID, ");

			sqlCommand.Append("m.ModuleTitle, ");
			sqlCommand.Append("m.ViewRoles, ");
			sqlCommand.Append("md.FeatureName ");

			sqlCommand.Append("FROM	");

			sqlCommand.Append("	mp_ForumThreads ft ");
			//sqlCommand.Append("ON fp.ThreadID = ft.ThreadID ");

			sqlCommand.Append("JOIN	mp_Forums f ");
			sqlCommand.Append("ON f.ItemID = ft.ForumID ");

			sqlCommand.Append("JOIN	mp_Modules m ");
			sqlCommand.Append("ON f.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
			sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

			sqlCommand.Append("JOIN	mp_PageModules pm ");
			sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

			sqlCommand.Append("JOIN	mp_Pages p ");
			sqlCommand.Append("ON p.PageID = pm.PageID ");

			sqlCommand.Append("WHERE ");
			sqlCommand.Append("p.SiteID = ?SiteID ");
			sqlCommand.Append("AND pm.PageID = ?PageID ");
			sqlCommand.Append(" ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static IDataReader ForumThreadGetPostsForRss(int siteId, int pageId, int moduleId, int itemId, int threadId, int maximumDays)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT		fp.*, ");
			sqlCommand.Append("ft.ThreadSubject, ");
			sqlCommand.Append("ft.ForumID, ");
			sqlCommand.Append("p.PageID, ");
			sqlCommand.Append("p.AuthorizedRoles, ");
			sqlCommand.Append("m.ModuleID, ");
			sqlCommand.Append("m.ViewRoles, ");
			sqlCommand.Append("COALESCE(s.Name,'Guest') as StartedBy, ");
			sqlCommand.Append("COALESCE(up.Name, 'Guest') as PostAuthor, ");
			sqlCommand.Append("up.TotalPosts as PostAuthorTotalPosts, ");
			sqlCommand.Append("up.AvatarUrl as PostAuthorAvatar, ");
			sqlCommand.Append("up.WebSiteURL as PostAuthorWebSiteUrl, ");
			sqlCommand.Append("up.Signature as PostAuthorSignature ");


			sqlCommand.Append("FROM		mp_ForumPosts fp ");

			sqlCommand.Append("JOIN		mp_ForumThreads ft ");
			sqlCommand.Append("ON		fp.ThreadID = ft.ThreadID ");

			sqlCommand.Append("JOIN		mp_Forums f ");
			sqlCommand.Append("ON		ft.ForumID = f.ItemID ");

			sqlCommand.Append("JOIN		mp_Modules m ");
			sqlCommand.Append("ON		f.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN		mp_PageModules pm ");
			sqlCommand.Append("ON		pm.ModuleID = m.ModuleID ");

			sqlCommand.Append("JOIN		mp_Pages p ");
			sqlCommand.Append("ON		pm.PageID = p.PageID ");

			sqlCommand.Append("LEFT OUTER JOIN		mp_Users u ");
			sqlCommand.Append("ON		ft.MostRecentPostUserID = u.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN		mp_Users s ");
			sqlCommand.Append("ON		ft.StartedByUserID = s.UserID ");

			sqlCommand.Append("LEFT OUTER JOIN		mp_Users up ");
			sqlCommand.Append("ON		up.UserID = fp.UserID ");

			sqlCommand.Append("WHERE	p.SiteID = ?SiteID ");
			sqlCommand.Append("AND	(?PageID = -1 OR p.PageID = ?PageID) ");
			sqlCommand.Append("AND	(?ModuleID = -1 OR m.ModuleID = ?ModuleID) ");
			sqlCommand.Append("AND	(?ItemID = -1 OR f.ItemID = ?ItemID) ");
			sqlCommand.Append("AND	(?ThreadID = -1 OR ft.ThreadID = ?ThreadID) ");
			//sqlCommand.Append("AND	(?MaximumDays = -1 OR datediff('dd', now(), fp.PostDate) <= ?MaximumDays) ");

			sqlCommand.Append("AND	( (?MaximumDays = -1) OR  ((now() - ?MaximumDays) >= fp.PostDate )) ");


			sqlCommand.Append("ORDER BY	fp.PostDate DESC ; ");

			MySqlParameter[] arParams = new MySqlParameter[6];

			arParams[0] = new MySqlParameter("?SiteID", SqlDbType.Int);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = siteId;

			arParams[1] = new MySqlParameter("?PageID", SqlDbType.Int);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = pageId;

			arParams[2] = new MySqlParameter("?ModuleID", SqlDbType.Int);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = moduleId;

			arParams[3] = new MySqlParameter("?ItemID", SqlDbType.Int);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = itemId;

			arParams[4] = new MySqlParameter("?ThreadID", SqlDbType.Int);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = threadId;

			arParams[5] = new MySqlParameter("?MaximumDays", SqlDbType.Int);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = maximumDays;

			//String test = sqlCommand.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}

		public static DataSet ForumThreadGetSubscribers(int forumId, int threadId, int currentPostUserId, bool includeCurrentUser)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT u.Email, ");
			sqlCommand.Append("COALESCE(fts.ThreadSubscriptionID, -1) AS ThreadSubID, ");
			sqlCommand.Append("COALESCE(fs.SubscriptionID, -1) AS ForumSubID, ");
			sqlCommand.Append("COALESCE(fts.SubGuid, '00000000-0000-0000-0000-000000000000') AS ThreadSubGuid, ");
			sqlCommand.Append("COALESCE(fs.SubGuid, '00000000-0000-0000-0000-000000000000') AS ForumSubGuid ");

			sqlCommand.Append("FROM	mp_Users u ");

			sqlCommand.Append("LEFT OUTER JOIN mp_ForumThreadSubscriptions fts ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("fts.UserID = u.UserID ");
			sqlCommand.Append("AND fts.ThreadID = ?ThreadID ");
			sqlCommand.Append("AND fts.UnSubscribeDate IS NULL ");

			sqlCommand.Append("LEFT OUTER JOIN mp_ForumSubscriptions fs ");
			sqlCommand.Append("ON ");
			sqlCommand.Append("fs.UserID = u.UserID ");
			sqlCommand.Append("AND fs.ForumID = ?ForumID ");
			sqlCommand.Append("AND fs.UnSubscribeDate IS NULL ");

			sqlCommand.Append("WHERE ");
			if (!includeCurrentUser)
			{
				sqlCommand.Append(" u.UserID <> ?CurrentPostUserID ");
				sqlCommand.Append("AND ");
			}

			sqlCommand.Append("(");

			sqlCommand.Append("(");
			sqlCommand.Append("fts.ThreadSubscriptionID IS NOT NULL ");
			sqlCommand.Append(")");

			sqlCommand.Append("OR ");

			sqlCommand.Append("(");
			sqlCommand.Append("fs.SubscriptionID IS NOT NULL ");
			sqlCommand.Append(")");

			sqlCommand.Append(")");
			sqlCommand.Append(";");

			MySqlParameter[] arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ForumID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = forumId;

			arParams[1] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = threadId;

			arParams[2] = new MySqlParameter("?CurrentPostUserID", MySqlDbType.Int32);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = currentPostUserId;

			return MySqlHelper.ExecuteDataset(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


		public static IDataReader ForumThreadGetSubscriber(Guid subGuid)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT * ");
			sqlCommand.Append("FROM	mp_ForumThreadSubscriptions ");

			sqlCommand.Append("WHERE SubGuid = ?SubGuid ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = subGuid.ToString();

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);


		}

		public static bool ForumThreadAddSubscriber(int threadId, int userId, Guid subGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT COUNT(*) As SubscriptionCount ");
			sqlCommand.Append("FROM mp_ForumThreadSubscriptions  ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID AND UnSubscribeDate IS NULL ; ");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			int subscriptionCount = 0;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					subscriptionCount = Convert.ToInt32(reader["SubscriptionCount"]);
				}
			}

			sqlCommand = new StringBuilder();

			int rowsAffected = -1;

			arParams = new MySqlParameter[3];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = DateTime.UtcNow;



			if (subscriptionCount > 0)
			{
				sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
				sqlCommand.Append("SET UnSubscribeDate = ?CurrentTime ");
				//sqlCommand.Append("UnSubscribeDate = Null ");
				sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID AND UnSubscribeDate IS NULL ;");

				rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			}


			sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO	mp_ForumThreadSubscriptions ( ");

			sqlCommand.Append("ThreadID, ");
			sqlCommand.Append("SubGuid, ");
			sqlCommand.Append("SubscribeDate, ");
			sqlCommand.Append("UserID ");

			sqlCommand.Append(") ");

			sqlCommand.Append("VALUES ( ");

			sqlCommand.Append("?ThreadID, ");
			sqlCommand.Append("?SubGuid, ");
			sqlCommand.Append("?CurrentTime, ");
			sqlCommand.Append("?UserID ");

			sqlCommand.Append(") ;");

			arParams = new MySqlParameter[4];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			arParams[2] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = subGuid.ToString();

			arParams[3] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = DateTime.UtcNow;



			rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadUnSubscribe(Guid subGuid)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = ?CurrentTime ");
			sqlCommand.Append("WHERE SubGuid = ?SubGuid  ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?SubGuid", MySqlDbType.VarChar, 36);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = subGuid.ToString();

			arParams[1] = new MySqlParameter("?CurrentTime", MySqlDbType.DateTime);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = DateTime.UtcNow;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadUNSubscribe(int threadId, int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = now() ");
			sqlCommand.Append("WHERE ThreadID = ?ThreadID AND UserID = ?UserID ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = userId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumThreadUnsubscribeAll(int userId)
		{
			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("UPDATE mp_ForumThreadSubscriptions ");
			sqlCommand.Append("SET UnSubscribeDate = now() ");
			sqlCommand.Append("WHERE UserID = ?UserID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = userId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

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

			StringBuilder sqlCommand = new StringBuilder();

			sqlCommand.Append("SELECT COALESCE(Max(ThreadSequence) + 1,1) As ThreadSequence FROM mp_ForumPosts WHERE ThreadID = ?ThreadID ; ");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			int threadSequence = 1;

			using (IDataReader reader = MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams))
			{
				if (reader.Read())
				{
					threadSequence = Convert.ToInt32(reader["ThreadSequence"]);
				}
			}

			sqlCommand = new StringBuilder();
			//sqlCommand.Append("SET @ThreadSequence = " + threadSequence.ToString() + " ; ");

			sqlCommand.Append("INSERT INTO mp_ForumPosts ( ");
			sqlCommand.Append("ThreadID, ");
			sqlCommand.Append("Subject, ");
			sqlCommand.Append("Post, ");
			sqlCommand.Append("PostDate, ");
			sqlCommand.Append("Approved, ");
			sqlCommand.Append("UserID, ");

			sqlCommand.Append("PostGuid, ");
			sqlCommand.Append("AnswerVotes, ");
			sqlCommand.Append("ApprovedBy, ");
			sqlCommand.Append("ApprovedUtc, ");
			sqlCommand.Append("UserIp, ");

			sqlCommand.Append("NotificationSent, ");
			sqlCommand.Append("ModStatus, ");

			sqlCommand.Append("ThreadSequence ");

			sqlCommand.Append(" ) ");

			sqlCommand.Append("VALUES (");
			sqlCommand.Append(" ?ThreadID , ");
			sqlCommand.Append(" ?Subject  , ");
			sqlCommand.Append(" ?Post, ");
			sqlCommand.Append(" ?PostDate, ");
			sqlCommand.Append(" ?Approved , ");
			sqlCommand.Append(" ?UserID , ");

			sqlCommand.Append("?PostGuid, ");
			sqlCommand.Append("0, ");
			sqlCommand.Append("?ApprovedBy, ");
			sqlCommand.Append("?ApprovedUtc, ");
			sqlCommand.Append("?UserIp, ");

			sqlCommand.Append("?NotificationSent, ");
			sqlCommand.Append("?ModStatus, ");

			sqlCommand.Append(" ?ThreadSequence  ");

			sqlCommand.Append(");");
			sqlCommand.Append("SELECT LAST_INSERT_ID();");

			arParams = new MySqlParameter[13];

			arParams[0] = new MySqlParameter("?ThreadID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = threadId;

			arParams[1] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = subject;

			arParams[2] = new MySqlParameter("?Post", MySqlDbType.MediumText);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = post;

			arParams[3] = new MySqlParameter("?Approved", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = approve;

			arParams[4] = new MySqlParameter("?UserID", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = userId;

			arParams[5] = new MySqlParameter("?PostDate", MySqlDbType.DateTime);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = postDate;

			arParams[6] = new MySqlParameter("?ThreadSequence", MySqlDbType.Int32);
			arParams[6].Direction = ParameterDirection.Input;
			arParams[6].Value = threadSequence;

			arParams[7] = new MySqlParameter("?PostGuid", MySqlDbType.VarChar, 36);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = postGuid.ToString();

			arParams[8] = new MySqlParameter("?ApprovedBy", MySqlDbType.VarChar, 36);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = approvedBy.ToString();

			arParams[9] = new MySqlParameter("?ApprovedUtc", MySqlDbType.DateTime);
			arParams[9].Direction = ParameterDirection.Input;
			if (approvedUtc > DateTime.MinValue)
			{
				arParams[9].Value = approvedUtc;
			}
			else
			{
				arParams[9].Value = DBNull.Value;
			}

			arParams[10] = new MySqlParameter("?UserIp", MySqlDbType.VarChar, 50);
			arParams[10].Direction = ParameterDirection.Input;
			arParams[10].Value = userIp;

			arParams[11] = new MySqlParameter("?NotificationSent", MySqlDbType.Int32);
			arParams[11].Direction = ParameterDirection.Input;
			arParams[11].Value = intNotificationSent;

			arParams[12] = new MySqlParameter("?ModStatus", MySqlDbType.Int32);
			arParams[12].Direction = ParameterDirection.Input;
			arParams[12].Value = modStatus;


			int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());

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

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_ForumPosts ");
			sqlCommand.Append("SET Subject = ?Subject, ");
			sqlCommand.Append("Post = ?Post, ");
			sqlCommand.Append("SortOrder = ?SortOrder, ");

			sqlCommand.Append("NotificationSent = ?NotificationSent, ");
			sqlCommand.Append("ModStatus = ?ModStatus, ");

			sqlCommand.Append("ApprovedBy = ?ApprovedBy, ");
			sqlCommand.Append("ApprovedUtc = ?ApprovedUtc, ");


			sqlCommand.Append("Approved = ?Approved ");
			sqlCommand.Append("WHERE PostID = ?PostID ;");

			MySqlParameter[] arParams = new MySqlParameter[9];

			arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = postId;

			arParams[1] = new MySqlParameter("?Subject", MySqlDbType.VarChar, 255);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = subject;

			arParams[2] = new MySqlParameter("?Post", MySqlDbType.MediumText);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = post;

			arParams[3] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = sortOrder;

			arParams[4] = new MySqlParameter("?Approved", MySqlDbType.Int32);
			arParams[4].Direction = ParameterDirection.Input;
			arParams[4].Value = approve;

			arParams[5] = new MySqlParameter("?ApprovedBy", MySqlDbType.VarChar, 36);
			arParams[5].Direction = ParameterDirection.Input;
			arParams[5].Value = approvedBy.ToString();

			arParams[6] = new MySqlParameter("?ApprovedUtc", MySqlDbType.DateTime);
			arParams[6].Direction = ParameterDirection.Input;
			if (approvedUtc > DateTime.MinValue)
			{
				arParams[6].Value = approvedUtc;
			}
			else
			{
				arParams[6].Value = DBNull.Value;
			}

			arParams[7] = new MySqlParameter("?NotificationSent", MySqlDbType.Int32);
			arParams[7].Direction = ParameterDirection.Input;
			arParams[7].Value = intNotificationSent;

			arParams[8] = new MySqlParameter("?ModStatus", MySqlDbType.Int32);
			arParams[8].Direction = ParameterDirection.Input;
			arParams[8].Value = modStatus;



			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumPostUpdateThreadSequence(
			int postId,
			int threadSequence)
		{
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("UPDATE mp_ForumPosts ");
			sqlCommand.Append("SET ThreadSequence = ?ThreadSequence ");
			sqlCommand.Append("WHERE PostID = ?PostID ;");

			MySqlParameter[] arParams = new MySqlParameter[2];

			arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = postId;

			arParams[1] = new MySqlParameter("?ThreadSequence", MySqlDbType.Int32);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = threadSequence;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}

		public static bool ForumPostDelete(int postId)
		{

			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_ForumPosts ");
			sqlCommand.Append("WHERE PostID = ?PostID ;");

			MySqlParameter[] arParams = new MySqlParameter[1];

			arParams[0] = new MySqlParameter("?PostID", MySqlDbType.Int32);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = postId;

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);

		}






	}
}
