namespace mojoPortal.Web.SharedFilesUI;


public class SharedFilesDisplaySettings : BasePluginDisplaySettings
{
	public SharedFilesDisplaySettings() : base() { }

	public bool HideDescription { get; set; } = false;
	public bool HideSize { get; set; } = false;
	public bool HideDownloadCount { get; set; } = false;
	public bool HideModified { get; set; } = false;
	public bool HideUploadedBy { get; set; } = false;
	public bool HideFirstColumnIfNotEditable { get; set; } = false;
	public bool ShowClickableFolderPathCrumbs { get; set; } = true;
	public string PathSeparator { get; set; } = "/";
	public string NewWindowLinkMarkup { get; set; } = "onclick=\"window.open(this.href,'_blank');return false;\"";
	public string IeNewWindowLinkMarkup { get; set; } = " target='_blank' ";
	public string DeleteButtonCssClass { get; set; } = "sharedfiles-delete deleteitem";
	public string UpFolderButtonCssClass { get; set; } = "sharedfiles-upfolder";
	public string DownloadButtonCssClass { get; set; } = "sharedfiles-download";
	public string PropertiesButtonCssClass { get; set; } = "sharedfiles-properties";
}