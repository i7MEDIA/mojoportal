using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.EventCalendar.UI;

public class EventCalendarPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("c5e6a5df-ac2a-43d3-bb7f-9739bc47194e");

	public override Guid Id => FeatureGuid;
	public override string Name => "EventCalendar";
	public override string LegacyApplicationName => "eventcalendar";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature = new ContentFeature
		{
			FeatureGuid = new Guid("c5e6a5df-ac2a-43d3-bb7f-9739bc47194e"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "EventCalResources",
			FeatureNameReasourceKey = "EventCalendarFeatureName",
			ControlSource = "EventCalendar/EventCalendarModule.ascx",
			SortOrder = 620,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = true,
			SearchListNameResourceKey = "EventCalendarSearchName",
			DeleteProvider = "CalendarContentDeleteHandler",
			SupportsPageReuse = false,
			PartialView = "",
			SkinFileName = ""
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "CalendarUseFillerOnEmptyDays",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "ShowTimeInMonthViewSetting",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 102,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "EnableMapSetting",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 104,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "",
			ResourceKey = "",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "min|100;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapWidthSetting",
			GroupNameKey = "",
			DefaultValue = "500",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 120,
			RegexValidationExpression = "^[1-9][0-9]{0,4}[px|%]?",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapEnableMapTypeSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 130,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapEnableZoomSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 140,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapShowInfoWindowSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 150,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapEnableLocalSearchSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 160,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapEnableDirectionsSetting",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 170,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapInitialMapTypeSetting",
			GroupNameKey = "",
			DefaultValue = "G_NORMAL_MAP",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/GMapTypeSetting.ascx",
			HelpKey = "",
			SortOrder = 180,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "GoogleMapInitialZoomSetting",
			GroupNameKey = "",
			DefaultValue = "13",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/GMapZoomLevelSetting.ascx",
			HelpKey = "",
			SortOrder = 190,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "EventCalResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 300,
			RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		features.Add(feature);

		return features;
	}
}

