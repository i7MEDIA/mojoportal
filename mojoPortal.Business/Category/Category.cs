// Author:					
// Created:					2011-10-13
// Last Modified:			2011-10-13
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

    public class Category
    {

        public Category()
        { }


        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid parentGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private string category = string.Empty;
        private string description = string.Empty;
        private int itemCount = 0;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private DateTime modifiedUtc = DateTime.UtcNow;
        private Guid modifiedBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid ParentGuid
        {
            get { return parentGuid; }
            set { parentGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public string CategoryText
        {
            get { return category; }
            set { category = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
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
        public DateTime ModifiedUtc
        {
            get { return modifiedUtc; }
            set { modifiedUtc = value; }
        }
        public Guid ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }

        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of Category.
        /// </summary>
        public static int CompareByCategory(Category category1, Category category2)
        {
            return category1.CategoryText.CompareTo(category2.CategoryText);
        }
        /// <summary>
        /// Compares 2 instances of Category.
        /// </summary>
        public static int CompareByDescription(Category category1, Category category2)
        {
            return category1.Description.CompareTo(category2.Description);
        }
        /// <summary>
        /// Compares 2 instances of Category.
        /// </summary>
        public static int CompareByItemCount(Category category1, Category category2)
        {
            return category1.ItemCount.CompareTo(category2.ItemCount);
        }
        /// <summary>
        /// Compares 2 instances of Category.
        /// </summary>
        public static int CompareByCreatedUtc(Category category1, Category category2)
        {
            return category1.CreatedUtc.CompareTo(category2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of Category.
        /// </summary>
        public static int CompareByModifiedUtc(Category category1, Category category2)
        {
            return category1.ModifiedUtc.CompareTo(category2.ModifiedUtc);
        }

        #endregion


    }

}
