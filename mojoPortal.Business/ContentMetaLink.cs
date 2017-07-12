// Author:					
// Created:					2009-12-05
// Last Modified:			2009-12-05
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

    public class ContentMetaLink
    {

        public ContentMetaLink()
        { }


        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid contentGuid = Guid.Empty;
        private string rel = string.Empty;
        private string href = string.Empty;
        private string hrefLang = string.Empty;
        private string rev = string.Empty;
        private string type = string.Empty;
        private string media = string.Empty;
        private int sortRank = 0;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;
        private Guid lastModBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid ContentGuid
        {
            get { return contentGuid; }
            set { contentGuid = value; }
        }
        public string Rel
        {
            get { return rel; }
            set { rel = value; }
        }
        public string Href
        {
            get { return href; }
            set { href = value; }
        }
        public string HrefLang
        {
            get { return hrefLang; }
            set { hrefLang = value; }
        }
        public string Rev
        {
            get { return rev; }
            set { rev = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Media
        {
            get { return media; }
            set { media = value; }
        }
        public int SortRank
        {
            get { return sortRank; }
            set { sortRank = value; }
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
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByRel(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.Rel.CompareTo(contentMetaLink2.Rel);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByHref(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.Href.CompareTo(contentMetaLink2.Href);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByHrefLang(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.HrefLang.CompareTo(contentMetaLink2.HrefLang);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByRev(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.Rev.CompareTo(contentMetaLink2.Rev);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByType(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.Type.CompareTo(contentMetaLink2.Type);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByMedia(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.Media.CompareTo(contentMetaLink2.Media);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareBySortRank(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.SortRank.CompareTo(contentMetaLink2.SortRank);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByCreatedUtc(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.CreatedUtc.CompareTo(contentMetaLink2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContentMetaLink.
        /// </summary>
        public static int CompareByLastModUtc(ContentMetaLink contentMetaLink1, ContentMetaLink contentMetaLink2)
        {
            return contentMetaLink1.LastModUtc.CompareTo(contentMetaLink2.LastModUtc);
        }

        #endregion


    }

}
