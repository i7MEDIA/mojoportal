using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.Poll.UI;

public class PollPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("0d0d0518-bfb6-480c-b34f-4d8c8e947f7c");

	public override Guid Id => FeatureGuid;
	public override string Name => "Poll";
	public override string LegacyApplicationName => "pollfeature-byChristianFredh";
	public override Version Version => new(0, 0, 0, 5);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("0d0d0518-bfb6-480c-b34f-4d8c8e947f7c"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
				ResourceFile = "PollResources",
				FeatureNameReasourceKey = "PollFeatureName",
				ControlSource = "Poll/PollModule.ascx",
				SortOrder = 500,
				IsCacheable = false,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = false,
				SearchListNameResourceKey = "",
				DeleteProvider = "PollContentDeleteHandler",
				SupportsPageReuse = true,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "PollResources",
				ResourceKey = "PollShowMyPollHistoryButtonSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 300,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "PollResources",
				ResourceKey = "ResultBarColor",
				GroupNameKey = "",
				DefaultValue = "#3082af",
				ControlType = "color",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 400,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "PollResources",
				ResourceKey = "CustomCssClassSetting",
				GroupNameKey = "",
				DefaultValue = "",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "custom-module-css-class-help",
				SortOrder = 500,
				RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			features.Add(feature);
		}

		return features;
	}
}
