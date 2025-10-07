using Lucene.Net.Analysis;
using Lucene.Net.Analysis.El;

namespace mojoPortal.SearchIndex;
public class GreekAnalysisProvider : LuceneSettingsProvider
{
	public override Analyzer GetAnalyzer() => new GreekAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
}
