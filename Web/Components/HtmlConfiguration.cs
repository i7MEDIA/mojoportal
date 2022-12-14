using mojoPortal.Web.Framework;
using System;
using System.Collections;

namespace mojoPortal.Web.ContentUI
{
    public class HtmlConfiguration
    {
        public HtmlConfiguration()
        { }

        public HtmlConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            enableContentRatingSetting = WebUtils.ParseBoolFromHashtable(settings, "EnableContentRatingSetting", enableContentRatingSetting);

            enableRatingCommentsSetting = WebUtils.ParseBoolFromHashtable(settings, "EnableRatingCommentsSetting", enableRatingCommentsSetting);

            enableSlideShow = WebUtils.ParseBoolFromHashtable(settings, "HtmlEnableSlideShow", enableSlideShow);

            slideContainerHeight = WebUtils.ParseInt32FromHashtable(settings, "HtmlSlideContainerHeight", slideContainerHeight);

            pauseSlideOnHover = WebUtils.ParseBoolFromHashtable(settings, "HtmlSlideShowPauseOnHover", pauseSlideOnHover);

            slideDuration = WebUtils.ParseInt32FromHashtable(settings, "HtmlSlideDuration", slideDuration);

            transitionSpeed = WebUtils.ParseInt32FromHashtable(settings, "HtmlTransitionSpeed", transitionSpeed);

            randomizeSlides = WebUtils.ParseBoolFromHashtable(settings, "HtmlSlideShowRandomizeSlides", randomizeSlides);

            enableSlideShowPager = WebUtils.ParseBoolFromHashtable(settings, "HtmlEnableSlideShowPager", enableSlideShowPager);

            slideShowPagerBefore = WebUtils.ParseBoolFromHashtable(settings, "HtmlSlideShowPagerBefore", slideShowPagerBefore);

            useSlideClearTypeCorrections = WebUtils.ParseBoolFromHashtable(settings, "HtmlSlideShowUseExtraClearTypeCorrections", useSlideClearTypeCorrections);

            if (settings.Contains("HtmlCustomCssClassSetting"))
            {
                instanceCssClass = settings["HtmlCustomCssClassSetting"].ToString();
            }

            if (settings.Contains("HtmlSlideTransitions"))
            {
                slideTransitions = settings["HtmlSlideTransitions"].ToString();
            }

            if (settings.Contains("HtmlSlideContainerClass"))
            {
                slideContainerClass = settings["HtmlSlideContainerClass"].ToString().Trim();
            }

            showFacebookLikeButton = WebUtils.ParseBoolFromHashtable(settings, "UseFacebookLikeButton", showFacebookLikeButton);

            if (settings.Contains("FacebookLikeButtonTheme"))
            {
                facebookLikeButtonTheme = settings["FacebookLikeButtonTheme"].ToString().Trim();
            }

            facebookLikeButtonShowFaces = WebUtils.ParseBoolFromHashtable(settings, "FacebookLikeButtonShowFaces", facebookLikeButtonShowFaces);

            facebookLikeButtonWidth = WebUtils.ParseInt32FromHashtable(settings, "FacebookLikeButtonWidth", facebookLikeButtonWidth);

            facebookLikeButtonHeight = WebUtils.ParseInt32FromHashtable(settings, "FacebookLikeButtonHeight", facebookLikeButtonHeight);

            enableContentVersioning = WebUtils.ParseBoolFromHashtable(settings, "HtmlEnableVersioningSetting", enableContentVersioning);

            versionPageSize = WebUtils.ParseInt32FromHashtable(settings, "HtmlVersionPageSizeSetting", versionPageSize);

            useWysiwygEditor = WebUtils.ParseBoolFromHashtable(settings, "UseWysiwygEditor", useWysiwygEditor);

            showCreatedBy = WebUtils.ParseBoolFromHashtable(settings, "ShowCreatedBy", showCreatedBy);

            showCreatedDate = WebUtils.ParseBoolFromHashtable(settings, "ShowCreatedDate", showCreatedDate);

            showModifiedBy = WebUtils.ParseBoolFromHashtable(settings, "ShowModifiedBy", showModifiedBy);

            showModifiedDate = WebUtils.ParseBoolFromHashtable(settings, "ShowModifiedDate", showModifiedDate);

            showAuthorAvatar = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthorAvatar", showAuthorAvatar);

            showAuthorBio = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthorBio", showAuthorBio);

            enableSlideClick = WebUtils.ParseBoolFromHashtable(settings, "EnableSlideClick", enableSlideClick);


        }

        private bool enableSlideClick = true;

        public bool EnableSlideClick
        {
            get { return enableSlideClick; }
            set { enableSlideClick = value; }
        }

        private bool enableInlineEditing = true;

        public bool EnableInlineEditing
        {
            get { return enableInlineEditing; }

        }

        private bool showAuthorAvatar = false;

        public bool ShowAuthorAvatar
        {
            get { return showAuthorAvatar; }
            
        }

        private bool showAuthorBio = false;

        public bool ShowAuthorBio
        {
            get { return showAuthorBio; }
            
        }

        private bool showCreatedBy = false;

        public bool ShowCreatedBy
        {
            get { return showCreatedBy; }
        }

        private bool showCreatedDate = false;

        public bool ShowCreatedDate
        {
            get { return showCreatedDate; }
        }

        private bool showModifiedBy = false;

        public bool ShowModifiedBy
        {
            get { return showModifiedBy; }
        }

        private bool showModifiedDate = false;

        public bool ShowModifiedDate
        {
            get { return showModifiedDate; }
        }

        private bool useWysiwygEditor = true;

        public bool UseWysiwygEditor
        {
            get { return useWysiwygEditor; }
        }

        private int versionPageSize = 10;

        public int VersionPageSize
        {
            get { return versionPageSize; }
        }

        private bool enableContentVersioning = false;

        public bool EnableContentVersioning
        {
            get { return enableContentVersioning; }
        }

        private int facebookLikeButtonHeight = 35;

        public int FacebookLikeButtonHeight
        {
            get { return facebookLikeButtonHeight; }
        }

        private int facebookLikeButtonWidth = 450;

        public int FacebookLikeButtonWidth
        {
            get { return facebookLikeButtonWidth; }
        }

        private bool facebookLikeButtonShowFaces = false;

        public bool FacebookLikeButtonShowFaces
        {
            get { return facebookLikeButtonShowFaces; }
        }

        private string facebookLikeButtonTheme = "light";

        public string FacebookLikeButtonTheme
        {
            get { return facebookLikeButtonTheme; }
        }

        private bool enableContentRatingSetting = false;

        public bool EnableContentRatingSetting
        {
            get { return enableContentRatingSetting; }
        }

        private bool enableRatingCommentsSetting = false;

        public bool EnableRatingCommentsSetting
        {
            get { return enableRatingCommentsSetting; }
        }

        private bool enableSlideShow = false;

        public bool EnableSlideShow
        {
            get { return enableSlideShow; }
        }

        private bool enableSlideShowPager = false;

        public bool EnableSlideShowPager
        {
            get { return enableSlideShowPager; }
        }

        private bool slideShowPagerBefore = false;

        public bool SlideShowPagerBefore
        {
            get { return slideShowPagerBefore; }
        }

        private int slideContainerHeight = 0;

        public int SlideContainerHeight
        {
            get { return slideContainerHeight; }
        }

        private string slideTransitions = "fade";

        public string SlideTransitions
        {
            get { return slideTransitions; }
        }

        private bool pauseSlideOnHover = true;

        public bool PauseSlideOnHover
        {
            get { return pauseSlideOnHover; }
        }

        private int slideDuration = 3000;

        public int SlideDuration
        {
            get { return slideDuration; }
        }

        private int transitionSpeed = 1000;

        public int TransitionSpeed
        {
            get { return transitionSpeed; }
        }

        private string slideContainerClass = string.Empty;

        public string SlideContainerClass
        {
            get { return slideContainerClass; }
        }

        private bool randomizeSlides = false;

        public bool RandomizeSlides
        {
            get { return randomizeSlides; }
        }

        private bool useSlideClearTypeCorrections = true;

        public bool UseSlideClearTypeCorrections
        {
            get { return useSlideClearTypeCorrections; }
        }
        

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        private bool showFacebookLikeButton = false;

        public bool ShowFacebookLikeButton
        {
            get { return showFacebookLikeButton; }
        }

        public static bool UseHtmlDiff
        {
            get { return ConfigHelper.GetBoolProperty("HtmlContent:UseHtmlDiff", true); }
        }


    }
}