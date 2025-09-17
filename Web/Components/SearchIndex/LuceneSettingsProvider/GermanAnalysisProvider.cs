using Lucene.Net.Analysis;
using Lucene.Net.Analysis.De;

namespace mojoPortal.SearchIndex
{
	public class GermanAnalysisProvider : LuceneSettingsProvider
	{
		public override Analyzer GetAnalyzer() => new GermanAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}