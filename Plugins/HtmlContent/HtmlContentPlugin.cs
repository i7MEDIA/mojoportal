using System;
using System.Collections.Generic;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.HtmlContent;

public class HtmlContentPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("881e4e00-93e4-444c-b7b0-6672fb55de10");

	public override Guid Id => FeatureGuid;
	public override string Name => "HtmlContent";
	public override Version Version => new(1, 0, 0, 0);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "Resource",
			FeatureNameReasourceKey = "HtmlContentFeatureName",
			ControlSource = "Modules/HtmlModule.ascx",
			SortOrder = 100,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			IsSearchable = true,
			SearchListNameResourceKey = "HtmlContentSearchName",
			DeleteProvider = "HtmlContentDeleteHandler",
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlCustomCssClassSetting",
			DefaultValue = string.Empty,
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "custom-module-css-class-help",
			SortOrder = 100,
			RegexValidationExpression = @"^([^\d\s\.#\-][\w\-]*\s?)*$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlEditorHeightSetting",
			DefaultValue = "500",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 200,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "UseWysiwygEditor",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "UseWysiwygEditor-help",
			SortOrder = 250,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowAuthorAvatar",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 303,
			RegexValidationExpression = string.Empty,
			HelpKey = "html-ShowAuthorAvatar-help"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowAuthorBio",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 307,
			RegexValidationExpression = string.Empty,
			HelpKey = "html-ShowAuthorBio-help"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowCreatedBy",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 310,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowCreatedDate",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 320,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowModifiedBy",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 330,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "ShowModifiedDate",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			SortOrder = 340,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ContentVersionSettings",
			ResourceKey = "HtmlEnableVersioningSetting",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "html-version-help",
			SortOrder = 400,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "ContentVersionSettings",
			ResourceKey = "HtmlVersionPageSizeSetting",
			DefaultValue = "10",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 500,
			RegexValidationExpression = @"^[1-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlEnableSlideShow",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-EnableSlideShow-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "EnableSlideClick",
			DefaultValue = "true",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-EnableSlideClick-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideContainerHeight",
			DefaultValue = "0",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-SlideContainerHeight-help",
			SortOrder = -50,
			RegexValidationExpression = @"^[0-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideTransitions",
			DefaultValue = "fade,curtainX,turnUp,slideY,zoom,scrollRight",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-SlideTransitions-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideContainerClass",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlSlideContainerClass-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideShowRandomizeSlides",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlSlideShowRandomizeSlides-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideShowUseExtraClearTypeCorrections",
			DefaultValue = "true",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlSlideShowUseExtraClearTypeCorrections-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideShowPauseOnHover",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlSlideShowPauseOnHover-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideDuration",
			DefaultValue = string.Empty,
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-SlideshowTransitionSpeed-help",
			SortOrder = -50,
			RegexValidationExpression = @"^[0-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlTransitionSpeed",
			DefaultValue = "1000",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlTransitionSpeed-help",
			SortOrder = -50,
			RegexValidationExpression = @"^[0-9][0-9]{0,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlEnableSlideShowPager",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-EnableSlideShowPager-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "MainSettings",
			ResourceKey = "HtmlSlideShowPagerBefore",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = "html-HtmlSlideShowPagerBefore-help",
			SortOrder = -50,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "EnableContentRatingSetting",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 1700,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "EnableRatingCommentsSetting",
			DefaultValue = "false",
			ControlType = string.Empty,
			ControlSrc = string.Empty,
			HelpKey = string.Empty,
			SortOrder = 1800,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "UseFacebookLikeButton",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "UseFacebookLikeButton-help",
			SortOrder = 1900,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "FacebookLikeButtonTheme",
			DefaultValue = "light",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/Sharing/FacebookLikeButtonThemeSetting.ascx",
			HelpKey = "FacebookLikeButtonTheme-help",
			SortOrder = 2000,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "FacebookLikeButtonShowFaces",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = string.Empty,
			HelpKey = "FacebookLikeButtonShowFaces-help",
			SortOrder = 2100,
			RegexValidationExpression = string.Empty
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "FacebookLikeButtonWidth",
			DefaultValue = "450",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FacebookLikeButtonWidth-help",
			SortOrder = 2200,
			RegexValidationExpression = @"^[1-9][0-9]{1,4}$"
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "Resource",
			GroupNameKey = "SocialSettings",
			ResourceKey = "FacebookLikeButtonHeight",
			DefaultValue = "35",
			ControlType = "TextBox",
			ControlSrc = string.Empty,
			HelpKey = "FacebookLikeButtonHeight-help",
			SortOrder = 2300,
			RegexValidationExpression = @"^[1-9][0-9]{1,4}$"
		});

		return [feature];
	}
}
