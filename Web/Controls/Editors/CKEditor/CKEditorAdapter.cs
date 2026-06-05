using mojoPortal.Business.WebHelpers;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class CKEditorAdapter : IWebEditor
{
	public CKEditorAdapter() => InitializeEditor();


	#region Private Properties

	private readonly CKEditorControl _editor = new();

	#endregion


	#region Public Properties

	public string ControlID
	{
		get => _editor.ID;
		set => _editor.ID = value;
	}

	public string ClientID => _editor.ClientID;

	public string Text
	{
		get => _editor.Text;
		set => _editor.Text = value;
	}

	public string ScriptBaseUrl { get; set; } = string.Empty;

	public string SiteRoot { get; set; } = string.Empty;

	public string SkinName { get; set; } = string.Empty;

	public string EditorCSSUrl
	{
		get; set
		{
			field = value;
			_editor.EditorCSSUrl = value;
		}
	} = string.Empty;

	public Unit Width
	{
		get; set
		{
			field = value;
			_editor.Width = field;
		}
	} = Unit.Percentage(98);

	public Unit Height
	{
		get; set
		{
			field = value;
			_editor.Height = field;
		}
	} = Unit.Pixel(350);

	public Direction TextDirection
	{
		get; set
		{
			field = value;
			_editor.TextDirection = value;
		}
	} = Direction.LeftToRight;

	public ToolBar ToolBar
	{
		get; set
		{
			field = value;
			_editor.ToolBar = value;
			SetToolBar();
		}
	} = ToolBar.AnonymousUser;

	public bool SetFocusOnStart
	{
		get; set
		{
			field = value;
			_editor.AutoFocus = field;
		}
	} = false;

	public bool FullPageMode
	{
		get; set
		{
			field = value;
			_editor.FullPageMode = value;
		}
	} = false;

	public bool UseFullyQualifiedUrlsForResources { get; set; } = false;

	#endregion


	#region Private Methods

	private void InitializeEditor() =>
		_editor.Skin = WebConfigSettings.CKEditorSkin;


	private void SetToolBar()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
		var currentSkin = siteSettings.Skin;
		var currentPage = CacheHelper.GetCurrentPage();

		if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
		{
			currentSkin = currentPage.Skin;
		}

		var skinTemplates = new FileInfo($"""{skinRootFolder + currentSkin}\config\ckeditortemplates.js""");
		var skinUrl = SiteUtils.DetermineSkinBaseUrl(currentSkin);

		_editor.MojoSkinPath = skinUrl;

		switch (ToolBar)
		{
			case ToolBar.Full:
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();
				_editor.EnableFileBrowser = true;
				_editor.StylesJsonUrl = "~/Services/CKeditorStyles.ashx"
					.ToLinkBuilder()
					.AddParam("cb", Guid.NewGuid().ToString("N")) //cache busting guid
					.ToString();
				_editor.DropFileUploadUrl = $"~/Services/FileService.ashx"
					.ToLinkBuilder()
					.AddParam("cmd", "uploadfromeditor")
					.AddParam("rz", true)
					.AddParam("ko", WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower())
					.AddParam("t", Global.FileSystemToken)
					.ToString();

				break;

			case ToolBar.FullWithTemplates:
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();
				_editor.EnableFileBrowser = true;
				_editor.TemplatesJsonUrl = "~/Services/CKeditorTemplates.ashx"
					.ToLinkBuilder()
					.AddParam("cb", Guid.NewGuid().ToString("N")) //cache busting guid
					.ToString();

				if (skinTemplates.Exists)
				{
					_editor.SkinTemplatesUrl = $"~/{skinUrl}config/ckeditortemplates.js"
						.ToLinkBuilder()
						.AddParam("cb", Guid.NewGuid().ToString("N")) //cache busting guid
						.ToString();
				}

				_editor.StylesJsonUrl = "~/Services/CKeditorStyles.ashx"
					.ToLinkBuilder()
					.AddParam("cb", Guid.NewGuid().ToString("N")) //cache busting guid
					.ToString();
				_editor.DropFileUploadUrl = $"~/Services/FileService.ashx"
					.ToLinkBuilder()
					.AddParam("cmd", "uploadfromeditor")
					.AddParam("rz", true)
					.AddParam("ko", WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower())
					.AddParam("t", Global.FileSystemToken)
					.ToString();

				break;

			case ToolBar.Newsletter:
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();
				_editor.EnableFileBrowser = true;
				_editor.FullPageMode = true;

				break;

			case ToolBar.ForumWithImages:
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();
				_editor.EnableFileBrowser = true;
				_editor.ForcePasteAsPlainText = true;

				break;

			case ToolBar.Forum:
				_editor.ForcePasteAsPlainText = true;

				break;

			case ToolBar.AnonymousUser:

				break;

			case ToolBar.SimpleWithSource:

				break;
		}
	}

	#endregion


	#region Public Methods

	public Control GetEditorControl()
	{
		InitializeEditor();

		return _editor;
	}

	#endregion
}
