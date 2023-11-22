//using log4net;
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
		public static Guid FeatureGuid => Guid.Parse("026cbead-2b80-4491-906d-b83e37179ccf");

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

		//private static readonly ILog log = LogManager.GetLogger(typeof(Blog));

		#region Public Properties

		public Guid BlogGuid { get; set; } = Guid.Empty;

		public Guid ModuleGuid { get; set; } = Guid.Empty;

		public int ItemId { get; private set; } = -1;

		public int PreviousItemId { get; private set; } = -1;

		public int NextItemId { get; private set; } = -1;

		public int ModuleId { get; set; } = -1;

		public string UserName { get; set; } = string.Empty;
		
		public Guid UserGuid { get; set; } = Guid.Empty;
		
		public Guid LastModUserGuid { get; set; } = Guid.Empty;

		public string Title { get; set; } = string.Empty;

		public string SubTitle { get; set; } = string.Empty;

		public string Category { get; set; } = string.Empty;

		public string Excerpt { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string Location { get; set; } = string.Empty;

		public string MetaKeywords { get; set; } = string.Empty;

		public string MetaDescription { get; set; } = string.Empty;

		public string CompiledMeta { get; set; } = string.Empty;

		public DateTime StartDate { get; set; } = DateTime.UtcNow;

		public DateTime EndDate { get; set; } = DateTime.MaxValue;

		public bool Approved { get; set; } = true;

		public Guid ApprovedBy { get; set; } = Guid.Empty;

		public DateTime ApprovedDate { get; set; } = DateTime.MaxValue;

		public bool IsPublished { get; set; } = true;

		public bool IsInNewsletter { get; set; } = true;

		public bool IncludeInFeed { get; set; } = true;

		public int AllowCommentsForDays { get; set; } = 60;

		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

		public DateTime LastModUtc { get; set; } = DateTime.UtcNow;

		public string ItemUrl { get; set; } = string.Empty;

		public bool ShowAuthorName { get; set; } = true;

		public bool ShowAuthorAvatar { get; set; } = true;

		public bool ShowAuthorBio { get; set; } = true;

		public bool IncludeInSearch { get; set; } = true;

		public bool UseBingMap { get; set; } = false;

		public string MapHeight { get; set; } = "300";

		public string MapWidth { get; set; } = "500";

		public bool ShowMapOptions { get; set; } = false;

		public bool ShowZoomTool { get; set; } = false;

		public bool ShowLocationInfo { get; set; } = false;

		public bool UseDrivingDirections { get; set; } = false;

		public string MapType { get; set; } = "G_SATELLITE_MAP";

		public int MapZoom { get; set; } = 13;

		public bool ShowDownloadLink { get; set; } = false;

		public bool IncludeInSiteMap { get; set; } = true;

		public bool ExcludeFromRecentContent { get; set; } = false;

		public string PreviousPostUrl { get; private set; } = string.Empty;

		public string NextPostUrl { get; private set; } = string.Empty;

		public string PreviousPostTitle { get; private set; } = string.Empty;

		public string NextPostTitle { get; private set; } = string.Empty;

		public int CommentCount { get; private set; } = 0;

		public int UserId { get; private set; } = -1;

		public string UserLoginName { get; private set; } = string.Empty;

		public string UserDisplayName { get; private set; } = string.Empty;

		public string UserFirstName { get; private set; } = string.Empty;

		public string UserLastName { get; private set; } = string.Empty;

		public string UserEmail { get; private set; } = string.Empty;

		public string UserAvatar { get; private set; } = string.Empty;

		public string AuthorBio { get; private set; } = string.Empty;

		public bool IncludeInNews { get; set; } = false;

		public string PubName { get; set; } = string.Empty;

		public string PubLanguage { get; set; } = string.Empty;

		public string PubAccess { get; set; } = string.Empty;

		public string PubGenres { get; set; } = string.Empty;

		public string PubKeyWords { get; set; } = string.Empty;

		public string PubGeoLocations { get; set; } = string.Empty;

		public string PubStockTickers { get; set; } = string.Empty;

		public string HeadlineImageUrl { get; set; } = string.Empty;

		public bool IncludeImageInExcerpt { get; set; } = true;

		public bool IncludeImageInPost { get; set; } = true;


		/// <summary>
		/// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
		/// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
		/// So we store extra properties here so we don't need any other objects.
		/// </summary>
		public int SiteId { get; set; } = -1;

		/// <summary>
		/// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
		/// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
		/// So we store extra properties here so we don't need any other objects.
		/// </summary>
		public string SearchIndexPath { get; set; } = string.Empty;


		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the blog.
		/// </summary>
		/// <param name="itemID">The item ID.</param>
		private void GetBlog(int itemId)
		{
			using IDataReader reader = DBBlog.GetSingleBlog(itemId, DateTime.UtcNow);
			if (reader.Read())
			{
				this.ItemId = Convert.ToInt32(reader["ItemID"].ToString(), CultureInfo.InvariantCulture);
				this.ModuleId = Convert.ToInt32(reader["ModuleID"].ToString(), CultureInfo.InvariantCulture);
				this.UserName = reader["Name"].ToString();
				this.Title = reader["Heading"].ToString();
				this.Excerpt = reader["Abstract"].ToString();
				this.Description = reader["Description"].ToString();

				this.MetaKeywords = reader["MetaKeywords"].ToString();
				this.MetaDescription = reader["MetaDescription"].ToString();

				this.StartDate = Convert.ToDateTime(reader["StartDate"].ToString());

				// this is to support dbs that don't have bit data type
				//string inNews = reader["IsInNewsletter"].ToString();
				//this.isInNewsletter = (inNews == "True" || inNews == "1");

				this.IsInNewsletter = Convert.ToBoolean(reader["IsInNewsletter"]);

				//string inFeed = reader["IncludeInFeed"].ToString();
				//this.includeInFeed = (inFeed == "True" || inFeed == "1");

				this.IncludeInFeed = Convert.ToBoolean(reader["IncludeInFeed"]);

				if (reader["AllowCommentsForDays"] != DBNull.Value)
				{
					this.AllowCommentsForDays = Convert.ToInt32(reader["AllowCommentsForDays"]);
				}

				this.BlogGuid = new Guid(reader["BlogGuid"].ToString());
				this.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
				this.Location = reader["Location"].ToString();
				this.CompiledMeta = reader["CompiledMeta"].ToString();

				if (reader["CreatedDate"] != DBNull.Value)
				{
					this.CreatedUtc = Convert.ToDateTime(reader["CreatedDate"]);
				}

				if (reader["LastModUtc"] != DBNull.Value)
				{
					this.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
				}

				string var = reader["UserGuid"].ToString();
				if (var.Length == 36) this.UserGuid = new Guid(var);

				var = reader["LastModUserGuid"].ToString();
				if (var.Length == 36) this.LastModUserGuid = new Guid(var);

				ItemUrl = reader["ItemUrl"].ToString();

				PreviousPostUrl = reader["PreviousPost"].ToString();
				PreviousPostTitle = reader["PreviousPostTitle"].ToString();
				NextPostUrl = reader["NextPost"].ToString();
				NextPostTitle = reader["NextPostTitle"].ToString();

				CommentCount = Convert.ToInt32(reader["CommentCount"]);

				IsPublished = Convert.ToBoolean(reader["IsPublished"]);

				this.PreviousItemId = Convert.ToInt32(reader["PreviousItemID"].ToString(), CultureInfo.InvariantCulture);
				this.NextItemId = Convert.ToInt32(reader["NextItemID"].ToString(), CultureInfo.InvariantCulture);

				this.UserId = Convert.ToInt32(reader["UserID"]);
				this.UserDisplayName = reader["Name"].ToString();
				this.UserLoginName = reader["LoginName"].ToString();
				this.UserFirstName = reader["FirstName"].ToString();
				this.UserLastName = reader["LastName"].ToString();
				this.UserEmail = reader["Email"].ToString();
				this.UserAvatar = reader["AvatarUrl"].ToString();
				this.AuthorBio = reader["AuthorBio"].ToString();

				this.SubTitle = reader["SubTitle"].ToString();

				if (reader["EndDate"] != DBNull.Value)
				{
					this.EndDate = Convert.ToDateTime(reader["EndDate"]);
				}

				if (reader["ApprovedDate"] != DBNull.Value)
				{
					this.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
				}

				if (reader["ApprovedBy"] != DBNull.Value)
				{
					var = reader["ApprovedBy"].ToString();
					if (var.Length == 36) ApprovedBy = new Guid(var);
				}

				if (reader["Approved"] != DBNull.Value)
				{
					this.Approved = Convert.ToBoolean(reader["Approved"]);
				}


				// below added 2012-12-06

				if (reader["ShowAuthorName"] != DBNull.Value)
				{
					this.ShowAuthorName = Convert.ToBoolean(reader["ShowAuthorName"]);
				}

				if (reader["ShowAuthorAvatar"] != DBNull.Value)
				{
					this.ShowAuthorAvatar = Convert.ToBoolean(reader["ShowAuthorAvatar"]);
				}

				if (reader["ShowAuthorBio"] != DBNull.Value)
				{
					this.ShowAuthorBio = Convert.ToBoolean(reader["ShowAuthorBio"]);
				}

				if (reader["IncludeInSearch"] != DBNull.Value)
				{
					this.IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
				}

				if (reader["IncludeInSiteMap"] != DBNull.Value)
				{
					this.IncludeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
				}

				if (reader["UseBingMap"] != DBNull.Value)
				{
					this.UseBingMap = Convert.ToBoolean(reader["UseBingMap"]);
				}

				if (reader["MapHeight"] != DBNull.Value)
				{
					this.MapHeight = reader["MapHeight"].ToString();
				}

				if (reader["MapWidth"] != DBNull.Value)
				{
					this.MapWidth = reader["MapWidth"].ToString();
				}

				if (reader["ShowMapOptions"] != DBNull.Value)
				{
					this.ShowMapOptions = Convert.ToBoolean(reader["ShowMapOptions"]);
				}

				if (reader["ShowZoomTool"] != DBNull.Value)
				{
					this.ShowZoomTool = Convert.ToBoolean(reader["ShowZoomTool"]);
				}

				if (reader["ShowLocationInfo"] != DBNull.Value)
				{
					this.ShowLocationInfo = Convert.ToBoolean(reader["ShowLocationInfo"]);
				}

				if (reader["UseDrivingDirections"] != DBNull.Value)
				{
					this.UseDrivingDirections = Convert.ToBoolean(reader["UseDrivingDirections"]);
				}

				if (reader["MapType"] != DBNull.Value)
				{
					this.MapType = reader["MapType"].ToString();
				}

				if (reader["MapZoom"] != DBNull.Value)
				{
					this.MapZoom = Convert.ToInt32(reader["MapZoom"]);
				}

				if (reader["ShowDownloadLink"] != DBNull.Value)
				{
					this.ShowDownloadLink = Convert.ToBoolean(reader["ShowDownloadLink"]);
				}

				if (reader["ExcludeFromRecentContent"] != DBNull.Value)
				{
					this.ExcludeFromRecentContent = Convert.ToBoolean(reader["ExcludeFromRecentContent"]);
				}

				if (reader["IncludeInNews"] != DBNull.Value)
				{
					this.IncludeInNews = Convert.ToBoolean(reader["IncludeInNews"]);
				}

				PubName = reader["PubName"].ToString();
				PubLanguage = reader["PubLanguage"].ToString();
				PubAccess = reader["PubAccess"].ToString();
				PubGenres = reader["PubGenres"].ToString();
				PubKeyWords = reader["PubKeyWords"].ToString();
				PubGeoLocations = reader["PubGeoLocations"].ToString();
				PubStockTickers = reader["PubStockTickers"].ToString();
				HeadlineImageUrl = reader["HeadlineImageUrl"].ToString();

				if (reader["IncludeImageInExcerpt"] != DBNull.Value)
				{
					IncludeImageInExcerpt = Convert.ToBoolean(reader["IncludeImageInExcerpt"]);
				}

				if (reader["IncludeImageInPost"] != DBNull.Value)
				{
					IncludeImageInPost = Convert.ToBoolean(reader["IncludeImageInPost"]);
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
			BlogGuid = Guid.NewGuid();
			CreatedUtc = DateTime.UtcNow;

			if (Approved)
			{
				ApprovedDate = DateTime.UtcNow;
			}

			newID = DBBlog.AddBlog(
				BlogGuid,
				ModuleGuid,
				ModuleId,
				UserName,
				Title,
				Excerpt,
				Description,
				StartDate,
				IsInNewsletter,
				IncludeInFeed,
				AllowCommentsForDays,
				Location,
				UserGuid,
				CreatedUtc,
				ItemUrl,
				MetaKeywords,
				MetaDescription,
				CompiledMeta,
				IsPublished,
				SubTitle,
				EndDate,
				Approved,
				ApprovedBy,
				ApprovedDate,
				ShowAuthorName,
				ShowAuthorAvatar,
				ShowAuthorBio,
				IncludeInSearch,
				UseBingMap,
				MapHeight,
				MapWidth,
				ShowMapOptions,
				ShowZoomTool,
				ShowLocationInfo,
				UseDrivingDirections,
				MapType,
				MapZoom,
				ShowDownloadLink,
				IncludeInSiteMap,
				ExcludeFromRecentContent,
				IncludeInNews,
				PubName,
				PubLanguage,
				PubAccess,
				PubGenres,
				PubKeyWords,
				PubGeoLocations,
				PubStockTickers,
				HeadlineImageUrl,
				IncludeImageInExcerpt,
				IncludeImageInPost
			);

			ItemId = newID;

			bool result = (newID > 0);

			//IndexHelper.IndexItem(this);
			if (result)
			{
				var e = new ContentChangedEventArgs();
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
			LastModUtc = DateTime.UtcNow;
			if (Approved && ApprovedDate == DateTime.MaxValue) 
			{ 
				ApprovedDate = DateTime.UtcNow; 
			}

			bool result = DBBlog.UpdateBlog(
				ModuleId,
				ItemId,
				UserName,
				Title,
				Excerpt,
				Description,
				StartDate,
				IsInNewsletter,
				IncludeInFeed,
				AllowCommentsForDays,
				Location,
				LastModUserGuid,
				LastModUtc,
				ItemUrl,
				MetaKeywords,
				MetaDescription,
				CompiledMeta,
				IsPublished,
				SubTitle,
				EndDate,
				Approved,
				ApprovedBy,
				ApprovedDate,
				ShowAuthorName,
				ShowAuthorAvatar,
				ShowAuthorBio,
				IncludeInSearch,
				UseBingMap,
				MapHeight,
				MapWidth,
				ShowMapOptions,
				ShowZoomTool,
				ShowLocationInfo,
				UseDrivingDirections,
				MapType,
				MapZoom,
				ShowDownloadLink,
				IncludeInSiteMap,
				ExcludeFromRecentContent,
				IncludeInNews,
				PubName,
				PubLanguage,
				PubAccess,
				PubGenres,
				PubKeyWords,
				PubGeoLocations,
				PubStockTickers,
				HeadlineImageUrl,
				IncludeImageInExcerpt,
				IncludeImageInPost
			);

			var e = new ContentChangedEventArgs();
			OnContentChanged(e);

			return result;
		}

		#endregion


		#region Public Methods

		public void CreateHistory(Guid siteGuid)
		{
			if (BlogGuid == Guid.Empty)
			{
				return;
			}

			var currentVersion = new Blog(ItemId);

			if (currentVersion.Description == Description)
			{
				return;
			}

			var history = new ContentHistory();
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
			if (ItemId > 0)
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
			DBBlog.DeleteItemCategories(ItemId);
			DBBlog.DeleteAllCommentsForBlog(ItemId);
			DBBlog.UpdateCommentStats(ModuleId);
			bool result = DBBlog.DeleteBlog(ItemId);
			DBBlog.UpdateEntryStats(ModuleId);

			var e = new ContentChangedEventArgs();
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
			var dataSet = new DataSet();

			var posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetBlogsForMetaWeblogApi(moduleId, beginDate, DateTime.UtcNow))
			{
				while (reader.Read())
				{
					var row = posts.NewRow();

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

			var categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetBlogCategoriesForMetaWeblogApi(moduleId, beginDate, DateTime.UtcNow))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = categories.NewRow();

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
			var dataTable = new DataTable();

			dataTable.Columns.Add("ID", typeof(int));
			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.Columns.Add("CategoryID", typeof(int));
			dataTable.Columns.Add("Category", typeof(string));

			return dataTable;

		}

		private static DataTable GetAttachmentsTableStructure()
		{
			var dataTable = new DataTable();

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
			var dataTable = new DataTable();



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

			var posts = GetPostsTableStructure();

			if (pageSize < 1)
			{
				pageSize = 1;
			}

			if (pageNumber < 1)
			{
				pageNumber = 1;
			}

			using (IDataReader reader = GetPage(moduleId, beginDate, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					var row = posts.NewRow();


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

			var categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			var attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(moduleId, beginDate, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = attachments.NewRow();

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

			var posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetBlogEntriesByMonth(month, year, moduleId, currentTime, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					var row = posts.NewRow();


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

			var categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(month, year, moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = categories.NewRow();

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

			var attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(month, year, moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{

					var row = attachments.NewRow();

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
			var dataSet = new DataSet();

			var posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetEntriesByCategory(moduleId, categoryId, currentTime, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					var row = posts.NewRow();

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

			var categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForPage(moduleId, categoryId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = categories.NewRow();

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

			var attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForPage(moduleId, categoryId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = attachments.NewRow();

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
			catch (ConstraintException) { }
			catch (ArgumentException) { }

			return dataSet;
		}

		public static DataSet GetClosedDataSet(
			int moduleId,
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			var dataSet = new DataSet();

			var posts = GetPostsTableStructure();

			using (IDataReader reader = DBBlog.GetClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					//posts.Load(reader);
					var row = posts.NewRow();

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

			var categories = GetCategoryTableStructure();

			using (IDataReader reader = DBBlog.GetCategoriesForClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = categories.NewRow();

					row["ID"] = reader["ID"];
					row["ItemID"] = reader["ItemID"];
					row["CategoryID"] = reader["CategoryID"];
					row["Category"] = reader["Category"];

					categories.Rows.Add(row);
				}

			}

			categories.TableName = "Categories";
			dataSet.Tables.Add(categories);

			var attachments = GetAttachmentsTableStructure();

			using (IDataReader reader = DBBlog.GetAttachmentsForClosed(moduleId, DateTime.UtcNow, pageNumber, pageSize))
			{
				while (reader.Read())
				{
					//categories.Load(reader);
					var row = attachments.NewRow();

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
			var dataTable = new DataTable();

			dataTable.Columns.Add("ItemID", typeof(int));
			dataTable.Columns.Add("Heading", typeof(string));
			dataTable.Columns.Add("ItemUrl", typeof(string));

			int counter = 1;

			using (IDataReader reader = DBBlog.GetRelatedPosts(itemId))
			{
				while (reader.Read())
				{
					if (counter > maxToGet) { continue; }

					var row = dataTable.NewRow();
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

		public static IDataReader GetBlogComments(int moduleId, int itemId)
		{
			return DBBlog.GetBlogComments(moduleId, itemId);
		}

		public static DataTable GetBlogCommentsTable(int moduleId, int itemId)
		{
			var dataTable = new DataTable();

			dataTable.Columns.Add("Comment", typeof(string));
			dataTable.Columns.Add("Name", typeof(string));

			using (IDataReader reader = DBBlog.GetBlogComments(moduleId, itemId))
			{
				while (reader.Read())
				{
					var row = dataTable.NewRow();

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
			string name,
			string title,
			string url,
			string comment,
			DateTime dateCreated)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				name = "unknown";
			}

			if (!string.IsNullOrWhiteSpace(title) 
				&& !string.IsNullOrWhiteSpace(url) 
				&& !string.IsNullOrWhiteSpace(comment))
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

		public static bool UpdateBlogCategory(int categoryId, string category)
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
			var dataTable = new DataTable();

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
					var row = dataTable.NewRow();

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
			ContentChanged?.Invoke(this, e);
		}
		#endregion
	}
}