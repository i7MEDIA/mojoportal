namespace mojoPortal.Web.Editor;

public class EditorConfig
{
	public string ConfigPath { get; set; }
	public string CssPath { get; set; }
	public bool UseSkinCss { get; set; } = true;
	public string ContainerCssClass { get; set; } = "wysiwygeditor modulecontent";
	public string Theme { get; set; }
}