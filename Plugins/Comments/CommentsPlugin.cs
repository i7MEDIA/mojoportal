using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.Comments;

public class CommentsPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("06451ec6-d4d7-47e3-a1ce-d19aaf7f98fe");

	public override Guid Id => FeatureGuid;
	public override string Name => "Comments";
	public override string LegacyApplicationName => "comments";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
			ResourceFile = "CommentResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "Comments/CommentsModule.ascx",
			SortOrder = 900,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SupportsPageReuse = true
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "CommentSystemSetting",
			DefaultValue = "internal",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/CommentSystems/CommentSystemSetting.ascx",
			HelpKey = "Blog-CommentSystemSetting-help",
			SortOrder = 1000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "AllowComments",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-AllowComments-help",
			SortOrder = 2000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "RequireApprovalForComments",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-RequireApprovalForComments-help",
			SortOrder = 3000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "ContentNotifyOnComment",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-ContentNotifyOnComment-help",
			SortOrder = 4000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "NotifyEmailSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-NotifyEmailSetting-help",
			SortOrder = 5000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "AllowCommentTitle",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-AllowCommentTitle-help",
			SortOrder = 6000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "AllowWebSiteUrlForComments",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-AllowWebSiteUrlForComments-help",
			SortOrder = 7000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "UseCommentSpamBlocker",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-UseCommentSpamBlocker-help",
			SortOrder = 8000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "RequireAuthenticationForComments",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-RequireAuthenticationForComments-help",
			SortOrder = 9000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "CheckKeywordBlacklist",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 9500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "SortCommentsDescending",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "Comments-SortCommentsDescending-help",
			SortOrder = 10000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "DateTimeFormat",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "BlogDateTimeFormat-help",
			SortOrder = 11000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "DisableAvatars",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 11500,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "CommentResources",
			GroupNameKey = string.Empty,
			ResourceKey = "CustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 12000,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		return [feature];
	}
}
