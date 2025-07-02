namespace mojoPortal.Web.UI;


/// <summary>
/// Display Settings for the Search Results page.
/// Configuration is per skin via the config/Core/SearchResults-display.json file.
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