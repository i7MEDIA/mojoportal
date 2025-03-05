using System.Configuration.Provider;
using mojoPortal.Business;

namespace mojoPortal.SearchIndex;

public abstract class IndexBuilderProvider : ProviderBase
{
	public abstract void RebuildIndex(PageSettings pageSettings, string indexPath);

	public abstract void ContentChangedHandler(object sender, ContentChangedEventArgs e);
}
