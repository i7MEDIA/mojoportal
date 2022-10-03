// Author:				    
// Created:			        2010-11-17
// Last Modified:		    2011-02-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-02-16 added changes from Jamie Eubanks to support colorbox

using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.GalleryUI
{
    /// <summary>
    /// encapsulates the feature instance configuration loaded from module settings into a more friendly object
    /// </summary>
    public class GalleryConfiguration
    {
        public GalleryConfiguration()
        { }

        public GalleryConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            showTechnicalData = WebUtils.ParseBoolFromHashtable(settings, "GalleryShowTechnicalDataSetting", showTechnicalData);
            useCompactMode = WebUtils.ParseBoolFromHashtable(settings, "GalleryCompactModeSetting", useCompactMode);
            //useSlideShow = WebUtils.ParseBoolFromHashtable(settings, "UseSilverlightSlideshow", useSlideShow);

            if (settings.Contains("SlideShowTheme"))
            {
                slideShowTheme = settings["SlideShowTheme"].ToString();
            }

            slideShowWidth = WebUtils.ParseInt32FromHashtable(settings, "SlideShowWidth", slideShowWidth);
            slideShowHeight = WebUtils.ParseInt32FromHashtable(settings, "SlideShowHeight", slideShowHeight);

            slideShowWindowlessMode = WebUtils.ParseBoolFromHashtable(settings, "SlideShowWindowlessMode", slideShowWindowlessMode);

            thumbnailWidth = WebUtils.ParseInt32FromHashtable(settings, "GalleryThumbnailWidthSetting", thumbnailWidth);
            thumbnailHeight = WebUtils.ParseInt32FromHashtable(settings, "GalleryThumbnailHeightSetting", thumbnailHeight);
            thumbsPerPage = WebUtils.ParseInt32FromHashtable(settings, "GalleryThumbnailsPerPageSetting", thumbsPerPage);

            webSizeWidth = WebUtils.ParseInt32FromHashtable(settings, "GalleryWebImageWidthSetting", webSizeWidth);
            webSizeHeight = WebUtils.ParseInt32FromHashtable(settings, "GalleryWebImageHeightSetting", webSizeHeight);

            if (settings.Contains("CustomCssClassSetting"))
            {
                customCssClass = settings["CustomCssClassSetting"].ToString();
            }


            if (settings.Contains("ResizeBackgroundColor"))
            {
                resizeBackgroundColor = settings["ResizeBackgroundColor"].ToString();
            }

            
            if (settings.Contains("ColorBoxTransition"))
            {
                colorBoxTransition = settings["ColorBoxTransition"].ToString();
            }
            if (settings.Contains("ColorBoxTransitionSpeed"))
            {
                colorBoxTransitionSpeed = settings["ColorBoxTransitionSpeed"].ToString();
            }
            if (settings.Contains("ColorBoxOpacity"))
            {
                colorBoxOpacity = settings["ColorBoxOpacity"].ToString();
            }

            colorBoxUseSlideshow = WebUtils.ParseBoolFromHashtable(settings, "ColorBoxUseSlideshow", colorBoxUseSlideshow);

            if (settings.Contains("ColorBoxSlideshowSpeed"))
            {
                colorBoxSlideshowSpeed = settings["ColorBoxSlideshowSpeed"].ToString();
            }

            colorBoxSlideshowAuto = WebUtils.ParseBoolFromHashtable(settings, "ColorBoxSlideShowAuto", colorBoxSlideshowAuto);

            colorBoxSlideShowStartAuto = WebUtils.ParseBoolFromHashtable(settings, "ColorBoxSlideShowStartAuto", colorBoxSlideShowStartAuto);

            useNivoSlider = WebUtils.ParseBoolFromHashtable(settings, "UseNivoSlider", useNivoSlider);
           
            
        }

        private bool useNivoSlider = false;

        public bool UseNivoSlider
        {
            get { return useNivoSlider; }
        }

        private string resizeBackgroundColor = "#FFFFFF";

        public Color ResizeBackgroundColor
        {
            get
            {
                try
                {
                    return ColorTranslator.FromHtml(resizeBackgroundColor);
                }
                catch (Exception)
                {
                    return Color.White;
                }
            }
        }

        private bool showTechnicalData = false;

        public bool ShowTechnicalData
        {
            get { return showTechnicalData; }
        }

        private bool useCompactMode = false;

        public bool UseCompactMode
        {
            get { return useCompactMode; }
        }

        //private bool useSlideShow = false;

        public bool UseSlideShow
        {
            get { return false; }
        }

        private string slideShowTheme = "LightTheme";

        public string SlideShowTheme
        {
            get { return slideShowTheme; }
        }

        private int slideShowWidth = 640;

        public int SlideShowWidth
        {
            get { return slideShowWidth; }
        }

        private int slideShowHeight = 480;

        public int SlideShowHeight
        {
            get { return slideShowHeight; }
        }

        private bool slideShowWindowlessMode = false;

        public bool SlideShowWindowlessMode
        {
            get { return slideShowWindowlessMode; }
        }

        private int thumbnailWidth = 100;

        public int ThumbnailWidth
        {
            get { return thumbnailWidth; }
        }

        private int thumbnailHeight = 75;

        public int ThumbnailHeight
        {
            get { return thumbnailHeight; }
        }

        private int thumbsPerPage = 20;

        public int ThumbsPerPage
        {
            get { return thumbsPerPage; }
        }

        private int webSizeWidth = 550;

        public int WebSizeWidth
        {
            get { return webSizeWidth; }
        }

        private int webSizeHeight = 550;

        public int WebSizeHeight
        {
            get { return webSizeHeight; }
        }

        private string customCssClass = string.Empty;

        public string CustomCssClass
        {
            get { return customCssClass; }
        }

       
        public const string ColorBoxDefaultTransition = "elastic";
        public const string ColorBoxDefaultTransitionSpeed = "350";
        public const string ColorBoxDefaultOpacity = ".7";
        public const bool ColorBoxDefaultUseSlideShow = false;
        public const string ColorBoxDefaultSlideShowSpeed = "3500";
        public const bool ColorBoxDefaultSlideShowAuto = true;

        private string colorBoxTransition = ColorBoxDefaultTransition;
        public string ColorBoxTransition { get { return colorBoxTransition; } }
       

        private string colorBoxTransitionSpeed = ColorBoxDefaultTransitionSpeed;
        public string ColorBoxTransitionSpeed { get { return colorBoxTransitionSpeed; } }
       

        private string colorBoxOpacity = ColorBoxDefaultOpacity;
        public string ColorBoxOpacity { get { return colorBoxOpacity; } }
       

        private bool colorBoxUseSlideshow = ColorBoxDefaultUseSlideShow;
        public bool ColorBoxUseSlideshow { get { return colorBoxUseSlideshow; } }
        

        private string colorBoxSlideshowSpeed = ColorBoxDefaultSlideShowSpeed;
        public string ColorBoxSlideshowSpeed { get { return colorBoxSlideshowSpeed; } }
        

        private bool colorBoxSlideshowAuto = ColorBoxDefaultSlideShowAuto;
        public bool ColorBoxSlideshowAuto { get { return colorBoxSlideshowAuto; } }

        private bool colorBoxSlideShowStartAuto = false;
        public bool ColorBoxSlideShowStartAuto
        {
            get { return colorBoxSlideShowStartAuto; }
        }
       

        public static bool UseGreybox
        {
            get { return ConfigHelper.GetBoolProperty("ImageGallery:UseGreybox", false); }
        }

        public static bool DeleteImagesWhenModuleIsDeleted
        {
            get { return ConfigHelper.GetBoolProperty("ImageGallery:DeleteImagesWhenModuleIsDeleted", false); }
        }

        public static int MaxFilesToUploadAtOnce
        {
            get { return ConfigHelper.GetIntProperty("ImageGallery:MaxFilesToUploadAtOnce", 20); }
        }

    }
}