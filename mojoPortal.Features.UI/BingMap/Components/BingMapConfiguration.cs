// Author:				    
// Created:			        2010-06-11
// Last Modified:		    2011-11-28
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
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.MapUI
{
    /// <summary>
    /// encapsulates the feature instance configuration loaded from module settings into a more friendly object
    /// </summary>
    public class BingMapConfiguration
    {
        public BingMapConfiguration()
        { }

        public BingMapConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("LocationSetting"))
            {
                location = settings["LocationSetting"].ToString();
            }

            if (settings.Contains("Caption"))
            {
                caption = settings["Caption"].ToString();
            }


          

            showLocationPin = WebUtils.ParseBoolFromHashtable(settings, "ShowLocationPin", showLocationPin);

            mapHeight = WebUtils.ParseInt32FromHashtable(settings, "MapHeightSetting", mapHeight);

            //mapWidth = WebUtils.ParseInt32FromHashtable(settings, "MapWidthSetting", mapWidth);

            if (settings.Contains("MapWidthSetting"))
            {
                mapWidth = settings["MapWidthSetting"].ToString();
            }

            zoomSetting = WebUtils.ParseInt32FromHashtable(settings, "InitialZoomSetting", zoomSetting);

            if (settings.Contains("InitialMapStyleSetting"))
            {
                mapStyle = settings["InitialMapStyleSetting"].ToString();
            }

            showMapControls = WebUtils.ParseBoolFromHashtable(settings, "ShowMapControls", showMapControls);

            enableDrivingDirections = WebUtils.ParseBoolFromHashtable(settings, "EnableDrivingDirections", enableDrivingDirections);

            if (settings.Contains("DrivingDirectionsDistanceUnit"))
            {
                distanceUnit = settings["DrivingDirectionsDistanceUnit"].ToString();
            }

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
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

        private bool showLocationPin = false;

        public bool ShowLocationPin
        {
            get { return showLocationPin; }
        }

        private string mapStyle = "VEMapStyle.Road";

        public string MapStyle
        {
            get { return mapStyle; }
        }

        private string distanceUnit = "VERouteDistanceUnit.Mile";

        public string DistanceUnit
        {
            get { return distanceUnit; }
        }

        private bool showMapControls = true;

        public bool ShowMapControls
        {
            get { return showMapControls; }
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

        private int zoomSetting = 10;

        public int ZoomSetting
        {
            get { return zoomSetting; }
        }

        private bool enableDrivingDirections = false;

        public bool EnableDrivingDirections
        {
            get { return enableDrivingDirections; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

    }
}