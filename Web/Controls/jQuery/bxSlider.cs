//	Author:            Jamie Eubanks
// Created:			    2012-05-17
//	Last Modified:		 2012-05-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Encapsulation of the bxSlider jQuery content slider - http://www.bxslider.com/
    /// Internally uses a UL
    /// Example and usage tips: http://www.bxslider.com/examples
    /// </summary>
    public class bxSlider : Panel
    {
       private bool a = false; //Track whether we are appending to the script

       #region Control settings

       private string bxScriptFile = "jquery.bxSlider.min.js";
       /// <summary>
       /// Name of bxSlider script file
       /// </summary>
       /// 
       public string BxScriptFile
       {
           get { return bxScriptFile; }
           set { bxScriptFile = value; }
       }

       private bool useEasing = false;
       /// <summary>
       /// When true, jQuery easing will be loaded and easing transitions can be used
       /// </summary>
       /// 
       public bool UseEasing
       {
          get { return useEasing; }
          set { useEasing = value; }
       }

       private string easingScriptFile = "jquery.easing.1.3.js";
       /// <summary>
       /// Name of easing script file
       /// </summary>
       /// 
       public string EasingScriptFile
       {
          get { return easingScriptFile; }
          set { easingScriptFile = value; }
       }

       private bool autoLocalize = true;
       /// <summary>
       /// When set to true, will pull "previous" and "next" text from localized resource file
       /// </summary>
       /// 
       public bool AutoLocalize
       {
          get { return autoLocalize; }
          set { autoLocalize = value; }
       }


       #endregion

       #region bxSlider General Settings

       private const string modeDefault = "horizontal";
       private const int speedDefault = 500;
       private const bool infiniteLoopDefault = true;
       private const bool controlsDefault = true;
       private const string prevTextDefault = "prev";
       private const string prevImageDefault = "";
       private const string prevSelectorDefault = "";
       private const string nextTextDefault = "next";
       private const string nextImageDefault = "";
       private const string nextSelectorDefault = "";
       private const int startingSlideDefault = 0;
       private const bool randomStartDefault = false;
       private const bool hideControlOnEndDefault = false;
       private const bool captionsDefault = false;
       private const string captionsSelectorDefault = "";
       private const string wrapperClassDefault = "bx-wrapper";
       private const string easingDefault = "swing";

       private string mode = "horizontal";
       /// <summary>
       /// Type of transition between each slide
       /// Options: horizontal (D), vertical, fade
       /// </summary>
       /// 
       public string Mode
       {
          get { return mode; }
          set { mode = value; }
       }

       private int speed = 500;
       /// <summary>
       /// In ms, duration of time slide transitions
       /// </summary>
       /// 
       public int Speed
       {
          get { return speed; }
          set { speed = value; }
       }

       private bool infiniteLoop  = true;
       /// <summary>
       /// Display the first slide successively after the last
       /// </summary>
       /// 
       public bool InfinteLoop
       {
          get { return infiniteLoop; }
          set { infiniteLoop = value; }
       }

       private bool displayControls = true;
       /// <summary>
       /// Display previous and next controls
       /// </summary>
       /// 
       public bool DisplayControls
       {
           get { return displayControls; }
           set { displayControls = value; }
       }

       private string prevText = "prev";
       /// <summary>
       /// Text displayed for 'previous' control
       /// </summary>
       /// 
       public string PrevText
       {
          get { return prevText; }
          set { prevText = value; }
       }

       private string prevImage = "";
       /// <summary>
       /// Filepath of image used for 'previous' control. ex: "images/prev.jpg"
       /// </summary>
       /// 
       public string PrevImage
       {
          get { return prevImage; }
          set { prevImage = value; }
       }

       private string prevSelector = "";
       /// <summary>
       /// jQuery selector - element to contain the previous control, e.g. "#prev"
       /// </summary>
       /// 
       public string PrevSelector
       {
          get { return prevSelector; }
          set { prevSelector = value; }
       }

       private string nextText = "next";
       /// <summary>
       /// Text displayed for 'next' control
       /// </summary>
       /// 
       public string NextText
       {
          get { return nextText; }
          set { nextText = value; }
       }

       private string nextImage = "";
       /// <summary>
       /// Filepath of image used for 'next' control. ex: "images/next.jpg"
       /// </summary>
       /// 
       public string NextImage
       {
          get { return nextImage; }
          set { nextImage = value; }
       }

       private string nextSelector = "";
       /// <summary>
       /// jQuery selector - element to contain the next control, e.g. "#next"
       /// </summary>
       /// 
       public string NextSelector
       {
          get { return nextSelector; }
          set { nextSelector = value; }
       }

       private int startingSlide = 0;
       /// <summary>
       /// Show will start on specified slide. Note: slide numbering is zero based
       /// </summary>
       /// 
       public int StartingSlide
       {
          get { return startingSlide; }
          set { startingSlide = value; }
       }

       private bool randomStart = false;
       /// <summary>
       /// If true show will start on a random slide
       /// </summary>
       /// 
       public bool RandomStart
       {
          get { return randomStart; }
          set { randomStart = value; }
       }

       private bool hideControlOnEnd = false;
       /// <summary>
       /// If true will hide 'next' control on last slide and 'prev' control on first
       /// </summary>
       /// 
       public bool HideControlOnEnd
       {
          get { return hideControlOnEnd; }
          set { hideControlOnEnd = value; }
       }

       private bool captions = false;
       /// <summary>
       /// Display image captions (reads the image 'title' attribute)
       /// </summary>
       /// 
       public bool Captions
       {
          get { return captions; }
          set { captions = value; }
       }

       private string captionsSelector = "";
       /// <summary>
       /// jQuery selector - element to contain the captions, e.g. '#captions'
       /// </summary>
       /// 
       public string CaptionsSelector
       {
          get { return captionsSelector; }
          set { captionsSelector = value; }
       }

       private string wrapperClass = "bx-wrapper";
       /// <summary>
       /// Classname attached to the slider wrapper
       /// </summary>
       /// 
       public string WrapperClass
       {
          get { return wrapperClass; }
          set { wrapperClass = value; }
       }

       private string easing = "swing";
       /// <summary>
       /// Used with jquery.easing.1.3.js - see http://gsgd.co.uk/sandbaox/jquery/easing/ for available options
       /// </summary>
       /// 
       public string Easing
       {
          get { return easing; }
          set { easing = value; }
       }

        #endregion

       #region bxSlider Auto Settings

       private const bool autoDefault = false;
       private const int pauseDefault = 3000;
       private const bool autoControlsDefault = false;
       private const string autoControlsSelectorDefault = "";
       private const string startTextDefault = "start";
       private const string startImageDefault = "";
       private const string stopTextDefault = "stop";
       private const string stopImageDefault = "";
       private const int autoDelayDefault = 0;
       private const string autoDirectionDefault = "next";
       private const bool autoHoverDefault = false;
       private const bool autoStartDefault = true;

       private bool auto = false;
       /// <summary>
        /// Make slide transitions occur automatically
        /// </summary>
        public bool Auto
        {
            get { return auto; }
            set { auto = value; }
        }

        private int pause = 3000;
        /// <summary>
        /// In ms, the duration between each slide transition
        /// </summary>
        public int Pause
        {
           get { return pause; }
           set { pause = value; }
        }

        private bool autoControls = false;
        /// <summary>
        /// Display 'start' and 'stop' controls for auto show
        /// </summary>
        public bool AutoControls
        {
           get { return autoControls; }
           set { autoControls = value; }
        }

        private string autoControlsSelector = "";
        /// <summary>
        /// jQuery selector element to contain the auto controls. e.g. "#auto-controls"
        /// </summary>
        public string AutoControlsSelector
        {
           get { return autoControlsSelector; }
           set { autoControlsSelector = value; }
        }

        private string startText = "start";
        /// <summary>
        /// Text displayed for 'start' control
        /// </summary>
        /// 
        public string StartText
        {
           get { return startText; }
           set { startText = value; }
        }

        private string startImage = "";
        /// <summary>
        /// Filepath of image used for 'start' control. ex: "images/start.jpg"
        /// </summary>
        /// 
        public string StartImage
        {
           get { return startImage; }
           set { startImage = value; }
        }

        private string stopText = "stop";
        /// <summary>
        /// Text displayed for 'stop' control
        /// </summary>
        /// 
        public string StopText
        {
           get { return stopText; }
           set { stopText = value; }
        }

        private string stopImage = "";
        /// <summary>
        /// Filepath of image used for 'stop' control. ex: "images/stop.jpg"
        /// </summary>
        /// 
        public string StopImage
        {
           get { return StopImage; }
           set { StopImage = value; }
        }

        private int autoDelay = 0;
        /// <summary>
        /// In ms, the amount of time before starting the auto show
        /// </summary>
        /// 
        public int AutoDelay
        {
           get { return autoDelay; }
           set { AutoDelay = value; }
        }

        private string autoDirection = "next";
        /// <summary>
        /// Direction in which auto show will traverse
        /// Options: next, prev
        /// </summary>
        /// 
        public string AutoDirection
        {
           get { return autoDirection; }
           set { autoDirection = value; }
        }

        private bool autoHover = false;
        /// <summary>
        /// If true show will pause on mouse over
        /// </summary>
        public bool AutoHover
        {
           get { return autoHover; }
           set { autoHover = value; }
        }

        private bool autoStart = true;
        /// <summary>
        /// If false show will wait for 'start' control to be clicked to activate
        /// </summary>
        public bool AutoStart
        {
           get { return autoStart; }
           set { autoStart = value; }
        }

       #endregion

       #region bxSlider Pager Settings

        private const bool pagerDefault = false;
        private const string pagerTypeDefault = "full";
        private const string pagerSelectorDefault = "";
        private const string pagerLocationDefault = "bottom";
        private const string pagerShortSeparatorDefault = "/";
        private const string pagerActiveClassDefault = "pager-active";

       private bool pager = false;
       /// <summary>
        /// Display a numeric pager
        /// </summary>
        public bool Pager
        {
           get { return pager; }
           set { pager = value; }
        }

        private string pagerType = "full";
        /// <summary>
        /// If "full", pager displays 1,2,3... If "short" pager displays 1 / 4
        /// Options: full, short
        /// </summary>
        public string PagerType
        {
            get { return pagerType; }
            set { pagerType = value; }
        }

        private string pagerSelector = "";
        /// <summary>
        /// jQuery selector - element to contain the pager. e.g. "#pager"
        /// </summary>
        public string PagerSelector
        {
           get { return pagerSelector; }
           set { pagerSelector = value; }
        }

        private string pagerLocation = "bottom";
        /// <summary>
        /// Location of pager
        /// Options: bottom, top
        /// </summary>
        public string PagerLocation
        {
           get { return pagerLocation; }
           set { pagerLocation = value; }
        }

        private string pagerShortSeparator = "/";
        /// <summary>
        /// Characters used in between 'short' pager numbers. e.g. value "of" would display 1 of 4
        /// </summary>
        public string PagerShortSeparator
        {
            get { return pagerShortSeparator; }
            set { pagerShortSeparator = value; }
        }

        private string pagerActiveClass = "pager-active";
        /// <summary>
        /// Classname attached to the active pager link
        /// </summary>
        public string PagerActiveClass
        {
            get { return pagerActiveClass; }
            set { pagerActiveClass = value; }
        }
       #endregion

       #region bxSlider Multiple Slide Display Settings

        private const int displaySlideQtyDefault = 1;
        private const int moveSlideQtyDefault = 1;

        private int displaySlideQty = 1;
        /// <summary>
        /// Number of slides to display at once
        /// </summary>
        public int DisplaySlideQty
        {
            get { return displaySlideQty; }
            set { displaySlideQty = value; }
        }

        private int moveSlideQty = 1;
        /// <summary>
        /// Number of slides to move at once
        /// </summary>
        public int MoveSlideQty
        {
           get { return moveSlideQty; }
           set { moveSlideQty = value; }
        }
        #endregion

       #region bxSlider Ticker Settings

        private const bool tickerDefault = false;
        private const int tickerSpeedDefault = 5000;
        private const string tickerDirectionDefault = "next";
        private const bool tickerHoverDefault = false;

        private bool ticker = false;
        /// <summary>
        /// Continuous motion ticker mode (similar to a news ticker)
        /// </summary>
        public bool Ticker
        {
            get { return ticker; }
            set { ticker = value; }
        }

        private int tickerSpeed = 5000;
        /// <summary>
        /// Use a value between 1 and 5000 to determine ticker speed. The smaller the value the faster the ticker speed
        /// Options: Integer between 1 and 5000
        /// </summary>
        public int TickerSpeed
        {
            get { return tickerSpeed; }
            set { tickerSpeed = value; }
        }

        private string tickerDirection = "next";
        /// <summary>
        /// Direction in which ticker show will traverse
        /// </summary>
        public string TickerDirection
        {
            get { return tickerDirection; }
            set { tickerDirection = value; }
        }

        private bool tickerHover = false;
        /// <summary>
        /// If true ticker will pause on mouseover
        /// </summary>
        public bool TickerHover
        {
            get { return tickerHover; }
            set { tickerHover = value; }
        }
       #endregion

        private static void SBApp(ref bool a, ref StringBuilder sb, string id, bool input)
        {
           if (a) sb.Append(","); else a = true;
           sb.Append(id + ": " + input.ToString().ToLower());
        }

        private static void SBApp(ref bool a, ref StringBuilder sb, string id, int input)
        {
           if (a) sb.Append(","); else a = true;
           sb.Append(id + ": " + input.ToInvariantString());
        }

        private static void SBApp(ref bool a, ref StringBuilder sb, string id, string input)
        {
           if (a) sb.Append(","); else a = true;
           sb.Append(id + ": '" + input + "'");
        }

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");
            script.Append("$(document).ready(function(){");
            script.Append("$('#" + this.ClientID + "').bxSlider({");

            if (auto != autoDefault) { SBApp(ref a, ref script, "auto", auto); }
            if (mode != modeDefault) { SBApp(ref a, ref script, "mode", mode); }
            if (speed != speedDefault) { SBApp(ref a, ref script, "speed", speed); }
            if (infiniteLoop != infiniteLoopDefault) { SBApp(ref a, ref script, "infiniteLoop", infiniteLoop); }
            if (displayControls != controlsDefault) { SBApp(ref a, ref script, "controls", displayControls); }
            if (prevText != prevTextDefault) { SBApp(ref a, ref script, "prevText", prevText); }
            if (prevImage != prevImageDefault) { SBApp(ref a, ref script, "prevImage", prevImage); }
            if (prevSelector != prevSelectorDefault) { SBApp(ref a, ref script, "prevSelector", prevSelector); }
            if (nextText != nextTextDefault) { SBApp(ref a, ref script, "nextText", nextText); }
            if (nextImage != nextImageDefault) { SBApp(ref a, ref script, "nextImage", nextImage); }
            if (nextSelector != nextSelectorDefault) { SBApp(ref a, ref script, "nextSelector", nextSelector); }
            if (startingSlide != startingSlideDefault) { SBApp(ref a, ref script, "startingSlide", startingSlide); }
            if (randomStart != randomStartDefault) { SBApp(ref a, ref script, "randomStart", randomStart); }
            if (hideControlOnEnd != hideControlOnEndDefault) { SBApp(ref a, ref script, "hideControlOnEnd", hideControlOnEnd); }
            if (captions != captionsDefault) { SBApp(ref a, ref script, "captions", captions); }
            if (captionsSelector != captionsSelectorDefault) { SBApp(ref a, ref script, "captionsSelector", captionsSelector); }
            if (wrapperClass != wrapperClassDefault) { SBApp(ref a, ref script, "wrapperClass", wrapperClass); }
            if (easing != easingDefault) { SBApp(ref a, ref script, "easing", easing); }
            if (pause != pauseDefault) { SBApp(ref a, ref script, "pause", pause); }
            if (autoControls != autoControlsDefault) { SBApp(ref a, ref script, "autoControls", autoControls); }
            if (autoControlsSelector != autoControlsSelectorDefault) { SBApp(ref a, ref script, "autoControlsSelector", autoControlsSelector); }
            if (startText != startTextDefault) { SBApp(ref a, ref script, "startText", startText); }
            if (startImage != startImageDefault) { SBApp(ref a, ref script, "startImage", startImage); }
            if (stopText != stopTextDefault) { SBApp(ref a, ref script, "stopText", stopText); }
            if (stopImage != stopImageDefault) { SBApp(ref a, ref script, "stopImage", stopImage); }
            if (autoDelay != autoDelayDefault) { SBApp(ref a, ref script, "autoDelay", autoDelay); }
            if (autoDirection != autoDirectionDefault) { SBApp(ref a, ref script, "autoDirection", autoDirection); }
            if (autoHover != autoHoverDefault) { SBApp(ref a, ref script, "autoHover", autoHover); }
            if (autoStart != autoStartDefault) { SBApp(ref a, ref script, "autoStart", autoStart); }
            if (pager != pagerDefault) { SBApp(ref a, ref script, "pager", pager); }
            if (pagerType != pagerTypeDefault) { SBApp(ref a, ref script, "pagerType", pagerType); }
            if (pagerSelector != pagerSelectorDefault) { SBApp(ref a, ref script, "pagerSelector", pagerSelector); }
            if (pagerLocation != pagerLocationDefault) { SBApp(ref a, ref script, "pagerLocation", pagerLocation); }
            if (pagerShortSeparator != pagerShortSeparatorDefault) { SBApp(ref a, ref script, "pagerShortSeparator", pagerShortSeparator); }
            if (pagerActiveClass != pagerActiveClassDefault) { SBApp(ref a, ref script, "pagerActiveClass", pagerActiveClass); }
            if (displaySlideQty != displaySlideQtyDefault) { SBApp(ref a, ref script, "displaySlideQty", displaySlideQty); }
            if (moveSlideQty != moveSlideQtyDefault) { SBApp(ref a, ref script, "moveSlideQty", moveSlideQty); }
            if (ticker != tickerDefault) { SBApp(ref a, ref script, "ticker", ticker); }
            if (tickerSpeed != tickerSpeedDefault) { SBApp(ref a, ref script, "tickerSpeed", tickerSpeed); }
            if (tickerDirection != tickerDirectionDefault) { SBApp(ref a, ref script, "tickerDirection", tickerDirection); }
            if (tickerHover != tickerHoverDefault) { SBApp(ref a, ref script, "tickerHover", tickerHover); }

            script.Append("}); ");
            script.Append("}); ");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }

        private void SetupMainScript()
        {
            //we are assuming that jquery is already loaded in the page via script loader

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jbxslider", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/" + bxScriptFile) + "\" type=\"text/javascript\" ></script>");

            if (useEasing)
            {
               Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "jbxeasing", "\n<script  src=\""
                        + Page.ResolveUrl("~/ClientScript/jqmojo/" + easingScriptFile) + "\" type=\"text/javascript\" ></script>");
            }


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (autoLocalize)
            {
                prevText = Resource.Previous;
                nextText = Resource.Next;

            }
            if (CssClass.Length == 0) { CssClass = "bxslider"; }
            SetupMainScript();
            SetupInstanceScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class='" + wrapperClass + "'>");
            base.Render(writer);
            writer.Write("</div>");
        }

    }
}