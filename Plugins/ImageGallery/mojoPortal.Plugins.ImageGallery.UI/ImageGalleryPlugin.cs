using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;

namespace mojoPortal.Plugins.ImageGallery.UI;

public class ImageGalleryPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("d572f6b4-d0ed-465d-ad60-60433893b401");

	public override Guid Id => FeatureGuid;
	public override string Name => "ImageGallery";
	public override string LegacyApplicationName => "imagegallery";
	public override Version Version => new(0, 0, 0, 8);

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var features = new List<ContentFeature>();

		var feature = new ContentFeature
		{
			FeatureGuid = new Guid("d572f6b4-d0ed-465d-ad60-60433893b401"),
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "GalleryResources",
			FeatureNameReasourceKey = "ImageGalleryFeatureName",
			ControlSource = "ImageGallery/Gallery.ascx",
			SortOrder = 540,
			IsCacheable = false,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif",
			IsSearchable = true,
			SearchListNameResourceKey = "ImageGallerySearchName",
			DeleteProvider = "GalleryContentDeleteHandler",
			SupportsPageReuse = true,
			PartialView = "",
			SkinFileName = ""
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryShowTechnicalDataSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "false",
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
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryThumbnailHeightSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "75",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1020,
			RegexValidationExpression = "^[1-9][0-9]{0,3}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryThumbnailWidthSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "100",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1030,
			RegexValidationExpression = "^[1-9][0-9]{0,3}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryThumbnailsPerPageSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "20",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1040,
			RegexValidationExpression = "^[1-9][0-9]{0,3}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryWebImageHeightSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "550",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1050,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryWebImageWidthSetting",
			GroupNameKey = "ImageSettings",
			DefaultValue = "550",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 1060,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ResizeBackgroundColor",
			GroupNameKey = "ImageSettings",
			DefaultValue = "#FFFFFF",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/ColorSetting.ascx",
			HelpKey = "imagegallery-ResizeBackgroundColor-help",
			SortOrder = 1070,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "GalleryCompactModeSetting",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "",
			SortOrder = 2009,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxTransition",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "elastic",
			ControlType = "ISettingControl",
			ControlSrc = "~/Controls/ColorBoxTransitionSetting.ascx",
			HelpKey = "colorbox-transition-help",
			SortOrder = 2010,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxTransitionSpeed",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "350",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "colorbox-transitionspeed-help",
			SortOrder = 2020,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxOpacity",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = ".7",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "colorbox-opacity-help",
			SortOrder = 2030,
			RegexValidationExpression = "^[01]$|^[.]\\d$|^[.]\\d\\d$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxUseSlideshow",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "colorbox-slideshow-help",
			SortOrder = 2040,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxSlideshowSpeed",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "3500",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "colorbox-slideshowspeed-help",
			SortOrder = 2050,
			RegexValidationExpression = "^[1-9][0-9]{0,4}$",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxSlideShowStartAuto",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "colorbox-SlideShowStartAuto-help",
			SortOrder = 2055,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "ColorBoxSlideShowAuto",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "true",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "colorbox-slideshowauto-help",
			SortOrder = 2060,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "UseNivoSlider",
			GroupNameKey = "ColorBoxSettings",
			DefaultValue = "false",
			ControlType = "CheckBox",
			ControlSrc = "",
			HelpKey = "imagegallery-nivoslider-help",
			SortOrder = 2080,
			RegexValidationExpression = "",
			Attributes = "",
			Options = "",
			Roles = "",
			ShowToUnauthorized = false
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ResourceFile = "GalleryResources",
			ResourceKey = "CustomCssClassSetting",
			GroupNameKey = "MiscSettings",
			DefaultValue = "",
			ControlType = "TextBox",
			ControlSrc = "",
			HelpKey = "custom-module-css-class-help",
			SortOrder = 4000,
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

