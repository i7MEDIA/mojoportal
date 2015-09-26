// Author:					Joe Audette
// Created:				    2007-12-04
// Last Modified:			2012-07-02
//		
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	
// 2011-09-15 updates by Jamie Eubanks to support the newer v3 api which requires no api key and supports ssl
// the previous implementation was also retained because the newer one doesn't support local search
// so if local search is enabled and an api key is present and the request is not over ssl it will use the older api

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Controls.google;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This a control for displaying a google map based on the location set in the location property.
    /// This is more simple than using latitude longitude, lat and long are derived automatically if possible
    /// from the location. Alternatively you can set the coordinates.
    /// 
    /// TODO: implement more of the optional features like overlays etc
    /// </summary>
    public class LocationMap : Panel
    {

        #region Private Properties

        private string googleJsBasePath = "~/ClientScript/google/";
        private string gMapApiKey = string.Empty;
        private string gMapWebConfigKey = "GoogleMapsAPIKey";
        private bool apiKeyFound = false;
        private MapType gmapType = MapType.G_NORMAL_MAP;
        private int zoomLevel = 13; //default is 13
        private string noApiKeyWarning = "Could not find Google Maps API Key in Site Settings or Web.config";

        private string protocol = "http";


        private float latitude = 0;
        private float longitude = 0;
        private bool useLatLong = false;
        private string location = string.Empty;

        private int mapHeight = 300;
        private string mapWidth = "500px";

        private bool enableMapType = false;
        private bool enableZoom = false;
        private bool showInfoWindow = false;
        private bool enableLocalSearch = false;
        private bool enableDrivingDirections = false;
        private string directionsButtonText = "Get Directions From";
        private string directionsButtonToolTip = "Get Directions From";

        #endregion

        #region Public Properties

        //public string Protocol
        //{
        //    get { return protocol; }
        //    set { protocol = value; }
        //}

        /// <summary>
        /// message to display if the api key is not found
        /// </summary>
        public string NoApiKeyWarning
        {
            get { return noApiKeyWarning; }
            set { noApiKeyWarning = value; }
        }

        /// <summary>
        /// default is 13, lower number zooms out higher number zooms in
        /// </summary>
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }


        public MapType GmapType
        {
            get { return gmapType; }
            set { gmapType = value; }
        }

        /// <summary>
        /// This should be the path to the folder containing mojoPortal custom javascript for working with google APIs
        /// i.e ~/ClientScript/google/
        /// This is NOT the path to the google server.
        /// </summary>
        public string GoogleJsBasePath
        {
            get { return googleJsBasePath; }
            set { googleJsBasePath = value; }
        }

        /// <summary>
        /// To use google maps you need an API key.
        /// Get it here: http://code.google.com/apis/maps/signup.html
        /// put it in Web.config as <add key="GoogleMapsAPIKey" value="" />
        /// </summary>
        public string GMapApiKey
        {
            get { return gMapApiKey; }
            set { gMapApiKey = value; }
        }

        /// <summary>
        /// You can set this on the control if you need to use a different Web.config key for some reason.
        /// </summary>
        protected string GMapWebConfigKey
        {
            get { return gMapWebConfigKey; }
            set { gMapWebConfigKey = value; }
        }

        /// <summary>
        /// Indicates whether the GMapApiKey was initialized.
        /// Initialization happens in on pre-render
        /// </summary>
        protected bool ApiKeyFound
        {
            get { return apiKeyFound; }

        }






        /// <summary>
        /// gets or sets the text on the button used for requesting directions
        /// </summary>
        public string DirectionsButtonText
        {
            get { return directionsButtonText; }
            set { directionsButtonText = value; }
        }

        /// <summary>
        /// gets or sets the tooltip text on the button used for requesting directions
        /// </summary>
        public string DirectionsButtonToolTip
        {
            get { return directionsButtonToolTip; }
            set { directionsButtonToolTip = value; }
        }

        /// <summary>
        /// if true the options for satelite and hybrid maps are vailable
        /// </summary>
        public bool EnableMapType
        {
            get { return enableMapType; }
            set { enableMapType = value; }
        }

        /// <summary>
        /// if true uses the ma control with zoom tool
        /// </summary>
        public bool EnableZoom
        {
            get { return enableZoom; }
            set { enableZoom = value; }
        }

        /// <summary>
        /// if true shows a ballon with the location info
        /// </summary>
        public bool ShowInfoWindow
        {
            get { return showInfoWindow; }
            set { showInfoWindow = value; }
        }

        /// <summary>
        /// if true shows a search input and searches google for things close to the location
        /// </summary>
        public bool EnableLocalSearch
        {
            get { return enableLocalSearch; }
            set { enableLocalSearch = value; }
        }

        /// <summary>
        /// if true shows an input and a button for entering a start address and requesting directions.
        /// </summary>
        public bool EnableDrivingDirections
        {
            get { return enableDrivingDirections; }
            set { enableDrivingDirections = value; }
        }



        /// <summary>
        /// pixel height of rendered map
        /// </summary>
        public int MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        /// <summary>
        /// pixel width of rendered map
        /// </summary>
        public string MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        /// <summary>
        /// If set to true the map will be centered on the Latitude/Longitude coordinates.
        /// </summary>
        public bool UseLatLong
        {
            get { return useLatLong; }
            set { useLatLong = value; }
        }

        /// <summary>
        /// If UseLatLong is true, the map will be centered using this Latitude.
        /// </summary>
        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// If UseLatLong is true, the map will be centered using this Longitude.
        /// </summary>
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// If UseLatLong is false and this string has a value of a location that can be looked up, the map will display the found location.
        /// </summary>
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private bool forceOldApi = false;

        public bool ForceOldApi
        {
            get { return forceOldApi; }
            set { forceOldApi = value; }
        }

        private bool useOldApi = false;

        private string mapUtilsFile = "mojogmaputilsv3-v3-min.js";
        public string MapUtilsFile
        {
            get { return mapUtilsFile; }
            set { mapUtilsFile = value; }
        }

        #endregion


        #region Controls

        private Panel pnlMapCanvas;

        private Panel pnlDirections;
        private TextBox txtStartLocation;
        private mojoButton btnGetDirections;


        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }
        }

        /// <summary>
        /// Sets up the javscript for this map instance to be created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LoadSettings();
            IncludeGMapScript();
            

            pnlMapCanvas = new Panel();
            pnlMapCanvas.CssClass = "gmap";
            pnlMapCanvas.ID = "pnlMapCanvas";

            this.Controls.Add(pnlMapCanvas);

            if (enableDrivingDirections)
            {
                btnGetDirections = new mojoButton();
                btnGetDirections.ID = "btnGetDirections";
                btnGetDirections.Text = directionsButtonText;
                btnGetDirections.ToolTip = directionsButtonToolTip;
                btnGetDirections.CssClass = "button gmapbutton";
                

                this.Controls.Add(btnGetDirections);

                Literal space = new Literal();
                space.Text = "&nbsp;";
                this.Controls.Add(space);

                txtStartLocation = new TextBox();
                txtStartLocation.ID = "txtStartLocation";
                txtStartLocation.CssClass = "gmaptextbox";
                this.Controls.Add(txtStartLocation);

                pnlDirections = new Panel();
                pnlDirections.ID = "pnlDirections";
                pnlDirections.CssClass = "gdirections";
                this.Controls.Add(pnlDirections);

                if (useOldApi)
                {
                    btnGetDirections.Attributes.Add("onclick", "showMapAndDirections('" + pnlMapCanvas.ClientID
                        + "','" + pnlDirections.ClientID + "','" + txtStartLocation.ClientID + "','"
                        + location + "','" + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName + "');return false;");
                }
                else
                {
                    string attribscr;
                    attribscr = "showMapAndDirections('" + pnlMapCanvas.ClientID
                        + "','" + pnlDirections.ClientID + "','" + txtStartLocation.ClientID + "','"
                        + location + "'";
                    if (enableZoom)
                    {
                        attribscr += ",true";
                    }
                    else
                    {
                        attribscr += ",false";
                    }
                    if (enableMapType)
                    {
                        attribscr += ",true";
                    }
                    else
                    {
                        attribscr += ",false";
                    }
                    attribscr += ", '" + this.GmapType.ToString() + "'";

                    attribscr += ");return false;";
                    btnGetDirections.Attributes.Add("onclick", attribscr);

                }

                this.DefaultButton = btnGetDirections.ID;

            }

            if (
                (!mapWidth.EndsWith("px"))
                && (!mapWidth.EndsWith("%"))
            )
            {
                mapWidth += "px";
            }

            if (SiteUtils.UseMobileSkin())
            {
                this.pnlMapCanvas.Attributes.Add("style", "width:98%;"
                + "height:" + mapHeight.ToInvariantString() + "px;");
            }
            else
            {
                this.pnlMapCanvas.Attributes.Add("style", "width:"
                    + mapWidth
                    + ";height:" + mapHeight.ToInvariantString() + "px;");
            }

            SetupCss();
            SetupScript();

        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            //script.Append("\n<!-- \n");

            script.Append("\n  var gm" + this.pnlMapCanvas.ClientID + " = document.getElementById('" + this.pnlMapCanvas.ClientID + "'); ");
            script.Append("if(gm" + this.pnlMapCanvas.ClientID + "){");

            script.Append("showGMap(gm" + this.pnlMapCanvas.ClientID + ", '" + location + "'");

            //script.Append("enableMapType, useMapControl, showInfoWindow, enableLocalSearch ");
            if (enableMapType)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (enableZoom)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (showInfoWindow)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (enableLocalSearch)
            {
                SetupSearchScript();
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (useOldApi)
            {
                script.Append("," + this.GmapType.ToString() + "");
            }
            else
            {
                /* JGE Pass map type parameter as a string, to be translated in JS */
                script.Append(", '" + this.GmapType.ToString() + "'");
            }

            script.Append("," + this.ZoomLevel.ToString() + "");


            //script.Append(" ");

            script.Append(");");

            script.Append("}");

            //script.Append("\n//--> ");
            script.Append(" </script>");


            Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "setup" + this.ClientID,
                script.ToString());

        }

        private void SetupSearchScript()
        {
            

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(GMapBasePanel),
                "guds", "\n<script type=\"text/javascript\" src=\""
                + protocol + "://www.google.com/uds/api?file=uds.js&amp;v=1.0"
                + "\" ></script>");

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(GMapBasePanel),
                "gudslocal", "\n<script type=\"text/javascript\" src=\""
                + protocol + "://www.google.com/uds/solutions/localsearch/gmlocalsearch.js"
                + "\" ></script>");

        }

        private void SetupCss()
        {
            if (!enableLocalSearch) { return; }
            if (!useOldApi) { return; }

            if (Page.Header == null) { return; }
            
           
            if (Page.Header.FindControl("gsearchcss") == null)
            {
                Literal cssLink = new Literal();
                cssLink.ID = "gsearchcss";
                cssLink.Text = "\n<link href='"
                + protocol
                + "://www.google.com/uds/css/gsearch.css' type='text/css' rel='stylesheet' media='screen' />";

                Page.Header.Controls.Add(cssLink);
            }

            if (Page.Header.FindControl("gmlocalsearchcss") == null)
            {
                Literal cssLink2 = new Literal();
                cssLink2.ID = "gmlocalsearchcss";
                cssLink2.Text = "\n<link href='"
                + protocol
                + "://www.google.com/uds/solutions/localsearch/gmlocalsearch.css' type='text/css' rel='stylesheet' media='screen' />";

                Page.Header.Controls.Add(cssLink2);
            }

        }


        private void LoadSettings()
        {
            

            if (gMapApiKey.Length > 0)
            {
                // don't look in Web.config if setting has been already made.
                apiKeyFound = true;
                //return;

            }
            else if (ConfigurationManager.AppSettings[gMapWebConfigKey] != null)
            {
                gMapApiKey = ConfigurationManager.AppSettings[gMapWebConfigKey];
                if (gMapApiKey.Length > 0) apiKeyFound = true;

            }

            if((enableLocalSearch) && (apiKeyFound) && (protocol == "http"))
            {
                useOldApi = true;
            }

        }


        


        private void IncludeGMapScript()
        {
            if (useOldApi)
            {
                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "gmap", "\n<script src=\""
                    + protocol + "://maps.google.com/maps?file=api&amp;v=2&amp;key="
                    + gMapApiKey
                    + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "mojogmaputils", "\n<script src=\""
                    + Page.ResolveUrl(googleJsBasePath + "mojogmaputils.js") + "\" type=\"text/javascript\"></script>");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "gmap", "\n<script src=\""
                + protocol
                + "://maps.googleapis.com/maps/api/js?v=3.6&amp;sensor=false"
                + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "mojogmaputils", "\n<script src=\""
                    + Page.ResolveUrl(googleJsBasePath + mapUtilsFile) + "\" type=\"text/javascript\"></script>");
            }

        }

    }
}
