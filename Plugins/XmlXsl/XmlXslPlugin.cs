using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.XmlXsl;

public class XmlXslPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("fa969c0a-6d02-4dcb-86b8-ac69d80c1fb1");

	public override Guid Id => FeatureGuid;
	public override string Name => "XmlXsl";
	public override string LegacyApplicationName => "xmlmodule";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "XmlResources",
			FeatureNameReasourceKey = "XmlXslFeatureName",
			ControlSource = "XmlXsl/XmlModule.ascx",
			SortOrder = 1000,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "XmlModuleXmlSourceSetting",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "XmlModuleXslSourceSetting",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "XmlUrl",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "XslUrl",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 200,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "AllowedHostnames",
			DefaultValue = string.Empty,
			ControlType = "textarea",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 300,
			RegexValidationExpression = "HostnameList",
			Roles = "Admins",
			ShowToUnauthorized = true
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 400,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "AllowExternalImages",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "xml-AllowExternalImages-help",
			SortOrder = 500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "XmlResources",
			ResourceKey = "TrustContent",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "xml-TrustContent-help",
			SortOrder = 600,
			RegexValidationExpression = string.Empty
		});

		return [feature];
	}
}
