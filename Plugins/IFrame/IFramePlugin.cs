using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.IFrame;

public class IFramePlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("36d66cea-a047-4411-ab29-1344493b5a33");

	public override Guid Id => FeatureGuid;
	public override string Name => "IFrame";
	public override string LegacyApplicationName => "iframe";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "IframeResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "IFrame/IframeModule.ascx",
			SortOrder = 800,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SupportsPageReuse = true
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameSrc",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameSrc-help",
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameName",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameName-help",
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameTitle",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameTitle-help",
			SortOrder = 300,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameAlign",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameAlign-help",
			SortOrder = 400,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameCss",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameCss-help",
			SortOrder = 500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameBorder",
			DefaultValue = "0",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameBorder-help",
			SortOrder = 600,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameHeight",
			DefaultValue = "100%",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameHeight-help",
			SortOrder = 700,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameWidth",
			DefaultValue = "100%",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameWidth-help",
			SortOrder = 800,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameMarginHeight",
			DefaultValue = "0",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameMarginHeight-help",
			SortOrder = 900,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameMarginWidth",
			DefaultValue = "0",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameMarginWidth-help",
			SortOrder = 1000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "IframeResources",
			ResourceKey = "frameScrolling",
			DefaultValue = "Auto",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "frameScrolling-help",
			SortOrder = 1100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 1200,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
