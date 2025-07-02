namespace mojoPortal.Web.LinksUI;

/// <summary>
/// Display Settings for List feature.
/// Configuration is per skin via the config/plugins/List/display.json file.
/// </summary>
public class ListDisplaySettings : BasePluginDisplaySettings
{
    public ListDisplaySettings() : base() { }

    public bool DescriptionAboveLink { get; set; } = false;
}