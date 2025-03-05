using Lucene.Net.Analysis;
using Lucene.Net.Analysis.BR;

namespace mojoPortal.SearchIndex;
public class BrazilianAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new BrazilianAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}
