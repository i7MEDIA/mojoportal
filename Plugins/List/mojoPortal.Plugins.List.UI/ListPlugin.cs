using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.List.UI;

public class ListPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("74bdbcc2-0e79-47ff-bcd4-a159270bf36e");

	public override Guid Id => FeatureGuid;
	public override string Name => "List";
	public override string LegacyApplicationName => "linksmodule";
	public override Version Version => new(1, 0, 0, 9);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("74bdbcc2-0e79-47ff-bcd4-a159270bf36e"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
				ResourceFile = "LinkResources",
				FeatureNameReasourceKey = "FeatureName",
				ControlSource = "List/ListModule.ascx",
				SortOrder = 500,
				IsCacheable = true,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = true,
				SearchListNameResourceKey = "LinksSearchName",
				DeleteProvider = "LinksContentDeleteHandler",
				SupportsPageReuse = true,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "IntroContent",
				GroupNameKey = "",
				DefaultValue = "",
				ControlType = "",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 1,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "LinksShowDescriptionSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "Links-ShowDescriptionSetting-help",
				SortOrder = 100,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "LinksShowOnlyDescriptionSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "Links-ShowOnlyDescriptionSetting-help",
				SortOrder = 200,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "LinksEnablePagingSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "Links-EnablePagingSetting-help",
				SortOrder = 300,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "UseAjaxPaging",
				GroupNameKey = "",
				DefaultValue = "true",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "Links-useajaxpaging-help",
				SortOrder = 400,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "LinksPageSizeSetting",
				GroupNameKey = "",
				DefaultValue = "50",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "Links-PageSizeSetting-help",
				SortOrder = 500,
				RegexValidationExpression = "^[1-9][0-9]{0,4}$",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "LinksExtraCssClassSetting",
				GroupNameKey = "",
				DefaultValue = "",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "custom-module-css-class-help",
				SortOrder = 900,
				RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "ShowPageLeftContentSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "Links-ShowPageLeftContentSetting-help",
				SortOrder = 10000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "LinkResources",
				ResourceKey = "ShowPageRightContentSetting",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "LinksShowPageRightContentSetting-help",
				SortOrder = 10100,
				RegexValidationExpression = "",
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
