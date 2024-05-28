namespace mojoPortal.Web.UI;

public class RecentContentDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public RecentContentDisplaySettings() : base() { }

	public string ItemHeadingElement { get; set; } = "h3";

	public bool ShowExcerpt { get; set; } = true;

	public bool ShowAuthor { get; set; } = true;

	public string AuthorFormat { get; set; } = string.Empty;

	public bool ShowCreatedDate { get; set; } = true;

	public string CreatedFormat { get; set; } = string.Empty;

	public bool ShowLastModDate { get; set; } = true;

	public string ModifiedFormat { get; set; } = string.Empty;

	public bool ShowFeedLinkTop { get; set; } = true;

	public bool ShowFeedLinkBottom { get; set; } = false;

	public string FeedIconPath { get; set; } = "~/Data/SiteImages/feed.png";

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
	/// </summary>
	public string DateFormat { get; set; } = "d";
}