//  Author:                     
//  Created:                    2007-08-30
//	Last Modified:              2013-01-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Configuration.Provider;
using mojoPortal.Business;

namespace mojoPortal.SearchIndex
{
    /// <summary>
    ///  
    /// </summary>
    public abstract class IndexBuilderProvider : ProviderBase
    {
        public abstract void RebuildIndex(
            PageSettings pageSettings,
            string indexPath);


        public abstract void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e);
       
    }
}
