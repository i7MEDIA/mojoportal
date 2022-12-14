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
    public class AudioPlayerConfiguration
    {
        public AudioPlayerConfiguration()
        { }

        public AudioPlayerConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            autoStart = WebUtils.ParseBoolFromHashtable(settings, "AutoStartSetting", autoStart);
            continuousPlay = WebUtils.ParseBoolFromHashtable(settings, "ContinuousPlaySetting", continuousPlay);
            disableShuffle = WebUtils.ParseBoolFromHashtable(settings, "DisableShuffleSetting", disableShuffle);

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
    }
}