using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI;

public partial class MetaContent : UserControl
{
	//private static readonly ILog log = LogManager.GetLogger(typeof(MetaContent));
	private readonly List<string> keywords = [];
	private SiteSettings siteSettings = null;

	public string KeywordCsv { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	/// <summary>
	/// see http://www.mojoportal.com/Forums/Thread.aspx?thread=9301&mid=34&pageid=5&ItemID=2&pagenumber=1#post38648
	/// </summary>
	public bool AddOpenGraphDescription { get; set; } = true;
	//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12155~1#post50489
	// "<meta property=\"og:description\" content=\"{0}\" />";
	// "<meta name=\"og:description\" content=\"{0}\" />";
	public string OpenGraphDescriptionFormat { get; set; } = "<meta property=\"og:description\" content=\"{0}\" />";
	public string AdditionalMetaMarkup { get; set; } = string.Empty;
	public bool DisableContentType { get; set; } = false;
	public bool IncludeFacebookAppId { get; set; } = true;


	public void AddKeword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			keywords.Add(keyword);
		}
	}

	//protected void Page_Load(object sender, EventArgs e) { }

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (WebConfigSettings.AutoSetContentType)
		{
			AddEncoding();
		}

		AddDescription();
		AddKeywords();

		if (AdditionalMetaMarkup.Length > 0)
		{
			var additionalMeta = new Literal { Text = AdditionalMetaMarkup };
			Controls.Add(additionalMeta);
		}

		siteSettings ??= CacheHelper.GetCurrentSiteSettings();

		if (siteSettings is not null)
		{
			AddOpenSearchLink();
			AddFacebookAppId();
		}
	}


	private void AddKeywords()
	{
		if (!string.IsNullOrWhiteSpace(KeywordCsv))
		{
			keywords.AddRange(KeywordCsv.SplitOnCharAndTrim(','));
		}

		if (keywords.Count > 0)
		{
			Controls.Add(new Literal { Text = $"\n<meta name=\"keywords\" content=\"{string.Join(",", keywords)}\" />" });
		}
	}


	private void AddDescription()
	{
		if (string.IsNullOrWhiteSpace(Description))
		{
			return;
		}

		string metaDescription = $"\n<meta name=\"description\" content=\"{Description}\" />";

		if (AddOpenGraphDescription)
		{
			metaDescription += $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphDescriptionFormat, Description)}";
		}

		Controls.Add(new Literal { Text = metaDescription });
	}


	private void AddEncoding()
	{
		if (DisableContentType)
		{
			return;
		}

		string contentTypeMeta = $"\n<meta http-equiv=\"Content-Type\" content=\"{WebConfigSettings.ContentMimeType}; charset={WebConfigSettings.ContentEncoding}\" />";

		Controls.Add(new Literal { Text = contentTypeMeta });
	}

	private void AddOpenSearchLink()
	{
		if (WebConfigSettings.DisableSearchIndex || WebConfigSettings.DisableOpenSearchAutoDiscovery)
		{
			return;
		}

		string searchTitle = siteSettings.OpenSearchName.Coalesce(string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName));
		
		string openSearchLink = $"\n<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\"{searchTitle}\" href=\"{"SearchEngineInfo.ashx".ToLinkBuilder()}\" />";

		Controls.Add(new Literal { Text = openSearchLink });
	}


	private void AddFacebookAppId()
	{
		if (IncludeFacebookAppId)
		{
			string fbAppId = WebConfigSettings.FacebookAppId.Coalesce(siteSettings.FacebookAppId);
			
			if (!string.IsNullOrWhiteSpace(fbAppId))
			{
				string meta = $"<meta property=\"fb:app_id\" content=\"{siteSettings.FacebookAppId}\"/>";
				Controls.Add(new Literal { Text = meta });
			}
		}
	}
}
