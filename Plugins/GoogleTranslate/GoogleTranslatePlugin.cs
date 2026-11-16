using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.GoogleTranslate;

public class GoogleTranslatePlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("5cb1a666-3de9-47df-8b52-ec0df2deeacf");

	public override Guid Id => FeatureGuid;
	public override string Name => "GoogleTranslate";
	public override string LegacyApplicationName => "googletranslate";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "GTranslateResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "GoogleTranslate/GoogleTranslateModule.ascx",
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
			ResourceFile = "GTranslateResources",
			ResourceKey = "UseSimpleLayout",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "googletranslate-UseSimpleLayout-help",
			SortOrder = 500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GTranslateResources",
			ResourceKey = "ShowToolbar",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "googletranslate-showtoolbar-help",
			SortOrder = 600,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GTranslateResources",
			ResourceKey = "TrackInGoogleAnalytics",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "googletranslate-TrackInGoogleAnalytics-help",
			SortOrder = 700,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GTranslateResources",
			ResourceKey = "AllowOnSecurePages",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "googletranslate-AllowOnSecurePages-help",
			SortOrder = 700,
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
			SortOrder = 800,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
