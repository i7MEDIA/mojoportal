using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Core.Extensions;
using Resources;

namespace mojoPortal.Web.UI;

/// <summary>
/// http://www.microsofttranslator.com/widget/
/// </summary>
public class MsTranslatorControl : WebControl
{
	private string divId = "MicrosoftTranslatorWidget";
	private string mode = "manual";

	/// <summary>
	/// manual, notify, or auto
	/// </summary>
	public string Mode
	{
		get { return mode; }
		set { mode = value; }
	}

	private string fromLanguageCode = string.Empty;

	public string FromLanguageCode
	{
		get { return fromLanguageCode; }
		set { fromLanguageCode = value; }
	}

	private string layout = "ts";
	/// <summary>
	/// not sure what other settings are allowed here, ts is the default
	/// </summary>
	public string Layout
	{
		get { return layout; }
		set { layout = value; }
	}

	private int pixelWidth = 200;
	public int PixelWidth
	{
		get { return pixelWidth; }
		set { pixelWidth = value; }
	}

	private int minHeight = 83;
	public int MinHeight
	{
		get { return minHeight; }
		set { minHeight = value; }
	}

	private string borderColor = "#4C8D2C";

	public string WidgetBorderColor
	{
		get { return borderColor; }
		set { borderColor = value; }
	}

	private string backgroundColor = "#9FC28E";

	public string BackgroundColor
	{
		get { return backgroundColor; }
		set { backgroundColor = value; }
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		if (fromLanguageCode.Length == 0)
		{
			CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
			fromLanguageCode = defaultCulture.TwoLetterISOLanguageName;
		}

	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null) { return; }

		//base.Render(writer);
		//if (string.IsNullOrEmpty(customSearchId)) { return; }
		if (string.IsNullOrEmpty(divId)) { return; }

		writer.Write("<div id=\"" + divId + "\"");
		writer.Write("style=\"width: " + pixelWidth.ToInvariantString() + "px; ");
		writer.Write("min-height: " + minHeight.ToInvariantString() + "px; ");
		writer.Write("border-color: " + borderColor + "; ");
		writer.Write("background-color: " + backgroundColor + ";");
		writer.Write("\"");
		writer.Write(">");

		writer.Write("<noscript>");


		writer.Write("<a href=\"http://www.microsofttranslator.com/bv.aspx?a=" + Page.Server.UrlEncode(Page.Request.Url.ToString()) + "\">" + Resource.TranslateThisPage + "</a>");

		writer.Write("<br />Powered by <a href=\"http://www.microsofttranslator.com\">Microsoft® Translator</a>");
		writer.Write("</noscript>");

		writer.Write("</div>");

		writer.Write("<script type=\"text/javascript\"> /* <![CDATA[ */ ");

		writer.Write("setTimeout(function() { var s = document.createElement(\"script\");");
		writer.Write("s.type = \"text/javascript\"; ");
		writer.Write("s.charset = \"UTF-8\"; ");
		writer.Write("s.src = \"http://www.microsofttranslator.com/Ajax/V2/Widget.aspx?mode=" + mode + "&from=" + fromLanguageCode + "&layout=" + layout + "\"; ");
		writer.Write("var p = document.getElementsByTagName('head')[0] || document.documentElement; ");
		writer.Write("p.insertBefore(s, p.firstChild); ");
		writer.Write("}, 0); ");

		writer.Write(" /* ]]> */ </script>");


	}


}