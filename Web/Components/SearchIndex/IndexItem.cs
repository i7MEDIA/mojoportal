using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using mojoPortal.Web;

namespace mojoPortal.SearchIndex;

[Serializable()]
public class IndexItem : IComparable
{
	#region Constructors

	public IndexItem() { }

	private Lucene.Net.Documents.Document luceneDoc = null;

	public IndexItem(Lucene.Net.Documents.Document doc, float score)
	{
		luceneDoc = doc;

		DocKey = luceneDoc.Get("Key");
		SiteId = Convert.ToInt32(luceneDoc.Get("SiteID"), CultureInfo.InvariantCulture);
		PageName = luceneDoc.Get("PageName");
		ModuleTitle = luceneDoc.Get("ModuleTitle");
		Title = luceneDoc.Get("Title");
		Intro = luceneDoc.Get("Intro");
		ViewPage = luceneDoc.Get("ViewPage");
		QueryStringAddendum = luceneDoc.Get("QueryStringAddendum");

		if (bool.TryParse(luceneDoc.Get("UseQueryStringParams"), out bool useQString))
		{
			UseQueryStringParams = useQString;
		}

		Author = luceneDoc.GetNullSafeString("Author");
	}

	#endregion

	#region Properties

	public string IndexPath { get; set; } = string.Empty;

	/// <summary>
	/// same as Key but only populated when retrieving items from the index
	/// </summary>
	public string DocKey { get; private set; } = string.Empty;

	public string Key => $"{SiteId}~{PageId}~{ModuleId}~{ItemKey}{QueryStringAddendum}";

	public int SiteId { get; set; } = -1;

	private bool didLoadPageIdFromLuceneDoc = false;
	private int pageID = -1;
	public int PageId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadPageIdFromLuceneDoc)
			{
				pageID = Convert.ToInt32(luceneDoc.Get("PageID"), CultureInfo.InvariantCulture);
				didLoadPageIdFromLuceneDoc = true;
			}
			return pageID;
		}
		set
		{
			pageID = value;
		}
	}

	private bool didLoadModuleIdFromLuceneDoc = false;

	private int moduleID = -1;
	public int ModuleId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadModuleIdFromLuceneDoc)
			{
				moduleID = Convert.ToInt32(luceneDoc.Get("ModuleID"), CultureInfo.InvariantCulture);
				didLoadModuleIdFromLuceneDoc = true;
			}

			return moduleID;
		}
		set
		{
			moduleID = value;
		}
	}

	private bool didLoadItemIdFromLuceneDoc = false;

	private int itemID = -1;
	public int ItemId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadItemIdFromLuceneDoc)
			{
				itemID = Convert.ToInt32(luceneDoc.Get("ItemID"), CultureInfo.InvariantCulture);
				didLoadItemIdFromLuceneDoc = true;
			}

			return itemID;
		}
		set
		{
			itemID = value;
		}
	}

	private string itemKey = string.Empty;
	public string ItemKey
	{
		get
		{
			if (ItemId > -1)
			{
				return ItemId.ToString(CultureInfo.InvariantCulture);
			}
			return itemKey;
		}
		set
		{
			itemKey = value;
		}
	}

	private bool didLoadPageNumberFromLuceneDoc = false;
	private int pageNumber = 1; // for use in pageable modules like forums
	public int PageNumber
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadPageNumberFromLuceneDoc)
			{
				pageNumber = Convert.ToInt32(luceneDoc.Get("PageNumber"), CultureInfo.InvariantCulture);
				didLoadPageNumberFromLuceneDoc = true;
			}

			return pageNumber;
		}
		set
		{
			pageNumber = value;
		}
	}

	public bool RemoveOnly { get; set; } = false;

	[XmlIgnore]
	public string PageName { get; set; } = string.Empty;

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

	[XmlIgnore]
	public string Url
	{
		get
		{
			if (UseQueryStringParams)
			{
				return ViewPage.ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).ItemId(ItemId).ToString() + QueryStringAddendum;
			}
			else
			{
				return ViewPage.ToLinkBuilder().ToString();
			}
		}
	}

	[XmlIgnore]
	public string LinkText
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Title))
			{
				return PageName + PageNameItemItemSeparator + Title;
			}

			return PageName;
		}
	}

	[XmlIgnore]
	public string PageNameItemItemSeparator { get; set; } = " &gt; ";

	private string pageMetaDescription = string.Empty;
	[XmlIgnore]
	public string PageMetaDescription
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadPageMetaFromLuceneDoc)
			{
				string s = luceneDoc.Get("PageMetaDesc");

				if (!string.IsNullOrWhiteSpace(s))
				{
					pageMetaDescription = s;
				}

				didLoadPageMetaFromLuceneDoc = true;
			}

			return pageMetaDescription;
		}
		set
		{
			pageMetaDescription = value;
		}
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

	private string pageMetaKeywords = string.Empty;
	[XmlIgnore]
	public string PageMetaKeywords
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !didLoadPageMetaKeywordsFromLuceneDoc)
			{
				string s = luceneDoc.Get("Keyword");
				if (!string.IsNullOrWhiteSpace(s))
				{
					pageMetaKeywords = s;
				}
				didLoadPageMetaKeywordsFromLuceneDoc = true;
			}

			return pageMetaKeywords;
		}
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

	private bool didLoadFeatureIdFromLuceneDoc = false;

	private string featureId = Guid.Empty.ToString();
	public string FeatureId
	{
		get
		{
			if (luceneDoc is not null && !didLoadFeatureIdFromLuceneDoc)
			{
				string fid = luceneDoc.Get("FeatureId");
				if (!string.IsNullOrWhiteSpace(fid))
				{
					featureId = fid;
				}
				didLoadFeatureIdFromLuceneDoc = true;
			}
			return featureId;
		}
		set { featureId = value; }
	}

	private bool didLoadFeatureNameFromLuceneDoc = false;

	private string featureName = string.Empty;
	[XmlIgnore]
	public string FeatureName
	{
		get
		{
			if (luceneDoc is not null && !didLoadFeatureNameFromLuceneDoc)
			{
				featureName = luceneDoc.Get("Feature");
				didLoadFeatureNameFromLuceneDoc = true;
			}
			return featureName;
		}
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

	public string FeatureResourceFile { get; set; } = string.Empty;

	[XmlIgnore]
	public string Author { get; set; } = string.Empty;

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

	[XmlIgnore]
	public string Categories { get; set; } = string.Empty;

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
	public string ModuleTitle { get; set; } = string.Empty;

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
	public string Title { get; set; } = string.Empty;

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
	public string Intro { get; set; } = string.Empty;

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
		get
		{
			if (luceneDoc is not null && !didLoadContentAbstrctFromLuceneDoc)
			{
				contentAbstract = luceneDoc.Get("Abstract");
				didLoadContentAbstrctFromLuceneDoc = true;
			}

			return contentAbstract;
		}
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

	private string content = string.Empty;
	[XmlIgnore]
	public string Content
	{
		get
		{
			if (luceneDoc is not null && !didLoadContentFromLuceneDoc)
			{
				content = luceneDoc.Get("contents");
				didLoadContentFromLuceneDoc = true;
			}
			return content;
		}
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
	public string OtherContent { get; set; } = string.Empty;

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

	private string viewRoles = string.Empty;
	[XmlIgnore]
	public string ViewRoles
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !didGetViewRolesFromDoc)
			{
				viewRoles = luceneDoc.Get("ViewRoles");
				didGetViewRolesFromDoc = true; ;
			}
			return viewRoles;
		}
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

	private string moduleViewRoles = string.Empty;
	[XmlIgnore]
	public string ModuleViewRoles
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !didGetModuleViewRolesFromDoc)
			{
				moduleViewRoles = luceneDoc.Get("ModuleRole");
				didGetModuleViewRolesFromDoc = true; ;
			}
			return moduleViewRoles;
		}
		set
		{
			moduleViewRoles = value;
		}
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
	public string ViewPage { get; set; } = "Default.aspx";

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

	public bool UseQueryStringParams { get; set; } = true;

	public string QueryStringAddendum { get; set; } = string.Empty;

	public DateTime PublishBeginDate { get; set; } = DateTime.MinValue;

	public DateTime PublishEndDate { get; set; } = DateTime.MaxValue;

	private bool didLoadCreatedUtcFromLuceneDoc = false;

	private DateTime createdUtc = DateTime.MinValue;
	public DateTime CreatedUtc
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !didLoadCreatedUtcFromLuceneDoc)
			{
				if (DateTime.TryParse(luceneDoc.Get("CreatedUtc"), out DateTime c))
				{
					createdUtc = c;
				}

				didLoadCreatedUtcFromLuceneDoc = true; ;
			}

			return createdUtc;
		}
		set
		{
			createdUtc = value;
		}
	}

	private bool didLoadLastModUtcFromLuceneDoc = false;

	private DateTime lastModUtc = DateTime.MinValue;
	public DateTime LastModUtc
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !didLoadLastModUtcFromLuceneDoc)
			{
				if (DateTime.TryParse(luceneDoc.Get("LastModUtc"), out DateTime c))
				{
					lastModUtc = c;
				}

				didLoadLastModUtcFromLuceneDoc = true; ;
			}

			return lastModUtc;
		}
		set
		{
			lastModUtc = value;
		}
	}

	public bool ExcludeFromRecentContent { get; set; } = false;

	#endregion

	public int CompareTo(object obj)
	{
		if (obj is not IndexItem i)
		{
			return -1;
		}

		// sort descending on LastModUtc
		return i.LastModUtc > LastModUtc ? 1 : -1;
	}
}