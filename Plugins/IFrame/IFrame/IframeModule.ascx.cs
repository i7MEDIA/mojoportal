using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Features.UI;

public partial class IframeModule : SiteModuleControl
{
	// FeatureGuid 36d66cea-a047-4411-ab29-1344493b5a33

	private string frameSrc = string.Empty;
	private string frameName = string.Empty;
	private string frameTitle = string.Empty;

	// deprecated - use styles instead
	private string frameAlign = string.Empty; // left, right, top, middle, bottom
	private string frameCss = string.Empty;

	private string frameBorder = "0"; // 0 or 1
	private string frameHeight = "100%";
	private string frameWidth = "100%";
	private string frameMarginHeight = "0";
	private string frameMarginWidth = "0";
	private string frameScrolling = "auto"; // yes, no, auto


	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	#endregion


	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateControls();
	}


	private void PopulateControls()
	{
		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		if (string.IsNullOrWhiteSpace(frameSrc))
		{
			return;
		}

		var attribs = new Dictionary<string, string>();

		if (!string.IsNullOrWhiteSpace(frameName))
		{
			attribs.Add("name", frameName);
		}

		if (!string.IsNullOrWhiteSpace(frameTitle))
		{
			attribs.Add("title", frameTitle);
		}

		attribs.Add("src", frameSrc);

		if (!string.IsNullOrWhiteSpace(frameAlign))
		{
			attribs.Add("align", frameAlign);
		}

		if (!string.IsNullOrWhiteSpace(frameCss))
		{
			attribs.Add("class", frameCss);
		}

		attribs.Add("frameborder", frameBorder);
		attribs.Add("height", frameHeight);
		attribs.Add("width", frameWidth);

		if (!string.IsNullOrWhiteSpace(frameMarginHeight))
		{
			attribs.Add("marginheight", frameMarginHeight);
		}

		if (!string.IsNullOrWhiteSpace(frameMarginWidth))
		{
			attribs.Add("marginwidth", frameMarginWidth);
		}

		attribs.Add("scrolling", frameScrolling);

		var markup = $"<iframe {string.Join(" ", attribs.Select(x => $"{x.Key}=\"{x.Value}\""))}></iframe>";

		litFrame.Text = markup;
	}

	private void LoadSettings()
	{
		frameSrc = WebUtils.ParseStringFromHashtable(Settings, "frameSrc", frameSrc);
		frameName = WebUtils.ParseStringFromHashtable(Settings, "frameName", frameName);
		frameTitle = WebUtils.ParseStringFromHashtable(Settings, "frameTitle", frameTitle);
		frameAlign = WebUtils.ParseStringFromHashtable(Settings, "frameAlign", frameAlign);
		frameCss = WebUtils.ParseStringFromHashtable(Settings, "frameCss", frameCss);
		frameBorder = WebUtils.ParseStringFromHashtable(Settings, "frameBorder", frameBorder);
		frameHeight = WebUtils.ParseStringFromHashtable(Settings, "frameHeight", frameHeight);
		frameWidth = WebUtils.ParseStringFromHashtable(Settings, "frameWidth", frameWidth);
		frameMarginHeight = WebUtils.ParseStringFromHashtable(Settings, "frameMarginHeight", frameMarginHeight);
		frameMarginWidth = WebUtils.ParseStringFromHashtable(Settings, "frameMarginWidth", frameMarginWidth);
		frameScrolling = WebUtils.ParseStringFromHashtable(Settings, "frameScrolling", frameScrolling);
	}
}
