// Author:				    
// Created:			        2010-06-11
// Last Modified:		    2010-06-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.google;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.MapUI
{
    /// <summary>
    /// encapsulates the feature instance configuration loaded from module settings into a more friendly object
    /// </summary>
    public class GoogleMapConfiguration
    {
        public GoogleMapConfiguration()
        { }

        public GoogleMapConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            enableMapType = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableMapTypeSetting", enableMapType);

            enableZoom = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableZoomSetting", enableZoom);

            showInfoWindow = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowInfoWindowSetting", showInfoWindow);

            enableLocalSearch = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableLocalSearchSetting", enableLocalSearch);

            enableDrivingDirections = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableDirectionsSetting", enableDrivingDirections);

            useLocationAsTitle = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapUseLocationForTitleSetting", useLocationAsTitle);

            useLocationAsCaption = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowLocationCaptionSetting", useLocationAsCaption);

			UseIframe = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapUseIframeSetting", UseIframe);

            mapHeight = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapHeightSetting", mapHeight);

            //mapWidth = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapWidthSetting", mapWidth);
            if (settings.Contains("GoogleMapWidthSetting"))
            {
                mapWidth = settings["GoogleMapWidthSetting"].ToString();
            }

            zoomSetting = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapInitialZoomSetting", zoomSetting);

            if (settings.Contains("CustomCssClassSetting"))
            {
                InstanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("GoogleMapInitialMapTypeSetting"))
            {
                string gmType = settings["GoogleMapInitialMapTypeSetting"].ToString();
                try
                {
                    GoogleMapType = (MapType)Enum.Parse(typeof(MapType), gmType);
                }
                catch (ArgumentException) { }

            }

            if (settings.Contains("GoogleMapLocationSetting"))
            {
                location = settings["GoogleMapLocationSetting"].ToString();
            }

            if (settings.Contains("GoogleMapApiKeySetting"))
            {
                googleApiKey = settings["GoogleMapApiKeySetting"].ToString();
            }

            if (settings.Contains("GoogleMapCaptionSetting"))
            {
                caption = settings["GoogleMapCaptionSetting"].ToString();
            }

        }

        private string location = string.Empty;

        public string Location
        {
            get { return location; }
        }

        private string caption = string.Empty;

        public string Caption
        {
            get { return caption; }
        }

        private string googleApiKey = string.Empty;

        public string GoogleApiKey
        {
            get { return googleApiKey; }
        }

        private int mapHeight = 300;

        public int MapHeight
        {
            get { return mapHeight; }
        }

        private string mapWidth = "500px";

        public string MapWidth
        {
            get { return mapWidth; }
        }

        private bool enableMapType = false;

        public bool EnableMapType
        {
            get { return enableMapType; }
        }

        private bool enableZoom = false;

        public bool EnableZoom
        {
            get { return enableZoom; }
        }

        private bool showInfoWindow = false;

        public bool ShowInfoWindow
        {
            get { return showInfoWindow; }
        }

        private bool enableLocalSearch = false;

        public bool EnableLocalSearch
        {
            get { return enableLocalSearch; }
        }

        private bool useLocationAsTitle = false;

        public bool UseLocationAsTitle
        {
            get { return useLocationAsTitle; }
        }

        private bool useLocationAsCaption = false;

        public bool UseLocationAsCaption
        {
            get { return useLocationAsCaption; }
        }

        private bool enableDrivingDirections = false;

        public bool EnableDrivingDirections
        {
            get { return enableDrivingDirections; }
        }

		public MapType GoogleMapType { get; private set; } = MapType.G_NORMAL_MAP;

		private int zoomSetting = 13;

        public int ZoomSetting
        {
            get { return zoomSetting; }
        }

		public string InstanceCssClass { get; private set; } = string.Empty;
		public bool UseIframe { get; private set; } = true;

		public MapRatio MapRatio { get; private set; } = MapRatio.r16by9;
	}
}