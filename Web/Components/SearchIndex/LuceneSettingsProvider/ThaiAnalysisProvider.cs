using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Th;

namespace mojoPortal.SearchIndex;

public class ThaiAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new ThaiAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}