using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.Survey.UI;

public class SurveyPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("263ecff1-f321-4bb8-8d0a-64c0ba89caa7");

	public override Guid Id => FeatureGuid;
	public override string Name => "Survey";
	public override string LegacyApplicationName => "surveyfeature";
	public override Version Version => new(1, 9, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("263ecff1-f321-4bb8-8d0a-64c0ba89caa7"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
				ResourceFile = "SurveyResources",
				FeatureNameReasourceKey = "SurveyFeatureName",
				ControlSource = "Survey/SurveyModule.ascx",
				SortOrder = 500,
				IsCacheable = false,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = false,
				SearchListNameResourceKey = "",
				DeleteProvider = "SurveyContentDeleteHandler",
				SupportsPageReuse = false,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "Resource",
				ResourceKey = "CustomCssClassSetting",
				GroupNameKey = "",
				DefaultValue = "",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "custom-module-css-class-help",
				SortOrder = 10,
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
