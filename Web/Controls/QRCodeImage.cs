using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

public class QRCodeImage : Image
{
	private static readonly ILog log = LogManager.GetLogger(typeof(QRCodeImage));

	private readonly string QRCodeGeneratorUrl = ConfigHelper.GetStringProperty("QRCodeGeneratorUrl", "https://api.qrserver.com/v1/create-qr-code/?size={0}x{1}&format={2}&data={3}");

	public int SizeInPixels { get; set; } = 150;
	public string TextToEncode { get; set; } = string.Empty;
	public bool AutoDetectPageUrl { get; set; } = false;
	public bool RemoveSkinParam { get; set; } = true;
	public string ImageFormat { get; set; } = "jpg";

	private void SetTextFromPageUrl()
	{
		if (RemoveSkinParam)
		{
			try
			{
				TextToEncode = FilteredUrl(WebUtils.GetSiteRoot() + Page.Request.RawUrl);
			}
			catch (Exception ex)
			{
				log.Error("handled error ", ex);
			}
		}
		else
		{
			TextToEncode = WebUtils.GetSiteRoot() + Page.Request.RawUrl;
		}
	}


	private string FilteredUrl(string rawUrl)
	{
		if (string.IsNullOrEmpty(rawUrl) || !rawUrl.Contains("?"))
		{
			return rawUrl;
		}

		if (rawUrl.IndexOf("?") == (rawUrl.Length - 1))
		{
			return rawUrl.Replace("?", string.Empty);
		}

		var baseUrl = rawUrl.Substring(0, rawUrl.IndexOf("?"));
		var queryString = rawUrl.Substring(rawUrl.IndexOf("?"), rawUrl.Length - rawUrl.IndexOf("?"));
		queryString = WebUtils.RemoveQueryStringParam(queryString, "skin");

		if (queryString.Length > 0)
		{
			return $"{baseUrl}?{queryString}";
		}

		return baseUrl;
	}


	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (AutoDetectPageUrl)
		{
			SetTextFromPageUrl();
		}

		if (string.IsNullOrWhiteSpace(TextToEncode))
		{
			return;
		}

		ImageUrl = string.Format(CultureInfo.InvariantCulture, QRCodeGeneratorUrl, SizeInPixels, SizeInPixels, ImageFormat, Page.Server.UrlEncode(TextToEncode));
		AlternateText = TextToEncode;
		ToolTip = TextToEncode;
	}


	protected override void Render(HtmlTextWriter writer)
	{
		// don't render if the text to encode has not been set
		if (TextToEncode.Length > 0)
		{
			base.Render(writer);
		}
	}
}