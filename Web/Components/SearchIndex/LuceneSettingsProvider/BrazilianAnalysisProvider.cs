using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Br;


namespace mojoPortal.SearchIndex;

public class BrazilianAnalysisProvider : LuceneSettingsProvider
{

	public override Analyzer GetAnalyzer()
	{
		return new BrazilianAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}