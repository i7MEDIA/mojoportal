// Author:					
// Created:				    2009-09-12
// Last Modified:			2013-08-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2011-02-17 modifications by Joe Davis for pager

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// A wrapper control for http://malsup.com/jquery/cycle/
    /// Using a panel which renders as a div any first child elements of the panel can be cycled
    /// </summary>
    public class SlidePanel : BasePanel
    {
        /*
         Options
         The following default options are provided by the plugin. They can be overridden by passing an options object to the cycle method, by using metadata 
         on the container element, or by redefining these values in your own code. Lots of cool effects can be achieved by taking advantage of these options, 
         but none of them are required. So override the ones you like and ignore the ones you don't care about.
        
         $.fn.cycle.defaults = { 
    fx:           'fade', // name of transition effect (or comma separated names, ex: fade,scrollUp,shuffle) 
    timeout:       4000,  // milliseconds between slide transitions (0 to disable auto advance) 
    timeoutFn:     null,  // callback for determining per-slide timeout value:  function(currSlideElement, nextSlideElement, options, forwardFlag) 
    continuous:    0,     // true to start next transition immediately after current one completes 
    speed:         1000,  // speed of the transition (any valid fx speed value) 
    speedIn:       null,  // speed of the 'in' transition 
    speedOut:      null,  // speed of the 'out' transition 
    next:          null,  // selector for element to use as click trigger for next slide 
    prev:          null,  // selector for element to use as click trigger for previous slide 
    prevNextClick: null,  // callback fn for prev/next clicks:  function(isNext, zeroBasedSlideIndex, slideElement) 
    pager:         null,  // selector for element to use as pager container 
    pagerClick:    null,  // callback fn for pager clicks:  function(zeroBasedSlideIndex, slideElement) 
    pagerEvent:   'click', // name of event which drives the pager navigation 
    pagerAnchorBuilder: null, // callback fn for building anchor links:  function(index, DOMelement) 
    before:        null,  // transition callback (scope set to element to be shown):     function(currSlideElement, nextSlideElement, options, forwardFlag) 
    after:         null,  // transition callback (scope set to element that was shown):  function(currSlideElement, nextSlideElement, options, forwardFlag) 
    end:           null,  // callback invoked when the slideshow terminates (use with autostop or nowrap options): function(options) 
    easing:        null,  // easing method for both in and out transitions 
    easeIn:        null,  // easing for "in" transition 
    easeOut:       null,  // easing for "out" transition 
    shuffle:       null,  // coords for shuffle animation, ex: { top:15, left: 200 } 
    animIn:        null,  // properties that define how the slide animates in 
    animOut:       null,  // properties that define how the slide animates out 
    cssBefore:     null,  // properties that define the initial state of the slide before transitioning in 
    cssAfter:      null,  // properties that defined the state of the slide after transitioning out 
    fxFn:          null,  // function used to control the transition: function(currSlideElement, nextSlideElement, options, afterCalback, forwardFlag) 
    height:       'auto', // container height 
    startingSlide: 0,     // zero-based index of the first slide to be displayed 
    sync:          1,     // true if in/out transitions should occur simultaneously 
    random:        0,     // true for random, false for sequence (not applicable to shuffle fx) 
    fit:           0,     // force slides to fit container 
    containerResize: 1,   // resize container to fit largest slide 
    pause:         0,     // true to enable "pause on hover" 
    pauseOnPagerHover: 0, // true to pause when hovering over pager link 
    autostop:      0,     // true to end slideshow after X transitions (where X == slide count) 
    autostopCount: 0,     // number of transitions (optionally used with autostop to define X) 
    delay:         0,     // additional delay (in ms) for first transition (hint: can be negative) 
    slideExpr:     null,  // expression for selecting slides (if something other than all children is required) 
    cleartype:     !$.support.opacity,  // true if clearType corrections should be applied (for IE) 
    cleartypeNoBg: false, // set to true to disable extra cleartype fixing (leave false to force background color setting on slides) 
    nowrap:        0,     // true to prevent slideshow from wrapping 
    fastOnEvent:   0,     // force fast transitions when triggered manually (via pager or prev/next); value == time in ms 
    randomizeEffects: 1,  // valid when multiple effects are used; true to make the effect sequence random 
    rev:           0,     // causes animations to transition in reverse 
    manualTrump:   true,  // causes manual transition to stop an active transition instead of being ignored 
    requeueOnImageNotLoaded: true, // requeue the slideshow if any image slides are not yet loaded 
    requeueTimeout: 250   // ms delay for requeue 
};
        
         */

        private bool enableSlideShow = true;
        /// <summary>
        /// true by default, set to false and the slideshow is disabled and behavior is like a normal <asp:Panel
        /// </summary>
        public bool EnableSlideShow
        {
            get { return enableSlideShow; }
            set { enableSlideShow = value; }
        }

        private string fx = "fade";
        /// <summary>
        /// name of transition effect (or comma separated names, ex: fade,scrollUp,scrollDown,scrollRight,scrollLeft,turnUp,turnDown,turnRight,turnLeft,curtainX,curtainY,slideX,slideY,shuffle,pinch,zoom) 
        /// </summary>
        public string TransitionEffect
        {
            get { return fx; }
            set { fx = value; }
        }

        private int speed = 1000;
        /// <summary>
        /// speed of the transition (any valid fx speed value) 
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private int speedIn = 0;
        /// <summary>
        /// speed of the 'in' transition 
        /// </summary>
        public int SpeedIn
        {
            get { return speedIn; }
            set { speedIn = value; }
        }

        private int speedOut = 0;
        /// <summary>
        /// speed of the 'out' transition 
        /// </summary>
        public int SpeedOut
        {
            get { return speedOut; }
            set { speedOut = value; }
        }

        private int timeout = 3000;
        /// <summary>
        /// milliseconds between slide transitions (0 to disable auto advance) 
        /// </summary>
        public int TransitionInterval
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private string timeoutFn = string.Empty;
        /// <summary>
        /// callback for determining per-slide timeout value:  function(currSlideElement, nextSlideElement, options, forwardFlag)
        /// </summary>
        public string TransitionIntervalCallback
        {
            get { return timeoutFn; }
            set { timeoutFn = value; }
        }

        private bool pauseOnHover = false;
        /// <summary>
        /// true to enable "pause on hover" 
        /// </summary>
        public bool PauseOnHover
        {
            get { return pauseOnHover; }
            set { pauseOnHover = value; }
        }

        private bool continuous = false;
        /// <summary>
        /// true to start next transition immediately after current one completes 
        /// </summary>
        public bool Continuous
        {
            get { return continuous; }
            set { continuous = value; }
        }

        private string next = string.Empty;
        /// <summary>
        /// selector for element to use as click trigger for next slide 
        /// </summary>
        public string NextSelector
        {
            get { return next; }
            set { next = value; }
        }

        private string prev = string.Empty;
        /// <summary>
        /// selector for element to use as click trigger for previous slide
        /// </summary>
        public string PreviousSelector
        {
            get { return prev; }
            set { prev = value; }
        }

        private string prevNextClick = string.Empty;
        /// <summary>
        /// callback fn for prev/next clicks:  function(isNext, zeroBasedSlideIndex, slideElement) 
        /// </summary>
        public string PrevNextClick
        {
            get { return prevNextClick; }
            set { prevNextClick = value; }
        }

        private string pager = string.Empty;
        /// <summary>
        /// selector for element to use as pager container 
        /// </summary>
        public string Pager
        {
            get { return pager; }
            set { pager = value; }
        }

        private bool pagerBefore = false;
        public bool PagerBefore
        {
            get { return PagerBefore; }
            set { pagerBefore = value; }
        }

        private string pagerClick = string.Empty;
        /// <summary>
        /// callback fn for pager clicks:  function(zeroBasedSlideIndex, slideElement)
        /// </summary>
        public string PagerClick
        {
            get { return pagerClick; }
            set { pagerClick = value; }
        }

        private string pagerEvent = "click";
        /// <summary>
        /// name of event which drives the pager navigation, default is click
        /// </summary>
        public string PagerEvent
        {
            get { return pagerEvent; }
            set { pagerEvent = value; }
        }

        private string pagerAnchorBuilder = string.Empty;
        /// <summary>
        /// callback fn for building anchor links:  function(index, DOMelement)
        /// </summary>
        public string PagerAnchorBuilder
        {
            get { return pagerAnchorBuilder; }
            set { pagerAnchorBuilder = value; }
        }

        private string before = string.Empty;
        /// <summary>
        /// transition callback (scope set to element to be shown):     function(currSlideElement, nextSlideElement, options, forwardFlag) 
        /// </summary>
        public string Before
        {
            get { return before; }
            set { before = value; }
        }

        private string after = string.Empty;
        /// <summary>
        /// transition callback (scope set to element that was shown):  function(currSlideElement, nextSlideElement, options, forwardFlag)
        /// </summary>
        public string After
        {
            get { return after; }
            set { after = value; }
        }

        private string end = string.Empty;
        /// <summary>
        /// callback invoked when the slideshow terminates (use with autostop or nowrap options): function(options) 
        /// </summary>
        public string End
        {
            get { return end; }
            set { end = value; }
        }

        //TODO: implement? Requires easing plugin
        // easing method for both in and out transitions 
        private string easing = string.Empty;
        // easing for "in" transition 
        private string easeIn = string.Empty;
        // easing for "out" transition
        private string easeOut = string.Empty;

        private string shuffle = string.Empty;
        /// <summary>
        /// coords for shuffle animation, ex: { top:15, left: 200 } 
        /// </summary>
        public string Shuffle
        {
            get { return shuffle; }
            set { shuffle = value; }
        }

        private string animIn = string.Empty;
        /// <summary>
        /// properties that define how the slide animates in
        /// </summary>
        public string AnimIn
        {
            get { return animIn; }
            set { animIn = value; }
        }

        private string animOut = string.Empty;
        /// <summary>
        /// properties that define how the slide animates out
        /// </summary>
        public string AnimOut
        {
            get { return animOut; }
            set { animOut = value; }
        }

        private string cssBefore = string.Empty;
        /// <summary>
        /// properties that define the initial state of the slide before transitioning in
        /// </summary>
        public string CssBefore
        {
            get { return cssBefore; }
            set { cssBefore = value; }
        }

        private string cssAfter = string.Empty;
        /// <summary>
        /// properties that defined the state of the slide after transitioning out
        /// </summary>
        public string CssAfter
        {
            get { return cssAfter; }
            set { cssAfter = value; }
        }

        private string fxFn = string.Empty;
        /// <summary>
        /// function used to control the transition: function(currSlideElement, nextSlideElement, options, afterCalback, forwardFlag) 
        /// </summary>
        public string FxFn
        {
            get { return fxFn; }
            set { fxFn = value; }
        }

        private string height = "auto";
        /// <summary>
        /// container height default is auto
        /// </summary>
        public string ContainerHeight
        {
            get { return height; }
            set { height = value; }
        }

        private int startingSlide = 0;
        /// <summary>
        /// zero-based index of the first slide to be displayed
        /// </summary>
        public int StartingSlide
        {
            get { return startingSlide; }
            set { startingSlide = value; }
        }

        private bool sync = true;
        /// <summary>
        /// true if in/out transitions should occur simultaneously default is true
        /// </summary>
        public bool Sync
        {
            get { return sync; }
            set { sync = value; }
        }

        private bool random = false;
        /// <summary>
        /// true for random, false for sequence (not applicable to shuffle fx) 
        /// </summary>
        public bool Random
        {
            get { return random; }
            set { random = value; }
        }

        private bool fit = false;
        /// <summary>
        /// force slides to fit container default is false
        /// </summary>
        public bool Fit
        {
            get { return fit; }
            set { fit = value; }
        }

        private bool containerResize = true;
        /// <summary>
        /// resize container to fit largest slide default is true
        /// </summary>
        public bool ContainerResize
        {
            get { return containerResize; }
            set { containerResize = value; }
        }

        private bool pauseOnPagerHover = false;
        /// <summary>
        /// true to pause when hovering over pager link default is false
        /// </summary>
        public bool PauseOnPagerHover
        {
            get { return pauseOnPagerHover; }
            set { pauseOnPagerHover = value; }
        }

        private bool autostop = false;
        /// <summary>
        /// true to end slideshow after X transitions (where X == slide count), default is false
        /// </summary>
        public bool Autostop
        {
            get { return autostop; }
            set { autostop = value; }
        }

        private int autostopCount = 0;
        /// <summary>
        /// number of transitions (optionally used with autostop to define X)
        /// </summary>
        public int AutostopCount
        {
            get { return autostopCount; }
            set { autostopCount = value; }
        }

        private int delay = 0;
        /// <summary>
        /// additional delay (in ms) for first transition (hint: can be negative)
        /// </summary>
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        private string slideExpr = string.Empty;
        /// <summary>
        /// expression for selecting slides (if something other than all children is required) 
        /// </summary>
        public string SlideExpr
        {
            get { return slideExpr; }
            set { slideExpr = value; }
        }

        private bool cleartypeNoBg = false;
        /// <summary>
        /// set to true to disable extra cleartype fixing (leave false to force background color setting on slides)
        /// </summary>
        public bool CleartypeNoBg
        {
            get { return cleartypeNoBg; }
            set { cleartypeNoBg = value; }
        }

        private bool nowrap = false;
        /// <summary>
        /// true to prevent slideshow from wrapping
        /// </summary>
        public bool Nowrap
        {
            get { return nowrap; }
            set { nowrap = value; }
        }

        private int fastOnEvent = 0;
        /// <summary>
        /// force fast transitions when triggered manually (via pager or prev/next); value == time in ms 
        /// </summary>
        public int FastOnEvent
        {
            get { return fastOnEvent; }
            set { fastOnEvent = value; }
        }

        private bool randomizeEffects = true;
        /// <summary>
        /// valid when multiple effects are used; true to make the effect sequence random 
        /// </summary>
        public bool RandomizeEffects
        {
            get { return randomizeEffects; }
            set { randomizeEffects = value; }
        }

        private bool rev = false;
        /// <summary>
        /// causes animations to transition in reverse 
        /// </summary>
        public bool ReverseTransitions
        {
            get { return rev; }
            set { rev = value; }
        }

        private bool manualTrump = true;
        /// <summary>
        /// causes manual transition to stop an active transition instead of being ignored
        /// </summary>
        public bool ManualTrump
        {
            get { return manualTrump; }
            set { manualTrump = value; }
        }

        private bool requeueOnImageNotLoaded = true;
        /// <summary>
        /// requeue the slideshow if any image slides are not yet loaded default is true
        /// </summary>
        public bool RequeueOnImageNotLoaded
        {
            get { return requeueOnImageNotLoaded; }
            set { requeueOnImageNotLoaded = value; }
        }

        private int requeueTimeout = 250;

        public int RequeueTimeout
        {
            get { return requeueTimeout; }
            set { requeueTimeout = value; }
        }

        private string slideContainerClass = string.Empty;
        /// <summary>
        /// this is if you want to supplement the id selector so the slide container is not the outer div but any div inside the outer div with a particular css class.
        /// </summary>
        public string SlideContainerClass
        {
            get { return slideContainerClass; }
            set { slideContainerClass = value; }
        }


        private bool enableSlideClick = true;
        /// <summary>
        /// true is the default behavior but it interferes with clicking links within the slide
        /// added 2013-07-04 based on this forum thread: https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11870~1#post49373
        /// </summary>
        public bool EnableSlideClick
        {
            get { return enableSlideClick; }
            set { enableSlideClick = value; }
        }


        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            //this CDATA section prevents html validation errors from the markup in the javascript
            script.Append("\n //<![CDATA[ \n");

            if (slideContainerClass.Length > 0)
            {
                script.Append("$('#" + this.ClientID + "  div." + slideContainerClass + "')");

                if (pager.Length > 0 && pagerBefore)
                {
                    script.Append(".before('<div id=\"" + this.ClientID + pager + "\" class=\"cyclenav\"></div>').cycle({");
                }
                else if (pager.Length > 0)
                {
                    script.Append(".after('<div id=\"" + this.ClientID + pager + "\" class=\"cyclenav\"></div>').cycle({");
                }
                else
                {
                    script.Append(".cycle({");
                }
            }
            else
            {
                script.Append("$('#" + this.ClientID + "')");

                if (pager.Length > 0 && pagerBefore)
                {
                    script.Append(".before('<div id=\"" + this.ClientID + pager + "\" class=\"cyclenav\"></div>').cycle({");
                }
                else if (pager.Length > 0)
                {
                    script.Append(".after('<div id=\"" + this.ClientID + pager + "\" class=\"cyclenav\"></div>').cycle({");
                }
                else
                {
                    script.Append(".cycle({");
                }
            }

            script.Append("fx:'" + fx + "'");
            script.Append(",speed:" + speed.ToInvariantString());
            if (speedIn > 0) { script.Append(",speedIn:" + speedIn.ToInvariantString()); }
            if (speedOut > 0) { script.Append(",speedOut:" + speedOut.ToInvariantString()); }
            script.Append(",timeout:" + timeout.ToInvariantString());
            if (next.Length > 0)
            {
                script.Append(",next:'" + next + "'");
            }
            else
            {
                if (slideContainerClass.Length > 0)
                {
                    script.Append(",next:'#" + this.ClientID + " > div." + slideContainerClass + "'");
                }
                else
                {
                    script.Append(",next:'#" + this.ClientID + "'");
                }
            }
            if (prev.Length > 0) { script.Append(",prev:'" + prev + "'"); }
            if (prevNextClick.Length > 0) { script.Append(",prevNextClick:" + prevNextClick); }
            if (pager.Length > 0) { script.Append(",pager:'#" + this.ClientID + pager + "'"); }
            if (pagerClick.Length > 0) { script.Append(",pagerClick:" + pagerClick); }
            if (pagerEvent != "click") { script.Append(",pagerEvent:'" + pagerEvent + "'"); }
            if (pagerAnchorBuilder.Length > 0) { script.Append(",pagerAnchorBuilder:" + pagerAnchorBuilder); }
            if (before.Length > 0) { script.Append(",before:" + before); }
            if (after.Length > 0) { script.Append(",after:" + after); }
            if (end.Length > 0) { script.Append(",end:" + end); }
            if (shuffle.Length > 0) { script.Append(",shuffle:" + shuffle); }
            if (animIn.Length > 0) { script.Append(",animIn:" + animIn); }
            if (animOut.Length > 0) { script.Append(",animOut:" + animOut); }
            if (cssBefore.Length > 0) { script.Append(",cssBefore:" + cssBefore); }
            if (cssAfter.Length > 0) { script.Append(",cssAfter:" + cssAfter); }
            if (fxFn.Length > 0) { script.Append(",fxFn:" + fxFn); }
            if (height != "auto") { script.Append(",height:'" + height + "'"); }
            if (startingSlide > 0) { script.Append(",startingSlide:" + startingSlide.ToInvariantString()); }
            if (!sync) { script.Append(",sync:0"); }
            if (random) { script.Append(",random:1"); }
            if (fit) { script.Append(",fit:1"); }
            if (!containerResize) { script.Append(",containerResize:0"); }
            if (pauseOnHover) { script.Append(",pause:1"); }
            if (pauseOnPagerHover) { script.Append(",pauseOnPagerHover:1"); }
            if (autostop) { script.Append(",pauseOnPagerHover:1"); }
            if (autostopCount > 0) { script.Append(",autostopCount:" + autostopCount.ToInvariantString()); }
            if (delay != 0) { script.Append(",delay:" + delay.ToInvariantString()); }
            if (continuous) { script.Append(",autostop:1"); }
            if (slideExpr.Length > 0) { script.Append(",slideExpr:'" + slideExpr + "'"); }
            if (cleartypeNoBg) { script.Append(",cleartypeNoBg:true"); }
            if (nowrap) { script.Append(",nowrap:1"); }
            if (fastOnEvent > 0) { script.Append(",fastOnEvent:" + fastOnEvent.ToInvariantString()); }
            if (!randomizeEffects) { script.Append(",randomizeEffects:0"); }
            if (rev) { script.Append(",rev:1"); }
            if (!manualTrump) { script.Append(",manualTrump:0"); }
            if (!requeueOnImageNotLoaded) { script.Append(",requeueOnImageNotLoaded:false"); }
            if (requeueTimeout != 250) { script.Append(",requeueTimeout:" + requeueTimeout.ToInvariantString()); }
            if (timeoutFn.Length > 0) { script.Append(",timeoutFn:" + timeoutFn); }
            //script.Append("})");
            script.Append("});");

            if (!enableSlideClick)
            {

                script.Append("$('#" + this.ClientID + "').off('click');");

            }

            script.Append("  \n //]]> \n");
            script.Append(" </script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!enableSlideShow) { return; }

            ValidateSettings();
            SetupMainScript();
            SetupInstanceScript();
        }

        private void ValidateSettings()
        {
            string allowedTransitions = "fade,scrollUp,scrollDown,scrollRight,scrollLeft,turnUp,turnDown,turnRight,turnLeft,curtainX,curtainY,slideX,slideY,shuffle,pinch,zoom";
            List<string> allowed = allowedTransitions.SplitOnChar(',');
            List<string> supplied = fx.SplitOnCharAndTrim(',');
            StringBuilder cleanTranstions = new StringBuilder();
            string comma = string.Empty;
            foreach (string t in supplied)
            {
                if (allowed.Contains(t))
                {
                    cleanTranstions.Append(comma + t);
                    comma = ",";
                }
            }

            fx = cleanTranstions.ToString();
            if (fx.Length == 0) { fx = "fade"; }

        }


        private void SetupMainScript()
        {
           
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
            //        "jqcycle", "\n<script  src=\""
            //        + Page.ResolveUrl("~/ClientScript/jqmojo/cycle.js") + "\" type=\"text/javascript\" ></script>");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jqcycle", "\n<script src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.cycle.all.min.js") + "\" type=\"text/javascript\"></script>");
         
        }
    }
}
