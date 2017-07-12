//	Author:                 
//  Created:			    2010-08-21
//	Last Modified:		    2010-08-21
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
    /// a wrapper control for jCarousel plugin
    /// http://sorgalla.com/projects/jcarousel/
    /// Example usage: expects a ul where each li is a slide
    /// <portal:Carousel id="jc1" runat="server">
    /// <ul class="jcarousel-skin-name"> replace name with tango or ie7 like jcarousel-skin-tango
    ///     <li><img src="http://static.flickr.com/66/199481236_dc98b5abb3_s.jpg" width="75" height="75" alt="" /></li> 
    ///     <li><img src="http://static.flickr.com/75/199481072_b4a0d09597_s.jpg" width="75" height="75" alt="" /></li> 
    /// </ul>
    /// </portal:Carousel>
    /// this control does not add the needed css to the page so you need to do that yourself
    /// best way is in style.config file of your skin add one of these corresponding to the skin name:
    /// <file cssvpath="/Data/style/jcarousel/tango/skin.css" imagebasevpath="/Data/style/jcarousel/tango/">none</file>
    /// <file cssvpath="/Data/style/jcarousel/ie7/skin.css" imagebasevpath="/Data/style/jcarousel/ie7/">none</file>
    /// it seems to be needed to add this at the bottom of style.config, otherwise other css interferes and you get a script error about item sizes.
    /// </summary>
    public class Carousel : Panel
    {
        #region Properties

        private bool vertical = false;
        /// <summary>
        /// Specifies wether the carousel appears in horizontal or vertical orientation. Changes the carousel from a left/right style to a up/down style carousel.
        /// </summary>
        public bool Vertical
        {
            get { return vertical; }
            set { vertical = value; }
        }

        private bool rtl = false;
        /// <summary>
        /// Specifies wether the carousel appears in RTL (Right-To-Left) mode.
        /// </summary>
        public bool RightToLeft
        {
            get { return rtl; }
            set { rtl = value; }
        }

        private int start = 1;
        /// <summary>
        /// The index of the item to start with.
        /// </summary>
        public int Start
        {
            get { return start; }
            set { start = value; }
        }

        private int offset = 1;
        /// <summary>
        /// The index of the first available item at initialisation.
        /// </summary>
        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private int size = 0;
        /// <summary>
        /// The number of total items. Number of existing <li> elements if size is not passed explicitly by value greater than 0
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private int scroll = 3;
        /// <summary>
        /// The number of items to scroll by.
        /// </summary>
        public int Scroll
        {
            get { return scroll; }
            set { scroll = value; }
        }

        private int visible = 0;
        /// <summary>
        /// If  greater than 0, the width/height of the items will be calculated and set depending on the width/height of the clipping, so that exactly that number of items will be visible.
        /// the value to set here is the number of items to show
        /// not needed if the css is correct and it can determine the size
        /// </summary>
        public int VisibleItems
        {
            get { return visible; }
            set { visible = value; }
        }

        private int animation = 200; //200 = fast, 600 = slow
        /// <summary>
        /// The speed of the scroll animation as milliseconds as integer (See jQuery Documentation). If set to 0, animation is turned off.
        /// default = 200 (fast)
        /// </summary>
        public int Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        private string easing = string.Empty;
        /// <summary>
        /// The name of the easing effect that you want to use (See jQuery Documentation). http://api.jquery.com/animate/
        /// </summary>
        public string Easing
        {
            get { return easing; }
            set { easing = value; }
        }

        private int auto = 0;
        /// <summary>
        /// Specifies how many seconds to periodically autoscroll the content. If set to 0 (default) then autoscrolling is turned off.
        /// </summary>
        public int Auto
        {
            get { return auto; }
            set { auto = value; }
        }

        private string wrap = string.Empty;
        /// <summary>
        /// Specifies whether to wrap at the first/last item (or both) and jump back to the start/end. Options are "first", "last", "both" or "circular" as string. 
        /// If left blank, wrapping is turned off (default).
        /// </summary>
        public string WrapMode
        {
            get { return wrap; }
            set { wrap = value; }
        }

        private string initCallback = string.Empty;
        /// <summary>
        /// JavaScript function that is called right after initialisation of the carousel. Two parameters are passed: 
        /// The instance of the requesting carousel and the state of the carousel initialisation (init, reset or reload)
        /// </summary>
        public string InitCallback
        {
            get { return initCallback; }
            set { initCallback = value; }
        }

        private string itemLoadCallback = string.Empty;
        /// <summary>
        /// JavaScript function that is called when the carousel requests a set of items to be loaded. 
        /// Two parameters are passed: The instance of the requesting carousel and the state of the carousel action (prev, next or init). 
        /// Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation:
        /// Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemLoadCallback
        {
            get { return itemLoadCallback; }
            set { itemLoadCallback = value; }
        }

        private string itemFirstInCallback = string.Empty;
        /// <summary>
        /// JavaScript function that is called (after the scroll animation) when an item becomes the first one in the visible range of the carousel. 
        /// Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position 
        /// of the item in the list and the state of the carousel action (prev, next or init). 
        /// Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        /// Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemFirstInCallback
        {
            get { return itemFirstInCallback; }
            set { itemFirstInCallback = value; }
        }

        private string itemFirstOutCallback = string.Empty;
        /// <summary>
        /// JavaScript function that is called (after the scroll animation) when an item isn't longer the first one in the visible range of the carousel. 
        /// Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position of the 
        /// item in the list and the state of the carousel action (prev, next or init). 
        /// Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        /// Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemFirstOutCallback
        {
            get { return itemFirstOutCallback; }
            set { itemFirstOutCallback = value; }
        }

        private string itemLastInCallback = string.Empty;
        /// <summary>
        /// JavaScript function that is called (after the scroll animation) when an item becomes the last one in the visible range of the carousel. 
        /// Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position 
        /// of the item in the list and the state of the carousel action (prev, next or init). 
        /// Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        /// Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemLastInCallback
        {
            get { return itemLastInCallback; }
            set { itemLastInCallback = value; }
        }

        private string itemLastOutCallback = string.Empty;
        /// <summary>
        ///JavaScript function that is called when an item isn't longer the last one in the visible range of the carousel. 
        ///Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position 
        ///of the item in the list and the state of the carousel action (prev, next or init). 
        ///Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        ///Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemLastOutCallback
        {
            get { return itemLastOutCallback; }
            set { itemLastOutCallback = value; }
        }

        private string itemVisibleInCallback = string.Empty;
        /// <summary>
        ///JavaScript function that is called (after the scroll animation) when an item is in the visible range of the carousel. 
        ///Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position 
        ///of the item in the list and the state of the carousel action (prev, next or init). 
        ///Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        ///Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemVisibleInCallback
        {
            get { return itemVisibleInCallback; }
            set { itemVisibleInCallback = value; }
        }

        private string itemVisibleOutCallback = string.Empty;
        /// <summary>
        ///JavaScript function that is called (after the scroll animation) when an item isn't longer in the visible range of the carousel. 
        ///Four parameters are passed: The instance of the requesting carousel and the <li> object itself, the index which indicates the position 
        ///of the item in the list and the state of the carousel action (prev, next or init). 
        ///Alternatively, you can pass a hash of one or two functions which are triggered before and/or after animation
        ///Example value : {onBeforeAnimation: callback1,onAfterAnimation: callback2 }
        /// </summary>
        public string ItemVisibleOutCallback
        {
            get { return itemVisibleOutCallback; }
            set { itemVisibleOutCallback = value; }
        }

        private string buttonNextCallback = string.Empty;
        /// <summary>
        ///JavaScript function that is called when the state of the 'next' control is changing. The responsibility of this method is to 
        ///enable or disable the 'next' control. 
        ///Three parameters are passed: The instance of the requesting carousel, the control element and a flag indicating whether the 
        ///button should be enabled or disabled.
        /// </summary>
        public string ButtonNextCallback
        {
            get { return buttonNextCallback; }
            set { buttonNextCallback = value; }
        }

        private string buttonPrevCallback = string.Empty;
        /// <summary>
        ///JavaScript function that is called when the state of the 'previous' control is changing. The responsibility of this method is to enable or 
        ///disable the 'previous' control. Three parameters are passed: The instance of the requesting carousel, the control element and a flag indicating 
        ///whether the button should be enabled or disabled.
        /// </summary>
        public string ButtonPrevCallback
        {
            get { return buttonPrevCallback; }
            set { buttonPrevCallback = value; }
        }

        private string buttonNextHTML = "<div></div>";
        /// <summary>
        ///The HTML markup for the auto-generated next button. If set to null, no next-button is created.
        /// </summary>
        public string ButtonNextHTML
        {
            get { return buttonNextHTML; }
            set { buttonNextHTML = value; }
        }

        private string buttonPrevHTML = "<div></div>";
        /// <summary>
        ///The HTML markup for the auto-generated next button. If set to null, no next-button is created.
        /// </summary>
        public string ButtonPrevHTML
        {
            get { return buttonPrevHTML; }
            set { buttonPrevHTML = value; }
        }

        private string buttonNextEvent = "click";
        /// <summary>
        ///Specifies the event which triggers the next scroll.
        /// </summary>
        public string ButtonNextEvent
        {
            get { return buttonNextEvent; }
            set { buttonNextEvent = value; }
        }

        private string buttonPrevEvent = "click";
        /// <summary>
        ///Specifies the event which triggers the previous scroll.
        /// </summary>
        public string ButtonPrevEvent
        {
            get { return buttonPrevEvent; }
            set { buttonPrevEvent = value; }
        }

        private int itemFallbackDimension = 0;
        /// <summary>
        /// If, for some reason, jCarousel can not detect the width of an item, you can set a fallback dimension 
        /// (width or height, depending on the orientation) here to ensure correct calculations.
        /// </summary>
        public int ItemFallbackDimension
        {
            get { return itemFallbackDimension; }
            set { itemFallbackDimension = value; }
        }

        #endregion

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function(){");

            script.Append("$('#" + ClientID +  " > ul').jcarousel({");

            if (vertical)
            {
                script.Append("vertical:true");
            }
            else
            {
                script.Append("vertical:false");
            }

            if (rtl) { script.Append(",rtl:true");  }

            if (start > 1) { script.Append(",start:" + start.ToInvariantString()); }

            if (offset > 1) { script.Append(",offset:" + offset.ToInvariantString()); }

            if (size > 0) { script.Append(",size:" + size.ToInvariantString()); }

            if (auto > 0) { script.Append(",auto:" + auto.ToInvariantString()); }

            script.Append(",scroll:" + scroll.ToInvariantString());

            if (wrap.Length > 0) { script.Append(",wrap:'" + wrap + "'"); }

            if (initCallback.Length > 0) { script.Append(",initCallback:" + initCallback); }

            if (itemLoadCallback.Length > 0) { script.Append(",itemLoadCallback:" + itemLoadCallback); }

            if (itemFirstInCallback.Length > 0) { script.Append(",itemFirstInCallback:" + itemFirstInCallback); }

            if (itemFirstOutCallback.Length > 0) { script.Append(",itemFirstOutCallback:" + itemFirstOutCallback); }

            if (itemLastInCallback.Length > 0) { script.Append(",itemLastInCallback:" + itemLastInCallback); }

            if (itemLastOutCallback.Length > 0) { script.Append(",itemLastOutCallback:" + itemLastOutCallback); }

            if (itemVisibleInCallback.Length > 0) { script.Append(",itemVisibleInCallback:" + itemVisibleInCallback); }

            if (itemVisibleOutCallback.Length > 0) { script.Append(",itemVisibleOutCallback:" + itemVisibleOutCallback); }

            if (buttonNextCallback.Length > 0) { script.Append(",buttonNextCallback:" + buttonNextCallback); }

            if (buttonPrevCallback.Length > 0) { script.Append(",buttonPrevCallback:" + buttonPrevCallback); }

            if (buttonNextHTML != "<div>,/div>") { script.Append(",buttonNextHTML:'" + buttonNextHTML + "'"); }
            if (buttonPrevHTML != "<div>,/div>") { script.Append(",buttonPrevHTML:'" + buttonPrevHTML + "'"); }

            if (buttonNextEvent != "click") { script.Append(",buttonNextEvent:'" + buttonNextEvent + "'"); }
            if (buttonPrevEvent != "click") { script.Append(",buttonPrevEvent:'" + buttonPrevEvent + "'"); }

            if (itemFallbackDimension > 0) { script.Append(",itemFallbackDimension:" + itemFallbackDimension.ToInvariantString()); }


            script.Append("});");

            script.Append("});");

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
                    "jcarousel", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.jcarousel.min.js") + "\" type=\"text/javascript\" ></script>");


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //if (autoLocalize)
            //{
            //    prevText = Resource.Previous;
            //    nextText = Resource.Next;
            //    firstText = Resource.First;
            //    lastText = Resource.Last;

            //}
            SetupMainScript();
            SetupInstanceScript();
        }

    }
}