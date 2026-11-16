using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.SharedFiles.UI;

public class SharedFilesPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("dc873d76-5bf2-4ac5-bff7-434a87a3fc8e");

	public override Guid Id => FeatureGuid;
	public override string Name => "SharedFiles";
	public override string LegacyApplicationName => "sharedfiles";
	public override Version Version => new(1, 1, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("dc873d76-5bf2-4ac5-bff7-434a87a3fc8e"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
				ResourceFile = "SharedFileResources",
				FeatureNameReasourceKey = "SharedFilesFeatureName",
				ControlSource = "SharedFiles/SharedFilesModule.ascx",
				SortOrder = 500,
				IsCacheable = false,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = true,
				SearchListNameResourceKey = "SharedFilesSearchName",
				DeleteProvider = "SharedFilesContentDeleteHandler",
				SupportsPageReuse = true,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "SharedFilesEnableVersioningSetting",
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
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowDownloadCountToAllUsers",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "SharedFiles-ShowDownloadCountToAllUsers-help",
				SortOrder = 200,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowDescription",
				GroupNameKey = "",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "SharedFiles-ShowDescription-help",
				SortOrder = 300,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowSize",
				GroupNameKey = "",
				DefaultValue = "true",
				ControlType = "CheckBox",
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
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowModified",
				GroupNameKey = "",
				DefaultValue = "true",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 500,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowUploadedBy",
				GroupNameKey = "",
				DefaultValue = "true",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 600,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "ShowObjectCount",
				GroupNameKey = "",
				DefaultValue = "true",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "",
				SortOrder = 700,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "DefaultSortSetting",
				GroupNameKey = "",
				DefaultValue = "ASC",
				ControlType = "ISettingControl",
				ControlSrc = "~/Controls/SortSetting.ascx",
				HelpKey = "",
				SortOrder = 800,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "SharedFileResources",
				ResourceKey = "CustomCssClassSetting",
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

			features.Add(feature);
		}

		return features;
	}
}
