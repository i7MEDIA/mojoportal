using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.ChildPageSiteMap;

public class ChildPageSiteMapPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("557ea1a4-a994-415c-9b42-1bac0d2f32ae");

	public override Guid Id => FeatureGuid;
	public override string Name => "ChildPageSiteMap";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "ChildPageSiteMapFeatureName",
			ControlSource = "Modules/ChildPageSiteMapModule.ascx",
			SortOrder = 9000,
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
			SortOrder = 100,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
