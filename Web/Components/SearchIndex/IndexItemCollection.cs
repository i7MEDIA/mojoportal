// Author:				
// Created:			    2005-06-30
// Last Modified:		2009-01-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;

namespace mojoPortal.SearchIndex
{
    /// <summary>
    ///
    /// </summary>
    public class IndexItemCollection : CollectionBase
    {
        private int itemCount;
        private int pageIndex;
        private long executionTime;


        public int ItemCount
        {
            get { return this.itemCount; }
            set { this.itemCount = value; }
        }


        public int PageIndex
        {
            get { return this.pageIndex; }
            set { this.pageIndex = value; }
        }


        public long ExecutionTime
        {
            get { return this.executionTime; }
            set { this.executionTime = value; }
        }


        public IndexItem this[int index]
        {
            get { return (IndexItem)this.List[index]; }
        }


        public IndexItemCollection()
        {
        }


        public void Add(IndexItem item)
        {
            this.List.Add(item);
        }


        public void Remove(IndexItem item)
        {
            this.List.Remove(item);
        }

    }
}
