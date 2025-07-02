namespace mojoPortal.Web.GalleryUI;

/// <summary>
/// Display Settings for Gallery feature.
/// Configuration is per skin via the config/plugins/Gallery/display.json file.
/// </summary>
public class GalleryDisplaySettings : BasePluginDisplaySettings
{
	public GalleryDisplaySettings() : base() { }

	public string NivoTheme { get; set; } = "theme-default";

	public string NivoConfig { get; set; } = string.Empty;

	public bool NivoLinkToFullSize { get; set; } = true;

	public bool NivoUseFullSizeImages { get; set; } = false;
}