using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Fr;

namespace mojoPortal.SearchIndex;

public class FrenchAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new FrenchAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
}