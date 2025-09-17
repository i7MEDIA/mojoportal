using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ar;


namespace mojoPortal.SearchIndex;

public class ArabicAnalysisProvider : LuceneSettingsProvider
{

	public override Analyzer GetAnalyzer()
	{
		return new ArabicAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
	}
}