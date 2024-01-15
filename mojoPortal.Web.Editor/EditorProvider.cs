using System.Configuration.Provider;

namespace mojoPortal.Web.Editor;

public abstract class EditorProvider : ProviderBase
{
	public abstract IWebEditor GetEditor();
}
