using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Th;

namespace mojoPortal.SearchIndex;

public class ThaiAnalysisProvider : LuceneSettingsProvider
{

	public override Analyzer GetAnalyzer()
	{
		return new ThaiAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}