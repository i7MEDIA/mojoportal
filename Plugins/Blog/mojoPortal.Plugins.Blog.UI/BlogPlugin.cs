using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.Blog.UI;

public class BlogPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("026cbead-2b80-4491-906d-b83e37179ccf");

	public override Guid Id => FeatureGuid;
	public override string Name => "Blog";
	public override string LegacyApplicationName => "blog";
	public override Version Version => new(2, 6, 2, 3);

	public override IEnumerable<string> GetViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Blog/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Blog/Views/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<string> GetPartialViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Blog/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Blog/Views/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature1 = new ContentFeature
		{
			FeatureGuid = new Guid("026cbead-2b80-4491-906d-b83e37179ccf"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "BlogResources",
			FeatureNameReasourceKey = "BlogFeatureName",
			ControlSource = "Blog/BlogModule.ascx",
			SortOrder = 400,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = true,
			SearchListNameResourceKey = "BlogSearchName",
			DeleteProvider = "BlogContentDeleteHandler",
			SupportsPageReuse = false,
			PartialView = "",
			SkinFileName = ""
		};

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowBlogSearchBox",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 90,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogAuthorSetting",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogAuthorSetting-help",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogAuthorEmailSetting",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogAuthorEmailSetting-help",
			SortOrder = 200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogCopyrightSetting",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogCopyrightSetting-help",
			SortOrder = 300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogDescriptionSetting",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogDescriptionSetting-help",
			SortOrder = 500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultUrlPrefix",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogDefaultUrlPrefix-help",
			SortOrder = 550,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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
			Attributes = "min|0;max|1000;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogDateTimeFormat",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogDateTimeFormat-help",
			SortOrder = 700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "RelatedItemsToShow",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "0",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/RelatedPostsToShowSetting.ascx",
			HelpKey = "blog-RelatedItemsToShow-help",
			SortOrder = 750,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultIncludeImageInExcerptChecked",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultIncludeImageInExcerptChecked-help",
			SortOrder = 760,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultIncludeImageInPostChecked",
			GroupNameKey = "GeneralSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultIncludeImageInPostChecked-help",
			SortOrder = 761,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "HideDetailsFromUnauthencticated",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-HideDetailsFromUnauthencticated-help",
			SortOrder = 1500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogUseExcerptSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogUseExcerptSetting-help",
			SortOrder = 1600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "UseExcerptInFeedSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogUseExcerptInFeedSetting-help",
			SortOrder = 1700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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
			Attributes = "min|10;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogExcerptSuffixSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "...",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogExcerptSuffixSetting-help",
			SortOrder = 1900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogMoreLinkText",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "read more",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogMoreLinkText-help",
			SortOrder = 2000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowTitleOnlySetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowTitleOnlySetting-help",
			SortOrder = 2100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowPagerInListSetting",
			GroupNameKey = "ListViewSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowPagerInListSetting-help",
			SortOrder = 3100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ManagingEditorName",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-ManagingEditorName-help",
			SortOrder = 3200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ManagingEditorEmail",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-ManagingEditorEmail-help",
			SortOrder = 3205,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "LanguageCode",
			GroupNameKey = "FeedSettings",
			DefaultValue = "en-US",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-LanguageCode-help",
			SortOrder = 3210,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "FeedLogoUrl",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-FeedLogoUrl-help",
			SortOrder = 3215,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "HasExplicitContent",
			GroupNameKey = "FeedSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-HasExplicitContent-help",
			SortOrder = 3300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "iTunesMainCategory",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-iTunesMainCategory-help",
			SortOrder = 3225,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "iTunesSubCategory",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-iTunesSubCategory-help",
			SortOrder = 3235,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowFeedLinksSetting",
			GroupNameKey = "FeedSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowFeedLinksSetting-help",
			SortOrder = 4000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "AddFeedDiscoveryLink",
			GroupNameKey = "FeedSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-AddFeedDiscoveryLink-help",
			SortOrder = 4150,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogDisableFeedSetting",
			GroupNameKey = "FeedSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogDisableFeedSetting-help",
			SortOrder = 4200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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

		feature1.Settings.Add(new ContentFeatureSetting
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

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "RSSAddSignature",
			GroupNameKey = "FeedSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogRSSAddSignature-help",
			SortOrder = 4400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "RSSAddCommentsLink",
			GroupNameKey = "FeedSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogRSSAddCommentsLink-help",
			SortOrder = 4500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogFeedburnerFeedUrl",
			GroupNameKey = "FeedSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "BlogFeedburnerFeedUrl-help",
			SortOrder = 4900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowCalendarSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowCalendarSetting-help",
			SortOrder = 5000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowCalendarOnPostDetail",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 5001,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowArchiveSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowArchiveSetting-help",
			SortOrder = 5100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowCategoriesSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowCategoriesSetting-help",
			SortOrder = 5200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogUseTagCloudForCategoriesSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogUseTagCloudForCategoriesSetting-help",
			SortOrder = 5300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogNavigationOnRightSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogNavigationOnRightSetting-help",
			SortOrder = 5400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogShowStatisticsSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowStatisticsSetting-help",
			SortOrder = 5500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowNextPreviousLinks",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-ShowNextPreviousLinks-help",
			SortOrder = 5510,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ContentNotifyOnComment",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogContentNotifyOnComment-help",
			SortOrder = 6000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogAllowComments",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogAllowComments-help",
			SortOrder = 6100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "RequireApprovalForComments",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-RequireApprovalForComments-help",
			SortOrder = 6150,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogCommentForDaysDefault",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "90",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/CommentDaysSetting.ascx",
			HelpKey = "",
			SortOrder = 6200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "AllowCommentTitle",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-AllowCommentTitle-help",
			SortOrder = 6250,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "AllowWebSiteUrlForComments",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogAllowWebSiteUrlForComments-help",
			SortOrder = 6300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogUseCommentSpamBlocker",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogUseCommentSpamBlocker-help",
			SortOrder = 6400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "RequireAuthenticationForComments",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogRequireAuthenticationForComments-help",
			SortOrder = 6500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "SortCommentsDescending",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-SortCommentsDescending-help",
			SortOrder = 6550,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "CommentSystemSetting",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "internal",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/CommentSystems/CommentSystemSetting.ascx",
			HelpKey = "Blog-CommentSystemSetting-help",
			SortOrder = 6600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "IntenseDebateAccountId",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-IntenseDebateAccountId-help",
			SortOrder = 6700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DisqusSiteShortName",
			GroupNameKey = "CommentSystemSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-DisqusSiteShortName-help",
			SortOrder = 6800,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowTweetThisLink",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-ShowTweetThisLink-help",
			SortOrder = 8010,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "AddTweetThisToFeed",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-AddTweetThisToFeed-help",
			SortOrder = 8020,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "UseFacebookLikeButton",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-UseFacebookLikeButton-help",
			SortOrder = 8050,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "AddFacebookLikeToFeed",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-AddFacebookLikeToFeed-help",
			SortOrder = 8051,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "FacebookLikeButtonTheme",
			GroupNameKey = "SocialSettings",
			DefaultValue = "light",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/Sharing/FacebookLikeButtonThemeSetting.ascx",
			HelpKey = "FacebookLikeButtonTheme-help",
			SortOrder = 8100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "FacebookLikeButtonShowFaces",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FacebookLikeButtonShowFaces-help",
			SortOrder = 8100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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
			Attributes = "min|10;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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
			Attributes = "min|10;",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "EnableContentRatingSetting",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "",
			ControlSrc = "",
			HelpKey = "BlogEnableContentRatingSetting-help",
			SortOrder = 9300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "EnableRatingCommentsSetting",
			GroupNameKey = "SocialSettings",
			DefaultValue = "false",
			ControlType = "",
			ControlSrc = "",
			HelpKey = "BlogEnableRatingCommentsSetting-help",
			SortOrder = 9400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "UpperSidebar",
			GroupNameKey = "SidebarSettings",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/HtmlSetting.ascx",
			HelpKey = "",
			SortOrder = 9500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "LowerSidebar",
			GroupNameKey = "SidebarSettings",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/HtmlSetting.ascx",
			HelpKey = "",
			SortOrder = 9600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowPageLeftContentSetting",
			GroupNameKey = "AdditionalContentSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowPageLeftContentSetting-help",
			SortOrder = 9710,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowPageRightContentSetting",
			GroupNameKey = "AdditionalContentSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowPageRightContentSetting-help",
			SortOrder = 9715,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ShowGoogleNewsTabInEditPage",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-ShowGoogleNewsTabInEditPage-help",
			SortOrder = 10010,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultPublicationName",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultPublicationName-help",
			SortOrder = 10015,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultPublicationLanguage",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "en",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultPublicationLanguage-help",
			SortOrder = 10020,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "PublicationAccess",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/PublicationAccessSetting.ascx",
			HelpKey = "Blog-PublicationAccess-help",
			SortOrder = 10025,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultGenres",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultGenres-help",
			SortOrder = 10030,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "DefaultIncludeInNewsChecked",
			GroupNameKey = "GoogleNewsSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "Blog-DefaultIncludeInNewsChecked-help",
			SortOrder = 10035,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "BlogEnableVersioningSetting",
			GroupNameKey = "MiscellaneousSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "html-version-help",
			SortOrder = 10200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "MiscellaneousSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 10400,
			RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
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

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "GoogleMapIncludeWithExcerptSetting",
			GroupNameKey = "MiscellaneousSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogGoogleMapIncludeWithExcerptSetting-help",
			SortOrder = 10600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "UsePostTitleAsPageHeadingSetting",
			GroupNameKey = "MiscellaneousSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogUsePostTitleAsPageHeading-help",
			SortOrder = 10700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature1.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "",
			ResourceKey = "FeaturedPostId",
			GroupNameKey = "MiscellaneousSettings",
			DefaultValue = "",
			ControlType = "",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 20000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		features.Add(feature1);

		var feature2 = new ContentFeature
		{
			FeatureGuid = new Guid("031eb6a0-acf5-4559-8356-af6049d57ac1"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "BlogResources",
			FeatureNameReasourceKey = "BlogPostListFeatureName",
			ControlSource = "Blog/PostListModule.ascx",
			SortOrder = 401,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SearchListNameResourceKey = "",
			DeleteProvider = "",
			SupportsPageReuse = true,
			PartialView = "",
			SkinFileName = ""
		};

		feature2.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "PostListBlogInstance",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/BlogInstanceSetting.ascx",
			HelpKey = "",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature2.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "PostListBlogInstanceCategories",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/BlogInstanceCategoriesSetting.ascx",
			HelpKey = "",
			SortOrder = 110,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature2.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "PostListLayout",
			GroupNameKey = "",
			DefaultValue = "_BlogPostList",
			ControlType = "ISettingControl",
			ControlSrc = "~/Blog/Controls/PostListLayoutSelector.ascx",
			HelpKey = "",
			SortOrder = 120,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature2.Settings.Add(new ContentFeatureSetting
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

		feature2.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "BlogResources",
			ResourceKey = "ExtraCssClassSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 140,
			RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		features.Add(feature2);

		return features;
	}
}

