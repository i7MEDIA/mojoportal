using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.ELetterUI;

public partial class NewsLetterSubscribeModuleModule : SiteModuleControl
{
	// FeatureGuid 6c358bd7-6b78-4b3f-a56a-b1146dfa4c34

	private string NewsletterButtonTextSetting = string.Empty;
	private string NewsletterWatermarkTextSetting = string.Empty;
	private string NewsletterThankYouMessageSetting = string.Empty;
	private bool NewsletterShowListSetting = true;
	private bool NewsletterIncludeDescriptionInListSetting = false;
	private bool NewsletterShowFormatOptionsSetting = false;
	private bool NewsletterHtmlIsDefaultSetting = true;
	private bool NewsletterShowmoreInfoLinkSetting = false;
	private string NewsletterMoreInfoTextSetting = string.Empty;
	private bool NewsletterShowPreviousEditionLinksSetting = false;
	private int NewsletterOverrideInputWidthSetting = 0;
	private string CustomCssClassSetting = string.Empty;

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
		
		pnlOuterWrap.SetOrAppendCss(CustomCssClassSetting);

		// make this usique in case multiple instances on the same page
		subscribe1.ValidationGroup = $"subscribe{ModuleId.ToInvariantString()}";

		if (!string.IsNullOrWhiteSpace(NewsletterButtonTextSetting))
		{
			subscribe1.ButtonText = NewsletterButtonTextSetting;
		}

		if (!string.IsNullOrWhiteSpace(NewsletterWatermarkTextSetting))
		{
			subscribe1.WatermarkText = NewsletterWatermarkTextSetting;
		}

		if (!string.IsNullOrWhiteSpace(NewsletterThankYouMessageSetting))
		{
			subscribe1.ThankYouMessage = NewsletterThankYouMessageSetting;
		}

		if (!string.IsNullOrWhiteSpace(NewsletterMoreInfoTextSetting))
		{
			subscribe1.MoreInfoText = NewsletterMoreInfoTextSetting;
		}

		subscribe1.ShowList = NewsletterShowListSetting;
		subscribe1.IncludeDescriptionInList = NewsletterIncludeDescriptionInListSetting;
		subscribe1.ShowFormatOptions = NewsletterShowFormatOptionsSetting;
		subscribe1.HtmlIsDefault = NewsletterHtmlIsDefaultSetting;
		subscribe1.ShowMoreInfoLink = NewsletterShowmoreInfoLinkSetting;
		subscribe1.ShowPreviousEditionsLink = NewsletterShowPreviousEditionLinksSetting;
		subscribe1.OverrideInputWidth = NewsletterOverrideInputWidthSetting;
	}

	private void LoadSettings()
	{
		NewsletterButtonTextSetting = Settings.ParseString("NewsletterButtonTextSetting", NewsletterButtonTextSetting);
		NewsletterWatermarkTextSetting = Settings.ParseString("NewsletterWatermarkTextSetting", NewsletterWatermarkTextSetting);
		NewsletterThankYouMessageSetting = Settings.ParseString("NewsletterThankYouMessageSetting", NewsletterThankYouMessageSetting);
		NewsletterMoreInfoTextSetting = Settings.ParseString("NewsletterMoreInfoTextSetting", NewsletterMoreInfoTextSetting);
		CustomCssClassSetting = Settings.ParseString("CustomCssClassSetting", CustomCssClassSetting);
		NewsletterShowListSetting = Settings.ParseBool("NewsletterShowListSetting", NewsletterShowListSetting);
		NewsletterIncludeDescriptionInListSetting = Settings.ParseBool("NewsletterIncludeDescriptionInListSetting", NewsletterIncludeDescriptionInListSetting);
		NewsletterShowFormatOptionsSetting = Settings.ParseBool("NewsletterShowFormatOptionsSetting", NewsletterShowFormatOptionsSetting);
		NewsletterHtmlIsDefaultSetting = Settings.ParseBool("NewsletterHtmlIsDefaultSetting", NewsletterHtmlIsDefaultSetting);
		NewsletterShowmoreInfoLinkSetting = Settings.ParseBool("NewsletterShowmoreInfoLinkSetting", NewsletterShowmoreInfoLinkSetting);
		NewsletterShowPreviousEditionLinksSetting = Settings.ParseBool("NewsletterShowPreviousEditionLinksSetting", NewsletterShowPreviousEditionLinksSetting);
		NewsletterOverrideInputWidthSetting = Settings.ParseInt32("NewsletterOverrideInputWidthSetting", NewsletterOverrideInputWidthSetting);

	}

	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}
	#endregion
}