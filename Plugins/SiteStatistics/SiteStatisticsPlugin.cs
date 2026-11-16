using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.SiteStatistics;

public class SiteStatisticsPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("1999e312-0378-4bca-8753-201d17957a74");

	public override Guid Id => FeatureGuid;
	public override string Name => "SiteStatistics";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "SiteStatisticsFeatureName",
			ControlSource = "Modules/SiteStatisticsModule.ascx",
			SortOrder = 910,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "SiteStatistics_ShowMemberStatisitics",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "SiteStatistics_ShowOnlineStatistics",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 110,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "SiteStatistics_ShowOnlineMembers",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 120,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "ShowMemberGraphSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 130,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "MemberGraphHeightSetting",
			DefaultValue = "300",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 140,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "MemberGraphWidthSetting",
			DefaultValue = "500",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 150,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		return [feature];
	}
}
