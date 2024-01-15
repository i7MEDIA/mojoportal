using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class TextAreaAdapter : IWebEditor
{
	public TextAreaAdapter() { }

	private TextAreaEditorControl Editor = new TextAreaEditorControl();
	private Unit editorWidth = Unit.Percentage(98);
	private Unit editorHeight = Unit.Pixel(350);

	public string ControlID
	{
		get { return Editor.ID; }
		set { Editor.ID = value; }
	}

	public string ClientID
	{
		get { return Editor.ClientID; }
	}

	public string Text
	{
		get { return Editor.Text; }
		set { Editor.Text = value; }
	}

	public Unit Width
	{
		get { return editorWidth; }
		set
		{
			editorWidth = value;
			Editor.Width = editorWidth;
		}
	}

	public Unit Height
	{
		get { return editorHeight; }
		set
		{
			editorHeight = value;
			Editor.Height = editorHeight;

		}
	}
	public Control GetEditorControl()
	{
		return Editor;
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
}