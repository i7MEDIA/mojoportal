using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.SiteSearch;

public class SiteSearchPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("09c0b1fc-a92d-4f3c-9f9b-b167c82d6089");

	public override Guid Id => FeatureGuid;
	public override string Name => "SiteSearch";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "SiteSearchModuleFeatureName",
			ControlSource = "Modules/SiteSearchModule.ascx",
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
			ResourceKey = "ShowSearchResultsInsteadOfRedirect",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "search-module-no-redirect-help",
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "ShowExcerpt",
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
			GroupNameKey = string.Empty,
			ResourceKey = "SearchableFeature",
			DefaultValue = "881e4e00-93e4-444c-b7b0-6672fb55de10,026cbead-2b80-4491-906d-b83e37179ccf,38aa5a84-9f5c-42eb-8f4c-105983d419fb,74bdbcc2-0e79-47ff-bcd4-a159270bf36e,dc873d76-5bf2-4ac5-bff7-434a87a3fc8e,d572f6b4-d0ed-465d-ad60-60433893b401,5a343d88-bce1-43d1-98ae-b42d77893e7b,0cefbf18-56de-11dc-8f36-bac755d89593",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/SearchableFeatureFilterSetting.ascx",
			HelpKey = string.Empty,
			SortOrder = 120,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = string.Empty,
			ResourceKey = "PageSize",
			DefaultValue = "10",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 130,
			RegexValidationExpression = @"^[1-9][0-9]{0,2}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = string.Empty,
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 140,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
