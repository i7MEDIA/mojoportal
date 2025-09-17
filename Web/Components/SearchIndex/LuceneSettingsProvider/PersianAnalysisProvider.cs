using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Fa;

namespace mojoPortal.SearchIndex
{
	public class PersianAnalysisProvider : LuceneSettingsProvider
	{
		public override Analyzer GetAnalyzer() => new PersianAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}