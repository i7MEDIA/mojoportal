using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web.Controls;

public class OdiogoItem : Panel
{
	const string ItemScriptTemplate = "\n<script data-loader=\"OdiogoItem\">\n<!--\nshowOdiogoReadNowButton ('{0}', '{1}', '{2}', 290, 55);\n//-->\n</script>" +
		"\n<script data-loader=\"OdiogoItem\">\nshowInitialOdiogoReadNowFrame ('{0}', '{2}', 290, 0);\n//-->\n</script>\n";

	public string OdiogoFeedId { get; set; } = string.Empty;

	public string ItemTitle { get; set; } = string.Empty;

	public string ItemId { get; set; } = string.Empty;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		CssClass = "odiogo";
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		SetupMainScript();
		SetupItem();

		Visible = OdiogoFeedId.Length > 0;
	}

	private void SetupItem()
	{
		if (OdiogoFeedId.Length == 0) return;
		if (ItemTitle.Length == 0) return;
		if (ItemId.Length == 0) return;

		var litItem = new Literal
		{
			Text = string.Format(CultureInfo.InvariantCulture,
			ItemScriptTemplate,
			OdiogoFeedId,
			ItemTitle.HtmlEscapeQuotes(),
			ItemId)
		};

		Controls.Add(litItem);
	}

	private void SetupMainScript()
	{
		if (string.IsNullOrWhiteSpace(OdiogoFeedId))
		{
			return;
		}

		Page.ClientScript.RegisterClientScriptBlock(
			typeof(OdiogoItem)
			, $"odiogofeed{OdiogoFeedId}"
			, $"\n<script data-loader=\"OdiogoItem\" src=\"http://podcasts.odiogo.com/odiogo_js.php?feed_id={OdiogoFeedId}&amp;platform=mp\" ></script>");
	}
}