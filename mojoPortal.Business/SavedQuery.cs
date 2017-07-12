// Author:					
// Created:					2009-12-24
// Last Modified:			2009-12-24
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
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class SavedQuery
    {

        public SavedQuery()
        { }


        #region Private Properties

        private Guid id = Guid.Empty;
        private string name = string.Empty;
        private string statement = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;
        private Guid lastModBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Statement
        {
            get { return statement; }
            set { statement = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }

        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of SavedQuery.
        /// </summary>
        public static int CompareByName(SavedQuery savedQuery1, SavedQuery savedQuery2)
        {
            return savedQuery1.Name.CompareTo(savedQuery2.Name);
        }
        /// <summary>
        /// Compares 2 instances of SavedQuery.
        /// </summary>
        public static int CompareByStatement(SavedQuery savedQuery1, SavedQuery savedQuery2)
        {
            return savedQuery1.Statement.CompareTo(savedQuery2.Statement);
        }
        /// <summary>
        /// Compares 2 instances of SavedQuery.
        /// </summary>
        public static int CompareByCreatedUtc(SavedQuery savedQuery1, SavedQuery savedQuery2)
        {
            return savedQuery1.CreatedUtc.CompareTo(savedQuery2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of SavedQuery.
        /// </summary>
        public static int CompareByLastModUtc(SavedQuery savedQuery1, SavedQuery savedQuery2)
        {
            return savedQuery1.LastModUtc.CompareTo(savedQuery2.LastModUtc);
        }

        #endregion


    }

}
