using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class TextAreaAdapter : IWebEditor
{
	private readonly TextAreaEditorControl _editor = new();
	private Unit _editorWidth = Unit.Percentage(98);
	private Unit _editorHeight = Unit.Pixel(350);

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

	public Unit Width
	{
		get => _editorWidth;
		set
		{
			_editorWidth = value;
			_editor.Width = _editorWidth;
		}
	}

	public Unit Height
	{
		get => _editorHeight;
		set
		{
			_editorHeight = value;
			_editor.Height = _editorHeight;
		}
	}

	#region Unused Properties - these exist only for compatibility with the editor plug in model

	public string ScriptBaseUrl { get; set; } = string.Empty;
	public string SiteRoot { get; set; } = string.Empty;
	public string SkinName { get; set; } = string.Empty;
	public string EditorCSSUrl { get; set; } = string.Empty;
	public Direction TextDirection { get; set; } = Direction.LeftToRight;
	public ToolBar ToolBar { get; set; } = ToolBar.AnonymousUser;
	public bool SetFocusOnStart { get; set; } = false;
	public bool FullPageMode { get; set; } = false;
	public bool UseFullyQualifiedUrlsForResources { get; set; } = false;

	#endregion


	public TextAreaAdapter() { }


	public Control GetEditorControl() => _editor;
}
