using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ru;

namespace mojoPortal.SearchIndex;

public class RussianAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new RussianAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}