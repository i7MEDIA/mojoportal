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

    public class CategoryItem
    {

        public CategoryItem()
        { }


        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid itemGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid categoryGuid = Guid.Empty;
        private Guid extraGuid = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
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
        public Guid CategoryGuid
        {
            get { return categoryGuid; }
            set { categoryGuid = value; }
        }
        public Guid ExtraGuid
        {
            get { return extraGuid; }
            set { extraGuid = value; }
        }

        #endregion

        

    }

}
