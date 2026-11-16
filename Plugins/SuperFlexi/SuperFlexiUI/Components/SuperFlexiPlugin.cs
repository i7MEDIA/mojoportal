using mojoPortal.Web;
using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SuperFlexiUI;

public class SuperFlexiPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("4FF93793-1187-4022-899C-C3E9096A855F");

	public override Guid Id => FeatureGuid;
	public override string Name => "SuperFlexi";
	public override string LegacyApplicationName => "SuperFlexi";
	public override Version Version => new(0, 0, 2, 7);

	public override void RegisterWebApi(HttpConfiguration config)
	{
		config.Routes.MapHttpRoute(
			name: "SuperFlexi",
			routeTemplate: "SuperFlexi/{action}/{id}",
			defaults: new { controller = "SuperFlexi", id = RouteParameter.Optional }
		);
	}

	public override IEnumerable<string> GetViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/SuperFlexi/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<string> GetPartialViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/SuperFlexi/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,SQLite,pgsql",
			ResourceFile = "SuperFlexiResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "SuperFlexi/Module.ascx",
			SortOrder = 500,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			IsSearchable = true,
			SearchListNameResourceKey = "SearchName",
			DeleteProvider = "SuperFlexiDeleteHandler",
			SupportsPageReuse = true
		};

		// Definition Settings
		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "MarkupDefinitionFile",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = "ISettingControl",
			ControlSrc = "~/SuperFlexi/Controls/MarkupDefinitionSetting.ascx",
			HelpKey = "sflexi-MarkupDefinitionSetting-help",
			SortOrder = 100
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "MarkupDefinitionContent",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			HelpKey = "sflexi-MarkupDefinitionContent-help",
			SortOrder = 101
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "IsGlobalView",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			HelpKey = "sflexi-IsGlobalView-help",
			SortOrder = 125
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "GlobalViewSortOrder",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "sflexi-GlobalViewSortOrder-help",
			SortOrder = 126,
			RegexValidationExpression = @"^[-+]?\d*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "ModuleFriendlyName",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "sflexi-ModuleFriendlyName-help",
			SortOrder = 130
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "InstanceFeaturedImage",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = "ISettingControl",
			ControlSrc = "~/SuperFlexi/Controls/FeaturedImageSetting.ascx",
			HelpKey = "sflexi-InstanceFeaturedImageSetting-help",
			SortOrder = 150
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "DescendingSortOrder",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			HelpKey = "sflexi-DescendingSortOrder-help",
			SortOrder = 160
		});

		// Search Settings
		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "IncludeInSearch",
			GroupNameKey = "SearchSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			HelpKey = "sflexi-IncludeInSearch-help",
			SortOrder = 200
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "SolutionSearchFriendlyName",
			GroupNameKey = "SearchSettings",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "sflexi-SearchFriendlyNameSetting-help",
			SortOrder = 205
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "RelatedSearchPage",
			GroupNameKey = "SearchSettings",
			DefaultValue = string.Empty,
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/PageSelectorSetting.ascx",
			HelpKey = "sflexi-RelatedSearchPage-help",
			SortOrder = 210
		});

		// Advanced Settings
		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "ExtraCssClassSetting",
			GroupNameKey = "AdvancedSettings",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "sflexi-ExtraCssClassSetting-help",
			SortOrder = 300,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "MaxItems",
			GroupNameKey = "AdvancedSettings",
			DefaultValue = "ALL",
			ControlType = "TextBox",
			HelpKey = "sflexi-MaxItems-help",
			SortOrder = 305,
			RegexValidationExpression = @"^(?i)(?:ALL)|\d*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "CustomizableSettings",
			GroupNameKey = "AdvancedSettings",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			HelpKey = "sflexi-CustomizableSettings-help",
			SortOrder = 310
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "PageSize",
			GroupNameKey = "AdvancedSettings",
			DefaultValue = "0",
			ControlType = "TextBox",
			HelpKey = "sflexi-PageSize-help",
			RegexValidationExpression = @"^[0-9]*$",
			SortOrder = 320
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "HeaderContent",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			SortOrder = 1
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "SuperFlexiResources",
			ResourceKey = "FooterContent",
			GroupNameKey = "DefinitionSettings",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			SortOrder = 1
		});

		return [feature];
	}
}
