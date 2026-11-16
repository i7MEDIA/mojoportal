using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.CustomMenu;

public class CustomMenuPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("b6c34a90-0d79-4c5a-ba3c-75dd0fdfc0ef");

	public override Guid Id => FeatureGuid;
	public override string Name => "CustomMenu";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,FirebirdSql,SqlAzure,SqlCe",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "CustomMenuFeatureName",
			ControlSource = "Modules/CustomMenu.ascx",
			SortOrder = 9000,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SupportsPageReuse = true
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomMenuStartingPage",
			DefaultValue = string.Empty,
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/PageSelectorSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomMenuView",
			DefaultValue = "_CustomMenu",
			ControlType = "CustomField",
			ControlSrc = "~/Controls/ViewSelectorSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 105,
			RegexValidationExpression = string.Empty,
			Attributes = "ViewName|_CustomMenu;ViewPath|Views/Common;"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomMenuShowStartingNode",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 110,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomMenuMaxDepth",
			DefaultValue = "-1",
			ControlType = "Number",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 120,
			RegexValidationExpression = string.Empty,
			Attributes = "min|-1;max|12;"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 130,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
