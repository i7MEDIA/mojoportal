using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace mojoPortal.SearchIndex;

public class StandardAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer()
	{
		return new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}