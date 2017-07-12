// Author:					
// Created:				    2010-06-10
// Last Modified:			2012-07-03
//		
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// A simple Bing Map control. 
    /// Declare it on the page and set the location 
    /// if you want to use the driving directions you have to provide the clientid (html id)
    /// of a textbox, a button and a div to show the directions using the provided properties.
    /// </summary>
    public class BingMap : Panel
    {
        private string protocol = "http";
        private string version = "6.3";
        private string securityParam = string.Empty;

        /// <summary>
        /// the version of BingMaps ajax control
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string market = "en-us";
        /// <summary>
        /// the locale, default is en-us, see also AutoSetMarket
        /// </summary>
        public string Market
        {
            get { return market; }
            set { market = value; }
        }

        private bool autoSetMarket = true;
        /// <summary>
        /// If true (the default is true) sets the market using the culture of the executing thread
        /// </summary>
        public bool AutoSetMarket
        {
            get { return autoSetMarket; }
            set { autoSetMarket = value; }
        }

        private string location = string.Empty;
        /// <summary>
        /// The location to show and center on the map
        /// </summary>
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string locationAlias = string.Empty;
        /// <summary>
        /// The label to show for the pushpin, if left blank the location will be used.
        /// </summary>
        public string LocationAlias
        {
            get { return locationAlias; }
            set { locationAlias = value; }
        }

        private bool showLocationPin = false;

        public bool ShowLocationPin
        {
            get { return showLocationPin; }
            set { showLocationPin = value; }
        }

        private int zoom = 13;
        /// <summary>
        /// 1 to 19, default is 13
        /// </summary>
        public int Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        private string mapStyle = "VEMapStyle.Road";
        /// <summary>
        /// options are: VEMapStyle.Road, VEMapStyle.Shaded, VEMapStyle.Aerial, VEMapStyle.Hybrid, VEMapStyle.Birdseye, VEMapStyle.BirdseyeHybrid
        /// </summary>
        public string MapStyle
        {
            get { return mapStyle; }
            set { mapStyle = value; }
        }

        private bool showMapControls = true;

        public bool ShowMapControls
        {
            get { return showMapControls; }
            set { showMapControls = value; }
        }

        private bool showGetDirections = false;

        public bool ShowGetDirections
        {
            get { return showGetDirections; }
            set { showGetDirections = value; }
        }

        private string fromTextBoxId = string.Empty;

        public string FromTextBoxClientId
        {
            get { return fromTextBoxId; }
            set { fromTextBoxId = value; }
        }

        private string getDirectionsButtonId = string.Empty;

        public string GetDirectionsButtonClientId
        {
            get { return getDirectionsButtonId; }
            set { getDirectionsButtonId = value; }
        }

        private string showDirectionsPanelClientId = string.Empty;
 
        public string ShowDirectionsPanelClientId
        {
            get { return showDirectionsPanelClientId; }
            set { showDirectionsPanelClientId = value; }
        }

        private string distanceUnit = "VERouteDistanceUnit.Mile";
        /// <summary>
        /// options are VERouteDistanceUnit.Mile, VERouteDistanceUnit.Kilometer
        /// </summary>
        public string DistanceUnit
        {
            get { return distanceUnit; }
            set { distanceUnit = value; }
        }

        private string distanceUnitLabel = "miles";

        public string DistanceUnitLabel
        {
            get { return distanceUnitLabel; }
            set { distanceUnitLabel = value; }
        }

        private string distanceLabel = "Distance";

        public string DistanceLabel
        {
            get { return distanceLabel; }
            set { distanceLabel = value; }
        }

        private string resetSearchLabel = "reset";

        public string ResetSearchLinkLabel
        {
            get { return resetSearchLabel; }
            set { resetSearchLabel = value; }
        }

        private string resetSearchLinkUrl = string.Empty;

        public string ResetSearchLinkUrl
        {
            get { return resetSearchLinkUrl; }
            set { resetSearchLinkUrl = value; }
        }

        private string mapWidth = "600px";

        public string MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        //private string mapHeight = "400px";

        //public string MapHeight
        //{
        //    get { return mapHeight; }
        //    set { mapHeight = value; }
        //}

        private string GetMarket()
        {
            string m = CultureInfo.CurrentUICulture.Name.ToLower();

            switch (m)
            {
                    //supported markets
                case "cs-cz":
                case "da-dk":
                case "nl-be":
                case "nl-nl":
                case "en-au":
                case "en-ca":
                case "en-gb":
                case "en-us":
                case "fi-fi":
                case "fr-ca":
                case "fr-fr":
                case "de-de":
                case "it-it":
                case "ja-jp":
                case "nb-no":
                case "pt-br":
                case "pt-pt":
                case "es-mx":
                case "es-es":
                case "es-us":
                case "sv-se":

                    // case "en-in": this one needs a different ajax url http://dev.mapindia.live.com/mapcontrol/mapcontrol.ashx?v=6.2


                    return m;
                    
                    //fallback to en-us if unsupported
                default:
                    return "en-us";
            }

           
        }

        private bool useDefaultDimensions = true;
        public bool UseDefaultDimensions
        {
            get { return useDefaultDimensions; }
            set { useDefaultDimensions = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            version = WebConfigSettings.BingMapsVersion;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }

            //if (useDefaultDimensions)
            //{
            //    //if (Width == Unit.Empty) { Width = Unit.Pixel(600); }
            //    //if (Height == Unit.Empty) { Height = Unit.Pixel(400); }
            //}

            // zero these out so they are not automatically added
            // we will add it manually with Atrributes.Add
            Width = Unit.Empty;
            //Height = Unit.Empty;

            if (
                (!mapWidth.EndsWith("px"))
                && (!mapWidth.EndsWith("%"))
            )
            {
                mapWidth += "px";
            }

            //if (
            //    (!mapHeight.EndsWith("px"))
            //    && (!mapHeight.EndsWith("%"))
            //)
            //{
            //    mapHeight += "px";
            //}

            if (SiteUtils.UseMobileSkin())
            {
                //Width = Unit.Percentage(98); 
                mapWidth = "98%";
            }

            Attributes.Add("style", "width:" + mapWidth  + ";height:" + Height.ToString() + ";");

            if (CssClass.Length == 0) { CssClass = "bmap"; }

            if (autoSetMarket) { market = GetMarket(); }

            if (SiteUtils.IsSecureRequest()) 
            { 
                protocol = "https";
                securityParam = "&amp;s=1";
            }

            SetupScripts();
        }

        private void SetupScripts()
        {
            SetupMainScript();
            SetupInitScript();
        }

        private void SetupInitScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n <script type=\"text/javascript\"> ");

            script.Append("\n var map" + ClientID + " = new VEMap('" + ClientID + "'); ");
            script.Append("map" + ClientID + ".LoadMap(); ");

            if (location.Length > 0)
            {
                script.Append("try { ");
                script.Append("map" + ClientID + ".Geocode('" + location + "',map" + ClientID + "findCallback); ");
                script.Append("}catch(e){ alert(e.message); }");

                script.Append("function map" + ClientID + "findCallback(layer, findResults, placeResults, moreResults, error){");
                script.Append("if (placeResults != null){ ");

                

                if (showLocationPin)
                {
                    if (locationAlias.Length == 0) { locationAlias = location; }

                    script.Append("var shape = new VEShape(VEShapeType.Pushpin, placeResults[0].LatLong); ");

                    //script.Append("var icon = new VECustomIconSpecification(); ");
                    //script.Append("icon.Image=\"/Data/SiteImages/location_pin.png\"; ");
                    //script.Append("icon.ImageOffset = new VEPixel(10, -20);");
                    ////script.Append("icon.TextContent=\" \";");
                    //script.Append("shape.SetCustomIcon(icon);");
                    //script.Append("shape.SetIconAnchor(placeResults[0].LatLong); ");

                    //script.Append("shape.SetTitle('<h1>" + locationAlias + "</h1>'); ");
                    script.Append("shape.SetTitle('" + locationAlias + "'); ");
                    script.Append("map" + ClientID + ".ClearInfoBoxStyles(); ");
                    script.Append("map" + ClientID + ".AddShape(shape);  ");
                }

                script.Append("map" + ClientID + ".SetCenterAndZoom(placeResults[0].LatLong," + zoom.ToInvariantString() + "); ");
                script.Append("map" + ClientID + ".SetMapStyle(" + mapStyle + "); ");

                if (!showMapControls) { script.Append("map" + ClientID + ".HideDashboard(); "); }

                script.Append("} ");

                script.Append("} ");

                if ((showGetDirections) && (getDirectionsButtonId.Length > 0) && (fromTextBoxId.Length > 0) && (showDirectionsPanelClientId.Length > 0))
                {
                    if (resetSearchLinkUrl.Length == 0) { resetSearchLinkUrl = Page.Request.Url.ToString(); }

                    script.Append("function map" + ClientID + "ShowTurns(route){ ");
                    script.Append("var turns = \"<p><b>" + distanceLabel + "</b> \" + route.Distance.toFixed(1) + \" " + distanceUnitLabel + "\"; ");
                    script.Append("turns += \" &nbsp;&nbsp;<a href='" + resetSearchLinkUrl + "'>" + resetSearchLabel + "</a></p>\"; ");
                    script.Append("turns += \"<ol class='turns'>\"; ");

                    script.Append("var legs = route.RouteLegs; ");
                    script.Append("var leg = null; ");

                    script.Append("for(var i = 0; i < legs.length; i++) {");
                    script.Append("leg = legs[i];  ");
                    script.Append("var legNum = i + 1; ");

                    script.Append("var turn = null; ");
                    script.Append("var legDistance = null; ");

                    script.Append("for(var j = 0; j < leg.Itinerary.Items.length; j ++) {");
                    script.Append("turn = leg.Itinerary.Items[j];  ");
                    script.Append("turns += \"<li>\" + turn.Text + \" - <span class='turn'>\" + turn.Distance.toFixed(1) + \" " + distanceUnitLabel + "\" + \"</span></li>\"; ");
                    script.Append("} ");

                    script.Append("} ");

                    script.Append("turns += \"</ol>\"; ");

                    script.Append("var d = document.getElementById('" + showDirectionsPanelClientId + "'); ");
                    script.Append("d.innerHTML = turns; ");

                    script.Append("} ");

                    script.Append("var button" + ClientID + " = document.getElementById('" + getDirectionsButtonId + "'); ");
                    script.Append("button" + ClientID + ".onclick =");
                    script.Append("function map" + ClientID + "GetDirections(){");
                    script.Append("var fromLocation = document.getElementById('" + fromTextBoxId + "').value; ");
                    script.Append("var toLocation = '" + location + "'; ");
                    script.Append("var options = new VERouteOptions(); ");
                    script.Append("options.DrawRoute = true; ");
                    //script.Append("options.SetBestMapView = false; ");
                    script.Append("options.RouteCallback  = map" + ClientID + "ShowTurns; ");
                    script.Append("options.DistanceUnit = " + distanceUnit + "; ");
                    script.Append("options.ShowDisambiguation = true; ");
                    script.Append("map" + ClientID + ".GetDirections([fromLocation, toLocation], options); ");

                    script.Append("return false; ");
                    script.Append("} ");

                }
            }
            else
            {
                //location not specified
                script.Append("map" + ClientID + ".SetZoomLevel(" + zoom.ToInvariantString() + "); ");

            }

            script.Append("\n </script>");


            Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "setup" + this.ClientID,
                script.ToString());

        }

        private void SetupMainScript()
        {
            //Firefox 4 broke bing map when plotting points like in In Site Analytics Pro
            // found this workaround http://social.msdn.microsoft.com/Forums/en-CA/vemapcontroldev/thread/17efab17-d70c-40b3-9e50-75c65d59385e
            if (WebConfigSettings.UseBingMapWorkaroundForFirefox4) // make it possible to back this out if it gets fixed somehow in FF or in Bing
            {
                // 2011-06-25 the same problem persists in Firefox 5 so changed to always inlcud ehtis for all versions of FF
                // it may not be needed in older versions of FF but there is no side effects other than the extra http request for loading this extra script
                if (BrowserHelper.IsFF())
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                        "bmapcompat", "\n<script  src=\""
                        + protocol + "://dev.virtualearth.net/mapcontrol/v6.3/js/atlascompat.js\" type=\"text/javascript\" ></script>");
                }
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "bmapmain", "\n<script  src=\"" 
                    + protocol + "://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=" + version + "&amp;mkt=" + market + securityParam + "\" type=\"text/javascript\" ></script>");

        }

    }
}