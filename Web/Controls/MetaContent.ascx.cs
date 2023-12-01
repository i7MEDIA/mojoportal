using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI;

public partial class MetaContent : UserControl
{
	//private static readonly ILog log = LogManager.GetLogger(typeof(MetaContent));
	private StringBuilder keywords = null;
	private SiteSettings siteSettings = null;

	public bool PreZoomForIPhone { get; set; } = true;
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
		if (keyword == null) { return; }

		if (keywords == null)
		{
			keywords = new StringBuilder();
			keywords.Append(keyword);
			return;
		}

		if (keywords.Length > 0)
		{
			keywords.Append("," + keyword);
		}
		else
		{
			keywords.Append(keyword);
		}
	}

	protected void Page_Load(object sender, EventArgs e) { }

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (WebConfigSettings.AutoSetContentType) { AddEncoding(); }

		AddDescription();
		AddKeywords();

		if (AdditionalMetaMarkup.Length > 0)
		{
			Literal additionalMeta = new Literal();
			additionalMeta.Text = AdditionalMetaMarkup;
			Controls.Add(additionalMeta);
		}

		AddOpenSearchLink();
		AddIPhoneZoom();

		if (IncludeFacebookAppId)
		{
			AddFacebookAppId();
		}
	}

	private void AddIPhoneZoom()
	{
		//TODO: Skin Config: Get this from skin config
		if (!PreZoomForIPhone) { return; }
		if (HttpContext.Current == null) { return; }
		if (Request.UserAgent == null) { return; }
		if (Request.UserAgent.Length == 0) { return; }
		if (!Request.UserAgent.Contains("iPhone")) { return; }

		//http://developer.apple.com/library/ios/#technotes/tn2010/tn2262/_index.html
		//<meta name="viewport" content="width=device-width" />

		Controls.Add(new Literal { Text = "\n<meta name=\"viewport\" content=\"width=670, initial-scale=0.45, minimum-scale=0.45\" />" });
	}

	private void AddKeywords()
	{
		if (keywords != null)
		{
			if (KeywordCsv.Length > 0)
			{
				KeywordCsv = KeywordCsv + "," + keywords.ToString();
			}
			else
			{
				KeywordCsv = keywords.ToString();
			}
		}

		if (KeywordCsv.Length == 0) { return; }

		Controls.Add(new Literal { Text = $"\n<meta name=\"keywords\" content=\"{KeywordCsv}\" />" });
	}

	private void AddDescription()
	{
		if (Description.Length == 0) { return; }
				
		string metaDescription = $"\n<meta name=\"description\" content=\"{Description}\" />";
		
		if (AddOpenGraphDescription)
		{
			metaDescription += $"\n{string.Format(CultureInfo.InvariantCulture, OpenGraphDescriptionFormat, Description)}";
		}

		Controls.Add(new Literal { Text = metaDescription });
	}

	private void AddEncoding()
	{
		if (DisableContentType) { return; }

		string contentTypeMeta = $"\n<meta http-equiv=\"Content-Type\" content=\"{WebConfigSettings.ContentMimeType}; charset={WebConfigSettings.ContentEncoding}\" />";

		Controls.Add(new Literal { Text = contentTypeMeta });
	}

	private void AddOpenSearchLink()
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }
		if (WebConfigSettings.DisableOpenSearchAutoDiscovery) { return; }

		siteSettings ??= CacheHelper.GetCurrentSiteSettings();
		if (siteSettings == null) { return; }

		string searchTitle;
		if (siteSettings.OpenSearchName.Length > 0)
		{
			searchTitle = siteSettings.OpenSearchName;
		}
		else
		{
			searchTitle = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);
		}

		string openSearchLink = $"\n<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\"{searchTitle}\" href=\"{SiteUtils.GetNavigationSiteRoot()}/SearchEngineInfo.ashx\" />";

		Controls.Add(new Literal { Text = openSearchLink });
	}

	private void AddFacebookAppId()
	{
		string fbAppId;
		if (WebConfigSettings.FacebookAppId.Length > 0)
		{
			fbAppId = WebConfigSettings.FacebookAppId;
		}
		else
		{
			siteSettings ??= CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null) { return; }
			fbAppId = siteSettings.FacebookAppId;
		}

		if (fbAppId.Length == 0) { return; }

		string meta = $"<meta property=\"fb:app_id\" content=\"{siteSettings.FacebookAppId}\"/>";

		Controls.Add(new Literal { Text = meta });
	}
}
