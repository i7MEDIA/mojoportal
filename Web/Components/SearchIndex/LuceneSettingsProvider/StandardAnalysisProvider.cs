using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace mojoPortal.SearchIndex;

public class StandardAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}