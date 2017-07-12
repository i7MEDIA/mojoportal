// Author:					    
// Created:				        2013-01-11
// Last Modified:			    2013-01-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration.Provider;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Fa;


namespace mojoPortal.SearchIndex
{
    public class PersianAnalysisProvider : LuceneSettingsProvider
    {

        public override Analyzer GetAnalyzer()
        {
            return new PersianAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }
    }
}