using System.Configuration.Provider;
using Lucene.Net.Analysis;

namespace mojoPortal.SearchIndex;

public abstract class LuceneSettingsProvider : ProviderBase
{
	public abstract Analyzer GetAnalyzer();
}