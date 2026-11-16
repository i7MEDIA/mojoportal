using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.HtmlInclude;

public class HtmlIncludePlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("f193cce1-6e28-4b73-88e7-e00cd1d642c9");

	public override Guid Id => FeatureGuid;
	public override string Name => "HtmlInclude";
	public override string LegacyApplicationName => "htmlinclude";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "HtmlIncludeResources",
			FeatureNameReasourceKey = "HtmlFragmentIncludeFeatureName",
			ControlSource = "HtmlInclude/HtmlFragmentInclude.ascx",
			SortOrder = 950,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "HtmlIncludeResources",
			ResourceKey = "HtmlFragmentSourceSetting",
			DefaultValue = "fragment1.htm",
			ControlType = "TextBox",
			SortOrder = 100
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "HtmlIncludeResources",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 200,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
