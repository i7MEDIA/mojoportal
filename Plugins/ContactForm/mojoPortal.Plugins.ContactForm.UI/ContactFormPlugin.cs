using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.ContactForm.UI;

public class ContactFormPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("99019eb8-db13-4bb3-81ea-073571a60dba");

	public override Guid Id => FeatureGuid;
	public override string Name => "ContactForm";
	public override string LegacyApplicationName => "contactform";
	public override Version Version => new(0, 0, 0, 6);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature = new ContentFeature
		{
			FeatureGuid = new Guid("99019eb8-db13-4bb3-81ea-073571a60dba"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "ContactFormResources",
			FeatureNameReasourceKey = "ContactFormFeatureName",
			ControlSource = "ContactForm/ContactForm.ascx",
			SortOrder = 500,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SearchListNameResourceKey = "",
			DeleteProvider = "ContactFormContentDeleteHandler",
			SupportsPageReuse = true,
			PartialView = "",
			SkinFileName = ""
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContactFormEmailSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "ContactFormEmailSetting-help",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContactFormEmailAliasSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "ContactFormEmailAliasSetting-help",
			SortOrder = 200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContactFormEmailBccSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "ContactFormEmailBccSetting-help",
			SortOrder = 300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "KeepMessagesInDatabase",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "ContactForm-KeepMessagesInDatabase-help",
			SortOrder = 500,
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
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContactFormUseCommentSpamBlocker",
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
			ResourceFile = "ContactFormResources",
			ResourceKey = "BlockBadWords",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlockBadWordsModuleSetting-help",
			SortOrder = 710,
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
			Attributes = "min|1;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContactFormSubjectPrefixSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "AppendIPToMessageSetting",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "ContentEditorSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/ContentEditorSetting.ascx",
			HelpKey = "",
			SortOrder = 1010,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ContactFormResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 1100,
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

