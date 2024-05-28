namespace mojoPortal.Web.GalleryUI;


public class GalleryDisplaySettings : BasePluginDisplaySettings
{
	public GalleryDisplaySettings() : base() { }

	public string NivoTheme { get; set; } = "theme-default";

	public string NivoConfig { get; set; } = string.Empty;

	public bool NivoLinkToFullSize { get; set; } = true;

	public bool NivoUseFullSizeImages { get; set; } = false;
}