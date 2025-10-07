using Lucene.Net.Analysis;
using Lucene.Net.Analysis.AR;

namespace mojoPortal.SearchIndex;
public class ArabicAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new ArabicAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}
