namespace mojoPortal.Web.UI;


/// <summary>
/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
/// </summary>
public class SearchResultsDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public SearchResultsDisplaySettings() : base() { }

	public string ItemHeadingElement { get; set; } = "h3";

	public bool ShowAuthor { get; set; } = false;

	public string AuthorFormat { get; set; } = string.Empty;

	public bool ShowCreatedDate { get; set; } = false;

	public string CreatedFormat { get; set; } = string.Empty;

	public bool ShowLastModDate { get; set; } = false;

	public string ModifiedFormat { get; set; } = string.Empty;

	public bool ShowModifiedDateFilters { get; set; } = false;

	public bool ShowFeatureFilter { get; set; } = true;
}