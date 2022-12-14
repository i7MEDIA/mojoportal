// Author:				    
// Created:			        2011-12-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Last Modified: 2011-12-06

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.MediaPlayerUI
{
    public class VideoPlayerConfiguration
    {
        public VideoPlayerConfiguration()
        { }

        public VideoPlayerConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            autoStart = WebUtils.ParseBoolFromHashtable(settings, "AutoStartSetting", autoStart);
            continuousPlay = WebUtils.ParseBoolFromHashtable(settings, "ContinuousPlaySetting", continuousPlay);
            disableShuffle = WebUtils.ParseBoolFromHashtable(settings, "DisableShuffleSetting", disableShuffle);

            allowFullScreen = WebUtils.ParseBoolFromHashtable(settings, "AllowFullScreen", allowFullScreen);

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("HeaderContent"))
            {
                headerContent = settings["HeaderContent"].ToString();
            }

            if (settings.Contains("FooterContent"))
            {
                footerContent = settings["FooterContent"].ToString();
            }
        }

        private string headerContent = string.Empty;

        public string HeaderContent
        {
            get { return headerContent; }
        }

        private string footerContent = string.Empty;

        public string FooterContent
        {
            get { return footerContent; }
        }

        private string instanceCssClass = "bluemonday";

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        private bool allowFullScreen = true;

        public bool AllowFullScreen
        {
            get { return allowFullScreen; }
        }

        private bool autoStart = false;

        public bool AutoStart
        {
            get { return autoStart; }
        }

        private bool continuousPlay = false;

        public bool ContinuousPlay
        {
            get { return continuousPlay; }
        }

        private bool disableShuffle = false;

        public bool DisableShuffle
        {
            get { return disableShuffle; }
        }

        public static bool EditPageSuppressPageMenu
        {
            get { return ConfigHelper.GetBoolProperty("KDMediaPlayer:EditSuppressPageMenu", true); }
        }

        public static bool EnableWarnings
        {
            get { return ConfigHelper.GetBoolProperty("KDMediaPlayer:EnableWarnings", false); }
        }

        public static bool EnableErrors
        {
            get { return ConfigHelper.GetBoolProperty("KDMediaPlayer:EnableErrors", true); }
        }

        /// <summary>
        /// options are window, transparent, opaque, direct, gpu
        /// </summary>
        public static string VideoWindowMode
        {
            get { return ConfigHelper.GetStringProperty("KDMediaPlayer:VideoWindowMode", "opaque"); }
        }

        /// <summary>
        /// valid options are metadata, none, and auto, default is metadata
        /// </summary>
        public static string VideoPreload
        {
            get { return ConfigHelper.GetStringProperty("KDMediaPlayer:VideoPreload", "metadata"); }
        }
    }
}