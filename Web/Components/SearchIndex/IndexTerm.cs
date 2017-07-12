// Author:					    
// Created:				        2013-01-12
// Last Modified:			    2013-01-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;


namespace mojoPortal.SearchIndex
{
    public class IndexTerm : IComparable
    {
        private string term = string.Empty;

        public string Term
        {
            get { return term; }
            set { term = value; }
        }

        private int frequency = 1;

        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public int CompareTo(object obj)
        {
            IndexTerm i = obj as IndexTerm;
            if (i == null) { return -1; }

            // sort descending on frequency
            if (i.Frequency > this.frequency) return 1;

            return -1;

        }

        
    }
}