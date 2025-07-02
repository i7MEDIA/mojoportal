namespace mojoPortal.Web.UI;

/// <summary>
/// Display Settings for Core functionality.
/// Configuration is per skin via the config/Core/PageLayoutPage-display.json file.
/// </summary>
public class CoreDisplaySettings : BaseDisplaySettings
{
    public CoreDisplaySettings() : base() { }
	public override string FeatureName => "Core";

	public string DefaultPageHeaderMarkup { get; set; } = "<h2>{0}</h2>";
	public string AlertSuccessMarkup { get; set; } = "<div class='alert alert-success'>{0}</div>";
	public string AlertNoticeMarkup { get; set; } = "<div class='alert alert-info'>{0}</div>";
	public string AlertWarningMarkup { get; set; } = "<div class='alert alert-warning'>{0}</div>";
	public string AlertErrorMarkup { get; set; } = "<div class='alert alert-danger'>{0}</div>";
}