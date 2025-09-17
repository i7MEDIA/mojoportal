using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cz;

namespace mojoPortal.SearchIndex;

public class CzechAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new CzechAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
}