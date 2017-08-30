using System;

namespace mojoPortal.Business
{
	public class BlogPostModel
	{
		public Guid BlogGuid { get; set; }
		public Guid ModuleGuid { get; set; }
		public int Id { get; set; }
		public int PreviousItemId { get; set; }
		public int NextItemId { get; set; }
		public int ModuleId { get; set; }
		public string UserName { get; set; }
		public Guid UserGuid { get; set; }
		public Guid LastModUserGuid { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string Category { get; set; }
		public string Excerpt { get; set; }
		public string Body { get; set; }
		public string Location { get; set; }
		public string MetaKeywords { get; set; }
		public string MetaDescription { get; set; }
		public string CompiledMeta { get; set; }
		public DateTime PostDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool Approved { get; set; }
		public Guid ApprovedBy { get; set; }
		public DateTime ApprovedDate { get; set; }
		public bool IsPublished { get; set; }
		public bool IsInNewsletter { get; set; }
		public bool IncludeInFeed { get; set; }
		public int AllowCommentsForDays { get; set; }
		public DateTime CreatedUtc { get; set; }
		public DateTime LastModUtc { get; set; }
		public string ItemUrl { get; set; }
		public bool ShowAuthorName { get; set; }
		public bool ShowAuthorAvatar { get; set; }
		public bool ShowAuthorBio { get; set; }
		public bool IncludeInSearch { get; set; }
		public bool UseBingMap { get; set; }
		public string MapHeight { get; set; }
		public string MapWidth { get; set; }
		public bool ShowMapOptions { get; set; }
		public bool ShowZoomTool { get; set; }
		public bool ShowLocationInfo { get; set; }
		public bool UseDrivingDirections { get; set; }
		public string MapType { get; set; }
		public int MapZoom { get; set; }
		public bool ShowDownloadLink { get; set; }
		public bool IncludeInSiteMap { get; set; }
		public bool ExcludeFromRecentContent { get; set; }
		public string PreviousPostUrl { get; set; }
		public string NextPostUrl { get; set; }
		public string PreviousPostTitle { get; set; }
		public string NextPostTitle { get; set; }
		public int CommentCount { get; set; }
		public int AuthorUserId { get; set; }
		public string AuthorLoginName { get; set; }
		public string AuthorDisplayName { get; set; }
		public string AuthorFirstName { get; set; }
		public string AuthorLastName { get; set; }
		public string AuthorEmail { get; set; }
		public string AuthorAvatar { get; set; }
		public string AuthorBio { get; set; }
		public bool IncludeInNews { get; set; }
		public string PubName { get; set; }
		public string PubLanguage { get; set; }
		public string PubAccess { get; set; }
		public string PubGenres { get; set; }
		public string PubKeyWords { get; set; }
		public string PubGeoLocations { get; set; }
		public string PubStockTickers { get; set; }
		public string HeadlineImageUrl { get; set; }
		public bool IncludeImageInExcerpt { get; set; }
		public bool IncludeImageInPost { get; set; }
		public bool FeaturedPost { get; set; }
	}
}
