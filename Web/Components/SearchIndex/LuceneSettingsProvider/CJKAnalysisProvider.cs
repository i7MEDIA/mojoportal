using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cjk;


namespace mojoPortal.SearchIndex;

public class CJKAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer()
	{
		return new CJKAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}