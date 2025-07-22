#nullable enable
using Lucene.Net.Documents;
using mojoPortal.Web;
using System;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace mojoPortal.SearchIndex;

[Serializable()]
public class IndexItem : IComparable
{
	private readonly Document? luceneDoc = null;


	#region Constructors

	public IndexItem() { }


	public IndexItem(Document doc, float score)
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
		ItemImage = luceneDoc.GetNullSafeString(nameof(ItemImage));
	}

	#endregion


	#region Properties

	public string IndexPath { get; set; } = string.Empty;
	/// <summary>
	/// same as Key but only populated when retrieving items from the index
	/// </summary>
	public string DocKey { get; set; } = string.Empty;
	public string Key => $"{SiteId}~{PageId}~{ModuleId}~{ItemKey}{QueryStringAddendum}";
	public int SiteId { get; set; } = -1;


	private int pageID = -1;
	private bool isPageIdLoaded = false;

	public int PageId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isPageIdLoaded)
			{
				pageID = Convert.ToInt32(luceneDoc.Get("PageID"), CultureInfo.InvariantCulture);
				isPageIdLoaded = true;
			}
			return pageID;
		}
		set => pageID = value;
	}


	private int moduleID = -1;
	private bool isModuleIdLoaded = false;

	public int ModuleId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isModuleIdLoaded)
			{
				moduleID = Convert.ToInt32(luceneDoc.Get("ModuleID"), CultureInfo.InvariantCulture);
				isModuleIdLoaded = true;
			}

			return moduleID;
		}
		set => moduleID = value;
	}


	private int itemID = -1;
	private bool isItemIdLoaded = false;

	public int ItemId
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isItemIdLoaded)
			{
				itemID = Convert.ToInt32(luceneDoc.Get("ItemID"), CultureInfo.InvariantCulture);
				isItemIdLoaded = true;
			}

			return itemID;
		}
		set => itemID = value;
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
		set => itemKey = value;
	}


	private int pageNumber = 1; // for use in pageable modules like forums
	private bool isPageNumberLoaded = false;

	public int PageNumber
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isPageNumberLoaded)
			{
				pageNumber = Convert.ToInt32(luceneDoc.Get("PageNumber"), CultureInfo.InvariantCulture);
				isPageNumberLoaded = true;
			}

			return pageNumber;
		}
		set => pageNumber = value;
	}

	public bool RemoveOnly { get; set; } = false;


	[XmlIgnore]
	public string PageName { get; set; } = string.Empty;

	// This is needed to support xml serialization, string with special characters can cause invalid xml, base 64 encoding them gets around the problem.


	[XmlElement(ElementName = "pageName", DataType = "base64Binary")]
	public byte[] PageNameSerialization
	{
		get => Encoding.Unicode.GetBytes(PageName);
		set => PageName = Encoding.Unicode.GetString(value);
	}


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
	private bool isPageMetaDescriptionLoaded = false;

	[XmlIgnore]
	public string PageMetaDescription
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isPageMetaDescriptionLoaded)
			{
				string s = luceneDoc.Get("PageMetaDesc");

				if (!string.IsNullOrWhiteSpace(s))
				{
					pageMetaDescription = s;
				}

				isPageMetaDescriptionLoaded = true;
			}

			return pageMetaDescription;
		}
		set => pageMetaDescription = value;
	}


	[XmlElement(ElementName = "pageMetaDescription", DataType = "base64Binary")]
	public byte[] PageMetaDescriptionSerialization
	{
		get => Encoding.Unicode.GetBytes(PageMetaDescription);
		set => PageMetaDescription = Encoding.Unicode.GetString(value);
	}


	private string pageMetaKeywords = string.Empty;
	private bool isPageMetaKeywordsLoaded = false;

	[XmlIgnore]
	public string PageMetaKeywords
	{
		get
		{
			//lazy load
			if (luceneDoc is not null && !isPageMetaKeywordsLoaded)
			{
				string s = luceneDoc.Get("Keyword");
				if (!string.IsNullOrWhiteSpace(s))
				{
					pageMetaKeywords = s;
				}
				isPageMetaKeywordsLoaded = true;
			}

			return pageMetaKeywords;
		}
		set => pageMetaKeywords = value;
	}


	[XmlElement(ElementName = "pageMetaKeywords", DataType = "base64Binary")]
	public byte[] PageMetaKeywordsSerialization
	{
		get => Encoding.Unicode.GetBytes(PageMetaKeywords);
		set => PageMetaKeywords = Encoding.Unicode.GetString(value);
	}


	private string featureId = Guid.Empty.ToString();
	private bool isFeatureIdLoaded = false;

	public string FeatureId
	{
		get
		{
			if (luceneDoc is not null && !isFeatureIdLoaded)
			{
				string fid = luceneDoc.Get("FeatureId");
				if (!string.IsNullOrWhiteSpace(fid))
				{
					featureId = fid;
				}
				isFeatureIdLoaded = true;
			}
			return featureId;
		}
		set => featureId = value;
	}


	private string featureName = string.Empty;
	private bool isFeatureNameLoaded = false;

	[XmlIgnore]
	public string FeatureName
	{
		get
		{
			if (luceneDoc is not null && !isFeatureNameLoaded)
			{
				featureName = luceneDoc.Get("Feature");
				isFeatureNameLoaded = true;
			}
			return featureName;
		}
		set => featureName = value;
	}


	[XmlElement(ElementName = "featureName", DataType = "base64Binary")]
	public byte[] FeatureNameSerialization
	{
		get => Encoding.Unicode.GetBytes(FeatureName);
		set => FeatureName = Encoding.Unicode.GetString(value);
	}


	public string FeatureResourceFile { get; set; } = string.Empty;


	[XmlIgnore]
	public string Author { get; set; } = string.Empty;


	[XmlElement(ElementName = "author", DataType = "base64Binary")]
	public byte[] AuthorSerialization
	{
		get => Encoding.Unicode.GetBytes(Author);
		set => Author = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string Categories { get; set; } = string.Empty;


	[XmlElement(ElementName = "categories", DataType = "base64Binary")]
	public byte[] CategoriesSerialization
	{
		get => Encoding.Unicode.GetBytes(Categories);
		set => Categories = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string ModuleTitle { get; set; } = string.Empty;


	[XmlElement(ElementName = "moduleTitle", DataType = "base64Binary")]
	public byte[] ModuleTitleSerialization
	{
		get => Encoding.Unicode.GetBytes(ModuleTitle);
		set => ModuleTitle = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string Title { get; set; } = string.Empty;


	[XmlElement(ElementName = "title", DataType = "base64Binary")]
	public byte[] TitleSerialization
	{
		get => Encoding.Unicode.GetBytes(Title);
		set => Title = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string Intro { get; set; } = string.Empty;


	[XmlElement(ElementName = "intro", DataType = "base64Binary")]
	public byte[] IntroSerialization
	{
		get => Encoding.Unicode.GetBytes(Intro);
		set => Intro = Encoding.Unicode.GetString(value);
	}


	private string contentAbstract = string.Empty;
	private bool isContentAbstractLoaded = false;

	[XmlIgnore]
	public string ContentAbstract
	{
		get
		{
			if (luceneDoc is not null && !isContentAbstractLoaded)
			{
				contentAbstract = luceneDoc.Get("Abstract");
				isContentAbstractLoaded = true;
			}

			return contentAbstract;
		}
		set => contentAbstract = value;
	}


	[XmlElement(ElementName = "contentAbstract", DataType = "base64Binary")]
	public byte[] ContentAbstractSerialization
	{
		get => Encoding.Unicode.GetBytes(ContentAbstract);
		set => ContentAbstract = Encoding.Unicode.GetString(value);
	}


	private string content = string.Empty;
	private bool isContentLoaded = false;

	[XmlIgnore]
	public string Content
	{
		get
		{
			if (luceneDoc is not null && !isContentLoaded)
			{
				content = luceneDoc.Get("contents");
				isContentLoaded = true;
			}
			return content;
		}
		set => content = value;
	}


	[XmlElement(ElementName = "content", DataType = "base64Binary")]
	public byte[] ContentSerialization
	{
		get => Encoding.Unicode.GetBytes(Content);
		set => Content = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string OtherContent { get; set; } = string.Empty;


	[XmlElement(ElementName = "otherContent", DataType = "base64Binary")]
	public byte[] OtherContentSerialization
	{
		get => Encoding.Unicode.GetBytes(OtherContent);
		set => OtherContent = Encoding.Unicode.GetString(value);
	}


	private string viewRoles = string.Empty;
	private bool isViewRolesLoaded = false;

	[XmlIgnore]
	public string ViewRoles
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !isViewRolesLoaded)
			{
				viewRoles = luceneDoc.Get("ViewRoles");
				isViewRolesLoaded = true; ;
			}
			return viewRoles;
		}
		set => viewRoles = value;
	}


	[XmlElement(ElementName = "viewRoles", DataType = "base64Binary")]
	public byte[] ViewRolesSerialization
	{
		get => Encoding.Unicode.GetBytes(ViewRoles);
		set => ViewRoles = Encoding.Unicode.GetString(value);
	}


	private string moduleViewRoles = string.Empty;
	private bool isModuleViewRolesLoaded = false;

	[XmlIgnore]
	public string ModuleViewRoles
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !isModuleViewRolesLoaded)
			{
				moduleViewRoles = luceneDoc.Get("ModuleRole");
				isModuleViewRolesLoaded = true; ;
			}
			return moduleViewRoles;
		}
		set => moduleViewRoles = value;
	}


	[XmlElement(ElementName = "moduleViewRoles", DataType = "base64Binary")]
	public byte[] ModuleViewRolesSerialization
	{
		get => Encoding.Unicode.GetBytes(ModuleViewRoles);
		set => ModuleViewRoles = Encoding.Unicode.GetString(value);
	}


	[XmlIgnore]
	public string ViewPage { get; set; } = "Default.aspx";


	[XmlElement(ElementName = "viewPage", DataType = "base64Binary")]
	public byte[] ViewPageSerialization
	{
		get => Encoding.Unicode.GetBytes(ViewPage);
		set => ViewPage = Encoding.Unicode.GetString(value);
	}

	public bool UseQueryStringParams { get; set; } = true;
	public string QueryStringAddendum { get; set; } = string.Empty;
	public DateTime PublishBeginDate { get; set; } = DateTime.MinValue;
	public DateTime PublishEndDate { get; set; } = DateTime.MaxValue;

 
	private DateTime createdUtc = DateTime.MinValue;
	private bool isCreatedUtcLoaded = false;

	public DateTime CreatedUtc
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !isCreatedUtcLoaded)
			{
				if (DateTime.TryParse(luceneDoc.Get("CreatedUtc"), out DateTime c))
				{
					createdUtc = c;
				}

				isCreatedUtcLoaded = true; ;
			}

			return createdUtc;
		}
		set => createdUtc = value;
	}


	private DateTime lastModUtc = DateTime.MinValue;
	private bool isLastModUtcLoaded = false;

	public DateTime LastModUtc
	{
		get
		{
			// lazyLoad
			if (luceneDoc is not null && !isLastModUtcLoaded)
			{
				if (DateTime.TryParse(luceneDoc.Get("LastModUtc"), out DateTime c))
				{
					lastModUtc = c;
				}

				isLastModUtcLoaded = true; ;
			}

			return lastModUtc;
		}
		set => lastModUtc = value;
	}

	public bool ExcludeFromRecentContent { get; set; } = false;


	[XmlIgnore]
	public string ItemImage { get; set; } = string.Empty;


	[XmlElement(ElementName = "ItemImage", DataType = "base64Binary")]
	public byte[] ItemImageSerialization
	{
		get => Encoding.Unicode.GetBytes(ItemImage);
		set => ItemImage = Encoding.Unicode.GetString(value);
	}

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
