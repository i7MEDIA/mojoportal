using Lucene.Net.Analysis;
using Lucene.Net.Analysis.CJK;

namespace mojoPortal.SearchIndex;

public class CJKAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new CJKAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}
