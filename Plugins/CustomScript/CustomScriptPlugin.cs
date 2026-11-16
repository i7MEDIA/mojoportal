using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.CustomScript;

public class CustomScriptPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("662d49fd-cb44-42a5-a6a7-b905bbe65889");

	public override Guid Id => FeatureGuid;
	public override string Name => "CustomScript";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "CustomScriptModuleFeatureName",
			ControlSource = "Modules/CustomScript.ascx",
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
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ScriptUrl",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "CustomScriptModule-ScriptUrl-help",
			SortOrder = 1000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "ScriptUrlPosition",
			DefaultValue = "inBody",
			ControlType = "ISettingControl",
			ControlSrc = "mojoPortal.Web.UI.ScriptPositionSetting, mojoPortal.Web",
			SortOrder = 2000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "RawScriptPosition",
			DefaultValue = "bottomStartup",
			ControlType = "ISettingControl",
			ControlSrc = "mojoPortal.Web.UI.ScriptPositionSetting, mojoPortal.Web",
			SortOrder = 3000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "AddScriptElementToRawScript",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "CustomScriptModule-AddScriptElementToRawScript-help",
			SortOrder = 4000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "RawScript",
			DefaultValue = string.Empty,
			ControlType = "ISettingControl",
			ControlSrc = "mojoPortal.Web.UI.mojoTextArea, mojoPortal.Web",
			HelpKey = "CustomScript-RawScript-help",
			SortOrder = 5000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ModuleSettingsGeneralTab",
			ResourceKey = "RenderStandardWrapperDivs",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "CustomScriptModule-RenderStandardWrapperDivs-help",
			SortOrder = 6000,
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
			SortOrder = 7000,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
