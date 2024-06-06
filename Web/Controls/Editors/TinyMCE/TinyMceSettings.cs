namespace mojoPortal.Web.Editor;

public class TinyMceSettings : ITinyMceSettings
{
	public TinyMceSettings() { }

	public TinyMceSettings Clone()
	{
		return this.MemberwiseClone() as TinyMceSettings;
	}

	#region NonConfig Settings
	// these are not populated from the config file but can be set programatically

	public bool Inline { get; set; } = false;
	public string OnSaveCallback { get; set; } = string.Empty;
	public bool SaveEnableWhenDirty { get; set; } = true;
	public string CustomToolbarElementClientId { get; set; } = string.Empty;
	public string EditorAreaCSS { get; set; } = string.Empty;
	public string TemplatesUrl { get; set; } = string.Empty;
	public string StyleFormats { get; set; } = string.Empty;
	public string EmotionsBaseUrl { get; set; } = string.Empty;
	public string FileManagerUrl { get; set; } = string.Empty;
	public string ImagesUploadUrl { get; set; } = string.Empty;
	public string GlobarVarToAssignEditor { get; set; } = string.Empty;

	#endregion


	public string Name { get; set; } = "NotFound";
	public bool AutoFocus { get; set; } = false;
	public string Menubar { get; set; } = string.Empty;
	public bool DisableMenuBar { get; set; } = true;
	public string Plugins { get; set; } = "emotions,directionality,inlinepopups";
	public string Toolbar1Buttons { get; set; } = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
	public string Toolbar2Buttons { get; set; } = string.Empty;
	public string Toolbar3Buttons { get; set; } = string.Empty;
	public string ExtendedValidElements { get; set; } = string.Empty;
	public bool ForcePasteAsPlainText { get; set; } = true;
	public bool ConvertUrls { get; set; } = false;
	public bool EnableObjectResizing { get; set; } = false;
	public int UnDoLevels { get; set; } = 10;
	public string Theme { get; set; } = "modern";
	public string Skin { get; set; } = "lightgray";
	public bool AutoLocalize { get; set; } = true;
	public string Language { get; set; } = "en";
	public string TextDirection { get; set; } = "ltr";
	public bool EnableBrowserSpellCheck { get; set; } = true;
	public string EditorBodyCssClass { get; set; } = "wysiwygeditor modulecontent art-postcontent";
	private bool noWrap = false;
	public bool NoWrap
	{
		get { return noWrap; }
		set { Inline = noWrap; }
	}
	public string RemovedMenuItems { get; set; } = "newdocument,print";
	public int FileDialogHeight { get; set; } = 700;
	public int FileDialogWidth { get; set; } = 860;
	public bool EnableImageAdvancedTab { get; set; } = false;
	public bool ShowStatusbar { get; set; } = true;
	public bool PromptOnNavigationWithUnsavedChanges { get; set; } = false;
}
