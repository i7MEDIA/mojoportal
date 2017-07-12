// Author:				
// Created:			    2005-06-26
// Last Modified:		2013-04-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace mojoPortal.SearchIndex
{
    /// <summary>
    ///
    /// </summary>
    [Serializable()]
    public class IndexItem : IComparable
    {
        #region Constructors

        public IndexItem()
        {
        }

        private Lucene.Net.Documents.Document luceneDoc = null;

        public IndexItem(Lucene.Net.Documents.Document doc, float score)
        {
            luceneDoc = doc;

            docKey = luceneDoc.Get("Key");
            siteID = Convert.ToInt32(luceneDoc.Get("SiteID"), CultureInfo.InvariantCulture);

            PageName = luceneDoc.Get("PageName");
            ModuleTitle = luceneDoc.Get("ModuleTitle");
            Title = luceneDoc.Get("Title");
            intro = luceneDoc.Get("Intro");
            ViewPage = luceneDoc.Get("ViewPage");
            QueryStringAddendum = luceneDoc.Get("QueryStringAddendum");
            bool useQString;
            if (bool.TryParse(luceneDoc.Get("UseQueryStringParams"), out useQString))
            {
                useQueryStringParams = useQString;
            }
            Author = luceneDoc.GetNullSafeString("Author");



            // the below are lazy loaded if accessed from the public getters

            //ViewRoles = luceneDoc.Get("ViewRoles");
            //ModuleViewRoles = luceneDoc.Get("ModuleRole");
            //SiteId = Convert.ToInt32(luceneDoc.Get("SiteID"), CultureInfo.InvariantCulture);
            //PageId = Convert.ToInt32(luceneDoc.Get("PageID"), CultureInfo.InvariantCulture);
            
            //PageIndex = Convert.ToInt32(luceneDoc.Get("PageIndex"), CultureInfo.InvariantCulture);
            //PageNumber = Convert.ToInt32(luceneDoc.Get("PageNumber"), CultureInfo.InvariantCulture);

            //string fid = luceneDoc.Get("FeatureId");
            //if ((fid != null)&&(fid.Length > 0))
            //{
            //    FeatureId = fid;
            //}
            //FeatureName = luceneDoc.Get("FeatureName");
            //ItemId = Convert.ToInt32(luceneDoc.Get("ItemID"), CultureInfo.InvariantCulture);
            //ModuleId = Convert.ToInt32(luceneDoc.Get("ModuleID"), CultureInfo.InvariantCulture);
            
            
            //DateTime pubBegin = DateTime.MinValue;
            //if (DateTime.TryParse(luceneDoc.Get("PublishBeginDate"), out pubBegin))
            //{
            //    this.publishBeginDate = pubBegin;
            //}

            //DateTime pubEnd = DateTime.MaxValue;
            //if (DateTime.TryParse(luceneDoc.Get("PublishEndDate"), out pubEnd))
            //{
            //    this.publishEndDate = pubEnd;
            //}

            

            //try
            //{
            //    long createdTicks = Convert.ToInt64(luceneDoc.Get("CreatedUtc"));
            //    CreatedUtc = new DateTime(createdTicks);
            //}
            //catch (FormatException) { }

            //try
            //{
            //    long lastModTicks = Convert.ToInt64(luceneDoc.Get("LastModUtc"));
            //    LastModUtc = new DateTime(lastModTicks);
            //}
            //catch (FormatException) { }


            
            //boost = doc.GetBoost();
            //boost = luceneDoc.Boost;

            
        }

        #endregion

        #region Private Properties

        private int siteID = -1;
        private int pageID = -1;
        private int moduleID = -1;
        private int itemID = -1;
        private int pageIndex = -1;
        private int pageNumber = 1; // for use in pageable modules like forums
        private string pageName = string.Empty;
        private string featureId = Guid.Empty.ToString();
        private string featureName = string.Empty;
        private string featureResourceFile = string.Empty;
        private string moduleTitle = string.Empty;
        private string title = string.Empty;
        private string content = string.Empty;
        private string otherContent = string.Empty;
        private string intro = string.Empty;
        private string viewRoles = string.Empty;
        private string moduleViewRoles = string.Empty;
        private string viewPage = "Default.aspx";
        private bool useQueryStringParams = true;
        private string queryStringAddendum = string.Empty;
        private DateTime publishBeginDate = DateTime.MinValue;
        private DateTime publishEndDate = DateTime.MaxValue;
        private string indexPath = string.Empty;
        private string itemKey = string.Empty;
        private string pageMetaDescription = string.Empty;
        private string pageMetaKeywords = string.Empty;
        

        //private float score = 0;
        //private float boost = 0;

        private bool removeOnly = false;

        
        

        #endregion


        #region Public Properties

        public string IndexPath
        {
            get { return indexPath; }
            set { indexPath = value; }
        }

        private string docKey = string.Empty;

        // same as Key but only populated when retrieving items from the index
        public string DocKey
        {
            get { return docKey; }
        }

        public string Key
        {
            get
            {
                return this.siteID.ToString(CultureInfo.InvariantCulture) + "~"
                    + this.pageID.ToString(CultureInfo.InvariantCulture) + "~"
                    + this.moduleID.ToString(CultureInfo.InvariantCulture) + "~"
                    + ItemKey
                    + this.queryStringAddendum;
            }
        }

        
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private bool didLoadPageIdFromLuceneDoc = false;

        public int PageId
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadPageIdFromLuceneDoc))
                {
                    pageID = Convert.ToInt32(luceneDoc.Get("PageID"), CultureInfo.InvariantCulture);
                    didLoadPageIdFromLuceneDoc = true;
                }
                return pageID; 
            }
            set { pageID = value; }
        }


        private bool didLoadModuleIdFromLuceneDoc = false;

        public int ModuleId
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadModuleIdFromLuceneDoc))
                {
                    moduleID = Convert.ToInt32(luceneDoc.Get("ModuleID"), CultureInfo.InvariantCulture);
                    didLoadModuleIdFromLuceneDoc = true;
                }
                
                return moduleID; }
            set { moduleID = value; }
        }

        private bool didLoadItemIdFromLuceneDoc = false;

        public int ItemId
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadItemIdFromLuceneDoc))
                {
                    itemID = Convert.ToInt32(luceneDoc.Get("ItemID"), CultureInfo.InvariantCulture);
                    didLoadItemIdFromLuceneDoc = true;
                }
                
                return itemID; }
            set { itemID = value; }
        }

        public string ItemKey
        {
            get 
            {
                if (ItemId > -1)
                {
                    //return ItemId.ToString(CultureInfo.InvariantCulture) + itemKey;
                    return ItemId.ToString(CultureInfo.InvariantCulture);
                }
                return itemKey; 
            }
            set { itemKey = value; }
        }

        /// <summary>
        /// legacy field not needed anymore
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private bool didLoadPageNumberFromLuceneDoc = false;

        public int PageNumber
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadPageNumberFromLuceneDoc))
                {
                    pageNumber = Convert.ToInt32(luceneDoc.Get("PageNumber"), CultureInfo.InvariantCulture);
                    didLoadPageNumberFromLuceneDoc = true;
                }

                return pageNumber; }
            set { pageNumber = value; }
        }

        public bool RemoveOnly
        {
            get { return removeOnly; }
            set { removeOnly = value; }
        }


        [XmlIgnore]
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        // This is needed to support xml serialization, string with special characterscan cause invalid xml, base 64 encoding them gets around the problem.

        [XmlElement(ElementName = "pageName", DataType = "base64Binary")]
        public byte[] PageNameSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageName);
            }
            set
            {
                PageName = System.Text.Encoding.Unicode.GetString(value);
            }
        }



        private bool didLoadPageMetaFromLuceneDoc = false;
        //PageMetaDesc

        [XmlIgnore]
        public string PageMetaDescription
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadPageMetaFromLuceneDoc))
                {
                    string s = luceneDoc.Get("PageMetaDesc");
                    if ((s != null) && (s.Length > 0))
                    {
                        pageMetaDescription = s;
                    }
                    didLoadPageMetaFromLuceneDoc = true;
                }

                return pageMetaDescription; 
            }
            set { pageMetaDescription = value; }
        }

        [XmlElement(ElementName = "pageMetaDescription", DataType = "base64Binary")]
        public byte[] PageMetaDescriptionSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageMetaDescription);
            }
            set
            {
                PageMetaDescription = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        private bool didLoadPageMetaKeywordsFromLuceneDoc = false;

        [XmlIgnore]
        public string PageMetaKeywords
        {
            get {
                //lazy load
                if ((luceneDoc != null) && (!didLoadPageMetaKeywordsFromLuceneDoc))
                {
                    string s = luceneDoc.Get("Keyword");
                    if ((s != null) && (s.Length > 0))
                    {
                        pageMetaKeywords = s;
                    }
                    didLoadPageMetaKeywordsFromLuceneDoc = true;
                }

                return pageMetaKeywords; }
            set { pageMetaKeywords = value; }
        }

        [XmlElement(ElementName = "pageMetaKeywords", DataType = "base64Binary")]
        public byte[] PageMetaKeywordsSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(PageMetaKeywords);
            }
            set
            {
                PageMetaKeywords = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        //private bool didLoadItemMetaFromLuceneDoc = false;
        
        //private string itemMetaDescription = string.Empty;

        //[XmlIgnore]
        //public string ItemMetaDescription
        //{
        //    get
        //    {
        //        //lazy load
        //        if ((luceneDoc != null) && (!didLoadItemMetaFromLuceneDoc))
        //        {
        //            string s = luceneDoc.Get("ItemMetaDesc");
        //            if ((s != null) && (s.Length > 0))
        //            {
        //                itemMetaDescription = s;
        //            }
        //            didLoadItemMetaFromLuceneDoc = true;
        //        }

        //        return itemMetaDescription;
        //    }
        //    set { itemMetaDescription = value; }
        //}

        //[XmlElement(ElementName = "itemMetaDescription", DataType = "base64Binary")]
        //public byte[] ItemMetaDescriptionSerialization
        //{
        //    get
        //    {
        //        return System.Text.Encoding.Unicode.GetBytes(ItemMetaDescription);
        //    }
        //    set
        //    {
        //        ItemMetaDescription = System.Text.Encoding.Unicode.GetString(value);
        //    }
        //}

        //private bool didLoadItemMetaKeywordsFromLuceneDoc = false;

        //private string itemMetaKeywords = string.Empty;

        //[XmlIgnore]
        //public string ItemMetaKeywords
        //{
        //    get
        //    {
        //        //lazy load
        //        if ((luceneDoc != null) && (!didLoadItemMetaKeywordsFromLuceneDoc))
        //        {
        //            string s = luceneDoc.Get("ItemKeywords");
        //            if ((s != null) && (s.Length > 0))
        //            {
        //                itemMetaKeywords = s;
        //            }
        //            didLoadItemMetaKeywordsFromLuceneDoc = true;
        //        }

        //        return itemMetaKeywords;
        //    }
        //    set { itemMetaKeywords = value; }
        //}

        //[XmlElement(ElementName = "itemMetaKeywords", DataType = "base64Binary")]
        //public byte[] ItemMetaKeywordsSerialization
        //{
        //    get
        //    {
        //        return System.Text.Encoding.Unicode.GetBytes(ItemMetaKeywords);
        //    }
        //    set
        //    {
        //        ItemMetaKeywords = System.Text.Encoding.Unicode.GetString(value);
        //    }
        //}

        private bool didLoadFeatureIdFromLuceneDoc = false;

        public string FeatureId
        {
            get {
                if ((luceneDoc != null) && (!didLoadFeatureIdFromLuceneDoc))
                {
                    string fid = luceneDoc.Get("FeatureId");
                    if ((fid != null) && (fid.Length > 0))
                    {
                        featureId = fid;
                    }
                    didLoadFeatureIdFromLuceneDoc = true;
                }
                return featureId; }
            set { featureId = value; }
        }

        private bool didLoadFeatureNameFromLuceneDoc = false;

        [XmlIgnore]
        public string FeatureName
        {
            get {
                if ((luceneDoc != null) && (!didLoadFeatureNameFromLuceneDoc))
                {
                    featureName = luceneDoc.Get("Feature");
                    didLoadFeatureNameFromLuceneDoc = true;
                }
                return featureName; }
            set { featureName = value; }
        }

        [XmlElement(ElementName = "featureName", DataType = "base64Binary")]
        public byte[] FeatureNameSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(FeatureName);
            }
            set
            {
                FeatureName = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        public string FeatureResourceFile
        {
            get { return featureResourceFile; }
            set { featureResourceFile = value; }
        }

        private string author = string.Empty;

        [XmlIgnore]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        [XmlElement(ElementName = "author", DataType = "base64Binary")]
        public byte[] AuthorSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Author);
            }
            set
            {
                Author = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        private string categories = string.Empty;

        [XmlIgnore]
        public string Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        [XmlElement(ElementName = "categories", DataType = "base64Binary")]
        public byte[] CategoriesSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Categories);
            }
            set
            {
                Categories = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string ModuleTitle
        {
            get { return moduleTitle; }
            set { moduleTitle = value; }
        }

        [XmlElement(ElementName = "moduleTitle", DataType = "base64Binary")]
        public byte[] ModuleTitleSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ModuleTitle);
            }
            set
            {
                ModuleTitle = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [XmlElement(ElementName = "title", DataType = "base64Binary")]
        public byte[] TitleSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Title);
            }
            set
            {
                Title = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string Intro
        {
            get { return intro; }
            set { intro = value; }
        }

        [XmlElement(ElementName = "intro", DataType = "base64Binary")]
        public byte[] IntroSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Intro);
            }
            set
            {
                Intro = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        private string contentAbstract = string.Empty;

        private bool didLoadContentAbstrctFromLuceneDoc = false;

        [XmlIgnore]
        public string ContentAbstract
        {
            get {
                if ((luceneDoc != null) && (!didLoadContentAbstrctFromLuceneDoc))
                {
                    contentAbstract = luceneDoc.Get("Abstract");
                    didLoadContentAbstrctFromLuceneDoc = true;
                }

                return contentAbstract; }
            set { contentAbstract = value; }
        }

        [XmlElement(ElementName = "contentAbstract", DataType = "base64Binary")]
        public byte[] ContentAbstractSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ContentAbstract);
            }
            set
            {
                ContentAbstract = System.Text.Encoding.Unicode.GetString(value);
            }
        }


        private bool didLoadContentFromLuceneDoc = false;

        [XmlIgnore]
        public string Content
        {
            get {

                if ((luceneDoc != null) && (!didLoadContentFromLuceneDoc))
                {
                    content = luceneDoc.Get("contents");
                    didLoadContentFromLuceneDoc = true;
                }
                return content; }
            set { content = value; }
        }

        [XmlElement(ElementName = "content", DataType = "base64Binary")]
        public byte[] ContentSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(Content);
            }
            set
            {
                Content = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string OtherContent
        {
            get { return otherContent; }
            set { otherContent = value; }
        }

        [XmlElement(ElementName = "otherContent", DataType = "base64Binary")]
        public byte[] OtherContentSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(OtherContent);
            }
            set
            {
                OtherContent = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        private bool didGetViewRolesFromDoc = false;

        [XmlIgnore]
        public string ViewRoles
        {
            get {
                // lazyLoad
                if ((luceneDoc != null) && (!didGetViewRolesFromDoc))
                {
                    viewRoles = luceneDoc.Get("ViewRoles");
                    didGetViewRolesFromDoc = true; ;
                }
                return viewRoles; }
            set { viewRoles = value; }
        }

        [XmlElement(ElementName = "viewRoles", DataType = "base64Binary")]
        public byte[] ViewRolesSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ViewRoles);
            }
            set
            {
                ViewRoles = System.Text.Encoding.Unicode.GetString(value);
            }
        }

        private bool didGetModuleViewRolesFromDoc = false;

        [XmlIgnore]
        public string ModuleViewRoles
        {
            get { 
                // lazyLoad
                if ((luceneDoc != null) && (!didGetModuleViewRolesFromDoc))
                {
                    moduleViewRoles = luceneDoc.Get("ModuleRole");
                    didGetModuleViewRolesFromDoc = true; ;
                }
                return moduleViewRoles; 
            }
            set { moduleViewRoles = value; }
        }

        [XmlElement(ElementName = "moduleViewRoles", DataType = "base64Binary")]
        public byte[] ModuleViewRolesSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ModuleViewRoles);
            }
            set
            {
                ModuleViewRoles = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        [XmlIgnore]
        public string ViewPage
        {
            get { return viewPage; }
            set { viewPage = value; }
        }

        [XmlElement(ElementName = "viewPage", DataType = "base64Binary")]
        public byte[] ViewPageSerialization
        {
            get
            {
                return System.Text.Encoding.Unicode.GetBytes(ViewPage);
            }
            set
            {
                ViewPage = System.Text.Encoding.Unicode.GetString(value);
            }
        }  

        public bool UseQueryStringParams
        {
            get { return useQueryStringParams; }
            set { useQueryStringParams = value; }
        }

        public string QueryStringAddendum
        {
            get { return queryStringAddendum; }
            set { queryStringAddendum = value; }
        }

        public DateTime PublishBeginDate
        {
            get { return publishBeginDate; }
            set { publishBeginDate = value; }
        }

        public DateTime PublishEndDate
        {
            get { return publishEndDate; }
            set { publishEndDate = value; }
        }

        private DateTime createdUtc = DateTime.MinValue;

        private bool didLoadCreatedUtcFromLuceneDoc = false;

        public DateTime CreatedUtc
        {
            get {
                // lazyLoad
                if ((luceneDoc != null) && (!didLoadCreatedUtcFromLuceneDoc))
                { 
                    DateTime c = DateTime.MinValue;
                    if (DateTime.TryParse(luceneDoc.Get("CreatedUtc"), out c))
                    {
                        createdUtc = c;
                    }
                 
                    didLoadCreatedUtcFromLuceneDoc = true; ;
                }
                return createdUtc; }
            set { createdUtc = value; }
        }

        private DateTime lastModUtc = DateTime.MinValue;

        private bool didLoadLastModUtcFromLuceneDoc = false;

        public DateTime LastModUtc
        {
            get {
                // lazyLoad
                if ((luceneDoc != null) && (!didLoadLastModUtcFromLuceneDoc))
                {
                    DateTime c = DateTime.MinValue;
                    if (DateTime.TryParse(luceneDoc.Get("LastModUtc"), out c))
                    {
                        lastModUtc = c;
                    }

                    didLoadLastModUtcFromLuceneDoc = true; ;
                }
                return lastModUtc; }
            set { lastModUtc = value; }
        }



        private bool excludeFromRecentContent = false;

        public bool ExcludeFromRecentContent
        {
            get { return excludeFromRecentContent; }
            set { excludeFromRecentContent = value; }
        }

        #endregion

        public int CompareTo(object obj)
        {
            IndexItem i = obj as IndexItem;
            if (i == null) { return -1; }

            // sort descending on LastModUtc
            if (i.LastModUtc > this.LastModUtc) return 1;

            return -1;

        }


        

    }


}
