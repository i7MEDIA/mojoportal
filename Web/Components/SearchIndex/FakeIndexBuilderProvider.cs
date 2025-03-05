using mojoPortal.Business;

namespace mojoPortal.SearchIndex;

/// <summary>
/// The purpose of this provider is just to make sure there is always at least one
/// provider in the collection even if we have to disable the search index by
/// removing the other providers
/// </summary>
public class FakeIndexBuilderProvider : IndexBuilderProvider
{
	public FakeIndexBuilderProvider()
	{ }

	public override void RebuildIndex(PageSettings pageSettings, string indexPath)
	{ }

	public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
	{ }
}
