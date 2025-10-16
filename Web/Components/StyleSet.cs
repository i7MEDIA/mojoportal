namespace mojoPortal.Web;

public class StyleSet
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string Thumbnail { get; set; }
	public string CssClasses { get; set; }
	public string[] ApplyTo { get; set; } = [];// Feature Strong Names
}