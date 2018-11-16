// Created:				    2004-08-14
// Last Modified:			2017-07-17

using log4net;
using mojoPortal.Data;
using System;
using System.Data;
using System.Globalization;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a blog post
	/// </summary>
	public class Blog : IIndexableContent
	{
		private const string featureGuid = "026cbead-2b80-4491-906d-b83e37179ccf";

		public static Guid FeatureGuid
		{
			get { return new Guid(featureGuid); }
		}

		#region Constructors

		public Blog()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Blog"/> class.
		/// </summary>
		/// <param name="itemID">The item ID.</param>
		public Blog(int itemId)
		{
			if (itemId > -1)
			{
				GetBlog(itemId);
			}

		}

		#endregion

		#region Private Properties

		private static readonly ILog log = LogManager.GetLogger(typeof(Blog));

		private int itemID = -1;
		private int previousItemId = -1;
		private int nextItemId = -1;
		private Guid blogGuid = Guid.Empty;
		private Guid moduleGuid = Guid.Empty;
		private int moduleID = -1;
		private string userName = string.Empty;
		private string title = string.Empty;

		private string location = string.Empty;

		//aliased as Abstract
		private string excerpt = string.Empty;

		private string description = string.Empty;
		private DateTime startDate = DateTime.UtcNow;
		private bool isPublished = true;
		private bool isInNewsletter = true;
		private bool includeInFeed = true;
		private string category = string.Empty;
		private int allowCommentsForDays = 60;
		private Guid userGuid = Guid.Empty;
		private Guid lastModUserGuid = Guid.Empty;
		private DateTime createdUtc = DateTime.UtcNow;
		private DateTime lastModUtc = DateTime.UtcNow;
		private string itemUrl = string.Empty;
		private string previousPostUrl = string.Empty;
		private string previousPostTitle = string.Empty;
		private string nextPostUrl = string.Empty;
		private string nextPostTitle = string.Empty;
		private int commentCount = 0;

		private string metaKeywords = string.Empty;
		private string metaDescription = string.Empty;
		private string compiledMeta = string.Empty;

		private int siteId = -1;
		private string searchIndexPath = string.Empty;





		#endregion

		#region Public Properties

		public Guid BlogGuid
		{
			get { return blogGuid; }

		}

		public Guid ModuleGuid
		{
			get { return moduleGuid; }
			set { moduleGuid = value; }
		}

		public int ItemId
		{
			get { return itemID; }
		}

		public int PreviousItemId
		{
			get { return previousItemId; }
		}

		public int NextItemId
		{
			get { return nextItemId; }
		}

		public int ModuleId
		{
			get { return moduleID; }
			set { moduleID = value; }
		}

		public string UserName
		{
			get { return userName; }
			set { userName = value; }
		}

		public Guid UserGuid
		{
			get { return userGuid; }
			set { userGuid = value; }
		}

		public Guid LastModUserGuid
		{
			get { return lastModUserGuid; }
			set { lastModUserGuid = value; }
		}

		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		private string subTitle = string.Empty;

		public string SubTitle
		{
			get { return subTitle; }
			set { subTitle = value; }
		}

		public string Category
		{
			get { return category; }
			set { category = value; }
		}

		public string Excerpt
		{
			get { return excerpt; }
			set { excerpt = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public string Location
		{
			get { return location; }
			set { location = value; }
		}

		public string MetaKeywords
		{
			get { return metaKeywords; }
			set { metaKeywords = value; }
		}

		public string MetaDescription
		{
			get { return metaDescription; }
			set { metaDescription = value; }
		}

		public string CompiledMeta
		{
			get { return compiledMeta; }
			set { compiledMeta = value; }
		}

		public DateTime StartDate
		{
			get { return startDate; }
			set { startDate = value; }
		}

		private DateTime endDate = DateTime.MaxValue;

		public DateTime EndDate
		{
			get { return endDate; }
			set { endDate = value; }
		}

		private bool approved = true;

		public bool Approved
		{
			get { return approved; }
			set { approved = value; }
		}

		private Guid approvedBy = Guid.Empty;

		public Guid ApprovedBy
		{
			get { return approvedBy; }
			set { approvedBy = value; }
		}

		private DateTime approvedDate = DateTime.MaxValue;

		public DateTime ApprovedDate
		{
			get { return approvedDate; }
			set { approvedDate = value; }
		}

		public bool IsPublished
		{
			get { return isPublished; }
			set { isPublished = value; }
		}

		public bool IsInNewsletter
		{
			get { return isInNewsletter; }
			set { isInNewsletter = value; }
		}

		public bool IncludeInFeed
		{
			get { return includeInFeed; }
			set { includeInFeed = value; }
		}

		public int AllowCommentsForDays
		{
			get { return allowCommentsForDays; }
			set { allowCommentsForDays = value; }
		}

		public DateTime CreatedUtc
		{
			get { return createdUtc; }
			set { createdUtc = value; }
		}

		public DateTime LastModUtc
		{
			get { return lastModUtc; }
			set { lastModUtc = value; }
		}

		public string ItemUrl
		{
			get { return itemUrl; }
			set { itemUrl = value; }
		}

		//

		private bool showAuthorName = true;

		public bool ShowAuthorName
		{
			get { return showAuthorName; }
			set { showAuthorName = value; }
		}

		private bool showAuthorAvatar = true;

		public bool ShowAuthorAvatar
		{
			get { return showAuthorAvatar; }
			set { showAuthorAvatar = value; }
		}

		private bool showAuthorBio = true;

		public bool ShowAuthorBio
		{
			get { return showAuthorBio; }
			set { showAuthorBio = value; }
		}

		private bool includeInSearch = true;

		public bool IncludeInSearch
		{
			get { return includeInSearch; }
			set { includeInSearch = value; }
		}

		private bool useBingMap = false;

		public bool UseBingMap
		{
			get { return useBingMap; }
			set { useBingMap = value; }
		}

		private string mapHeight = "300";

		public string MapHeight
		{
			get { return mapHeight; }
			set { mapHeight = value; }
		}

		private string mapWidth = "500";

		public string MapWidth
		{
			get { return mapWidth; }
			set { mapWidth = value; }
		}

		private bool showMapOptions = false;

		public bool ShowMapOptions
		{
			get { return showMapOptions; }
			set { showMapOptions = value; }
		}

		private bool showZoomTool = false;

		public bool ShowZoomTool
		{
			get { return showZoomTool; }
			set { showZoomTool = value; }
		}

		private bool showLocationInfo = false;

		public bool ShowLocationInfo
		{
			get { return showLocationInfo; }
			set { showLocationInfo = value; }
		}

		private bool useDrivingDirections = false;

		public bool UseDrivingDirections
		{
			get { return useDrivingDirections; }
			set { useDrivingDirections = value; }
		}

		private string mapType = "G_SATELLITE_MAP";

		public string MapType
		{
			get { return mapType; }
			set { mapType = value; }
		}

		private int mapZoom = 13;

		public int MapZoom
		{
			get { return mapZoom; }
			set { mapZoom = value; }
		}

		private bool showDownloadLink = false;

		public bool ShowDownloadLink
		{
			get { return showDownloadLink; }
			set { showDownloadLink = value; }
		}

		private bool includeInSiteMap = true;

		public bool IncludeInSiteMap
		{
			get { return includeInSiteMap; }
			set { includeInSiteMap = value; }
		}

		private bool excludeFromRecentContent = false;

		public bool ExcludeFromRecentContent
		{
			get { return excludeFromRecentContent; }
			set { excludeFromRecentContent = value; }
		}






		public string PreviousPostUrl
		{
			get { return previousPostUrl; }

		}

		public string NextPostUrl
		{
			get { return nextPostUrl; }

		}

		public string PreviousPostTitle
		{
			get { return previousPostTitle; }

		}

		public string NextPostTitle
		{
			get { return nextPostTitle; }

		}

		public int CommentCount
		{
			get { return commentCount; }
		}

		private int userId = -1;

		public int UserId
		{
			get { return userId; }
		}

		private string userLoginName = string.Empty;

		public string UserLoginName
		{
			get { return userLoginName; }

		}

		private string userDisplayName = string.Empty;

		public string UserDisplayName
		{
			get { return userDisplayName; }

		}

		private string userFirstName = string.Empty;

		public string UserFirstName
		{
			get { return userFirstName; }

		}

		private string userLastName = string.Empty;

		public string UserLastName
		{
			get { return userLastName; }

		}

		private string userEmail = string.Empty;

		public string UserEmail
		{
			get { return userEmail; }

		}

		private string userAvatar = string.Empty;

		public string UserAvatar
		{
			get { return userAvatar; }

		}

		private string authorBio = string.Empty;

		public string AuthorBio
		{
			get { return authorBio; }

		}

		private bool includeInNews = false;

		public bool IncludeInNews
		{
			get { return includeInNews; }
			set { includeInNews = value; }
		}

		private string pubName = string.Empty;

		public string PubName
		{
			get { return pubName; }
			set { pubName = value; }
		}

		private string pubLanguage = string.Empty;

		public string PubLanguage
		{
			get { return pubLanguage; }
			set { pubLanguage = value; }
		}

		private string pubAccess = string.Empty;

		public string PubAccess
		{
			get { return pubAccess; }
			set { pubAccess = value; }
		}

		private string pubGenres = string.Empty;

		public string PubGenres
		{
			get { return pubGenres; }
			set { pubGenres = value; }
		}

		private string pubKeyWords = string.Empty;

		public string PubKeyWords
		{
			get { return pubKeyWords; }
			set { pubKeyWords = value; }
		}

		private string pubGeoLocations = string.Empty;

		public string PubGeoLocations
		{
			get { return pubGeoLocations; }
			set { pubGeoLocations = value; }
		}

		private string pubStockTickers = string.Empty;

		public string PubStockTickers
		{
			get { return pubStockTickers; }
			set { pubStockTickers = value; }
		}

		private string headlineImageUrl = string.Empty;

		public string HeadlineImageUrl
		{
			get { return headlineImageUrl; }
			set { headlineImageUrl = value; }
		}

		private bool includeImageInExcerpt = true;

		public bool IncludeImageInExcerpt
		{
			get { return includeImageInExcerpt; }
			set { includeImageInExcerpt = value; }
		}

		private bool includeImageInPost = true;

		public bool IncludeImageInPost
		{
			get { return includeImageInPost; }
			set { includeImageInPost = value; }
		}




		/// <summary>
		/// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
		/// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
		/// So we store extra properties here so we don't need any other objects.
		/// </summary>
		public int SiteId
		{
			get { return siteId; }
			set { siteId = value; }
		}

		/// <summary>
		/// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
		/// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
		/// So we store extra properties here so we don't need any other objects.
		/// </summary>
		public string SearchIndexPath
		{
			get { return searchIndexPath; }
			set { searchIndexPath = value; }
		}


		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the blog.
		/// </summary>
		/// <param name="itemID">The item ID.</param>
		private void GetBlog(int itemId)
		{
			using (IDataReader reader = DBBlog.GetSingleBlog(itemId, DateTime.UtcNow))
			{
				if (reader.Read())
				{
					this.itemID = Convert.ToInt32(reader["ItemID"].ToString(), CultureInfo.InvariantCulture);
					this.moduleID = Convert.ToInt32(reader["ModuleID"].ToString(), CultureInfo.InvariantCulture);
					this.userName = reader["Name"].ToString();
					this.title = reader["Heading"].ToString();
					this.excerpt = reader["Abstract"].ToString();
					this.description = reader["Description"].ToString();

					this.metaKeywords = reader["MetaKeywords"].ToString();
					this.metaDescription = reader["MetaDescription"].ToString();

					this.startDate = Convert.ToDateTime(reader["StartDate"].ToString());

					// this is to support dbs that don't have bit data type
					//string inNews = reader["IsInNewsletter"].ToString();
					//this.isInNewsletter = (inNews == "True" || inNews == "1");

					this.isInNewsletter = Convert.ToBoolean(reader["IsInNewsletter"]);

					//string inFeed = reader["IncludeInFeed"].ToString();
					//this.includeInFeed = (inFeed == "True" || inFeed == "1");

					this.includeInFeed = Convert.ToBoolean(reader["IncludeInFeed"]);

					if (reader["AllowCommentsForDays"] != DBNull.Value)
					{
						this.allowCommentsForDays = Convert.ToInt32(reader["AllowCommentsForDays"]);
					}

					this.blogGuid = new Guid(reader["BlogGuid"].ToString());
					this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
					this.location = reader["Location"].ToString();
					this.compiledMeta = reader["CompiledMeta"].ToString();

					if (reader["CreatedDate"] != DBNull.Value)
					{
						this.createdUtc = Convert.ToDateTime(reader["CreatedDate"]);
					}

					if (reader["LastModUtc"] != DBNull.Value)
					{
						this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					}

					string var = reader["UserGuid"].ToString();
					if (var.Length == 36) this.userGuid = new Guid(var);

					var = reader["LastModUserGuid"].ToString();
					if (var.Length == 36) this.lastModUserGuid = new Guid(var);

					itemUrl = reader["ItemUrl"].ToString();

					previousPostUrl = reader["PreviousPost"].ToString();
					previousPostTitle = reader["PreviousPostTitle"].ToString();
					nextPostUrl = reader["NextPost"].ToString();
					nextPostTitle = reader["NextPostTitle"].ToString();

					commentCount = Convert.ToInt32(reader["CommentCount"]);

					isPublished = Convert.ToBoolean(reader["IsPublished"]);

					this.previousItemId = Convert.ToInt32(reader["PreviousItemID"].ToString(), CultureInfo.InvariantCulture);
					this.nextItemId = Convert.ToInt32(reader["NextItemID"].ToString(), CultureInfo.InvariantCulture);

					this.userId = Convert.ToInt32(reader["UserID"]);
					this.userDisplayName = reader["Name"].ToString();
					this.userLoginName = reader["LoginName"].ToString();
					this.userFirstName = reader["FirstName"].ToString();
					this.userLastName = reader["LastName"].ToString();
					this.userEmail = reader["Email"].ToString();
					this.userAvatar = reader["AvatarUrl"].ToString();
					this.authorBio = reader["AuthorBio"].ToString();

					this.subTitle = reader["SubTitle"].ToString();

					if (reader["EndDate"] != DBNull.Value)
					{
						this.endDate = Convert.ToDateTime(reader["EndDate"]);
					}

					if (reader["ApprovedDate"] != DBNull.Value)
					{
						this.approvedDate = Convert.ToDateTime(reader["ApprovedDate"]);
					}

					if (reader["ApprovedBy"] != DBNull.Value)
					{
						var = reader["ApprovedBy"].ToString();
						if (var.Length == 36) this.approvedBy = new Guid(var);
					}

					if (reader["Approved"] != DBNull.Value)
					{
						this.approved = Convert.ToBoolean(reader["Approved"]);
					}


					// below added 2012-12-06

					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						this.showAuthorName = Convert.ToBoolean(reader["ShowAuthorName"]);
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						this.showAuthorAvatar = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						this.showAuthorBio = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}

					if (reader["IncludeInSearch"] != DBNull.Value)
					{
						this.includeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
					}

					if (reader["IncludeInSiteMap"] != DBNull.Value)
					{
						this.includeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						this.UseBingMap = Convert.ToBoolean(reader["UseBingMap"]);
					}

					if (reader["MapHeight"] != DBNull.Value)
					{
						this.mapHeight = reader["MapHeight"].ToString();
					}

					if (reader["MapWidth"] != DBNull.Value)
					{
						this.mapWidth = reader["MapWidth"].ToString();
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						this.showMapOptions = Convert.ToBoolean(reader["ShowMapOptions"]);
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						this.showZoomTool = Convert.ToBoolean(reader["ShowZoomTool"]);
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						this.showLocationInfo = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						this.useDrivingDirections = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}

					if (reader["MapType"] != DBNull.Value)
					{
						this.mapType = reader["MapType"].ToString();
					}

					if (reader["MapZoom"] != DBNull.Value)
					{
						this.mapZoom = Convert.ToInt32(reader["MapZoom"]);
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						this.showDownloadLink = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}

					if (reader["ExcludeFromRecentContent"] != DBNull.Value)
					{
						this.excludeFromRecentContent = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);
					}

					if (reader["IncludeInNews"] != DBNull.Value)
					{
						this.includeInNews = Convert.ToBoolean(reader["IncludeInNews"]);
					}

					pubName = reader["PubName"].ToString();
					pubLanguage = reader["PubLanguage"].ToString();
					pubAccess = reader["PubAccess"].ToString();
					pubGenres = reader["PubGenres"].ToString();
					pubKeyWords = reader["PubKeyWords"].ToString();
					pubGeoLocations = reader["PubGeoLocations"].ToString();
					pubStockTickers = reader["PubStockTickers"].ToString();
					headlineImageUrl = reader["HeadlineImageUrl"].ToString();

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						includeImageInExcerpt = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}

					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						includeImageInPost = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
				}
			}
		}

		/// <summary>
		/// Creates a new blog
		/// </summary>
		/// <returns>true if successful</returns>
		private bool Create()
		{
			int newID = 0;
			blogGuid = Guid.NewGuid();
			createdUtc = DateTime.UtcNow;

			if (approved)
			{
				approvedDate = DateTime.UtcNow;
			}

			newID = DBBlog.AddBlog(
				blogGuid,
				moduleGuid,
				moduleID,
				userName,
				title,
				excerpt,
				description,
				startDate,
				isInNewsletter,
				includeInFeed,
				allowCommentsForDays,
				location,
				userGuid,
				createdUtc,
				itemUrl,
				metaKeywords,
				metaDescription,
				compiledMeta,
				isPublished,
				subTitle,
				endDate,
				approved,
				approvedBy,
				approvedDate,
				showAuthorName,
				showAuthorAvatar,
				showAuthorBio,
				includeInSearch,
				useBingMap,
				mapHeight,
				MapWidth,
				showMapOptions,
				showZoomTool,
				showLocationInfo,
				useDrivingDirections,
				mapType,
				mapZoom,
				showDownloadLink,
				includeInSiteMap,
				excludeFromRecentContent,
				includeInNews,
				pubName,
				pubLanguage,
				pubAccess,
				pubGenres,
				pubKeyWords,
				pubGeoLocations,
				pubStockTickers,
				headlineImageUrl,
				includeImageInExcerpt,
				includeImageInPost
			);

			itemID = newID;

			bool result = (newID > 0);

			//IndexHelper.IndexItem(this);
			if (result)
			{
				ContentChangedEventArgs e = new ContentChangedEventArgs();
				OnContentChanged(e);
			}

			return result;
		}

		/// <summary>
		/// Updates this instance.
		/// </summary>
		/// <returns></returns>
		private bool Update()
		{
			lastModUtc = DateTime.UtcNow;
			if ((approved) && (approvedDate == DateTime.MaxValue)) { approvedDate = DateTime.UtcNow; }

			bool result = DBBlog.UpdateBlog(
				moduleID,
				itemID,
				userName,
				title,
				excerpt,
				description,
				startDate,
				isInNewsletter,
				includeInFeed,
				allowCommentsForDays,
				location,
				lastModUserGuid,
				lastModUtc,
				itemUrl,
				metaKeywords,
				metaDescription,
				compiledMeta,
				isPublished,
				subTitle,
				endDate,
				approved,
				approvedBy,
				approvedDate,
				showAuthorName,
				showAuthorAvatar,
				showAuthorBio,
				includeInSearch,
				useBingMap,
				mapHeight,
				MapWidth,
				showMapOptions,
				showZoomTool,
				showLocationInfo,
				useDrivingDirections,
				mapType,
				mapZoom,
				showDownloadLink,
				includeInSiteMap,
				excludeFromRecentContent,
				includeInNews,
				pubName,
				pubLanguage,
				pubAccess,
				pubGenres,
				pubKeyWords,
				pubGeoLocations,
				pubStockTickers,
				headlineImageUrl,
				includeImageInExcerpt,
				includeImageInPost
			);

			//IndexHelper.IndexItem(this);
			ContentChangedEventArgs e = new ContentChangedEventArgs();
			OnContentChanged(e);

			return result;
		}

		#endregion


		#region Public Methods

		public void CreateHistory(Guid siteGuid)
		{
			if (blogGuid == Guid.Empty)
			{
				return;
			}

			Blog currentVersion = new Blog(itemID);

			if (currentVersion.Description == Description)
			{
				return;
			}

			ContentHistory history = new ContentHistory();
			history.ContentGuid = currentVersion.BlogGuid;
			history.Title = currentVersion.Title;
			history.ContentText = currentVersion.Description;
			history.SiteGuid = siteGuid;
			history.UserGuid = currentVersion.LastModUserGuid;
			history.CreatedUtc = currentVersion.LastModUtc;
			history.Save();
		}


		/// <summary>
		/// Saves this instance.
		/// </summary>
		/// <returns></returns>
		public bool Save()
		{
			if (itemID > 0)
			{
				return Update();
			}
			else
			{
				return Create();
			}


		}

		public bool Delete()
		{
			DBBlog.DeleteItemCategories(itemID);
			DBBlog.DeleteAllCommentsForBlog(itemID);
			DBBlog.UpdateCommentStats(moduleID);
			bool result = DBBlog.DeleteBlog(itemID);
			DBBlog.UpdateEntryStats(moduleID);

			ContentChangedEventArgs e = new ContentChangedEventArgs();
			e.IsDeleted = true;
			OnContentChanged(e);

			return result;
		}

		#endregion


		#region Static Methods

		/// <summary>
		/// Gets the posts for this blog instance.
		/// </summary>
		/// <param name="moduleID">The module ID.</param>
		/// <returns></returns>
		//public static IDataReader GetBlogs(int moduleID)
		//{
		//    return dbBlog.Blog_GetBlogs(moduleID);
		//}

		/// <summary>
		/// Gets the blogs.
		/// </summary>
		/// <param name="moduleID">The module ID.</param>
		/// <param name="endDate">The end date.</param>
		/// <returns></returns>
		public static IDataReader GetBlogs(int moduleId, DateTime beginDate)
		{
			return DBBlog.GetBlogs(moduleId, beginDate, DateTime.UtcNow);
		}

		public static IDataReader GetBlogsForFeed(int moduleId, DateTime beginDate)
		{
			return DBBlog.GetBlogsForFeed(moduleId, beginDate, DateTime.UtcNow);
		}

		public static IDataReader GetBlogsForMetaWeblogApi(int moduleId, DateTime beginDate)
		{
			return DBBlog.GetBlogsForMetaWeblogApi(moduleId, beginDate, DateTime.UtcNow);
		}

		public static DataSet GetBlogsForMetaWeblogApiDataSet(int moduleId, DateTime beginDate)
		{
			DataSet dataSet = new DataSet();

			DataTable posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetBlogsForMetaWeblogApi(moduleId, beginDate, DateTime.UtcNow))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					DataRow row = posts.NewRow();


					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					row["BlogGuid"] = reader["BlogGuid"].ToString();
					row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					row["Heading"] = reader["Heading"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					row["Description"] = reader["Description"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Location"] = reader["Location"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["MetaDescription"] = reader["MetaDescription"];
					row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["IsPublished"] = Convert.ToBoolean(reader["IsPublished"]);
					row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					row["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					row["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					row["HeadlineImageUrl"] = reader["HeadlineImageUrl"];
					row["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					row["Name"] = reader["Name"];
					row["LoginName"] = reader["LoginName"];
					row["Email"] = reader["Email"];


					posts.Rows.Add(row);
				}
			}

			posts.TableName = "Posts";
			dataSet.Tables.Add(posts);

			DataTable categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetBlogCategoriesForMetaWeblogApi(moduleId, beginDate, DateTime.UtcNow))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}
			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postCategories",
					dataSet.Tables["Posts"].Columns["ItemID"],
					dataSet.Tables["Categories"].Columns["ItemID"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }


			return dataSet;
		}

		private static DataTable GetCategoryTableStructure()
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("ID", typeof(int));
			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.Columns.Add("CategoryID", typeof(int));
			dataTable.Columns.Add("Category", typeof(string));

			return dataTable;

		}

		private static DataTable GetAttachmentsTableStructure()
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("RowGuid", typeof(string));
			dataTable.Columns.Add("ItemGuid", typeof(string));
			dataTable.Columns.Add("FileName", typeof(string));
			dataTable.Columns.Add("ServerFileName", typeof(string));
			dataTable.Columns.Add("ContentLength", typeof(long));
			dataTable.Columns.Add("ContentType", typeof(string));
			dataTable.Columns.Add("ShowDownloadLink", typeof(bool));

			return dataTable;

		}

		private static DataTable GetPostsTableStructure()
		{
			DataTable dataTable = new DataTable();



			dataTable.Columns.Add("ModuleID", typeof(int));
			dataTable.Columns.Add("BlogGuid", typeof(string));
			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ItemID"] };
			dataTable.Columns.Add("CreatedDate", typeof(DateTime));
			dataTable.Columns.Add("Heading", typeof(string));
			dataTable.Columns.Add("SubTitle", typeof(string));
			dataTable.Columns.Add("StartDate", typeof(DateTime));
			dataTable.Columns.Add("EndDate", typeof(DateTime));
			dataTable.Columns.Add("Description", typeof(string));
			dataTable.Columns.Add("ItemUrl", typeof(string));
			dataTable.Columns.Add("Location", typeof(string));
			dataTable.Columns.Add("MetaKeywords", typeof(string));
			dataTable.Columns.Add("MetaDescription", typeof(string));
			dataTable.Columns.Add("Abstract", typeof(string));
			dataTable.Columns.Add("LastModUtc", typeof(DateTime));
			dataTable.Columns.Add("IsPublished", typeof(bool));
			dataTable.Columns.Add("IncludeInFeed", typeof(bool));
			dataTable.Columns.Add("CommentCount", typeof(int));
			dataTable.Columns.Add("AllowCommentsForDays", typeof(int));
			dataTable.Columns.Add("UserID", typeof(int));
			dataTable.Columns.Add("Name", typeof(string));
			dataTable.Columns.Add("FirstName", typeof(string));
			dataTable.Columns.Add("LastName", typeof(string));
			dataTable.Columns.Add("LoginName", typeof(string));
			dataTable.Columns.Add("Email", typeof(string));
			dataTable.Columns.Add("AvatarUrl", typeof(string));
			dataTable.Columns.Add("AuthorBio", typeof(string));

			//below added 2012-12-06
			dataTable.Columns.Add("ShowAuthorName", typeof(bool));
			dataTable.Columns.Add("ShowAuthorAvatar", typeof(bool));
			dataTable.Columns.Add("ShowAuthorBio", typeof(bool));
			dataTable.Columns.Add("UseBingMap", typeof(bool));
			dataTable.Columns.Add("MapHeight", typeof(string));
			dataTable.Columns.Add("MapWidth", typeof(string));
			dataTable.Columns.Add("ShowMapOptions", typeof(bool));
			dataTable.Columns.Add("ShowZoomTool", typeof(bool));
			dataTable.Columns.Add("ShowLocationInfo", typeof(bool));
			dataTable.Columns.Add("UseDrivingDirections", typeof(bool));
			dataTable.Columns.Add("MapType", typeof(string));
			dataTable.Columns.Add("MapZoom", typeof(int));
			dataTable.Columns.Add("ShowDownloadLink", typeof(bool));
			// added 2014-02-04
			dataTable.Columns.Add("HeadlineImageUrl", typeof(string));
			dataTable.Columns.Add("IncludeImageInExcerpt", typeof(bool));
			dataTable.Columns.Add("IncludeImageInPost", typeof(bool));

			return dataTable;

		}

		public static DataSet GetPageDataSet(
			int moduleId,
			DateTime beginDate,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			DataSet dataSet = new DataSet();

			DataTable posts = GetPostsTableStructure();

			using (IDataReader reader = GetPage(moduleId, beginDate, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					DataRow row = posts.NewRow();


					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					row["BlogGuid"] = reader["BlogGuid"].ToString();
					row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					row["Heading"] = reader["Heading"];
					row["SubTitle"] = reader["SubTitle"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					row["Description"] = reader["Description"];
					row["Abstract"] = reader["Abstract"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Location"] = reader["Location"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["MetaDescription"] = reader["MetaDescription"];
					row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["IsPublished"] = true;
					row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					row["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);

					if (reader["AllowCommentsForDays"] != DBNull.Value)
					{
						row["AllowCommentsForDays"] = reader["AllowCommentsForDays"];
					}

					row["UserID"] = Convert.ToInt32(reader["UserID"]);
					row["Name"] = reader["Name"];
					row["FirstName"] = reader["FirstName"];
					row["LastName"] = reader["LastName"];
					row["LoginName"] = reader["LoginName"];
					row["Email"] = reader["Email"];
					row["AvatarUrl"] = reader["AvatarUrl"];
					row["AuthorBio"] = reader["AuthorBio"];

					// added 2012-12-06
					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						row["ShowAuthorName"] = Convert.ToBoolean(reader["ShowAuthorName"]);
					}
					else
					{
						row["ShowAuthorName"] = true;
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						row["ShowAuthorAvatar"] = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}
					else
					{
						row["ShowAuthorAvatar"] = true;
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						row["ShowAuthorBio"] = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}
					else
					{
						row["ShowAuthorBio"] = true;
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						row["UseBingMap"] = Convert.ToBoolean(reader["UseBingMap"]);
					}
					else
					{
						row["UseBingMap"] = false;
					}

					row["MapHeight"] = reader["MapHeight"];
					row["MapWidth"] = reader["MapWidth"];
					row["MapType"] = reader["MapType"];

					if (reader["MapZoom"] != DBNull.Value)
					{
						row["MapZoom"] = Convert.ToInt32(reader["MapZoom"]);
					}
					else
					{
						row["MapZoom"] = 13;
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						row["ShowMapOptions"] = Convert.ToBoolean(reader["ShowMapOptions"]);
					}
					else
					{
						row["ShowMapOptions"] = false;
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						row["ShowZoomTool"] = Convert.ToBoolean(reader["ShowZoomTool"]);
					}
					else
					{
						row["ShowZoomTool"] = false;
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						row["ShowLocationInfo"] = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}
					else
					{
						row["ShowLocationInfo"] = false;
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						row["UseDrivingDirections"] = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}
					else
					{
						row["UseDrivingDirections"] = false;
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}


					row["HeadlineImageUrl"] = reader["HeadlineImageUrl"];

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						row["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}
					else
					{
						row["IncludeImageInExcerpt"] = true;
					}


					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						row["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
					else
					{
						row["IncludeImageInPost"] = true;
					}

					posts.Rows.Add(row);
				}
			}

			posts.TableName = "Posts";
			dataSet.Tables.Add(posts);

			DataTable categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			DataTable attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = attachments.NewRow();

					row["RowGuid"] = reader["RowGuid"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["FileName"] = reader["FileName"];
					row["ServerFileName"] = reader["ServerFileName"];
					row["ContentLength"] = Convert.ToInt64(reader["ContentLength"]);
					row["ContentType"] = reader["ContentType"];

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					attachments.Rows.Add(row);
				}

			}

			attachments.TableName = "Attachments";
			dataSet.Tables.Add(attachments);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postCategories",
					dataSet.Tables["Posts"].Columns["ItemID"],
					dataSet.Tables["Categories"].Columns["ItemID"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }

			try
			{
				dataSet.Relations.Add("postAttachments",
					dataSet.Tables["Posts"].Columns["BlogGuid"],
					dataSet.Tables["Attachments"].Columns["ItemGuid"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }


			return dataSet;
		}

		public static DataSet GetBlogEntriesByMonth(
			int month,
			int year,
			int moduleId,
			DateTime currentTime,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			DataSet dataSet = new DataSet();

			DataTable posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetBlogEntriesByMonth(month, year, moduleId, currentTime, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					DataRow row = posts.NewRow();


					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					row["BlogGuid"] = reader["BlogGuid"].ToString();
					row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					row["Heading"] = reader["Heading"];
					row["SubTitle"] = reader["SubTitle"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					row["Description"] = reader["Description"];
					row["Abstract"] = reader["Abstract"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Location"] = reader["Location"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["MetaDescription"] = reader["MetaDescription"];
					row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["IsPublished"] = true;
					row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					row["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					row["UserID"] = Convert.ToInt32(reader["UserID"]);
					row["Name"] = reader["Name"];
					row["FirstName"] = reader["FirstName"];
					row["LastName"] = reader["LastName"];
					row["LoginName"] = reader["LoginName"];
					row["Email"] = reader["Email"];
					row["AvatarUrl"] = reader["AvatarUrl"];
					row["AuthorBio"] = reader["AuthorBio"];

					// added 2012-12-06
					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						row["ShowAuthorName"] = Convert.ToBoolean(reader["ShowAuthorName"]);
					}
					else
					{
						row["ShowAuthorName"] = true;
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						row["ShowAuthorAvatar"] = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}
					else
					{
						row["ShowAuthorAvatar"] = true;
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						row["ShowAuthorBio"] = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}
					else
					{
						row["ShowAuthorBio"] = true;
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						row["UseBingMap"] = Convert.ToBoolean(reader["UseBingMap"]);
					}
					else
					{
						row["UseBingMap"] = false;
					}

					row["MapHeight"] = reader["MapHeight"];
					row["MapWidth"] = reader["MapWidth"];
					row["MapType"] = reader["MapType"];

					if (reader["MapZoom"] != DBNull.Value)
					{
						row["MapZoom"] = Convert.ToInt32(reader["MapZoom"]);
					}
					else
					{
						row["MapZoom"] = 13;
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						row["ShowMapOptions"] = Convert.ToBoolean(reader["ShowMapOptions"]);
					}
					else
					{
						row["ShowMapOptions"] = false;
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						row["ShowZoomTool"] = Convert.ToBoolean(reader["ShowZoomTool"]);
					}
					else
					{
						row["ShowZoomTool"] = false;
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						row["ShowLocationInfo"] = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}
					else
					{
						row["ShowLocationInfo"] = false;
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						row["UseDrivingDirections"] = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}
					else
					{
						row["UseDrivingDirections"] = false;
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					row["HeadlineImageUrl"] = reader["HeadlineImageUrl"];

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						row["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}
					else
					{
						row["IncludeImageInExcerpt"] = true;
					}


					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						row["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
					else
					{
						row["IncludeImageInPost"] = true;
					}


					posts.Rows.Add(row);
				}
			}

			posts.TableName = "Posts";
			dataSet.Tables.Add(posts);

			DataTable categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(month, year, moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postCategories",
					dataSet.Tables["Posts"].Columns["ItemID"],
					dataSet.Tables["Categories"].Columns["ItemID"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }

			DataTable attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(month, year, moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{

					DataRow row = attachments.NewRow();

					row["RowGuid"] = reader["RowGuid"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["FileName"] = reader["FileName"];
					row["ServerFileName"] = reader["ServerFileName"];
					row["ContentLength"] = Convert.ToInt64(reader["ContentLength"]);
					row["ContentType"] = reader["ContentType"];

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					attachments.Rows.Add(row);
				}

			}

			attachments.TableName = "Attachments";
			dataSet.Tables.Add(attachments);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postAttachments",
					dataSet.Tables["Posts"].Columns["BlogGuid"],
					dataSet.Tables["Attachments"].Columns["ItemGuid"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }


			return dataSet;
		}

		public static DataSet GetBlogEntriesByCategory(
			int moduleId,
			int categoryId,
			DateTime currentTime,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			DataSet dataSet = new DataSet();

			DataTable posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetEntriesByCategory(moduleId, categoryId, currentTime, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					DataRow row = posts.NewRow();


					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					row["BlogGuid"] = reader["BlogGuid"].ToString();
					row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					row["Heading"] = reader["Heading"];
					row["SubTitle"] = reader["SubTitle"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					row["Description"] = reader["Description"];
					row["Abstract"] = reader["Abstract"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Location"] = reader["Location"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["MetaDescription"] = reader["MetaDescription"];
					row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["IsPublished"] = true;
					row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					row["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					row["UserID"] = Convert.ToInt32(reader["UserID"]);
					row["Name"] = reader["Name"];
					row["FirstName"] = reader["FirstName"];
					row["LastName"] = reader["LastName"];
					row["LoginName"] = reader["LoginName"];
					row["Email"] = reader["Email"];
					row["AvatarUrl"] = reader["AvatarUrl"];
					row["AuthorBio"] = reader["AuthorBio"];

					// added 2012-12-06
					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						row["ShowAuthorName"] = Convert.ToBoolean(reader["ShowAuthorName"]);
					}
					else
					{
						row["ShowAuthorName"] = true;
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						row["ShowAuthorAvatar"] = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}
					else
					{
						row["ShowAuthorAvatar"] = true;
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						row["ShowAuthorBio"] = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}
					else
					{
						row["ShowAuthorBio"] = true;
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						row["UseBingMap"] = Convert.ToBoolean(reader["UseBingMap"]);
					}
					else
					{
						row["UseBingMap"] = false;
					}

					row["MapHeight"] = reader["MapHeight"];
					row["MapWidth"] = reader["MapWidth"];
					row["MapType"] = reader["MapType"];

					if (reader["MapZoom"] != DBNull.Value)
					{
						row["MapZoom"] = Convert.ToInt32(reader["MapZoom"]);
					}
					else
					{
						row["MapZoom"] = 13;
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						row["ShowMapOptions"] = Convert.ToBoolean(reader["ShowMapOptions"]);
					}
					else
					{
						row["ShowMapOptions"] = false;
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						row["ShowZoomTool"] = Convert.ToBoolean(reader["ShowZoomTool"]);
					}
					else
					{
						row["ShowZoomTool"] = false;
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						row["ShowLocationInfo"] = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}
					else
					{
						row["ShowLocationInfo"] = false;
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						row["UseDrivingDirections"] = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}
					else
					{
						row["UseDrivingDirections"] = false;
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					row["HeadlineImageUrl"] = reader["HeadlineImageUrl"];

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						row["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}
					else
					{
						row["IncludeImageInExcerpt"] = true;
					}


					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						row["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
					else
					{
						row["IncludeImageInPost"] = true;
					}


					posts.Rows.Add(row);
				}
			}

			posts.TableName = "Posts";
			dataSet.Tables.Add(posts);

			DataTable categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(moduleId, categoryId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postCategories",
					dataSet.Tables["Posts"].Columns["ItemID"],
					dataSet.Tables["Categories"].Columns["ItemID"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }

			DataTable attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(moduleId, categoryId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = attachments.NewRow();

					row["RowGuid"] = reader["RowGuid"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["FileName"] = reader["FileName"];
					row["ServerFileName"] = reader["ServerFileName"];
					row["ContentLength"] = Convert.ToInt64(reader["ContentLength"]);
					row["ContentType"] = reader["ContentType"];

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					attachments.Rows.Add(row);
				}

			}

			attachments.TableName = "Attachments";
			dataSet.Tables.Add(attachments);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postAttachments",
					dataSet.Tables["Posts"].Columns["BlogGuid"],
					dataSet.Tables["Attachments"].Columns["ItemGuid"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }


			return dataSet;
		}

		public static DataSet GetClosedDataSet(
			int moduleId,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			DataSet dataSet = new DataSet();

			DataTable posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					DataRow row = posts.NewRow();


					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
					row["BlogGuid"] = reader["BlogGuid"].ToString();
					row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
					row["Heading"] = reader["Heading"];
					row["SubTitle"] = reader["SubTitle"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);
					row["EndDate"] = Convert.ToDateTime(reader["EndDate"]);
					row["Description"] = reader["Description"];
					row["Abstract"] = reader["Abstract"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Location"] = reader["Location"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["MetaDescription"] = reader["MetaDescription"];
					row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					row["IsPublished"] = true;
					row["IncludeInFeed"] = Convert.ToBoolean(reader["IncludeInFeed"]);
					row["CommentCount"] = Convert.ToInt32(reader["CommentCount"]);
					row["UserID"] = Convert.ToInt32(reader["UserID"]);
					row["Name"] = reader["Name"];
					row["FirstName"] = reader["FirstName"];
					row["LastName"] = reader["LastName"];
					row["LoginName"] = reader["LoginName"];
					row["Email"] = reader["Email"];
					row["AvatarUrl"] = reader["AvatarUrl"];
					row["AuthorBio"] = reader["AuthorBio"];

					// added 2012-12-06
					if (reader["ShowAuthorName"] != DBNull.Value)
					{
						row["ShowAuthorName"] = Convert.ToBoolean(reader["ShowAuthorName"]);
					}
					else
					{
						row["ShowAuthorName"] = true;
					}

					if (reader["ShowAuthorAvatar"] != DBNull.Value)
					{
						row["ShowAuthorAvatar"] = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
					}
					else
					{
						row["ShowAuthorAvatar"] = true;
					}

					if (reader["ShowAuthorBio"] != DBNull.Value)
					{
						row["ShowAuthorBio"] = Convert.ToBoolean(reader["ShowAuthorBio"]);
					}
					else
					{
						row["ShowAuthorBio"] = true;
					}

					if (reader["UseBingMap"] != DBNull.Value)
					{
						row["UseBingMap"] = Convert.ToBoolean(reader["UseBingMap"]);
					}
					else
					{
						row["UseBingMap"] = false;
					}

					row["MapHeight"] = reader["MapHeight"];
					row["MapWidth"] = reader["MapWidth"];
					row["MapType"] = reader["MapType"];

					if (reader["MapZoom"] != DBNull.Value)
					{
						row["MapZoom"] = Convert.ToInt32(reader["MapZoom"]);
					}
					else
					{
						row["MapZoom"] = 13;
					}

					if (reader["ShowMapOptions"] != DBNull.Value)
					{
						row["ShowMapOptions"] = Convert.ToBoolean(reader["ShowMapOptions"]);
					}
					else
					{
						row["ShowMapOptions"] = false;
					}

					if (reader["ShowZoomTool"] != DBNull.Value)
					{
						row["ShowZoomTool"] = Convert.ToBoolean(reader["ShowZoomTool"]);
					}
					else
					{
						row["ShowZoomTool"] = false;
					}

					if (reader["ShowLocationInfo"] != DBNull.Value)
					{
						row["ShowLocationInfo"] = Convert.ToBoolean(reader["ShowLocationInfo"]);
					}
					else
					{
						row["ShowLocationInfo"] = false;
					}

					if (reader["UseDrivingDirections"] != DBNull.Value)
					{
						row["UseDrivingDirections"] = Convert.ToBoolean(reader["UseDrivingDirections"]);
					}
					else
					{
						row["UseDrivingDirections"] = false;
					}

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					row["HeadlineImageUrl"] = reader["HeadlineImageUrl"];

					if (reader["IncludeImageInExcerpt"] != DBNull.Value)
					{
						row["IncludeImageInExcerpt"] = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
					}
					else
					{
						row["IncludeImageInExcerpt"] = true;
					}


					if (reader["IncludeImageInPost"] != DBNull.Value)
					{
						row["IncludeImageInPost"] = Convert.ToBoolean(reader["IncludeImageInPost"]);
					}
					else
					{
						row["IncludeImageInPost"] = true;
					}


					posts.Rows.Add(row);
				}
			}

			posts.TableName = "Posts";
			dataSet.Tables.Add(posts);

			DataTable categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			DataTable attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					DataRow row = attachments.NewRow();

					row["RowGuid"] = reader["RowGuid"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["FileName"] = reader["FileName"];
					row["ServerFileName"] = reader["ServerFileName"];
					row["ContentLength"] = Convert.ToInt64(reader["ContentLength"]);
					row["ContentType"] = reader["ContentType"];

					if (reader["ShowDownloadLink"] != DBNull.Value)
					{
						row["ShowDownloadLink"] = Convert.ToBoolean(reader["ShowDownloadLink"]);
					}
					else
					{
						row["ShowDownloadLink"] = false;
					}

					attachments.Rows.Add(row);
				}

			}

			attachments.TableName = "Attachments";
			dataSet.Tables.Add(attachments);

			// create a releationship
			try
			{
				dataSet.Relations.Add("postCategories",
					dataSet.Tables["Posts"].Columns["ItemID"],
					dataSet.Tables["Categories"].Columns["ItemID"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }

			try
			{
				dataSet.Relations.Add("postAttachments",
					dataSet.Tables["Posts"].Columns["BlogGuid"],
					dataSet.Tables["Attachments"].Columns["ItemGuid"]);
			}
			catch (System.Data.ConstraintException) { }
			catch (ArgumentException) { }


			return dataSet;
		}

		public static IDataReader GetPage(
			int moduleId,
			DateTime beginDate,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			return DBBlog.GetPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize, out totalPages);
		}


		public static IDataReader GetBlogsForSiteMap(int siteId)
		{
			return DBBlog.GetBlogsForSiteMap(siteId, DateTime.UtcNow);
		}

		public static IDataReader GetBlogsForNewsMap(int siteId, int hoursOld)
		{
			return DBBlog.GetBlogsForNewsMap(siteId, DateTime.UtcNow.AddHours(-hoursOld)); // google requires that only items published in the last 2 days are in news map
		}

		/// <summary>
		/// Gets unpublished blog posts
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public static IDataReader GetDrafts(int moduleId)
		{
			return DBBlog.GetDrafts(moduleId);
		}

		public static IDataReader GetPageOfDrafts(
			int moduleId,
			Guid userGuid,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			return DBBlog.GetPageOfDrafts(
				moduleId,
				userGuid,
				DateTime.UtcNow,
				pageNumber,
				pageSize,
				out totalPages);
		}

		public static int GetCountOfDrafts(
			int moduleId,
			Guid userGuid)
		{
			return DBBlog.GetCountOfDrafts(moduleId, userGuid, DateTime.UtcNow);
		}

		public static int GetCountClosed(
			int moduleId)
		{
			return DBBlog.GetCountClosed(moduleId, DateTime.UtcNow);
		}

		//public static int CountOfDrafts(int moduleId)
		//{
		//    int result = 0;

		//    using (IDataReader reader = GetDrafts(moduleId))
		//    {
		//        while (reader.Read())
		//        {
		//            result += 1;
		//        }
		//    }


		//    return result;

		//}

		public static bool DeleteByModule(int moduleId)
		{
			return DBBlog.DeleteByModule(moduleId);
		}

		public static bool DeleteBySite(int siteId)
		{
			return DBBlog.DeleteBySite(siteId);
		}


		public static IDataReader GetBlogStats(int moduleId)
		{
			return DBBlog.GetBlogStats(moduleId);
		}

		/// <summary>
		/// can get up to 20 releated items, limited further by maxToGet
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="maxToGet"></param>
		/// <returns></returns>
		public static DataTable GetRelatedPosts(int itemId, int maxToGet)
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.Columns.Add("Heading", typeof(string));
			dataTable.Columns.Add("ItemUrl", typeof(string));


			int counter = 1;

			using (IDataReader reader = DBBlog.GetRelatedPosts(itemId))
			{
				while (reader.Read())
				{
					if (counter > maxToGet) { continue; }

					DataRow row = dataTable.NewRow();
					row["ItemID"] = Convert.ToInt32(reader["ItemID"]);
					row["Heading"] = reader["Heading"];
					row["ItemUrl"] = reader["ItemUrl"];
					dataTable.Rows.Add(row);

					counter += 1;
				}

			}

			return dataTable;
		}

		public static IDataReader GetBlogEntriesByMonth(int month, int year, int moduleId)
		{
			return DBBlog.GetBlogEntriesByMonth(month, year, moduleId, DateTime.UtcNow);
		}

		public static IDataReader GetBlogMonthArchive(int moduleId)
		{
			return DBBlog.GetBlogMonthArchive(moduleId, DateTime.UtcNow);
		}

		public static IDataReader GetSingleBlog(int itemId)
		{
			return DBBlog.GetSingleBlog(itemId, DateTime.UtcNow);
		}



		//public static bool DeleteBlog(int itemID) 
		//{
		//    //TODO: make instance method to support ContentChanged event

		//    Blog blog = new Blog(itemID);
		//    bool result = dbBlog.Blog_DeleteBlog(itemID);
		//    if (result)
		//    {
		//        result = dbBlog.Blog_DeleteItemCategories(itemID);
		//        //IndexHelper.RemoveIndexItem(blog.ModuleID, itemID);
		//    }

		//    return result;
		//}

		public static IDataReader GetBlogComments(int moduleId, int itemId)
		{
			return DBBlog.GetBlogComments(moduleId, itemId);
		}

		public static DataTable GetBlogCommentsTable(int moduleId, int itemId)
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("Comment", typeof(string));
			dataTable.Columns.Add("Name", typeof(string));

			using (IDataReader reader = DBBlog.GetBlogComments(moduleId, itemId))
			{
				while (reader.Read())
				{
					DataRow row = dataTable.NewRow();

					row["Comment"] = reader["Comment"];
					row["Name"] = reader["Name"];

					dataTable.Rows.Add(row);
				}
			}

			return dataTable;
		}

		public static bool AddBlogComment(
			int moduleId,
			int itemId,
			String name,
			String title,
			String url,
			String comment,
			DateTime dateCreated)
		{
			if (name == null)
			{
				name = "unknown";
			}
			if (name.Length < 1)
			{
				name = "unknown";
			}

			if ((title != null) && (url != null) && (comment != null))
			{
				if (title.Length > 100)
				{
					title = title.Substring(0, 100);
				}

				if (name.Length > 100)
				{
					name = name.Substring(0, 100);
				}

				if (url.Length > 200)
				{
					url = url.Substring(0, 200);
				}

				return DBBlog.AddBlogComment(
					moduleId,
					itemId,
					name,
					title,
					url,
					comment,
					dateCreated);
			}

			return false;
		}

		public static bool UpdateCommentCount(Guid blogGuid, int commentCount)
		{
			return DBBlog.UpdateCommentCount(blogGuid, commentCount);
		}

		public static bool DeleteBlogComment(int commentId)
		{
			return DBBlog.DeleteBlogComment(commentId);
		}


		public static int AddBlogCategory(int moduleId, string category)
		{
			return DBBlog.AddBlogCategory(moduleId, category);
		}

		public static bool UpdateBlogCategory(
			int categoryId,
			string category)
		{
			return DBBlog.UpdateBlogCategory(categoryId, category);

		}


		public static bool DeleteCategory(int categoryId)
		{
			return DBBlog.DeleteCategory(categoryId);
		}


		public static IDataReader GetCategories(int moduleId)
		{
			return DBBlog.GetCategories(moduleId);
		}

		public static IDataReader GetCategoriesList(int moduleId)
		{
			return DBBlog.GetCategoriesList(moduleId);
		}


		public static int AddItemCategory(
			int itemId,
			int categoryId)
		{
			return DBBlog.AddBlogItemCategory(itemId, categoryId);
		}

		public static bool DeleteItemCategories(int itemId)
		{
			return DBBlog.DeleteItemCategories(itemId);
		}

		public static IDataReader GetItemCategories(int itemId)
		{
			return DBBlog.GetBlogItemCategories(itemId);
		}

		public static IDataReader GetEntriesByCategory(int moduleId, int categoryId)
		{
			return DBBlog.GetEntriesByCategory(moduleId, categoryId, DateTime.UtcNow);
		}

		public static IDataReader GetCategory(int categoryId)
		{
			return DBBlog.GetCategory(categoryId);
		}



		public static DataTable GetBlogsByPage(int siteId, int pageId)
		{
			DataTable dataTable = new DataTable();

			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.Columns.Add("ModuleID", typeof(int));
			dataTable.Columns.Add("CommentCount", typeof(int));
			dataTable.Columns.Add("ModuleTitle", typeof(string));
			dataTable.Columns.Add("Heading", typeof(string));
			dataTable.Columns.Add("Abstract", typeof(string));
			dataTable.Columns.Add("ItemUrl", typeof(string));
			dataTable.Columns.Add("Description", typeof(string));
			dataTable.Columns.Add("StartDate", typeof(DateTime));
			dataTable.Columns.Add("EndDate", typeof(DateTime));
			dataTable.Columns.Add("LastModUtc", typeof(DateTime));
			dataTable.Columns.Add("MetaDescription", typeof(string));
			dataTable.Columns.Add("MetaKeywords", typeof(string));
			dataTable.Columns.Add("ViewRoles", typeof(string));
			dataTable.Columns.Add("IncludeInSearch", typeof(bool));
			dataTable.Columns.Add("ExcludeFromRecentContent", typeof(bool));

			dataTable.Columns.Add("Name", typeof(string));
			dataTable.Columns.Add("FirstName", typeof(string));
			dataTable.Columns.Add("LastName", typeof(string));

			using (IDataReader reader = DBBlog.GetBlogsByPage(siteId, pageId))
			{
				while (reader.Read())
				{
					DataRow row = dataTable.NewRow();

					row["ItemID"] = reader["ItemID"];
					row["ModuleID"] = reader["ModuleID"];
					row["CommentCount"] = reader["CommentCount"];
					row["ModuleTitle"] = reader["ModuleTitle"];
					row["Heading"] = reader["Heading"];
					row["Abstract"] = reader["Abstract"];
					row["ItemUrl"] = reader["ItemUrl"];
					row["Description"] = reader["Description"];
					row["StartDate"] = Convert.ToDateTime(reader["StartDate"]);

					row["Name"] = reader["Name"];
					row["FirstName"] = reader["FirstName"];
					row["LastName"] = reader["LastName"];

					if (reader["EndDate"] != DBNull.Value)
					{
						row["EndDate"] = Convert.ToDateTime(reader["EndDate"]);
					}
					else
					{
						row["EndDate"] = DateTime.MaxValue;
					}

					if (reader["LastModUtc"] != DBNull.Value)
					{
						row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
					}
					else
					{
						row["LastModUtc"] = Convert.ToDateTime(reader["StartDate"]);
					}

					row["MetaDescription"] = reader["MetaDescription"];
					row["MetaKeywords"] = reader["MetaKeywords"];
					row["ViewRoles"] = reader["ViewRoles"];
					if (reader["IncludeInSearch"] != DBNull.Value)
					{
						row["IncludeInSearch"] = Convert.ToBoolean(reader["IncludeInSearch"]);
					}
					else
					{
						row["IncludeInSearch"] = true;
					}

					if (reader["ExcludeFromRecentContent"] != DBNull.Value)
					{
						row["ExcludeFromRecentContent"] = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);
					}
					else
					{
						row["ExcludeFromRecentContent"] = false;
					}



					dataTable.Rows.Add(row);
				}
			}

			return dataTable;
		}



		#endregion

		#region IIndexableContent

		public event ContentChangedEventHandler ContentChanged;

		protected void OnContentChanged(ContentChangedEventArgs e)
		{
			if (ContentChanged != null)
			{
				ContentChanged(this, e);
			}
		}




		#endregion




	}
}