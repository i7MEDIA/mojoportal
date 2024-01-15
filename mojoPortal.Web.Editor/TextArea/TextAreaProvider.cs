using System.Collections.Specialized;

namespace mojoPortal.Web.Editor;

public class TextAreaProvider : EditorProvider
{
	public override IWebEditor GetEditor()
	{
		return new TextAreaAdapter();
	}

	public override void Initialize(
		string name,
		NameValueCollection config)
	{
		base.Initialize(name, config);
		// don't read anything from config
		// here as this would raise an error under Medium Trust
	}
}