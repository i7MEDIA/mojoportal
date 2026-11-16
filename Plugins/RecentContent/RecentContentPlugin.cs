using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.RecentContent;

public class RecentContentPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("f889c7c8-78e1-4cd2-a4cd-a0723f9a7cf0");

	public override Guid Id => FeatureGuid;
	public override string Name => "RecentContent";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "RecentContentFeatureName",
			ControlSource = "Modules/RecentContentModule.ascx",
			SortOrder = 9000,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			IsSearchable = false,
			SearchListNameResourceKey = string.Empty,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "SearchableFeature",
			DefaultValue = "881e4e00-93e4-444c-b7b0-6672fb55de10,026cbead-2b80-4491-906d-b83e37179ccf,38aa5a84-9f5c-42eb-8f4c-105983d419fb,74bdbcc2-0e79-47ff-bcd4-a159270bf36e,dc873d76-5bf2-4ac5-bff7-434a87a3fc8e,d572f6b4-d0ed-465d-ad60-60433893b401,5a343d88-bce1-43d1-98ae-b42d77893e7b,0cefbf18-56de-11dc-8f36-bac755d89593",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/SearchableFeatureFilterSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "MaxRecentItemsToGet",
			DefaultValue = "5",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 110,
			RegexValidationExpression = @"^[1-9][0-9]{0,2}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "MaxDaysOldRecentItemsToGet",
			DefaultValue = "30",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 120,
			RegexValidationExpression = @"^[1-9][0-9]{0,2}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "RecentContentUseCreatedDate",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-UseCreatedDate-help",
			SortOrder = 130,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ShowExcerpt",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-UseCreatedDate-help",
			SortOrder = 140,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ShowAuthor",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-UseCreatedDate-help",
			SortOrder = 150,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ShowCreatedDate",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-UseCreatedDate-help",
			SortOrder = 160,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ShowLastModDate",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-UseCreatedDate-help",
			SortOrder = 170,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 180,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "EnableFeed",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "RecentContent-EnableFeed-help",
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "ShowFeedLink",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 210,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedCacheTimeInMinutes",
			DefaultValue = "10",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedCacheTimeInMinutes-help",
			SortOrder = 220,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedTimeToLiveInMinutes",
			DefaultValue = "10",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedTimeToLiveInMinutes-help",
			SortOrder = 230,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedChannelTitle",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedChannelTitle-help",
			SortOrder = 240,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedChannelDescription",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedChannelDescription-help",
			SortOrder = 250,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedChannelCopyright",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedChannelCopyright-help",
			SortOrder = 260,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedChannelManagingEditor",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FeedChannelManagingEditor-help",
			SortOrder = 270,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "FeedSettings",
			ResourceKey = "FeedburnerFeedUrl",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "BlogFeedburnerFeedUrl-help",
			SortOrder = 280,
			RegexValidationExpression = string.Empty
		});

		return [feature];
	}
}
