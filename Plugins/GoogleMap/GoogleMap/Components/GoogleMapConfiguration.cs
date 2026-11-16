using System;
using System.Collections;
using mojoPortal.Web.Controls.google;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.MapUI;

/// <summary>
/// encapsulates the feature instance configuration loaded from module settings into a more friendly object
/// </summary>
public class GoogleMapConfiguration
{
	public GoogleMapConfiguration() { }

	public GoogleMapConfiguration(Hashtable settings)
	{
		LoadSettings(settings);
	}

	private void LoadSettings(Hashtable settings)
	{
		if (settings == null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		EnableMapType = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableMapTypeSetting", EnableMapType);
		EnableZoom = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableZoomSetting", EnableZoom);
		ShowInfoWindow = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowInfoWindowSetting", ShowInfoWindow);
		EnableLocalSearch = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableLocalSearchSetting", EnableLocalSearch);
		EnableDrivingDirections = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableDirectionsSetting", EnableDrivingDirections);
		UseLocationAsTitle = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapUseLocationForTitleSetting", UseLocationAsTitle);
		UseLocationAsCaption = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowLocationCaptionSetting", UseLocationAsCaption);
		UseIframe = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapUseIframeSetting", UseIframe);
		MapHeight = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapHeightSetting", MapHeight);
		MapWidth = WebUtils.ParseStringFromHashtable(settings, "GoogleMapWidthSetting", MapWidth);
		ZoomSetting = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapInitialZoomSetting", ZoomSetting);
		InstanceCssClass = WebUtils.ParseStringFromHashtable(settings, "CustomCssClassSetting", InstanceCssClass);
		Location = WebUtils.ParseStringFromHashtable(settings, "GoogleMapLocationSetting", Location);
		GoogleApiKey = WebUtils.ParseStringFromHashtable(settings, "GoogleMapApiKeySetting", GoogleApiKey);
		Caption = WebUtils.ParseStringFromHashtable(settings, "GoogleMapCaptionSetting", Caption);

		string gmType = WebUtils.ParseStringFromHashtable(settings, "GoogleMapInitialMapTypeSetting", "G_NORMAL_MAP");
		try
		{
			GoogleMapType = (MapType)Enum.Parse(typeof(MapType), gmType);
		}
		catch (ArgumentException) { }
	}

	public string Location { get; private set; } = string.Empty;

	public string Caption { get; private set; } = string.Empty;

	public string GoogleApiKey { get; private set; } = string.Empty;

	public int MapHeight { get; private set; } = 300;

	public string MapWidth { get; private set; } = "500px";

	public bool EnableMapType { get; private set; } = false;

	public bool EnableZoom { get; private set; } = false;

	public bool ShowInfoWindow { get; private set; } = false;

	public bool EnableLocalSearch { get; private set; } = false;

	public bool UseLocationAsTitle { get; private set; } = false;

	public bool UseLocationAsCaption { get; private set; } = false;

	public bool EnableDrivingDirections { get; private set; } = false;

	public MapType GoogleMapType { get; private set; } = MapType.G_NORMAL_MAP;

	public int ZoomSetting { get; private set; } = 13;

	public string InstanceCssClass { get; private set; } = string.Empty;
	public bool UseIframe { get; private set; } = true;

	public MapRatio MapRatio { get; private set; } = MapRatio.r16by9;
}