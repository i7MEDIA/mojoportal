using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.GoogleMap;

public class GoogleMapPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("5219a554-d176-481e-abc0-9ea2ee4c8efc");

	public override Guid Id => FeatureGuid;
	public override string Name => "GoogleMap";
	public override string LegacyApplicationName => "googlemap";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "GMapResources",
			FeatureNameReasourceKey = "GoogleMapFeatureName",
			ControlSource = "GoogleMap/GoogleMapModule.ascx",
			SortOrder = 600,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapLocationSetting",
			DefaultValue = "Country Club Plaza, Broadway Boulevard, Kansas City, MO",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "googlemap-location-help",
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapCaptionSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 110,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapUseIframeSetting",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "googlemap-useiframe-help",
			SortOrder = 111,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapInitialMapTypeSetting",
			DefaultValue = "r16by9",
			ControlType = "DropDownList",
			HelpKey = string.Empty,
			SortOrder = 112,
			RegexValidationExpression = string.Empty,
			Options = "Resource:Ratio16by9|r16by9;Resource:Ratio4by3|r4by3;"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapHeightSetting",
			DefaultValue = "300",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 120,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapWidthSetting",
			DefaultValue = "500",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 130,
			RegexValidationExpression = "^[1-9][0-9]{0,4}[px|%]?"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapEnableZoomSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 150,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapShowInfoWindowSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 160,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapEnableLocalSearchSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 170,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapEnableDirectionsSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 180,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapUseLocationForTitleSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 190,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapShowLocationCaptionSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapEnableMapTypeSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 205,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapInitialMapTypeSetting",
			DefaultValue = "G_NORMAL_MAP",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/GMapTypeSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 210,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "GoogleMapInitialZoomSetting",
			DefaultValue = "13",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/GMapZoomLevelSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 220,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GMapResources",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 230,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
