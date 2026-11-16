namespace mojoPortal.Plugins.Forums.UI;

public class ForumsPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("38aa5a84-9f5c-42eb-8f4c-105983d419fb");

	public override Guid Id => FeatureGuid;
	public override string Name => "Forums";
	public override string LegacyApplicationName => "forums";
	public override Version Version => new(2, 2, 0, 5);

	public override void RegisterWebApi(HttpConfiguration config)
	{
		if (!config.Routes.ContainsKey("ForumMod"))
		{
			config.Routes.MapHttpRoute(
				name: "ForumMod",
				routeTemplate: "api/forummod/{id}",
				defaults: new { controller = "ForumMod", id = RouteParameter.Optional }
			);
		}
	}

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature = new ContentFeature
		{
			FeatureGuid = new Guid("38aa5a84-9f5c-42eb-8f4c-105983d419fb"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "ForumResources",
			FeatureNameReasourceKey = "ForumsFeatureName",
			ControlSource = "Forums/ForumModule.ascx",
			SortOrder = 450,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = true,
			SearchListNameResourceKey = "ForumsSearchName",
			DeleteProvider = "ForumContentDeleteHandler",
			SupportsPageReuse = false,
			PartialView = "",
			SkinFileName = ""
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ShowForumSearchBox",
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
			ResourceFile = "ForumResources",
			ResourceKey = "ForumEnableAntiSpamSetting",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 2000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "AllowEditingPostsLessThanMinutesOld",
			GroupNameKey = "",
			DefaultValue = "60",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "forum-AllowEditingPostsLessThanMinutesOld-help",
			SortOrder = 210,
			RegexValidationExpression = "^(\\+|-)?\\d+$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "CloseThreadsOlderThanDays",
			GroupNameKey = "",
			DefaultValue = "-1",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "forum-CloseThreadsOlderThanDays-help",
			SortOrder = 3000,
			RegexValidationExpression = "^(\\+|-)?\\d+$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "IncludePostBodyInNotificationEmail",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 4000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "IncludeCurrentUserInNotifications",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "forums-IncludeCurrentUserInNotifications-help",
			SortOrder = 5000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "SuppressNotificationOfPostEdits",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "forums-SuppressNotificationOfPostEdits-help",
			SortOrder = 6000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "OverrideNotificationFromAddress",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "forum-OverrideNotificationFromAddress-help",
			SortOrder = 6500,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "OverrideNotificationFromAlias",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "forum-OverrideNotificationFromAlias-help",
			SortOrder = 6600,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ForumEnableRSSAtModuleLevel",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 7000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ForumEnableRSSAtForumLevel",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 8000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ForumEnableRSSAtThreadLevel",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 9000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "RSSFeedMaxDaysOldSetting",
			GroupNameKey = "",
			DefaultValue = "90",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 10000,
			RegexValidationExpression = "^[1-9][0-9]{0,3}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "RSSFeedMaxPostsSetting",
			GroupNameKey = "",
			DefaultValue = "90",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 11000,
			RegexValidationExpression = "^[1-9][0-9]{0,3}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ShowSubscriberCount",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 12000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "LinkGravatarToUserProfile",
			GroupNameKey = "",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "forums-LinkGravatarToUserProfile-help",
			SortOrder = 13000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 14000,
			RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ShowPageLeftContentSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowPageLeftContentSetting-help",
			SortOrder = 15000,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "ForumResources",
			ResourceKey = "ShowPageRightContentSetting",
			GroupNameKey = "",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "BlogShowPageRightContentSetting-help",
			SortOrder = 16000,
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

