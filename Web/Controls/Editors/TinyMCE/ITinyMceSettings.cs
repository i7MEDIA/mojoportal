namespace mojoPortal.Web.Editor;

public interface ITinyMceSettings
{
	bool Inline { get; set; }
	bool AutoFocus { get; set; }
	string Menubar { get; set; }
	bool DisableMenuBar { get; set; }
	string Plugins { get; set; }
	string Toolbar1Buttons { get; set; }
	string Toolbar2Buttons { get; set; }
	string Toolbar3Buttons { get; set; }
	string ExtendedValidElements { get; set; }
	bool ForcePasteAsPlainText { get; set; }
	bool ConvertUrls { get; set; }
	bool EnableObjectResizing { get; set; }
	int UnDoLevels { get; set; }
	string Theme { get; set; }
	string Skin { get; set; }
	bool AutoLocalize { get; set; }
	string Language { get; set; }
	string TextDirection { get; set; }
	bool EnableBrowserSpellCheck { get; set; }
	string EditorBodyCssClass { get; set; }
	bool NoWrap { get; set; }
	string RemovedMenuItems { get; set; }
	int FileDialogHeight { get; set; }
	int FileDialogWidth { get; set; }
	bool EnableImageAdvancedTab { get; set; }
	bool ShowStatusbar { get; set; }
	string CustomToolbarElementClientId { get; set; }
	string EditorAreaCSS { get; set; }
	string TemplatesUrl { get; set; }
	string StyleFormats { get; set; }
	string EmotionsBaseUrl { get; set; }
	string FileManagerUrl { get; set; }
	string GlobarVarToAssignEditor { get; set; }
	string OnSaveCallback { get; set; }
	bool SaveEnableWhenDirty { get; set; }
	bool PromptOnNavigationWithUnsavedChanges { get; set; }
	string ImagesUploadUrl { get; set; }
}
