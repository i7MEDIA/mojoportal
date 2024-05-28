namespace mojoPortal.Web.UI;

public class SearchModuleDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public SearchModuleDisplaySettings() : base() { }

	public int OverridePageSize { get; set; } = 0;

	public string OverrideButtonText { get; set; } = string.Empty;

	public string OverrideWatermarkText { get; set; } = string.Empty;

	public bool UseWatermark { get; set; } = true;

	public string ItemHeadingElement { get; set; } = "h3";

	public bool ShowExcerpt { get; } = true;

	public bool ShowAuthor { get; set; } = false;

	public string AuthorFormat { get; set; } = string.Empty;

	public bool ShowCreatedDate { get; set; } = false;

	public string CreatedFormat { get; set; } = string.Empty;

	public bool ShowLastModDate { get; set; } = false;

	public string ModifiedFormat { get; set; } = string.Empty;
}