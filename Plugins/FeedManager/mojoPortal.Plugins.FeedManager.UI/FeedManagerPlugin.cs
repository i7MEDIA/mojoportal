namespace mojoPortal.Plugins.FeedManager.UI;

public class FeedManagerPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("5ef82464-e2d3-4982-8dfd-01e1afa615f9");

	public override Guid Id => FeatureGuid;
	public override string Name => "FeedManager";
	public override string LegacyApplicationName => "feed-manager";
	public override Version Version => new(1, 0, 1, 1);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature = new ContentFeature
		{
			FeatureGuid = new Guid("5ef82464-e2d3-4982-8dfd-01e1afa615f9"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "FeedResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "FeedManager/FeedManagerModule.ascx",
			SortOrder = 500,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = false,
			SearchListNameResourceKey = "",
			DeleteProvider = "FeedContentDeleteHandler",
			SupportsPageReuse = true,
			PartialView = "",
			SkinFileName = ""
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorShowFeedNameBeforeContent",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowFeedNameBeforeContent-help",
			SortOrder = 80,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorShowFeedName",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowFeedName-help",
			SortOrder = 100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedShowAggregateFeedLink",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowAggregateFeedLink-help",
			SortOrder = 200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "UseAutoDiscoveryAggregateFeedLink",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-UseAutoDiscoveryAggregateFeedLink-help",
			SortOrder = 250,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedShowIndividualFeedLinks",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowIndividualFeedLinks-help",
			SortOrder = 300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedShowFeedListRightSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowFeedListRightSetting-help",
			SortOrder = 400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedUseFeedListAsFilterSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-UseFeedListAsFilterSetting-help",
			SortOrder = 500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedSortAscending",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-SortAscending-help",
			SortOrder = 600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorPageSizeSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "10",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "FeedManager-PageSizeSetting-help",
			SortOrder = 700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "UseScrollerSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-UseScrollerSetting-help",
			SortOrder = 750,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorShowAuthor",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowAuthor-help",
			SortOrder = 800,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "ShowDateSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowDateSetting-help",
			SortOrder = 900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "FeedDateFormatSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "FeedDateFormatSetting-help",
			SortOrder = 1000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedListLabelSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "Feeds",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "FeedManager-FeedListLabelSetting-help",
			SortOrder = 1200,
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
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedUseCalendarView",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-UseCalendarView-help",
			SortOrder = 1400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedPadEmptyDaysInCalendarView",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-PadEmptyDaysInCalendarView-help",
			SortOrder = 1500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "NavigationSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 1600,
			RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedShowHeadingsOnlySetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ShowHeadingsOnlySetting-help",
			SortOrder = 1700,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "OpenLinkInNewWindow",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-OpenLinkInNewWindow-help",
			SortOrder = 1750,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorExcerptSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-UseExcerptSetting-help",
			SortOrder = 1800,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorExcerptLengthSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "250",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ExcerptLengthSetting-help",
			SortOrder = 1900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSAggregatorExcerptSuffixSetting",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "...",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "FeedManager-ExcerptSuffixSetting-help",
			SortOrder = 2000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "FeedUseExcerptSuffixAsLinkToPost",
			GroupNameKey = "ExcerptModeSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-FeedUseExcerptSuffixAsLinkToPost-help",
			SortOrder = 2100,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "FilterInvalidMarkupAndPotentiallyInsecureContent",
			GroupNameKey = "SecuritySettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-FilterInvalidMarkupAndPotentiallyInsecureContent-help",
			SortOrder = 2200,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedAllowExternalImages",
			GroupNameKey = "SecuritySettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-AllowExternalImages-help",
			SortOrder = 2300,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedShowErrorMessageOnInvalidPosts",
			GroupNameKey = "SecuritySettings",
			DefaultValue = "false",
			ControlType = "",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 2400,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "RSSFeedFeedListCacheTimeoutSetting",
			GroupNameKey = "CacheSettings",
			DefaultValue = "240",
			ControlType = "",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 2500,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$",
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
			Attributes = "min|10;",
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
			ResourceFile = "FeedResources",
			ResourceKey = "EnableSelectivePublishing",
			GroupNameKey = "SelectivePublishingSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-EnableSelectivePublishing-help",
			SortOrder = 2900,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "FeedResources",
			ResourceKey = "EnableInPlacePublishing",
			GroupNameKey = "SelectivePublishingSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "FeedManager-EnableInPlacePublishing-help",
			SortOrder = 3000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		features.Add(feature);

		return features;
	}
}

