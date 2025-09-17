using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Nl;

namespace mojoPortal.SearchIndex;

public class DutchAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer()
	{
		return new DutchAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}