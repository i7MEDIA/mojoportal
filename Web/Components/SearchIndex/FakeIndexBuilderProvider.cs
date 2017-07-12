// Author:					    
// Created:				        2007-11-30
// Last Modified:			    2013-01-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;

namespace mojoPortal.SearchIndex
{
    /// <summary>
    /// The purpose of this provider is just to make sure there is always at least one
    /// provider in the collection even if we have to disable the search index by
    /// removing the other providers
    /// </summary>
    public class FakeIndexBuilderProvider : IndexBuilderProvider
    {
        public FakeIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            // Do nothing


        }


        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            
            // Do nothing

        }


        

    }
}
