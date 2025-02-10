using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public partial class MetaContent : UserControl
{
	private readonly List<string> keywords = [];
	private SiteSettings siteSettings = null;

	public string Url { get; set; }
	public string Type { get; set; } = "website";
	public string Title { get; set; }
	public string Description { get; set; } = string.Empty;
	public string Image { get; set; }

	public string KeywordCsv { get; set; } = string.Empty;
	public string AdditionalMetaMarkup { get; set; } = string.Empty;
	public bool DisableContentType { get; set; } = false;
	public bool IncludeFacebookAppId { get; set; } = true;

	public bool AddOpenGraphMetadata { get; set; } = true;

	public string OpenGraphUrlFormat { get; set; } = """<meta property="og:url" content="{0}" />""";
	public string OpenGraphTypeFormat { get; set; } = """<meta property="og:type" content="{0}" />""";
	public string OpenGraphTitleFormat { get; set; } = """<meta property="og:title" content="{0}" />""";
	public string OpenGraphDescriptionFormat { get; set; } = """<meta property="og:description" content="{0}" />""";
	public string OpenGraphImageFormat { get; set; } = """<meta property="og:image" content="{0}" />""";


	public void AddKeword(string keyword)
	{
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			keywords.Add(keyword);
		}
	}


	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (WebConfigSettings.AutoSetContentType)
		{
			AddEncoding();
		}

		AddDescription();
		AddKeywords();

		if (AddOpenGraphMetadata)
		{
			AddOpenGraph();
		}

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

		Controls.Add(new Literal { Text = metaDescription });
	}


	private void AddOpenGraph()
	{

		if (!string.IsNullOrWhiteSpace(Url))
		{
			Controls.Add(new Literal { Text = $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphUrlFormat, Url)}" });
		}

		if (!string.IsNullOrWhiteSpace(Type))
		{
			Controls.Add(new Literal { Text = $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphTypeFormat, Type)}" });
		}

		if (!string.IsNullOrWhiteSpace(Title))
		{
			Controls.Add(new Literal { Text = $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphTitleFormat, Title)}" });
		}

		if (!string.IsNullOrWhiteSpace(Description))
		{
			Controls.Add(new Literal { Text = $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphDescriptionFormat, Description)}" });
		}

		if (!string.IsNullOrWhiteSpace(Image))
		{
			Controls.Add(new Literal { Text = $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphImageFormat, Image)}" });
		}
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
