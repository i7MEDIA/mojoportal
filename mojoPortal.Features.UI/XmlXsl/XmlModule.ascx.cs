using System;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.XmlUI;


public partial class XmlModule : SiteModuleControl
{
	private static readonly ILog log = LogManager.GetLogger(typeof(XmlModule));

	private XmlConfiguration config = new();

	protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;

	private string xmlBasePath = string.Empty;
	private string xslBasePath = string.Empty;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}


	private void PopulateControls()
	{
		Title1.EditUrl = "XmlXsl/XmlEdit.aspx".ToLinkBuilder().ToString();
		Title1.EditText = XmlResources.XmlEditButton;

		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
		}

		pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);

		var xmlUrl = string.Empty;
		var xslUrl = string.Empty;

		if (!string.IsNullOrWhiteSpace(config.XmlFileSource))
		{
			xmlUrl = WebUtils.ResolveServerUrl(xmlBasePath + config.XmlFileSource);
		}

		if (!string.IsNullOrWhiteSpace(config.XslFileSource))
		{
			xslUrl = WebUtils.ResolveServerUrl(xslBasePath + config.XslFileSource);
		}

		if (!string.IsNullOrWhiteSpace(config.XmlUrl))
		{
			xmlUrl = config.XmlUrl;
		}

		if (!string.IsNullOrWhiteSpace(config.XslUrl))
		{
			xslUrl = config.XslUrl;
		}

		Literal litContent;

		if (config.TrustContent)
		{
			litContent = litTrustedContent;
		}
		else
		{
			litContent = litUnTrustedContent;
		}

		if (
			!string.IsNullOrWhiteSpace(xmlUrl) &&
			!string.IsNullOrWhiteSpace(xslUrl)
		)
		{
			try
			{
				litContent.Text = XmlHelper.TransformXML(xmlUrl, xslUrl);

				if (litContent.Text.Length == 0)
				{
					//probably exception swallowed by XmlHelper
					litContent.Text = XmlResources.GenericError;
				}
			}
			catch (Exception ex)
			{
				log.Info("swallowed excpetion to keep from breaking the page", ex);
				litContent.Text = XmlResources.GenericError;
			}
		}
	}


	private void LoadSettings()
	{
		config = new XmlConfiguration(Settings);

		if (WebConfigSettings.XmlUseMediaFolder)
		{
			xmlBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/media/xml/");
			xslBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/media/xsl/");
		}
		else
		{
			xmlBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/xml/");
			xslBasePath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/xsl/");
		}


		if (config.AllowExternalImages) allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;
		UntrustedContent1.TrustedImageUrlPattern = allowedImageUrlRegexPattern;

	}

}
