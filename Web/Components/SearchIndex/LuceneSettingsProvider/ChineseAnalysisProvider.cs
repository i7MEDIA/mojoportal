using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cn;

namespace mojoPortal.SearchIndex;
public class ChineseAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new ChineseAnalyzer();
}
