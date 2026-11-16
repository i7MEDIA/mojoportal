using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.KDMediaPlayer.UI;

public class KDMediaPlayerPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("92fd0c20-19dc-44c7-a990-0d9e281fc87c");

	public override Guid Id => FeatureGuid;
	public override string Name => "KDMediaPlayer";
	public override string LegacyApplicationName => "kdmediaplayer";
	public override Version Version => new(1, 0, 0, 1);

	protected override string GetProviderName(Type providerType) =>
		providerType.Name == "MediaPlayerContentDeleteHandler"
			? "KDMediaPlayerContentDeleteHandler"
			: base.GetProviderName(providerType);
	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("92fd0c20-19dc-44c7-a990-0d9e281fc87c"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
				ResourceFile = "MediaPlayerResources",
				FeatureNameReasourceKey = "AudioPlayerFeatureName",
				ControlSource = "KDMediaPlayer/AudioPlayer.ascx",
				SortOrder = 551,
				IsCacheable = true,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = false,
				SearchListNameResourceKey = "",
				DeleteProvider = "KDMediaPlayerContentDeleteHandler",
				SupportsPageReuse = true,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "AutoStartSetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerAutoStartSetting-help",
				SortOrder = 1000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "ContinuousPlaySetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerContinuousPlaySetting-help",
				SortOrder = 2000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "DisableShuffleSetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerDisableShuffleSetting-help",
				SortOrder = 3000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "CustomCssClassSetting",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "bluemonday",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "media-player-custom-module-css-class-help",
				SortOrder = 4000,
				RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "HeaderContent",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "",
				ControlType = "ISettingControl",
				ControlSrc = "~/Controls/HtmlSetting.ascx",
				HelpKey = "mediaplayer-headercontent-help",
				SortOrder = 5000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "FooterContent",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "",
				ControlType = "ISettingControl",
				ControlSrc = "~/Controls/HtmlSetting.ascx",
				HelpKey = "mediaplayer-footercontent-help",
				SortOrder = 6000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			features.Add(feature);
		}
		{
			var feature = new ContentFeature
			{
				FeatureGuid = new Guid("30560810-7624-4933-be42-0f95b92f2ff0"),
				SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite",
				ResourceFile = "MediaPlayerResources",
				FeatureNameReasourceKey = "VideoPlayerFeatureName",
				ControlSource = "KDMediaPlayer/VideoPlayer.ascx",
				SortOrder = 551,
				IsCacheable = true,
				DefaultCacheTime = 0,
				ExcludeFromFeatureList = false,
				Icon = "blank.gif",
				IsSearchable = false,
				SearchListNameResourceKey = "",
				DeleteProvider = "KDMediaPlayerContentDeleteHandler",
				SupportsPageReuse = true,
				PartialView = "",
				SkinFileName = ""
			};

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "AutoStartSetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerAutoStartSetting-help",
				SortOrder = 1000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "ContinuousPlaySetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerContinuousPlaySetting-help",
				SortOrder = 2000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "DisableShuffleSetting",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "false",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerDisableShuffleSetting-help",
				SortOrder = 3000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "AllowFullScreen",
				GroupNameKey = "PlaybackControlSection",
				DefaultValue = "true",
				ControlType = "CheckBox",
				ControlSrc = "",
				HelpKey = "MediaPlayerAllowFullScreen-help",
				SortOrder = 3050,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "CustomCssClassSetting",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "bluemonday",
				ControlType = "TextBox",
				ControlSrc = "",
				HelpKey = "media-player-custom-module-css-class-help",
				SortOrder = 4000,
				RegexValidationExpression = "^([^\\d\\s\\.#\\-][\\w\\-]*\\s?)*$",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "HeaderContent",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "",
				ControlType = "ISettingControl",
				ControlSrc = "~/Controls/HtmlSetting.ascx",
				HelpKey = "mediaplayer-headercontent-help",
				SortOrder = 5000,
				RegexValidationExpression = "",
				Attributes = "",
				Options = "",
				Roles = "",
				ShowToUnauthorized = false
			});

			feature.Settings.Add(new ContentFeatureSetting
			{
				ResourceFile = "MediaPlayerResources",
				ResourceKey = "FooterContent",
				GroupNameKey = "MiscellaneousSection",
				DefaultValue = "",
				ControlType = "ISettingControl",
				ControlSrc = "~/Controls/HtmlSetting.ascx",
				HelpKey = "mediaplayer-footercontent-help",
				SortOrder = 6000,
				RegexValidationExpression = "",
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
