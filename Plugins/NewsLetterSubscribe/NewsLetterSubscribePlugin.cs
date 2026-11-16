using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.NewsLetterSubscribe;

public class NewsLetterSubscribePlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("6c358bd7-6b78-4b3f-a56a-b1146dfa4c34");

	public override Guid Id => FeatureGuid;
	public override string Name => "NewsLetterSubscribe";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "NewsletterSignUpFeatureName",
			ControlSource = "Modules/NewsLetterSubscribeModule.ascx",
			SortOrder = 900,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			IsSearchable = false,
			SearchListNameResourceKey = string.Empty,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 75,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterButtonTextSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterButtonTextSetting-help",
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterWatermarkTextSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterWatermarkTextSetting-help",
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterOverrideInputWidthSetting",
			DefaultValue = "0",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterOverrideInputWidthSetting-help",
			SortOrder = 250,
			RegexValidationExpression = @"^[0-9][0-9]{0,3}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterThankYouMessageSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterThankYouMessageSetting-help",
			SortOrder = 300,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterShowListSetting",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterShowListSetting-help",
			SortOrder = 400,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterIncludeDescriptionInListSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterIncludeDescriptionInListSetting-help",
			SortOrder = 500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterShowPreviousEditionLinksSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterShowPreviousEditionLinksSetting-help",
			SortOrder = 550,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterShowFormatOptionsSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterShowFormatOptionsSetting-help",
			SortOrder = 600,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterHtmlIsDefaultSetting",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterHtmlIsDefaultSetting-help",
			SortOrder = 700,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterShowmoreInfoLinkSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterShowmoreInfoLinkSetting-help",
			SortOrder = 800,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "NewsletterMoreInfoTextSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "NewsletterMoreInfoTextSetting-help",
			SortOrder = 900,
			RegexValidationExpression = string.Empty
		});

		return [feature];
	}
}
