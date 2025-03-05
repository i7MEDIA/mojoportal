using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Nl;

namespace mojoPortal.SearchIndex;
public class DutchAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new DutchAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}
