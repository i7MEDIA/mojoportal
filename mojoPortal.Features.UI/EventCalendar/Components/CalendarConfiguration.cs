using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Controls.google;

namespace mojoPortal.Features.UI.EventCalendar
{
    public class CalendarConfiguration
    {
		public CalendarConfiguration() { }
		public CalendarConfiguration(Hashtable settings)
		{
			UseFillerOnEmptyDays = WebUtils.ParseBoolFromHashtable(settings, "CalendarUseFillerOnEmptyDays", true);

			if (settings.Contains("CustomCssClassSetting"))
			{
				InstanceCssClass = settings["CustomCssClassSetting"].ToString();
			}

			ShowTimeInMonthView = WebUtils.ParseBoolFromHashtable(settings, "ShowTimeInMonthViewSetting", ShowTimeInMonthView);

			if (settings.Contains("GoogleMapInitialMapTypeSetting"))
			{
				string gmType = settings["GoogleMapInitialMapTypeSetting"].ToString();
				try
				{
					MapType = (MapType)Enum.Parse(typeof(MapType), gmType);
				}
				catch (ArgumentException) { }

			}

			MapHeight = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapHeightSetting", MapHeight);

			if (settings.Contains("GoogleMapWidthSetting"))
			{
				MapWidth = settings["GoogleMapWidthSetting"].ToString();
			}

			MapInitialZoom = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapInitialZoomSetting", MapInitialZoom);

			MapEnableMapType = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableMapTypeSetting", false);

			MapEnableZoom = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableZoomSetting", false);

			MapShowInfoWindow = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowInfoWindowSetting", false);

			MapEnableLocalSearch = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableLocalSearchSetting", false);

			MapEnableDirections = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableDirectionsSetting", false);

			EnableMap = WebUtils.ParseBoolFromHashtable(settings, "EnableMapSetting", EnableMap);
		}

		public bool UseFillerOnEmptyDays { get; set; } = true;
		public bool EnableMap { get; set; } = true;
		public bool ShowTimeInMonthView { get; set; } = true;
		public string InstanceCssClass { get; set; } = string.Empty;
		public int MapHeight { get; set; } = 300;
		public string MapWidth { get; set; } = "500px";
		public bool MapEnableMapType { get; set; } = false;
		public bool MapEnableZoom { get; set; } = false;
		public bool MapShowInfoWindow { get; set; } = false;
		public bool MapEnableLocalSearch { get; set; } = false;
		public bool MapEnableDirections { get; set; } = false;
		public MapType MapType { get; set; } = MapType.G_NORMAL_MAP;
		public int MapInitialZoom { get; set; } = 13;
	}
}